using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StickyPointer : MonoBehaviour
{
    public Camera activeCam;
    public Button sticky;

    void Start()
    {
        activeCam = Camera.main;
        SetActiveCam(activeCam);
    }

    void Update()
    {
        Vector3 pos = activeCam.WorldToScreenPoint(this.transform.position);
        sticky.transform.position = pos;
    }

    public void SetActiveCam(Camera cam)
    {
        activeCam = cam;
    }
}
