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

            // 如果"小熠岩土勘察"文件夹不存在，则新建文件夹
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
            this.Title = _projectName + " - 小熠岩土勘察";
            ShowProjectNavigationMenu.IsChecked = true;
            bind.TreeItem[1].IsExpanded = true;
            bind.TreeItem[2].IsExpanded = true;
        }
        // 设置程序标题及相关参数函数，不带参数为未运行或正在关闭项目
        private void SetProgramText()
        {
            this.Title = "小熠岩土勘察";
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

                // 选取"室内试验"-"颗粒分析"子节点
                if (selectedItem.Header.ToString() == "颗粒分析")
                {
                    // 实例化一个RoutineSoilTest类列表，并读取数据库信息赋值给此实例
                    List<GrainAnalysisTest> gats = GrainAnalysisTestDataBase.ReadAllData(Program.currentProject);

                    // 实例化RoutineSoilTestControl用户控件，并赋值
                    GrainAnalysisTestControl gatc = new GrainAnalysisTestControl(gats);
                    this.ContectGrid.Children.Add(gatc);
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

                // 赋值室内试验-颗粒分析
                if (GrainAnalysisTestDataBase.ReadAllData(Program.currentProject).Count > 0)
                {
                    bind.AddItemToSecondTree(3, "颗粒分析");
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

        // 单击菜单"室内试验"-"土工常规试验"-"查看/添加常规土工试验数据"
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

        #region 室内试验模块-颗粒分析试验

        // 单击菜单"室内试验"-"颗粒分析试验"-"查看/添加颗粒分析试验数据"
        private void ShowAddGrainAnalysisTestDataMenu_Click(object sender, RoutedEventArgs e)
        {
            ShowAddGrainAnalysisTestData();
        }

        // 查看/添加颗粒分析试验数据函数
        private void ShowAddGrainAnalysisTestData()
        {
            this.ContectGrid.Children.Clear();

            // 实例化一个GrainAnalysisTest类列表，并读取数据库信息赋值给此实例
            List<GrainAnalysisTest> gats = GrainAnalysisTestDataBase.ReadAllData(Program.currentProject);

            // 实例化GrainAnalysisTestControl用户控件，并赋值
            GrainAnalysisTestControl gatc = new GrainAnalysisTestControl(gats);
            this.ContectGrid.Children.Add(gatc);
        }

        #endregion

        #region 统计分析模块-工作量统计

        /// <summary>
        /// 单击菜单"统计分析"-"工作量统计"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WorkloadStatisticMenu_Click(object sender, RoutedEventArgs e)
        {
            WLStatistic();
        }

        /// <summary>
        /// 统计工作量函数
        /// </summary>
        private void WLStatistic()
        {
            // 实例化一个WorkLoadStatistic窗口
            WorkLoadStatistic workLoadStatistic = new WorkLoadStatistic();
            workLoadStatistic.ShowDialog();
        }

        #endregion

        #region 统计分析模块-标贯/动探统计

        /// <summary>
        /// 单击菜单"统计分析"-"标贯/动探统计"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NTestStatisticMenu_Click(object sender, RoutedEventArgs e)
        {
            NTestStatistic();
        }

        /// <summary>
        /// 统计标贯/动探函数
        /// </summary>
        private void NTestStatistic()
        {
            // 实例化一个NTestStatistic窗口
            NTestStatistic nTestStatistic = new NTestStatistic();
            nTestStatistic.ShowDialog();
        }

        #endregion

        #region 统计分析模块-静力触探统计

        /// <summary>
        /// 单击菜单"统计分析"-"静力触探摩阻力统计"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PsStatisticMenu_Click(object sender, RoutedEventArgs e)
        {
            PsStatistic();
        }

        /// <summary>
        /// 统计摩阻力函数
        /// </summary>
        private void PsStatistic()
        {
            // 实例化一个CPTStatistic窗口
            CPTStatistic psStatistic = new CPTStatistic();
            psStatistic.ShowDialog();
        }

        #endregion

        #region 统计分析模块-土工常规统计

        /// <summary>
        /// 单击菜单"统计分析"-"土工常规统计"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RSTStatisticMenu_Click(object sender, RoutedEventArgs e)
        {
            RSTStatistic();
        }

        /// <summary>
        /// 统计土工常规函数
        /// </summary>
        private void RSTStatistic()
        {
            // 实例化一个RSTStatistic窗口
            RSTStatistic rstStatistic = new RSTStatistic();
            rstStatistic.ShowDialog();
        }

        #endregion

        #region 统计分析模块-颗粒分析统计

        /// <summary>
        /// 单击菜单"统计分析"-"颗粒分析统计"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GATStatisticMenu_Click(object sender, RoutedEventArgs e)
        {
            GATStatistic();
        }

        /// <summary>
        /// 统计颗粒分析函数
        /// </summary>
        private void GATStatistic()
        {
            // 实例化一个GATStatistic窗口
            GATStatistic gatStatistic = new GATStatistic();
            gatStatistic.ShowDialog();
        }

        #endregion

        #region 统计分析模块-承载力和变形模量计算

        /// <summary>
        /// 单击菜单"统计分析"-"承载力和变形模量计算"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BearAndModulusCalculateMenu_Click(object sender, RoutedEventArgs e)
        {
            BearAndModulusCalculate();
        }

        /// <summary>
        /// 承载力和变形模量计算函数
        /// </summary>
        private void BearAndModulusCalculate()
        {
            BearingAndModulusCalculation calculation = new BearingAndModulusCalculation();
            calculation.ShowDialog();
        }

        #endregion

        #region 绘图模块-绘制钻孔柱状图

        /// <summary>
        /// 单击菜单"绘图"-"绘制钻孔柱状图"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawZkMenu_Click(object sender, RoutedEventArgs e)
        {
            DrawZk();
        }

        /// <summary>
        /// 绘制柱状图函数
        /// </summary>
        private void DrawZk()
        {
            // 实例化一个OutputZkToCad窗口
            OutputZkToCad outputZk = new OutputZkToCad();
            outputZk.ShowDialog();
        }

        #endregion

        #region 绘图模块-绘制静力触探曲线图

        /// <summary>
        /// 单击菜单"绘图"-"绘制静力触探曲线图"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawCptMenu_Click(object sender, RoutedEventArgs e)
        {
            DrawJk();
        }

        private void DrawJk()
        {
            // 实例化一个OutputJkToCad窗口
            OutputJkToCad outputJk = new OutputJkToCad();
            outputJk.ShowDialog();
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

        #region 其他

        /// <summary>
        /// 点击"菜单"-"项目"-"退出"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitProgramMenu_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }



        #endregion

        #region 导入旧版数据

        /// <summary>
        /// 单击菜单"工具"-"导入旧版数据"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImportOldDataMenu_Click(object sender, RoutedEventArgs e)
        {
            // 检查是否关闭当前项目
            if (Program.currentProject != null)
            {
                MessageBox.Show("您有正处于打开状态的项目，请关闭当前项目后再执行本操作。");
                return;
            }

            // 选择输出目录
            string folderPath;

            System.Windows.Forms.FolderBrowserDialog programPathBrowser = new System.Windows.Forms.FolderBrowserDialog();
            if (programPathBrowser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                folderPath = programPathBrowser.SelectedPath;
            else
                return;

            if (folderPath.Substring(folderPath.LastIndexOf(@"\")+1) != "金浪岩土")
            {
                MessageBox.Show("选择的文件夹不正确，请选择\"金浪岩土\"文件夹！");
                return;
            }

            // 耐心等候提示
            MessageBox.Show("导入过程可能需要15-30秒的时间，请耐心等候，点击\"确定\"继续。");

            // 遍历项目文件夹
            List<string> projectlist = new List<string>();
            List<string> ignoreprojectlist = new List<string>();
            DirectoryInfo dir = new DirectoryInfo(folderPath);
            foreach (DirectoryInfo dChild in dir.GetDirectories("*"))
            {
                // 获取项目名称
                string projectName = dChild.ToString();
                
                // 获取项目文件夹路径
                string projectPath = folderPath + @"\" + dChild + @"\";

                // 新建项目
                ProjectDataBase.Create(projectName);

                // 读取分层信息
                StreamReader reader = new StreamReader(projectPath + "基本信息.txt");
                int countLine = 0;
                while (true)
                {
                    countLine++;
                    string line = reader.ReadLine();
                    if (countLine <= 3)
                        continue;
                    if (line == null)
                        break;

                    string number = line.Substring(0, line.IndexOf("/"));
                    line = line.Substring(line.IndexOf("/") + 1);
                    string name = line.Substring(0, line.IndexOf("/"));
                    line = line.Substring(line.IndexOf("/") + 1);
                    string description = line;
                    string geo = "Q4al";
                    ProjectDataBase.AddLayer(projectName, number, name, geo, description);
                }
                reader.Close();

                // 读取钻孔信息
                List<string> numlist = ProjectDataBase.ReadLayerNumberList(projectName);
                List<string> deslist = ProjectDataBase.ReadLayerDescriptionList(projectName);
                DirectoryInfo dirZk = new DirectoryInfo(projectPath + @"钻孔\");
                foreach(FileInfo dZkFile in dirZk.GetFiles("*.txt"))
                {
                    // 读取基本信息
                    StreamReader zkReader = new StreamReader(projectPath + @"钻孔\" + dZkFile.ToString());
                    zkReader.ReadLine();
                    string zkname= zkReader.ReadLine();
                    zkReader.ReadLine();
                    double zkaltitude = Convert.ToDouble(zkReader.ReadLine());
                    BoreholeDataBase.AddZkBasicInfo(projectName, zkname, zkaltitude, Constants.NullNumber, Constants.NullNumber, Constants.NullNumber, Constants.NullNumber);
                    zkReader.ReadLine();

                    // 读取分层信息
                    List<string> zknumberlist = new List<string>();
                    List<string> zknamelist = new List<string>();
                    List<string> zkgeolist = new List<string>();
                    List<string> zkdescriptionlist = new List<string>();
                    List<double> zkdepthlist = new List<double>();
                    while (true)
                    {
                        string line = zkReader.ReadLine();
                        if (line == "===取样===")
                            break;

                        zknumberlist.Add(line.Substring(0, line.IndexOf("/")));
                        line = line.Substring(line.IndexOf("/") + 1);
                        zknamelist.Add(line.Substring(0, line.IndexOf("/")));
                        line = line.Substring(line.IndexOf("/") + 1);
                        zkdepthlist.Add(Convert.ToDouble(line));
                        zkgeolist.Add("Q4al");
                        int index = numlist.IndexOf(zknumberlist[zknumberlist.Count - 1]);
                        zkdescriptionlist.Add(deslist[index]);
                    }
                    BoreholeDataBase.AddLayerListToZk(projectName, zkname, zknumberlist, zknamelist, zkgeolist, zkdescriptionlist, zkdepthlist);

                    // 读取取样信息
                    List<string> spnumberlist = new List<string>();
                    List<double> spdepthlist = new List<double>();
                    List<int> spisdisturbedlist = new List<int>();
                    countLine = 0;
                    while (true)
                    {
                        string line = zkReader.ReadLine();
                        if (line == "===标贯===")
                            break;

                        countLine++;
                        spnumberlist.Add(countLine.ToString());
                        spdepthlist.Add(Convert.ToDouble(line.Substring(0, line.IndexOf("/"))));
                        line = line.Substring(line.IndexOf("/") + 1);
                        spisdisturbedlist.Add(line == "扰" ? 1 : 0);
                    }
                    BoreholeDataBase.AddSampleListToZk(projectName, zkname, spnumberlist, spdepthlist, spisdisturbedlist);

                    // 读取标贯信息
                    List<string> ntnumberlist = new List<string>();
                    List<double> ntdepthlist = new List<double>();
                    List<double> ntvaluelist = new List<double>();
                    List<string> nttypelist = new List<string>();
                    countLine = 0;
                    while (true)
                    {
                        string line = zkReader.ReadLine();
                        if (line == null)
                            break;

                        countLine++;
                        ntnumberlist.Add(countLine.ToString());
                        ntdepthlist.Add(Convert.ToDouble(line.Substring(0, line.IndexOf("/"))));
                        line = line.Substring(line.IndexOf("/") + 1);
                        ntvaluelist.Add(Convert.ToDouble(line));
                        nttypelist.Add("N");
                    }
                    BoreholeDataBase.AddNTestListToZk(projectName, zkname, ntnumberlist, ntdepthlist, ntvaluelist, nttypelist);

                    zkReader.Close();
                }

                // 读取静力触探信息
                DirectoryInfo dirJk = new DirectoryInfo(projectPath + @"静力触探\");
                foreach (FileInfo dJkFile in dirJk.GetFiles("*.txt"))
                {
                    // 读取基本信息
                    StreamReader jkReader = new StreamReader(projectPath + @"静力触探\" + dJkFile.ToString());
                    jkReader.ReadLine();
                    string jkname = jkReader.ReadLine();
                    jkReader.ReadLine();
                    double jkaltitude = Convert.ToDouble(jkReader.ReadLine());
                    CPTDataBase.AddJkBasicInfo(projectName, jkname, jkaltitude, Constants.NullNumber, Constants.NullNumber);
                    jkReader.ReadLine();

                    // 读取分层信息
                    List<string> jknumberlist = new List<string>();
                    List<string> jknamelist = new List<string>();
                    List<string> jkgeolist = new List<string>();
                    List<string> jkdescriptionlist = new List<string>();
                    List<double> jkdepthlist = new List<double>();
                    while (true)
                    {
                        string line = jkReader.ReadLine();
                        if (line == "===Ps值===")
                            break;

                        jknumberlist.Add(line.Substring(0, line.IndexOf("/")));
                        line = line.Substring(line.IndexOf("/") + 1);
                        jknamelist.Add(line.Substring(0, line.IndexOf("/")));
                        line = line.Substring(line.IndexOf("/") + 1);
                        jkdepthlist.Add(Convert.ToDouble(line));
                        jkgeolist.Add("Q4al");
                        int index = numlist.IndexOf(jknumberlist[jknumberlist.Count - 1]);
                        jkdescriptionlist.Add(deslist[index]);
                    }
                    CPTDataBase.AddLayerListToJk(projectName, jkname, jknumberlist, jknamelist, jkgeolist, jkdescriptionlist, jkdepthlist);

                    // 读取Ps值信息
                    List<double> pslist = new List<double>();
                    while (true)
                    {
                        string line = jkReader.ReadLine();
                        if (line == null)
                            break;

                        pslist.Add(Convert.ToDouble(line));
                    }
                    CPTDataBase.AddPsListToJk(projectName, jkname, pslist);
                }

                // 读取土工试验
                StreamReader rstReader = new StreamReader(projectPath + @"土工试验\NormalTest.txt");
                List<RoutineSoilTest> rsts = new List<RoutineSoilTest>();
                while (true)
                {
                    string line = rstReader.ReadLine();
                    if (line == null)
                        break;

                    string zkNumber = line.Substring(0, line.IndexOf("/"));
                    line = line.Substring(line.IndexOf("/") + 1);
                    double sampleDepth = Convert.ToDouble(line.Substring(0, line.IndexOf("/")));
                    line = line.Substring(line.IndexOf("/") + 1);

                    string sampleLayer = ProjectDataBase.ReadLayerNumberList(projectName)[0];
                    List<ZkLayer> layers = BoreholeDataBase.ReadZkLayer(projectName, zkNumber);
                    for (int i = 0; i < layers.Count; i++)
                    {
                        if (sampleDepth <= layers[i].Depth)
                        {
                            sampleLayer = layers[i].Number;
                            break;
                        }
                    }

                    double waterLevel = Convert.ToDouble(line.Substring(0, line.IndexOf("/")));
                    line = line.Substring(line.IndexOf("/") + 1);
                    double density = Convert.ToDouble(line.Substring(0, line.IndexOf("/")));
                    line = line.Substring(line.IndexOf("/") + 1);
                    double specificGravity = Convert.ToDouble(line.Substring(0, line.IndexOf("/")));
                    line = line.Substring(line.IndexOf("/") + 1);
                    double voidRatio = Convert.ToDouble(line.Substring(0, line.IndexOf("/")));
                    line = line.Substring(line.IndexOf("/") + 1);
                    double saturation = Convert.ToDouble(line.Substring(0, line.IndexOf("/")));
                    line = line.Substring(line.IndexOf("/") + 1);
                    double liquidLimit = Convert.ToDouble(line.Substring(0, line.IndexOf("/")));
                    line = line.Substring(line.IndexOf("/") + 1);
                    double plasticLimit = Convert.ToDouble(line.Substring(0, line.IndexOf("/")));
                    line = line.Substring(line.IndexOf("/") + 1);
                    double plasticIndex = Convert.ToDouble(line.Substring(0, line.IndexOf("/")));
                    line = line.Substring(line.IndexOf("/") + 1);
                    double liquidityIndex = Convert.ToDouble(line.Substring(0, line.IndexOf("/")));
                    line = line.Substring(line.IndexOf("/") + 1);
                    double compressibility = Convert.ToDouble(line.Substring(0, line.IndexOf("/")));
                    line = line.Substring(line.IndexOf("/") + 1);
                    double modulus = Convert.ToDouble(line.Substring(0, line.IndexOf("/")));
                    line = line.Substring(line.IndexOf("/") + 1);
                    double frictionAngle = Convert.ToDouble(line.Substring(0, line.IndexOf("/"))) == 0 ? Constants.NullNumber : Convert.ToDouble(line.Substring(0, line.IndexOf("/")));
                    line = line.Substring(line.IndexOf("/") + 1);
                    double cohesion = Convert.ToDouble(line.Substring(0, line.IndexOf("/"))) == 0 ? Constants.NullNumber : Convert.ToDouble(line.Substring(0, line.IndexOf("/")));
                    line = line.Substring(line.IndexOf("/") + 1);
                    double permeability = line.Substring(0, line.IndexOf("/")) == "0.000000E+000" ? Constants.NullNumber : Convert.ToDouble(line.Substring(0, line.IndexOf("/")));
                    RoutineSoilTest rst = new RoutineSoilTest(zkNumber, sampleDepth, sampleLayer, waterLevel, density, specificGravity, voidRatio, saturation, liquidLimit, plasticLimit, plasticIndex, liquidityIndex, compressibility, modulus, frictionAngle, cohesion, permeability);
                    rsts.Add(rst);
                }
                rstReader.Close();

                GC.Collect();
                GC.WaitForPendingFinalizers();
                RoutineSoilTestDataBase.Refresh(projectName, rsts);

                projectlist.Add(projectName);
            }

            // 成功提示
            StringBuilder sb = new StringBuilder();
            if (projectlist.Count > 0)
            {
                sb.AppendLine("已成功导入以下项目：");
                for (int i = 0; i < projectlist.Count; i++)
                    sb.AppendLine((i + 1).ToString() + projectlist[i]);
            }
            else
                sb.AppendLine("没有导入任何项目。");
            MessageBox.Show(sb.ToString());
        }

        #endregion
    }
}
