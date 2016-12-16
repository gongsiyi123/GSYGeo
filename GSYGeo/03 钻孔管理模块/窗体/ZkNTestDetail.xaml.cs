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
    /// NTestDetail.xaml 的交互逻辑
    /// </summary>
    public partial class ZkNTestDetail : Window
    {
        // 定义工具提示
        System.Windows.Controls.ToolTip tt1 = new System.Windows.Controls.ToolTip();
        System.Windows.Controls.ToolTip tt2 = new System.Windows.Controls.ToolTip();
        System.Windows.Controls.ToolTip tt3 = new System.Windows.Controls.ToolTip();
        System.Windows.Controls.ToolTip tt4 = new System.Windows.Controls.ToolTip();
        System.Windows.Controls.ToolTip tt7 = new System.Windows.Controls.ToolTip();
        System.Windows.Controls.ToolTip tt8 = new System.Windows.Controls.ToolTip();
        System.Windows.Controls.ToolTip tt9 = new System.Windows.Controls.ToolTip();

        // 判断确定按钮的可用性参数
        bool setCanDepth = false;
        bool setCanValue = false;

        // 定义深度列表
        List<double> LastDepthList;

        // 带2个参数的构造函数，用于新增试验
        public ZkNTestDetail(List<double> _lastDepthList, string _testNumber)
        {
            InitializeComponent();

            // 赋值深度列表
            if (_lastDepthList != null)
            {
                LastDepthList = _lastDepthList;
            }

            // 赋值编号输入框
            this.NTestNumberTextBox.Text = _testNumber;

            // 定义试验编号、深度输入框的工具提示
            DefineToolTip();

            // 默认选中"N"
            this.typeN.IsChecked = true;

            // 默认聚焦深度输入框
            this.NTestDepthTextBox.Focus();
        }

        // 带5个参数的构造函数，用于编辑试验
        public ZkNTestDetail(List<double> _lastDepthList, string _testNumber, double _depth,double _value,ZkNTest.ntype _type)
        {
            InitializeComponent();

            // 赋值新窗体标题
            this.Title = "编辑标贯/动探试验";

            // 赋值深度列表
            if (_lastDepthList != null)
            {
                LastDepthList = _lastDepthList;
            }

            // 定义试验编号、深度输入框的工具提示
            DefineToolTip();

            // 赋值
            this.NTestNumberTextBox.Text = _testNumber;
            this.NTestDepthTextBox.Text = _depth.ToString();
            this.NTestValueTextBox.Text = _value.ToString();
            if (_type == ZkNTest.ntype.N)
            {
                this.typeN.IsChecked = true;
            }
            else if (_type == ZkNTest.ntype.N10)
            {
                this.typeN10.IsChecked = true;
            }
            else if (_type == ZkNTest.ntype.N635)
            {
                this.typeN635.IsChecked = true;
            }
            else if (_type == ZkNTest.ntype.N120)
            {
                this.typeN120.IsChecked = true;
            }

            // 默认聚焦深度输入框
            this.NTestDepthTextBox.Focus();
        }

        // 定义试验编号、深度的工具提示的函数
        private void DefineToolTip()
        {
            tt1.Content = "输入的深度不是有效数字";
            this.NTestDepthTextBox.ToolTip = tt1;
            tt1.PlacementTarget = this.NTestDepthTextBox;
            tt1.Foreground = Brushes.Red;
            tt1.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;

            tt2.Content = "试验深度必须大于0";
            this.NTestDepthTextBox.ToolTip = tt2;
            tt2.PlacementTarget = this.NTestDepthTextBox;
            tt2.Foreground = Brushes.Red;
            tt2.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;

            tt3.Content = "取样深度不能为空";
            this.NTestDepthTextBox.ToolTip = tt3;
            tt3.PlacementTarget = this.NTestDepthTextBox;
            tt3.Foreground = Brushes.Red;
            tt3.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;

            tt4.Content = "输入的深度处已有其他试验";
            this.NTestDepthTextBox.ToolTip = tt4;
            tt4.PlacementTarget = this.NTestDepthTextBox;
            tt4.Foreground = Brushes.Red;
            tt4.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            
            tt7.Content = "试验击数不能为空";
            this.NTestNumberTextBox.ToolTip = tt7;
            tt7.PlacementTarget = this.NTestValueTextBox;
            tt7.Foreground = Brushes.Red;
            tt7.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;

            tt8.Content = "试验击数不是有效数字";
            this.NTestNumberTextBox.ToolTip = tt8;
            tt8.PlacementTarget = this.NTestValueTextBox;
            tt8.Foreground = Brushes.Red;
            tt8.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;

            tt9.Content = "试验击数必须大于0";
            this.NTestNumberTextBox.ToolTip = tt9;
            tt9.PlacementTarget = this.NTestValueTextBox;
            tt9.Foreground = Brushes.Red;
            tt9.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
        }
        
        // 当 试验深度 输入框内容变化时，验证是否为数字，并验证是否与历史试验深度冲突
        private void NTestDepthTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            tt1.IsOpen = false;
            tt2.IsOpen = false;
            tt3.IsOpen = false;
            tt4.IsOpen = false;
            string str = this.NTestDepthTextBox.Text;
            double num;
            if(string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
            {
                this.NTestDepthTextBox.BorderBrush = Brushes.Red;
                tt3.IsOpen = true;
                setCanDepth = false;
            }
            else if(!double.TryParse(str,out num))
            {
                this.NTestDepthTextBox.BorderBrush = Brushes.Red;
                tt1.IsOpen = true;
                setCanDepth = false;
            }
            else if(double.TryParse(str,out num) && Convert.ToDouble(str) <= 0)
            {
                this.NTestDepthTextBox.BorderBrush = Brushes.Red;
                tt2.IsOpen = true;
                setCanDepth = false;
            }
            else if(double.TryParse(str,out num) && LastDepthList.Contains(Convert.ToDouble(str)))
            {
                this.NTestDepthTextBox.BorderBrush = Brushes.Red;
                tt4.IsOpen = true;
                setCanDepth = false;
            }
            else
            {
                this.NTestDepthTextBox.BorderBrush = Brushes.Gray;
                setCanDepth = true;
            }
        }

        // 当 试验击数 输入框内容变化时，验证是否为数字，并验证合法性
        private void NTestValueTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            tt7.IsOpen = false;
            tt8.IsOpen = false;
            tt9.IsOpen = false;
            string str = this.NTestValueTextBox.Text;
            double num;
            if(string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
            {
                this.NTestValueTextBox.BorderBrush = Brushes.Red;
                tt7.IsOpen = true;
                setCanValue = false;
            }
            else if(!double.TryParse(str, out num))
            {
                this.NTestValueTextBox.BorderBrush = Brushes.Red;
                tt8.IsOpen = true;
                setCanValue = false;
            }
            else if(double.TryParse(str, out num) && Convert.ToDouble(str) <= 0)
            {
                this.NTestValueTextBox.BorderBrush = Brushes.Red;
                tt9.IsOpen = true;
                setCanValue = false;
            }
            else
            {
                this.NTestValueTextBox.BorderBrush = Brushes.Gray;
                setCanValue = true;
            }
        }

        // Save命令的Executed事件处理函数
        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        // Save命令的CanExecuted事件处理函数
        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            // 绑定"确定"按钮的可用状态
            if (setCanDepth && setCanValue)
            {
                e.CanExecute = true;
            }
        }

        // 点击"取消"
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

    }
}
