using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct LinkInstance {
    public int entanglementIndex;
    public LinkVFX[] linkInstances;
}

[System.Serializable]
public struct EntanglementLinkConfig {

    public IntricationMode mode;
    public LinkProceduralAnimationConfig config;
}

public class EntanglementLinkDisplay : MonoBehaviour
{
    public LinkVFX linkVFXPrefab;
    private LinkInstance[] links = new LinkInstance[0];
    public EntanglementLinkConfig[] configs;
    private void Update()
    {
        if(links.Length != DiceManager.instance.intricationGroups.Length)
        {
            LinkInstance[] newLinks = new LinkInstance[DiceManager.instance.intricationGroups.Length];
            for(int i=0; i<Mathf.Min(links.Length, DiceManager.instance.intricationGroups.Length); i++)
            {
                newLinks[i] = links[i];
            }
            for(int i=Mathf.Min(links.Length, DiceManager.instance.intricationGroups.Length); i<links.Length; i++)
            {
                for(int j=0; j<links[i].linkInstances.Length; j++)
                {
                    Destroy(links[i].linkInstances[j].gameObject);
                }
            }
            links = newLinks;
        }
        for(int i=0; i<links.Length; i++)
        {
            int diceCount = 0;
            if(DiceManager.instance.intricationGroups[i].diceIndex != null)
                diceCount = DiceManager.instance.intricationGroups[i].diceIndex.Length;
            int linkCount = diceCount * (diceCount - 1) / 2;
            if((links[i].linkInstances == null && linkCount != 0) || (links[i].linkInstances != null && links[i].linkInstances.Length != linkCount))
            {
                LinkVFX[] oldInstances = links[i].linkInstances;
                if(oldInstances == null)
                    oldInstances = new LinkVFX[0];
                LinkVFX[] newInstances = new LinkVFX[linkCount];
                for(int j=0; j<Mathf.Min(oldInstances.Length, newInstances.Length); j++)
                {
                    newInstances[j] = oldInstances[j];
                }
                for(int j=newInstances.Length; j<oldInstances.Length; j++)
                {
                    Destroy(oldInstances[j].gameObject);
                }
                for(int j=oldInstances.Length; j<newInstances.Length; j++)
                {
                    LinkVFX newInstance = Instantiate(linkVFXPrefab);
                    newInstances[j] = newInstance;
                }
                links[i].linkInstances = newInstances;
            }
            int[] diceIndex = DiceManager.instance.intricationGroups[i].diceIndex;
            if(diceIndex != null)
            {
                int linkIndex = 0;
                for(int j=0; j<diceIndex.Length; j++)
                {
                    for(int k=j+1; k<diceIndex.Length; k++)
                    {
                        int dice1 = diceIndex[j];
                        int dice2 = diceIndex[k];
                        foreach(var config in configs)
                        {
                            if(config.mode ==  DiceManager.instance.intricationGroups[i].mode)
                                    links[i].linkInstances[linkIndex].config = config.config;
                        }
                        links[i].linkInstances[linkIndex].originOffset = DieStateDisplay.dicePositions[dice1];
                        links[i].linkInstances[linkIndex].targetOffset = DieStateDisplay.dicePositions[dice2];
                        linkIndex++;
                    }
                }
            }
        }
    }
}
