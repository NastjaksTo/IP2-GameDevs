using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Delegates are used to pass methods as arguments to other methods 
/// </summary>
/// <param name="_slot"></param>
public delegate void SlotUpdated(InventorySlot _slot);


/// <summary>
/// A InventroySlot holds a Item and the Amount of them.
/// </summary>
[System.Serializable]
public class InventorySlot { 

    public ItemType[] AllowedItems = new ItemType[0];                               //list of allowed items for the slot - 0 = all item types are allowed
    [System.NonSerialized] public InventoryInterface parentUserInterface;
    [System.NonSerialized] public GameObject slotDisplay;                           //referenz to the display of the slot
    [System.NonSerialized] public SlotUpdated OnAfterUpdate;
    [System.NonSerialized] public SlotUpdated OnBeforeUpdate;
    public Item itemInInventorySlot;                                
    public int amountOfItemInInventorySlot;                         


    public InventorySlot() {
        UpdateSlot(new Item(), 0);
    }

    public InventorySlot(Item _itemInInventorySlot, int _amountOfItemInInventorySlot) {
        UpdateSlot(_itemInInventorySlot, _amountOfItemInInventorySlot);
    }

   public ItemObject ItemObject {
        get {
            if (itemInInventorySlot.Id >= 0) { //wenn dass Item exestiert
                return parentUserInterface.inventory.database.ItemObjects[itemInInventorySlot.Id];
            }
            return null;
        }
    }

    /// <summary>
    /// Update the slot.
    /// </summary>
    /// <param name="_itemInInventorySlot">the item in the slot</param>
    /// <param name="_amountOfItemInInventorySlot">the amount of the item</param>
    public void UpdateSlot(Item _itemInInventorySlot, int _amountOfItemInInventorySlot) {
        if (OnBeforeUpdate != null) {
            OnBeforeUpdate.Invoke(this); 
        }
        itemInInventorySlot = _itemInInventorySlot;
        amountOfItemInInventorySlot = _amountOfItemInInventorySlot;
        if (OnAfterUpdate != null) {
            OnAfterUpdate.Invoke(this);
        }
    }


    /// <summary>
    /// Remove the item from the slot (reset the item inside the slot).
    /// </summary>
    public void RemoveItem() {
        UpdateSlot(new Item(), 0);
    }

    /// <summary>
    /// Increase the amount of the item.
    /// </summary>
    /// <param name="value">the amount of the item to be added to the current amount of the item</param>
    public void AddAmount(int value) {
        UpdateSlot(itemInInventorySlot, amountOfItemInInventorySlot += value);
    }


    /// <summary>
    /// Checks if the item can be placed in the slot. First it is checked if there is a restriction for the slot at all, if not return true.
    /// "AllowedItems.Length <= 0" all items are allowed, "_itemObject.data.Id < 0" the slot is empty.
    /// If only a specific item type is allowed, it is checked if the item type equal to the requested item type
    /// </summary>
    /// <param name="_itemObject">the item to be placed in the slot</param>
    /// <returns>return true if the item is allowed to be placed in the slot. return false if not</returns>
    public bool CanPlaceInSlot(ItemObject _itemObject) {

        if (AllowedItems.Length <= 0 || _itemObject == null || _itemObject.data.Id < 0) {
            return true;
        }

        for (int i = 0; i < AllowedItems.Length; i++) {
            if (_itemObject.itemType == AllowedItems[i]) {
                return true;
            }
        }
        return false;
    }
}
