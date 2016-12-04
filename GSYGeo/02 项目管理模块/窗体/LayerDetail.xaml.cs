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
    /// LayerDetail.xaml 的交互逻辑
    /// </summary>
    public partial class LayerDetail : Window
    {
        // 无参数的构造函数
        public LayerDetail()
        {
            InitializeComponent();

            InitialComboBox();
        }

        // 带参数的构造函数
        public LayerDetail(string _number,string _name,string _geo,string _description)
        {
            InitializeComponent();

            // 窗口赋值
            this.LayerNumberTextBox.Text = _number;
            this.LayerNameTextBox.Text = _name;
            //this.LayerGeoTextBox.Text = _geo;
            InitialComboBox();
            this.LayerGeoComboBox.Text = _geo;
            this.LayerDescriptionTextBox.Text = _description;
        }

        private void InitialComboBox()
        {
            this.LayerGeoComboBox.ItemsSource = Geology.Genesis.Values;
        }

        // 点击"取消"
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        // Save命令的Executed事件处理函数
        private void CommandBindingSave_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        // Save命令的CanExecuted事件处理函数
        private void CommandBindingSave_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            // 绑定"确定"按钮的可用状态
            if(!string.IsNullOrEmpty(this.LayerNumberTextBox.Text) && !string.IsNullOrWhiteSpace(this.LayerNumberTextBox.Text) && !string.IsNullOrEmpty(this.LayerNameTextBox.Text) && !string.IsNullOrWhiteSpace(this.LayerNameTextBox.Text) && !string.IsNullOrEmpty(this.LayerGeoComboBox.Text) && !string.IsNullOrWhiteSpace(this.LayerGeoComboBox.Text))
            {
                e.CanExecute = true;
            }
        }
    }
}
