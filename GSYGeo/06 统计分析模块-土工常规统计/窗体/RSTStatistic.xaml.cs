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
    /// RSTStatistic.xaml 的交互逻辑
    /// </summary>
    public partial class RSTStatistic : Window
    {
        #region 参数定义

        // 定义数据统计列表RSTStatisticDataGrid控件的数据源DataTable
        DataTable dtRST = new DataTable("Statistic");

        #endregion

        #region 构造函数

        public RSTStatistic()
        {
            InitializeComponent();

            // 初始化DataTable
            InitialRSTStatisticDataGrid();

            // 设置绑定
            this.RSTStatisticDataGrid.DataContext = dtRST;

            // 填充筛选ComboBox
            InitialComboBox();
            this.LayerNumberComboBox.SelectedIndex = 0;
        }

        #endregion

        #region 统计数据

        // 定义统计项目
        private string[] staName = new string[]
        {
            "statisticType",
            "waterLevel",
            "density",
            "specificGravity",
            "voidRatio",
            "saturation",
            "liquidLimit",
            "plasticLimit",
            "plastticIndex",
            "liquidityIndex",
            "compressibility",
            "modulus",
            "frictionAngle",
            "cohesion",
            "permeability"
        };
        private string[] staType = new string[]
        {
            "统计数",
            "最大值",
            "最小值",
            "平均值",
            "标准差",
            "变异系数",
            "统计修正系数",
            "标准值"
        };

        // 试验类型列表
        public static StatisticRST.RSTType[] typeList = new StatisticRST.RSTType[]
        {
                    StatisticRST.RSTType.waterLevel,
                    StatisticRST.RSTType.density,
                    StatisticRST.RSTType.specificGravity,
                    StatisticRST.RSTType.voidRatio,
                    StatisticRST.RSTType.saturation,
                    StatisticRST.RSTType.liquidLimit,
                    StatisticRST.RSTType.plasticLimit,
                    StatisticRST.RSTType.plasticIndex,
                    StatisticRST.RSTType.liquidityIndex,
                    StatisticRST.RSTType.compressibility,
                    StatisticRST.RSTType.modulus,
                    StatisticRST.RSTType.frictionAngle,
                    StatisticRST.RSTType.cohesion,
                    StatisticRST.RSTType.permeability
        };

        // 定义统计数据列表
        List<StatisticRST> statisticList;

        // 初始化RSTStatisticDataGrid，不带参数
        private void InitialRSTStatisticDataGrid()
        {
            // 初始化统计数据列表
            statisticList = SelectStatisticData();

            // 定义RSTStatisticDataGrid数据列
            foreach (string sta in staName)
            {
                dtRST.Columns.Add(new DataColumn(sta, typeof(string)));
            }

            // 将统计项目列表赋值给DataTable
            DataRow dr;
            for (int i = 0; i < 8; i++)
            {
                dr = dtRST.NewRow();
                dr[0] = staType[i];
                dtRST.Rows.Add(dr);
            }
        }

        // 根据所选分层刷新DataTable
        private void RefreshDataTableByLayer(string _layerNumber)
        {
            // 恢复初始状态
            dtRST.Clear();
            DataRow dr;
            for (int i = 0; i < 8; i++)
            {
                dr = dtRST.NewRow();
                dr[0] = staType[i];
                dtRST.Rows.Add(dr);
            }
            
            // 按顺序赋值各统计项目列表
            List<double> countList = new List<double>();
            List<double> maxList = new List<double>();
            List<double> minList = new List<double>();
            List<double> averageList = new List<double>();
            List<double> standardDeviationList = new List<double>();
            List<double> variableCoefficientList = new List<double>();
            List<double> correctionCoefficientList = new List<double>();
            List<double> standardValue = new List<double>();
            for (int i = 0; i < statisticList.Count; i++)
            {
                if (statisticList[i].Layer == _layerNumber)
                {
                    for(int j = 0; j < typeList.Length; j++)
                    {
                        if (statisticList[i].Type == typeList[j])
                        {
                            countList.Add(statisticList[i].Count);
                            maxList.Add(statisticList[i].Max);
                            minList.Add(statisticList[i].Min);
                            averageList.Add(statisticList[i].Average);
                            standardDeviationList.Add(statisticList[i].StandardDeviation);
                            variableCoefficientList.Add(statisticList[i].VariableCoefficient);
                            correctionCoefficientList.Add(statisticList[i].CorrectionCoefficient);
                            standardValue.Add(statisticList[i].StandardValue);
                        }
                    }
                }
            }

            // 无数据时退出
            if (countList.Count == 0)
            {
                return;
            }

            // 赋值DataTable
            for (int j = 1; j <= 14; j++)
            {
                if (countList[j - 1] == 0)
                {
                    dtRST.Rows[0][j] = "/";
                    dtRST.Rows[1][j] = "/";
                    dtRST.Rows[2][j] = "/";
                    dtRST.Rows[3][j] = "/";
                    dtRST.Rows[4][j] = "/";
                    dtRST.Rows[5][j] = "/";
                    dtRST.Rows[6][j] = "/";
                    dtRST.Rows[7][j] = "/";
                }
                else if (j == 1 || j == 5 || j == 6 || j == 7 || j == 8 || j == 11 || j == 12 || j == 13)
                {
                    dtRST.Rows[0][j] = countList[j - 1].ToString("0");
                    dtRST.Rows[1][j] = maxList[j - 1].ToString("0.0");
                    dtRST.Rows[2][j] = minList[j - 1].ToString("0.0");
                    dtRST.Rows[3][j] = averageList[j - 1].ToString("0.0");
                    dtRST.Rows[4][j] = standardDeviationList[j - 1].ToString() == "-0.19880205" ? "/" : standardDeviationList[j - 1].ToString("0.0");
                    dtRST.Rows[5][j] = variableCoefficientList[j - 1].ToString() == "-0.19880205" ? "/" : variableCoefficientList[j - 1].ToString("0.00");
                    dtRST.Rows[6][j] = correctionCoefficientList[j - 1].ToString() == "-0.19880205" ? "/" : correctionCoefficientList[j - 1].ToString("0.00");
                    dtRST.Rows[7][j] = standardValue[j - 1].ToString() == "-0.19880205" ? "/" : standardValue[j - 1].ToString("0.0");
                }
                else if (j == 2 || j == 3 || j == 4 || j == 9 || j == 10)
                {
                    dtRST.Rows[0][j] = countList[j - 1].ToString("0");
                    dtRST.Rows[1][j] = maxList[j - 1].ToString("0.00");
                    dtRST.Rows[2][j] = minList[j - 1].ToString("0.00");
                    dtRST.Rows[3][j] = averageList[j - 1].ToString("0.00");
                    dtRST.Rows[4][j] = standardDeviationList[j - 1].ToString() == "-0.19880205" ? "/" : standardDeviationList[j - 1].ToString("0.00");
                    dtRST.Rows[5][j] = variableCoefficientList[j - 1].ToString() == "-0.19880205" ? "/" : variableCoefficientList[j - 1].ToString("0.00");
                    dtRST.Rows[6][j] = correctionCoefficientList[j - 1].ToString() == "-0.19880205" ? "/" : correctionCoefficientList[j - 1].ToString("0.00");
                    dtRST.Rows[7][j] = standardValue[j - 1].ToString() == "-0.19880205" ? "/" : standardValue[j - 1].ToString("0.00");
                }
                else
                {
                    dtRST.Rows[0][j] = countList[j - 1].ToString("0");
                    dtRST.Rows[1][j] = maxList[j - 1].ToString() == "-0.19880205" ? "/" : maxList[j - 1].ToString("0.0E0");
                    dtRST.Rows[2][j] = minList[j - 1].ToString() == "-0.19880205" ? "/" : minList[j - 1].ToString("0.0E0");
                    dtRST.Rows[3][j] = averageList[j - 1].ToString() == "-0.19880205" ? "/" : averageList[j - 1].ToString("0.0E0");
                    dtRST.Rows[4][j] = standardDeviationList[j - 1].ToString() == "-0.19880205" ? "/" : standardDeviationList[j - 1].ToString("0.0E0");
                    dtRST.Rows[5][j] = variableCoefficientList[j - 1].ToString() == "-0.19880205" ? "/" : variableCoefficientList[j - 1].ToString("0.00");
                    dtRST.Rows[6][j] = correctionCoefficientList[j - 1].ToString() == "-0.19880205" ? "/" : correctionCoefficientList[j - 1].ToString("0.00");
                    dtRST.Rows[7][j] = standardValue[j - 1].ToString() == "-0.19880205" ? "/" : standardValue[j - 1].ToString("0.0E0");
                }
            }
        }

        /// <summary>
        /// 筛选统计数据
        /// </summary>
        /// <returns></returns>
        public static List<StatisticRST> SelectStatisticData()
        {
            // 定义统计数据列表
            List<StatisticRST> statisticList = new List<StatisticRST>();

            // 定义分层编号列表、分层岩土名称列表、试验类型列表
            List<string> layerNumberList = ProjectDataBase.ReadLayerNumberList(Program.currentProject);
            List<string> layerNameList = ProjectDataBase.ReadLayerNameList(Program.currentProject);

            // 在分层编号列表中循环，赋值统计数据列表
            for(int i = 0; i < layerNumberList.Count; i++)
            {
                // 层号、岩土名称
                string layerNumber = layerNumberList[i];
                string layerName = layerNameList[i];
                
                // 读取分层试验数据
                List<RoutineSoilTest> rsts = RoutineSoilTestDataBase.SelectByZkAndLayer(Program.currentProject, "", layerNumber);

                List<double> datalist_waterLevel = new List<double>();
                List<double> datalist_density = new List<double>();
                List<double> datalist_specificGravity = new List<double>();
                List<double> datalist_voidRatio = new List<double>();
                List<double> datalist_saturation = new List<double>();
                List<double> datalist_liquidLimit = new List<double>();
                List<double> datalist_plasticLimit = new List<double>();
                List<double> datalist_plasticIndex = new List<double>();
                List<double> datalist_liquidityIndex = new List<double>();
                List<double> datalist_compressibility = new List<double>();
                List<double> datalist_modulus = new List<double>();
                List<double> datalist_frictionAngle = new List<double>();
                List<double> datalist_cohesion = new List<double>();
                List<double> datalist_permeability = new List<double>();

                for(int j = 0; j < rsts.Count; j++)
                {
                    datalist_waterLevel.Add(rsts[j].waterLevel);
                    datalist_density.Add(rsts[j].density);
                    datalist_specificGravity.Add(rsts[j].specificGravity);
                    datalist_voidRatio.Add(rsts[j].voidRatio);
                    datalist_saturation.Add(rsts[j].saturation);
                    datalist_liquidLimit.Add(rsts[j].liquidLimit);
                    datalist_plasticLimit.Add(rsts[j].plasticLimit);
                    datalist_plasticIndex.Add(rsts[j].plasticIndex);
                    datalist_liquidityIndex.Add(rsts[j].liquidityIndex);
                    datalist_compressibility.Add(rsts[j].compressibility);
                    datalist_modulus.Add(rsts[j].modulus);
                    datalist_frictionAngle.Add(rsts[j].frictionAngle);
                    datalist_cohesion.Add(rsts[j].cohesion);
                    datalist_permeability.Add(rsts[j].permeability);
                }

                // 赋值统计数据列表
                //for(int j = 0; j < rsts.Count; j++)
                //{
                    statisticList.Add(new StatisticRST(layerNumber, layerName, typeList[0], datalist_waterLevel, 6));
                    statisticList.Add(new StatisticRST(layerNumber, layerName, typeList[1], datalist_density, 6));
                    statisticList.Add(new StatisticRST(layerNumber, layerName, typeList[2], datalist_specificGravity, 6));
                    statisticList.Add(new StatisticRST(layerNumber, layerName, typeList[3], datalist_voidRatio, 6));
                    statisticList.Add(new StatisticRST(layerNumber, layerName, typeList[4], datalist_saturation, 6));
                    statisticList.Add(new StatisticRST(layerNumber, layerName, typeList[5], datalist_liquidLimit, 6));
                    statisticList.Add(new StatisticRST(layerNumber, layerName, typeList[6], datalist_plasticLimit, 6));
                    statisticList.Add(new StatisticRST(layerNumber, layerName, typeList[7], datalist_plasticIndex, 6));
                    statisticList.Add(new StatisticRST(layerNumber, layerName, typeList[8], datalist_liquidityIndex, 6));
                    statisticList.Add(new StatisticRST(layerNumber, layerName, typeList[9], datalist_compressibility, 6));
                    statisticList.Add(new StatisticRST(layerNumber, layerName, typeList[10], datalist_modulus, 6));
                    statisticList.Add(new StatisticRST(layerNumber, layerName, typeList[11], datalist_frictionAngle, 6));
                    statisticList.Add(new StatisticRST(layerNumber, layerName, typeList[12], datalist_cohesion, 6));
                    statisticList.Add(new StatisticRST(layerNumber, layerName, typeList[13], datalist_permeability, 6));
                //}
                
            }

            // 返回
            return statisticList;
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

            RefreshDataTableByLayer(selectedLayer);
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
            object path = folderPath + @"\" + Program.currentProject + @"-土工常规试验统计.doc";

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
            List<StatisticRST> statisticList = SelectStatisticData();

            // 输出Word
            Word rstStatisticWord = new Word();
            rstStatisticWord.AddRSTStatisticTable(statisticList);
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
            OutputProgress prog = new OutputProgress(OutputProgress.OutputType.RST, path, "输出统计结果", "正在输出土工常规试验统计成果Word文档……");
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
