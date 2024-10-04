using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class KeyItemBase : ItemBase
{
    public override string UseItemResult(BattleUnit sourceUnit, BattleUnit targetUnit){
        return $"{sourceUnit.Battler.Base.Name}は「{Name}」を使った\nしかし何も起こらなかった…";
    }
}