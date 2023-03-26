using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepCountDisplay : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;

    void Start()
    {
        MouseToolSelector.instance.toolChangedDelegate += () => {
            gameObject.SetActive(MouseToolSelector.instance.currentTool == ToolMode.DragAndDrop);
        };
    }

    void Update()
    {
        text.text = MouseToolSelector.instance.currentStepCount + "/" + DiceManager.instance.config.targetThrowCount;
    }
}
