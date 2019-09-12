using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Login_button : MonoBehaviour
{
    [SerializeField] GameObject password;
    [SerializeField] GameObject user_name;
    [SerializeField] GameObject warming_text;
    // Start is called before the first frame update
    StreamReader myReader;
    void Start()
    {
        string path = "Assets/Local_DataBase/Teachers/Teacher_Password.txt";
        myReader = new StreamReader(path);
        warming_text.SetActive(false);
    }

    // Update is called once per frame
    public bool check_Passwor()
    {
        string user = user_name.GetComponent<UnityEngine.UI.Text>().text;
        string pass = password.GetComponent<UnityEngine.UI.Text>().text;
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
            SceneLoader manager = FindObjectOfType<SceneLoader>();
            manager.LoadScence(4);
        }
        else
        {
            warming_text.SetActive(true);
        }
    }
}
