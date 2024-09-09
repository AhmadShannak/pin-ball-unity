using Photon.Pun;
using TMPro;
using UnityEngine;

namespace Pinball {
  public class GameManager : MonoBehaviour {
    [SerializeField]
    string playerPath;
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
      // player.GetPhotonView(). = new object[] { player.GetPhotonView().ViewID };

      // bool isMine = player.GetPhotonView().IsMine;
      // Debug.Log(isMine);
      // var instantiatePosition = isMine ? player1Position : player2Position;
      // Debug.Log(instantiatePosition);
      // player.transform.position = instantiatePosition;
    }
  }
}
