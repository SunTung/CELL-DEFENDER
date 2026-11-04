using UnityEngine;
namespace CellDefender {
 public static class DamageSystem {
  public static int Apply(PlayerState player,int incoming) {
   if(player==null || incoming<=0 || !player.IsAlive)return 0;
   if(player.shieldCharges>0){player.shieldCharges--;return 0;}
   int amount=Mathf.Max(1,incoming);
   player.health=Mathf.Max(0,player.health-amount);
   return amount;
  }
  public static void Heal(PlayerState player,int amount) {
   if(player==null)return;
   player.health=Mathf.Clamp(player.health+Mathf.Max(0,amount),0,player.maxHealth);
  }
 }
}
