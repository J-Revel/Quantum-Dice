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
    public List<IntricationMode> intrications = new List<IntricationMode>();
    public DieEntanglementEffectConfig[] effects;
    public string shaderColorParam = "_FXColor";
    public Renderer renderer;
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
            bool effectActive = intrications.Contains(effects[i].entanglementMode);
            effects[i].particleEffects.SetActive(effectActive);
            if(effectActive)
            {
                activeColors.Add(effects[i].shaderColor);
            }
            
        }
        float colorIndex = effectTime / colorChangeDuration;
        Color previousColor = activeColors[Mathf.FloorToInt(colorIndex) % activeColors.Count];
        Color nextColor = activeColors[Mathf.CeilToInt(colorIndex) % activeColors.Count];
        Color currentColor = Color.Lerp(previousColor, nextColor, Mathf.Repeat(colorIndex, 1));
        propertyBlock.SetColor(shaderColorParam, currentColor);
        
        renderer.SetPropertyBlock(propertyBlock);
    }
}
