using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PushUp : MonoBehaviour
{

    Animation anim;
    float timeLeft = 20;
    public bool stop;
    public GameObject gameOver;

    private Text textRef2;
    private Text textRef3;


    void Start()
    {
        Data.control.points = 0;
        anim = GetComponent<Animation>();
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

            foreach (AnimationState state in anim)
            {
                state.speed = 0.3f;
            }


            if (Input.GetMouseButtonDown(0))
            {
                anim.Play("Take001");
                if (GameObject.Find("bar").GetComponent<Bar>().Good)
                {
                    Data.control.energy -= 1;
                    Data.control.points += 2;
                }
                if (!GameObject.Find("bar").GetComponent<Bar>().Good)
                {
                    Data.control.energy -= 2;
                    Data.control.points += 1;
                }
            }
            if (Input.GetMouseButtonUp(0))
                anim.Stop("Take001");
        }

        textRef2.text = "Timer = " + (int)timeLeft;
        textRef3.text = "Points = " + Data.control.points;
    }


}
