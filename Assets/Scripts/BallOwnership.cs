using System;
using Photon.Pun;
using UnityEngine;

namespace Pinball {
public class BallOwnership : MonoBehaviourPun
{
  // private void OnCollisionEnter2D(Collision2D other) {
  //   if (other.transform.parent.gameObject.GetComponent<Player>() != null) {
  //     PhotonView playerPhotonView = other.transform.parent.gameObject.GetComponent<PhotonView>();
  //     if (playerPhotonView != null && PhotonNetwork.IsConnected && photonView.Owner != playerPhotonView.Owner) {
  //       photonView.TransferOwnership(playerPhotonView.Owner);
  //     }
  //   }
  // }
  // void OnCollisionEnter2D(Collision collision)
    // {
    //     if (collision.gameObject.GetComponent<Player>())
    //     {
    //         PhotonView playerPhotonView = collision.gameObject.GetComponent<PhotonView>();
    //
    //         if (playerPhotonView != null && PhotonNetwork.IsConnected && photonView.Owner != playerPhotonView.Owner)
    //         {
    //             photonView.TransferOwnership(playerPhotonView.Owner);
    //         }
    //     }
    // }
 }
}