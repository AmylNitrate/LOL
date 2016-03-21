using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StatMenuUI : MonoBehaviour, MPLobbyListener
{
    private Text textRef1;
    private Text textRef2;
    private Text textRef3;

    public Texture2D signOutButton;


    private string _lobbyMessage;


    void Start()
    {
        textRef1 = GameObject.Find("EnergyText").GetComponent<Text>();
        textRef2 = GameObject.Find("UpgradePoints").GetComponent<Text>();
        textRef3 = GameObject.Find("Lobby").GetComponent<Text>();



        MultiplayerController.Instance.TrySilentSignIn();
    }

    // Update is called once per frame
    void Update()
    {

        textRef1.text = "" + Data.control.energy;
        textRef2.text = "" + Data.control.upgradePoints;

        textRef3.text = "Messaege: " + _lobbyMessage;
    }

    public void HideLobby()
    {
        _lobbyMessage = "";
    }
    public void SetLobbyStatusMessage(string message)
    {
        _lobbyMessage = message;
    }

    public void Connect()
    {

        RetainedUserPicksScript.Instance.multiplayerGame = true;
        _lobbyMessage = "Starting a multiplayer game...";
        MultiplayerController.Instance.lobbylisterner = this;
        MultiplayerController.Instance.SignInAndStartMPGame();


    }

}
