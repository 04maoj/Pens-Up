using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Setdob : MonoBehaviour
{
  public InputField year;
  public InputField month;
  public InputField day;
  public Text dob;
  public string birthday;
  public string u_name;

  private HashSet<string> complete_set;

  public void setDob()
  {
	 dob.text=day.text + "-" + month.text + "-" + year.text;

   u_name = FindObjectOfType<User_Info>().user_name;
   string teacher_path = "Assets/Local_DataBase/Teachers/" + u_name  + "/config.txt";
   using (StreamWriter sw = File.AppendText(teacher_path)) { sw.WriteLine(dob.text); }

  }

  void Start()
  {
    u_name = FindObjectOfType<User_Info>().user_name;
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
}
