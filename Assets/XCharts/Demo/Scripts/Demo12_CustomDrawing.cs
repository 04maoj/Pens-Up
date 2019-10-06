﻿using UnityEngine;
using UnityEngine.UI;
using XCharts;

[DisallowMultipleComponent]
[ExecuteInEditMode]
public class Demo12_CustomDrawing : MonoBehaviour
{
    LineChart chart;
    void Awake()
    {
        chart = gameObject.GetComponent<LineChart>();
        if (chart == null) return;

        chart.customDrawCallback = delegate (VertexHelper vh)
        {
            var dataPoints = chart.series.list[0].dataPoints;
            if (dataPoints.Count > 0)
            {
                var pos = dataPoints[3];
                var zeroPos = new Vector3(chart.coordinateX, chart.coordinateY);
                var startPos = new Vector3(pos.x, zeroPos.y);
                var endPos = new Vector3(pos.x, zeroPos.y + chart.coordinateHig);
                ChartDrawer.DrawLine(vh, startPos, endPos, 1, Color.blue);
                ChartDrawer.DrawCricle(vh,pos,5,Color.blue);
            }
        };
    }
}
