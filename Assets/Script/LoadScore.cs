using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class LoadScore : MonoBehaviour
{
    User_Info user;
    string user_name;
    private DrawManager DrawManager;
    
    // Start is called before the first frame update
    void Start()
    {
        user = FindObjectOfType<User_Info>();
        //user_name = staticname.i_name;
        user_name = user.Get_UserName();
        string path = "Assets/Local_DataBase/Students/" + user_name + "/Assessment1.score";
        Text box = this.gameObject.GetComponent<Text>();
        StreamReader sr = new StreamReader(path);
        string line = sr.ReadLine();
        float score = float.Parse(line);
        Debug.Log(score);
        box.text = score.ToString();
    }

    
}
