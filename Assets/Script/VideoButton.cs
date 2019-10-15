using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
public class VideoButton : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject videos;
    public World_Space_video my_players;
    public int current;
    private void OnMouseDown()
    {
        my_players.gameObject.SetActive(true);
        GameObject current_player = videos.transform.GetChild(current).gameObject;
        current_player.SetActive(true);
        my_players.myVideo = current_player.GetComponent<VideoPlayer>();
        my_players.PlayPause();
    }
}
