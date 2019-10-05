using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;
public class Assessment : MonoBehaviour
{
    private List<Tuple<float, float>> current_standard;
    //Return a list for a sample stroke
    private float infinity = 10000000000;
    private float total = 0;
    private float count = 0;
    // hit board 就是如果你碰到了边界
    private bool hit_board = false;
    //这个是如果你横变成横竖了
    private int incorrect_stroke_sequence = 0;
    //这个是上往下变成下往上了
    private int incorrect_order = 0;
    //这个是没有连好线
    private int incorrect_connections = 0;
    //还有一个是deviation
    public void Load_Standard(string input_name) {
        current_standard = new List<Tuple<float, float>>();
        string path = "Assets/Standards/" + input_name;
        StreamReader myReader = new StreamReader(path);
        string line = myReader.ReadLine();
        while (line != null) {
            string[] temp = line.Split(' ');
            if(temp.Length < 2)
            {
                Debug.Log("error when reading file");
                return;
            }
            current_standard.Add(new Tuple<float, float>(float.Parse(temp[0]), float.Parse(temp[1])));
            line = myReader.ReadLine();
        }
        return;
    }
    //DTW
    //Return best matched distance 
    public void compare_Deviation(List<Tuple<float, float>> to_compare) {
        int n = current_standard.Count;
        int m = to_compare.Count;
        float[,] opt = new float[n+1,m+1];
        for(int i = 0; i <= n; i ++)
        {
            for(int j = 0 ; j <= m; j ++)
            {
                opt[i,j] = infinity;
            }
        }
        opt[0,0] = 0;
        for(int i =  1; i <= n; i ++)
        {
            for(int j = 1; j <=m; j ++)
            {
                float dy = current_standard[i - 1].Item1 - to_compare[j - 1].Item1;
                float dx = current_standard[i - 1].Item2 - to_compare[j - 1].Item2;
                float cost = (float)Math.Sqrt(dx * dx + dy * dy);
                opt[i,j] = Math.Min(opt[i - 1, j], opt[i, j - 1]);
                opt[i, j] = cost + Math.Min(opt[i , j], opt[i-1,j-1]);
            }
        }
        total +=opt[n, m];
        count++;
    }
    public int GetTotalScore()
    {
        return Average_deviation() + Average_hit_board() + Average_Incorect_stroke() + Average_Sequence()+ AverageConnections();
    }

    public int Average_deviation() 
    {
        float average = total / count;
        if(average > 7000)
        {
            return 1;
        } else if(average > 4000)
        {
            return 2;
        }
        return 3;
    }
    public int Average_hit_board()
    {
        if (hit_board)
            return 0;
        return 1;
    }
    public int Average_Incorect_stroke()
    {
        if(incorrect_order > 2)
        {
            return 0;
        }
        if(incorrect_order == 1)
             return 1;
        return 2;
    }
    public int Average_Sequence()
    {
        if (incorrect_stroke_sequence > 2)
        {
            return 0;
        }
        if (incorrect_stroke_sequence == 1)
            return 1;
        return 2;
    }
    public void Add_Board()
    {
        hit_board = true;
    }
    public void Add_Incorrect_Stroke()
    {
        incorrect_stroke_sequence++;
    }
    public void IncorrectOrder()
    {
        incorrect_order++;
    }
    public void ConnectionIssues()
    {
        incorrect_connections++;
    }
    public int AverageConnections()
    {
        if (incorrect_connections > 2)
        {
            return 0;
        }
        if (incorrect_connections == 1)
            return 1;
        return 2;
    }
}
