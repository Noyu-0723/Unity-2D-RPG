using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AttackMagicDoubleBase : MoveBase
{
    [SerializeField] int power;
    public global::System.Int32 Power { get => power; }

    public override string RunMoveResult(BattleUnit sourceUnit, BattleUnit targetUnit){
        int damage1 = targetUnit.Battler.TakeDamage(power, sourceUnit.Battler, Mathf.Max(1, power / 5), magic: true, UseMP);
        int damage2 = targetUnit.Battler.TakeDamage(power, sourceUnit.Battler, Mathf.Max(1, power / 5), magic: true, UseMP);
        return $"{sourceUnit.Battler.Base.Name}の「{Name}」\n{targetUnit.Battler.Base.Name}に{damage1}のダメージ\n{targetUnit.Battler.Base.Name}に{damage2}のダメージ";
    }
}