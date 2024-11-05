using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToOtherArea : MonoBehaviour
{
    [SerializeField] string nextArea;

    public string GetNextArea(){
        return nextArea;
    }
}
