(() => {
  'use strict';
  const canvas = document.getElementById('pika-orbit-canvas');
  if (!canvas) return;
  const context = canvas.getContext('2d');
  if (!context) return;
  const scene = canvas.closest('.orbit-scene');
  const reduced = matchMedia('(prefers-reduced-motion: reduce)');

  const RINGS = 37;
  const POINTS_PER_RING = 135;
  const TOTAL_POINTS = RINGS * POINTS_PER_RING;

  // Pre-generate deterministic chaotic metadata for the intro animation
  const chaosMeta = [];
  for (let i = 0; i < TOTAL_POINTS; i++) {
    const s1 = (i * 9301 + 49297) % 233280;
    const r1 = s1 / 233280;
    const s2 = (s1 * 9301 + 49297) % 233280;
    const r2 = s2 / 233280;
    const s3 = (s2 * 9301 + 49297) % 233280;
    const r3 = s3 / 233280;

    const spread = 40 + r1 * 360;
    const theta = r2 * Math.PI * 2;
    const phi = (r3 - 0.5) * Math.PI;

    chaosMeta.push({
      cx: spread * Math.cos(theta) * Math.cos(phi),
      cy: spread * Math.sin(theta) * Math.cos(phi),
      cz: spread * Math.sin(phi) * 0.6,
      spawnT: (i / TOTAL_POINTS) * 0.32,
      driftSpeed: 0.8 + r1 * 1.6,
      driftAngle: r2 * Math.PI * 2
    });
  }

  // Opportunity Anchors mapped to cards
  // Index 0: WhatsApp (bottom-left)
  // Index 1: Opportunity (right)
  // Index 2: Customer 360 (top-left)
  // Opportunity tracking configurations mapped to cards
  // Connected directly to specific physical particles on the rotating torus:
  // Step 0: WhatsApp (bottom-left) -> sweeps along lower-left rim
  // Step 1: Customer 360 (top-left) -> sweeps along upper-left rim
  // Step 2: Opportunity (right) -> sweeps along upper-right rim
  const oppConfigs = [
    {
      targetId: "0",
      startAngle: Math.PI * 0.58, // lower-left sector entry (~104°)
      vAngle: -0.25,
      pointIndex: 0,
      ringIndex: 0,
      anchorUBase: 0,
      anchorV: -0.25
    },
    {
      targetId: "2",
      startAngle: Math.PI * 0.98, // upper-left sector entry (~176°)
      vAngle: 0.10,
      pointIndex: 0,
      ringIndex: 0,
      anchorUBase: 0,
      anchorV: 0.10
    },
    {
      targetId: "1",
      startAngle: -Math.PI * 0.35, // right sector entry (~ -63°)
      vAngle: -0.20,
      pointIndex: 0,
      ringIndex: 0,
      anchorUBase: 0,
      anchorV: -0.20
    }
  ];

  const cardMap = {};
  if (scene) {
    scene.querySelectorAll('.orbit-card[data-opp-target]').forEach(card => {
      cardMap[card.dataset.oppTarget] = card;
    });
  }
  const cards = Object.values(cardMap);

  let width = 0, height = 0, frame = 0, visible = true, phase = 0, last = 0;
  let mouseX = 0, mouseY = 0;

  // Intro states: 4.6s total (Chaos 0-1.5s -> Vortex 1.5-3.2s -> Torus 3.2-4.6s)
  const INTRO_DURATION = 4600;
  let isIntro = !reduced.matches;
  let introStart = 0;

  // Opportunity rotation: 9s per opportunity tracking a synchronized physical particle on the torus
  const OPP_DURATION = 9000;
  let currentOppStep = 0;
  let oppElapsed = 0;
  let isCardHovered = false;
  let isTransitioningCard = false;

  function hideAllCards() {
    Object.values(cardMap).forEach(c => {
      if (c) {
        c.classList.remove('is-active', 'is-popping-in', 'is-popping-out');
        const bar = c.querySelector('.orbit-card-progress-bar');
        if (bar) bar.style.width = '0%';
      }
    });
  }

  function showStep(index) {
    if (!oppConfigs.length) return;
    currentOppStep = (index + oppConfigs.length) % oppConfigs.length;
    oppElapsed = 0;
    isTransitioningCard = false;

    const cfg = oppConfigs[currentOppStep];
    const currentTargetId = cfg.targetId;

    // Synchronize anchor node directly with a physical particle on the rotating torus
    const currentRotation = phase * 0.12 + mouseX * 0.08;
    const uOffset = phase * 0.065;

    // Desired angle at the start of the card's appearance:
    // u + rotation = startAngle => u_point = startAngle - currentRotation - uOffset
    let desiredUPoint = cfg.startAngle - currentRotation - uOffset;
    desiredUPoint = ((desiredUPoint % (Math.PI * 2)) + Math.PI * 2) % (Math.PI * 2);

    // Snap to the exact nearest particle in the ring (0 to POINTS_PER_RING - 1)
    const ptIdx = Math.round((desiredUPoint / (Math.PI * 2)) * POINTS_PER_RING) % POINTS_PER_RING;
    cfg.pointIndex = ptIdx;
    cfg.anchorUBase = (ptIdx / POINTS_PER_RING) * Math.PI * 2;

    // Snap to the exact nearest ring on the torus (0 to RINGS - 1)
    let normV = ((cfg.vAngle % (Math.PI * 2)) + Math.PI * 2) % (Math.PI * 2);
    const ringIdx = Math.round((normV / (Math.PI * 2)) * RINGS) % RINGS;
    cfg.ringIndex = ringIdx;
    cfg.anchorV = (ringIdx / RINGS) * Math.PI * 2;

    Object.keys(cardMap).forEach(targetId => {
      const c = cardMap[targetId];
      if (!c) return;
      const bar = c.querySelector('.orbit-card-progress-bar');
      if (targetId === currentTargetId) {
        c.classList.remove('is-popping-out');
        c.classList.add('is-active', 'is-popping-in');
        if (bar) bar.style.width = '0%';
      } else {
        c.classList.remove('is-active', 'is-popping-in', 'is-popping-out');
        if (bar) bar.style.width = '0%';
      }
    });
  }

  function nextOpportunity() {
    if (isTransitioningCard || !oppConfigs.length) return;
    isTransitioningCard = true;
    const currentTargetId = oppConfigs[currentOppStep]?.targetId;
    const currentCard = cardMap[currentTargetId];
    if (currentCard) {
      currentCard.classList.remove('is-popping-in');
      currentCard.classList.add('is-popping-out');
    }
    setTimeout(() => {
      showStep(currentOppStep + 1);
    }, 550);
  }

  function startIntro() {
    if (reduced.matches) {
      isIntro = false;
      if (scene) scene.classList.remove('is-intro');
      showStep(0);
      return;
    }
    isIntro = true;
    introStart = performance.now();
    if (scene) scene.classList.add('is-intro');
    hideAllCards();
  }

  // Hover handlers for cards to pause auto-advance and tracking sweep
  Object.values(cardMap).forEach(card => {
    if (!card) return;
    card.addEventListener('mouseenter', () => { isCardHovered = true; });
    card.addEventListener('mouseleave', () => { isCardHovered = false; });
  });

  function resize() {
    const box = canvas.getBoundingClientRect();
    width = box.width; height = box.height;
    const dpr = Math.min(devicePixelRatio || 1, 2);
    canvas.width = Math.round(width * dpr); canvas.height = Math.round(height * dpr);
    context.setTransform(dpr, 0, 0, dpr, 0, 0);
    draw(performance.now());
  }

  function draw(time) {
    context.clearRect(0, 0, width, height);
    const cx = width * 0.5, cy = height * 0.46;
    const radius = Math.min(width * 0.34, height * 0.36);

    // Subtle background ambient glow
    const glow = context.createRadialGradient(cx, cy, 20, cx, cy, radius * 1.6);
    glow.addColorStop(0, 'rgba(166,221,45,.035)');
    glow.addColorStop(0.65, 'rgba(136,185,42,.055)');
    glow.addColorStop(1, 'rgba(100,145,30,0)');
    context.fillStyle = glow;
    context.fillRect(0, 0, width, height);

    // Intro progress calculation
    let introT = 1.0;
    if (isIntro) {
      const elapsedIntro = time - introStart;
      introT = Math.min(elapsedIntro / INTRO_DURATION, 1.0);
      if (introT >= 1.0) {
        isIntro = false;
        if (scene) scene.classList.remove('is-intro');
        showStep(0);
      }
    }

    const rotation = phase * 0.12 + mouseX * 0.08;
    const tilt = 0.72 + mouseY * 0.08;

    let particleIdx = 0;
    let anchorPx = null, anchorPy = null;
    let anchorDepth = 0;
    if (!isIntro && oppConfigs.length > 0) {
      const cfg = oppConfigs[currentOppStep] || oppConfigs[0];
      const uAnchor = cfg.anchorUBase + phase * 0.065;
      const vAnchor = cfg.anchorV;

      // 3D coordinates aligned with the exact torus particle mesh
      const trA = radius + Math.cos(vAnchor) * radius * 0.29;
      const targetXA = trA * Math.cos(uAnchor);
      const targetYA = trA * Math.sin(uAnchor);
      const targetZA = Math.sin(vAnchor) * radius * 0.29;

      const xxA = targetXA * Math.cos(rotation) - targetYA * Math.sin(rotation);
      const yyA = targetXA * Math.sin(rotation) + targetYA * Math.cos(rotation);
      const syA = yyA * Math.cos(tilt) - targetZA * Math.sin(tilt);
      const depthA = yyA * Math.sin(tilt) + targetZA * Math.cos(tilt);

      anchorDepth = depthA;
      const safeDepthA = Math.min(depthA, 580);
      const persA = Math.max(0.1, 650 / (650 - safeDepthA));
      anchorPx = cx + xxA * persA;
      anchorPy = cy + syA * persA;
    }

    for (let ring = 0; ring < RINGS; ring++) {
      const v = ring / RINGS * Math.PI * 2;
      for (let point = 0; point < POINTS_PER_RING; point++) {
        const u = point / POINTS_PER_RING * Math.PI * 2 + phase * 0.065;
        const meta = chaosMeta[particleIdx++];

        // Target Torus Coordinate
        const tr = radius + Math.cos(v) * radius * 0.29;
        const targetX = tr * Math.cos(u);
        const targetY = tr * Math.sin(u);
        const targetZ = Math.sin(v) * radius * 0.29;

        let curX = targetX, curY = targetY, curZ = targetZ;
        let ptAlpha = 1.0;

        if (isIntro) {
          if (introT < meta.spawnT) {
            // Particle not yet emerged
            continue;
          }

          if (introT < 0.35) {
            // Phase 1: Chaos & Scatter
            const p1Elapsed = (time - introStart) * 0.001;
            const drift = p1Elapsed * meta.driftSpeed;
            curX = meta.cx + Math.cos(meta.driftAngle + drift) * 16;
            curY = meta.cy + Math.sin(meta.driftAngle + drift) * 16;
            curZ = meta.cz + Math.sin(meta.driftAngle + drift * 0.5) * 12;
            ptAlpha = Math.min((introT - meta.spawnT) / 0.08, 1.0);
          } else if (introT < 0.70) {
            // Phase 2: Convergence / Vortex around Pika core
            const t2 = (introT - 0.35) / 0.35;
            const e2 = t2 * t2 * (3 - 2 * t2);
            const swirl = e2 * (Math.PI * 3.5);

            // Blend distance from chaos spread towards torus radius
            const rawDist = Math.hypot(meta.cx, meta.cy);
            const dist = rawDist * (1 - e2) + tr * e2;
            const curTheta = Math.atan2(meta.cy, meta.cx) + swirl;

            curX = dist * Math.cos(curTheta);
            curY = dist * Math.sin(curTheta);
            curZ = meta.cz * (1 - e2) + targetZ * e2;
          } else {
            // Phase 3: Torus Formation
            const t3 = (introT - 0.70) / 0.30;
            const e3 = t3 * t3 * (3 - 2 * t3);

            const rawDist = Math.hypot(meta.cx, meta.cy);
            const distVortex = rawDist * 0.2 + tr * 0.8;
            const curTheta = Math.atan2(meta.cy, meta.cx) + Math.PI * 3.5;
            const vortexX = distVortex * Math.cos(curTheta);
            const vortexY = distVortex * Math.sin(curTheta);
            const vortexZ = targetZ;

            curX = vortexX * (1 - e3) + targetX * e3;
            curY = vortexY * (1 - e3) + targetY * e3;
            curZ = vortexZ * (1 - e3) + targetZ * e3;
          }
        }

        // 3D rotation & tilt
        const xx = curX * Math.cos(rotation) - curY * Math.sin(rotation);
        const yy = curX * Math.sin(rotation) + curY * Math.cos(rotation);
        const sy = yy * Math.cos(tilt) - curZ * Math.sin(tilt);
        const depth = yy * Math.sin(tilt) + curZ * Math.cos(tilt);

        // Near-plane clipping to prevent negative perspective / camera inversion
        if (depth > 590) {
          continue;
        }

        const safeDepth = Math.min(depth, 580);
        const perspective = Math.max(0.1, 650 / (650 - safeDepth));
        const px = cx + xx * perspective;
        const py = cy + sy * perspective;

        const brightness = Math.max(0, Math.min((depth / radius + 1) * 0.5, 2.0));
        const wave = (Math.sin(u * 3 + v * 2 + phase) + 1) * 0.5;
        const particleRadius = Math.max(0.1, (0.55 + brightness * 0.65) * perspective);

        context.fillStyle = `rgba(${151 + Math.round(wave * 45)},${192 + Math.round(wave * 49)},${58 + Math.round(wave * 40)},${Math.max(0.05, (0.15 + brightness * 0.5) * ptAlpha)})`;
        context.beginPath();
        context.arc(px, py, particleRadius, 0, Math.PI * 2);
        context.fill();
      }
    }

    // Outer subtle orbital ring (only when torus has formed)
    if (!isIntro || introT > 0.75) {
      const ringAlpha = isIntro ? (introT - 0.75) / 0.25 : 1.0;
      context.strokeStyle = `rgba(192, 223, 136, ${0.1 * ringAlpha})`;
      context.lineWidth = 0.7;
      context.beginPath();
      context.ellipse(cx, cy, radius * 1.49, radius * 0.94, -0.35, 0, Math.PI * 2);
      context.stroke();

      for (let n = 0; n < 5; n++) {
        const a = phase * 0.15 + n * Math.PI * 0.4;
        const x = Math.cos(a) * radius * 1.49, y = Math.sin(a) * radius * 0.94;
        const px = cx + x * Math.cos(-0.35) - y * Math.sin(-0.35);
        const py = cy + x * Math.sin(-0.35) + y * Math.cos(-0.35);
        context.fillStyle = `rgba(198, 239, 106, ${ringAlpha})`;
        context.beginPath();
        context.arc(px, py, 2.4, 0, Math.PI * 2);
        context.fill();
      }
    }

    // Opportunity Tether & Glowing Node
    if (!isIntro && anchorPx !== null && anchorPy !== null && oppConfigs.length > 0) {
      const currentTargetId = oppConfigs[currentOppStep]?.targetId;
      const activeCardEl = cardMap[currentTargetId];
      if (activeCardEl && activeCardEl.classList.contains('is-active')) {
        const socketEl = activeCardEl.querySelector('.orbit-tether-socket') || activeCardEl;
        const sockBox = socketEl.getBoundingClientRect();
        const canvasBox = canvas.getBoundingClientRect();

        const sockX = (sockBox.left + sockBox.width * 0.5) - canvasBox.left;
        const sockY = (sockBox.top + sockBox.height * 0.5) - canvasBox.top;

        if (!isNaN(sockX) && !isNaN(sockY)) {
          context.save();

          // Organic curved control point
          const midX = (anchorPx + sockX) * 0.5;
          const midY = (anchorPy + sockY) * 0.5;
          const dx = sockX - anchorPx;
          const dy = sockY - anchorPy;
          const dist = Math.hypot(dx, dy);
          const nx = -dy / (dist || 1);
          const ny = dx / (dist || 1);
          const curveOffset = Math.sin(time * 0.0012) * 5 + (dist * 0.1);
          const cpX = midX + nx * curveOffset;
          const cpY = midY + ny * curveOffset;

          // 1. Soft atmospheric beam glow
          context.beginPath();
          context.moveTo(anchorPx, anchorPy);
          context.quadraticCurveTo(cpX, cpY, sockX, sockY);
          context.strokeStyle = 'rgba(181, 238, 25, 0.22)';
          context.lineWidth = 6;
          context.lineCap = 'round';
          context.stroke();

          // 2. High-intensity Core Laser Tether
          const tetherGrad = context.createLinearGradient(anchorPx, anchorPy, sockX, sockY);
          tetherGrad.addColorStop(0, '#ffffff');
          tetherGrad.addColorStop(0.25, 'rgba(214, 255, 77, 0.95)');
          tetherGrad.addColorStop(0.75, 'rgba(181, 238, 25, 0.85)');
          tetherGrad.addColorStop(1, 'rgba(181, 238, 25, 0.95)');

          context.beginPath();
          context.moveTo(anchorPx, anchorPy);
          context.quadraticCurveTo(cpX, cpY, sockX, sockY);
          context.strokeStyle = tetherGrad;
          context.lineWidth = 2.4;
          context.shadowColor = '#b5ee19';
          context.shadowBlur = 12;
          context.stroke();

          // 3. Traveling Photon Energy Packets (Data streaming from torus point into card)
          for (let k = 0; k < 3; k++) {
            const travelT = ((time * 0.0009) + k * 0.333) % 1.0;
            const omt = 1 - travelT;
            const pxPhoton = omt * omt * anchorPx + 2 * omt * travelT * cpX + travelT * travelT * sockX;
            const pyPhoton = omt * omt * anchorPy + 2 * omt * travelT * cpY + travelT * travelT * sockY;

            // Halo
            context.fillStyle = 'rgba(181, 238, 25, 0.6)';
            context.shadowBlur = 14;
            context.shadowColor = '#b5ee19';
            context.beginPath();
            context.arc(pxPhoton, pyPhoton, 5.0, 0, Math.PI * 2);
            context.fill();

            // Core
            context.fillStyle = '#ffffff';
            context.shadowBlur = 6;
            context.shadowColor = '#ffffff';
            context.beginPath();
            context.arc(pxPhoton, pyPhoton, 2.2, 0, Math.PI * 2);
            context.fill();
          }

          // 4. Expanding Radar Pulse Rings around Anchor Particle on the Torus
          for (let r = 0; r < 2; r++) {
            const pulseT = ((time * 0.0013) + r * 0.5) % 1.0;
            const radiusPulse = 4 + pulseT * 26;
            const alphaPulse = (1 - pulseT) * 0.85;
            context.strokeStyle = `rgba(181, 238, 25, ${alphaPulse})`;
            context.lineWidth = 1.8;
            context.shadowBlur = 8;
            context.shadowColor = '#b5ee19';
            context.beginPath();
            context.arc(anchorPx, anchorPy, radiusPulse, 0, Math.PI * 2);
            context.stroke();
          }

          // 5. High-intensity Glowing Torus Node (The Detected Point)
          context.shadowBlur = 18;
          context.shadowColor = '#b5ee19';

          // Outer halo
          context.fillStyle = 'rgba(181, 238, 25, 0.45)';
          context.beginPath();
          context.arc(anchorPx, anchorPy, 9, 0, Math.PI * 2);
          context.fill();

          // Lime ring
          context.fillStyle = '#b5ee19';
          context.beginPath();
          context.arc(anchorPx, anchorPy, 5.2, 0, Math.PI * 2);
          context.fill();

          // White center
          context.fillStyle = '#ffffff';
          context.shadowBlur = 4;
          context.shadowColor = '#ffffff';
          context.beginPath();
          context.arc(anchorPx, anchorPy, 2.8, 0, Math.PI * 2);
          context.fill();

          // Pulse at the card socket
          context.fillStyle = '#b5ee19';
          context.shadowBlur = 10;
          context.shadowColor = '#b5ee19';
          context.beginPath();
          context.arc(sockX, sockY, 4, 0, Math.PI * 2);
          context.fill();

          context.restore();
        }
      }
    }
  }

  function tick(time) {
    frame = 0;
    if (!visible || document.hidden || reduced.matches) return;

    const delta = time - last;
    if (delta > 16) {
      phase += Math.min(delta / 1000, 0.05);
      last = time;

      // Update opportunity timer and bottom loading border if not intro and not transitioning
      if (!isIntro && !isTransitioningCard) {
        if (!isCardHovered) {
          oppElapsed += delta;
        }
        const pct = Math.min((oppElapsed / OPP_DURATION) * 100, 100);
        const currentTargetId = oppConfigs[currentOppStep]?.targetId;
        const activeCardEl = cardMap[currentTargetId];
        if (activeCardEl) {
          const bar = activeCardEl.querySelector('.orbit-card-progress-bar');
          if (bar) {
            bar.style.width = pct.toFixed(2) + '%';
          }
        }
        if (oppElapsed >= OPP_DURATION && !isCardHovered) {
          nextOpportunity();
        }
      }

      draw(time);
    }
    frame = requestAnimationFrame(tick);
  }

  function sync() {
    cancelAnimationFrame(frame);
    frame = 0;
    last = performance.now();
    if (visible && !document.hidden && !reduced.matches) {
      frame = requestAnimationFrame(tick);
    } else {
      draw(performance.now());
    }
  }

  new ResizeObserver(resize).observe(canvas);
  new IntersectionObserver(entries => {
    visible = entries[0].isIntersecting;
    sync();
  }, { rootMargin: '80px' }).observe(canvas);

  document.addEventListener('visibilitychange', sync);
  reduced.addEventListener('change', () => {
    if (reduced.matches) {
      isIntro = false;
      if (scene) scene.classList.remove('is-intro');
      showStep(0);
    }
    sync();
  });

  if (canvas.parentElement) {
    canvas.parentElement.addEventListener('pointermove', event => {
      if (reduced.matches || event.pointerType === 'touch') return;
      const r = canvas.getBoundingClientRect();
      mouseX = (event.clientX - r.left) / r.width - 0.5;
      mouseY = (event.clientY - r.top) / r.height - 0.5;
    }, { passive: true });
  }

  const reveal = new IntersectionObserver(entries => entries.forEach(entry => {
    if (entry.isIntersecting) {
      entry.target.classList.add('orbit-reveal');
      reveal.unobserve(entry.target);
    }
  }), { threshold: 0.12 });
  document.querySelectorAll('.pika-orbit main section:not(#hero) > .pw2-container').forEach(section => reveal.observe(section));

  resize();
  startIntro();
  sync();
})();


