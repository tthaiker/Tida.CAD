using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tida.Canvas.Contracts;
using Tida.Canvas.Media;
using Tida.Geometry.Primitives;

namespace Tida.Canvas.Infrastructure.DrawObjects
{
    public class LineDimObject : DrawObject
    {
        private LineDimInfoLst _dimInfoList = new LineDimInfoLst();

        private static readonly Pen LinePen = Pen.CreateFrozenPen(Brushes.White, 1.2);

        private double _scale = 10;

        private double _boundLineLen = 15;

        private double _markLineLen = 6;

        private double ActualBoundLineLen => _scale * _boundLineLen;

        private double ActualMarkLineLen => _scale * _markLineLen;

        public LineDimObject(LineDimInfoLst dimInfoList, double dScale = 10, double dBoundLineLen = 15, double dMarkLineLen = 6)
        {
            _scale = dScale;
            _boundLineLen = dBoundLineLen;
            _markLineLen = dMarkLineLen;

            _dimInfoList = dimInfoList;
        }

        public override DrawObject Clone()
        {
            return new LineDimObject(_dimInfoList, _scale, _boundLineLen, _markLineLen);
        }

        public override Rectangle2D2 GetBoundingRect()
        {
            if ((_dimInfoList?.Count ?? 0) < 1) return Rectangle2D2.CreateEmpty();
            return new Rectangle2D2(new Line2D(_dimInfoList.First().PtList.First(), _dimInfoList.Last().PtList.Last()), _boundLineLen + 12*_scale);
        }

        public override bool ObjectInRectangle(Rectangle2D2 rect, ICanvasScreenConvertable canvasProxy, bool anyPoint)
        {
            return false;
        }

        public override bool PointInObject(Vector2D point, ICanvasScreenConvertable canvasProxy)
        {
            return false;
        }

        public override void Draw(ICanvas canvas, ICanvasScreenConvertable canvasProxy)
        {
            base.Draw(canvas, canvasProxy);

            try
            {
                if ((_dimInfoList?.Count??0) < 1) return;

                List<Line2D> list = new List<Line2D>();

                var firstPt = _dimInfoList.First().PtList.First();
                var endPt = _dimInfoList.Last().PtList.Last();

                var baseLine = new Line2D(firstPt, endPt);
                list.Add(baseLine);

                Line3D line3D = new Line3D(new Vector3D(firstPt.X, firstPt.Y, 0), new Vector3D(endPt.X, endPt.Y, 0));
                Vector3D rotate90 = new Vector3D(0, 0, 1).Cross(line3D.Direction).Normalize();
                if(rotate90.Dot(new Vector3D(0,1,0)) < 0)
                {
                    rotate90 = -rotate90;
                }
                Vector3D rotate45 = rotate90.ApplyMatrix4(Tida.Geometry.Alternation.Matrix4.identity.SetRotate(45, 0, 0, 1)).Normalize();

                double dAngle = rotate90.ToVector2D().AngleTo(Vector2D.BasisY);
                if(rotate90.X < 0)
                {
                    dAngle = - dAngle;
                }

                for (int i = 0; i < _dimInfoList.Count; i++)
                {
                    LineDimInfo dimInfo = _dimInfoList[i];

                    if (dimInfo.PtList.Count < 2) continue;

                    Vector2D startPt2D = dimInfo.PtList.First();
                    Vector3D startPt3D = new Vector3D(startPt2D.X, startPt2D.Y, 0);
                    if (i == 0)
                    {
                        var startTmp = startPt3D + rotate90 * ActualBoundLineLen / 2;
                        var endTmp = startPt3D - rotate90 * ActualBoundLineLen / 2;

                        var boundLine = new Line2D(startTmp.ToVector2D(), endTmp.ToVector2D());
                        list.Add(boundLine);

                        var startTmp3 = startPt3D + rotate45 * ActualMarkLineLen / 2;
                        var endTmp3 = startPt3D - rotate45 * ActualMarkLineLen / 2;
                        var markLine = new Line2D(startTmp3.ToVector2D(), endTmp3.ToVector2D());
                        list.Add(markLine);
                    }

                    Vector2D endPt2D = dimInfo.PtList.Last();
                    Vector3D endPt3D = new Vector3D(endPt2D.X, endPt2D.Y, 0);
                    var startTmp2 = endPt3D + rotate90 * ActualBoundLineLen / 2;
                    var endTmp2 = endPt3D - rotate90 * ActualBoundLineLen / 2;
                    var boundLine2 = new Line2D(startTmp2.ToVector2D(), endTmp2.ToVector2D());
                    list.Add(boundLine2);

                    //45斜线
                    var startTmp4 = endPt3D + rotate45 * ActualMarkLineLen / 2;
                    var endTmp4 = endPt3D - rotate45 * ActualMarkLineLen / 2;
                    var markLine2 = new Line2D(startTmp4.ToVector2D(), endTmp4.ToVector2D());
                    list.Add(markLine2);

                    //绘制文字
                    if (string.IsNullOrEmpty(dimInfo.Text)) continue;

                    Size sz = canvasProxy.GetStringRealSize(dimInfo.Text, 12, "微软雅思");

                    double dWidth = canvasProxy.ToUnit(sz.Width);
                    double dHeight = canvasProxy.ToUnit(sz.Height + 2);

                    Vector2D middlePt = (startPt2D + endPt2D) / 2;
                    canvas.DrawText(dimInfo.Text, 12, Brushes.White, middlePt.Offset(rotate90.ToVector2D() * dHeight).Offset(rotate90.Cross(Vector3D.BasisZ).Normalize().ToVector2D() * -dWidth / 2), dAngle);
                }

                list.ForEach(x =>
                {
                    canvas.DrawLine(LinePen, x);
                });
            }
            catch (Exception ex)
            {
            }
        }
    }

    public class LineDimInfo
    {
        public string Text { get; set; }

        public List<Vector2D> PtList { get; set; } = new List<Vector2D>();
    }

    public class LineDimInfoLst : List<LineDimInfo>
    {

    }
}
