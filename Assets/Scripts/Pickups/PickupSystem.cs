using UnityEngine;
namespace CellDefender {
 public static class PickupSystem {
  public static bool Collect(PlayerState p,PickupKind kind){
   if(p==null)return false;
   switch(kind){
    case PickupKind.Health: DamageSystem.Heal(p,Mathf.CeilToInt(p.maxHealth*0.25f));return false;
    case PickupKind.Shield: p.shieldCharges=Mathf.Min(9,p.shieldCharges+3);return false;
    case PickupKind.Energy: return ProgressionSystem.AddEnergy(p,10);
   }
   return false;
  }
 }
}
