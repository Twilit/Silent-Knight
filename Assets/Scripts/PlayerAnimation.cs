using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerAnimation : MonoBehaviour 
{
    Animator anim;
    PlayerMovement movement;
    CharacterController charController;
    PlayerAttack attack;
    FrameData frameData;

    [SerializeField]
    bool wasDashing;
	
	void Start () 
	{
        anim = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
        charController = GetComponent<CharacterController>();
        attack = GetComponent<PlayerAttack>();
        frameData = GetComponent<FrameData>();
    }	

	void Update () 
	{
        anim.SetFloat("airVelocityY", movement.VelocityY);
        anim.SetBool("onGround", charController.isGrounded);

        float animationSpeedPercent = ((movement.Dashing) ? 1 : 0.2f) * movement.InputDir.magnitude;
        anim.SetFloat("speedPercent", animationSpeedPercent, movement.SpeedSmoothTime, Time.deltaTime);

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
        else if ((movement.InputDir != Vector2.zero) || !charController.isGrounded || movement.Dodging)
        {
            anim.SetBool("suddenStop", false);
        }
    }

    public void EndStop()
    {
        anim.SetBool("suddenStop", false);
    }
}