using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    Animator anim;
    FrameData frameData;
    PlayerMovement movement;
    CharacterController charController;
    PlayerAttack attack;

    int reactType;
    List<int> reactList;

    public int ReactType
    {
        get
        {
            return reactType;
        }
    }

    void Start ()
    {
        //Referencing components on game object
        frameData = GetComponent<FrameData>();
        movement = GetComponent<PlayerMovement>();
        attack = GetComponent<PlayerAttack>();
        charController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        //Set up
        SetStats(800, 150, 40, 230, 120, 0); //Health: 800, Stamina: 150, ATK: 230, DEF: 120, Poise: 0
        SetHealthStaminaToMax();

        ReactListRefill();
    }	

	void Update ()
    {
        //Regen stamina if no action is being done, character is grounded, and the player is not attempting to run
        if ((frameData.ActionName == "react" || frameData.ActionName == "reactMidAir" || frameData.FrameType == 0) 
            && charController.isGrounded 
            /*&& !(movement.Dashing && movement.InputDir != Vector2.zero)*/ 
            && !(Input.GetButton("Run") && movement.InputDir != Vector2.zero))
        {
            currentStamina = Regen(staminaPerSec, currentStamina, maxStamina);
        }

        //print(currentStamina + "/" + maxStamina);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            SetStats(800, 200, 40, 230, 120, 0);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            SetStats(1200, 150, 40, 230, 120, 0);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            HealthAdjust("damage", 100);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            HealthAdjust("heal", 100);
        }
    }

    void ReactListRefill()
    {
        reactList = new List<int>();
        for (int n = 0; n < 5; n++)
        {
            reactList.Add(n);
        }
    }

    public override void HealthAdjust(string type, int amount)
    {
        if (!((frameData.ActionName == "roll" || frameData.ActionName == "roll2" ) && frameData.FrameType == 2))
        {
            if ((type == "damage") && (CurrentHealth > 0))
            {
                movement.CurrentSpeed = 0;

                if (charController.isGrounded)
                {
                    if (reactList.Count <= 1)
                    {
                        ReactListRefill();
                    }

                    int index = Random.Range(1, reactList.Count - 1);
                    reactType = reactList[index];
                    reactList.RemoveAt(index);
                }
                else
                {
                    reactType = 5;
                }

                anim.SetInteger("reactNumber", reactType);
                anim.SetTrigger("react");

                if (charController.isGrounded)
                {
                    frameData.ActionName = "react";
                }
                else
                {
                    frameData.ActionName = "reactMidAir";
                }
            }

            base.HealthAdjust(type, amount);
        }

        if (CurrentHealth <= 0)
        {
            Death();
        }
    }

    public override void Death()
    {
        anim.SetTrigger("death");

        movement.enabled = false;
        attack.enabled = false;
        movement.AtFeet.SetActive(false);
    }
}
