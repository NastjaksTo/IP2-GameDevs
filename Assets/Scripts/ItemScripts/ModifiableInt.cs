using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Delegate is a reference pointer to a method. 
/// It allows us to treat method as a variable and pass method as a variable for a callback. 
/// When it get called, it notifies all methods that reference the delegate. 
/// Anyone can subscribe to the service and they will receive the update at the right time automatically.
/// </summary>
public delegate void ModifiedEvent();

/// <summary>
/// Defines a  ModifiableInt
/// </summary>
[System.Serializable]
public class ModifiableInt {
    
    [SerializeField]
    private int baseAttributeValue;
    public int BaseValue { get { return baseAttributeValue; } set { baseAttributeValue = value; UpdateModifiedValue(); } } //run UpdateModifiedValue(), when the value gets set


    [SerializeField]
    private int modifiedValue;
    public int TotalAttributeValue { get { return modifiedValue; } set { modifiedValue = value; } }

    public List<IModifiers> modifiers = new List<IModifiers>();
    public event ModifiedEvent ValueModified;

    public ModifiableInt(ModifiedEvent method = null) {
        modifiedValue = baseAttributeValue;
        if (method != null) {
            ValueModified += method;
        }
    }

    /// <summary>
    /// Loop through all modifiers in list of modifiers, update the totalBuffValue and set the TotalAttributeValueOfPlayer.
    /// </summary>
    public void UpdateModifiedValue() {
        
        var totalBuffValue = 0;
        
        for (int i = 0; i < modifiers.Count; i++) {
            modifiers[i].AddValue(ref totalBuffValue);      //add the buffValue to the buffValue before
        }

        TotalAttributeValue = baseAttributeValue + totalBuffValue;

        if (ValueModified != null) {
            ValueModified.Invoke();
        }
    }

    /// <summary>
    /// Add the modifier to the list of modifiers
    /// </summary>
    /// <param name="_modifier">The modifier to add</param>
    public void AddModifier(IModifiers _modifier) {
        modifiers.Add(_modifier);
        UpdateModifiedValue();
    }

    /// <summary>
    /// Remove the modifier from the list of modifiers
    /// </summary>
    /// <param name="_modifier">The modifier to remove</param>
    public void RemoveModifier(IModifiers _modifier) {
        modifiers.Remove(_modifier);
        UpdateModifiedValue();
    }
}