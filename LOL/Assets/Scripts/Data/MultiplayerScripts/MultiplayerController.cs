using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GooglePlayGames;
using GooglePlayGames.BasicApi.Multiplayer;
using UnityEngine.SceneManagement;

public class MultiplayerController : RealTimeMultiplayerListener {

	private static MultiplayerController _instance = null;

	private uint minOpponents = 2;
	private uint maxOpponents = 2;
	private uint gameVariation = 0;

    //private bool showingWaitingRoom = false;

    public enum GameState
    {
        SettingUp,
        Playing,
        Finished,
        SetupFailed,
        Aborted
    };

    private GameState mGameState = GameState.SettingUp;

    private byte _protocolVersion = 1;
    private int _updateMessageLength = 22;
    private List<byte> _updateMessage;
    public MPUpdateListener updateListener;

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
        _updateMessage = new List<byte>(_updateMessageLength);
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
      
        PlayGamesPlatform.Instance.RealTime.CreateQuickGame (minOpponents, maxOpponents, gameVariation, this);
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

        //if (!showingWaitingRoom)
        //{
        //    showingWaitingRoom = true;
        //    PlayGamesPlatform.Instance.RealTime.ShowWaitingRoomUI();
        //}
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
        byte messageVersion = (byte)data[0];
        char messageType = (char)data[1];
        if (messageType == 'U' && data.Length == _updateMessageLength)
        {
            int points = System.BitConverter.ToInt32(data, 2);
            int energy = System.BitConverter.ToInt32(data, 6);
            Debug.Log("Player " + senderId + " has " + points + " points and " + energy + " energy");
            if (updateListener != null)
            {
                updateListener.UpdateReceived(senderId, points, energy);
            }
        }
	}


    public GameState State
    {
        get
        {
            return mGameState;
        }
    }

    public void SendMyUpdate(int points, int energy)
    {
        _updateMessage.Clear();
        _updateMessage.Add(_protocolVersion);
        _updateMessage.Add((byte)'U');
        _updateMessage.AddRange(System.BitConverter.GetBytes(points));
        _updateMessage.AddRange(System.BitConverter.GetBytes(energy));
        byte[] messageToSend = _updateMessage.ToArray();
        Debug.Log("Sending my update message " + messageToSend + "to all players in the room");
        PlayGamesPlatform.Instance.RealTime.SendMessageToAll(false, messageToSend);
    }

}


