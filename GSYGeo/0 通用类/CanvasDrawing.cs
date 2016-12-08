using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace GSYGeo
{
    // 用于在Canvas上绘制图形的类
    public static class CanvasDrawing
    {
        // 绘制一条直线，粗细为1，颜色为黑色
        public static void DrawLine(this Canvas _canvas,double _startX, double _startY, double _endX,double _endY)
        {
            LineGeometry myLineGeometry = new LineGeometry();
            myLineGeometry.StartPoint = new System.Windows.Point(_startX, _startY);
            myLineGeometry.EndPoint = new System.Windows.Point(_endX, _endY);

            Path myPath = new Path();
            myPath.Stroke = Brushes.Black;
            myPath.StrokeThickness = 1;
            myPath.Data = myLineGeometry;

            _canvas.Children.Add(myPath);
        }

        // 绘制一条直线，并指定粗细和颜色
        public static void DrawLine(this Canvas _canvas,double _startX, double _startY, double _endX,double _endY,double _thickness,Brush _brush)
        {
            LineGeometry myLineGeometry = new LineGeometry();
            myLineGeometry.StartPoint = new System.Windows.Point(_startX, _startY);
            myLineGeometry.EndPoint = new System.Windows.Point(_endX, _endY);

            Path myPath = new Path();
            myPath.Stroke = _brush;
            myPath.StrokeThickness = _thickness;
            myPath.Data = myLineGeometry;

            _canvas.Children.Add(myPath);
        }

        // 绘制一条多段线，粗细为1，颜色为黑色
        public static void DrawPLine(this Canvas _canas,List<double> _XList, List<double> _YList)
        {
            Path myPath = new Path();
            myPath.StrokeThickness = 1;
            myPath.Stroke = Brushes.Black;

            StreamGeometry myStreamGeometry = new StreamGeometry();
            myStreamGeometry.FillRule = FillRule.EvenOdd;

            using(StreamGeometryContext ctx = myStreamGeometry.Open())
            {
                ctx.BeginFigure(new System.Windows.Point(_XList[0], _YList[0]), true, false);
                for(int i = 1; i < _XList.Count; i++)
                {
                    ctx.LineTo(new System.Windows.Point(_XList[i], _YList[i]), true, false);
                }
            }

            myStreamGeometry.Freeze();

            myPath.Data = myStreamGeometry;

            _canas.Children.Add(myPath);
        }

        // 绘制一条多段线，并指定粗细和颜色
        public static void DrawPLine(this Canvas _canas, List<double> _XList, List<double> _YList,double _thickness,Brush _brush)
        {
            Path myPath = new Path();
            myPath.StrokeThickness = _thickness;
            myPath.Stroke = _brush;

            StreamGeometry myStreamGeometry = new StreamGeometry();
            myStreamGeometry.FillRule = FillRule.EvenOdd;

            using (StreamGeometryContext ctx = myStreamGeometry.Open())
            {
                ctx.BeginFigure(new System.Windows.Point(_XList[0], _YList[0]), true, false);
                for (int i = 1; i < _XList.Count; i++)
                {
                    ctx.LineTo(new System.Windows.Point(_XList[i], _YList[i]), true, false);
                }
            }

            myStreamGeometry.Freeze();

            myPath.Data = myStreamGeometry;

            _canas.Children.Add(myPath);
        }

        // 绘制一条虚线，粗细为1，颜色为黑色
        public static void DrawDotLine(this Canvas _canvas,double _startX, double _startY, double _endX,double _endY)
        {
            LineGeometry myLineGeometry = new LineGeometry();
            myLineGeometry.StartPoint = new System.Windows.Point(_startX, _startY);
            myLineGeometry.EndPoint = new System.Windows.Point(_endX, _endY);

            Path myPath = new Path();
            myPath.Stroke = Brushes.Black;
            myPath.StrokeThickness = 1;
            myPath.StrokeDashArray = new DoubleCollection() { 2 };
            myPath.Data = myLineGeometry;

            _canvas.Children.Add(myPath);
        }

        // 绘制一条虚线，并指定颜色和粗细
        public static void DrawDotLine(this Canvas _canvas, double _startX, double _startY, double _endX, double _endY,double _thickness,Brush _brush)
        {
            LineGeometry myLineGeometry = new LineGeometry();
            myLineGeometry.StartPoint = new System.Windows.Point(_startX, _startY);
            myLineGeometry.EndPoint = new System.Windows.Point(_endX, _endY);

            Path myPath = new Path();
            myPath.Stroke = _brush;
            myPath.StrokeThickness = _thickness;
            myPath.StrokeDashArray = new DoubleCollection() { 2 };
            myPath.Data = myLineGeometry;

            _canvas.Children.Add(myPath);
        }

        // 绘制一个圆，粗细为1，颜色为紫色，并指定是实心或空心
        public static void DrawCircle(this Canvas _canvas,double _X,double _Y,double _D,bool _isFill)
        {
            EllipseGeometry myEllipseGeometry = new EllipseGeometry();
            myEllipseGeometry.Center = new System.Windows.Point(_X, _Y);
            myEllipseGeometry.RadiusX = _D;
            myEllipseGeometry.RadiusY = _D;

            Path myPath = new Path();
            myPath.Stroke = Brushes.Purple;
            myPath.StrokeThickness = 1;
            myPath.Fill = _isFill == true ? Brushes.Purple : null;
            myPath.Data = myEllipseGeometry;

            _canvas.Children.Add(myPath);
        }

        // 绘制一个圆，指定粗细和颜色，并指定是实心或空心
        public static void DrawCircle(this Canvas _canvas, double _X, double _Y, double _R, bool _isFill,double _thickness,Brush _brush)
        {
            EllipseGeometry myEllipseGeometry = new EllipseGeometry();
            myEllipseGeometry.Center = new System.Windows.Point(_X, _Y);
            myEllipseGeometry.RadiusX = _R;
            myEllipseGeometry.RadiusY = _R;

            Path myPath = new Path();
            myPath.Stroke = _brush;
            myPath.StrokeThickness = _thickness;
            myPath.Fill = _isFill == true ? _brush : null;
            myPath.Data = myEllipseGeometry;
            
            _canvas.Children.Add(myPath);
        }

        // 绘制一个文字，字体大小为11，颜色为黑色，不加粗，非斜体，左对齐
        public static void DrawText(this Canvas _canvas,double _X,double _Y,string _text)
        {
            TextBlock myTextBlock = new TextBlock();
            myTextBlock.Text = _text;
            myTextBlock.TextAlignment = System.Windows.TextAlignment.Left;
            myTextBlock.FontSize = 11;
            myTextBlock.Foreground = Brushes.Black;

            Canvas.SetLeft(myTextBlock, _X);
            Canvas.SetTop(myTextBlock, _Y);
            _canvas.Children.Add(myTextBlock);
        }

        // 绘制一个文字，并指定字体大小，颜色，是否加粗，是否斜体，对齐方式
        public static void DrawText(this Canvas _canvas,double _X,double _Y,string _text,double _fontSize,Brush _brush,bool _isBold,bool _isItalic,bool _isLeft)
        {
            TextBlock myTextBlock = new TextBlock();
            myTextBlock.Text = _text;
            myTextBlock.TextAlignment = System.Windows.TextAlignment.Left;
            myTextBlock.FontSize = _fontSize;
            myTextBlock.Foreground = _brush;
            myTextBlock.FontWeight = _isBold == true ? System.Windows.FontWeights.Bold : System.Windows.FontWeights.Normal;
            myTextBlock.FontStyle = _isItalic == true ? System.Windows.FontStyles.Italic : System.Windows.FontStyles.Normal;

            if (_isLeft == true)
            {
                Canvas.SetLeft(myTextBlock, _X);
            }
            else
            {
                Canvas.SetRight(myTextBlock, _X);
            }
            Canvas.SetTop(myTextBlock, _Y);
            _canvas.Children.Add(myTextBlock);
        }

        // 绘制一个竖向文字，字体大小为11，颜色为黑色，不加粗，非斜体，左对齐，设置旋转
        public static void DrawVerticalText(this Canvas _canvas, double _X, double _Y, string _text)
        {
            string s = _text;
            _text = "";
            for (int i = 0; i < s.Length; i++)
            {
                _text += s.Substring(i, 1) + "\n";
            }
            TextBlock myTextBlock = new TextBlock();
            myTextBlock.Text = _text;
            myTextBlock.TextAlignment = System.Windows.TextAlignment.Left;
            myTextBlock.FontSize = 11;
            myTextBlock.Foreground = Brushes.Black;
            myTextBlock.TextWrapping = System.Windows.TextWrapping.Wrap;

            Canvas.SetLeft(myTextBlock, _X);
            Canvas.SetTop(myTextBlock, _Y);
            _canvas.Children.Add(myTextBlock);
        }
    }
}
