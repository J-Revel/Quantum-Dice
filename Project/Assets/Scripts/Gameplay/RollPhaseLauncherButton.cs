using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RollPhaseLauncherButton : MonoBehaviour
{
    private Button button;
    public float transitionDuration = 1;
    public Transform targetCameraPosition;
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => {
            StartCoroutine(PhaseTransitionCoroutine());
        });
        MouseToolSelector.instance.toolChangedDelegate += () => {
            if(MouseToolSelector.instance.currentTool == ToolMode.Entanglement && MouseToolSelector.instance.intricationToolMode == IntricationMode.None)
            {
                gameObject.SetActive(true);
            }
            else gameObject.SetActive(false);
        };
    }

    IEnumerator PhaseTransitionCoroutine()
    {
        Vector3 startPosition = Camera.main.transform.position;
        for(float time = 0; time < transitionDuration; time += Time.deltaTime)
        {
            Camera.main.transform.position = Vector3.Lerp(startPosition, targetCameraPosition.position, time / transitionDuration);
            yield return null;
        }
        Camera.main.transform.position = targetCameraPosition.position;
        MouseToolSelector.instance.SelectTool(ToolMode.DragAndDrop);
        DiceManager.instance.ValidateGraph();
    }

    void Update()
    {
        
    }
}
