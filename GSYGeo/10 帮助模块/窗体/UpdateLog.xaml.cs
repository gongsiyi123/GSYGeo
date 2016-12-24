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
    /// UpdateLog.xaml 的交互逻辑
    /// </summary>
    public partial class UpdateLog : Window
    {
        /// <summary>
        /// 更新内容列表
        /// </summary>
        private List<string> UpdateContent = new List<string>();

        /// <summary>
        /// 构造函数
        /// </summary>
        public UpdateLog()
        {
            InitializeComponent();

            // 项目立项
            UpdateContent.Add("项目立项");
            AddUpdate("立项", "2016.10.19", UpdateContent);

            // 内部测试版0.1.0_alpha发布
            UpdateContent.Add("基础功能构建初步完成，内测版软件名称定为\"小熠岩土勘察\"");
            UpdateContent.Add("1、项目管理模块，包括新建、打开、编辑、关闭、删除、导入、导出");
            UpdateContent.Add("2、钻孔模块，包括新建、删除、编辑、数据录入、查看");
            UpdateContent.Add("3、静力触探模块，包括新建、删除、编辑、数据录入、查看");
            UpdateContent.Add("4、室内试验模块，包括土工常规、颗分试验数据的录入、查看、筛选");
            UpdateContent.Add("5、统计分析模块，包括勘察工作量、标贯/动探、静力触探Ps值、土工常规、颗分试验的成果统计，以及相应Word表格的输出");
            UpdateContent.Add("6、绘图模块，包括钻孔柱状图、静力触探曲线图的CAD图形输出");
            UpdateContent.Add("7、设置模块，包括软件和项目的一些常规设置");
            UpdateContent.Add("8、帮助模块，包括用户反馈、更新日志等");
            AddUpdate("0.1.0_alpha", "2016.12.17", UpdateContent);

            // 内部测试版0.2.0_alpha发布
            UpdateContent.Add("1、统计分析模块，新增承载力和压缩模量综合取值和Word表格输出功能");
            UpdateContent.Add("2、工具模块，新增导入旧版软件数据功能，该功能为供内部使用的临时功能");
            AddUpdate("0.2.0_alpha", "2016.12.23", UpdateContent);

            // 内部测试版0.2.1_alpha发布
            UpdateContent.Add("1、统计分析模块，修复了土工试验统计项含有空数值时的计算错误");
            UpdateContent.Add("2、统计分析模块，修复了勘察工作量统计输出Word表格的一个合并单元格错误，该错误可导致程序崩溃");
            AddUpdate("0.2.1_alpha", "2016.12.24", UpdateContent);

            // 内部测试版0.2.2_alpha发布
            UpdateContent.Add("1、项目管理模块，新增部分地质年代和成因选项");
            AddUpdate("0.2.2_alpha", "2016.12.26", UpdateContent);
        }

        /// <summary>
        /// 更新函数
        /// </summary>
        /// <param name="_version">版本号</param>
        /// <param name="_date">更新日期</param>
        /// <param name="_content">更新内容</param>
        private void AddUpdate(string _version,string _date,List<string> _content)
        {
            StringBuilder updateReport = new StringBuilder();

            updateReport.Append("------------------------------\n");
            updateReport.Append("版本名称：  ");
            updateReport.Append(_version);

            updateReport.Append("\n------------------------------\n更新日期：\n    ");
            updateReport.Append(_date);

            updateReport.Append("\n更新内容：\n    ");
            for(int i = 0; i < _content.Count; i++)
            {
                updateReport.Append(_content[i]);
                updateReport.Append("\n    ");
            }
            updateReport.Append("\n\n\n");

            this.UpdateLogTextBox.AppendText(updateReport.ToString());
            
            _content.Clear();

            this.UpdateLogScrollViewer.ScrollToEnd();
        }

        /// <summary>
        /// 点击"确定"按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
