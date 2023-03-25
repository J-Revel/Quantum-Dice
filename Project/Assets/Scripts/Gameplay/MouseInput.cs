using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseInput : MonoBehaviour
{
    public LayerMask raycastLayer;
    public LayerMask groundLayer;
    private RollableDie hoverDie;
    private bool grabbing;
    public float grabHoverHeight = 2;
    public float rotationImpulse = 1;
    private Dictionary<RollableDie, Coroutine> coroutines = new Dictionary<RollableDie, Coroutine>();
    
    void Start()
    {
        
    }

    void Update()
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
                hoverDie.rigidbody.AddTorque(Random.insideUnitSphere * rotationImpulse, ForceMode.Acceleration);
                for(float t=0; t<5; t += Time.fixedDeltaTime)
                {
                    hoverDie.FixedUpdate();
                    Physics.Simulate(Time.fixedDeltaTime);
                    Physics.SyncTransforms();
                    storedPositions[cursor] = hoverDie.transform.position;
                    storedRotations[cursor] = hoverDie.transform.rotation;
                    cursor++;
                }
                if(coroutines.ContainsKey(hoverDie) && coroutines[hoverDie] != null)
                {
                    StopCoroutine(coroutines[hoverDie]);
                }
                coroutines[hoverDie] = StartCoroutine(ReplayAnimCoroutine(hoverDie, storedPositions, storedRotations, 1));

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
                if(die != null && die != hoverDie)
                {
                    die.hoverElement.SetActive(true);
                    if(hoverDie != null)
                        hoverDie.hoverElement.SetActive(false);
                    hoverDie = die;
                }
                if(die == null && hoverDie != null)
                {
                    hoverDie.hoverElement.SetActive(false);
                    hoverDie = null;        
                }
                if(Mouse.current.leftButton.wasPressedThisFrame && hoverDie != null)
                {
                    grabbing = true;
                    hoverDie.rigidbody.isKinematic = false;
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
        }
    }

    IEnumerator ReplayAnimCoroutine(RollableDie die, Vector3[] storedPositions, Quaternion[] storedRotations, int targetValue)
    {
        die.rigidbody.isKinematic = true;
        die.rigidbody.velocity = Vector3.zero;
        die.rigidbody.angularVelocity = Vector3.zero;
        Vector3 targetNormal = RollableDie.faceNormals[targetValue];
        Quaternion rotationOffset = Quaternion.FromToRotation(storedRotations[storedRotations.Length - 1] * targetNormal, Vector3.up);

        for(float time=0; time < storedPositions.Length * Time.fixedDeltaTime; time += Time.deltaTime)
        {
            int stepIndex = Mathf.FloorToInt(time / Time.fixedDeltaTime);
            die.transform.position = storedPositions[stepIndex];
            float lerpRatio = Mathf.Clamp01(time / 0.3f);
            die.transform.rotation = Quaternion.Lerp(Quaternion.identity, rotationOffset, lerpRatio) * storedRotations[stepIndex];
            yield return null;
        }
        int lastIndex = storedPositions.Length - 1;
        die.transform.position = storedPositions[lastIndex];
        die.transform.rotation = rotationOffset * storedRotations[lastIndex];
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
