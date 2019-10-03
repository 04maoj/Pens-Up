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
    private void Awake()
    {
        DontDestroyOnLoad(this);
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

    //storing the score of the students for each characters.
    //each input end with a *.
    //the order of score is sequence score, hit board score, deviation score, incorrect stroke score.
    public void Store_Individual_Scores(string character, List<int> scores)
    {
        Debug.Log("here");
        string path = "Assets/Local_DataBase/Students/" + user_name + "/IndivudalPerformance.txt";
        using (StreamWriter sw = File.AppendText(path))
        {
            sw.WriteLine(character);
            for(int i =  0; i < scores.Count; i ++)
            {
                sw.WriteLine(scores[i]);
            }
            sw.WriteLine("*");
        }
    }
}
