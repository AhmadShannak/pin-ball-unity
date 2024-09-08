using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace Pinball {
  public class Luncher : MonoBehaviourPunCallbacks {
    [SerializeField]
    Button playNowButton;

    private void Awake() {
      PhotonNetwork.AutomaticallySyncScene = true;
      playNowButton.onClick.AddListener(Connect);
    }

    public void Connect() {
      if (PhotonNetwork.IsConnected) {
        PhotonNetwork.JoinRandomRoom();
      } else {
        PhotonNetwork.ConnectUsingSettings();
      }
    }
    
#region MonoBehaviourPunCallbacks Callbacks
    public override void OnConnectedToMaster() {
      Debug.Log("OnConnectedToMaster() was called by PUN");
    }
    
    public override void OnDisconnected(DisconnectCause cause) {
      Debug.LogWarningFormat("OnDisconnected() was called by PUN with reason {0}", cause);
    }
    
    public override void OnJoinRandomFailed(short returnCode, string message) {
      Debug.Log("OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");
      PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }
#endregion
  }
}