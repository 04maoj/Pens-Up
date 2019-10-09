using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AssessProcessMgmt : MonoBehaviour
{
    public GameObject canvas;
    public GameObject process;
    private DrawManager drawMgmt;
    // Start is called before the first frame update
    void Start()
    {
        process = canvas.transform.Find("Processing").gameObject;
        process.SetActive(false);
        drawMgmt = FindObjectOfType<DrawManager>();
        if (File.Exists("Assets/Standards/" + drawMgmt.getCharacter()))
        {
            File.Delete("Assets/Standards/" + drawMgmt.getCharacter());
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
