using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class Add_Feed_Back_button : MonoBehaviour
{
    // Start is called before the first frame update
    public Text manual;
    public Text title;
    public Text success;
    public string user_name;
    public void Write_Comment()
    {
        string path = "Assets/Local_DataBase/Students/" + user_name + "/feedback/" + title.text + ".txt";
        using (StreamWriter sw = File.AppendText(path))
        {
            sw.WriteLine(title.text);
            sw.WriteLine(manual.text);
        }
        success.enabled = true;
    }
}
