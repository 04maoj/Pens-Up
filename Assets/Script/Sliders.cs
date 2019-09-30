using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliders : MonoBehaviour
{
    // Start is called before the first frame update
    int current = 0;
    private void OnMouseDown()
    {
        current += 1;
        FindObjectOfType<Scence_Manager>().SwitchTut(current);
    }
    public void Update_course(int to)
    {
        current = to;
    }
}
