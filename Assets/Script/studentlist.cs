using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class studentlist : MonoBehaviour
{
    // Start is called before the first frame update
	public GameObject spawner;
	public GameObject Content;
	public string u_name;
	private HashSet<string> complete_set;
    void Start()
    {
			u_name = FindObjectOfType<User_Info>().user_name;
			string path1 = "Assets/Local_DataBase/Teachers/" + u_name + "/Student_List.txt";
			Debug.Log(u_name+" " + path1);
			complete_set = new HashSet<string>();
			if (File.Exists(path1))
        {
           // Read a text file line by line.
           Debug.Log("Yes It exist");
           string[] lines = File.ReadAllLines(path1);
					 int i=0;
			foreach (string line in lines)
			    {
			        complete_set.Add(line);
			        Debug.Log(line);
							var current_spawned = Instantiate(spawner, transform.position, Quaternion.identity);
			        current_spawned.gameObject.transform.SetParent(Content.transform);
			        current_spawned.transform.localScale = new Vector3(1, 1, 1);
			        current_spawned.transform.GetChild(0).GetComponent<Text>().text = line;
							current_spawned.transform.position = new Vector3(1750,1400-i*350,1000);
							i=i+1;
						}
					}

	// 	for (int i=0; i<3; i++) {
  //       var current_spawned = Instantiate(spawner, transform.position, Quaternion.identity);
  //       current_spawned.gameObject.transform.SetParent(Content.transform);
  //       current_spawned.transform.localScale = new Vector3(1, 1, 1);
  //       current_spawned.transform.GetChild(0).GetComponent<Text>().text = "Students" + (i+1);
	// 			current_spawned.transform.position = new Vector3(540+i*800,1400,1000);
	// }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
