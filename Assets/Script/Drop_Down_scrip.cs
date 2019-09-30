using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Drop_Down_scrip : MonoBehaviour
{
    // Start is called before the first frame update

    public void OnDropDownChanged(Dropdown dropDown)
    {
        Debug.Log("DROP DOWN CHANGED -> " + dropDown.value);
    }
}
