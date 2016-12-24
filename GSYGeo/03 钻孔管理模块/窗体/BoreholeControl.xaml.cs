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
    /// Borehole.xaml 的交互逻辑
    /// </summary>
    public partial class BoreholeControl : UserControl
    {
        #region 参数定义

        /// <summary>
        /// 判断是否为新建钻孔的参数
        /// </summary>
        public bool isNewZk;

        /// <summary>
        /// 定义编辑钻孔时的老钻孔名称
        /// </summary>
        public string oldZkName = "";

        /// <summary>
        /// 定义分层列表LayerListDataGrid控件的数据源DataTable
        /// </summary>
        public DataTable dtLayer = new DataTable("Layer");

        /// <summary>
        /// 定义取样列表SampleListDataGrid控件的数据源DataTable
        /// </summary>
        public DataTable dtSample = new DataTable("Sample");

        /// <summary>
        /// 定义标贯/动探列表NTestListDataTable控件的数据源DataTable
        /// </summary>
        public DataTable dtNTest = new DataTable("NTest");

        /// <summary>
        /// 定义钻孔编号、孔口高程的工具提示
        /// </summary>
        System.Windows.Controls.ToolTip tt0 = new System.Windows.Controls.ToolTip();
        System.Windows.Controls.ToolTip tt1 = new System.Windows.Controls.ToolTip();
        System.Windows.Controls.ToolTip tt2 = new System.Windows.Controls.ToolTip();
        System.Windows.Controls.ToolTip tt3 = new System.Windows.Controls.ToolTip();
        System.Windows.Controls.ToolTip tt4 = new System.Windows.Controls.ToolTip();
        System.Windows.Controls.ToolTip tt5 = new System.Windows.Controls.ToolTip();
        System.Windows.Controls.ToolTip tt6 = new System.Windows.Controls.ToolTip();
        System.Windows.Controls.ToolTip tt7 = new System.Windows.Controls.ToolTip();

        /// <summary>
        /// 定义保存按钮可用性判断参数
        /// </summary>
        bool setCanZkName = false;
        bool setCanZkAltitude = false;
        bool setCanZkAxisX = true;
        bool setCanZkAxisY = true;
        bool setCanZkIniWL = true;
        bool setCanZkStaWL = true;

        #endregion

        #region 工具提示

        /// <summary>
        /// 定义工具提示
        /// </summary>
        private void DefineToolTip()
        {
            tt0.Content = "钻孔编号重复";
            this.ZKNameTextBox.ToolTip = tt0;
            tt0.PlacementTarget = this.ZKNameTextBox;
            tt0.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            tt0.Foreground = Brushes.Red;
            
            tt1.Content = "钻孔编号不能为空";
            this.ZKNameTextBox.ToolTip = tt1;
            tt1.PlacementTarget = this.ZKNameTextBox;
            tt1.Foreground = Brushes.Red;
            tt1.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;

            tt2.Content = "孔口高程不能为空";
            this.ZKAltitudeTextBox.ToolTip = tt2;
            tt2.PlacementTarget = this.ZKAltitudeTextBox;
            tt2.Foreground = Brushes.Red;
            tt2.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;

            tt3.Content = "输入的高程不是有效数字";
            this.ZKAltitudeTextBox.ToolTip = tt3;
            tt3.PlacementTarget = this.ZKAltitudeTextBox;
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

            tt6.Content = "输入的初见水位不是有效数字";
            this.InitialWaterLevelTextBox.ToolTip = tt6;
            tt6.PlacementTarget = this.InitialWaterLevelTextBox;
            tt6.Foreground = Brushes.Red;
            tt6.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;

            tt7.Content = "输入的稳定水位不是有效数字";
            this.StableWaterLevelTextBox.ToolTip = tt7;
            tt7.PlacementTarget = this.StableWaterLevelTextBox;
            tt7.Foreground = Brushes.Red;
            tt7.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
        }

        /// <summary>
        /// 钻孔编号输入框内容变化时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZKNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            tt0.IsOpen = false;
            tt1.IsOpen = false;
            string str = this.ZKNameTextBox.Text;
            if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
            {
                this.ZKNameTextBox.BorderBrush = Brushes.Red;
                setCanZkName = false;
                tt1.IsOpen = true;
            }
            else if (BoreholeDataBase.ReadZkList(Program.currentProject).Contains(str) && str!=oldZkName)
            {
                this.ZKNameTextBox.BorderBrush = Brushes.Red;
                setCanZkName = false;
                tt0.IsOpen = true;
            }
            else
            {
                this.ZKNameTextBox.BorderBrush = Brushes.Gray;
                setCanZkName = true;

                // 绘图
                DrawZk();
            }
        }

        /// <summary>
        /// 孔口高程输入框内容变化时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZKAltitudeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            tt2.IsOpen = false;
            tt3.IsOpen = false;
            string str = this.ZKAltitudeTextBox.Text;
            double num;
            if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
            {
                this.ZKAltitudeTextBox.BorderBrush = Brushes.Red;
                tt2.IsOpen = true;
                setCanZkAltitude = false;
            }
            else if(!double.TryParse(str,out num))
            {
                this.ZKAltitudeTextBox.BorderBrush = Brushes.Red;
                tt3.IsOpen = true;
                setCanZkAltitude = false;
            }
            else
            {
                this.ZKAltitudeTextBox.BorderBrush = Brushes.Gray;
                setCanZkAltitude = true;

                // 绘图
                DrawZk();
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
                setCanZkAxisX = true;
            }
            else if(!double.TryParse(str,out num))
            {
                this.AxisXTextBox.BorderBrush = Brushes.Red;
                tt4.IsOpen = true;
                setCanZkAxisX = false;
            }
            else
            {
                this.AxisXTextBox.BorderBrush = Brushes.Gray;
                setCanZkAxisX = true;
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
                setCanZkAxisY = true;
            }
            else if (!double.TryParse(str, out num))
            {
                this.AxisYTextBox.BorderBrush = Brushes.Red;
                tt5.IsOpen = true;
                setCanZkAxisY = false;
            }
            else
            {
                this.AxisYTextBox.BorderBrush = Brushes.Gray;
                setCanZkAxisY = true;
            }
        }

        /// <summary>
        /// 初见水位输入框内容变化时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InitialWaterLevelTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            tt6.IsOpen = false;
            string str = this.InitialWaterLevelTextBox.Text;
            double num;
            if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
            {
                this.InitialWaterLevelTextBox.BorderBrush = Brushes.Gray;
                setCanZkIniWL = true;
            }
            else if(!double.TryParse(str,out num))
            {
                this.InitialWaterLevelTextBox.BorderBrush = Brushes.Red;
                tt6.IsOpen = true;
                setCanZkIniWL = false;
            }
            else
            {
                this.InitialWaterLevelTextBox.BorderBrush = Brushes.Gray;
                setCanZkIniWL = true;
            }
        }

        /// <summary>
        /// 稳定水位输入框内容变化时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StableWaterLevelTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            tt7.IsOpen = false;
            string str = this.StableWaterLevelTextBox.Text;
            double num;
            if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
            {
                this.StableWaterLevelTextBox.BorderBrush = Brushes.Gray;
                setCanZkStaWL = true;
            }
            else if(!double.TryParse(str,out num))
            {
                this.StableWaterLevelTextBox.BorderBrush = Brushes.Red;
                tt7.IsOpen = true;
                setCanZkStaWL = false;
            }
            else
            {
                this.StableWaterLevelTextBox.BorderBrush = Brushes.Gray;
                setCanZkStaWL = true;
            }
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 无参数的构造函数，新建钻孔时调用
        /// </summary>
        public BoreholeControl()
        {
            InitializeComponent();

            // 设置当前为新钻孔，设置删除按钮可用性
            this.DeleteZkButton.IsEnabled = false;
            isNewZk = true;

            // 初始化DataTable
            InitialLayerListDataTable();
            InitialSampleListDataTable();
            InitialNTestListDataTable();

            // 定义各输入框的工具提示
            DefineToolTip();

            // 设置绑定
            this.LayerListDataGrid.DataContext = dtLayer;
            this.SampleListDataGrid.DataContext = dtSample;
            this.NTestListDataGrid.DataContext = dtNTest;
        }

        /// <summary>
        /// 带参数的构造函数，选取某钻孔时调用
        /// </summary>
        /// <param name="_selectedZk">所选钻孔</param>
        public BoreholeControl(Borehole _selectedZk)
        {
            InitializeComponent();

            // 设置当前为旧钻孔，设置删除按钮可用性
            this.DeleteZkButton.IsEnabled = true;
            isNewZk = false;
            oldZkName = _selectedZk.Name;

            // 初始化DataTable
            InitialLayerListDataTable();
            InitialSampleListDataTable();
            InitialNTestListDataTable();

            // 定义各输入框的工具提示
            DefineToolTip();

            // 设置绑定
            this.LayerListDataGrid.DataContext = dtLayer;
            this.SampleListDataGrid.DataContext = dtSample;
            this.NTestListDataGrid.DataContext = dtNTest;

            // 赋值基本信息
            this.ZKNameTextBox.Text = _selectedZk.Name;
            this.ZKAltitudeTextBox.Text = _selectedZk.Altitude.ToString();
            this.AxisXTextBox.Text = _selectedZk.X.ToString() != "-0.19880205" ? _selectedZk.X.ToString() : null;
            this.AxisYTextBox.Text = _selectedZk.Y.ToString() != "-0.19880205" ? _selectedZk.Y.ToString() : null;
            this.InitialWaterLevelTextBox.Text = _selectedZk.InitialWaterLevel.ToString() != "-0.19880205" ? _selectedZk.InitialWaterLevel.ToString() : null;
            this.StableWaterLevelTextBox.Text = _selectedZk.StableWaterLevel.ToString() != "-0.19880205" ? _selectedZk.StableWaterLevel.ToString() : null;

            // 赋值分层列表
            for (int i = 0; i < _selectedZk.Layers.Count; i++)
            {
                string number = _selectedZk.Layers[i].Number;
                string name = _selectedZk.Layers[i].Name;
                string geo = _selectedZk.Layers[i].Geo;
                string description = _selectedZk.Layers[i].Description;
                double depth = _selectedZk.Layers[i].Depth;
                AddRowToLayerListDataTable(number, name, geo, depth, description);
            }

            // 赋值取样列表
            for(int i = 0; i < _selectedZk.Samples.Count; i++)
            {
                string name = _selectedZk.Samples[i].Name;
                double depth = _selectedZk.Samples[i].Depth;
                bool isDisturbed = _selectedZk.Samples[i].IsDisturbed;
                AddRowToSampleListDataTable(name, depth, isDisturbed);
            }

            // 赋值标贯/动探列表
            for(int i = 0; i < _selectedZk.NTests.Count; i++)
            {
                string name = _selectedZk.NTests[i].Name;
                double depth = _selectedZk.NTests[i].Depth;
                double value = _selectedZk.NTests[i].Value;
                ZkNTest.ntype type = _selectedZk.NTests[i].Type;
                AddRowToNTestListDataTable(name, depth, value, type);
            }

            // 更新状态标签
            this.IsChangedTextBlock.Text = "";

            // 绘图
            DrawZk();
        }

        #endregion

        #region 钻孔分层

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
        /// <param name="_numberList">分层编号列表</param>
        /// <param name="_nameList">岩土名称列表</param>
        /// <param name="_geoList">地质年代成因列表</param>
        /// <param name="_depthList">层底深度列表</param>
        /// <param name="_descriptionList">分层描述列表</param>
        private void InitialLayerListDataTable(List<string> _numberList,List<string> _nameList,List<string> _geoList,List<double> _depthList,List<string> _descriptionList)
        {
            // 定义LayerListDataTable数据列
            dtLayer.Columns.Add(new DataColumn("numberList", typeof(string)));
            dtLayer.Columns.Add(new DataColumn("nameList", typeof(string)));
            dtLayer.Columns.Add(new DataColumn("geoList", typeof(string)));
            dtLayer.Columns.Add(new DataColumn("depthList", typeof(double)));
            dtLayer.Columns.Add(new DataColumn("descriptionList", typeof(string)));

            // 赋值
            DataRow dr;
            for(int i = 0; i < _numberList.Count; i++)
            {
                dr = dtLayer.NewRow();
                dr["numberList"] = _numberList[i];
                dr["nameList"] = _nameList[i];
                dr["geoList"] = _geoList[i];
                dr["depthList"] = _depthList[i];
                dr["descriptionList"] = _descriptionList[i];
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
        private void AddRowToLayerListDataTable(string _number,string _name,string _geo,double _depth,string _description)
        {
            DataRow dr = dtLayer.NewRow();
            dr = dtLayer.NewRow();
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
                DrawZk();
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
            if (selectIndex>0)
            {
                DataRowView drv0 = (DataRowView)this.LayerListDataGrid.Items[selectIndex-1];
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
                DrawZk();
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
            MessageBoxResult result = MessageBox.Show("确定要删除第 " + selectedNumber + " 层骂？", "删除钻孔分层", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (result == MessageBoxResult.OK)
            {
                DeleteRowLayerListDataTable(selectedIndex);

                // 绘图
                DrawZk();
            }
        }

        #endregion

        #region 钻孔取样

        /// <summary>
        /// 初始化SampleListDataTable，不带参数
        /// </summary>
        private void InitialSampleListDataTable()
        {
            // 定义LayerListDataTable数据列
            dtSample.Columns.Add(new DataColumn("numberList", typeof(string)));
            dtSample.Columns.Add(new DataColumn("depthList", typeof(double)));
            dtSample.Columns.Add(new DataColumn("isDisturbedList", typeof(bool)));
        }

        /// <summary>
        /// 初始化SampleListDataTable，带参数
        /// </summary>
        /// <param name="_numberList">取样编号列表</param>
        /// <param name="_depthList">取样深度列表</param>
        /// <param name="_isDisturbedList">取样类型列表</param>
        private void InitialSampleListDataTable(List<string> _numberList,List<double> _depthList,List<bool> _isDisturbedList)
        {
            // 定义LayerListDataTable数据列
            dtSample.Columns.Add(new DataColumn("numberList", typeof(string)));
            dtSample.Columns.Add(new DataColumn("depthList", typeof(double)));
            dtSample.Columns.Add(new DataColumn("isDisturbedList", typeof(bool)));

            // 赋值
            DataRow dr;
            for (int i = 0; i < _numberList.Count; i++)
            {
                dr = dtSample.NewRow();
                dr["numberList"] = _numberList[i];
                dr["depthList"] = _depthList[i];
                dr["isDisturbedList"] = _isDisturbedList[i];
                dtSample.Rows.Add(dr);
            }
        }

        /// <summary>
        /// 向SampleListDataTable添加行
        /// </summary>
        /// <param name="_number">取样编号</param>
        /// <param name="_depth">取样深度</param>
        /// <param name="_isDisturbed">取样类型</param>
        private void AddRowToSampleListDataTable(string _number,double _depth,bool _isDisturbed)
        {
            DataRow dr = dtSample.NewRow();
            dr["numberList"] = _number;
            dr["depthList"] = _depth;
            dr["isDisturbedList"] = _isDisturbed;
            dtSample.Rows.Add(dr);
        }

        /// <summary>
        /// 编辑SampleListDataTable行
        /// </summary>
        /// <param name="_rowIndex">行号</param>
        /// <param name="_number">取样编号</param>
        /// <param name="_depth">取样深度</param>
        /// <param name="_isDisturbed">取样类型</param>
        private void EditRowSampleListDataTable(int _rowIndex,string _number,double _depth,bool _isDisturbed)
        {
            DataRow dr = dtSample.Rows[_rowIndex];
            dr.BeginEdit();
            dr["numberList"] = _number;
            dr["depthList"] = _depth;
            dr["isDisturbedList"] = _isDisturbed;
            dr.EndEdit();
        }

        /// <summary>
        /// 删除SampleListDataTable行
        /// </summary>
        /// <param name="_rowIndex">行号</param>
        private void DeleteRowSampleListDataTable(int _rowIndex)
        {
            dtSample.Rows.RemoveAt(_rowIndex);

            // 刷新编号列表
            for(int i = 0; i < dtSample.Rows.Count; i++)
            {
                DataRow dr = dtSample.Rows[i];
                dr.BeginEdit();
                dr["numberList"] = (i + 1).ToString();
                dr.EndEdit();
            }
        }

        /// <summary>
        /// 点击"添加取样"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddSampleButton_Click(object sender, RoutedEventArgs e)
        {
            // 传递历史取样深度列表和历史取样编号列表
            List<string> lastNumberList = new List<string>();
            List<double> lastDepthList = new List<double>();
            if (this.SampleListDataGrid.Items.Count > 0)
            {
                for(int i = 0; i < this.SampleListDataGrid.Items.Count; i++)
                {
                    DataRowView drv = (DataRowView)this.SampleListDataGrid.Items[i];
                    lastNumberList.Add(drv[0].ToString());
                    lastDepthList.Add((double)drv[1]);
                }
            }

            // 传递取样编号
            string sampleNumber = (dtSample.Rows.Count + 1).ToString();

            // 实例化窗口
            ZkSampleDetail newSample = new ZkSampleDetail(lastDepthList, sampleNumber);
            newSample.ShowDialog();
            if (newSample.DialogResult == true)
            {
                string number = newSample.SampleNumberTextBox.Text;
                double depth = Convert.ToDouble(newSample.SampleDepthTextBox.Text);
                bool isDisturbed=false;
                if (newSample.UnDisturbed.IsChecked == true)
                {
                    isDisturbed = false;
                }
                else
                {
                    isDisturbed = true;
                }
                AddRowToSampleListDataTable(number, depth, isDisturbed);

                // 绘图
                DrawZk();
            }
        }

        /// <summary>
        /// 点击"编辑取样"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditSampleButton_Click(object sender, RoutedEventArgs e)
        {
            int selectIndex = this.SampleListDataGrid.SelectedIndex;

            // 传递历史取样深度列表和历史取样编号列表
            List<string> lastNumberList = new List<string>();
            List<double> lastDepthList = new List<double>();
            if (this.SampleListDataGrid.Items.Count > 0)
            {
                for (int i = 0; i < this.SampleListDataGrid.Items.Count; i++)
                {
                    if (i != selectIndex)
                    {
                        DataRowView drv0 = (DataRowView)this.SampleListDataGrid.Items[i];
                        lastNumberList.Add(drv0[0].ToString());
                        lastDepthList.Add((double)drv0[1]);
                    }
                }
            }

            // 赋值传递数据
            DataRowView drv = (DataRowView)this.SampleListDataGrid.SelectedItem;
            string number = (string)drv[0];
            double depth = (double)drv[1];
            bool isDisturbed = (bool)drv[2];
            
            // 实例化窗口
            ZkSampleDetail editSample = new ZkSampleDetail(lastDepthList, number, depth, isDisturbed);
            editSample.ShowDialog();
            if (editSample.DialogResult == true)
            {
                number = editSample.SampleNumberTextBox.Text;
                depth = Convert.ToDouble(editSample.SampleDepthTextBox.Text);
                isDisturbed = false;
                if (editSample.UnDisturbed.IsChecked == true)
                {
                    isDisturbed = false;
                }
                else
                {
                    isDisturbed = true;
                }
                EditRowSampleListDataTable(selectIndex, number, depth, isDisturbed);

                // 绘图
                DrawZk();
            }
        }

        /// <summary>
        /// 点击"删除取样"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteSampleButton_Click(object sender, RoutedEventArgs e)
        {
            DataRowView drv = (DataRowView)this.SampleListDataGrid.SelectedItem;
            string selectedNumber = (string)drv[0];
            int selectedIndex = SampleListDataGrid.SelectedIndex;
            MessageBoxResult result = MessageBox.Show("确定要删除编号为 " + selectedNumber + " 的取样吗？", "删除取样", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (result == MessageBoxResult.OK)
            {
                DeleteRowSampleListDataTable(selectedIndex);

                // 绘图
                DrawZk();
            }
        }

        #endregion

        #region 钻孔标贯/动探试验

        /// <summary>
        /// 初始化NTestListDataTable，不带参数
        /// </summary>
        private void InitialNTestListDataTable()
        {
            // 定义NTestListDataTable数据列
            dtNTest.Columns.Add(new DataColumn("numberList", typeof(string)));
            dtNTest.Columns.Add(new DataColumn("depthList", typeof(double)));
            dtNTest.Columns.Add(new DataColumn("valueList", typeof(double)));
            dtNTest.Columns.Add(new DataColumn("typeList", typeof(ZkNTest.ntype)));
        }

        /// <summary>
        /// 初始化NTestListDataTable，带参数
        /// </summary>
        /// <param name="_numberList">标贯/动探编号列表</param>
        /// <param name="_depthList">标贯/动探深度列表</param>
        /// <param name="_valueList">标贯/动探击数列表</param>
        /// <param name="_typeList">标贯/动探类型列表</param>
        private void InitialNTestListDataTable(List<string> _numberList,List<double> _depthList,List<double> _valueList,List<ZkNTest.ntype> _typeList)
        {
            // 定义NTestListDataTable数据列
            dtNTest.Columns.Add(new DataColumn("numberList", typeof(string)));
            dtNTest.Columns.Add(new DataColumn("depthList", typeof(double)));
            dtNTest.Columns.Add(new DataColumn("valueList", typeof(double)));
            dtNTest.Columns.Add(new DataColumn("typeList", typeof(ZkNTest.ntype)));

            // 赋值
            DataRow dr;
            for (int i = 0; i < _numberList.Count; i++)
            {
                dr = dtNTest.NewRow();
                dr["numberList"] = _numberList[i];
                dr["depthList"] = _depthList[i];
                dr["valueList"] = _valueList[i];
                dr["typeList"] = _typeList[i];
                dtNTest.Rows.Add(dr);
            }
        }

        /// <summary>
        /// 向NTestListDataTable添加行
        /// </summary>
        /// <param name="_number">标贯/动探编号</param>
        /// <param name="_depth">标贯/动探深度</param>
        /// <param name="_value">标贯/动探击数</param>
        /// <param name="_type">标贯/动探类型</param>
        private void AddRowToNTestListDataTable(string _number,double _depth,double _value,ZkNTest.ntype _type)
        {
            DataRow dr = dtNTest.NewRow();
            dr["numberList"] = _number;
            dr["depthList"] = _depth;
            dr["valueList"] = _value;
            dr["typeList"] = _type;
            dtNTest.Rows.Add(dr);
        }

        /// <summary>
        /// 编辑NTestListDataTable行
        /// </summary>
        /// <param name="_rowIndex"></param>
        /// <param name="_number">标贯/动探编号</param>
        /// <param name="_depth">标贯/动探深度</param>
        /// <param name="_value">标贯/动探击数</param>
        /// <param name="_type">标贯/动探类型</param>
        private void EditRowNTestListDataTable(int _rowIndex,string _number,double _depth,double _value,ZkNTest.ntype _type)
        {
            DataRow dr = dtNTest.Rows[_rowIndex];
            dr.BeginEdit();
            dr["numberList"] = _number;
            dr["depthList"] = _depth;
            dr["valueList"] = _value;
            dr["typeList"] = _type;
            dr.EndEdit();
        }

        /// <summary>
        /// 删除NTestListDataTable行
        /// </summary>
        /// <param name="_rowIndex">行号</param>
        private void DeleteRowNTestListDataTable(int _rowIndex)
        {
            dtNTest.Rows.RemoveAt(_rowIndex);

            // 刷新试验编号列表
            for(int i = 0; i < dtNTest.Rows.Count; i++)
            {
                DataRow dr = dtNTest.Rows[i];
                dr.BeginEdit();
                dr["numberList"] = (i + 1).ToString();
                dr.EndEdit();
            }
        }

        /// <summary>
        /// 点击"添加标贯/动探"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddNTestButton_Click(object sender, RoutedEventArgs e)
        {
            // 传递历史取样编号列表
            List<double> lastDepthList = new List<double>();
            if (this.NTestListDataGrid.Items.Count > 0)
            {
                for (int i = 0; i < this.NTestListDataGrid.Items.Count; i++)
                {
                    DataRowView drv = (DataRowView)this.NTestListDataGrid.Items[i];
                    lastDepthList.Add((double)drv[1]);
                }
            }

            // 传递试验编号
            string testNumber = (dtNTest.Rows.Count + 1).ToString();

            // 实例化窗口
            ZkNTestDetail newNTest = new ZkNTestDetail(lastDepthList, testNumber);
            newNTest.ShowDialog();
            if (newNTest.DialogResult == true)
            {
                string number = newNTest.NTestNumberTextBox.Text;
                double depth = Convert.ToDouble(newNTest.NTestDepthTextBox.Text);
                double value = Convert.ToDouble(newNTest.NTestValueTextBox.Text);
                ZkNTest.ntype type;
                if (newNTest.typeN.IsChecked == true)
                {
                    type = ZkNTest.ntype.N;
                }
                else if (newNTest.typeN10.IsChecked == true)
                {
                    type = ZkNTest.ntype.N10;
                }
                else if (newNTest.typeN635.IsChecked == true)
                {
                    type = ZkNTest.ntype.N635;
                }
                else
                {
                    type = ZkNTest.ntype.N120;
                }
                AddRowToNTestListDataTable(number, depth, value, type);

                // 绘图
                DrawZk();
            }
        }

        /// <summary>
        /// 点击"编辑标贯/动探"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditNTestButton_Click(object sender, RoutedEventArgs e)
        {
            int selectIndex = this.NTestListDataGrid.SelectedIndex;

            // 传递历史取样深度列表和历史取样编号列表
            List<string> lastNumberList = new List<string>();
            List<double> lastDepthList = new List<double>();
            if (this.NTestListDataGrid.Items.Count > 0)
            {
                for (int i = 0; i < this.NTestListDataGrid.Items.Count; i++)
                {
                    if (i != selectIndex)
                    {
                        DataRowView drv0 = (DataRowView)this.NTestListDataGrid.Items[i];
                        lastNumberList.Add(drv0[0].ToString());
                        lastDepthList.Add((double)drv0[1]);
                    }
                }
            }

            // 赋值传递数据
            DataRowView drv = (DataRowView)this.NTestListDataGrid.SelectedItem;
            string number = (string)drv[0];
            double depth = (double)drv[1];
            double value = (double)drv[2];
            ZkNTest.ntype type = (ZkNTest.ntype)drv[3];

            // 实例化窗口
            ZkNTestDetail editNTest = new ZkNTestDetail(lastDepthList, number, depth, value, type);
            editNTest.ShowDialog();
            if (editNTest.DialogResult == true)
            {
                number = editNTest.NTestNumberTextBox.Text;
                depth = Convert.ToDouble(editNTest.NTestDepthTextBox.Text);
                value = Convert.ToDouble(editNTest.NTestValueTextBox.Text);
                if (editNTest.typeN.IsChecked == true)
                {
                    type = ZkNTest.ntype.N;
                }
                else if (editNTest.typeN10.IsChecked == true)
                {
                    type = ZkNTest.ntype.N10;
                }
                else if (editNTest.typeN635.IsChecked == true)
                {
                    type = ZkNTest.ntype.N635;
                }
                else
                {
                    type = ZkNTest.ntype.N120;
                }
                EditRowNTestListDataTable(selectIndex, number, depth, value, type);
                
                // 绘图
                DrawZk();
            }
        }

        /// <summary>
        /// 点击"删除标贯/动探"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteNTestButton_Click(object sender, RoutedEventArgs e)
        {
            DataRowView drv = (DataRowView)this.NTestListDataGrid.SelectedItem;
            string selectedNumber = (string)drv[0];
            int selectedIndex = NTestListDataGrid.SelectedIndex;
            MessageBoxResult result = MessageBox.Show("确定要删除编号为 " + selectedNumber + " 的试验吗？", "删除标贯/动探试验", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (result == MessageBoxResult.OK)
            {
                DeleteRowNTestListDataTable(selectedIndex);
                
                // 绘图
                DrawZk();
            }
        }

        #endregion

        #region 保存钻孔

        /// <summary>
        /// Save命令的Executed事件处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CommandBindingSave_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // 提取钻孔基本信息参数
            string zkName = this.ZKNameTextBox.Text;
            double zkAltitude = Convert.ToDouble(this.ZKAltitudeTextBox.Text);
            double zkAxisX = !string.IsNullOrEmpty(this.AxisXTextBox.Text) && !string.IsNullOrWhiteSpace(this.AxisXTextBox.Text) ? Convert.ToDouble(this.AxisXTextBox.Text) : -0.19880205;
            double zkAxisY = !string.IsNullOrEmpty(this.AxisYTextBox.Text) && !string.IsNullOrWhiteSpace(this.AxisYTextBox.Text) ? Convert.ToDouble(this.AxisYTextBox.Text) : -0.19880205;
            double zkIniWL = !string.IsNullOrEmpty(this.InitialWaterLevelTextBox.Text) && !string.IsNullOrWhiteSpace(this.InitialWaterLevelTextBox.Text) ? Convert.ToDouble(this.InitialWaterLevelTextBox.Text) : -0.19880205;
            double zkStaWL = !string.IsNullOrEmpty(this.StableWaterLevelTextBox.Text) && !string.IsNullOrWhiteSpace(this.StableWaterLevelTextBox.Text) ? Convert.ToDouble(this.StableWaterLevelTextBox.Text) : -0.19880205;
            
            // 提取钻孔分层参数
            List<string> zkLayerNumberList = new List<string>();
            List<string> zkLayerNameList = new List<string>();
            List<string> zkLayerGeoList = new List<string>();
            List<double> zkLayerDepthList = new List<double>();
            List<string> zkLayerDescriptionList = new List<string>();
            for(int i = 0; i < dtLayer.Rows.Count; i++)
            {
                zkLayerNumberList.Add((string)dtLayer.Rows[i][0]);
                zkLayerNameList.Add((string)dtLayer.Rows[i][1]);
                zkLayerGeoList.Add((string)dtLayer.Rows[i][2]);
                zkLayerDepthList.Add((double)dtLayer.Rows[i][3]);
                zkLayerDescriptionList.Add((string)dtLayer.Rows[i][4]);
            }

            // 提取钻孔取样参数
            List<string> zkSampleNumberList = new List<string>();
            List<double> zkSampleDepthList = new List<double>();
            List<int> zkSampleIsDisturbedList = new List<int>();
            for(int i = 0; i < dtSample.Rows.Count; i++)
            {
                zkSampleNumberList.Add((string)dtSample.Rows[i][0]);
                zkSampleDepthList.Add((double)dtSample.Rows[i][1]);
                zkSampleIsDisturbedList.Add((bool)dtSample.Rows[i][2] == true ? 1 : 0);
            }

            // 提取钻孔标贯/动探参数
            List<string> zkNTestNumberList = new List<string>();
            List<double> zkNTestDepthList = new List<double>();
            List<double> zkNTestValueList = new List<double>();
            List<string> zkNTestTypeList = new List<string>();
            for(int i = 0; i < dtNTest.Rows.Count; i++)
            {
                zkNTestNumberList.Add((string)dtNTest.Rows[i][0]);
                zkNTestDepthList.Add((double)dtNTest.Rows[i][1]);
                zkNTestValueList.Add((double)dtNTest.Rows[i][2]);
                string s;
                if ((ZkNTest.ntype)dtNTest.Rows[i][3] == ZkNTest.ntype.N)
                    s = "N";
                else if ((ZkNTest.ntype)dtNTest.Rows[i][3] == ZkNTest.ntype.N10)
                    s = "N10";
                else if ((ZkNTest.ntype)dtNTest.Rows[i][3] == ZkNTest.ntype.N635)
                    s = "N635";
                else
                    s = "N120";
                zkNTestTypeList.Add(s);
            }

            // 保存钻孔到数据库
            if (isNewZk == false)
            {
                BoreholeDataBase.RemoveZk(Program.currentProject, oldZkName);
            }
            BoreholeDataBase.AddZkBasicInfo(Program.currentProject, zkName, zkAltitude, zkAxisX, zkAxisY, zkIniWL, zkStaWL);
            BoreholeDataBase.AddLayerListToZk(Program.currentProject, zkName, zkLayerNumberList, zkLayerNameList, zkLayerGeoList, zkLayerDescriptionList, zkLayerDepthList);
            BoreholeDataBase.AddSampleListToZk(Program.currentProject, zkName, zkSampleNumberList, zkSampleDepthList, zkSampleIsDisturbedList);
            BoreholeDataBase.AddNTestListToZk(Program.currentProject, zkName, zkNTestNumberList, zkNTestDepthList, zkNTestValueList, zkNTestTypeList);

            // 更新导航树
            MainWindow.bind.ReSetZkItem(Program.currentProject);

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
            if (setCanZkName == true && setCanZkAltitude == true && setCanZkAxisX == true && setCanZkAxisY == true && setCanZkIniWL == true && setCanZkStaWL == true)
            {
                e.CanExecute = true;
            }
        }

        #endregion

        #region 删除钻孔

        /// <summary>
        /// 点击"删除此钻孔"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteZkButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(oldZkName) && !string.IsNullOrWhiteSpace(oldZkName))
            {
                MessageBoxResult result = MessageBox.Show("您确定要删除 " + oldZkName + " 吗？\n该钻孔的所有资料将被删除，且不可恢复。", "删除钻孔", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    BoreholeDataBase.RemoveZk(Program.currentProject, oldZkName);
                    MainWindow.bind.ReSetZkItem(Program.currentProject);
                    this.Visibility = Visibility.Collapsed;
                }
            }
        }

        #endregion

        #region 绘制柱状图

        /// <summary>
        /// 绘制钻孔柱状图函数
        /// </summary>
        public void DrawZk()
        {
            // 清空旧柱状图
            ZkCanvas.Children.Clear();

            // 提取绘图区域宽度和高度
            double canvasWidth = this.ZkCanvas.Width;
            double canvasHeight = this.ZkCanvas.Height;

            // 绘制钻孔表头
            DrawZkLabel();

            // 绘制地面线和钻孔线
            DrawZkLine();

            // 绘制钻孔分层
            double maxDepth = dtLayer.Rows.Count == 0 ? 0 : (double)dtLayer.Rows[dtLayer.Rows.Count - 1][3];
            for(int i = 0; i < dtLayer.Rows.Count; i++)
            {
                double depth = (double)dtLayer.Rows[i][3];
                double oldDepth = i == 0 ? depth : (double)dtLayer.Rows[i - 1][3];
                string layerLabel = dtLayer.Rows[i][0].ToString() + " " + dtLayer.Rows[i][1].ToString();
                DrawZkLayer(depth, oldDepth, maxDepth, layerLabel);
            }

            // 绘制取样
            for(int i = 0; i < dtSample.Rows.Count; i++)
            {
                double depth = (double)dtSample.Rows[i][1];
                bool isDisturbed = (bool)dtSample.Rows[i][2];
                DrawZkSample(depth, maxDepth, isDisturbed);
            }

            // 绘制标贯
            for(int i = 0; i < dtNTest.Rows.Count; i++)
            {
                double depth = (double)dtNTest.Rows[i][1];
                double value = (double)dtNTest.Rows[i][2];
                ZkNTest.ntype type = (ZkNTest.ntype)dtNTest.Rows[i][3];
                DrawZkNTest(depth, maxDepth, value, type);
            }
        }

        /// <summary>
        /// 绘制钻孔表头函数
        /// </summary>
        private void DrawZkLabel()
        {
            // 提取绘图区域宽度和高度
            double w = this.ZkCanvas.Width;
            double h = this.ZkCanvas.Height;
            
            // 绘图
            this.ZkCanvas.DrawLine(w / 2 - 25, 33, w / 2 + 30, 33);
            this.ZkCanvas.DrawText(w / 2 - 15, 15, this.ZKNameTextBox.Text, 14, Brushes.Black, true, false, true);
            this.ZkCanvas.DrawText(w / 2 - 15, 32, this.ZKAltitudeTextBox.Text, 14, Brushes.Black, true, false, true);
        }

        /// <summary>
        /// 绘制地面线和钻孔线函数
        /// </summary>
        private void DrawZkLine()
        {
            // 提取绘图区域宽度和高度
            double w = this.ZkCanvas.Width;
            double h = this.ZkCanvas.Height;

            // 设置地面线Y坐标
            double y = 50;

            // 绘图
            this.ZkCanvas.DrawLine(0, y, w, y, 2, Brushes.Black);
            this.ZkCanvas.DrawLine(w / 2, y, w / 2, h, 8, Brushes.Black);
        }

        /// <summary>
        /// 绘制钻孔分层函数
        /// </summary>
        /// <param name="_depth">层底深度</param>
        /// <param name="_oldDepth">上一层层底深度</param>
        /// <param name="_maxDepth">最大钻探深度</param>
        /// <param name="_layerLabel">分层标签</param>
        private void DrawZkLayer(double _depth,double _oldDepth,double _maxDepth,string _layerLabel)
        {
            // 提取绘图区域宽度和高度
            double w = this.ZkCanvas.Width;
            double h = this.ZkCanvas.Height;

            // 设置地面Y坐标
            double dmy = 50;

            // 计算分层线Y坐标
            double y = dmy + (h - dmy) * (_depth / _maxDepth);
            double oldY = _oldDepth == _depth ? dmy : dmy + (h - dmy) * (_oldDepth / _maxDepth);

            // 计算分层标签
            string layerDepthLabel = _depth.ToString() + "(" + (Convert.ToDouble(this.ZKAltitudeTextBox.Text) - _depth).ToString() + ")";

            // 绘图
            this.ZkCanvas.DrawDotLine(50, y, w - 100, y);
            this.ZkCanvas.DrawText(w - 100, y - 10, layerDepthLabel, 14, Brushes.DarkBlue, true, false, true);
            this.ZkCanvas.DrawText(50, (y + oldY) / 2 - 10, _layerLabel, 14, Brushes.Red, true, false, true);
        }

        /// <summary>
        /// 绘制取样函数
        /// </summary>
        /// <param name="_depth">取样深度</param>
        /// <param name="_maxDepth">最大钻探深度</param>
        /// <param name="_isDisturbed">是否为扰动样，true为扰动</param>
        private void DrawZkSample(double _depth,double _maxDepth,bool _isDisturbed)
        {
            // 提取绘图区域宽度和高度
            double w = this.ZkCanvas.Width;
            double h = this.ZkCanvas.Height;

            // 设置地面Y坐标
            double dmy = 50;

            // 计算取样Y坐标
            double y = dmy + (h - dmy) * (_depth / _maxDepth);

            // 绘图
            this.ZkCanvas.DrawCircle(w / 2 + 20, y, 5, !_isDisturbed);
        }

        /// <summary>
        /// 绘制标贯/动探函数
        /// </summary>
        /// <param name="_depth">标贯/动探深度</param>
        /// <param name="_maxDepth">最大钻探深度</param>
        /// <param name="_value">标贯/动探击数</param>
        /// <param name="_type">标贯/动探类型</param>
        private void DrawZkNTest(double _depth,double _maxDepth,double _value,ZkNTest.ntype _type)
        {
            // 提取绘图区域宽度和高度
            double w = this.ZkCanvas.Width;
            double h = this.ZkCanvas.Height;

            // 设置地面Y坐标
            double dmy = 50;

            // 计算试验Y坐标
            double y = dmy + (h - dmy) * (_depth / _maxDepth);

            // 计算试验标签
            string nTestLabel = _type.ToString() + "=" + _value.ToString();

            // 绘图
            this.ZkCanvas.DrawText(w / 2 + 30, y - 8, nTestLabel, 14, Brushes.Brown, true, true, true);
        }

        #endregion
    }
}
