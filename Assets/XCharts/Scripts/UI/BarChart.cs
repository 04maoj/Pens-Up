using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;

namespace XCharts
{
    [AddComponentMenu("XCharts/BarChart", 14)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class BarChart : CoordinateChart
    {

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            m_Title.text = "BarChart";
            m_Tooltip.type = Tooltip.Type.Shadow;
            RemoveData();
            AddSerie(SerieType.Bar, "serie1");
			// static void WriteString(){
// 				string path = "Assets/Local_DataBase/Students/Jack123/A-CIndivudalPerformance.txt";
// 				StreamWriter writer = new StreamWriter (path, true);
// 				writer.WriteLine(path);
// 				wroter.Close();
// 				AssestDatabase.ImportAsset(path);
// 				TextAsset asset = Resource.Load(path);
// 				Debug.Log(asset.text);
// 			}
			// static void ReadString(){
// 		    string path = "Assets/Local_DataBase/Students/Jack123/A-CIndivudalPerformance.txt";
// 			StreamReader reader = new StreamReader(path);
// 			Debug.Log(reader.ReadToEnd());
// 			reader.Close();
// 		}
			
			
			// for (int i = 0; i < 5; i++)
//             {
//                 AddXAxisData("x" + (i + 1));
//                 AddData(0, Random.Range(10, 90));
//             }
        }
#endif
    }
}
