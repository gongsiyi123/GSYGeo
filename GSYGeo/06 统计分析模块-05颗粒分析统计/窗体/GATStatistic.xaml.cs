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
using System.Windows.Shapes;
using System.Data;

namespace GSYGeo
{
    /// <summary>
    /// GATStatistic.xaml 的交互逻辑
    /// </summary>
    public partial class GATStatistic : Window
    {
        #region 参数定义

        // 定义数据统计列表GATStatisticDataGrid控件的数据源DataTable
        public static DataTable dtGAT = new DataTable("Statistic");

        #endregion

        #region 构造函数

        public GATStatistic()
        {
            InitializeComponent();

            // 初始化DataTable
            InitialGATStatisticDataGrid();

            // 设置绑定
            this.GATStatisticDataGrid.DataContext = dtGAT;
            this.GATPreviewStatisticDataGrid.DataContext = dtGAT;

            // 填充筛选ComboBox
            InitialComboBox();
            this.LayerNumberComboBox.SelectedIndex = 0;
        }

        #endregion
        
        #region 统计累积数据

        // 定义统计项目
        private string[] staName = new string[]
        {
            "layerNumber",
            "layerName",
            "lessThan100",
            "lessThan20",
            "lessThan2",
            "lessThan0_5",
            "lessThan0_25",
            "lessThan0_075",
            "soilType",

            "group100to200",
            "group20to2",
            "group2to0_5",
            "group0_5to0_25",
            "group0_25to0_075",
            "group0_075to0"
        };

        // 定义统计数据列表
        public static List<StatisticGAT> statisticList;

        // 初始化GATStatisticDataGrid，不带参数
        private void InitialGATStatisticDataGrid()
        {
            // 初始化统计数据列表
            statisticList = SelectStatisticData();

            // 定义RSTStatisticDataGrid数据列
            if (dtGAT.Columns.Count == 0)
            {
                foreach (string sta in staName)
                {
                    dtGAT.Columns.Add(new DataColumn(sta, typeof(string)));
                }
            }
            
            // 将统计项目列表赋值给DataTable
            DataRow dr;
            for(int i = 0; i < statisticList.Count; i++)
            {
                dr = dtGAT.NewRow();

                dr["layerNumber"] = statisticList[i].Layer;
                dr["layerName"] = statisticList[i].Name;
                dr["lessThan100"] = "100";
                dr["lessThan20"] = statisticList[i].GroupLessThan20.ToString() == "-0.19880205" ? "" : statisticList[i].GroupLessThan20.ToString("0");
                dr["lessThan2"] = statisticList[i].GroupLessThan2.ToString() == "-0.19880205" ? "" : statisticList[i].GroupLessThan2.ToString("0");
                dr["lessThan0_5"] = statisticList[i].GroupLessThan0_5.ToString() == "-0.19880205" ? "" : statisticList[i].GroupLessThan0_5.ToString("0");
                dr["lessThan0_25"] = statisticList[i].GroupLessThan0_25.ToString() == "-0.19880205" ? "" : statisticList[i].GroupLessThan0_25.ToString("0");
                dr["lessThan0_075"] = statisticList[i].GroupLessThan0_075.ToString() == "-0.19880205" ? "" : statisticList[i].GroupLessThan0_075.ToString("0");
                dr["soilType"] = statisticList[i].SoilType;

                dr["group100to200"] = statisticList[i].Group100To20.ToString() == "-0.19880205" ? "0" : statisticList[i].Group100To20.ToString("0");
                dr["group20to2"] = statisticList[i].Group20To2.ToString() == "-0.19880205" ? "0" : statisticList[i].Group20To2.ToString("0");
                dr["group2to0_5"] = statisticList[i].Group2To0_5.ToString() == "-0.19880205" ? "0" : statisticList[i].Group2To0_5.ToString("0");
                dr["group0_5to0_25"] = statisticList[i].Group0_5To0_25.ToString() == "-0.19880205" ? "0" : statisticList[i].Group0_5To0_25.ToString("0");
                dr["group0_25to0_075"] = statisticList[i].Group0_25To0_075.ToString() == "-0.19880205" ? "0" : statisticList[i].Group0_25To0_075.ToString("0");
                dr["group0_075to0"] = statisticList[i].Group0_075To0.ToString() == "-0.19880205" ? "0" : statisticList[i].Group0_075To0.ToString("0");

                dtGAT.Rows.Add(dr);
            }
        }

