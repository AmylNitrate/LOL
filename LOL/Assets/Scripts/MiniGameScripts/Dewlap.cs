using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Dewlap : MonoBehaviour
{

    public GameObject dewlap;
    public Collider dewlapCollider;
    float timeLeft = 15;

    public GameObject gameOver;
    bool stop;

    private Text textRef2;


    // Use this for initialization
    void Start ()
    {
        Data.control.points = 0;
        textRef2 = GameObject.Find("Timer").GetComponent<Text>();
        stop = false;
	}

    // Update is called once per frame
    void Update()
    {

        if (!stop)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0)
            {
                GetPointsValue();
                stop = true;
                gameOver.SetActive(true);

            }

            Debug.Log(dewlap.transform.position.x);

            if (dewlap.transform.position.x < 1.2)
            {
                dewlap.transform.Translate((float)-0.01, (float)0.01, 0);
            }
            else if (dewlap.transform.position.x < 1.6)
            {
                dewlap.transform.Translate((float)-0.005, (float)0.005, 0);
            }
            else if (dewlap.transform.position.x < 1.7)
            {
                dewlap.transform.Translate((float)-0.001, (float)0.001, 0);
            }
            else if(dewlap.transform.position.x > 1.8)
            {
                Debug.Log("Stop");
            }

            if (Input.GetMouseButtonDown(0) && dewlap.transform.position.x > 1.4)
            {
                Data.control.energy--;
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                    if (hit.collider == dewlapCollider)
                    {
                        if (dewlap.transform.position.x < 1.9 && dewlap.transform.position.x > 1.3)
                        {
            
                            dewlap.transform.Translate((float)0.06, (float)-0.06, 0);

                        }

                    }
                
            }
            else if (dewlap.transform.position.x < 1.4)
            {
                Debug.Log("STOP");
            }


        }

        textRef2.text = "Timer = " + (int)timeLeft;

    }

    void GetPointsValue()
    {
        if(dewlap.transform.position.x < 1.9 && dewlap.transform.position.x > 1.7)
        {
            Data.control.points = 5;
        }
        else if(dewlap.transform.position.x < 1.7 && dewlap.transform.position.x > 1.5)
        {
            Data.control.points = 10;
        }
        else if(dewlap.transform.position.x < 1.5 && dewlap.transform.position.x > 1.2)
        {
            Data.control.points = 20;
        }
    }


}

