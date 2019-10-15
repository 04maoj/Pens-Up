using System.Collections;
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


    string path = "Assets/Local_DataBase/Students/" + staticname.i_name + "/Total_score_list";
    Debug.Log(path + staticname.i_name);
    complete_set = new HashSet<string>();
		if (File.Exists(path)) {
      string[] lines = File.ReadAllLines(path);
			int i=1;
			foreach (string line in lines) {
          complete_set.Add(line);
    			Debug.Log(line);
          string[] arr1 = line.Split(' ');
    			Debug.Log(arr1[1]);
    			float score = float.Parse(arr1[1]);
          Debug.Log(score);
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
          Debug.Log(i+ "haha " + line );
      }

    }
  	}

    // Update is called once per frame
    void Update()
    {

    }
}
