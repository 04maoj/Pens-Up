using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XCharts;
using System.IO;
using System;
[DisallowMultipleComponent]
[RequireComponent(typeof(BarChart))]
public class User_graph : MonoBehaviour
{
    // Start is called before the first frame update
    private BarChart chart;
    Dictionary<int, int> rank;
    public int num_coruses;
    public string t_n;
    private void Start()
    {
        chart = transform.GetComponent<BarChart>();
        chart.SetSize(250, 200);
        t_n = FindObjectOfType<User_Info>().user_name;
        string path = "Assets/Local_DataBase/Teachers/" + t_n + "/Student_List";
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
            Dictionary<string, float> score_list = new Dictionary<string, float>();
            StreamReader mysecondReader = new StreamReader(path_1);
            while (!mysecondReader.EndOfStream)
            {
                string[] input = mysecondReader.ReadLine().Split(' ');
                if (!score_list.ContainsKey(input[0]))
                {

                    score_list.Add(input[0], float.Parse(input[1]));
                }
                else
                {
                    if (score_list[input[0]] - float.Parse(input[1]) < 0.000000001)
                    {
                        score_list[input[0]] = float.Parse(input[1]);
                    }
                }
            }
            double total_score = 0;
            //add the score for each course. 
            foreach (var kvp in score_list)
                total_score += score_list[kvp.Key];
            total_score /= num_coruses;
            //Debug.Log(total_score + "   " + student);
            if (rank.ContainsKey((int)total_score))
                rank[(int)total_score]++;
            else
                rank.Add((int)total_score, 1);
        }
        chart.ClearData();
        foreach (var temp in rank)
        {
            chart.AddXAxisData(temp.Key.ToString());
            //chart.AddXAxisData("B-List");
            //chart.AddXAxisData("C-List");
            //chart.AddXAxisData("A-List");
            chart.AddData(0,temp.Value, temp.Key.ToString());
            //chart.AddData(temp.Key.ToString(),120);
        }
        //chart.ClearData();
        //chart.AddData(0, 10, "value1");
        //chart.AddData(0, 10, "value2");
    }

}
