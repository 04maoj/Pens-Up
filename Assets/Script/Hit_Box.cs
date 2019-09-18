using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit_Box : MonoBehaviour
{
    [SerializeField] int index;
    [SerializeField] int stroke;
    public void deleteItSelf()
    {
        gameObject.GetComponentInParent<Alphabate_manager>().remove_Hit(index, stroke);
    }

}
