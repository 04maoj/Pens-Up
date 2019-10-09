using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Feedback_button : MonoBehaviour
{
    // Start is called before the first frame update
    private string feedback;
    private string names;
    public void LoadFeedback(string c_name, string comments) {
        names = c_name;
        transform.GetChild(0).GetComponent<Text>().text = c_name;
        feedback = comments;
    }

    public void setMenue()
    {
        FindObjectOfType<Feedback_Menue>().Load_Message(feedback);
    }

}
