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
    /// <summary>
    /// 用于在Canvas上绘制图形的类
    /// </summary>
    public static class CanvasDrawing
    {
        /// <summary>
        /// 绘制一条直线，粗细为1，颜色为黑色
        /// </summary>
        /// <param name="_canvas">Canvas控件的名称</param>
        /// <param name="_startX">起点的X坐标</param>
        /// <param name="_startY">起点的Y坐标</param>
        /// <param name="_endX">终点的X坐标</param>
        /// <param name="_endY">终点的Y坐标</param>
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

        /// <summary>
        /// 绘制一条直线，并指定粗细和颜色
        /// </summary>
        /// <param name="_canvas">Canvas控件的名称</param>
        /// <param name="_startX">起点的X坐标</param>
        /// <param name="_startY">起点的Y坐标</param>
        /// <param name="_endX">终点的X坐标</param>
        /// <param name="_endY">终点的Y坐标</param>
        /// <param name="_thickness">直线的线宽</param>
        /// <param name="_brush">直线的画刷颜色</param>
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

        /// <summary>
        /// 绘制一条多段线，粗细为1，颜色为黑色
        /// </summary>
        /// <param name="_canas">Canvas控件的名称</param>
        /// <param name="_XList">直线端点X坐标列表</param>
        /// <param name="_YList">直线端点Y坐标列表</param>
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

        /// <summary>
        /// 绘制一条多段线，并指定粗细和颜色
        /// </summary>
        /// <param name="_canas">Canvas控件的名称</param>
        /// <param name="_XList">直线端点X坐标列表</param>
        /// <param name="_YList">直线端点Y坐标列表</param>
        /// <param name="_thickness">多段线的线宽</param>
        /// <param name="_brush">多段线的画刷颜色</param>
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

        /// <summary>
        /// 绘制一条虚线，粗细为1，颜色为黑色
        /// </summary>
        /// <param name="_canvas">Canvas控件的名称</param>
        /// <param name="_startX">起点的X坐标</param>
        /// <param name="_startY">起点的Y坐标</param>
        /// <param name="_endX">终点的X坐标</param>
        /// <param name="_endY">终点的Y坐标</param>
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

        /// <summary>
        /// 绘制一条虚线，并指定颜色和粗细
        /// </summary>
        /// <param name="_canvas">Canvas控件的名称</param>
        /// <param name="_startX">起点的X坐标</param>
        /// <param name="_startY">起点的Y坐标</param>
        /// <param name="_endX">终点的X坐标</param>
        /// <param name="_endY">终点的Y坐标</param>
        /// <param name="_thickness">虚线的线宽</param>
        /// <param name="_brush">虚线的画刷颜色</param>
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

        /// <summary>
        /// 绘制一个圆，粗细为1，颜色为紫色，并指定是实心或空心
        /// </summary>
        /// <param name="_canvas">Canvas控件的名称</param>
        /// <param name="_X">圆心的X坐标</param>
        /// <param name="_Y">圆心的Y坐标</param>
        /// <param name="_D">圆心的直径</param>
        /// <param name="_isFill">是否为实心，true为实心</param>
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

        /// <summary>
        /// 绘制一个圆，指定粗细和颜色，并指定是实心或空心
        /// </summary>
        /// <param name="_canvas">Canvas控件的名称</param>
        /// <param name="_X">圆心的X坐标</param>
        /// <param name="_Y">圆心的Y坐标</param>
        /// <param name="_D">圆心的直径</param>
        /// <param name="_isFill">是否为实心，true为实心</param>
        /// <param name="_thickness">圆的线宽</param>
        /// <param name="_brush">圆的画刷颜色</param>
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

        /// <summary>
        /// 绘制一个文字，字体大小为11，颜色为黑色，不加粗，非斜体，左对齐
        /// </summary>
        /// <param name="_canvas">Canvas控件的名称</param>
        /// <param name="_X">左上角点的X坐标</param>
        /// <param name="_Y">左上角点的Y坐标</param>
        /// <param name="_text">文本内容</param>
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

        /// <summary>
        /// 绘制一个文字，并指定字体大小，颜色，是否加粗，是否斜体，对齐方式
        /// </summary>
        /// <param name="_canvas">Canvas控件的名称</param>
        /// <param name="_X">左上角点的X坐标</param>
        /// <param name="_Y">左上角点的Y坐标</param>
        /// <param name="_text">文本内容</param>
        /// <param name="_fontSize">字体大小</param>
        /// <param name="_brush">字体颜色</param>
        /// <param name="_isBold">是否加粗</param>
        /// <param name="_isItalic">是否为斜体</param>
        /// <param name="_isLeft">是否为左对齐，true为左对齐</param>
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

        /// <summary>
        /// 绘制一个竖向文字，字体大小为11，颜色为黑色，不加粗，非斜体，左对齐，设置旋转
        /// </summary>
        /// <param name="_canvas">Canvas控件的名称</param>
        /// <param name="_X">左上角点的X坐标</param>
        /// <param name="_Y">左上角点的Y坐标</param>
        /// <param name="_text">文本内容</param>
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
