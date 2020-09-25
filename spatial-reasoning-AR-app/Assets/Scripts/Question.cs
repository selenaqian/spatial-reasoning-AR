/*
* Class to define the Question object, which holds the name of the model used for
* this question, the correct rotation, the margin of error on the rotation (range),
* and any alternate objects that might be shown (and chosen between) in the question.
*
* Intuitively, this class represents all of the information and methods needed to
* display the question, give tools to answer it, and check for correctness of that answer.
*
* @author Selena Qian
*/

﻿using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Question
{
    public String model;
    public String correctRotation;
    public float range;
    public String[] alternateObjects;

    public Question(String objName, String vec) {
      model = objName;
      correctRotation = vec;
    }

    public Question(String objName, String vec, String[] alts) {
      model = objName;
      correctRotation = vec;
      alternateObjects = alts;
    }

    public Boolean isCorrect(float x, float y, float z) {
      Vector3 correctRotationVector = stringToVector(correctRotation);
      return withinRange(x, correctRotationVector.x) && withinRange(y, correctRotationVector.y) && withinRange(z, correctRotationVector.z);
    }

    private Boolean withinRange(float current, float desired) {
      return Math.Abs(current - desired) < range || Math.Abs(current - desired) > 360 - range;
    }

    public String getModel() {
      return model;
    }

    public Quaternion getCorrectRotation() {
      Vector3 correctRotationVector = stringToVector(correctRotation);
      Debug.Log("rotation vector: " + correctRotationVector);
      return Quaternion.Euler(correctRotationVector);
    }

    public static Vector3 stringToVector(String str) {
      Debug.Log("converting string to vector");
      String[] strArray = str.Split(',');
      return new Vector3(float.Parse(strArray[0]), float.Parse(strArray[1]), float.Parse(strArray[2]));
    }
}
