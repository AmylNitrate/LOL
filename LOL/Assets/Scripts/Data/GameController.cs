using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GooglePlayGames.BasicApi.Multiplayer;

public class GameController : MonoBehaviour, MPUpdateListener {

    public GameObject opponentPrefab;

    private bool _multiplayerReady;
    private string _myParticipantId;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void SetupMultiplayerGame()
    {
        MultiplayerController.Instance.updateListener = this;
        _myParticipantId = MultiplayerController.Instance.GetMyParticipantId();

        List<Participant> allPlayers = MultiplayerController.Instance.GetAllPlayers();
        for (int i = 0; i < allPlayers.Count; i++)
        {
            string nextParticipantId = allPlayers[i].ParticipantId;
            Debug.Log("Setting up lizard for " + nextParticipantId);
        }
        _multiplayerReady = true;
    }

    void DoMultiplayerUpdate()
    {
        MultiplayerController.Instance.SendMyUpdate(Data.control.points, Data.control.energy);

    }

    public void UpdateReceived(string senderId, int points, int energy)
    {
        if (_multiplayerReady)
        {
            MiniGMenuUI miniGameUI;
            miniGameUI = GameObject.Find("UIManager").GetComponent<MiniGMenuUI>();
            miniGameUI.SetInfo(points, energy);
        }
    }

}
