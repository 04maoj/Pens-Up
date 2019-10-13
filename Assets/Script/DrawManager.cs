using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class DrawManager : MonoBehaviour
{
    // [SerializeField] bool replayMode = false;
    [SerializeField] string character;
    private float seperateValue = 400f;
    private GameObject clone;
    private LineRenderer lineRe;
    public GameObject target;
    private int counter = 0;
    private int index;
    private Vector3 currentPosition;
    // private Track_manager trackmg;
    private List<List<Tuple<float, float>>> list;
    //private User_Info user = FindObjectOfType<User_Info>();
    string userName;

    void Awake()
    {
        userName = FindObjectOfType<User_Info>().Get_UserName();
    }


    public List<List<Tuple<float, float>>> GetStrokes(String letter)
    {
        string path = "Assets/Local_DataBase/Students/" + userName + "/" + letter + "_all";
        Debug.Log("Replay path: " + path);
        string line = "";
        string[] temp = new string[2];
        Tuple<float, float> coordinate = null;
        List<List<Tuple<float, float>>> paints = new List<List<Tuple<float, float>>>();
        List<Tuple<float, float>> paint = new List<Tuple<float, float>>();
        StreamReader sr = new StreamReader(path);
        line = sr.ReadLine();
        //float preX = -1f;
        //float preY = -1f;
        float initX = float.Parse(line.Split(' ')[0]);
        float initY = float.Parse(line.Split(' ')[1]);
        float preX = float.Parse(line.Split(' ')[0]);
        float preY = float.Parse(line.Split(' ')[1]);
        float currentX = float.Parse(line.Split(' ')[0]);
        float currentY = float.Parse(line.Split(' ')[1]);
        paint.Add(new Tuple<float, float>(initX, initY));
        line = sr.ReadLine();
        while (line != null)
        {

            temp = line.Split(' ');
            //currentX = float.Parse(temp[0]) + initX;
            //currentY = float.Parse(temp[1]) + initY;
            //Debug.Log("RX: " + float.Parse(temp[0]) + ",RY: " + float.Parse(temp[1]) + ". CX: " + currentX + ",CY: " + currentY);

            // New stroke
            if (Math.Abs(float.Parse(temp[0]) + initX - preX) > seperateValue || Math.Abs(float.Parse(temp[1]) + initY - preY) > seperateValue)
            {
                paints.Add(paint);
                paint = new List<Tuple<float, float>>();
                initX = float.Parse(temp[0]);
                initY = float.Parse(temp[1]);
                coordinate = new Tuple<float, float>(initX, initY);
            }
            else
            {
                currentX = float.Parse(temp[0]) + initX;
                currentY = float.Parse(temp[1]) + initY;
                coordinate = new Tuple<float, float>(currentX, currentY);
            }
            //coordinate = new Tuple<float, float>(currentX + initX, currentY + initY);
            //Debug.Log("From: " + coordinate.Item1 + ", " + coordinate.Item2);
            paint.Add(coordinate);
            preX = currentX;
            preY = currentY;
            line = sr.ReadLine();
        }
        paints.Add(paint);
        sr.Close();
        // Debug.Log("Stroke #: " + paints.Count);
        return paints;
    }


    public void setCharacter(String chara)
    {
        this.character = chara;
    }

    public string getCharacter()
    {
        return this.character;
    }
}
