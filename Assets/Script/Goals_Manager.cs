using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class Goals_Manager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject goal_list;
    public void SaveGoals()
    {
        string user_name = FindObjectOfType<User_Info>().user_name;
        string path = "Assets/Local_DataBase/Students/" + user_name + "/goals.txt";
        StreamWriter sw = File.AppendText(path);
        for (int i = 0;  i < goal_list.transform.childCount; i ++)
        {
            Transform obj = goal_list.transform.GetChild(i);
            string goals = obj.GetChild(0).GetComponent<Text>().text;
            goals += " " + obj.GetChild(1).GetComponent<Dropdown>().options[obj.GetChild(1).GetComponent<Dropdown>().value].text;
            sw.WriteLine(goals);
            Debug.Log(goals);
        }
        sw.Close();
    }
}
