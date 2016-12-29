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
using System.IO;

namespace GSYGeo
{
    /// <summary>
    /// NewProject.xaml 的交互逻辑
    /// </summary>
    public partial class NewProject : Window
    {
        /// <summary>
        /// 实例化ProjectSetting类
        /// </summary>
        ProjectSetting projectSetting = new ProjectSetting();

        /// <summary>
        /// 定义分层数据表
        /// </summary>
        private DataTable dt = new DataTable("Layer");

        /// <summary>
        /// 判断是否为新建项目的变量
        /// </summary>
        private bool isNew = false;

        /// <summary>
        /// 定义编辑项目时新旧项目名称
        /// </summary>
        public string oldFile;
        public string newFile;

        /// <summary>
        /// 无参数的构造函数
        /// </summary>
        public NewProject()
        {
            InitializeComponent();

            // 赋值isNew为true
            isNew = true;
        }

        /// <summary>
        /// 带参数的构造函数，其中string[] _company，必须为string[8]形式
        /// </summary>
        /// <param name="_name">项目名称</param>
        /// <param name="_province">项目所在省份（可空）</param>
        /// <param name="_city">项目所在县市（可空）</param>
        /// <param name="_layerNumber">分层编号列表</param>
        /// <param name="_layerName">岩土名称列表</param>
        /// <param name="_layerGeo">地质年代成因列表</param>
        /// <param name="_layerDescription">分层描述列表</param>
        /// <param name="_company">公司信息数组</param>
        public NewProject(string _name,string _province,string _city,List<string> _layerNumber,List<string> _layerName,List<string> _layerGeo,List<string> _layerDescription,string[] _company)
        {
            InitializeComponent();

            // 赋值isNew为false
            isNew = false;

            // 赋值projectSetting
            projectSetting.Name = _name;
            projectSetting.Province = _province;
            projectSetting.City = _city;
            if (_layerNumber!=null && _layerNumber.Count == _layerName.Count && _layerNumber.Count == _layerGeo.Count && _layerNumber.Count == _layerDescription.Count)
            {
                for (int i = 0; i < _layerNumber.Count; i++)
                {
                    projectSetting.AddLayer(_layerNumber[i], _layerName[i], _layerGeo[i], _layerDescription[i]);
                }
            }
            if (_company!=null && _company.Length == 8)
            {
                for (int i = 0; i < _company.Length; i++)
                {
                    projectSetting.CompanyName = _company[0];
                    projectSetting.CompanyCode = _company[1];
                    projectSetting.Drawer = _company[2];
                    projectSetting.Writer = _company[3];
                    projectSetting.Checker = _company[4];
                    projectSetting.Inspector = _company[5];
                    projectSetting.Approver = _company[6];
                    projectSetting.FinalApprover = _company[7];
                }
            }

            // 初始化窗口
            this.ProjectNameTextBox.Text = projectSetting.Name;
            this.ProjectProvinceTextBox.Text = projectSetting.Province;
            this.ProjectCityTextBox.Text = projectSetting.City;
        }

