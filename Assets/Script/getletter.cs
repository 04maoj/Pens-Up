using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class getletter : MonoBehaviour
{
    
    // Start is called before the first frame update
    public void click()
    {
      var buttonSelf = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
      // buttonSelf.GetComponent<Image>().color = Color.red;
      Text text=buttonSelf.gameObject.GetComponentInChildren<Text>();
      Debug.Log(text.text);
      staticname.i_letter = text.text;
    }


}
