using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu()]
public class LevelConfig : ScriptableObject
{
    public int diceCount = 5;
    public LocalizedString[] introDialogueEntries;
    public LocalizedString[] rollPhaseDialogueEntries;
    public LocalizedString[] infoDialogueEntries;
}
