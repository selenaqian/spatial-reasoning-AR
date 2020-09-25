/*
* Class to define the Category object, which holds an array of Question objects
* and the progress of the user (stored in currentQuestion).
*
* Intuitively, this class represents a grouping of similar questions, e.g. questions
* that teach understanding of rotations.
*
* @author Selena Qian
*/

﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;

[System.Serializable]
public class Category
{
    public Question[] allData;
    public int currentQuestion;

    public Category(Question[] questions, int current) {
      allData = questions;
      currentQuestion = current;
    }

    public Category() {
      allData = new Question[10];
      currentQuestion = 0;
    }

    private Vector3 getVector(String vectorAsString) {
      Char[] separators = new Char[] {','};
      String[] splitValues = vectorAsString.Split(separators);

      float[] floatValueArray = new float[splitValues.Length];
      for (int i = 0; i < splitValues.Length; i++) {
        floatValueArray[i] = float.Parse(splitValues[i]);
      }

      Vector3 result = new Vector3(floatValueArray[0], floatValueArray[1], floatValueArray[2]);
      return result;
    }

    private String[] findAlternates(String combinedAlternates) {
      //TODO: implement this properly - parse for array values, doesn't matter right now because not being used
      return new String[4];
    }

    public String getCurrentModel() {
      return allData[currentQuestion].getModel();
    }

    public Quaternion getCurrentCorrectRotation() {
      return allData[currentQuestion].getCorrectRotation();
    }

    public int nextQuestion() {
      currentQuestion++;
      Debug.Log(currentQuestion);
      if (currentQuestion >= allData.Length) {
        categoryComplete();
        return -1;
      }
      return currentQuestion;
    }

    public void categoryComplete() {
      // load main Scene again, for now show a debug message
      Debug.Log("Category complete!");
    }

    public System.Boolean isCorrect(GameObject obj) {
      Vector3 rotation = obj.transform.rotation.eulerAngles;
      return allData[currentQuestion].isCorrect(rotation.x, rotation.y, rotation.z);
    }
}
