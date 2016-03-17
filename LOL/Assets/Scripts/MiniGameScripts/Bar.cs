using UnityEngine;
using System.Collections;

public class Bar : MonoBehaviour
{
   // public GameObject bar;
    public Collider top;
    public Collider bottom;
    public Collider goodZone;
    bool Down;
    bool Up;
    public bool Good;
    float speed;
    // Use this for initialization
    void Start ()
    {
        speed = (float)0.1;
        Down = true;
        Good = false;

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (GameObject.Find("Lizard").GetComponent<PushUp>().stop == false)
        {
            //Debug.Log(gameObject.transform.position.z);
            if (Down)
            {
                gameObject.transform.Translate(0, 0, -speed);

            }
            else if (!Down)
            {
                gameObject.transform.Translate(0, 0, speed);
            }
        }


    }

    void OnTriggerEnter(Collider col)
    {
        if (col == top)
        {
            Down = true;
            //MoveDown();
        }
        if (col == bottom)
        {
            Down = false;
            //MoveUp();
        }
        if(col == goodZone)
        {
            speed = (float)0.5;
            Good = true;

        }

    }
    void OnTriggerExit(Collider col)
    {
        if(col == goodZone)
        {
            speed = (float)0.1;
            Good = false;
        }

    }


}
