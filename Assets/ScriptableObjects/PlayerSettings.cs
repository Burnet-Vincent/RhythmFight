using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "ScriptableObjects/PlayerSettingsScriptableObject", order = 2)]
public class PlayerSettings : ScriptableObject
{
    [Header("Generals")]
    public int maxHealth = 3;
    public int stepSize = 1;

    [Header("Rhythm")]
    public float timeForPerfect = .15f;
    public float timeForGood = .3f;
}