        public static List<StatisticGAT> SelectStatisticData()
        {
            // 定义统计数据列表
            List<StatisticGAT> _statisticList = new List<StatisticGAT>();

            // 定义分层分层编号列表、分层岩土名称列表
            List<string> layerNumberList = ProjectDataBase.ReadLayerNumberList(Program.currentProject);
            List<string> layerNameList = ProjectDataBase.ReadLayerNameList(Program.currentProject);

            // 在分层编号列表中循环，赋值统计数据列表
            for(int i = 0; i < layerNumberList.Count; i++)
            {
                // 检查当前的分层编号下在数据库中是否有内容，并添加数据到统计列表
                List<GrainAnalysisTest> gatList = GrainAnalysisTestDataBase.SelectByZkAndLayer(Program.currentProject, "", layerNumberList[i]);
                if (gatList.Count > 0)
                {
                    StatisticGAT statistic = new StatisticGAT(layerNumberList[i], layerNameList[i], gatList);
                    _statisticList.Add(statistic);
                }
            }

            // 返回
            return _statisticList;
        }

        public static List<StatisticGAT> SelectStatisticData(string _layerNumber)
        {
            // 定义统计数据列表
            List<StatisticGAT> _statisticList = new List<StatisticGAT>();

            // 定义分层分层编号列表、分层岩土名称列表
            List<string> layerNumberList = ProjectDataBase.ReadLayerNumberList(Program.currentProject);
            List<string> layerNameList = ProjectDataBase.ReadLayerNameList(Program.currentProject);

            // 提取分层岩土名称
            string _layerName = "";
            for(int i = 0; i < layerNumberList.Count; i++)
            {
                if (layerNumberList[i] == _layerNumber)
                {
                    _layerName = layerNameList[i];
                }
            }

            // 赋值统计数据列表
            List<GrainAnalysisTest> gatList = GrainAnalysisTestDataBase.SelectByZkAndLayer(Program.currentProject, "", _layerNumber);
            if (gatList.Count > 0)
            {
                StatisticGAT statistic = new StatisticGAT(_layerNumber, _layerName, gatList);
                _statisticList.Add(statistic);
            }
            
            // 返回
            return _statisticList;
        }

        // 根据所选分层刷新DataTable
        private void RefreshDataTableByLayer(string _layerNumber)
        {
            // 恢复初始状态
            dtGAT.Clear();
            
            // 刷新数据
            statisticList = SelectStatisticData(_layerNumber);

            // 刷新DataTable
            for (int i = 0; i < statisticList.Count; i++)
            {
                if (statisticList[i].Layer == _layerNumber)
                {
                    // 将统计项目列表赋值给DataTable
                    DataRow dr;
                    for (int j = 0; j < statisticList.Count; j++)
                    {
                        dr = dtGAT.NewRow();

                        dr["layerNumber"] = statisticList[i].Layer;
                        dr["layerName"] = statisticList[i].Name;
                        dr["lessThan100"] = "100";
                        dr["lessThan20"] = statisticList[i].GroupLessThan20.ToString() == "-0.19880205" ? "" : statisticList[i].GroupLessThan20.ToString("0");
                        dr["lessThan2"] = statisticList[i].GroupLessThan2.ToString() == "-0.19880205" ? "" : statisticList[i].GroupLessThan2.ToString("0");
                        dr["lessThan0_5"] = statisticList[i].GroupLessThan0_5.ToString() == "-0.19880205" ? "" : statisticList[i].GroupLessThan0_5.ToString("0");
                        dr["lessThan0_25"] = statisticList[i].GroupLessThan0_25.ToString() == "-0.19880205" ? "" : statisticList[i].GroupLessThan0_25.ToString("0");
                        dr["lessThan0_075"] = statisticList[i].GroupLessThan0_075.ToString() == "-0.19880205" ? "" : statisticList[i].GroupLessThan0_075.ToString("0");
                        dr["soilType"] = statisticList[i].SoilType;

                        dr["group100to200"] = statisticList[i].Group100To20.ToString() == "-0.19880205" ? "0" : statisticList[i].Group100To20.ToString("0");
                        dr["group20to2"] = statisticList[i].Group20To2.ToString() == "-0.19880205" ? "0" : statisticList[i].Group20To2.ToString("0");
                        dr["group2to0_5"] = statisticList[i].Group2To0_5.ToString() == "-0.19880205" ? "0" : statisticList[i].Group2To0_5.ToString("0");
                        dr["group0_5to0_25"] = statisticList[i].Group0_5To0_25.ToString() == "-0.19880205" ? "0" : statisticList[i].Group0_5To0_25.ToString("0");
                        dr["group0_25to0_075"] = statisticList[i].Group0_25To0_075.ToString() == "-0.19880205" ? "0" : statisticList[i].Group0_25To0_075.ToString("0");
                        dr["group0_075to0"] = statisticList[i].Group0_075To0.ToString() == "-0.19880205" ? "0" : statisticList[i].Group0_075To0.ToString("0");

                        dtGAT.Rows.Add(dr);
                    }
                }
            }
        }

