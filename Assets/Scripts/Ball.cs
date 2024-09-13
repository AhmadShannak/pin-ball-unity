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
      if (Input.touchCount > 0 && photonView.IsMine) {
        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began) {
          isCharging = true;
          currentChargeTime = 0;    
        }

        if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved) {
          currentChargeTime += Time.deltaTime;
          currentChargeTime = Mathf.Clamp(currentChargeTime, 0, maxChargeTime);
        }

        if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) {
          float chargePercentage = currentChargeTime / maxChargeTime;
          float appliedForce = chargePercentage * maxForce;
        
          ballRb.AddForce(Vector2.up * appliedForce, ForceMode2D.Impulse);
          isCharging = false;
        }
      }
      
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
          var nonMasterPlayer = PhotonNetwork.PlayerList.FirstOrDefault(p => !p.IsMasterClient);
          if (Physics2D.gravity.y < 0) {
            photonView.RPC("SwapGravity", RpcTarget.All, 9.82f);
            photonView.TransferOwnership(nonMasterPlayer);
          }
          // photonView.RPC("SyncState", otherPlayer, this.transform.position, this.transform.rotation, Physics2D.gravity);
        } else {
          var masterPlayer = PhotonNetwork.PlayerList.FirstOrDefault(p => p.IsMasterClient);
          if (Physics2D.gravity.y > 0) {
            photonView.RPC("SwapGravity", RpcTarget.All, -9.82f);
            photonView.TransferOwnership(masterPlayer);
          }
          // photonView.RPC("SyncState", otherPlayer, this.transform.position, this.transform.rotation, Physics2D.gravity);
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
    
    [PunRPC]
    private void SyncState(Vector3 position, Vector3 rotation, Vector2 gravity) {
      // transform.position = position;
      // transform.localEulerAngles = rotation;
      Physics2D.gravity = gravity;
      Debug.Log("sTATE SYNCED");
    }
  }
}