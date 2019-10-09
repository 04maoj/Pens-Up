using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    void Start()
    {
        user  = FindObjectOfType<User_Info>();
        user_name.text= FindObjectOfType<User_Info>().Get_Name();
        teacher_name.text = FindObjectOfType<User_Info>().Get_Teacher();
    }
    public void Update_Status()
    {
        for(int i = 0; i  < courses_name.Count; i ++)
        {
            if(user.IsCourseFinished(courses_name[i]))
            {
                Debug.Log(courses_name[i]);
                Status.transform.GetChild(i).GetComponent<Image>().sprite = finshed;
            }
            else
            {
                Status.transform.GetChild(i).GetComponent<Image>().sprite = not_finshed;
            }
        }
    }

}
