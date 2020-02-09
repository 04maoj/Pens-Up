using UnityEngine;
using System.Collections;
public class DBManager : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        StartCoroutine(Register());
    }

    IEnumerator Register()
    {
        WWWForm form = new WWWForm();
        form.AddField("UserName", "YES");
        form.AddField("Password", "YEH");
        form.AddField("Name", "WOW");
        WWW www = new WWW("http://localhost:8888/sqlconnect/register.php",form);
        yield return www;
        Debug.Log(www.text);
        if (www.text == "0")
            Debug.Log("Success");
        else
            Debug.Log("NOPE");
    }
}
