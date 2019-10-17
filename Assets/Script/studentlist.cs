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
	private HashSet<string> complete_set2;
	public indiviual ad;
    void Start()
    {
			u_name = FindObjectOfType<User_Info>().user_name;
			string path1 = "Assets/Local_DataBase/Teachers/" + u_name + "/Student_List";
			Debug.Log(u_name+" " + path1);
			complete_set = new HashSet<string>();
			if (File.Exists(path1))
        {
           // Read a text file line by line.
           // Debug.Log("Yes It exist");
           string[] lines = File.ReadAllLines(path1);
					 int i=0;
			foreach (string line in lines)
			    {
			        complete_set.Add(line);
			        Debug.Log(line);
							//read the stutent's score text file
							var current_spawned = Instantiate(spawner, transform.position, Quaternion.identity);
			        current_spawned.gameObject.transform.SetParent(Content.transform);
			        current_spawned.transform.localScale = new Vector3(1, 1, 1);
			        current_spawned.transform.GetChild(0).GetComponent<Text>().text = line;
							ad = current_spawned.AddComponent<indiviual>();
							current_spawned.GetComponent<Button>().onClick.AddListener(delegate() {
                this.click();
            	});
							current_spawned.GetComponent<Button>().onClick.AddListener(delegate() {
								FindObjectOfType<SceneLoader>().LoadScence(20);
							});
							// current_spawned.transform.position = new Vector3(1750,1400-i*350,1000);
							current_spawned.GetComponent<Image>().color = Color.gray;
							i=i+1;

							string spath = "Assets/Local_DataBase/Students/" + line + "/Total_score_list";
							Debug.Log(line+" " + spath);
							complete_set2 = new HashSet<string>();
							float total =0;
							if (File.Exists(spath))
				        {
				        // Read a text file line by line.
					      // Debug.Log("Yes It exist in student");
					      string[] lines2 = File.ReadAllLines(spath);
								foreach (string line2 in lines2) {
								complete_set2.Add(line2);
					      // Debug.Log(line2+ " in " + line);

								//calculate the score adn set color
								string[] arr1 = line2.Split(' ');
							 	Debug.Log(arr1[1]);
								float score = float.Parse(arr1[1]);
								Debug.Log(score);
								total = total +score;
									}
								total = total/4;
								Debug.Log(total);
								if (total <=4) {
									current_spawned.GetComponent<Image>().color = Color.red;
								}
								else if (total <7) {
									current_spawned.GetComponent<Image>().color = Color.yellow;
								}
								else {
									current_spawned.GetComponent<Image>().color = Color.green;
								}
								}
						}
					}
					else {Debug.Log("not exists");}

    }
		public Text name;
		public void click(){
			// but = GameObject.Find("Button");
			// Button letter = but.GetComponent<Button>();
			var buttonSelf = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
			// buttonSelf.GetComponent<Image>().color = Color.red;
			Text text=buttonSelf.gameObject.GetComponentInChildren<Text>();
			Debug.Log(text.text);
			staticname.i_name = text.text;
			Debug.Log(staticname.i_name);
		}

}
