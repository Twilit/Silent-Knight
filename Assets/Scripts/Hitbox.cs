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

    public bool LandedHit
    {
        get
        {
            return !(hitEnemies.Count == 0);
        }
    }

    List<GameObject> hitEnemies;
    bool inHitStop;
    float lastCollisionTime;

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
        /*if (Time.time < lastCollisionTime + 0.1f)
        {
            return;
        }

        lastCollisionTime = Time.time;

        print("enter: " + frameData.ActionName);*/
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
            //Knockback sending the enemy away from the player
            //Vector2 playerCentre = new Vector2(transform.root.position.x, transform.root.position.z) + new Vector2(player.GetComponent<CharacterController>().center.x, player.GetComponent<CharacterController>().center.z);
            //Vector2 enemyCentre = new Vector2(hitEnemy.transform.position.x, hitEnemy.transform.position.z) + new Vector2(hitEnemy.GetComponent<CapsuleCollider>().center.x, hitEnemy.GetComponent<CapsuleCollider>().center.z);

            //Vector2 knockbackDir = (enemyCentre - playerCentre).normalized;

            //---

            //Knockback sending the enemy in the direction player is facing
            //Vector3 knockbackDir = transform.root.forward;

            //float knockbackSpeed = 0.4f;

            //Vector3 knockbackVelocity = new Vector3(knockbackDir.x, 0, knockbackDir.y)* knockbackSpeed;

            //Vector3 knockbackVelocity = new Vector3(knockbackDir.x, 0, knockbackDir.z)* knockbackSpeed;

            hitEnemy.GetComponent<Grunt>().HealthAdjust("damage", 20);
        }
        catch
        {
            print("oops");
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
