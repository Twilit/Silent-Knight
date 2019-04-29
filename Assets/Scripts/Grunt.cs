using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grunt : Enemy
{
    public int move;

    Animator anim;
    EnemyFrameData eframe;

    UnityEngine.AI.NavMeshAgent agent;
    public Transform target;
    Vector3 destination;
    Transform player;
    public GameObject detectBox;

    [SerializeField, Range(0,360)]
    float walkDir;

    int reactType;
    List<int> reactList;
    bool stunAnimationOver;
    float stunTimer = 0;

    float currentPoise;

    public bool StunAnimationOver
    {
        get
        {
            return stunAnimationOver;            
        }

        set
        {
            if (stunAnimationOver == false && value == true)
            {
                if (!anim.GetCurrentAnimatorStateInfo(0).IsTag("react"))
                {
                    stunAnimationOver = value;

                    anim.SetFloat("velocityX", 0);
                    anim.SetFloat("velocityY", 0);
                }
            }       
            
            stunAnimationOver = false;
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
            if (attackOver == false && value == true)
            {
                if (!anim.GetCurrentAnimatorStateInfo(0).IsTag("attack"))
                {
                    attackOver = value;

                    anim.SetFloat("velocityX", 0);
                    anim.SetFloat("velocityY", 0);

                    if (currentState == State.Attacking)
                    {
                        agent.speed = 1.5f;
                        agent.SetDestination(player.position);
                        currentState = State.Engaged;
                    }                    
                }
            }

            attackOver = false;
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
        eframe = GetComponent<EnemyFrameData>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        player = GameObject.FindGameObjectWithTag("Player").transform;

        SetStats(1000, 100, 40, 160, 90, 100); //Health: 500, Stamina: 100, ATK: 160, DEF: 90, Poise: 20
        SetHealthStaminaToMax();

        currentPoise = poise;

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

                    currentState = State.Alerted;
                }

                break;

            case State.Patrol:

                if (detectBox == null)
                {
                    currentState = State.Engaged;
                }

                break;

            case State.Alerted:
                
                RunToPlayer();

                if (agent.remainingDistance < 2f)
                {
                    currentState = State.Engaged;
                }

                break;

            case State.Engaged:

                MoveTowardsTarget(target.position);                

                if (agent.remainingDistance > 5f)
                {
                    currentState = State.Chase;
                }
                else if (agent.remainingDistance < 1.3f && TargetInFront())
                {
                    Attack();
                }

                break;

            case State.Attacking:

                if (eframe.EFrameType == 1)
                {
                    LookTowardsTarget();
                }
                else if (eframe.EFrameType == 2)
                {
                    //agent.Move(transform.forward * 2f * Time.deltaTime);
                    //print("in ft2");                    
                }

                break;

            case State.Chase:

                RunToPlayer();

                if (agent.remainingDistance < 1.3f && TargetInFront())
                {
                    Attack();
                }

                break;

            case State.Stunned:

                if (stunTimer > 0)
                {
                    stunTimer -= Time.deltaTime;
                }
                else if (stunTimer <= 0)
                {
                    agent.SetDestination(player.position);

                    currentState = State.Engaged;
                }

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

    bool TargetInFront()
    {
        Vector3 heading = target.position - transform.position;

        float dot = Vector3.Dot(heading, transform.forward);

        if (dot > 0.5f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void HealthAdjust(string type, int amount)
    {
        base.HealthAdjust(type, amount);

        if (currentHealth <= 0)
        {
            anim.SetTrigger("die");            
            agent.isStopped = true;
            agent.enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;
            enabled = false;
        }
        else
        {
            if (stunTimer <= 0)
            {
                currentPoise -= (amount / 4);
            }

            if (currentPoise <= 0)
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

                agent.speed = 0;
                stunTimer = 1f;
                currentPoise = poise;

                currentState = State.Stunned;
            }
        }      
    }

    void Attack()
    {
        agent.speed = 0;
        currentState = State.Attacking;

        move = Random.Range(1, 5);

        anim.SetInteger("attackNumber", move);
        anim.SetTrigger("attack");       

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
