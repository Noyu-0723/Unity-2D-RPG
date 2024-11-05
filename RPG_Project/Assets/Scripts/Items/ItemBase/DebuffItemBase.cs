using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DebuffItemBase : ItemBase
{
    [SerializeField] int value;

    public global::System.Int32 Value { get => value; }

    public override string UseItemResult(BattleUnit sourceUnit, BattleUnit targetUnit){
        targetUnit.Battler.NoMoveTurn += value;
        return $"{sourceUnit.Battler.Base.Name}は「{Name}」を使った\n{targetUnit.Battler.Base.Name}は動きづらそうにしている";
    }
    public override string UseItemResultInMenu(BattleUnit player){
        return $"{player.Battler.Base.Name}は「{Name}」を使った\nしかし何も起こらなかった…";
    }
}
