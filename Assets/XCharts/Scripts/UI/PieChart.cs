﻿using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XCharts
{
    [AddComponentMenu("XCharts/PieChart", 15)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class PieChart : BaseChart
    {
        [SerializeField] private Pie m_Pie = Pie.defaultPie;

        private bool isDrawPie;
        private bool m_IsEnterLegendButtom;
        private bool m_RefreshLabel;

        public Pie pie { get { return m_Pie; } }

        protected override void Awake()
        {
            base.Awake();
            raycastTarget = false;
        }

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            m_Pie = Pie.defaultPie;
            m_Title.text = "PieChart";
            RemoveData();
            AddSerie(SerieType.Pie, "serie1");
            AddData(0, 70, "pie1");
            AddData(0, 20, "pie2");
            AddData(0, 10, "pie3");
        }
#endif

        protected override void Update()
        {
            base.Update();
            if (!isDrawPie) RefreshChart();
        }

        protected override void DrawChart(VertexHelper vh)
        {
            base.DrawChart(vh);
            int serieNameCount = -1;
            bool isClickOffset = false;
            bool isDataHighlight = false;
            for (int i = 0; i < m_Series.Count; i++)
            {
                var serie = m_Series.list[i];
                serie.index = i;
                var data = serie.data;
                serie.animation.InitProgress(data.Count, 0, 360);
                if (!serie.show)
                {
                    continue;
                }
                if (!serie.animation.NeedAnimation(i)) break;
                bool isFinish = true;
                if (serie.pieClickOffset) isClickOffset = true;
                serie.pieDataMax = serie.yMax;
                serie.pieDataTotal = serie.yTotal;
                UpdatePieCenter(serie);

                float totalDegree = 360;
                float startDegree = 0;
                int showdataCount = 0;
                foreach (var sd in serie.data)
                {
                    if (sd.show && serie.pieRoseType == RoseType.Area) showdataCount++;
                    sd.canShowLabel = false;
                }
                for (int n = 0; n < data.Count; n++)
                {
                    var serieData = data[n];
                    float value = serieData.data[1];
                    serieNameCount = m_LegendRealShowName.IndexOf(serieData.name);
                    Color color = m_ThemeInfo.GetColor(serieNameCount);
                    serieData.pieStartAngle = startDegree;
                    serieData.pieToAngle = startDegree;
                    serieData.pieHalfAngle = startDegree;
                    serieData.pieCurrAngle = startDegree;
                    if (!serieData.show)
                    {
                        continue;
                    }
                    float degree = serie.pieRoseType == RoseType.Area ?
                        (totalDegree / showdataCount) : (totalDegree * value / serie.pieDataTotal);
                    serieData.pieToAngle = startDegree + degree;

                    serieData.pieOutsideRadius = serie.pieRoseType > 0 ?
                        serie.pieInsideRadius + (serie.pieOutsideRadius - serie.pieInsideRadius) * value / serie.pieDataMax :
                        serie.pieOutsideRadius;
                    if (serieData.highlighted)
                    {
                        isDataHighlight = true;
                        color *= 1.2f;
                        serieData.pieOutsideRadius += m_Pie.tooltipExtraRadius;
                    }
                    var offset = serie.pieSpace;
                    if (serie.pieClickOffset && serieData.selected)
                    {
                        offset += m_Pie.selectedOffset;
                    }
                    var halfDegree = (serieData.pieToAngle - startDegree) / 2;
                    serieData.pieHalfAngle = startDegree + halfDegree;
                    float currRad = serieData.pieHalfAngle * Mathf.Deg2Rad;
                    float currSin = Mathf.Sin(currRad);
                    float currCos = Mathf.Cos(currRad);
                    var center = serie.pieCenterPos;

                    serieData.pieCurrAngle = serieData.pieToAngle;
                    serieData.pieOffsetCenter = center;
                    serieData.pieInsideRadius = serie.pieInsideRadius;
                    if (serie.animation.CheckDetailBreak(n, serieData.pieToAngle))
                    {
                        isFinish = false;
                        serieData.pieCurrAngle = serie.animation.GetCurrDetail();
                    }
                    if (offset > 0)
                    {
                        serieData.pieOffsetRadius = serie.pieSpace / Mathf.Sin(halfDegree * Mathf.Deg2Rad);
                        serieData.pieInsideRadius -= serieData.pieOffsetRadius;
                        serieData.pieOutsideRadius -= serieData.pieOffsetRadius;
                        if (serie.pieClickOffset && serieData.selected)
                        {
                            serieData.pieOffsetRadius += m_Pie.selectedOffset;
                            if (serieData.pieInsideRadius > 0) serieData.pieInsideRadius += m_Pie.selectedOffset;
                            serieData.pieOutsideRadius += m_Pie.selectedOffset;
                        }

                        serieData.pieOffsetCenter = new Vector3(center.x + serieData.pieOffsetRadius * currSin,
                            center.y + serieData.pieOffsetRadius * currCos);

                        ChartDrawer.DrawDoughnut(vh, serieData.pieOffsetCenter, serieData.pieInsideRadius, serieData.pieOutsideRadius,
                            startDegree, serieData.pieCurrAngle, color);
                    }
                    else
                    {
                        ChartDrawer.DrawDoughnut(vh, center, serieData.pieInsideRadius, serieData.pieOutsideRadius,
                            startDegree, serieData.pieCurrAngle, color);
                    }
                    serieData.canShowLabel = serieData.pieCurrAngle >= serieData.pieHalfAngle;
                    isDrawPie = true;
                    startDegree = serieData.pieToAngle;
                    if (isFinish) serie.animation.SetDataFinish(n);
                    else break;
                }
                if (!serie.animation.IsFinish())
                {
                    float duration = serie.animation.duration > 0 ? (float)serie.animation.duration / 1000 : 1;
                    float speed = 360 / duration;
                    float symbolSpeed = serie.symbol.size / duration;
                    serie.animation.CheckProgress(Time.deltaTime * speed);
                    serie.animation.CheckSymbol(Time.deltaTime * symbolSpeed, serie.symbol.size);
                    RefreshChart();
                }
            }
            DrawLabelLine(vh);
            DrawLabelBackground(vh);
            raycastTarget = isClickOffset && isDataHighlight;
        }

        private void DrawLabelLine(VertexHelper vh)
        {
            foreach (var serie in m_Series.list)
            {
                if (serie.type == SerieType.Pie && serie.label.show)
                {
                    foreach (var serieData in serie.data)
                    {
                        if (serieData.canShowLabel)
                        {
                            int colorIndex = m_LegendRealShowName.IndexOf(serieData.name);
                            Color color = m_ThemeInfo.GetColor(colorIndex);
                            DrawLabelLine(vh, serie, serieData, color);
                        }
                    }
                }
            }
        }

        private void DrawLabelBackground(VertexHelper vh)
        {
            foreach (var serie in m_Series.list)
            {
                if (serie.type == SerieType.Pie && serie.label.show)
                {
                    foreach (var serieData in serie.data)
                    {
                        if (serieData.canShowLabel)
                        {
                            UpdateLabelPostion(serie, serieData);
                            DrawLabelBackground(vh, serie, serieData);
                        }
                    }
                }
            }
        }

        private void DrawLabelLine(VertexHelper vh, Serie serie, SerieData serieData, Color color)
        {
            if (serie.label.show
                && serie.label.position == SerieLabel.Position.Outside
                && serie.label.line)
            {
                var insideRadius = serieData.pieInsideRadius;
                var outSideRadius = serieData.pieOutsideRadius;
                var center = serie.pieCenterPos;
                var currAngle = serieData.pieHalfAngle;
                if (serie.label.lineColor != Color.clear) color = serie.label.lineColor;
                else if (serie.label.lineType == SerieLabel.LineType.HorizontalLine) color *= color;
                float currSin = Mathf.Sin(currAngle * Mathf.Deg2Rad);
                float currCos = Mathf.Cos(currAngle * Mathf.Deg2Rad);
                var radius1 = serie.label.lineType == SerieLabel.LineType.HorizontalLine ?
                    serie.pieOutsideRadius : outSideRadius;
                var radius2 = serie.pieOutsideRadius + serie.label.lineLength1;
                var radius3 = insideRadius + (outSideRadius - insideRadius) / 2;
                var pos0 = new Vector3(center.x + radius3 * currSin, center.y + radius3 * currCos);
                var pos1 = new Vector3(center.x + radius1 * currSin, center.y + radius1 * currCos);
                var pos2 = new Vector3(center.x + radius2 * currSin, center.y + radius2 * currCos);
                float tx, ty;
                Vector3 pos3, pos4, pos6;
                var horizontalLineCircleRadius = serie.label.lineWidth * 4f;
                var lineCircleDiff = horizontalLineCircleRadius - 0.3f;
                if (currAngle < 90)
                {
                    ty = serie.label.lineWidth * Mathf.Cos((90 - currAngle) * Mathf.Deg2Rad);
                    tx = serie.label.lineWidth * Mathf.Sin((90 - currAngle) * Mathf.Deg2Rad);
                    pos3 = new Vector3(pos2.x - tx, pos2.y + ty - serie.label.lineWidth);
                    var r4 = Mathf.Sqrt(radius1 * radius1 - Mathf.Pow(currCos * radius3, 2)) - currSin * radius3;
                    r4 += serie.label.lineLength1 - lineCircleDiff;
                    pos6 = pos0 + Vector3.right * lineCircleDiff;
                    pos4 = pos6 + Vector3.right * r4;
                }
                else if (currAngle < 180)
                {
                    ty = serie.label.lineWidth * Mathf.Sin((180 - currAngle) * Mathf.Deg2Rad);
                    tx = serie.label.lineWidth * Mathf.Cos((180 - currAngle) * Mathf.Deg2Rad);
                    pos3 = new Vector3(pos2.x - tx, pos2.y - ty + serie.label.lineWidth);
                    var r4 = Mathf.Sqrt(radius1 * radius1 - Mathf.Pow(currCos * radius3, 2)) - currSin * radius3;
                    r4 += serie.label.lineLength1 - lineCircleDiff;
                    pos6 = pos0 + Vector3.right * lineCircleDiff;
                    pos4 = pos6 + Vector3.right * r4;
                }
                else if (currAngle < 270)
                {
                    ty = serie.label.lineWidth * Mathf.Sin((180 + currAngle) * Mathf.Deg2Rad);
                    tx = serie.label.lineWidth * Mathf.Cos((180 + currAngle) * Mathf.Deg2Rad);
                    var currSin1 = Mathf.Sin((360 - currAngle) * Mathf.Deg2Rad);
                    var currCos1 = Mathf.Cos((360 - currAngle) * Mathf.Deg2Rad);
                    pos3 = new Vector3(pos2.x + tx, pos2.y - ty + serie.label.lineWidth);
                    var r4 = Mathf.Sqrt(radius1 * radius1 - Mathf.Pow(currCos1 * radius3, 2)) - currSin1 * radius3;
                    r4 += serie.label.lineLength1 - lineCircleDiff;
                    pos6 = pos0 + Vector3.left * lineCircleDiff;
                    pos4 = pos6 + Vector3.left * r4;
                }
                else
                {
                    ty = serie.label.lineWidth * Mathf.Cos((90 + currAngle) * Mathf.Deg2Rad);
                    tx = serie.label.lineWidth * Mathf.Sin((90 + currAngle) * Mathf.Deg2Rad);
                    pos3 = new Vector3(pos2.x + tx, pos2.y + ty - serie.label.lineWidth);
                    var currSin1 = Mathf.Sin((360 - currAngle) * Mathf.Deg2Rad);
                    var currCos1 = Mathf.Cos((360 - currAngle) * Mathf.Deg2Rad);
                    var r4 = Mathf.Sqrt(radius1 * radius1 - Mathf.Pow(currCos1 * radius3, 2)) - currSin1 * radius3;
                    r4 += serie.label.lineLength1 - lineCircleDiff;
                    pos6 = pos0 + Vector3.left * lineCircleDiff;
                    pos4 = pos6 + Vector3.left * r4;
                }
                var pos5 = new Vector3(currAngle > 180 ? pos3.x - serie.label.lineLength2 : pos3.x + serie.label.lineLength2, pos3.y);
                switch (serie.label.lineType)
                {
                    case SerieLabel.LineType.BrokenLine:
                        ChartDrawer.DrawLine(vh, pos1, pos2, serie.label.lineWidth, color);
                        ChartDrawer.DrawLine(vh, pos3, pos5, serie.label.lineWidth, color);
                        break;
                    case SerieLabel.LineType.Curves:
                        ChartDrawer.DrawCurves(vh, pos1, pos5, pos1, pos2, serie.label.lineWidth, color);
                        break;
                    case SerieLabel.LineType.HorizontalLine:
                        ChartDrawer.DrawCricle(vh, pos0, horizontalLineCircleRadius, color, 20);
                        ChartDrawer.DrawLine(vh, pos6, pos4, serie.label.lineWidth, color);
                        break;
                }
            }
        }

        protected override void OnRefreshLabel()
        {
            int serieNameCount = -1;
            for (int i = 0; i < m_Series.Count; i++)
            {
                var serie = m_Series.list[i];
                serie.index = i;
                if (!serie.show) continue;
                var data = serie.data;

                int showdataCount = 0;
                if (serie.pieRoseType == RoseType.Area)
                {
                    foreach (var sd in serie.data)
                    {
                        if (sd.show) showdataCount++;
                    }
                }
                for (int n = 0; n < data.Count; n++)
                {
                    var serieData = data[n];
                    if (!serieData.canShowLabel)
                    {
                        serieData.SetLabelActive(false);
                        continue;
                    }
                    if (!serieData.show) continue;
                    serieNameCount = m_LegendRealShowName.IndexOf(serieData.name);
                    Color color = m_ThemeInfo.GetColor(serieNameCount);

                    DrawLabel(serie, n, serieData, color);
                }
            }
        }

        private void DrawLabel(Serie serie, int dataIndex, SerieData serieData, Color serieColor)
        {
            if (serieData.labelText == null) return;
            var currAngle = serieData.pieHalfAngle;
            var isHighlight = (serieData.highlighted && serie.highlightLabel.show);
            var showLabel = ((serie.label.show || isHighlight) && serieData.canShowLabel);
            if (showLabel || serieData.showIcon)
            {
                serieData.SetLabelActive(showLabel);
                float rotate = 0;
                bool isInsidePosition = serie.label.position == SerieLabel.Position.Inside;
                if (serie.label.rotate > 0 && isInsidePosition)
                {
                    if (currAngle > 180) rotate += 270 - currAngle;
                    else rotate += -(currAngle - 90);
                }
                Color color = serieColor;
                if (isHighlight)
                {
                    if (serie.highlightLabel.color != Color.clear) color = serie.highlightLabel.color;
                }
                else if (serie.label.color != Color.clear)
                {
                    color = serie.label.color;
                }
                else
                {
                    color = isInsidePosition ? Color.white : serieColor;
                }
                var fontSize = isHighlight ? serie.highlightLabel.fontSize : serie.label.fontSize;
                var fontStyle = isHighlight ? serie.highlightLabel.fontStyle : serie.label.fontStyle;

                serieData.labelText.color = color;
                serieData.labelText.fontSize = fontSize;
                serieData.labelText.fontStyle = fontStyle;

                serieData.labelRect.transform.localEulerAngles = new Vector3(0, 0, rotate);

                UpdateLabelPostion(serie, serieData);
                if (!string.IsNullOrEmpty(serie.label.formatter))
                {
                    var value = serieData.data[1];
                    var total = serie.yTotal;
                    var content = serie.label.GetFormatterContent(serie.name, serieData.name, value, total);
                    if (serieData.SetLabelText(content)) RefreshChart();
                }
                serieData.SetGameObjectPosition(serieData.labelPosition);
                if (showLabel) serieData.SetLabelPosition(serie.label.offset);
            }
            else
            {
                serieData.SetLabelActive(false);
            }
            serieData.UpdateIcon();
        }

        protected void UpdateLabelPostion(Serie serie, SerieData serieData)
        {
            var currAngle = serieData.pieHalfAngle;
            var currRad = currAngle * Mathf.Deg2Rad;
            var offsetRadius = serieData.pieOffsetRadius;
            var insideRadius = serieData.pieInsideRadius;
            var outsideRadius = serieData.pieOutsideRadius;
            switch (serie.label.position)
            {
                case SerieLabel.Position.Center:
                    serieData.labelPosition = serie.pieCenterPos;
                    break;
                case SerieLabel.Position.Inside:
                    var labelRadius = offsetRadius + insideRadius + (outsideRadius - insideRadius) / 2;
                    var labelCenter = new Vector2(serie.pieCenterPos.x + labelRadius * Mathf.Sin(currRad),
                        serie.pieCenterPos.y + labelRadius * Mathf.Cos(currRad));
                    serieData.labelPosition = labelCenter;
                    break;
                case SerieLabel.Position.Outside:
                    if (serie.label.lineType == SerieLabel.LineType.HorizontalLine)
                    {
                        var radius1 = serie.pieOutsideRadius;
                        var radius3 = insideRadius + (outsideRadius - insideRadius) / 2;
                        var currSin = Mathf.Sin(currRad);
                        var currCos = Mathf.Cos(currRad);
                        var pos0 = new Vector3(serie.pieCenterPos.x + radius3 * currSin, serie.pieCenterPos.y + radius3 * currCos);
                        if (currAngle > 180)
                        {
                            currSin = Mathf.Sin((360 - currAngle) * Mathf.Deg2Rad);
                            currCos = Mathf.Cos((360 - currAngle) * Mathf.Deg2Rad);
                        }
                        var r4 = Mathf.Sqrt(radius1 * radius1 - Mathf.Pow(currCos * radius3, 2)) - currSin * radius3;
                        r4 += serie.label.lineLength1 + serie.label.lineWidth * 4;
                        r4 += serieData.labelText.preferredWidth / 2;
                        serieData.labelPosition = pos0 + (currAngle > 180 ? Vector3.left : Vector3.right) * r4;
                    }
                    else
                    {
                        labelRadius = serie.pieOutsideRadius + serie.label.lineLength1;
                        labelCenter = new Vector2(serie.pieCenterPos.x + labelRadius * Mathf.Sin(currRad),
                            serie.pieCenterPos.y + labelRadius * Mathf.Cos(currRad));
                        float labelWidth = serieData.labelText.preferredWidth;
                        if (currAngle > 180)
                        {
                            serieData.labelPosition = new Vector2(labelCenter.x - serie.label.lineLength2 - 5 - labelWidth / 2, labelCenter.y);
                        }
                        else
                        {
                            serieData.labelPosition = new Vector2(labelCenter.x + serie.label.lineLength2 + 5 + labelWidth / 2, labelCenter.y);
                        }
                    }
                    break;
            }
        }

        protected override void OnLegendButtonClick(int index, string legendName, bool show)
        {
            bool active = CheckDataShow(legendName, show);
            var bgColor1 = active ? m_ThemeInfo.GetColor(index) : m_ThemeInfo.legendUnableColor;
            m_Legend.UpdateButtonColor(legendName, bgColor1);
            RefreshChart();
        }

        protected override void OnLegendButtonEnter(int index, string legendName)
        {
            m_IsEnterLegendButtom = true;
            CheckDataHighlighted(legendName, true);
            RefreshChart();
        }

        protected override void OnLegendButtonExit(int index, string legendName)
        {
            m_IsEnterLegendButtom = false;
            CheckDataHighlighted(legendName, false);
            RefreshChart();
        }

        private void UpdatePieCenter(Serie serie)
        {
            if (serie.pieCenter.Length < 2) return;
            var centerX = serie.pieCenter[0] <= 1 ? chartWidth * serie.pieCenter[0] : serie.pieCenter[0];
            var centerY = serie.pieCenter[1] <= 1 ? chartHeight * serie.pieCenter[1] : serie.pieCenter[1];
            serie.pieCenterPos = new Vector2(centerX, centerY);
            var minWidth = Mathf.Min(chartWidth, chartHeight);
            serie.pieInsideRadius = serie.pieRadius[0] <= 1 ? minWidth * serie.pieRadius[0] : serie.pieRadius[0];
            serie.pieOutsideRadius = serie.pieRadius[1] <= 1 ? minWidth * serie.pieRadius[1] : serie.pieRadius[1];
        }

        protected override void CheckTootipArea(Vector2 local)
        {
            if (m_IsEnterLegendButtom) return;
            m_Tooltip.dataIndex.Clear();
            bool selected = false;
            foreach (var serie in m_Series.list)
            {
                int index = GetPosPieIndex(serie, local);
                m_Tooltip.dataIndex.Add(index);
                if (serie.type != SerieType.Pie) continue;
                bool refresh = false;
                for (int j = 0; j < serie.data.Count; j++)
                {
                    var serieData = serie.data[j];
                    if (serieData.highlighted != (j == index)) refresh = true;
                    serieData.highlighted = j == index;
                }
                if (index >= 0) selected = true;
                if (refresh) RefreshChart();
            }
            if (selected)
            {
                m_Tooltip.UpdateContentPos(new Vector2(local.x + 18, local.y - 25));
                RefreshTooltip();
            }
            else if (m_Tooltip.IsActive())
            {
                m_Tooltip.SetActive(false);
                RefreshChart();
            }
        }

        private int GetPosPieIndex(Serie serie, Vector2 local)
        {
            if (serie.type != SerieType.Pie) return -1;
            var dist = Vector2.Distance(local, serie.pieCenterPos);
            if (dist < serie.pieInsideRadius || dist > serie.pieOutsideRadius) return -1;
            Vector2 dir = local - new Vector2(serie.pieCenterPos.x, serie.pieCenterPos.y);
            float angle = VectorAngle(Vector2.up, dir);
            for (int i = 0; i < serie.data.Count; i++)
            {
                var serieData = serie.data[i];
                if (angle >= serieData.pieStartAngle && angle <= serieData.pieToAngle)
                {
                    return i;
                }
            }
            return -1;
        }

        float VectorAngle(Vector2 from, Vector2 to)
        {
            float angle;

            Vector3 cross = Vector3.Cross(from, to);
            angle = Vector2.Angle(from, to);
            angle = cross.z > 0 ? -angle : angle;
            angle = (angle + 360) % 360;
            return angle;
        }

        StringBuilder sb = new StringBuilder();
        protected override void RefreshTooltip()
        {
            base.RefreshTooltip();
            bool showTooltip = false;
            foreach (var serie in m_Series.list)
            {
                int index = m_Tooltip.dataIndex[serie.index];
                if (index < 0) continue;
                showTooltip = true;
                if (string.IsNullOrEmpty(tooltip.formatter))
                {
                    string key = serie.data[index].name;
                    if (string.IsNullOrEmpty(key)) key = m_Legend.GetData(index);

                    float value = serie.data[index].data[1];
                    sb.Length = 0;
                    if (!string.IsNullOrEmpty(serie.name))
                    {
                        sb.Append(serie.name).Append("\n");
                    }
                    sb.Append("<color=#").Append(m_ThemeInfo.GetColorStr(index)).Append(">● </color>")
                        .Append(key).Append(": ").Append(ChartCached.FloatToStr(value));
                    m_Tooltip.UpdateContentText(sb.ToString());
                }
                else
                {
                    m_Tooltip.UpdateContentText(m_Tooltip.GetFormatterContent(index, m_Series, null));
                }

                var pos = m_Tooltip.GetContentPos();
                if (pos.x + m_Tooltip.width > chartWidth)
                {
                    pos.x = chartWidth - m_Tooltip.width;
                }
                if (pos.y - m_Tooltip.height < 0)
                {
                    pos.y = m_Tooltip.height;
                }
                m_Tooltip.UpdateContentPos(pos);
            }
            m_Tooltip.SetActive(showTooltip);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            Vector2 local;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform,
                eventData.position, canvas.worldCamera, out local))
            {
                return;
            }
            for (int i = 0; i < m_Series.Count; i++)
            {
                var serie = m_Series.GetSerie(i);
                if (serie.type == SerieType.Pie)
                {
                    var index = GetPosPieIndex(serie, local);
                    if (index >= 0)
                    {
                        for (int j = 0; j < serie.data.Count; j++)
                        {
                            if (j == index) serie.data[j].selected = !serie.data[j].selected;
                            else serie.data[j].selected = false;
                        }
                    }
                }
            }
            RefreshChart();
        }
    }
}
