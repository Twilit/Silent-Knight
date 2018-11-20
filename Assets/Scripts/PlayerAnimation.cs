using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerAnimation : MonoBehaviour 
{
    Animator anim;
    PlayerMovement movement;
	
	void Start () 
	{
        anim = transform.GetChild(0).GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
    }	

	void Update () 
	{
        anim.SetFloat("airVelocityY", movement.VelocityY);
        anim.SetBool("onGround", GetComponent<CharacterController>().isGrounded);

        float animationSpeedPercent = ((movement.Dashing) ? 1 : 0.5f) * movement.InputDir.magnitude;
        anim.SetFloat("speedPercent", animationSpeedPercent, movement.SpeedSmoothTime, Time.deltaTime);
    }
}