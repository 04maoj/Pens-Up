using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UI_Manager : MonoBehaviour
{
    public List<RectTransform> courses;
    // Start is called before the first frame update
    private int current_active = 0;
    void Start()
    {
        current_active = 0;
        courses[0].gameObject.SetActive(true);
        courses[0].DOAnchorPos(Vector2.zero, 0.25f);
    }
    public void Get_Course(int index)
    {
        courses[index].gameObject.SetActive(true);
        courses[current_active].DOAnchorPos(new Vector2(-1000, 0), 0.25f);
        courses[index].DOAnchorPos(Vector2.zero, 0.25f);
        courses[current_active].gameObject.SetActive(false);
        current_active = index;
    }

}
