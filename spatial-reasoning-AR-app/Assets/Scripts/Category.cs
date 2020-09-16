using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;

[System.Serializable]
public class Category : MonoBehaviour
{
    public const String MODEL_DATA = "model";
    public const String ROTATION_DATA = "currentRotation";
    public const String ALTERNATE_MODELS = "alternateObjects";
    Question[] allQuestions;
    int currentQuestion;

    public Category(String file) {
      Dictionary<String, String>[] data = JsonUtility.FromJson<Dictionary<String, String>[]>(Resources.Load<TextAsset>(file).ToString());
      allQuestions = new Question[data.Length];
      populateAllQuestions(data);
      if(PlayerPrefs.HasKey("currentQuestion")) {
        int currentQuestion = PlayerPrefs.GetInt("currentQuestion");
      } else {
        int currentQuestion = 0;
      }
    }

    private void populateAllQuestions(Dictionary<String, String>[] data) {
      for (int i = 0; i < data.Length; i++) {
        Question temp;
        Vector3 rotationData = getVector(data[i][ROTATION_DATA]);
        if (data[i].ContainsKey(ALTERNATE_MODELS)) {
          String[] alternates = findAlternates(data[i][ALTERNATE_MODELS]);
          temp = new Question(data[i][MODEL_DATA], rotationData, alternates);
        } else {
          temp = new Question(data[i][MODEL_DATA], rotationData);
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

    public GameObject getCurrentModel() {
      return allQuestions[currentQuestion].getModel();
    }

    public void nextQuestion() {
      currentQuestion++;
      if (currentQuestion >= allQuestions.Length) {
        categoryComplete();
      }
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
