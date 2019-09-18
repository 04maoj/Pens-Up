using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alphabate_manager : MonoBehaviour
{
    [SerializeField] int stroke_number;
    [SerializeField] int total_score;



    [SerializeField] int double_write_penalities = 3;

    private HashSet<int> childrens;
    private HashSet<int> visite_stroke;
    private Stack<int> traverse_order;




    private bool finished;
    private void Start()
    {
        total_score = 0;
        finished = false;
        childrens = new HashSet<int>();
        visite_stroke = new HashSet<int>();
        Debug.Log(transform.childCount);
        traverse_order = new Stack<int>();
        for (int i =  0; i < transform.childCount; i ++)
        {
            childrens.Add(i);
        }
    }
    public void remove_Hit(int index, int stroke)
    {
        if (childrens.Contains(index))
        {
            childrens.Remove(index);
            if(visite_stroke.Contains(stroke))
            {
                double_write_penalities -= 1;
            }
            traverse_order.Push(index);
        }
        if (childrens.Count == 0 && !finished)
        {
            Debug.Log("Suucess");
            finished = true;
            LoadFinish();
            return;
        }
    }

    //to be implmented get the info from TrackManagement and store the stroke data in some form of inputs.
    void LoadFinish()
    {

    }
}
