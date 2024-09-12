using System;
using UnityEngine;

namespace Pinball {
  public class Ball : MonoBehaviour {
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
      
      if (this.transform.position.y > 0) {
        Physics2D.gravity = new Vector2(0, 9.8f);
      } else {
        Physics2D.gravity = new Vector2(0, -9.8f);
      }
    }
  }
}