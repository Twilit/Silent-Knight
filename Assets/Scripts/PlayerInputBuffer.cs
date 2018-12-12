//Kin Long Sha SHA17002700, Solihull College, VR & Games Design

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputBuffer : MonoBehaviour
{    
    FrameData frameData;

    string bufferedInput = null;

    public string BufferedInput
    {
        get { return bufferedInput; }

        set { bufferedInput = null; }
    }

    void Start ()
    {
        frameData = GetComponent<FrameData>();
    }
    
	void Update ()
    {
        //Store input if in frames of animation that allows you to buffer an action
        if ((frameData.FrameType == 2 || frameData.FrameType == 3) && GetInput() != null)
        {
            //bufferedInput = (GetInput() != bufferedInput) ? GetInput() : bufferedInput;
            bufferedInput = GetInput();

            //print(bufferedInput);
        }
	}

    //Actions you can buffer
    string GetInput()
    {
        if (Input.GetButtonDown("Attack"))
        {
            return "Attack";
        }
        else if (Input.GetButtonDown("Dodge"))
        {
            return "Dodge";
        }
        else
        {
            return null;
        }
    }
}
