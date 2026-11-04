using UnityEngine;
namespace CellDefender {
 public class GameSession:MonoBehaviour {
  public static GameSession Instance {get;private set;}
  public PlayerState player=new PlayerState();
  public GamePhase phase=GamePhase.Menu;
  public int currentStage=1;
  public bool endlessUnlocked;
  void Awake(){
   if(Instance!=null&&Instance!=this){Destroy(gameObject);return;}
   Instance=this;
   DontDestroyOnLoad(gameObject);
  }
  public void StartRun(){player=new PlayerState();currentStage=1;phase=GamePhase.Playing;Time.timeScale=1f;}
  public void Pause(){if(phase==GamePhase.Playing){phase=GamePhase.Paused;Time.timeScale=0f;}}
  public void Resume(){if(phase==GamePhase.Paused){phase=GamePhase.Playing;Time.timeScale=1f;}}
  public void Collect(PickupKind kind){
   if(phase!=GamePhase.Playing)return;
   if(PickupSystem.Collect(player,kind)){phase=GamePhase.SkillChoice;Time.timeScale=0f;}
  }
  public void SelectSkill(SkillKind kind){
   if(phase!=GamePhase.SkillChoice)return;
   if(ProgressionSystem.UpgradeSkill(player,kind)){phase=GamePhase.Playing;Time.timeScale=1f;}
  }
  public void CollectEnergy(int amount){
   if(phase!=GamePhase.Playing)return;
   if(ProgressionSystem.AddEnergy(player,amount)){
    // Temporary auto-upgrade until the three-choice UI is implemented.
    for(int i=0;i<4;i++)if(ProgressionSystem.UpgradeSkill(player,(SkillKind)i))break;
   }
  }
  public void TakeDamage(int damage){
   if(phase!=GamePhase.Playing)return;
   DamageSystem.Apply(player,damage);
   if(!player.IsAlive){ProgressionSystem.ApplyTrauma(player);phase=GamePhase.GameOver;Time.timeScale=0f;}
  }
  public void CompleteStage(){
   if(currentStage>=6){endlessUnlocked=true;phase=GamePhase.Victory;Time.timeScale=0f;}
   else currentStage++;
  }
 }
}
