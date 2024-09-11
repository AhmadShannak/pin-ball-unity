using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Pinball {
  public class SamplePlayer : MonoBehaviour {
    [SerializeField]
    Transform leftFlipper, rightFlipper;
    [SerializeField]
    Rigidbody2D leftFlipperRb, rightFlipperRb;
    [SerializeField]
    float rotationAmount = 40, flipDuration = 0.1f, returnDuration = 0.2f, flipTorque = 1000f, returnTorque = 500f;
    
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
      bool leftFlipperActive = false;
      bool rightFlipperActive = false;
      if (Input.GetKey(KeyCode.LeftArrow)) {
        leftFlipperActive = true;
      }

      if (Input.GetKey(KeyCode.RightArrow)) {
        rightFlipperActive = true;
      }
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
        Flip(leftFlipperRb, flipTorque);
      } else {
        ReturnFlipper(leftFlipperRb, returnTorque);
      }
      
      if (rightFlipperActive) {
        Flip(rightFlipperRb, -flipTorque);
      } else {
        ReturnFlipper(rightFlipperRb, -returnTorque);
      }
    }
    
    void Flip(Rigidbody2D flipperRb, float torque) {
      Debug.Log("Flips");
      flipperRb.AddTorque(torque);
    }
    
    void ReturnFlipper(Rigidbody2D flipperRb, float torque) {
      Debug.Log("Return Flipper");
      flipperRb.AddTorque(-torque);
    }
    
    void Flip(Controller controller) {
      Debug.Log("Flip"+ controller.ToString());
      var rotation = controller == Controller.Left ? rotationAmount : -rotationAmount;
      Transform flipper = controller == Controller.Left ? leftFlipper : rightFlipper;
      flipper.DOLocalRotate(new Vector3(0, 0, rotation), flipDuration, RotateMode.Fast);
    }
    
    void ReturnFlipper(Controller controller) {
      Debug.Log("Return Flipper"+ controller.ToString());
      var rotation = controller == Controller.Left ? initialRotationLeft : initialRotationRight;
      Transform flipper = controller == Controller.Left ? leftFlipper : rightFlipper;
      flipper.DORotateQuaternion(rotation, returnDuration);
    }
  }
}