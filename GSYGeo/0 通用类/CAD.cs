using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WW.Cad.IO;
using WW.Cad.Model;
using WW.Cad.Model.Entities;
using WW.Cad.Model.Tables;
using WW.Math;

namespace GSYGeo
{
    /// <summary>
    /// 操作CAD的类
    /// </summary>
    public class CAD
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CAD()
        {
            // 设置为CAD2004格式
            Model = new DxfModel(DxfVersion.Dxf21);
        }

        /// <summary>
        /// 属性，模型空间
        /// </summary>
        public DxfModel Model { get; set; }
        
        /// <summary>
        /// 方法，保存CAD图形
        /// </summary>
        /// <param name="_path">保存路径</param>
        /// <param name="_vportCenter">保存视图的中心坐标</param>
        /// <param name="_vportHeight">保存视图的高度</param>
        public void SaveAsDwg(string _path, double _vportCenterX, double _vportCenterY, double _vportHeight)
        {
            DxfVPort vport = DxfVPort.CreateActiveVPort();

            vport.Center = new Point2D(_vportCenterX, _vportCenterY);
            vport.Height = _vportHeight;
            Model.VPorts.Add(vport);

            DwgWriter.Write(_path, Model);
        }

        /// <summary>
        /// 方法，添加一个文字样式到模型空间
        /// </summary>
        /// <param name="_styleName">文字样式名称</param>
        /// <param name="_fontName">字体文件名称</param>
        /// <param name="_widthFactor">宽度因子</param>
        public DxfTextStyle AddStyle(string _styleName,string _fontName,double _widthFactor)
        {
            DxfTextStyle style = new DxfTextStyle(_styleName, _fontName);
            style.WidthFactor = _widthFactor;
            Model.TextStyles.Add(style);
            return style;
        }

        /// <summary>
        /// 方法，根据名称查找一个文字样式
        /// </summary>
        /// <param name="_styleName">文字样式名称</param>
        /// <returns></returns>
        public DxfTextStyle FindStyle(string _styleName)
        {
            return Model.TextStyles[_styleName];
        }

        /// <summary>
        /// 方法，向模型空间添加一条直线
        /// </summary>
        /// <param name="_model">模型空间</param>
        /// <param name="_startX">起点X坐标</param>
        /// <param name="_startY">起点Y坐标</param>
        /// <param name="_endX">终点X坐标</param>
        /// <param name="_endY">终点Y坐标</param>
        /// <returns></returns>
        public DxfPolyline2D AddLine(double _startX,double _startY,double _endX,double _endY)
        {
            if ((_startX == _endX) && (_startY == _endY))
                return null;

            DxfVertex2D[] pointList = new DxfVertex2D[2]
            {
                new DxfVertex2D(_startX,_startY),
                new DxfVertex2D(_endX,_endY),
            };

            DxfPolyline2D pline = new DxfPolyline2D(pointList);
            pline.Closed = false;

            Model.Entities.Add(pline);
            return pline;
        }

        /// <summary>
        /// 方法，向模型空间添加一条多段线
        /// </summary>
        /// <param name="_model">模型空间</param>
        /// <param name="_points">二维点数组</param>
        /// <returns></returns>
        public DxfPolyline2D AddPline(DxfVertex2D[] _points)
        {
            if (_points.Length == 0)
                return null;

            DxfPolyline2D pline = new DxfPolyline2D(_points);
            pline.Closed = false;

            Model.Entities.Add(pline);
            return pline;
        }

        /// <summary>
        /// 方法，向模型空间添加一个矩形
        /// </summary>
        /// <param name="_model">模型空间</param>
        /// <param name="_startX">起点X坐标</param>
        /// <param name="_startY">起点Y坐标</param>
        /// <param name="_endX">终点X坐标</param>
        /// <param name="_endY">终点Y坐标</param>
        /// <returns></returns>
        public DxfPolyline2D AddRectangle(double _startX, double _startY, double _endX, double _endY)
        {
            if ((_startX == _endX) && (_startY == _endY))
                return null;

            DxfVertex2D[] pointList = new DxfVertex2D[4]
            {
                new DxfVertex2D(_startX,_startY),
                new DxfVertex2D(_endX,_startY),
                new DxfVertex2D(_endX,_endY),
                new DxfVertex2D(_startX,_endY)
            };

            DxfPolyline2D rectangle = new DxfPolyline2D(pointList);
            rectangle.Closed = true;

            Model.Entities.Add(rectangle);
            return rectangle;
        }

