using UnityEngine;
namespace CellDefender {
 public sealed class EnemyRuntime:MonoBehaviour {
  public EnemyDefinition definition;
  public Transform player;
  public float hitPoints=20f;
  Vector2 origin;
  float age,attackTimer,spawnTimer;
  public System.Action<EnemyRuntime> SpawnRequested;
  void Start(){origin=transform.position;if(definition!=null)hitPoints=definition.maxHealth;}
  void Update(){
   if(definition==null)return;
   float dt=Time.deltaTime;age+=dt;attackTimer+=dt;spawnTimer+=dt;
   Vector2 p=transform.position;
   Vector2 toward=player==null?Vector2.down:((Vector2)player.position-p).normalized;
   switch(definition.kind){
    case EnemyKind.Strafe: p.x=origin.x+Mathf.Sin(age*2f)*1.5f; p.y-=definition.moveSpeed*dt;break;
    case EnemyKind.Armored:p+=Vector2.down*definition.moveSpeed*0.45f*dt;break;
    case EnemyKind.Aggressive:p+=toward*definition.moveSpeed*1.6f*dt;break;
    case EnemyKind.Escape:p-=toward*definition.moveSpeed*dt;break;
    case EnemyKind.Stationary:break;
    case EnemyKind.Mother:p+=Vector2.down*definition.moveSpeed*0.3f*dt;break;
    case EnemyKind.Boss:p.x=origin.x+Mathf.Sin(age)*2f;break;
   }
   transform.position=p;
   if(definition.kind==EnemyKind.Mother && spawnTimer>=3f){
    spawnTimer=0; if(SpawnRequested!=null)SpawnRequested(this);
   }
  }
  public void ReceiveDamage(float amount){
   hitPoints-=Mathf.Max(0,amount);
   if(hitPoints<=0){Destroy(gameObject);}
  }
  void OnTriggerEnter2D(Collider2D other){
   if(other.CompareTag("Player")){
    var receiver=other.GetComponent<PlayerDamageReceiver>();
    if(receiver!=null)receiver.Hit(definition==null?10:definition.contactDamage);
   }
  }
 }
}
