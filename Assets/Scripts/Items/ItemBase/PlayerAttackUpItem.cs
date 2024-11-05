using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerAttackUpItem : ItemBase
{
    [SerializeField] float value;

    public float Value { get => value; }

    public override string UseItemResult(BattleUnit sourceUnit, BattleUnit targetUnit){
        sourceUnit.Battler.Attack = (int)(sourceUnit.Battler.Attack * value);
        return $"{sourceUnit.Battler.Base.Name}は「{Name}」を使った\n{sourceUnit.Battler.Base.Name}は攻撃力が上がった";
    }
    public override string UseItemResultInMenu(BattleUnit player){
        return $"{player.Battler.Base.Name}は「{Name}」を使った\nしかし何も起こらなかった…";
    }
}
