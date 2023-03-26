using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public enum DialogueType
{
    LevelIntro,
    BeforeThrowPhase,
    Info,
}

public class DialoguePanel : MonoBehaviour
{
    public static DialoguePanel instance;
    public TMPro.TextMeshProUGUI bubbleText;
    public Transform bubbleTransform;
    public CanvasGroup overlayCanvasGroup;
    public float appearDuration = 0.5f;
    public float bubbleScaleAnimDuration = 0.3f;
    private int cursor = 0;
    public LocalizedString locKey;
    private bool allowSkip;
    private bool skip;

    private void Awake()
    {
        instance = this;
    }

    public void PlayDialogue(DialogueType dialogueType)
    {
        StartCoroutine(DialogueCoroutine(dialogueType));
    }

    public void PlayInfoDialogue()
    {
        StartCoroutine(DialogueCoroutine(DialogueType.Info));
    }
    
    IEnumerator DialogueCoroutine(DialogueType dialogueType)
    {
        bubbleTransform.localScale = Vector3.zero;
        for(float time = 0; time < appearDuration; time += Time.deltaTime)
        {
            overlayCanvasGroup.alpha = time / appearDuration;
            yield return null;
        }
        overlayCanvasGroup.alpha = 1;
        LocalizedString[] dialogueEntries = null;
        switch(dialogueType)
        {
            case DialogueType.LevelIntro:
                dialogueEntries = DiceManager.instance.config.introDialogueEntries;
                break;
            case DialogueType.BeforeThrowPhase:
                dialogueEntries = DiceManager.instance.config.rollPhaseDialogueEntries;
                break;
            case DialogueType.Info:
                dialogueEntries = DiceManager.instance.config.infoDialogueEntries;
                break;
        }
        for(int i=0; i<dialogueEntries.Length; i++)
        {
            bubbleText.text = dialogueEntries[i].GetLocalizedString();

            
            allowSkip = true;
            for(float time = 0; time < bubbleScaleAnimDuration; time += Time.deltaTime)
            {
                float ratio = time / bubbleScaleAnimDuration;
                bubbleTransform.localScale = Vector3.one * (1 - (1-ratio) * (1-ratio));
                yield return null;
            }
            while(!skip)
            {
                yield return null;
            }
            allowSkip = false;
            for(float time = 0; time < bubbleScaleAnimDuration; time += Time.deltaTime)
            {
                float ratio = time / bubbleScaleAnimDuration;
                bubbleTransform.localScale = Vector3.one * (1 - ratio * ratio);
                yield return null;
            }
            skip = false;
        }
        for(float time = 0; time < appearDuration; time += Time.deltaTime)
        {
            overlayCanvasGroup.alpha = 1 - time / appearDuration;
            yield return null;
        }
        overlayCanvasGroup.alpha = 0;
    }

    public void SkipDialogue()
    {
        if(allowSkip)
            skip = true;
    }

    void Update()
    {
        
    }
}
