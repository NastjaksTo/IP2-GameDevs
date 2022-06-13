using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SkillTree;

public class Fire1 : MonoBehaviour
{
    public float damage;

    private void Awake()
    {
        damage = 50 * (1 + skillTree.skillLevels[0]);
    }
}
