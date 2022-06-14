using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using static SkillTree;
using static PlayerDisplay;


/// <summary>
/// Hold all player attributes. Set, change and show them. And puts the armor on the player.
/// </summary>
public class PlayerAttributes : MonoBehaviour
{

    public static PlayerAttributes playerAttributesScript;
    
    public InventoryObject playerEquipment;     //reference to the EquipObject from the player. Referenz set in editor
    public Attribute[] playerAttributes;        //array of the attributes that the player have. Attributes set in editor

    private Transform chestOnPlayer;
    private Transform glovesOnPlayer;
    private Transform trousersOnPlayer;
    private Transform bootsOnPlayer;

    private GameObject weaponOnPlayer;
    public Transform weaponTransform;
    public bool hasWeaponEquiped;

    public int maxHealth;
    public float currentHealth;

    public int maxStamina;
    public float currentStamina;
    public float staminaRegenerationSpeed;

    public int maxMana;
    public float currentMana;
    public float manaRegenerationSpeed;

    public float physicalDamage;

    [HideInInspector] public bool fireKnowladgeEquiped;
    [HideInInspector] public bool iceKnowladgeEquiped;
    [HideInInspector] public bool earthKnowladgeEquiped;

    public TextMeshProUGUI textHealthPoints;         //reference set in editor
    public TextMeshProUGUI textMaxMana;              //reference set in editor
    public TextMeshProUGUI textMaxStamina;           //reference set in editor
    public TextMeshProUGUI textArmor;                //reference set in editor
    public TextMeshProUGUI textPhysicalDamage;       //reference set in editor

    private void Awake() => playerAttributesScript = this;  
    
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

        hasWeaponEquiped = false;
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

                for (int i = 0; i < _slot.itemInInventorySlot.buffs.Length; i++) {
                    for (int j = 0; j < playerAttributes.Length; j++) {
                        if (playerAttributes[j].type == _slot.itemInInventorySlot.buffs[i].attribute) { //wenn das attribut des items, dass gleiche ist wie auf dem Characterr
                            playerAttributes[j].totalAttributValue.RemoveModifier(_slot.itemInInventorySlot.buffs[i]);
                        }
                    }
                }

                if (_slot.ItemObject.characterDisplay != null) { //wenn das ausger�stete Item etwas hat, was den Char angezogen werden kann
                    switch (_slot.AllowedItems[0]) {
                        case ItemType.Weapon:
                            Destroy(weaponOnPlayer?.gameObject);
                            hasWeaponEquiped = false;
                            break;
                        default:
                            break;
                    }
                }


