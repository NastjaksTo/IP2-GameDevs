using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  Describes the buff of a item
/// </summary>
[System.Serializable]
public class ItemBuff : IModifiers {
    public Attributes attribute;                //attribute that have the buff
    public int buffValue;                       //buff value of the attribute

    /// <summary>
    /// Construtor to set the buff value for the item
    /// </summary>
    /// <param name="_buffValue">the value of the buff for the item</param>
    public ItemBuff(int _buffValue) {
        buffValue = _buffValue;
    }

    /// <summary>
    /// Add the buffvalue to the basevalue (the attribute value from the player before the buff from the item is applied) of the item
    /// </summary>
    /// <param name="baseValue">value of attribute before adding the buff</param>
    public void AddValue(ref int baseValue) {
        baseValue += buffValue;
    }

}