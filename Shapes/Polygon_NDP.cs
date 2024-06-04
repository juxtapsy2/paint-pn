﻿using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _21110603_Paint.Shapes
{
    public class Polygon_NDP : Shape
    {
        public List<Point> points;

        public Polygon_NDP()
        {
            name = "Polygon";
            points = new List<Point>();
        }

        public Polygon_NDP(Color color)
        {
            name = "Polygon";
            this.color = color;
            points = new List<Point>();
        }

        public override object Clone()
        {
            Polygon_NDP polygon = new Polygon_NDP
            {
                pointHead = pointHead,
                pointTail = pointTail,
                isSelected = isSelected,
                name = name,
                color = color,
                contourWidth = contourWidth,
                isFill = isFill
            };
            points.ForEach(point => polygon.points.Add(point));
            return polygon;
        }

        public override void drawShape(System.Drawing.Graphics g)
        {
            using (GraphicsPath path = graphicsPath)
            {
                if (!isFill)
                {
                    using (Pen pen = new Pen(color, contourWidth))
                    {
                        g.DrawPath(pen, path);
                    }
                }
                else
                {
                    using (Brush brush = new SolidBrush(color))
                    {
                        if (points.Count < 3)
                        {
                            using (Pen pen = new Pen(color, contourWidth))
                            {
                                g.DrawPath(pen, path);
                            }
                        }
                        else
                        {
                            g.FillPath(brush, path);
                        }
                    }
                }
            }
        }

        public override bool isHit(System.Drawing.Point p)
        {
            bool inside = false;
            using (GraphicsPath path = graphicsPath)
            {
                if (!isFill)
                {
                    using (Pen pen = new Pen(color, contourWidth+3))
                    {
                        inside = path.IsOutlineVisible(p, pen);
                    }
                }
                else
                {
                    inside = path.IsVisible(p);
                }
            }
            return inside;
        }

        protected override System.Drawing.Drawing2D.GraphicsPath graphicsPath
        {
            get
            {
                GraphicsPath path = new GraphicsPath();
                if (points.Count < 3)
                {
                    path.AddLine(points[0], points[1]);
                }
                else
                {
                    path.AddPolygon(points.ToArray());
                }

                return path;
            }
        }

        public override void moveShape(Point distance)
        {
            base.moveShape(distance);
            for (int i = 0; i < points.Count; i++)
            {
                points[i] = new Point(points[i].X + distance.X, points[i].Y + distance.Y);
            }
        }

        public override int isHitControlsPoint(Point p)
        {
            for (int i = 0; i < points.Count; i++)
            {
                GraphicsPath path = new GraphicsPath();
                path.AddRectangle(new Rectangle(points[i].X - 4, points[i].Y - 4, 8, 8));

                if (path.IsVisible(p)) return i;
            }
            return -1;
        }

        public override void moveControlPoint(Point pointCurrent, Point previous, int index)
        {
            int deltaX = pointCurrent.X - previous.X;
            int deltaY = pointCurrent.Y - previous.Y;
            points[index] = new Point(points[index].X + deltaX, points[index].Y + deltaY);

        }
    }
}
