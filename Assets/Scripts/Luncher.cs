using System;
using System.Collections;
using System.Collections.Generic;
using MEC;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pinball {
  public class Luncher : MonoBehaviourPunCallbacks {
    
    GameObject connectPanel, inputPanel, connectingText;
    
    const string connectingCorTag = "ConnectionTextCor";
    
    private void Awake() {
      PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void Connect(GameObject _connectPanel, GameObject _inputPanel, GameObject _connectingText) {
      connectPanel = _connectPanel;
      inputPanel = _inputPanel;
      connectingText = _connectingText;
      connectPanel.SetActive(true);
      inputPanel.SetActive(false);
      Timing.RunCoroutine(BlinkConnectingText().CancelWith(this.gameObject), connectingCorTag);
      
      if (PhotonNetwork.IsConnected) {
        PhotonNetwork.JoinRandomRoom();
      } else {
        PhotonNetwork.ConnectUsingSettings();
      }
    }
    
#region MonoBehaviourPunCallbacks Callbacks
    public override void OnConnectedToMaster() {
      Debug.Log("OnConnectedToMaster() was called by PUN");
      PhotonNetwork.JoinRandomRoom();
    }
    
    public override void OnDisconnected(DisconnectCause cause) {
      Debug.LogWarningFormat("OnDisconnected() was called by PUN with reason {0}", cause);
      connectPanel.SetActive(false);
      inputPanel.SetActive(true);
      Timing.KillCoroutines(connectingCorTag);
    }
    
    public override void OnJoinRandomFailed(short returnCode, string message) {
      Debug.Log("OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");
      PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }
    
    public override void OnJoinedRoom() {
      Timing.KillCoroutines(connectingCorTag);
      Debug.Log("OnJoinedRoom() called by PUN. Now this client is in a room.");
      PhotonNetwork.LoadLevel("Game");
    }

    public override void OnCreatedRoom() {
      Timing.KillCoroutines(connectingCorTag);
    }

#endregion
    IEnumerator<float> BlinkConnectingText() {
      while (true) {
        connectingText.gameObject.SetActive(true);
        yield return Timing.WaitForSeconds(0.5f);
        connectingText.gameObject.SetActive(false);
        yield return Timing.WaitForSeconds(0.5f);
      }
    }
    
    void OnDisable() {
      Timing.KillCoroutines(connectingCorTag);
    }
  }
}