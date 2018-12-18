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

    float backFillAmount;
    [SerializeField]
    float backFillDelay = 3f;
    float backFillTimer;
    float mainFillAmount;
    Entity entityStats;

    void Start()
    {
        entityStats = entity.GetComponent<Player>();
    }

	void LateUpdate () 
	{
        CatchUpTimer();
        UpdateStaminaBar();
    }

    void UpdateStaminaBar()
    {
        print(entityStats.CurrentStamina + "/" + entityStats.MaxStamina);

        mainFillAmount = entityStats.CurrentStamina / entityStats.MaxStamina;

        if (mainFill.fillAmount != mainFillAmount)
        {
            mainFill.fillAmount = mainFillAmount;

            if (mainFillAmount < backFillAmount && backFillTimer <= 0)
            {
                backFillTimer = backFillDelay;
            }
            else if (mainFillAmount > backFillAmount)
            {
                backFillAmount = mainFillAmount;
            }
        }
    }

    void CatchUpTimer()
    {
        if (backFillTimer > 0)
        {
            backFillTimer -= Time.deltaTime;
        }        
    }
}