        /// <summary>
        /// 方法，向模型空间添加一个圆
        /// </summary>
        /// <param name="_centerX">圆心的X坐标</param>
        /// <param name="_centerY">圆心的Y坐标</param>
        /// <param name="_radius">圆的半径</param>
        /// <param name="_isSolid">实心或空心，true为实心</param>
        /// <returns></returns>
        public DxfCircle AddCircle(double _centerX,double _centerY,double _radius,bool _isSolid)
        {
            DxfCircle circle = new DxfCircle(new Point2D(_centerX, _centerY), _radius);
            Model.Entities.Add(circle);

            if (_isSolid)
            {
                DxfHatch hatch = new DxfHatch();

                DxfHatch.BoundaryPath boundary = new DxfHatch.BoundaryPath();
                boundary.Type = BoundaryPathType.Outermost;
                hatch.BoundaryPaths.Add(boundary);
                DxfHatch.BoundaryPath.ArcEdge arcEdge = new DxfHatch.BoundaryPath.ArcEdge();
                arcEdge.Center = new Point2D(_centerX, _centerY);
                arcEdge.Radius = 1;
                arcEdge.StartAngle = 0;
                arcEdge.EndAngle = System.Math.PI * 2;
                boundary.Edges.Add(arcEdge);

                Model.Entities.Add(hatch);
            }

            return circle;
        }

        /// <summary>
        /// 方法，向模型空间添加一个图案填充
        /// </summary>
        /// <param name="_model">模型空间</param>
        /// <param name="_hatchName">填充名称</param>
        /// <param name="_startX">起点X坐标</param>
        /// <param name="_startY">起点Y坐标</param>
        /// <param name="_endX">终点X坐标</param>
        /// <param name="_endY">终点Y坐标</param>
        /// <returns></returns>
        public DxfHatch AddRecHatch(string _hatchName, double _startX, double _startY, double _endX, double _endY)
        {
            DxfHatch hatch = new DxfHatch();

            DxfHatch.BoundaryPath boundary = new DxfHatch.BoundaryPath();
            boundary.Type = BoundaryPathType.Polyline;
            hatch.BoundaryPaths.Add(boundary);

            DxfHatch.BoundaryPath.Polyline.Vertex[] vertexs = new DxfHatch.BoundaryPath.Polyline.Vertex[4]
            {
                new DxfHatch.BoundaryPath.Polyline.Vertex(_startX,_startY),
                new DxfHatch.BoundaryPath.Polyline.Vertex(_endX,_startY),
                new DxfHatch.BoundaryPath.Polyline.Vertex(_endX,_endY),
                new DxfHatch.BoundaryPath.Polyline.Vertex(_startX,_endY)
            };
            boundary.PolylineData = new DxfHatch.BoundaryPath.Polyline(vertexs);
            boundary.PolylineData.Closed = true;

            DxfPatternStore.Add(PatternReader.ReadPatterns("acad.pat"));
            hatch.Pattern = DxfPatternStore.GetPatternWithName(_hatchName);
            hatch.Scale = 0.5;

            Model.Entities.Add(hatch);
            return hatch;
        }

        /// <summary>
        /// 方法，向模型空间添加一个多行文本，居中对齐
        /// </summary>
        /// <param name="_model">模型</param>
        /// <param name="_text">文本内容</param>
        /// <param name="_x">起点X坐标</param>
        /// <param name="_y">起点Y坐标</param>
        /// <param name="_width">文本宽度</param>
        /// <param name="_height">文本高度</param>
        /// <param name="_style">文字样式</param>
        /// <returns></returns>
        public DxfMText AddMText(string _text,double _x,double _y,double _width,double _height,DxfTextStyle _style)
        {
            DxfMText mText = new DxfMText(_text, new Point3D(_x, _y, 0d), _height);

            mText.ReferenceRectangleWidth = _width;
            mText.Height = _height;
            mText.Style = _style;
            mText.AttachmentPoint = AttachmentPoint.MiddleCenter;

            Model.Entities.Add(mText);
            return mText;
        }

