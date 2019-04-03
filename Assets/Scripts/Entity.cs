//Kin Long Sha SHA17002700, Solihull College, VR & Games Design 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{    
    protected class EntityStats
    {
        //Health
        private int maxHealth = 1000;
        private int currentHealth;

        //Stamina
        private float maxStamina = 200f;
        private float currentStamina;
        private float staminaPerSec = 40f;

        //Attack
        private int attackPower = 300;

        //Defence
        private int defence = 150;

        //Poise
        private float poise = 0f;

        protected EntityStats(int mHP, int cHP, float mST, float cST, float rST, int AT, int DF, float PS)
        {
            maxHealth = mHP;
            currentHealth = cHP;

            maxStamina = mST;
            currentStamina = cST;
            staminaPerSec = rST;

            attackPower = AT;
            defence = DF;

            poise = PS;
        }

        protected EntityStats()
        {
            maxHealth = 1000;
            currentHealth = maxHealth;

            maxStamina = 200f;
            currentStamina = maxStamina;
            staminaPerSec = 40f;

            attackPower = 300;
            defence = 150;

            poise = 0f;
        }
    }
    
    //Health
    protected int maxHealth;
    protected int currentHealth;

    //Stamina
    protected float maxStamina;
    protected float currentStamina;
    protected float staminaPerSec;

    //Attack
    protected int attackPower;

    //Defence
    protected int defence;

    //Poise
    protected float poise;

    //Constructor
    public Entity()
    {
        maxHealth = 1000;
        maxStamina = 200f;
        staminaPerSec = 40f;

        attackPower = 300;
        defence = 300;

        poise = 0;
    }

    public float MaxStamina
    {
        get { return maxStamina; }
    }

    public float CurrentStamina
    {
        get { return currentStamina; }
    }

    public int MaxHealth
    {
        get { return maxHealth; }
    }

    public int CurrentHealth
    {
        get { return currentHealth; }
    }

    //Does exactly what it says on the tin, used when setting up
    protected void SetHealthStaminaToMax()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
    }

    protected void SetStats(int _maxHealth, float _maxStamina, float _staminaPerSec, int _attackPower, int _defence, float _poise)
    {
        maxHealth = _maxHealth;
        maxStamina = _maxStamina;
        staminaPerSec = _staminaPerSec;
        attackPower = _attackPower;
        defence = _defence;
        poise = _poise;

        //Debug.Log(maxHealth + "/" + maxStamina + "/" + staminaPerSec + "/" + attackPower + "/" + defence + "/" + poise);
    }

    //Regens either health or stamina, mostly used for regular stamina recovery
    //For stamina, should be constantly active, as long as conditions are met for player to be able to regain stamina
    //For health, unsure, perhaps an item/ability/spell effect
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

    //Called when player does action that costs stamina, with the action's stamina cost as parameter
    public void UseStamina(float cost)
    {
        if (currentStamina > 0)
        {
            currentStamina -= cost;
        }
    }

    public virtual void HealthAdjust(string type, int amount)
    {
        if (type == "damage")
        {
            if (currentHealth > 0)
            {
                currentHealth -= amount;
            }
        }
        else if (type == "heal")
        {
            if (currentHealth > 0 && currentHealth < maxHealth)
            {
                if ((currentHealth + amount) > maxHealth)
                {
                    currentHealth = maxHealth;
                }
                else
                {
                    currentHealth += amount;
                }
            }
        }
    }

    public virtual void Death()
    {
        gameObject.SetActive(false);
    }
}
