using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class Replay : MonoBehaviour
{
    private DrawManager drawManager;
    [SerializeField] string character;
    private GameObject clone;
    private LineRenderer lineRe;
    public GameObject target;
    // private Track_manager trackmg;
    List<List<Tuple<float, float>>> coordinates;
    List<Vector3> stroke;
    Vector3 currentPosition;
    int index = -1;
    // X-axis offset of the character
    public float offSetX;
    // Y-axis offset of the character
    public float offSetY;
    // Z-axis offset of the drawing space
    public float offSetZ = -1f;
    public float scale = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        Button btn = this.GetComponent<Button>();
        drawManager = FindObjectOfType<DrawManager>();
        btn.onClick.AddListener(OnClick);
        
    }

    // Update is called once per frame
    void OnClick()
    {
        // Debug.Log("'Replay' Pressed.");
        // drawManager.setCharacter(character);
        Debug.Log("Replay char: " + character);
        drawManager.setCharacter(character);
        coordinates = drawManager.GetStrokes(character);
        // Debug.Log("Got Strokes");
        // drawManager.DrawBot(coordinates);
        StartCoroutine(WaitAndPaint());
        // DrawBot(coordinates);
        Debug.Log("Finish Replay");
        
    }


    IEnumerator WaitAndPaint(){
    
    // IEnumerator WaitAndDraw(LineRenderer lr, int i, Vector3 points){
        // Debug.Log(Time.time);
        
        // Debug.Log("WAP: Index: " + index + " Pos: " + currentPosition);
        // Debug.Log(Time.time);
        // lineRe.SetPosition(index, currentPosition);
        // lr.SetPosition(i, points);

        int count = 0;
        foreach (List<Tuple<float, float>> eachStroke in coordinates){
            clone = (GameObject)Instantiate(target,target.transform.position,Quaternion.identity);
            lineRe = clone.GetComponent<LineRenderer>();
            lineRe.startColor = Color.red;
            lineRe.endColor = Color.blue;
            lineRe.startWidth = 2f;
            lineRe.endWidth = 2f;
            count = eachStroke.Count;
            // vectorList = new List<Vector3>();
            // List<Vector3> stroke = new List<Vector3>();
            stroke = new List<Vector3>();
            foreach (Tuple<float, float> point in eachStroke){
                Vector3 pointCoordinate = new Vector3(point.Item1, point.Item2, clone.transform.position.z);
                //Vector3 pointCoordinate = new Vector3((point.Item1 + offSetX) / scale, (point.Item2 + offSetY) / scale, offSetZ);
                Debug.Log("XYZ: " + pointCoordinate);
                stroke.Add(pointCoordinate);
            }
            lineRe.positionCount = count;

            Debug.Log("lineRe_Count: " + lineRe.positionCount);
            currentPosition = new Vector3();

            for (int i = 0; i < lineRe.positionCount; i++){
                index = i;
                currentPosition = stroke[i];
                Debug.Log("Index: " + index + " Pos: " + currentPosition);
                yield return new WaitForSeconds(0.1f);
                lineRe.SetPosition(i, currentPosition);
                
                // StartCoroutine(WaitAndDraw(lineRe, i, stroke[i]));
                
            }
        }
    }
}
