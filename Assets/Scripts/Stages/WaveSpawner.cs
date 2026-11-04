using UnityEngine;
namespace CellDefender {
 public class WaveSpawner:MonoBehaviour {
  public StageDefinition stage;
  public GameObject enemyPrefab;
  public Transform player;
  public float spawnHalfWidth=4f;
  float clock;
  int[] spawned;
  float[] nextSpawn;
  void Start(){if(stage!=null&&stage.waves!=null){spawned=new int[stage.waves.Length];nextSpawn=new float[stage.waves.Length];}}
  void Update(){
   if(stage==null||stage.waves==null||enemyPrefab==null)return;
   if(GameSession.Instance!=null&&GameSession.Instance.phase!=GamePhase.Playing)return;
   clock+=Time.deltaTime;
   for(int i=0;i<stage.waves.Length;i++){
    var wave=stage.waves[i];
    if(wave.enemy==null||clock<wave.startSeconds||spawned[i]>=wave.count||clock<nextSpawn[i])continue;
    var go=Instantiate(enemyPrefab,new Vector3(Random.Range(-spawnHalfWidth,spawnHalfWidth),9f,0),Quaternion.identity);
    var e=go.GetComponent<EnemyRuntime>();
    if(e!=null){e.definition=wave.enemy;e.player=player;}
    spawned[i]++;nextSpawn[i]=clock+wave.interval;
   }
  }
 }
}
