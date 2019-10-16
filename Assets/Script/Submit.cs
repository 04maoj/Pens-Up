using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEditor;
using System;
using System.IO;
using LitJson;


public class Submit : MonoBehaviour
{
    private AssessProcessMgmt assessMgmt;
    private Track_manager trackMgmt;
    private SceneLoader sceneLoad;
    private GameObject box;
    private Camera ca;
    private DrawManager drawManager;
    private Scence_Manager sceneMgmt;
    [SerializeField] string character;
    private GameObject clone;
    private LineRenderer lineRe;
    public GameObject target;
    // private Track_manager trackmg;
    List<List<Tuple<float, float>>> coordinates;
    List<Vector3> stroke;
    Vector3 currentPosition;
    int index = -1;
    // X-axis offset of the character
    public float offSetX;
    // Y-axis offset of the character
    public float offSetY;
    // Z-axis offset of the drawing space
    public float offSetZ = -1f;
    public float scale = 1;

    private string subscriptionKey;
    private string endpoint;
    private string uriBase;
    private string extraPara;

    public string user_name;
    public string path;
    public string filepath;
    public string contentpath;
    public string scorepath;
    public List<string> analyzed;
    public UnityWebRequest send = null;
    public UnityWebRequest get = null;
    private User_Info user;

    // Start is called before the first frame update
    private void Awake()
    {
        ca = Camera.main;
    }
    void Start()
    {
        //trackMgmt = FindObjectOfType<Track_manager>();
        assessMgmt = FindObjectOfType<AssessProcessMgmt>();
        box = assessMgmt.process;
        Button btn = this.GetComponent<Button>();
        drawManager = FindObjectOfType<DrawManager>();
        sceneMgmt = FindObjectOfType<Scence_Manager>();
        sceneLoad = FindObjectOfType<SceneLoader>();

        user = FindObjectOfType<User_Info>();
        user_name = user.Get_UserName();
        path = "Assets/Local_DataBase/Students/" + user_name + "/" + drawManager.getCharacter();
        filepath = path + ".png";
        contentpath = "Assets/Standards/" + drawManager.getCharacter() + ".content";

        scorepath = path + ".score";
        endpoint = "https://pensupocr.cognitiveservices.azure.com/";
        uriBase = "vision/v2.0/recognizeText";
        extraPara = "?mode=Handwritten";
        subscriptionKey = "4cba78cfd4db4905b67c7a528d0bee8f";
        btn.onClick.AddListener(OnClick);
        //btn.onClick.AddListener(OnClick);

    }

    // Update is called once per frame
    void OnClick()
    {
        drawManager.setCharacter(character);
        coordinates = drawManager.GetStrokes(character);
        StartCoroutine(Communicate());
    }

    public IEnumerator Communicate()
    {
        //yield return StartCoroutine(Draw());

        //Debug.Log("Replay Finished");
        ScreenCapture.CaptureScreenshot(filepath);
        // Wait for seconds to ensure the shot was stored.
        StartCoroutine(Wait());

        byte[] image = GetImageAsByteArray(filepath);
        Debug.Log(endpoint + uriBase + extraPara);
        send = UnityWebRequest.Put(endpoint + uriBase + extraPara, image);
        send.method = UnityWebRequest.kHttpVerbPOST;
        //send.downloadHandler = new DownloadHandlerBuffer();
        send.SetRequestHeader("Content-Type", "application/octet-stream");
        send.SetRequestHeader("Ocp-Apim-Subscription-Key", subscriptionKey);
        //send.uploadHandler.contentType = "application/octet-stream";

        yield return send.SendWebRequest();
        string response = send.GetResponseHeader("Operation-Location");
        Debug.Log("Send: method: " + send.method);
        Debug.Log("Send uri: " + send.uri);
        Debug.Log("Content-Type: " + send.GetRequestHeader("Content-Type"));
        Debug.Log("response: " + response);
        Debug.Log("Processing...");
        //trackMgmt.Clear_All();
        sceneMgmt.EraseAll();
        box.SetActive(true);

        yield return new WaitForSeconds(10f);

        get = UnityWebRequest.Get(response);
        get.method = UnityWebRequest.kHttpVerbGET;
        get.SetRequestHeader("Content-Type", "application/octet-stream");
        get.SetRequestHeader("Ocp-Apim-Subscription-Key", subscriptionKey);
        //get.uploadHandler.contentType = "application/octet-stream";
        Debug.Log("Get uri: " + get.uri);
        yield return get.SendWebRequest();

        float score = 0f;
        // If error occurs, use local evaluation algorithm instead.
        if (get.isNetworkError == true)
        {
            Debug.Log("NetworkError");
            score = Evaluate();

        }
        else if (get.isHttpError == true)
        {
            Debug.Log("HttpError");
            score = Evaluate();
        }
        else
        {
            Debug.Log("Success");
            Debug.Log(get.downloadHandler.text);
            File.WriteAllText(path + ".json", get.downloadHandler.text);
            //JsonData result = JsonMapper.ToObject(send.downloadHandler.text);
            analyzed = Decom(get.downloadHandler.text);
            Debug.Log("Word Count: " + analyzed.Count);
            score = ResultEvaluate();
            //for (int i = 0; i < analyzed.Count; i++)
            //{
            //    Debug.Log("Ana: " + analyzed[i]);
            //}
            //Debug.Log(result);
        }
        if (File.Exists(scorepath))
        {
            File.Delete(scorepath);
            //Debug.Log("Deleted");
        }
        File.WriteAllText(scorepath, score.ToString());
        box.SetActive(false);
        // Back to student page
        sceneLoad.LoadScence(24);

    }

