using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour 
{
    Animator anim;
    PlayerMovement movement;
    CharacterController charController;

    int frameType = 0;
    /*
    1 = Start Up
    2 = Active
    3 = Recovery
    4 = Cancellable

    0 = Not Attacking
    */

    public int FrameType
    {
        get { return frameType; }

        set { frameType = 0; }
    }

    int attackNumber = 0;

    public int AttackNumber
    {
        get { return attackNumber; }

        set
        {
            attackNumber = 0;
            frameType = 0;
        }
    }

    void Start () 
	{
        anim = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
        charController = GetComponent<CharacterController>();
    }
	
	void Update () 
	{
        if (Input.GetButtonDown("Attack"))
        {
            Attack();
        }

        anim.SetInteger("attackNumber", attackNumber);
	}

    void Attack()
    {
        if ((attackNumber == 0 && frameType == 0)
            || attackNumber == 2 && frameType == 4)
        {
            attackNumber = 1;
        }
        else if (attackNumber == 1 && frameType == 4)
        {
            attackNumber = 2;
            print("working");
        }
    }

    void FrameData(int getFrameType)
    {
        frameType = getFrameType;

        if (frameType == 0)
        {
            attackNumber = 0;
        }
    }
}
