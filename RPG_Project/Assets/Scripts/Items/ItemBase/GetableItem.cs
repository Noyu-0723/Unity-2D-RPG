using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// どのレベルで何を覚えるのか
[System.Serializable]
public class GetableItem
{
    [SerializeField] ItemBase itemBase;
    [SerializeField] int probability;

    public ItemBase ItemBase { get => itemBase; }
    public global::System.Int32 Probability { get => probability; }
}