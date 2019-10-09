using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class Word : MonoBehaviour
{
    // Start is called before the first frame update
    public int word_count;
    public AudioClip sound;
    public GameObject images;

    public void Decrement()
    {
        word_count--;
        Debug.Log(word_count);
        if(word_count == 0)
        {
            FindObjectOfType<AudioSource>().PlayOneShot(sound);
            images.SetActive(true);
        }
    }
}
