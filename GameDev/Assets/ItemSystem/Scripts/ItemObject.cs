using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// The base class to create a item. Its a ScriptableObject and can be create in the Editor
/// All items have a Ui Image, can be stackable or not, have a type and a description and have some Item informations 
/// </summary>
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory System/Items/item")]
public class ItemObject : ScriptableObject {

    public string itemName;
    public string characterDisplayName;
    public GameObject characterDisplay;
    public Sprite uiDisplayImage;               // inventory image
    public bool istStackable;
    public ItemType itemType;
    [TextArea(5, 20)] public string itemDescription;
    public Item data = new Item();

}


/// <summary>
/// Defines the types for a item 
/// </summary>
public enum ItemType {
    Amulet,
    Armor,
    Book,
    Boots,
    Default,
    Edible,
    Glove,
    Money,
    Trousers,
    Ring,
    Spell,
    Weapon,
    Questitem,
}



