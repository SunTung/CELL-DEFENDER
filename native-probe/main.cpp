#include <array>
#include <algorithm>
#include <iostream>
#include <stdexcept>
struct Player { int hp=100, shield=0, xp=0; std::array<int,4> skills{}; };
void check(bool ok,const char* msg){if(!ok)throw std::runtime_error(msg);}
void damage(Player& p,int amount){if(amount<=0)return;int b=std::min(p.shield,amount);p.shield-=b;p.hp=std::max(0,p.hp-(amount-b));}
void pickup(Player& p,int type){if(type==0)p.hp=std::min(100,p.hp+25);else if(type==1)p.shield=std::min(9,p.shield+3);else if(type==2)p.xp+=10;}
bool upgrade(Player& p,int skill){if(skill<0||skill>=4||p.skills[skill]>=6)return false;++p.skills[skill];return true;}
struct Bullet {float x=0,y=0,vx=0,vy=0;bool alive=true;};
struct Enemy {float x=0,y=0;int hp=3;bool alive=true;};
struct World {
 Player player;
 std::array<Bullet,16> bullets{};
 std::array<Enemy,8> enemies{};
 int bullet_count=0,enemy_count=0,kills=0;
 bool fire(float x,float y,float vx,float vy){if(bullet_count>=16)return false;bullets[bullet_count++]={x,y,vx,vy,true};return true;}
 bool spawn(float x,float y,int hp){if(enemy_count>=8)return false;enemies[enemy_count++]={x,y,hp,true};return true;}
 void tick(float dt){if(dt<=0||dt>1)return;for(int i=0;i<bullet_count;++i){auto& b=bullets[i];if(!b.alive)continue;b.x+=b.vx*dt;b.y+=b.vy*dt;for(int j=0;j<enemy_count;++j){auto& e=enemies[j];if(!e.alive)continue;float dx=b.x-e.x,dy=b.y-e.y;if(dx*dx+dy*dy<=0.25f){b.alive=false;if(--e.hp<=0){e.alive=false;++kills;player.xp+=10;}break;}}}}
};
int main(){try{
 Player p;for(int i=0;i<4;++i)pickup(p,1);check(p.shield==9,"shield cap");damage(p,11);check(p.shield==0&&p.hp==98,"damage absorption");damage(p,-50);check(p.hp==98,"negative damage");pickup(p,0);check(p.hp==100,"healing cap");pickup(p,2);check(p.xp==10,"energy");
 for(int i=0;i<6;++i)check(upgrade(p,0),"skill");check(!upgrade(p,0)&&!upgrade(p,-1)&&!upgrade(p,4),"skill cap");
 damage(p,500);check(p.hp==0,"fatal damage");
 World w;check(w.spawn(1,0,2),"spawn");check(w.fire(0,0,1,0),"fire");w.tick(1);check(w.enemies[0].hp==1&&w.kills==0,"hit");check(w.fire(0,0,1,0),"fire2");w.tick(1);check(!w.enemies[0].alive&&w.kills==1&&w.player.xp==10,"kill");w.tick(1);check(w.kills==1&&w.player.xp==10,"reward once");
 std::cout<<"CELL DEFENDER native gameplay smoke test: PASS\n";return 0;
 }catch(const std::exception& e){std::cerr<<"FAIL: "<<e.what()<<'\n';return 1;}}
