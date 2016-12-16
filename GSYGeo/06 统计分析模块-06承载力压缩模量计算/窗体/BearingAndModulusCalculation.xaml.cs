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
        string CurrentStandard = SettingDataBase.ReadLocalStandard();

        #endregion

        #region 构造函数

        public BearingAndModulusCalculation()
        {
            InitializeComponent();

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
                this.RSTTypeComboBox.ItemsSource = HubeiLocalStandard.RstSoilType;
        }

        /// <summary>
        /// 填充Ps值土质类型选框
        /// </summary>
        private void InitialCPTTypeComboBox()
        {
            if (CurrentStandard == "Hubei")
                this.CPTTypeComboBox.ItemsSource = HubeiLocalStandard.CptSoilType;
        }

        /// <summary>
        /// 填充标贯/动探土质类型选框
        /// </summary>
        private void InitialNTestTypeComboBox()
        {
            if (CurrentStandard == "Hubei")
                this.NTestTypeComboBox.ItemsSource = HubeiLocalStandard.NTestSoilType;
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
        private string RstSoilType(string _soilName)
        {
            // 根据当前规范查承载力
            if (CurrentStandard == "Hubei")
                return HubeiLocalStandard.SelectRstSoilType(_soilName);

            // 查询不到时返回"无法识别"
            return "无法识别";
        }

        /// <summary>
        /// 按Ps值查表时的土质类型识别
        /// </summary>
        /// <param name="_soilName">岩土名称</param>
        /// <returns></returns>
        private string CptSoilType(string _soilName)
        {
            // 根据当前规范查承载力
            if (CurrentStandard == "Hubei")
                return HubeiLocalStandard.SelectCptSoilType(_soilName);

            // 查询不到时返回"无法识别"
            return "无法识别";
        }

        /// <summary>
        /// 按标贯/动探查表时的土质类型识别
        /// </summary>
        /// <param name="_soilName">岩土名称</param>
        /// <returns></returns>
        private  string NTestSoilType(string _soilName)
        {
            // 根据当前规范查承载力
            if (CurrentStandard == "Hubei")
                return HubeiLocalStandard.SelectNTestSoilType(_soilName);

            // 查询不到时返回"无法识别"
            return "无法识别";
        }

        #endregion

        #region 计算

        /// <summary>
        /// 分层选框选项变化时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LayerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 提取当前分层编号和岩土名称
            string s = this.LayerComboBox.SelectedItem.ToString();
            string layerNumber = s.Substring(0, s.IndexOf(" "));
            string layerName = s.Substring(s.IndexOf(" ") + 1);

            // 识别土质
            this.RSTTypeComboBox.Text = RstSoilType(layerName);
            this.CPTTypeComboBox.Text = CptSoilType(layerName);
            this.NTestTypeComboBox.Text = NTestSoilType(layerName);
        }

        #endregion

        #region 输出



        #endregion

        #region 其他



        #endregion
        
    }
}
