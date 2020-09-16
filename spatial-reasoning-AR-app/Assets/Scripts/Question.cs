using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Question
{
    public String model;
    public Vector3 correctRotation;
    public float range;
    public String[] alternateObjects;

    public Question(String objName, Vector3 vec) {
      model = objName;
      correctRotation = vec;
    }

    public Question(String objName, Vector3 vec, String[] alts) {
      model = objName;
      correctRotation = vec;
      alternateObjects = alts;
    }

    public Boolean isCorrect(float x, float y, float z) {
      return withinRange(x, correctRotation.x) && withinRange(y, correctRotation.y) && withinRange(z, correctRotation.z);
    }

    private Boolean withinRange(float current, float desired) {
      return Math.Abs(current - desired) < range || Math.Abs(current - desired) > 360 - range;
    }

    public String getModel() {
      return model;
    }
}
