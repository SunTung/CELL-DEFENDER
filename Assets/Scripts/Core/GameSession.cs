using UnityEngine;
namespace CellDefender {
 public class GameSession:MonoBehaviour {
  public static GameSession Instance {get;private set;}
  public PlayerState player=new PlayerState();
  public GamePhase phase=GamePhase.Menu;
  public int currentStage=1;
  public bool endlessUnlocked;
  public SkillKind[] choices=new SkillKind[0];
  void Awake(){
   if(Instance!=null&&Instance!=this){Destroy(gameObject);return;}
   Instance=this;DontDestroyOnLoad(gameObject);
  }
  public void StartRun(){player=new PlayerState();currentStage=1;phase=GamePhase.Playing;choices=new SkillKind[0];Time.timeScale=1f;}
  public void Pause(){if(phase==GamePhase.Playing){phase=GamePhase.Paused;Time.timeScale=0f;}}
  public void Resume(){if(phase==GamePhase.Paused){phase=GamePhase.Playing;Time.timeScale=1f;}}
  void OfferSkills(){
   var available=new System.Collections.Generic.List<SkillKind>();
   for(int i=0;i<4;i++)if(player.skillLevels[i]<6)available.Add((SkillKind)i);
   for(int i=available.Count-1;i>0;i--){int j=Random.Range(0,i+1);var temp=available[i];available[i]=available[j];available[j]=temp;}
   choices=available.GetRange(0,Mathf.Min(3,available.Count)).ToArray();
   if(choices.Length>0){phase=GamePhase.SkillChoice;Time.timeScale=0f;}
  }
  public void Collect(PickupKind kind){
   if(phase!=GamePhase.Playing)return;
   if(PickupSystem.Collect(player,kind))OfferSkills();
  }
  public void CollectEnergy(int amount){
   if(phase!=GamePhase.Playing)return;
   if(ProgressionSystem.AddEnergy(player,amount))OfferSkills();
  }
  public void SelectSkill(SkillKind kind){
   if(phase!=GamePhase.SkillChoice)return;
   if(System.Array.IndexOf(choices,kind)<0)return;
   if(ProgressionSystem.UpgradeSkill(player,kind)){choices=new SkillKind[0];phase=GamePhase.Playing;Time.timeScale=1f;}
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
