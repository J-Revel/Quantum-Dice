using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmEntanglementButton : MonoBehaviour
{
    private Button button;
    
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener( () => {
            MouseToolSelector.instance.intricationGroupIndex = -1;
            MouseToolSelector.instance.SelectTool(ToolMode.Entanglement);
            gameObject.SetActive(false);
        });
        MouseToolSelector.instance.toolChangedDelegate += () => {
            if(MouseToolSelector.instance.currentTool == ToolMode.Entanglement && MouseToolSelector.instance.intricationToolMode != IntricationMode.None)
            {
                gameObject.SetActive(true);
            }
            else gameObject.SetActive(false);
        };
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
