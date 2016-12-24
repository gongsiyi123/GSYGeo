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
        /// <summary>
        /// 无参数的构造函数
        /// </summary>
        public LayerDetail()
        {
            InitializeComponent();

            InitialComboBox();
        }

        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        /// <param name="_number">分层编号</param>
        /// <param name="_name">岩土名称</param>
        /// <param name="_geo">地质年代成因</param>
        /// <param name="_description">分层描述</param>
        public LayerDetail(string _number,string _name,string _geo,string _description)
        {
            InitializeComponent();

            // 窗口赋值
            this.LayerNumberTextBox.Text = _number;
            this.LayerNameTextBox.Text = _name;
            InitialComboBox();
            this.LayerGeoComboBox.Text = _geo;
            this.LayerDescriptionTextBox.Text = _description;
        }

        /// <summary>
        /// 初始化地质年代和成因选框
        /// </summary>
        private void InitialComboBox()
        {
            this.LayerGeoComboBox.ItemsSource = Geology.Genesis.Values;
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

        /// <summary>
        /// Save命令的Executed事件处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CommandBindingSave_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        /// <summary>
        /// Save命令的CanExecuted事件处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
