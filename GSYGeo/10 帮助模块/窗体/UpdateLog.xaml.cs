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
        // 更新内容列表
        private List<string> UpdateContent = new List<string>();

        // 构造函数
        public UpdateLog()
        {
            InitializeComponent();

            // 项目立项
            UpdateContent.Clear();
            UpdateContent.Add("项目立项");
            AddUpdate("立项", "2016.10.19", UpdateContent);
        }

        // 更新函数
        private void AddUpdate(string _version,string _date,List<string> _content)
        {
            StringBuilder updateReport = new StringBuilder();
            updateReport.Append("----------\n");
            updateReport.Append("版本名称：  ");
            updateReport.Append(_version);
            updateReport.Append("\n----------\n更新日期：\n    ");
            updateReport.Append(_date);
            updateReport.Append("\n更新内容：\n    ");
            for(int i = 0; i < _content.Count; i++)
            {
                updateReport.Append(_content[i]);
                updateReport.Append("\n    ");
            }
            updateReport.Append("\n\n\n");
            this.UpdateLogTextBox.AppendText(updateReport.ToString());
        }

        // 点击"确定"
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
