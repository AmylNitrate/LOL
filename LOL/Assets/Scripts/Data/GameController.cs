using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GooglePlayGames.BasicApi.Multiplayer;

public class GameController : MonoBehaviour {

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
        _myParticipantId = MultiplayerController.Instance.GetMyParticipantId();

        List<Participant> allPlayers = MultiplayerController.Instance.GetAllPlayers();
        for (int i = 0; i < allPlayers.Count; i++)
        {
            string nextParticipantId = allPlayers[i].ParticipantId;
            Debug.Log("Setting up lizard for " + nextParticipantId);
        }
        _multiplayerReady = true;
    }
}
