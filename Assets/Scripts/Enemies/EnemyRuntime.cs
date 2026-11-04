using UnityEngine;
namespace CellDefender {
 public sealed class EnemyRuntime:MonoBehaviour {
  public EnemyDefinition definition;
  public GameObject projectileTemplate;
  public Transform player;
  public float hitPoints=20f;
  public EnemyKind kind;
  Vector2 origin;float age,attackTimer,spawnTimer;
  public System.Action<EnemyRuntime> SpawnRequested;
  public void Configure(EnemyKind type){kind=type;hitPoints=type==EnemyKind.Boss?100:20;origin=transform.position;}
  void Start(){origin=transform.position;if(definition!=null){kind=definition.kind;hitPoints=definition.maxHealth;}}
  void Update(){
   if(GameSession.Instance!=null&&GameSession.Instance.phase!=GamePhase.Playing)return;
   float dt=Time.deltaTime;age+=dt;attackTimer+=dt;spawnTimer+=dt;
   Vector2 p=transform.position;
   Vector2 toward=player==null?Vector2.down:((Vector2)player.position-p).normalized;
   float speed=definition==null?2f:definition.moveSpeed;
   switch(kind){
    case EnemyKind.Strafe:p.x=origin.x+Mathf.Sin(age*2)*1.5f;p.y-=speed*dt;break;
    case EnemyKind.Armored:p+=Vector2.down*speed*.45f*dt;break;
    case EnemyKind.Aggressive:p+=toward*speed*1.6f*dt;break;
    case EnemyKind.Escape:p-=toward*speed*dt;break;
    case EnemyKind.Stationary:break;
    case EnemyKind.Mother:p+=Vector2.down*speed*.3f*dt;break;
    case EnemyKind.Boss:p.x=origin.x+Mathf.Sin(age)*2f;break;
   }
   transform.position=p;
   if(kind==EnemyKind.Mother&&spawnTimer>=4f){spawnTimer=0;if(SpawnRequested!=null)SpawnRequested(this);}
   float interval=definition!=null&&definition.primaryAttack!=null?definition.primaryAttack.interval:1.8f;
   if(attackTimer>=interval){attackTimer=0;Fire(toward);}
   if(p.y< -10||Mathf.Abs(p.x)>15)Destroy(gameObject);
  }
  void Fire(Vector2 forward){
   if(projectileTemplate==null||kind==EnemyKind.Aggressive)return;
   AttackKind attack=definition!=null&&definition.primaryAttack!=null?definition.primaryAttack.kind:
     kind==EnemyKind.Boss?AttackKind.Spread:kind==EnemyKind.Escape?AttackKind.Homing:AttackKind.Straight;
   int count=attack==AttackKind.Spread?5:1;
   var directions=AttackPattern.Directions(attack,forward,count,7f);
   foreach(var dir in directions){
    var go=Instantiate(projectileTemplate,transform.position,Quaternion.identity);go.SetActive(true);
    var bullet=go.GetComponent<ProjectileRuntime>();
    bullet.hostile=true;bullet.kind=attack;bullet.target=player;bullet.velocity=dir*5;bullet.lifetime=5f;
    if(attack==AttackKind.LongBeam||attack==AttackKind.MediumBeam){
     go.transform.localScale=new Vector3(.25f,attack==AttackKind.LongBeam?3f:1.5f,1);
     go.transform.up=dir;
    }
   }
  }
  public void ReceiveDamage(float amount){
   hitPoints-=Mathf.Max(0,amount);
   if(hitPoints<=0){if(GameSession.Instance!=null)GameSession.Instance.CollectEnergy(definition==null?10:definition.energyDrop);Destroy(gameObject);}
  }
  void OnTriggerEnter2D(Collider2D other){
   var receiver=other.GetComponent<PlayerDamageReceiver>();
   if(receiver!=null){receiver.Hit(definition==null?10:definition.contactDamage);Destroy(gameObject);}
  }
 }
}
