using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
public class Track_manager : MonoBehaviour
{
    [SerializeField] bool record_mode = false;
    [SerializeField] string stroke_to_record;
    public GameObject prefabs ;
    public GameObject current;
    Vector3 start;
    Plane objPlane;
    List<List<Tuple<float, float>>> strokes;
    Stack<int> order;
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
            if(Vector3.Distance(current.transform.position,start) < 0.1)
            {
                Destroy(current);
            }
        }
    }
    public void Insert_Strok(List<Tuple<float, float>> input)
    {
        strokes.Add(input);
        if(record_mode) {
            string path = "Assets/Standards/" + stroke_to_record;
            using (StreamWriter sw = File.AppendText(path))
            {
                for(int i = 0; i < input.Count; i ++) {
                    sw.WriteLine(input[i].Item1+ " " + input[i].Item2);
                }
                Debug.Log("Done");
            }
        }
        else
        {
            Assessment tester = GetComponent<Assessment>();
            tester.Load_Standard(stroke_to_record);
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

    public void Replay(){
        string path = "Assets/Standards/" + stroke_to_record;
        string line = "";
        string[] temp = new string[2];
        float[] coordinate = new float[2];
        StreamReader sr = new StreamReader(path);
        line = sr.ReadLine();
        while (line != null){
            temp = line.Split(' ');
            for (int i = 0; i < temp.Length; i++){
                coordinate[i] = float.Parse(temp[i]);
            }
        }
    }
}
