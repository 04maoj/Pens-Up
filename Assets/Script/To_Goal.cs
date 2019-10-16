using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class To_Goal : MonoBehaviour
{
    public Text temp;
    public void Set_Student()
    {
        FindObjectOfType<User_Info>().setStudent(temp);
    }
}
