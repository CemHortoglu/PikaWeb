(() => {
  'use strict';
  const canvas = document.getElementById('pika-orbit-canvas');
  if (!canvas) return;
  const context = canvas.getContext('2d');
  if (!context) return;
  const reduced = matchMedia('(prefers-reduced-motion: reduce)');
  let width = 0, height = 0, frame = 0, visible = true, phase = 0, last = 0;
  let mouseX = 0, mouseY = 0;
  function resize() {
    const box = canvas.getBoundingClientRect();
    width = box.width; height = box.height;
    const dpr = Math.min(devicePixelRatio || 1, 2);
    canvas.width = Math.round(width * dpr); canvas.height = Math.round(height * dpr);
    context.setTransform(dpr, 0, 0, dpr, 0, 0);
    draw();
  }
  function draw() {
    context.clearRect(0, 0, width, height);
    const cx = width * .5, cy = height * .46;
    const radius = Math.min(width * .34, height * .36);
    const glow = context.createRadialGradient(cx,cy,20,cx,cy,radius*1.6);
    glow.addColorStop(0,'rgba(166,221,45,.035)');glow.addColorStop(.65,'rgba(136,185,42,.055)');glow.addColorStop(1,'rgba(100,145,30,0)');
    context.fillStyle=glow;context.fillRect(0,0,width,height);
    // A projected toroidal data field: every point follows the same continuous flow.
    const rotation = phase*.12 + mouseX*.08;
    for(let ring=0;ring<37;ring++){
      const v=ring/37*Math.PI*2;
      for(let point=0;point<135;point++){
        const u=point/135*Math.PI*2+phase*.065;
        const r=radius+Math.cos(v)*radius*.29;
        const x=r*Math.cos(u), y=r*Math.sin(u), z=Math.sin(v)*radius*.29;
        const xx=x*Math.cos(rotation)-y*Math.sin(rotation);
        const yy=x*Math.sin(rotation)+y*Math.cos(rotation);
        const tilt=.72+mouseY*.08;
        const sy=yy*Math.cos(tilt)-z*Math.sin(tilt);
        const depth=yy*Math.sin(tilt)+z*Math.cos(tilt);
        const perspective=650/(650-depth);
        const px=cx+xx*perspective, py=cy+sy*perspective;
        const brightness=(depth/radius+1)*.5;
        const wave=(Math.sin(u*3+v*2+phase)+1)*.5;
        context.fillStyle=`rgba(${151+Math.round(wave*45)},${192+Math.round(wave*49)},${58+Math.round(wave*40)},${.15+brightness*.5})`;
        context.beginPath();context.arc(px,py,(.55+brightness*.65)*perspective,0,Math.PI*2);context.fill();
      }
    }
    context.strokeStyle='rgba(192,223,136,.1)';context.lineWidth=.7;
    context.beginPath();context.ellipse(cx,cy,radius*1.49,radius*.94,-.35,0,Math.PI*2);context.stroke();
    for(let n=0;n<5;n++){
      const a=phase*.15+n*Math.PI*.4;
      const x=Math.cos(a)*radius*1.49,y=Math.sin(a)*radius*.94;
      const px=cx+x*Math.cos(-.35)-y*Math.sin(-.35),py=cy+x*Math.sin(-.35)+y*Math.cos(-.35);
      context.fillStyle='#c6ef6a';context.beginPath();context.arc(px,py,2.4,0,Math.PI*2);context.fill();
    }
  }
  function tick(time){
    frame=0;
    if(!visible || document.hidden || reduced.matches) return;
    if(time-last>32){phase+=Math.min((time-last)/1000,.05);last=time;draw();}
    frame=requestAnimationFrame(tick);
  }
  function sync(){cancelAnimationFrame(frame);frame=0;last=performance.now();if(visible&&!document.hidden&&!reduced.matches)frame=requestAnimationFrame(tick);else draw();}
  new ResizeObserver(resize).observe(canvas);
  new IntersectionObserver(entries=>{visible=entries[0].isIntersecting;sync();},{rootMargin:'80px'}).observe(canvas);
  document.addEventListener('visibilitychange',sync);reduced.addEventListener('change',sync);
  canvas.parentElement.addEventListener('pointermove',event=>{if(reduced.matches||event.pointerType==='touch')return;const r=canvas.getBoundingClientRect();mouseX=(event.clientX-r.left)/r.width-.5;mouseY=(event.clientY-r.top)/r.height-.5;},{passive:true});
  const reveal = new IntersectionObserver(entries=>entries.forEach(entry=>{if(entry.isIntersecting){entry.target.classList.add('orbit-reveal');reveal.unobserve(entry.target);}}),{threshold:.12});
  document.querySelectorAll('.pika-orbit main section:not(#hero) > .pw2-container').forEach(section=>reveal.observe(section));
  resize();sync();
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
})();
