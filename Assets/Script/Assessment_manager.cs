using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assessment_manager : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> active_stars;
    public List<GameObject> inactive_stars;
    private string current_worst = "";
    private int current_worst_score =1000000000;
    private int average_sequence= 0;
    private int average_hit_board=0;
    private int average_deviation=0;
    private int average_order=0;
    private int total_score = 0;


    private void Start()
    {
        UpdateStatus();
    }

    public void UpdateStatus()
    {
        total_score = 0;
        Assessment[] all_pratice = FindObjectsOfType<Assessment>();
        for(int i = 0; i < all_pratice.Length; i ++)
        {
            if (all_pratice[i].GetTotalScore() < current_worst_score)
            {
                current_worst = all_pratice[i].gameObject.name;
                current_worst_score = all_pratice[i].GetTotalScore();
            }
            total_score += all_pratice[i].GetTotalScore();
            average_sequence += all_pratice[i].Average_Sequence();
            average_hit_board += all_pratice[i].Average_hit_board();
            average_deviation += all_pratice[i].Average_deviation();
            average_order += all_pratice[i].Average_Incorect_stroke();
        }
        average_sequence /= all_pratice.Length;
        average_hit_board /= all_pratice.Length;
        average_deviation /= all_pratice.Length;
        average_order /= all_pratice.Length;
        total_score /= all_pratice.Length;
        Debug.Log(total_score);
        if (total_score >= 1)
        {
            active_stars[0].SetActive(true);
            inactive_stars[0].SetActive(false);
        }
        else
        {
            active_stars[0].SetActive(false);
            inactive_stars[0].SetActive(true);
        }
        if (total_score >= 4)
        {
            active_stars[1].SetActive(true);
            inactive_stars[1].SetActive(false);
        }
        else
        {
            active_stars[1].SetActive(false);
            inactive_stars[1].SetActive(true);
        }
        if (total_score >= 7)
        {
            active_stars[2].SetActive(true);
            inactive_stars[2].SetActive(false);
        }
        else
        {
            active_stars[2].SetActive(false);
            inactive_stars[2].SetActive(true);
        }
    }

}
