using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public GameObject wood;
    
    // Start is called before the first frame update
    void Start()
    {
        if (wood.tag == "Shortcut")
        {
            if (!GameController.shortcut)
            {
                wood.SetActive(false);
            }
        }
        else if (wood.tag == "EndDoor")
        {
            if (!GameController.endDoor)
            {
                wood.SetActive(true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (wood.tag == "Shortcut")
            {
                if (!GameController.shortcut)
                {
                    GameController.shortcut = true;
                    transform.GetChild(1).localRotation = Quaternion.Euler(0, 0, 40);
                    wood.SetActive(true);
                }
            }
            else if (wood.tag == "EndDoor")
            {
                if (!GameController.endDoor)
                {
                    GameController.endDoor = true;
                    transform.GetChild(1).localRotation = Quaternion.Euler(0, 0, 40);
                    wood.SetActive(false);
                }
            }
        }
    }
}
