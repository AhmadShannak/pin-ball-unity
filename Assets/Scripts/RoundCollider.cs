using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

namespace Pinball { 
  public class RoundCollider : MonoBehaviour {
    [SerializeField]
    bool master = false;
    
    private void OnCollisionEnter2D(Collision2D other) {
      if (other.gameObject.GetComponent<Ball>() != null) {
        var actorId = !master ? PhotonNetwork.MasterClient.ActorNumber : PhotonNetwork.PlayerList[1].ActorNumber;
        GameManager.ResetRound(actorId);
      }
    }
  }
}
