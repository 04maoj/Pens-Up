using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alphabate_manager : MonoBehaviour
{
    private HashSet<int> childrens;
    private bool finished;
    private void Start()
    {
        finished = false;
        childrens = new HashSet<int>();
        Debug.Log(transform.childCount);
        for (int i =  0; i < transform.childCount; i ++)
        {
            childrens.Add(i);
        }
    }
    public void remove_Hit(int index)
    {
        if (childrens.Count == 0 && !finished)
        {
            Debug.Log("Suucess");
            finished = true;
            return;
        }
        if (childrens.Contains(index))
            childrens.Remove(index);
    }
}
