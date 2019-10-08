using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class Goals_button : MonoBehaviour
{   
    // [SerializeField] public GameObject InputField1;
    [SerializeField] public Dropdown drop;
    int status = 0;

    public void setAttemp()
    {
        status = drop.value;
        string user_name = "Handsome";
        string path = "Assets/Local_DataBase/Students/" + user_name + "goals.txt";


        using (StreamWriter sw = File.AppendText(path))
        {
            switch (status)
            {
                case 0: 
                    sw.WriteLine("BASIC"); 
                    break;
                case 1:
                    sw.WriteLine("INTER");
                    break;
                case 2: 
                    sw.WriteLine("EXPERT"); 
                    break;
                default: 
                    StartCoroutine("Back_To_Home");
                    break;
            }
        }
    }

    public IEnumerator Back_To_Home()
    {
        yield return new WaitForSeconds(2);
        SceneLoader q = FindObjectOfType<SceneLoader>();
        q.LoadScence(7);
    }
}
