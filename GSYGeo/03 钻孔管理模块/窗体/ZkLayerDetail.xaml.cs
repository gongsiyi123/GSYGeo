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
    /// ZkLayerDetail.xaml 的交互逻辑
    /// </summary>
    public partial class ZkLayerDetail : Window
    {
        // 定义工具提示
        System.Windows.Controls.ToolTip tt = new System.Windows.Controls.ToolTip();
        System.Windows.Controls.ToolTip tt2 = new System.Windows.Controls.ToolTip();

        // 判断确定按钮的可用性参数
        bool canCommit = false;

        // 上一层层底深度的数值
        double LastDepth = -1;

        // 带1个参数的构造函数，用于新建分层
        public ZkLayerDetail(double _lastDepth)
        {
            InitializeComponent();

            // 赋值上一层层底深度
            LastDepth = _lastDepth;

            // 赋值各选框的候选项
            this.LayerNumberComboBox.ItemsSource = ProjectDataBase.ReadLayerNumberList(Program.currentProject);
            this.LayerNameComboBox.ItemsSource = ProjectDataBase.ReadLayerNameList(Program.currentProject);
            this.LayerGeoComboBox.ItemsSource = ProjectDataBase.ReadLayerGeoList(Program.currentProject);

            // 定义层底深度输入框的工具提示
            DefineToolTip();
        }

        // 带4个参数的构造函数，用于编辑分层
        public ZkLayerDetail(double _lastDepth,string _number,double _depth,string _description)
        {
            InitializeComponent();

            // 重新赋值窗体标题
            this.Title = "编辑钻孔分层";

            // 赋值上一层层底深度
            LastDepth = _lastDepth;

            // 赋值各选框的候选项
            this.LayerNumberComboBox.ItemsSource = ProjectDataBase.ReadLayerNumberList(Program.currentProject);
            this.LayerNameComboBox.ItemsSource = ProjectDataBase.ReadLayerNameList(Program.currentProject);
            this.LayerGeoComboBox.ItemsSource = ProjectDataBase.ReadLayerGeoList(Program.currentProject);

            // 定义层底深度输入框的工具提示
            DefineToolTip();

            // 赋值
            this.LayerNumberComboBox.SelectedValue = _number;
            this.LayerDepthTextBox.Text = _depth.ToString();
            this.LayerDescriptionTextBox.Text = _description;
        }

        // 定义层底深度输入框的工具提示的函数
        private void DefineToolTip()
        {
            tt.Content = "输入的深度不是有效数字";
            this.LayerDepthTextBox.ToolTip = tt;
            tt.PlacementTarget = this.LayerDepthTextBox;
            tt.Foreground = Brushes.Red;
            tt.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;

            tt2.Content = "输入的深度小于上一分层的深度，上一层为 " + LastDepth.ToString() + " 米";
            this.LayerDepthTextBox.ToolTip = tt2;
            tt2.PlacementTarget = this.LayerDepthTextBox;
            tt2.Foreground = Brushes.Red;
            tt2.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
        }

        // 当 层底深度 输入框内容变化时，验证是否为数字，并验证是否比上一层低
        private void LayerDepthTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string str = this.LayerDepthTextBox.Text;
            double num;
            if(string.IsNullOrEmpty(this.LayerDepthTextBox.Text) || string.IsNullOrWhiteSpace(this.LayerDepthTextBox.Text))
            {
                tt.IsOpen = false;
                tt2.IsOpen = false;
                canCommit = false;
            }
            else if (!double.TryParse(str, out num))
            {
                this.LayerDepthTextBox.BorderBrush = Brushes.Red;
                tt.IsOpen = true;
                canCommit = false;
            }
            else if(double.TryParse(str, out num) && Convert.ToDouble(str) <= LastDepth)
            {
                this.LayerDepthTextBox.BorderBrush = Brushes.Red;
                tt2.IsOpen = true;
                canCommit = false;
            }
            else
            {
                this.LayerDepthTextBox.BorderBrush = Brushes.Gray;
                tt.IsOpen = false;
                tt2.IsOpen = false;
                canCommit = true;
            }
        }

        // 选择分层编号选框时
        private void LayerNumberComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.LayerNameComboBox.SelectedIndex = this.LayerNumberComboBox.SelectedIndex;
            this.LayerGeoComboBox.SelectedIndex = this.LayerNumberComboBox.SelectedIndex;
            this.LayerDescriptionTextBox.Text = ProjectDataBase.ReadLayerDescriptionList(Program.currentProject)[this.LayerNumberComboBox.SelectedIndex];
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
            if (canCommit && !string.IsNullOrEmpty(this.LayerNumberComboBox.Text) && !string.IsNullOrWhiteSpace(this.LayerNumberComboBox.Text))
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
