using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StrengthMenuStatusUI : BattleUnit
{
    [SerializeField] Text mapText;
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] Text hpText;
    [SerializeField] Text mpText;
    [SerializeField] Text atText;
    [SerializeField] Text dfText;
    [SerializeField] Text spText;
    private Battler battler;
    public override void Setup(Battler player){
        this.battler = player;
        base.Setup(battler);
        mapText.text = $"フロア : {battler.map}";
        nameText.text = battler.Base.Name;
        levelText.text = $"Lv : {battler.Level}";
        hpText.text = $"HP : {battler.HP} / {battler.MaxHP}";
        mpText.text = $"MP : {battler.MP} / {battler.MaxMP}";
        atText.text = $"AT : {battler.Attack}";
        dfText.text = $"DF : {battler.Defence}";
        spText.text = $"SP : {battler.Speed}";
    }

    public override void UpdateUI(){
        levelText.text = $"Lv : {battler.Level}";
        hpText.text = $"HP : {battler.HP} / {battler.MaxHP}";
        mpText.text = $"MP : {battler.MP} / {battler.MaxMP}";
        atText.text = $"AT : {battler.Attack}";
        dfText.text = $"DF : {battler.Defence}";
        spText.text = $"SP : {battler.Speed}";
    }

    public void Open(){
        UpdateUI();
        gameObject.SetActive(true);
    }

    public void Close(){
        gameObject.SetActive(false);
    }
}
