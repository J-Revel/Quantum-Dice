using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyCostDisplay : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;

    void Start()
    {
        MouseToolSelector.instance.toolChangedDelegate += OnToolChanged;
    }

    void Update()
    {
        if(DiceManager.instance.intricationGroups.Length > 0)
        {
            int[] group = DiceManager.instance.intricationGroups[DiceManager.instance.intricationGroups.Length-1].diceIndex;
            if(group == null)
                MouseToolSelector.instance.energyCost = 0;
            else
                MouseToolSelector.instance.energyCost = Mathf.Max(0, group.Length - 1);
            text.text = MouseToolSelector.instance.energyCost.ToString();
        }
    }

    private void OnToolChanged()
    {
        if(MouseToolSelector.instance.currentTool == ToolMode.Entanglement && MouseToolSelector.instance.intricationToolMode != IntricationMode.None)
        {
            gameObject.SetActive(true);
        }
        else gameObject.SetActive(false);
    }
}
