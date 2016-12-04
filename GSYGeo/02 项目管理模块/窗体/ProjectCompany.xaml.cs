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
    /// ProjectCompany.xaml 的交互逻辑
    /// </summary>
    public partial class ProjectCompany : Window
    {
        // 无参数的构造函数
        public ProjectCompany()
        {
            InitializeComponent();

            // 填充公司名称和资质代码
            this.CompanyNameTextBox.Text = SettingDataBase.ReadCompanyName();
            this.CompanyCodeTextBox.Text = SettingDataBase.ReadCompanyCode();

            // 设置ComboBox数据
            InitialComboBox();
            
        }

        // 带参数的构造函数
        public ProjectCompany(string _companyName,string _companyCode,string _drawer,string _writer,string _checker,string _inspector,string _approver,string _finalApprover)
        {
            InitializeComponent();

            // 填充公司名称和资质代码
            if (string.IsNullOrEmpty(_companyName))
            {
                //this.CompanyNameTextBox.Text = SettingDataBase.ReadCompanyName();
            }
            else
            {
                this.CompanyNameTextBox.Text = _companyName;
            }
            if (string.IsNullOrEmpty(_companyCode))
            {
                //this.CompanyCodeTextBox.Text = SettingDataBase.ReadCompanyCode();
            }
            else
            {
                this.CompanyCodeTextBox.Text = _companyCode;
            }
            

            // 设置ComboBox数据
            InitialComboBox();
            this.DrawerComboBox.SelectedItem = _drawer;
            this.WriterComboBox.SelectedItem = _writer;
            this.CheckerComboBox.SelectedItem = _checker;
            this.InspectorComboBox.SelectedItem = _inspector;
            this.ApproverComboBox.SelectedItem = _approver;
            this.FinalApproverComboBox.SelectedItem = _finalApprover;
        }

        // 设置ComboBox数据
        private void InitialComboBox()
        {
            if (SettingDataBase.ReadCompanyPeople() != null)
            {
                this.DrawerComboBox.ItemsSource = SettingDataBase.ReadCompanyPeople();
                this.WriterComboBox.ItemsSource = SettingDataBase.ReadCompanyPeople();
                this.CheckerComboBox.ItemsSource = SettingDataBase.ReadCompanyPeople();
                this.InspectorComboBox.ItemsSource = SettingDataBase.ReadCompanyPeople();
                this.ApproverComboBox.ItemsSource = SettingDataBase.ReadCompanyPeople();
                this.FinalApproverComboBox.ItemsSource = SettingDataBase.ReadCompanyPeople();
            }
        }

        // Save命令的Excuted事件处理函数
        private void CommandBindingSave_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            
            this.DialogResult = true;
            this.Close();
        }

        // Save命令的CanExcuted事件处理函数
        private void CommandBindingSave_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            // 设置"确定"按钮的可用状态
            if(!string.IsNullOrEmpty(this.CompanyNameTextBox.Text) && !string.IsNullOrWhiteSpace(this.CompanyNameTextBox.Text) && !string.IsNullOrEmpty(this.CompanyCodeTextBox.Text) && !string.IsNullOrWhiteSpace(this.CompanyCodeTextBox.Text))
            {
                e.CanExecute = true;
            }
        }

        // 点击"取消"按钮
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
