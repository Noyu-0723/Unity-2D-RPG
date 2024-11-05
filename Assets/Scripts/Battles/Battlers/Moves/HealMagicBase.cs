using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class HealMagicBase : MoveBase
{
    [SerializeField] int healPoint;
    public global::System.Int32 HealPoint { get => healPoint; }

    public override string RunMoveResult(BattleUnit sourceUnit, BattleUnit targetUnit){
        int value = sourceUnit.Battler.Heal(healPoint, 0);
        if(value != -1){
            return $"{sourceUnit.Battler.Base.Name}は「{Name}」を使った\n{sourceUnit.Battler.Base.Name}は{value}回復した";
        }else{
            return $"{sourceUnit.Battler.Base.Name}は「{Name}」を使った\n{sourceUnit.Battler.Base.Name}は全回復した";
        }
    }

    public override string RunMoveResultInMenu(BattleUnit player){
        int value = player.Battler.Heal(healPoint, 0);
        if(value != -1){
            return $"{player.Battler.Base.Name}は「{Name}」を使った\n{player.Battler.Base.Name}は{value}回復した";
        }else{
            return $"{player.Battler.Base.Name}は「{Name}」を使った\n{player.Battler.Base.Name}は全回復した";
        }
    }
}
