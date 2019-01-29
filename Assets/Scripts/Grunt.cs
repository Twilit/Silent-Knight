using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grunt : Enemy
{
    Animator anim;
    UnityEngine.AI.NavMeshAgent agent;
    Transform target;

    int reactType;
    List<int> reactList;

    void Start () 
	{
        anim = GetComponent<Animator>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        target = GameObject.FindGameObjectWithTag("Player").transform;

        SetStats(500, 100, 40, 160, 90, 20); //Health: 500, Stamina: 100, ATK: 160, DEF: 90, Poise: 20
        SetHealthStaminaToMax();

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

        agent.Move(knockback);

        base.HealthAdjust(type, amount, knockback);
    }

    void Chase()
    {
        agent.SetDestination(target.position);
        anim.SetFloat("velocityY", agent.velocity.magnitude, 0.1f, Time.deltaTime);

        if (agent.remainingDistance < (agent.stoppingDistance + 2.5f))
        {
            transform.LookAt(target.transform);
        }
    }
}
