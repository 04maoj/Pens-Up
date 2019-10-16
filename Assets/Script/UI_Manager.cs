using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UI_Manager : MonoBehaviour
{
    public List<RectTransform> courses;
    // Start is called before the first frame update
    public Text named;
    private User_Info user_info;
    private DrawManager drawmgmt;
    private Track_manager trackmgmt;
    private int current_active = 0;
    void Start()
    {
        user_info = FindObjectOfType<User_Info>();
        drawmgmt = FindObjectOfType<DrawManager>();
        trackmgmt = FindObjectOfType<Track_manager>();

        if (named != null)
        {
            if (user_info == null)
            {
                named.text = "Bob the builder";
            }
            else
            {
                named.text = user_info.Get_Name();
            }
        }

        current_active = 0;
        courses[0].gameObject.SetActive(true);
        courses[0].DOAnchorPos(Vector2.zero, 0.25f);
    }
    public void Get_Course(int index)
    {
        //courses[index].gameObject.SetActive(true);
        if (courses[index].gameObject.GetComponent<Profile_Init>() != null)
        {
            courses[index].gameObject.GetComponent<Profile_Init>().Update_Status();
        }
        courses[current_active].DOAnchorPos(new Vector2(-1000, 0), 0.25f);
        courses[index].DOAnchorPos(Vector2.zero, 0.25f);
        //courses[current_active].gameObject.SetActive(false);
        current_active = index;
        //string currentLetter = staticname.i_letter;
        string currentLetter = "A";
        char temp = currentLetter.ToCharArray()[0];
        temp = (char)(temp + index);
        staticname.i_letter = temp.ToString();
        drawmgmt.setCharacter(staticname.i_letter);
        trackmgmt.SetLetter(staticname.i_letter);
        //Debug.Log("Set to: " + staticname.i_letter);
    }

}
