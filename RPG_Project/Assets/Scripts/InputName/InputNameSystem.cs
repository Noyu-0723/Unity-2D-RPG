using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class InputNameSystem : MonoBehaviour{

    [SerializeField] NameListUI nameListUI_A; // ひらがな
    [SerializeField] NameListUI nameListUI_B; // カタカナ
    [SerializeField] ResultNameUI resultNameUI;

    NameListUI nameListUI;
    public static string playerNameOrigin;
    
    private void Start(){
        playerNameOrigin = ""; // 変更の余地あり
        nameListUI_A.Init();
        nameListUI_B.Init();
        resultNameUI.Init();
        nameListUI_B.Close();
        nameListUI = nameListUI_A;
        nameListUI.SetCurrentString(nameListUI.selectedIndex);
    }

    private void Update(){
        if(Input.GetKeyDown(KeyCode.Return)){
            OnPressReturn();
        }else if(Input.GetKeyDown(KeyCode.Space)){
            OnPressSpace();
        }
    }

    private void OnPressReturn(){
        if(nameListUI.parentSelectedIndex <= 2){
            resultNameUI.currentNameIndex = Mathf.Clamp(resultNameUI.currentNameIndex, 0, resultNameUI.playerNameArray.Length - 1);
            resultNameUI.playerNameArray[resultNameUI.currentNameIndex] = nameListUI.GetCurrentString();
            resultNameUI.currentNameIndex = Mathf.Clamp(resultNameUI.currentNameIndex + 1, 0, resultNameUI.playerNameArray.Length);
            resultNameUI.ResultNameUpdate();
        }else if(nameListUI.parentSelectedIndex == 3){
            if(nameListUI.selectedIndex == 0){
                // 仮名変換
                int currentID = 88; // 改善の余地あり
                nameListUI.Close();
                if(nameListUI == nameListUI_A){
                    nameListUI = nameListUI_B;
                }else{
                    nameListUI = nameListUI_A;
                }
                nameListUI.Open();
                nameListUI.SetCurrentString(currentID);
            }else if(nameListUI.selectedIndex == 1){
                // 一文字削除
                OnPressSpace();
            }else if(nameListUI.selectedIndex == 2 && resultNameUI.currentNameIndex >= 1){
                // 決定
                for(int i = 0; i < resultNameUI.currentNameIndex; i++){
                    playerNameOrigin += resultNameUI.playerNameArray[i];
                }
                SceneManager.LoadScene("1F");
            }
        }
    }

    private void OnPressSpace(){
        resultNameUI.currentNameIndex = Mathf.Clamp(resultNameUI.currentNameIndex - 1, 0, resultNameUI.playerNameArray.Length);
        resultNameUI.playerNameArray[resultNameUI.currentNameIndex] = "＊";
        resultNameUI.ResultNameUpdate();
    }
}
