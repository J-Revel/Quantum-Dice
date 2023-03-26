using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitationDisplay : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;

    void Start()
    {
        
    }

    void Update()
    {
        text.text = DiceManager.instance.config.citation.GetLocalizedString();
    }
}
