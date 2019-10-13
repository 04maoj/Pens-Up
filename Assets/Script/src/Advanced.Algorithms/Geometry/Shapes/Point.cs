﻿using System;
using System.Collections;
namespace Advanced.Algorithms.Geometry
{
    /// <summary>
    /// Point object.
    /// </summary>
    public class Point: IComparable
    {
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }
        public bool isLeft;
        public Line line;
        public double X { get; private set; }
        public double Y { get; private set; }

        public override string ToString()
        {
            return X.ToString("F") + " " + Y.ToString("F");
        }

        public Point Clone()
        {
            return new Point(X, Y);
        }
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            Point otherptr = obj as Point;
            return (int)(X - otherptr.X);
        }
    }
}