                if (_slot.ItemObject.characterDisplayName != null) { 
                    switch (_slot.AllowedItems[0]) {
                        case ItemType.Armor:
                            var bodyName = _slot.ItemObject.characterDisplayName.ToString();                        
                            var bodyObj = GameObject.Find(bodyName);
                            bodyObj.GetComponent<SkinnedMeshRenderer>().enabled = false;

                            var objDefBody = GameObject.Find("Naked_Armor_body");
                            objDefBody.GetComponent<SkinnedMeshRenderer>().enabled = true;

                            break;
                        case ItemType.Boots:
                            var bootsName = _slot.ItemObject.characterDisplayName.ToString();
                            var bootsObj = GameObject.Find(bootsName);
                            bootsObj.GetComponent<SkinnedMeshRenderer>().enabled = false;

                            var objDefBoots = GameObject.Find("Naked_Armor_boots");
                            objDefBoots.GetComponent<SkinnedMeshRenderer>().enabled = true;

                            break;
                        case ItemType.Glove:
                            var GloveName = _slot.ItemObject.characterDisplayName.ToString();
                            var GloveObj = GameObject.Find(GloveName);
                            GloveObj.GetComponent<SkinnedMeshRenderer>().enabled = false;

                            var objDefGloves = GameObject.Find("Naked_Armor_gauntlets");
                            objDefGloves.GetComponent<SkinnedMeshRenderer>().enabled = true;

                            break;
                        case ItemType.Trousers:
                            var trousersName = _slot.ItemObject.characterDisplayName.ToString();
                            var trousersObj = GameObject.Find(trousersName);
                            trousersObj.GetComponent<SkinnedMeshRenderer>().enabled = false;

                            var objDefTrousers = GameObject.Find("Naked_Armor_legs");
                            objDefTrousers.GetComponent<SkinnedMeshRenderer>().enabled = true;

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

        AttributeModified();
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
                
                for (int i = 0; i < _slot.itemInInventorySlot.buffs.Length; i++) {
                    for (int j = 0; j < playerAttributes.Length; j++) {
                        if (playerAttributes[j].type == _slot.itemInInventorySlot.buffs[i].attribute) {
                            playerAttributes[j].totalAttributValue.AddModifier(_slot.itemInInventorySlot.buffs[i]);

                        }
                    }
                }


                if (_slot.ItemObject.characterDisplay != null) { //wenn das ausger�stete Item etwas hat, was den Char angezogen werden kann
                    switch (_slot.AllowedItems[0]) {
                        case ItemType.Weapon:
                            weaponOnPlayer = Instantiate(_slot.ItemObject.characterDisplay, weaponTransform);
                            hasWeaponEquiped = true;
                            break;
                        default:
                            break;
                    }
                }

                if (_slot.ItemObject.characterDisplayName != null) { //wenn das ausgeruestete Item etwas hat, was den Char angezogen werden kann
                    switch (_slot.AllowedItems[0]) {

                        case ItemType.Armor:
                            var bodyName = _slot.ItemObject.characterDisplayName.ToString();
                            var bodyObj = GameObject.Find(bodyName);
                            bodyObj.GetComponent<SkinnedMeshRenderer>().enabled = true;

                            var objDefBody = GameObject.Find("Naked_Armor_body");
                            objDefBody.GetComponent<SkinnedMeshRenderer>().enabled = false;

                            break;
                        case ItemType.Boots:
                            var bootsName = _slot.ItemObject.characterDisplayName.ToString();
                            var bootsObj = GameObject.Find(bootsName);
                            bootsObj.GetComponent<SkinnedMeshRenderer>().enabled = true;

                            var objDefBoots = GameObject.Find("Naked_Armor_boots");
                            objDefBoots.GetComponent<SkinnedMeshRenderer>().enabled = false;


                            break;
                        case ItemType.Glove:
                            var GloveName = _slot.ItemObject.characterDisplayName.ToString();
                            var GloveObj = GameObject.Find(GloveName);
                            GloveObj.GetComponent<SkinnedMeshRenderer>().enabled = true;

                            var objDefGloves = GameObject.Find("Naked_Armor_gauntlets");
                            objDefGloves.GetComponent<SkinnedMeshRenderer>().enabled = false;

                            break;
                        case ItemType.Trousers:
                            var trousersName = _slot.ItemObject.characterDisplayName.ToString();
                            var trousersObj = GameObject.Find(trousersName);
                            trousersObj.GetComponent<SkinnedMeshRenderer>().enabled = true;

                            var objDefTrousers = GameObject.Find("Naked_Armor_legs");
                            objDefTrousers.GetComponent<SkinnedMeshRenderer>().enabled = false;

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

        AttributeModified();
    }

  

    /// <summary>
    /// Updates the player values and the display of the values.
    /// </summary>
    public void AttributeModified() {
        SetMaxAttributValuesToPlayer();
        SetUiAttributValues();
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
            if (playerAttributes[i].type == Attributes.HealthPoints) {
                var equippedHealthValue = playerAttributes[i].totalAttributValue.TotalAttributeValue;
                maxHealth = equippedHealthValue + skillTree.healthSkillvalue;
            }

            if (playerAttributes[i].type == Attributes.ManaPoints) {
                var equippedManaValue = playerAttributes[i].totalAttributValue.TotalAttributeValue;
                maxMana = equippedManaValue + skillTree.manaSkillvalue;
            }

            if (playerAttributes[i].type == Attributes.Stamina) {
                var equippedStaminaValue = playerAttributes[i].totalAttributValue.TotalAttributeValue;
                maxStamina = equippedStaminaValue + skillTree.staminaSkillvalue;
            }

            if (playerAttributes[i].type == Attributes.PhysicalDamage) {
                physicalDamage = playerAttributes[i].totalAttributValue.TotalAttributeValue;
            }

            if (playerAttributes[i].type == Attributes.FireKnowledge) {
                if (playerAttributes[i].totalAttributValue.TotalAttributeValue == 1) {
                    fireKnowladgeEquiped = true;
                    //playerDisplay.UpdateSpellUI();
                } else {
                    fireKnowladgeEquiped = false;
                }
            }

            if (playerAttributes[i].type == Attributes.IceKnowledge) {
                if (playerAttributes[i].totalAttributValue.TotalAttributeValue == 1) {
                    iceKnowladgeEquiped = true;
                    //playerDisplay.UpdateSpellUI();
                } else {
                    iceKnowladgeEquiped = false;
                }
            }

            if (playerAttributes[i].type == Attributes.EarthKnowledge) {
                if (playerAttributes[i].totalAttributValue.TotalAttributeValue == 1) {
                    earthKnowladgeEquiped = true;
                    //playerDisplay.UpdateSpellUI();
                } else {
                    earthKnowladgeEquiped = false;
                    //playerDisplay.UpdateSpellUI();
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
                textHealthPoints.text = maxHealth.ToString();

            if (playerAttributes[i].type == Attributes.ManaPoints)
                textMaxMana.text = maxMana.ToString();

            if (playerAttributes[i].type == Attributes.Stamina)
                textMaxStamina.text = maxStamina.ToString();

            if (playerAttributes[i].type == Attributes.PhysicalDamage)
                textPhysicalDamage.text = playerAttributes[i].totalAttributValue.TotalAttributeValue.ToString();

            if (playerAttributes[i].type == Attributes.Armor)
                textArmor.text = playerAttributes[i].totalAttributValue.TotalAttributeValue.ToString();

           
        }


    }

    private void Update() {
 
        if (currentMana < maxMana) {
            currentMana += manaRegenerationSpeed * Time.deltaTime;
        }

        if (currentStamina < maxStamina && !Input.GetKey(KeyCode.LeftShift)) {
            currentStamina += staminaRegenerationSpeed * Time.deltaTime;
        }
    }
}

