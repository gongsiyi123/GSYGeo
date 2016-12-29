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
    /// BearingAndModulusCalculation.xaml 的交互逻辑
    /// </summary>
    public partial class BearingAndModulusCalculation : Window
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
        /// 承载力
        /// </summary>
        public static double Bearing = Constants.NullNumber;

        /// <summary>
        /// 压缩模量
        /// </summary>
        public static double Modulus = Constants.NullNumber;

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
        /// 承载力和压缩模量统计结构体
        /// </summary>
        public struct BearingAndModulus
        {
            public string layerInfo;

            public string RstType;
            public string BearingByRst;
            public string ModulusByRst;

            public string CptType;
            public string CptParameter;
            public string BearingByCpt;
            public string ModulusByCpt;

            public string NTestType;
            public string NTestParameter;
            public string BearingByNTest;
            public string ModulusByNTest;

            public string BearingFinal;
            public string ModulusFinal;
        }

        /// <summary>
        /// 实例化承载力和压缩模量统计结构体
        /// </summary>
        public static BearingAndModulus StatisticResult = new BearingAndModulus();

        /// <summary>
        /// 初始化试验指标统计结构体
        /// </summary>
        private static void ClearRstStatisticResult()
        {
            StatisticResult.RstType = "无法识别";
            StatisticResult.BearingByRst = "/";
            StatisticResult.ModulusByRst = "/";
            
            StatisticResult.BearingFinal = "/";
            StatisticResult.ModulusFinal = "/";
        }

        /// <summary>
        /// 初始化Ps值统计结构体
        /// </summary>
        private static void ClearCptStatisticResult()
        {
            StatisticResult.CptType = "无法识别";
            StatisticResult.CptParameter = "/";
            StatisticResult.BearingByCpt = "/";
            StatisticResult.ModulusByCpt = "/";

            StatisticResult.BearingFinal = "/";
            StatisticResult.ModulusFinal = "/";
        }

        /// <summary>
        /// 初始化标贯动探统计结构体
        /// </summary>
        private static void ClearNTestStatisticResult()
        {
            StatisticResult.NTestType = "无法识别";
            StatisticResult.NTestParameter = "/";
            StatisticResult.BearingByNTest = "/";
            StatisticResult.ModulusByNTest = "/";

            StatisticResult.BearingFinal = "/";
            StatisticResult.ModulusFinal = "/";
        }
        
        /// <summary>
        ///  实例化输出用承载力和压缩模量统计结构体列表
        /// </summary>
        public static List<BearingAndModulus> statisticListOutput = new List<BearingAndModulus>();

        /// <summary>
        /// 临时存储土工试验指标字符串
        /// </summary>
        private static string RstParameter = "/";

        #endregion

        #region 构造函数

        /// <summary>
        /// 无参数的构造函数
        /// </summary>
        public BearingAndModulusCalculation()
        {
            InitializeComponent();

            // 实例化计算中窗体,执行预加载
            OutputStatisticToWord.ShowCalculatingProgress(OutputProgress.OutputType.PreLoadAll);
            NTestStatisticList = OutputStatisticToWord.NTestStatisticList;
            RstStatisticList = OutputStatisticToWord.RstStatisticList;
            CptStatisticList = OutputStatisticToWord.CptStatisticList;

            // 初始化选框和标签
            InitialStandardTextBlock();
            InitialRSTTypeComboBox();
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
            for(int i = 0; i < layerNumberList.Count; i++)
                items.Add(layerNumberList[i] + " " + layerNameList[i]);

            this.LayerComboBox.ItemsSource = items;
            this.LayerComboBox.SelectedIndex = 0;
        }

        /// <summary>
        /// 填充试验指标土质类型选框
        /// </summary>
        private void InitialRSTTypeComboBox()
        {
            if (CurrentStandard == "Hubei")
                this.RSTTypeComboBox.ItemsSource = HubeiLocalStandardBearingAndModulus.RstSoilType;
        }

        /// <summary>
        /// 填充Ps值土质类型选框
        /// </summary>
        private void InitialCPTTypeComboBox()
        {
            if (CurrentStandard == "Hubei")
                this.CPTTypeComboBox.ItemsSource = HubeiLocalStandardBearingAndModulus.CptSoilType;
        }

        /// <summary>
        /// 填充标贯/动探土质类型选框
        /// </summary>
        private void InitialNTestTypeComboBox()
        {
            if (CurrentStandard == "Hubei")
                this.NTestTypeComboBox.ItemsSource = HubeiLocalStandardBearingAndModulus.NTestSoilType;
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
        /// 按试验指标查表时的土质类型识别
        /// </summary>
        /// <param name="_soilName">岩土名称</param>
        /// <returns></returns>
        public static string RstSoilType(string _soilName)
        {
            // 根据当前规范查承载力
            if (CurrentStandard == "Hubei")
                return HubeiLocalStandardBearingAndModulus.SelectRstSoilType(_soilName);

            // 查询不到时返回"无法识别"
            return "无法识别";
        }

        /// <summary>
        /// 按Ps值查表时的土质类型识别
        /// </summary>
        /// <param name="_soilName">岩土名称</param>
        /// <returns></returns>
        public static string CptSoilType(string _soilName)
        {
            // 根据当前规范查承载力
            if (CurrentStandard == "Hubei")
                return HubeiLocalStandardBearingAndModulus.SelectCptSoilType(_soilName);

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
                return HubeiLocalStandardBearingAndModulus.SelectNTestSoilType(_soilName);

            // 查询不到时返回"无法识别"
            return "无法识别";
        }

        #endregion

        #region 根据试验指标计算

        /// <summary>
        /// 根据试验指标查承载力，输入单参数
        /// </summary>
        /// <param name="_standard">采用的地方标准</param>
        /// <param name="_soilType">土质类型</param>
        /// <param name="_numberRow">输入参数</param>
        /// <returns></returns>
        public static double CalcuBearingByRst(string _standard, string _soilType, double _numberRow)
        {
            // 采用湖北省地方标准
            if (_standard == "Hubei")
            {
                // 土质类型为老黏性土
                if (_soilType == "老黏性土")
                {
                    double[] paraCol = HubeiLocalStandardBearingAndModulus.BearingRstOldClay.ParaCol;
                    double[] values = HubeiLocalStandardBearingAndModulus.BearingRstOldClay.Data;
                    return GeoMath.Interpolation(_numberRow, paraCol, values);
                }

                // 土质类型为淤泥、淤泥质土
                if (_soilType == "淤泥、淤泥质土")
                {
                    double[] paraRow = HubeiLocalStandardBearingAndModulus.BearingRstSludge.ParaCol;
                    double[] values = HubeiLocalStandardBearingAndModulus.BearingRstSludge.Data;
                    return GeoMath.Interpolation(_numberRow, paraRow, values);
                }
            }

            // 无法查取时返回空值
            return Constants.NullNumber;
        }

        /// <summary>
        /// 根据试验指标查承载力，输入双参数
        /// </summary>
        /// <param name="_standard">采用的地方标准</param>
        /// <param name="_soilType">土质类型</param>
        /// <param name="_numberRow">输入行参数</param>
        /// <param name="_numberCol">输入列参数</param>
        /// <returns></returns>
        public static double CalcuBearingByRst(string _standard,string _soilType,double _numberRow,double _numberCol)
        {
            // 采用湖北省地方标准
            if (_standard == "Hubei")
            {
                // 土质类型为一般黏性土
                if (_soilType == "一般黏性土")
                {
                    double[] paraRow = HubeiLocalStandardBearingAndModulus.BearingRstClay.ParaRow;
                    double[] paraCol = HubeiLocalStandardBearingAndModulus.BearingRstClay.ParaCol;
                    double[,] values = HubeiLocalStandardBearingAndModulus.BearingRstClay.Data;
                    return GeoMath.TwoWayInterpolation(_numberRow, _numberCol, paraRow, paraCol, values);
                }

                // 土质类型为新近沉积黏性土
                if (_soilType == "新近沉积黏性土")
                {
                    double[] paraRow = HubeiLocalStandardBearingAndModulus.BearingRstNewClay.ParaRow;
                    double[] paraCol = HubeiLocalStandardBearingAndModulus.BearingRstNewClay.ParaCol;
                    double[,] values = HubeiLocalStandardBearingAndModulus.BearingRstNewClay.Data;
                    return GeoMath.TwoWayInterpolation(_numberRow, _numberCol, paraRow, paraCol, values);
                }

                // 土质类型为粉土
                if (_soilType == "粉土")
                {
                    double[] paraRow = HubeiLocalStandardBearingAndModulus.BearingRstSilt.ParaRow;
                    double[] paraCol = HubeiLocalStandardBearingAndModulus.BearingRstSilt.ParaCol;
                    double[,] values = HubeiLocalStandardBearingAndModulus.BearingRstSilt.Data;
                    return GeoMath.TwoWayInterpolation(_numberRow, _numberCol, paraRow, paraCol, values);
                }
            }

            // 无法查取时返回空值
            return Constants.NullNumber;
        }

        /// <summary>
        /// 根据试验指标计算承载力函数
        /// </summary>
        public static void CalcuByRst(List<StatisticRST> _rstStatisticList,string _soilType)
        {
            // 没有统计数据时退出
            if (_rstStatisticList == null)
                return;
            
            // 提取数据库中的试验指标
            StatisticRST.RSTType[] typeList = new StatisticRST.RSTType[]
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

            List<double> countList = new List<double>();
            List<double> maxList = new List<double>();
            List<double> minList = new List<double>();
            List<double> averageList = new List<double>();
            List<double> standardDeviationList = new List<double>();
            List<double> variableCoefficientList = new List<double>();
            List<double> correctionCoefficientList = new List<double>();
            List<double> standardValue = new List<double>();

            for (int i = 0; i < _rstStatisticList.Count; i++)
                if (_rstStatisticList[i].Layer == LayerNumber)
                    for (int j = 0; j < typeList.Length; j++)
                        if (_rstStatisticList[i].Type == typeList[j])
                        {
                            countList.Add(_rstStatisticList[i].Count);
                            maxList.Add(_rstStatisticList[i].Max);
                            minList.Add(_rstStatisticList[i].Min);
                            averageList.Add(_rstStatisticList[i].Average);
                            standardDeviationList.Add(_rstStatisticList[i].StandardDeviation);
                            variableCoefficientList.Add(_rstStatisticList[i].VariableCoefficient);
                            correctionCoefficientList.Add(_rstStatisticList[i].CorrectionCoefficient);
                            standardValue.Add(_rstStatisticList[i].StandardValue);
                        }

            // 初始化试验指标结构体
            ClearRstStatisticResult();

            // 直接赋值模量
            StatisticResult.ModulusByRst = averageList[10] == Constants.NullNumber ? "/" : averageList[10].ToString("0.0");
            
            // 定义统计指标
            double paraRow = Constants.NullNumber;
            double paraCol = Constants.NullNumber;

            // 按规范和土质类型提取并计算填充指标
            if (CurrentStandard == "Hubei")
            {
                if (_soilType == "一般黏性土")
                {
                    paraRow = averageList[3];
                    paraCol = averageList[8];

                    if (paraRow != Constants.NullNumber && paraCol != Constants.NullNumber)
                        RstParameter = "孔隙比=" + paraRow.ToString("0.00") + "\n液性指数=" + paraCol.ToString("0.00");
                    else
                        RstParameter = "/";

                    StatisticResult.BearingByRst = CalcuBearingByRst(CurrentStandard, _soilType, paraRow, paraCol) == Constants.NullNumber ? "/" : CalcuBearingByRst(CurrentStandard, _soilType, paraRow, paraCol).ToString("0");
                }
                else if (_soilType == "新近沉积黏性土")
                {
                    paraRow = averageList[3];
                    paraCol = averageList[8];

                    if (paraRow != Constants.NullNumber && paraCol != Constants.NullNumber)
                        RstParameter = "孔隙比=" + paraRow.ToString("0.00") + "\n液性指数=" + paraCol.ToString("0.00");
                    else
                        RstParameter = "/";

                    StatisticResult.BearingByRst = CalcuBearingByRst(CurrentStandard, _soilType, paraRow, paraCol) == Constants.NullNumber ? "/" : CalcuBearingByRst(CurrentStandard, _soilType, paraRow, paraCol).ToString("0");
                }
                else if (_soilType == "老黏性土")
                {
                    paraRow = averageList[0] / averageList[5];

                    if (paraRow != Constants.NullNumber)
                        RstParameter = "含水比=" + paraRow.ToString("0.00");
                    else
                        RstParameter = "/";

                    StatisticResult.BearingByRst = CalcuBearingByRst(CurrentStandard, _soilType, paraRow) == Constants.NullNumber ? "/" : CalcuBearingByRst(CurrentStandard, _soilType, paraRow).ToString("0");
                }
                else if (_soilType == "淤泥、淤泥质土")
                {
                    paraRow = averageList[0];

                    if (paraRow != Constants.NullNumber)
                        RstParameter = "含水量=" + paraRow.ToString("0.0");
                    else
                        RstParameter = "/";

                    StatisticResult.BearingByRst = CalcuBearingByRst(CurrentStandard, _soilType, paraRow) == Constants.NullNumber ? "/" : CalcuBearingByRst(CurrentStandard, _soilType, paraRow).ToString("0");
                }
                else if (_soilType == "粉土")
                {
                    paraRow = averageList[3];
                    paraCol = averageList[0];

                    if (paraRow != Constants.NullNumber && paraCol != Constants.NullNumber)
                        RstParameter = "孔隙比=" + paraRow.ToString("0.0") + "\n含水量=" + paraCol.ToString("0.00");
                    else
                        RstParameter = "/";

                    StatisticResult.BearingByRst = CalcuBearingByRst(CurrentStandard, _soilType, paraRow, paraCol) == Constants.NullNumber ? "/" : CalcuBearingByRst(CurrentStandard, _soilType, paraRow, paraCol).ToString("0");
                }
                else
                {
                    RstParameter = "/";
                    StatisticResult.BearingByRst = "/";
                }
            }
        }

        /// <summary>
        /// 填充各显示框
        /// </summary>
        private void FillRstTextBox()
        {
            this.RSTParameterTextBox.Text = RstParameter;
            this.RSTBearingTextBox.Text = StatisticResult.BearingByRst == "/" ? "/" : StatisticResult.BearingByRst + " kPa";
            this.RSTModulusTextBox.Text = StatisticResult.ModulusByRst == "/" ? "/" : StatisticResult.ModulusByRst + " MPa";
        }
        
        /// <summary>
        /// 试验指标土质类型选框选项变化时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RSTTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CalcuByRst(RstStatisticList, this.RSTTypeComboBox.SelectedItem.ToString());

            FillRstTextBox();
        }

        #endregion

        #region 根据Ps值计算

        /// <summary>
        /// 根据Ps值查承载力，输入单参数
        /// </summary>
        /// <param name="_standard">采用的地方标准</param>
        /// <param name="_soilType">土质类型</param>
        /// <param name="_numberRow">输入参数</param>
        /// <returns></returns>
        public static double CalcuBearingByCpt(string _standard, string _soilType, double _numberRow)
        {
            // 采用湖北省地方标准
            if (_standard == "Hubei")
            {
                // 土质类型为老黏性土
                if (_soilType == "一般黏性土")
                {
                    double[] paraCol = HubeiLocalStandardBearingAndModulus.BearingAndModulusCptClay.ParaCol;
                    double[] values = HubeiLocalStandardBearingAndModulus.BearingAndModulusCptClay.BearingData;
                    return GeoMath.Interpolation(_numberRow, paraCol, values);
                }

                // 土质类型为老黏性土
                if (_soilType == "老黏性土")
                {
                    double[] paraRow = HubeiLocalStandardBearingAndModulus.BearingAndModulusCptOldClay.ParaCol;
                    double[] values = HubeiLocalStandardBearingAndModulus.BearingAndModulusCptOldClay.BearingData;
                    return GeoMath.Interpolation(_numberRow, paraRow, values);
                }

                // 土质类型为粉土
                if (_soilType == "粉土")
                {
                    double[] paraRow = HubeiLocalStandardBearingAndModulus.BearingAndModulusCptSilt.ParaCol;
                    double[] values = HubeiLocalStandardBearingAndModulus.BearingAndModulusCptSilt.BearingData;
                    return GeoMath.Interpolation(_numberRow, paraRow, values);
                }

                // 土质类型为素填土
                if (_soilType == "素填土")
                {
                    double[] paraRow = HubeiLocalStandardBearingAndModulus.BearingAndModulusCptFill.ParaCol;
                    double[] values = HubeiLocalStandardBearingAndModulus.BearingAndModulusCptFill.BearingData;
                    return GeoMath.Interpolation(_numberRow, paraRow, values);
                }

                // 土质类型为粉细砂
                if (_soilType == "粉、细砂土")
                {
                    double[] paraRow = HubeiLocalStandardBearingAndModulus.BearingAndModulusCptFineSand.ParaCol;
                    double[] values = HubeiLocalStandardBearingAndModulus.BearingAndModulusCptFineSand.BearingData;
                    return GeoMath.Interpolation(_numberRow, paraRow, values);
                }

                // 土质类型为中粗砂
                if (_soilType == "中、粗砂土")
                {
                    double[] paraRow = HubeiLocalStandardBearingAndModulus.BearingAndModulusCptCoarseSand.ParaCol;
                    double[] values = HubeiLocalStandardBearingAndModulus.BearingAndModulusCptCoarseSand.BearingData;
                    return GeoMath.Interpolation(_numberRow, paraRow, values);
                }
            }

            // 无法查取时返回空值
            return Constants.NullNumber;
        }

        /// <summary>
        /// 根据Ps值查压缩模量，输入单参数
        /// </summary>
        /// <param name="_standard">采用的地方标准</param>
        /// <param name="_soilType">土质类型</param>
        /// <param name="_numberRow">输入参数</param>
        /// <returns></returns>
        public static double CalcuModulusByCpt(string _standard, string _soilType, double _numberRow)
        {
            // 采用湖北省地方标准
            if (_standard == "Hubei")
            {
                // 土质类型为老黏性土
                if (_soilType == "一般黏性土")
                {
                    double[] paraCol = HubeiLocalStandardBearingAndModulus.BearingAndModulusCptClay.ParaCol;
                    double[] values = HubeiLocalStandardBearingAndModulus.BearingAndModulusCptClay.ModulusData;
                    return GeoMath.Interpolation(_numberRow, paraCol, values);
                }

                // 土质类型为老黏性土
                if (_soilType == "老黏性土")
                {
                    double[] paraRow = HubeiLocalStandardBearingAndModulus.BearingAndModulusCptOldClay.ParaCol;
                    double[] values = HubeiLocalStandardBearingAndModulus.BearingAndModulusCptOldClay.ModulusData;
                    return GeoMath.Interpolation(_numberRow, paraRow, values);
                }

                // 土质类型为粉土
                if (_soilType == "粉土")
                {
                    double[] paraRow = HubeiLocalStandardBearingAndModulus.BearingAndModulusCptSilt.ParaCol;
                    double[] values = HubeiLocalStandardBearingAndModulus.BearingAndModulusCptSilt.ModulusData;
                    return GeoMath.Interpolation(_numberRow, paraRow, values);
                }

                // 土质类型为素填土
                if (_soilType == "素填土")
                {
                    double[] paraRow = HubeiLocalStandardBearingAndModulus.BearingAndModulusCptFill.ParaCol;
                    double[] values = HubeiLocalStandardBearingAndModulus.BearingAndModulusCptFill.ModulusData;
                    return GeoMath.Interpolation(_numberRow, paraRow, values);
                }

                // 土质类型为粉细砂
                if (_soilType == "粉、细砂土")
                {
                    double[] paraRow = HubeiLocalStandardBearingAndModulus.BearingAndModulusCptFineSand.ParaCol;
                    double[] values = HubeiLocalStandardBearingAndModulus.BearingAndModulusCptFineSand.ModulusData;
                    return GeoMath.Interpolation(_numberRow, paraRow, values);
                }

                // 土质类型为中粗砂
                if (_soilType == "中、粗砂土")
                {
                    double[] paraRow = HubeiLocalStandardBearingAndModulus.BearingAndModulusCptCoarseSand.ParaCol;
                    double[] values = HubeiLocalStandardBearingAndModulus.BearingAndModulusCptCoarseSand.ModulusData;
                    return GeoMath.Interpolation(_numberRow, paraRow, values);
                }
            }

            // 无法查取时返回空值
            return Constants.NullNumber;
        }

        /// <summary>
        /// 根据Ps计算承载力和压缩模量函数
        /// </summary>
        public static void CalcuByCpt(List<StatisticCPT> _cptStatisticList,string _soilType)
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
                    StatisticResult.BearingByCpt = CalcuBearingByCpt(CurrentStandard, _soilType, psValue) == Constants.NullNumber ? "/" : CalcuBearingByCpt(CurrentStandard, _soilType, psValue).ToString("0");
                    StatisticResult.ModulusByCpt = CalcuModulusByCpt(CurrentStandard, _soilType, psValue) == Constants.NullNumber ? "/" : CalcuModulusByCpt(CurrentStandard, _soilType, psValue).ToString("0.0");
                }
                else
                {
                    StatisticResult.CptParameter = "/";
                    StatisticResult.BearingByCpt = "/";
                    StatisticResult.ModulusByCpt = "/";
                }
            }
        }

        /// <summary>
        /// 填充各文本框
        /// </summary>
        private void FillCptTextBox()
        {
            this.CPTParameterTextBox.Text = StatisticResult.CptParameter == "/" ? "/" : StatisticResult.CptParameter + " MPa";
            this.CPTBearingTextBox.Text = StatisticResult.BearingByCpt == "/" ? "/" : StatisticResult.BearingByCpt + " kPa";
            this.CPTModulusTextBox.Text = StatisticResult.ModulusByCpt == "/" ? "/" : StatisticResult.ModulusByCpt + " MPa";
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

        #region 根据标贯/动探计算

        /// <summary>
        /// 根据标贯/动探值查承载力，输入单参数
        /// </summary>
        /// <param name="_standard">采用的地方标准</param>
        /// <param name="_soilType">土质类型</param>
        /// <param name="_numberRow">输入参数</param>
        /// <returns></returns>
        public static double CalcuBearingByNTest(string _standard, string _soilType, double _numberRow)
        {
            // 采用湖北省地方标准
            if(_standard=="Hubei")
            {
                // 土质类型为碎石土
                if (_soilType == "碎石土")
                {
                    double[] paraCol = HubeiLocalStandardBearingAndModulus.BearingAndModulusNTestGravels.ParaCol;
                    double[] values = HubeiLocalStandardBearingAndModulus.BearingAndModulusNTestGravels.BearingData;
                    return GeoMath.Interpolation(_numberRow, paraCol, values);
                }

                // 土质类型为一般黏性土
                if (_soilType=="一般黏性土")
                {
                    double[] paraCol = HubeiLocalStandardBearingAndModulus.BearingAndModulusNTestClay.ParaCol;
                    double[] values = HubeiLocalStandardBearingAndModulus.BearingAndModulusNTestClay.BearingData;
                    return GeoMath.Interpolation(_numberRow, paraCol, values);
                }

                // 土质类型为老黏性土
                if (_soilType == "老黏性土")
                {
                    double[] paraCol = HubeiLocalStandardBearingAndModulus.BearingAndModulusNTestOldClay.ParaCol;
                    double[] values = HubeiLocalStandardBearingAndModulus.BearingAndModulusNTestOldClay.BearingData;
                    return GeoMath.Interpolation(_numberRow, paraCol, values);
                }

                // 土质类型为杂填土
                if (_soilType == "杂填土")
                {
                    double[] paraCol = HubeiLocalStandardBearingAndModulus.BearingAndModulusNTestMixedFill.ParaCol;
                    double[] values = HubeiLocalStandardBearingAndModulus.BearingAndModulusNTestMixedFill.BearingData;
                    return GeoMath.Interpolation(_numberRow, paraCol, values);
                }

                // 土质类型为粉、细砂
                if (_soilType == "粉、细砂")
                {
                    double[] paraCol = HubeiLocalStandardBearingAndModulus.BearingAndModulusNTestFineSand.ParaCol;
                    double[] values = HubeiLocalStandardBearingAndModulus.BearingAndModulusNTestFineSand.BearingData;
                    return GeoMath.Interpolation(_numberRow, paraCol, values);
                }

                // 土质类型为中、粗砂
                if (_soilType == "中、粗砂")
                {
                    double[] paraCol = HubeiLocalStandardBearingAndModulus.BearingAndModulusNTestCoarseSand.ParaCol;
                    double[] values = HubeiLocalStandardBearingAndModulus.BearingAndModulusNTestCoarseSand.BearingData;
                    return GeoMath.Interpolation(_numberRow, paraCol, values);
                }

                // 土质类型为砾砂
                if (_soilType == "砾砂")
                {
                    double[] paraCol = HubeiLocalStandardBearingAndModulus.BearingAndModulusNTestGravellySand.ParaCol;
                    double[] values = HubeiLocalStandardBearingAndModulus.BearingAndModulusNTestGravellySand.BearingData;
                    return GeoMath.Interpolation(_numberRow, paraCol, values);
                }
            }

            // 无法查取时返回空值
            return Constants.NullNumber;
        }

        /// <summary>
        /// 根据标贯/动探值查压缩模量，输入单参数
        /// </summary>
        /// <param name="_standard">采用的地方标准</param>
        /// <param name="_soilType">土质类型</param>
        /// <param name="_numberRow">输入参数</param>
        /// <returns></returns>
        public static double CalcuModulusByNTest(string _standard, string _soilType, double _numberRow)
        {
            // 采用湖北省地方标准
            if (_standard == "Hubei")
            {
                // 土质类型为一般黏性土
                if (_soilType == "一般黏性土")
                {
                    double[] paraCol = HubeiLocalStandardBearingAndModulus.BearingAndModulusNTestClay.ParaCol;
                    double[] values = HubeiLocalStandardBearingAndModulus.BearingAndModulusNTestClay.ModulusData;
                    return GeoMath.Interpolation(_numberRow, paraCol, values);
                }

                // 土质类型为老黏性土
                if (_soilType == "老黏性土")
                {
                    double[] paraCol = HubeiLocalStandardBearingAndModulus.BearingAndModulusNTestOldClay.ParaCol;
                    double[] values = HubeiLocalStandardBearingAndModulus.BearingAndModulusNTestOldClay.ModulusData;
                    return GeoMath.Interpolation(_numberRow, paraCol, values);
                }

                // 土质类型为杂填土
                if (_soilType == "杂填土")
                {
                    double[] paraCol = HubeiLocalStandardBearingAndModulus.BearingAndModulusNTestMixedFill.ParaCol;
                    double[] values = HubeiLocalStandardBearingAndModulus.BearingAndModulusNTestMixedFill.ModulusData;
                    return GeoMath.Interpolation(_numberRow, paraCol, values);
                }

                // 土质类型为粉、细砂
                if (_soilType == "粉、细砂")
                {
                    double[] paraCol = HubeiLocalStandardBearingAndModulus.BearingAndModulusNTestFineSand.ParaCol;
                    double[] values = HubeiLocalStandardBearingAndModulus.BearingAndModulusNTestFineSand.ModulusData;
                    return GeoMath.Interpolation(_numberRow, paraCol, values);
                }

                // 土质类型为中、粗砂
                if (_soilType == "中、粗砂")
                {
                    double[] paraCol = HubeiLocalStandardBearingAndModulus.BearingAndModulusNTestCoarseSand.ParaCol;
                    double[] values = HubeiLocalStandardBearingAndModulus.BearingAndModulusNTestCoarseSand.ModulusData;
                    return GeoMath.Interpolation(_numberRow, paraCol, values);
                }
            }

            // 无法查取时返回空值
            return Constants.NullNumber;
        }

        /// <summary>
        /// 根据标贯/动探计算承载力和压缩模量函数
        /// </summary>
        public static void CalcuByNTest(List<StatisticNTest> _nTestStatisticList,string _soilType)
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
                    if(_soilType == "一般黏性土"|| _soilType == "老黏性土"|| _soilType == "粉、细砂"|| _soilType == "中、粗砂")
                    {
                        if (ntestType == ZkNTest.ntype.N)
                        {
                            StatisticResult.NTestParameter = ntestType.ToString() + "(击)= " + ntestValue.ToString("0.0");
                            StatisticResult.BearingByNTest = CalcuBearingByNTest(CurrentStandard, _soilType, ntestValue) == Constants.NullNumber ? "/" : CalcuBearingByNTest(CurrentStandard, _soilType, ntestValue).ToString("0");
                            StatisticResult.ModulusByNTest = CalcuModulusByNTest(CurrentStandard, _soilType, ntestValue) == Constants.NullNumber ? "/" : CalcuModulusByNTest(CurrentStandard, _soilType, ntestValue).ToString("0.0");
                        }
                        else if (_soilType == "中、粗砂" && ntestType == ZkNTest.ntype.N635)
                        {
                            StatisticResult.NTestParameter = ntestType.ToString() + "(击)= " + ntestValue.ToString("0.0");
                            StatisticResult.BearingByNTest = CalcuBearingByNTest(CurrentStandard, _soilType, ntestValue) == Constants.NullNumber ? "/" : CalcuBearingByNTest(CurrentStandard, _soilType, ntestValue).ToString("0");
                            StatisticResult.ModulusByNTest = CalcuModulusByNTest(CurrentStandard, _soilType, ntestValue) == Constants.NullNumber ? "/" : CalcuModulusByNTest(CurrentStandard, _soilType, ntestValue).ToString("0.0");
                        }
                        else
                        {
                            StatisticResult.NTestParameter = "/";
                            StatisticResult.BearingByNTest = "/";
                            StatisticResult.ModulusByNTest = "/";
                        }
                    }  
                    else if (_soilType == "碎石土" || _soilType == "杂填土" || _soilType == "砾砂")
                    {
                        if (ntestType == ZkNTest.ntype.N635)
                        {
                            StatisticResult.NTestParameter = ntestType.ToString() + "(击)= " + ntestValue.ToString("0.0");
                            StatisticResult.BearingByNTest = CalcuBearingByNTest(CurrentStandard, _soilType, ntestValue) == Constants.NullNumber ? "/" : CalcuBearingByNTest(CurrentStandard, _soilType, ntestValue).ToString("0");
                            StatisticResult.ModulusByNTest = CalcuModulusByNTest(CurrentStandard, _soilType, ntestValue) == Constants.NullNumber ? "/" : CalcuModulusByNTest(CurrentStandard, _soilType, ntestValue).ToString("0.0");
                        }
                        else
                        {
                            StatisticResult.NTestParameter = "/";
                            StatisticResult.BearingByNTest = "/";
                            StatisticResult.ModulusByNTest = "/";
                        }
                    }
                    else
                    {
                        StatisticResult.NTestParameter = "/";
                        StatisticResult.BearingByNTest = "/";
                        StatisticResult.ModulusByNTest = "/";
                    }
                }
                else
                {
                    StatisticResult.NTestParameter = "/";
                    StatisticResult.BearingByNTest = "/";
                    StatisticResult.ModulusByNTest = "/";
                }
            }
        }

        /// <summary>
        /// 填充各文本框
        /// </summary>
        private void FillNTestTextBox()
        {
            this.NTestParameterTextBox.Text = StatisticResult.NTestParameter == "/" ? "/" : StatisticResult.NTestParameter + " 击";
            this.NTestBearingTextBox.Text = StatisticResult.BearingByNTest == "/" ? "/" : StatisticResult.BearingByNTest + " kPa";
            this.NTestModulusTextBox.Text = StatisticResult.ModulusByNTest == "/" ? "/" : StatisticResult.ModulusByNTest + " MPa";
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
            BearingAndModulusToWord output = new BearingAndModulusToWord(CurrentStandard);
            output.ShowDialog();

            // 点击"确认并输出"按钮后，启动输出程序
            if (output.DialogResult==true)
            {
                // 计算统计结果
                CalcuOutput(output.dtLayer, RstStatisticList, CptStatisticList, NTestStatisticList);
                
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
                string path = folderPath + @"\" + Program.currentProject + @"-承载力和压缩模量综合统计表.doc";

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
        public static void CalcuOutput(DataTable _dt, List<StatisticRST> _rstStaList, List<StatisticCPT> _cptStaList, List<StatisticNTest> _ntestStaList)
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
                StatisticResult.RstType = _dt.Rows[i]["currentRstType"].ToString();
                StatisticResult.CptType = _dt.Rows[i]["currentCptType"].ToString();
                StatisticResult.NTestType = _dt.Rows[i]["currentNTestType"].ToString();

                // 根据传递的土质类型重新计算参数
                CalcuByRst(_rstStaList, StatisticResult.RstType);
                CalcuByCpt(_cptStaList, StatisticResult.CptType);
                CalcuByNTest(_ntestStaList, StatisticResult.NTestType);

                // 赋值其他参数
                StatisticResult.CptParameter = StatisticResult.CptParameter == "/" ? "/" : StatisticResult.CptParameter.Substring(StatisticResult.CptParameter.IndexOf("=") + 1);
                StatisticResult.NTestParameter = StatisticResult.NTestParameter == "/" ? "/" : StatisticResult.NTestParameter.Substring(StatisticResult.NTestParameter.IndexOf("=") + 1);

                double[] tmp = new double[3] { 9999, 9999, 9999 };
                double num;

                if (double.TryParse(StatisticResult.BearingByRst, out num))
                    tmp[0] = num;
                if (double.TryParse(StatisticResult.BearingByCpt, out num))
                    tmp[1] = num;
                if (double.TryParse(StatisticResult.BearingByNTest, out num))
                    tmp[2] = num;
                StatisticResult.BearingFinal = tmp.Min() < 9999 ? tmp.Min().ToString("0") : "/";

                for (int k = 0; k < 3; k++)
                    tmp[k] = 9999;
                if (double.TryParse(StatisticResult.ModulusByRst, out num))
                    tmp[0] = num;
                if (double.TryParse(StatisticResult.ModulusByCpt, out num))
                    tmp[1] = num;
                if (double.TryParse(StatisticResult.ModulusByNTest, out num))
                    tmp[2] = num;
                StatisticResult.ModulusFinal = tmp.Min() < 9999 ? tmp.Min().ToString("0.0") : "/";

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
            bamStatisticWord.AddBearingAndModulusTable(statisticListOutput);
            bamStatisticWord.SaveAndQuit(_path);
        }

        // 实例化输出进度窗体
        private void ShowProgressBar(string _path)
        {
            OutputProgress prog = new OutputProgress(OutputProgress.OutputType.BearingAndModulus, _path, "输出统计结果", "正在输出承载力和压缩模量综合统计成果Word文档……");
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
            this.RSTTypeComboBox.Text = RstSoilType(LayerName);
            this.CPTTypeComboBox.Text = CptSoilType(LayerName);
            this.NTestTypeComboBox.Text = NTestSoilType(LayerName);

            // 清空数据
            ClearRstStatisticResult();
            ClearCptStatisticResult();
            ClearNTestStatisticResult();

            // 计算指标
            CalcuByRst(RstStatisticList, RstSoilType(LayerName));
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
