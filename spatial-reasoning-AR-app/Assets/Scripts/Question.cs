using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

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

    public Question(String objName, Vector3 vec, GameObject[] alts) {
      model = (GameObject)Resources.Load(objName);
      correctRotation = vec;
      alternateObjects = alts;
    }

    public Boolean isCorrect(int x, int y, int z) {
      return withinRange(x, correctRotation.x) && withinRange(y, correctRotation.y) && withinRange(z, correctRotation.z);
    }

    private Boolean withinRange(float current, float desired) {
      return Math.Abs(current - desired) < RANGE;
    }

    public GameObject getModel() {
      return model;
    }
}
