using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericStartJump : MonoBehaviour
{
    ThirdPersonControl fpControl;

    // AI Controller

    private void Start()
    {
        fpControl = GetComponentInParent<ThirdPersonControl>();

        if (fpControl == null)
        {
            // get AI controller
        }
    }

    public void StartJump()
    {
        if (fpControl != null)
        {
            fpControl.ApplyJump();
        }
        else
        {
            // AIController.ApplyJump()
        }
    }
}
