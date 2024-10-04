using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUnit : BattleUnit
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] Text hpText;
    [SerializeField] Text mpText;
    [SerializeField] Text atText;
    public override void Setup(Battler battler){
        base.Setup(battler);
        nameText.text = battler.Base.Name;
        levelText.text = $"Lv : {battler.Level}";
        hpText.text = $"HP : {battler.HP} / {battler.MaxHP}";
        mpText.text = $"MP : {battler.MP} / {battler.MaxMP}";
        atText.text = $"AT : {battler.Attack}";
    }

    public override void UpdateUI(){
        levelText.text = $"Lv : {Battler.Level}";
        hpText.text = $"HP : {Battler.HP} / {Battler.MaxHP}";
        mpText.text = $"MP : {Battler.MP} / {Battler.MaxMP}";
        atText.text = $"AT : {Battler.Attack}";
    }
}
