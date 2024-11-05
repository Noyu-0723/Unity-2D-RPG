using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] Battler battler;
    [SerializeField] List<string> lines;
    [SerializeField] List<string> linesAfter;
    public enum Types {
        Battler,
        Ally,
    }
    [SerializeField] Types type; // キャラクターのタイプを格納
    
    public List<global::System.String> Lines { get => lines; }
    public List<global::System.String> LinesAfter { get => linesAfter; }
    public Types Type { get => type; }

    public int state;

    private void Start(){
        if(PlayerController.playerProgress[2].Contains(gameObject.name)){
            gameObject.SetActive(false);
        }
    }

    public Battler GetBattler(){
        return battler;
    }
}
