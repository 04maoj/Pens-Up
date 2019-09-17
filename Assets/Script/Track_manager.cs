using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track_manager : MonoBehaviour
{
    public GameObject prefabs ;
    public GameObject current;
    Vector3 start;
    Plane objPlane;
    private void Start()
    {
        objPlane = new Plane(Camera.main.transform.forward * -1, this.transform.position);
    }
    void Update()
    {   
        //Some on just stated 
        if((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            Ray myRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            float rayDistance;
            if (objPlane.Raycast(myRay, out rayDistance))
                start = myRay.GetPoint(rayDistance);
            current = Instantiate(prefabs, start, Quaternion.identity);
            RaycastHit test_hit;
            myRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(myRay,out test_hit))
            {
                if(test_hit.collider!= null)
                {
                   if(test_hit.collider.GetComponent<Hit_Box>() != null)
                        test_hit.collider.GetComponent<Hit_Box>().deleteItSelf();
                }
            }

        }
        else if((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            if(Vector3.Distance(current.transform.position,start) < 0.1)
            {
                Destroy(current);
            }
        }
    }
}