// The three stages are real, keyboard-accessible controls for the illustration.
(() => {
 const buttons = [...document.querySelectorAll('[data-scene-step]')];
 const scene = document.querySelector('[data-network-step]');
 if (!scene) return;
 const tr = document.documentElement.lang.startsWith('tr');
 const copy = tr ? [
  ['BİRLEŞİK MÜŞTERİ GÖRÜNÜMÜ','Parçalar birleşir. Hikâye tamamlanır.','Alışverişler, tercihler ve etkileşimler tek yerde.'],
  ['MÜŞTERİ VE ÜRÜN ZEKÂSI','Bir sonraki ihtiyaç görünür olur.','Alışveriş ritmi ve ürün ilişkileri fırsata dönüşür.'],
  ['KİŞİYE ÖZEL İLETİŞİM','Doğru mesaj, doğru anda buluşur.','WhatsApp, e-posta ve SMS ile izinli iletişim.']
 ] : [
  ['UNIFIED CUSTOMER VIEW','The pieces connect. The story comes together.','Purchases, preferences and interactions, together.'],
  ['CUSTOMER & PRODUCT INTELLIGENCE','The next need comes into focus.','Purchase rhythms and product relationships reveal opportunities.'],
  ['PERSONAL COMMUNICATION','The right message meets the moment.','Consented communication through WhatsApp, email and SMS.']
 ];
 buttons.forEach(button => button.addEventListener('click', () => {
  const index = Number(button.dataset.sceneStep);
  buttons.forEach(item => {const active = item === button; item.classList.toggle('active', active);item.setAttribute('aria-pressed', String(active));});
  scene.dataset.networkStep = String(index);
  scene.querySelector('.network-insight small').textContent = copy[index][0];
  scene.querySelector('[data-scene-title]').textContent = copy[index][1];
  scene.querySelector('[data-scene-description]').textContent = copy[index][2];
 }));
})();

