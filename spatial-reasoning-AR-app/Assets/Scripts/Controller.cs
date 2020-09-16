using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;
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

    void Awake()
    {
      rotations = JsonUtility.FromJson<Category>(Resources.Load<TextAsset>(ROTATIONS_JSON).ToString());
      Debug.Log(rotations);
      myTrackedImageManager  = GetComponent<ARTrackedImageManager>();
      current = null;
      previous = null;
    }

    void OnEnable()
    {
        myTrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        myTrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.updated) {
          currentPosition = trackedImage.transform.position;
          currentRotation = trackedImage.transform.rotation;
          /* If an image is properly tracked */
          if (trackedImage.trackingState == TrackingState.Tracking) {
            Debug.Log("tracking");
            if(current == null || current.name != rotations.getCurrentModel()) {
              updateCurrent();
            }
            checkCorrect();
          }
        }
      }

      private void updateCurrent() {
        Debug.Log("creating new");
        Debug.Log("current model:" + rotations.getCurrentModel());
        GameObject intermediate = Resources.Load(rotations.getCurrentModel()) as GameObject;
        Debug.Log("intermediate: " + intermediate);
        current = Instantiate(intermediate);

        if (previous!= null && previous != current) {
          previous.SetActive(false);
        }

        current.SetActive(true);
        previous = current;
        current.transform.position = currentPosition;
        current.transform.rotation = currentRotation;
        Debug.Log(currentRotation.eulerAngles);
      }

      private void checkCorrect() {
        if(rotations.isCorrect(current)) {
          Debug.Log("correct rotation! " + currentRotation.eulerAngles);
          if(rotations.nextQuestion() > 0) {
            updateCurrent();
          }
        }
      }

    /** functions:
    * check if correctRotation
      * change question number
      * update prefab for TrackedImageManager based on question number
    */
}
