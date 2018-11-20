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
        if (movement.input.magnitude != 0)
        {
            anim.SetBool("moving", true);
        }
        else
        {
            anim.SetBool("moving", false);
        }
	}
}