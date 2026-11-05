(function(root,factory){const api=factory();if(typeof module==='object'&&module.exports)module.exports=api;else root.CellControls=api;})(typeof globalThis!=='undefined'?globalThis:this,function(){
const clamp=(v,a,b)=>Math.max(a,Math.min(b,v));
const deltaAngle=(a,b)=>((a-b+540)%360)-180;
class Controller{
 constructor(){this.mode='touch';this.touchId=null;this.originX=0;this.playerOrigin=180;this.gamma=null;this.alpha=null;this.zero=null;this.filtered=0;this.sensitivity=1;}
 setMode(mode){if(!['touch','tilt','yaw'].includes(mode))return false;this.mode=mode;this.touchId=null;this.zero=null;this.filtered=0;return true;}
 beginTouch(id,x,playerX){if(this.mode!=='touch'||this.touchId!==null)return false;this.touchId=id;this.originX=x;this.playerOrigin=playerX;return true;}
 drag(id,x){if(this.mode!=='touch'||id!==this.touchId||!Number.isFinite(x))return null;return clamp(this.playerOrigin+x-this.originX,16,344);}
 endTouch(id){if(id===this.touchId)this.touchId=null;}
 orientation(gamma,alpha){if(Number.isFinite(gamma))this.gamma=gamma;if(Number.isFinite(alpha))this.alpha=alpha;}
 calibrate(){const v=this.mode==='tilt'?this.gamma:this.mode==='yaw'?this.alpha:null;if(v===null)return false;this.zero=v;this.filtered=0;return true;}
 step(playerX,dt){if(this.mode==='touch'||!Number.isFinite(dt)||dt<=0)return playerX;const v=this.mode==='tilt'?this.gamma:this.alpha;if(v===null)return playerX;if(this.zero===null){this.zero=v;return playerX;}const d=this.mode==='tilt'?v-this.zero:deltaAngle(v,this.zero);const dead=3;const target=Math.abs(d)<dead?0:clamp((Math.abs(d)-dead)*Math.sign(d)/22,-1,1);this.filtered+=(target-this.filtered)*clamp(dt*12,0,1);return clamp(playerX+this.filtered*240*this.sensitivity*dt,16,344);}
}
return {Controller,deltaAngle};
});
