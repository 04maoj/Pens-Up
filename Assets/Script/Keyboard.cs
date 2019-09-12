using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyboard : MonoBehaviour
{
    // Start is called before the first frame update
    TouchScreenKeyboard keyboard;
    private void OnMouseDown()
    {
        keyboard.active = true;
        Debug.Log("Ture");
    }
}
