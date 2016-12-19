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
    /// CPTStatistic.xaml 的交互逻辑
    /// </summary>
    public partial class CPTStatistic : Window
    {
        #region 参数定义

        // 定义数据统计列表CPTStatisticDataGrid控件的数据源DataTable
        DataTable dtCS = new DataTable("Statistic");

        // 定义数据明细列表CPTDetailDataGrid控件的数据源DataTable
        DataTable dtCD = new DataTable("Detail");

        #endregion

        #region 构造函数

        public CPTStatistic()
        {
            InitializeComponent();

            // 初始化DataTable
            InitialCPTStatisticDataGrid();
            InitialCPTDetailDataGrid();

            // 设置绑定
            this.CPTStatisticDataGrid.DataContext = dtCS;
            this.CPTDetailDataGrid.DataContext = dtCD;

            // 填充筛选ComboBox
            InitialComboBox();
            this.LayerListComboBox.SelectedIndex = 0;
        }

        #endregion

        #region 统计数据

        // 定义统计项目
        private string[] staName = new string[]
        {
            "jkNumber","jkName","count","max","min","average","standardDeviation","variableCoefficient","correctionCoefficient","standardValue"
        };

        // 初始化CPTStatisticDataGrid，不带参数
        private void InitialCPTStatisticDataGrid()
        {
            // 定义统计数据列表
            List<StatisticCPT> statisticList = SelectStatisticData();
            
            // 定义CPTStatisticDataGrid数据列
            foreach (string sta in staName)
            {
                dtCS.Columns.Add(new DataColumn(sta, typeof(string)));
            }

            // 将统计数据列表赋值给DataTable
            DataRow dr;
            for (int i = 0; i < statisticList.Count; i++)
            {
                dr = dtCS.NewRow();
                dr["jkNumber"] = statisticList[i].Layer;
                dr["jkName"] = statisticList[i].Name;
                dr["count"] = statisticList[i].Count.ToString("0");
                dr["max"] = statisticList[i].Max.ToString("0.00");
                dr["min"] = statisticList[i].Min.ToString("0.00");
                dr["average"] = statisticList[i].Average.ToString("0.00");
                dr["standardDeviation"] = statisticList[i].StandardDeviation.ToString() == "-0.19880205" ? "/" : statisticList[i].StandardDeviation.ToString("0.00");
                dr["variableCoefficient"] = statisticList[i].VariableCoefficient.ToString() == "-0.19880205" ? "/" : statisticList[i].VariableCoefficient.ToString("0.00");
                dr["correctionCoefficient"] = statisticList[i].CorrectionCoefficient.ToString() == "-0.19880205" ? "/" : statisticList[i].CorrectionCoefficient.ToString("0.0");
                dr["standardValue"] = statisticList[i].StandardValue.ToString() == "-0.19880205" ? "/" : statisticList[i].StandardValue.ToString("0.00");
                dtCS.Rows.Add(dr);
            }
        }

        public static List<StatisticCPT> SelectStatisticData()
        {
            // 定义统计数据列表
            List<StatisticCPT> statisticList = new List<StatisticCPT>();

            // 定义分层分层编号列表、分层岩土名称列表、试验类型列表
            List<string> layerNumberList = ProjectDataBase.ReadLayerNumberList(Program.currentProject);
            List<string> layerNameList = ProjectDataBase.ReadLayerNameList(Program.currentProject);

            // 在分层编号列表中循环，赋值统计数据列表
            for (int i = 0; i < layerNumberList.Count; i++)
            {
                // 检查当前的分层编号下，数据库中是否有内容
                List<double> psList = CPTDataBase.ReadLayerPs(Program.currentProject, layerNumberList[i]);
                if (psList.Count > 0)
                {
                    // 添加一组符合筛选条件的统计数据
                    StatisticCPT statePs = new StatisticCPT(layerNumberList[i], layerNameList[i], psList, 6);
                    statisticList.Add(statePs);
                }
            }

            // 返回
            return statisticList;
        }

        #endregion

        #region 查看明细

        // 填充筛选ComboBox函数
        private void InitialComboBox()
        {
            List<string> layerNumberlist = ProjectDataBase.ReadLayerNumberList(Program.currentProject);
            List<string> layerNamelist = ProjectDataBase.ReadLayerNameList(Program.currentProject);
            List<string> layerlist = new List<string>();
            for (int i = 0; i < layerNumberlist.Count; i++)
            {
                layerlist.Add(layerNumberlist[i] + "   " + layerNamelist[i]);
            }
            this.LayerListComboBox.ItemsSource = layerlist;
        }

        // 定义明细查看项目
        private string[] detName = new string[]
        {
            "jkNumber","psDepth","psValue"
        };

        // 初始化CPTDetailcDataGrid，不带参数
        private void InitialCPTDetailDataGrid()
        {
            // 定义CPTStatisticDataGrid数据列
            foreach (string det in detName)
            {
                dtCD.Columns.Add(new DataColumn(det, typeof(string)));
            }
        }

        /// <summary>
        /// 选取分层筛选选框时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LayerListComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 筛选
            string selectedLayer = this.LayerListComboBox.SelectedItem.ToString();
            selectedLayer = selectedLayer.Substring(0, selectedLayer.IndexOf("   "));
            List<StatisticPs> pss = CPTDataBase.ReadLayerPsAsStatePs(Program.currentProject, selectedLayer);
            dtCD.Clear();
            DataRow dr;
            for (int i = 0; i < pss.Count; i++)
            {
                dr = dtCD.NewRow();
                dr["jkNumber"] = pss[i].JkNumber;
                dr["psDepth"] = pss[i].PsDepth.ToString("0.0");
                dr["psValue"] = pss[i].PsValue.ToString("0.00");
                dtCD.Rows.Add(dr);
            }
        }

        #endregion

        #region 输出Word文件

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
            object path = folderPath + @"\" + Program.currentProject + @"-静力触探摩阻力统计表.doc";

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

            // 定义统计数据列表
            List<StatisticCPT> statisticList = SelectStatisticData();

            // 输出Word
            Word cptStatisticWord = new Word();
            cptStatisticWord.AddPsStatisticTable(statisticList);
            cptStatisticWord.SaveAndQuit(path);
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
            OutputProgress prog = new OutputProgress(OutputProgress.OutputType.Ps, path, "输出统计结果", "正在输出静力触探摩阻力统计成果Word文档……");
            prog.ShowDialog();
        }

        #endregion

        #region 其他

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #endregion
    }
}