        /// <summary>
        /// 方法，将岩土名称转换为填充文件里的英文名称
        /// </summary>
        /// <param name="_hatchName">岩土名称</param>
        /// <returns></returns>
        public string HatchTrans(string _hatchName)
        {
            Dictionary<string, string> hatchName = new Dictionary<string, string>();

            hatchName.Add("安山岩", "asy");
            hatchName.Add("白云岩", "byy");
            hatchName.Add("白云质灰岩", "byzhy");
            hatchName.Add("板岩", "by");
            hatchName.Add("变粒岩", "bly");
            hatchName.Add("粗面岩", "cmy");
            hatchName.Add("粗砂", "cs");
            hatchName.Add("大理岩", "dly");
            hatchName.Add("蛋白土", "dbt");
            hatchName.Add("二长岩", "ecy");
            hatchName.Add("粉砂", "fs");
            hatchName.Add("构造角砾岩", "gzjly");
            hatchName.Add("硅质灰岩", "gzhy");
            hatchName.Add("含泥砂岩", "hnsy");
            hatchName.Add("含砂泥岩", "hsny");
            hatchName.Add("黑云变粒岩", "hybly");
            hatchName.Add("黑曜岩", "hyy");
            hatchName.Add("花岗斑岩", "hgby");
            hatchName.Add("花岗片麻岩", "hgpmy");
            hatchName.Add("花岗闪长岩", "hgscy");
            hatchName.Add("花岗岩", "hgy");
            hatchName.Add("黄土", "ht");
            hatchName.Add("黄土质粘土", "htznt");
            hatchName.Add("黄土状粉质砂土", "htzyst");
            hatchName.Add("黄土状粉质粘土", "htzynt");
            hatchName.Add("黄土状粘土", "htznt");
            hatchName.Add("灰岩", "hy");
            hatchName.Add("辉长岩", "hcy");
            hatchName.Add("辉绿岩", "tchly");
            hatchName.Add("辉石岩", "hsy");
            hatchName.Add("混合岩", "hhy");
            hatchName.Add("角砾灰岩", "jlhy");
            hatchName.Add("角砾土", "jlt");
            hatchName.Add("角砾岩", "jly");
            hatchName.Add("角砾状灰岩", "jlzhy");
            hatchName.Add("角闪片岩", "jspy");
            hatchName.Add("角页岩", "jyy");
            hatchName.Add("块石", "ks");
            hatchName.Add("块石土", "kst");
            hatchName.Add("砾砂", "lsa");
            hatchName.Add("砾石", "lsi");
            hatchName.Add("砾岩", "ly");
            hatchName.Add("流纹岩", "lwy");
            hatchName.Add("绿泥片岩", "lnpy");
            hatchName.Add("卵石", "ls");
            hatchName.Add("卵石土", "lst");
            hatchName.Add("煤层", "GLmc");
            hatchName.Add("糜棱岩", "mly");
            hatchName.Add("泥灰岩", "nhy");
            hatchName.Add("泥炭土", "ntt");
            hatchName.Add("泥岩", "ny");
            hatchName.Add("泥质砂岩", "nzsy");
            hatchName.Add("凝灰岩", "nhy");
            hatchName.Add("片麻岩", "pmy");
            hatchName.Add("片岩", "py");
            hatchName.Add("漂石", "ps");
            hatchName.Add("漂石土", "pst");
            hatchName.Add("千枚岩", "qmy");
            hatchName.Add("砂", "sa");
            hatchName.Add("砂岩", "sy");
            hatchName.Add("砂质砾岩", "szly");
            hatchName.Add("砂质页岩", "szyy");
            hatchName.Add("闪长斑岩", "scby");
            hatchName.Add("闪长岩", "scy");
            hatchName.Add("蛇纹岩", "swy");
            hatchName.Add("石膏", "sg");
            hatchName.Add("石膏土", "sgt");
            hatchName.Add("石灰岩", "shy");
            hatchName.Add("石英片岩", "sypy");
            hatchName.Add("石英岩", "syy");
            hatchName.Add("素填土", "stt");
            hatchName.Add("碎石", "ss");
            hatchName.Add("碎石土", "sst");
            hatchName.Add("碎石质粉质粘土", "sszynt");
            hatchName.Add("碎石质粘土", "ssznt");
            hatchName.Add("细砂", "xs");
            hatchName.Add("新粘土", "xnt");
            hatchName.Add("玄武岩", "xwy");
            hatchName.Add("粉质砂土", "yst");
            hatchName.Add("粉质粘土", "ynt");
            hatchName.Add("盐岩", "yy");
            hatchName.Add("盐渍土", "yzt");
            hatchName.Add("页岩", "yy");
            hatchName.Add("淤泥", "ynz");
            hatchName.Add("淤泥混砂", "ynhs");
            hatchName.Add("淤泥质粉质砂土", "ysty");
            hatchName.Add("淤泥质粉质粘土", "ynty");
            hatchName.Add("淤泥质粘土", "nty");
            hatchName.Add("圆砾", "yl");
            hatchName.Add("圆砾土", "ylt");
            hatchName.Add("杂填土", "ztt");
            hatchName.Add("粘土", "nxt");
            hatchName.Add("正长岩", "zcy");
            hatchName.Add("中粗砂", "zcs");
            hatchName.Add("中砂", "zs");
            hatchName.Add("粉土", "ft");
            hatchName.Add("种植土", "zzt");
            hatchName.Add("橄榄岩", "gly");
            hatchName.Add("燧石灰岩", "sshy");
            hatchName.Add("鲕状灰岩", "ezhy");

            if (hatchName.ContainsKey(_hatchName))
                return hatchName[_hatchName];
            else
                return "SOLID";
        }

