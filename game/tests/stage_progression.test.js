const assert=require('node:assert/strict');const {Game,STAGES}=require('../web/game.js');
assert.equal(STAGES.length,6);
let sounds=[];const g=new Game(()=>0.5);g.menu();g.start();sounds.push(...g.drain());
assert.equal(g.state,'playing');assert.equal(g.stage,1);assert.equal(g.upgrade('clone'),false,'upgrades require earned level');g.pendingUpgrades=24;
for(const skill of ['bullet','beam','defense','clone']){for(let i=0;i<6;i++)assert.equal(g.upgrade(skill),true);assert.equal(g.upgrade(skill),false);}
assert.equal(g.shield,9);g.pause();const t=g.time;g.tick(.05);assert.equal(g.time,t);g.pause();
for(let stage=1;stage<=6;stage++){
  g.stageTime=49.99;g.bossSpawned=false;g.tick(.05);assert.equal(g.bossSpawned,true);
  g.stageTime=59.99;g.tick(.05);assert.equal(g.stage,stage,'boss must be defeated before advance');for(const e of g.enemies)if(e.type==='boss')e.dead=true;g.tick(.05);
  if(stage<6){assert.equal(g.stage,stage+1);assert.equal(g.state,'playing');}else assert.equal(g.state,'clear');
}
assert.equal(g.stage,6);assert.equal(g.state,'clear');
const h=new Game();h.start();h.hurt(1000);assert.equal(h.state,'over');assert.ok(h.drain().includes('game_over'));
console.log('six-stage progression, bosses, pause, upgrades, ending: PASS');
assert.equal(g.startEndless(),true);assert.equal(g.stage,7);assert.equal(g.endless,true);assert.equal(g.state,'playing');
