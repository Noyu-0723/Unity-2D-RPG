using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MeditationItemBase : ItemBase
{
    [SerializeField] int magicPoint;
    public global::System.Int32 MagicPoint { get => magicPoint; }

    public override string UseItemResult(BattleUnit sourceUnit, BattleUnit targetUnit){
        int value = sourceUnit.Battler.Meditation(magicPoint, 0);
        if(value != -1){
            return $"{sourceUnit.Battler.Base.Name}は「{Name}」を使った\nMPが{value}回復した";
        }else{
            return $"{sourceUnit.Battler.Base.Name}は「{Name}」を使った\nMPが全回復した";
        }
    }
    public override string UseItemResultInMenu(BattleUnit player){
        int value = player.Battler.Meditation(magicPoint, 0);
        if(value != -1){
            return $"{player.Battler.Base.Name}は「{Name}」を使った\nMPが{value}回復した";
        }else{
            return $"{player.Battler.Base.Name}は「{Name}」を使った\nMPが全回復した";
        }
    }
}
