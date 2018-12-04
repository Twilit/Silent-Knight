using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController charController;
    Animator anim;
    PlayerAttack attack;
    FrameData frameData;
    PlayerInputBuffer inputBuffer;

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

    float fallMultiplier = 1;
    bool stepOffLedge;

    [SerializeField]
    float moveSpeed = 4.0f;

    public float MoveSpeed
    {
        get
        {
            return moveSpeed;
        }
    }

    [SerializeField]
    float dashSpeed = 6.0f;

    public float DashSpeed
    {
        get
        {
            return dashSpeed;
        }
    }

    [SerializeField]
    float speedSmoothTime = 0.1f;

    public float SpeedSmoothTime
    {
        get
        {
            return speedSmoothTime;
        }
    }

    float speedSmoothVelocity;
    float currentSpeed;
    
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

    float currentDodgeSpeed;

    [SerializeField]
    float initialDodgeSpeed = 5.0f;
    [SerializeField]
    float endDodgeSpeed = 2.0f;

    [SerializeField]
    float dodgeSmoothTime = 0.5f;

    float dodgeSmoothVelocity;

    private float velocityY;

    public float VelocityY
    {
        get
        {
            return velocityY;
        }
    }

    float turnSmoothVelocity;

    private Vector2 input;
    private Vector2 inputDir;

    public Vector2 InputDir
    {
        get
        {
            return inputDir;
        }
    }

    private bool dashing;
    private bool justStartedMoving = true;

    public bool Dashing
    {
        get
        {
            return dashing;
        }
    }

	void Start ()
	{
        charController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        attack = GetComponent<PlayerAttack>();
        frameData = GetComponent<FrameData>();
        inputBuffer = GetComponent<PlayerInputBuffer>();
    }
	
	void Update ()
	{
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        inputDir = input.normalized;

        Gravity();

        if (frameData.ActionName != "roll")
        {
            Movement(inputDir);
        }

        InitiateDodge(inputDir);

        DuringDodge();
    }

    void Movement(Vector2 inputDir)
    {
        float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg;

        if (frameData.ActionName == null || (frameData.ActionName == "attackMidAir"))
        {
            Jump();
            Dash();

            if (inputDir != Vector2.zero)
            {
                //int smoothedAngle = 120;
                float turnSmoothTime = 0.15f;

                /*if ((((Mathf.Sign(targetRotation) == -1) ? (targetRotation + 360) : targetRotation) > (((Mathf.Sign(transform.eulerAngles.y + smoothedAngle) == -1) ? (transform.eulerAngles.y + smoothedAngle + 360) : transform.eulerAngles.y + smoothedAngle)) ||
                    (((Mathf.Sign(targetRotation) == -1) ? (targetRotation + 360) : targetRotation) < (((Mathf.Sign(transform.eulerAngles.y - smoothedAngle) == -1) ? (transform.eulerAngles.y - smoothedAngle - 360) : transform.eulerAngles.y - smoothedAngle)))))*/

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
            
            float targetSpeed = ((dashing) ? dashSpeed : moveSpeed) * inputDir.magnitude;
            currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));

            Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;

            charController.Move(velocity * Time.deltaTime);
        }
        else if (frameData.ActionName == "attack1" || frameData.ActionName == "attack2" || frameData.ActionName == "attack3" || frameData.ActionName == "attackRolling")
        {
            float turnSmoothTime = 0.35f;

            if ((frameData.FrameType == 1 || frameData.FrameType == 2) && inputDir != Vector2.zero)
            {
                if (Quaternion.Angle(Quaternion.Euler(Vector3.up * targetRotation), transform.rotation) < 120)
                {
                    transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime));
                }
            }
            if (frameData.FrameType == 2)
            {
                Vector3 velocity = transform.forward * attack.AttackMovement + Vector3.up * velocityY;
                charController.Move(velocity * Time.deltaTime);
            }
        }
        else if (frameData.ActionName == "attackRunning" && (frameData.FrameType == 1 || frameData.FrameType == 2))
        {
            Vector3 velocity = transform.forward * attack.AttackMovement + Vector3.up * velocityY;
            charController.Move(velocity * Time.deltaTime);
        }
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
        else if (frameData.ActionName == "roll")
        {
            print("Roll");
        }
    }

    void Jump()
    {

        if (Input.GetButtonDown("Jump"))
        {
            if (charController.isGrounded)
            {
                stepOffLedge = false;

                float jumpVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight);
                velocityY = jumpVelocity;
            }
        }
    }

    void Dash()
    {
        if (charController.isGrounded && Input.GetButton("Run"))
        {
            dashing = true;
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

    /*
    1 = Start Up / Unbufferable Recovery
    2 = Active
    3 = Bufferable Recovery
    4 = Cancellable
    
    5 = Invincibility
    0 = Nothing
    */

    void InitiateDodge(Vector2 inputDir)
    {
        float dodgeDirection;
        //print(frameData.ActionName);
        if ((Input.GetButtonDown("Dodge") || inputBuffer.BufferedInput == "Dodge") /*&& frameData.ActionName != "roll"*/)
        {
            if (frameData.FrameType == 0 || frameData.FrameType == 4)
            {
                if (charController.isGrounded)
                {
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

                    currentSpeed = 0;

                    if (inputDir != Vector2.zero)
                    {
                        dodgeDirection = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg;
                    }
                    else
                    {
                        dodgeDirection = transform.eulerAngles.y;
                    }

                    transform.eulerAngles = Vector3.up * dodgeDirection;
                    currentDodgeSpeed = initialDodgeSpeed;
                }
                
                inputBuffer.BufferedInput = null;
                //print(initialDodgeSpeed);
            }
        }        
    }

    void DuringDodge()
    {
        if (frameData.ActionName == "roll" || frameData.ActionName == "roll2")
        {           
            
            currentDodgeSpeed = Mathf.SmoothDamp(currentDodgeSpeed, endDodgeSpeed, ref dodgeSmoothVelocity, GetModifiedSmoothTime(dodgeSmoothTime));

            /*if (currentDodgeSpeed <= (endDodgeSpeed + 0.1))
            {
                EndDodge();
            }*/

            Vector3 velocity = transform.forward * currentDodgeSpeed + Vector3.up * velocityY;
            charController.Move(velocity * Time.deltaTime);
            

        }
    }

    public void EndDodge()
    {
        frameData.ActionName = null;
        frameData.FrameType = 0;

        anim.SetInteger("attackNumber", 0);
    }

    void Gravity()
    {
        if (velocityY < 0 && !charController.isGrounded)
        {
            fallMultiplier = 4.5f;
        }
        else
        {
            fallMultiplier = 1;
        }

        if (charController.isGrounded)
        {
            velocityY = -10;
            stepOffLedge = true;
        }
        else if (!charController.isGrounded && stepOffLedge)
        {
            velocityY = 0;
            stepOffLedge = false;
        }
        else
        {
            velocityY += gravity * fallMultiplier * Time.deltaTime;
        }
    }

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
