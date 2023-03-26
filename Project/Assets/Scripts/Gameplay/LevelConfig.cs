using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public enum VictoryConditionType
{
    NSupSum, // n dés doit être supérieure ou égale à un certain seuil
    AllDifferentValues, // chaque dé doit avoir une valeur different entre elle, n= All
    NAllSame, // N dés doivent avoir la même valeur
    MixedValues, // n dés doivent avoir une valeur spécifique, tandis que le reste doit avoir une valeur différente
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
    public int diceCount = 5; //nombre dé disponible

    public int QuantumEnergy = 0;

    public int throwsDisable = 0;

    public List<VictoryCondition> conditions;

}
