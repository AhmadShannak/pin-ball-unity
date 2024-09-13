using System;
using System.Linq;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
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
    private BallFire _ballFire;
    
    private void Start() {
       photonView = this.gameObject.GetComponent<PhotonView>();
       GameManager.roundStarter = PhotonNetwork.MasterClient.ActorNumber;
    }

    void Update() {
      if (Input.touchCount > 0 && photonView.IsMine) {
        Touch touch = Input.GetTouch(0);
        Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

        // Check if the touch is on the ball
        RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);

        if (hit.collider != null && hit.collider.gameObject == ballRb.gameObject) {
          if (touch.phase == TouchPhase.Began)
          {
            isCharging = true;
            currentChargeTime = 0;
          }

          if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
          {
            currentChargeTime += Time.deltaTime;
            currentChargeTime = Mathf.Clamp(currentChargeTime, 0, maxChargeTime);
          }

          if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
          {
            float chargePercentage = currentChargeTime / maxChargeTime;
            float appliedForce = chargePercentage * maxForce;

            ballRb.AddForce(Vector2.up * appliedForce, ForceMode2D.Impulse);
            isCharging = false;
          }
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
          if (ballRb.gravityScale > 0) {
            photonView.RPC("SwapGravity", RpcTarget.All, -1);
            // photonView.TransferOwnership(nonMasterPlayer);
          }
          // photonView.RPC("SyncState", otherPlayer, this.transform.position, this.transform.rotation, Physics2D.gravity);
        } else {
          var masterPlayer = PhotonNetwork.PlayerList.FirstOrDefault(p => p.IsMasterClient);
          if (ballRb.gravityScale < 0) {
            photonView.RPC("SwapGravity", RpcTarget.All, 1);
            // photonView.TransferOwnership(masterPlayer);
          }
          // photonView.RPC("SyncState", otherPlayer, this.transform.position, this.transform.rotation, Physics2D.gravity);
        }
      }
    }

    private void OnTriggerEnter2D(Collider2D other) {
      if (other.gameObject.GetComponent<BallFire>() != null) {
        _ballFire = other.gameObject.GetComponent<BallFire>();
      }
    }

    private void OnCollisionEnter2D(Collision2D other) {
      if (photonView != null && !photonView.IsMine) {
        return;
      }
      if (other.gameObject.name == "Bottom") {
        photonView.RPC("ResetRound", RpcTarget.All, PhotonNetwork.PlayerList[1].ActorNumber);
      } else if (other.gameObject.name == "Top") {
        photonView.RPC("ResetRound", RpcTarget.All, PhotonNetwork.MasterClient.ActorNumber);
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
    private void SwapGravity(int gravity) {
      ballRb.gravityScale = gravity;
      Debug.Log("Swap gravity");
    }
    
    [PunRPC]
    private void SyncState(Vector3 position, Vector3 rotation, Vector2 gravity) {
      // transform.position = position;
      // transform.localEulerAngles = rotation;
      Physics2D.gravity = gravity;
      Debug.Log("sTATE SYNCED");
    }
    
    [PunRPC]
    public void ResetRound(int winnerID) {
      _ballFire.Reset();
      var winner = PhotonNetwork.PlayerList.FirstOrDefault(p => p.ActorNumber == winnerID);
      var winnerScore = winner.GetScore();
      winnerScore++;
      winner.SetScore(winnerScore);
      GameManager.roundStarter = PhotonNetwork.PlayerList.FirstOrDefault(p => p.ActorNumber != GameManager.roundStarter).ActorNumber;
      photonView.TransferOwnership(PhotonNetwork.PlayerList.FirstOrDefault(p => p.ActorNumber == GameManager.roundStarter));
      ballRb.velocity = Vector2.zero;
      this.transform.position = GameManager.roundStarter == PhotonNetwork.MasterClient.ActorNumber
        ? new Vector3(2.11f, -4.27f, 0f)
        : new Vector3(-2.11f, 4.27f, 0f);
      ballRb.velocity = Vector2.zero;
      Debug.Log("ResetRound" + GameManager.roundStarter);
    }
  }
}