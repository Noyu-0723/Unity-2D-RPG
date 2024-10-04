using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 戦闘時に持っているアイテムから生成される
public class Item{
    public ItemBase Base { get; set; }
    public Item(ItemBase itemBase){
        Base = itemBase;
    }
}