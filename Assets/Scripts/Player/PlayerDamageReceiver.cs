using UnityEngine;
namespace CellDefender {
 public sealed class PlayerDamageReceiver:MonoBehaviour {
  public float invulnerabilitySeconds=0.6f;
  float nextHit;
  public void Hit(int amount){
   if(Time.time<nextHit || GameSession.Instance==null)return;
   nextHit=Time.time+invulnerabilitySeconds;
   GameSession.Instance.TakeDamage(amount);
  }
 }
}
