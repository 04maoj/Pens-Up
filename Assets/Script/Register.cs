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
    [SerializeField] public GameObject success_text;
    [SerializeField] public Dropdown teacher_list;
    public void registerAttemp()
    {
        status = drop.value;
        string user = user_name.GetComponent <InputField>().text;
        string pass = password.GetComponent<InputField>().text;
        string path = "";
        if (status == 0)
        {
            path = "Assets/Local_DataBase/Teachers/Teacher_Password.txt";
            Directory.CreateDirectory("Assets/Local_DataBase/Teachers/" + user);
        } 
        else {
            path = "Assets/Local_DataBase/Students/Student_Password.txt";
            Directory.CreateDirectory("Assets/Local_DataBase/Students/" + user);
        }

        using (StreamWriter sw = File.AppendText(path))
        {
            sw.WriteLine(user + " " + pass);
            StartCoroutine("Back_To_Home");
        }
    }
    private void Load_teacher_list()
    {

    }
    public IEnumerator Back_To_Home()
    {
        success_text.SetActive(true);
        yield return new WaitForSeconds(2);
        SceneLoader q = FindObjectOfType<SceneLoader>();
        q.LoadScence(0);
    }
}
