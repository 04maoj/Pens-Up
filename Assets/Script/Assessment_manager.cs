using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class Assessment_manager : MonoBehaviour
{
    // Start is called before the first frame update
    public string course_title;
    public List<GameObject> active_stars;
    public List<GameObject> inactive_stars;
    public GameObject Issue_rank;
    public TextMesh worst;



    private User_Info my_info;
    private char current_worst = '*';
    private int current_worst_score =1000000000;
    private double average_sequence = 0;
    private double average_hit_board =0;
    private double average_deviation =0;
    private double average_order =0;
    private double average_connection = 0;
    private double total_score = 0;
    private void Awake()
    {
        my_info = FindObjectOfType<User_Info>();
        //UpdateStatus();
    }

    public void UpdateStatus()
    {
        total_score = 0;
        average_sequence = 0;
        average_hit_board = 0;
        average_deviation = 0;
        average_order = 0;
        Assessment[] all_pratice = FindObjectsOfType<Assessment>();
        for(int i = 0; i < all_pratice.Length; i ++)
        {
            if (all_pratice[i].GetTotalScore() < current_worst_score)
            {
                current_worst = all_pratice[i].gameObject.name[0];
                current_worst_score = all_pratice[i].GetTotalScore();
            }

            total_score += all_pratice[i].GetTotalScore();
            List<int> scores = new List<int>();
            scores.Add(all_pratice[i].Average_Sequence());
            average_sequence += all_pratice[i].Average_Sequence();

            scores.Add(all_pratice[i].Average_hit_board());
            average_hit_board += all_pratice[i].Average_hit_board();

            scores.Add(all_pratice[i].Average_deviation());
            average_deviation += all_pratice[i].Average_deviation();

            scores.Add(all_pratice[i].Average_Incorect_stroke());
            average_order += all_pratice[i].Average_Incorect_stroke();

            scores.Add(all_pratice[i].AverageConnections());
            average_connection += all_pratice[i].AverageConnections();
            my_info.Store_Individual_Scores(all_pratice[i].gameObject.name, scores, course_title);
        }
        List<Tuple<double, string>> problemList = new List<Tuple<double, string>>();
        average_sequence /= all_pratice.Length;
        Debug.Log(average_sequence);
        problemList.Add(new Tuple<double, string>(average_sequence/2 *100, "Sequence Error"));
        average_hit_board /= all_pratice.Length;
        problemList.Add(new Tuple<double, string>(average_hit_board/1 * 100 * 1.25, "Hit Board Error"));
        average_deviation /= all_pratice.Length;
        problemList.Add(new Tuple<double, string>(average_deviation/3 * 100, "Distance from correct sequence"));
        average_order /= all_pratice.Length;
        problemList.Add(new Tuple<double, string>(average_order/2 * 100, "Order sequence"));
        average_connection /= all_pratice.Length;
        problemList.Add(new Tuple<double, string>(average_connection / 2 * 100, "Connections sequence"));
        total_score /= all_pratice.Length;
        problemList.Sort();
        my_info.Store_Total_Scores(total_score, course_title);
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
        if (total_score >= 5)
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
        for(int i  = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        worst.gameObject.SetActive(true);
        Debug.Log(current_worst);
        worst.text = current_worst + "";
        //if(current_worst < 'a')
        //{
        //    worst.text = current_worst + "";
        //    //transform.GetChild((current_worst-'A')*2).gameObject.SetActive(true);
        //}
        //else
        //{
        //    worst.text = current_worst +"";
        //    //transform.GetChild(((current_worst - 'a')*2)+1).gameObject.SetActive(true);
        //}
        int count = 0;
        double current_min = -1;
        for(int i = 0; i < problemList.Count; i++) {
            Debug.Log(problemList[i].Item1 +"  "+ problemList[i].Item2);
            if(Math.Abs(problemList[i].Item1 - current_min) > 0.0000001 && Math.Abs(problemList[i].Item1 - 100) > 0.000000001 && problemList[i].Item1<= 100)
            {
                count += 1;
                Issue_rank.transform.GetChild(i).GetComponent<TextMesh>().text = count +". " + problemList[i].Item2;
                current_min = problemList[i].Item1;
            } else if (Math.Abs(problemList[i].Item1 - 100) > 0.000001 && problemList[i].Item1 <= 100)
            {
                Issue_rank.transform.GetChild(i).GetComponent<TextMesh>().text = count + ". " + problemList[i].Item2;
            }
            else
            {
                Issue_rank.transform.GetChild(i).GetComponent<TextMesh>().text = "";
            }
        }
    }

}