// Draw the illustrative revenue curve on entry, then trace it with a soft highlight.
(() => {
 const chart = document.querySelector('.report-chart');
 const svg = chart?.querySelector('svg');
 const curve = svg?.querySelector('path[fill="none"]');
 const area = svg?.querySelector('path[fill^="url("]');
 if (!curve || !area || typeof curve.animate !== 'function') return;
 const reduced = matchMedia('(prefers-reduced-motion: reduce)');
 const length = curve.getTotalLength();
 const tracer = curve.cloneNode(false);
 tracer.setAttribute('stroke', '#c0da87');
 tracer.setAttribute('stroke-width', '4');
 tracer.setAttribute('stroke-linecap', 'round');
 tracer.setAttribute('aria-hidden', 'true');
 tracer.style.pointerEvents = 'none';
 tracer.style.strokeDasharray = `28 ${length + 28}`;
 tracer.style.filter = 'drop-shadow(0 0 3px #a9ca70)';
 tracer.style.visibility = 'hidden';
 svg.append(tracer);
 let visible = false;
 let animations = [];
 function reset() {
  animations.forEach(animation => animation.cancel());
  animations = [];
  tracer.style.visibility = 'hidden';
 }
 function start() {
  reset();
  if (reduced.matches || !visible || document.hidden) return;
  animations.push(curve.animate([
   { strokeDasharray: `${length} ${length}`, strokeDashoffset: length },
   { strokeDasharray: `${length} ${length}`, strokeDashoffset: 0 }
  ], { duration: 1900, easing: 'cubic-bezier(.25,.1,.25,1)' }));
  animations.push(area.animate([{ opacity: 0 }, { opacity: 1 }],
   { duration: 2000, easing: 'ease-out' }));
  tracer.style.visibility = 'visible';
  animations.push(tracer.animate([
   { strokeDashoffset: 28, opacity: 0 },
   { opacity: .9, offset: .12 },
   { opacity: .9, offset: .85 },
   { strokeDashoffset: -length, opacity: 0 }
  ], { duration: 4800, delay: 1900, iterations: Infinity, easing: 'linear', fill: 'backwards' }));
 }
 new IntersectionObserver(entries => {
  visible = entries[0].isIntersecting;
  if (visible) start(); else reset();
 }, { threshold: .25 }).observe(chart);
  reduced.addEventListener('change', start);
  document.addEventListener('visibilitychange', () => {
   if (document.hidden) animations.forEach(animation => animation.pause());
   else if (visible && !reduced.matches) animations.forEach(animation => animation.play());
  });

  // Floating Frosted-Glass Navbar Scroll Handler
  (() => {
    const header = document.querySelector('.pika-orbit .top-header-info');
    if (!header) return;

    let ticking = false;
    const updateScroll = () => {
      const isScrolled = window.scrollY > 20;
      header.classList.toggle('is-scrolled', isScrolled);
      ticking = false;
    };

    window.addEventListener('scroll', () => {
      if (!ticking) {
        requestAnimationFrame(updateScroll);
        ticking = true;
      }
    }, { passive: true });

    updateScroll();
  })();

  // Pika Navbar & Dropdown Controller (Mutual exclusivity, hover, click & universal dismissal)
  (() => {
    const navbar = document.querySelector('.pika-orbit #navbar');
    if (!navbar) return;

    const navDropdowns = Array.from(navbar.querySelectorAll('.nav-item.dropdown'));
    const langDropdown = navbar.querySelector('.nav-language-dropdown');
    const allDropdowns = [...navDropdowns, langDropdown].filter(Boolean);

    function isDropdownOpen(dropdown) {
      if (!dropdown) return false;
      const menu = dropdown.querySelector('.dropdown-menu');
      return dropdown.classList.contains('show') || (menu && menu.classList.contains('show'));
    }

    function closeDropdown(dropdown) {
      if (!dropdown) return;
      dropdown.classList.remove('show');
      const toggle = dropdown.querySelector('[data-bs-toggle="dropdown"], .nav-language-trigger, .dropdown-toggle');
      const menu = dropdown.querySelector('.dropdown-menu');
      if (toggle) {
        toggle.classList.remove('show');
        toggle.setAttribute('aria-expanded', 'false');
      }
      if (menu) {
        menu.classList.remove('show');
      }
    }

    function openDropdown(dropdown) {
      if (!dropdown) return;
      allDropdowns.forEach(other => {
        if (other !== dropdown) closeDropdown(other);
      });

      dropdown.classList.add('show');
      const toggle = dropdown.querySelector('[data-bs-toggle="dropdown"], .nav-language-trigger, .dropdown-toggle');
      const menu = dropdown.querySelector('.dropdown-menu');
      if (toggle) {
        toggle.classList.add('show');
        toggle.setAttribute('aria-expanded', 'true');
      }
      if (menu) {
        menu.classList.add('show');
      }
    }

    function closeAllDropdowns() {
      allDropdowns.forEach(closeDropdown);
    }

    let closeTimer = null;
    let activeDropdown = null;

    // Standard nav dropdowns (Platform, Kanallar, Kaynaklar, Kurumsal)
    navDropdowns.forEach(dropdown => {
      const toggle = dropdown.querySelector('[data-bs-toggle="dropdown"], .dropdown-toggle');

      dropdown.addEventListener('mouseenter', () => {
        if (window.innerWidth < 992) return;
        if (closeTimer) {
          clearTimeout(closeTimer);
          closeTimer = null;
        }
        activeDropdown = dropdown;
        openDropdown(dropdown);
      });

      dropdown.addEventListener('mouseleave', () => {
        if (window.innerWidth < 992) return;
        closeTimer = setTimeout(() => {
          if (activeDropdown === dropdown) {
            closeDropdown(dropdown);
            activeDropdown = null;
          }
        }, 180);
      });

      if (toggle) {
        toggle.addEventListener('click', (e) => {
          if (window.innerWidth >= 992) {
            e.preventDefault();
            e.stopPropagation();
            if (isDropdownOpen(dropdown)) {
              closeDropdown(dropdown);
              activeDropdown = null;
            } else {
              activeDropdown = dropdown;
              openDropdown(dropdown);
            }
          }
        });
      }
    });

    // Language Dropdown Controller (Full hover, click toggle & clean dismissal)
    if (langDropdown) {
      const langTrigger = langDropdown.querySelector('.nav-language-trigger, [data-bs-toggle="dropdown"], .dropdown-toggle');

      // Desktop Hover
      langDropdown.addEventListener('mouseenter', () => {
        if (window.innerWidth < 992) return;
        if (closeTimer) {
          clearTimeout(closeTimer);
          closeTimer = null;
        }
        openDropdown(langDropdown);
      });

      langDropdown.addEventListener('mouseleave', () => {
        if (window.innerWidth < 992) return;
        if (closeTimer) clearTimeout(closeTimer);
        closeTimer = setTimeout(() => {
          closeDropdown(langDropdown);
        }, 180);
      });

      // Click Toggle (Works for both desktop click and mobile tap)
      if (langTrigger) {
        langTrigger.addEventListener('click', (e) => {
          e.preventDefault();
          e.stopPropagation();
          if (isDropdownOpen(langDropdown)) {
            closeDropdown(langDropdown);
          } else {
            openDropdown(langDropdown);
          }
        });
      }

      // Close on selecting any language item (Only items inside .dropdown-menu, NOT the trigger button!)
      langDropdown.querySelectorAll('.dropdown-menu .dropdown-item, .dropdown-menu button, .dropdown-menu a').forEach(btn => {
        btn.addEventListener('click', () => {
          closeDropdown(langDropdown);
        });
      });
    }

    // Universal Outside-Click Dismissal
    document.addEventListener('click', (e) => {
      if (langDropdown && !langDropdown.contains(e.target)) {
        closeDropdown(langDropdown);
      }
      if (!navbar.contains(e.target)) {
        closeAllDropdowns();
        activeDropdown = null;
      }
    });

    // Escape Key Dismissal
    document.addEventListener('keydown', (e) => {
      if (e.key === 'Escape') {
        closeAllDropdowns();
        activeDropdown = null;
      }
    });

    // Scroll Dismissal
    window.addEventListener('scroll', () => {
      if (langDropdown && isDropdownOpen(langDropdown)) {
        closeDropdown(langDropdown);
      }
    }, { passive: true });

    // Close on clicking dropdown items across navbar
    navbar.querySelectorAll('.dropdown-item').forEach(item => {
      item.addEventListener('click', () => {
        closeAllDropdowns();
        activeDropdown = null;
      });
    });
  })();
})();

