using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliders : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnMouseDown()
    {
        FindObjectOfType<UI_Manager>().Get_B();
    }
}
