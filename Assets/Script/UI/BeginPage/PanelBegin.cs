using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelBegin : MonoBehaviour
{
    #region SeralizedFeild
    [SerializeField] UIButton m_teacherButton;
    [SerializeField] UIButton m_studetnButton;
    # endregion
    private void Start()
    {
        EventDelegate.Add(m_teacherButton.onClick, onTeacherClick);
        EventDelegate.Add(m_studetnButton.onClick, onStudentClick);
    }
    private void OnDestroy()
    {
        EventDelegate.Remove(m_teacherButton.onClick, onTeacherClick);
        EventDelegate.Remove(m_studetnButton.onClick, onStudentClick);
    }
    public void onStudentClick()
    {
        Debug.Log("YEsss");
        SceneLoader.GetInstance().LoadScence(1);
    }

    public void onTeacherClick()
    {
        SceneLoader.GetInstance().LoadScence(2);
    }
}
