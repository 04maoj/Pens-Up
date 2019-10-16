using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Advanced.Algorithms.Geometry;
using Advanced.Algorithms.DataStructures;
public class Alphabate_manager : MonoBehaviour
{
    //preset
    [SerializeField] int stroke_number;
    [SerializeField] int total_score;
    [SerializeField] string strokeName;
    [SerializeField] int double_write_penalities = 3;
    public AudioClip sound;
    List<int> childrens;
    private HashSet<int> visite_stroke;
    private Stack<int> traverse_order;
    private List<int>[] strokes_and_hit_box;
    private List<int>[] connections_points;
    private bool finished;
    int expected_stroke;
    int current_stroke;
    List<Line> lines;
    List<Line> lines_1;
    List<Point> points;
    List<Point> points_1;
    int c = 0;
    private void Start()
    {
        Reset();
    }
    public string Get_Strok_Name()
    {
        return strokeName;
    }
    //exit status 0 means fine, 1 means normal missing stoke, 2 means missing connection points
    public int remove_Hit(HashSet<int> to_be_delete, int stroke_number, List<Tuple<float, float>> input)
    {
        traverse_order.Clear();
        List<int> get_back = new List<int>();
        foreach (int to_dete in to_be_delete)
        {
            if (childrens.Contains(to_dete))
            {
                childrens.Remove(to_dete);
                traverse_order.Push(to_dete);
                get_back.Add(to_dete);
            }
        }
        if (traverse_order.Count == 0)
        {
            for (int j = 0; j < get_back.Count; j++)
            {
                childrens.Add(get_back[j]);
            }
            return 1;
        }
        for (int i = strokes_and_hit_box[stroke_number].Count - 1; i > -1; i--)
        {
            if (traverse_order.Count == 0)
            {
                for (int j = 0; j < get_back.Count; j++)
                {
                    childrens.Add(get_back[j]);
                }
                return 1;
            }
            int u = traverse_order.Pop();
            Debug.Log(u + "   " + strokes_and_hit_box[stroke_number][i]);
            if (u != strokes_and_hit_box[stroke_number][i])
            {
                for (int j = 0; j < get_back.Count; j++)
                {
                    childrens.Add(get_back[j]);
                }
                return 1;
            }
        }
        if(current_stroke == 0)
        {
            insertLines(input, current_stroke);
        }
        else
        {
            lines_1.Clear();
            foreach (var temp in lines)
            {
                lines_1.Add(temp);
            }
            points_1.Clear();
            foreach (var point in points)
            {
                points_1.Add(point);
            }
            insertLines(input, current_stroke);
            bool poss = CheckIntersections(current_stroke, connections_points[current_stroke]);
            if (!CheckIntersections(current_stroke, connections_points[current_stroke]))
            {
                lines.Clear();
                foreach (var temp in lines_1)
                {
                    lines.Add(temp);
                }
                points.Clear();
                foreach (var point in points_1)
                {
                    points.Add(point);
                }
                for (int j = 0; j < get_back.Count; j++)
                {
                    childrens.Add(get_back[j]);
                }
                return 2;
            }
        }
        current_stroke++;
        if (childrens.Count == 0 && !finished)
        {

            FindObjectOfType<Scence_Manager>().Decrement_total_Character(strokeName);
            FindObjectOfType<Track_manager>().Finished_one();
            if (transform.parent.GetComponent<Word>() == null)
                FindObjectOfType<AudioSource>().PlayOneShot(sound);
            else
                transform.parent.GetComponent<Word>().Decrement();
            finished = true;
            return 0;
        } 
        return 0;
    }
    public bool finish()
    {
        return finished;
    }
    public bool Increment_Stroke(int stroke)
    {
        if (stroke == expected_stroke) {
            expected_stroke++;
            return true;
        }
        return false;
    }
    public void Reset()
    {
        lines = new List<Line>();
        lines_1 = new List<Line>();
        total_score = 0;
        finished = false;
        childrens = new List<int>();
        current_stroke = 0;
        visite_stroke = new HashSet<int>();
        strokes_and_hit_box = new List<int>[stroke_number];
        traverse_order = new Stack<int>();
        connections_points = new List<int>[stroke_number];
        points = new List<Point>();
        points_1 = new List<Point>();
        for (int i = 0; i < transform.childCount; i++)
        {
            for (int j = 0; j < transform.GetChild(i).GetComponent<Hit_Box>().stroke_number.Count; j++) {
                childrens.Add(transform.GetChild(i).GetComponent<Hit_Box>().index);
            }
        }
        expected_stroke = 0;
        if (strokeName == "A")
        {
            strokes_and_hit_box[0] = new List<int> { 0, 1, 2, 3, };
            strokes_and_hit_box[1] = new List<int> { 0, 4, 5, 6 };
            strokes_and_hit_box[2] = new List<int> { 2, 7,5 };
            connections_points[1] = new List<int> {0};
            connections_points[2] = new List<int> { 0,1 };
        }
        else if (strokeName == "a")
        {
            strokes_and_hit_box[0] = new List<int> { 0, 1, 2, 3, 4 };
        }
        else if (strokeName == "B")
        {
            strokes_and_hit_box[0] = new List<int> { 0, 1, 2, 3, 4 };
            strokes_and_hit_box[1] = new List<int> { 0, 5, 6, 7, 8, 9, 2 };
            strokes_and_hit_box[2] = new List<int> { 2, 9, 10, 11, 12, 13, 4 };
            connections_points[1] = new List<int> {0};
            connections_points[2] = new List<int> {0};
        }
        else if (strokeName == "C")
        {
            strokes_and_hit_box[0] = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 };
        }
        else if (strokeName == "c")
        {
            strokes_and_hit_box[0] = new List<int> { 0, 1, 2, 3, 4 };
        }
        else if (strokeName == "b")
        {
            strokes_and_hit_box[0] = new List<int> { 0, 1, 2, 3 };
            strokes_and_hit_box[1] = new List<int> { 1, 4, 5, 6, 7, 8, 3 };
            connections_points[1] = new List<int> { 0 };
        } else if (strokeName == "n") {
            strokes_and_hit_box[0] = new List<int> { 0, 1, 2 };
            strokes_and_hit_box[1] = new List<int> { 0, 3, 4, 5, 6 };
            connections_points[1] = new List<int> { 0 };
        } else if (strokeName == "t")
        {
            strokes_and_hit_box[0] = new List<int> { 0, 1, 2 };
            strokes_and_hit_box[1] = new List<int> { 4, 1, 5, 6 };
            connections_points[1] = new List<int> { 0 };
        } else if (strokeName == "p")
        {
            strokes_and_hit_box[0] = new List<int> { 0, 1, 2, 3, 4 };
            strokes_and_hit_box[1] = new List<int> { 0, 5, 6, 7, 2 };
            connections_points[1] = new List<int> { 0 };
        }
        else if (strokeName == "l")
        {
            strokes_and_hit_box[0] = new List<int> { 0, 1, 2, 3, 4 };
        }
        else if (strokeName == "e")
        {
            strokes_and_hit_box[0] = new List<int> { 0, 1, 2, 3, 4, 5, 6 };
        }
        else if (strokeName == "u")
        {
            strokes_and_hit_box[0] = new List<int> { 0, 1, 2, 3, 4 };
        } else if (strokeName == "y")
        {
            strokes_and_hit_box[0] = new List<int> { 0, 1, 2, 3 };
            strokes_and_hit_box[1] = new List<int> { 4, 5, 3, 6, 7 };
            connections_points[1] = new List<int> {0};
        } else if (strokeName == "g")
        {
            strokes_and_hit_box[0] = new List<int> { 0, 1, 2, 3, 4, 5 };
            strokes_and_hit_box[1] = new List<int> { 0, 6, 5, 7, 8 };
            connections_points[1] = new List<int> { 0};
        }
        else if (strokeName == "o")
        {
            strokes_and_hit_box[0] = new List<int> { 0, 1, 2, 3, 4, 5 };
        } else if (strokeName == "r")
        {
            strokes_and_hit_box[0] = new List<int> { 0, 1, 2, 3 };
            strokes_and_hit_box[1] = new List<int> { 0, 4 };
            connections_points[1] = new List<int> { 0 };
        } else if (strokeName == "F")
        {
            strokes_and_hit_box[0] = new List<int> { 0, 1, 2 };
            strokes_and_hit_box[1] = new List<int> { 0, 3 };
            strokes_and_hit_box[2] = new List<int> { 0, 4};
            connections_points[1] = new List<int> { 0 };
            connections_points[2] = new List<int> { 0 };
        } else if(strokeName == "D")
        {
            strokes_and_hit_box[0] = new List<int> { 0, 1, 2};
            strokes_and_hit_box[1] = new List<int> { 0, 3, 4,2 };
            connections_points[1] = new List<int> { 0 };
        } else if(strokeName == "d")
        {
            strokes_and_hit_box[0] = new List<int> { 0, 1 };
            strokes_and_hit_box[1] = new List<int> {2,1};
            connections_points[1] = new List<int> { 0 };
        }
        else if (strokeName == "E")
        {
            strokes_and_hit_box[0] = new List<int> { 0, 1 ,2 };
            strokes_and_hit_box[1] = new List<int> { 0, 3 };
            strokes_and_hit_box[2] = new List<int> { 1, 4 };
            strokes_and_hit_box[3] = new List<int> { 2, 5 };
            connections_points[1] = new List<int> { 0 };
            connections_points[2] = new List<int> { 0 };
            connections_points[3] = new List<int> { 0 };
        } else if(strokeName == "f")
        {
            strokes_and_hit_box[0] = new List<int> { 0, 1, 2 };
            strokes_and_hit_box[1] = new List<int> { 1 };
            connections_points[1] = new List<int> { 0 };
        }
        else if (strokeName == "G")
        {
            strokes_and_hit_box[0] = new List<int> { 0, 1, 2,3 };
            strokes_and_hit_box[1] = new List<int> { 3};
            connections_points[1] = new List<int> { 0 };
        }
        else if (strokeName == "H")
        {
            strokes_and_hit_box[0] = new List<int> { 0, 1, 2 };
            strokes_and_hit_box[1] = new List<int> { 3,4,5 };
            strokes_and_hit_box[2] = new List<int> {1, 6,4 };
            connections_points[1] = new List<int> { 2 };
            connections_points[2] = new List<int> { 0,1 };
        }
        else if (strokeName == "h")
        {
            strokes_and_hit_box[0] = new List<int> { 0, 1, 2};
            strokes_and_hit_box[1] = new List<int> { 1,3,4,5 };
            connections_points[1] = new List<int> { 0 };
        }

    }
    public Assessment Get_Assessment()
    {
        return gameObject.GetComponent<Assessment>();
    }
    //
    public void insertLines(List<Tuple<float, float>> input,int num)
    {
        for(int i = 1; i < input.Count; i ++)
        {
            //Debug.Log(input[i - 1].Item1 + "  " + input[i - 1].Item2);
            //Debug.Log(input[i].Item1 + "  " + input[i].Item2);
            Line temp = new Line(new Point(input[i - 1].Item1, input[i - 1].Item2), new Point(input[i].Item1, input[i].Item2), 5, num);
            lines.Add(temp);
            temp.Left.line = lines[lines.Count - 1];
            //temp.Left.isIntersect = false;
            points.Add(lines[lines.Count - 1].Left);
            temp.Right.line = lines[lines.Count - 1];
            //temp.Right.isLeft = false;
            //temp.Right.isIntersect = false;
            points.Add(lines[lines.Count - 1].Right);
            //Debug.Log(lines[lines.Count - 1].Right.line.stroke_number);
        }
    }
    public bool CheckIntersections (int line1, List<int> second) {
        int t_c = 0;
        //var bentleyOttmannAlgorithm = new BentleyOttmann();
        //var actualIntersections = bentleyOttmannAlgorithm.FindIntersections(lines);
        ////foreach(Line t in lines)
        ////{
        ////    Debug.Log(t.stroke_number);
        ////}
        //int q = 0;
        //foreach (var temp in actualIntersections) {
        //    foreach(var a in temp.Value)
        //    {
        //        Debug.Log(q + " "+a.stroke_number);

        //    }
        //    q++;
        //}

        //AVLTree<Line> status = new AVLTree<Line>();
        //PriorityQueue<Point> Q = new PriorityQueue<Point>();
        //foreach(Point t in points) {
        //    Q.Enqueue(t);
        //}
        //while(Q.Peek() != null)
        //{
        //    Point ptr = Q.Dequeue();
        //    if(ptr.isLeft && !ptr.isIntersect)
        //    {
        //        status.Insert(ptr.line);
        //        if(status.NextLower(ptr.line)!= null)
        //        {
        //            Point temp = LineIntersection.FindIntersection(status.NextLower(ptr.line), ptr.line, 5);
        //            if (temp != null)
        //            {
        //                temp.intersectLine = status.NextLower(ptr.line);
        //                temp.isIntersect = true;
        //                Q.Enqueue(temp);
        //            }
        //        }
        //        if(status.NextHigher(ptr.line) != null)
        //        {
        //            Point temp = LineIntersection.FindIntersection(status.NextHigher(ptr.line), ptr.line, 5);
        //            if (temp != null)
        //            {
        //                temp.intersectLine = status.NextHigher(ptr.line);
        //                temp.isIntersect = true;
        //                Q.Enqueue(temp);
        //            }
        //        }
        //    } else if(!ptr.isLeft && !ptr.isIntersect) {
        //        if(status.NextHigher(ptr.line) != null && status.NextLower(ptr.line) != null)
        //        {
        //            Point temp = LineIntersection.FindIntersection(status.NextHigher(ptr.line), status.NextLower(ptr.line), 5);
        //            if (temp != null)
        //            {
        //                temp.line = status.NextHigher(ptr.line);
        //                temp.intersectLine = status.NextLower(ptr.line);
        //                temp.isIntersect = true;
        //                Q.Enqueue(temp);
        //            }
        //        }
        //        status.Delete(ptr.line);
        //    }
        //    else
        //    {
        //        status.Swap(ptr.line, ptr.intersectLine);
        //        Line l1, l2;
        //        if(ptr.line < ptr.intersectLine)
        //        {
        //            l1 = ptr.line;
        //        }

        //        if (status.NextLower(ptr.intersectLine) != null)
        //        {
        //            Point temp = LineIntersection.FindIntersection(ptr.intersectLine, status.NextLower(ptr.intersectLine), 5);
        //            if (temp != null)
        //            {
        //                temp.line = ptr.intersectLine;
        //                temp.intersectLine = status.NextLower(ptr.intersectLine);
        //                temp.isIntersect = true;
        //                Q.Enqueue(temp);
        //            }
        //        }

        //    }
        //}
        foreach (Line l1 in lines)
        {
            foreach (Line l2 in lines)
            {
                if (l1 != l2)
                {
                    if (LineIntersection.FindIntersection(l1, l2, 5) != null)
                    {
                        int u = Math.Min(l1.stroke_number, l2.stroke_number);
                        int v = Math.Max(l1.stroke_number, l2.stroke_number);
                        //Debug.Log(u + " " + v);
                        for (int p = 0; p < second.Count; p++)
                        {
                            int x = Math.Min(line1, second[p]);
                            int y = Math.Max(line1, second[p]);
                            if (x == u && v == y)
                            {
                                t_c++;
                            }
                        }
                        if (t_c >= second.Count)
                            return true;

                    }
                }
            }
        }
        //}
        //    if (ptr.isLeft)
        //    {
        //        T.Insert(ptr.line);
        //        if(LineIntersection.FindIntersection(T.NextHigher(ptr.line), ptr.line,5) != null) {
        //            int u = Math.Min(ptr.line.stroke_number, T.NextHigher(ptr.line).stroke_number);
        //            int v = Math.Max(ptr.line.stroke_number, T.NextHigher(ptr.line).stroke_number);
        //            Debug.Log(u + " " + v);
        //            for (int p = 0; p < second.Count; p++)
        //            {
        //                int x = Math.Min(line1, second[p]);
        //                int y = Math.Max(line1, second[p]);
        //                if (x == u && v == y)
        //                {
        //                    t_c++;
        //                }
        //            }
        //            if (t_c >= second.Count)
        //                return true;

        //        }
        //        if (LineIntersection.FindIntersection(T.NextLower(ptr.line), ptr.line, 5) != null)
        //        {
        //            int u = Math.Min(ptr.line.stroke_number, T.NextLower(ptr.line).stroke_number);
        //            int v = Math.Max(ptr.line.stroke_number, T.NextLower(ptr.line).stroke_number);
        //            Debug.Log(u + " " + v);
        //            for (int p = 0; p < second.Count; p++)
        //            {
        //                int x = Math.Min(line1, second[p]);
        //                int y = Math.Max(line1, second[p]);
        //                if (x == u && v == y)
        //                {

        //                    t_c++;
        //                }
        //            }
        //            if (t_c >= second.Count)
        //                return true;
        //        }
        //    }
        //    else
        //    {
        //        if (LineIntersection.FindIntersection(T.NextLower(ptr.line), T.NextHigher(ptr.line), 5) != null)
        //        {
        //            int u = Math.Min(T.NextLower(ptr.line).stroke_number, T.NextHigher(ptr.line).stroke_number);
        //            int v = Math.Max(T.NextLower(ptr.line).stroke_number, T.NextHigher(ptr.line).stroke_number);
        //            for (int p = 0; p < second.Count; p++)
        //            {
        //                int x = Math.Min(line1, second[p]);
        //                int y = Math.Max(line1, second[p]);
        //                if (x == u && v == y)
        //                {

        //                    t_c++;
        //                }
        //            }
        //            if (t_c >= second.Count)
        //                return true;
        //        }
        //        T.Delete(ptr.line);
        //    }
        //}
        return false;
    }
}
