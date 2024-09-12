using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallFire : MonoBehaviour {
  [SerializeField]
  PhysicsMaterial2D physicMaterial2D;
  [SerializeField]
  private BoxCollider2D[] boxes;
  private void Awake() {
  }

  private void OnTriggerExit2D(Collider2D other) {
    Debug.Log("Fucking hell");
    foreach (var box in boxes) {
      Debug.Log("saber");
      box.sharedMaterial = physicMaterial2D;
      box.isTrigger = false;
    }
  }
}
