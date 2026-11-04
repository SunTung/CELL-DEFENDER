using System;
using UnityEngine;
namespace CellDefender {
 public enum EnemyKind { Strafe, Armored, Aggressive, Escape, Stationary, Mother, Boss }
 public enum AttackKind { Homing, Ram, Straight, Spread, LongBeam, MediumBeam }
 public enum PickupKind { Health, Shield, Energy }
 public enum SkillKind { Bullet, Beam, Defense, Clone }
 public enum GamePhase { Menu, Playing, SkillChoice, Paused, GameOver, Victory }
 [Serializable] public class PlayerState {
  public int health=100,maxHealth=100,shieldCharges=0,energy=0,experienceLevel=1,evolutionStage=1;
  public int[] skillLevels=new int[4];
  public int experienceRequired {get {return experienceLevel*50;}}
  public bool IsAlive {get{return health>0;}}
 }
}
