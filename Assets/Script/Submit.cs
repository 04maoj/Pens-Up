using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.IO;
using LitJson;


public class Submit : MonoBehaviour
{
    private Camera ca;
    private DrawManager drawManager;
    private Scence_Manager sMgr;
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

    private string url;
    //public string url = "https://australiaeast.api.cognitive.microsoft.com/vision/v1.0/ocr";
    private string endpoint;
    private string uriBase;
    private string extraPara;
    //public string uri;

    // the Batch Read method endpoint
    //public string uriBase = endpoint + "/read/core/asyncBatchAnalyze";
    public string filepath;
    public string contentpath;
    public List<string> analyzed;
    public UnityWebRequest send = null;
    public UnityWebRequest get = null;


    // Start is called before the first frame update
    private void Awake()
    {
        ca = Camera.main;
    }
    void Start()
    {
        Button btn = this.GetComponent<Button>();
        drawManager = FindObjectOfType<DrawManager>();
        sMgr = FindObjectOfType<Scence_Manager>();

        filepath = "Assets/Standards/" + drawManager.getCharacter() + ".png";
        contentpath = "Assets/Standards/" + drawManager.getCharacter() + ".content";
        url = "https://australiaeast.api.cognitive.microsoft.com/vision/v2.0/recognizeText?mode=Handwritten";
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
        // Debug.Log("Got Strokes");
        // drawManager.DrawBot(coordinates);
        //StartCoroutine(WaitAndPaint());


        //StartCoroutine(DrawAndCommunicate());
        Evaluate();
    }

    public IEnumerator DrawAndCommunicate()
    {


        yield return StartCoroutine(Draw());

        Debug.Log("Replay Finished");
        ScreenCapture.CaptureScreenshot(filepath);
        //Texture2D img = ScreenCapture.CaptureScreenshotAsTexture(1);
        //byte[] image = img.GetRawTextureData();

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

        yield return new WaitForSeconds(10f);

        get = UnityWebRequest.Get(response);
        get.method = UnityWebRequest.kHttpVerbGET;
        get.SetRequestHeader("Content-Type", "application/octet-stream");
        get.SetRequestHeader("Ocp-Apim-Subscription-Key", subscriptionKey);
        //get.uploadHandler.contentType = "application/octet-stream";
        Debug.Log("Get uri: " + get.uri);
        yield return get.SendWebRequest();

        if (get.isNetworkError == true)
        {
            Debug.Log("NetworkError");
        }
        else if (get.isHttpError == true)
        {
            Debug.Log("HttpError");
        }
        else
        {
            Debug.Log("Success");
            Debug.Log(get.downloadHandler.text);
            File.WriteAllText("Assets/Standards/" + drawManager.getCharacter() + ".json", get.downloadHandler.text);
            //JsonData result = JsonMapper.ToObject(send.downloadHandler.text);
            analyzed = Decom(get.downloadHandler.text);
            Debug.Log("Word Count: " + analyzed.Count);
            for (int i = 0; i < analyzed.Count; i++)
            {
                Debug.Log("Ana: " + analyzed[i]);
            }
            //Debug.Log(result);
        }

        List<string> content = LoadContent(contentpath);
        for (int i = 0; i < content.Count; i++)
        {
            Debug.Log(content[i]);
        }
        //List<List<Tuple<float, float>>> standard = drawManager.GetStrokes("Assessment1_std");
        //List<List<Tuple<float, float>>> input = drawManager.GetStrokes("Assessment1");
        //float score = CompareFloat(input, standard);
        //Debug.Log("score: " + score);
        //float score = Compare(analyzed, content);
        //File.WriteAllText("Assets/Standards/" + drawManager.getCharacter() + ".score", score.ToString());
    }

    public IEnumerator Draw()
    {
        sMgr.EraseAll();
        int count = 0;
        foreach (List<Tuple<float, float>> eachStroke in coordinates)
        {
            clone = (GameObject)Instantiate(target, target.transform.position, Quaternion.identity);
            lineRe = clone.GetComponent<LineRenderer>();
            lineRe.alignment = LineAlignment.View;
            lineRe.startColor = Color.red;
            lineRe.endColor = Color.blue;
            lineRe.startWidth = 0.2f;
            lineRe.endWidth = 0.2f;
            count = eachStroke.Count;
            stroke = new List<Vector3>();
            foreach (Tuple<float, float> point in eachStroke)
            {
                Vector3 pointCoordinate = new Vector3(point.Item1, point.Item2, -10f);

                Vector3 pointCam = Camera.main.ScreenToWorldPoint(pointCoordinate);
                pointCam.z = -5f;

                stroke.Add(pointCam);
            }
            lineRe.positionCount = count;

            currentPosition = new Vector3();

            for (int i = 0; i < lineRe.positionCount; i++)
            {
                index = i;
                currentPosition = stroke[i];
                lineRe.SetPosition(i, currentPosition);


            }
        }
        yield return new WaitForSeconds(3f);
    }

    public List<string> Decom(string str)
    {
        List<string> back = new List<string>();
        JsonData jd = JsonMapper.ToObject(str);
        Debug.Log(jd["recognitionResult"].Count);
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
            }
            //back.Add(jd["recognitionResults"]["lines"][i]["text"].ToString());
        }
        //back.Add(jd["recognitionResults"]["lines"][lineCount - 1]["words"][jd["recognitionResults"]["lines"][lineCount - 1]["words"].Count - 1]["confidence"].ToString());
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




    void Evaluate()
    {
        List<List<Tuple<float, float>>> standard = drawManager.GetStrokes("Assessment1_std");
        List<List<Tuple<float, float>>> input = drawManager.GetStrokes("Assessment1");
        float score = CompareFloat(input, standard);
        File.WriteAllText("Assets/Standards/" + drawManager.getCharacter() + ".score", score.ToString());
        Debug.Log("score: " + score);
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
