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
            transform.eulerAngles = Vector3.up * Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg;
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

        //velocity.y = yVelocity;

        //velocity = transform.TransformDirection(velocity);

        //transform.eulerAngles = new Vector3(0, velocity.magnitude,0);

        charController.Move(velocity * Time.deltaTime);
	}
}
