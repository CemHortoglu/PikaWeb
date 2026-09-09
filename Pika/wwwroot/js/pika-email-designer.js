(() => {
 const root=document.querySelector('.email-lab'); if(!root)return;
 const tr=root.dataset.language==='tr', canvas=root.querySelector('.email-canvas'), edit=root.querySelector('.email-edit'), status=root.querySelector('.email-status');
 let selected=null, dragged=null;
 const defaults={heading:tr?'Size özel, yeni bir başlangıç.':'A fresh start, just for you.',text:tr?'Sevdiğiniz detayları yeniden keşfedin. Yeni koleksiyonumuzla tanışın.':'Rediscover the details you love. Meet our new collection.',button:tr?'Koleksiyonu keşfet':'Explore the collection'};
 function select(block){selected=block;canvas.querySelectorAll('.mail-block').forEach(b=>b.classList.toggle('selected',b===block));edit.disabled=!block||!defaults[block.dataset.type];edit.value=edit.disabled?'':block.querySelector('.mail-content').textContent;root.querySelectorAll('[data-action="up"],[data-action="down"],[data-action="remove"]').forEach(b=>b.disabled=!block);}
 function add(type,before=null){if(!['heading','text','image','button','divider'].includes(type))return;const b=document.createElement('div');b.className='mail-block';b.dataset.type=type;b.draggable=true;b.tabIndex=0;b.setAttribute('aria-label',`${type} — ${tr?'Seç ve düzenle':'Select to edit'}`);let c=document.createElement(type==='image'?'img':type==='divider'?'hr':type==='heading'?'h3':'div');c.className='mail-content';if(type==='image'){c.src='/images/pika-commerce-editorial.png';c.alt=tr?'Örnek koleksiyon görseli':'Sample collection image';c.draggable=false;}else c.textContent=defaults[type]||'';b.append(c);canvas.insertBefore(b,before);select(b);status.textContent=tr?'Blok eklendi.':'Block added.';}
 root.querySelectorAll('[data-block]').forEach(b=>{b.addEventListener('click',()=>add(b.dataset.block));b.addEventListener('dragstart',e=>{dragged=null;e.dataTransfer.setData('text/plain',b.dataset.block);});});
 canvas.addEventListener('click',e=>{const b=e.target.closest('.mail-block');if(b)select(b);});canvas.addEventListener('keydown',e=>{if(e.key==='Enter'||e.key===' '){e.preventDefault();select(e.target.closest('.mail-block'));}});
 canvas.addEventListener('dragstart',e=>{dragged=e.target.closest('.mail-block');if(dragged)e.dataTransfer.setData('text/plain',dragged.dataset.type);});
 canvas.addEventListener('dragover',e=>{e.preventDefault();canvas.classList.add('drag-over');});canvas.addEventListener('dragleave',()=>canvas.classList.remove('drag-over'));canvas.addEventListener('drop',e=>{e.preventDefault();canvas.classList.remove('drag-over');let before=e.target.closest('.mail-block');if(before&&e.clientY>before.getBoundingClientRect().top+before.offsetHeight/2)before=before.nextElementSibling;if(dragged){if(before!==dragged)canvas.insertBefore(dragged,before);select(dragged);status.textContent=tr?'Blok taşındı.':'Block moved.';}else add(e.dataTransfer.getData('text/plain'),before);dragged=null;});root.addEventListener('dragend',()=>{dragged=null;canvas.classList.remove('drag-over');});
 edit.addEventListener('input',()=>{if(selected&&!edit.disabled)selected.querySelector('.mail-content').textContent=edit.value;});
 root.querySelectorAll('[data-action]').forEach(b=>b.addEventListener('click',()=>{const a=b.dataset.action;if(a==='preview'){const mobile=canvas.classList.toggle('mobile');b.textContent=mobile?(tr?'Masaüstü görünüm':'Desktop view'):(tr?'Mobil görünüm':'Mobile view');return;}if(!selected)return;if(a==='remove'){selected.remove();select(null);}if(a==='up'&&selected.previousElementSibling)canvas.insertBefore(selected,selected.previousElementSibling);if(a==='down'&&selected.nextElementSibling)canvas.insertBefore(selected.nextElementSibling,selected);status.textContent=tr?'Tasarım güncellendi.':'Design updated.';}));
 ['image','heading','text','button'].forEach(t=>add(t));select(null);status.textContent='';
 // One introductory demonstration; any user interaction immediately takes over.
 const motion=matchMedia('(prefers-reduced-motion: reduce)');
 let demoStarted=false, demoStopped=false;
 const timers=[], animations=[];
 const ghost=document.createElement('div');
 ghost.className='email-demo-ghost'; ghost.setAttribute('aria-hidden','true');
 const later=(fn,ms)=>timers.push(setTimeout(()=>{if(!demoStopped)fn();},ms));
 function stopDemo(){
  demoStopped=true;timers.forEach(clearTimeout);animations.forEach(a=>a.cancel());ghost.remove();
  canvas.classList.remove('drag-over');root.querySelectorAll('.demo-source').forEach(b=>b.classList.remove('demo-source'));
  if(demoStarted){select(null);status.textContent=tr?'Sıra sizde. Blokları sürükleyerek tasarlayın.':'Your turn. Drag blocks to design.';}
 }
 function demonstrate(source,target,commit){
  if(!source||!target)return;
  const a=source.getBoundingClientRect(),b=target.getBoundingClientRect();
  ghost.textContent='↖  '+(source.dataset.block?source.textContent:(tr?'İçerik bloğu':'Content block'));
  ghost.style.left=`${a.left+12}px`;ghost.style.top=`${a.top+12}px`;
  document.body.append(ghost);source.classList.add('demo-source');canvas.classList.add('drag-over');
  animations.push(ghost.animate([{transform:'translate(0,0) scale(.96)',opacity:0},{transform:'translate(0,0) scale(1)',opacity:1,offset:.15},{transform:`translate(${b.left-a.left+24}px,${b.top-a.top}px) scale(1.03)`,opacity:1,offset:.85},{transform:`translate(${b.left-a.left+24}px,${b.top-a.top}px) scale(1)`,opacity:0}],{duration:2400,easing:'ease-in-out',fill:'forwards'}));
  later(()=>{commit();source.classList.remove('demo-source');canvas.classList.remove('drag-over');ghost.remove();select(null);},2400);
 }
 function startDemo(){
  if(demoStarted||demoStopped||motion.matches)return;
  demoStarted=true;status.textContent=tr?'Kısa bir tasarım gösterimi · Dilediğiniz an başlayabilirsiniz.':'A short design demonstration · Start whenever you like.';
  later(()=>demonstrate(canvas.querySelector('[data-type="heading"]'),canvas.firstElementChild,()=>canvas.prepend(canvas.querySelector('[data-type="heading"]'))),500);
  later(()=>demonstrate(canvas.querySelector('[data-type="image"]'),canvas.firstElementChild,()=>canvas.prepend(canvas.querySelector('[data-type="image"]'))),4300);
  later(()=>demonstrate(root.querySelector('[data-block="divider"]'),canvas.querySelector('[data-type="button"]'),()=>add('divider',canvas.querySelector('[data-type="button"]'))),8100);
  later(stopDemo,12000);
 }
 root.addEventListener('pointerdown',stopDemo,{once:true,capture:true});
 root.addEventListener('keydown',stopDemo,{once:true,capture:true});
 addEventListener('resize',()=>{if(demoStarted)stopDemo();},{passive:true});
 document.addEventListener('visibilitychange',()=>{if(document.hidden&&demoStarted)stopDemo();});
 motion.addEventListener('change',()=>{if(motion.matches)stopDemo();});
 if('IntersectionObserver' in window){const introObserver=new IntersectionObserver(entries=>{if(entries[0].isIntersecting)startDemo();else if(demoStarted)stopDemo();},{threshold:.35});introObserver.observe(root.querySelector('.email-workbench'));}
})();

