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

    [SerializeField]
    float moveSpeed = 5.0f;
    [SerializeField]
    float dashSpeed = 8.0f;
    [SerializeField]
    float speedSmoothTime = 0.1f;
    float speedSmoothVelocity;
    float currentSpeed;

    private float velocityY;

    public float VelocityY
    {
        get
        {
            return velocityY;
        }
    }

    public float SpeedSmoothTime
    {
        get
        {
            return speedSmoothTime;
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
        dashing = Input.GetButton("Run");

        if (inputDir != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg;

            //int smoothedAngle = 120;
            float turnSmoothTime = ((dashing) ? 0.5f : 0.15f);

            /*if ((((Mathf.Sign(targetRotation) == -1) ? (targetRotation + 360) : targetRotation) > (((Mathf.Sign(transform.eulerAngles.y + smoothedAngle) == -1) ? (transform.eulerAngles.y + smoothedAngle + 360) : transform.eulerAngles.y + smoothedAngle)) ||
                (((Mathf.Sign(targetRotation) == -1) ? (targetRotation + 360) : targetRotation) < (((Mathf.Sign(transform.eulerAngles.y - smoothedAngle) == -1) ? (transform.eulerAngles.y - smoothedAngle - 360) : transform.eulerAngles.y - smoothedAngle)))))*/

            if (/*Quaternion.Angle(Quaternion.Euler(Vector3.up * targetRotation), transform.rotation) > smoothedAngle || */justStartedMoving)
            {
                transform.eulerAngles = Vector3.up * targetRotation;                
            }
            else
            {
                transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
            }

            justStartedMoving = false;
        }
        else
        {
            justStartedMoving = true;
        }

        float targetSpeed = ((dashing) ? dashSpeed : moveSpeed) * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

        Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;

        charController.Move(velocity * Time.deltaTime);
    }

    void Jump()
    {
        if (charController.isGrounded)
        {
            float jumpVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight);
            velocityY = jumpVelocity;
        }
    }

    void Gravity()
    {
        float fallMultiplier = 1;

        if (velocityY < 0)
        {
            fallMultiplier = 2.5f;
        }
        else
        {
            fallMultiplier = 1;
        }

        if (charController.isGrounded)
        {
            velocityY = -1;
        }
        else
        {
            velocityY += gravity * fallMultiplier * Time.deltaTime;
        }
    }
}
