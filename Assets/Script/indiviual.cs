using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class indiviual : MonoBehaviour
{
    public Text name;
    public void click(){
      // but = GameObject.Find("Button");
      // Button letter = but.GetComponent<Button>();
      var buttonSelf = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
      // buttonSelf.GetComponent<Image>().color = Color.red;
      Text text=buttonSelf.gameObject.GetComponentInChildren<Text>();
      Debug.Log(text.text);
      staticname.i_name = text.text;
      Debug.Log(staticname.i_name);
    }

    void Start() {
      name.text= staticname.i_name;
      Debug.Log(staticname.i_name);
    }

}
