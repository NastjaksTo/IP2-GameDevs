using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/// <summary>
/// Defines the types that an inventory can have
/// </summary>
public enum InterfaceType {
    Inventory,
    Equipment,
    Dealer,
    Chest
}

/// <summary>
/// Base class for all types of inventories. An inventory object can be created in the editor
/// </summary>
[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject {
    public string savePath;
    public ItemDatabaseObject database;                                     //reference to the item database to get the items
    public InterfaceType type;
    public Inventory Container;                                             //Conteiner that holds the slots

    public InventorySlot[] GetSlots { get { return Container.Slots; } }

    /// <summary>
    /// Function adds the item with the amount to the list.
    /// Checks if the item already exists. If the item does not exist in the inventory or is not stackable, it is placed in a new slot.
    /// If the item is available and stackable, the amount is increased.
    /// </summary>
    /// <param name="_item">the itme to be added</param>
    /// <param name="_amount">the amount of the item to be added</param>
    /// <returns>false if the item cannot be added because there are no free slots, true when added</returns>
    public bool AddItem(Item _item, int _amount) {
        if (CountEmptySlot <= 0) {
            return false;
        }

        InventorySlot slot = FindeItemOnInventory(_item);

        if (!database.ItemObjects[_item.Id].istStackable || slot == null) {
            SetFirstEmptySlot(_item, _amount);
            return true;
        }
        slot.AddAmount(_amount);
        return true;
    }

    /// <summary>
    /// Checks if the item already exists in the inventory.
    /// </summary>
    /// <param name="_Item"></param>
    /// <returns>return the slot if the item exists and return null if not.</returns>
    public InventorySlot FindeItemOnInventory(Item _Item) {
        for (int i = 0; i < GetSlots.Length; i++) {
            if (GetSlots[i].itemInInventorySlot.Id == _Item.Id) {
                return GetSlots[i];
            }
        }
        return null;
    }

    /// <summary>
    /// Loop through all slots in the inventory and count the empty one. Return the amount of empty slots. (itemInInventorySlot.Id <= -1 means the slot is empty)
    /// </summary>
    public int CountEmptySlot {
        get {
            int counter = 0;
            for (int i = 0; i < GetSlots.Length; i++) {
                if (GetSlots[i].itemInInventorySlot.Id <= -1) {
                    counter++;
                }
            }
            return counter;
        }
    }

    /// <summary>
    /// Finde and set the first empty slot in the inventory.
    /// </summary>
    /// <param name="_item">the item that shoud set in the slot</param>
    /// <param name="_amount">the amount of the item that shoud set in the slot</param>
    /// <returns>returns the set slot ore</returns>
    public InventorySlot SetFirstEmptySlot(Item _item, int _amount) {
        for (int i = 0; i < GetSlots.Length; i++) {
            if (GetSlots[i].itemInInventorySlot.Id <= -1) {                 //A empty slot have the item id = -1
                GetSlots[i].UpdateSlot(_item, _amount);
                return GetSlots[i];
            }
        }

        //TODO: was passiert, wenn das INventar voll ist.
        return null;
    }

    /// <summary>
    /// Swaps the places of the items. Checks if the items can be placed in the slots. Creates a temporary copy of item 2 to swap the values of the items.
    /// </summary>
    /// <param name="item1">the item that is dragged</param>
    /// <param name="item2">the item to be swapped with</param>
    public void SwappItems(InventorySlot item1, InventorySlot item2) {
        if (item2.CanPlaceInSlot(item1.ItemObject) && item1.CanPlaceInSlot(item2.ItemObject)) {
            InventorySlot temp = new InventorySlot(item2.itemInInventorySlot, item2.amountOfItemInInventorySlot);
            item2.UpdateSlot(item1.itemInInventorySlot, item1.amountOfItemInInventorySlot);
            item1.UpdateSlot(temp.itemInInventorySlot, temp.amountOfItemInInventorySlot);
        }
    }

    /// <summary>
    /// Remove a item from the inventory.
    /// Loop through all slots, if the item to be removed is in the inventory, it will be removed ( -> the slot is set to the default state).
    /// </summary>
    /// <param name="_item">item to remove</param>
    public void RemoveItem(Item _item) {
        for (int i = 0; i < GetSlots.Length; i++) {
            if (GetSlots[i].itemInInventorySlot == _item) {
                GetSlots[i].UpdateSlot(null, 0);
            }
        }
    }

    /// <summary>
    /// Save the inventory. Function can be called in the editor.
    /// </summary>
    [ContextMenu("Save")]
    public void Save() {
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, Container);
        stream.Close();
    }

    /// <summary>
    /// Load the inventory. Function can be called in the editor.
    /// </summary>
    [ContextMenu("Load")]
    public void Load() {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath))) {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
            Inventory newContainer = (Inventory)formatter.Deserialize(stream);

            for (int i = 0; i < GetSlots.Length; i++) {
                GetSlots[i].UpdateSlot(newContainer.Slots[i].itemInInventorySlot, newContainer.Slots[i].amountOfItemInInventorySlot);
            }

            stream.Close();
        }
    }

    /// <summary>
    /// Clears the inventory (Container). Function can be called in the editor.
    /// </summary>
    [ContextMenu("Clear")]
    public void Clear() {
        Container.Clear();
    }
}