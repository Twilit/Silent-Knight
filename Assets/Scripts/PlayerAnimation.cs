//Kin Long Sha SHA17002700, Solihull College, VR & Games Design 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour 
{
    //-------------------------------------------------------------------------
    //Variables

    //--------------------------
    //-Components

    Animator anim;
    PlayerMovement movement;
    CharacterController charController;
    PlayerAttack attack;
    FrameData frameData;

    //--------------------------

    [SerializeField]
    bool wasDashing;

    //-------------------------------------------------------------------------
    //Functions

    void Start () 
	{
        //Referencing components on game object
        anim = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
        charController = GetComponent<CharacterController>();
        attack = GetComponent<PlayerAttack>();
        frameData = GetComponent<FrameData>();
    }	

	void Update () 
	{
        //Tells animator vertical air velocity and whether character is on ground
        anim.SetFloat("airVelocityY", movement.VelocityY);
        anim.SetBool("onGround", charController.isGrounded);

        //Sets and smooths speedPercent parameter for smooth transition between idle, moving, running animations
        float animationSpeedPercent = ((movement.Dashing) ? 1 : 0.2f) * movement.InputDir.magnitude;
        anim.SetFloat("speedPercent", animationSpeedPercent, movement.SpeedSmoothTime, Time.deltaTime);

        //Plays suddenStop animation if player, well, suddenly stops after running
        if (movement.CurrentSpeed >= (movement.DashSpeed - 0.1f))
        {
            wasDashing = true;
        }
        else
        {
            wasDashing = false;
        }
        
        if ((movement.InputDir == Vector2.zero) && wasDashing)
        {
            anim.SetBool("suddenStop", true);
            wasDashing = false;
        }
        else if ((movement.InputDir != Vector2.zero) || !charController.isGrounded || frameData.ActionName == "roll")
        {
            anim.SetBool("suddenStop", false);
        }
    }

    //Ends the suddenStop animation
    public void EndStop()
    {
        anim.SetBool("suddenStop", false);
    }

    //-------------------------------------------------------------------------
}