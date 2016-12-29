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
    /// CPTControl.xaml 的交互逻辑
    /// </summary>
    public partial class CPTControl : UserControl
    {
        #region 参数定义

        /// <summary>
        /// 判断是否为新建触探孔的参数
        /// </summary>
        public bool isNewJk;

        /// <summary>
        /// 定义编辑触探孔时的老触探孔名称
        /// </summary>
        public string oldJkName = "";

        /// <summary>
        /// 定义分层列表LayerListDataGrid控件的数据源DataTable
        /// </summary>
        public DataTable dtLayer = new DataTable("Layer");

        /// <summary>
        /// 定义摩阻力列表PsListDataGrid控件的数据源DataTable
        /// </summary>
        public DataTable dtPs = new DataTable("Ps");

        /// <summary>
        /// 定义摩阻力深度列表PsDepthListDataGrid控件的数据源DataTable
        /// </summary>
        public DataTable dtDepth = new DataTable("PsDepth");

        /// <summary>
        /// 定义钻孔编号、孔口高程的工具提示
        /// </summary>
        System.Windows.Controls.ToolTip tt0 = new System.Windows.Controls.ToolTip();
        System.Windows.Controls.ToolTip tt1 = new System.Windows.Controls.ToolTip();
        System.Windows.Controls.ToolTip tt2 = new System.Windows.Controls.ToolTip();
        System.Windows.Controls.ToolTip tt3 = new System.Windows.Controls.ToolTip();
        System.Windows.Controls.ToolTip tt4 = new System.Windows.Controls.ToolTip();
        System.Windows.Controls.ToolTip tt5 = new System.Windows.Controls.ToolTip();

        /// <summary>
        /// 定义保存按钮可用性判断参数
        /// </summary>
        bool setCanJkName = false;
        bool setCanJkAltitude = false;
        bool setCanJkAxisX = true;
        bool setCanJkAxisY = true;

        #endregion

        #region 工具提示

        /// <summary>
        /// 定义工具提示
        /// </summary>
        private void DefineToolTip()
        {
            tt0.Content = "触探孔编号重复";
            this.JKNameTextBox.ToolTip = tt0;
            tt0.PlacementTarget = this.JKNameTextBox;
            tt0.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            tt0.Foreground = Brushes.Red;

            tt1.Content = "触探孔编号不能为空";
            this.JKNameTextBox.ToolTip = tt1;
            tt1.PlacementTarget = this.JKNameTextBox;
            tt1.Foreground = Brushes.Red;
            tt1.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;

            tt2.Content = "孔口高程不能为空";
            this.JKAltitudeTextBox.ToolTip = tt2;
            tt2.PlacementTarget = this.JKAltitudeTextBox;
            tt2.Foreground = Brushes.Red;
            tt2.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;

            tt3.Content = "输入的高程不是有效数字";
            this.JKAltitudeTextBox.ToolTip = tt3;
            tt3.PlacementTarget = this.JKAltitudeTextBox;
            tt3.Foreground = Brushes.Red;
            tt3.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;

            tt4.Content = "输入的X坐标不是有效数字";
            this.AxisXTextBox.ToolTip = tt4;
            tt4.PlacementTarget = this.AxisXTextBox;
            tt4.Foreground = Brushes.Red;
            tt4.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;

            tt5.Content = "输入的Y坐标不是有效数字";
            this.AxisYTextBox.ToolTip = tt5;
            tt5.PlacementTarget = this.AxisYTextBox;
            tt5.Foreground = Brushes.Red;
            tt5.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
        }

        /// <summary>
        /// 触探孔编号输入框内容变化时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void JKNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            tt0.IsOpen = false;
            tt1.IsOpen = false;
            string str = this.JKNameTextBox.Text;
            if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
            {
                this.JKNameTextBox.BorderBrush = Brushes.Red;
                setCanJkName = false;
                tt1.IsOpen = true;
            }
            else if (CPTDataBase.ReadJkList(Program.currentProject).Contains(str) && str!=oldJkName)
            {
                this.JKNameTextBox.BorderBrush = Brushes.Red;
                setCanJkName = false;
                tt0.IsOpen = true;
            }
            else
            {
                this.JKNameTextBox.BorderBrush = Brushes.Gray;
                setCanJkName = true;
            }
        }

        /// <summary>
        /// 孔口高程输入框内容变化时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void JKAltitudeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            tt2.IsOpen = false;
            tt3.IsOpen = false;
            string str = this.JKAltitudeTextBox.Text;
            double num;
            if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
            {
                this.JKAltitudeTextBox.BorderBrush = Brushes.Red;
                setCanJkAltitude = false;
                tt2.IsOpen = true;
            }
            else if(!double.TryParse(str,out num))
            {
                this.JKAltitudeTextBox.BorderBrush = Brushes.Red;
                setCanJkAltitude = false;
                tt3.IsOpen = true;
            }
            else
            {
                this.JKAltitudeTextBox.BorderBrush = Brushes.Gray;
                setCanJkAltitude = true;
            }
        }

        /// <summary>
        /// X坐标输入框内容变化时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AxisXTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            tt4.IsOpen = false;
            string str = this.AxisXTextBox.Text;
            double num;
            if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
            {
                this.AxisXTextBox.BorderBrush = Brushes.Gray;
                setCanJkAxisX = true;
            }
            else if(!double.TryParse(str,out num))
            {
                this.AxisXTextBox.BorderBrush = Brushes.Red;
                setCanJkAxisX = false;
                tt4.IsOpen = true;
            }
            else
            {
                this.AxisXTextBox.BorderBrush = Brushes.Gray;
                setCanJkAxisX = true;
            }
        }

        /// <summary>
        /// Y坐标输入框内容变化时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AxisYTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            tt5.IsOpen = false;
            string str = this.AxisYTextBox.Text;
            double num;
            if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
            {
                this.AxisYTextBox.BorderBrush = Brushes.Gray;
                setCanJkAxisY = true;
            }
            else if(!double.TryParse(str,out num))
            {
                this.AxisYTextBox.BorderBrush = Brushes.Red;
                setCanJkAxisY = false;
                tt5.IsOpen = true;
            }
            else
            {
                this.AxisYTextBox.BorderBrush = Brushes.Gray;
                setCanJkAxisY = true;
            }
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 无参数的构造函数，新建静力触探孔时调用
        /// </summary>
        public CPTControl()
        {
            InitializeComponent();

            // 设置当前为新触探孔，设置删除按钮可用性
            this.DeleteJkButton.IsEnabled = false;
            isNewJk = true;

            // 初始化DataTable
            InitialLayerListDataTable();
            InitialPsListDataTable();

            // 定义各输入框的工具提示
            DefineToolTip();

            // 设置绑定
            this.LayerListDataGrid.DataContext = dtLayer;
            this.PsListDataGrid.DataContext = dtPs;
        }

        /// <summary>
        /// 带参数的构造函数，选取某静力触探孔时调用
        /// </summary>
        /// <param name="_selectCPT">所选静力触探孔</param>
        public CPTControl(CPT _selectCPT)
        {
            InitializeComponent();

            // 设置当前为旧触探孔，设置删除按钮可用性
            this.DeleteJkButton.IsEnabled = true;
            isNewJk = false;
            oldJkName = _selectCPT.Name;

            // 初始化DataTable
            InitialLayerListDataTable(_selectCPT.Layers);
            InitialPsListDataTable(_selectCPT.PsList);

            // 定义各输入框的工具提示
            DefineToolTip();

            // 设置绑定
            this.LayerListDataGrid.DataContext = dtLayer;
            this.PsListDataGrid.DataContext = dtPs;

            // 赋值基本信息
            this.JKNameTextBox.Text = _selectCPT.Name;
            this.JKAltitudeTextBox.Text = _selectCPT.Altitude.ToString();
            this.AxisXTextBox.Text = _selectCPT.X.ToString() != "-0.19880205" ? _selectCPT.ToString() : null;
            this.AxisYTextBox.Text = _selectCPT.Y.ToString() != "-0.19880205" ? _selectCPT.ToString() : null;

            // 更新状态标签
            this.IsChangedTextBlock.Text = "";

            // 绘图
            DrawJk();
        }

        #endregion

        #region 触探孔分层

        /// <summary>
        /// 初始化LayerListDataTable，不带参数
        /// </summary>
        private void InitialLayerListDataTable()
        {
            // 定义LayerListDataTable数据列
            dtLayer.Columns.Add(new DataColumn("numberList", typeof(string)));
            dtLayer.Columns.Add(new DataColumn("nameList", typeof(string)));
            dtLayer.Columns.Add(new DataColumn("geoList", typeof(string)));
            dtLayer.Columns.Add(new DataColumn("depthList", typeof(double)));
            dtLayer.Columns.Add(new DataColumn("descriptionList", typeof(string)));
        }

        /// <summary>
        /// 初始化LayerListDataTable，带参数
        /// </summary>
        /// <param name="_layers">钻孔分层列表</param>
        private void InitialLayerListDataTable(List<ZkLayer> _layers)
        {
            // 定义LayerListDataTable数据列
            dtLayer.Columns.Add(new DataColumn("numberList", typeof(string)));
            dtLayer.Columns.Add(new DataColumn("nameList", typeof(string)));
            dtLayer.Columns.Add(new DataColumn("geoList", typeof(string)));
            dtLayer.Columns.Add(new DataColumn("depthList", typeof(double)));
            dtLayer.Columns.Add(new DataColumn("descriptionList", typeof(string)));

            // 赋值
            DataRow dr;
            for (int i = 0; i < _layers.Count; i++)
            {
                dr = dtLayer.NewRow();
                dr["numberList"] = _layers[i].Number;
                dr["nameList"] = _layers[i].Name;
                dr["geoList"] = _layers[i].Geo;
                dr["depthList"] = _layers[i].Depth;
                dr["descriptionList"] = _layers[i].Description;
                dtLayer.Rows.Add(dr);
            }
        }

        /// <summary>
        /// 向LayerListDataTable添加行
        /// </summary>
        /// <param name="_number">分层编号</param>
        /// <param name="_name">岩土名称</param>
        /// <param name="_geo">地质年代成因</param>
        /// <param name="_depth">层底深度</param>
        /// <param name="_description">分层描述</param>
        private void AddRowToLayerListDataTable(string _number, string _name, string _geo, double _depth, string _description)
        {
            DataRow dr = dtLayer.NewRow();
            dr["numberList"] = _number;
            dr["nameList"] = _name;
            dr["geoList"] = _geo;
            dr["depthList"] = _depth;
            dr["descriptionList"] = _description;
            dtLayer.Rows.Add(dr);
        }

        /// <summary>
        /// 编辑LayerListDataTable行
        /// </summary>
        /// <param name="_rowIndex">行号</param>
        /// <param name="_number">分层编号</param>
        /// <param name="_name">岩土名称</param>
        /// <param name="_geo">地质年代成因</param>
        /// <param name="_depth">层底深度</param>
        /// <param name="_description">分层描述</param>
        private void EditRowLayerListDataTable(int _rowIndex, string _number, string _name, string _geo, double _depth, string _description)
        {
            DataRow dr = dtLayer.Rows[_rowIndex];
            dr.BeginEdit();
            dr["numberList"] = _number;
            dr["nameList"] = _name;
            dr["geoList"] = _geo;
            dr["depthList"] = _depth;
            dr["descriptionList"] = _description;
            dr.EndEdit();
        }

        /// <summary>
        /// 删除LayerListDataTable行
        /// </summary>
        /// <param name="_rowIndex">行号</param>
        private void DeleteRowLayerListDataTable(int _rowIndex)
        {
            dtLayer.Rows.RemoveAt(_rowIndex);
        }

        /// <summary>
        /// 点击"添加分层"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddLayerButton_Click(object sender, RoutedEventArgs e)
        {
            // 传递上一层的层底深度，如果没有上一层，则将lastDepth赋值为-1
            double lastDepth = -1;
            if (this.LayerListDataGrid.Items.Count > 0)
            {
                DataRowView drv = (DataRowView)this.LayerListDataGrid.Items[this.LayerListDataGrid.Items.Count - 1];
                lastDepth = (double)drv[3];
            }

            // 实例化窗口
            ZkLayerDetail newLayer = new ZkLayerDetail(lastDepth);
            newLayer.ShowDialog();
            if (newLayer.DialogResult == true)
            {
                string number = newLayer.LayerNumberComboBox.Text;
                string name = newLayer.LayerNameComboBox.Text;
                string geo = newLayer.LayerGeoComboBox.Text;
                double depth = Convert.ToDouble(newLayer.LayerDepthTextBox.Text);
                string description = newLayer.LayerDescriptionTextBox.Text;
                AddRowToLayerListDataTable(number, name, geo, depth, description);

                // 绘图
                DrawJk();
            }
        }

        /// <summary>
        /// 点击"编辑分层"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditLayerButton_Click(object sender, RoutedEventArgs e)
        {
            // 传递上一层的层底深度，如果没有上一层，则将lastDepth赋值为-1
            double lastDepth = -1;
            int selectIndex = this.LayerListDataGrid.SelectedIndex;
            if (selectIndex > 0)
            {
                DataRowView drv0 = (DataRowView)this.LayerListDataGrid.Items[selectIndex - 1];
                lastDepth = (double)drv0[3];
            }

            // 赋值传递数据
            DataRowView drv = (DataRowView)this.LayerListDataGrid.SelectedItem;
            string number = (string)drv[0];
            string name = (string)drv[1];
            string geo = (string)drv[2];
            double depth = (double)drv[3];
            string description = (string)drv[4];

            // 实例化窗口
            ZkLayerDetail editLayer = new ZkLayerDetail(lastDepth, number, depth, description);
            editLayer.ShowDialog();
            if (editLayer.DialogResult == true)
            {
                string newNumber = editLayer.LayerNumberComboBox.Text;
                string newName = editLayer.LayerNameComboBox.Text;
                string newGeo = editLayer.LayerGeoComboBox.Text;
                double newDepth = Convert.ToDouble(editLayer.LayerDepthTextBox.Text);
                string newDescription = editLayer.LayerDescriptionTextBox.Text;
                EditRowLayerListDataTable(selectIndex, newNumber, newName, newGeo, newDepth, newDescription);

                // 绘图
                DrawJk();
            }
        }

        /// <summary>
        /// 点击"删除分层"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteLayerButton_Click(object sender, RoutedEventArgs e)
        {
            DataRowView drv = (DataRowView)this.LayerListDataGrid.SelectedItem;
            string selectedNumber = (string)drv[0];
            int selectedIndex = LayerListDataGrid.SelectedIndex;
            MessageBoxResult result = MessageBox.Show("确定要删除第 " + selectedNumber + " 层骂？", "删除触探孔分层", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (result == MessageBoxResult.OK)
            {
                DeleteRowLayerListDataTable(selectedIndex);

                // 绘图
                DrawJk();
            }
        }

        #endregion

        #region 触探孔摩阻力

        /// <summary>
        /// 初始化PsListDataTable，不带参数
        /// </summary>
        private void InitialPsListDataTable()
        {
            // 定义PsListDataTable数据列
            dtPs.Columns.Add(new DataColumn("PsList", typeof(double)));
            dtPs.Columns.Add(new DataColumn("PsDepthList", typeof(double)));

            // 订阅RowChanged事件，处理函数为RowChanged
            dtPs.RowChanged += new DataRowChangeEventHandler(RowChanged);
        }

        /// <summary>
        /// 初始化PsListDataTable，带参数
        /// </summary>
        /// <param name="_psList">ps值列表</param>
        private void InitialPsListDataTable(List<double> _psList)
        {
            // 定义PsListDataTable数据列
            dtPs.Columns.Add(new DataColumn("PsList", typeof(double)));
            dtPs.Columns.Add(new DataColumn("PsDepthList", typeof(double)));

            // 赋值Ps值
            DataRow dr;
            for(int i = 0; i < _psList.Count; i++)
            {
                dr = dtPs.NewRow();
                dr["PsList"] = _psList[i];
                dtPs.Rows.Add(dr);
            }

            // 订阅RowChanged事件，处理函数为RowChanged
            dtPs.RowChanged += new DataRowChangeEventHandler(RowChanged);
        }

        /// <summary>
        /// 刷新PsListDataTable
        /// </summary>
        /// <param name="_psList">ps值列表</param>
        private void RefreshPsListDataTable(List<double> _psList)
        {
            dtPs.Rows.Clear();
            DataRow dr;
            for (int i = 0; i < _psList.Count; i++)
            {
                dr = dtPs.NewRow();
                dr["PsList"] = _psList[i];
                dtPs.Rows.Add(dr);
            }
        }

        #endregion

        #region 保存触探孔

        /// <summary>
        /// Save命令的Executed事件处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CommandBindingSave_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // 提取触探孔基本信息参数
            string jkName = this.JKNameTextBox.Text;
            double jkAltitude = Convert.ToDouble(this.JKAltitudeTextBox.Text);
            double jkAxisX = !string.IsNullOrEmpty(this.AxisXTextBox.Text) && !string.IsNullOrWhiteSpace(this.AxisXTextBox.Text) ? Convert.ToDouble(this.AxisXTextBox.Text) : -0.19880205;
            double jkAxisY = !string.IsNullOrEmpty(this.AxisYTextBox.Text) && !string.IsNullOrWhiteSpace(this.AxisYTextBox.Text) ? Convert.ToDouble(this.AxisYTextBox.Text) : -0.19880205;

            // 提取触探孔分层参数
            List<string> jkLayerNumberList = new List<string>();
            List<string> jkLayerNameList = new List<string>();
            List<string> jkLayerGeoList = new List<string>();
            List<double> jkLayerDepthList = new List<double>();
            List<string> jkLayerDescriptionList = new List<string>();
            for (int i = 0; i < dtLayer.Rows.Count; i++)
            {
                jkLayerNumberList.Add((string)dtLayer.Rows[i][0]);
                jkLayerNameList.Add((string)dtLayer.Rows[i][1]);
                jkLayerGeoList.Add((string)dtLayer.Rows[i][2]);
                jkLayerDepthList.Add((double)dtLayer.Rows[i][3]);
                jkLayerDescriptionList.Add((string)dtLayer.Rows[i][4]);
            }

            // 提取触探孔摩阻力参数
            List<double> jkPsList = new List<double>();
            for (int i = 0; i < dtPs.Rows.Count; i++)
            {
                jkPsList.Add((double)dtPs.Rows[i][0]);
            }

            // 保存触探孔到数据库
            if (isNewJk == false)
            {
                CPTDataBase.RemoveJk(Program.currentProject, oldJkName);
            }
            CPTDataBase.AddJkBasicInfo(Program.currentProject, jkName, jkAltitude, jkAxisX, jkAxisY);
            CPTDataBase.AddLayerListToJk(Program.currentProject, jkName, jkLayerNumberList, jkLayerNameList, jkLayerGeoList, jkLayerDescriptionList, jkLayerDepthList);
            CPTDataBase.AddPsListToJk(Program.currentProject, jkName, jkPsList);

            // 更新导航树
            if (!MainWindow.bind.IsExistSecondTreeItem(2, "静力触探"))
            {
                MainWindow.bind.AddItemToSecondTree(2, "静力触探");
            }
            MainWindow.bind.ReSetJkItem(Program.currentProject);

            // 更新状态标签
            this.IsChangedTextBlock.Text = "已保存";
            this.IsChangedTextBlock.Foreground = Brushes.Blue;
            MessageBox.Show("保存成功！");
            this.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Save命令的CanExecuted事件处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CommandBindingSave_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            // 绑定"保存此钻孔"的可用状态
            if (setCanJkName == true && setCanJkAltitude == true && setCanJkAxisX == true && setCanJkAxisY == true)
            {
                e.CanExecute = true;
            }
        }

        #endregion

        #region 删除触探孔

        /// <summary>
        /// 点击"删除此钻孔"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteJkButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(oldJkName) && !string.IsNullOrWhiteSpace(oldJkName))
            {
                MessageBoxResult result = MessageBox.Show("您确定要删除 " + oldJkName + " 吗？\n该触探孔的所有资料将被删除，且不可恢复。", "删除静力触探孔", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    CPTDataBase.RemoveJk(Program.currentProject, oldJkName);
                    MainWindow.bind.ReSetJkItem(Program.currentProject);
                    this.Visibility = Visibility.Collapsed;
                }
            }
        }

        #endregion

        #region 相关事件

        /// <summary>
        /// dtPs.RowChanged事件的处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RowChanged(object sender,DataRowChangeEventArgs e)
        {
            DrawJk();
        }

        #endregion

        #region 绘制摩阻力曲线图

        /// <summary>
        /// 绘制钻孔柱状图函数
        /// </summary>
        public void DrawJk()
        {
            // 清空旧柱状图
            JkCanvas.Children.Clear();

            // 如果没有Ps数据，则退出
            List<double> psList = new List<double>();
            for (int i = 0; i < dtPs.Rows.Count; i++)
            {
                double num;
                if(!double.TryParse(dtPs.Rows[i][0].ToString(),out num))
                {
                    continue;
                }
                double ps = Convert.ToDouble(dtPs.Rows[i][0]);
                psList.Add(ps);
            }
            if (psList.Count == 0)
            {
                return;
            }

            // 提取最大Ps值
            double maxPs = psList.Max();

            // 提取绘图区域宽度和高度
            double canvasWidth = this.JkCanvas.Width;
            double canvasHeight = this.JkCanvas.Height;

            // 绘制触探孔孔表头
            DrawJkLabel();

            // 绘制地面线和触探孔线
            DrawJkLine();

            // 绘制刻度和网格
            DrawJkScale(maxPs);

            // 绘制触探孔分层
            double maxDepth = dtLayer.Rows.Count == 0 ? 0 : (double)dtLayer.Rows[dtLayer.Rows.Count - 1][3];
            for (int i = 0; i < dtLayer.Rows.Count; i++)
            {
                double depth = (double)dtLayer.Rows[i][3];
                double oldDepth = i == 0 ? depth : (double)dtLayer.Rows[i - 1][3];
                string layerLabel = dtLayer.Rows[i][0].ToString() + " " + dtLayer.Rows[i][1].ToString();
                DrawJkLayer(depth, oldDepth, maxDepth, layerLabel);
            }

            // 绘制摩阻力曲线
            DrawJkCurve(maxPs);
        }

        /// <summary>
        /// 绘制触探孔孔表头函数
        /// </summary>
        private void DrawJkLabel()
        {
            // 提取绘图区域宽度和高度
            double w = this.JkCanvas.Width;
            double h = this.JkCanvas.Height;

            // 绘图
            this.JkCanvas.DrawLine(0, 33, 60, 33);
            this.JkCanvas.DrawText(15, 15, this.JKNameTextBox.Text, 14, Brushes.Black, true, false, true);
            this.JkCanvas.DrawText(15, 32, this.JKAltitudeTextBox.Text, 14, Brushes.Black, true, false, true);
        }

        /// <summary>
        /// 绘制地面线和触探孔线函数
        /// </summary>
        private void DrawJkLine()
        {
            // 提取绘图区域宽度和高度
            double w = this.JkCanvas.Width;
            double h = this.JkCanvas.Height;

            // 设置地面线Y坐标
            double y = 50;

            // 绘图
            this.JkCanvas.DrawLine(0, y, w, y, 2, Brushes.Black);
            this.JkCanvas.DrawLine(25, y, 25, h, 8, Brushes.Black);
        }

        /// <summary>
        /// 绘制刻度和网格函数
        /// </summary>
        /// <param name="_maxScale">最大尺寸</param>
        private void DrawJkScale(double _maxScale)
        {
            // 提取绘图区域宽度和高度
            double w = this.JkCanvas.Width;
            double h = this.JkCanvas.Height;

            // 设置地面线Y坐标
            double y = 50;

            // 设置最大刻度范围和刻度间距
            double minScale = _maxScale < 4 ? 0.5 : 1;
            double maxScale = Math.Ceiling(_maxScale * 2 / minScale) * minScale;
            int countScale = Convert.ToInt32(maxScale / minScale);

            // 绘图
            for(int i = 1; i < countScale; i++)
            {
                this.JkCanvas.DrawLine(25 + (w - 25) / countScale * i, y, 25 + (w - 25) / countScale * i, y - 3);
                this.JkCanvas.DrawText(25 + (w - 25) / countScale * i, y - 20, (minScale * i).ToString());
                this.JkCanvas.DrawDotLine(25 + (w - 25) / countScale * i, y, 25 + (w - 25) / countScale * i, h, 0.5, Brushes.Gray);
            }
        }

        /// <summary>
        /// 绘制触探孔分层函数
        /// </summary>
        /// <param name="_depth">层底深度</param>
        /// <param name="_oldDepth">上一层层底深度</param>
        /// <param name="_maxDepth">最大钻探深度</param>
        /// <param name="_layerLabel">分层标签</param>
        private void DrawJkLayer(double _depth, double _oldDepth, double _maxDepth, string _layerLabel)
        {
            // 提取绘图区域宽度和高度
            double w = this.JkCanvas.Width;
            double h = this.JkCanvas.Height;

            // 设置地面线Y坐标
            double dmy = 50;

            // 计算分层线Y坐标
            double y = dmy + (h - dmy) * (_depth / _maxDepth);
            double oldY = _oldDepth == _depth ? dmy : dmy + (h - dmy) * (_oldDepth / _maxDepth);

            // 计算分层标签
            string layerDepthLabel = _depth.ToString() + "(" + (Convert.ToDouble(this.JKAltitudeTextBox.Text) - _depth).ToString() + ")";

            // 绘图
            this.JkCanvas.DrawDotLine(0, y, w - 100, y);
            this.JkCanvas.DrawText(w - 100, y - 10, layerDepthLabel, 14, Brushes.DarkBlue, true, false, true);
            this.JkCanvas.DrawText(50, (y + oldY) / 2 - 10, _layerLabel, 14, Brushes.Red, true, false, false);
        }

        /// <summary>
        /// 绘制摩阻力曲线函数
        /// </summary>
        /// <param name="_maxScale">最大尺寸</param>
        private void DrawJkCurve(double _maxScale)
        {
            // 提取绘图区域宽度和高度
            double w = this.JkCanvas.Width;
            double h = this.JkCanvas.Height;

            // 设置地面线Y坐标
            double y = 50;

            // 设置最大刻度范围和刻度间距
            double minScale = 0.5;
            double maxScale = Math.Ceiling(_maxScale * 2 / minScale) * minScale;

            // 提取Ps列表数据
            List<double> psList = new List<double>();
            for(int i = 0; i < dtPs.Rows.Count; i++)
            {
                double num;
                if (!double.TryParse(dtPs.Rows[i][0].ToString(), out num))
                {
                    continue;
                }
                double ps = (double)dtPs.Rows[i][0];
                psList.Add(ps);
            }

            // 转换坐标列表
            List<double> XList = new List<double>();
            List<double> YList = new List<double>();
            for(int i = 0; i < psList.Count; i++)
            {
                double _x = 25 + (w - 25) * psList[i] / maxScale;
                double _y = y + (h - y) / psList.Count * i;
                XList.Add(_x);
                YList.Add(_y);
            }

            // 绘图
            this.JkCanvas.DrawPLine(XList, YList, 1, Brushes.Green);
        }

        #endregion

        #region 复制和粘贴

        /// <summary>
        /// 按下CTRL+V，复制Excel中的数据粘贴到DataGrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PsListDataGrid_KeyDown(object sender, KeyEventArgs e)
        {
            // 判断按下的是否为CTRL+V
            if (ModifierKeys.Control == Keyboard.Modifiers && e.Key == Key.V)
            {
                // 获取Excel复制的数据，数据为空时退出函数
                List<string[]> excelData = OfficeOperation.GetDataFromExcelClipBoard();
                if (excelData == null)
                {
                    return;
                }

                // 获取当前选中的DataGrid行号和列号
                int row, column;
                if (this.PsListDataGrid.SelectedCells.Count == 0)
                {
                    row = 0;
                    column = 0;
                }
                else
                {
                    row = this.PsListDataGrid.Items.IndexOf(this.PsListDataGrid.SelectedCells[0].Item);
                    column = this.PsListDataGrid.SelectedCells[0].Column.DisplayIndex;
                }

                // 将复制的数据添加到绑定的DataTable
                dtPs = DtOperation.PasteFromExcel(dtPs, row, column, excelData, -1);
            }
        }

        #endregion
    }
}
