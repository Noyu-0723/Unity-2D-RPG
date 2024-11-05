using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData{
    public Vector3 playerPosition;
    public List<string> doorProgress;
    public List<string> itemBoxProgress;
    public List<string> characterProgress;
    public int level;
    public int hp;
    public int mp;
    public int hasExp;
    public int walkCount;
    public List<ItemBase> inventory;
    public string map;

    public SaveData(){
        doorProgress = new List<string>();
        itemBoxProgress = new List<string>();
        characterProgress = new List<string>();
        inventory = new List<ItemBase>();
    }
}
