using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuantumEnergyDisplay : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;

    void Start()
    {
        
    }

    void Update()
    {
        text.text = MouseToolSelector.instance.currentQuantumEnergy + "/" + DiceManager.instance.config.quantumEnergy;
    }
}
