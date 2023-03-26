using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;


[System.Serializable]
public enum VictoryConditionType
{
    NSupSum, // n des doit etre superieure ou egale e un certain seuil OK
    AllDifferentValues, // chaque de doit avoir une valeur different entre elle, n= All OK
    NAllSame, // N des doivent avoir la meme valeur OK
    MixedValues, // n des doivent avoir une valeur specifique, tandis que le reste doit avoir une valeur differente
    PairValues, //n des doivent etre impair OK
    ImpairValues, //n des doivent etre paire OK
    NSumEqual //n des doivent avoir comme somme = Value OK
}

[System.Serializable]
public struct VictoryCondition
{
    public VictoryConditionType vc;
    public int N;
    public int value;
}

[CreateAssetMenu()]
public class LevelConfig : ScriptableObject
{
    public int diceCount = 5; //nombre des disponible

    public int quantumEnergy = 0;

    public int targetThrowCount = 0;

    public List<VictoryCondition> conditions;

    public LocalizedString[] introDialogueEntries;
    public LocalizedString[] rollPhaseDialogueEntries;
    public LocalizedString[] infoDialogueEntries;
    public LocalizedString citation;
}
