using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    FrameData frameData;
    PlayerMovement movement;
    CharacterController charController;

    void Start ()
    {
        //Referencing components on game object
        frameData = GetComponent<FrameData>();
        movement = GetComponent<PlayerMovement>();
        charController = GetComponent<CharacterController>();
        //Set up
        SetHealthStaminaToMax();
	}	

	void Update ()
    {
        if (frameData.FrameType == 0 && !(movement.Dashing && charController.isGrounded))
        {
            currentStamina = Regen(staminaPerSec, currentStamina, maxStamina);
        }

        print(currentStamina + "/" + maxStamina);
    }
}
