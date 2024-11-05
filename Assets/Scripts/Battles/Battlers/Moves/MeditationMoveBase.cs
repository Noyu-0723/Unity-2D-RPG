using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MeditationMoveBase : MoveBase
{
    [SerializeField] int magicPoint;
    public global::System.Int32 MagicPoint { get => magicPoint; }

    public override string RunMoveResult(BattleUnit sourceUnit, BattleUnit targetUnit){
        int value = sourceUnit.Battler.Meditation(magicPoint, 0);
        if(value != -1){
            return $"{sourceUnit.Battler.Base.Name}は「{Name}」を使った\nMPが{value}回復した";
        }else{
            return $"{sourceUnit.Battler.Base.Name}は「{Name}」を使った\nMPが全回復した";
        }
    }
}
