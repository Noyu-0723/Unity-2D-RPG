using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BattlerBase : ScriptableObject
{
    // Battlerの基礎データ
    [SerializeField] new string name;
    [SerializeField] int maxHP;
    [SerializeField] int maxMP;
    [SerializeField] int attack;
    [SerializeField] int defence;
    [SerializeField] int speed;
    [SerializeField] Sprite sprite;
    [SerializeField] List<LearnableMove> learnableAttacks;
    [SerializeField] List<LearnableMove> learnableMagics;
    [SerializeField] List<GetableItem> getableItems;
    [SerializeField] int exp;

    public global::System.String Name { get => name; }
    public global::System.Int32 MaxHP { get => maxHP; }
    public global::System.Int32 MaxMP { get => maxMP; }
    public global::System.Int32 Attack { get => attack; }
    public global::System.Int32 Defence { get => defence; }
    public global::System.Int32 Speed { get => speed; }
    public Sprite Sprite { get => sprite; }
    public List<LearnableMove> LearnableAttacks { get => learnableAttacks; }
    public List<LearnableMove> LearnableMagics { get => learnableMagics; }
    public List<GetableItem> GetableItems { get => getableItems; }
    public global::System.Int32 Exp { get => exp; }

    public void Init(){
        // 初期化処理
        name = InputNameSystem.playerNameOrigin;
    }
}