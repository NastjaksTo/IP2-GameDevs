using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  A item have a id and can have some buffs
/// </summary>
[System.Serializable]
public class Item {
    public int Id = -1;
    public ItemBuff[] buffs;

    /// <summary>
    ///  Konstruktor to set a item
    /// </summary>
    /// <param name="item"> the item that should be set </param>
    public Item(ItemObject item) {

        Id = item.data.Id;
        buffs = new ItemBuff[item.data.buffs.Length];                  

        for (int i = 0; i < buffs.Length; i++) {
            buffs[i] = new ItemBuff(item.data.buffs[i].buffValue) {        
                attribute = item.data.buffs[i].attribute
            };
        }
    }

    /// <summary>
    /// Konstruktor to set a default (empty) item
    /// </summary>
    public Item() {
        Id = -1;
    }
}