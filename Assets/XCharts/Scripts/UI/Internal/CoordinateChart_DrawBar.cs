using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XCharts
{
    public partial class CoordinateChart
    {
        protected float m_BarLastOffset = 0;

        protected void DrawYBarSerie(VertexHelper vh, Serie serie, int colorIndex, ref List<float> seriesHig)
        {
            if (!IsActive(serie.name)) return;
            var xAxis = m_XAxises[serie.axisIndex];
            var yAxis = m_YAxises[serie.axisIndex];
            if (!yAxis.show) yAxis = m_YAxises[(serie.axisIndex + 1) % m_YAxises.Count];

            float categoryWidth = yAxis.GetDataWidth(coordinateHig, m_DataZoom);
            float barGap = GetBarGap();
            float totalBarWidth = GetBarTotalWidth(categoryWidth, barGap);
            float barWidth = serie.GetBarWidth(categoryWidth);
            float offset = (categoryWidth - totalBarWidth) / 2;
            float barGapWidth = barWidth + barWidth * barGap;
            float space = serie.barGap == -1 ? offset : offset + m_BarLastOffset;

            var showData = serie.GetDataList(m_DataZoom);
            int maxCount = serie.maxShow > 0 ?
                (serie.maxShow > showData.Count ? showData.Count : serie.maxShow)
                : showData.Count;
            if (seriesHig.Count < serie.minShow)
            {
                for (int i = 0; i < serie.minShow; i++)
                {
                    seriesHig.Add(0);
                }
            }
            for (int i = serie.minShow; i < maxCount; i++)
            {
                if (i >= seriesHig.Count)
                {
                    seriesHig.Add(0);
                }
                float value = showData[i].data[1];
                float pX = seriesHig[i] + coordinateX + xAxis.zeroXOffset + yAxis.axisLine.width;
                float pY = coordinateY + +i * categoryWidth;
                if (!yAxis.boundaryGap) pY -= categoryWidth / 2;
                float barHig = (xAxis.minValue > 0 ? value - xAxis.minValue : value)
                    / (xAxis.maxValue - xAxis.minValue) * coordinateWid;
                seriesHig[i] += barHig;

                float currHig = CheckAnimation(serie, i, barHig);

                Vector3 p1 = new Vector3(pX, pY + space + barWidth);
                Vector3 p2 = new Vector3(pX + currHig, pY + space + barWidth);
                Vector3 p3 = new Vector3(pX + currHig, pY + space);
                Vector3 p4 = new Vector3(pX, pY + space);
                serie.dataPoints.Add(new Vector3(pX + currHig, pY + space + barWidth / 2));
                var highlight = (m_Tooltip.show && m_Tooltip.IsSelected(i))
                    || serie.data[i].highlighted
                    || serie.highlighted;
                if (serie.show)
                {
                    Color areaColor = serie.GetAreaColor(m_ThemeInfo, colorIndex, highlight);
                    Color areaToColor = serie.GetAreaToColor(m_ThemeInfo, colorIndex, highlight);
                    ChartDrawer.DrawPolygon(vh, p1, p2, p3, p4, areaColor, areaToColor);
                }
            }
            if (!m_Series.IsStack(serie.stack, SerieType.Bar))
            {
                m_BarLastOffset += barGapWidth;
            }
        }

        private float CheckAnimation(Serie serie, int dataIndex, float barHig)
        {
            float currHig = barHig;
            if (!serie.animation.IsFinish())
            {
                if (serie.animation.IsInDelay()) currHig = 0;
                else
                {
                    var speed = serie.animation.duration > 0 ? barHig / serie.animation.duration * 1000 : barHig;
                    currHig = serie.animation.GetDataState(dataIndex) + speed * Time.deltaTime;
                    serie.animation.SetDataState(dataIndex, currHig);
                    if (Mathf.Abs(currHig) >= Mathf.Abs(barHig))
                    {
                        serie.animation.End();
                        currHig = barHig;
                    }
                }
                RefreshChart();
            }
            return currHig;
        }

        protected void DrawXBarSerie(VertexHelper vh, Serie serie, int colorIndex, ref List<float> seriesHig)
        {
            if (!IsActive(serie.name)) return;
            var showData = serie.GetDataList(m_DataZoom);
            var yAxis = m_YAxises[serie.axisIndex];
            var xAxis = m_XAxises[serie.axisIndex];
            if (!xAxis.show) xAxis = m_XAxises[(serie.axisIndex + 1) % m_XAxises.Count];
            float categoryWidth = xAxis.GetDataWidth(coordinateWid, m_DataZoom);
            float barGap = GetBarGap();
            float totalBarWidth = GetBarTotalWidth(categoryWidth, barGap);
            float barWidth = serie.GetBarWidth(categoryWidth);
            float offset = (categoryWidth - totalBarWidth) / 2;
            float barGapWidth = barWidth + barWidth * barGap;
            float space = serie.barGap == -1 ? offset : offset + m_BarLastOffset;
            int maxCount = serie.maxShow > 0 ?
                (serie.maxShow > showData.Count ? showData.Count : serie.maxShow)
                : showData.Count;

            if (seriesHig.Count < serie.minShow)
            {
                for (int i = 0; i < serie.minShow; i++)
                {
                    seriesHig.Add(0);
                }
            }
            for (int i = serie.minShow; i < maxCount; i++)
            {
                if (i >= seriesHig.Count)
                {
                    seriesHig.Add(0);
                }
                float value = showData[i].data[1];
                float pX = coordinateX + i * categoryWidth;
                float zeroY = coordinateY + yAxis.zeroYOffset;
                if (!xAxis.boundaryGap) pX -= categoryWidth / 2;
                float pY = seriesHig[i] + zeroY + xAxis.axisLine.width;
                float barHig = (yAxis.minValue > 0 ? value - yAxis.minValue : value)
                    / (yAxis.maxValue - yAxis.minValue) * coordinateHig;
                seriesHig[i] += barHig;

                float currHig = CheckAnimation(serie, i, barHig);

                Vector3 p1 = new Vector3(pX + space, pY);
                Vector3 p2 = new Vector3(pX + space, pY + currHig);
                Vector3 p3 = new Vector3(pX + space + barWidth, pY + currHig);
                Vector3 p4 = new Vector3(pX + space + barWidth, pY);
                serie.dataPoints.Add(new Vector3(pX + space + barWidth / 2, pY + currHig));
                var highlight = (m_Tooltip.show && m_Tooltip.IsSelected(i))
                    || serie.data[i].highlighted
                    || serie.highlighted;

                if (serie.show)
                {
                    Color areaColor = serie.GetAreaColor(m_ThemeInfo, colorIndex, highlight);
                    Color areaToColor = serie.GetAreaToColor(m_ThemeInfo, colorIndex, highlight);
                    ChartDrawer.DrawPolygon(vh, p4, p1, p2, p3, areaColor, areaToColor);
                }
            }
            if (!m_Series.IsStack(serie.stack, SerieType.Bar))
            {
                m_BarLastOffset += barGapWidth;
            }
        }

        private float GetBarGap()
        {
            float gap = 0.3f;
            for (int i = 0; i < m_Series.Count; i++)
            {
                var serie = m_Series.list[i];
                if (serie.type == SerieType.Bar)
                {
                    if (serie.barGap != 0)
                    {
                        gap = serie.barGap;
                    }
                }
            }
            return gap;
        }


        private HashSet<string> barStackSet = new HashSet<string>();
        private float GetBarTotalWidth(float categoryWidth, float gap)
        {
            float total = 0;
            float lastGap = 0;
            barStackSet.Clear();
            for (int i = 0; i < m_Series.Count; i++)
            {
                var serie = m_Series.list[i];
                if (serie.type == SerieType.Bar && serie.show)
                {
                    if (!string.IsNullOrEmpty(serie.stack))
                    {
                        if (barStackSet.Contains(serie.stack)) continue;
                        barStackSet.Add(serie.stack);
                    }
                    var width = GetStackBarWidth(categoryWidth, serie);
                    if (gap == -1)
                    {
                        if (width > total) total = width;
                    }
                    else
                    {
                        lastGap = width * gap;
                        total += width;
                        total += lastGap;
                    }
                }
            }
            if (total > 0 && gap != -1) total -= lastGap;
            return total;
        }

        private float GetStackBarWidth(float categoryWidth, Serie now)
        {
            if (string.IsNullOrEmpty(now.stack)) return now.GetBarWidth(categoryWidth);
            float barWidth = 0;
            for (int i = 0; i < m_Series.Count; i++)
            {
                var serie = m_Series.list[i];
                if (serie.type == SerieType.Bar && serie.show && now.stack.Equals(serie.stack))
                {
                    if (serie.barWidth > barWidth) barWidth = serie.barWidth;
                }
            }
            if (barWidth > 1) return barWidth;
            else return barWidth * categoryWidth;
        }
    }
}