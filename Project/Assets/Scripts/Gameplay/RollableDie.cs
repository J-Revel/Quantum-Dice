using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollableDie : MonoBehaviour
{
    public GameObject hoverElement;
    public new Rigidbody rigidbody;
    public bool attracted;
    public Vector3 attractionPosition;
    public float grabAttractionForce = 10;
    public float grabSlowdown = 0.95f;
    public float grabTargetDistance = 5;
    public float gravity = 30;
    public float gravityHeightThresholdStart = 1.2f;
    public float gravityHeightThresholdEnd = 2;
    public static Vector3[] faceNormals = {Vector3.forward, Vector3.up, Vector3.left, Vector3.right, Vector3.down, Vector3.back};
    
    void Start()
    {
        
    }

    public void FixedUpdate()
    {
        float gravityHeightRatio = Mathf.Clamp01((transform.position.y - gravityHeightThresholdStart) / (gravityHeightThresholdEnd - gravityHeightThresholdStart));
        rigidbody.AddForce(gravity * Vector3.Lerp(Vector3.down, (transform.position - Camera.main.transform.position).normalized, gravityHeightRatio), ForceMode.Acceleration);
        if(attracted)
        {
            float distanceRatio = Mathf.Clamp01((attractionPosition - transform.position).magnitude / grabTargetDistance);
            rigidbody.AddForce(grabAttractionForce * distanceRatio * (attractionPosition - transform.position).normalized, ForceMode.Acceleration);
            rigidbody.velocity = rigidbody.velocity * Mathf.Pow(Time.deltaTime, grabSlowdown);
        }
    }
}
