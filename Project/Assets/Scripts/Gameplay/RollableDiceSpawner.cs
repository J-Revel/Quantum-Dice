using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollableDiceSpawner : MonoBehaviour
{
    public DieSlot diePrefab;
    public float spawnDistance = 3;
    public Vector3 eulerRotation = new Vector3(0, 45, 0);
    void Start()
    {
        for(int i=0; i<DiceManager.instance.dice.Length; i++)
        {
            float firstDieOffset = -DiceManager.instance.dice.Length / 2.0f;
            Instantiate(diePrefab, transform.position + Vector3.right * (firstDieOffset + i) * spawnDistance, Quaternion.Euler(eulerRotation)).dieIndex = i;
        }
    }

    void Update()
    {
        
    }
}
