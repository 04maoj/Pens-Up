﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Lettercolor : MonoBehaviour
{
    // Start is called before the first frame update
    private HashSet<string> complete_set;
    void Start()
    {
      //find the specific button


    string path = "Assets/Local_DataBase/Students/" + "Minz" + "/Total_score_list.txt";
    complete_set = new HashSet<string>();
		if (File.Exists(path)) {
      string[] lines = File.ReadAllLines(path);
			int i=1;
			foreach (string line in lines) {
          complete_set.Add(line);
    			// Debug.Log(line);
          string[] arr1 = line.Split(' ');
    			Debug.Log(arr1[1]);
    			int score = int.Parse(arr1[1]);
          Debug.Log(i.ToString());
          GameObject but = GameObject.Find(i.ToString());
          Button letter = but.GetComponent<Button>();
          if (score <=4) {
            letter.GetComponent<Image>().color = Color.red;
          }
          else if (score <=7) {
            letter.GetComponent<Image>().color = Color.yellow;
          }
          else {
            letter.GetComponent<Image>().color = Color.green;
          }
          i++;
          // Debug.Log(i);
      }

    }
  	}

    // Update is called once per frame
    void Update()
    {

    }
}
