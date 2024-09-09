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

    Controller controller;
    Transform activeFlipper => controller == Controller.Left ? leftFlipper : rightFlipper;
    private Camera camera;
    private Quaternion initialRotationLeft, initialRotationRight;

    private void Awake() {
      camera = Camera.main;
      initialRotationLeft = leftFlipper.rotation;
      initialRotationRight = rightFlipper.rotation;
    }

    private void Update() {
      if (Input.GetMouseButtonDown(0) && camera != null) {
        controller = (Input.mousePosition.x / (float)Screen.width) > 0.5f
          ? Controller.Right
          : Controller.Left;
        Flip();
        Debug.Log("Mouse Down");
      }
      if (Input.GetMouseButtonUp(0)) {
        Debug.Log("Mouse Up");
        ReturnFlipper();
      }
    }
    
    void Flip() {
      var rotation = controller == Controller.Left ? rotationAmount : -rotationAmount; 
      activeFlipper.DORotate(new Vector3(0, 0, rotation), flipDuration, RotateMode.Fast);
    }
    
    void ReturnFlipper() {
      var rotation = controller == Controller.Left ? initialRotationLeft : initialRotationRight;
      activeFlipper.DORotateQuaternion(rotation, returnDuration);
    }
  }
}