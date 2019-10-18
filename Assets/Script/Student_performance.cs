using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XCharts;
using System.IO;
using System;
[DisallowMultipleComponent]
[RequireComponent(typeof(BarChart))]
public class Student_performance : MonoBehaviour
{
    // Start is called before the first frame update
    private BarChart chart;
    Dictionary<string, float> rank;
    public string t_n;
    public bool maunul;
    private void Start()
    {
        chart = transform.GetComponent<BarChart>();
        if (maunul)
        {
            t_n = staticname.i_name;
        } else
            t_n = FindObjectOfType<User_Info>().user_name;
        string path_1 = "Assets/Local_DataBase/Students/" + t_n + "/Total_score_list";
        rank = new Dictionary<string, float>();
        rank.Add("A-H", 0);
        rank.Add("B-List", 0);
        rank.Add("C_List", 0);
        rank.Add("A-List", 0);
        if (File.Exists(path_1))
        {
            StreamReader mysecondReader = new StreamReader(path_1);
            while (!mysecondReader.EndOfStream)
            {
                string[] input = mysecondReader.ReadLine().Split(' ');
                Debug.Log(input[0] + " " + input[1]);
                if (!rank.ContainsKey(input[0]))
                {
                    rank.Add(input[0], float.Parse(input[1]));
                }
                else
                {
                    if(rank[input[0]]- float.Parse(input[1]) < 0.000000001)
                    {
                        rank[input[0]] = float.Parse(input[1]);
                    }
                }
            }

        }
      
        chart.ClearData();
        chart.AddXAxisData("A-H");
        chart.AddXAxisData("B-List");
        chart.AddXAxisData("C-List");
        chart.AddXAxisData("A_List");
        foreach (var temp in rank)
        {
            chart.AddData(0, temp.Value, temp.Key);
        }
        //chart.ClearData();
        //chart.AddData(0, 10, "value1");
        //chart.AddData(0, 10, "value2");
    }

}
