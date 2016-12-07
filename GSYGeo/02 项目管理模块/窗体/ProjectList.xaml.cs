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
using System.IO;

namespace GSYGeo
{
    /// <summary>
    /// ProjectList.xaml 的交互逻辑
    /// </summary>
    public partial class ProjectList : Window
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProjectList()
        {
            InitializeComponent();

            // 设置ListView数据绑定
            SetBindListView();
        }

        /// <summary>
        /// ListView控件的数据绑定函数
        /// </summary>
        private void SetBindListView()
        {
            List<string> projectList = ProjectDataBase.ReadProjectList();
            this.ProjectListListView.ItemsSource = projectList;
        }

        /// <summary>
        /// 打开项目的函数
        /// </summary>
        private void OpenProject()
        {
            Program.currentProject = this.ProjectListListView.SelectedItem.ToString();
            this.DialogResult = true;
            this.Close();
        }

        /// <summary>
        /// Open命令的Executed事件处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CommandBindingOpen_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // 打开项目
            OpenProject();
        }

        /// <summary>
        /// Open命令的CanExecuted事件处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CommandBindingOpen_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (this.ProjectListListView.SelectedItems.Count > 0)
            {
                e.CanExecute = true;
            }
        }

        /// <summary>
        /// 设置单击空白处取消ListView选中状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProjectListListView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.ProjectListListView.UnselectAll();
        }

        /// <summary>
        /// 设置双击项目名称时打开项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProjectListListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // 选中项目时触发
            if (this.ProjectListListView.SelectedItems.Count > 0)
            {
                // 打开项目
                OpenProject();
            }
        }

        /// <summary>
        /// 点击"关闭"按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
