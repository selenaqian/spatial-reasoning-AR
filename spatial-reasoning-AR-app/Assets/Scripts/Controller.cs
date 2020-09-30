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
using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;
using TMPro;
using System;

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
    public Button beginExercises;

    /**
    * Loads questions from data file.
    * Sets myTrackedImageManager to the component on the AR Session Origin.
    */
    void Awake()
    {
      rotations = JsonUtility.FromJson<Category>(Resources.Load<TextAsset>(ROTATIONS_JSON).ToString());
      Debug.Log(rotations);
      myTrackedImageManager  = GetComponent<ARTrackedImageManager>();
      current = null;
      previous = null;
      prompt = null;
      previousPrompt = null;
      submit.gameObject.SetActive(false);
      beginExercises.gameObject.SetActive(false);
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
      foreach(Touch touch in Input.touches) {
        if (submitText.text.Equals("That's correct!") && rotations.nextQuestion() > 0) {
          Debug.Log("correct, reset");
          reset();
          break;
        }
        else if (submitText.text.Equals("Incorrect. Try again!")) {
          Debug.Log("incorrect, reset");
          reset();
          break;
        }
      }
    }

    /**
    * Changes text of submit button back to "Submit" and changes color back to blue.
    * Creates new current and prompt objects.
    */
    private void reset() {
      submitText.text = "Submit";
      submit.GetComponent<Image>().color = new Color(51, 152, 221);
      createNewCurrent();
      createNewPrompt();
    }

    /**
    * Moves from tutorial to exercises.
    */
    public void begin() {
      beginExercises.gameObject.SetActive(false);
      submit.gameObject.SetActive(true);
      rotations.nextQuestion();
      reset();
    }

    /**
    * Gets position and rotation of tracked image, updates current model to new if needed,
    * updates position and location of model based on tracked image. Also calls the
    * correctness checker if in tutorial mode.
    */
    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.updated) {
          currentPosition = trackedImage.transform.position;
          currentRotation = trackedImage.transform.rotation;
          /* If an image is properly tracked */
          if (trackedImage.trackingState == TrackingState.Tracking) {
            Debug.Log("tracking");
            if(current == null || !current.name.Equals(rotations.getCurrentModel())) {
              if (current==null) Debug.Log("current null");
              else {
                Debug.Log("current not correct name");
                Debug.Log("current name: " + current.name + "needed name: " + rotations.getCurrentModel());
              }
              createNewCurrent();
              createNewPrompt();
            }
            updateCurrentPosition();
            updatePromptPosition();
            if (rotations.getCurrent().isTutorial()) {
              checkCorrect();
            }
          }
        }
      }

      private void createNewCurrent() {
        Debug.Log("creating new");
        Debug.Log("current model:" + rotations.getCurrentModel());
        GameObject temp = Resources.Load(rotations.getCurrentModel()) as GameObject;
        Debug.Log("temp: " + temp.name + " correct rotation: " + rotations.getCurrentCorrectRotation());
        current = Instantiate(temp);
        current.name = rotations.getCurrentModel(); // the name was tutorial(Clone) instead of tutorial!

        if (previous!= null && previous != current) {
          previous.SetActive(false);
        }

        current.SetActive(true);
        previous = current;
      }

      private void createNewPrompt() {
        GameObject temp = Resources.Load(rotations.getCurrentModel()) as GameObject;
        prompt = Instantiate(temp);
        prompt.transform.rotation = rotations.getCurrentCorrectRotation();

        //TODO: doesn't seem to be working
        Color translucent = prompt.GetComponent<MeshRenderer>().material.color;
        translucent.a = 0.5f;
        prompt.GetComponent<MeshRenderer>().material.color = translucent;

        if (previousPrompt!= null && previousPrompt != current) {
          previousPrompt.SetActive(false);
        }

        prompt.SetActive(true);
        previousPrompt = prompt;
      }

      private void updatePromptPosition() {
        Vector3 desiredLocation = new Vector3(0.5f,0.8f,0.25f);
        prompt.transform.position = arCamera.GetComponent<Camera>().ViewportToWorldPoint(desiredLocation);
      }

      private void updateCurrentPosition() {
        current.transform.position = currentPosition;
        current.transform.rotation = currentRotation;
        Debug.Log(currentRotation.eulerAngles);
      }

      public void checkCorrect() {
        Debug.Log("checking");
        if(rotations.isCorrect(current)) {
          Debug.Log("correct rotation! " + currentRotation.eulerAngles);
          if (rotations.getCurrent().isTutorial()) {
            promptText.text = "Good job!";
            beginExercises.gameObject.SetActive(true);
            //This currently assumes only 1 tutorial object. Would need to add in a checker of some sort for more tutorial objects if have multiple.
          }
          else {
            submitText.text = "That's right!";
            submit.GetComponent<Image>().color = new Color(33, 202, 35);
          }
        }
        else {
          Debug.Log("incorrect");
          submitText.text = "Incorrect. Try again!";
          submit.GetComponent<Image>().color = new Color(227, 57, 55);
        }
      }
}
