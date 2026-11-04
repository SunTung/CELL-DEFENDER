#include "touch_adapter.hpp"
#include <cmath>
#include <iostream>
struct MockRuntime {double dx=0,dy=0; int nudges=0,shots=0;bool queue_nudge(std::uint64_t,double x,double y){dx+=x;dy+=y;++nudges;return true;}bool trigger_attack(std::uint64_t){++shots;return true;}};
int main(){MockRuntime r;cell::TouchAdapter a;cell::Viewport v{360,640};
 if(!a.apply(r,7,{true,180,320},v))return 1;
 if(!a.apply(r,7,{true,216,384},v))return 2;
 if(std::abs(r.dx-0.9)>1e-5||std::abs(r.dy-1.6)>1e-5||r.nudges!=1)return 3;
 if(!a.autofire(r,7)||r.shots!=1)return 4;
 a.apply(r,7,{false,0,0},v);a.apply(r,7,{true,100,100},v);
 if(r.nudges!=1)return 5;
 if(a.apply(r,7,{true,1,1},{0,640}))return 6;
 std::cout<<"CELL touch adapter PASS\n";
}