// ==========================================================================
// Pika Demo Request Modal - UI & Interaction Enhancer
// ==========================================================================
(() => {
  function enhanceDemoModal() {
    const modal = document.getElementById('demoModal');
    if (!modal) return;

    // Ensure root classes are present
    if (!modal.classList.contains('pika-demo-modal-root')) {
      modal.classList.add('pika-demo-modal-root');
    }

    const dialog = modal.querySelector('.modal-dialog');
    if (dialog && !dialog.classList.contains('pika-demo-dialog')) {
      dialog.classList.add('pika-demo-dialog');
    }

    const content = modal.querySelector('.modal-content');
    if (content && !content.classList.contains('pika-demo-modal-content')) {
      content.classList.add('pika-demo-modal-content');
    }

    const form = modal.querySelector('#demoRequestForm');
    if (!form) return;

    const isTr = !document.documentElement.lang || document.documentElement.lang.toLowerCase().startsWith('tr');

    // 1. If split-grid layout is missing, construct the split container
    let grid = modal.querySelector('.pika-demo-grid');
    if (!grid && content) {
      const showcaseHtml = `
        <div class="pika-demo-showcase">
          <div class="pika-demo-showcase-bg-orb" aria-hidden="true"></div>
          <div class="pika-demo-showcase-inner">
            <div class="pika-demo-brand">
              <span class="orbit-nav-mark" aria-hidden="true"></span>
              <span class="orbit-nav-word">pika</span>
              <span class="pika-demo-badge">
                <i class="ri-flashlight-fill"></i>
                <span>${isTr ? 'CANLI DEMO' : 'LIVE DEMO'}</span>
              </span>
            </div>

            <div class="pika-demo-hero-text">
              <h3 class="pika-demo-headline">
                ${isTr ? 'Akıllı Pazarlamanın Yeni Nesil Dünyasını Keşfedin' : 'Discover Next-Gen Marketing Intelligence'}
              </h3>
              <p class="pika-demo-subtext">
                ${isTr ? '15 dakikalık interaktif demoda, platformun işletmenize özel müşteri ve kampanya zekâsını nasıl ürettiğini canlı görün.' : 'In a 15-minute live walkthrough, see how Pika transforms customer & product data into high-converting automated actions.'}
              </p>
            </div>

            <div class="pika-demo-features">
              <div class="pika-demo-feat-item">
                <div class="pika-demo-feat-icon pika-demo-feat-icon--intel">
                  <i class="ri-brain-line"></i>
                </div>
                <div class="pika-demo-feat-content">
                  <strong>${isTr ? 'Müşteri & Ürün Zekâsı' : 'Customer & Product Intelligence'}</strong>
                  <span>${isTr ? 'Müşteri niyetlerini ve satın alma ritmini anlık karar motoruyla yakalayın.' : 'Capture customer intent and repeat-purchase rhythms with real-time decisioning.'}</span>
                </div>
              </div>

              <div class="pika-demo-feat-item">
                <div class="pika-demo-feat-icon pika-demo-feat-icon--action">
                  <i class="ri-route-line"></i>
                </div>
                <div class="pika-demo-feat-content">
                  <strong>${isTr ? 'Omnichannel Orkestrasyon' : 'Omnichannel Flow Automation'}</strong>
                  <span>${isTr ? 'WhatsApp, SMS ve E-posta akışlarını tek bir tuvalde zahmetsizce yönetin.' : 'Orchestrate WhatsApp, SMS, and Email journeys effortlessly in one unified canvas.'}</span>
                </div>
              </div>

              <div class="pika-demo-feat-item">
                <div class="pika-demo-feat-icon pika-demo-feat-icon--custom">
                  <i class="ri-magic-line"></i>
                </div>
                <div class="pika-demo-feat-content">
                  <strong>${isTr ? 'Sektörünüze Özel Canlı Senaryo' : 'Tailored Business Scenarios'}</strong>
                  <span>${isTr ? 'Genel slaytlar değil; tam sizin sektörünüze ve verilerinize uygun canlı simülasyon.' : 'Not generic slides: a live simulation specifically tailored to your industry and growth goals.'}</span>
                </div>
              </div>
            </div>

            <div class="pika-demo-trust-footer">
              <div class="pika-demo-trust-pill">
                <i class="ri-shield-check-line"></i>
                <span>${isTr ? 'KVKK & İYS Uyumlu' : 'KVKK & GDPR Compliant'}</span>
              </div>
              <div class="pika-demo-trust-pill">
                <i class="ri-time-line"></i>
                <span>${isTr ? '15 Dk Hızlı Tur' : '15-Min Fast Tour'}</span>
              </div>
              <div class="pika-demo-trust-pill">
                <i class="ri-user-star-line"></i>
                <span>${isTr ? 'Birebir Uzman Desteği' : '1-on-1 Specialist'}</span>
              </div>
            </div>
          </div>
        </div>
      `;

      grid = document.createElement('div');
      grid.className = 'pika-demo-grid';
      grid.innerHTML = showcaseHtml;

      const formPane = document.createElement('div');
      formPane.className = 'pika-demo-form-pane';
      formPane.appendChild(form);
      grid.appendChild(formPane);

      let closeBtn = modal.querySelector('.pika-demo-close-btn');
      if (!closeBtn) {
        closeBtn = document.createElement('button');
        closeBtn.type = 'button';
        closeBtn.className = 'btn-close pika-demo-close-btn';
        closeBtn.setAttribute('data-bs-dismiss', 'modal');
        closeBtn.setAttribute('aria-label', isTr ? 'Kapat' : 'Close');
        closeBtn.innerHTML = '<i class="ri-close-line"></i>';
      }

      content.innerHTML = '';
      content.appendChild(closeBtn);
      content.appendChild(grid);
    }

    // 2. Eradicate any rogue legacy elements inside form
    form.querySelectorAll('.modal-header, .modal-footer, .demo-request-modal > p.mb-3, .modal-title, button.btn-close').forEach(el => el.remove());

    // 3. Ensure balanced 5-row field layout (Ad+Soyad, Email+Phone, Company (col-12), Website+City, Message)
    const companyCol = form.querySelector('input[name="companyName"]')?.closest('.col-sm-6, .col-md-6');
    const isLegacyLayout = !form.querySelector('.pika-demo-fields') || !!companyCol;

    if (isLegacyLayout) {
      const vals = {
        firstName: form.querySelector('[name="firstName"]')?.value || '',
        lastName: form.querySelector('[name="lastName"]')?.value || '',
        email: form.querySelector('[name="email"]')?.value || '',
        phone: form.querySelector('[name="phone"]')?.value || '',
        companyName: form.querySelector('[name="companyName"]')?.value || '',
        website: form.querySelector('[name="website"]')?.value || '',
        city: form.querySelector('[name="city"]')?.value || '',
        message: form.querySelector('[name="message"]')?.value || ''
      };

      const recipientEmail = form.dataset.recipientEmail || '';

      form.innerHTML = `
        <div class="pika-demo-form-header">
          <div class="pika-demo-form-eyebrow">${isTr ? 'HIZLI BAŞLANGIÇ' : 'GET STARTED'}</div>
          <h4 class="pika-demo-form-title" id="demoModalTitle">${isTr ? 'Demo Talep Edin' : 'Request a Live Demo'}</h4>
          <p class="pika-demo-form-subtitle">${isTr ? 'Formu doldurun, ürün uzmanımız 24 saat içinde sizinle iletişime geçsin.' : 'Fill in your details and our product specialists will connect with you within 24 hours.'}</p>
        </div>

        <div class="pika-demo-fields">
          <div class="row g-3">
            <!-- Row 1: Ad & Soyad -->
            <div class="col-sm-6">
              <div class="pika-input-group">
                <label class="pika-input-label">${isTr ? 'Adınız' : 'First Name'} <span class="text-danger">*</span></label>
                <div class="pika-input-wrap">
                  <i class="ri-user-line pika-input-icon"></i>
                  <input class="form-control pika-form-control" name="firstName" required placeholder="${isTr ? 'Adınız' : 'First Name'}" autocomplete="given-name" value="${vals.firstName.replace(/"/g, '&quot;')}" />
                </div>
              </div>
            </div>
            <div class="col-sm-6">
              <div class="pika-input-group">
                <label class="pika-input-label">${isTr ? 'Soyadınız' : 'Last Name'} <span class="text-danger">*</span></label>
                <div class="pika-input-wrap">
                  <i class="ri-user-line pika-input-icon"></i>
                  <input class="form-control pika-form-control" name="lastName" required placeholder="${isTr ? 'Soyadınız' : 'Last Name'}" autocomplete="family-name" value="${vals.lastName.replace(/"/g, '&quot;')}" />
                </div>
              </div>
            </div>

            <!-- Row 2: E-posta & Telefon -->
            <div class="col-sm-6">
              <div class="pika-input-group">
                <label class="pika-input-label">${isTr ? 'İş E-postası' : 'Work Email'} <span class="text-danger">*</span></label>
                <div class="pika-input-wrap">
                  <i class="ri-mail-line pika-input-icon"></i>
                  <input class="form-control pika-form-control" type="email" name="email" required placeholder="ad@sirketiniz.com" autocomplete="email" value="${vals.email.replace(/"/g, '&quot;')}" />
                </div>
              </div>
            </div>
            <div class="col-sm-6">
              <div class="pika-input-group">
                <label class="pika-input-label">${isTr ? 'Telefon Numarası' : 'Phone Number'} <span class="text-danger">*</span></label>
                <div class="pika-input-wrap pika-phone-wrap">
                  <span class="pika-phone-code"><i class="ri-phone-line"></i> +90</span>
                  <input class="form-control pika-form-control pika-phone-input" name="phone" type="tel" required maxlength="13" inputmode="numeric" placeholder="5XX XXX XX XX" autocomplete="tel" value="${vals.phone.replace(/"/g, '&quot;')}" />
                </div>
              </div>
            </div>

            <!-- Row 3: Şirket Adı (Tam Satır - Boşluk Bırakmaz) -->
            <div class="col-12">
              <div class="pika-input-group">
                <label class="pika-input-label">${isTr ? 'Şirket Adı' : 'Company Name'} <span class="text-danger">*</span></label>
                <div class="pika-input-wrap">
                  <i class="ri-building-line pika-input-icon"></i>
                  <input class="form-control pika-form-control" name="companyName" required placeholder="${isTr ? 'Şirketinizin Adı' : 'Your Company Name'}" autocomplete="organization" value="${vals.companyName.replace(/"/g, '&quot;')}" />
                </div>
              </div>
            </div>

            <!-- Row 4: Web Sitesi & Şehir (Dengeli İkili Satır) -->
            <div class="col-sm-6">
              <div class="pika-input-group">
                <label class="pika-input-label">${isTr ? 'Web Sitesi' : 'Website'} <span class="pika-label-sub">(${isTr ? 'İsteğe Bağlı' : 'Optional'})</span></label>
                <div class="pika-input-wrap">
                  <i class="ri-global-line pika-input-icon"></i>
                  <input class="form-control pika-form-control" name="website" placeholder="sirketiniz.com" autocomplete="url" value="${vals.website.replace(/"/g, '&quot;')}" />
                </div>
              </div>
            </div>
            <div class="col-sm-6">
              <div class="pika-input-group">
                <label class="pika-input-label">${isTr ? 'Şehir' : 'City'} <span class="text-danger">*</span></label>
                <div class="pika-input-wrap">
                  <i class="ri-map-pin-line pika-input-icon"></i>
                  <input class="form-control pika-form-control" name="city" required placeholder="${isTr ? 'Örn. İstanbul' : 'e.g. Istanbul'}" value="${vals.city.replace(/"/g, '&quot;')}" />
                </div>
              </div>
            </div>

            <!-- Row 5: Mesaj (Tam Satır) -->
            <div class="col-12">
              <div class="pika-input-group">
                <label class="pika-input-label">${isTr ? 'Mesajınız veya İhtiyaçlarınız' : 'Your Message or Requirements'} <span class="pika-label-sub">(${isTr ? 'İsteğe Bağlı' : 'Optional'})</span></label>
                <div class="pika-input-wrap">
                  <textarea class="form-control pika-form-control pika-textarea" rows="3" name="message" placeholder="${isTr ? 'Öne çıkarmak istediğiniz hedefler veya sorularınız...' : 'Goals, target channels, or questions for the demo...'}">${vals.message}</textarea>
                </div>
              </div>
            </div>
          </div>
        </div>

        <div id="demoResult" class="pika-alert d-none mt-3"></div>

        <div class="pika-demo-form-footer">
          <button type="submit" class="pika-demo-submit-btn">
            <span class="pika-demo-btn-text">${isTr ? 'Ücretsiz Demo Talep Edin' : 'Request Free Demo'}</span>
            <i class="ri-arrow-right-up-line pika-demo-btn-icon"></i>
          </button>
          <div class="pika-demo-privacy-note">
            <i class="ri-lock-2-line"></i>
            <span>${isTr ? 'Bilgileriniz KVKK kapsamında korunur ve 3. taraflarla paylaşılmaz.' : 'Your data is strictly protected under KVKK/GDPR and never shared.'}</span>
          </div>
        </div>
      `;

      if (recipientEmail) {
        form.dataset.recipientEmail = recipientEmail;
      }
    }

    // 4. Close button guarantee
    let closeBtn = modal.querySelector('.pika-demo-close-btn');
    if (closeBtn && !closeBtn.querySelector('i')) {
      closeBtn.innerHTML = '<i class="ri-close-line"></i>';
    }

    // 5. Phone input formatting mask
    const phoneInput = form.querySelector('input[name="phone"]');
    if (phoneInput && !phoneInput.dataset.formatted) {
      phoneInput.dataset.formatted = 'true';
      phoneInput.addEventListener('input', function() {
        const v = this.value.replace(/\D/g, '').substring(0, 10);
        let f = '';
        if (v.length > 0) f = v.substring(0, 3);
        if (v.length > 3) f += ' ' + v.substring(3, 6);
        if (v.length > 6) f += ' ' + v.substring(6, 8);
        if (v.length > 8) f += ' ' + v.substring(8, 10);
        this.value = f;
      });
    }
  }

  function enhanceMobileDrawer() {
    const drawer = document.getElementById('navbarOffcanvas');
    if (!drawer) return;

    // 1. Ensure clean Orbit branding in drawer header
    const header = drawer.querySelector('.offcanvas-header');
    if (header) {
      const logoLink = header.querySelector('.logo, .logo-brand');
      if (logoLink) {
        const legacyImg = logoLink.querySelector('img');
        if (legacyImg) {
          logoLink.innerHTML = `
            <span class="orbit-nav-mark" aria-hidden="true"></span>
            <span class="orbit-nav-word">pika</span>
          `;
          logoLink.className = 'logo navbar-brand logo-brand p-0 d-inline-flex align-items-center';
        }
      }

      const closeBtn = header.querySelector('.close-btn');
      if (closeBtn && (!closeBtn.querySelector('i') || closeBtn.querySelector('.ri-close-fill'))) {
        closeBtn.innerHTML = '<i class="ri-close-line"></i>';
      }
    }

    // 2. Ensure others-options uses modern segmented language switch and clean buttons
    const others = drawer.querySelector('.others-options');
    if (others && !others.querySelector('.pika-mobile-lang-segmented')) {
      const isEn = window.location.pathname.startsWith('/en') || (document.documentElement.lang && document.documentElement.lang.toLowerCase().startsWith('en'));
      const isTr = !isEn;
      others.innerHTML = `
        <div class="pika-mobile-actions">
          <div class="pika-mobile-lang-segmented" role="group" aria-label="Dil Seçimi">
            <button type="button" class="pika-mobile-lang-pill ${isTr ? 'active' : ''}" data-lang-set="tr">
              <i class="ri-global-line"></i>
              <span>Türkçe (TR)</span>
            </button>
            <button type="button" class="pika-mobile-lang-pill ${isEn ? 'active' : ''}" data-lang-set="en">
              <i class="ri-global-line"></i>
              <span>English (EN)</span>
            </button>
          </div>
          <a href="/Account/Login" class="pika-mobile-login-btn">
            <i class="ri-user-line"></i>
            <span>${isTr ? 'Giriş' : 'Login'}</span>
          </a>
          <button type="button" class="pika-mobile-demo-btn" data-bs-toggle="modal" data-bs-target="#demoModal">
            <span>${isTr ? 'Demo Talebi' : 'Demo Request'}</span>
            <i class="ri-arrow-right-up-line"></i>
          </button>
        </div>
      `;

      others.querySelectorAll('[data-lang-set]').forEach(btn => {
        btn.addEventListener('click', () => {
          const lang = btn.getAttribute('data-lang-set');
          const fallback = lang === 'en' ? '/en/' : '/';
          const alternate = document.querySelector(`link[rel="alternate"][hreflang="${lang}"]`)?.getAttribute('href');
          window.location.assign(alternate || fallback);
        });
      });
    }
  }

  if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', () => {
      enhanceDemoModal();
      enhanceMobileDrawer();
    });
  } else {
    enhanceDemoModal();
    enhanceMobileDrawer();
  }

  document.addEventListener('show.bs.modal', (e) => {
    if (e.target && e.target.id === 'demoModal') {
      enhanceDemoModal();
    }
  });

  document.addEventListener('show.bs.offcanvas', (e) => {
    if (e.target && e.target.id === 'navbarOffcanvas') {
      enhanceMobileDrawer();
    }
  });
})();



