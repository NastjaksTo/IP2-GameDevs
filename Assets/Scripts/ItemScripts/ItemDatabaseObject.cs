using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Hold all itemsObjects that exist in the game and gives them an ID. This way you don't have to give each item an ID manually. A ItemDatabaseObject can be created in the editor.
/// Extend ISerializationCallbackReceiver -> unity dont serialize dictionary. So we use the two callbacks to manually serialize it.
/// </summary>
[CreateAssetMenu(fileName = "New Item Database", menuName ="Inventory System/Items/Database")]
public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver {

    public ItemObject[] ItemObjects;  //set it the editor

    /// <summary>
    ///  Call UpdateID() to update the Ids of the items in the database after deserialization.
    /// </summary>
    public void OnAfterDeserialize() {
        UpdateID();
    }

    public void OnBeforeSerialize() {
    }

    /// <summary>
    /// Update the Ids of the items in the database. 
    /// </summary>
    [ContextMenu("Update ID's")]
    public void UpdateID() {
        for (int i = 0; i < ItemObjects.Length; i++) {
            if (ItemObjects[i].data.Id != i) {
                ItemObjects[i].data.Id = i;
            }
        }
    }
}
