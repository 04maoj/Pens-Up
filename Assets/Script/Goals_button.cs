using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class Goals_button : MonoBehaviour
{   
    [SerializeField] public GameObject InputField1;
    [SerializeField] public GameObject InputField2;
    [SerializeField] public GameObject InputField3;
    [SerializeField] public GameObject InputField4_day;
    [SerializeField] public GameObject InputField4_min;
    [SerializeField] public GameObject success_text;
    public void setAttemp()
    {
        string goal1 = InputField1.GetComponent <InputField>().text;
        string goal2 = InputField2.GetComponent<InputField>().text;
        string goal3 = InputField3.GetComponent <InputField>().text;
        string goal4a = InputField4_min.GetComponent<InputField>().text;
        string goal4b = InputField4_day.GetComponent <InputField>().text;
        
        string path = "";
        path = "Assets/Local_DataBase/Students/Student_Goal.txt";

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
