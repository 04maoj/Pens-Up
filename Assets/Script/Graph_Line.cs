using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Graph_Line : MonoBehaviour
{
    private RectTransform graphContainer;
    [SerializeField]private Sprite circleSprite;

    private void Awake()
    {
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
        RenderCircle(new Vector2(200, 200));
    }

    private void RenderCircle(Vector2 anchoredPosition)
    {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        // gameObject.GetComponent<Image>().color = new Color(1,1,1, .5f);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMin =new Vector2(0, 0);
        rectTransform.anchorMax =new Vector2(0, 0);

    }
}
