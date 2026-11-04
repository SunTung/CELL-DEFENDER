using UnityEngine;
namespace CellDefender {
 public sealed class RuntimeBootstrap:MonoBehaviour {
  public PlayerRuntime player;
  public GameObject enemyTemplate,enemyBulletTemplate,playerBulletTemplate;
  float timer;int wave;
  void Start(){
   player.shotPrefab=playerBulletTemplate;
  }
  void Update(){
   if(GameSession.Instance==null||GameSession.Instance.phase!=GamePhase.Playing)return;
   timer-=Time.deltaTime;if(timer>0)return;
   timer=Mathf.Max(.4f,1.5f-Time.timeSinceLevelLoad*.002f);
   if(enemyTemplate==null)return;
   var go=Instantiate(enemyTemplate,new Vector3(Random.Range(-4f,4f),8.5f,0),Quaternion.identity);
   go.SetActive(true);
   var e=go.GetComponent<EnemyRuntime>();
   e.player=player.transform;e.projectileTemplate=enemyBulletTemplate;
   e.Configure((EnemyKind)(wave++%7));
  }
 }
}
