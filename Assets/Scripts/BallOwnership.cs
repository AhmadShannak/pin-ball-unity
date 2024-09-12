using Photon.Pun;
using UnityEngine;

namespace Pinball {
public class BallOwnership : MonoBehaviourPun
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            PhotonView playerPhotonView = collision.gameObject.GetComponent<PhotonView>();

            if (playerPhotonView != null && PhotonNetwork.IsConnected && photonView.Owner != playerPhotonView.Owner)
            {
                photonView.TransferOwnership(playerPhotonView.Owner);
            }
        }
    }
 }
}