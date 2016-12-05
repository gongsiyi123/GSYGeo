using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Data.SQLite;
using System.IO;
using System.Threading;

namespace GSYGeo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        #region 参数定义

        // 定义导航树数据绑定
        public static TreeViewBinding bind = new TreeViewBinding();

        #endregion

        #region 构造函数

        // 默认构造函数
        public MainWindow()
        {
            InitializeComponent();

            // 程序自检
            ProgramCheck();

            // 设置初始菜单可用性为否
            MenuIsEnable(false);

            // 设置导航树数据绑定
            SetTreeViewBinding();
        }

        #endregion

        #region 公用函数

        // 程序自检函数，检查必要设置并初始化
        private void ProgramCheck()
        {
            // 如果数据存储路径注册表项不存在，则新建项并设置默认路径
            if (!Program.IsExitProgramPath())
            {
                Program.SetProgramPath(Program.defaultPath);
            }

            // 如果"巩氏岩土"文件夹不存在，则新建文件夹
            if (!System.IO.Directory.Exists(Program.ReadProgramPath()))
            {
                System.IO.Directory.CreateDirectory(Program.ReadProgramPath());
            }

            // 如果设置信息数据库不存在，则新建数据库，并初始化数据库表
            if (!System.IO.File.Exists(Program.ReadProgramPath() + "\\设置信息.gsygeo"))
            {
                SettingDataBase.Create();
            }
        }

        // 设置菜单可用性函数
        private void MenuIsEnable(bool _isEnable)
        {
            this.EditProjectMenu.IsEnabled = _isEnable;
            this.CloseProjectMenu.IsEnabled = _isEnable;
            this.ExportProjectManu.IsEnabled = _isEnable;
            this.ZkMenu.IsEnabled = _isEnable;
            this.InsituTestMenu.IsEnabled = _isEnable;
            this.IndoorTestMenu.IsEnabled = _isEnable;
            this.CalculateMenu.IsEnabled = _isEnable;
            this.DrawMenu.IsEnabled = _isEnable;
            this.ReportMenu.IsEnabled = _isEnable;
            this.WindowMenu.IsEnabled = _isEnable;
        }

        // 设置导航树数据绑定函数
        private void SetTreeViewBinding()
        {
            this.ProjectTreeView.ItemsSource = bind.TreeItem;
        }

        // 设置程序标题及相关参数函数，带参数为正在运行或打开项目
        private void SetProgramText(string _projectName)
        {
            this.Title = _projectName + " - 巩氏岩土";
            ShowProjectNavigationMenu.IsChecked = true;
            bind.TreeItem[1].IsExpanded = true;
            bind.TreeItem[2].IsExpanded = true;
        }
        // 设置程序标题及相关参数函数，不带参数为未运行或正在关闭项目
        private void SetProgramText()
        {
            this.Title = "巩氏岩土";
            ShowProjectNavigationMenu.IsChecked = false;
            bind.TreeItem[1].IsExpanded = false;
            bind.TreeItem[1].Items.Clear();
            bind.TreeItem[2].IsExpanded = false;
            bind.TreeItem[2].Items.Clear();
            bind.TreeItem[3].IsExpanded = false;
            bind.TreeItem[3].Items.Clear();
            this.ContectGrid.Children.Clear();
        }

        #endregion

        #region 导航树模块

        // 设置导航树的初始展开状态
        private void InitialTreeViewIsOpen()
        {
            TreeViewItem item = (TreeViewItem)this.ProjectTreeView.Items[1];
            item.IsExpanded = true;
            TreeViewItem item2 = (TreeViewItem)this.ProjectTreeView.Items[2];
            for(int i = 0; i < item2.Items.Count; i++)
            {
                TreeViewItem childItem = (TreeViewItem)item2.Items[i];
                childItem.IsExpanded = true;
            }
            TreeViewItem item3 = (TreeViewItem)this.ProjectTreeView.Items[3];
            item.IsExpanded = true;
        }

        // TreeView选择节点变化时激活不同的内容控件
        private void ProjectTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            // 读取选中节点
            TreeViewItem selectedItem = (TreeViewItem)e.NewValue;

            // 根据选中状况清空窗口中的控件
            if (selectedItem == null)
            {
                this.ContectGrid.Children.Clear();
                return;
            }

            if (selectedItem.Header.ToString() != "基本信息" && selectedItem.Parent != null)
            {
                this.ContectGrid.Children.Clear();
            }

            // 选中一级节点时
            // 选中"基本信息"
            if (selectedItem.Header.ToString() == "基本信息")
            {
                ProjectBasicInfo prjb = new ProjectBasicInfo();
                this.ContectGrid.Children.Add(prjb);
            }

            // 选中二级、三级节点时
            if (selectedItem.Parent != null)
            {
                // 读取父节点
                TreeViewItem parentItem = (TreeViewItem)selectedItem.Parent;

                // 选中"钻孔"子节点
                if (parentItem.Header.ToString() == "钻孔")
                {
                    // 读取当前所选钻孔名称
                    string prj = Program.currentProject;
                    string zkName = selectedItem.Header.ToString();

                    // 实例化一个Borehole类，并将当前所选钻孔的数据库信息赋值给此实例
                    Borehole bh = new Borehole(zkName, BoreholeDataBase.ReadAltitude(prj, zkName));
                    bh.X = BoreholeDataBase.ReadXAxis(prj, zkName);
                    bh.Y = BoreholeDataBase.ReadYAxis(prj, zkName);
                    bh.InitialWaterLevel = BoreholeDataBase.ReadInitialWaterLevel(prj, zkName);
                    bh.StableWaterLevel = BoreholeDataBase.ReadStableWaterLevel(prj, zkName);
                    bh.Layers = BoreholeDataBase.ReadZkLayer(prj, zkName);
                    bh.Samples = BoreholeDataBase.ReadZkSample(prj, zkName);
                    bh.NTests = BoreholeDataBase.ReadZkNTest(prj, zkName);

                    // 实例化BoreholeControl用户控件，并赋值
                    BoreholeControl bhc = new BoreholeControl(bh);
                    this.ContectGrid.Children.Add(bhc);
                }

                // 选取"原位测试"-"静力触探"子节点
                if (parentItem.Header.ToString() == "静力触探")
                {
                    // 读取当前所选触探孔名称
                    string prj = Program.currentProject;
                    string jkName = selectedItem.Header.ToString();

                    // 实例化一个CPT类，并将当前所选触探孔的数据库信息赋值给此实例
                    CPT cpt = new CPT(jkName, CPTDataBase.ReadAltitude(prj, jkName));
                    cpt.X = CPTDataBase.ReadXAxis(prj, jkName);
                    cpt.Y = CPTDataBase.ReadYAxis(prj, jkName);
                    cpt.Layers = CPTDataBase.ReadJkLayer(prj, jkName);
                    cpt.PsList = CPTDataBase.ReadJkPs(prj, jkName);

                    // 实例化CPTControl用户控件，并赋值
                    CPTControl cptc = new CPTControl(cpt);
                    this.ContectGrid.Children.Add(cptc);
                }

                // 选取"室内试验"-"土工常规"子节点
                if (selectedItem.Header.ToString() == "土工常规")
                {
                    // 实例化一个RoutineSoilTest类列表，并读取数据库信息赋值给此实例
                    List<RoutineSoilTest> rsts = RoutineSoilTestDataBase.ReadAllData(Program.currentProject);

                    // 实例化RoutineSoilTestControl用户控件，并赋值
                    RoutineSoilTestControl rstc = new RoutineSoilTestControl(rsts);
                    this.ContectGrid.Children.Add(rstc);
                }
            }
        }

        #endregion

        #region 设置模块

        // 单击菜单"工具"-"设置"
        private void MenuItem_Setting_Click(object sender, RoutedEventArgs e)
        {
            Setting setting = new Setting();
            setting.ShowDialog();
        }

        #endregion

        #region 项目管理模块

        // 单击菜单"项目"-"新建项目"
        private void NewProjectMenu_Click(object sender, RoutedEventArgs e)
        {
            // 隐藏项目基本信息窗口
            foreach (TreeViewItem item in this.ProjectTreeView.Items)
            {
                item.IsSelected = false;
            }

            // 重置窗口
            SetProgramText();

            // 新建
            NewProject newProject = new NewProject();
            newProject.ShowDialog();
            if (newProject.DialogResult == true)
            {
                MenuIsEnable(true);
                SetProgramText(Program.currentProject);

                // 赋值钻孔列表
                bind.ReSetZkItem(Program.currentProject);
            }
        }

        // 单击菜单"项目"-"编辑当前项目"
        private void EditProjectMenu_Click(object sender, RoutedEventArgs e)
        {
            // 隐藏项目基本信息窗口
            foreach(TreeViewItem item in this.ProjectTreeView.Items)
            {
                item.IsSelected = false;
            }

            // 赋值要传递的参数
            string name = Program.currentProject;
            string province = ProjectDataBase.ReadProjectProvince(name);
            string city = ProjectDataBase.ReadProjectCity(name);
            List<string> layerNumberList = ProjectDataBase.ReadLayerNumberList(name);
            List<string> layerNameList = ProjectDataBase.ReadLayerNameList(name);
            List<string> layerGeoList = ProjectDataBase.ReadLayerGeoList(name);
            List<string> layerDescriptionList = ProjectDataBase.ReadLayerDescriptionList(name);
            string[] company = ProjectDataBase.ReadProjectCompany(name);
            
            // 实例化窗口
            NewProject editProject = new NewProject(name, province, city, layerNumberList,layerNameList,layerGeoList,layerDescriptionList,company);
            editProject.ShowDialog();

            // 更新项目数据文件
            if (editProject.DialogResult == true)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                File.Move(editProject.oldFile, editProject.newFile);
            }

            // 重命名窗口
            SetProgramText(Program.currentProject);
        }

        // 单击菜单"项目"-"关闭当前项目"
        private void CloseProjectMenu_Click(object sender, RoutedEventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Program.currentProject = null;
            MenuIsEnable(false);
            SetProgramText();
        }

        // 单击菜单"项目"-"打开项目"
        private void OpenProjectMenu_Click(object sender, RoutedEventArgs e)
        {
            // 隐藏项目基本信息窗口
            foreach (TreeViewItem item in this.ProjectTreeView.Items)
            {
                item.IsSelected = false;
            }

            // 重置窗口
            SetProgramText();

            // 打开
            ProjectList projectList = new ProjectList();
            projectList.ShowDialog();
            if (projectList.DialogResult == true)
            {
                MenuIsEnable(true);
                SetProgramText(Program.currentProject);

                // 赋值钻孔列表
                bind.ReSetZkItem(Program.currentProject);

                // 赋值原位测试-静力触探列表
                if (CPTDataBase.ReadJkList(Program.currentProject).Count > 0)
                {
                    bind.AddItemToSecondTree(2, "静力触探");
                }
                bind.ReSetJkItem(Program.currentProject);

                // 赋值室内试验-土工常规
                if (RoutineSoilTestDataBase.ReadAllData(Program.currentProject).Count > 0)
                {
                    bind.AddItemToSecondTree(3, "土工常规");
                    bind.TreeItem[3].IsExpanded = true;
                }
            }
        }

        // 单击菜单"项目"-"项目管理器"
        private void ProjectManagerMenu_Click(object sender, RoutedEventArgs e)
        {
            // 隐藏项目基本信息窗口
            foreach (TreeViewItem item in this.ProjectTreeView.Items)
            {
                item.IsSelected = false;
            }

            // 激活项目管理器
            ProjectManager projectManager = new ProjectManager();
            projectManager.ShowDialog();
        }

        #endregion

        #region 钻孔模块

        // 单击菜单"钻孔"-"添加新钻孔"
        private void AddNewZkMenu_Click(object sender, RoutedEventArgs e)
        {
            AddNewZk();
        }

        // 添加新钻孔函数
        private void AddNewZk()
        {
            this.ContectGrid.Children.Clear();
            BoreholeControl newBoreHole = new BoreholeControl();
            this.ContectGrid.Children.Add(newBoreHole);
        }

        #endregion

        #region 原位测试-静力触探模块

        // 单击菜单"原位测试"-"静力触探"-"添加静力触探孔"
        private void AddCptMenu_Click(object sender, RoutedEventArgs e)
        {
            AddNewJk();
        }

        // 添加静力触探孔函数
        private void AddNewJk()
        {
            this.ContectGrid.Children.Clear();
            CPTControl newCPT = new CPTControl();
            this.ContectGrid.Children.Add(newCPT);
        }

        #endregion

        #region 室内试验-常规土工试验模块

        // 单击菜单"室内试验"-"常规土工试验据"-"查看/添加常规土工试验数据"
        private void ShowAddRoutineSoilTestDataMenu_Click(object sender, RoutedEventArgs e)
        {
            ShowAddRoutineSoilTestData();
        }

        // 查看/添加常规土工试验数据函数
        private void ShowAddRoutineSoilTestData()
        {
            this.ContectGrid.Children.Clear();

            // 实例化一个RoutineSoilTest类列表，并读取数据库信息赋值给此实例
            List<RoutineSoilTest> rsts = RoutineSoilTestDataBase.ReadAllData(Program.currentProject);

            // 实例化RoutineSoilTestControl用户控件，并赋值
            RoutineSoilTestControl rstc = new RoutineSoilTestControl(rsts);
            this.ContectGrid.Children.Add(rstc);
        }


        #endregion

        #region 帮助模块

        // 单击菜单"帮助"-"更新日志"
        private void UpdateLogMenu_Click(object sender, RoutedEventArgs e)
        {
            UpdateLog updateLog = new UpdateLog();
            updateLog.ShowDialog();
        }

        // 单击菜单"帮助"-"用户反馈"
        private void FeedBackMenu_Click(object sender, RoutedEventArgs e)
        {
            FeedBack feedBack = new FeedBack();
            feedBack.ShowDialog();
        }

        #endregion


    }
}
