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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;

namespace GSYGeo
{
    /// <summary>
    /// ProjectBasicInfo.xaml 的交互逻辑
    /// </summary>
    public partial class ProjectBasicInfo : UserControl
    {
        /// <summary>
        /// 定义DataGrid控件的数据源Datatable
        /// </summary>
        public DataTable dt = new DataTable("Layer");

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public ProjectBasicInfo()
        {
            InitializeComponent();

            // 赋值项目信息
            InitialProjectBasicInfo();

            // 绑定
            this.ProjectLayerDataGrid.DataContext = dt;
        }

        /// <summary>
        /// 赋值项目数据
        /// </summary>
        private void InitialProjectBasicInfo()
        {
            if (Program.currentProject != null)
            {
                dt.Clear();
                this.DrawerComboBox.ItemsSource = SettingDataBase.ReadCompanyPeople();
                this.WriterComboBox.ItemsSource = SettingDataBase.ReadCompanyPeople();
                this.CheckerComboBox.ItemsSource = SettingDataBase.ReadCompanyPeople();
                this.InspectorComboBox.ItemsSource = SettingDataBase.ReadCompanyPeople();
                this.ApproverComboBox.ItemsSource = SettingDataBase.ReadCompanyPeople();
                this.FinalApproverComboBox.ItemsSource = SettingDataBase.ReadCompanyPeople();
                string prj = Program.currentProject;
                this.ProjectNameTextBox.Text = prj;
                this.ProjectProvinceTextBox.Text = ProjectDataBase.ReadProjectProvince(prj);
                this.ProjectCityTextBox.Text = ProjectDataBase.ReadProjectCity(prj);
                string[] company = ProjectDataBase.ReadProjectCompany(prj);
                this.CompanyNameTextBox.Text = company[0];
                this.CompanyCodeTextBox.Text = company[1];
                this.DrawerComboBox.Text = company[2];
                this.WriterComboBox.Text = company[3];
                this.CheckerComboBox.Text = company[4];
                this.InspectorComboBox.Text = company[5];
                this.ApproverComboBox.Text = company[6];
                this.FinalApproverComboBox.Text = company[7];
                InitialDataTable(ProjectDataBase.ReadLayerNumberList(prj), ProjectDataBase.ReadLayerNameList(prj), ProjectDataBase.ReadLayerGeoList(prj), ProjectDataBase.ReadLayerDescriptionList(prj));
                
            }
        }

        /// <summary>
        /// 初始化DataTable，不带参数
        /// </summary>
        private void InitialDataTable()
        {
            // 定义DataTable数据列
            dt.Columns.Add(new DataColumn("numberList", typeof(string)));
            dt.Columns.Add(new DataColumn("nameList", typeof(string)));
            dt.Columns.Add(new DataColumn("geoList", typeof(string)));
            dt.Columns.Add(new DataColumn("descriptionList", typeof(string)));
        }

        /// <summary>
        /// 初始化DataTable，带参数
        /// </summary>
        /// <param name="_numberList">分层编号列表</param>
        /// <param name="_nameList">岩土名称列表</param>
        /// <param name="_geoList">地质年代成因列表</param>
        /// <param name="_descriptionList">分层描述列表</param>
        private void InitialDataTable(List<string> _numberList, List<string> _nameList, List<string> _geoList, List<string> _descriptionList)
        {
            if (dt.Columns.Count == 0)
            {
                // 定义DataTable数据列并赋值
                dt.Columns.Add(new DataColumn("numberList", typeof(string)));
                dt.Columns.Add(new DataColumn("nameList", typeof(string)));
                dt.Columns.Add(new DataColumn("geoList", typeof(string)));
                dt.Columns.Add(new DataColumn("descriptionList", typeof(string)));
            }
            
            DataRow dr;
            for (int i = 0; i < _numberList.Count; i++)
            {
                dr = dt.NewRow();
                dr["numberList"] = _numberList[i];
                dr["nameList"] = _nameList[i];
                dr["geoList"] = _geoList[i];
                dr["descriptionList"] = _descriptionList[i];
                dt.Rows.Add(dr);
            }
        }

        /// <summary>
        /// 控件可视性变化时刷新数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            InitialProjectBasicInfo();
        }

        /// <summary>
        /// 鼠标点击时取消DataGrid的选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProjectLayerDataGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.ProjectLayerDataGrid.UnselectAll();
        }
    }
}
