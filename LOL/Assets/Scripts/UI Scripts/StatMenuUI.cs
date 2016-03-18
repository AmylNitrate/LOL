using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StatMenuUI : MonoBehaviour, MPLobbyListener
{
    private Text textRef1;
    private Text textRef2;
    private Text textRef3;

    public Texture2D signOutButton;
    public Texture2D[] buttonTextures;
    private float buttonWidth;
    private float buttonHeight;

    private string _lobbyMessage;


    void Start()
    {
        textRef1 = GameObject.Find("EnergyText").GetComponent<Text>();
        textRef2 = GameObject.Find("UpgradePoints").GetComponent<Text>();
        textRef3 = GameObject.Find("Lobby").GetComponent<Text>();

        // I know that 301x55 looks good on a 660-pixel wide screen, so we can extrapolate from there
        buttonWidth = 301.0f * Screen.width / 660.0f;
        buttonHeight = 55.0f * Screen.width / 660.0f;

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
