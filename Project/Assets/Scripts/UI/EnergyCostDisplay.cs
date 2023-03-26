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

    // Update is called once per frame
    void Update()
    {
        text.text = MouseToolSelector.instance.energyCost.ToString();
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
