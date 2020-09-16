using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Question : MonoBehaviour
{
    public const float RANGE = 10.0f;
    private GameObject model;
    private Vector3 correctRotation;
    private GameObject[] alternateObjects;

    public Question(String objName, Vector3 vec) {
      model = (GameObject)Resources.Load(objName);
      correctRotation = vec;
    }

    public Question(String objName, Vector3 vec, String[] alts) {
      model = (GameObject)Resources.Load(objName);
      correctRotation = vec;
      alternateObjects = new GameObject[alts.Length];
      for (int i = 0; i < alts.Length; i++) {
        alternateObjects[i] = (GameObject)Resources.Load(alts[i]);
      }
    }

    public Boolean isCorrect(float x, float y, float z) {
      return withinRange(x, correctRotation.x) && withinRange(y, correctRotation.y) && withinRange(z, correctRotation.z);
    }

    private Boolean withinRange(float current, float desired) {
      return Math.Abs(current - desired) < RANGE;
    }

    public GameObject getModel() {
      return model;
    }
}
