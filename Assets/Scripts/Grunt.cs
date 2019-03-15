using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grunt : Enemy
{
    Animator anim;
    UnityEngine.AI.NavMeshAgent agent;
    public Transform target;
    Transform player;

    [SerializeField, Range(0,360)]
    float walkDir;

    int reactType;
    List<int> reactList;

    void Start () 
	{
        anim = GetComponent<Animator>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        player = GameObject.FindGameObjectWithTag("Player").transform;

        SetStats(500, 100, 40, 160, 90, 20); //Health: 500, Stamina: 100, ATK: 160, DEF: 90, Poise: 20
        SetHealthStaminaToMax();

        currentState = State.Idle;

        ReactListRefill();
    }	

	void Update () 
	{
        Chase();
	}

    void ReactListRefill()
    {
        reactList = new List<int>();
        for (int n = 0; n < 5; n++)
        {
            reactList.Add(n);
        }
    }

    public override void HealthAdjust(string type, int amount, Vector3 knockback)
    {
        if (reactList.Count <= 1)
        {
            ReactListRefill();
        }

        int index = Random.Range(1, reactList.Count - 1);
        reactType = reactList[index];
        reactList.RemoveAt(index);

        anim.SetInteger("reactNumber", reactType);
        anim.SetTrigger("react");

        KnockedBack(knockback);

        base.HealthAdjust(type, amount, knockback);
    }

    Vector2 DetermineDir(Vector3 velocityDir)
    {
        Vector2 faceDir = new Vector2(transform.forward.x, transform.forward.z);
        /*
        print (faceDir);
        moveDir = (Quaternion.Euler(moveDir) * Quaternion.AngleAxis(transform.rotation.y, Vector3.up)).eulerAngles;

        return new Vector2(moveDir.normalized.x, moveDir.normalized.z);*/

        velocityDir = velocityDir.normalized;
        Vector2 _velocityDir = new Vector2(velocityDir.x, velocityDir.z);

        float _walkDir = (Vector2.SignedAngle(faceDir, _velocityDir) + 90) * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(_walkDir), Mathf.Sin(_walkDir));
    }

    void Chase()
    {
        agent.SetDestination(target.position);
        anim.SetFloat("velocityY", DetermineDir(agent.velocity).y/(agent.velocity.magnitude), 0.2f, Time.deltaTime);
        anim.SetFloat("velocityX", DetermineDir(agent.velocity).x, 0.2f, Time.deltaTime);

        if (/*agent.remainingDistance < (agent.stoppingDistance + 2.5f)*/ true)
        {
            Vector3 playerDir = (player.position - transform.position);

            playerDir = playerDir / playerDir.magnitude;

            transform.rotation = Quaternion.LookRotation(new Vector3(playerDir.x, 0, playerDir.z));
        }
    }

    void KnockedBack(Vector3 amount)
    {
        agent.Move(amount);
    }
}
