using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckLoadUI : MonoBehaviour
{
    SelectableText[] selectableTexts;
    int selectedIndex;
    public global::System.Int32 SelectedIndex { get => selectedIndex; } // 他のスクリプトからも参照できるように

    public void Init(){
        // 自分の子要素で<SelectableText>コンポーネントを持っているものを集める
        selectedIndex = 0;
        selectableTexts = GetComponentsInChildren<SelectableText>();
    }

    public void HandleUpdate(){
        if(Input.GetKeyDown(KeyCode.S)){
            selectedIndex++;
        }else if(Input.GetKeyDown(KeyCode.W)){
            selectedIndex--;
        }

        selectedIndex = Mathf.Clamp(selectedIndex, 0, selectableTexts.Length - 1);

        for(int i = 0; i < selectableTexts.Length; i++){
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
}

