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
        /// <summary>
        /// 定义工具提示
        /// </summary>
        System.Windows.Controls.ToolTip tt = new System.Windows.Controls.ToolTip();
        System.Windows.Controls.ToolTip tt0 = new System.Windows.Controls.ToolTip();
        System.Windows.Controls.ToolTip tt1 = new System.Windows.Controls.ToolTip();
        System.Windows.Controls.ToolTip tt2 = new System.Windows.Controls.ToolTip();

        /// <summary>
        /// 判断确定按钮的可用性参数
        /// </summary>
        bool setCanDepth = false;

        /// <summary>
        /// 定义历史取样编号、深度列表
        /// </summary>
        List<double> LastDepthList;

        /// <summary>
        /// 带2个参数的构造函数，用于新增取样
        /// </summary>
        /// <param name="_lastDepthList">历史取样编号列表</param>
        /// <param name="_sampleNumber">取样深度</param>
        public ZkSampleDetail(List<double> _lastDepthList,string _sampleNumber)
        {
            InitializeComponent();

            // 赋值历史取样编号、深度
            if (_lastDepthList != null)
            {
                LastDepthList = _lastDepthList;
            }

            // 赋值编号输入框
            this.SampleNumberTextBox.Text = _sampleNumber;

            // 定义取样深度、编号输入框的工具提示
            DefineToolTip();

            // 默认选中"原状样"
            this.UnDisturbed.IsChecked = true;

            // 默认聚焦深度输入框
            this.SampleDepthTextBox.Focus();
        }

        /// <summary>
        /// 带5个参数的构造函数，用于编辑取样
        /// </summary>
        /// <param name="_lastDepthList">历史取样编号列表</param>
        /// <param name="_testNumber">取样编号</param>
        /// <param name="_depth">取样深度</param>
        /// <param name="_isDisturbed">取样类型，true为扰动样</param>
        public ZkSampleDetail(List<double> _lastDepthList,string _testNumber,double _depth,bool _isDisturbed)
        {
            InitializeComponent();

            // 赋值新窗体标题
            this.Title = "编辑钻孔取样";

            // 赋值历史取样编号、深度
            if (_lastDepthList != null)
            {
                LastDepthList = _lastDepthList;
            }
            
            // 定义取样深度、编号输入框的工具提示
            DefineToolTip();

            // 赋值
            this.SampleNumberTextBox.Text = _testNumber;
            this.SampleDepthTextBox.Text = _depth.ToString();
            if (_isDisturbed == false)
            {
                this.UnDisturbed.IsChecked = true;
            }
            else
            {
                this.Disturbed.IsChecked = true;
            }

            // 默认聚焦深度输入框
            this.SampleDepthTextBox.Focus();
        }

        /// <summary>
        /// 定义取样深度、编号输入框的工具提示的函数
        /// </summary>
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
        }

        /// <summary>
        /// 当 取样深度 输入框内容变化时，验证是否为数字，并验证是否与历史取样深度冲突
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Save命令的Executed事件处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        /// <summary>
        /// Save命令的CanExecuted事件处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            // 绑定"确定"按钮的可用状态
            if (setCanDepth)
            {
                e.CanExecute = true;
            }
        }

        /// <summary>
        /// 点击"取消"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

    }
}
