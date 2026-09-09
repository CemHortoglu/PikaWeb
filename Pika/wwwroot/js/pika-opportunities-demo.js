(() => {
 const demo=document.querySelector('.opp-demo');
 if(!demo||!('IntersectionObserver' in window)||matchMedia('(prefers-reduced-motion: reduce)').matches)return;
 demo.classList.add('is-ready');
 const observer=new IntersectionObserver(entries=>{if(entries[0].isIntersecting){demo.classList.add('is-playing');observer.disconnect();}},{threshold:.15});observer.observe(demo);
})();
