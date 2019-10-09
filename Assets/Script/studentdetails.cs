using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class getage : MonoBehaviour
{
    // Start is called before the first frame update
    public string u_name;
    public Text dob;
    public string birthday;
    private HashSet<string> complete_set;
    void Start()
    {
      u_name = staticname.i_name;
      string teacher_path = "Assets/Local_DataBase/Teachers/" + u_name  + "/config.txt";
      //  Debug.Log(u_name+" " + teacher_path);
        complete_set = new HashSet<string>();
        if(File.Exists(teacher_path)) {
            // Debug.Log("Yes It exist");
            string[] lines = File.ReadAllLines(teacher_path);
            foreach (string line in lines) {
              complete_set.Add(line);
              // Debug.Log(line);
              birthday = line;
            }
        }
        Debug.Log(birthday);
        dob.text=birthday;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
