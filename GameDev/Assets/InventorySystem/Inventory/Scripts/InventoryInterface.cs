using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;


/// <summary>
/// Hold mouse data information.
/// </summary>
public static class MouseData {
    public static InventoryInterface interfaceMouseIsOver;
    public static GameObject tempItemBeingDragged;
    public static GameObject slotHoverdOver;
}


/// <summary>
/// Base class for all interface types to Display the Inventory - Just a representation of that what happen in the Inventory. And describes the behaivior on the Interface.
/// </summary>
public class InventoryInterface : MonoBehaviour {
    public InventoryObject inventory;                                                                               //Reference to the InventoryObject that should be displayd
    public Dictionary<GameObject, InventorySlot> slotsOnInterface = new Dictionary<GameObject, InventorySlot>();    //Hold all InventorySlot on the Interface. 
    public GameObject[] slots;

    public TextMeshProUGUI itemTitelUI;
    public TextMeshProUGUI itemDescriptionUI;
    public Image itemImageUI;
    public bool AAAA = true;



    /// <summary>
    ///  Start is called before the first frame update
    ///  Loop over all items inside the interfaces database and link the items to the interface as the parent.
    ///  Create the slots and update them to make sure that all items can be displayed.
    ///  Add OnEnterInterface and OnExitInterface event to the interface.
    /// </summary>
    void Start() {

        for (int i = 0; i < inventory.Container.Slots.Length; i++) {
            inventory.Container.Slots[i].parentUserInterface = this;
            inventory.GetSlots[i].OnAfterUpdate += OnSlotUpdate;
        }
        
        CreateSlots();
        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnPointerEnterInterface(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnPointerExitInterface(gameObject); });
        UpdateAllSlotDisplay();
    }

    public void LoadInterface() {
        for (int i = 0; i < inventory.Container.Slots.Length; i++) {
                inventory.Container.Slots[i].parentUserInterface = this;
                inventory.GetSlots[i].OnAfterUpdate += OnSlotUpdate; 
        }
        CreateSlots();
     
    
    }

    /// <summary>
    /// Set up the slot on the Interface. Creates a new dictionary with slotsOnInterface to prevent data problems.
    /// Loops over each slot and pass them to the new dictinoary + add them the events to to enable drag and drop.
    /// </summary>
    public void CreateSlots() {
        slotsOnInterface = new Dictionary<GameObject, InventorySlot>();

        for (int i = 0; i < inventory.Container.Slots.Length; i++) {

            var currentSlot = slots[i];

            AddEvent(currentSlot, EventTriggerType.PointerEnter, delegate { OnPointerEnterSlot(currentSlot); });
            AddEvent(currentSlot, EventTriggerType.PointerExit, delegate { OnPointerExitSlot(currentSlot); });
            AddEvent(currentSlot, EventTriggerType.BeginDrag, delegate { OnStartDrag(currentSlot); });
            AddEvent(currentSlot, EventTriggerType.EndDrag, delegate { OnEndDrag(currentSlot); });
            AddEvent(currentSlot, EventTriggerType.Drag, delegate { OnDrag(currentSlot); });

            inventory.GetSlots[i].slotDisplay = currentSlot;
            slotsOnInterface.Add(currentSlot, inventory.Container.Slots[i]);
        }
    }



    /// <summary>
    /// Adds an event to the Slot.
    /// </summary>
    /// <param name="obj">The object with which the event is to be linked (slotprefab)</param>
    /// <param name="type">The event that occurs when a function is to be executed.</param>
    /// <param name="action">The function to be executed when the event occurs</param>
    protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action) {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }


    /// <summary>
    /// Function updates all slots. Loops over all slots on the interface.
    /// If there is an Item in the slot (Id >= 0): set the sprite of the Item, set the Image to visible, set the amount text (if the amount is more then 1).
    /// If there is no Item in the slot: remove the sprite of the Item, set the Image to invisible, remove the text.
    /// </summary>
    public void UpdateAllSlotDisplay() {
        foreach (KeyValuePair<GameObject, InventorySlot> _slot in slotsOnInterface) {

            if (_slot.Value.itemInInventorySlot.Id >= 0) {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = _slot.Value.ItemObject.uiDisplayImage;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = _slot.Value.amountOfItemInInventorySlot == 1 ? "" : _slot.Value.amountOfItemInInventorySlot.ToString("n0"); //text update x:x => wie ein IF 
            } else {
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                _slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }
    }


    /// <summary>
    /// Function updates one slot.
    /// If there is an Item in the slot (Id >= 0): set the sprite of the Item, set the Image to visible, set the amount text (if the amount is more then 1).
    /// If there is no Item in the slot: remove the sprite of the Item, set the Image to invisible, remove the text.
    /// </summary>
    /// <param name="_slot"></param>
    private void OnSlotUpdate(InventorySlot _slot) {
        if (_slot.itemInInventorySlot.Id >= 0) {
            _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite = _slot.ItemObject.uiDisplayImage;
            _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
            _slot.slotDisplay.GetComponentInChildren<TextMeshProUGUI>().text = _slot.amountOfItemInInventorySlot == 1 ? "" : _slot.amountOfItemInInventorySlot.ToString("n0");
        } else { //wenn ichts im slot drin ist muss die UI an der stelle f�r den slot geclerat werden
            _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
            _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
            _slot.slotDisplay.GetComponentInChildren<TextMeshProUGUI>().text = "";
        }
    }




    /// <summary>
    /// Set the slotHoverdOver information. If there is no object on the mouse that is dragged,t he information of the item in the slot over which the mouse pointer hovers is displayed.
    /// </summary>
    /// <param name="obj">The Item we're hovering over</param>
    public void OnPointerEnterSlot(GameObject obj) {
        MouseData.slotHoverdOver = obj;
        if (MouseData.tempItemBeingDragged == null) {
            ShoweItemDescription(obj);
        }
    }

    /// <summary>
    /// Show the information of the object in the slot over which the mouse pointer hovers, if there is an item inside the slot.
    /// </summary>
    /// <param name="obj">The item we're hovering over</param>
    public void ShoweItemDescription(GameObject obj) {
        if (slotsOnInterface[obj].itemInInventorySlot.Id >= 0) {
            itemDescriptionUI.text = slotsOnInterface[obj].ItemObject.itemDescription;
            itemTitelUI.text = slotsOnInterface[obj].ItemObject.itemName;
            itemImageUI.sprite = slotsOnInterface[obj].ItemObject.uiDisplayImage;
        }
    }


    /// <summary>
    /// Set the slotHoverdOver to null if the mouse exit the slot.
    /// </summary>
    /// <param name="obj">The item we're hovering over</param>
    public void OnPointerExitSlot(GameObject obj) {
        MouseData.slotHoverdOver = null;
    }



    /// <summary>
    /// Set the interfaceMouseIsOver when the mouse enters an interface.
    /// </summary>
    /// <param name="obj">The item we're hovering over</param>
    private void OnPointerEnterInterface(GameObject obj) {
        MouseData.interfaceMouseIsOver = obj.gameObject.GetComponent<InventoryInterface>();
    }

    /// <summary>
    /// If the mouse exit the interface, set the interfaceMouseIsOver to null;
    /// </summary>
    /// <param name="obj">The item we're hovering over</param>
    private void OnPointerExitInterface(GameObject obj) {
        if (MouseData.slotHoverdOver = null) {
            MouseData.interfaceMouseIsOver = null;
        }
    }


    /// <summary>
    /// Calls the CreateTempItem when we on start dragging and store the TempItem
    /// </summary>
    /// <param name="obj">The item we're dragging</param>
    public void OnStartDrag(GameObject obj) {
        MouseData.tempItemBeingDragged = CreateTempItem(obj);
    }


    /// <summary>
    /// Created a temporary Item (a visual representation), that follow the mouse during the drag.
    /// </summary>
    /// <param name="obj">The object that is being dragged</param>
    /// <returns>The created temporery item</returns>
    public GameObject CreateTempItem(GameObject obj) {

        GameObject tempItem = null;
        if (slotsOnInterface[obj].itemInInventorySlot.Id >= 0) {                //If the object exists in the inventory
            tempItem = new GameObject();                                        //Create object that store the visual representation
            var rt = tempItem.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(110, 110);                               //set the size of the temporary Item
            tempItem.transform.SetParent(transform.parent);                     //Set the parent of the mouseobject to be the canvers that our inventory is displaying

            var img = tempItem.AddComponent<Image>();                           //add a Image component
            img.sprite = slotsOnInterface[obj].ItemObject.uiDisplayImage;       //set the image
            img.raycastTarget = false;
        }
        return tempItem;
    }


    /// <summary>
    /// If there is an item on the mouse, ensures that during dragging the position of the TempItem is the same as that of the mouse.
    /// </summary>
    /// <param name="obj">The Item that is being dragged</param>
    public void OnDrag(GameObject obj) {
        if (MouseData.tempItemBeingDragged != null) {
            MouseData.tempItemBeingDragged.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }



    /// <summary>
    /// On stop dragging: Destroys the temporary item on the mouse
    /// If the mouse is over a slot, swapp the place of the Items we're dragging with the item in the slot BUT just when the slot that is dragged contains something
    /// </summary>
    /// <param name="obj">The Item that is being dragged.</param>
    public void OnEndDrag(GameObject obj) {

        Destroy(MouseData.tempItemBeingDragged);
        if (slotsOnInterface[obj].itemInInventorySlot.Id >= 0) {
            if (MouseData.slotHoverdOver) {
                InventorySlot mouseHoverSlotData = MouseData.interfaceMouseIsOver.slotsOnInterface[MouseData.slotHoverdOver];
                inventory.SwappItems(slotsOnInterface[obj], mouseHoverSlotData);
            }

        }
    }
}


