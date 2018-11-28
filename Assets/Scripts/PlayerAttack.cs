using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour 
{
    Animator anim;
    PlayerMovement movement;
    CharacterController charController;
    FrameData frameData;

    void Start () 
	{
        anim = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
        charController = GetComponent<CharacterController>();
        frameData = GetComponent<FrameData>();
    }
	
	void Update () 
	{
        if (Input.GetButtonDown("Attack") && charController.isGrounded)
        {
            Attack();
        }
        else if (frameData.ActionName == null)
        {
            anim.SetInteger("attackNumber", 0);
        }

        print("action: " + frameData.ActionName + " frameType: " + frameData.FrameType);
	}

    void Attack()
    {
        if ((frameData.ActionName == null && frameData.FrameType == 0)
            || frameData.ActionName == "attack2" && frameData.FrameType == 4)
        {
            frameData.ActionName = "attack1";
            anim.SetInteger("attackNumber", 1);
        }
        else if (frameData.ActionName == "attack1" && frameData.FrameType == 4)
        {
            frameData.ActionName = "attack2";
            anim.SetInteger("attackNumber", 2);
        }
    }
}
