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
                entanglementClickable.gameObject.SetActive(true);
                die.gameObject.SetActive(false);
                break;
        }
    }

    private void OnClicked()
    {
        if(diceDisplays[0].intrications.Contains(MouseToolSelector.instance.intricationGroupIndex))
        {
            MouseToolSelector.instance.currentQuantumEnergy--;
            DiceManager.instance.RemoveFromIntricationGroup(MouseToolSelector.instance.intricationGroupIndex, dieIndex);
        }
        else
        {
            MouseToolSelector.instance.currentQuantumEnergy++;
            DiceManager.instance.AddToIntricationGroup(MouseToolSelector.instance.intricationGroupIndex, dieIndex);
        }
        foreach(DieStateDisplay stateDisplay in diceDisplays)
        {
            if(stateDisplay.intrications.Contains(MouseToolSelector.instance.intricationGroupIndex))
            {
                stateDisplay.intrications.Remove(MouseToolSelector.instance.intricationGroupIndex);
            }
            else
            {
                stateDisplay.intrications.Add(MouseToolSelector.instance.intricationGroupIndex);
            }
        }
        
    }
}
