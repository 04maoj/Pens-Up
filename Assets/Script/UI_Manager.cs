using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UI_Manager : MonoBehaviour
{
    public RectTransform A, B;
    // Start is called before the first frame update
    void Start()
    {
        A.gameObject.SetActive(true);
        A.DOAnchorPos(Vector2.zero, 0.25f);
    }

    public void Get_B()
    {
        B.gameObject.SetActive(true);
        A.DOAnchorPos(new Vector2(-1000,0), 0.25f);
        B.DOAnchorPos(Vector2.zero, 0.25f);
        A.gameObject.SetActive(false);
    }
    
}
