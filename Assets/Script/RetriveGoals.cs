using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class RetriveGoals : MonoBehaviour
{

    public Text goal1;
    public Text goal2;
    public Text goal3;
    public Text goal4;

    // Start is called before the first frame update
    void Start()
    {   
        string user_name = "Handsome";
        string path = "Assets/Local_DataBase/Students/" + user_name + "goals.txt";
        string basic = "BASIC";
        string inter = "INTER";
        string expert = "EXPERT";
        string insane = "INSANE";

        FileStream inFile = new FileStream(path, FileMode.Open, FileAccess.Read);
        StreamReader reader = new StreamReader(inFile);

        try
        {
            string matching_string = reader.ReadLine();
            while (matching_string != null)
            {
                if (matching_string.Contains(basic)) 
                { 
                    goal1.text = "1. Attempt all of the courses.";
                    goal2.text = "2. Attempt all of the courses at least ONCE.";
                    goal3.text = "3. Log in for 7 days in a month.";
                    goal4.text = "4. Stay online for 20 minutes per day for each day to count.";
                }
                else if (matching_string.Contains(inter))
                {
                    goal1.text = "1. Score 50% of all the courses";
                    goal2.text = "2. Attempt all of the courses at least 3 Times.";
                    goal3.text = "3. Log in for 20 days in a month.";
                    goal4.text = "4. Stay online for 45 minutes per day for each day to count.";
                }
                else if (matching_string.Contains(expert))
                {
                    goal1.text = "1. Score MAX Score in all of the courses.";
                    goal2.text = "2. Attempt all of the courses at least 5 Times.";
                    goal3.text = "3. Log in for 30 consecutive days.";
                    goal4.text = "4. Stay online for 45 minutes per day for each day to count.";
                }
                else
                {
                    goal1.text = "1. 10000 HOURS!";
                    goal2.text = "2. Respect!";
                    goal3.text = "3. Good Luck!!";
                    goal4.text = "4. GOOD LUCK!!!";
                }
            }
        }
        catch (System.Exception)
        {
        
            throw;
        }
        finally
        {
            reader.Close();
            inFile.Close();
        }
        
    }
}
