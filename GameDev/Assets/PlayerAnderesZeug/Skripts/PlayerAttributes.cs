using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Hold all player attributes. Set, change and show them.
/// </summary>
public class PlayerAttributes : MonoBehaviour {

    public InventoryObject playerEquipment;     //referenz to the EquipObject from the player. Referenz set in editor
    public Attribute[] playerAttributes;        //array of the attributes that the player have. Attributes set in editor

    public int maxHealth;
    public int currentHealth;

    public int maxStamina;
    public int currentStamina;

    public int maxMana;
    public int currentMana;

    public TextMeshProUGUI textHealthPoints;         //referenz set in editor
    public TextMeshProUGUI textArmor;                //referenz set in editor
    public TextMeshProUGUI textPhysicalDamage;      //referenz set in editor
   


    /// <summary>
    /// loob through all attributes of the player and set the parent.
    /// loop  through the equipment and set the funktioncs
    /// set the default player values, attribute values and set the disyplay of the attributes
    /// </summary>
    private void Start() {

        for (int i = 0; i < playerAttributes.Length; i++) {
            playerAttributes[i].SetParent(this);
        }
        for (int i = 0; i < playerEquipment.GetSlots.Length; i++) {
            playerEquipment.GetSlots[i].OnBeforeUpdate += OnBeforeSlotUpdate;
            playerEquipment.GetSlots[i].OnAfterUpdate += OnAfterSlotUpdate;
        }

        SetDefaultPlayerValue();
        SetUiAttributValues();
        SetMaxAttributValuesToPlayer();

        currentHealth = maxHealth;
        currentMana = maxMana;
        currentStamina = maxStamina;
    }

    /// <summary>
    /// Remove the buff in the slot, from the player.
    /// </summary>
    /// <param name="_slot">the slot with the item that is removed</param>
    public void OnBeforeSlotUpdate(InventorySlot _slot) {

        if (_slot.ItemObject == null) { //wenn vorher nichts im slot ist 
            return;
        }
        switch (_slot.parentUserInterface.inventory.type) {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                //print(string.Concat("Removed ", _slot.ItemObject, " from ", _slot.parentUserInterface.inventory.type));

                for (int i = 0; i < _slot.itemInInventorySlot.buffs.Length; i++) {
                    for (int j = 0; j < playerAttributes.Length; j++) {
                        if (playerAttributes[j].type == _slot.itemInInventorySlot.buffs[i].attribute) { //wenn das attribut des items, dass gleiche ist wie auf dem Characterr
                            playerAttributes[j].totalAttributValue.RemoveModifier(_slot.itemInInventorySlot.buffs[i]);
                        }
                    }
                }

                break;

            case InterfaceType.Dealer:
                break;
            case InterfaceType.Chest:
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Add the buff of the item in the slot to the player 
    /// </summary>
    /// <param name="_slot">the slot with the item that is add</param>
    public void OnAfterSlotUpdate(InventorySlot _slot) {

        if (_slot.ItemObject == null) {
            return;
        }

        switch (_slot.parentUserInterface.inventory.type) {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                //print(string.Concat("Placed ", _slot.ItemObject, " on ", _slot.parentUserInterface.inventory.type));

                for (int i = 0; i < _slot.itemInInventorySlot.buffs.Length; i++) {
                    for (int j = 0; j < playerAttributes.Length; j++) {
                        if (playerAttributes[j].type == _slot.itemInInventorySlot.buffs[i].attribute) {
                            playerAttributes[j].totalAttributValue.AddModifier(_slot.itemInInventorySlot.buffs[i]);
                        }
                    }
                }

                break;
            case InterfaceType.Dealer:
                break;
            case InterfaceType.Chest:
                break;
            default:
                break;
        }
    }


    /// <summary>
    /// Updates the player values and the display of the values.
    /// </summary>
    public void AttributeModified() {
        SetUiAttributValues();
        SetMaxAttributValuesToPlayer();
    }

    /// <summary>
    /// Set the deafult attribute values of the player
    /// </summary>
    private void SetDefaultPlayerValue() {
        for (int i = 0; i < playerAttributes.Length; i++) {

            if (playerAttributes[i].type == Attributes.HealthPoints)
                playerAttributes[i].totalAttributValue.BaseValue = 100;
            if (playerAttributes[i].type == Attributes.ManaPoints)
                playerAttributes[i].totalAttributValue.BaseValue = 100;
            if (playerAttributes[i].type == Attributes.Stamina)
                playerAttributes[i].totalAttributValue.BaseValue = 100;
        }
    }

    /// <summary>
    /// Set the max attribute values on the player
    /// </summary>
    private void SetMaxAttributValuesToPlayer() {
        for (int i = 0; i < playerAttributes.Length; i++) {
           
            if (playerAttributes[i].type == Attributes.HealthPoints)
                maxHealth = playerAttributes[i].totalAttributValue.TotalAttributeValue; 
            if (playerAttributes[i].type == Attributes.ManaPoints)
                maxMana = playerAttributes[i].totalAttributValue.TotalAttributeValue; 
            if (playerAttributes[i].type == Attributes.Stamina)
                maxStamina = playerAttributes[i].totalAttributValue.TotalAttributeValue;
            
        }
    }

    /// <summary>
    /// set the display of the attribute values
    /// </summary>
    private void SetUiAttributValues() {
        for (int i = 0; i < playerAttributes.Length; i++) {
           
            if (playerAttributes[i].type == Attributes.HealthPoints)
                textHealthPoints.text = playerAttributes[i].totalAttributValue.TotalAttributeValue.ToString();
            if (playerAttributes[i].type == Attributes.PhysicalDamage)
                textPhysicalDamage.text = playerAttributes[i].totalAttributValue.TotalAttributeValue.ToString();
            if (playerAttributes[i].type == Attributes.Armor)
                textArmor.text = playerAttributes[i].totalAttributValue.TotalAttributeValue.ToString();
        }
    }

}

