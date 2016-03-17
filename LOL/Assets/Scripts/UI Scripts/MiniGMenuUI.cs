using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MiniGMenuUI : MonoBehaviour {
    private Text textRef2;
    private Text textRef3;
    private Text textRef4;
    // Use this for initialization
    void Start ()
    {
        textRef2 = GameObject.Find("PointsText").GetComponent<Text>();
        textRef3 = GameObject.Find("OppEnergy").GetComponent<Text>();
        textRef4 = GameObject.Find("OppPoints").GetComponent<Text>();

    }
	
	// Update is called once per frame
	void Update ()
  

{
        textRef2.text = "" + Data.control.points;
        textRef3.text = ""; // + Data.control.OppEnergy;
        textRef4.text = ""; // + Data.control.OppPoints;

    }
}
