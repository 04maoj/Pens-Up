using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
public class Feedback_init : MonoBehaviour
{
    // Start is called before the first frame update

    string u_name;
    public GameObject spawner;
    public GameObject Content;
    void Start()
    {
        Regex myreg = new Regex(@"\.txt$");
        u_name = FindObjectOfType<User_Info>().user_name;
        string[] feedbacks_file = Directory.GetFiles("Assets/Local_DataBase/Students/" + u_name + "/feedback");
        for (int i = 0; i < feedbacks_file.Length; i++)
        {
            if (myreg.IsMatch(feedbacks_file[i]))
            {
                string[] lines = File.ReadAllLines(feedbacks_file[i]);
                int c = 0;
                string c_name = "";
                string comments = "";
                foreach (string line in lines)
                {
                    if(c == 0)
                    {
                        c_name = line;
                    }
                    else
                    {
                        comments += line;
                    }
                    c++;
                }
                var current_spawned = Instantiate(spawner, transform.position, Quaternion.identity);
                current_spawned.gameObject.transform.SetParent(Content.transform);
                current_spawned.transform.localScale = new Vector3(3, 2, 1);
                current_spawned.GetComponent<Feedback_button>().LoadFeedback(c_name, comments);
            }

        }
    }


}
