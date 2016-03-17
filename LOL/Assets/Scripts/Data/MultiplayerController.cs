using UnityEngine;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi.Multiplayer;
using UnityEngine.SceneManagement;

public class MultiplayerController : RealTimeMultiplayerListener {

	private static MultiplayerController _instance = null;

	private uint minOpponents = 1;
	private uint maxOpponents = 1;
	private uint gameVariation = 0;

	public MPLobbyListener lobbylisterner;


	private MultiplayerController()
	{
		PlayGamesPlatform.DebugLogEnabled = true;
		PlayGamesPlatform.Activate ();
	}

	public static MultiplayerController Instance 
	{
		get{
			if (_instance == null) {
				_instance = new MultiplayerController ();
			}
			return _instance;
		}
	}

	public void SignInAndStartMPGame()
	{
		if (!PlayGamesPlatform.Instance.localUser.authenticated) {
			PlayGamesPlatform.Instance.localUser.Authenticate ((bool success) => {
				if (success) {
					Debug.Log ("you've signed in! Welcome" + PlayGamesPlatform.Instance.localUser.userName);
					StartMatchMaking();
				} else {
					Debug.Log ("sign in failed");
				}
			});
		}else{
			Debug.Log ("You're already signed in");
			StartMatchMaking ();
		}
	}

	public void TrySilentSignIn()
	{
		if (!PlayGamesPlatform.Instance.localUser.authenticated) {
			PlayGamesPlatform.Instance.Authenticate ((bool success) => {
				if (success) {
					Debug.Log ("Silently signed in! Welcome" + PlayGamesPlatform.Instance.localUser.userName);
				} else {
					Debug.Log ("You're NOT signed in");
				}
			}, true);
		} else {
			Debug.Log ("You're already signed in");
		}
	}

	public void SignOut()
	{
		PlayGamesPlatform.Instance.SignOut ();
	}
	public bool isAuthenticated()
	{
		return PlayGamesPlatform.Instance.localUser.authenticated;
	}

	private void ShowMPStatus(string message)
	{
		Debug.Log (message);
		if (lobbylisterner != null) {
			lobbylisterner.SetLobbyStatusMessage (message);
		}
	}

	public void StartMatchMaking()
	{
		PlayGamesPlatform.Instance.RealTime.CreateQuickGame (minOpponents, maxOpponents, gameVariation, this);
	}
	public void OnRoomSetupProgress (float percent)
	{
		ShowMPStatus ("We are " + percent + "% done with setup");
	}

	public void OnRoomConnected (bool success)
	{
		if (success) {
			lobbylisterner.HideLobby ();
			lobbylisterner = null;
			SceneManager.LoadScene ("MiniGameMenu");
			ShowMPStatus ("We are connected to the room! Start Game");
		} else {
			ShowMPStatus ("Error connecting to room");
		}
	}

	public void OnLeftRoom ()
	{
		ShowMPStatus("We have left the room ! Clean Up!");
	}

	public void OnParticipantLeft (Participant participant)
	{
		throw new System.NotImplementedException ();
	}

	public void OnPeersConnected (string[] participantIds)
	{
		foreach (string participantID in participantIds) {
			ShowMPStatus ("Player " + participantID + " has joined");
		}
	}

	public void OnPeersDisconnected (string[] participantIds)
	{
		foreach (string participantID in participantIds) {
			ShowMPStatus ("Player " + participantID + " has left");
		}
	}

	public void OnRealTimeMessageReceived (bool isReliable, string senderId, byte[] data)
	{
		ShowMPStatus ("We have recieved some gameplay messages from participant ID: " + senderId);
	}
}


