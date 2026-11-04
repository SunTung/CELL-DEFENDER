using UnityEngine;
namespace CellDefender {
 public sealed class ProjectileRuntime:MonoBehaviour {
  public Vector2 velocity;public float lifetime=5f;public int damage=10;
  public bool hostile=true;public AttackKind kind=AttackKind.Straight;
  public Transform target;public float homingTurnRate=180f;
  void Update(){
   if(GameSession.Instance!=null&&GameSession.Instance.phase!=GamePhase.Playing)return;
   float dt=Time.deltaTime;lifetime-=dt;
   if(lifetime<=0){Destroy(gameObject);return;}
   if(kind==AttackKind.Homing&&target!=null){
    Vector2 desired=((Vector2)target.position-(Vector2)transform.position).normalized*velocity.magnitude;
    velocity=Vector2.MoveTowards(velocity,desired,velocity.magnitude*homingTurnRate*Mathf.Deg2Rad*dt);
   }
   transform.position+=(Vector3)(velocity*dt);
  }
  void OnTriggerEnter2D(Collider2D other){
   if(hostile){
    var receiver=other.GetComponent<PlayerDamageReceiver>();
    if(receiver!=null){receiver.Hit(damage);Destroy(gameObject);}
   }else{
    var enemy=other.GetComponent<EnemyRuntime>();
    if(enemy!=null){enemy.ReceiveDamage(damage);Destroy(gameObject);}
   }
  }
 }
}
