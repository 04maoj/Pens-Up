using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class studentdetails : MonoBehaviour
{
    // Start is called before the first frame update
    public string u_name;
    public string birthday;
    private HashSet<string> complete_set;
    private HashSet<string> complete_set2;
    public GameObject spawner;
    public GameObject Content;
    void Start()
    {
      u_name = staticname.i_name;
      
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
