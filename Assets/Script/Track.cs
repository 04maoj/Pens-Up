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
    private List<int> candidate;
    private List<int> m_hitBoxDelete;
    private float init_x;
    private float init_y;
    /// <summary>
    /// Haset of the hit box index to be deleted
    /// </summary>
    public HashSet<int> m_CheckOccurence;
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
        m_CheckOccurence = new HashSet<int>();
        objPlane = new Plane(Camera.main.transform.forward * -1, transform.position);
        eneded = false;
        corrdinates = new List<Tuple<float, float>>();
        pressure = new List<float>();
        candidate = new List<int>();
        m_hitBoxDelete = new List<int>();
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
                        corrdinates.Add(new Tuple<float, float>(Input.mousePosition.x, Input.mousePosition.y));
                        pressure.Add(Input.GetTouch(0).pressure);
                    }

                }
                Ray myRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                float rayDistance;
                if (objPlane.Raycast(myRay, out rayDistance))
                    transform.position = myRay.GetPoint(rayDistance);
                RaycastHit test_hit;
                Ray hit_ray = Generate_Ray(Input.mousePosition);
                //Send a RayCast that sense whether it hits the background gameObject
                if (Physics.Raycast(hit_ray.origin, hit_ray.direction, out test_hit))
                {
                    if (test_hit.collider != null)
                    {
                        if (test_hit.collider.GetComponent<Hit_Box>() != null)
                        {
                            //If it hits the hitbox retreive the information of the hit box.
                            Tuple<Alphabate_manager, int> val = test_hit.collider.GetComponent<Hit_Box>().F_GetsInfo();
                            int hitBoxId = val.Item2;
                            HashSet<int> possibleCandidate = val.Item1.F_GetsPossibleStrokWithHitBoxId(hitBoxId);

                            //Try to determine to current stroke. By finding the intersections. 
                            if (stroke_number == -1 && possibleCandidate.Count >= 1)
                            {
                                //If there is no candidate, add all in. 
                                if (candidate.Count == 0)
                                {
                                    foreach (int temp in possibleCandidate)
                                    {
                                        candidate.Add(temp);
                                    }
                                }
                                // Peforming Intersection
                                else
                                {
                                    for (int q = 0; q < candidate.Count; q++)
                                    {
                                        if (possibleCandidate.Contains(candidate[q]))
                                        {
                                            candidate.Remove(candidate[q]);
                                        }
                                    }
                                }
                                //if after Intersection not possible solution, then the current one must wrote on two strokes. 
                                if (candidate.Count == 0)
                                {
                                    manger.Not_Same();
                                    Destroy(gameObject);
                                    m_CheckOccurence.Clear();
                                }
                                alphbate = val.Item1;
                                if (!m_CheckOccurence.Contains(val.Item2))
                                {
                                    m_CheckOccurence.Add(val.Item2);
                                    m_hitBoxDelete.Add(val.Item2);
                                }
                            }
                            else
                            {
                                if (possibleCandidate.Contains(stroke_number))
                                {
                                    manger.Not_Same();
                                    Destroy(gameObject);
                                    m_CheckOccurence.Clear();
                                }
                                else
                                {
                                    if(!m_CheckOccurence.Contains(val.Item2))
                                    {
                                        m_CheckOccurence.Add(val.Item2);
                                        m_hitBoxDelete.Add(val.Item2);
                                    }
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
                manger.Insert_Strok(corrdinates, alphbate, m_hitBoxDelete, stroke_number);
                manger.InsertAll(corrdinates);
                manger.InsertPressure(pressure);
            }
        }

    }
}