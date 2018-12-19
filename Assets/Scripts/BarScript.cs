using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarScript : MonoBehaviour 
{
    [SerializeField]
    GameObject entity;
    [SerializeField]
    Image backFill;
    [SerializeField]
    Image mainFill;
    [SerializeField]
    float barMaxDelta = 0.5f;

    float mainFillAmount;
    float baseMax;
    Entity entityStats;

    void Start()
    {
        entityStats = entity.GetComponent<Player>();
        baseMax = entityStats.MaxStamina;
    }

	void LateUpdate () 
	{
        UpdateBarFill(entityStats.CurrentStamina, entityStats.MaxStamina);
    }

    void UpdateBarFill(float current, float max)
    {
        //print(current + "/" + max);
        
        mainFillAmount = current / max;

        if (mainFill.fillAmount != mainFillAmount)
        {            
            mainFill.fillAmount = mainFillAmount;
        }

        if (mainFillAmount < backFill.fillAmount)
        {
            backFill.fillAmount = Mathf.MoveTowards(backFill.fillAmount, mainFillAmount, barMaxDelta * Time.deltaTime);
        }
        else
        {
            backFill.fillAmount = mainFillAmount;
        }
    }

    void UpdateBarSize()
    {
        if (baseMax != entityStats.MaxStamina)
        {

        }
    }
}