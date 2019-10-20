using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Set_Student_name : MonoBehaviour
{
    // Start is called before the first frame update
    public Text test;
    public void Set_name() {
FindObjectOfType<User_Info>().setStudent(test);
    }   


}
