using Photon.Pun;
using TMPro;
using UnityEngine;

namespace Pinball {
  public class GameManager : MonoBehaviour {
    [SerializeField]
    TextMeshProUGUI roomIDText;
    void Start() {
      roomIDText.text = "Room ID: " + PhotonNetwork.CurrentRoom.Name;
    }
  }
}
