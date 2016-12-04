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
using System.Data;

namespace GSYGeo
{
    /// <summary>
    /// ProjectLayer.xaml 的交互逻辑
    /// </summary>
    public partial class ProjectLayer : Window
    {
        // 定义DataGrid控件的数据源Datatable
        public DataTable dt = new DataTable("Layer");

        // 无参数的构造函数
        public ProjectLayer()
        {
            InitializeComponent();

            // 初始化DataTable
            InitialDataTable();

            // 绑定
            this.LayerListDataGrid.DataContext = dt;
        }

        // 带参数的构造函数
        public ProjectLayer(List<string> _numberList,List<string> _nameList,List<string> _geoList,List<string> _descriptionList)
        {
            InitializeComponent();

            // 初始化DataTable
            InitialDataTable(_numberList, _nameList, _geoList, _descriptionList);

            // 绑定
            this.LayerListDataGrid.DataContext = dt.DefaultView;
        }

        // 初始化DataTable，不带参数
        private void InitialDataTable()
        {
            // 定义DataTable数据列
            dt.Columns.Add(new DataColumn("numberList", typeof(string)));
            dt.Columns.Add(new DataColumn("nameList", typeof(string)));
            dt.Columns.Add(new DataColumn("geoList", typeof(string)));
            dt.Columns.Add(new DataColumn("descriptionList", typeof(string)));
        }

        // 初始化DataTable，带参数
        private void InitialDataTable(List<string> _numberList, List<string> _nameList, List<string> _geoList, List<string> _descriptionList)
        {
            // 定义DataTable数据列并赋值
            dt.Columns.Add(new DataColumn("numberList", typeof(string)));
            dt.Columns.Add(new DataColumn("nameList", typeof(string)));
            dt.Columns.Add(new DataColumn("geoList", typeof(string)));
            dt.Columns.Add(new DataColumn("descriptionList", typeof(string)));

            // 赋值
            DataRow dr;
            for(int i = 0; i < _numberList.Count; i++)
            {
                dr = dt.NewRow();
                dr["numberList"] = _numberList[i];
                dr["nameList"] = _nameList[i];
                dr["geoList"] = _geoList[i];
                dr["descriptionList"] = _descriptionList[i];
                dt.Rows.Add(dr);
            }
        }

        // 向DataTable添加行
        private void AddRowToDataTable(string _number,string _name,string _geo,string _description)
        {
            DataRow dr = dt.NewRow();
            dr["numberList"] = _number;
            dr["nameList"] = _name;
            dr["geoList"] = _geo;
            dr["descriptionList"] = _description;
            dt.Rows.Add(dr);
        }

        // 编辑DataTable行
        private void EditRowDataTable(int _rowIndex,string _number, string _name, string _geo, string _description)
        {
            DataRow dr = dt.Rows[_rowIndex];
            dr.BeginEdit();
            dr["numberList"] = _number;
            dr["nameList"] = _name;
            dr["geoList"] = _geo;
            dr["descriptionList"] = _description;
            dr.EndEdit();
        }

        // 删除DataTable行
        private void DeleteRowDataTable(int _rowIndex)
        {
            dt.Rows.RemoveAt(_rowIndex);
        }

        // 点击"添加"
        private void AddLayerButton_Click(object sender, RoutedEventArgs e)
        {
            LayerDetail layerDetail = new LayerDetail();
            layerDetail.ShowDialog();
            if (layerDetail.DialogResult == true)
            {
                string number = layerDetail.LayerNumberTextBox.Text;
                string name = layerDetail.LayerNameTextBox.Text;
                string geo = layerDetail.LayerGeoComboBox.Text;
                string description = layerDetail.LayerDescriptionTextBox.Text;
                AddRowToDataTable(number, name, geo, description);
            }
        }

        // 点击"编辑"
        private void EditLayerButton_Click(object sender, RoutedEventArgs e)
        {
            // 赋值传递数据
            DataRowView drv = (DataRowView)this.LayerListDataGrid.SelectedItem;
            string oldNumber = (string)drv[0];
            string oldName = (string)drv[1];
            string oldGeo = (string)drv[2];
            string oldDescription = (string)drv[3];
            int selectIndex = this.LayerListDataGrid.SelectedIndex;

            // 实例化窗口
            LayerDetail layerDetail = new LayerDetail(oldNumber, oldName, oldGeo, oldDescription);
            layerDetail.ShowDialog();
            if (layerDetail.DialogResult == true)
            {
                string number = layerDetail.LayerNumberTextBox.Text;
                string name = layerDetail.LayerNameTextBox.Text;
                string geo = layerDetail.LayerGeoComboBox.Text;
                string description = layerDetail.LayerDescriptionTextBox.Text;
                EditRowDataTable(selectIndex, number, name, geo, description);
            }
        }

        // 点击"删除"
        private void DeleteLayerButton_Click(object sender, RoutedEventArgs e)
        {
            DataRowView drv = (DataRowView)this.LayerListDataGrid.SelectedItem;
            string selectedNumber = (string)drv[0];
            int selectedIndex = LayerListDataGrid.SelectedIndex;
            MessageBoxResult result = MessageBox.Show("确定要删除第 " + selectedNumber + " 层吗？", "删除分层", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.OK)
            {
                DeleteRowDataTable(selectedIndex);
            }
        }

        // 点击DataGrid空白处取消选中状态
        private void LayerListDataGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.LayerListDataGrid.UnselectAll();
        }

        // 点击"确定"
        private void CommitButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            Close();
        }

        // 点击"取消"
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Close();
        }
    }
}
