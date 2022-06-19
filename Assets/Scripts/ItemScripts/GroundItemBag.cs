using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Class is holding the items that the object on the map is representing. A lootbag is a list ob ItemObjects.
/// In the editor we dan set which items are inside the lootbag.
/// </summary>
public class GroundItemBag : MonoBehaviour {
    public ItemObject[] itemInBag;
    public int id;
}
