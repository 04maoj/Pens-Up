using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class list : MonoBehaviour
{
    // Start is called before the first frame update
	public GameObject spawner;
	public GameObject Content;
    void Start()
    {
		for (int i=0; i<3; i++) {
        var current_spawned = Instantiate(spawner, transform.position, Quaternion.identity);
        current_spawned.gameObject.transform.SetParent(Content.transform);
        current_spawned.transform.localScale = new Vector3(1, 1, 1);
        current_spawned.transform.GetChild(0).GetComponent<Text>().text = "Hey";
				current_spawned.transform.position = new Vector3(550+i*800,1000,1000);
	}
    }

    // Update is called once per frame
    void Update()
    {

    }
}
