using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class NameListUI : MonoBehaviour{

    [SerializeField] GameObject parentPanel;

    public SelectableText[] selectableTexts;
    public int selectedIndex = 0;
    public int parentSelectedIndex = 0;

    public void Init(){
        // 自分の子要素で<SelectableText>コンポーネントを持っているものを集める
        selectableTexts = GetComponentsInChildren<SelectableText>();
        SetGetSelectedIndexFunction();
    }

    private void SetGetSelectedIndexFunction(){
        foreach(SelectableText SelectableText in selectableTexts){
            SelectableText.OnSelectAction = GetSelectedIndex;
        }
        SetCurrentString(selectedIndex);
    }

    public void SetCurrentString(int index){
        // indexの番号に入っている文字を選択状態にする
        EventSystem.current.SetSelectedGameObject(selectableTexts[index].gameObject);
    }

    // カーソルの移動をする:親の変更をする
    public void GetSelectedIndex(Transform parent){
        // 親から見て何番目の子要素かを取得
        Transform grandParent = parent?.parent;
        parentSelectedIndex = grandParent.GetSiblingIndex();
        selectedIndex = parent.GetSiblingIndex();
    }

    // 今選択している文字を返す
    public string GetCurrentString(){
        int currentID = parentSelectedIndex * 30 + selectedIndex; // 改善の余地あり
        return selectableTexts[currentID].GetText();
    }

    public void Open(){
        gameObject.SetActive(true);
    }
    public void Close(){
        gameObject.SetActive(false);
    }
}
