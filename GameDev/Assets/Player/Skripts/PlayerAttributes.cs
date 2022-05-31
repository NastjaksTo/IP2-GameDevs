using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

/// <summary>
/// Hold all player attributes. Set, change and show them. And puts the armor on the player.
/// </summary>
public class PlayerAttributes : MonoBehaviour {

    public InventoryObject playerEquipment;     //reference to the EquipObject from the player. Referenz set in editor
    public Attribute[] playerAttributes;        //array of the attributes that the player have. Attributes set in editor

    private Transform chestOnPlayer;
    private Transform glovesOnPlayer;
    private Transform trousersOnPlayer;
    private Transform bootsOnPlayer;

    private GameObject weaponOnPlayer;
    public Transform weaponTransform;


    private BoneCombiner boneCombiner;

    public int maxHealth;
    public int currentHealth;

    public int maxStamina;
    public float currentStamina;
    public float staminaRegenerationSpeed;

    public int maxMana;
    public float currentMana;
    public float manaRegenerationSpeed;

    public bool fireKnowladgeEquiped;
    public bool iceKnowladgeEquiped;
    public bool earthKnowladgeEquiped;

    public TextMeshProUGUI textHealthPoints;         //reference set in editor
    public TextMeshProUGUI textMaxMana;              //reference set in editor
    public TextMeshProUGUI textMaxStamina;           //reference set in editor
    public TextMeshProUGUI textArmor;                //reference set in editor
    public TextMeshProUGUI textPhysicalDamage;       //reference set in editor

    /// <summary>
    /// loob through all attributes of the player and set the parent.
    /// loop  through the equipment and set the funktioncs
    /// set the default player values, attribute values and set the disyplay of the attributes
    /// </summary>
    private void Start() {

        boneCombiner = new BoneCombiner(gameObject);

        for (int i = 0; i < playerAttributes.Length; i++) {
            playerAttributes[i].SetParent(this);
        }
        for (int i = 0; i < playerEquipment.GetSlots.Length; i++) {
            playerEquipment.GetSlots[i].OnBeforeUpdate += OnRemoveItemFromEquip;
            playerEquipment.GetSlots[i].OnAfterUpdate += OnAddItemToEquip;
        }

        SetDefaultPlayerValue();
        SetUiAttributValues();
        SetMaxAttributValuesToPlayer();

        currentHealth = maxHealth;
        currentMana = maxMana;
        currentStamina = maxStamina;

        manaRegenerationSpeed = 1;
        staminaRegenerationSpeed = 5;
    }

    /// <summary>
    /// Remove the buff in the slot, from the player.
    /// </summary>
    /// <param name="_slot">the slot with the item that is removed</param>
    public void OnRemoveItemFromEquip(InventorySlot _slot) {

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

                if (_slot.ItemObject.characterDisplay != null) { //wenn das ausger�stete Item etwas hat, was den Char angezogen werden kann
                    switch (_slot.AllowedItems[0]) {

                        case ItemType.Armor:
                            //Destroy(chestOnPlayer.gameObject);
                            break;
                        case ItemType.Boots:
                            //Destroy(bootsOnPlayer.gameObject);
                            break;
                        case ItemType.Glove:
                            //Destroy(glovesOnPlayer.gameObject);
                            break;
                        case ItemType.Trousers:
                            //Destroy(trousersOnPlayer.gameObject);
                            break;
                        case ItemType.Weapon:
                            Destroy(weaponOnPlayer?.gameObject);


                            break;
                        default:
                            break;
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
    public void OnAddItemToEquip(InventorySlot _slot) {

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


                if (_slot.ItemObject.characterDisplay != null) { //wenn das ausger�stete Item etwas hat, was den Char angezogen werden kann
                    switch (_slot.AllowedItems[0]) {

                        case ItemType.Armor:
                            //chestOnPlayer = boneCombiner.AddLimb(_slot.ItemObject.characterDisplay, _slot.ItemObject.boneName);
                            //chestOnPlayer = boneCombiner.AddLimb(_slot.ItemObject.characterDisplay);
                            break;
                        case ItemType.Boots:
                            //bootsOnPlayer = boneCombiner.AddLimb(_slot.ItemObject.characterDisplay);
                            break;
                        case ItemType.Glove:
                            //glovesOnPlayer = boneCombiner.AddLimb(_slot.ItemObject.characterDisplay);
                            break;
                        case ItemType.Trousers:
                            //trousersOnPlayer = boneCombiner.AddLimb(_slot.ItemObject.characterDisplay);
                            break;
                        case ItemType.Weapon:
                            weaponOnPlayer = Instantiate(_slot.ItemObject.characterDisplay, weaponTransform);
                            break;
                        default:
                            break;
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

            if (playerAttributes[i].type == Attributes.FireKnowledge) {
                if (playerAttributes[i].totalAttributValue.TotalAttributeValue == 1) {
                    fireKnowladgeEquiped = true;
                } else {
                    fireKnowladgeEquiped = false;
                }
            }

            if (playerAttributes[i].type == Attributes.IceKnowledge) {
                if (playerAttributes[i].totalAttributValue.TotalAttributeValue == 1) {
                    iceKnowladgeEquiped = true;
                } else {
                    iceKnowladgeEquiped = false;
                }
            }

            if (playerAttributes[i].type == Attributes.EarthKnowledge) {
                if (playerAttributes[i].totalAttributValue.TotalAttributeValue == 1) {
                    earthKnowladgeEquiped = true;
                } else {
                    earthKnowladgeEquiped = false;
                }

            }
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
            if (playerAttributes[i].type == Attributes.ManaPoints)
                textMaxMana.text = playerAttributes[i].totalAttributValue.TotalAttributeValue.ToString();
            if (playerAttributes[i].type == Attributes.Stamina)
                textMaxStamina.text = playerAttributes[i].totalAttributValue.TotalAttributeValue.ToString();
        }


    }

    private void Update() {
        AttributeModified();
        if (currentMana < maxMana) {
            currentMana += manaRegenerationSpeed * Time.deltaTime;
        }
        if (currentStamina < maxStamina && !Input.GetKey(KeyCode.LeftShift)) {
            currentStamina += staminaRegenerationSpeed * Time.deltaTime;
        }
    }
}

