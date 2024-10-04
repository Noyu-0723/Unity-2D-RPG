using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 技の基礎データ
[CreateAssetMenu]
public class MoveBase : ScriptableObject
{
    [SerializeField] new string name;
    [SerializeField] int useMP;
    public global::System.String Name { get => name; }
    public global::System.Int32 UseMP { get => useMP; }

    public virtual string RunMoveResult(BattleUnit sourceUnit, BattleUnit targetUnit){
        return "";
    }
}