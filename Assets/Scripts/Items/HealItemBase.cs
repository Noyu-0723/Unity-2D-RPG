using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class HealItemBase : ItemBase
{
    [SerializeField] int healPoint;
    public global::System.Int32 HealPoint { get => healPoint; }

    public override string UseItemResult(BattleUnit sourceUnit, BattleUnit targetUnit){
        sourceUnit.Battler.Heal(healPoint, 0);
        return $"{sourceUnit.Battler.Base.Name}は「{Name}」を使った\n{sourceUnit.Battler.Base.Name}は{healPoint}回復した";
    }
}
