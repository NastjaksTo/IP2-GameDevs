using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Describes the ineraction of the player with the collectible Items on the map. 
/// Connects the player with his inventories.
/// </summary>
public class PlayerInventory : MonoBehaviour {

    public InventoryObject playerInventory;             // reference to the player's inventory inventory. referenz set in editor
    public InventoryObject playerEquipment;             // reference to the player's equipment inventory. referenz set in editor
    public PlayeInteractionAlert collectAlert;          // referenz set in editor

    private GroundItem item;
    private GroundItemBag itemBag;

    /// <summary>
    /// When the player collides with something, it is checked whether it is an item or a itemBag.
    /// If it ist a item or a itemBag: the collect alert displayed and the item or the itemBag is temporarily stored.
    /// </summary>
    /// <param name="other">The object with which the player collides.</param>
    public void OnTriggerEnter(Collider other) {
        if (other.GetComponent<GroundItem>() || other.GetComponent<GroundItemBag>()) {
            collectAlert.OpenCollectAlertUi();
            item = other.GetComponent<GroundItem>();
            itemBag = other.GetComponent<GroundItemBag>();
        }
    }

    /// <summary>
    /// When the player leaves the collision, a check is made to see if it is an item or an ItemBag.
    /// If there is no item or an ItemBag the temporarily stored informations reset and the collection message disappears.
    /// </summary>
    /// <param name="other">The object with which the player collides.</param>
    private void OnTriggerExit(Collider other) {
        if (other.GetComponent<GroundItem>() == null || other.GetComponent<GroundItemBag>() == null) {
            collectAlert.CloseCollectAlertUi();
            item = null;
            itemBag = null;
        }
    }


    /// <summary>
    /// Collects a single item: 
    /// If it was added to the inventory -> close the collect alert and destroy the collected item on the Map.
    /// </summary>
    /// <param name="item">The item to be collected.</param>
    public void ClollectItem(GroundItem item) {
        Item _item = new Item(item.item);
        if (playerInventory.AddItem(_item, 1)) { 
            collectAlert.CloseCollectAlertUi();
            Destroy(item.gameObject); 
        }
    }

    /// <summary>
    /// Collects a bag with items: loops through the items in the bag and add them to the inventory.
    /// Then close the collect alert and destroy the collected bag on the map.
    /// </summary>
    /// <param name="_itemBag">The bag with items to be collected.</param>
    public void ClollectItems(GroundItemBag _itemBag) { //TODO: wenn das inventar voll ist werden alle nicht hinzugef�gten Items auch zerst�rt!!!
        
        for (int i = 0; i < _itemBag.itemInBag.Length; i++) {
            Item _item = new Item(_itemBag.itemInBag[i]);
            playerInventory.AddItem(_item, 1);
        }
        
        collectAlert.CloseCollectAlertUi();
        Destroy(_itemBag.gameObject);
    }


    /// <summary>
    /// Checks each frame the input for the key P, L and E.
    /// On P: Save the current inventory and equipment of the player.
    /// On L: Loads the player's last saved inventory and equipment.
    /// On E: Collect an item or bag of items if one is available.
    /// </summary>
    private void Update() {


        if (Input.GetKeyDown(KeyCode.E)) {

            if (itemBag) {
                ClollectItems(itemBag);
            }

            if (item) {
                ClollectItem(item);
            }
        }
    }


    /// <summary>
    /// The inventory is reset when the application is closed.
    /// </summary>
    private void OnApplicationQuit() {
        playerInventory.Clear();
        playerEquipment.Clear();
    }
}
