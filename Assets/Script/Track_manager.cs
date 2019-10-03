using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
public class Track_manager : MonoBehaviour
{
    [SerializeField] bool record_mode = false;
    [SerializeField] GameObject Error_Incorrect_Stroke_Order = null;
    [SerializeField] GameObject Error_Not_Same_Stroke = null;
    [SerializeField] GameObject Error_Write_On_Character = null;
    [SerializeField] GameObject Error_Sequence = null;
    [SerializeField] GameObject Error_Finished= null;
    // public bool replayMode = false;
    [SerializeField] string stroke_to_record= null;
    public GameObject prefabs;
    public GameObject current;
    private bool hit_board;
    Vector3 start;
    Plane objPlane;
    List<List<Tuple<float, float>>> strokes;
    Stack<int> order;
    float total_diviation = 0;
    int total_deviation = 0;
    private Assessment tester;
    private void Start() {
        Set_Error_Inactive();
        objPlane = new Plane(Camera.main.transform.forward * -1, this.transform.position);
        strokes = new List<List<Tuple<float, float>>>();
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
            if(current != null) {

                if (Vector3.Distance(current.transform.position, start) < 0.1)
                {
                    Destroy(current);
                }
            }
        }
    }
    public void Insert_Strok(List<Tuple<float, float>> input, Alphabate_manager alphabate, HashSet<int> to_be_delete,int stroke_number)
    {
        if(record_mode) {
            string path = "Assets/Standards/" + stroke_to_record;
            StreamWriter sw = File.AppendText(path);

            float start_y = input[0].Item1;
            float start_x = input[0].Item2;
            for (int i = 0; i < input.Count; i ++) {
                sw.WriteLine((input[i].Item1 - start_y )+ " " + (input[i].Item2- start_x));
            }
            sw.Close();
            Debug.Log("Done");
        }
        else
        {
            if(alphabate == null)
            {
                Destroy(current);
            }
            tester = alphabate.Get_Assessment();
            if (current.GetComponent<Track>().Get_Stroke_Number() == -1) {
                Set_Error_Inactive();
                Error_Write_On_Character.SetActive(true);
                Destroy(current);
                return;
            }
            bool success = alphabate.remove_Hit(to_be_delete, stroke_number);
            if (!success)
            {
                Set_Error_Inactive();
                Error_Sequence.SetActive(true);
                tester.IncorrectOrder();
                Destroy(current);
                return;
            }
            if (!alphabate.Increment_Stroke(current.GetComponent<Track>().Get_Stroke_Number()))
            {
                Set_Error_Inactive();
                Error_Incorrect_Stroke_Order.SetActive(true);
                tester.Add_Incorrect_Stroke();
                Destroy(current);
                return;
            }
            if (alphabate.finish())
                Finished_one();
            tester.Load_Standard(current.GetComponent<Track>().Get_Current_Alphabate().Get_Strok_Name() + current.GetComponent<Track>().Get_Stroke_Number());
            tester.compare_Deviation(input);
            if (hit_board)
                tester.Add_Board();
            hit_board = false;
        }
    }
    public void Set_Error_Inactive()
    {
        Error_Incorrect_Stroke_Order.SetActive(false);
        Error_Not_Same_Stroke.SetActive(false);
        Error_Write_On_Character.SetActive(false);
        Error_Finished.SetActive(false);
        Error_Sequence.SetActive(false);
    }
    public void Clear_All()
    {
        RemoveStrokes();
        total_diviation = 0;
    }
    public List<List<Tuple<float, float>>> GiveStrokes()
    {
        return strokes;
    }
    public void RemoveStrokes()
    {
        strokes.Clear();
    }
    public void Not_Same()
    {
        Set_Error_Inactive();
        Error_Not_Same_Stroke.SetActive(true);
    }
    public void HitBoarders()
    {
        hit_board = true;
    }
    public void Finished_one() {
        Set_Error_Inactive();
        Error_Finished.SetActive(true);
    }

    // public List<List<Tuple<float, float>>> GetStrokes(){
    //     string path = "Assets/Standards/" + stroke_to_record;
    //     string line = "";
    //     string[] temp = new string[2];
    //     Tuple<float, float> coordinate = null;
    //     List<List<Tuple<float, float>>> paints = new List<List<Tuple<float, float>>>();
    //     List<Tuple<float, float>> paint = new List<Tuple<float, float>>();
    //     StreamReader sr = new StreamReader(path);
    //     line = sr.ReadLine();
    //     float preX = -1f;
    //     float preY = -1f;
    //     float currentX = 0f;
    //     float currentY = 0f;
    //     while (line != null){
    //         temp = line.Split(' ');
    //         currentX = float.Parse(temp[0]);
    //         currentY = float.Parse(temp[1]);
    //         // New stroke
    //         if (Math.Abs(currentX - preX) > seperateValue || Math.Abs(currentY - preY) > seperateValue){
    //             paints.Add(paint);
    //             paint = new List<Tuple<float, float>>();
    //         }
    //         coordinate = new Tuple<float, float>(currentX, currentY);
    //         paint.Add(coordinate);
    //         preX = currentX;
    //         preY = currentY;
    //     }
    //     paints.Add(paint);
    //     sr.Close();
    //     return paints;
    // }
}
