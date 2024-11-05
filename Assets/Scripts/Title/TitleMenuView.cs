using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenuView : MonoBehaviour{
    private PlayerController playerController;
    private SaveLoadManager saveLoadManager;
    private SaveData saveData;

    SelectableText[] selectableTexts;
    int selectedIndex;

    public void Start(){
        playerController = FindObjectOfType<PlayerController>();
        if(playerController != null){
            playerController.gameObject.SetActive(false);
        }
        saveLoadManager = GetComponent<SaveLoadManager>();
        saveData = saveLoadManager.LoadGame();
        selectableTexts = GetComponentsInChildren<SelectableText>();
        selectedIndex = 0;
    }

    public void Update(){
        if(Input.GetKeyDown(KeyCode.Return)){
            if(selectedIndex == 0){
                saveLoadManager.DeleteSaveData();
                if(playerController != null){
                    PlayerController.playerProgress = new List<string>[]{
                        new List<string>(),
                        new List<string>(),
                        new List<string>()
                    };
                    Destroy(playerController.gameObject);
                }
                SceneManager.LoadScene("InputName");
            }else if(selectedIndex == 1 && saveData != null){
                SceneManager.LoadScene(saveData.map);
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
