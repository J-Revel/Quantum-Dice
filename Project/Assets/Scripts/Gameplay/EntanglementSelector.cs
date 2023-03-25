using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntanglementSelector : MonoBehaviour
{
    private Clickable clickable;
    public ToolMode toolMode;

    void Start()
    {
        clickable = GetComponent<Clickable>();
        clickable.clickedDelegate += () => { 
            MouseToolSelector.instance.SelectTool(toolMode); 
        };
    }
}
