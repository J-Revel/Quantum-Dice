using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DieEntanglementEffectConfig
{
    public IntricationMode entanglementMode;
    public GameObject particleEffects;
    public Color shaderColor;
}

public class DieStateDisplay : MonoBehaviour
{
    public List<int> intrications = new List<int>();
    public DieEntanglementEffectConfig[] effects;
    public string shaderColorParam = "_FXColor";
    public string alphaColorParam = "_Alpha";
    public new Renderer renderer;
    private MaterialPropertyBlock propertyBlock;

    private float effectTime = 0;
    public float colorChangeDuration = 0.5f;

    void Start()
    {
        propertyBlock = new MaterialPropertyBlock();
        renderer.GetPropertyBlock(propertyBlock);
    }

    void Update() {
        List<Color> activeColors = new List<Color>();
        for(int i=0; i<effects.Length; i++)
        {
            bool effectActive = false;
            // check if present in intrication group with the same entanglement mode as the displayed effect
            foreach(int intricationGroupIndex in intrications)
            {
                if(DiceManager.instance.intricationGroups[intricationGroupIndex].mode == effects[i].entanglementMode)
                {
                    // RemoveFromIntricationGroup(intricationGroupIndex);
                    effectActive = true;
                }
            }
            if(effects[i].particleEffects != null)
                effects[i].particleEffects.SetActive(effectActive);
            if(effectActive)
            {
                activeColors.Add(effects[i].shaderColor);
            }
        }
        float colorIndex = effectTime / colorChangeDuration;
        if(activeColors.Count > 0)
        {
            Color previousColor = activeColors[Mathf.FloorToInt(colorIndex) % activeColors.Count];
            Color nextColor = activeColors[Mathf.CeilToInt(colorIndex) % activeColors.Count];
            Color currentColor = Color.Lerp(previousColor, nextColor, Mathf.Repeat(colorIndex, 1));
            propertyBlock.SetColor(shaderColorParam, currentColor);
        }
        propertyBlock.SetFloat(alphaColorParam, activeColors.Count > 0 ? 1 : 0);
        
        renderer.SetPropertyBlock(propertyBlock);
        effectTime += Time.deltaTime;
    }
}
