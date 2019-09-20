using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
public class Track_manager : MonoBehaviour
{
    [SerializeField] bool record_mode = false;
    // public bool replayMode = false;
    [SerializeField] string stroke_to_record;
    public GameObject prefabs ;
    public GameObject current;
    Vector3 start;
    Plane objPlane;
    List<List<Tuple<float, float>>> strokes;
    Stack<int> order;
    private float seperateValue = 20f;
    float total_diviation = 0;
    int total_deviation = 0;
   
     private void Start() {
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
    public void Insert_Strok(List<Tuple<float, float>> input)
    {
        strokes.Add(input);
        if(record_mode) {
            string path = "Assets/Standards/" + stroke_to_record;
            // if(File.Exists(path)){
            //     File.Delete(path);
            // }
            StreamWriter sw = File.AppendText(path);
            for (int i = 0; i < input.Count; i ++) {
                sw.WriteLine(input[i].Item1+ " " + input[i].Item2);
            }
            // Seperate strokes.
            // sw.WriteLine("0 0");
            sw.Close();
            Debug.Log("Done");
            // String[] lines = new String[input.Count];
            // for(int i = 0; i < input.Count; i++){
            //     lines[i] = input[i].Item1+ " " + input[i].Item2;
            // }
            // File.WriteAllLines(path, lines);
            // Debug.Log("Stroke Recorded.");
        }
        else
        {
            Assessment tester = GetComponent<Assessment>();
            if(current.GetComponent<Track>().Get_Stroke_Number() == -1)
            {
                Debug.Log("Negative Marks");
                return;
            }
            Debug.Log(current.GetComponent<Track>().Get_Current_Alphabate() + current.GetComponent<Track>().Get_Stroke_Number());
            tester.Load_Standard(current.GetComponent<Track>().Get_Current_Alphabate() + current.GetComponent<Track>().Get_Stroke_Number());
            total_diviation += tester.compare_Deviation(input);
            Debug.Log(total_diviation);
        }
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
