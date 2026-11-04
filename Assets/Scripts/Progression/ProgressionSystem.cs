using UnityEngine;
namespace CellDefender {
 public static class ProgressionSystem {
  public static bool AddEnergy(PlayerState p,int amount){
   if(p==null || amount<=0)return false;
   p.energy+=amount;
   if(p.energy<p.experienceRequired)return false;
   p.energy-=p.experienceRequired;
   p.experienceLevel++;
   return true;
  }
  public static bool UpgradeSkill(PlayerState p,SkillKind kind){
   int i=(int)kind;
   if(p==null||p.skillLevels[i]>=6)return false;
   p.skillLevels[i]++;
   return true;
  }
  public static void ApplyTrauma(PlayerState p){
   if(p==null)return;
   p.experienceLevel=Mathf.Max(1,p.experienceLevel-1);
   p.energy=0;
   // Skill-loss ordering is reserved for a separate acquisition ledger.
  }
 }
}
