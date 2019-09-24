using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Hit_Box : MonoBehaviour
{
    [SerializeField] int index;
    [SerializeField] List<int> stroke_number = new List<int>();
    public Tuple<List<int>, Alphabate_manager> deleteItSelf()
    {
        string character = gameObject.GetComponentInParent<Alphabate_manager>().Get_Strok_Name();
        gameObject.GetComponentInParent<Alphabate_manager>().remove_Hit(index);
        return new Tuple<List<int>, Alphabate_manager>(stroke_number, gameObject.GetComponentInParent<Alphabate_manager>());
    }

}
