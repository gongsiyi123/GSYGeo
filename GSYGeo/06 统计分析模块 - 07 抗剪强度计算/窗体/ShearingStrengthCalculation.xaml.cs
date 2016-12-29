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
    /// ShearingStrengthCalculation.xaml 的交互逻辑
    /// </summary>
    public partial class ShearingStrengthCalculation : Window
    {
        #region 参数定义

        /// <summary>
        /// 定义当前正采用的标准
        /// </summary>
        public static string CurrentStandard = SettingDataBase.ReadLocalStandard();

        /// <summary>
        /// 层号
        /// </summary>
        public static string LayerNumber = null;

        /// <summary>
        /// 岩土名称
        /// </summary>
        public static string LayerName = null;

        /// <summary>
        /// 内摩擦角
        /// </summary>
        public static double Friction = Constants.NullNumber;

        /// <summary>
        /// 粘聚力
        /// </summary>
        public static double Cohesion = Constants.NullNumber;

        /// <summary>
        /// 预加载标贯/动探统计列表
        /// </summary>
        static List<StatisticNTest> NTestStatisticList;

        /// <summary>
        /// 预加载试验指标统计列表
        /// </summary>
        static List<StatisticRST> RstStatisticList;

        /// <summary>
        /// 预加载Ps值统计列表
        /// </summary>
        static List<StatisticCPT> CptStatisticList;

        /// <summary>
        /// 抗剪强度统计结构体
        /// </summary>
        public struct ShearingStrength
        {
            public string layerInfo;

            public string RstType;
            public string FrictionByRst;
            public string CohesionByRst;

            public string CptType;
            public string CptParameter;
            public string FrictionByCpt;
            public string CohesionByCpt;

            public string NTestType;
            public string NTestParameter;
            public string FrictionByNTest;
            public string CohesionByNTest;

            public string FrictionFinal;
            public string CohesionFinal;
        }

        /// <summary>
        /// 实例化承载力和压缩模量统计结构体
        /// </summary>
        public static ShearingStrength StatisticResult = new ShearingStrength();

        /// <summary>
        /// 初始化试验指标统计结构体
        /// </summary>
        private static void ClearRstStatisticResult()
        {
            StatisticResult.RstType = "无法识别";
            StatisticResult.FrictionByRst = "/";
            StatisticResult.CohesionByRst = "/";

            StatisticResult.FrictionFinal = "/";
            StatisticResult.CohesionFinal = "/";
        }

        /// <summary>
        /// 初始化Ps值统计结构体
        /// </summary>
        private static void ClearCptStatisticResult()
        {
            StatisticResult.CptType = "无法识别";
            StatisticResult.CptParameter = "/";
            StatisticResult.FrictionByCpt = "/";
            StatisticResult.CohesionByCpt = "/";

            StatisticResult.FrictionFinal = "/";
            StatisticResult.CohesionFinal = "/";
        }

        /// <summary>
        /// 初始化标贯动探统计结构体
        /// </summary>
        private static void ClearNTestStatisticResult()
        {
            StatisticResult.NTestType = "无法识别";
            StatisticResult.NTestParameter = "/";
            StatisticResult.FrictionByNTest = "/";
            StatisticResult.CohesionByNTest = "/";

            StatisticResult.FrictionFinal = "/";
            StatisticResult.CohesionFinal = "/";
        }

        /// <summary>
        ///  实例化输出用承载力和压缩模量统计结构体列表
        /// </summary>
        public static List<ShearingStrength> statisticListOutput = new List<ShearingStrength>();
        
        #endregion

        #region 构造函数

        /// <summary>
        /// 无参数的构造函数
        /// </summary>
        public ShearingStrengthCalculation()
        {
            InitializeComponent();

            // 实例化计算中窗体,执行预加载
            OutputStatisticToWord.ShowCalculatingProgress(OutputProgress.OutputType.PreLoadAll);
            NTestStatisticList = OutputStatisticToWord.NTestStatisticList;
            RstStatisticList = OutputStatisticToWord.RstStatisticList;
            CptStatisticList = OutputStatisticToWord.CptStatisticList;

            // 初始化选框和标签
            InitialStandardTextBlock();
            InitialCPTTypeComboBox();
            InitialNTestTypeComboBox();
            InitialLayerComboBox();
        }

        #endregion

        #region 初始化

        /// <summary>
        /// 填充分层选框
        /// </summary>
        private void InitialLayerComboBox()
        {
            List<string> layerNumberList = ProjectDataBase.ReadLayerNumberList(Program.currentProject);
            List<string> layerNameList = ProjectDataBase.ReadLayerNameList(Program.currentProject);

            List<string> items = new List<string>();
            for (int i = 0; i < layerNumberList.Count; i++)
                items.Add(layerNumberList[i] + " " + layerNameList[i]);

            this.LayerComboBox.ItemsSource = items;
            this.LayerComboBox.SelectedIndex = 0;
        }

        /// <summary>
        /// 填充Ps值土质类型选框
        /// </summary>
        private void InitialCPTTypeComboBox()
        {
            if (CurrentStandard == "Hubei")
                this.CPTTypeComboBox.ItemsSource = HubeiLocalStandardShearingStrength.CptSoilType;
        }

        /// <summary>
        /// 填充标贯/动探土质类型选框
        /// </summary>
        private void InitialNTestTypeComboBox()
        {
            if (CurrentStandard == "Hubei")
                this.NTestTypeComboBox.ItemsSource = HubeiLocalStandardShearingStrength.NTestSoilType;
        }

        /// <summary>
        /// 填充地方标准标签
        /// </summary>
        private void InitialStandardTextBlock()
        {
            if (CurrentStandard == "Hubei")
                this.StandardTextBlock.Text = "执行标准：《岩土工程勘察工作规程》(DB42/169-2003)";
        }

        #endregion

        #region 土质识别

        /// <summary>
        /// 按Ps值查表时的土质类型识别
        /// </summary>
        /// <param name="_soilName">岩土名称</param>
        /// <returns></returns>
        public static string CptSoilType(string _soilName)
        {
            // 根据当前规范查承载力
            if (CurrentStandard == "Hubei")
                return HubeiLocalStandardShearingStrength.SelectCptSoilType(_soilName);

            // 查询不到时返回"无法识别"
            return "无法识别";
        }

        /// <summary>
        /// 按标贯查表时的土质类型识别
        /// </summary>
        /// <param name="_soilName">岩土名称</param>
        /// <returns></returns>
        public static string NTestSoilType(string _soilName)
        {
            // 根据当前规范查承载力
            if (CurrentStandard == "Hubei")
                return HubeiLocalStandardShearingStrength.SelectNTestSoilType(_soilName);

            // 查询不到时返回"无法识别"
            return "无法识别";
        }

        #endregion

        #region 根据试验指标计算

        /// <summary>
        /// 根据试验指标计算抗剪强度函数
        /// </summary>
        public static void CalcuByRst(List<StatisticRST> _rstStatisticList)
        {
            // 没有统计数据时退出
            if (_rstStatisticList == null)
                return;

            // 提取数据库中的试验指标
            StatisticRST.RSTType[] typeList = new StatisticRST.RSTType[]
            {
                StatisticRST.RSTType.frictionAngle,
                StatisticRST.RSTType.cohesion,
            };

            List<double> averageList = new List<double>();

            for (int i = 0; i < _rstStatisticList.Count; i++)
                if (_rstStatisticList[i].Layer == LayerNumber)
                    for (int j = 0; j < typeList.Length; j++)
                        if (_rstStatisticList[i].Type == typeList[j])
                            averageList.Add(_rstStatisticList[i].Average);

            // 初始化试验指标结构体
            ClearRstStatisticResult();

            // 直接赋值抗剪强度
            if (averageList.Count > 0)
                StatisticResult.FrictionByRst = averageList[0] == Constants.NullNumber ? "/" : averageList[0].ToString("0.0");
            else
                StatisticResult.FrictionByRst = "/";
            if (averageList.Count > 1)
                StatisticResult.CohesionByRst = averageList[1] == Constants.NullNumber ? "/" : averageList[1].ToString("0.0");
            else
                StatisticResult.CohesionByRst = "/";
        }

        /// <summary>
        /// 填充各显示框
        /// </summary>
        private void FillRstTextBox()
        {
            this.RSTFrictionTextBox.Text = StatisticResult.FrictionByRst == "/" ? "/" : StatisticResult.FrictionByRst + " °";
            this.RSTCohesionTextBox.Text = StatisticResult.CohesionByRst == "/" ? "/" : StatisticResult.CohesionByRst + " kPa";
        }

        #endregion

        #region 根据Ps值计算

        /// <summary>
        /// 根据Ps值查内摩擦角，输入单参数
        /// </summary>
        /// <param name="_standard">采用的地方标准</param>
        /// <param name="_soilType">土质类型</param>
        /// <param name="_numberRow">输入参数</param>
        /// <returns></returns>
        public static double CalcuFrictionByCpt(string _standard, string _soilType, double _numberRow)
        {
            // 采用湖北省地方标准
            if (_standard == "Hubei")
            {
                // 土质类型为淤泥质土、一般黏性土、老黏性土
                if (_soilType == "淤泥质土、一般黏性土、老黏性土")
                {
                    double[] paraCol = HubeiLocalStandardShearingStrength.ShearingStrengthCptClay.ParaCol;
                    double[] values = HubeiLocalStandardShearingStrength.ShearingStrengthCptClay.FrictionData;
                    return GeoMath.Interpolation(_numberRow, paraCol, values);
                }

                // 土质类型为粉土
                if (_soilType == "粉土")
                {
                    double[] paraRow = HubeiLocalStandardShearingStrength.ShearingStrengthCptSilt.ParaCol;
                    double[] values = HubeiLocalStandardShearingStrength.ShearingStrengthCptSilt.FrictionData;
                    return GeoMath.Interpolation(_numberRow, paraRow, values);
                }
            }

            // 无法查取时返回空值
            return Constants.NullNumber;
        }

        /// <summary>
        /// 根据Ps值查粘聚力，输入单参数
        /// </summary>
        /// <param name="_standard">采用的地方标准</param>
        /// <param name="_soilType">土质类型</param>
        /// <param name="_numberRow">输入参数</param>
        /// <returns></returns>
        public static double CalcuCohesionByCpt(string _standard, string _soilType, double _numberRow)
        {
            // 采用湖北省地方标准
            if (_standard == "Hubei")
            {
                // 土质类型为淤泥质土、一般黏性土、老黏性土
                if (_soilType == "淤泥质土、一般黏性土、老黏性土")
                {
                    double[] paraCol = HubeiLocalStandardShearingStrength.ShearingStrengthCptClay.ParaCol;
                    double[] values = HubeiLocalStandardShearingStrength.ShearingStrengthCptClay.CohesionData;
                    return GeoMath.Interpolation(_numberRow, paraCol, values);
                }

                // 土质类型为粉土
                if (_soilType == "粉土")
                {
                    double[] paraRow = HubeiLocalStandardShearingStrength.ShearingStrengthCptSilt.ParaCol;
                    double[] values = HubeiLocalStandardShearingStrength.ShearingStrengthCptSilt.CohesionData;
                    return GeoMath.Interpolation(_numberRow, paraRow, values);
                }
            }

            // 无法查取时返回空值
            return Constants.NullNumber;
        }

        /// <summary>
        /// 根据Ps计算抗剪强度函数
        /// </summary>
        public static void CalcuByCpt(List<StatisticCPT> _cptStatisticList, string _soilType)
        {
            // 没有统计数据时退出
            if (_cptStatisticList == null)
                return;

            // 初始化Ps值结构体
            ClearCptStatisticResult();

            // 定义统计指标
            double psValue = Constants.NullNumber;

            // 提取数据库中的试验指标
            for (int i = 0; i < _cptStatisticList.Count; i++)
                if (_cptStatisticList[i].Layer == LayerNumber)
                    psValue = _cptStatisticList[i].StandardValue == Constants.NullNumber ? _cptStatisticList[i].Average : _cptStatisticList[i].StandardValue;

            // 按规范和土质类型提取并计算填充指标
            if (CurrentStandard == "Hubei")
            {
                if (psValue != Constants.NullNumber)
                {
                    StatisticResult.CptParameter = "Ps值(MPa)= " + psValue.ToString("0.00");
                    StatisticResult.FrictionByCpt = CalcuFrictionByCpt(CurrentStandard, _soilType, psValue) == Constants.NullNumber ? "/" : CalcuFrictionByCpt(CurrentStandard, _soilType, psValue).ToString("0");
                    StatisticResult.CohesionByCpt = CalcuCohesionByCpt(CurrentStandard, _soilType, psValue) == Constants.NullNumber ? "/" : CalcuCohesionByCpt(CurrentStandard, _soilType, psValue).ToString("0.0");
                }
                else
                {
                    StatisticResult.CptParameter = "/";
                    StatisticResult.FrictionByCpt = "/";
                    StatisticResult.CohesionByCpt = "/";
                }
            }
        }

        /// <summary>
        /// 填充各文本框
        /// </summary>
        private void FillCptTextBox()
        {
            this.CPTParameterTextBox.Text = StatisticResult.CptParameter == "/" ? "/" : StatisticResult.CptParameter + " MPa";
            this.CPTFrictionTextBox.Text = StatisticResult.FrictionByCpt == "/" ? "/" : StatisticResult.FrictionByCpt + " °";
            this.CPTCohesionTextBox.Text = StatisticResult.CohesionByCpt == "/" ? "/" : StatisticResult.CohesionByCpt + " kPa";
        }

        /// <summary>
        /// Ps值土质类型选框选项变化时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CPTTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CalcuByCpt(CptStatisticList, this.CPTTypeComboBox.SelectedItem.ToString());

            FillCptTextBox();
        }

        #endregion

        #region 根据标贯锤击数计算

        /// <summary>
        /// 根据标贯值查内摩擦角，输入单参数
        /// </summary>
        /// <param name="_standard">采用的地方标准</param>
        /// <param name="_soilType">土质类型</param>
        /// <param name="_numberRow">输入参数</param>
        /// <returns></returns>
        public static double CalcuFrictionByNTest(string _standard, string _soilType, double _numberRow)
        {
            // 采用湖北省地方标准
            if (_standard == "Hubei")
            {
                // 土质类型为一般黏性土、老黏性土
                if (_soilType == "一般黏性土、老黏性土")
                {
                    double[] paraCol = HubeiLocalStandardShearingStrength.ShearingStrengthNTestClay.ParaCol;
                    double[] values = HubeiLocalStandardShearingStrength.ShearingStrengthNTestClay.FrictionData;
                    return GeoMath.Interpolation(_numberRow, paraCol, values);
                }

                // 土质类型为砂土
                if (_soilType == "砂土")
                {
                    double[] paraCol = HubeiLocalStandardShearingStrength.ShearingStrengthNTestSand.ParaCol;
                    double[] values = HubeiLocalStandardShearingStrength.ShearingStrengthNTestSand.FrictionData;
                    return GeoMath.Interpolation(_numberRow, paraCol, values);
                }
            }

            // 无法查取时返回空值
            return Constants.NullNumber;
        }

        /// <summary>
        /// 根据标贯值查粘聚力，输入单参数
        /// </summary>
        /// <param name="_standard">采用的地方标准</param>
        /// <param name="_soilType">土质类型</param>
        /// <param name="_numberRow">输入参数</param>
        /// <returns></returns>
        public static double CalcuCohesionByNTest(string _standard, string _soilType, double _numberRow)
        {
            // 采用湖北省地方标准
            if (_standard == "Hubei")
            {
                // 土质类型为一般黏性土、老黏性土
                if (_soilType == "一般黏性土、老黏性土")
                {
                    double[] paraCol = HubeiLocalStandardShearingStrength.ShearingStrengthNTestClay.ParaCol;
                    double[] values = HubeiLocalStandardShearingStrength.ShearingStrengthNTestClay.CohesionData;
                    return GeoMath.Interpolation(_numberRow, paraCol, values);
                }

                // 土质类型为砂土
                if (_soilType == "砂土")
                {
                    return 0;
                }
            }

            // 无法查取时返回空值
            return Constants.NullNumber;
        }

        /// <summary>
        /// 根据标贯计算承载力和压缩模量函数
        /// </summary>
        public static void CalcuByNTest(List<StatisticNTest> _nTestStatisticList, string _soilType)
        {
            // 没有统计数据时退出
            if (_nTestStatisticList == null)
                return;

            // 初始化标贯/动探结构体
            ClearNTestStatisticResult();

            // 定义统计指标
            double ntestValue = Constants.NullNumber;
            ZkNTest.ntype ntestType = ZkNTest.ntype.N;

            // 提取数据库中的试验指标
            for (int i = 0; i < _nTestStatisticList.Count; i++)
                if (_nTestStatisticList[i].Layer == LayerNumber)
                {
                    ntestValue = _nTestStatisticList[i].StandardValue == Constants.NullNumber ? _nTestStatisticList[i].Average : _nTestStatisticList[i].StandardValue;
                    ntestType = _nTestStatisticList[i].Type;
                }

            // 按规范和土质类型提取并计算填充指标
            if (CurrentStandard == "Hubei")
            {
                if (ntestValue != Constants.NullNumber)
                {
                    if (ntestType == ZkNTest.ntype.N)
                    {
                        StatisticResult.NTestParameter = ntestType.ToString() + "(击)= " + ntestValue.ToString("0.0");
                        StatisticResult.FrictionByNTest = CalcuFrictionByNTest(CurrentStandard, _soilType, ntestValue) == Constants.NullNumber ? "/" : CalcuFrictionByNTest(CurrentStandard, _soilType, ntestValue).ToString("0.0");
                        StatisticResult.CohesionByNTest = CalcuCohesionByNTest(CurrentStandard, _soilType, ntestValue) == Constants.NullNumber ? "/" : CalcuCohesionByNTest(CurrentStandard, _soilType, ntestValue).ToString("0.0");
                    }
                    else
                    {
                        StatisticResult.NTestParameter = "/";
                        StatisticResult.FrictionByNTest = "/";
                        StatisticResult.CohesionByNTest = "/";
                    }
                }
                else
                {
                    StatisticResult.NTestParameter = "/";
                    StatisticResult.FrictionByNTest = "/";
                    StatisticResult.CohesionByNTest = "/";
                }
            }
        }

        /// <summary>
        /// 填充各文本框
        /// </summary>
        private void FillNTestTextBox()
        {
            this.NTestParameterTextBox.Text = StatisticResult.NTestParameter == "/" ? "/" : StatisticResult.NTestParameter + " 击";
            this.NTestFrictionTextBox.Text = StatisticResult.FrictionByNTest == "/" ? "/" : StatisticResult.FrictionByNTest + " °";
            this.NTestCohesionTextBox.Text = StatisticResult.CohesionByNTest == "/" ? "/" : StatisticResult.CohesionByNTest + " kPa";
        }

        /// <summary>
        /// 标贯/动探土质类型选框选项变化时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NTestTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CalcuByNTest(NTestStatisticList, this.NTestTypeComboBox.SelectedItem.ToString());

            FillNTestTextBox();
        }

        #endregion

        #region 输出

        /// <summary>
        /// 点击"输出为Word表格"按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OutputToWordButton_Click(object sender, RoutedEventArgs e)
        {
            // 实例化BearingAndModulusToWord窗口类，启动窗口
            ShearingStrengthToWord output = new ShearingStrengthToWord(CurrentStandard);
            output.ShowDialog();

            // 点击"确认并输出"按钮后，启动输出程序
            if (output.DialogResult == true)
            {
                // 计算统计结果
                CalcuOutput(output.dtLayer, RstStatisticList, CptStatisticList, NTestStatisticList);

                // 选择输出目录
                string folderPath;
                System.Windows.Forms.FolderBrowserDialog programPathBrowser = new System.Windows.Forms.FolderBrowserDialog();
                if (programPathBrowser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    folderPath = programPathBrowser.SelectedPath;
                else
                    return;
                string path = folderPath + @"\" + Program.currentProject + @"-抗剪强度综合统计表.doc";

                // 启动输出窗体
                ShowProgressBar(path);
            }
        }

        /// <summary>
        /// 计算统计结果
        /// </summary>
        /// <param name="_dt">从土质类型窗口获取的DataTable</param>
        /// <param name="_rstStaList">试验指标统计列表</param>
        /// <param name="_cptStaList">静力触探摩阻力统计列表</param>
        /// <param name="_ntestStaList">标贯/动探统计列表</param>
        public static void CalcuOutput(DataTable _dt,List<StatisticRST> _rstStaList,List<StatisticCPT> _cptStaList,List<StatisticNTest> _ntestStaList)
        {
            // 清空统计列表结构体
            statisticListOutput.Clear();

            // 赋值统计结构体列表
            for (int i = 0; i < _dt.Rows.Count; i++)
            {
                // 更新当前的分层编号和分层名称
                LayerNumber = ProjectDataBase.ReadLayerNumberList(Program.currentProject)[i];
                LayerName = ProjectDataBase.ReadLayerNameList(Program.currentProject)[i];

                // 获取传递的土质参数
                StatisticResult.layerInfo = _dt.Rows[i]["layerInfo"].ToString();
                StatisticResult.CptType = _dt.Rows[i]["currentCptType"].ToString();
                StatisticResult.NTestType = _dt.Rows[i]["currentNTestType"].ToString();

                // 根据传递的土质类型重新计算参数
                CalcuByRst(_rstStaList);
                CalcuByCpt(_cptStaList, StatisticResult.CptType);
                CalcuByNTest(_ntestStaList, StatisticResult.NTestType);

                // 赋值其他参数
                StatisticResult.CptParameter = StatisticResult.CptParameter == "/" ? "/" : StatisticResult.CptParameter.Substring(StatisticResult.CptParameter.IndexOf("=") + 1);
                StatisticResult.NTestParameter = StatisticResult.NTestParameter == "/" ? "/" : StatisticResult.NTestParameter.Substring(StatisticResult.NTestParameter.IndexOf("=") + 1);

                double[] tmp = new double[3] { 9999, 9999, 9999 };
                double num;

                if (double.TryParse(StatisticResult.FrictionByRst, out num))
                    tmp[0] = num;
                if (double.TryParse(StatisticResult.FrictionByCpt, out num))
                    tmp[1] = num;
                if (double.TryParse(StatisticResult.FrictionByNTest, out num))
                    tmp[2] = num;
                StatisticResult.FrictionFinal = tmp.Min() < 9999 ? tmp.Min().ToString("0") : "/";

                for (int k = 0; k < 3; k++)
                    tmp[k] = 9999;
                if (double.TryParse(StatisticResult.CohesionByRst, out num))
                    tmp[0] = num;
                if (double.TryParse(StatisticResult.CohesionByCpt, out num))
                    tmp[1] = num;
                if (double.TryParse(StatisticResult.CohesionByNTest, out num))
                    tmp[2] = num;
                StatisticResult.CohesionFinal = tmp.Min() < 9999 ? tmp.Min().ToString("0") : "/";

                // 添加到列表
                statisticListOutput.Add(StatisticResult);
            }
        }

        /// <summary>
        /// 输出Word文档函数
        /// </summary>
        /// <param name="_path">输出路径</param>
        public static void OutputToWord(string _path)
        {
            // 输出Word
            Word bamStatisticWord = new Word();
            bamStatisticWord.AddShearingStrengthTable(statisticListOutput);
            bamStatisticWord.SaveAndQuit(_path);
        }

        // 实例化输出进度窗体
        private void ShowProgressBar(string _path)
        {
            OutputProgress prog = new OutputProgress(OutputProgress.OutputType.ShearingStrength, _path, "输出统计结果", "正在输出抗剪强度综合统计成果Word文档……");
            prog.ShowDialog();
        }

        #endregion

        #region 其他

        /// <summary>
        /// 分层选框选项变化时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LayerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 提取当前分层编号和岩土名称
            string s = this.LayerComboBox.SelectedItem.ToString();
            LayerNumber = s.Substring(0, s.IndexOf(" "));
            LayerName = s.Substring(s.IndexOf(" ") + 1);

            // 识别土质
            this.CPTTypeComboBox.Text = CptSoilType(LayerName);
            this.NTestTypeComboBox.Text = NTestSoilType(LayerName);

            // 清空数据
            ClearRstStatisticResult();
            ClearCptStatisticResult();
            ClearNTestStatisticResult();

            // 计算指标
            CalcuByRst(RstStatisticList);
            CalcuByCpt(CptStatisticList, CptSoilType(LayerName));
            CalcuByNTest(NTestStatisticList, NTestSoilType(LayerName));

            // 填充数据
            FillRstTextBox();
            FillCptTextBox();
            FillNTestTextBox();
        }

        #endregion
    }
}
