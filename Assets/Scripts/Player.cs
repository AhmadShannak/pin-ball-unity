using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pinball {
  public class Player : MonoBehaviour {
    enum Controller {
      Left,
      Right
    }

    Controller controller;
    
    private void Update() {
      if (Input.GetMouseButtonDown(0) && Camera.main != null) {
        controller = (Input.mousePosition.x / (float)Screen.width) > 0.5f
          ? Controller.Right
          : Controller.Left;
        
        
      }
    }
  }
}