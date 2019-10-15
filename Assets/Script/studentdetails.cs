using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class studentdetails : MonoBehaviour
{
    // Start is called before the first frame update
    public string u_name;
    public Text dob;
    public string birthday;
    private HashSet<string> complete_set;
    private HashSet<string> complete_set2;
    public GameObject spawner;
    public GameObject Content;
    void Start()
    {
      u_name = staticname.i_name;
      string teacher_path = "Assets/Local_DataBase/Students/" + u_name  + "/config.txt";
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

        string course_path = "Assets/Local_DataBase/Students/" + u_name  + "/Complted_course";
        Debug.Log(u_name+" " + course_path);
        complete_set2 = new HashSet<string>();
          if(File.Exists(course_path)) {
              Debug.Log("Yes It exist");
              string[] lines = File.ReadAllLines(course_path);
              foreach (string line in lines) {
                complete_set2.Add(line);
                Debug.Log(line);
                var current_spawned = Instantiate(spawner, transform.position, Quaternion.identity);
                current_spawned.gameObject.transform.SetParent(Content.transform);
                current_spawned.transform.localScale = new Vector3(1, 1, 1);
                current_spawned.transform.GetChild(0).GetComponent<Text>().text = line;

              }
          }


    }

    // Update is called once per frame
    void Update()
    {

    }
}
