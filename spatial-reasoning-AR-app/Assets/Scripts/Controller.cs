/*
* Controls flow of application. Changes what appears as the prefab on the AR marker,
* calls checks for correctness and subsequent updates to the Category object.
*
* Currently supports only one category of questions, rotations, but could easily
* (and likely will) be modified to support multiple categories.
*
* @author Selena Qian
*/

﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{
    public const String ROTATIONS_JSON = "Rotations";
    ARTrackedImageManager myTrackedImageManager;
    Category rotations;
    GameObject previous;
    GameObject current;
    Vector3 currentPosition;
    Quaternion currentRotation;
    GameObject prompt;
    GameObject previousPrompt;
    public TextMeshProUGUI promptText;
    public Camera arCamera;
    public Button submit;
    public TextMeshProUGUI submitText;
    public TextMeshProUGUI responseText;
    public Button beginExercises;
    public TextMeshProUGUI progressText;
    public Button redo;

    void Start() {
      Application.targetFrameRate = 60;
    }

    /*
    * Loads questions from data file.
    * Sets myTrackedImageManager to the component on the AR Session Origin.
    */
    void Awake()
    {
      readData();
      myTrackedImageManager = GetComponent<ARTrackedImageManager>();
      current = null;
      previous = null;
      prompt = null;
      previousPrompt = null;
      submit.gameObject.SetActive(false);
      beginExercises.gameObject.SetActive(false);
      redo.gameObject.SetActive(false);
      responseText.gameObject.SetActive(false);
      reset();
    }

    void OnEnable()
    {
        myTrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        myTrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    void Update()
    {
      if (prompt != null && current.activeSelf) {
        updatePromptPosition();
      }
      foreach(Touch touch in Input.touches) {
        if (touch.phase == TouchPhase.Ended) {
          if (IsPointerOverUIObject()) return;
          if (responseText.gameObject.activeSelf && responseText.text.Equals(rotations.getCurrent().getCorrect())) {
            advance();
            break;
          }
          else if (responseText.gameObject.activeSelf && responseText.text.Equals(rotations.getCurrent().getWrong())) {
            reset();
            break;
          }
          else if (!responseText.gameObject.activeSelf) {
            showHint();
          }
          else responseText.gameObject.SetActive(false);
        }
      }
    }

    void OnApplicationPause(bool paused) {
      if (paused) {
        saveData();
      }
    }

    /**
    * Saves current status of rotations to persistentDataPath. Allows for reading back later.
    */
    public void saveData() {
      String json = JsonUtility.ToJson(rotations);
      Debug.Log("writing out: " + json);
      File.WriteAllText(Application.persistentDataPath + "/Rotations.txt", json);
    }

    private void readData() {
      if(System.IO.File.Exists(Application.persistentDataPath + "/Rotations.txt")) {
        Debug.Log("reading from saved");
        rotations = JsonUtility.FromJson<Category>(File.ReadAllText(Application.persistentDataPath + "/Rotations.txt"));
      }
      else {
        rotations = JsonUtility.FromJson<Category>(Resources.Load<TextAsset>(ROTATIONS_JSON).ToString());
      }
    }

    private void showHint() {
      responseText.color = Color.white;
      if (current != null && current.activeSelf) {
        responseText.text = "Click the Submit button when you're ready to check the rotation!";
      }
      else {
        responseText.text = "Position the image in the camera view to make the object appear.";
      }
      responseText.gameObject.SetActive(true);
    }

    private Color changeTransparency(Color c, float f) {
      c.a = f;
      return c;
    }

    // from: https://answers.unity.com/questions/1469696/ui-button-and-touch-input-conflict.html
    private bool IsPointerOverUIObject() {
     PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
     eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
     List<RaycastResult> results = new List<RaycastResult>();
     EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
     return results.Count > 0;
   }

    /*
    * Changes text of submit button back to "Submit" and changes color back to blue.
    * Creates new current and prompt objects.
    */
    private void reset() {
      beginExercises.gameObject.SetActive(false);
      redo.gameObject.SetActive(false);
      responseText.gameObject.SetActive(false);
      if (rotations.getCurrent().isTutorial()) {
        progressText.text = "Tutorial";
      }
      else {
        promptText.text = rotations.getCurrent().getPrompt();
        progressText.text = (rotations.currentQuestion - rotations.getNumberTutorial() + 1) + " of " + rotations.getNumberExercise();
        submit.gameObject.SetActive(true);
        submit.GetComponent<Image>().color = changeTransparency(submit.GetComponent<Image>().color, 1.0f);
        submitText.color = changeTransparency(submitText.color, 1.0f);
      }
      current = createNew(current);
      previous = setPrevious(previous, current);
      prompt = createNew(prompt);
      previousPrompt = setPrevious(previousPrompt, prompt);
    }

    private void advance() {
      if (rotations.nextQuestion() > 0) {
        reset();
      }
      else {
        SceneManager.LoadScene("CategoryComplete");
      }
    }

    /*
    * Moves from tutorial to exercises.
    */
    public void begin() {
      rotations.nextQuestion();
      reset();
    }

    /*
    * Goes back through tutorial with randomized rotation.
    */
    public void redoTutorial() {
      rotations.currentQuestion = 0;
      float randomRotation = UnityEngine.Random.Range(0.0f,360.0f);
      rotations.getCurrent().correctRotation = "0," + randomRotation + ",0";
      reset();
    }

    /*
    * Gets position and rotation of tracked image, updates current model to new if needed,
    * updates position and location of model based on tracked image. Also calls the
    * correctness checker if in tutorial mode.
    */
    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs) {
      foreach (var trackedImage in eventArgs.updated) {
        currentPosition = trackedImage.transform.position;
        currentRotation = trackedImage.transform.rotation;
        /* If an image is properly tracked */
        if (trackedImage.trackingState == TrackingState.Tracking) {
          if (responseText.text == "Position the image in the camera view to make the object appear.") {
            responseText.gameObject.SetActive(false);
          }
          promptText.text = rotations.getCurrent().getPrompt();
          if(current == null || !current.name.Equals(rotations.getCurrentModel())) {
            current = createNew(current);
            previous = setPrevious(previous, current);
            prompt = createNew(prompt);
            previousPrompt = setPrevious(previousPrompt, prompt);
          }
          current.SetActive(true);
          prompt.SetActive(true);
          updateCurrentPosition();
          updatePromptPosition();
          if (rotations.getCurrent().isTutorial()) {
            checkCorrect();
          }
        }
      }
    }

    private GameObject createNew(GameObject curr) {
      GameObject temp = Resources.Load(rotations.getCurrentModel()) as GameObject;
      curr = Instantiate(temp);
      curr.name = rotations.getCurrentModel();
      curr.SetActive(false);
      Debug.Log("created new: " + curr.name);
      return curr;
    }

    private GameObject setPrevious(GameObject prev, GameObject curr) {
      if (prev!= null && prev != current) {
        Destroy(prev);
      }
      prev = curr;
      return prev;
    }

    private void updatePromptPosition() {
      Vector3 desiredLocation = new Vector3(0.5f,0.8f,0.25f);
      prompt.transform.position = arCamera.GetComponent<Camera>().ViewportToWorldPoint(desiredLocation);
      prompt.transform.rotation = rotations.getCurrentCorrectRotation();
    }

    private void updateCurrentPosition() {
      current.transform.position = currentPosition;
      current.transform.rotation = currentRotation;
      Debug.Log(currentRotation.eulerAngles);
    }

    /**
    * Checks if current rotation of the object "current" matches the desired rotation.
    */
    public void checkCorrect() {
      if(rotations.isCorrect(current)) {
        responseText.text = rotations.getCurrent().getCorrect();
        responseText.color = new Color(0.47f, 0.87f, 0.36f);
        current.SetActive(false);
        prompt.SetActive(false);
        if (rotations.getCurrent().isTutorial()) {
          beginExercises.gameObject.SetActive(true);
          redo.gameObject.SetActive(true);
          //This currently assumes only 1 tutorial object. Would need to add in a checker of some sort for more tutorial objects if have multiple.
        }
        else {
          responseText.gameObject.SetActive(true);
          hideSubmitButton();
        }
      }
      else {
        responseText.text = rotations.getCurrent().getWrong();
        responseText.color = new Color(0.98f, 0.39f, 0.27f);
        responseText.gameObject.SetActive(true);
        hideSubmitButton();
      }
    }

    private void hideSubmitButton() {
      submit.GetComponent<Image>().color = changeTransparency(submit.GetComponent<Image>().color, 0.0f);
      submitText.color = changeTransparency(submitText.color, 0.0f);
    }

    /*
    * Method called for dropdown that allows for selection of question number and skipping around.
    */
    public void setQuestion(int number) {
      if (number < 0 || number > rotations.allData.Length - 1) {
        return;
      }
      rotations.currentQuestion = number;
      reset();
    }

    public Question getCurrentQuestion() {
      return rotations.getCurrent();
    }
}
