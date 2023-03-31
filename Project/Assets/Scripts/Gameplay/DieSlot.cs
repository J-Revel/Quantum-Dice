using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieSlot : MonoBehaviour
{
    public Clickable entanglementClickable;
    public RollableDie die;
    public DieStateDisplay[] diceDisplays;

    public int dieIndex { get {return die.dieIndex;} set { die.dieIndex = value; }}

    void Start()
    {
        MouseToolSelector.instance.toolChangedDelegate += OnToolChanged;
        entanglementClickable.clickedDelegate += OnClicked;
        foreach(DieStateDisplay display in diceDisplays)
        {
            display.dieIndex = dieIndex;
        }
    }

    void Update()
    {
        if(die.attracted)
        {
            die.transform.SetParent(null);
            Destroy(gameObject);
        }
    }

    private void OnToolChanged()
    {
        switch(MouseToolSelector.instance.currentTool)
        {
            case ToolMode.DragAndDrop:
                entanglementClickable.gameObject.SetActive(false);
                die.gameObject.SetActive(true);
                break;
            case ToolMode.Entanglement:
                if(MouseToolSelector.instance.intricationToolMode != IntricationMode.None)
                {
                    entanglementClickable.gameObject.SetActive(true);
                    die.gameObject.SetActive(false);
                }
                else 
                {
                    entanglementClickable.gameObject.SetActive(false);
                    die.gameObject.SetActive(true);
                }
                break;
        }
    }

    private void OnClicked()
    {
        IntricationGroup intricationGroup = DiceManager.instance.intricationGroups[MouseToolSelector.instance.intricationGroupIndex];
        bool isInGroup = false;
        if(intricationGroup.diceIndex == null)
            intricationGroup.diceIndex = new int[0];
        for(int i=0; i<intricationGroup.diceIndex.Length; i++)
        {
            if(dieIndex == intricationGroup.diceIndex[i])
                isInGroup = true;
        }
        if(isInGroup)
        {
            DiceManager.instance.RemoveFromIntricationGroup(MouseToolSelector.instance.intricationGroupIndex, dieIndex);
        }
        else
        {
            DiceManager.instance.AddToIntricationGroup(MouseToolSelector.instance.intricationGroupIndex, dieIndex);
        }
        // foreach(DieStateDisplay stateDisplay in diceDisplays)
        // {
        //     if(isInGroup)
        //     {
        //         stateDisplay.intrications.Remove(MouseToolSelector.instance.intricationGroupIndex);
        //     }
        //     else
        //     {
        //         stateDisplay.intrications.Add(MouseToolSelector.instance.intricationGroupIndex);
        //     }
        // }
        
    }
}
