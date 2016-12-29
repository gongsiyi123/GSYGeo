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
using WW.Cad.IO;
using WW.Cad.Model;
using WW.Cad.Model.Entities;
using WW.Cad.Model.Tables;
using WW.Math;

namespace GSYGeo
{
    /// <summary>
    /// OutputJkToCad.xaml 的交互逻辑
    /// </summary>
    public partial class OutputJkToCad : Window
    {
        #region 参数定义

        /// <summary>
        /// 触探孔列表
        /// </summary>
        public static List<CPT> JkList;

        /// <summary>
        /// 比例尺列表
        /// </summary>
        public static List<double> ScaleList;

        #endregion

        #region 工具提示

        /// <summary>
        /// 定义比例尺的工具提示
        /// </summary>
        System.Windows.Controls.ToolTip tt = new System.Windows.Controls.ToolTip();

        /// <summary>
        /// 定义工具提示函数
        /// </summary>
        private void DefineToolTip()
        {
            tt.Content = "这里的比例尺是指要绘制的剖面图钻孔的纵向比例尺";
            this.ScaleListBox.ToolTip = tt;
            tt.PlacementTarget = this.ScaleListBox;
            tt.Placement = System.Windows.Controls.Primitives.PlacementMode.Right;
            tt.Foreground = Brushes.Red;
            ToolTipService.SetShowDuration(this.ScaleListBox, 1500);
        }

        /// <summary>
        /// 鼠标移入事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScaleTextBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            tt.IsOpen = true;
        }

        /// <summary>
        /// 鼠标移出事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScaleTextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            tt.IsOpen = false;
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public OutputJkToCad()
        {
            InitializeComponent();

            // 读取数据并赋值ComboBox
            ReadJkList();
            ReadScaleList();

            // 定义工具提示
            DefineToolTip();
        }

        #endregion

        #region 读取数据

        /// <summary>
        /// 读取触探孔数据函数
        /// </summary>
        private void ReadJkList()
        {
            // 读取钻孔数据
            JkList = CPTDataBase.ReadJkListAsClass(Program.currentProject);

            // 赋值ComboBox
            for (int i = 0; i < JkList.Count; i++)
            {
                CheckBox checkBox = new CheckBox();
                checkBox.Content = JkList[i].Name;
                this.JkListBox.Items.Add(checkBox);
            }
        }

        /// <summary>
        /// 读取比例尺数据函数
        /// </summary>
        private void ReadScaleList()
        {
            // 读取比例尺数据
            ScaleList = new List<double>();
            ScaleList.Add(100);
            ScaleList.Add(200);
            ScaleList.Add(500);

            // 赋值ComboBox，默认全选
            for (int i = 0; i < ScaleList.Count; i++)
            {
                CheckBox checkBox = new CheckBox();
                checkBox.Content = "1：" + ScaleList[i];
                checkBox.IsChecked = true;
                this.ScaleListBox.Items.Add(checkBox);
            }
        }

        #endregion

        #region 输出

        /// <summary>
        /// 点击"输出"按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OutputButton_Click(object sender, RoutedEventArgs e)
        {
            // 读取选中的触探孔列表
            List<CPT> checkedJkList = new List<CPT>();
            foreach (CheckBox checkBox in this.JkListBox.Items)
                if (checkBox.IsChecked == true)
                    for (int i = 0; i < JkList.Count; i++)
                        if (JkList[i].Name == checkBox.Content.ToString())
                            checkedJkList.Add(JkList[i]);

            // 没有选中时退出
            if (checkedJkList.Count == 0)
            {
                MessageBox.Show("亲，您没有选中任何触探孔哦！");
                return;
            }

            // 选择输出目录
            string path;

            System.Windows.Forms.FolderBrowserDialog programPathBrowser = new System.Windows.Forms.FolderBrowserDialog();
            if (programPathBrowser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                path = programPathBrowser.SelectedPath + @"\" + Program.currentProject + @"-静力触探曲线图.dwg";
            else
                return;

            if (File.Exists((string)path))
                File.Delete((string)path);

            // 读取选中的比例尺列表
            List<double> checkedScaleList = new List<double>();
            foreach (CheckBox checkBox in this.ScaleListBox.Items)
                if (checkBox.IsChecked == true)
                    checkedScaleList.Add(Convert.ToDouble(checkBox.Content.ToString().Substring(checkBox.Content.ToString().IndexOf("：") + 1)));


            // 启动输出窗体
            ShowProgressBar(path, checkedJkList, checkedScaleList);
        }

        /// <summary>
        /// 绘制静力触探曲线函数
        /// </summary>
        /// <param name="_path">输出文件的路径</param>
        /// <param name="_checkedZkList">选中的钻孔列表</param>
        /// <param name="_checkedScaleList">选中的比例尺列表</param>
        public static void OutputToCad(string _path, List<CPT> _checkedJkList, List<double> _checkedScaleList)
        {
            // 实例化CAD对象
            CAD cad = new CAD();

            // 添加文字样式
            DxfTextStyle style1 = cad.AddStyle("GB2312", "仿宋_GB2312.ttf", 1);
            DxfTextStyle style2 = cad.AddStyle("GB2312_08", "仿宋_GB2312.ttf", 0.8);

            // 循环绘图
            for (int i = 0; i < _checkedJkList.Count; i++)
            {
                cad.DrawJk(i, Program.currentProject, ProjectDataBase.ReadProjectCompany(Program.currentProject), _checkedJkList[i], _checkedScaleList, style1, style2);
            }

            // 保存CAD文件
            cad.SaveAsDwg(_path, 195 * _checkedJkList.Count / 8, 280, 195 * _checkedJkList.Count);
        }

        /// <summary>
        /// 实例化输出进度窗体
        /// </summary>
        /// <param name="obj">输出路径</param>
        private void ShowProgressBar(string _path, List<CPT> _checkedJkList, List<double> _checkedScaleList)
        {
            // 实例化窗体
            OutputProgress prog = new OutputProgress(OutputProgress.OutputType.JkCad, _path, "输出静力触探曲线图", "正在输出静力触探曲线图……", _checkedJkList, _checkedScaleList);
            prog.ShowDialog();
        }

        #endregion

        #region 其他

        /// <summary>
        /// 选中"全选"框时，选中所有钻孔
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectAllJkCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            foreach (CheckBox item in this.JkListBox.Items)
            {
                item.IsChecked = true;
            }
        }

        /// <summary>
        /// 取消选中"全选"框时，取消选中所有钻孔
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectAllJkCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (CheckBox item in this.JkListBox.Items)
            {
                item.IsChecked = false;
            }
        }

        /// <summary>
        /// 点击"关闭"按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #endregion
    }
}
