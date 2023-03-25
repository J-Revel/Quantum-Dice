using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieSlot : MonoBehaviour
{
    public Clickable entanglementClickable;
    public RollableDie die;

    public int dieIndex { get {return die.dieIndex;} set { die.dieIndex = value; }}

    void Start()
    {
        MouseToolSelector.instance.toolChangedDelegate += OnToolChanged;
    }

    public void StartEntanglementTool()
    {
        
    }

    public void EndEntanglementTool()
    {
        
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
}
