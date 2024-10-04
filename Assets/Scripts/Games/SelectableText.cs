using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SelectableText : Selectable{

    Text text;
    public UnityAction<Transform> OnSelectAction = null;

    protected override void Awake(){
        text = GetComponent<Text>();
    }

    public void SetText(string newName){
        if(text == null){
            text = GetComponent<Text>();
        }
        text.text = newName;
    }

    public string GetText(){
        return text.text;
    }

    // 選択中なら色を変える
    public void SetColor(bool selected){
        if(text == null){
            text = GetComponent<Text>();
        }
        if(selected){
            text.color = Color.yellow;
        }else{
            text.color = Color.white;
        }
    }

    // 選択状態になったときに勝手に実行される関数
    public override void OnSelect(BaseEventData eventData){
        if(text == null){
            text = GetComponent<Text>();
        }
        text.color = Color.yellow;
        OnSelectAction?.Invoke(transform);
    }
    // 非選択状態になったときに勝手に実行される関数
    public override void OnDeselect(BaseEventData eventData){
        if(text == null){
            text = GetComponent<Text>();
        }
        text.color = Color.white;
    }
}
