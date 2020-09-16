using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;

public class Category
{
    private Question[] allQuestions;
    private int currentQuestion;

    public Category(String file) {
      DataArray data = JsonUtility.FromJson<DataArray>(Resources.Load<TextAsset>(file).ToString());
      allQuestions = new Question[data.allData.Length];
      populateAllQuestions(data.allData);
      Debug.Log("questions: " + allQuestions[0]);
      if(PlayerPrefs.HasKey("currentQuestion")) {
        currentQuestion = PlayerPrefs.GetInt("currentQuestion");
      } else {
        currentQuestion = 0;
      }
      Debug.Log("currentQuestion: " + currentQuestion);
    }

    private void populateAllQuestions(Data[] data) {
      for (int i = 0; i < data.Length; i++) {
        Question temp;
        Vector3 rotationData = getVector(data[i].correctRotation);
        if (data[i].alternateObjects != null) {
          String[] alternates = findAlternates(data[i].alternateObjects);
          temp = new Question(data[i].model, rotationData, alternates);
        } else {
          temp = new Question(data[i].model, rotationData);
        }
        allQuestions[i] = temp;
      }
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
      return allQuestions[currentQuestion].getModel();
    }

    public int nextQuestion() {
      currentQuestion++;
      if (currentQuestion >= allQuestions.Length) {
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
      return allQuestions[currentQuestion].isCorrect(rotation.x, rotation.y, rotation.z);
    }
}
