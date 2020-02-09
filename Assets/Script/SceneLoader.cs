using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    #region Singleton Value
    static SceneLoader _scenceLoader;
    #endregion

    #region private values
    private int m_currentScence;
    #endregion
    public static SceneLoader GetInstance()
    {
        if (_scenceLoader == null)
            _scenceLoader = new SceneLoader();
            return _scenceLoader;
    }

    public void LoadScence(int index)
    {
        m_currentScence = index;
        SceneManager.LoadScene(m_currentScence);
    }

}
