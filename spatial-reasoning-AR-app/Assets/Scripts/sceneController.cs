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
  public void toMenu() {
    SceneManager.LoadScene("Menu");
  }

  public void toQuestions() {
    SceneManager.LoadScene("MainQuestions");
  }

  public void toVideo() {
    SceneManager.LoadScene("TutorialVideo");
  }
}
