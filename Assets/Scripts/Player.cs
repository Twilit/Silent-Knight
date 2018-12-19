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
        SetStats(800, 150, 40, 230, 120, 0); //Health: 800, Stamina: 150, ATK: 230, DEF: 120, Poise: 0
        SetHealthStaminaToMax();
	}	

	void Update ()
    {
        //Regen stamina if no action is being done, character is grounded, and the player is not attempting to run
        if (frameData.FrameType == 0 && charController.isGrounded /*&& !(movement.Dashing && movement.InputDir != Vector2.zero)*/ && !(Input.GetButton("Run") && movement.InputDir != Vector2.zero))
        {
            currentStamina = Regen(staminaPerSec, currentStamina, maxStamina);
        }

        //print(currentStamina + "/" + maxStamina);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            SetStats(800, 200, 40, 230, 120, 0);
        }
    }
}
