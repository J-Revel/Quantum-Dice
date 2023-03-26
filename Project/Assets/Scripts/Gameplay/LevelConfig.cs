using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;


[System.Serializable]
public enum VictoryConditionType
{
    NSupSum, // n d�s doit �tre sup�rieure ou �gale � un certain seuil
    AllDifferentValues, // chaque d� doit avoir une valeur different entre elle, n= All
    NAllSame, // N d�s doivent avoir la m�me valeur
    MixedValues, // n d�s doivent avoir une valeur sp�cifique, tandis que le reste doit avoir une valeur diff�rente
    PairValues, //n des doivent etre impair
    ImpairValues //n des doivent etre paire
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
