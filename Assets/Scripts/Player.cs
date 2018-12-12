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
        //Regen stamina if no action is being done, character is grounded, and the player is not attempting to run
        if (frameData.FrameType == 0 && charController.isGrounded /*&& !(movement.Dashing && movement.InputDir != Vector2.zero)*/ && !(Input.GetButton("Run") && movement.InputDir != Vector2.zero))
        {
            currentStamina = Regen(staminaPerSec, currentStamina, maxStamina);
        }

        print(currentStamina + "/" + maxStamina);
    }
}
