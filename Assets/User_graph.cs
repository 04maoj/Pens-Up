using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XCharts;
using System.IO;
using System;
[DisallowMultipleComponent]
[RequireComponent(typeof(PieChart))]
public class User_graph : MonoBehaviour
{
    // Start is called before the first frame update
    private PieChart chart;
    Dictionary<int, int> rank;
    public int num_coruses;
    public string t_n;
    private void Start()
    {
        chart = transform.GetComponent<PieChart>();
        string path = "Assets/Local_DataBase/Teachers/" + t_n + "/Student_List.txt";
        StreamReader myReader = new StreamReader(path);
        rank = new Dictionary<int, int>();
        while (!myReader.EndOfStream)
        {
            string student = myReader.ReadLine();
            // Reading the score.
            string path_1 = "Assets/Local_DataBase/Students/" + student + "/Total_score_list";
            if (!File.Exists(path_1))
            {
                if (rank.ContainsKey(0))
                    rank[0]++;
                else
                    rank.Add(0, 1);
                continue;
            }
            //use hashmap to prevent duplicates.
            Dictionary<string, int> score_list = new Dictionary<string, int>();
            StreamReader mysecondReader = new StreamReader(path_1);
            while (!mysecondReader.EndOfStream)
            {
                string[] input = mysecondReader.ReadLine().Split(' ');
                if (!score_list.ContainsKey(input[0]))
                {
                    score_list.Add(input[0], int.Parse(input[1]));
                }
                else
                {
                    score_list[input[0]] = Mathf.Max(score_list[input[0]], int.Parse(input[1]));
                }
            }
            double total_score = 0;
            //add the score for each course. 
            foreach (var kvp in score_list)
                total_score += score_list[kvp.Key];
            total_score /= num_coruses;
            if (rank.ContainsKey((int)total_score))
                rank[(int)total_score]++;
            else
                rank.Add((int)total_score, 1);
        }
        chart.ClearData();
        foreach (var temp in rank)
        {
            chart.AddData(0, temp.Value, temp.Key.ToString());
        }
        //chart.ClearData();
        //chart.AddData(0, 10, "value1");
        //chart.AddData(0, 10, "value2");
    }

}
