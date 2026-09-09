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
   const expandable = desktop.matches && cards.length <= 5;
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
    if (!desktop.matches || cards.length > 5) return;
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

/* Sparse scroll-linked scenery for long, text-only marketing chapters. */
(() => {
 'use strict';
 if (!document.body.classList.contains('pika-inner')) return;
 const reduced = matchMedia('(prefers-reduced-motion: reduce)');
 const sections = [...document.querySelectorAll('main section')].filter(section =>
  !section.parentElement.closest('section') &&
  section.matches('.ph-section,.pika-sol-section,.pika-section,.pk-section') &&
  !section.matches('[id="faq"],[id="cta"]') &&
  !section.className.match(/hero|cta|faq|dark/) && !section.classList.contains('py-4') &&
  !section.querySelector('img,video,canvas,svg,form,.accordion,.pika-story-gallery,.pika-context-network')
 );
 let variant = 0;
 const layers = [];
 sections.forEach((section, index) => {
  if (index % 2 !== 0 || variant >= 6) return;
  section.classList.add('pika-ambient-section');
  const layer = document.createElement('div');
  layer.className = `pika-ambient-layer pika-ambient-layer--${variant++}`;
  layer.setAttribute('aria-hidden', 'true');
  section.prepend(layer);
  layers.push({section, layer});
 });
 if (!layers.length || !('IntersectionObserver' in window)) return;
 const visible = new Set();
 let frame = 0;
 const update = () => {
  frame = 0;
  visible.forEach(item => {
   const rect = item.section.getBoundingClientRect();
   const progress = Math.max(-1, Math.min(1, (innerHeight / 2 - rect.top - rect.height / 2) / (innerHeight + rect.height)));
   item.layer.style.setProperty('--ambient-y', `${reduced.matches ? 0 : progress * (innerWidth < 768 ? 150 : 320)}px`);
  });
 };
 const schedule = () => {
  if (!frame && !reduced.matches && !document.hidden) frame = requestAnimationFrame(update);
 };
 const observer = new IntersectionObserver(entries => {
  entries.forEach(entry => {
   const item = layers.find(candidate => candidate.section === entry.target);
   if (entry.isIntersecting) visible.add(item); else visible.delete(item);
  });
  schedule();
 }, {rootMargin:'100px'});
 layers.forEach(item => observer.observe(item.section));
 window.addEventListener('scroll', schedule, {passive:true});
 window.addEventListener('resize', schedule, {passive:true});
 document.addEventListener('visibilitychange', schedule);
 reduced.addEventListener('change', () => {
  layers.forEach(item => item.layer.style.setProperty('--ambient-y','0px'));
  schedule();
 });
})();

// One-time chapter arrivals retain fully visible content without JavaScript.
(() => {
 if (matchMedia('(prefers-reduced-motion: reduce)').matches || !('IntersectionObserver' in window)) return;
 const targets = document.querySelectorAll('.pika-entity-page .pika-intelligence-col,.pika-entity-page .pika-action-node,.pika-entity-page .pika-principle-entry,.pika-entity-page .pika-flow-step');
 const observer = new IntersectionObserver(entries => entries.forEach(entry => {
  if (!entry.isIntersecting) return;
  entry.target.classList.add('pika-chapter-arrived');
  observer.unobserve(entry.target);
 }), {threshold:.12});
 targets.forEach(target => observer.observe(target));
})();
