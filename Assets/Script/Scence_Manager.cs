using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scence_Manager : MonoBehaviour
{
    private int total_character;
    void Start()
    {
        total_character = FindObjectsOfType<Alphabate_manager>().Length;
        Debug.Log(total_character);
    }

    public void Decrement_total_Character()
    {
        total_character--;
        Debug.Log(total_character);
        if(total_character <= 0)
        {
            Debug.Log("Lesson finish");
        }
    }
    public void EraseAll()
    {
        Track[] tracks = FindObjectsOfType<Track>();
        for(int i = 0; i < tracks.Length; i ++)
        {
            Destroy(tracks[i].gameObject);
        }
        Alphabate_manager[] alphabates = FindObjectsOfType<Alphabate_manager>();
        for(int i = 0; i < alphabates.Length; i ++)
        {
            alphabates[i].Reset();

        }
    }

}
