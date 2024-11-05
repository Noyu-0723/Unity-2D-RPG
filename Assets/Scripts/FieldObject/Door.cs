using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] string value;

    private void Start(){
        if(PlayerController.playerProgress[0].Contains(gameObject.name)){
            gameObject.SetActive(false);
        }
    }

    public bool isOpenDoor(Item key){
        bool isOpen = false;
        if(key.Base.Type == ItemBase.Types.Key){
            isOpen = key.Base.Name == value;
            if(isOpen){
                gameObject.SetActive(false);
            }
        }
        return isOpen;
    }
}
