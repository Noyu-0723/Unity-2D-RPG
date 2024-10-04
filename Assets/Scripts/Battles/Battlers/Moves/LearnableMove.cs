using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// どのレベルで何を覚えるのか
[System.Serializable]
public class LearnableMove
{
    [SerializeField] MoveBase moveBase;
    [SerializeField] int level;

    public MoveBase MoveBase { get => moveBase; }
    public global::System.Int32 Level { get => level; }
}