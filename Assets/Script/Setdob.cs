using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setdob : MonoBehaviour
{
  public InputField year;
  public InputField month;
  public InputField day;
  public Text dob;
  public void setget(){
    dob.text=day.text + "-" + month.text + "-" + year.text;
    }
}
