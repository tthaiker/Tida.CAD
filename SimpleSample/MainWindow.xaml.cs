using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tida.Canvas.Contracts;
using Tida.Canvas.Infrastructure.DrawObjects;
using Tida.Geometry.Primitives;

namespace SimpleSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            canvasControl.Layers = new CanvasLayer[]
            {
                _canvasLayer = new CanvasLayer()
            };
        }

        private readonly CanvasLayer _canvasLayer;

        private void AddLine_Click(object sender, RoutedEventArgs e)
        {
            var line = new Tida.Canvas.Infrastructure.DrawObjects.Line(new Vector2D(-3,3),new Vector2D(3,-3));
            _canvasLayer.AddDrawObject(line);
        }

        private void AddCustomObject_Click(object sender, RoutedEventArgs e)
        {
            var customObject = new CustomObject();
            _canvasLayer.AddDrawObject(customObject);
        }

        private void RemovedSelected_Click(object sender, RoutedEventArgs e)
        {
            _canvasLayer.RemoveDrawObjects(_canvasLayer.DrawObjects.Where(p => p.IsSelected).ToArray());
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            _canvasLayer.Clear();
        }

        private void CreateDim_Click(object sender, RoutedEventArgs e)
        {
            {
                LineDimInfoLst list = new LineDimInfoLst();

                {
                    LineDimInfo lineDimInfo = new LineDimInfo();
                    lineDimInfo.Text = "梯段宽\n100";
                    lineDimInfo.PtList.Add(new Vector2D(0, 3));
                    lineDimInfo.PtList.Add(new Vector2D(2, 3));
                    list.Add(lineDimInfo);
                }

                {
                    LineDimInfo lineDimInfo = new LineDimInfo();
                    lineDimInfo.Text = "梯井\n50";
                    lineDimInfo.PtList.Add(new Vector2D(2, 3));
                    lineDimInfo.PtList.Add(new Vector2D(3, 3));
                    list.Add(lineDimInfo);
                }

                {
                    LineDimInfo lineDimInfo = new LineDimInfo();
                    lineDimInfo.Text = "梯段宽\n200";
                    lineDimInfo.PtList.Add(new Vector2D(3, 3));
                    lineDimInfo.PtList.Add(new Vector2D(6, 3));
                    list.Add(lineDimInfo);
                }

                var customObject = new LineDimObject(list, 0.1);
                _canvasLayer.AddDrawObject(customObject);

            }

            //{
            //    LineDimInfoLst list = new LineDimInfoLst();

            //    {
            //        LineDimInfo lineDimInfo = new LineDimInfo();
            //        lineDimInfo.Text = "梯段宽\n100";
            //        lineDimInfo.PtList.Add(new Vector2D(0, 0));
            //        lineDimInfo.PtList.Add(new Vector2D(3, 3));
            //        list.Add(lineDimInfo);
            //    }

            //    {
            //        LineDimInfo lineDimInfo = new LineDimInfo();
            //        lineDimInfo.Text = "梯井\n50";
            //        lineDimInfo.PtList.Add(new Vector2D(3, 3));
            //        lineDimInfo.PtList.Add(new Vector2D(5, 5));
            //        list.Add(lineDimInfo);
            //    }

            //    {
            //        LineDimInfo lineDimInfo = new LineDimInfo();
            //        lineDimInfo.Text = "梯段宽\n200";
            //        lineDimInfo.PtList.Add(new Vector2D(5, 5));
            //        lineDimInfo.PtList.Add(new Vector2D(7, 7));
            //        list.Add(lineDimInfo);
            //    }

            //    var customObject = new LineDimObject(list, 0.1);
            //    _canvasLayer.AddDrawObject(customObject);

            //}

            {
                LineDimInfoLst list = new LineDimInfoLst();





                {
                    LineDimInfo lineDimInfo = new LineDimInfo();
                    lineDimInfo.Text = "梯段宽\n200";

                    lineDimInfo.PtList.Add(new Vector2D(7, 7));
                    lineDimInfo.PtList.Add(new Vector2D(5, 5));
                    list.Add(lineDimInfo);
                }

                {
                    LineDimInfo lineDimInfo = new LineDimInfo();
                    lineDimInfo.Text = "梯井\n50";

                    lineDimInfo.PtList.Add(new Vector2D(5, 5));
                    lineDimInfo.PtList.Add(new Vector2D(3, 3));
                    list.Add(lineDimInfo);
                }

                {
                    LineDimInfo lineDimInfo = new LineDimInfo();
                    lineDimInfo.Text = "梯段宽\n100";

                    lineDimInfo.PtList.Add(new Vector2D(3, 3));
                    lineDimInfo.PtList.Add(new Vector2D(0, 0));
                    list.Add(lineDimInfo);
                }

                var customObject = new LineDimObject(list, 0.1);
                _canvasLayer.AddDrawObject(customObject);

                
            }

            {
                Vector2D start = new Vector2D(-100, -100);
                Vector2D end = new Vector2D(-3, -3);
                FragmentDimObject fragmentDimObject = new FragmentDimObject(start, end);
                _canvasLayer.AddDrawObject(fragmentDimObject);
            }

            this.canvasControl.ZoomToFit();

        }
    }
}
