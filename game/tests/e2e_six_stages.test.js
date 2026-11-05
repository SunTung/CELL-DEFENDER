const assert=require('node:assert/strict');
const {Game}=require('../web/game.js');
const g=new Game(()=>0.5);let ticks=0,bossKills=0,transitions=[],events={};
const record=()=>{for(const s of g.drain())events[s]=(events[s]||0)+1};
assert.equal(g.state,'intro');g.menu();assert.equal(g.state,'menu');g.start();record();assert.equal(g.state,'playing');
for(let i=0;i<100000 && g.state==='playing' && !g.endless;i++){
  g.hp=100;g.shield=9;
  // Scripted test pilot: strike the actual boss entity with real projectile collisions.
  const boss=g.enemies.find(e=>e.type==='boss'&&!e.dead);
  if(boss){
    // Place test shots on collision course; tick performs damage and defeat handling.
    g.bullets.push({x:boss.x,y:boss.y+4,damage:5});
  }
  const before=g.stage;
  g.tick(.05);ticks++;record();
  if(g.stage!==before){transitions.push([before,g.stage]);assert.equal(g.stage,before+1)}
}
assert.equal(g.state,'clear','six stages must reach clear screen');assert.equal(g.stage,6);assert.equal(g.bossesDefeated,6);assert.equal(transitions.length,5);assert.equal(g.stageTime>=60,true);assert.equal(g.startEndless(),true);record();assert.equal(g.stage,7);assert.equal(g.endless,true);assert.equal(g.state,'playing');
const before=g.time;for(let i=0;i<100;i++){g.hp=100;g.tick(.05)}record();assert(g.time>before);assert(events.shoot>0);assert(events.explosion>=6);assert(events.mission_clear>=6);assert(events.start>=2);
console.log(JSON.stringify({result:'PASS',ticks,simulatedSeconds:Math.round(ticks*.05),bossKills:g.bossesDefeated,transitions,events,finalState:g.state,stage:g.stage},null,2));
