using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class Replay : MonoBehaviour
{
    private DrawManager drawManager;
    [SerializeField] string character;
    private GameObject clone;
    private LineRenderer lineRe;
    public GameObject target;
    // private Track_manager trackmg;
    List<List<Tuple<float, float>>> coordinates;
    
    // Start is called before the first frame update
    void Start()
    {
        Button btn = this.GetComponent<Button>();
        drawManager = FindObjectOfType<DrawManager>();
        btn.onClick.AddListener(OnClick);
        
    }

    // Update is called once per frame
    void OnClick()
    {
        Debug.Log("'Replay' Pressed.");
        // drawManager.setCharacter(character);
        Debug.Log("Replay char: " + character);
        coordinates = drawManager.GetStrokes(character);
        Debug.Log("Got Strokes");
        drawManager.DrawBot(coordinates);
        Debug.Log("Finish Replay");
        
    }
}
