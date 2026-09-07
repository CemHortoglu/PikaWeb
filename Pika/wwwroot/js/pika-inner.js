(() => {
 'use strict';
 if (!document.body.classList.contains('pika-inner')) return;
 const reduced = matchMedia('(prefers-reduced-motion: reduce)');
 if ('IntersectionObserver' in window) {
  const observer = new IntersectionObserver(entries => entries.forEach(entry => {
   entry.target.style.animationPlayState = entry.isIntersecting ? 'running' : 'paused';
  }), {rootMargin:'50px'});
  document.querySelectorAll('.inner-scene-window,.inner-atmosphere > span,.inner-flow-link,.inner-chart-line').forEach(el=>observer.observe(el));
  const reveal = new IntersectionObserver(entries => entries.forEach(entry => {
   if(entry.isIntersecting){if(!reduced.matches)entry.target.classList.add('inner-revealed');reveal.unobserve(entry.target);}
  }), {threshold:.08});
  document.querySelectorAll('main > section:not(.inner-hero) > .container').forEach(el=>reveal.observe(el));
 }
 // Keep original accordion, form and navigation handlers; expose the current page.
 document.querySelectorAll('#navbar a[href]').forEach(link => {
  if(new URL(link.href, location.href).pathname.replace(/\/$/,'') === location.pathname.replace(/\/$/,'')) link.setAttribute('aria-current','page');
 });
 document.querySelectorAll('main form label').forEach((label,index)=>{
  if(label.htmlFor) return;
  const field = label.parentElement.querySelector('input,textarea,select');
  if(field){if(!field.id)field.id=`inner-field-${index}`;label.htmlFor=field.id;}
 });
})();