        private void InitialComboBox()
        {
            List<string> layerNumberlist = ProjectDataBase.ReadLayerNumberList(Program.currentProject);
            List<string> layerNamelist = ProjectDataBase.ReadLayerNameList(Program.currentProject);
            List<string> layerlist = new List<string>();
            for (int i = 0; i < layerNumberlist.Count; i++)
            {
                layerlist.Add(layerNumberlist[i] + "   " + layerNamelist[i]);
            }
            this.LayerNumberComboBox.ItemsSource = layerlist;
        }

        private void LayerNumberComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 筛选
            string selectedLayer = this.LayerNumberComboBox.SelectedItem.ToString();
            selectedLayer = selectedLayer.Substring(0, selectedLayer.IndexOf("   "));

            // 刷新statisticList
            RefreshDataTableByLayer(selectedLayer);

            // 绘图
            Draw();
        }

        #endregion

        #region 绘图

        private void Draw()
        {
            // 清理旧图形
            this.DrawCurveCanvas.Children.Clear();

            // 定义有关长度
            double m = 10;
            double w = this.DrawCurveCanvas.Width;
            double h = this.DrawCurveCanvas.Height;

            //绘图
            DrawAxis(m, w, h);
            DrawCurve(m, w, h);
        }

        private void DrawAxis(double m, double w, double h)
        {
            // 绘制坐标轴和标签
            CanvasDrawing.DrawLine(this.DrawCurveCanvas, m, m, w - m, m);
            CanvasDrawing.DrawLine(this.DrawCurveCanvas, m, h - m, w - m, h - m);
            CanvasDrawing.DrawLine(this.DrawCurveCanvas, m, m, m, h - m);
            CanvasDrawing.DrawLine(this.DrawCurveCanvas, w - m, m, w - m, h - m);

            // 绘制纵坐标和横向网格线
            for(int i = 0; i < 11; i++)
            {
                CanvasDrawing.DrawText(this.DrawCurveCanvas, w - m + 3, (h - 2 * m) / 10 * i, (100 - i * 10).ToString(), 11, Brushes.Black, false, false, false);
                CanvasDrawing.DrawText(this.DrawCurveCanvas, w - m + 3, (h - 2 * m) / 10 * i, (i * 10).ToString());

                CanvasDrawing.DrawLine(this.DrawCurveCanvas, m, m + (h - 2 * m) / 10 * i, w - m, m + (h - 2 * m) / 10 * i);
            }

            // 绘制横坐标和纵向网格线
            double uniteLen = (w - 2 * m) / 5;
            double[] dScale = new double[9] { 0,0.05,0.1,0.16,0.22,0.30,0.4,0.52,0.7 };
            double[] lenNumber = new double[14] { 100, 60, 40, 20, 10, 5, 2, 1, 0.5, 0.2, 0.1, 0.05, 0.01, 0.005 };

            int k = 0;
            for (int i = 0; i < 5; i++)
            {
                for(int j = 0; j < 9; j++)
                {
                    double X = m + uniteLen * dScale[j] + uniteLen * i;

                    if(j!=9 && !(i==0 && j==0))
                        CanvasDrawing.DrawLine(this.DrawCurveCanvas, X, m, X, h - m);

                    if ((i == 0) && (j == 0 || j == 4 || j == 6 || j == 8))
                    {
                        CanvasDrawing.DrawText(this.DrawCurveCanvas, X - 5, h - m, lenNumber[k].ToString());
                        k++;
                    }
                    else if ((i == 1 || i == 2) && (j == 0 || j == 5 || j == 8))
                    {
                        CanvasDrawing.DrawText(this.DrawCurveCanvas, X - 5, h - m, lenNumber[k].ToString());
                        k++;
                    }
                    else if ((i == 3 || i == 4) && (j == 0 || j == 5))
                    {
                        CanvasDrawing.DrawText(this.DrawCurveCanvas, X - 5, h - m, lenNumber[k].ToString());
                        k++;
                    }
                }
            }
            
        }

