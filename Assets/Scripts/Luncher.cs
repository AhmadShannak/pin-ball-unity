using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Pinball {
  public class Luncher : MonoBehaviour {
    [SerializeField]
    Button playNowButton;

    private void Awake() {
      PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void Connect() {
      if (PhotonNetwork.IsConnected) {
        PhotonNetwork.JoinRandomRoom();
      } else {
        PhotonNetwork.ConnectUsingSettings();
      }
    }
  }
}