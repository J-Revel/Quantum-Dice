using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class EntanglementPostprocessManager : MonoBehaviour
{
    public float animDuration = 0.5f;
    public bool active = false;
    public ColorAdjustments colorAdjustments;
    public VolumeProfile profile;
    public float targetValue = -100;
    public float time = 0;
    IEnumerator Start()
    {
        profile.TryGet<ColorAdjustments>(out colorAdjustments);
        MouseToolSelector.instance.toolChangedDelegate += () => {
            active = (MouseToolSelector.instance.currentTool == ToolMode.Entanglement && MouseToolSelector.instance.intricationToolMode != IntricationMode.None);
        };
        while(true)
        {
            if(active)
                time += Time.deltaTime;
            else time -= Time.deltaTime;
            time = Mathf.Clamp(time, 0, animDuration);
            colorAdjustments.saturation.SetValue(new FloatParameter(time / animDuration * targetValue));
            yield return null;
        }
    }
}
