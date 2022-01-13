using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTextureAllocator : MonoBehaviour
{
    public bool metalMode;
    public Material[] textureList;

    private Renderer character;
    private int rand;
    private bool materialOverride;

    void Start()
    {
        character = GetComponentInChildren<SkinnedMeshRenderer>();

        if (textureList.Length < 1)
        {
            return;
        }

        if (!materialOverride)
        {
            rand = Random.Range(0, 100);
        }

        MainLoop();
    }

    void MainLoop()
    {
        float loopMargins = 100 / textureList.Length;

        int loopCounts = 0;
        for (float i = 0; i < loopMargins * textureList.Length; i += loopMargins)
        {
            if (rand >= i && rand <= i + loopMargins)
            {
                character.material = textureList[loopCounts];
                if (metalMode && (i + loopMargins) == (loopMargins * textureList.Length))
                {
                    character.material = textureList[Random.Range(0, textureList.Length - 1)];
                }
                break;
            }
            loopCounts++;
        }

        if (metalMode)
        {
            if (rand == 98 || rand == 99 || rand == 100)
            {
                character.material = textureList[textureList.Length - 1];
            }
        }
    }

    public void MaterialOverride(int val)
    {
        materialOverride = true;
        rand = val;
    }

    public int GetMaterialID()
    {
        return rand;
    }

    public bool isMetal()
    {
        if (metalMode)
        {
            if (rand == 98 || rand == 99 || rand == 100)
            {
                return true;
            }
        }
        return false;
    }
}
