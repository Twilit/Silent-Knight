using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    enum PickUpType { HealthUp, StaminaUp, HealthRestore, };
    GameController gameController;
       
    [SerializeField]
    PickUpType type;

    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (type == PickUpType.HealthRestore)
            {
                other.GetComponent<Player>().HealthAdjust("heal", 100);
            }
            else if (type == PickUpType.HealthUp)
            {
                gameController.healthUpgradeLevel += 1;
                other.GetComponent<Player>().Upgrade();
            }
            else if (type == PickUpType.StaminaUp)
            {
                gameController.staminaUpgradeLevel += 1;
                other.GetComponent<Player>().Upgrade();
            }

            gameObject.SetActive(false);
        }
    }
}
