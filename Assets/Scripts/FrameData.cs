using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameData : MonoBehaviour
{
    PlayerMovement movement;

    int frameType = 0;
    /*
    1 = Start Up / Unbufferable Recovery
    2 = Active
    3 = Bufferable Recovery
    4 = Cancellable
    
    5 = Invincibility
    0 = Nothing
    */

    public int FrameType
    {
        get { return frameType; }

        set { frameType = value; }
    }

    string actionName = null;

    public string ActionName
    {
        get { return actionName; }

        set { actionName = value; }
    }

    string currentAction = null;

    void Start ()
    {
        movement = GetComponent<PlayerMovement>();
	}

	void Update ()
    {
        if (actionName == null && !movement.Dodging && frameType != 0)
        {
            frameType = 0;
        }
    }

    void Frames(int getFrameType)
    {
        if (!((getFrameType == 0 || getFrameType == 5) && frameType != 4)) //Fixes bug that cancels attack when previous attack ends
        {
            frameType = getFrameType;
        }

        if (actionName != null)
        {

            if (frameType == 0)
            {
                actionName = null;
                currentAction = null;
            }
        }
    }
}
