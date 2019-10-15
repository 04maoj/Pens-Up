using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class lettername : MonoBehaviour
{
    // Start is called before the first frame update
    public Text name;
    void Start() {
      name.text = staticname.i_letter;
    }


    // Update is called once per frame
    void Update()
    {

    }
}
