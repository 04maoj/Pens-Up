using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class Profile_Init : MonoBehaviour
{
    // Start is called before the first frame update
    public Text user_name;
    public Text teacher_name;
    public List<string> courses_name;
    public GameObject Status;
    public Sprite finshed;
    public Sprite not_finshed;
    private User_Info user;
    public GameObject seeGoals;
    void Start()
    {
        user = FindObjectOfType<User_Info>();
        user_name.text = FindObjectOfType<User_Info>().Get_Name();
        teacher_name.text = FindObjectOfType<User_Info>().Get_Teacher();
        string user_named = FindObjectOfType<User_Info>().user_name;
        if(File.Exists("Assets/Local_DataBase/Students/" + user_named + "/goals.txt"))
        {
            seeGoals.SetActive(true);
        }
        else
        {
            seeGoals.SetActive(false);
        }
    }
    public void Update_Status()
    {
        for (int i = 0; i < courses_name.Count; i++)
        {
            if (user.IsCourseFinished(courses_name[i]))
            {
                Status.transform.GetChild(i).GetComponent<Image>().sprite = finshed;
            }
            else
            {
                Status.transform.GetChild(i).GetComponent<Image>().sprite = not_finshed;
            }
        }
    }

}
