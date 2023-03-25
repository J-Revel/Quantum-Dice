using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateHighlight : MonoBehaviour
{
    public GameObject toActivate;
    private Clickable clickable;

    void Start()
    {
        clickable = GetComponent<Clickable>();
        clickable.hoverStartDelegate += HoverStart;
        clickable.hoverEndDelegate += HoverEnd;
    }

    public void HoverStart()
    {
        toActivate.gameObject.SetActive(true);
    }

    public void HoverEnd()
    {
        toActivate.gameObject.SetActive(false);
    }
}
