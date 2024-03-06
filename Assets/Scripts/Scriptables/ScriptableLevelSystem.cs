using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Scriptable Level System", menuName = "Level System")]
public class ScriptableLevelSystem : ScriptableObject
{
    public List<Level> levels;

    private void OnValidate()
    {
        if (levels == null || levels.Count == 0) return;

        for (int i = 0; i < levels.Count; i++)
        {
            levels[i].lvl = i + 2;
            levels[i].name = "Level " + (i + 2);

            if (levels[i].MeleeAttackUnlocked != null && levels[i].MagicAttackUnlocked == null)
            {
                Debug.LogWarning("Level " + (i + 2) + " has a melee attack but no magic attack. Please add a magic attack to this level.");
            }
            else if (levels[i].MeleeAttackUnlocked != null && levels[i].MagicAttackUnlocked == null)
            {
                Debug.LogWarning("Level " + (i + 2) + " has a magic attack but no melee attack. Please add a melee attack to this level.");
            }
        }
    }
}

[Serializable]
public class Level
{
    [HideInInspector] public string name;
    [HideInInspector] public int lvl;
    public int XPToReachLevel;
    public ScriptableMeleeAttack MeleeAttackUnlocked;
    public ScriptableMagicAttack MagicAttackUnlocked;
}

