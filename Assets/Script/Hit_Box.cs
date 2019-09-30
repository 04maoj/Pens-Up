using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Hit_Box : MonoBehaviour
{
    [SerializeField] int index;
    [SerializeField] public List<int> stroke_number = new List<int>();
    //Return the stroke that this hit box belongs and a tuple of the hit box index and it's parent.
    public Tuple<List<int>, Tuple<Alphabate_manager, int>> deleteItSelf()
    {
        string character = gameObject.GetComponentInParent<Alphabate_manager>().Get_Strok_Name();
        //gameObject.GetComponentInParent<Alphabate_manager>().remove_Hit(index);
        Tuple<Alphabate_manager, int> temp = new Tuple<Alphabate_manager, int>(gameObject.GetComponentInParent<Alphabate_manager>(), index);
        return new Tuple<List<int>, Tuple<Alphabate_manager, int> >(stroke_number, temp);
    }

}
