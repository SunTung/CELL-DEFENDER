using System.Collections.Generic;
using UnityEngine;
namespace CellDefender {
 public static class AttackPattern {
  public static List<Vector2> Directions(AttackKind kind,Vector2 forward,int count,float angleDegrees){
   var result=new List<Vector2>();
   if(forward.sqrMagnitude<0.0001f)forward=Vector2.down;
   forward.Normalize();
   if(kind!=AttackKind.Spread){result.Add(forward);return result;}
   count=Mathf.Clamp(count,1,32);
   for(int i=0;i<count;i++){
    float angle=(i-(count-1)*0.5f)*angleDegrees;
    result.Add(Quaternion.Euler(0,0,angle)*forward);
   }
   return result;
  }
 }
}
