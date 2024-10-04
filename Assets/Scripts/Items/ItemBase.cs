using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 技の基礎データ
[CreateAssetMenu]
public class ItemBase : ScriptableObject{
    [SerializeField] new string name;
    [SerializeField] int type;

    /*
    type: 0 -> BattleItem
    type: 1 -> KeyItem
    type: 2 ->
    */

    public global::System.String Name { get => name; }
    public global::System.Int32 Type { get => type; }

    public virtual string UseItemResult(BattleUnit sourceUnit, BattleUnit targetUnit){
        return "";
    }
}