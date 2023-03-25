using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDiceButton : MonoBehaviour
{
    public Button button;
    public TMPro.TextMeshProUGUI text;
    public int dieIndex = 0;
    private int displayedValue = 0;
    public float rollAnimDuration = 0.5f;

    public void Update()
    {
        if(displayedValue != DiceManager.instance.dice[dieIndex].value)
        {
            StartCoroutine(RollAnimCoroutine());
            displayedValue = DiceManager.instance.dice[dieIndex].value;
        }
    }

    public void Roll()
    {
        DiceManager.instance.RollDice(dieIndex);
    }

    private IEnumerator RollAnimCoroutine()
    {
        for(float time=0; time < rollAnimDuration; time += Time.deltaTime)
        {
            text.text = Random.Range(1, 7).ToString();
            yield return null;
        }
        text.text = DiceManager.instance.dice[dieIndex].value.ToString();
    }
}
