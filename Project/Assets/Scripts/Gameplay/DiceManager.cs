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
        List<int> valuesPossible = new List<int> { 1, 2, 3, 4, 5, 6 };
        Debug.Log($"Lancer du dés {diceIndex} !");
        //parcourir l'ensemble des groupe d'intrication
    
        for (int i = 0; i < intricationGroups.Length; i++ )
        {
            int[] dicesIndex = intricationGroups[i].diceIndex;

            //verifier s'il est contenue dans la liste diceIndex
            Debug.Log($"Groupe {i} !");

            if (Array.IndexOf(dicesIndex, diceIndex) != -1)
            {
                Debug.Log($"Dés {diceIndex} appartient au groupe {i}");
                
                string dicesIndex1 = "List dicesIndex before: ";
                foreach (var item in dicesIndex)
                {
                    dicesIndex1 += item.ToString() + ", ";
                }
                Debug.Log(dicesIndex1);

                int[] dicesIndexCopy = dicesIndex.Except(new int[] { diceIndex }).ToArray();

                string dicesIndex2 = "List dicesIndexCopy before: ";
                foreach (var item in dicesIndexCopy)
                {
                    dicesIndex2 += item.ToString() + ", ";
                }
                Debug.Log(dicesIndex2);

                if (intricationGroups[i].mode == IntricationMode.Selfish)
                {
                    for (int k = 0; k < dicesIndexCopy.Length; k++)
                    {
                        valuesPossible.Remove(k);
                    }
                }

                if (intricationGroups[i].mode == IntricationMode.Gregarious)
                {
                    Debug.Log($"group Greagrious");
                    string result1 = "List valuesPossible before: ";
                    foreach (var item in valuesPossible)
                    {
                        result1 += item.ToString() + ", ";
                    }
                    Debug.Log(result1);

                    for (int k = 0; k < dicesIndexCopy.Length; k++) // a achanger pour une boucle while
                    {
                        //Debug.Log($"dicesIndexCopy[{k}] = {dicesIndexCopy[k]}");
                        if(dice[dicesIndexCopy[k]].value != 0)
                        {
                            valuesPossible = new List<int> { dice[dicesIndexCopy[k]].value };
                        }
                    }

                    string result2 = "List valuesPossible after: ";
                    foreach (var item in valuesPossible)
                    {
                        result2 += item.ToString() + ", ";
                    }
                    Debug.Log(result2);
                    //Debug.Log(valuesPossible);
                }
                if (intricationGroups[i].mode == IntricationMode.Opposite)
                {
                    Debug.Log($"group Opposite");

                    for (int k = 0; k < dicesIndexCopy.Length; k++)
                    {
                        int tmp = 7 - dicesIndexCopy[k];
                            valuesPossible = new List<int> { tmp };
                    }
                }
            }
        }
        System.Random random = new System.Random();
        int index = random.Next(valuesPossible.Count);/**/

        //int indexValue = System.Random Range(0, valuesPossible.Count);
        dice[diceIndex].value = valuesPossible[index];

        diceRollDelegate?.Invoke();
    }
}
