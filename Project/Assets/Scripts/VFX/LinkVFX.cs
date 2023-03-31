using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct HarmonicData {
    public float freq;
    public float movement;
    public Vector3 displacement;
    public float offset;
    public float linkIndexOffset;
    public bool active;
    public bool worldspace;
    public bool random;
}

[System.Serializable]
public struct LinkProceduralAnimationConfig {
    public Vector3 apexOffset;
    public HarmonicConfig config;
}

public class LinkVFX : MonoBehaviour
{
    public Transform origin;
    public Vector3 originOffset;
    public Transform target;
    public Vector3 targetOffset;

    public LineRenderer lineRendererPrefab;
    private LineRenderer[] lineRenderers;
    public float pointPerUnit = 2;
    public LinkProceduralAnimationConfig config;
    private float time;
    public float animRatio = 0;
    private MaterialPropertyBlock propertyBlock;

    private Vector3 originPosition { get { return origin == null ? transform.position + originOffset : origin.position + originOffset; } }
    private Vector3 targetPosition { get { return target == null ? transform.position + targetOffset : target.position + targetOffset; } }

    public void Start()
    {
        lineRenderers = new LineRenderer[config.config.lineCount];
        for(int i=0; i<config.config.lineCount; i++)
        {
            lineRenderers[i] = Instantiate(lineRendererPrefab, Vector3.zero, Quaternion.identity, transform);
            lineRenderers[i].positionCount = Mathf.CeilToInt((originPosition - targetPosition).magnitude * pointPerUnit);
        }
        propertyBlock = new MaterialPropertyBlock();
        lineRenderers[0].GetPropertyBlock(propertyBlock);
    }

    public void Update()
    {
        time += Time.deltaTime;
        for(int i=0; i<lineRenderers.Length; i++)
        {
            float linkIndex = i / (float)lineRenderers.Length;
            int pointCount = Mathf.RoundToInt((originPosition - targetPosition).magnitude * pointPerUnit);
            lineRenderers[i].positionCount = pointCount+1;
            Vector3 direction = (targetPosition - originPosition).normalized;
            Vector3 up = Vector3.up;
            Vector3 normal = Vector3.Cross(direction, up);
            for(int j=0; j<=pointCount; j++) {
                float f = j / (float)pointCount;
                float scale = (1 - f * f) * (1 - (1-f) * (1-f));
                Vector3 pointPos = Vector3.Lerp(originPosition, targetPosition, f);
                pointPos += config.apexOffset * scale;
                foreach(HarmonicData harmonic in config.config.harmonics)
                {
                    if(harmonic.active)
                    {
                        float fScale = 1;
                        if(harmonic.worldspace)
                        {
                            fScale = (originPosition - targetPosition).magnitude;
                        }
                        float spaceAnimOffset = f * fScale * harmonic.freq;
                        float timeAnimOffset = harmonic.movement * time + linkIndex * harmonic.linkIndexOffset;
                        // pointPos += harmonic.displacement * scale * Mathf.Sin((harmonic.offset + f / fScale * harmonic.freq + harmonic.movement * (time + linkIndex * harmonic.linkIndexOffset)) * Mathf.PI * 2);
                        Vector3 displacement = harmonic.displacement.x * normal + harmonic.displacement.y * up + harmonic.displacement.z * direction;
                        float value = 0;
                        if(harmonic.random)
                            value = Random.Range(-1.0f, 1.0f);
                        else 
                            value = Mathf.Sin((harmonic.offset + spaceAnimOffset + timeAnimOffset) * Mathf.PI * 2);
                        pointPos += displacement * scale * value;
                    }
                }
                lineRenderers[i].SetPosition(j, pointPos);
                lineRenderers[i].material = config.config.material;
                lineRenderers[i].widthMultiplier = config.config.lineThickness;
            }
            propertyBlock.SetFloat("_AnimRatio", animRatio);
            lineRenderers[i].SetPropertyBlock(propertyBlock);
        }
    }
}