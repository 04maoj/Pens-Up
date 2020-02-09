using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delete_Rel : MonoBehaviour
{
    // Start is called before the first frame update
    public void Try_Delete()
    {
            Track[] tracks = FindObjectsOfType<Track>();
            for (int i = 0; i < tracks.Length; i++)
            {
                Destroy(tracks[i].gameObject);
            }
            Draw[] draws = FindObjectsOfType<Draw>();
            for (int i = 0; i < draws.Length; i++)
            {
                Destroy(draws[i].gameObject);
            }
        }
}
