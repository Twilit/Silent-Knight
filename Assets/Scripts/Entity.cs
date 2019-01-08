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

    public float MaxStamina
    {
        get { return maxStamina; }
    }

    public float CurrentStamina
    {
        get { return currentStamina; }
    }

    public float MaxHealth
    {
        get { return maxHealth; }
    }

    public float CurrentHealth
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
}
