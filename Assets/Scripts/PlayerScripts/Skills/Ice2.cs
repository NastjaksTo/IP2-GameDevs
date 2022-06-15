using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SkillTree;

public class Ice2 : MonoBehaviour
{
    public float damage;

    private void Awake()
    {
        damage = 25 * (1 + skillTree.skillLevels[7]);
    }
}
