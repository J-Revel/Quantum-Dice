using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePanel : MonoBehaviour
{
    public TMPro.TextMeshPro bubbleText;
    public Transform bubbleTransform;
    public CanvasGroup overlayCanvasGroup;
    public float appearDuration = 0.5f;
    private int cursor = 0;
    
    IEnumerator Start()
    {
        // bubbleText.text = LocalizationSettings.StringDatabase.GetLocalizedString();
        for(float time = 0; time < appearDuration; time += Time.deltaTime)
        {
            overlayCanvasGroup.alpha = time / appearDuration;
            yield return null;
        }
        overlayCanvasGroup.alpha = 1;
    }

    void Update()
    {
        
    }
}
