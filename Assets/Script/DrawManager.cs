using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class DrawManager : MonoBehaviour
{
    [SerializeField] bool replayMode = false;
    [SerializeField] string character;
    private float seperateValue = 20f;
    private GameObject clone;
    private LineRenderer lineRe;
    public GameObject target;
    // private Track_manager trackmg;
    List<List<Tuple<float, float>>> coordinates;
    
    private void Start(){
        // if(replayMode){
        //     trackmg = FindObjectOfType<Track_manager>();
        //     List<List<Tuple<float, float>>> coordinates = trackmg.GetStrokes();
        //     DrawBot(coordinates);
        // }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(replayMode){
            Debug.Log("RESET");
            replayMode = false;
            // trackmg = FindObjectOfType<Track_manager>();
            Debug.Log("TRACK_MANAGER");
            coordinates = GetStrokes(character);
            Debug.Log("LIST");
            DrawBot(coordinates);
            Debug.Log("DRAW");
            
        }
    }

    public void DrawBot(List<List<Tuple<float, float>>> coordinates){
        // List<Vector3> coordinate3D = new List<Vector3>();
        // Vector3 point = new Vector3(0, 0, 2f);
        // for (int i = 0; i < coordinates.Count; i++){
        //     Vector3 point = new Vector3(coordinates[i].Item1, coordinates[i].Item2, -1);
        //     coordinate3D.Add(point);
        // }

        clone = (GameObject)Instantiate(target, target.transform.position, Quaternion.identity);
        lineRe = clone.GetComponent<LineRenderer>();
        lineRe.startWidth = 1f;
        lineRe.endWidth = 1f;
        
        // for(int i = 0; i < coordinates.Count; i++){
        //     List<Vector3> stroke = new List<Vector3>();
        //     for(int j = 0; j < )
        // }
        foreach (List<Tuple<float, float>> eachStroke in coordinates){
            List<Vector3> stroke = new List<Vector3>();
            foreach (Tuple<float, float> point in eachStroke){
                Vector3 pointCoordinate = new Vector3(point.Item1, point.Item2, -2);
                stroke.Add(pointCoordinate);
            }
            lineRe.positionCount = stroke.Count;
            for(int i = 0; i < lineRe.positionCount; i++){
                lineRe.SetPosition(i, stroke[i]);
            }
        }
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
        float currentX = 0f;
        float currentY = 0f;
        while (line != null){
            temp = line.Split(' ');
            currentX = float.Parse(temp[0]);
            currentY = float.Parse(temp[1]);
            // Check if new stroke
            if (Math.Abs(currentX - preX) > seperateValue || Math.Abs(currentY - preY) > seperateValue){
                paints.Add(paint);
                paint = new List<Tuple<float, float>>();
            }
            coordinate = new Tuple<float, float>(currentX, currentY);
            paint.Add(coordinate);
            preX = currentX;
            preY = currentY;
        }
        paints.Add(paint);
        sr.Close();
        return paints;
    }
}
