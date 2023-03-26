using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntanglementSelector : MonoBehaviour
{
    private Clickable clickable;
    public ToolMode toolMode;
    public IntricationMode intricationMode;

    void Start()
    {
        bool available = false;
        foreach(IntricationMode mode in DiceManager.instance.config.availableEntanglements)
        {
            if(mode == intricationMode)
                available = true;
        }
        gameObject.SetActive(available);
        clickable = GetComponent<Clickable>();
        clickable.clickedDelegate += () => { 
            IntricationGroup[] oldGroups = DiceManager.instance.intricationGroups;
            IntricationGroup[] newGroups = new IntricationGroup[oldGroups.Length + 1];
            for(int i=0; i<oldGroups.Length; i++)
                newGroups[i] = oldGroups[i];
            newGroups[oldGroups.Length] = new IntricationGroup();
            newGroups[oldGroups.Length].mode = intricationMode;
            DiceManager.instance.intricationGroups = newGroups;
            MouseToolSelector.instance.intricationGroupIndex = oldGroups.Length;
            MouseToolSelector.instance.SelectTool(toolMode);
        };
    }
}
