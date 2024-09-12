using Photon.Pun;
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
    
    void Start() {
      roomIDText.text = "Room ID: " + PhotonNetwork.CurrentRoom.Name;
      CreatePlayers();
    }

    void CreatePlayers() {
      GameObject player = PhotonNetwork.Instantiate(playerPath, Vector3.zero, Quaternion.identity);
     

      if (PhotonNetwork.IsMasterClient) { // Only the master client should instantiate the ball
           GameObject ball = PhotonNetwork.Instantiate(ballPath, new Vector3 (2.11f,-4.27f,0f) , Quaternion.identity);
      }
    }
  }
}
