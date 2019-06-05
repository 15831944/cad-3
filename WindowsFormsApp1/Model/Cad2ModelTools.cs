﻿using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace RegulatoryPlan.Model
{
    public class AutoCad2ModelTools
    {
        public static DbTextModel DbText2Model(DBText dbText)
        {
            DbTextModel dbModel = new DbTextModel();
            dbModel.Height = dbText.Height;
            //dbModel.Position = new System.Drawing.PointF((float)dbText.Position.X, (float)dbText.Position.Y);
            dbModel.Position = Point3d2Pointf(dbText.Position);
            dbModel.Rotation = dbText.Rotation;
            dbModel.ThickNess = dbText.Thickness;
            dbModel.Text = dbText.TextString;
            return dbModel;
        }
        public static DbTextModel DbText2Model(MText dbText)
        {
            DbTextModel dbModel = new DbTextModel();
            dbModel.Height = dbText.ActualHeight;
            //dbModel.Position = new System.Drawing.PointF((float)dbText.Position.X, (float)dbText.Position.Y);
            dbModel.Position = Point3d2Pointf(dbText.Location);
            dbModel.Rotation = dbText.Rotation;
      //      dbModel.ThickNess = dbText.TextHeight;
            dbModel.Text = dbText.Text;
            return dbModel;
        }

        public static HatchModel Hatch2Model(Hatch dbText)
        {
            HatchModel dbModel = new HatchModel();
            int cont = dbText.NumberOfLoops;
            for (int i = 0; i < cont; i++)
            {
                HatchLoop loop = dbText.GetLoopAt(i);
                if (loop.IsPolyline)
                {

                    foreach (BulgeVertex vertex in loop.Polyline)
                    {
                        dbModel.loopPoints.Add(Point2d2Pointf(vertex.Vertex));
                    }


                }
                else
                {
                    Curve2dCollection col_cur2d = loop.Curves;
                    foreach (Curve2d item in col_cur2d)
                    {
                        Point2d[] M_point2d = item.GetSamplePoints(4);
                        foreach (Point2d point in M_point2d)
                        {
                            dbModel.loopPoints.Add(Point2d2Pointf(point));
                        }
                    }
                }



               
            }

            dbModel.Area = dbText.Area;
            dbModel.Color = dbText.ColorIndex;
            return dbModel;
        }

        public static LineModel Line2Model(Line line)
        {
            LineModel dbModel = new LineModel();

            dbModel.StartPoint = Point3d2Pointf(line.StartPoint);

            dbModel.EndPoint = Point3d2Pointf(line.EndPoint);

            dbModel.Angle = line.Angle;

            dbModel.Length = line.Length;

            return dbModel;
        }


        public static CircleModel Circle2Model(Circle line)
        {
            CircleModel dbModel = new CircleModel();

           dbModel.Center= Point3d2Pointf(line.Center);

            dbModel.Radius =line.Radius;

            return dbModel;
        }

        public static PolyLineModel Polyline2Model(Autodesk.AutoCAD.DatabaseServices.Polyline polyLine)
        {
            PolyLineModel polylineModel = new PolyLineModel();
            polylineModel.Area = polyLine.Area;
            polylineModel.Closed = polyLine.Closed;
            polylineModel.Vertices = new System.Collections.ArrayList();
            int vn = polyLine.NumberOfVertices;  //lwp已知的多段线
            for (int i = 0; i < vn; i++)
            {
                Point2d pt = polyLine.GetPoint2dAt(i);
                SegmentType st = polyLine.GetSegmentType(i);
                if (st == SegmentType.Arc)
                {
                    ArcModel arc = new ArcModel();
                    CircularArc2d cir = polyLine.GetArcSegment2dAt(i);
                    //  arc.Center = new System.Drawing.PointF((float)cir.Center.X,(float)cir.Center.Y);
                    arc.Center = Point2d2Pointf(cir.Center);
                    arc.Radius = cir.Radius;
                    arc.Startangel = cir.StartAngle;
                    arc.EndAngel = cir.EndAngle;
                    //  arc.StartPoint = new System.Drawing.PointF((float)cir.StartPoint.X, (float)cir.StartPoint.Y);
                    if (cir.HasStartPoint)
                    {
                        arc.StartPoint = Point2d2Pointf(cir.StartPoint);
                    }
                    //  arc.EndPoint = new System.Drawing.PointF((float)cir.EndPoint.X, (float)cir.EndPoint.Y);
                    if (cir.HasEndPoint)
                    {
                        arc.EndPoint = Point2d2Pointf(cir.EndPoint);
                    }
                    polylineModel.Vertices.Add(arc);
                }
                else if (st == SegmentType.Line)
                {
                    LineModel line = new LineModel();
                    LineSegment2d lineSe = polyLine.GetLineSegment2dAt(i);
                    if (lineSe.HasStartPoint)
                    {
                        line.StartPoint = Point2d2Pointf(lineSe.StartPoint);
                    }
                    if (lineSe.HasEndPoint)
                    {
                        line.EndPoint = Point2d2Pointf(lineSe.EndPoint);
                    }
                    if (line.StartPoint.X == line.EndPoint.X && line.StartPoint.Y == line.EndPoint.Y)
                    {
                        line.Angle = 0;
                        line.Length = 0;
                    }
                    else if (line.StartPoint.X == line.EndPoint.X)
                    {
                        line.Angle = 90;

                    }
                    polylineModel.Vertices.Add(line);
                }
            }
            return polylineModel;
        }

        private static System.Drawing.PointF Point2d2Pointf(Point2d point2d)
        {
            return new System.Drawing.PointF((float)point2d.X, (float)point2d.Y);
        }

        private static System.Drawing.PointF Point3d2Pointf(Point3d point2d)
        {
            return new System.Drawing.PointF((float)point2d.X, (float)point2d.Y);
        }
    }
}
