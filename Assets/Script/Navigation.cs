using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigation : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] int next;
    public bool is_Box = false;
    public int to;
    public void OnMouseDown()
    {
        if(!is_Box)
        {
            SceneLoader loader = FindObjectOfType<SceneLoader>();
            loader.LoadScence(next);
        }
        else
        {
            FindObjectOfType<UI_Manager>().Get_Course(to);
        }
    }
}
