using System;
using UnityEngine;
namespace CellDefender {
 [CreateAssetMenu(menuName="CELL/Attack Definition")]
 public class AttackDefinition:ScriptableObject {
  public AttackKind kind;
  [Min(0)] public int damage=10;
  [Min(0.1f)] public float speed=5f;
  [Min(0.1f)] public float interval=1f;
  [Min(0)] public float spreadAngle=7f;
  [Min(0.1f)] public float beamLength=3f;
  [Min(1)] public int projectileCount=1;
 }
}
