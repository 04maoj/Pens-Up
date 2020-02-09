using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Hit_Box : MonoBehaviour
{
    [SerializeField] public int index;
    [SerializeField] public List<int> stroke_number = new List<int>();
    /// <summary>
    /// Return the stroke that this hit box belongs and a tuple of the hit box index and it's parent.
    /// </summary>
    /// <returns></returns>
    public Tuple<Alphabate_manager, int> F_GetsInfo()
    {
        string character = gameObject.GetComponentInParent<Alphabate_manager>().Get_Strok_Name();
        Tuple<Alphabate_manager, int> temp = new Tuple<Alphabate_manager, int>(gameObject.GetComponentInParent<Alphabate_manager>(), index);
        return temp;
    }

}
