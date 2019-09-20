using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resolutionManager : MonoBehaviour
{
    public int width;
    public int height;

    public void SetWidth(int w) { this.width = w; }
    public void SetHeight(int h) { this.height = h; }

    public void Set_res() { Screen.SetResolution(width, height, false); }

    // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }
}
