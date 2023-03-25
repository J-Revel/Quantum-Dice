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
    Gregarious, // same value
    Opposite, // 2 dice only => Opposite values : result sum = 7 (1-6, 2-5, 3-4)
    Selfish, // All values are different
}

[System.Serializable]
public struct IntricationGroup
{
    public IntricationMode mode;
    int[] diceIndex;
}

public class DiceManager : MonoBehaviour
{
    public LevelConfig config;
    public DiceState[] dice;
    public IntricationGroup[] intricationGroups;
    public System.Action diceRollDelegate;

    public void Start()
    {
        dice = new DiceState[config.diceCount];
        for(int i=0; i<config.diceCount; i++)
            dice[i].value = 0;
    }
    public void RollDice(int diceIndex)
    {
        dice[diceIndex].value = Random.Range(1, 7);
        diceRollDelegate?.Invoke();
    }
}
