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

    string actionName = null;
    //Action names
    /*
    attack1
    attack2
    attackRunning
    attackRolling
    attackMidAir
    roll
    roll2
    */

    string currentAction = null;

    public int FrameType
    {
        get { return frameType; }

        set { frameType = value; }
    }

    public string ActionName
    {
        get { return actionName; }

        set { actionName = value; }
    }

    void Start ()
    {
        //Referencing components on game object
        movement = GetComponent<PlayerMovement>();
        anim = GetComponent<Animator>();
	}

	void Update ()
    {
        //Clear frameType when it should be 0
        if (actionName == null && frameType != 0)
        {
            frameType = 0;
        }
    }

    //Handles the changing of frameType during animation
    //Called in animation events
    void Frames(int getFrameType)
    {
        if (!((getFrameType == 0) && frameType != 4)) //Fixes bug that cancels attack when previous attack ends
        {
            frameType = getFrameType;
        }

        //Ends action when frameType is changed to 0
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
