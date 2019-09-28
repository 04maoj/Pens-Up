using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class World_Space_video : MonoBehaviour
{
    // Start is called before the first frame update
    public VideoPlayer myVideo;
    public GameObject endButton;
    public GameObject other_UI;
    public void Awake()
    {   
        myVideo.loopPointReached += CheckOver;
        endButton.SetActive(false);
        other_UI.SetActive(false);
    }
    public void PlayPause()
    {
        if (myVideo.isPlaying)
        {
            myVideo.Pause();
        }
        else
        {
            myVideo.Play();
        }
    }
    void CheckOver(VideoPlayer vp) {
        endButton.SetActive(true);
    }
    public void End()
    {
        other_UI.SetActive(true);
        gameObject.SetActive(false);
    }

}