        private void DrawCurve(double m, double w, double h)
        {
            if (statisticList.Count == 0)
                return;

            double uniteLen = (w - 2 * m) / 5;
            double[] dScale = new double[9] { 0, 0.05, 0.1, 0.16, 0.22, 0.30, 0.4, 0.52, 0.7 };
            double x1 = m;
            double x2 = m + uniteLen * dScale[8] + uniteLen * 0;
            double x3 = m + uniteLen * dScale[8] + uniteLen * 1;
            double x4 = m + uniteLen * dScale[5] + uniteLen * 2;
            double x5 = m + (uniteLen * dScale[7] + uniteLen * dScale[8]) / 2 + uniteLen * 2;
            double x6 = m + (uniteLen * dScale[1] + uniteLen * dScale[2]) / 2 + uniteLen * 3;
            double x7 = w - m;

            double uniteHei = (h - 2 * m) / 100;
            double y1 = m;
            double y2 = m + (100 - statisticList[0].GroupLessThan20) * uniteHei;
            double y3 = m + (100 - statisticList[0].GroupLessThan2) * uniteHei;
            double y4 = m + (100 - statisticList[0].GroupLessThan0_5) * uniteHei;
            double y5 = m + (100 - statisticList[0].GroupLessThan0_25) * uniteHei;
            double y6 = m + (100 - statisticList[0].GroupLessThan0_075) * uniteHei;
            double y7 = h - m;

            double[,] axis = new double[2, 7] { { x1, x2, x3, x4, x5, x6, x7 }, { y1, y2, y3, y4, y5, y6, y7 } };
            for(int i = 0; i < 7; i++)
            {
                CanvasDrawing.DrawCircle(this.DrawCurveCanvas, axis[0, i], axis[1, i], 2, true, 1, Brushes.DarkRed);
                if(i!=6)
                    CanvasDrawing.DrawLine(this.DrawCurveCanvas, axis[0, i], axis[1, i], axis[0, i + 1], axis[1, i + 1], 1, Brushes.Red);
            }
        }

        #endregion

        #region 输出Word文档

        /// <summary>
        /// 点击"输出为Word文档"按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OutputToWordButton_Click(object sender, RoutedEventArgs e)
        {
            // 选择输出目录
            string folderPath;
            System.Windows.Forms.FolderBrowserDialog programPathBrowser = new System.Windows.Forms.FolderBrowserDialog();
            if (programPathBrowser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                folderPath = programPathBrowser.SelectedPath;
            }
            else
            {
                return;
            }
            object path = folderPath + @"\" + Program.currentProject + @"-颗粒分析试验统计表.doc";

            // 启动输出窗体
            ShowProgressBar(path);
        }

        /// <summary>
        /// 输出Word文档函数
        /// </summary>
        /// <param name="obj">输出路径</param>
        public static void OutputToWord(object obj)
        {
            // 参数转换
            object path = obj.ToString();

            // 初始化统计数据列表
            statisticList = SelectStatisticData();

            // 输出Word
            Word rstStatisticWord = new Word();
            rstStatisticWord.AddGATStatisticTable(statisticList);
            rstStatisticWord.SaveAndQuit(path);
        }

        /// <summary>
        /// 实例化输出进度窗体
        /// </summary>
        /// <param name="obj">输出路径</param>
        private void ShowProgressBar(object obj)
        {
            // 参数转换
            string path = obj.ToString();

            // 实例化窗体
            OutputProgress prog = new OutputProgress(OutputProgress.OutputType.GAT, path, "输出统计结果", "正在输出颗粒分析试验统计成果Word文档……");
            prog.ShowDialog();
        }

        #endregion

        #region 其他

        /// <summary>
        /// 点击"关闭"按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #endregion
    }
}
