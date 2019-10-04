using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class User_Info : MonoBehaviour
{
    // Start is called before the first frame update
    public string user_name = "Bob_the_Builder";
    public string named = "Bob the Builder";
    public string teacher = "Alice the creater";
    private HashSet<string> complete_set;
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    private void Start()
    {
        string path1 = "Assets/Local_DataBase/Students/" + user_name + "/Complted_course";
        complete_set = new HashSet<string>();
        Debug.Log(path1);
        if (File.Exists(path1))
        {
            // Read a text file line by line.
            Debug.Log("Yes");
            string[] lines = File.ReadAllLines(path1);
            foreach (string line in lines)
            {
                complete_set.Add(line);
                Debug.Log(line);
            }
        }
    }
    public void Update_user_name(string nameded, bool students)
    {
        user_name = nameded;
        string path;
        if(students)
        {
            path = "Assets/Local_DataBase/Students/" + user_name + "/config.txt";
        }
        else
        {
            path = "Assets/Local_DataBase/Teachers/" + user_name + "/config.txt";
        }

        StreamReader myReader = new StreamReader(path);
        named = myReader.ReadLine();
        if(students)
        {
            teacher = myReader.ReadLine();
        }
    }
    public string Get_Name()
    {
        return named;
    }
    public string Get_Teacher()
    {
        return teacher;
    }

    public bool IsCourseFinished(string course_name)
    {
        return complete_set.Contains(course_name);
    }

    //storing the score of the students for each characters.
    //each input end with a *.
    //the order of score is sequence score, hit board score, deviation score, incorrect stroke score.
    public void Store_Individual_Scores(string character, List<int> scores,string course_title)
    {
        string path1 = "Assets/Local_DataBase/Students/" + user_name + "/Complted_course";
        complete_set.Add(course_title);
        string path = "Assets/Local_DataBase/Students/" + user_name + "/" + course_title+ "IndivudalPerformance.txt";
        using (StreamWriter sw = File.AppendText(path))
        {
            sw.WriteLine(character);
            for(int i =  0; i < scores.Count; i ++)
            {
                sw.WriteLine(scores[i]);
            }
            sw.WriteLine("*");
        }
        using (StreamWriter writer = new StreamWriter(path1))
        {
            foreach(string courses in complete_set)
            {
                writer.WriteLine(courses);
            }
        }
    }
}
