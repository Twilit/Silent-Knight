using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFrameData : MonoBehaviour
{
    int eFrameType;
    Grunt grunt;

    [SerializeField]
    GameObject footHitBox;
    [SerializeField]
    GameObject axeHitBox;

    //1 = Start Up
    //2 = Active
    //0 = Nothing

    public int EFrameType
    {
        get { return eFrameType; }

        set { eFrameType = value; }
    }

    void EFrames(int getEFrameType)
    {
        EFrameType = getEFrameType;
    }

    void HitboxActive(bool on, GameObject hitbox)
    {
        if (on)
        {
            hitbox.SetActive(true);
        }
        else
        {
            hitbox.SetActive(false);
        }
    }

    private void Start()
    {
        grunt = GetComponent<Grunt>();
    }

    private void Update()
    {
        if (EFrameType == 2)
        {
            if (grunt.move != 3)
            {
                HitboxActive(true, axeHitBox);
            }
            else
            {
                HitboxActive(true, footHitBox);
            }
        }
        else
        {
            HitboxActive(false, axeHitBox);
            HitboxActive(false, footHitBox);
        }
    }
}
