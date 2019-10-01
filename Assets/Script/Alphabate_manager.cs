using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alphabate_manager : MonoBehaviour
{
    //preset
    [SerializeField] int stroke_number;
    [SerializeField] int total_score;
    [SerializeField] string strokeName;
    [SerializeField] int double_write_penalities = 3;
    HashSet<int> childrens;
    private HashSet<int> visite_stroke;
    private Stack<int> traverse_order;
    private List<int>[] strokes_and_hit_box;
    private bool finished;
    int expected_stroke;
     private void Start()
    {
        Reset();
    }
    public string Get_Strok_Name()
    {
        return strokeName;
    }
    public bool remove_Hit(List<int> to_be_delete, int stroke_number)
    {
        traverse_order.Clear();
        for(int i = 0; i < to_be_delete.Count; i ++)
        {
            if (childrens.Contains(to_be_delete[i]))
            {
                childrens.Remove(to_be_delete[i]);
                traverse_order.Push(to_be_delete[i]);
            }
        }
        if (traverse_order.Count == 0)
            return false;
        for(int i = strokes_and_hit_box[stroke_number].Count- 1; i> -1; i--)
        {
            if(traverse_order.Count == 0)
            {
                break;
            }
            int u = traverse_order.Pop();
            Debug.Log(u + "   " + strokes_and_hit_box[stroke_number][i]);
            if(u != strokes_and_hit_box[stroke_number][i])
            {
                //Debug.Log(u);
                for(int j = 0; j < to_be_delete.Count; j ++)
                {
                    childrens.Add(to_be_delete[j]);
                }
                return false;
            }
        }

        if (childrens.Count == 0 && !finished)
        {

            FindObjectOfType<Scence_Manager>().Decrement_total_Character();
            FindObjectOfType<Track_manager>().Finished_one();
            finished = true;
            return true;
        }
        return true;
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
    public void Reset()
    {
        total_score = 0;
        finished = false;
        childrens = new HashSet<int>();
        visite_stroke = new HashSet<int>();
        strokes_and_hit_box = new List<int>[stroke_number];
        traverse_order = new Stack<int>();
        for (int i = 0; i < transform.childCount; i++)
        {
            childrens.Add(i);
        }
        expected_stroke = 0;
        if(strokeName == "A")
        {
            strokes_and_hit_box[0] = new List<int> { 0, 1, 2, 3, 4, 5 };
            strokes_and_hit_box[1] = new List<int> { 6, 7, 8, 9, 10 };
            strokes_and_hit_box[2] = new List<int> { 11, 12};
        } else if(strokeName == "a") {
            strokes_and_hit_box[0] = new List<int> { 0, 1, 2, 3, 4, 5};
        } else if(strokeName == "B")
        {
            strokes_and_hit_box[0] = new List<int> { 0, 1, 2, 3, 4 };
            strokes_and_hit_box[1] = new List<int> { 0, 5, 6, 7, 8,9 };
            strokes_and_hit_box[2] = new List<int> { 9,10, 11, 12,13};
        }

    }
    public Assessment Get_Assessment()
    {
        return gameObject.GetComponent<Assessment>();
    }
}
