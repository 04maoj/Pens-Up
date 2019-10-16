using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Track : MonoBehaviour
{
    private Plane objPlane;
    private bool eneded = false;
    public int stroke_number;
    private Alphabate_manager alphbate;
    private Track_manager manger;
    private List<Tuple<float, float>> corrdinates;
    private List<float> pressure;
    private float init_x;
    private float init_y;
    public HashSet<int> to_be_delete;
    Ray Generate_Ray(Vector3 touchPosit)
    {
        Vector3 mousePosFar = new Vector3(touchPosit.x, touchPosit.y, Camera.main.farClipPlane);
        Vector3 mousePosNear = new Vector3(touchPosit.x, touchPosit.y, Camera.main.nearClipPlane);
        Vector3 mousePosF = Camera.main.ScreenToWorldPoint(mousePosFar);
        Vector3 mousePosN = Camera.main.ScreenToWorldPoint(mousePosNear);
        return new Ray(mousePosN, mousePosF - mousePosN);
    }

    private void Start()
    {
        manger = FindObjectOfType<Track_manager>();
        to_be_delete = new HashSet<int>();
        objPlane = new Plane(Camera.main.transform.forward * -1, transform.position);
        eneded = false;
        corrdinates = new List<Tuple<float, float>>();
        pressure = new List<float>();
        stroke_number = -1;
    }
    public int Get_Stroke_Number()
    {
        return stroke_number;
    }
    public Alphabate_manager Get_Current_Alphabate()
    {
        return alphbate;
    }
    void Update()
    {
        if (!eneded)
        {
            if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) || Input.GetMouseButton(0))
            {
                //lets only include the corrdinates if it is 1 unit > than the last stored one.
                if (corrdinates.Count == 0)
                {
                    init_x = Input.mousePosition.x;
                    init_y = Input.mousePosition.y;
                    corrdinates.Add(new Tuple<float, float>(Input.mousePosition.x, Input.mousePosition.y));
                    pressure.Add(Input.GetTouch(0).pressure);
                }
                else
                {
                    float diff_x = Input.mousePosition.x - corrdinates[corrdinates.Count - 1].Item1;
                    diff_x *= diff_x;
                    float diff_y = Input.mousePosition.y - corrdinates[corrdinates.Count - 1].Item2;
                    diff_y *= diff_y;
                    if (Math.Sqrt(diff_x + diff_y) >= 1)
                    {
                        corrdinates.Add(new Tuple<float, float>(Input.mousePosition.x - init_x, Input.mousePosition.y - init_y));
                        pressure.Add(Input.GetTouch(0).pressure);
                    }

                }
                //manger.InsertAll(corrdinates);

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
                        {
                            Tuple<List<int>, Tuple<Alphabate_manager, int>> val = test_hit.collider.GetComponent<Hit_Box>().deleteItSelf();
                            if (stroke_number == -1 && val.Item1.Count > 1)
                            {
                                alphbate = val.Item2.Item1;
                                to_be_delete.Add(val.Item2.Item2);
                            }
                            else if (stroke_number == -1)
                            {
                                stroke_number = val.Item1[0];
                                alphbate = val.Item2.Item1;
                                to_be_delete.Add(val.Item2.Item2);
                            }
                            else
                            {
                                bool possible = false;
                                for (int i = 0; i < val.Item1.Count; i++)
                                {
                                    if (stroke_number == val.Item1[i])
                                        possible = true;
                                }
                                if (!possible)
                                {
                                    manger.Not_Same();
                                    Destroy(gameObject);
                                    to_be_delete.Clear();
                                }
                                else
                                {
                                    to_be_delete.Add(val.Item2.Item2);
                                }
                            }

                        }
                        else if (test_hit.collider.tag == "Boarders")
                        {
                            manger.HitBoarders();
                        }

                    }
                }
            }
            else if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
            {
                eneded = true;
                manger.Insert_Strok(corrdinates, alphbate, to_be_delete, stroke_number);
                manger.InsertAll(corrdinates);
                manger.InsertPressure(pressure);
            }
        }

    }
}