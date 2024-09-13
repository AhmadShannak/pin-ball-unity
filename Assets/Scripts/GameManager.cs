using System.Linq;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using TMPro;
using UnityEngine;

namespace Pinball {

  public class GameManager : MonoBehaviour {
    [SerializeField]
    string playerPath;
    [SerializeField]
    string ballPath;
    [SerializeField]
    TextMeshProUGUI roomIDText;
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
        GameManager.roundStarter = PhotonNetwork.MasterClient.ActorNumber; 
        ball = PhotonNetwork.Instantiate(ballPath, new Vector3 (2.11f,-4.27f,0f) , Quaternion.identity);
      }
    }

    public static void ResetRound(int winnerID) {
      var winner = PhotonNetwork.PlayerList.FirstOrDefault(p => p.ActorNumber == winnerID);
      var winnerScore = winner.GetScore();
      winnerScore++;
      winner.SetScore(winnerScore);
      roundStarter = PhotonNetwork.PlayerList.FirstOrDefault(p => p.ActorNumber != roundStarter)!.ActorNumber;

      ball.transform.position = roundStarter == PhotonNetwork.MasterClient.ActorNumber
        ? new Vector3(2.11f, -4.27f, 0f)
        : new Vector3(-2.11f, 4.27f, 0f);
    }
  }
}
