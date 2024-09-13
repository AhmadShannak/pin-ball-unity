using System;
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
      if (Input.GetKeyDown(KeyCode.Space)) {
        isCharging = true;
        currentChargeTime = 0;    
      }

      if (Input.GetKey(KeyCode.Space)) {
        currentChargeTime += Time.deltaTime;
        currentChargeTime = Mathf.Clamp(currentChargeTime, 0, maxChargeTime);
      }

      if (Input.GetKeyUp(KeyCode.Space)) {
        float chargePercentage = currentChargeTime / maxChargeTime;
        float appliedForce = chargePercentage * maxForce;
        
        ballRb.AddForce(Vector2.up * appliedForce, ForceMode2D.Impulse);
        isCharging = false;
      }
      
      if (photonView.IsMine) {
        photonView.RPC("SwapGravity", RpcTarget.All);
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
    private void SwapGravity() {
      Debug.Log("Swap gravity");
      if (this.transform.position.y > 0) {
        Physics2D.gravity = new Vector2(0, 9.8f);
      } else {
        Physics2D.gravity = new Vector2(0, -9.8f);
      }
    }
  }
}