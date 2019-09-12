using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigation : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] int next;
    private void OnMouseDown()
    {
        SceneLoader loader = FindObjectOfType<SceneLoader>();
        loader.LoadScence(next);
    }
}
