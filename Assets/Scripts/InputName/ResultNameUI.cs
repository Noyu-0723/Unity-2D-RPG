using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultNameUI : MonoBehaviour{
    public GameObject parentObject; // 親オブジェクト
    public int currentNameIndex;
    public string[] playerNameArray;

    public void Init(){
        playerNameArray = new string[] {"＊", "＊", "＊", "＊", "＊", "＊"}; // 改善の余地あり
        currentNameIndex = 0;
        ResultNameUpdate();
    }
    
    public void ResultNameUpdate(){
        Text resultText = parentObject.GetComponentInChildren<Text>();
        resultText.text = ""; // 初期化
        for (int i = 0; i < playerNameArray.Length; i++){
            resultText.text += playerNameArray[i];
        }
    }
}
