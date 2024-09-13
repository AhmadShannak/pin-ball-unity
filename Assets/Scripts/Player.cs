using System;
using Photon.Pun;
using UnityEngine;

namespace Pinball {
  public class Player : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback {
    [SerializeField]
    Transform leftFlipper, rightFlipper;
    [SerializeField]
    Rigidbody2D leftFlipperRb, rightFlipperRb;
    [SerializeField]
    float rotationAmount = 40, flipTorque = 1000f, returnTorque = 500f;
    
    bool isMine;
    
    enum Controller {
      Left,
      Right
    }
    private Camera camera;
    private Quaternion initialRotationLeft, initialRotationRight;

    private void Awake() {
      camera = Camera.main;
      initialRotationLeft = leftFlipper.localRotation;
      initialRotationRight = rightFlipper.localRotation;
    }

    private void Update() {
      if (!photonView.IsMine) {
        return;
      }
      
      bool leftFlipperActive = false;
      bool rightFlipperActive = false;
#if UNITY_EDITOR
      if (Input.GetKey(KeyCode.LeftArrow)) {
        leftFlipperActive = true;
      }

      if (Input.GetKey(KeyCode.RightArrow)) {
        rightFlipperActive = true;
      }
#endif
      foreach (var touch in Input.touches) {
        if (camera != null) {
          if (touch.position.x / (float)Screen.width < 0.5f) {
            leftFlipperActive = true;
          } else {
            rightFlipperActive = true;
          }
        }
      }

      if (leftFlipperActive) {
        photonView.RPC("Flip", RpcTarget.All, Controller.Left);
      } else {
        photonView.RPC("ReturnFlipper", RpcTarget.All, Controller.Left);
      }
      
      if (rightFlipperActive) {
        photonView.RPC("Flip", RpcTarget.All, Controller.Right);
      } else {
        photonView.RPC("ReturnFlipper", RpcTarget.All, Controller.Right);
      }
    }
    
    [PunRPC]
    void Flip(Controller controller) {
      int notMine = isMine ? 1 : -1;
      if (controller == Controller.Left) {
        leftFlipperRb.AddTorque(notMine * flipTorque);
      } else {
        rightFlipperRb.AddTorque(notMine * -flipTorque);
      }
    }
    
    [PunRPC]
    void ReturnFlipper(Controller controller) {
      int notMine = isMine ? 1 : -1;
      if (controller == Controller.Left) {
        leftFlipperRb.AddTorque(notMine * -returnTorque);
      } else {
        rightFlipperRb.AddTorque(notMine * returnTorque);
      }
    }
    
    public void OnPhotonInstantiate(PhotonMessageInfo info) {
      var player = info.photonView.gameObject;
      isMine = player.GetPhotonView().IsMine;
      var instantiatePosition = isMine ? new Vector3(0, -4.05f, 0) : new Vector3(0, 4.05f, 0);
      var instantiateRotation = isMine ? Vector3.zero : new Vector3(-180, 0, 0);
      player.transform.position = instantiatePosition;
      player.transform.localEulerAngles = instantiateRotation;
      initialRotationLeft = leftFlipper.rotation;
      initialRotationRight = rightFlipper.rotation;
      if (isMine) {
        return;
      }
      Debug.Log("Not Mine");
      var leftHingeJoint2D = player.GetComponent<Player>().leftFlipperRb.GetComponent<HingeJoint2D>();
      var rightHingeJoint2D = player.GetComponent<Player>().rightFlipperRb.GetComponent<HingeJoint2D>();
      
      var limits = leftHingeJoint2D.limits;
      limits = new JointAngleLimits2D {
        min = -limits.min,
        max = -limits.max
      };
      leftHingeJoint2D.limits = limits;
      
      limits = rightHingeJoint2D.limits;
      limits = new JointAngleLimits2D {
        min = -limits.min,
        max = -limits.max
      };
      rightHingeJoint2D.limits = limits;
    }
    
    [PunRPC]
    void InitializePositionAndRotation(int photonViewID, Vector3 position, Vector3 rotation) {
      PhotonView targetPhotonView = PhotonView.Find(photonViewID);
      if (targetPhotonView == null) {
        return;
      }
      targetPhotonView.gameObject.transform.position = position;
      targetPhotonView.transform.localEulerAngles = rotation;
    }
  }
}
