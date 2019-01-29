using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
	void Start ()
	{
        SetStats(500, 100, 40, 160, 90, 20); //Health: 500, Stamina: 100, ATK: 160, DEF: 90, Poise: 20
        SetHealthStaminaToMax();
    }
	
	void Update ()
	{
		
	}

    public override void HealthAdjust(string type, int amount, Vector3 knockback)
    {
        base.HealthAdjust(type, amount, knockback);
    }
}
