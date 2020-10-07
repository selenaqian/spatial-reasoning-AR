/*
* Stores methods related to advancing or switching the scene on button press.
* @author Selena Qian
*/

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneController : MonoBehaviour
{
  /**
  * Loads next scene in the list. Used to go from splash screen to menu.
  */
  public void nextScene() {
    int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
    if (SceneManager.sceneCountInBuildSettings > nextSceneIndex) {
      SceneManager.LoadScene(nextSceneIndex);
    }
  }
}
