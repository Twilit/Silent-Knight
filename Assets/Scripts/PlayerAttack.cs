﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour 
{
    Animator anim;
    PlayerMovement movement;
    CharacterController charController;
    FrameData frameData;
    PlayerInputBuffer inputBuffer;

    float attackDirection;

    public float AttackDirection
    {
        get { return attackDirection; }
    }

    float attackMovement = 2f;
    public float AttackMovement
    {
        get { return attackMovement; }
    }

    /*
     attackNumber:
     -2 Second Roll
     -1 Roll
     0 Nothing
     1 Neutral First Swing
     2 Neutral Second Swing
     3 Post Running Swing
     4 Post Rolling Swing
     */ 

    void Start () 
	{
        anim = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
        charController = GetComponent<CharacterController>();
        frameData = GetComponent<FrameData>();
        inputBuffer = GetComponent<PlayerInputBuffer>();
    }
	
	void Update () 
	{
        if ((Input.GetButtonDown("Attack") && charController.isGrounded) || inputBuffer.BufferedInput == "Attack")
        {
            /*if (frameData.ActionName == null && !movement.Dodging && frameData.FrameType != 0)
            {
                frameData.FrameType = 0;
            }*/

            if (frameData.FrameType == 0 || frameData.FrameType == 4)
            {
                Attack();
            }
        }
        else if (frameData.ActionName == null)
        {
            anim.SetInteger("attackNumber", 0);
        }

        if ((!charController.isGrounded) && frameData.ActionName != null)
        {
            CancelAttack();
        }

        print("action: " + frameData.ActionName + " frameType: " + frameData.FrameType);
        //print("currentSpeed: " + movement.CurrentSpeed + " dashSpeed: " + movement.DashSpeed);
	}

    void Attack()
    {
        bool running = false;

        if (frameData.FrameType == 0)
        {
            if (movement.CurrentSpeed >= movement.DashSpeed - 0.1f)
            {
                running = true;
            }

            movement.CurrentSpeed = 0;

            if (movement.InputDir != Vector2.zero)
            {
                attackDirection = Mathf.Atan2(movement.InputDir.x, movement.InputDir.y) * Mathf.Rad2Deg;
            }
            else
            {
                attackDirection = transform.eulerAngles.y;
            }

            transform.eulerAngles = Vector3.up * attackDirection;
        }    

        if (frameData.ActionName == null && movement.Dashing && running)
        {
            attackMovement = 5.5f;
            frameData.ActionName = "attackRunning";
            anim.SetInteger("attackNumber", 3);
        }
        else if ((frameData.ActionName == "roll" && frameData.FrameType == 4)
            || (frameData.ActionName == "roll2" && frameData.FrameType == 4))
        {
            attackMovement = 1.5f;
            frameData.ActionName = "attackRolling";
            anim.SetInteger("attackNumber", 4);
        }
        else if ((frameData.ActionName == null && frameData.FrameType == 0) 
            || (frameData.ActionName == "attack2" && frameData.FrameType == 4)
            || (frameData.ActionName == "attackRunning" && frameData.FrameType == 4))
        {
            attackMovement = 4.5f;
            frameData.ActionName = "attack1";
            anim.SetInteger("attackNumber", 1);
        }
        else if ((frameData.ActionName == "attack1" && frameData.FrameType == 4)
            || (frameData.ActionName == "attackRolling" && frameData.FrameType == 4))
        {
            attackMovement = 3f;
            frameData.ActionName = "attack2";
            anim.SetInteger("attackNumber", 2);
        }

        inputBuffer.BufferedInput = null;
    }

    void CancelAttack()
    {
        frameData.FrameType = 0;
        frameData.ActionName = null;
    }
}