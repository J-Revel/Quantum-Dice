using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSwapHighlight : MonoBehaviour
{
    public MeshRenderer[] meshRenderers;
    public Material highlightMaterial;
    private Material[] defaultMaterials;
    private Clickable clickable;

    void Start()
    {
        clickable = GetComponent<Clickable>();
        clickable.hoverStartDelegate += HoverStart;
        clickable.hoverEndDelegate += HoverEnd;
        defaultMaterials = new Material[meshRenderers.Length];
        for(int i=0; i<meshRenderers.Length; i++)
        {
            defaultMaterials[i] = meshRenderers[i].material;
        }
    }

    public void HoverStart()
    {
        for(int i=0; i<meshRenderers.Length; i++)
        {
            meshRenderers[i].material = highlightMaterial;
        }
    }

    public void HoverEnd()
    {
        for(int i=0; i<meshRenderers.Length; i++)
        {
            meshRenderers[i].material = defaultMaterials[i];
        }
    }
}
