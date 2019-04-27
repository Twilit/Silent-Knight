using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grunt : Enemy
{
    Animator anim;

    UnityEngine.AI.NavMeshAgent agent;
    public Transform target;
    Vector3 destination;
    Transform player;
    public GameObject detectBox;

    [SerializeField, Range(0,360)]
    float walkDir;

    int reactType;
    List<int> reactList;
    bool stunOver;

    public bool StunOver
    {
        get
        {
            return stunOver;            
        }

        set
        {
            if (stunOver == false && value == true)
            {
                if (!anim.GetCurrentAnimatorStateInfo(0).IsTag("react"))
                {
                    stunOver = value;
                    agent.isStopped = false;
                    currentState = State.Engaged;
                }
            }       
            
            stunOver = false;
        }
    }

    bool attackOver;

    public bool AttackOver
    {
        get
        {
            return attackOver;
        }

        set
        {

        }
    }

    float walkSpeed;
    float runSpeed;

    public Grunt()
    {
        walkSpeed = 1.5f;
        runSpeed = 3f;

        currentState = State.Idle;
    }

    void Start () 
	{
        anim = GetComponent<Animator>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        player = GameObject.FindGameObjectWithTag("Player").transform;

        SetStats(500, 100, 40, 160, 90, 20); //Health: 500, Stamina: 100, ATK: 160, DEF: 90, Poise: 20
        SetHealthStaminaToMax();

        ReactListRefill();
    }	

	void Update () 
	{
        GruntAI();
        print(currentState);
	}

    void GruntAI()
    {
        switch (currentState)
        {
            case State.Idle:

                if (detectBox == null)
                {
                    SetDestination();

                    currentState = State.Chase;
                }

                break;

            case State.Patrol:

                if (detectBox == null)
                {
                    currentState = State.Engaged;
                }

                break;

            case State.Engaged:

                MoveTowardsTarget(target.position);                

                if (agent.remainingDistance > 7f)
                {
                    currentState = State.Chase;
                }
                else if (agent.remainingDistance < 2f)
                {
                    Attack();
                }

                break;

            case State.Attacking:
                               
                break;

            case State.Chase:

                RunToPlayer();

                if (agent.remainingDistance < 3f)
                {
                    currentState = State.Engaged;
                }

                break;

            case State.Stunned:

                

                break;
        }
    }

    void ReactListRefill()
    {
        reactList = new List<int>();
        for (int n = 0; n < 5; n++)
        {
            reactList.Add(n);
        }
    }

    public override void HealthAdjust(string type, int amount)
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

        agent.isStopped = true;
        currentState = State.Stunned;

        //KnockedBack(knockback);

        base.HealthAdjust(type, amount);
    }

    void Attack()
    {
        agent.isStopped = true;
        currentState = State.Attacking;

        int move = Random.Range(1, 5);

        anim.SetInteger("attackNumber", move);
        anim.SetTrigger("attack");
        print("attack: " + move);
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

        //print(Vector2.SignedAngle(faceDir, _velocityDir));

        float _walkDir = (Vector2.SignedAngle(faceDir, _velocityDir) + 90) * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(_walkDir), Mathf.Sin(_walkDir));
    }

    void MoveTowardsTarget(Vector3 targetPos)
    {
        agent.speed = 1.5f;
        agent.SetDestination(targetPos);

        anim.SetFloat("velocityY", DetermineDir(agent.velocity).y * (agent.velocity.magnitude / agent.speed), 0.02f, Time.deltaTime);
        anim.SetFloat("velocityX", DetermineDir(agent.velocity).x * (agent.velocity.magnitude / agent.speed), 0.02f, Time.deltaTime);

        LookTowardsTarget();
    }

    void LookTowardsTarget()
    {
        Vector3 targetDir = (player.position - transform.position);

        targetDir = targetDir / targetDir.magnitude;

        Vector3 newDir = Vector3.RotateTowards(transform.forward, new Vector3(targetDir.x, 0, targetDir.z), 4f * Time.deltaTime, 0.0f);

        transform.rotation = Quaternion.LookRotation(newDir);
    }

    void RunToPlayer()
    {
        anim.SetFloat("velocityY", 2, 0.05f, Time.deltaTime);

        agent.speed = 3f;
        agent.SetDestination(player.position);

        LookTowardsTarget();
    }

    void SetDestination()
    {
        //Vector2 offset = Random.insideUnitCircle;

        destination = target.position; //+ new Vector3(offset.x, 0, offset.y);
    }

    void KnockedBack(Vector3 amount)
    {
        agent.Move(amount);
    }
}
