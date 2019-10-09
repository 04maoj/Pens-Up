using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class indiviual : MonoBehaviour
{
    public GameObject but;
    public void go(){
      but = GameObject.Find("Button");
      Button letter = but.GetComponent<Button>();
      letter.GetComponent<Image>().color = Color.red;
    }

}
