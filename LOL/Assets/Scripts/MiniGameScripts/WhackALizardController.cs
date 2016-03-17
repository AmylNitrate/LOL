using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class WhackALizardController : MonoBehaviour
{
    public static WhackALizardController instance;
    [SerializeField]
    List<GameObject> spawnPoints = new List<GameObject>();


	public GameObject lizard;
	public GameObject tail;

    public float baseSpawnTime;
    float spawnTime;

    public GameObject gameOver;
    bool stop;
    float timeLeft = 20;
    private Text textRef2;
    private Text textRef3;


    void Awake()
    {
        instance = this;
        GetRandomSpawnTime();

    }

    void Start()
    {
        Data.control.points = 0;
        stop = false;
        textRef2 = GameObject.Find("Timer").GetComponent<Text>();
        textRef3 = GameObject.Find("Points").GetComponent<Text>();

    }

    void Update()
    {
        if (!stop)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0)
            {
                stop = true;
                gameOver.SetActive(true);


            }
            if (spawnTime <= 0)
            {
                Spawn();
            }
            else
            {
                spawnTime -= Time.deltaTime;
            }
        }
        textRef2.text = "Timer = " + (int)timeLeft;
        textRef3.text = "Points = " + Data.control.points;

    }

    void GetRandomSpawnTime()
    {
        spawnTime = Random.Range(baseSpawnTime * 0.8f, baseSpawnTime * 1.2f);
    }

    void Spawn()
    {
        GameObject temp = spawnPoints[Random.Range(0, spawnPoints.Count)];
        Vector3 pos = temp.transform.position;
        GameObject tempLiz;
        if (temp.GetComponent<WhackHole>().isOccupied)
        {
            Spawn();
        }
        else
        {
            bool spawnTail = (Random.Range(0, 4) == 1) ? true : false;
            if (spawnTail)
            {
                tempLiz = Instantiate(tail, pos, Quaternion.identity) as GameObject;
            }
            else
            {
                tempLiz = Instantiate(lizard, pos, Quaternion.identity) as GameObject;
            }
            tempLiz.GetComponent<WhackLizard>().Init(temp);
            temp.GetComponent<WhackHole>().isOccupied = true;
            GetRandomSpawnTime();
        }
    }
}
