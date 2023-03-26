using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            bool verificationR1 = true; //rip le nom :(
            if (intricationMode.Equals(IntricationMode.Gregarious)) { 

                //noeud 1
                for(int i = 0; i < diceInGroup.Length; i++)
                {
                    //noeud 2
                    for (int j = 0; j < diceInGroup.Length; j++)
                    {
                        //regarde dans les groupe de type Opposite et Selfish
                        for(int k = 0; k < DiceManager.instance.intricationGroups.Length; k++)
                        {
                            if (DiceManager.instance.intricationGroups[k].mode.Equals(IntricationMode.Opposite) || DiceManager.instance.intricationGroups[k].mode.Equals(IntricationMode.Opposite))
                            {
                                verificationR1 &= !(DiceManager.instance.intricationGroups[k].diceIndex.Contains(i) && DiceManager.instance.intricationGroups[k].diceIndex.Contains(j));
                            }
                        }
                    }

                }

            }

            bool verificationR2 = true;
            if (intricationMode.Equals(IntricationMode.Gregarious))
            {

            }
            if (intricationMode.Equals(IntricationMode.Selfish)) { }

            bool verificationR3 = true; // regle qu'un des est li�e a qu'un seul opposite
            if (intricationMode.Equals(IntricationMode.Opposite))
            {
                for (int i = 0; i < diceInGroup.Length; i++)
                {
                    for (int j = 0; j < DiceManager.instance.intricationGroups.Length; j++)
                    {
                        if (DiceManager.instance.intricationGroups[j].mode.Equals(IntricationMode.Opposite) && j != currentGroupIndex)
                            verificationR3 &= !(DiceManager.instance.intricationGroups[j].diceIndex.Contains(currentGroupIndex));
                    }
                }
            }

            button.interactable = verificationR3 && verificationR1 && diceInGroup != null && diceInGroup.Length > 1 && MouseToolSelector.instance.currentQuantumEnergy >= MouseToolSelector.instance.energyCost;
        
        }
    }
}


/*private bool CheckGregariousAndOpposite(List<GraphNode> diceValues)
{
    // On commence par trouver les paires de d�s de m�me valeur
    var pairs = diceValues.GroupBy(d => d.state.value)
                          .Where(g => g.Count() >= 2)
                          .SelectMany(g => g.Combinations(2));

    // On v�rifie si chaque paire a un �l�ment dans l'autre paire
    foreach (var pair1 in pairs)
    {
        foreach (var pair2 in pairs)
        {
            if (pair1 != pair2 && pair1.Any(d => pair2.Any(d2 => d2 == d)))
            {
                return true;
            }
        }
    }

    return false;
}*/
