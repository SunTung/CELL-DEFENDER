(function(root,factory){const api=factory();if(typeof module==='object'&&module.exports)module.exports=api;else root.CellGame=api;})(typeof globalThis!=='undefined'?globalThis:this,function(){
const SOUNDS={start:'start',shoot:'shoot',bullet:'bullet',hit:'player_hit',explosion:'explosion',beam:'beam',heal:'heal',clear:'mission_clear',over:'game_over'};
const STAGES=['血管','肺泡','腸道','肝臟','神經網絡','核心突變'];
const SKILLS=['bullet','beam','defense','clone'];
const ENEMY_TYPES=['strafe','armored','aggressive','escape','stationary','mother'];
const STAGE_ENEMIES=[['strafe','aggressive'],['strafe','escape','mother'],['armored','aggressive'],['armored','stationary','mother'],['strafe','escape','stationary'],['armored','mother','aggressive']];
class Game{
 constructor(random=Math.random){this.random=random;this.soundEnabled=true;this.sound=[];this.state='intro';this.reset();this.state='intro';}
 reset(){this.stage=1;this.endless=false;this.stageTime=0;this.time=0;this.score=0;this.hp=100;this.shield=0;this.xp=0;this.level=1;this.totalXp=0;this.pendingUpgrades=0;this.nextLevelXp=30;this.beamVisualTime=0;this.items={cloneCore:0};this.skills={bullet:0,beam:0,defense:0,clone:0};this.player={x:180,y:530};this.enemies=[];this.bullets=[];this.spawnClock=0;this.fireClock=0;this.beamClock=0;this.kills=0;this.bossSpawned=false;this.enemyShots=[];this.bossFireClock=0;this.cloneFireClock=0;this.bossesDefeated=0;this.state='playing';this.sound=[];}
 emit(k){if(this.soundEnabled&&SOUNDS[k])this.sound.push(SOUNDS[k]);}drain(){return this.sound.splice(0);}
 menu(){this.state='menu';this.sound=[];}start(){this.reset();this.emit('start');}
 pause(){if(this.state==='playing')this.state='paused';else if(this.state==='paused')this.state='playing';}
 move(x,y){if(this.state!=='playing'||!Number.isFinite(x)||!Number.isFinite(y))return;this.player.x=Math.max(16,Math.min(344,x));this.player.y=Math.max(100,Math.min(615,y));}
 heal(){if(this.state!=='playing')return;this.hp=Math.min(100,this.hp+25);this.emit('heal');}
 hurt(n){if(this.state!=='playing'||n<=0)return;const blocked=Math.min(this.shield,n);this.shield-=blocked;this.hp=Math.max(0,this.hp-(n-blocked));this.emit('hit');if(!this.hp){this.state='over';this.emit('over');}}
 upgrade(name){if(this.state!=='playing'||!SKILLS.includes(name)||this.skills[name]>=6||this.pendingUpgrades<=0)return false;this.pendingUpgrades--;this.skills[name]++;if(name==='defense')this.shield=Math.min(9,this.shield+3);return true;}
 spawn(type='normal'){const boss=type==='boss';if(type==='normal'){const pool=STAGE_ENEMIES[Math.min(5,this.stage-1)];type=pool[Math.floor(this.random()*pool.length)];}const hp=boss?30+this.stage*10:type==='armored'?7+this.stage:2+Math.floor(this.stage/2);this.enemies.push({x:boss?180:25+this.random()*310,y:boss?80:-20,hp,r:boss?33:type==='armored'?19:14,type,dead:false,clock:0});}
 fire(){const count=1+Math.floor(this.skills.bullet/2);for(let i=0;i<count;i++)this.bullets.push({x:this.player.x+(i-(count-1)/2)*10,y:this.player.y-18,damage:1+Math.floor(this.skills.bullet/3)});this.emit('shoot');}
 beam(){if(this.skills.beam===0)return;this.beamVisualTime=0.22;this.emit('beam');for(const e of this.enemies){if(e.hp>0&&Math.abs(e.x-this.player.x)<12+this.skills.beam*5){e.hp-=this.skills.beam*2;this.defeat(e);}}}
 defeat(e){if(e.hp<=0&&!e.dead){e.dead=true;this.score+=e.type==='boss'?3000:100;const reward=e.type==='boss'?100:10;this.xp+=reward;this.totalXp+=reward;while(this.xp>=this.nextLevelXp){this.xp-=this.nextLevelXp;this.level++;this.pendingUpgrades++;this.nextLevelXp=Math.ceil(30*Math.pow(1.3,this.level-1));}this.kills++;if(e.type==='boss')this.bossesDefeated++;this.emit('explosion');}}
 nextStage(){if(this.stage<6){this.stage++;this.stageTime=0;this.spawnClock=0;this.enemies=[];this.bullets=[];this.bossSpawned=false;this.enemyShots=[];this.emit('clear');}else{this.endless=true;this.stage++;this.stageTime=0;this.enemies=[];this.bullets=[];this.bossSpawned=false;this.enemyShots=[];this.emit('clear');}}
 finish(){if(this.state==='playing'){this.state='clear';this.emit('clear');}}
 startEndless(){if(this.state!=='clear')return false;this.state='playing';this.endless=true;this.stage=7;this.stageTime=0;this.enemies=[];this.bullets=[];this.bossSpawned=false;this.enemyShots=[];this.emit('start');return true;}
 tick(dt){if(this.state!=='playing'||!Number.isFinite(dt)||dt<=0)return;dt=Math.min(dt,0.05);this.beamVisualTime=Math.max(0,this.beamVisualTime-dt);this.time+=dt;this.stageTime+=dt;this.spawnClock+=dt;this.fireClock+=dt;this.beamClock+=dt;this.cloneFireClock+=dt;this.bossFireClock+=dt;
 if(this.spawnClock>=Math.max(.35,1.05-this.stage*.06)){this.spawnClock=0;this.spawn();}
 if(this.fireClock>=Math.max(.08,.22-this.skills.bullet*.015)){this.fireClock=0;this.fire();}
 if(this.skills.clone&&this.cloneFireClock>=Math.max(.15,.7-this.skills.clone*.08)){this.cloneFireClock=0;const count=Math.min(3,Math.ceil(this.skills.clone/2));for(let i=0;i<count;i++){const x=this.player.x+(i-(count-1)/2)*28;this.bullets.push({x,y:this.player.y-32,damage:1+Math.floor(this.skills.clone/3),clone:true});}this.emit('shoot');}
 if(this.skills.beam&&this.beamClock>=Math.max(1.5,5-this.skills.beam*.5)){this.beamClock=0;this.beam();}
 for(const b of this.bullets)b.y-=400*dt;
 const enemySpeedMultiplier=Math.min(1.8,1+0.16*(this.stage-1));
 for(const e of this.enemies){e.clock+=dt;if(e.type==='boss'){e.x=180+Math.sin(this.time*.7)*95;if(this.bossFireClock>=1.2){this.bossFireClock=0;for(const offset of [-.6,0,.6])this.enemyShots.push({x:e.x,y:e.y+20,vx:offset*85,vy:(145+this.stage*8)*enemySpeedMultiplier});}}else if(e.type==='stationary'){e.y=Math.min(120,e.y+45*enemySpeedMultiplier*dt);}else{e.y+=(e.type==='escape'?120:e.type==='armored'?45:70+this.stage*5)*enemySpeedMultiplier*dt;if(e.type==='strafe')e.x=Math.max(15,Math.min(345,e.x+Math.sin(e.clock*5)*50*enemySpeedMultiplier*dt));if(e.type==='aggressive')e.x+=Math.sign(this.player.x-e.x)*30*enemySpeedMultiplier*dt;if(e.type==='mother'&&e.clock>2){e.clock=0;this.enemyShots.push({x:e.x,y:e.y,vx:0,vy:110*enemySpeedMultiplier});}}}
 for(const shot of this.enemyShots){shot.x+=shot.vx*dt;shot.y+=shot.vy*dt;if(Math.hypot(shot.x-this.player.x,shot.y-this.player.y)<14){shot.y=999;this.hurt(8);}}
 this.enemyShots=this.enemyShots.filter(shot=>shot.y<660&&shot.x>=0&&shot.x<=360);
 for(const b of this.bullets){if(b.y<0)continue;for(const e of this.enemies){if(!e.dead&&Math.hypot(b.x-e.x,b.y-e.y)<e.r+4){e.hp-=b.damage;b.y=-999;this.emit('bullet');this.defeat(e);break;}}}
 this.bullets=this.bullets.filter(b=>b.y>-30);const kept=[];for(const e of this.enemies){if(e.dead)continue;if(e.y>660){this.hurt(10);continue;}if(e.type!=='boss'&&Math.hypot(e.x-this.player.x,e.y-this.player.y)<e.r+13){this.hurt(15);continue;}kept.push(e);}this.enemies=kept;
 if(this.state!=='playing')return;
 if(this.stageTime>=390&&!this.bossSpawned){this.spawn('boss');this.bossSpawned=true;}
 if(this.stageTime>=420&&this.bossSpawned&&!this.enemies.some(e=>e.type==='boss'&&!e.dead)){if(this.stage===6&&!this.endless){this.state='clear';this.emit('clear');}else this.nextStage();}
 }
}
return {Game,SOUNDS,STAGES,SKILLS,ENEMY_TYPES,STAGE_ENEMIES};
});
