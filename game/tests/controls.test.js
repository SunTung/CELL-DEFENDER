const assert=require('node:assert/strict');const {Controller,deltaAngle}=require('../web/controls.js');
let c=new Controller();assert(c.beginTouch(1,100,180));assert.equal(c.drag(1,160),240);assert.equal(c.drag(2,160),null);assert.equal(c.drag(1,-500),16);c.endTouch(1);assert.equal(c.drag(1,200),null);
assert(c.setMode('tilt'));assert.equal(c.calibrate(),false);c.orientation(0,0);assert(c.calibrate());c.orientation(25,0);let x=180;for(let i=0;i<60;i++)x=c.step(x,1/60);assert(x>200&&x<=344);c.orientation(-25,0);for(let i=0;i<60;i++)x=c.step(x,1/60);assert(x<344);
assert(c.setMode('yaw'));c.orientation(0,359);assert(c.calibrate());c.orientation(0,1);assert.equal(deltaAngle(1,359),2);x=180;for(let i=0;i<60;i++)x=c.step(x,1/60);assert.equal(x,180);c.orientation(0,25);for(let i=0;i<60;i++)x=c.step(x,1/60);assert(x>180);
assert(c.setMode('touch'));assert.equal(c.step(180,1/60),180);assert.equal(c.setMode('invalid'),false);console.log('CONTROL MODES PASS');
