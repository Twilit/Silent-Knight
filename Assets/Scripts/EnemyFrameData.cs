using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFrameData : MonoBehaviour
{
    int eFrameType;
    Grunt grunt;
    AudioSource audio;

    [SerializeField]
    AudioClip melee;
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

        set
        {
            if (value == 2 && eFrameType != 2)
            {
                if (grunt.move != 3)
                {
                    HitboxActive(true, axeHitBox);

                    audio.clip = melee;
                    audio.Play();
                }
                else
                {
                    HitboxActive(true, footHitBox);
                }
            }
            else if (value != 2)
            {
                HitboxActive(false, axeHitBox);
                HitboxActive(false, footHitBox);
            }

            eFrameType = value;
        }
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
        audio = GetComponent<AudioSource>();
    }

    private void Update()
    {

    }
}
