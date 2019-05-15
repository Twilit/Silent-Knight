using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StatusBar
{
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

            if (transform.tag == "HealthBar")
            {
                baseMax = 600; //entityStats.MaxHealth;
            }
            else if (transform.tag == "StaminaBar")
            {
                baseMax = 150; //entityStats.MaxStamina;
            }
        }

        void LateUpdate()
        {
            if (transform.tag == "HealthBar")
            {
                UpdateBarFill(entityStats.CurrentHealth, entityStats.MaxHealth);
                UpdateBarSize(entityStats.MaxHealth);
            }
            else if (transform.tag == "StaminaBar")
            {
                UpdateBarFill(entityStats.CurrentStamina, entityStats.MaxStamina);
                UpdateBarSize(entityStats.MaxStamina);
            }
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

        void UpdateBarSize(float max)
        {
            float offset = 0;

            if (baseMax != max)
            {
                offset = max - baseMax;

                float sizeInc = ((offset / baseMax) * (barRect.sizeDelta.x));
                barRect.sizeDelta = new Vector2((barRect.sizeDelta.x + sizeInc), barRect.sizeDelta.y);
                barRect.anchoredPosition = new Vector2(barRect.anchoredPosition.x + sizeInc / 2, barRect.anchoredPosition.y);

                sizeInc = ((offset / baseMax) * (barMaskRect.sizeDelta.x));
                barMaskRect.sizeDelta = new Vector2((barMaskRect.sizeDelta.x + sizeInc), barMaskRect.sizeDelta.y);
                barMaskRect.anchoredPosition = new Vector2(barMaskRect.anchoredPosition.x + sizeInc / 2, barMaskRect.anchoredPosition.y);

                sizeInc = ((offset / baseMax) * (backFillRect.sizeDelta.x));
                backFillRect.sizeDelta = new Vector2((backFillRect.sizeDelta.x + sizeInc), backFillRect.sizeDelta.y);
                backFillRect.anchoredPosition = new Vector2(backFillRect.anchoredPosition.x + sizeInc / 2, backFillRect.anchoredPosition.y);

                sizeInc = ((offset / baseMax) * (mainFillRect.sizeDelta.x));
                mainFillRect.sizeDelta = new Vector2((mainFillRect.sizeDelta.x + sizeInc), mainFillRect.sizeDelta.y);
                mainFillRect.anchoredPosition = new Vector2(mainFillRect.anchoredPosition.x + sizeInc / 2, mainFillRect.anchoredPosition.y);

                /*barMaskRect.sizeDelta = new Vector2(barMaskRect.sizeDelta.x + ((offset / baseMax) * (barRect.sizeDelta.x)), barMaskRect.sizeDelta.y);
                barMaskRect.anchoredPosition = new Vector2(barMaskRect.anchoredPosition.x + ((offset / baseMax) * (barRect.sizeDelta.x)) / 2, barMaskRect.anchoredPosition.y);

                backFillRect.sizeDelta = new Vector2(backFillRect.sizeDelta.x + ((offset / baseMax) * (barRect.sizeDelta.x)), backFillRect.sizeDelta.y);
                backFillRect.anchoredPosition = new Vector2(backFillRect.anchoredPosition.x + ((offset / baseMax) * (barRect.sizeDelta.x)) / 2, backFillRect.anchoredPosition.y);

                mainFillRect.sizeDelta = new Vector2(mainFillRect.sizeDelta.x + ((offset / baseMax) * (barRect.sizeDelta.x)), mainFillRect.sizeDelta.y);
                mainFillRect.anchoredPosition = new Vector2(mainFillRect.anchoredPosition.x + ((offset / baseMax) * (barRect.sizeDelta.x)) / 2, mainFillRect.anchoredPosition.y);*/

                baseMax = max;
            }
        }
    }
}