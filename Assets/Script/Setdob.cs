using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Setdob : MonoBehaviour
{

  public Text named;
  public InputField year;
  public InputField month;
  public InputField day;
  public Text dob;

  public void setDob()
  {

    string teacher_path = "Assets/Local_DataBase/Teachers/" + named.text  + "/config.txt";

    dob.text=day.text + "-" + month.text + "-" + year.text;

    using (StreamWriter sw = File.AppendText(teacher_path)) { sw.WriteLine(dob.text); }
  
  }

  public void getDob()
  {
    string teacher_path = "Assets/Local_DataBase/Teachers/" + named.text  + "/config.txt";
    var contents = File.ReadLines(teacher_path);
    // dob.text = contents[contents.Length - 1];
  }
}
