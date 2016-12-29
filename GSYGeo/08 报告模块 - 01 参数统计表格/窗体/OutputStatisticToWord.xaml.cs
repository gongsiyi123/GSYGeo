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
    /// OutputStatisticToWord.xaml 的交互逻辑
    /// </summary>
    public partial class OutputStatisticToWord : Window
    {
        #region 参数定义

        /// <summary>
        /// 定义当前正采用的标准
        /// </summary>
        public static string CurrentStandard = SettingDataBase.ReadLocalStandard();

        /// <summary>
        /// 输出项目
        /// </summary>
        private string[] tables = new string[]
        {
            "勘察工作量统计",
            "标贯/动探统计",
            "静力触探摩阻力统计",
            "土工常规试验统计",
            "颗粒分析试验统计",
            "承载力和压缩模量综合取值",
            "抗剪强度综合取值",
        };

        /// <summary>
        /// 预加载工作量统计列表
        /// </summary>
        public static StatisticWordLoad WlStatisticList;

        /// <summary>
        /// 预加载标贯/动探统计列表
        /// </summary>
        public static List<StatisticNTest> NTestStatisticList;

        /// <summary>
        /// 预加载试验指标统计列表
        /// </summary>
        public static List<StatisticRST> RstStatisticList;

        /// <summary>
        /// 预加载Ps值统计列表
        /// </summary>
        public static List<StatisticCPT> CptStatisticList;

        /// <summary>
        /// 预加载颗分试验统计列表
        /// </summary>
        public static List<StatisticGAT> GatStatisticList;

        /// <summary>
        /// 输出项列表
        /// </summary>
        public static List<string> CheckedTable = new List<string>();

        /// <summary>
        /// 承载力和压缩模量土质类型DataTable
        /// </summary>
        public static DataTable dtBam = new DataTable();

        /// <summary>
        /// 抗剪强度土质类型DataTable
        /// </summary>
        public static DataTable dtSs = new DataTable();

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public OutputStatisticToWord()
        {
            InitializeComponent();

            // 实例化计算中窗体,执行预加载
            ShowCalculatingProgress(OutputProgress.OutputType.PreLoadAll);

            // 读取输出项
            ReadTables();
        }

        #endregion

        #region 预加载

        /// <summary>
        /// 预加载全部数据
        /// </summary>
        public static void PreviewLoadAll()
        {
            WlStatisticList = WorkLoadStatistic.SelectStatisticData();

            NTestStatisticList = NTestStatistic.SelectStatisticData();

            RstStatisticList = RSTStatistic.SelectStatisticData();

            CptStatisticList = CPTStatistic.SelectStatisticData();

            GatStatisticList = GATStatistic.SelectStatisticData();
        }

        /// <summary>
        /// 预加载标贯/动探
        /// </summary>
        public static void PreviewLoadNtest()
        {
            NTestStatisticList = NTestStatistic.SelectStatisticData();
        }

        #endregion

        #region 读取数据

        /// <summary>
        /// 读取输出项
        /// </summary>
        private void ReadTables()
        {
            for(int i = 0; i < tables.Length; i++)
            {
                CheckBox checkbox = new CheckBox();
                checkbox.Content = tables[i];
                this.SelectTableListBox.Items.Add(checkbox);
            }
        }

        #endregion

        #region 输出

        /// <summary>
        /// 点击"确定"按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CommitButton_Click(object sender, RoutedEventArgs e)
        {
            // 读取选中的输出项
            CheckedTable.Clear();
            foreach (CheckBox item in this.SelectTableListBox.Items)
                if (item.IsChecked == true)
                    CheckedTable.Add(item.Content.ToString());

            // 没有选中时退出
            if (CheckedTable.Count == 0)
            {
                MessageBox.Show("亲，您没有选中任何项哦！");
                return;
            }

            // 同时选中"承载力和压缩模量综合取值"和"抗剪强度综合取值"时
            if (CheckedTable.Contains("承载力和压缩模量综合取值") && CheckedTable.Contains("抗剪强度综合取值"))
            {
                // 实例化土质类型对话框
                BearingAndModulusToWord bamType = new BearingAndModulusToWord(CurrentStandard);
                bamType.Title = "确认 承载力和压缩模量计算 的土质类型选项";

                // "确定"按钮替换为"下一步"
                bamType.CommitButton.Content = "下一步";
                bamType.ShowDialog();
                if (bamType.DialogResult == true)
                {
                    // 初始化DataTable
                    dtBam.Clear();
                    dtBam = bamType.dtLayer;

                    // 实例化计算中窗体,计算承载力和压缩模量统计数据
                    ShowCalculatingProgress(OutputProgress.OutputType.BearingAndModulus);

                    // 实例化土质类型对话框
                    ShearingStrengthToWord ssType = new ShearingStrengthToWord(CurrentStandard);
                    ssType.Title = "确认 抗剪强度计算 的土质类型选项";
                    ssType.ShowDialog();

                    // 计算抗剪强度统计数据
                    if (ssType.DialogResult == true)
                    {
                        // 初始化DataTable
                        dtSs.Clear();
                        dtSs = ssType.dtLayer;

                        // 实例化计算中窗体,计算抗剪强度统计数据
                        ShowCalculatingProgress(OutputProgress.OutputType.ShearingStrength);
                    }
                    else
                        return;
                }
                else
                {
                    return;
                }
            }
            // 只选中"抗剪强度综合取值"时
            else if (!CheckedTable.Contains("承载力和压缩模量综合取值") && CheckedTable.Contains("抗剪强度综合取值"))
            {
                ShearingStrengthToWord ssType = new ShearingStrengthToWord(CurrentStandard);
                ssType.Title = "确认 抗剪强度计算 的土质类型选项";
                ssType.ShowDialog();
                if (ssType.DialogResult == true)
                {
                    // 初始化DataTable
                    dtSs.Clear();
                    dtSs = ssType.dtLayer;
                    
                    // 实例化计算中窗体,计算抗剪强度统计数据
                    ShowCalculatingProgress(OutputProgress.OutputType.ShearingStrength);
                }
                else
                    return;
            }
            // 只选中"承载力和压缩模量综合取值"时
            else if (CheckedTable.Contains("承载力和压缩模量综合取值") && !CheckedTable.Contains("抗剪强度综合取值"))
            {
                BearingAndModulusToWord bamType = new BearingAndModulusToWord(CurrentStandard);
                bamType.Title = "确认 承载力和压缩模量计算 的土质类型选项";
                bamType.ShowDialog();
                if (bamType.DialogResult == true)
                {
                    // 初始化DataTable
                    dtBam.Clear();
                    dtBam = bamType.dtLayer;

                    // 实例化计算中窗体,计算抗剪强度统计数据
                    ShowCalculatingProgress(OutputProgress.OutputType.BearingAndModulus);
                }
                else
                    return;
            }

            // 选择输出目录
            string folderPath;
            System.Windows.Forms.FolderBrowserDialog programPathBrowser = new System.Windows.Forms.FolderBrowserDialog();
            if (programPathBrowser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                folderPath = programPathBrowser.SelectedPath;
            else
                return;
            string path = folderPath + @"\" + Program.currentProject + @"-参数统计表格.doc";
            
            // 启动输出窗体
            ShowProgressBar(path);
        }

        /// <summary>
        /// 计算承载力和压缩模量统计结果
        /// </summary>
        public static void CalculatingBam()
        {
            BearingAndModulusCalculation.CalcuOutput(dtBam, RstStatisticList, CptStatisticList, NTestStatisticList);
        }

        /// <summary>
        /// 计算抗剪强度统计结果
        /// </summary>
        public static void CalculatingSs()
        {
            ShearingStrengthCalculation.CalcuOutput(dtSs, RstStatisticList, CptStatisticList, NTestStatisticList);
        }

        /// <summary>
        /// 输出Word文档函数
        /// </summary>
        /// <param name="obj">输出路径</param>
        public static void OutputToWord(string _path)
        {
            // 输出Word
            Word allStaTableWord = new Word();
            allStaTableWord.AddAllStatisticTable(CheckedTable, WlStatisticList, NTestStatisticList, CptStatisticList, RstStatisticList, GatStatisticList, BearingAndModulusCalculation.statisticListOutput, ShearingStrengthCalculation.statisticListOutput);
            allStaTableWord.SaveAndQuit(_path);
        }

        /// <summary>
        /// 实例化输出进度窗体
        /// </summary>
        /// <param name="obj">输出路径</param>
        private void ShowProgressBar(string _path)
        {
            // 实例化窗体
            OutputProgress prog = new OutputProgress(OutputProgress.OutputType.AllTables, _path, "输出统计结果", "正在批量生成参数统计表格Word文档……");
            prog.ShowDialog();
        }

        /// <summary>
        /// 实例化计算中窗体
        /// </summary>
        /// <param name="_type">计算类型</param>
        public static void ShowCalculatingProgress(OutputProgress.OutputType _type)
        {
            // 实例化窗体
            CalcuProgress prog = new CalcuProgress(_type);
            prog.ShowDialog();
        }

        #endregion

        #region 其他

        /// <summary>
        /// 选中"全选"框时，选中所有输出项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IsSelectAllCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            foreach (CheckBox item in this.SelectTableListBox.Items)
                item.IsChecked = true;
        }

        /// <summary>
        /// 取消选中"全选"框时，取消选中所有输出项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IsSelectAllCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (CheckBox item in this.SelectTableListBox.Items)
                item.IsChecked = false;
        }

        #endregion
    }
}
