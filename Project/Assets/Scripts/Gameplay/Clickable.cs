using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clickable : MonoBehaviour
{
    public System.Action clickedDelegate;
    public System.Action hoverStartDelegate;
    public System.Action hoverEndDelegate;

    public UnityEngine.Events.UnityEvent clickEvent;

    private void Start()
    {
        clickedDelegate += () => {clickEvent.Invoke();};
    }
}
