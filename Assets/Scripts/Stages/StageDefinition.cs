using System;
using UnityEngine;
namespace CellDefender {
 [Serializable] public class SpawnWave {
  public float startSeconds;
  public EnemyDefinition enemy;
  [Min(1)] public int count=1;
  [Min(0.1f)] public float interval=1f;
 }
 [CreateAssetMenu(menuName="CELL/Stage Definition")]
 public class StageDefinition:ScriptableObject {
  public int stageNumber=1;
  public string stageName="Bloodstream";
  public SpawnWave[] waves;
  public EnemyDefinition miniBoss;
  public EnemyDefinition boss;
  public bool unlockEndlessAfterCompletion;
 }
}
