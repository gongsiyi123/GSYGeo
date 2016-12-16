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
    /// ProjectManager.xaml 的交互逻辑
    /// </summary>
    public partial class ProjectManager : Window
    {
        // 定义项目列表
        List<string> projectList = ProjectDataBase.ReadProjectList();

        // 构造函数
        public ProjectManager()
        {
            InitializeComponent();

            // 读取项目列表，绑定ListView
            this.ProjectListListView.ItemsSource = projectList;
        }

        // 点击"导入"
        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog selectProjectFileDialog = new Microsoft.Win32.OpenFileDialog();
            selectProjectFileDialog.Title = "导入项目文件";
            selectProjectFileDialog.Filter = "项目数据文件|*.gsygeo";
            selectProjectFileDialog.RestoreDirectory = false;
            selectProjectFileDialog.Multiselect = true;
            if (selectProjectFileDialog.ShowDialog() == true)
            {
                try
                {
                    string[] projectFullName = selectProjectFileDialog.FileNames;
                    string[] projectSafeName = selectProjectFileDialog.SafeFileNames;
                    StringBuilder s = new StringBuilder();
                    s.Append("已将以下项目导入到数据库：\n\n");
                    bool isExist = true;
                    int j = 0, k = 0;
                    for (int i = 0; i < projectFullName.Length; i++)
                    {
                        if (!File.Exists(Program.ReadProgramPath() + "\\" + projectSafeName[i]))
                        {
                            File.Move(projectFullName[i], Program.ReadProgramPath() + "\\" + projectSafeName[i]);
                            j++;
                            s.Append(j.ToString() + "、" + projectSafeName[i].Remove(projectSafeName[i].Length - 7) + "\n");
                            projectList.Add(projectSafeName[i].Remove(projectSafeName[i].Length - 7));
                        }
                        else
                        {
                            if (isExist)
                            {
                                s.Append("\n以下项目由于数据库中存在同名项目所以导入失败，请您核实:\n\n");
                                isExist = false;
                            }
                            k++;
                            s.Append(k.ToString() + "、" + projectSafeName[i].Remove(projectSafeName[i].Length - 7) + "\n");
                        }
                    }
                    MessageBox.Show(s.ToString(), "导入完成", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch(Exception error)
                {
                    MessageBox.Show("导入过程中出现错误：\n\n" + error.Message,"导入项目失败",MessageBoxButton.OK,MessageBoxImage.Error);
                }
            }
            this.ProjectListListView.Items.Refresh();
        }

        // 点击"导出"
        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < this.ProjectListListView.SelectedItems.Count; i++)
            {
                list.Add(this.ProjectListListView.SelectedItems[i].ToString());
            }
            if (list != null)
            {
                try
                {
                    System.Windows.Forms.FolderBrowserDialog folderDialog = new System.Windows.Forms.FolderBrowserDialog();
                    folderDialog.ShowDialog();
                    if (folderDialog.SelectedPath != string.Empty)
                    {
                        StringBuilder s = new StringBuilder();
                        s.Append("已将以下" + list.Count.ToString() + "个项目：\n\n");
                        for(int i = 0; i < list.Count; i++)
                        {
                            File.Copy(Program.ReadProgramPath() + "\\" + list[i] + ".gsygeo", folderDialog.SelectedPath + "\\" + list[i] + ".gsygeo");
                            s.Append((i + 1).ToString() + "、" + list[i] + "\n");
                        }
                        s.Append("\n输出至此目录：\n\n");
                        s.Append(folderDialog.SelectedPath);
                        MessageBox.Show(s.ToString());
                    }
                }
                catch(Exception error)
                {
                    MessageBox.Show("导出过程中出现错误：\n\n" + error.Message, "导出项目失败", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // 点击"删除"
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            List<string> list = new List<string>();
            StringBuilder s = new StringBuilder();
            s.Append("您确定要删除以下项目吗？\n\n");
            for (int i = 0; i < this.ProjectListListView.SelectedItems.Count; i++)
            {
                if (this.ProjectListListView.SelectedItem.ToString() == Program.currentProject)
                {
                    MessageBox.Show("请关闭当前已打开的项目后再执行删除操作");
                    return;
                }
                list.Add(this.ProjectListListView.SelectedItems[i].ToString());
                s.Append((i + 1).ToString() + "、" + list[i] + "\n");
            }
            s.Append("\n这" + list.Count.ToString() + "个项目的全部资料将被删除，且无法恢复！");
            MessageBoxResult result = MessageBox.Show(s.ToString(), "删除项目", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if(result==MessageBoxResult.OK)
            {
                try
                {
                    for(int i = 0; i < list.Count; i++)
                    {
                        File.Delete(Program.ReadProgramPath() + "\\" + list[i] + ".gsygeo");
                        projectList.Remove(list[i]);
                    }
                    MessageBox.Show("删除成功！");
                }
                catch(Exception error)
                {
                    MessageBox.Show("删除过程中出现错误：\n\n" + error.Message, "删除项目失败", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            this.ProjectListListView.Items.Refresh();
        }

        // 设置ListView点击空白处时取消选择
        private void ProjectListListView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.ProjectListListView.UnselectAll();
        }

        // 点击"关闭"
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
