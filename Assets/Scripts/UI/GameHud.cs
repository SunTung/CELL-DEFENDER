using UnityEngine;
namespace CellDefender {
 public sealed class GameHud:MonoBehaviour {
  GUIStyle heading,button;
  void Setup(){
   if(heading!=null)return;
   heading=new GUIStyle(GUI.skin.label){fontSize=24,fontStyle=FontStyle.Bold,alignment=TextAnchor.MiddleCenter};
   heading.normal.textColor=Color.white;
   button=new GUIStyle(GUI.skin.button){fontSize=22,alignment=TextAnchor.MiddleCenter};
  }
  void OnGUI(){
   var s=GameSession.Instance;if(s==null)return;Setup();
   float scale=Mathf.Min(Screen.width/540f,Screen.height/960f);
   var old=GUI.matrix;GUI.matrix=Matrix4x4.Scale(new Vector3(scale,scale,1));
   float w=Screen.width/scale,h=Screen.height/scale;
   GUI.Box(new Rect(8,8,w-16,90),"");
   GUI.Label(new Rect(16,12,w-32,32),"HP "+s.player.health+"/"+s.player.maxHealth+"    SHIELD "+s.player.shieldCharges+"    STAGE "+s.currentStage,heading);
   GUI.Label(new Rect(16,46,w-32,30),"LEVEL "+s.player.experienceLevel+"    ENERGY "+s.player.energy+"/"+s.player.experienceRequired,heading);
   if(s.phase==GamePhase.SkillChoice){
    float top=h*.34f;GUI.Box(new Rect(25,top,w-50,320),"");
    GUI.Label(new Rect(40,top+15,w-80,50),"CHOOSE SKILL",heading);
    for(int i=0;i<s.choices.Length;i++){
     var skill=s.choices[i];
     if(GUI.Button(new Rect(50,top+70+i*76,w-100,66),skill+"  Lv."+(s.player.skillLevels[(int)skill]+1),button))s.SelectSkill(skill);
    }
   }else if(s.phase==GamePhase.GameOver||s.phase==GamePhase.Victory){
    GUI.Box(new Rect(25,h*.35f,w-50,220),"");
    GUI.Label(new Rect(35,h*.35f+30,w-70,55),s.phase==GamePhase.Victory?"VICTORY":"GAME OVER",heading);
    if(GUI.Button(new Rect(65,h*.35f+120,w-130,65),"RESTART",button))s.StartRun();
   }else if(s.phase==GamePhase.Playing){
    if(GUI.Button(new Rect(w-100,105,85,50),"PAUSE"))s.Pause();
   }else if(s.phase==GamePhase.Paused){
    if(GUI.Button(new Rect(w*.25f,h*.45f,w*.5f,70),"RESUME",button))s.Resume();
   }
   GUI.matrix=old;
  }
 }
}
