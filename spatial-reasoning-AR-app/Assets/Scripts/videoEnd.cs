using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class videoEnd : MonoBehaviour
{
    //adapted from https://forum.unity.com/threads/how-to-know-video-player-is-finished-playing-video.483935/
    public VideoPlayer vid;
    public Button button;

    void Start()
    {
      vid = this.GetComponent<VideoPlayer>();
      Debug.Log("correctly set vid. " + vid);
      vid.loopPointReached += CheckOver;
    }

    void CheckOver(UnityEngine.Video.VideoPlayer vp)
    {
      Debug.Log("video finished");
      SceneManager.LoadScene("AltMenu");
    }
}
