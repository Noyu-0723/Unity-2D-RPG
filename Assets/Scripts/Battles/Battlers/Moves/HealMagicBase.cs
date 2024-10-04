using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class HealMagicBase : MoveBase
{
    [SerializeField] int healPoint;
    public global::System.Int32 HealPoint { get => healPoint; }

    public override string RunMoveResult(BattleUnit sourceUnit, BattleUnit targetUnit){
        sourceUnit.Battler.Heal(healPoint, UseMP);
        return $"{sourceUnit.Battler.Base.Name}の「{Name}」\n{sourceUnit.Battler.Base.Name}は{healPoint}回復した";
    }
}
