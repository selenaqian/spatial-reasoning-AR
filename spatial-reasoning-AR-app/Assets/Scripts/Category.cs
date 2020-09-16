using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;

public class Category : MonoBehaviour
{
    Question[] allQuestions;
    int currentQuestion;

    public Category(String file) {
      allQuestions = JsonUtility.FromJson<Question[]>(Resources.Load<TextAsset>(file).ToString());
      if(PlayerPrefs.HasKey("currentQuestion")) {
        int currentQuestion = PlayerPrefs.GetInt("currentQuestion");
      } else {
        int currentQuestion = 0;
      }
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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
