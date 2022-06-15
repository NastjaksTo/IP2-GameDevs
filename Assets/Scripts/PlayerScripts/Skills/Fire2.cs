using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SkillTree;

public class Fire2 : MonoBehaviour
{
    public float damage;

    private void Awake()
    {
        damage = 100 * (1 + skillTree.skillLevels[6]);
    }
}
