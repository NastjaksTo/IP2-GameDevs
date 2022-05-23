using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// defines that an inventory is a list of InventorySlot (per default its 35 slots ). 
/// </summary>
[System.Serializable]
public class Inventory {
    public InventorySlot[] Slots = new InventorySlot[35];

    /// <summary>
    /// Loops through all InventorySlots an remove the items from them.
    /// </summary>
    public void Clear() {
        for (int i = 0; i < Slots.Length; i++) {
            Slots[i].RemoveItem();
        }
    }
}