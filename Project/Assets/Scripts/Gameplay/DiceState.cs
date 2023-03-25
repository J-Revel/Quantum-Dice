using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DiceState
{
    public int value; // 0 = not rolled, 1->6 = face visible
}

public enum IntricationMode
{
    Gregarious,
    Opposite,
    Selfish,
}

[System.Serializable]
public struct IntricationGroup
{
    public IntricationMode mode;
    int[] diceIndex;
}

public class DiceManager : MonoBehaviour
{
    public DiceState[] dice;
    public IntricationGroup[] intricationGroups;

    public void RollDice(int diceIndex)
    {
        dice[diceIndex].value = Random.Range(1, 7);
    }
}
