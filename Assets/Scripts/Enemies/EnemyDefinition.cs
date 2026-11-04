using UnityEngine;
namespace CellDefender {
 [CreateAssetMenu(menuName="CELL/Enemy Definition")]
 public class EnemyDefinition:ScriptableObject {
  public EnemyKind kind;
  [Min(1)] public int maxHealth=20;
  [Min(0)] public int contactDamage=10;
  [Min(0)] public int energyDrop=10;
  [Min(0)] public float moveSpeed=2f;
  public AttackDefinition primaryAttack;
  public AttackDefinition secondaryAttack;
  [Min(0)] public int spawnLimit=4;
 }
}
