using System;
using System.Linq;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Pinball {

  public class GameManager : MonoBehaviourPunCallbacks {
    [SerializeField]
    string playerPath;
    [SerializeField]
    string ballPath;
    [SerializeField]
    TextMeshProUGUI roomIDText, masterScore, clientScore;
    [SerializeField]
    Vector3 player1Position = new Vector3(0, -3, 0), player2Position = new Vector3(0, 3, 0);
    
    public static int roundStarter = -1;
    private static GameObject ball = null;
    
    void Start() {
      roomIDText.text = "Room ID: " + PhotonNetwork.CurrentRoom.Name;
      CreatePlayers();
    }

    void CreatePlayers() {
      GameObject player = PhotonNetwork.Instantiate(playerPath, Vector3.zero, Quaternion.identity);
     

      if (PhotonNetwork.IsMasterClient) { // Only the master client should instantiate the ball
        Debug.Log(roundStarter);
        ball = PhotonNetwork.Instantiate(ballPath, new Vector3 (2.11f,-4.27f,0f) , Quaternion.identity);
      }
    }

    private void Update() {
      masterScore.text = "Score: " + PhotonNetwork.MasterClient?.GetScore();
      clientScore.text = "Score: " + PhotonNetwork.PlayerList.FirstOrDefault(p => p.ActorNumber != PhotonNetwork.MasterClient.ActorNumber)?.GetScore();
    }
    
    public override void OnPlayerLeftRoom(Photon.Realtime.Player player) {
      Debug.Log("Left room");
      // Check if we are still in the room
      if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom) {
        // Load the Launcher scene
        SceneManager.LoadScene("Luncher");
        // Leave the room
        PhotonNetwork.LeaveRoom();
      }
    }
  }
}
