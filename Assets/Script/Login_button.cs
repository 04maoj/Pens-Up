using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class Login_button : MonoBehaviour
{
    [SerializeField] GameObject password;
    [SerializeField] GameObject user_name;
    [SerializeField] GameObject warming_text;
    public bool student = false;
    StreamReader myReader;
    void Start()
    {
        string path = "Assets/Local_DataBase/Teachers/Teacher_Password.txt";
        if (student)
        {
            path= "Assets/Local_DataBase/Students/Student_Password.txt";
        }
        myReader = new StreamReader(path);
        warming_text.SetActive(false);
    }
    public bool check_Passwor()
    {
        string user = user_name.GetComponent<InputField>().text;
        string pass = password.GetComponent<InputField>().text;
        string line = myReader.ReadLine();
        while(line != null) {
            string[] temp = line.Split(' ');
            if(user.Equals(temp[0]))
            {
                if(pass.Equals(temp[1]))
                {
                    return true;
                }
            }
            line = myReader.ReadLine();
        }
        return false;
    }

    public void Login()
    {
        if(check_Passwor())
        {
            Debug.Log("Yes");
            FindObjectOfType<User_Info>().Update_user_name(user_name.GetComponent<InputField>().text,student);
            SceneLoader manager = FindObjectOfType<SceneLoader>();
            manager.LoadScence(7);
        }
        else
        {
            warming_text.SetActive(true);
        }
    }
    public void Register()
    {
        SceneLoader manager = FindObjectOfType<SceneLoader>();
        manager.LoadScence(3);
    }
}
