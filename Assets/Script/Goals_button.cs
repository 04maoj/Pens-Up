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
    public void setAttemp()
    {
        status = drop.value;
        string goal1 = inputfield1.GetComponent <InputField>().text;
        string goal2 = inputfield2.GetComponent<InputField>().text;
        string goal3 = inputfield3.GetComponent <InputField>().text;
        string goal4a = inputfield4_min.GetComponent<InputField>().text;
        string goal4b = inputfield4_day.GetComponent <InputField>().text;
        string path = "";
        if (status == 0)
        {
            path = "Assets/Local_DataBase/Students/Student_Goal.txt";
        } 
        else {
            path = "Assets/Local_DataBase/Students/Student_Goal.txt";
        }

        using (StreamWriter sw = File.AppendText(path))
        {
            sw.WriteLine(goal1 + " " + goal2 + " " + goal3 + " " + goal4a + " " + goal4b);
            StartCoroutine("Back_To_Home");
        }
    }
    public IEnumerator Back_To_Home()
    {
        success_text.SetActive(true);
        yield return new WaitForSeconds(2);
        SceneLoader q = FindObjectOfType<SceneLoader>();
        q.LoadScence(11);
    }
}
