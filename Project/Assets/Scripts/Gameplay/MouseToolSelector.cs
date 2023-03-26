using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ToolMode
{
    Entanglement,
    DragAndDrop,
}
public class MouseToolSelector : MonoBehaviour
{
    public static MouseToolSelector instance;
    public ToolMode currentTool;
    public int intricationGroupIndex;

    public int currentQuantumEnergy;
    public int currentStepCount;
    public int maxStepCount;

    public IntricationMode intricationToolMode {
        get
        {
            if(intricationGroupIndex < 0)
                return IntricationMode.None;
            return DiceManager.instance.intricationGroups[intricationGroupIndex].mode;
        }

    }

    
    public System.Action toolChangedDelegate;

    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        DialoguePanel.instance.PlayDialogue(DialogueType.LevelIntro);
        currentQuantumEnergy = DiceManager.instance.config.quantumEnergy;
        currentStepCount = 0;
    }

    public void SelectTool(ToolMode toolMode)
    {
        if(currentTool != toolMode)
        {
            currentTool = toolMode;
            if(toolMode == ToolMode.DragAndDrop)
                DialoguePanel.instance.PlayDialogue(DialogueType.BeforeThrowPhase);
        }
        toolChangedDelegate?.Invoke();
    }
    
}