    public IEnumerator Wait()
    {
        AssetDatabase.Refresh();
        yield return new WaitForSeconds(3f);
    }

    public List<string> Decom(string str)
    {
        List<string> back = new List<string>();
        JsonData jd = JsonMapper.ToObject(str);
        //Debug.Log(jd["recognitionResult"].Count);
        int before = 3;
        int after = 2;
        int lineCount = jd["recognitionResult"]["lines"].Count - before - after;
        int wordCount;
        for (int i = 0; i < lineCount; i++)
        {
            wordCount = jd["recognitionResult"]["lines"][before + i]["words"].Count;
            for (int j = 0; j < wordCount; j++)
            {
                back.Add(jd["recognitionResult"]["lines"][before + i]["words"][j]["text"].ToString());
                Debug.Log("Result; " + jd["recognitionResult"]["lines"][before + i]["words"][j]["text"].ToString());
            }
            //back.Add(jd["recognitionResults"]["lines"][i]["text"].ToString());
        }
        //back.Add(jd["recognitionResults"]["lines"][lineCount - 1]["words"][jd["recognitionResults"]["lines"][lineCount - 1]["words"].Count - 1]["confidence"].ToString());
        foreach (string temp in back)
        {
            Debug.Log(temp);
        }

        return back;
    }

    public byte[] GetImageAsByteArray(string imageFilePath)
    {
        // Open a read-only file stream for the specified file.
        using (FileStream fileStream =
            new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
        {
            // Read the file's contents into a byte array.
            BinaryReader binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }
    }

    public List<string> LoadContent(string contentpath)
    {
        StreamReader sr = new StreamReader(contentpath);
        List<string> content = new List<string>();
        string line = "";
        line = sr.ReadLine();
        while (line != null)
        {
            content.Add(line);
            line = sr.ReadLine();
        }
        return content;
    }

    public float ResultEvaluate()
    {
        List<string> content = LoadContent(contentpath);
        int contentCount = content.Count;
        int anaCount = analyzed.Count;
        string anaString = "";
        string contentString = "";
        float score = 100f;
        for (int i = 0; i < anaCount; i++)
        {
            anaString += analyzed[i];
        }
        for (int i = 0; i < contentCount; i++)
        {
            contentString += content[i];
            contentString += content[i];
        }
        if (anaString.Length != contentString.Length)
        {
            score = anaString.Length * 100 / contentString.Length * 100;
        }
        string anaSub = "";
        string contentSub = "";
        float part = 100 / anaString.Length * 100;
        for (int i = 0; i < anaString.Length; i++)
        {
            anaSub = anaString.Substring(i, 1);
            contentSub = contentString.Substring(i, 1);
            if (anaSub.Equals(contentSub) == false)
            {
                score -= part;
            }
            //Debug.Log(score);
        }
        score = Math.Max(0, score);
        //Debug.Log("anaString: " + anaString);
        //Debug.Log("contentString: " + contentString);
        score = score / 100;
        Debug.Log("Score: " + score);
        return score;
    }

    public float Evaluate()
    {
        List<List<Tuple<float, float>>> standard = drawManager.GetStrokes("Assessment1_std");
        List<List<Tuple<float, float>>> input = drawManager.GetStrokes("Assessment1");
        float score = CompareFloat(input, standard);
        return score;

    }

    public float CompareFloat(List<List<Tuple<float, float>>> analyzed, List<List<Tuple<float, float>>> content)
    {
        float score = 100;
        int index0 = 0;
        int index1 = 0;
        int currentIdx = 0;
        bool sameNum = false;
        int anaCount = analyzed.Count;
        int conCount = content.Count;
        float strokeDevi = 0;
        float pointDevi = 0;
        float totalDevi = 0;
        float threshold = 100f;
        int currentMin = 0;
        if (anaCount == conCount)
        {
            sameNum = true;
        }
        int minCount = Math.Min(anaCount, conCount);
        for (int i = 0; i < minCount; i++)
        {
            strokeDevi = 0;
            currentMin = Math.Min(analyzed[i].Count, content[i].Count);
            for (int j = 0; j < currentMin; j++)
            {
                pointDevi = 0;
                if (Math.Abs(analyzed[i][j].Item1 - content[i][j].Item1) > threshold || Math.Abs(analyzed[i][j].Item2 - content[i][j].Item2) > threshold)
                {
                    pointDevi = Math.Max(Math.Abs(analyzed[i][j].Item1 - content[i][j].Item1), Math.Abs(analyzed[i][j].Item2 - content[i][j].Item2));

                }
                strokeDevi += pointDevi;
            }
            totalDevi += strokeDevi;
        }
        score -= totalDevi / 1000;
        return score;
    }


}
