using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 技の基礎データ
[CreateAssetMenu]
public class MoveBase : ScriptableObject
{
    [SerializeField] new string name;
    [SerializeField] int useMP;
    [SerializeField] string message;
    public global::System.String Name { get => name; }
    public global::System.Int32 UseMP { get => useMP; }
    public global::System.String Message { get => message; }

    public enum Types{
        BattleOnly,
        UseableInMenu,
    }
    [SerializeField] Types type;
    public Types Type { get => type; set => type = value; }

    public virtual string RunMoveResult(BattleUnit sourceUnit, BattleUnit targetUnit){
        return "";
    }

    public virtual string RunMoveResultInMenu(BattleUnit player){
        return "";
    }
}
