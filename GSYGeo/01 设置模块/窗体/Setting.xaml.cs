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
using System.Windows.Forms;
using System.Data.SQLite;
using System.Data;
using System.Collections.ObjectModel;
using System.IO;

namespace GSYGeo
{
    /// <summary>
    /// Setting2.xaml 的交互逻辑
    /// </summary>
    public partial class Setting : Window
    {
        // 实例化ProgramPath类并赋初始值为当前数据存储路径，用于对"配置"-"数据存储位置"文本框进行数据绑定
        ProgramPath programPath = new ProgramPath(Program.ReadProgramPath());

        // 实例化Company类，用于对 "公司" 文本框进行数据绑定
        Company company = new Company();

        // 定义设置变化判断变量
        private bool isDirty = false;

        // 定义标准表里
        private string industrialStandard = null;
        private string localStandard = null;

        // 默认构造函数
        public Setting()
        {
            InitializeComponent();

            // 设置窗口数据绑定
            SetBinding();

            // 设置初始时"应用"按钮不可用
            isDirty = false;
        }

        // 设置数据绑定函数
        private void SetBinding()
        {
            // "配置"-"数据存储位置" 绑定到 programPath实例
            System.Windows.Data.Binding bindProgramPath = new System.Windows.Data.Binding("Path");
            bindProgramPath.Source = programPath;
            bindProgramPath.Mode = BindingMode.OneWay;
            textBoxDataBasePath.SetBinding(System.Windows.Controls.TextBox.TextProperty, bindProgramPath);

            // "公司" 绑定到 SettingDatabase
            // 创建连接到设置信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\设置信息.gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 绑定单位名称
                System.Windows.Data.Binding bindCompanyName = new System.Windows.Data.Binding("Name");
                bindCompanyName.Source = company;
                bindCompanyName.Mode = BindingMode.OneWay;
                CompanyNameTextBox.SetBinding(System.Windows.Controls.TextBox.TextProperty, bindCompanyName);

                // 绑定单位资质代码
                System.Windows.Data.Binding bindCompanyCode = new System.Windows.Data.Binding("Code");
                bindCompanyCode.Source = company;
                bindCompanyCode.Mode = BindingMode.OneWay;
                CompanyCodeTextBox.SetBinding(System.Windows.Controls.TextBox.TextProperty, bindCompanyCode);

                // 绑定人员列表
                this.CompanyPeopleListView.ItemsSource = company.People;
            }
        }
        
        // 点击"配置"-"浏览"
        private void SetDataBasePath_Click(object sender, RoutedEventArgs e)
        {
            // 保存旧路径
            string oldPath = Program.ReadProgramPath();

            // 弹出路径选择对话框，更改数据存储位置
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.Description = "选择数据存储文件夹";
            if (folderDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (folderDlg.SelectedPath + "\\小熠岩土勘察" != Program.ReadProgramPath())
                {
                    MessageBoxResult result = System.Windows.MessageBox.Show("  您确定要更改数据存储位置吗？\n  所有的项目文件和设置文件将迁移至新位置，点击确定按钮执行操作。", "更改数据存储位置", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        
                        try
                        {
                            // 保存新路径
                            string newPath = folderDlg.SelectedPath + "\\小熠岩土勘察";

                            // 检查是否已有文件夹
                            if (Directory.Exists(newPath))
                            {
                                // 失败通知
                                System.Windows.MessageBox.Show("  无法迁移数据存储文件夹，您选择的路径已存在同名文件夹，请核对数据或重新选择。", "数据存储文件夹迁移失败", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }

                            // 迁移文件
                            Directory.Move(oldPath, newPath);

                            // 更新programPath实例的Path属性，以此更新数据存储路径注册表项
                            programPath.Path = newPath;

                            // 成功通知
                            System.Windows.MessageBox.Show("  数据存储文件夹已成功迁移至下列位置：\t" + Program.ReadProgramPath(), "数据存储文件夹迁移成功", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        catch
                        {
                            // 失败通知
                            System.Windows.MessageBox.Show("  无法迁移数据存储文件夹，请检查您选择的路径是否具有修改权限。", "数据存储文件夹迁移失败", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show("  您选择了原来的位置，数据存储位置将不会更改。", "更改数据存储位置", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        // 点击"公司"-"添加"
        private void AddPeopleButton_Click(object sender, RoutedEventArgs e)
        {
            EditPeople newPeople = new EditPeople();
            bool? result = newPeople.ShowDialog();
            if (result == true)
            {
                // 点击"确定"后添加人员到数据库并刷新listview
                company.AddPeople(newPeople.PeopleNameTextBox.Text);
                this.CompanyPeopleListView.Items.Refresh();
            }
        }

        // 点击"公司"-"编辑"
        private void EditPeopleButton_Click(object sender, RoutedEventArgs e)
        {
            string oldName = this.CompanyPeopleListView.SelectedValue.ToString();
            EditPeople editPeople = new EditPeople(oldName);
            bool? result = editPeople.ShowDialog();
            if (result == true)
            {
                // 点击"确定"后编辑人员到数据库并刷新listview
                company.EditPeople(oldName, editPeople.PeopleNameTextBox.Text);
                this.CompanyPeopleListView.Items.Refresh();
            }
        }

        // 点击"公司"-"删除"
        private void DeletePeopleButton_Click(object sender, RoutedEventArgs e)
        {
            // 删除选中的人员并刷新listview
            company.DeletePeople(this.CompanyPeopleListView.SelectedItem.ToString());
            this.CompanyPeopleListView.Items.Refresh();
        }

        // 设置鼠标点击人员列表控件CompanyPeopleListView的空白处时清空选择
        private void CompanyPeopleListView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.CompanyPeopleListView.UnselectAll();
        }

        // Save命令的Executed事件处理函数
        private void CommandBindingSave_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // 保存"公司"设置
            company.Name = CompanyNameTextBox.Text;
            company.Code = CompanyCodeTextBox.Text;
            isDirty = false;

            // 保存"标准"设置
            SettingDataBase.EditIndustrialStandard(industrialStandard);
            SettingDataBase.EditLocalStandard(localStandard);

            // 关闭窗口
            if (e.Parameter.ToString() == "Commit")
            {
                this.Close();
            }
        }

        // Save命令的CanExecuted事件处理函数
        private void CommandBindingSave_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            // 设置"确定"按钮的可用状态
            if (e.Source.ToString() == "System.Windows.Controls.Button: 确定")
            {
                e.CanExecute = true;
            }

            // 设置"应用"按钮的可用状态
            if (e.Source.ToString() == "System.Windows.Controls.Button: 应用")
            {
                e.CanExecute = isDirty;
            }
            
        }

        // "公司"-"公司名称"文本框内文本有变化时
        private void CompanyNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isDirty = true;
        }

        // "公司"-"资质编号"文本框内文本有变化时
        private void CompanyCodeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isDirty = true;
        }

        // 点击"取消"
        private void CancelSetting_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // 选择行业标准-水利水电
        private void WaterConservancy_Checked(object sender, RoutedEventArgs e)
        {
            industrialStandard = "WaterConservancy";
        }

        // 选择地方标准-湖北省
        private void Hubei_Checked(object sender, RoutedEventArgs e)
        {
            localStandard = "Hubei";
        }
    }
}
