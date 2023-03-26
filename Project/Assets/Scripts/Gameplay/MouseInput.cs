using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class MouseInput : MonoBehaviour
{
    public LayerMask raycastLayer;
    public LayerMask groundLayer;
    private RollableDie hoverDie;
    private Clickable hoveredClickable;
    private bool grabbing;
    public float grabHoverHeight = 2;
    public float rotationImpulse = 1;
    public float verticalImpulse = 2;
    public float rotationOffsetDuration = 1;
    private Dictionary<RollableDie, Coroutine> coroutines = new Dictionary<RollableDie, Coroutine>();
    
    void Start()
    {
        
    }

    void Update()
    {
        switch(MouseToolSelector.instance.currentTool)
        {
            case ToolMode.DragAndDrop:
                HandleDragAndDrop();
                break;
            case ToolMode.Entanglement:
                HandleEntanglement();
                break;
        }
    }

    public void HandleDragAndDrop()
    {
        if(grabbing)
        {
            if(Mouse.current.leftButton.wasReleasedThisFrame)
            {
                Vector3[] storedPositions = new Vector3[Mathf.RoundToInt(5.0f / Time.fixedDeltaTime) + 1];
                Quaternion[] storedRotations = new Quaternion[Mathf.RoundToInt(5.0f / Time.fixedDeltaTime) + 1];
                grabbing = false;
                hoverDie.attracted = false;
                if(hoverDie != null)
                    hoverDie.hoverElement.SetActive(false);
                Physics.autoSimulation = false;
                int cursor = 0;
                hoverDie.rigidbody.AddTorque(hoverDie.transform.InverseTransformDirection(hoverDie.rigidbody.velocity).normalized * rotationImpulse, ForceMode.Acceleration);
                hoverDie.rigidbody.AddForce(Vector3.up * verticalImpulse, ForceMode.Acceleration);
                for(float t=0; t<5; t += Time.fixedDeltaTime)
                {
                    hoverDie.FixedUpdate();
                    Physics.Simulate(Time.fixedDeltaTime);
                    Physics.SyncTransforms();
                    storedPositions[cursor] = hoverDie.transform.position;
                    storedRotations[cursor] = hoverDie.transform.rotation;
                    cursor++;
                }
                DiceManager.instance.RollDice(hoverDie.dieIndex);
                int targetValue = DiceManager.instance.dice[hoverDie.dieIndex].value-1;
                coroutines[hoverDie] = StartCoroutine(ReplayAnimCoroutine(hoverDie, storedPositions, storedRotations, targetValue));

                hoverDie = null;
                Physics.autoSimulation = true;
            }
        }
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 1000, raycastLayer))
        {
            if(grabbing)
            {
            }
            else
            {
                RollableDie die = hit.collider.GetComponent<RollableDie>();
                Clickable clickable = hit.collider.GetComponent<Clickable>();
                if(die != null)
                {
                    if(hoveredClickable != null)
                    {
                        hoveredClickable.hoverEndDelegate?.Invoke();
                        hoveredClickable = null;
                    }
                    if(die != hoverDie)
                    {
                        die.hoverElement.SetActive(true);
                        if(hoverDie != null)
                            hoverDie.hoverElement.SetActive(false);
                        hoverDie = die;
                    }
                }
                else if(clickable != null)
                {
                    if(hoveredClickable != null)
                    {
                        hoveredClickable.hoverEndDelegate?.Invoke();
                    }
                    hoveredClickable = clickable;
                    hoveredClickable.hoverStartDelegate?.Invoke();
                }
                if(die == null && hoverDie != null)
                {
                    hoverDie.hoverElement.SetActive(false);
                    hoverDie = null;        
                }
                if(clickable == null && hoveredClickable != null)
                {
                    hoveredClickable.hoverEndDelegate?.Invoke();
                    hoveredClickable = null;
                }
                if(Mouse.current.leftButton.wasPressedThisFrame)
                {
                    if(hoverDie != null)
                    {
                        if(coroutines.ContainsKey(hoverDie) && coroutines[hoverDie] != null)
                        {
                            StopCoroutine(coroutines[hoverDie]);
                        }
                        grabbing = true;
                        hoverDie.rigidbody.isKinematic = false;
                    }
                    else if(hoveredClickable != null)
                    {
                        hoveredClickable.clickedDelegate?.Invoke();
                    }
                }
            }
        }
        else
        {
            if(!grabbing && hoverDie != null)
            {
                hoverDie.hoverElement.SetActive(false);
                hoverDie = null;        
            }
            if(hoveredClickable != null)
            {
                hoveredClickable.hoverEndDelegate?.Invoke();
                hoveredClickable = null;
            }
        }
    }

    public void HandleEntanglement()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 1000, raycastLayer))
        {
            Clickable clickable = hit.collider.GetComponent<Clickable>();
            if(clickable != null)
            {
                if(hoveredClickable != null)
                {
                    hoveredClickable.hoverEndDelegate?.Invoke();
                }
                hoveredClickable = clickable;
                hoveredClickable.hoverStartDelegate?.Invoke();
            }
            if(clickable == null && hoveredClickable != null)
            {
                hoveredClickable.hoverEndDelegate?.Invoke();
                hoveredClickable = null;
            }
            if(Mouse.current.leftButton.wasPressedThisFrame)
            {
                if(hoveredClickable != null)
                {
                    hoveredClickable.clickedDelegate?.Invoke();
                }
            }
        }
    }

    IEnumerator ReplayAnimCoroutine(RollableDie die, Vector3[] storedPositions, Quaternion[] storedRotations, int targetValue)
    {
        die.rigidbody.isKinematic = true;
        die.rigidbody.velocity = Vector3.zero;
        die.rigidbody.angularVelocity = Vector3.zero;
        Vector3 targetNormal = targetValue >= 0 ? RollableDie.faceNormals[targetValue] : Vector3.up;
        Quaternion targetRotation = storedRotations[storedRotations.Length - 1];
        Quaternion rotationOffset = Quaternion.FromToRotation(Quaternion.Inverse(targetRotation) * Vector3.up, targetNormal);
        if((targetRotation * rotationOffset * targetNormal).y < 0)
        {
            Vector3 rotAxis;
            float rotAngle;
            rotationOffset.ToAngleAxis(out rotAngle, out rotAxis);
            rotationOffset *= Quaternion.AngleAxis(180, rotAxis);
        }

        for(float time=0; time < storedPositions.Length * Time.fixedDeltaTime; time += Time.deltaTime)
        {
            int stepIndex = Mathf.FloorToInt(time / Time.fixedDeltaTime);
            die.transform.position = storedPositions[stepIndex];
            float lerpRatio = Mathf.Clamp01(time / rotationOffsetDuration);
            die.transform.rotation = storedRotations[stepIndex] * Quaternion.Lerp(Quaternion.identity, rotationOffset, 1 - (1-lerpRatio) * (1-lerpRatio));
            yield return null;
        }
        yield return null;
        int lastIndex = storedPositions.Length - 1;
        Vector3 axis;
        float angle;
        rotationOffset.ToAngleAxis(out angle, out axis);
        axis = die.transform.TransformDirection(axis);
        die.transform.position = storedPositions[lastIndex];
        die.transform.rotation = storedRotations[lastIndex] * rotationOffset;
    }

    void FixedUpdate()
    {
        if(grabbing)
        {
            hoverDie.attracted = true;
            
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 1000, groundLayer))
            {
                hoverDie.attractionPosition = new Vector3(hit.point.x, hit.point.y, hit.point.z) + (Camera.main.transform.position - hit.point).normalized * grabHoverHeight;
            }
        }
    }
}
