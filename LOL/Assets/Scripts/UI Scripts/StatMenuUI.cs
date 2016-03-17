using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StatMenuUI : MonoBehaviour, MPLobbyListener
{
    private Text textRef1;
    private Text textRef2;

	public Texture2D signOutButton;
	public Texture2D[] buttonTextures;
	private float buttonWidth;
	private float buttonHeight;	

	private bool _showLobbyDialog;
	private string _lobbyMessage;

	public GUISkin guiskin;

    void Start()
    {
        textRef1 = GameObject.Find("EnergyText").GetComponent<Text>();
        textRef2 = GameObject.Find("UpgradePoints").GetComponent<Text>();

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
    }

	void OnGUI() {
		if (!_showLobbyDialog) {
			for (int i = 0; i < 2; i++) {
				if (GUI.Button (new Rect ((float)Screen.width * 0.5f - (buttonWidth / 2),
					    (float)Screen.height * (0.6f + (i * 0.2f)) - (buttonHeight / 2),
					    buttonWidth,
					    buttonHeight), buttonTextures [i])) {
					Debug.Log ("Mode " + i + " was clicked!");

					if (i == 0) {
						// Single player mode!
						RetainedUserPicksScript.Instance.multiplayerGame = false;

					} else if (i == 1) {
						RetainedUserPicksScript.Instance.multiplayerGame = true;
						_lobbyMessage = "Starting a multiplayer game...";
						_showLobbyDialog = true;
						MultiplayerController.Instance.lobbylisterner = this;
						MultiplayerController.Instance.SignInAndStartMPGame ();
					}
				}
			}
		}
		if (MultiplayerController.Instance.isAuthenticated()) {
			if (GUI.Button(new Rect(Screen.width  - (buttonWidth * 0.75f),
				Screen.height - (buttonHeight * 0.75f),
				buttonWidth * 0.75f,
				buttonHeight * 0.75f), signOutButton)) {
				MultiplayerController.Instance.SignOut();
			}
		}
		if (_showLobbyDialog) {
			GUI.skin = guiskin;
			GUI.Box (new Rect (Screen.width * 0.25f, Screen.height * 0.4f, Screen.width * 0.5f, Screen.height * 0.5f), _lobbyMessage);
		}
	}
	public void HideLobby()
	{
		_lobbyMessage = "";
		_showLobbyDialog = false;
	}
	public void SetLobbyStatusMessage (string message)
	{
		_lobbyMessage = message;
	}
}
