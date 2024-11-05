using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 戦闘時に覚えている技から生成される
public class Move{
    public MoveBase Base { get; set; }
    public Move(MoveBase moveBase){
        Base = moveBase;
    }
}