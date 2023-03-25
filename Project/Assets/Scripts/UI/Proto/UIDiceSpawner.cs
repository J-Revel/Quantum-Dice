using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDiceSpawner : MonoBehaviour
{
    public UIDiceButton diePrefab;
    private UIDiceButton[] diceButtons;

    public void Start()
    {
        int length = DiceManager.instance.dice.Length;
        diceButtons = new UIDiceButton[length];
        for(int i=0; i<length; i++)
        {
            diceButtons[i] = Instantiate(diePrefab, transform);
            diceButtons[i].dieIndex = i;
        }
    }
}
