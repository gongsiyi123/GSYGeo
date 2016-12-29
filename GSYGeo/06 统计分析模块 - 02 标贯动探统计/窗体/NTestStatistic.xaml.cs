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
using System.Threading;

namespace GSYGeo
{
    /// <summary>
    /// NTestStatistic.xaml 的交互逻辑
    /// </summary>
    public partial class NTestStatistic : Window
    {
        #region 参数定义

        /// <summary>
        /// 定义数据统计列表NTestStatisticDataGrid控件的数据源DataTable
        /// </summary>
        DataTable dtNS = new DataTable("Statistic");

        /// <summary>
        /// 定义数据明细列表NTestDetailDataGrid控件的数据源DataTable
        /// </summary>
        DataTable dtND = new DataTable("Detail");
        
        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public NTestStatistic()
        {
            InitializeComponent();

            // 实例化计算中窗体,执行预加载
            OutputStatisticToWord.ShowCalculatingProgress(OutputProgress.OutputType.PreLoadNtest);

            // 初始化DataTable
            InitialNTestStatisticDataGrid();
            InitialNTestDetailDataGrid();

            // 设置绑定
            this.NTestStatisticDataGrid.DataContext = dtNS;
            this.NTestDetailDataGrid.DataContext = dtND;

            // 填充筛选ComboBox
            InitialComboBox();
            this.LayerListComboBox.SelectedIndex = 0;
        }

        #endregion

        #region 统计数据

        /// <summary>
        /// 定义统计项目
        /// </summary>
        private string[] staName = new string[]
        {
            "zkNumber","zkName","type","count","max","min","average","standardDeviation","variableCoefficient","correctionCoefficient","standardValue"
        };

        /// <summary>
        /// 初始化NTestStatisticDataGrid，不带参数
        /// </summary>
        private void InitialNTestStatisticDataGrid()
        {
            // 定义统计数据列表
            List<StatisticNTest> statisticList = OutputStatisticToWord.NTestStatisticList;

            // 定义NTestStatisticDataGrid数据列
            foreach (string sta in staName)
            {
                dtNS.Columns.Add(new DataColumn(sta, typeof(string)));
            }

            // 将统计数据列表赋值给DataTable
            DataRow dr;
            for(int i = 0; i < statisticList.Count; i++)
            {
                dr = dtNS.NewRow();
                dr["zkNumber"] = statisticList[i].Layer;
                dr["zkName"] = statisticList[i].Name;
                dr["type"] = statisticList[i].Type;
                dr["count"] = statisticList[i].Count.ToString("0");
                dr["max"] = statisticList[i].Max.ToString("0");
                dr["min"] = statisticList[i].Min.ToString("0");
                dr["average"] = statisticList[i].Average.ToString("0.0");
                dr["standardDeviation"] = statisticList[i].StandardDeviation.ToString() == "-0.19880205" ? "/" : statisticList[i].StandardDeviation.ToString("0.0");
                dr["variableCoefficient"] = statisticList[i].VariableCoefficient.ToString() == "-0.19880205" ? "/" : statisticList[i].VariableCoefficient.ToString("0.00");
                dr["correctionCoefficient"] = statisticList[i].CorrectionCoefficient.ToString() == "-0.19880205" ? "/" : statisticList[i].CorrectionCoefficient.ToString("0.0");
                dr["standardValue"] = statisticList[i].StandardValue.ToString() == "-0.19880205" ? "/" : statisticList[i].StandardValue.ToString("0.0");
                dtNS.Rows.Add(dr);
            }
        }

        /// <summary>
        /// 筛选统计数据
        /// </summary>
        /// <returns></returns>
        public static List<StatisticNTest> SelectStatisticData()
        {
            // 定义统计数据列表
            List<StatisticNTest> statisticList = new List<StatisticNTest>();

            // 定义分层分层编号列表、分层岩土名称列表、试验类型列表
            List<string> layerNumberList = ProjectDataBase.ReadLayerNumberList(Program.currentProject);
            List<string> layerNameList = ProjectDataBase.ReadLayerNameList(Program.currentProject);
            ZkNTest.ntype[] typeList = new ZkNTest.ntype[] { ZkNTest.ntype.N, ZkNTest.ntype.N10, ZkNTest.ntype.N635, ZkNTest.ntype.N120 };

            // 在分层编号列表中循环，赋值统计数据列表
            for (int i = 0; i < layerNumberList.Count; i++)
            {
                // 在试验类型列表中循环
                for (int j = 0; j < typeList.Length; j++)
                {
                    // 检查当前的分层编号和试验类型的组合下在数据库中是否有内容
                    List<ZkNTest> nTestList = BoreholeDataBase.ReadLayerNTest(Program.currentProject, layerNumberList[i], typeList[j]);
                    if (nTestList.Count > 0)
                    {
                        // 提取符合条件的标贯/动探列表中的试验数据值
                        List<double> dataList = new List<double>();
                        foreach (ZkNTest nTest in nTestList)
                        {
                            dataList.Add(nTest.Value);
                        }

                        // 添加一组符合筛选条件的统计数据
                        StatisticNTest stateNTest = new StatisticNTest(layerNumberList[i], layerNameList[i], typeList[j], dataList, 6);
                        statisticList.Add(stateNTest);
                    }
                }
            }

            // 返回
            return statisticList;
        }

        #endregion

        #region 查看明细

        /// <summary>
        /// 填充筛选ComboBox函数
        /// </summary>
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

        /// <summary>
        /// 定义明细查看项目
        /// </summary>
        private string[] detName = new string[]
        {
            "zkNumber","nTestDepth","nTestValue","nTestType"
        };

        /// <summary>
        /// 初始化NTestDetailcDataGrid，不带参数
        /// </summary>
        private void InitialNTestDetailDataGrid()
        {
            // 定义NTestStatisticDataGrid数据列
            foreach (string det in detName)
            {
                dtND.Columns.Add(new DataColumn(det, typeof(string)));
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
            List<ZkNTest> ntests = BoreholeDataBase.ReadLayerNTest(Program.currentProject, selectedLayer);
            dtND.Clear();
            DataRow dr;
            for(int i = 0; i < ntests.Count; i++)
            {
                dr = dtND.NewRow();
                dr["zkNumber"] = ntests[i].ZkNumber;
                dr["nTestDepth"] = ntests[i].Depth.ToString("0.00");
                dr["nTestValue"] = ntests[i].Value.ToString("0");
                dr["nTestType"] = ntests[i].Type;
                dtND.Rows.Add(dr);   
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
                folderPath = programPathBrowser.SelectedPath;
            else
                return;
            string path = folderPath + @"\" + Program.currentProject + @"-标贯动探统计表.doc";

            // 启动输出窗体
            ShowProgressBar(path);
        }

        /// <summary>
        /// 输出Word文档函数
        /// </summary>
        /// <param name="obj">输出路径</param>
        public static void OutputToWord(string _path)
        {
            // 定义统计数据列表
            List<StatisticNTest> statisticList = SelectStatisticData();

            // 输出Word
            Word ntestStatisticWord = new Word();
            ntestStatisticWord.AddNTestStatisticTable(statisticList);
            ntestStatisticWord.SaveAndQuit(_path);
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
            OutputProgress prog = new OutputProgress(OutputProgress.OutputType.NTest, path, "输出统计结果", "正在输出标贯/动探锤击数统计成果Word文档……");
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
            this.Close();
        }

        #endregion
    }
}
