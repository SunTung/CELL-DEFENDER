const assert=require('node:assert/strict');const {Game}=require('../web/game.js');
let cycles=0,shots=0,clears=0,overs=0;const issues=[];
for(let seed=1;seed<=1000;seed++){
 let x=seed;const rng=()=>{x=(Math.imul(1664525,x)+1013904223)>>>0;return x/4294967296;};
 const g=new Game(rng);g.menu();g.start();g.drain();
 for(let t=0;t<1000&&g.state==='playing';t++){
  if(t%30===0)g.move((seed+t)%360,480);
  if(t%160===0)g.heal();g.tick(.05);
  for(const e of g.drain())if(e==='shoot')shots++;
  if(g.hp<0||g.hp>100||g.shield<0||g.shield>9||!Number.isFinite(g.score)||!Number.isFinite(g.xp)){issues.push('invalid state seed='+seed);break;}
 }
 if(g.state==='clear')clears++;else if(g.state==='over')overs++;else if(g.state!=='playing')issues.push('invalid terminal state '+g.state);
 g.start();assert.equal(g.hp,100);assert.equal(g.score,0);assert.equal(g.state,'playing');cycles++;
}
console.log(JSON.stringify({cycles,shots,clears,overs,issues,pass:issues.length===0},null,2));if(issues.length)process.exit(1);
