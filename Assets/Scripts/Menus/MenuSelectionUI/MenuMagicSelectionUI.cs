using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMagicSelectionUI : MonoBehaviour
{
    // 使える技の数だけコマンド(テキスト)を生成 => Prefabを生成
    [SerializeField] RectTransform movesParent;
    [SerializeField] SelectableText moveTextPrefab;
    List<SelectableText> selectableTexts = new List<SelectableText>();

    int selectedIndex;
    public global::System.Int32 SelectedIndex { get => selectedIndex; } // 他のスクリプトからも参照できるように

    public void Init(List<Move> moves){
        // 自分の子要素で<SelectableText>コンポーネントを持っているものを集める
        // selectableTexts = GetComponentsInChildren<SelectableText>();
        selectedIndex = 0;
        SetMovesUISize(moves);
    }

    void SetMovesUISize(List<Move> moves){
        Vector2 uiSize = movesParent.sizeDelta;
        uiSize.y = 33 + 30 * moves.Count;
        movesParent.sizeDelta = uiSize;

        for(int i = 0; i < moves.Count; i++){
            SelectableText moveText = Instantiate(moveTextPrefab, movesParent);
            if(moves[i].Base.UseMP == 0){
                moveText.SetText(moves[i].Base.Name);
            }else{
                moveText.SetText($"{moves[i].Base.Name}({moves[i].Base.UseMP})");
            }
            selectableTexts.Add(moveText);
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

    public void DeleteMoveTexts(){
        foreach(var text in selectableTexts){
            Destroy(text.gameObject);
        }
        selectableTexts.Clear();
    }
}