        /// <summary>
        /// Save命令的Executed事件处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CommandBindingSave_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // 新建项目的操作
            if (isNew == true)
            {
                // 保存项目设置
                projectSetting.Name = this.ProjectNameTextBox.Text;
                projectSetting.Province = this.ProjectProvinceTextBox.Text;
                projectSetting.City = this.ProjectCityTextBox.Text;

                // 检查项目名是否为空
                if (string.IsNullOrEmpty(projectSetting.Name) || string.IsNullOrWhiteSpace(projectSetting.Name))
                    return;

                // 检测是否有同名项目
                List<string> projectList = ProjectDataBase.ReadProjectList();
                if (projectList != null && projectList.Contains(projectSetting.Name))
                {
                    MessageBox.Show("项目数据库中已存在名为\"" + projectSetting.Name + "\"的项目，无法新建项目。", "请检查项目名称", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // 创建项目数据库
                ProjectDataBase.Create(projectSetting.Name);

                // 存储项目基本数据和公司数据
                ProjectDataBase.EditProjectBasicInfo(projectSetting.Name, projectSetting.Name, projectSetting.Province, projectSetting.City);
                ProjectDataBase.EditProjectCompany(projectSetting.Name, projectSetting.CompanyName, projectSetting.CompanyCode, projectSetting.Drawer, projectSetting.Writer, projectSetting.Checker, projectSetting.Inspector, projectSetting.Approver, projectSetting.FinalApprover);

                // 删除旧分层数据，存储新分层数据
                if (ProjectDataBase.ReadLayerNumberList(projectSetting.Name).Count > 0)
                {
                    ProjectDataBase.DeleteAllLayer(projectSetting.Name);
                }
                if (projectSetting.LayerList != null)
                {
                    for (int i = 0; i < projectSetting.LayerList.Count; i++)
                    {
                        string number = projectSetting.LayerList[i].Number;
                        string name = projectSetting.LayerList[i].Name;
                        string geo = projectSetting.LayerList[i].Geo;
                        string description = projectSetting.LayerList[i].Description;
                        ProjectDataBase.AddLayer(projectSetting.Name, number, name, geo, description);
                    }
                }
            }
            // 非新建项目的操作
            else
            {
                // 保存项目设置
                string oldProjectName = projectSetting.Name;
                projectSetting.Name = this.ProjectNameTextBox.Text;
                projectSetting.Province = this.ProjectProvinceTextBox.Text;
                projectSetting.City = this.ProjectCityTextBox.Text;

                // 存储项目基本数据和公司数据
                ProjectDataBase.EditProjectBasicInfo(oldProjectName, projectSetting.Name, projectSetting.Province, projectSetting.City);
                ProjectDataBase.EditProjectCompany(oldProjectName, projectSetting.CompanyName, projectSetting.CompanyCode, projectSetting.Drawer, projectSetting.Writer, projectSetting.Checker, projectSetting.Inspector, projectSetting.Approver, projectSetting.FinalApprover);

                // 删除旧分层数据，存储新分层数据
                if (ProjectDataBase.ReadLayerNumberList(oldProjectName).Count > 0)
                {
                    ProjectDataBase.DeleteAllLayer(oldProjectName);
                }
                if (projectSetting.LayerList != null)
                {
                    for (int i = 0; i < projectSetting.LayerList.Count; i++)
                    {
                        string number = projectSetting.LayerList[i].Number;
                        string name = projectSetting.LayerList[i].Name;
                        string geo = projectSetting.LayerList[i].Geo;
                        string description = projectSetting.LayerList[i].Description;
                        ProjectDataBase.AddLayer(oldProjectName, number, name, geo, description);
                    }
                }

                // 更改数据库文件名称，传递给主窗口更改数据文件名
                oldFile = Program.ReadProgramPath() + "\\" + oldProjectName + ".gsygeo";
                newFile = Program.ReadProgramPath() + "\\" + projectSetting.Name + ".gsygeo";
            }

            // 关闭窗口
            this.DialogResult = true;
            Program.currentProject = projectSetting.Name;
            this.Close();
        }

        /// <summary>
        /// Save命令的CanExecuted事件处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CommandBindingSave_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            // 设置"确定"按钮的可用状态
            if (e.Source.ToString() == "System.Windows.Controls.Button: 确定" && !string.IsNullOrEmpty(this.ProjectNameTextBox.Text) && !string.IsNullOrWhiteSpace(this.ProjectNameTextBox.Text))
            {
                e.CanExecute = true;
            }
        }

