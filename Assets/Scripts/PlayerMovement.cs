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
    [Range(0, 1), SerializeField]
    float airDodgeSpeedSmooth;
    [SerializeField]
    float airDodgeUpwardVelocity;

    float airDodgeTimer;
    [SerializeField]
    float airDodgeDuration = 0.5f;

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
    float initialAirDodgeSpeed = 8.0f;
    [SerializeField]
    float endAirDodgeSpeed = 0f;

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

    bool groundDodging = false;
    bool airDodging = false;

    public bool Dodging
    {
        get
        {
            return groundDodging;
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

        if (!groundDodging && !airDodging)
        {
            Movement(inputDir);
        }

        if (frameData.FrameType == 0 || frameData.FrameType == 4)
        {
            InitiateDodge(inputDir);
        }

        DuringDodge();
    }

    void Movement(Vector2 inputDir)
    {
        float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg;

        if (frameData.ActionName == null)
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
        else if (frameData.ActionName == "attack1" || frameData.ActionName == "attack2")
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

    void InitiateDodge(Vector2 inputDir)
    {
        float dodgeDirection;

        if ((Input.GetButtonDown("Dodge") || inputBuffer.BufferedInput == "Dodge") && !(groundDodging || airDodging))
        {
            frameData.ActionName = "roll";
            currentSpeed = 0;
            //frameData.FrameType = 0;

            if (charController.isGrounded)
            {
                groundDodging = true;
                airDodging = false;
            }
            else
            {
                airDodging = true;
                groundDodging = false;

                airDodgeTimer = airDodgeDuration;
                velocityY = airDodgeUpwardVelocity;
            }

            if (inputDir != Vector2.zero)
            {
                dodgeDirection = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg;
            }
            else
            {
                dodgeDirection = transform.eulerAngles.y;
            }

            transform.eulerAngles = Vector3.up * dodgeDirection;
            currentDodgeSpeed = (groundDodging) ? initialDodgeSpeed : initialAirDodgeSpeed;

            inputBuffer.BufferedInput = null;
            //print(initialDodgeSpeed);
        }        
    }

    void DuringDodge()
    {
        if (groundDodging)
        {
            anim.SetBool("groundDodge", true);

            /*if (!charController.isGrounded)
            {
                EndDodge();
                print("rolled off edge");
            }*/

            currentDodgeSpeed = Mathf.SmoothDamp(currentDodgeSpeed, endDodgeSpeed, ref dodgeSmoothVelocity, GetModifiedSmoothTime(dodgeSmoothTime));

            if (currentDodgeSpeed <= (endDodgeSpeed + 0.1))
            {
                EndDodge();
            }

            Vector3 velocity = transform.forward * currentDodgeSpeed + Vector3.up * velocityY;
            charController.Move(velocity * Time.deltaTime);
        }
        else if (airDodging)
        {
            anim.SetBool("airDodge", true);

            if (charController.isGrounded || airDodgeTimer <= 0)
            {
                EndDodge();
            }
            else
            {
                currentDodgeSpeed = Mathf.SmoothDamp(currentDodgeSpeed, endAirDodgeSpeed, ref dodgeSmoothVelocity, GetModifiedSmoothTime(dodgeSmoothTime));

                Vector3 velocity = transform.forward * currentDodgeSpeed + Vector3.up * velocityY;
                charController.Move(velocity * Time.deltaTime);

                airDodgeTimer -= Time.deltaTime;
            }
        }
    }

    public void EndDodge()
    {
        frameData.ActionName = null;
        frameData.FrameType = 0;

        anim.SetBool("groundDodge", false);
        groundDodging = false;

        anim.SetBool("airDodge", false);
        airDodging = false;
    }

    void Gravity()
    {
        if (airDodging)
        {
            fallMultiplier = 4f;
        }
        else if (velocityY < 0 && !charController.isGrounded)
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
        if (groundDodging)
        {
            if (groundDodgeSpeedSmooth == 0)
            {
                return float.MaxValue;
            }

            return smoothTime / groundDodgeSpeedSmooth;
        }
        else if (airDodging)
        {
            if (airDodgeSpeedSmooth == 0)
            {
                return float.MaxValue;
            }

            return smoothTime / airDodgeSpeedSmooth;
        }

        if (charController.isGrounded)
        {
            if (frameData.ActionName == "attack1" || frameData.ActionName == "attack2")
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
