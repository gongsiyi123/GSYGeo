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
    /// ZkSampleDetail.xaml 的交互逻辑
    /// </summary>
    public partial class ZkSampleDetail : Window
    {
        // 定义工具提示
        System.Windows.Controls.ToolTip tt = new System.Windows.Controls.ToolTip();
        System.Windows.Controls.ToolTip tt0 = new System.Windows.Controls.ToolTip();
        System.Windows.Controls.ToolTip tt1 = new System.Windows.Controls.ToolTip();
        System.Windows.Controls.ToolTip tt2 = new System.Windows.Controls.ToolTip();
        System.Windows.Controls.ToolTip tt3 = new System.Windows.Controls.ToolTip();
        System.Windows.Controls.ToolTip tt4 = new System.Windows.Controls.ToolTip();

        // 判断确定按钮的可用性参数
        bool setCanDepth = false;
        bool setCanNumber = false;

        // 定义历史取样编号、深度列表
        List<double> LastDepthList;
        List<string> LastNumberList;

        // 带2个参数的构造函数，用于新增取样
        public ZkSampleDetail(List<string> _lastNumberList,List<double> _lastDepthList)
        {
            InitializeComponent();

            // 赋值历史取样编号、深度
            if (_lastDepthList != null)
            {
                LastDepthList = _lastDepthList;
            }
            if (_lastNumberList != null)
            {
                LastNumberList = _lastNumberList;   
            }

            // 定义取样深度、编号输入框的工具提示
            DefineToolTip();

            // 默认选中"原状样"
            this.UnDisturbed.IsChecked = true;
        }

        // 带5个参数的构造函数，用于编辑取样
        public ZkSampleDetail(List<string> _lastNumberList,List<double> _lastDepthList,string _number,double _depth,bool _isDisturbed)
        {
            InitializeComponent();

            // 赋值新窗体标题
            this.Title = "编辑钻孔取样";

            // 赋值历史取样编号、深度
            if (_lastDepthList != null)
            {
                LastDepthList = _lastDepthList;
            }
            if (_lastNumberList != null)
            {
                LastNumberList = _lastNumberList;
            }

            // 定义取样深度、编号输入框的工具提示
            DefineToolTip();

            // 赋值
            this.SampleNumberTextBox.Text = _number;
            this.SampleDepthTextBox.Text = _depth.ToString();
            if (_isDisturbed == false)
            {
                this.UnDisturbed.IsChecked = true;
            }
            else
            {
                this.Disturbed.IsChecked = true;
            }
        }

        // 定义取样深度、编号输入框的工具提示的函数
        private void DefineToolTip()
        {
            tt.Content = "输入的深度不是有效数字";
            this.SampleDepthTextBox.ToolTip = tt;
            tt.PlacementTarget = this.SampleDepthTextBox;
            tt.Foreground = Brushes.Red;
            tt.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;

            tt0.Content = "取样深度必须大于0";
            this.SampleDepthTextBox.ToolTip = tt0;
            tt0.PlacementTarget = this.SampleDepthTextBox;
            tt0.Foreground = Brushes.Red;
            tt0.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;

            tt1.Content = "取样深度不能为空";
            this.SampleDepthTextBox.ToolTip = tt1;
            tt1.PlacementTarget = this.SampleDepthTextBox;
            tt1.Foreground = Brushes.Red;
            tt1.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;

            tt2.Content = "输入的深度处已有其他取样";
            this.SampleDepthTextBox.ToolTip = tt2;
            tt2.PlacementTarget = this.SampleDepthTextBox;
            tt2.Foreground = Brushes.Red;
            tt2.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;

            tt3.Content = "取样编号不能为空";
            this.SampleNumberTextBox.ToolTip = tt3;
            tt3.PlacementTarget = this.SampleNumberTextBox;
            tt3.Foreground = Brushes.Red;
            tt3.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;

            tt4.Content = "取样编号重复";
            this.SampleNumberTextBox.ToolTip = tt4;
            tt4.PlacementTarget = this.SampleNumberTextBox;
            tt4.Foreground = Brushes.Red;
            tt4.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
        }

        // 当 取样编号 输入框内容变化时，验证是否为空值，并验证是否与历史取样编号重复
        private void SampleNumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            tt3.IsOpen = false;
            tt4.IsOpen = false;
            string str = this.SampleNumberTextBox.Text;
            if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
            {
                this.SampleNumberTextBox.BorderBrush = Brushes.Red;
                tt3.IsOpen = true;
                setCanNumber = false;
            }
            else if (LastNumberList.Contains(str))
            {
                this.SampleNumberTextBox.BorderBrush = Brushes.Red;
                tt4.IsOpen = true;
                setCanNumber = false;
            }
            else
            {
                this.SampleNumberTextBox.BorderBrush = Brushes.Gray;
                setCanNumber = true;
            }
        }

        // 当 取样深度 输入框内容变化时，验证是否为数字，并验证是否与历史取样深度冲突
        private void SampleDepthTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            tt.IsOpen = false;
            tt0.IsOpen = false;
            tt1.IsOpen = false;
            tt2.IsOpen = false;
            string str = this.SampleDepthTextBox.Text;
            double num;
            if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
            {
                this.SampleDepthTextBox.BorderBrush = Brushes.Red;
                tt1.IsOpen = true;
                setCanDepth = false;
            }
            else if (!double.TryParse(str, out num))
            {
                this.SampleDepthTextBox.BorderBrush = Brushes.Red;
                tt.IsOpen = true;
                setCanDepth = false;
            }
            else if(double.TryParse(str,out num) && Convert.ToDouble(str) <= 0)
            {
                this.SampleDepthTextBox.BorderBrush = Brushes.Red;
                tt0.IsOpen = true;
                setCanDepth = false;
            }
            else if (double.TryParse(str, out num) && LastDepthList.Contains(Convert.ToDouble(str)))
            {
                this.SampleDepthTextBox.BorderBrush = Brushes.Red;
                tt2.IsOpen = true;
                setCanDepth = false;
            }
            else
            {
                this.SampleDepthTextBox.BorderBrush = Brushes.Gray;
                setCanDepth = true;
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
            if (setCanDepth && setCanNumber)
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
