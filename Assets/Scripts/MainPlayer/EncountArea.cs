using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncountArea : MonoBehaviour
{
    [SerializeField] List<Battler> enemies;

    public Battler GetRandomBattler(){
        int r = Random.Range(0, enemies.Count);
        return enemies[r];
    }
}