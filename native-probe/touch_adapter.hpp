#pragma once
#include <algorithm>
#include <cmath>
#include <cstdint>
namespace cell {
struct TouchState { bool down=false; float x=0,y=0; };
struct Viewport { float width=360,height=640; };
class TouchAdapter {
 float previous_x_=0, previous_y_=0; bool tracking_=false;
public:
 template<class Runtime> bool apply(Runtime& runtime,std::uint64_t player_id,TouchState touch,Viewport view){
  if(view.width<=0||view.height<=0||!std::isfinite(touch.x)||!std::isfinite(touch.y))return false;
  if(!touch.down){tracking_=false;return true;}
  float x=std::clamp(touch.x,0.0f,view.width),y=std::clamp(touch.y,0.0f,view.height);
  if(!tracking_){previous_x_=x;previous_y_=y;tracking_=true;return true;}
  const double dx=static_cast<double>(x-previous_x_)*9.0/view.width;
  const double dy=static_cast<double>(y-previous_y_)*16.0/view.height;
  previous_x_=x;previous_y_=y;
  return runtime.queue_nudge(player_id,dx,dy);
 }
 template<class Runtime> bool autofire(Runtime& runtime,std::uint64_t player_id){return runtime.trigger_attack(player_id);}
};
}
