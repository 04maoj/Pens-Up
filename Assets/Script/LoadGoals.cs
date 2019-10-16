using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
public class LoadGoals : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Content;
    public GameObject spawner;
    public bool isTeacher;
    void Start()
    {
        string user_name;
        if (isTeacher)
        {
            user_name = FindObjectOfType<User_Info>().c_student;
            Debug.Log(user_name);
        }
        else
        {
            user_name = FindObjectOfType<User_Info>().user_name;
        }
        string path_1 = "Assets/Local_DataBase/Students/" + user_name + "/goals.txt";
        StreamReader myReader = new StreamReader(path_1);
        while (!myReader.EndOfStream)
        {
            string temp = myReader.ReadLine();
            var current_spawned = Instantiate(spawner, transform.position, Quaternion.identity);
            current_spawned.gameObject.transform.SetParent(Content.transform);
            current_spawned.transform.localScale = new Vector3(1, 1, 1);
            current_spawned.transform.GetChild(0).GetComponent<Text>().text = temp;
        }
        myReader.Close();

    }

}
