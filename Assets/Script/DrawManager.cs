using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class DrawManager : MonoBehaviour
{
    // [SerializeField] bool replayMode = false;
    [SerializeField] string character;
    private float seperateValue = 10f;
    private GameObject clone;
    private LineRenderer lineRe;
    public GameObject target;
    private int counter = 0;
    private int index;
    Vector3 currentPosition;
    // private Track_manager trackmg;
    List<List<Tuple<float, float>>> list;
    
    private void Start(){
        
        
    }

    public void DrawBot(List<List<Tuple<float, float>>> coordinates){
        // List<Vector3> coordinate3D = new List<Vector3>();
        // Vector3 point = new Vector3(0, 0, 2f);
        // for (int i = 0; i < coordinates.Count; i++){
        //     Vector3 point = new Vector3(coordinates[i].Item1, coordinates[i].Item2, -1);
        //     coordinate3D.Add(point);
        // }

        // clone = (GameObject)Instantiate(target, target.transform.position, Quaternion.identity);
        // lineRe = clone.GetComponent<LineRenderer>();
        // lineRe.startWidth = 1f;
        // lineRe.endWidth = 1f;
        
        // for(int i = 0; i < coordinates.Count; i++){
        //     List<Vector3> stroke = new List<Vector3>();
        //     for(int j = 0; j < )
        // }
        // foreach (List<Tuple<float, float>> eachStroke in coordinates){
        //     List<Vector3> stroke = new List<Vector3>();
        //     foreach (Tuple<float, float> point in eachStroke){
        //         Vector3 pointCoordinate = new Vector3(point.Item1, point.Item2, -2);
        //         stroke.Add(pointCoordinate);
        //     }
        //     lineRe.positionCount = stroke.Count;
        //     for(int i = 0; i < lineRe.positionCount; i++){
        //         lineRe.SetPosition(i, stroke[i]);
        //     }
        // }

        // int index = 0;
        int count = 0;
        foreach (List<Tuple<float, float>> eachStroke in coordinates){
            clone = (GameObject)Instantiate(target,target.transform.position,Quaternion.identity);
            lineRe = clone.GetComponent<LineRenderer>();
            lineRe.startColor = Color.red;
            lineRe.endColor = Color.blue;
            lineRe.startWidth = 20f;
            lineRe.endWidth = 20f;
            count = eachStroke.Count;
            // vectorList = new List<Vector3>();
            List<Vector3> stroke = new List<Vector3>();
            foreach (Tuple<float, float> point in eachStroke){
                Vector3 pointCoordinate = new Vector3(point.Item1, point.Item2, clone.transform.position.z);
                stroke.Add(pointCoordinate);
            }
            lineRe.positionCount = count;

            Debug.Log("lineRe_Count: " + lineRe.positionCount);
            currentPosition = new Vector3();

            for (int i = 0; i < lineRe.positionCount; i++){
                index = i;
                currentPosition = stroke[i];
                StartCoroutine(Wait());
                lineRe.SetPosition(i, currentPosition);
                
                // StartCoroutine(WaitAndDraw(lineRe, i, stroke[i]));
                
            }
        }
    }

    IEnumerator Wait(){
    
    // IEnumerator WaitAndDraw(LineRenderer lr, int i, Vector3 points){
        Debug.Log(Time.time);
        yield return new WaitForSeconds(0.5f);
        Debug.Log(Time.time);
        // lineRe.SetPosition(index, currentPosition);
        // lr.SetPosition(i, points);
    }

    public List<List<Tuple<float, float>>> GetStrokes(String letter){
        string path = "Assets/Standards/" + letter;
        string line = "";
        string[] temp = new string[2];
        Tuple<float, float> coordinate = null;
        List<List<Tuple<float, float>>> paints = new List<List<Tuple<float, float>>>();
        List<Tuple<float, float>> paint = new List<Tuple<float, float>>();
        StreamReader sr = new StreamReader(path);
        line = sr.ReadLine();
        float preX = -1f;
        float preY = -1f;
        preX = float.Parse(line.Split(' ')[0]);
        preY = float.Parse(line.Split(' ')[1]);
        float currentX = 0f;
        float currentY = 0f;
        while (line != null){
            // if (line.Equals("0 0")){
            //     line = sr.ReadLine();
            //     continue;
            // }
            temp = line.Split(' ');
            if (float.Parse(temp[0]) == 0 && float.Parse(temp[1]) == 0){
                line = sr.ReadLine();
                continue;
            }
            currentX = float.Parse(temp[0]);
            currentY = float.Parse(temp[1]);
            // Debug.Log("PreX: " + preX + " PreY: " + preY + " CX: " + currentX + " CY: " + currentY);
            // Check if new stroke
            // Debug.Log("DiffX: " + Math.Abs(currentX - preX) + " DiffY: " + Math.Abs(currentY - preY));
            
            
            if (Math.Abs(currentX - preX) > seperateValue || Math.Abs(currentY - preY) > seperateValue){
                if (counter != 0){
                    paints.Add(paint);
                    paint = new List<Tuple<float, float>>();
                }
                // line = sr.ReadLine();
                // continue;
            }
            coordinate = new Tuple<float, float>(currentX, currentY);
            paint.Add(coordinate);
            preX = currentX;
            preY = currentY;
            line = sr.ReadLine();
            counter++;
        }
        paints.Add(paint);
        sr.Close();
        // Debug.Log("Stroke #: " + paints.Count);
        return paints;
    }

    // public List<List<Tuple<float, float>>> GetStrokes(String letter){
    //     string path = "Assets/Standards/" + letter;
    //     string line = "";
    //     string[] temp = new string[2];
    //     Tuple<float, float> coordinate = null;
    //     List<List<Tuple<float, float>>> paints = new List<List<Tuple<float, float>>>();
    //     List<Tuple<float, float>> paint = null;
    //     StreamReader sr = new StreamReader(path);
    //     line = sr.ReadLine();
    //     float preX = -1f;
    //     float preY = -1f;
    //     preX = float.Parse(line.Split(' ')[0]);
    //     preY = float.Parse(line.Split(' ')[1]);
    //     float currentX = 0f;
    //     float currentY = 0f;
    //     while (line != null){
    //         temp = line.Split(' ');
    //         currentX = float.Parse(temp[0]);
    //         currentY = float.Parse(temp[1]);
    //         // Debug.Log("PreX: " + preX + " PreY: " + preY + " CX: " + currentX + " CY: " + currentY);
    //         // Check if new stroke
    //         // Debug.Log("DiffX: " + Math.Abs(currentX - preX) + " DiffY: " + Math.Abs(currentY - preY));
            
    //         if (currentX == 0 && currentY == 0){
    //         // if (Math.Abs(currentX - preX) > seperateValue || Math.Abs(currentY - preY) > seperateValue){
    //             if (counter != 0){
    //                 paints.Add(paint);
    //             }
    //             paint = new List<Tuple<float, float>>();
    //             line = sr.ReadLine();
    //             counter++;
    //             continue;
                
    //         }
    //         // paints.Add(paint);
    //         coordinate = new Tuple<float, float>(currentX, currentY);
    //         paint.Add(coordinate);
    //         preX = currentX;
    //         preY = currentY;
    //         counter++;
    //         line = sr.ReadLine();
    //     }
    //     paints.Add(paint);
    //     sr.Close();
    //     // Debug.Log("Stroke #: " + paints.Count);
    //     return paints;
    // }

    public void setCharacter(String chara){
        this.character = chara;
    }
}
