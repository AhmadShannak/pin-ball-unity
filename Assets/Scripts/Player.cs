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
      initialRotationLeft = leftFlipper.rotation;
      initialRotationRight = rightFlipper.rotation;
    }

    private void Update() {
      bool leftFlipperActive = false;
      bool rightFlipperActive = false;

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
        Flip(Controller.Left);
      } else {
        ReturnFlipper(Controller.Left);
      }
      
      if (rightFlipperActive) {
        Flip(Controller.Right);
      } else {
        ReturnFlipper(Controller.Right);
      }
    }
    
    void Flip(Controller controller) {
      if (photonView.IsMine) {
        FlipP1(controller);
      } else {
        FlipP2(controller);

      }
    }
    void FlipP1(Controller controller) {
      var rotation = controller == Controller.Left ? rotationAmount : -rotationAmount;
      Transform flipper = controller == Controller.Left ? leftFlipper : rightFlipper;
      flipper.DORotate(new Vector3(0, 0, rotation), flipDuration, RotateMode.Fast);
    }
    
    void FlipP2(Controller controller) {
      var rotation = controller == Controller.Left ? -rotationAmount : rotationAmount;
      Transform flipper = controller == Controller.Left ? leftFlipper : rightFlipper;
      flipper.DORotate(new Vector3(0, 0, rotation), flipDuration, RotateMode.Fast);
    }
    
    void ReturnFlipper(Controller controller) {
      var rotation = controller == Controller.Left ? initialRotationLeft : initialRotationRight;
      Transform flipper = controller == Controller.Left ? leftFlipper : rightFlipper;
      var playerRotation = this.transform.localEulerAngles;
      rotation = Quaternion.Euler(playerRotation) * rotation;
      flipper.DORotateQuaternion(rotation, returnDuration);
    }
    
    public void OnPhotonInstantiate(PhotonMessageInfo info) {
      var player = info.photonView.gameObject;
      bool isMine = player.GetPhotonView().IsMine;
      var instantiatePosition = isMine ? new Vector3(0, -3, 0) : new Vector3(0, 3, 0);
      player.transform.position = instantiatePosition;
      player.transform.localEulerAngles = isMine ? Vector3.zero : new Vector3(-180, 0, 0);
    }
  }
}