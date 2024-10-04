using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenuView : MonoBehaviour{

    SelectableText[] selectableTexts;
    int selectedIndex;

    public void Start(){
        // 自分の子要素で<SelectableText>コンポーネントを持っているものを集める
        selectableTexts = GetComponentsInChildren<SelectableText>();
        selectedIndex = 0;
    }

    public void Update(){
        if(Input.GetKeyDown(KeyCode.Return)){
            if(selectedIndex == 0){
                SceneManager.LoadScene("InputName");
            }else if(selectedIndex == 1){
                SceneManager.LoadScene("Dungeon2");
            }
        }

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
        selectedIndex = 0;
        gameObject.SetActive(true);
    }
    public void Close(){
        gameObject.SetActive(false);
    }
}
