using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSelectionUI : MonoBehaviour
{
    // 使えるアイテムの数だけコマンド(テキスト)を生成 => Prefabを生成
    [SerializeField] RectTransform itemsParent;
    [SerializeField] SelectableText itemTextPrefab;
    List<SelectableText> selectableTexts = new List<SelectableText>();

    int selectedIndex;
    public global::System.Int32 SelectedIndex { get => selectedIndex; } // 他のスクリプトからも参照できるように

    public void Init(List<Item> items){
        // 自分の子要素で<SelectableText>コンポーネントを持っているものを集める
        // selectableTexts = GetComponentsInChildren<SelectableText>();
        selectedIndex = 0;
        SetitemsUISize(items);
    }

    void SetitemsUISize(List<Item> items){
        Vector2 uiSize = itemsParent.sizeDelta;
        uiSize.y = 33 + 30 * items.Count;
        itemsParent.sizeDelta = uiSize;

        for(int i = 0; i < items.Count; i++){
            SelectableText itemText = Instantiate(itemTextPrefab, itemsParent);
            itemText.SetText(items[i].Base.Name);
            selectableTexts.Add(itemText);
        }
    }

    public void HandleUpdate(){
        if(Input.GetKeyDown(KeyCode.S)){
            selectedIndex++;
        }else if(Input.GetKeyDown(KeyCode.W)){
            selectedIndex--;
        }

        selectedIndex = Mathf.Clamp(selectedIndex, 0, selectableTexts.Count - 1);

        for(int i = 0; i < selectableTexts.Count; i++){
            if(selectedIndex == i){
                selectableTexts[i].SetColor(true);
            }else{
                selectableTexts[i].SetColor(false);
            }
        }
    }

    public void Open(){
        gameObject.SetActive(true);
    }
    public void Close(){
        selectedIndex = 0;
        gameObject.SetActive(false);
    }

    public void DeleteItemTexts(){
        foreach(var text in selectableTexts){
            Destroy(text.gameObject);
        }
        selectableTexts.Clear();
    }
}
