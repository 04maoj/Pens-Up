using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


public class Scence_Manager : MonoBehaviour
{
    private int total_character;
    public UI_Manager ui_manager;
    public World_Space_video my_players;
    public GameObject videos;
    public GameObject left;
    public GameObject right;
    private bool[] played_tut_list;
    public GameObject finished_screen;
    private int current = 0;
    void Start()
    {
        if(finished_screen != null)
            finished_screen.SetActive(false);
        if(videos != null) {

            played_tut_list = new bool[videos.transform.childCount + 1];
            for (int i = 0; i < videos.transform.childCount; i++)
            {
                played_tut_list[i] = false;
            }
            played_tut_list[0] = true;
            total_character = FindObjectsOfType<Alphabate_manager>().Length;
            my_players.gameObject.SetActive(true);
            left.SetActive(false);
            right.SetActive(true);
            GameObject current_player = videos.transform.GetChild(0).gameObject;
            current_player.SetActive(true);
            my_players.myVideo = current_player.GetComponent<VideoPlayer>();
            my_players.PlayPause();

        }
    }
    public void SwitchTut(int index)
    {
        current += index;
        EraseAll();
        if (current > 0)
            left.SetActive(true);
        else
            left.SetActive(false);
        if (current < ui_manager.courses.Count - 1)
            right.SetActive(true);
        else
            right.SetActive(false);
        if (!played_tut_list[current])
        {
            my_players.gameObject.SetActive(true);
            GameObject current_player = videos.transform.GetChild(current).gameObject;
            current_player.SetActive(true);
            my_players.myVideo = current_player.GetComponent<VideoPlayer>();
            my_players.PlayPause();
            played_tut_list[current] = true;

        }
        ui_manager.Get_Course(current);
    }
    public void Decrement_total_Character()
    {
        total_character--;
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
    public void FinishCourse()
    {
        finished_screen.SetActive(true);
        finished_screen.GetComponentInChildren< Assessment_manager>().UpdateStatus();
        FindObjectOfType<Track_manager>().gameObject.SetActive(false);
    }
}
