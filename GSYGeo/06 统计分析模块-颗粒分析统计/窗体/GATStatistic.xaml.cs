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

            // 填充筛选ComboBox
            InitialComboBox();
            this.LayerNumberComboBox.SelectedIndex = 0;
        }

        #endregion

        #region 统计数据

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
            "soilType"
        };

        // 定义统计数据列表
        public static List<StatisticGAT> statisticList;

        // 初始化GATStatisticDataGrid，不带参数
        private void InitialGATStatisticDataGrid()
        {
            // 初始化统计数据列表
            statisticList = SelectStatisticData();

            // 定义RSTStatisticDataGrid数据列
            foreach (string sta in staName)
            {
                dtGAT.Columns.Add(new DataColumn(sta, typeof(string)));
            }

            // 将统计项目列表赋值给DataTable
            DataRow dr;
            for(int i = 0; i < statisticList.Count; i++)
            {
                dr = dtGAT.NewRow();
                dr["layerNumber"] = statisticList[i].Layer;
                dr["layerName"] = statisticList[i].Name;
                dr["lessThan100"] = "100";
                dr["lessThan20"] = statisticList[i].GroupLessThan20.ToString() == "-0.19880205" ? "/" : statisticList[i].GroupLessThan20.ToString("0");
                dr["lessThan2"] = statisticList[i].GroupLessThan2.ToString() == "-0.19880205" ? "/" : statisticList[i].GroupLessThan2.ToString("0");
                dr["lessThan0_5"] = statisticList[i].GroupLessThan0_5.ToString() == "-0.19880205" ? "/" : statisticList[i].GroupLessThan0_5.ToString("0");
                dr["lessThan0_25"] = statisticList[i].GroupLessThan0_25.ToString() == "-0.19880205" ? "/" : statisticList[i].GroupLessThan0_25.ToString("0");
                dr["lessThan0_075"] = statisticList[i].GroupLessThan0_075.ToString() == "-0.19880205" ? "/" : statisticList[i].GroupLessThan0_075.ToString("0");
                dr["soilType"] = statisticList[i].SoilType;
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
                        dr["lessThan20"] = statisticList[i].GroupLessThan20.ToString() == "-0.19880205" ? "/" : statisticList[i].GroupLessThan20.ToString("0");
                        dr["lessThan2"] = statisticList[i].GroupLessThan2.ToString() == "-0.19880205" ? "/" : statisticList[i].GroupLessThan2.ToString("0");
                        dr["lessThan0_5"] = statisticList[i].GroupLessThan0_5.ToString() == "-0.19880205" ? "/" : statisticList[i].GroupLessThan0_5.ToString("0");
                        dr["lessThan0_25"] = statisticList[i].GroupLessThan0_25.ToString() == "-0.19880205" ? "/" : statisticList[i].GroupLessThan0_25.ToString("0");
                        dr["lessThan0_075"] = statisticList[i].GroupLessThan0_075.ToString() == "-0.19880205" ? "/" : statisticList[i].GroupLessThan0_075.ToString("0");
                        dr["soilType"] = statisticList[i].SoilType;
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
        }

        #endregion

        #region 绘图



        #endregion

        #region 输出Word文档



        #endregion

        #region 其他



        #endregion


    }
}
