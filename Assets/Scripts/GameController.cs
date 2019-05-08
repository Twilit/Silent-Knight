using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    

    public static GameController gCInstance;

    GameObject playerInLevel;

    enum Room { One, Two, Three, Four, Five, Six, };
    Room currentRoom;

    public int healthUpgradeLevel = 0;
    public int staminaUpgradeLevel = 0;

    public class EnemyPoint
    {
        public GameObject enemyObject;
        public Transform enemyTrans;

        public EnemyPoint(GameObject newEnemyObject, Transform newEnemyPosition)
        {
            enemyObject = newEnemyObject;
            enemyTrans = newEnemyPosition;
        }
    }

    List<EnemyPoint> enemies = new List<EnemyPoint>();

    [SerializeField]
    GameObject su;
    [SerializeField]
    GameObject hu;

    public Transform SU1;
    public Transform SU2;
    public Transform SU3;
    public Transform SU4;

    public Transform HU1;
    public Transform HU2;
    public Transform HU3;
    public Transform HU4;

    public static bool shortcut = false;
    public static bool endDoor = false;

    public GameController()
    {
        currentRoom = Room.One;
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (gCInstance == null)
        {
            gCInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        SpawnPickUps();

        var enemiesInLevel = GameObject.FindGameObjectsWithTag("Enemy");

        /*
        foreach(GameObject enemy in enemiesInLevel)
        {
            enemies.Add(new EnemyPoint(enemy, enemy.transform));
        }
        foreach(EnemyPoint enemyPoint in enemies)
        {
            print(enemyPoint.enemyObject + " " + enemyPoint.enemyPosition);
        }*/
    }

    void Update()
    {
        //print("Health: " + healthUpgradeLevel + " / Stamina: " + staminaUpgradeLevel);
    }

    void SpawnAll()
    {
        foreach (EnemyPoint enemyPoint in enemies)
        {


            enemyPoint.enemyObject.GetComponent<Grunt>().enabled = true;
            enemyPoint.enemyObject.GetComponent<Grunt>().Teleport(enemyPoint.enemyTrans);

            enemyPoint.enemyObject.GetComponent<Grunt>().Respawn();
        }
    }

    void SpawnPickUps()
    {
        Instantiate(su, SU1.position, Quaternion.Euler(-90, 0, 0));
        Instantiate(su, SU2.position, Quaternion.Euler(-90, 0, 0));
        Instantiate(su, SU3.position, Quaternion.Euler(-90, 0, 0));
        Instantiate(su, SU4.position, Quaternion.Euler(-90, 0, 0));

        Instantiate(hu, HU1.position, Quaternion.Euler(-90, 0, 0));
        Instantiate(hu, HU2.position, Quaternion.Euler(-90, 0, 0));
        Instantiate(hu, HU3.position, Quaternion.Euler(-90, 0, 0));
        Instantiate(hu, HU4.position, Quaternion.Euler(-90, 0, 0));
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(0);
    }
}
