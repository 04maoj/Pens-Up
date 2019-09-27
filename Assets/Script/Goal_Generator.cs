using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal_Generation : MonoBehaviour
{
    protected IDictionary<int,string> goal_dict = new Dictionary<int, string> ()
    {
        {1, "loginDays"}
        {2, "praticeTimes"}
        {3, "blah"}
        {4, "blahblah"}
        {5, "blahblahblah"}
        {6, "blahblahblahblah"}
    };


    protected Generate_Goal(IDictionary<int, string> dict)
    {
        foreach (KeyValuePair<int, string> item in dict)
        {
            if (item.KEY == 1) Console.WriteLine("")
            else if (item.KEY == 2) Console.WriteLine("")
            else if (item.KEY == 3) Console.WriteLine("")
            else if (item.KEY == 4) Console.WriteLine("")
            else if (item.KEY == 4) Console.WriteLine("")
        }
    }
}
