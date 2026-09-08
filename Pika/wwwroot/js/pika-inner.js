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

(() => {
 const desktop = matchMedia('(min-width:1100px)');
 document.querySelectorAll('.pika-story-gallery').forEach(gallery => {
  const cards = [...gallery.querySelectorAll('.pika-story-card')];
  let selected = 0;
  const render = () => {
   const expandable = desktop.matches && cards.length <= 4;
   gallery.classList.toggle('is-enhanced', expandable);
   cards.forEach((card, index) => {
    const open = !expandable || index === selected;
    card.classList.toggle('is-active', index === selected);
    card.querySelector('button').setAttribute('aria-expanded', String(open));
    card.querySelector('p').hidden = !open;
   });
  };
  cards.forEach((card, index) => {
   const button = card.querySelector('button');
   button.addEventListener('click', () => { selected = index; render(); });
   button.addEventListener('keydown', event => {
    if (!desktop.matches || cards.length > 4) return;
    const offsets = { ArrowRight:1, ArrowLeft:-1 };
    if (!(event.key in offsets) && event.key !== 'Home' && event.key !== 'End') return;
    event.preventDefault();
    selected = event.key === 'Home' ? 0 : event.key === 'End' ? cards.length - 1 : (index + offsets[event.key] + cards.length) % cards.length;
    render();
    cards[selected].querySelector('button').focus();
   });
  });
  desktop.addEventListener('change', render);
  render();
 });
})();
