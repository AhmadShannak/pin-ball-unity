using DG.Tweening;
using Photon.Pun;
using UnityEngine;

namespace Pinball {
  public class Player : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback {
    [SerializeField]
    Transform leftFlipper, rightFlipper;
    [SerializeField]
    float rotationAmount = 40, flipDuration = 0.1f, returnDuration = 0.2f;
    
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
      if (Input.GetKeyDown(KeyCode.LeftArrow)) {
        leftFlipperActive = true;
      }

      if (Input.GetKeyDown(KeyCode.RightArrow)) {
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
      var rotation = controller == Controller.Left ? rotationAmount : -rotationAmount;
      Transform flipper = controller == Controller.Left ? leftFlipper : rightFlipper;
      flipper.DOLocalRotate(new Vector3(0, 0, rotation), flipDuration, RotateMode.Fast);
    }
    
    [PunRPC]
    void ReturnFlipper(Controller controller) {
      var rotation = controller == Controller.Left ? initialRotationLeft : initialRotationRight;
      Transform flipper = controller == Controller.Left ? leftFlipper : rightFlipper;
      flipper.DORotateQuaternion(rotation, returnDuration);
    }
    
    public void OnPhotonInstantiate(PhotonMessageInfo info) {
      var player = info.photonView.gameObject;
      bool isMine = player.GetPhotonView().IsMine;
      var instantiatePosition = isMine ? new Vector3(0, -3, 0) : new Vector3(0, 3, 0);
      var instantiateRotation = isMine ? Vector3.zero : new Vector3(-180, 0, 0);
      player.transform.position = instantiatePosition;
      player.transform.localEulerAngles = instantiateRotation;
      initialRotationLeft = leftFlipper.rotation;
      initialRotationRight = rightFlipper.rotation;
      // photonView.RPC("InitializePositionAndRotation", RpcTarget.All, info.photonView.ViewID, instantiatePosition, instantiateRotation);
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