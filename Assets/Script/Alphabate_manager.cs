using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Advanced.Algorithms.Geometry;
public class Alphabate_manager : MonoBehaviour
{
    //preset
    [SerializeField] int stroke_number;
    [SerializeField] int total_score;
    [SerializeField] string strokeName;
    [SerializeField] int double_write_penalities = 3;
    public AudioClip sound;
    public List<int> childrens;
    private HashSet<int> visite_stroke;
    private Stack<int> traverse_order;
    private List<int>[] strokes_and_hit_box;
    private List<int>[] connections_points;
    private bool finished;
    int expected_stroke;
    public int current_stroke;
    List<Line> lines;
    List<Line> lines_1;
    List<Point> points;
    List<Point> points_1;
    List<bool> visited;
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
    public int remove_Hit(List<int> to_be_delete, int stroke_numbers, List<Tuple<float, float>> input)
    {
        traverse_order.Clear();
        List<int> get_back = new List<int>();
        if (strokes_and_hit_box[stroke_numbers].Count != to_be_delete.Count)
        {
            return 1;
        }
        for (int i = strokes_and_hit_box[stroke_numbers].Count - 1; i > -1; i--)
        {
            if (to_be_delete[i] != strokes_and_hit_box[stroke_numbers][i])
                return 1;
        }
        if (current_stroke == 0 || strokeName == "t")
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
        visited[stroke_numbers] = true;
        if (checkFinsih() && !finished)
        {
            Debug.Log(childrens.Count);
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
    private bool checkFinsih()
    {
        foreach (bool temp in visited)
        {
            if (temp == false)
                return false;
        }
        return true;
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
        visited = new List<bool>();
        for(int i = 0; i < stroke_number; i ++)
        {
            visited.Add(false);
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            for (int j = 0; j < transform.GetChild(i).GetComponent<Hit_Box>().stroke_number.Count; j++) {
                childrens.Add(transform.GetChild(i).GetComponent<Hit_Box>().index);
            }
        }
        expected_stroke = 0;
        if (strokeName == "A")
        {
            strokes_and_hit_box[0] = new List<int> { 0, 1, 2 };
            strokes_and_hit_box[1] = new List<int> { 0, 3,4 };
            strokes_and_hit_box[2] = new List<int> { 1,5,3 };
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
            strokes_and_hit_box[0] = new List<int> { 0, 1 };
            strokes_and_hit_box[1] = new List<int> { 2,3,4};
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
            Line temp = new Line(new Point(input[i - 1].Item1+0.1, input[i - 1].Item2+0.1), new Point(input[i].Item1, input[i].Item2));
            temp.stroke_number = num;
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
    internal class Event : IComparable
    {
        public  double x, y;
        public bool isUpper;
        public Line line;
        public bool isIntersect;
        public Line line1;
        public Event(double y, double x,bool isUpper, Line line)
        {
            this.x = x;
            this.y = y;
            this.isUpper = isUpper;
            this.line = line;
            isIntersect = false;
        }
        public Event(Point temp)
        {
            this.x = temp.X;
            this.y = temp.Y;
            this.line = temp.line;
            isIntersect = false;
        }
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            Event otherptr = obj as Event;
            if(otherptr.y == y)
            {
                return (int)(x - otherptr.x);
            }
            else
            {
                return (int)(otherptr.y - y); 
            }

        }
    }
    internal class Status : IComparable
    {
        public double x;
        public Line line;
        public Status(double x, Line line)
        {
            this.x = x;
            this.line = line;
        }
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            Status otherptr = obj as Status;
            if (Math.Abs(line.Left.X - otherptr.line.Left.X) <= 0.00000001 && Math.Abs(line.Right.Y - otherptr.line.Right.Y) <= 0.00000001 && Math.Abs(line.Left.Y - otherptr.line.Left.Y) <= 0.00000001 && Math.Abs(line.Right.X - otherptr.line.Right.X) <= 0.00000001)
                return 0;
            else
                return (int)(x - otherptr.x);
        }

    }
    public double calc_x(Line l, double y)
    {
        double slope = (l.Left.Y - l.Right.Y) / (l.Left.X - l.Right.X);
        double b = l.Left.Y - slope * l.Left.X;
        return (y - b) / slope;
    }
    public bool CheckIntersections (int line1, List<int> second) {
        int t_c = 0;
        var bentleyOttmannAlgorithm = new BentleyOttmann(5);
        var actualIntersections = bentleyOttmannAlgorithm.FindIntersections(lines);
        //foreach(Line t in lines)
        //{
        //    Debug.Log(t.stroke_number);
        //}
        int q = 0;
        foreach (var temp in actualIntersections) {
            //Debug.Log(q + " "+temp.Value[0].stroke_number+" "+ temp.Value[1].stroke_number);
            //q++;
                            int u = Math.Min(temp.Value[0].stroke_number, temp.Value[1].stroke_number);
                            int v = Math.Max(temp.Value[0].stroke_number, temp.Value[1].stroke_number);
            Debug.Log(u + " " + v);
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
        //PriorityQueue<Event> Q = new PriorityQueue<Event>();
        //foreach (Line line in lines)
        //{
        //    Event a = new Event(line.Left);
        //    Event b = new Event(line.Right);
        //    if (a.CompareTo(b) < 0)
        //    {
        //        a.isUpper = true;
        //        b.isUpper = false;
        //    }
        //    else
        //    {
        //        a.isUpper = false;
        //        b.isUpper = true;
        //    }
        //    Q.Enqueue(a);
        //    Q.Enqueue(b);

        //}
        //AVLTree<Status> T = new AVLTree<Status>();
        //while(Q.Peek() != null)
        //{
        //Event ptr = Q.Dequeue();
        //Status temp1 = new Status(calc_x(ptr.line, ptr.y), ptr.line);
        //if (ptr.isUpper && !ptr.isIntersect)
        //{
        //    T.Insert(temp1);
        //    if(T.NextLower(temp1)!= null)
        //    {
        //        Point temp = LineIntersection.FindIntersection(T.NextLower(temp1).line, ptr.line, 5);
        //        if (temp != null)
        //        {
        //            Event temp2 = new Event(temp);
        //            temp2.line = T.NextLower(temp1).line;
        //            temp2.line1 = ptr.line;
        //            temp2.isIntersect = true;
        //            Q.Enqueue(temp2);
        //        }
        //    }
        //    if(T.NextHigher(temp1) != null)
        //    {
        //        Point temp = LineIntersection.FindIntersection(T.NextHigher(temp1).line, ptr.line, 5);
        //        if (temp != null)
        //        {
        //            Event temp2 = new Event(temp);
        //            temp2.line = T.NextLower(temp1).line;
        //            temp2.line1 = ptr.line;
        //            temp2.isIntersect = true;
        //            Q.Enqueue(temp2);
        //        }
        //    }
        //} else if(!ptr.isUpper && !ptr.isIntersect) {
        //    if(T.NextHigher(temp1) != null && T.NextLower(temp1) != null)
        //    {
        //        Point temp = LineIntersection.FindIntersection(T.NextHigher(temp1).line, T.NextLower(temp1).line, 5);
        //        if (temp != null)
        //        {
        //            Event temp2 = new Event(temp);
        //            temp2.line = T.NextLower(temp1).line;
        //            temp2.line1 = ptr.line;
        //            temp2.isIntersect = true;
        //            Q.Enqueue(temp2);
        //        }
        //    }
        //    T.Delete(temp1);
        //}
        //else
        //{
        //    T.Delete(ptr)
        //    Line l1, l2;
        //    if(ptr.line < ptr.intersectLine)
        //    {
        //        l1 = ptr.line;
        //    }

        //    if (status.NextLower(ptr.intersectLine) != null)
        //    {
        //        Point temp = LineIntersection.FindIntersection(ptr.intersectLine, status.NextLower(ptr.intersectLine), 5);
        //        if (temp != null)
        //        {
        //            temp.line = ptr.intersectLine;
        //            temp.intersectLine = status.NextLower(ptr.intersectLine);
        //            temp.isIntersect = true;
        //            Q.Enqueue(temp);
        //        }
        //    }

        //}
        //}
        //foreach (Line l1 in lines1)
        //{
        //    foreach (Line l2 in lines1)
        //    {
        //        if (l1 != l2)
        //        {
        //            if (LineIntersection.FindIntersection(l1, l2, 2) != null)
        //            {
        //                int u = Math.Min(l1.stroke_number, l2.stroke_number);
        //                int v = Math.Max(l1.stroke_number, l2.stroke_number);
        //                Debug.Log(u + " " + v);
        //                //for (int p = 0; p < second.Count; p++)
        //                //{
        //                //    int x = Math.Min(line1, second[p]);
        //                //    int y = Math.Max(line1, second[p]);
        //                //    if (x == u && v == y)
        //                //    {
        //                //        t_c++;
        //                //    }
        //                //}
        //                //if (t_c >= second.Count)
        //                    //return true;

        //            }
        //        }
        //    }
        //}
        ////return true;
        //foreach (Line l1 in lines)
        //{
        //    foreach (Line l2 in lines)
        //    {
        //        if (l1 != l2)
        //        {
        //            if (LineIntersection.FindIntersection(l1, l2, 2) != null)
        //            {
        //                int u = Math.Min(l1.stroke_number, l2.stroke_number);
        //                int v = Math.Max(l1.stroke_number, l2.stroke_number);
        //                Debug.Log(u + " " + v);
        //                for (int p = 0; p < second.Count; p++)
        //                {
        //                    int x = Math.Min(line1, second[p]);
        //                    int y = Math.Max(line1, second[p]);
        //                    if (x == u && v == y)
        //                    {
        //                        t_c++;
        //                    }
        //                }
        //                if (t_c >= second.Count)
        //                    return true;

        //            }
        //        }
        //    }
        //}
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
