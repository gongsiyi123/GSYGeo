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
    /// WorkLoadStatistic.xaml 的交互逻辑
    /// </summary>
    public partial class WorkLoadStatistic : Window
    {
        #region 参数定义

        /// <summary>
        /// 定义数据统计列表WordLoadDataGrid控件的数据源DataTable
        /// </summary>
        DataTable dtWL = new DataTable("Statistic");

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public WorkLoadStatistic()
        {
            InitializeComponent();

            // 初始化DataTable
            InitialWordLoadStatisticDataGrid();

            // 设置绑定
            this.WorkLoadDataGrid.DataContext = dtWL;
        }

        #endregion

        #region 统计数据

        /// <summary>
        /// 定义DataTable列
        /// </summary>
        private string[] staName = new string[]
        {
            "type",
            "unite",
            "amount",
            "remark"
        };

        /// <summary>
        /// 定义工作项目数组
        /// </summary>
        public static string[] typeList = new string[]
        {
            "地形测绘",
            "工程地质测绘",
            "钻探",
            "钻孔封孔",
            "原状样",
            "扰动样",
            "标准贯入试验",
            "轻型动力触探试验",
            "重型动力触探试验",
            "超重型动力触探试验",
            "静力触探试验",
            "土工常规试验",
            "室内渗透试验",
            "颗粒分析试验",
            "天然建筑材料调查"
        };

        /// <summary>
        /// 定义单位数组
        /// </summary>
        public static string[] uniteList = new string[]
        {
            "km2",
            "km2",
            "m/孔",
            "m/孔",
            "组",
            "组",
            "次",
            "次",
            "次",
            "次",
            "m/孔",
            "组",
            "组",
            "组",
            "处"
        };

        /// <summary>
        /// 定义工作量数组
        /// </summary>
        public static string[] amountList = new string[15];

        /// <summary>
        /// 定义备注数组
        /// </summary>
        public static string[] remarkList = new string[]
        {
            "1:1000",
            "1:1000",
            "",
            "",
            "",
            "",
            "N",
            "N10",
            "N63.5",
            "N120",
            "",
            "",
            "",
            "",
            ""
        };

        /// <summary>
        /// 初始化NTestStatisticDataGrid
        /// </summary>
        private void InitialWordLoadStatisticDataGrid()
        {
            // 定义统计数据列表
            StatisticWordLoad statisticData = SelectStatisticData();

            // 定义NTestStatisticDataGrid数据列
            foreach (string sta in staName)
            {
                dtWL.Columns.Add(new DataColumn(sta, typeof(string)));
            }

            // 赋值工作量数组
            amountList[0] = "";
            amountList[1] = "";
            amountList[2] = statisticData.Borehole + "/" + statisticData.CountBorehole;
            amountList[3] = statisticData.Borehole + "/" + statisticData.CountBorehole;
            amountList[4] = statisticData.UndisturbedSample.ToString();
            amountList[5] = statisticData.DisturbedSample.ToString();
            amountList[6] = statisticData.NTestStandard.ToString();
            amountList[7] = statisticData.NTestN10.ToString();
            amountList[8] = statisticData.NTestN635.ToString();
            amountList[9] = statisticData.NTestN120.ToString();
            amountList[10] = statisticData.CPT + "/" + statisticData.CountCPT;
            amountList[11] = statisticData.RST.ToString();
            amountList[12] = statisticData.Permeability.ToString();
            amountList[13] = statisticData.GAT.ToString();
            amountList[14] = "";

            // 将统计数据列表赋值给DataTable
            DataRow dr;
            for(int i = 0; i < amountList.Length; i++)
            {
                if (amountList[i] == "" || amountList[i] == "0")
                    continue;

                dr = dtWL.NewRow();
                dr["type"] = typeList[i];
                dr["unite"] = uniteList[i];
                dr["amount"] = amountList[i];
                dr["remark"] = remarkList[i];
                dtWL.Rows.Add(dr);
            }
        }

        /// <summary>
        /// 筛选统计数据
        /// </summary>
        /// <returns></returns>
        private static StatisticWordLoad SelectStatisticData()
        {
            // 定义统计数据列表
            StatisticWordLoad statisticData = new StatisticWordLoad();

            // 读取钻孔列表
            List<Borehole> zkList = BoreholeDataBase.ReadZkListAsClass(Program.currentProject);

            // 统计钻孔数量
            int countZk = zkList.Count;

            double countZkDepth = 0;
            int countUndisturbedSample = 0, countDisturbedSample = 0;
            int countNTestStandard = 0, countNTestN10 = 0, countNTestN635 = 0, countNTestN120 = 0;
            foreach (Borehole zk in zkList)
            {
                // 统计钻孔进尺
                countZkDepth += zk.Layers[zk.Layers.Count - 1].Depth;
                
                // 统计原状样、扰动样数量
                foreach(ZkSample sample in zk.Samples)
                    if (sample.IsDisturbed)
                        countDisturbedSample++;
                    else
                        countUndisturbedSample++;

                // 统计标贯/动探数量
                foreach (ZkNTest ntest in zk.NTests)
                    if (ntest.Type == ZkNTest.ntype.N)
                        countNTestStandard++;
                    else if (ntest.Type == ZkNTest.ntype.N10)
                        countNTestN10++;
                    else if (ntest.Type == ZkNTest.ntype.N635)
                        countNTestN635++;
                    else
                        countNTestN120++;
            }

            // 读取土工常规试验列表
            List<RoutineSoilTest> rstList = RoutineSoilTestDataBase.ReadAllData(Program.currentProject);

            // 统计常规试验数量、室内渗透试验数量
            int countRST = 0, countPermeability = 0;
            double n = -0.19880205;
            foreach (RoutineSoilTest rst in rstList)
            {
                // 统计室内渗透试验
                if (rst.permeability != n)
                    countPermeability++;

                //统计土工常规试验
                if (rst.waterLevel != n || 
                    rst.voidRatio != n || 
                    rst.specificGravity != n || 
                    rst.saturation != n || 
                    rst.plasticLimit != n || 
                    rst.plasticIndex != n || 
                    rst.modulus != n || 
                    rst.liquidLimit != n || 
                    rst.liquidityIndex != n || 
                    rst.frictionAngle != n || 
                    rst.density != n || 
                    rst.compressibility != n || 
                    rst.cohesion != n)
                    countRST++;
            }

            // 读取颗粒分析试验列表
            List<GrainAnalysisTest> gatList = GrainAnalysisTestDataBase.ReadAllData(Program.currentProject);

            // 统计颗分试验数量
            int countGAT = gatList.Count;

            // 读取静力触探列表
            List<CPT> jkList = CPTDataBase.ReadJkListAsClass(Program.currentProject);

            // 统计钻孔数量
            int countJk = jkList.Count;

            // 统计静力触探进尺
            double countJkDepth = 0;
            foreach (CPT jk in jkList)
                countJkDepth += jk.Layers[jk.Layers.Count - 1].Depth;

            // 赋值给统计列表
            statisticData.CountBorehole = countZk;
            statisticData.CountBoreholePacking = countZk;
            statisticData.Borehole = countZkDepth;
            statisticData.BoreholePacking = countZkDepth;
            statisticData.UndisturbedSample = countUndisturbedSample;
            statisticData.DisturbedSample = countDisturbedSample;
            statisticData.NTestStandard = countNTestStandard;
            statisticData.NTestN10 = countNTestN10;
            statisticData.NTestN635 = countNTestN635;
            statisticData.NTestN120 = countNTestN120;
            statisticData.CountCPT = countJk;
            statisticData.CPT = countJkDepth;
            statisticData.RST = countRST;
            statisticData.Permeability = countPermeability;
            statisticData.GAT = countGAT;

            // 返回
            return statisticData;
        }

        #endregion

        #region 输出

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
            string path = folderPath + @"\" + Program.currentProject + @"-勘察工作量统计表.doc";

            // 启动输出窗体
            ShowProgressBar(path);
        }

        /// <summary>
        /// 输出Word文档函数
        /// </summary>
        /// <param name="_path"></param>
        public static void OutputToWord(string _path)
        {
            // 定义统计数据列表
            StatisticWordLoad statisticData = SelectStatisticData();

            // 输出Word
            Word workLoadStatisticWord = new Word();
            workLoadStatisticWord.AddWorkLoadStatisticTable(statisticData);
            workLoadStatisticWord.SaveAndQuit(_path);
        }

        /// <summary>
        /// 实例化输出进度窗体
        /// </summary>
        /// <param name="obj">输出路径</param>
        private void ShowProgressBar(string _path)
        {
            // 实例化窗体
            OutputProgress prog = new OutputProgress(OutputProgress.OutputType.WordLoad, _path, "输出统计结果", "正在输出勘察工作量统计成果Word文档……");
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
