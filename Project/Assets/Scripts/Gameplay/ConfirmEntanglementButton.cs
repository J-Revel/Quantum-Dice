using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmEntanglementButton : MonoBehaviour
{
    private Button button;
    public GameObject toShow;
    public bool cancel;
    
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => {
            if(cancel)
            {
                DiceManager.instance.RemoveIntricationGroup(MouseToolSelector.instance.intricationGroupIndex);
            }
            else
            {
                MouseToolSelector.instance.currentQuantumEnergy -= MouseToolSelector.instance.energyCost;
            }
            MouseToolSelector.instance.intricationGroupIndex = -1;
            MouseToolSelector.instance.SelectTool(ToolMode.Entanglement);
            gameObject.SetActive(false);
        });
        MouseToolSelector.instance.toolChangedDelegate += () => {
            if(MouseToolSelector.instance.currentTool == ToolMode.Entanglement && MouseToolSelector.instance.intricationToolMode != IntricationMode.None)
            {
                gameObject.SetActive(true);
                if(toShow != null)
                    toShow.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
                if(toShow != null)
                    toShow.SetActive(false);
            }
        };
        gameObject.SetActive(false);
    }
    
    void Update()
    {
        if(!cancel)
        {
            int currentGroupIndex = MouseToolSelector.instance.intricationGroupIndex;
            int[] diceInGroup = DiceManager.instance.intricationGroups[currentGroupIndex].diceIndex;
            IntricationMode intricationMode = MouseToolSelector.instance.intricationToolMode;
            button.interactable = diceInGroup != null && diceInGroup.Length > 1 && MouseToolSelector.instance.currentQuantumEnergy >= MouseToolSelector.instance.energyCost;
        }
    }
}
