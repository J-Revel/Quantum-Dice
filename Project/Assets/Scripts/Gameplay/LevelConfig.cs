using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class LevelConfig : ScriptableObject
{
    public int diceCount = 5;
    public string[] introDialogueLocKeys;
}
