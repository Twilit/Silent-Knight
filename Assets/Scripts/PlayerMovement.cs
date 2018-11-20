using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController charController;

    [SerializeField]
    float jumpSpeed = 5.0f;
    [SerializeField]
    float gravity = 1.0f;

    float yVelocity = 0.0f;

    [SerializeField]
    float moveSpeed = 5.0f;

    [SerializeField]
    float turnSmoothTime = 0.2f;
    float turnSmoothVelocity;

    public Vector2 input;

	void Start ()
	{
        charController = GetComponent<CharacterController>();
	}
	
	void Update ()
	{
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 inputDir = input.normalized;

        if (inputDir != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg;
            print("tr " + targetRotation);
            print("eul " + transform.eulerAngles.y);

            int smoothedAngle = 120;

            if ((((Mathf.Sign(targetRotation) == -1) ? (targetRotation + 360) : targetRotation) > (((Mathf.Sign(transform.eulerAngles.y + smoothedAngle) == -1) ? (transform.eulerAngles.y + smoothedAngle + 360) : transform.eulerAngles.y + smoothedAngle)) ||
                (((Mathf.Sign(targetRotation) == -1) ? (targetRotation + 360) : targetRotation) < (((Mathf.Sign(transform.eulerAngles.y - smoothedAngle) == -1) ? (transform.eulerAngles.y - smoothedAngle - 360) : transform.eulerAngles.y - smoothedAngle)))))
            {
                transform.eulerAngles = Vector3.up * targetRotation;
            }
            else
            {
                transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
            }

            //transform.eulerAngles = Vector3.up * targetRotation;

            //transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
        }
        
        if (charController.isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                yVelocity = jumpSpeed;
            }
        }
        else
        {
            yVelocity -= gravity;
        }
        
        Vector3 velocity = transform.forward * moveSpeed * inputDir.magnitude + Vector3.up * yVelocity;

        charController.Move(velocity * Time.deltaTime);
	}
}
