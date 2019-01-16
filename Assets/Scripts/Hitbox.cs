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
                BloodEffect(other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position));
                DealHitDamage(other.gameObject);

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
        print("hit");

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
