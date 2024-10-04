using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultNameUI : MonoBehaviour
{
    public GameObject parentObject; // 親オブジェクト
    public int currentNameIndex;
    public string[] playerNameArray = {"＊", "＊", "＊", "＊", "＊", "＊"}; // 改善の余地あり

    public void Init(){
        currentNameIndex = 0;
        ResultNameUpdate();
    }
    
    public void ResultNameUpdate(){
        parentObject.GetComponentInChildren<Text>().text = "";
        for(int i = 0; i < playerNameArray.Length; i++){
            parentObject.GetComponentInChildren<Text>().text += playerNameArray[i];
        }
    }
}
