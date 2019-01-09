//Kin Long Sha SHA17002700, Solihull College, VR & Games Design 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour 
{
    //-------------------------------------------------------------------------
    //Variables

    //--------------------------
    //-Components

    public ParticleSystem trails;
    public ParticleSystem trailsTip;

    Animator anim;
    PlayerMovement movement;
    CharacterController charController;
    FrameData frameData;
    PlayerInputBuffer inputBuffer;
    Player player;

    //--------------------------
    //-Movement during Attacks

    float attackDirection;
    float attackMovement = 2f;
    float attackStaminaCost = 0f;

    public float AttackDirection
    {
        get { return attackDirection; }
    }
    
    public float AttackMovement
    {
        get { return attackMovement; }
    }

    //-------------------------------------------------------------------------
    //Functions

    void Start () 
	{
        //Referencing components on game object
        anim = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
        charController = GetComponent<CharacterController>();
        frameData = GetComponent<FrameData>();
        inputBuffer = GetComponent<PlayerInputBuffer>();
        player = GetComponent<Player>();
        
    }
	
	void Update () 
	{
        //When player presses attack button, or if an attack input is stored in the input buffer
        if ((Input.GetButtonDown("Attack") /*&& charController.isGrounded*/) || inputBuffer.BufferedInput == "Attack")
        {
            /*if (frameData.ActionName == null && !movement.Dodging && frameData.FrameType != 0)
            {
                frameData.FrameType = 0;
            }*/

            //When the player character is doing nothing, or in a cancellable part of an animation
            if (frameData.FrameType == 0 || frameData.FrameType == 4)
            {
                Attack();
            }
        }

        //Tells animator there is no attack
        else if (frameData.ActionName == null)
        {
            anim.SetInteger("attackNumber", 0);
        }

        //Cancels ground attacks if player moves off ground during attack
        if ((!charController.isGrounded) && frameData.ActionName != null && frameData.ActionName != "roll" && frameData.ActionName != "attackMidAir")
        {
            CancelAttack();            
        }
        //Cancels air attacks when landing
        else if (charController.isGrounded && frameData.ActionName == "attackMidAir")
        {
            CancelAttack();
        }

        ParticleSystem.EmissionModule emission = trails.emission;

        //Emit sword trails when attacking
        if ((frameData.ActionName == "attack1"
            || frameData.ActionName == "attack2"
            || frameData.ActionName == "attackRunning"
            || frameData.ActionName == "attackRolling"
            || frameData.ActionName == "attackMidAir")
            
            && frameData.FrameType == 2)
        {
            trails.Play();
            trailsTip.Play();
            //emission.enabled = true;
        }
        else
        {
            trails.Stop();
            trailsTip.Stop();
            //emission.enabled = false;
        }

        /*if (frameData.ActionName != null && frameData.FrameType == 1 && anim.GetFloat("attackNumber") == 0)
        {
            CancelAttack();
        }*/

        print("action: " + frameData.ActionName + " frameType: " + frameData.FrameType);
        //print("currentSpeed: " + movement.CurrentSpeed + " dashSpeed: " + movement.DashSpeed);
	}

    //Handles all attacks, such as attacking direction, and which attack to do, and setting the movement during that attack
    void Attack()
    {
        //If character is grounded and has sufficient stamina

        if (player.CurrentStamina > 0)
        {
            bool running = false;

            //When player is doing nothing or finishing a roll
            if (frameData.FrameType == 0 ||
                ((frameData.ActionName == "roll" && frameData.FrameType == 4)
                || (frameData.ActionName == "roll2" && frameData.FrameType == 4)))
            {
                //Checks if player is running for the purposes of whether to do a running attack or not
                if (movement.CurrentSpeed >= movement.DashSpeed - 0.01f
                    || !((frameData.ActionName == "roll" && frameData.FrameType == 4)
                || (frameData.ActionName == "roll2" && frameData.FrameType == 4)))
                {
                    running = true;
                }

                if (charController.isGrounded)
                {
                    //Cancels previous movement
                    movement.CurrentSpeed = 0;

                    //Attacks in direction of directional input
                    //or in direction player character is facing if there is no direction input
                    if (movement.InputDir != Vector2.zero && !running)
                    {
                        attackDirection = Mathf.Atan2(movement.InputDir.x, movement.InputDir.y) * Mathf.Rad2Deg;
                    }
                    else
                    {
                        attackDirection = transform.eulerAngles.y;
                    }

                    transform.eulerAngles = Vector3.up * attackDirection;
                }
            }

            /*
            attackNumber (Animation):
            -2 Second Roll
            -1 Roll
            0 Nothing
            1 Neutral First Swing
            2 Neutral Second Swing
            3 Neutral Third Swing
            4 Post Rolling Swing
            5 Post Running Swing
            6 Mid Air Swing
            */

            if (charController.isGrounded)
            {
                anim.SetInteger("reactNumber", 0);

                //RUNNING ATTACK when player is doing nothing but running
                if (frameData.ActionName == null && movement.Dashing && running)
                {
                    attackMovement = 5.5f;
                    attackStaminaCost = 41f;
                    frameData.ActionName = "attackRunning";
                    frameData.FrameType = 1;
                    anim.SetInteger("attackNumber", 5);
                }
                //ROLLING ATTACK when player is coming out of a roll
                else if ((frameData.ActionName == "roll" && frameData.FrameType == 4)
                    || (frameData.ActionName == "roll2" && frameData.FrameType == 4))
                {
                    attackMovement = 1.5f;
                    attackStaminaCost = 28f;
                    frameData.ActionName = "attackRolling";
                    frameData.FrameType = 1;
                    anim.SetInteger("attackNumber", 4);
                }
                //FIRST ATTACK when player is doing nothing, or finishing either the second attack or running attack
                else if ((frameData.ActionName == null && frameData.FrameType == 0)
                    || (frameData.ActionName == "attack3" && frameData.FrameType == 4)
                    || (frameData.ActionName == "attackRunning" && frameData.FrameType == 4)
                    || (frameData.ActionName == "attack2" && frameData.FrameType == 4)
                    || (frameData.ActionName == "react" && frameData.FrameType == 4))
                {
                    attackMovement = 4.5f;
                    attackStaminaCost = 33f;
                    frameData.ActionName = "attack1";
                    frameData.FrameType = 1;
                    anim.SetInteger("attackNumber", 1);
                }
                //SECOND ATTACK when player is finishing either the first attack or rolling attack
                else if ((frameData.ActionName == "attack1" && frameData.FrameType == 4)
                    || (frameData.ActionName == "attackRolling" && frameData.FrameType == 4))
                {
                    attackMovement = 3f;
                    attackStaminaCost = 35f;
                    frameData.ActionName = "attack2";
                    frameData.FrameType = 1;
                    anim.SetInteger("attackNumber", 2);
                }
                /*else if (frameData.ActionName == "attack2" && frameData.FrameType == 4)
                {
                    attackMovement = 5f;
                    frameData.ActionName = "attack3";
                    anim.SetInteger("attackNumber", 3);
                }*/
            }
            //MIDAIR ATTACK when player is in air and doing nothing
            else
            {
                if (frameData.ActionName == null)
                {
                    attackMovement = 0f;
                    attackStaminaCost = 31f;
                    frameData.ActionName = "attackMidAir";
                    frameData.FrameType = 1;
                    anim.SetInteger("attackNumber", 6);
                    //anim.SetLayerWeight(1, 0.95f);
                }
            }

            //Spends stamina
            player.UseStamina(attackStaminaCost);
        }

        //Empties input buffer as buffered input has been acted upon
        inputBuffer.BufferedInput = null;
    }

    //Cancels attack
    void CancelAttack()
    {
        frameData.FrameType = 0;
        frameData.ActionName = null;
        //anim.SetLayerWeight(1, 0f);
    }

    //-------------------------------------------------------------------------
}
