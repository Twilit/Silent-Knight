using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    protected enum State { Idle, Patrol, Chase, Engaged, Stunned };

    protected State currentState;

    void Start ()
	{
        SetStats(500, 100, 40, 160, 90, 20); //Health: 500, Stamina: 100, ATK: 160, DEF: 90, Poise: 20
        SetHealthStaminaToMax();
    }
	
	void Update ()
	{
		
	}

    public override void HealthAdjust(string type, int amount)
    {
        base.HealthAdjust(type, amount);
    }
}
