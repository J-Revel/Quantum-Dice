using System;
using UnityEngine;

[CreateAssetMenu()]
public class HarmonicConfig : ScriptableObject
{
    public HarmonicData[] harmonics;
    public Material material;
    public int lineCount = 1;
    public float lineThickness = 0.05f;
}