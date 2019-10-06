﻿using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XCharts
{
    /// <summary>
    /// A data item of serie.
    /// 系列中的一个数据项。可存储数据名和1-n维的数据。
    /// </summary>
    [System.Serializable]
    public class SerieData
    {
        [SerializeField] private string m_Name;
        [SerializeField] private bool m_Selected;
        [SerializeField] private bool m_ShowIcon;
        [SerializeField] private Sprite m_IconImage;
        [SerializeField] private Color m_IconColor = Color.white;
        [SerializeField] private float m_IconWidth = 40;
        [SerializeField] private float m_IconHeight = 40;
        [SerializeField] private Vector3 m_IconOffset;

        [SerializeField] private List<float> m_Data = new List<float>();

        private bool m_Show = true;
        private bool m_LabelAutoSize;
        private float m_LabelPaddingLeftRight;
        private float m_LabelPaddingTopBottom;

        public int index { get; set; }
        /// <summary>
        /// the name of data item.
        /// 数据项名称。
        /// </summary>
        public string name { get { return m_Name; } set { m_Name = value; } }
        /// <summary>
        /// 数据项图例名称。当数据项名称不为空时，图例名称即为系列名称；反之则为索引index。
        /// </summary>
        /// <value></value>
        public string legendName { get { return string.IsNullOrEmpty(name) ? ChartCached.IntToStr(index) : name; } }
        /// <summary>
        /// Whether the data item is selected.
        /// 该数据项是否被选中。
        /// </summary>
        public bool selected { get { return m_Selected; } set { m_Selected = value; } }
        /// <summary>
        /// Whether the data icon is show.
        /// 是否显示图标。
        /// </summary>
        public bool showIcon { get { return m_ShowIcon; } set { m_ShowIcon = value; } }
        /// <summary>
        /// The image of icon.
        /// 图标的图片。
        /// </summary>
        public Sprite iconImage { get { return m_IconImage; } set { m_IconImage = value; } }
        /// <summary>
        /// 图标颜色。
        /// </summary>
        public Color iconColor { get { return m_IconColor; } set { m_IconColor = value; } }
        /// <summary>
        /// 图标宽。
        /// </summary>
        public float iconWidth { get { return m_IconWidth; } set { m_IconWidth = value; } }
        /// <summary>
        /// 图标高。
        /// </summary>
        public float iconHeight { get { return m_IconHeight; } set { m_IconHeight = value; } }
        /// <summary>
        /// 图标偏移。
        /// </summary>
        public Vector3 iconOffset { get { return m_IconOffset; } set { m_IconOffset = value; } }

        /// <summary>
        /// An arbitrary dimension data list of data item.
        /// 可指定任意维数的数值列表。
        /// </summary>
        public List<float> data { get { return m_Data; } set { m_Data = value; } }
        /// <summary>
        /// [default:true] Whether the data item is showed.
        /// 该数据项是否要显示。
        /// </summary>
        public bool show { get { return m_Show; } set { m_Show = value; } }
        /// <summary>
        /// Whether the data item is highlighted.
        /// 该数据项是否被高亮，一般由鼠标悬停或图例悬停触发高亮。
        /// </summary>
        public bool highlighted { get; set; }
        /// <summary>
        /// the label of data item.
        /// 该数据项的文本标签。
        /// </summary>
        public Text labelText { get; private set; }
        public RectTransform labelRect { get; private set; }
        /// <summary>
        /// 标志位置。
        /// </summary>
        public Vector3 labelPosition { get; set; }
        /// <summary>
        /// 是否可以显示Label
        /// </summary>
        public bool canShowLabel { get; set; }
        /// <summary>
        /// the maxinum value.
        /// 最大值。
        /// </summary>
        public float max { get { return m_Data.Max(); } }
        /// <summary>
        /// the mininum value.
        /// 最小值。
        /// </summary>
        public float min { get { return m_Data.Min(); } }
        public Image icon { get; private set; }
        public RectTransform iconRect { get; private set; }
        /// <summary>
        /// 关联的gameObject
        /// </summary>
        public GameObject gameObject { get; private set; }
        /// <summary>
        /// 饼图数据项的开始角度（运行时自动计算）
        /// </summary>
        public float pieStartAngle { get; set; }
        /// <summary>
        /// 饼图数据项的结束角度（运行时自动计算）
        /// </summary>
        public float pieToAngle { get; set; }
        /// <summary>
        /// 饼图数据项的一半时的角度（运行时自动计算）
        /// </summary>
        public float pieHalfAngle { get; set; }
        /// <summary>
        /// 饼图数据项的当前角度（运行时自动计算）
        /// </summary>
        public float pieCurrAngle { get; set; }
        /// <summary>
        /// 饼图数据项的内半径
        /// </summary>
        public float pieInsideRadius{get;set;}
        /// <summary>
        /// 饼图数据项的外半径
        /// </summary>
        public float pieOutsideRadius { get; set; }
        /// <summary>
        /// 饼图数据项的偏移半径
        /// </summary>
        public float pieOffsetRadius { get; set; }
        public Vector3 pieOffsetCenter { get; set; }

        public float GetData(int index)
        {
            if (index >= 0 && index < m_Data.Count) return m_Data[index];
            else return 0;
        }

        public void InitLabel(GameObject labelObj, bool autoSize, float paddingLeftRight, float paddingTopBottom)
        {
            gameObject = labelObj;
            m_LabelAutoSize = autoSize;
            m_LabelPaddingLeftRight = paddingLeftRight;
            m_LabelPaddingTopBottom = paddingTopBottom;
            labelText = labelObj.GetComponentInChildren<Text>();
            labelRect = labelText.GetComponent<RectTransform>();
        }

        public void SetLabelActive(bool active)
        {
            if (labelRect)
            {
                ChartHelper.SetActive(labelRect, active);
            }
        }

        public bool SetLabelText(string text)
        {
            if (labelText)
            {
                labelText.text = text;
                if (m_LabelAutoSize)
                {
                    var newSize = new Vector2(labelText.preferredWidth + m_LabelPaddingLeftRight * 2,
                                        labelText.preferredHeight + m_LabelPaddingTopBottom * 2);
                    var sizeChange = newSize.x != labelRect.sizeDelta.x || newSize.y != labelRect.sizeDelta.y;
                    if (sizeChange) labelRect.sizeDelta = newSize;
                    return sizeChange;
                }
            }
            return false;
        }

        public float GetLabelWidth()
        {
            if (labelRect) return labelRect.sizeDelta.x;
            else return 0;
        }

        public float GetLabelHeight()
        {
            if (labelRect) return labelRect.sizeDelta.y;
            return 0;
        }

        public void SetGameObjectPosition(Vector3 position)
        {
            if (gameObject)
            {
                gameObject.transform.localPosition = position;
            }
        }

        public void SetLabelPosition(Vector3 position)
        {
            if (labelRect) labelRect.localPosition = position;
        }

        public void SetIconObj(GameObject iconObj)
        {
            icon = iconObj.GetComponent<Image>();
            iconRect = iconObj.GetComponent<RectTransform>();
            UpdateIcon();
        }

        public void UpdateIcon()
        {
            if (icon == null) return;
            if (m_ShowIcon)
            {
                ChartHelper.SetActive(icon.gameObject, true);
                icon.sprite = m_IconImage;
                icon.color = m_IconColor;
                iconRect.sizeDelta = new Vector2(m_IconWidth, m_IconHeight);
                icon.transform.localPosition = m_IconOffset;
            }
            else
            {
                ChartHelper.SetActive(icon.gameObject, false);
            }
        }
    }
}
