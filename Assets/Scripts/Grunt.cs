using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grunt : Enemy
{
    Animator anim;

    int reactType;
    List<int> reactList;

    void Start () 
	{
        anim = GetComponent<Animator>();

        SetStats(500, 100, 40, 160, 90, 20); //Health: 500, Stamina: 100, ATK: 160, DEF: 90, Poise: 20
        SetHealthStaminaToMax();

        ReactListRefill();
    }	

	void Update () 
	{		

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

        base.HealthAdjust(type, amount);
    }
}
