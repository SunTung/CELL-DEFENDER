#include <array>
#include <cassert>
#include <iostream>
#include <algorithm>
struct Player {int hp=100, shield=0, xp=0, level=1; std::array<int,4> skills{};};
void damage(Player& p,int amount){int blocked=std::min(p.shield,amount);p.shield-=blocked;p.hp=std::max(0,p.hp-(amount-blocked));}
void pickup(Player& p,int type){if(type==0)p.hp=std::min(100,p.hp+25);else if(type==1)p.shield=std::min(9,p.shield+3);else p.xp+=10;}
bool upgrade(Player& p,int skill){if(skill<0||skill>=4||p.skills[skill]>=6)return false;++p.skills[skill];return true;}
int main(){Player p;pickup(p,1);pickup(p,1);pickup(p,1);pickup(p,1);assert(p.shield==9);damage(p,11);assert(p.shield==0&&p.hp==98);pickup(p,0);assert(p.hp==100);pickup(p,2);assert(p.xp==10);for(int i=0;i<6;i++)assert(upgrade(p,0));assert(!upgrade(p,0));assert(!upgrade(p,4));damage(p,500);assert(p.hp==0);std::cout<<"CELL DEFENDER native rules smoke test: PASS\n";}
