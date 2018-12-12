//Kin Long Sha SHA17002700, Solihull College, VR & Games Design 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    //Health
    protected int maxHealth = 1000;
    protected int currentHealth;

    //Stamina
    protected float maxStamina = 200f;
    protected float currentStamina;
    protected float staminaPerSec = 40f;

    //Attack
    protected int attackPower = 300;

    //Defence
    protected int defence = 150;

    //Poise
    protected float poise = 0;



    public float CurrentStamina
    {
        get { return currentStamina; }
    }

    void Start ()
	{
        
	}
	
	void Update ()
	{
        
    }

    //Does exactly what it says on the tin, used when setting up
    protected void SetHealthStaminaToMax()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
    }

    //Regens either health or stamina, mostly stamina
    //For stamina, should be called in Update(), if entity is in frameType 0
    protected float Regen(float regenRate, float currentValue, float maxValue)
    {
        //Regen stamina when current value is less than max
        if (currentValue < maxValue)
        {
            currentValue += regenRate * Time.deltaTime;
            //print(currentValue);
        }

        //If current value goes over maximum
        if (currentValue > maxValue)
        {
            currentValue = maxValue;
        }

        return currentValue;
    }

    public void UseStamina(float cost)
    {
        if (currentStamina > 0)
        {
            currentStamina -= cost;
        }
    }
}
