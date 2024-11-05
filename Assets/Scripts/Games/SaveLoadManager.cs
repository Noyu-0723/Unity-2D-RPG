using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour{
    private string saveFilePath;

    private void Awake(){
        saveFilePath = Path.Combine(Application.persistentDataPath, "save.json");
    }

    public void SaveGame(SaveData saveData){
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(saveFilePath, json);
    }

    public SaveData LoadGame(){
        if(File.Exists(saveFilePath)){
            string json = File.ReadAllText(saveFilePath);
            return JsonUtility.FromJson<SaveData>(json);
        }
        return null; 
    }

    public void DeleteSaveData(){
        if(File.Exists(saveFilePath)){
            File.Delete(saveFilePath);
        }
    }
}
