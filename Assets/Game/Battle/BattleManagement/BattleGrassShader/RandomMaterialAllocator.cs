using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMaterialAllocator : MonoBehaviour
{
    public Renderer[] planes;
    public Material[] materials;

    void Start()
    {
        if (planes.Length < 1 || materials.Length < 1)
        {
            return;
        }

        int rand = Random.Range(0, materials.Length);
        for (int i = 0; i < planes.Length; i ++)
        {
            planes[i].material = materials[rand];
        }
    }
}
