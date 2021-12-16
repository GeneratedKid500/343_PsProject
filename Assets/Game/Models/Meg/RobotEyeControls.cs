using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotEyeControls : MonoBehaviour
{
    [Header("MaterialInput")]
    public Material eyes;

    [Header("ChangeEye")]
    public EyeStates State;
    int curState;

    // Start is called before the first frame update
    void Start()
    {
        CallEyeStates((int)State);
    }
    private void LateUpdate()
    {
        if ((int)State != curState)
        {
            curState = (int)State;
            CallEyeStates(curState);
        }
    }

    public int CallEyeStates(int newState = -1)
    {
        if (newState == -1)
        {
            return (int)State;
        }

        State = (EyeStates)newState;
        curState = newState;

        switch (newState)
        {
            case 0:
                eyes.mainTextureOffset = new Vector2(0, 0);
                break;
            case 1:
                eyes.mainTextureOffset = new Vector2(0.5f, 0);
                break;
            case 2:
                eyes.mainTextureOffset = new Vector2(0, -0.25f);
                break;
            case 3:
                eyes.mainTextureOffset = new Vector2(0.5f, -0.25f);
                break;
            case 4:
                eyes.mainTextureOffset = new Vector2(0, -0.5f);
                break;
            case 5:
                eyes.mainTextureOffset = new Vector2(0.5f, -0.5f);
                break;
            case 6:
                eyes.mainTextureOffset = new Vector2(0, -0.75f);
                break;
            case 7:
                eyes.mainTextureOffset = new Vector2(0.5f, -0.75f);
                break;

        }
        return newState;
    }

    public enum EyeStates { Wink, Excited, Closed, Open, Happy, Anrgy, Hurt, Off }

}
