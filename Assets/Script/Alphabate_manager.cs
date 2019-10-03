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
    List<int> childrens;
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
    public bool remove_Hit(HashSet<int> to_be_delete, int stroke_number)
    {
        traverse_order.Clear();
        List<int> get_back = new List<int>();
        foreach (int to_dete in to_be_delete)
        {
            if (childrens.Contains(to_dete))
            {
                childrens.Remove(to_dete);
                traverse_order.Push(to_dete);
                get_back.Add(to_dete);
            }
        }
        if (traverse_order.Count == 0)
            return false;
        for(int i = strokes_and_hit_box[stroke_number].Count- 1; i> -1; i--)
        {
            if(traverse_order.Count == 0)
            {
                for(int j = 0; j < get_back.Count; j ++)
                {
                    childrens.Add(get_back[j]);
                }
                return false;
            }
            int u = traverse_order.Pop();
            Debug.Log(u + "   " + strokes_and_hit_box[stroke_number][i]);
            if(u != strokes_and_hit_box[stroke_number][i])
            {
                //Debug.Log(u);
                for (int j = 0; j < get_back.Count; j++)
                {
                    childrens.Add(get_back[j]);
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
        } else {
            Debug.Log(childrens.Count);
            for(int i = 0; i < childrens.Count; i ++) {
                Debug.Log("They aer " + childrens[i]);
             }
            
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
        childrens = new List<int>();
        visite_stroke = new HashSet<int>();
        strokes_and_hit_box = new List<int>[stroke_number];
        traverse_order = new Stack<int>();
        for (int i = 0; i < transform.childCount; i++)
        {
            for(int j = 0; j < transform.GetChild(i).GetComponent<Hit_Box>().stroke_number.Count;j++) {
                childrens.Add(transform.GetChild(i).GetComponent<Hit_Box>().index);
            }
        }
        expected_stroke = 0;
        if(strokeName == "A")
        {
            strokes_and_hit_box[0] = new List<int> { 0, 1, 2, 3, 4, 5 };
            strokes_and_hit_box[1] = new List<int> {0, 6, 7, 8, 9, 10 };
            strokes_and_hit_box[2] = new List<int> { 3,11, 12,8};
        } else if(strokeName == "a") {
            strokes_and_hit_box[0] = new List<int> { 0, 1, 2, 3, 4};
        } else if(strokeName == "B")
        {
            strokes_and_hit_box[0] = new List<int> { 0, 1, 2, 3, 4 };
            strokes_and_hit_box[1] = new List<int> {0, 5, 6, 7, 8,9,2 };
            strokes_and_hit_box[2] = new List<int> {2, 9,10, 11, 12,13,4};
        } else if(strokeName == "C")
        {
            strokes_and_hit_box[0] = new List<int> { 0,1,2,3,4,5,6,7 };
        } else if(strokeName  == "c")
        {
            strokes_and_hit_box[0] = new List<int> { 0, 1, 2, 3, 4};
        } else if(strokeName == "b") {
            strokes_and_hit_box[0] = new List<int> { 0, 1, 2, 3};
            strokes_and_hit_box[1] = new List<int> { 1, 4 , 5 , 6 , 7 , 8,3 };
        }


    }
    public Assessment Get_Assessment()
    {
        return gameObject.GetComponent<Assessment>();
    }
}
