using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A lootbag is a list ob ItemObjects.
/// In the editor we can set which items are inside the lootbag.
/// </summary>
public class GroundItemBag : MonoBehaviour {
    public ItemObject[] itemInBag;
    public int id;
}
