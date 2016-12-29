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
    /// ShearingStrengthToWord.xaml 的交互逻辑
    /// </summary>
    public partial class ShearingStrengthToWord : Window
    {
        #region 参数定义

        /// <summary>
        /// 当前采用的规范
        /// </summary>
        string CurrentStandard = null;

        /// <summary>
        /// 项目所有分层编号列表
        /// </summary>
        List<string> layerNumber = ProjectDataBase.ReadLayerNumberList(Program.currentProject);
        /// <summary>
        /// 项目所有分层岩土名称列表
        /// </summary>
        List<string> layerName = ProjectDataBase.ReadLayerNameList(Program.currentProject);
        
        /// <summary>
        /// 按Ps值土质类型列表
        /// </summary>
        List<string> CptTypes = new List<string>();
        /// <summary>
        /// 按Ps值当前选择土质类型
        /// </summary>
        List<string> SelectCptType = new List<string>();

        /// <summary>
        /// 按标贯/动探土质类型列表
        /// </summary>
        List<string> NTestTypes = new List<string>();
        /// <summary>
        /// 按标贯/动探当前选择土质类型
        /// </summary>
        List<string> SelectNTestType = new List<string>();

        /// <summary>
        /// 存储DataGrid数据的DataTable
        /// </summary>
        public DataTable dtLayer = new DataTable("layers");

        #endregion

        #region 构造函数

        /// <summary>
        /// 输入一个参数的构造函数
        /// </summary>
        /// <param name="_currentStandard">当前采用的规范</param>
        public ShearingStrengthToWord(string _currentStandard)
        {
            InitializeComponent();

            // 当前所采用规范
            CurrentStandard = _currentStandard;

            // 初始化土质类型
            InitialSoilType();

            // 初始化DataTable
            InitialDataTable();

            // 设置数据绑定
            this.SoilTypeDataGrid.DataContext = dtLayer;
        }

        #endregion

        #region 初始化

        /// <summary>
        /// 初始化土质类型函数
        /// </summary>
        public void InitialSoilType()
        {
            // 当采用湖北省地方标准
            if (CurrentStandard == "Hubei")
            {
                // 初始化土质类型列表
                foreach (string type in HubeiLocalStandardShearingStrength.CptSoilType)
                    CptTypes.Add(type);
                foreach (string type in HubeiLocalStandardShearingStrength.NTestSoilType)
                    NTestTypes.Add(type);

                // 初始化当前选择的土质类型
                for (int i = 0; i < layerName.Count; i++)
                {
                    SelectCptType.Add(HubeiLocalStandardShearingStrength.SelectCptSoilType(layerName[i]));
                    SelectNTestType.Add(HubeiLocalStandardShearingStrength.SelectNTestSoilType(layerName[i]));
                }
            }
        }

        /// <summary>
        /// 初始化DataTable
        /// </summary>
        public void InitialDataTable()
        {
            // 初始化DataTable的列
            dtLayer.Columns.Add(new DataColumn("layerInfo", typeof(string)));
            dtLayer.Columns.Add(new DataColumn("cptType", typeof(List<string>)));
            dtLayer.Columns.Add(new DataColumn("currentCptType", typeof(string)));
            dtLayer.Columns.Add(new DataColumn("nTestType", typeof(List<string>)));
            dtLayer.Columns.Add(new DataColumn("currentNTestType", typeof(string)));

            // 初始化DataTable的行
            DataRow dr;
            for (int i = 0; i < layerNumber.Count; i++)
            {
                dr = dtLayer.NewRow();
                dr["layerInfo"] = layerNumber[i] + " " + layerName[i];
                dr["cptType"] = CptTypes;
                dr["currentCptType"] = SelectCptType[i];
                dr["nTestType"] = NTestTypes;
                dr["currentNTestType"] = SelectNTestType[i];
                dtLayer.Rows.Add(dr);
            }
        }

        #endregion

        #region 传递参数

        /// <summary>
        /// 点击"确认并输出"按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CommitButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        #endregion
    }
}
