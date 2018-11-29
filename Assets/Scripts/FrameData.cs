using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameData : MonoBehaviour
{
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
		
	}

	void Update ()
    {
		
	}

    void Frames(int getFrameType)
    {
        if (!(getFrameType == 0 && frameType != 4))
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
