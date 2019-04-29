using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : MonoBehaviour 
{
    [SerializeField]
    GameObject bloodSplatter;
    [SerializeField]
    int damage;
    [SerializeField]
    AudioClip hit1;
    [SerializeField]
    AudioClip hit2;
    [SerializeField]
    AudioClip hit3;


    AudioSource audio;

    void Start () 
	{
        audio = GetComponent<AudioSource>();
	}	

	void Update () 
	{		

	}

    //Maybe set hitbox inactive after hit to solve multihit problem?

    private void OnTriggerEnter(Collider other)
    {
        if (true)
        {
            try
            {
                int hitSound = Random.Range(1, 3);

                if (hitSound == 1)
                {
                    audio.clip = hit1;
                }
                else if (hitSound == 2)
                {
                    audio.clip = hit2;
                }
                else if (hitSound == 3)
                {
                    audio.clip = hit3;
                }

                audio.Play();

                other.transform.root.gameObject.GetComponent<Player>().HealthAdjust("damage", damage);
                Instantiate(bloodSplatter, other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position), Quaternion.identity);
                transform.root.GetComponent<EnemyFrameData>().EFrameType = 0;
                gameObject.SetActive(false);
            }
            catch
            {
                
            }
        }
    }
}
