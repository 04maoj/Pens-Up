using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{
    private Plane objPlane;
    private bool eneded = false;
    Ray Generate_Ray(Vector3 touchPosit) {
        Vector3 mousePosFar = new Vector3(touchPosit.x, touchPosit.y, Camera.main.farClipPlane);
        Vector3 mousePosNear = new Vector3(touchPosit.x, touchPosit.y, Camera.main.nearClipPlane);
        Vector3 mousePosF = Camera.main.ScreenToWorldPoint(mousePosFar);
        Vector3 mousePosN = Camera.main.ScreenToWorldPoint(mousePosNear);
        return new Ray(mousePosN, mousePosF - mousePosN);
    }

    private void Start()
    {
        Track_manager maanger = FindObjectOfType<Track_manager>();
        objPlane = new Plane(Camera.main.transform.forward * -1, transform.position);
        eneded = false;
    }
    void Update()
    {   
        if(!eneded)
        {
            if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) || Input.GetMouseButton(0))
            {
                Ray myRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                float rayDistance;
                if (objPlane.Raycast(myRay, out rayDistance))
                    transform.position = myRay.GetPoint(rayDistance);
                RaycastHit test_hit;
                Ray hit_ray = Generate_Ray(Input.mousePosition);
                if (Physics.Raycast(hit_ray.origin, hit_ray.direction, out test_hit))
                {
                    if (test_hit.collider != null)
                    {
                        if (test_hit.collider.GetComponent<Hit_Box>() != null)
                            test_hit.collider.GetComponent<Hit_Box>().deleteItSelf();
                    }
                }
            }
            else if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
            {
                eneded = true;
            }
        }

    }
}