using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour 
{
    public GameObject player;

    PlayerAttack attack;
    FrameData frameData;

    List<GameObject> hitEnemies;

    void Start () 
	{
        hitEnemies = new List<GameObject>();

        attack = player.GetComponent<PlayerAttack>();
        frameData = player.GetComponent<FrameData>();
	}	

	void Update () 
	{	
        if ((frameData.FrameType == 1) && (hitEnemies.Count != 0))
        {
            hitEnemies.Clear();
            print("reset List");
        }
	}

    private void OnTriggerStay(Collider other)
    {
        if (frameData.FrameType == 2 && InAttack())
        {
            if (!hitEnemies.Contains(other.gameObject))
            {
                hitEnemies.Add(other.gameObject);

                DealHitDamage(other.gameObject);
            }
        }
    }

    bool InAttack()
    {
        if (frameData.ActionName == "attack1"
            || frameData.ActionName == "attack2"
            || frameData.ActionName == "attackRunning"
            || frameData.ActionName == "attackRolling"
            || frameData.ActionName == "attackMidAir")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void DealHitDamage(GameObject hitEnemy)
    {
        print("hit");

        try
        {
            
        }
        catch
        {

        }
    }
}