        /// <summary>
        /// 方法，绘制单个钻孔柱状图
        /// </summary>
        /// <param name="_drawIndex">柱状图编号，从0开始，用于绘制多个柱状图时计算其摆放位置</param>
        /// <param name="_projectName">工程名称</param>
        /// <param name="_companyName">公司名称</param>
        /// <param name="_zk">钻孔数据</param>
        /// <param name="_scaleList">比例尺列表</param>
        /// <param name="_style1">钻孔柱状图的文字样式</param>
        /// <param name="_style2">剖面图钻孔的文字样式</param>
        public void DrawZk(int _drawIndex, string _projectName, string[] _company, Borehole _zk, List<double> _scaleList, DxfTextStyle _style1, DxfTextStyle _style2)
        {
            // 当钻孔内没有分层时退出
            if (_zk.Layers.Count == 0)
                return;

            // 计算图形横向偏移量，用于绘制多个柱状图时的空间摆放距离
            double xDis = _drawIndex * 195;

            // 绘制钻孔柱状图框架
            AddRectangle(0 + xDis, 0, 195 + xDis, 280);
            AddRectangle(10 + xDis, 20, 185 + xDis, 261);
            Model.Entities[Model.Entities.Count - 1].LineWeight = 30;

            double[,] pointKJ = new double[25, 4]
            {
                {10,254,185,254 }, {10,247,57,247 }, {63,247,185,247 }, { 10,240,185,240}, {10,220,185,220 },
                {131.5,233,185,233 }, {132.5,225,150.5,225 }, {152.5,225,170.5,225 }, {172.5,225,184,225 },
                {30,261,30,240 }, {57,254,57,240 }, {63,254,63,240 }, {90,261,90,240 }, {110,261,110,240 },
                {137,254,137,240 }, {157,254,157,240 }, {22,240,22,20 }, {30,240,30,20 }, {42,240,42,20 },
                {52,240,52,20 }, {62,240,62,20 }, {77,240,77,20 }, {131.5,240,131.5,20 },
                {151.5,240,151.5,20 }, {171.5,240,171.5,20 }
            };
            for (int i = 0; i < 25; i++)
                AddLine(pointKJ[i, 0] + xDis, pointKJ[i, 1], pointKJ[i, 2] + xDis, pointKJ[i, 3]);

            string[] textKJ = new string[31]
            {
                "工程名称","钻孔编号","孔口高程","勘察单位","钻孔深度","钻孔日期","初见水位","稳定水位","地\n质\n年\n代","及\n成\n因","层\n \n序",
                "层\n底\n标\n高\n(m)","层\n底\n深\n度\n(m)","分\n层\n厚\n度\n(m)","岩  土  描  述","标贯/动探","取 样","注水试验","击 数","深 度(m)",
                "编 号","深 度(m)","渗透系数\n(cm/s)","深度(m)","钻 孔 柱 状 图","制图:","校核:","审查:","核定:","X:","Y:"
            };
            double[,] ptextKJ = new double[31, 3]
            {
                {20,257.5,20 }, {20,250.5,20 }, {20,243.5,20 }, {100,257.5,20 }, {100,250.5,20 }, {100,243.5,20 }, {147,250.5,20 }, {147,243.5,20 },
                {13,230,6 }, {19,230,6 }, {26,230,8 }, {36,230,12 }, {47,230,10 }, {57,230,10 }, {104.25,230,54.5 }, {141.5,236.5,20 },
                {161.5,236.5,20 }, {178.25,236.5,13.5 }, {141.5,229,20 }, {141.5,222.5,20 }, {161.5,229,20 }, {161.5,222.5,20 },{178.25,229,13.5 },
                {178.25,222.5,13.5 }, {97.5,270,31 }, {32,13,10 }, {70,13,10 }, {106,13,10 }, {144,13,10 }, {65.5,252,5 }, {65.5,245,5 }
            };
            for (int i = 0; i < textKJ.Length; i++)
            {
                DxfMText t = AddMText(textKJ[i], ptextKJ[i, 0] + xDis, ptextKJ[i, 1], ptextKJ[i, 2], 2.5, _style1);
                if (i == 11 || i == 12 || i == 13 || i == 22)
                    t.LineSpacingFactor = 0.8;
            }

            // 绘制表头信息
            AddMText(_projectName, 60 + xDis, 257.5, 60, 2.5, _style1);
            AddMText(_zk.Name, 43.5 + xDis, 250.5, 27, 2.5, _style1);
            AddMText(_zk.Altitude.ToString("0.00") + "      m", 43.5 + xDis, 243.5, 27, 2.5, _style1);
            AddMText(_company[0], 147.5 + xDis, 257.5, 75, 2.5, _style1);
            AddMText(_zk.Layers[_zk.Layers.Count - 1].Depth.ToString("0.00") + "   m", 123.5 + xDis, 250.5, 27, 2.5, _style1);

            // 计算比例尺
            double scale;
            if (_zk.Layers[_zk.Layers.Count - 1].Depth <= 20)
                scale = 100;
            else if (_zk.Layers[_zk.Layers.Count - 1].Depth <= 40)
                scale = 200;
            else if (_zk.Layers[_zk.Layers.Count - 1].Depth <= 100)
                scale = 500;
            else
                scale = 1000;
            AddMText("柱\n状\n图\n1:" + scale, 69.5 + xDis, 230, 12, 2.5, _style1);

            // 绘制分层
            for(int i = 0; i < _zk.Layers.Count; i++)
            {
                // 分层线及深度标签
                double drawY = 220 - _zk.Layers[i].Depth / scale * 1000;

                AddLine(10 + xDis, drawY, 131.5 + xDis, drawY);
                AddMText(_zk.Layers[i].Number, 26 + xDis, drawY + 3, 8, 2.5, _style1);
                AddMText((_zk.Altitude - _zk.Layers[i].Depth).ToString("0.00"), 36 + xDis, drawY + 3, 8, 2.5, _style1);
                AddMText(_zk.Layers[i].Depth.ToString("0.00"), 47 + xDis, drawY + 3, 8, 2.5, _style1);
                if (i > 0)
                    AddMText((_zk.Layers[i].Depth - _zk.Layers[i - 1].Depth).ToString("0.00"), 57 + xDis, drawY + 3, 8, 2.5, _style1);
                else
                    AddMText(_zk.Layers[i].Depth.ToString("0.00"), 57 + xDis, drawY + 3, 8, 2.5, _style1);

                // 地质填充
                double hatchY1 = drawY;
                double hatchY2;
                if (i > 0)
                    hatchY2 = 220 - _zk.Layers[i - 1].Depth / scale * 1000;
                else
                    hatchY2 = 220;

                try
                {
                    string s = _zk.Layers[i].Name;
                    if (s.Contains("黏"))
                        s = s.Replace("黏", "粘");
                    AddRecHatch(HatchTrans(s), 62 + xDis, hatchY1, 77 + xDis, hatchY2);
                }
                catch
                {
                    AddRecHatch("SOLID", 62 + xDis, hatchY1, 77 + xDis, hatchY2);
                }

                // 岩土描述
                double presY = hatchY2 - 0.7;

                DxfMText t = AddMText(_zk.Layers[i].Name + "：" + _zk.Layers[i].Description, 78 + xDis, presY, 54.5, 1.5, _style1);
                t.AttachmentPoint = AttachmentPoint.TopLeft;
            }

            // 绘制标贯/动探
            for(int i = 0; i < _zk.NTests.Count; i++)
            {
                double drawY = 220 - _zk.NTests[i].Depth / scale * 1000;

                AddLine(131.5 + xDis, drawY, 151.5 + xDis, drawY);
                AddMText(_zk.NTests[i].Value.ToString("0"), 141.5 + xDis, drawY + 2, 20, 2.5, _style1);
                AddMText(_zk.NTests[i].Depth.ToString("0.00"), 141.5 + xDis, drawY - 2, 20, 2.5, _style1);
            }

            // 绘制取样
            for(int i = 0; i < _zk.Samples.Count; i++)
            {
                double drawY = 220 - _zk.Samples[i].Depth * 1000 / scale;

                AddLine(151.5 + xDis, drawY, 171.5 + xDis, drawY);
                AddMText(_zk.Samples[i].Name, 161.5 + xDis, drawY + 2, 20, 2.5, _style1);
                AddMText(_zk.Samples[i].Depth.ToString("0.00"), 161.5 + xDis, drawY - 2, 20, 2.5, _style1);
            }

            // 绘制人员信息
            AddMText(_company[2], 44 + xDis, 13, 10, 2.5, _style1);
            AddMText(_company[4], 82 + xDis, 13, 10, 2.5, _style1);
            AddMText(_company[5], 118 + xDis, 13, 10, 2.5, _style1);
            AddMText(_company[6], 156 + xDis, 13, 10, 2.5, _style1);

            // 绘制剖面图钻孔
            double distanceY = 280;
            for(int i = 0; i < _scaleList.Count; i++)
            {
                // 计算绘图起始位置
                distanceY = distanceY + 40 + _zk.Layers[_zk.Layers.Count - 1].Depth * 1000 / _scaleList[i];

                // 绘制钻孔编号、孔口高程、钻孔轴线
                AddLine(124 + xDis, distanceY + 10, 136 + xDis, distanceY + 10);
                AddMText(_zk.Name, 130 + xDis, distanceY + 13.5, 12, 4, _style2);
                AddMText(_zk.Altitude.ToString("0.00"), 130 + xDis, distanceY + 7, 12, 4, _style2);
                DxfPolyline2D l = AddLine(130 + xDis, distanceY, 130 + xDis, distanceY - _zk.Layers[_zk.Layers.Count - 1].Depth * 1000 / _scaleList[i]);
                l.LineWeight = 50;

                // 绘制分层
                for(int j = 0; j < _zk.Layers.Count; j++)
                {
                    double layerY = distanceY - _zk.Layers[j].Depth * 1000 / _scaleList[i];
                    double layerYold = distanceY;
                    if (j > 0)
                        layerYold = distanceY - _zk.Layers[j - 1].Depth * 1000 / _scaleList[i];

                    if (j < _zk.Layers.Count - 1)
                        AddLine(120 + xDis, layerY, 130 + xDis, layerY);
                    DxfMText text = AddMText(_zk.Layers[j].Depth.ToString("0.00") + "(" + (_zk.Altitude - _zk.Layers[j].Depth).ToString("0.00") + ")", 132 + xDis, layerY, 25, 3, _style2);
                    text.AttachmentPoint = AttachmentPoint.MiddleLeft;

                    try
                    {
                        string s = _zk.Layers[j].Name;
                        if (s.Contains("黏"))
                            s = s.Replace("黏", "粘");
                        AddRecHatch(HatchTrans(s), 120 + xDis, layerY, 130 + xDis, layerYold);
                    }
                    catch
                    {
                        AddRecHatch("SOLID", 120 + xDis, layerY, 130 + xDis, layerYold);
                    }
                }

                // 绘制取样
                for(int j = 0; j < _zk.Samples.Count; j++)
                {
                    double sampleY = distanceY - _zk.Samples[j].Depth * 1000 / _scaleList[i];

                    if (_zk.Samples[j].IsDisturbed)
                        AddCircle(127.5 + xDis, sampleY, 1, false);
                    else
                        AddCircle(127.5 + xDis, sampleY, 1, true);
                }

                // 绘制标贯/动探
                for(int j = 0; j < _zk.NTests.Count; j++)
                {
                    double bgY = distanceY - _zk.NTests[j].Depth * 1000 / _scaleList[i];

                    DxfMText text = AddMText(_zk.NTests[j].Type.ToString() + "=" + _zk.NTests[j].Value, 120.5 + xDis, bgY, 6, 2.5, _style2);
                    text.AttachmentPoint = AttachmentPoint.MiddleLeft;
                }

                // 绘制比例尺标记
                double scY = distanceY - _zk.Layers[_zk.Layers.Count - 1].Depth * 1000 / _scaleList[i] / 2;

                DxfMText t = AddMText("1:" + _scaleList[i], 20 + xDis, scY, 80, 20, _style2);
                t.AttachmentPoint = AttachmentPoint.MiddleLeft;
            }
        }

