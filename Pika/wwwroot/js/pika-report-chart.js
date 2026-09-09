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