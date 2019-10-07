using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
public class Ranking_Init : MonoBehaviour
{
    string t_n;
    public int number_course;
    List<Tuple<double, string>> ranks;
    public GameObject Content;
    public GameObject spawner;
    void Start()
    {
        t_n = FindObjectOfType<User_Info>().Get_Teacher();
        string path = "Assets/Local_DataBase/Teachers/" + t_n + "/Student_List.txt";
        StreamReader myReader = new StreamReader(path);
        while(!myReader.EndOfStream) {
            string student = myReader.ReadLine();
            string path_1 = "Assets/Local_DataBase/Students/" + student + "/Total_score_list";
            Dictionary<string, int> score_list = new Dictionary<string, int>();
            StreamReader mysecondReader = new StreamReader(path_1);
            while(!mysecondReader.EndOfStream)
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
            foreach (var kvp in score_list)
                total_score = score_list[kvp.Key];
            total_score /= number_course;
            ranks.Add(new Tuple<double, string>(total_score, student));
        }
        ranks.Sort();
        for(int i = 0; i < ranks.Count; i++)
        {
            var current_spawned = (spawner, transform.position, transform.rotation);
            //current_spawne;
        }
    }

}
