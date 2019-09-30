﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


public class Scence_Manager : MonoBehaviour
{
    private int total_character;
    public UI_Manager ui_manager;
    public World_Space_video my_players;
    public GameObject videos;
    void Start()
    {
        total_character = FindObjectsOfType<Alphabate_manager>().Length;
        //SwitchTut(0);
        my_players.gameObject.SetActive(true);
        GameObject current_player = videos.transform.GetChild(0).gameObject;
        current_player.SetActive(true);
        my_players.myVideo = current_player.GetComponent<VideoPlayer>();
        my_players.PlayPause();
    }
    public void SwitchTut(int index)
    {
        my_players.gameObject.SetActive(true);
        GameObject current_player = videos.transform.GetChild(index).gameObject;
        current_player.SetActive(true);
        my_players.myVideo = current_player.GetComponent<VideoPlayer>();
        my_players.PlayPause();
        ui_manager.Get_Course(index);
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
