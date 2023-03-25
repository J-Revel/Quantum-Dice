using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ToolMode
{
    DragAndDrop,
    Entanglement,
}
public class MouseToolSelector : MonoBehaviour
{
    public static MouseToolSelector instance;
    public ToolMode currentTool;
    
    public System.Action toolChangedDelegate;

    public void Awake()
    {
        instance = this;
    }

    public void SelectTool(ToolMode toolMode)
    {
        currentTool = toolMode;
        toolChangedDelegate?.Invoke();
    }
    
}
