using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class Register : MonoBehaviour
{   
    //0 for teacher and 1 for student
    int status = 0;
    [SerializeField] public Dropdown drop;
    [SerializeField] public GameObject password;
    [SerializeField] public GameObject user_name;
    [SerializeField] public GameObject name_g;
    [SerializeField] public GameObject success_text;
    [SerializeField] public Dropdown teacher_list;
    public void registerAttemp()
    {
        status = drop.value;
        string user = user_name.GetComponent <InputField>().text;
        string pass = password.GetComponent<InputField>().text;
        string names = name_g.GetComponent<InputField>().text;
        string path = "";
        string path1 = "";
        if (status == 0)
        {
            path = "Assets/Local_DataBase/Teachers/Teacher_Password.txt";
            Directory.CreateDirectory("Assets/Local_DataBase/Teachers/" + user);
            path1 = "Assets/Local_DataBase/Teachers/" + user  + "/config.txt";
        } 
        else {
            path = "Assets/Local_DataBase/Students/Student_Password.txt";
            Directory.CreateDirectory("Assets/Local_DataBase/Students/" + user);
            Directory.CreateDirectory("Assets/Local_DataBase/Students/" + user+"/feedback");
            path1 = "Assets/Local_DataBase/Students/" + user + "/config.txt";
        }
        using (StreamWriter sw = File.AppendText(path1))
        {
            sw.WriteLine(names);
            if (status == 1)
            {
                int current_te = teacher_list.value;
                string myteacher = teacher_list.options[current_te].text;
                sw.WriteLine(myteacher);
                string path_2 = "Assets/Local_DataBase/Teachers/" + myteacher + "/Student_List.txt";
                using (StreamWriter sw_2 = File.AppendText(path_2))
                {
                    sw_2.WriteLine(user);
                }
            }
        }
        using (StreamWriter sw = File.AppendText(path))
        {
            sw.WriteLine(user + " " + pass);
            StartCoroutine("Back_To_Home");
        }
    }
    private void Load_teacher_list()
    {
        teacher_list.gameObject.SetActive(true);
        teacher_list.ClearOptions();
        string path = "Assets/Local_DataBase/Teachers/Teacher_Password.txt";
        StreamReader myReader = new StreamReader(path);
        List<string> teacher = new List<string>();
        string line = myReader.ReadLine();
        while (line != null)
        {
            string[] temp = line.Split(' ');
            if (temp.Length < 2)
            {
                Debug.Log("error when reading file");
                return;
            }
            teacher.Add(temp[0]);
            line = myReader.ReadLine();
        }
        teacher_list.AddOptions(teacher);
    }
    public IEnumerator Back_To_Home()
    {
        success_text.SetActive(true);
        yield return new WaitForSeconds(2);
        SceneLoader q = FindObjectOfType<SceneLoader>();
        q.LoadScence(0);
    }
    public void DropdownValueChanged(Dropdown change)
    {
        Debug.Log(change.value);
        if(change.value ==1)
        {
            Load_teacher_list();
        }
    }
}
