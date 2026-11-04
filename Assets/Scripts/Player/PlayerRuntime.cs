using UnityEngine;
namespace CellDefender {
 [RequireComponent(typeof(Collider2D))]
 public class PlayerRuntime:MonoBehaviour {
  public float speed=8f;
  public float shotInterval=0.22f;
  public GameObject shotPrefab;
  public float shotSpeed=13f;
  float cooldown;
  void Update(){
   if(GameSession.Instance!=null && GameSession.Instance.phase!=GamePhase.Playing)return;
   Vector2 input=new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
   Vector2 destination=(Vector2)transform.position+Vector2.ClampMagnitude(input,1)*speed*Time.deltaTime;
   if(Input.GetMouseButton(0)){
    Vector3 mouse=Camera.main.ScreenToWorldPoint(Input.mousePosition);
    destination=Vector2.MoveTowards(transform.position,new Vector2(mouse.x,mouse.y),speed*Time.deltaTime);
   }
   transform.position=new Vector3(Mathf.Clamp(destination.x,-4.2f,4.2f),Mathf.Clamp(destination.y,-7.5f,7.5f),0);
   cooldown-=Time.deltaTime;
   if(cooldown<=0 && shotPrefab!=null){
    cooldown=shotInterval;
    var go=Instantiate(shotPrefab,transform.position+Vector3.up*0.5f,Quaternion.identity);
    var projectile=go.GetComponent<ProjectileRuntime>();
    if(projectile!=null){projectile.hostile=false;projectile.velocity=Vector2.up*shotSpeed;}
   }
  }
  public void ReceiveHit(int amount){if(GameSession.Instance!=null)GameSession.Instance.TakeDamage(amount);}
 }
}
