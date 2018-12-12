using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    FrameData frameData;

    void Start ()
    {
        //Referencing components on game object
        frameData = GetComponent<FrameData>();
        //Set up
        SetHealthStaminaToMax();
	}	

	void Update ()
    {
        if (frameData.FrameType == 0)
        {
            currentStamina = Regen(staminaPerSec, currentStamina, maxStamina);
        }

        print(currentStamina + "/" + maxStamina);
    }
}
