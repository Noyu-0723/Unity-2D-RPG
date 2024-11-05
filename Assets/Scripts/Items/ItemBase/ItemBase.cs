using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 技の基礎データ
[CreateAssetMenu]
public class ItemBase : ScriptableObject{
    [SerializeField] new string name;
    public enum Types {
        Attack,
        Heal,
        Debuff,
        Buff,
        Key,
        MoveScene,
    }
    [SerializeField] Types type;
    [SerializeField] bool isSave;
    [SerializeField] string message;

    public global::System.String Name { get => name; }
    public Types Type { get => type; set => type = value; }
    public global::System.Boolean IsSave { get => isSave; }
    public global::System.String Message { get => message; }

    public virtual string UseItemResult(BattleUnit sourceUnit, BattleUnit targetUnit){
        return "";
    }

    public virtual string UseItemResultInMenu(BattleUnit player){
        return "";
    }

    public virtual bool UseItemResultMoveScene(){
        return false;
    }
}