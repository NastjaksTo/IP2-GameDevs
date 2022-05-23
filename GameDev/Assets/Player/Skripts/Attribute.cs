using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Describes an attribute
/// </summary>
[System.Serializable]
public class Attribute {
    [System.NonSerialized]
    public PlayerAttributes parent;
    public Attributes type;
    public ModifiableInt totalAttributValue;

    /// <summary>
    /// Set the parent. Fires the "ModifiableInt(AttributeModified)" function every time a value changes.
    /// </summary>
    /// <param name="_parent">The parent. (e.g referenz to the Player(- Attributes))</param>
    public void SetParent(PlayerAttributes _parent) {
        parent = _parent;
        totalAttributValue = new ModifiableInt(AttributeModified);
    }

    /// <summary>
    ///  Calls the AttributeModified function in the parent.
    /// </summary>
    public void AttributeModified() {
        parent.AttributeModified();
    }
}


/// <summary>
/// Defines the types of attributes for the item buffs and the player
/// </summary>
public enum Attributes {
    Armor,
    EarthKnowledge,
    FireKnowledge,
    IceKnowledge,
    HealthPoints,
    ManaPoints,
    PhysicalDamage,
    Stamina,
}