(() => {
 const demo=document.querySelector('.journey-demo');
 if(!demo||!('IntersectionObserver' in window)||matchMedia('(prefers-reduced-motion: reduce)').matches)return;
 demo.classList.add('is-ready');
 const observer=new IntersectionObserver(entries=>{if(entries[0].isIntersecting){demo.classList.add('is-playing');observer.disconnect();}},{threshold:.2});
 observer.observe(demo);
})();
