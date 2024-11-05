using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AttackMoveBase : MoveBase
{
    [SerializeField] int power;
    public global::System.Int32 Power { get => power; }

    public override string RunMoveResult(BattleUnit sourceUnit, BattleUnit targetUnit){
        int damage = targetUnit.Battler.TakeDamage(power, sourceUnit.Battler, Mathf.Max(1, power / 5));
        return $"{sourceUnit.Battler.Base.Name}の「{Name}」\n{targetUnit.Battler.Base.Name}に{damage}のダメージ";
    }
}
