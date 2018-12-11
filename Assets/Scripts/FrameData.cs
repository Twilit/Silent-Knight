//Kin Long Sha SHA17002700, Solihull College, VR & Games Design 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameData : MonoBehaviour
{
    PlayerMovement movement;
    Animator anim;

    int frameType = 0;
    //Types of frame when performing actions
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
        anim = GetComponent<Animator>();
	}

	void Update ()
    {
        if (actionName == null && frameType != 0)
        {
            frameType = 0;
        }
    }

    void Frames(int getFrameType)
    {
        if (!((getFrameType == 0) && frameType != 4)) //Fixes bug that cancels attack when previous attack ends
        {
            frameType = getFrameType;
        }

        if (actionName != null)
        {

            if (frameType == 0)
            {
                actionName = null;
                currentAction = null;
                anim.SetInteger("attackNumber", 0);
            }
        }
    }
}
