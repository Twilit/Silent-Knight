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

    //Maybe set hitbox inactive after hit to solve multihit problem?

    private void OnTriggerEnter(Collider other)
    {
        if (true)
        {
            try
            {
                other.transform.root.gameObject.GetComponent<Player>().HealthAdjust("damage", 70);
            }
            catch
            {
                
            }
        }
    }
}
