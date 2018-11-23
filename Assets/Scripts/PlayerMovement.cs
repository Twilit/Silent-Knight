using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController charController;

    [SerializeField]
    float jumpHeight = 5.0f;
    [SerializeField]
    float gravity = -1.0f;
    [Range(0, 1), SerializeField]
    float airControlPercent;
    [Range(0, 1), SerializeField]
    float dashControlPercent;

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
    }

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
    }
	
	void Update ()
	{
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        inputDir = input.normalized;

        Gravity();

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        Movement(inputDir);
    }

    void Movement(Vector2 inputDir)
    {
        Dash();

        if (inputDir != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg;

            //int smoothedAngle = 120;
            float turnSmoothTime = 0.15f;

            /*if ((((Mathf.Sign(targetRotation) == -1) ? (targetRotation + 360) : targetRotation) > (((Mathf.Sign(transform.eulerAngles.y + smoothedAngle) == -1) ? (transform.eulerAngles.y + smoothedAngle + 360) : transform.eulerAngles.y + smoothedAngle)) ||
                (((Mathf.Sign(targetRotation) == -1) ? (targetRotation + 360) : targetRotation) < (((Mathf.Sign(transform.eulerAngles.y - smoothedAngle) == -1) ? (transform.eulerAngles.y - smoothedAngle - 360) : transform.eulerAngles.y - smoothedAngle)))))*/

            if (/*Quaternion.Angle(Quaternion.Euler(Vector3.up * targetRotation), transform.rotation) > smoothedAngle || */justStartedMoving && charController.isGrounded && !dashing)
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

    void Jump()
    {
        if (charController.isGrounded)
        {
            stepOffLedge = false;

            float jumpVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight);
            velocityY = jumpVelocity;
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
        if (charController.isGrounded)
        {
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
