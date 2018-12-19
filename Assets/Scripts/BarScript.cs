﻿using System.Collections;
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

    RectTransform barRect;
    RectTransform barMaskRect;
    RectTransform mainFillRect;
    RectTransform backFillRect;

    void Start()
    {
        entityStats = entity.GetComponent<Player>();

        barRect = GetComponent<RectTransform>();
        barMaskRect = transform.GetChild(0).GetComponent<RectTransform>();
        backFillRect = transform.GetChild(0).transform.GetChild(0).GetComponent<RectTransform>();
        mainFillRect = transform.GetChild(0).transform.GetChild(1).GetComponent<RectTransform>();

        baseMax = entityStats.MaxStamina;
    }

	void LateUpdate () 
	{
        UpdateBarFill(entityStats.CurrentStamina, entityStats.MaxStamina);

        UpdateBarSize();
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
        float offset = 0;

        if (baseMax != entityStats.MaxStamina)
        {
            offset = entityStats.MaxStamina - baseMax;
            float sizeInc = ((offset / baseMax) * (barRect.sizeDelta.x));

            barRect.sizeDelta = new Vector2((barRect.sizeDelta.x + sizeInc), barRect.sizeDelta.y);
            barRect.anchoredPosition = new Vector2(barRect.anchoredPosition.x + sizeInc/2, barRect.anchoredPosition.y);

            /*barMaskRect.sizeDelta = new Vector2(barMaskRect.sizeDelta.x + ((offset / baseMax) * (barRect.sizeDelta.x)), barMaskRect.sizeDelta.y);
            barMaskRect.anchoredPosition = new Vector2(barMaskRect.anchoredPosition.x + ((offset / baseMax) * (barRect.sizeDelta.x)) / 2, barMaskRect.anchoredPosition.y);

            backFillRect.sizeDelta = new Vector2(backFillRect.sizeDelta.x + ((offset / baseMax) * (barRect.sizeDelta.x)), backFillRect.sizeDelta.y);
            backFillRect.anchoredPosition = new Vector2(backFillRect.anchoredPosition.x + ((offset / baseMax) * (barRect.sizeDelta.x)) / 2, backFillRect.anchoredPosition.y);

            mainFillRect.sizeDelta = new Vector2(mainFillRect.sizeDelta.x + ((offset / baseMax) * (barRect.sizeDelta.x)), mainFillRect.sizeDelta.y);
            mainFillRect.anchoredPosition = new Vector2(mainFillRect.anchoredPosition.x + ((offset / baseMax) * (barRect.sizeDelta.x)) / 2, mainFillRect.anchoredPosition.y);*/

            baseMax = entityStats.MaxStamina;
        }
    }
}