        /// <summary>
        /// 方法，绘制单个静力触探曲线图
        /// </summary>
        /// <param name="_drawIndex">曲线图编号，从0开始，用于绘制多个曲线图时计算其摆放位置</param>
        /// <param name="_projectName">工程名称</param>
        /// <param name="_companyName">公司名称</param>
        /// <param name="_jk">触探孔数据</param>
        /// <param name="_scaleList">比例尺列表</param>
        /// <param name="_style1">曲线图的文字样式</param>
        /// <param name="_style2">剖面图触探孔的文字样式</param>
        public void DrawJk(int _drawIndex,string _projectName,string[] _company,CPT _jk,List<double> _scaleList,DxfTextStyle _style1,DxfTextStyle _style2)
        {
            // 当触探孔内没有分层时退出
            if (_jk.Layers.Count == 0)
                return;

            // 计算图形横向偏移量，用于绘制多个柱状图时的空间摆放距离
            double xDis = _drawIndex * 195;

            // 绘制钻孔柱状图框架
            AddRectangle(0 + xDis, 0, 195 + xDis, 280);
            AddRectangle(10 + xDis, 20, 185 + xDis, 261);
            Model.Entities[Model.Entities.Count - 1].LineWeight = 30;

            double[,] pointKJ = new double[28, 4]
            {
                {10,254,185,254 }, {10,247,57,247 }, {63,247,185,247 }, { 10,240,185,240}, {10,220,185,220 },
                {30,261,30,240 }, {57,254,57,240 }, {63,254,63,240 }, {90,261,90,240 }, {110,261,110,240 },
                {137,254,137,240 }, {157,254,157,240 }, {22,240,22,20 }, {30,240,30,20 }, {42,240,42,20 },
                {52,240,52,20 }, {62,240,62,20 }, {77,240,77,20 },
                {87,220,87,222 }, {97,220,97,222 },{107,220,107,222 },{117,220,117,222 },{127,220,127,222 },
                {137,220,137,222 },{147,220,147,222 },{157,220,157,222 },{167,220,167,222 },{177,220,177,222 }
            };
            for (int i = 0; i < 28; i++)
                AddLine(pointKJ[i, 0] + xDis, pointKJ[i, 1], pointKJ[i, 2] + xDis, pointKJ[i, 3]);

            string[] textKJ = new string[32]
            {
                "工程名称","钻孔编号","孔口高程","勘察单位","钻孔深度","钻孔日期","初见水位","稳定水位","地\n质\n年\n代","及\n成\n因","层\n \n序",
                "层\n底\n标\n高\n(m)","层\n底\n深\n度\n(m)","分\n层\n厚\n度\n(m)","比 贯 入 阻 力 (MPa)","1","2","3","4","5",
                "6","7","8","9","10","静 力 触 探 曲 线 图","制图:","校核:","审查:","核定:","X:","Y:"
            };
            double[,] ptextKJ = new double[32, 3]
            {
                {20,257.5,20 }, {20,250.5,20 }, {20,243.5,20 }, {100,257.5,20 }, {100,250.5,20 }, {100,243.5,20 }, {147,250.5,20 }, {147,243.5,20 },
                {13,230,6 }, {19,230,6 }, {26,230,8 }, {36,230,12 }, {47,230,10 }, {57,230,10 }, {131,234.5,108 }, {87,227,2 },
                {97,227,2 }, {107,227,2 }, {117,227,2 }, {127,227,2 }, {137,227,2 }, {147,227,2 },{157,227,2 },
                {167,227,2 },{177,227,2 }, {97.5,270,175 }, {32,13,10 }, {70,13,10 }, {106,13,10 }, {144,13,10 }, {65.5,252,5 }, {65.5,245,5 }
            };
            for (int i = 0; i < textKJ.Length; i++)
            {
                DxfMText t = AddMText(textKJ[i], ptextKJ[i, 0] + xDis, ptextKJ[i, 1], ptextKJ[i, 2], 2.5, _style1);
                if (i == 11 || i == 12 || i == 13)
                    t.LineSpacingFactor = 0.8;
            }

            // 绘制表头信息
            AddMText(_projectName, 60 + xDis, 257.5, 60, 2.5, _style1);
            AddMText(_jk.Name, 43.5 + xDis, 250.5, 27, 2.5, _style1);
            AddMText(_jk.Altitude.ToString("0.00") + "      m", 43.5 + xDis, 243.5, 27, 2.5, _style1);
            AddMText(_company[0], 147.5 + xDis, 257.5, 75, 2.5, _style1);
            AddMText(_jk.Layers[_jk.Layers.Count - 1].Depth.ToString("0.00") + "   m", 123.5 + xDis, 250.5, 27, 2.5, _style1);

            // 计算比例尺
            double scale;
            if (_jk.Layers[_jk.Layers.Count - 1].Depth <= 20)
                scale = 100;
            else if (_jk.Layers[_jk.Layers.Count - 1].Depth <= 40)
                scale = 200;
            else if (_jk.Layers[_jk.Layers.Count - 1].Depth <= 100)
                scale = 500;
            else
                scale = 1000;
            AddMText("柱\n状\n图\n1:" + scale, 69.5 + xDis, 230, 12, 2.5, _style1);

            // 绘制分层
            for (int i = 0; i < _jk.Layers.Count; i++)
            {
                // 分层线及深度标签
                double drawY = 220 - _jk.Layers[i].Depth / scale * 1000;

                AddLine(10 + xDis, drawY, 77 + xDis, drawY);
                AddMText(_jk.Layers[i].Number, 26 + xDis, drawY + 3, 8, 2.5, _style1);
                AddMText((_jk.Altitude - _jk.Layers[i].Depth).ToString("0.00"), 36 + xDis, drawY + 3, 8, 2.5, _style1);
                AddMText(_jk.Layers[i].Depth.ToString("0.00"), 47 + xDis, drawY + 3, 8, 2.5, _style1);
                if (i > 0)
                    AddMText((_jk.Layers[i].Depth - _jk.Layers[i - 1].Depth).ToString("0.00"), 57 + xDis, drawY + 3, 8, 2.5, _style1);
                else
                    AddMText(_jk.Layers[i].Depth.ToString("0.00"), 57 + xDis, drawY + 3, 8, 2.5, _style1);

                // 地质填充
                double hatchY1 = drawY;
                double hatchY2;
                if (i > 0)
                    hatchY2 = 220 - _jk.Layers[i - 1].Depth / scale * 1000;
                else
                    hatchY2 = 220;

                try
                {
                    string s = _jk.Layers[i].Name;
                    if (s.Contains("黏"))
                        s = s.Replace("黏", "粘");
                    AddRecHatch(HatchTrans(s), 62 + xDis, hatchY1, 77 + xDis, hatchY2);
                }
                catch
                {
                    AddRecHatch("SOLID", 62 + xDis, hatchY1, 77 + xDis, hatchY2);
                }
            }

            // 绘制Ps曲线
            int n1 = _jk.PsList.Count;
            DxfVertex2D[] pointList = new DxfVertex2D[n1];
            for(int i = 0; i < n1; i++)
            {
                double drawY = 220 - Convert.ToDouble(i) / scale * 100;
                double drawX = 77 + _jk.PsList[i] * 10 + xDis;
                pointList[i] = new DxfVertex2D(drawX, drawY);
            }
            AddPline(pointList);

            // 绘制人员信息
            AddMText(_company[2], 44 + xDis, 13, 10, 2.5, _style1);
            AddMText(_company[4], 82 + xDis, 13, 10, 2.5, _style1);
            AddMText(_company[5], 118 + xDis, 13, 10, 2.5, _style1);
            AddMText(_company[6], 156 + xDis, 13, 10, 2.5, _style1);

            // 绘制剖面静力触探曲线图
            double distanceY = 280;
            for (int i = 0; i < _scaleList.Count; i++)
            {
                // 计算绘图起始位置
                distanceY = distanceY + 40 + _jk.Layers[_jk.Layers.Count - 1].Depth * 1000 / _scaleList[i];

                // 绘制钻孔编号、孔口高程、钻孔轴线
                AddLine(124 + xDis, distanceY + 10, 136 + xDis, distanceY + 10);
                AddMText(_jk.Name, 130 + xDis, distanceY + 13.5, 12, 4, _style2);
                AddMText(_jk.Altitude.ToString("0.00"), 130 + xDis, distanceY + 7, 12, 4, _style2);
                DxfPolyline2D l = AddLine(130 + xDis, distanceY, 130 + xDis, distanceY - _jk.Layers[_jk.Layers.Count - 1].Depth * 1000 / _scaleList[i]);
                l.LineWeight = 50;

                // 绘制分层
                for (int j = 0; j < _jk.Layers.Count; j++)
                {
                    double layerY = distanceY - _jk.Layers[j].Depth * 1000 / _scaleList[i];
                    double layerYold = distanceY;
                    if (j > 0)
                        layerYold = distanceY - _jk.Layers[j - 1].Depth * 1000 / _scaleList[i];

                    if (j < _jk.Layers.Count - 1)
                        AddLine(123 + xDis, layerY, 130 + xDis, layerY);
                    DxfMText text = AddMText(_jk.Layers[j].Depth.ToString("0.00") + "(" + (_jk.Altitude - _jk.Layers[j].Depth).ToString("0.00") + ")", 121 + xDis, layerY + 1.5, 33, 3, _style2);
                    text.AttachmentPoint = AttachmentPoint.MiddleLeft;

                    try
                    {
                        string s = _jk.Layers[j].Name;
                        if (s.Contains("黏"))
                            s = s.Replace("黏", "粘");
                        AddRecHatch(HatchTrans(s), 123 + xDis, layerY, 130 + xDis, layerYold);
                    }
                    catch
                    {
                        AddRecHatch("SOLID", 123 + xDis, layerY, 130 + xDis, layerYold);
                    }
                }

                // 绘制Ps值曲线
                int n2 = _jk.PsList.Count;
                DxfVertex2D[] pointList2 = new DxfVertex2D[n2];
                for(int j = 0; j < n2; j++)
                {
                    double drawY = distanceY - Convert.ToDouble(j) / _scaleList[i] * 100;
                    double drawX = 130 + _jk.PsList[j] * 10 + xDis;
                    pointList2[j] = new DxfVertex2D(drawX, drawY);
                }
                AddPline(pointList2);

                // 绘制Ps刻度
                double kdY = distanceY - _jk.Layers[_jk.Layers.Count - 1].Depth * 1000 / _scaleList[i] - 15;
                AddLine(130 + xDis, kdY, 180 + xDis, kdY);
                AddLine(130 + xDis, kdY, 130 + xDis, kdY + 8);
                for(int j = 1; j <= 5; j++)
                {
                    AddLine(130 + 10 * j + xDis, kdY, 130 + 10 * j + xDis, kdY + 2);
                    AddMText(j.ToString(), 130 + 10 * j + xDis, kdY + 7, 2, 3, _style2);
                }

                // 绘制比例尺标记
                double scY = distanceY - _jk.Layers[_jk.Layers.Count - 1].Depth * 1000 / _scaleList[i] / 2;

                DxfMText t = AddMText("1:" + _scaleList[i], 20 + xDis, scY, 80, 20, _style2);
                t.AttachmentPoint = AttachmentPoint.MiddleLeft;
            }
        }
    }
}
