using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public int[] diceIndex;
}

public class DiceManager : MonoBehaviour
{
    public static DiceManager instance;
    public LevelConfig config;
    public DiceState[] dice;
    public IntricationGroup[] intricationGroups;
    public System.Action diceRollDelegate;

    public void Awake()
    {
        instance = this;
        dice = new DiceState[config.diceCount];
        for(int i=0; i<config.diceCount; i++)
            dice[i].value = 0;
    }
    
    public void RollDice(int diceIndex)
    {
        //liste des valeurs possible
        int[] valuesPossible = { 1, 2, 3, 4, 5, 6 };
        
        //parcourir l'ensemble des groupe d'intrication
        for (int i = 0; i < intricationGroups.Length; i++ )
        {
            int[] dicesIndex = intricationGroups[i].diceIndex;

            //verifier s'il est contenue dans la liste diceIndex

            if (Array.IndexOf(dicesIndex, diceIndex) != -1)
            {

                int[] dicesIndexCopy = dicesIndex.Except(new int[] { diceIndex }).ToArray();

                if (intricationGroups[i].mode == IntricationMode.Selfish)
                {
                    for (int k = 0; k < dicesIndexCopy.Length; k++)
                    {
                        valuesPossible = valuesPossible.Except(new int[] { k }).ToArray();
                    }
                }

                if (intricationGroups[i].mode == IntricationMode.Gregarious)
                {
                    for (int k = 0; k < dicesIndexCopy.Length; k++) // a achanger pour une boucle while
                    {
                        if(dicesIndexCopy[k] != 0)
                        {
                            dice[diceIndex].value = dicesIndexCopy[k];
                        }
                    }
                }
                if (intricationGroups[i].mode == IntricationMode.Opposite)
                {
                    for (int k = 0; k < dicesIndexCopy.Length; k++)
                    {
                        int tmp = 7 - dicesIndexCopy[k];
                        //valuesPossible = {tmp};
                    }
                }
            }
        }


        //dice[diceIndex].value = System.Random.Range(1, 7);

        diceRollDelegate?.Invoke();
    }
}
