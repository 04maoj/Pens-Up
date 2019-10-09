using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
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
        ranks = new List<Tuple<double, string>>();
        t_n = FindObjectOfType<User_Info>().Get_Teacher();
        string path = "Assets/Local_DataBase/Teachers/" + t_n + "/Student_List.txt";
        StreamReader myReader = new StreamReader(path);
        while(!myReader.EndOfStream) {
            string student = myReader.ReadLine();
            // Reading the score.
            string path_1 = "Assets/Local_DataBase/Students/" + student + "/Total_score_list";
            if(!File.Exists(path_1))
            {
                Debug.Log(path_1);
                ranks.Add(new Tuple<double, string>(0, student));
                continue;
            }
            //use hashmap to prevent duplicates.
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
            //add the score for each course. 
            foreach (var kvp in score_list)
                total_score += score_list[kvp.Key];
            total_score /= number_course;
            ranks.Add(new Tuple<double, string>(total_score, student));
        }
        ranks.Sort();
        ranks.Reverse();
        for(int i = 0; i < ranks.Count; i++)
        {
            var current_spawned = Instantiate(spawner, transform.position, Quaternion.identity);
            current_spawned.gameObject.transform.SetParent(Content.transform);
            current_spawned.transform.localScale = new Vector3(1, 1, 1);
            current_spawned.transform.GetChild(0).GetComponent<Text>().text = ranks[i].Item1 + " " + ranks[i].Item2;
        }
    }

}
