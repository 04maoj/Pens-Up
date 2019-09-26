using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alphabate_manager : MonoBehaviour
{
    [SerializeField] int stroke_number;
    [SerializeField] int total_score;
    [SerializeField] string strokeName;
    [SerializeField] int double_write_penalities = 3;
    HashSet<int> childrens;
    private HashSet<int> visite_stroke;
    private Stack<int> traverse_order;
    private bool finished;
    int expected_stroke;
     private void Start()
    {
        total_score = 0;
        finished = false;
        childrens = new HashSet<int>();
        visite_stroke = new HashSet<int>();
        traverse_order = new Stack<int>();
        for (int i = 0; i < transform.childCount; i++)
        {
            childrens.Add(i);
        }
        expected_stroke = 0;
    }
    public string Get_Strok_Name()
    {
        return strokeName;
    }
    public void remove_Hit(int index)
    {
        if (childrens.Contains(index))
        {
            childrens.Remove(index);
            traverse_order.Push(index);
        }
        if (childrens.Count == 0 && !finished)
        {

            FindObjectOfType<Scence_Manager>().Decrement_total_Character();
            FindObjectOfType<Track_manager>().Finished_one();
            finished = true;
            return;
        }
    }
    public bool finish()
    {
        return finished;
    }
    public bool Increment_Stroke(int stroke) 
    {
        if(stroke == expected_stroke) {
            expected_stroke++;
            return true;
        }
        return false;
    }

}
