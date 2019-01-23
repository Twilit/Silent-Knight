using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour 
{
    public GameObject player;
    [SerializeField]
    GameObject bloodSplatter;

    PlayerAttack attack;
    Animator anim;
    FrameData frameData;

    [SerializeField]
    List<GameObject> hitEnemies;
    bool inHitStop;

    void Start () 
	{
        hitEnemies = new List<GameObject>();

        attack = player.GetComponent<PlayerAttack>();
        anim = player.GetComponent<Animator>();
        frameData = player.GetComponent<FrameData>();
	}	

	void Update () 
	{	
        if ((((frameData.FrameType == 1) && InAttack()) || frameData.ActionName == "" ) && (hitEnemies.Count != 0))
        {
            hitEnemies.Clear();
            StopCoroutine("HitStop");
            //print("reset List");
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        //print("enter: " + frameData.ActionName);
    }

    private void OnTriggerExit(Collider other)
    {
        //print("exit: " + frameData.ActionName);
    }

    private void OnTriggerStay(Collider other)
    {
        //print("colliding: " + frameData.ActionName);
        //Debug.LogWarning("Collided at: " + frameData.FrameType);
        if (frameData.FrameType == 2 && InAttack())
        {
            if (!hitEnemies.Contains(other.transform.root.gameObject))
            {
                hitEnemies.Add(other.transform.root.gameObject);
                BloodEffect(other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position));
                DealHitDamage(other.transform.root.gameObject);

                if (inHitStop)
                {
                    StopCoroutine("HitStop");
                }

                StartCoroutine("HitStop");
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
        //print("hit");

        try
        {
            
        }
        catch
        {

        }
    }

    void BloodEffect(Vector3 pos)
    {
        Instantiate(bloodSplatter, pos, Quaternion.identity);
    }

    IEnumerator HitStop()
    {
        anim.speed = 0f;
        inHitStop = true;

        yield return new WaitForSeconds(0.05f);

        for (float i = 0; i < 1; i += 0.1f)
        {
            anim.speed = i;
            yield return new WaitForSeconds(0.017f);
        }

        anim.speed = 1f;
        inHitStop = false;
    }
}
