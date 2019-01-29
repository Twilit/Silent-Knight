using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : MonoBehaviour 
{	
	void Start () 
	{	

	}	

	void Update () 
	{		

	}

    private void OnTriggerStay(Collider other)
    {
        try
        {
            other.transform.root.gameObject.GetComponent<Player>().HealthAdjust("damage", 70, Vector3.back);
            print("worked");
        }
        catch
        {
            print("?");
        }
    }
}
