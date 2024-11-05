using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AttackItemBase : ItemBase
{
    [SerializeField] int power;
    public global::System.Int32 Power { get => power; }

    public override string UseItemResult(BattleUnit sourceUnit, BattleUnit targetUnit){
        int damage =  targetUnit.Battler.TakeDamage(power, sourceUnit.Battler, Mathf.Max(1, power / 5), magic: true);
        return $"{sourceUnit.Battler.Base.Name}は「{Name}」を使った\n{targetUnit.Battler.Base.Name}に{damage}のダメージ";
    }
    public override string UseItemResultInMenu(BattleUnit player){
        return $"{player.Battler.Base.Name}は「{Name}」を使った\nしかし何も起こらなかった…";
    }
}
