using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public GameObject icons;
    private int current = 0;
    int[] alphbate_list;
    void Start()
    {
        if (finished_screen != null)
            finished_screen.SetActive(false);
        if (videos != null)
        {

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
        if(icons != null)
        {
            for(int i = 0; i < icons.transform.childCount;i ++)
            {
                icons.transform.GetChild(i).GetChild(1).GetComponent<Image>().color = Color.gray;
            }
        }
        alphbate_list = new int[26];
        for(int i = 0; i < 26; i ++)
        {
            alphbate_list[i] = 2;

        }
    }
    public void SwitchTut(int index)
    {
        current += index;
        //Debug.Log("current: " + current);
        EraseAll();

        if (current > 0)
        {

            left.SetActive(true);
        }

        else
            left.SetActive(false);
        if (current < ui_manager.courses.Count - 1)
        {
            right.SetActive(true);
        }

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
    public void Decrement_total_Character(string name)
    {
        total_character--;
        int c = name.ToLower()[0] - 'a';
        alphbate_list[c]--;
        if(alphbate_list[c] <= 0)
        {
            icons.transform.GetChild(c).GetChild(1).GetComponent<Image>().color = Color.green;
        }
        if (total_character <= 0)
        {
            Debug.Log("Lesson finish");
        }
    }
    public void EraseAll()
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
        Alphabate_manager[] alphabates = FindObjectsOfType<Alphabate_manager>();
        for (int i = 0; i < alphabates.Length; i++)
        {
            alphabates[i].Reset();
        }
    }
    public void FinishCourse()
    {
        finished_screen.SetActive(true);
        finished_screen.GetComponentInChildren<Assessment_manager>().UpdateStatus();
        FindObjectOfType<Track_manager>().gameObject.SetActive(false);
    }
}