        /// <summary>
        /// 点击"项目划分岩土层设置"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProjectLayerSettingButton_Click(object sender, RoutedEventArgs e)
        {
            // 传递projectSetting参数至ProjectLayer窗口
            List<string> numberList = new List<string>();
            List<string> nameList = new List<string>();
            List<string> geoList = new List<string>();
            List<string> descriptionList = new List<string>();
            if (projectSetting.LayerList != null)
            {
                for (int i = 0; i < projectSetting.LayerList.Count; i++)
                {
                    numberList.Add(projectSetting.LayerList[i].Number);
                    nameList.Add(projectSetting.LayerList[i].Name);
                    geoList.Add(projectSetting.LayerList[i].Geo);
                    descriptionList.Add(projectSetting.LayerList[i].Description);
                }
            }

            // 实例化ProjectLayer窗口，按历史分成情况构造窗口
            ProjectLayer projectLayer;
            if (projectSetting.LayerList == null || projectSetting.LayerList.Count == 0)
            {
                projectLayer = new ProjectLayer();
                projectLayer.ShowDialog();
            }
            else
            {
                projectLayer = new ProjectLayer(numberList, nameList, geoList, descriptionList);
                projectLayer.ShowDialog();
            }

            // 点击确定按钮后，刷新projectSetting.LayerList属性
            if (projectLayer.DialogResult == true)
            {
                // 清空旧projectSetting.LayerList属性
                if (projectSetting.LayerList != null)
                {
                    projectSetting.LayerList.Clear();
                }
                
                // 循环读取projectLayer窗口中的第i行的分层数据，赋值新projectSetting.LayerList属性
                for (int i = 0; i < projectLayer.LayerListDataGrid.Items.Count; i++)
                {
                    // 读取projectLayer窗口中的第i行的分层数据
                    string number = projectLayer.dt.Rows[i][0].ToString();
                    string name = projectLayer.dt.Rows[i][1].ToString();
                    string geo = projectLayer.dt.Rows[i][2].ToString();
                    string description = projectLayer.dt.Rows[i][3].ToString();

                    // 往projectSetting.LayerList添加行
                    projectSetting.AddLayer(number, name, geo, description);
                }
            }
        }

        /// <summary>
        /// 点击"项目公司信息设置"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProjectCompanySettingButton_Click(object sender, RoutedEventArgs e)
        {
            // 赋值传递数据，并初始化projectCompany窗体
            string name;
            if (!string.IsNullOrEmpty(projectSetting.CompanyName))
            {
                name = projectSetting.CompanyName;
            }
            else
            {
                name = SettingDataBase.ReadCompanyName();
            }
            string code;
            if (!string.IsNullOrEmpty(projectSetting.CompanyCode))
            {
                code = projectSetting.CompanyCode;
            }
            else
            {
                code = SettingDataBase.ReadCompanyCode();
            }
            string drawer = projectSetting.Drawer;
            string writer = projectSetting.Writer;
            string checker = projectSetting.Checker;
            string inspector = projectSetting.Inspector;
            string approver = projectSetting.Approver;
            string finalApprover = projectSetting.FinalApprover;
            ProjectCompany projectCompany = new ProjectCompany(name, code, drawer, writer, checker, inspector, approver, finalApprover);
            projectCompany.ShowDialog();

            // 点击确定按钮后，刷新projectSetting类中关于公司信息的属性
            if (projectCompany.DialogResult == true)
            {
                projectSetting.CompanyName = projectCompany.CompanyNameTextBox.Text;
                projectSetting.CompanyCode = projectCompany.CompanyCodeTextBox.Text;
                projectSetting.Drawer = projectCompany.DrawerComboBox.Text;
                projectSetting.Writer = projectCompany.WriterComboBox.Text;
                projectSetting.Checker = projectCompany.CheckerComboBox.Text;
                projectSetting.Inspector = projectCompany.InspectorComboBox.Text;
                projectSetting.Approver = projectCompany.ApproverComboBox.Text;
                projectSetting.FinalApprover = projectCompany.FinalApproverComboBox.Text;
            }   
        }

        /// <summary>
        /// 点击"取消"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelProjectSetting_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
