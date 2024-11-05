using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EmemyDefenceDownItem : ItemBase
{
    [SerializeField] float value;

    public float Value { get => value; }

    public override string UseItemResult(BattleUnit sourceUnit, BattleUnit targetUnit){
        targetUnit.Battler.Defence = (int)(targetUnit.Battler.Defence * value);
        return $"{sourceUnit.Battler.Base.Name}は「{Name}」を使った\n{targetUnit.Battler.Base.Name}の防御力が下がった";
    }
    public override string UseItemResultInMenu(BattleUnit player){
        return $"{player.Battler.Base.Name}は「{Name}」を使った\nしかし何も起こらなかった…";
    }
}
