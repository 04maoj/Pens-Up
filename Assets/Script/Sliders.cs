using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliders : MonoBehaviour
{
    // Start is called before the first frame update
    public bool left;
    private void OnMouseDown()
    {
        int current = 0;
        if (left)
            current -= 1;
        else
            current += 1;
        FindObjectOfType<Scence_Manager>().SwitchTut(current);
    }
    //public void Update_course(int to)
    //{
    //    current = to;
    //}
}
