using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Battler{
    [SerializeField] BattlerBase _base;
    [SerializeField] int level;
    [SerializeField] int hasExp;
    [SerializeField] List<ItemBase> inventory = new List<ItemBase>();
    public BattlerBase Base { get => _base; }
    public global::System.Int32 Level { get => level; set => level = value; }
    public global::System.Int32 HasExp { get => hasExp; set => hasExp = value; }
    public List<ItemBase> Inventory { get => inventory; set => inventory = value; }

    // ステータス
    public int MaxHP {get; set;}
    public int HP {get; set;}
    public int MaxMP {get; set;}
    public int MP {get; set;}
    public int Attack {get; set;}
    public int Defence {get; set;}
    public int Speed {get; set;}
    public int DebuffTurn {get; set;}
    public int BuffTurn {get; set;}
    public int NoMoveTurn {get; set;}
    public List<Move> Attacks {get; set;}
    public List<Move> Magics {get; set;}
    public List<Item> Items {get; set;}

    // 初期化(ゲーム開始時に一度だけ実行)
    public void Init(){
        // 覚える技から習得可能な技を生成
        Attacks = new List<Move>();
        foreach(var learnableAttack in Base.LearnableAttacks){
            if(learnableAttack.Level <= level){
                Attacks.Add(new Move(learnableAttack.MoveBase));
            }
        }
        Magics = new List<Move>();
        foreach(var learnableMagic in Base.LearnableMagics){
            if(learnableMagic.Level <= level){
                Magics.Add(new Move(learnableMagic.MoveBase));
            }
        }
        // 持っているアイテムから生成
        Items = new List<Item>();
        foreach(var item in inventory){
            Items.Add(new Item(item));
        }
        UpdateStatus();
    }

    public void UpdateStatus(){
        MaxHP = _base.MaxHP + _base.MaxHP * level / 10;
        HP = MaxHP;
        MaxMP = _base.MaxMP + _base.MaxMP * level / 10;
        MP = MaxMP;
        Attack = _base.Attack + _base.Attack * level / 10;
        Defence = _base.Defence + _base.Defence * level / 10;
        Speed = _base.Speed + _base.Speed * level / 10;
    }
    
    public int TakeDamage(int attackPower, Battler attacker){
        int damage = Mathf.Max(1, (attacker.Attack + attackPower) - Defence); 
        HP = Mathf.Clamp(HP - damage, 0, MaxHP);
        return damage;
    }

    public void Heal(int healPoint, int useMP){
        HP = Mathf.Clamp(HP + healPoint, 0, MaxHP);
        MP = Mathf.Clamp(MP - useMP, 0, MaxMP);
    }

    public Move GetRandomAttack(){
        int r = Random.Range(0, Attacks.Count);
        return Attacks[r];
    }

    public string GetRandomItem(BattleUnit targetUnit){
        int r = Random.Range(1, 101);
        foreach(var getItem in targetUnit.Battler.Base.GetableItems){
            if(r <= getItem.Probability){
                Item item = new Item(getItem.ItemBase);
                Items.Add(item);
                return $"なんと{targetUnit.Battler.Base.Name}は「{getItem.ItemBase.Name}」を落とした";
            }
        }
        return null;
    }

    public bool IsLevelUp(){
        if(HasExp >= level * 10 + (int)Mathf.Pow(level, 1.2f)){
            level++;
            return true;
        }
        return false;
    }

    // 新しくなんの技を覚えるのか
    public Move LearnedMove(){
        foreach(var learnableAttack in Base.LearnableAttacks){
            // まだ覚えていないかつ習得可能な技があれば登録する
            if(learnableAttack.Level <= level && !Attacks.Exists(move => move.Base == learnableAttack.MoveBase)){
                Move move = new Move(learnableAttack.MoveBase);
                Attacks.Add(move);
                return move;
            }
        }
        foreach(var learnableMagic in Base.LearnableMagics){
            // まだ覚えていないかつ習得可能な技があれば登録する
            if(learnableMagic.Level <= level && !Magics.Exists(move => move.Base == learnableMagic.MoveBase)){
                Move move = new Move(learnableMagic.MoveBase);
                Magics.Add(move);
                return move;
            }
        }
        return null;
    }
}