//Kin Long Sha SHA17002700, Solihull College, VR & Games Design 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    GameObject player;
    Vector3 offset;

	void Start ()
	{
        player = GameObject.FindWithTag("Player");
        offset = transform.position - player.transform.position;
	}
	
	void LateUpdate ()
	{
        transform.position = player.transform.position + offset;
	}
}
