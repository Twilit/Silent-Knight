//Kin Long Sha SHA17002700, Solihull College, VR & Games Design 
//Using tutorial by Sebastian Lague: https://www.youtube.com/playlist?list=PLFt_AvWsXl0djuNM22htmz3BUtHHtOh7v
//Along with own code

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //-------------------------------------------------------------------------
    //Variables

    //--------------------------
    //-Components

    public ParticleSystem runDust;
    public ParticleSystem landDust;

    public GameObject AtFeet;

    CharacterController charController;
    Animator anim;
    PlayerAttack attack;
    FrameData frameData;
    PlayerInputBuffer inputBuffer;
    Player player;

    //--------------------------
    //-Constants

    [SerializeField]
    float moveSpeed = 4.0f;
    [SerializeField]
    float dashSpeed = 6.0f;
    [SerializeField]
    float speedSmoothTime = 0.1f;
    [SerializeField]
    float jumpHeight = 5.0f;
    [SerializeField]
    float gravity = -1.0f;
    [Range(0, 1), SerializeField]
    float airControlPercent;
    [Range(0, 1), SerializeField]
    float dashControlPercent;
    [Range(0, 1), SerializeField]
    float attackControlPercent;
    [Range(0, 1), SerializeField]
    float groundDodgeSpeedSmooth;
    [SerializeField]
    float initialDodgeSpeed = 5.0f;
    [SerializeField]
    float endDodgeSpeed = 2.0f;
    [SerializeField]
    float dodgeSmoothTime = 0.5f;
    [SerializeField]
    float jumpStaminaCost = 20f;
    [SerializeField]
    float dashStaminaCost = 5f;
    [SerializeField]
    float rollStaminaCost = 45f;

    //--------------------------
    //-Not Constants

    float fallMultiplier = 1;
    bool stepOffLedge;

    float speedSmoothVelocity;
    float dodgeSmoothVelocity;
    float turnSmoothVelocity;

    float currentSpeed;
    float currentDodgeSpeed;
    private float velocityY;
    private bool dashing;
    private bool justStartedMoving = true;

    //--------------------------
    //-Input

    private Vector2 input;
    private Vector2 inputDir;


    //-------------------------------------------------------------------------
    //Properties

    public bool StepOffLedge
    {
        //Landing dust particle effect plays when value is set to true when it wasn't true beforehand
        set
        {
            if (value == true && stepOffLedge != value)
            {
                landDust.Play();
                anim.SetInteger("reactNumber", 0);
            }

            stepOffLedge = value;
        }
    }

    public float MoveSpeed
    {
        get
        {
            return moveSpeed;
        }
    }

    public float DashSpeed
    {
        get
        {
            return dashSpeed;
        }
    }

    public float SpeedSmoothTime
    {
        get
        {
            return speedSmoothTime;
        }
    }
    
    public float CurrentSpeed
    {
        get
        {
            return currentSpeed;
        }
        set
        {
            currentSpeed = value;
        }
    }

    public float VelocityY
    {
        get
        {
            return velocityY;
        }
    }
    
    public Vector2 InputDir
    {
        get
        {
            return inputDir;
        }
    }
    
    public bool Dashing
    {
        get
        {
            return dashing;
        }
    }

    //-------------------------------------------------------------------------
    //Functions

    void Start ()
	{
        //Referencing components on game object
        charController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        attack = GetComponent<PlayerAttack>();
        frameData = GetComponent<FrameData>();
        inputBuffer = GetComponent<PlayerInputBuffer>();
        player = GetComponent<Player>();
    }
	
	void Update ()
	{
        //Get the player's inputs
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        inputDir = input.normalized;

        //Handles falling, and pressing the player onto the ground
        Gravity();

        //Handles ground movement, air movement, and movement whilst performing attacks
        Movement(inputDir);

        //Handles dodging
        InitiateDodge(inputDir);
        DuringDodge();

        //Handles interaction
        Interact();

        //Handles dust particle effects when moving, running, rolling and at the start of jump
        Dust();
    }

    void Movement(Vector2 inputDir)
    {
        if (frameData.ActionName != "roll")
        {
            //Get the direction the player wants to move to via their input
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg;

            //When player is either not performing any actions or is using the air attack - and therefore allowed to move freely
            if (frameData.ActionName == null || (frameData.ActionName == "attackMidAir") || (frameData.ActionName == "reactMidAir"))
            {
                //Handles jumping
                Jump();
                //Handles running
                Dash();

                //When there is directional input from player
                if (inputDir != Vector2.zero)
                {
                    //int smoothedAngle = 120;
                    //Smooth time for player turning and changing direction in normal movement
                    float turnSmoothTime = 0.15f;

                    //Relics of a foolish struggle
                    /*if ((((Mathf.Sign(targetRotation) == -1) ? (targetRotation + 360) : targetRotation) > (((Mathf.Sign(transform.eulerAngles.y + smoothedAngle) == -1) ? (transform.eulerAngles.y + smoothedAngle + 360) : transform.eulerAngles.y + smoothedAngle)) ||
                        (((Mathf.Sign(targetRotation) == -1) ? (targetRotation + 360) : targetRotation) < (((Mathf.Sign(transform.eulerAngles.y - smoothedAngle) == -1) ? (transform.eulerAngles.y - smoothedAngle - 360) : transform.eulerAngles.y - smoothedAngle)))))*/

                    //Player instantly turns to target direction when a direction is input when there was no directional input before,
                    //but changes direction slower if there were already directional input
                    if (/*Quaternion.Angle(Quaternion.Euler(Vector3.up * targetRotation), transform.rotation) > smoothedAngle || */justStartedMoving && charController.isGrounded && /*!dashing*/ currentSpeed <= moveSpeed)
                    {
                        transform.eulerAngles = Vector3.up * targetRotation;
                    }
                    else
                    {
                        transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime));
                    }

                    justStartedMoving = false;
                }
                else
                {
                    justStartedMoving = true;
                }

                //Works out player's target speed based on if dashing or not, and their diredctional input
                float targetSpeed = ((dashing) ? dashSpeed : moveSpeed) * inputDir.magnitude;
                //Smooths player's speed between not moving, normal speed, and running speed
                currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));

                //Set the player's full velocity using their current speed and vertical velocity
                Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;

                //Tells character controller component to move player character based on velocity
                charController.Move(velocity * anim.speed * Time.deltaTime);
            }
            //Handles player movement when attacking
            else if (frameData.ActionName == "attack1" || frameData.ActionName == "attack2" || frameData.ActionName == "attack3" || frameData.ActionName == "attackRolling")
            {
                //Smooth time for player turning and changing direction during attacking
                float turnSmoothTime = 0.35f;

                //Turns the player character according to directional input during initial frames of attack animation
                if ((frameData.FrameType == 1 || frameData.FrameType == 2) && inputDir != Vector2.zero)
                {
                    //Doesn't turn if the input is opposite to the current direction of character
                    //This is so player can dodge backwards without unwanted turning during attack
                    if (Quaternion.Angle(Quaternion.Euler(Vector3.up * targetRotation), transform.rotation) < 120)
                    {
                        transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime));
                    }
                }
                //Move forward slightly during active frames of attack animation 
                if (frameData.FrameType == 2)
                {
                    Vector3 velocity = transform.forward * attack.AttackMovement * anim.speed + Vector3.up * velocityY;
                    charController.Move(velocity * Time.deltaTime);
                }
            }
            else if (frameData.ActionName == "attackRunning" && (frameData.FrameType == 1 || frameData.FrameType == 2))
            {
                //Similar to above except player can't change direction and there is movement up until end of active frames

                Vector3 velocity = transform.forward * attack.AttackMovement * anim.speed + Vector3.up * velocityY;
                charController.Move(velocity * Time.deltaTime);
            }
            //Would've made rolling attack a lot quicker when turning during attack, so that player can dodge away and attack the opposite direction.
            //No longer needed - attacking after dodge now begins facing direction input
            /*else if (frameData.ActionName == "attackRolling")
            {
                float turnSmoothTime = 0.2f;

                if ((frameData.FrameType == 1) && inputDir != Vector2.zero)
                {
                    transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime));
                }
                if (frameData.FrameType == 2)
                {
                    Vector3 velocity = transform.forward * attack.AttackMovement + Vector3.up * velocityY;
                    charController.Move(velocity * Time.deltaTime);
                }
            }*/
        }
    }

    void Jump()
    {
        //Set vertical velocity to jump velocity when jump button is pressed, and character is grounded
        if (Input.GetButtonDown("Jump"))
        {
            if (charController.isGrounded && player.CurrentStamina > 0)
            {
                StepOffLedge = false;

                float jumpVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight);
                velocityY = jumpVelocity;

                player.UseStamina(jumpStaminaCost);
            }
        }
    }

    void Dash()
    {
        //Game decides on whether player is running based on whether he is holding the run button when grounded
        //or based on current speed when in air
        //"Running" in the air sounds counterintuitive, but this gives player a reason to do a running jump - covers more distance

        if (charController.isGrounded && Input.GetButton("Run") && player.CurrentStamina > 0)
        {
            dashing = true;

            if (inputDir != Vector2.zero)
            {
                player.UseStamina(dashStaminaCost * Time.deltaTime);
            }            
        }
        else if (!charController.isGrounded && (currentSpeed >= moveSpeed + 1f))
        {
            dashing = true;
        }
        else
        {
            dashing = false;
        }
    }

    //Types of frame when performing actions
    /*
    1 = Start Up / Unbufferable Recovery
    2 = Active
    3 = Bufferable Recovery
    4 = Cancellable
    
    5 = Invincibility
    0 = Nothing
    */

    //Handles beginning a dodge
    void InitiateDodge(Vector2 inputDir)
    {
        //Direction player dodges towards
        float dodgeDirection;

        //print(frameData.ActionName);

        //When player presses dodge button, or if a dodge input is stored in the input buffer
        if ((Input.GetButtonDown("Dodge") || inputBuffer.BufferedInput == "Dodge") /*&& frameData.ActionName != "roll"*/)
        {
            //When the player character is doing nothing, or in a cancellable part of an animation
            if (frameData.FrameType == 0 || frameData.FrameType == 4)
            {
                //If character is grounded and has sufficient stamina
                if (charController.isGrounded && player.CurrentStamina > 0)
                {
                    anim.SetInteger("reactNumber", 0);

                    //Spends stamina
                    player.UseStamina(rollStaminaCost);
                    
                    //Do roll1 when doing nothing or finishing attack etc.
                    //Do roll2 when finishing roll1
                    if (frameData.ActionName == "roll")
                    {
                        frameData.ActionName = "roll2";
                        anim.SetInteger("attackNumber", -2);
                    }
                    else
                    {
                        frameData.ActionName = "roll";
                        anim.SetInteger("attackNumber", -1);
                    }

                    frameData.FrameType = 1;

                    //Cancels previous movement
                    currentSpeed = 0;

                    //Dodges in direction of directional input
                    //or in direction player character is facing if there is no direction input
                    if (inputDir != Vector2.zero)
                    {
                        dodgeDirection = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg;
                    }
                    else
                    {
                        dodgeDirection = transform.eulerAngles.y;
                    }

                    transform.eulerAngles = Vector3.up * dodgeDirection;

                    //Sets current dodge speed to the set initial dodge speed
                    currentDodgeSpeed = initialDodgeSpeed;
                }
                
                //Empties input buffer as buffered input has been acted upon
                inputBuffer.BufferedInput = null;
                //print(initialDodgeSpeed);
            }
        }        
    }

    //Handles movement throughout dodge duration
    void DuringDodge()
    {
        if (frameData.ActionName == "roll" || frameData.ActionName == "roll2")
        {           
            //Smooths current dodge speed from initial speed to end speed
            currentDodgeSpeed = Mathf.SmoothDamp(currentDodgeSpeed, endDodgeSpeed, ref dodgeSmoothVelocity, GetModifiedSmoothTime(dodgeSmoothTime));

            /*if (currentDodgeSpeed <= (endDodgeSpeed + 0.1))
            {
                EndDodge();
            }*/

            //Moves with character controller
            Vector3 velocity = transform.forward * currentDodgeSpeed + Vector3.up * velocityY;
            charController.Move(velocity * Time.deltaTime);
        }
    }

    //Cancels dodge
    public void EndDodge()
    {
        frameData.ActionName = null;
        frameData.FrameType = 0;

        anim.SetInteger("attackNumber", 0);
    }

    void Interact()
    {

    }

    void Gravity()
    {
        //Player character falls faster than rises for weightier jump
        if (velocityY < 0 && !charController.isGrounded)
        {
            fallMultiplier = 4.5f;
        }
        else
        {
            fallMultiplier = 1;
        }

        //Presses player down onto ground when grounded for consistent groundcheck
        if (charController.isGrounded)
        {
            velocityY = -10;
            StepOffLedge = true;
        }
        //Stops grounded dowward force from making player character fall too fast from stepping off ledges
        else if (!charController.isGrounded && stepOffLedge)
        {
            velocityY = 0;
            StepOffLedge = false;
        }
        //Sets vertical velocity based on above
        else
        {
            velocityY += gravity * fallMultiplier * Time.deltaTime;
        }
    }

    void Dust()
    {
        ParticleSystem.MainModule dustMain = runDust.main;

        //Vary sizes of dust clouds based on player's current actions
        if ((dashing && charController.isGrounded && currentSpeed >= moveSpeed + 0.01) || (frameData.ActionName == "attackRunning" && (frameData.FrameType == 1 || frameData.FrameType == 2)))
        {
            dustMain.startSize = 2.1f;
        }
        else if ((!dashing && charController.isGrounded && currentSpeed >= moveSpeed - 0.01) || velocityY > 3)
        {
            dustMain.startSize = 0.9f;
        }
        else if (frameData.ActionName == "roll" || frameData.ActionName == "roll2")
        {
            dustMain.startSize = 1.6f;
        }
        else //if (!charController.isGrounded)
        {
            dustMain.startSize = 0f;
        }
    }

    //Player movement smoothed differently based on current state and action
    float GetModifiedSmoothTime(float smoothTime)
    {
        if (frameData.ActionName == "roll" || frameData.ActionName == "roll2")
        {
            if (groundDodgeSpeedSmooth == 0)
            {
                return float.MaxValue;
            }

            return smoothTime / groundDodgeSpeedSmooth;
        }

        if (charController.isGrounded)
        {
            if (frameData.ActionName == "attack1" || frameData.ActionName == "attack2" || frameData.ActionName == "attack3")
            {
                if (attackControlPercent == 0)
                {
                    return float.MaxValue;
                }

                return smoothTime / attackControlPercent;
            }

            if (!dashing)
            {
                return smoothTime;
            }
            else
            {
                if (dashControlPercent == 0)
                {
                    return float.MaxValue;
                }

                return smoothTime / dashControlPercent;
            }            
        }

        if (airControlPercent == 0)
        {
            return float.MaxValue;
        }

        return smoothTime / airControlPercent;
    }
}

//-------------------------------------------------------------------------
