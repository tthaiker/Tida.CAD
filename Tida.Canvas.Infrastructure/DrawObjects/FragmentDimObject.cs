using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tida.Canvas.Contracts;
using Tida.Canvas.Media;
using Tida.Geometry.Primitives;

namespace Tida.Canvas.Infrastructure.DrawObjects
{
    public class FragmentDimObject : DrawObject
    {
        private static readonly Pen LinePen = Pen.CreateFrozenPen(Brushes.White, 1.2);

        private Vector2D _startPt;

        private Vector2D _endPt;

        private double _fragmentLen = 6;


        public FragmentDimObject(Vector2D startPt, Vector2D endPt, double fragmentLen = -1)
        {
            this._startPt = startPt;
            this._endPt = endPt;
            if(fragmentLen < 0)
            {
                this._fragmentLen = (endPt - startPt).Modulus() / 15;
            }
            else
            {
                this._fragmentLen = fragmentLen;
            }    
            
        }

        public override DrawObject Clone()
        {
            return new FragmentDimObject(_startPt, _endPt, _fragmentLen);
        }

        public override Rectangle2D2 GetBoundingRect()
        {
            return new Rectangle2D2(new Line2D(_startPt, _endPt), 6);
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

            if (_startPt.IsAlmostEqualTo(_endPt)) return;

            try
            {
                //绘制折断线
                Vector3D vecStart3D = new Vector3D(_startPt.X, _startPt.Y, 0);
                Vector3D vecEnd3D = new Vector3D(_endPt.X, _endPt.Y, 0);
                Vector3D vecMiddlePt = (vecStart3D + vecEnd3D) / 2;

                Vector3D lineDirection = (vecEnd3D - vecStart3D).Normalize();
                Vector3D rotate90 = new Vector3D(0, 0, 1).Cross(lineDirection).Normalize();

                Vector3D firstOffset = vecMiddlePt - lineDirection * _fragmentLen / 2;
                Vector3D secondOffset = vecMiddlePt + lineDirection * _fragmentLen / 2;
                Vector3D firstUpOffset = vecMiddlePt + rotate90 * _fragmentLen / 2;
                Vector3D secondDownOffset = vecMiddlePt - rotate90 * _fragmentLen / 2;

                List<Line2D> list = new List<Line2D>();
                var line1 = new Line2D(vecStart3D.ToVector2D(), firstOffset.ToVector2D());
                var line2 = new Line2D(firstOffset.ToVector2D(), firstUpOffset.ToVector2D());
                var line3 = new Line2D(firstUpOffset.ToVector2D(), secondDownOffset.ToVector2D());
                var line4 = new Line2D(secondDownOffset.ToVector2D(), secondOffset.ToVector2D());
                var line5 = new Line2D(secondOffset.ToVector2D(), vecEnd3D.ToVector2D());
                list.Add(line1);
                list.Add(line2);
                list.Add(line3);
                list.Add(line4);
                list.Add(line5);

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
}
