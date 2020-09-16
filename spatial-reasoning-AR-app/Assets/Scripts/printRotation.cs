/*
* Checks if rotation of object has changed and prints the new rotation to console. Used for initial testing.
* @author Selena Qian
*/

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class printRotation : MonoBehaviour
{
  Quaternion previousRotation;

    // Update is called once per frame
    void Update()
    {
        ifNewRotationPrint();
    }

    private void ifNewRotationPrint() {
      if (transform.rotation != previousRotation) {
        Debug.Log(transform.rotation.eulerAngles);
        previousRotation = transform.rotation;
      }
    }
}
