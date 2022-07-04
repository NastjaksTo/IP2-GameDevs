using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines a IModifiere
/// </summary>
public interface IModifiers {

    /// <summary>
    /// Add the baseBuffValue to the reference value and set the reference value to the result.
    /// </summary>
    /// <param name="baseBuffValue">Total value of the buffs for the attribute before the new buff was added.</param> 
    void AddValue(ref int baseBuffValue);
}
