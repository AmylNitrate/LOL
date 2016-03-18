using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GooglePlayGames;
using GooglePlayGames.BasicApi.Multiplayer;
using UnityEngine.SceneManagement;

public class MultiplayerController : RealTimeMultiplayerListener {

	private static MultiplayerController _instance = null;

	private uint minOpponents = 1;
	private uint maxOpponents = 2;
	private uint gameVariation = 0;

    private bool showingWaitingRoom = false;

    public enum GameState
    {
        SettingUp,
        Playing,
        Finished,
        SetupFailed,
        Aborted
    };

    private GameState mGameState = GameState.SettingUp;

    public MPLobbyListener lobbylisterner;

    public List<Participant> GetAllPlayers()
    {
        return PlayGamesPlatform.Instance.RealTime.GetConnectedParticipants();
    }
    public string GetMyParticipantId()
    {
        return PlayGamesPlatform.Instance.RealTime.GetSelf().ParticipantId;
    }


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
            StartMatchMaking();
			
		}
	}

	public void TrySilentSignIn()
	{
		if (!PlayGamesPlatform.Instance.localUser.authenticated) {
			PlayGamesPlatform.Instance.Authenticate ((bool success) => {
				if (success) {
					Debug.Log ("Silently signed in! Welcome" + PlayGamesPlatform.Instance.localUser.userName);
                    StartMatchMaking();
                } else {
					Debug.Log ("You're NOT signed in");
				}
			}, true);
		} else {
			Debug.Log ("You're already signed in");
            StartMatchMaking();
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
      
        PlayGamesPlatform.Instance.RealTime.CreateWithInvitationScreen (minOpponents, maxOpponents, gameVariation, this);
	}

    public static void AcceptFromInbox()
    {
        
        PlayGamesPlatform.Instance.RealTime.AcceptFromInbox(_instance);
    }

    public static void AcceptInvitation(string invitationId)
    {
        
        PlayGamesPlatform.Instance.RealTime.AcceptInvitation(invitationId, _instance);
    }

    public void OnRoomSetupProgress (float percent)
	{
		ShowMPStatus ("We are " + percent + "% done with setup");

        if (!showingWaitingRoom)
        {
            showingWaitingRoom = true;
            PlayGamesPlatform.Instance.RealTime.ShowWaitingRoomUI();
        }
    }

	public void OnRoomConnected (bool success)
	{
		if (success) {
            mGameState = GameState.Playing;
			lobbylisterner.HideLobby ();
			lobbylisterner = null;
			SceneManager.LoadScene ("MiniGameMenu");
			ShowMPStatus ("We are connected to the room! Start Game");
		} else {
			ShowMPStatus ("Error connecting to room");
            mGameState = GameState.SetupFailed;
		}
	}

	public void OnLeftRoom ()
	{
        if (mGameState != GameState.Finished)
        {

            mGameState = GameState.Aborted;
        }
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


    public GameState State
    {
        get
        {
            return mGameState;
        }
    }

}


