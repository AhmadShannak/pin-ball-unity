using DG.Tweening;
using UnityEngine;

namespace Pinball {
  public class Player : MonoBehaviour {
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
      var rotation = controller == Controller.Left ? rotationAmount : -rotationAmount;
      Transform flipper = controller == Controller.Left ? leftFlipper : rightFlipper;
      flipper.DORotate(new Vector3(0, 0, rotation), flipDuration, RotateMode.Fast);
    }
    
    void ReturnFlipper(Controller controller) {
      var rotation = controller == Controller.Left ? initialRotationLeft : initialRotationRight;
      Transform flipper = controller == Controller.Left ? leftFlipper : rightFlipper;
      flipper.DORotateQuaternion(rotation, returnDuration);
    }
  }
}