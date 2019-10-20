using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class DrawManager_T : MonoBehaviour
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
    string currentLetter;

    void Awake()
    {
        //userName = FindObjectOfType<User_Info>().Get_UserName();
        userName = staticname.i_name;
        currentLetter = staticname.i_letter;
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
        float initX = 0, initY = 0, cx, cy, tempx, tempy;
        line = sr.ReadLine();
        while ((line = sr.ReadLine()) != null)
        {   
            temp = line.Split(' ');
            tempx = float.Parse(temp[0]);
            tempy = float.Parse(temp[1]);
            if(tempx >= 700 && tempy >= 700) {
                paints.Add(paint);
                paint = new List<Tuple<float, float>>();
                coordinate = new Tuple<float, float>(tempx, tempy);
                paint.Add(coordinate);
                initX = tempx;
                initY = tempy;
            } else {
                cx = tempx + initX;
                cy = tempy + initY;
                coordinate = new Tuple<float, float>(cx, cy);
                paint.Add(coordinate);
            }
        }
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
