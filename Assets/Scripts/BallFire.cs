using System;
using System.Collections;
using System.Collections.Generic;
using MEC;
using Unity.VisualScripting;
using UnityEngine;

public class BallFire : MonoBehaviour {
  [SerializeField]
  PhysicsMaterial2D physicMaterial2D;
  [SerializeField]
  private BoxCollider2D[] boxes;

  private void OnTriggerExit2D(Collider2D other) {
    Timing.RunCoroutine(TriggerExit());
  }

  IEnumerator<float> TriggerExit() {
    yield return Timing.WaitForSeconds(1);
    foreach (var box in boxes) {
      Debug.Log("saber");
      box.sharedMaterial = physicMaterial2D;
      box.isTrigger = false;
    }
    yield break;
  }
  
  public void Reset() {
    foreach (var box in boxes) {
      box.sharedMaterial = null;
      box.isTrigger = true;
    }
  }
}
