using System;
using System.Linq;
using Photon.Pun;
using UnityEngine;

namespace Pinball {
  public class Ball : MonoBehaviourPun, IPunOwnershipCallbacks {
    [SerializeField]
    float speed = 5f;
    [SerializeField]
    Rigidbody2D ballRb;
    [SerializeField]
    float maxChargeTime = 2.0f;
    [SerializeField]
    float maxForce = 1000f;

    bool isCharging = false;
    float currentChargeTime = 0;
    GameObject fireTower;
    private PhotonView photonView;
    
    private void Start() {
       photonView = this.gameObject.GetComponent<PhotonView>();
    }

    void Update() {
      if (Input.GetKeyDown(KeyCode.Space) && photonView.IsMine) {
        isCharging = true;
        currentChargeTime = 0;    
      }

      if (Input.GetKey(KeyCode.Space) && photonView.IsMine) {
        currentChargeTime += Time.deltaTime;
        currentChargeTime = Mathf.Clamp(currentChargeTime, 0, maxChargeTime);
      }

      if (Input.GetKeyUp(KeyCode.Space) && photonView.IsMine) {
        float chargePercentage = currentChargeTime / maxChargeTime;
        float appliedForce = chargePercentage * maxForce;
        
        ballRb.AddForce(Vector2.up * appliedForce, ForceMode2D.Impulse);
        isCharging = false;
      }
      
      if (photonView.IsMine) {
        if (this.transform.position.y > 0) {
          var otherPlayer = PhotonNetwork.PlayerList.FirstOrDefault(p => !p.IsMasterClient);
          photonView.TransferOwnership(otherPlayer);
          photonView.RPC("SwapGravity", RpcTarget.All, 9.82f);
        } else {
          photonView.RPC("SwapGravity", RpcTarget.All, -9.82f);
        }
      }
    }

    public void OnOwnershipRequest(PhotonView targetView, Photon.Realtime.Player requestingPlayer) {
      Debug.Log("Ownership requested");
    }

    public void OnOwnershipTransfered(PhotonView targetView, Photon.Realtime.Player previousOwner) {
      Debug.Log("Ownership transfered");
    }

    public void OnOwnershipTransferFailed(PhotonView targetView, Photon.Realtime.Player senderOfFailedRequest) {
      Debug.Log("Ownership transfer failed");
    }
    
    [PunRPC]
    private void SwapGravity(float gravity) {
      Physics2D.gravity = new Vector2(0, gravity);
      Debug.Log("Swap gravity");
    }
  }
}