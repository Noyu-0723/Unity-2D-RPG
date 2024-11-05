using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    [SerializeField] List<ItemBase> itemList;

    private void Start(){
        if(PlayerController.playerProgress[1].Contains(gameObject.name)){
            gameObject.SetActive(false);
        }
    }

    public Item GetItem(){
        gameObject.SetActive(false);
        int r = Random.Range(0, itemList.Count);
        Item item = new Item(itemList[r]);
        return item;
    }
}
