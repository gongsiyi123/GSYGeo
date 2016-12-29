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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;

namespace GSYGeo
{
    /// <summary>
    /// GrainAnalysisTestControl.xaml 的交互逻辑
    /// </summary>
    public partial class GrainAnalysisTestControl : UserControl
    {
        #region 参数定义

        /// <summary>
        /// 定义试验数据列表TestDataListDataGrid控件的数据源DataTable
        /// </summary>
        public DataTable dtGAT = new DataTable("GAT");

        /// <summary>
        /// 定义判断当前是否正在编辑DataGrid的变量
        /// </summary>
        bool isEditing = false;

        /// <summary>
        /// 定义备份DataTable
        /// </summary>
        private DataTable dtBackUp = new DataTable("BackUp");

        #endregion

        #region 构造函数

        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        /// <param name="_gats">颗分试验数据列表</param>
        public GrainAnalysisTestControl(List<GrainAnalysisTest> _gats)
        {
            InitializeComponent();

            // 初始化DataTable
            InitialTestDataListDataGrid(_gats);

            // 设置绑定
            this.GrainAnalysisTestDataGrid.DataContext = dtGAT;

            // 计算取样所属分层并刷新数据库
            CalcuSampleLayer();
            Save();

            // 填充筛选ComboBox
            InitialComboBox();

            // 设置按钮可用性
            SetButtonEnable(false);
        }

        #endregion

        #region 公用函数

        /// <summary>
        /// 设置按钮的可用性函数
        /// </summary>
        /// <param name="_isEditing">是否正在编辑</param>
        private void SetButtonEnable(bool _isEditing)
        {
            if (_isEditing)
            {
                isEditing = _isEditing;
                SelectByZkComboBox.IsEnabled = false;
                SelectByLayerComboBox.IsEnabled = false;
                EditButton.IsEnabled = false;
                SaveButton.IsEnabled = true;
                ClearButton.IsEnabled = true;
                RecoverButton.IsEnabled = false;
                GrainAnalysisTestDataGrid.IsReadOnly = false;
                GrainAnalysisTestDataGrid.Foreground = Brushes.Black;
            }
            else
            {
                isEditing = _isEditing;
                SelectByZkComboBox.IsEnabled = true;
                SelectByLayerComboBox.IsEnabled = true;
                EditButton.IsEnabled = true;
                SaveButton.IsEnabled = false;
                ClearButton.IsEnabled = false;
                RecoverButton.IsEnabled = false;
                GrainAnalysisTestDataGrid.IsReadOnly = true;
                GrainAnalysisTestDataGrid.Foreground = Brushes.Gray;
            }
        }

        #endregion

        #region 试验数据

        /// <summary>
        /// 定义试验项目
        /// </summary>
        private string[] gatName = new string[]
        {
            "zkNumber","sampleDepth","sampleLayer","Group20ToMax","Group2To20","Group0_5To2","Group0_25To0_5","Group0_075To0_25","Group0To0_075"
        };

        /// <summary>
        /// 初始化TestDataListDataGrid，不带参数
        /// </summary>
        private void InitialTestDataListDataGrid()
        {
            // 定义TestDataListDataGrie数据列
            foreach (string gat in gatName)
            {
                dtGAT.Columns.Add(new DataColumn(gat, typeof(string)));
            }
        }

        /// <summary>
        /// 初始化TestDataListDataGrid，带参数
        /// </summary>
        /// <param name="_gats">颗分试验数据列表</param>
        private void InitialTestDataListDataGrid(List<GrainAnalysisTest> _gats)
        {
            // 定义TestDataListDataGrie数据列
            foreach (string gat in gatName)
            {
                dtGAT.Columns.Add(new DataColumn(gat, typeof(string)));
            }

            // 赋值
            DataRow dr;
            for (int i = 0; i < _gats.Count; i++)
            {
                dr = dtGAT.NewRow();
                dr["zkNumber"] = _gats[i].zkNumber;
                dr["sampleDepth"] = _gats[i].sampleDepth.ToString("0.00");
                dr["sampleLayer"] = _gats[i].sampleLayer;
                dr["Group0To0_075"] = _gats[i].Group0To0_075.ToString() == "-0.19880205" ? "" : _gats[i].Group0To0_075.ToString("0.00");
                dr["Group0_075To0_25"] = _gats[i].Group0_075To0_25.ToString() == "-0.19880205" ? "" : _gats[i].Group0_075To0_25.ToString("0.00");
                dr["Group0_25To0_5"] = _gats[i].Group0_25To0_5.ToString() == "-0.19880205" ? "" : _gats[i].Group0_25To0_5.ToString("0.00");
                dr["Group0_5To2"] = _gats[i].Group0_5To2.ToString() == "-0.19880205" ? "" : _gats[i].Group0_5To2.ToString("0.00");
                dr["Group2To20"] = _gats[i].Group2To20.ToString() == "-0.19880205" ? "" : _gats[i].Group2To20.ToString("0.00");
                dr["Group20ToMax"] = _gats[i].Group20ToMax.ToString() == "-0.19880205" ? "" : _gats[i].Group20ToMax.ToString("0.00");
                dtGAT.Rows.Add(dr);
            }
        }

        /// <summary>
        /// 重置刷新TestDataListDataGrid
        /// </summary>
        /// <param name="_gats">颗分试验数据列表</param>
        private void RefreshTestDataListDataGrid(List<GrainAnalysisTest> _gats)
        {
            // 清空旧数据
            dtGAT.Clear();

            // 赋值新数据
            DataRow dr;
            for (int i = 0; i < _gats.Count; i++)
            {
                dr = dtGAT.NewRow();
                dr["zkNumber"] = _gats[i].zkNumber;
                dr["sampleDepth"] = _gats[i].sampleDepth.ToString("0.00");
                dr["sampleLayer"] = _gats[i].sampleLayer;
                dr["Group0To0_075"] = _gats[i].Group0To0_075.ToString() == "-0.19880205" ? "" : _gats[i].Group0To0_075.ToString("0.00");
                dr["Group0_075To0_25"] = _gats[i].Group0_075To0_25.ToString() == "-0.19880205" ? "" : _gats[i].Group0_075To0_25.ToString("0.00");
                dr["Group0_25To0_5"] = _gats[i].Group0_25To0_5.ToString() == "-0.19880205" ? "" : _gats[i].Group0_25To0_5.ToString("0.00");
                dr["Group0_5To2"] = _gats[i].Group0_5To2.ToString() == "-0.19880205" ? "" : _gats[i].Group0_5To2.ToString("0.00");
                dr["Group2To20"] = _gats[i].Group2To20.ToString() == "-0.19880205" ? "" : _gats[i].Group2To20.ToString("0.00");
                dr["Group20ToMax"] = _gats[i].Group20ToMax.ToString() == "-0.19880205" ? "" : _gats[i].Group20ToMax.ToString("0.00");
                dtGAT.Rows.Add(dr);
            }
        }

        #endregion

        #region 筛选、清空和编辑

        /// <summary>
        /// 填充筛选ComboBox函数
        /// </summary>
        private void InitialComboBox()
        {
            List<string> zklist = BoreholeDataBase.ReadZkList(Program.currentProject);
            zklist.Insert(0, "全部钻孔");
            List<string> layerNumberlist = ProjectDataBase.ReadLayerNumberList(Program.currentProject);
            List<string> layerNamelist = ProjectDataBase.ReadLayerNameList(Program.currentProject);
            List<string> layerlist = new List<string>();
            for (int i = 0; i < layerNumberlist.Count; i++)
            {
                layerlist.Add(layerNumberlist[i] + "   " + layerNamelist[i]);
            }
            layerlist.Insert(0, "全部分层");
            this.SelectByZkComboBox.ItemsSource = zklist;
            this.SelectByLayerComboBox.ItemsSource = layerlist;
            this.SelectByZkComboBox.SelectedIndex = 0;
            this.SelectByLayerComboBox.SelectedIndex = 0;
        }

        /// <summary>
        /// 筛选数据函数
        /// </summary>
        /// <param name="_zk">钻孔编号</param>
        /// <param name="_layer">分层编号</param>
        private void SelectData(string _zk, string _layer)
        {
            if (_zk == "全部钻孔")
            {
                _zk = "";
            }
            if (_layer == "全部分层")
            {
                _layer = "";
            }
            if(_layer!="")
            {
                _layer = _layer.Substring(0, _layer.IndexOf("   "));
            }
            List<GrainAnalysisTest> gats = GrainAnalysisTestDataBase.SelectByZkAndLayer(Program.currentProject, _zk, _layer);
            
            RefreshTestDataListDataGrid(gats);
        }

        /// <summary>
        /// 选择钻孔筛选框时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectByZkComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string zk = this.SelectByZkComboBox.SelectedItem == null ? "" : this.SelectByZkComboBox.SelectedItem.ToString();
            string layer = this.SelectByLayerComboBox.SelectedItem == null ? "" : this.SelectByLayerComboBox.SelectedItem.ToString();
            SelectData(zk, layer);
        }

        /// <summary>
        /// 选择分层筛选框时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectByLayerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string zk = this.SelectByZkComboBox.SelectedItem == null ? "" : this.SelectByZkComboBox.SelectedItem.ToString();
            string layer = this.SelectByLayerComboBox.SelectedItem == null ? "" : this.SelectByLayerComboBox.SelectedItem.ToString();
            SelectData(zk, layer);
        }

        /// <summary>
        /// 点击"编辑"按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            this.SelectByZkComboBox.SelectedIndex = 0;
            this.SelectByLayerComboBox.SelectedIndex = 0;
            SetButtonEnable(true);
        }

        /// <summary>
        /// 点击"清空"按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            dtBackUp = dtGAT.Clone();
            dtBackUp.Clear();
            foreach(DataRow dr in dtGAT.Rows)
            {
                dtBackUp.ImportRow(dr);
            }
            dtGAT.Clear();
            RecoverButton.IsEnabled = true;
            ClearButton.IsEnabled = false;
        }

        /// <summary>
        /// 点击"恢复"按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RecoverButton_Click(object sender, RoutedEventArgs e)
        {
            if (dtBackUp != null)
            {
                foreach(DataRow dr in dtBackUp.Rows)
                {
                    dtGAT.ImportRow(dr);
                }
            }
            RecoverButton.IsEnabled = false;
            ClearButton.IsEnabled = true;
        }

        #endregion

        #region 保存

        /// <summary>
        /// 检查保存数据合法性函数
        /// </summary>
        /// <returns></returns>
        private bool CanSave()
        {
            for (int i = 0; i < dtGAT.Rows.Count; i++)
            {
                for (int j = 0; j < dtGAT.Columns.Count; j++)
                {
                    // 检查取样孔号是否与数据库中匹配
                    if (j == 0)
                    {
                        string zkName = dtGAT.Rows[i][j].ToString();
                        if (!BoreholeDataBase.ReadZkList(Program.currentProject).Contains(zkName))
                        {
                            MessageBox.Show("第" + (i + 1) + "行的取样孔号 " + zkName + " 无法在钻孔数据库中找到，请核实");
                            return false;
                        }
                    }
                    // 检查取样深度是否为有效数字
                    else if (j == 1)
                    {
                        double num;
                        string dep = dtGAT.Rows[i][j].ToString();
                        if (string.IsNullOrEmpty(dep) || string.IsNullOrWhiteSpace(dep))
                        {
                            MessageBox.Show("第" + (i + 1) + "行的取样深度 " + dep + " 是空值，取样深度不能为空");
                            return false;
                        }
                        else if (!double.TryParse(dep, out num))
                        {
                            MessageBox.Show("第" + (i + 1) + "行的取样深度 " + dep + " 不是有效数字");
                            return false;
                        }
                    }
                    // 跳过取样所属分层的检查
                    else if (j == 2)
                    {
                        continue;
                    }
                    // 检查试验数据是否为空
                    else
                    {
                        double num;
                        string data = dtGAT.Rows[i][j].ToString();
                        if (!string.IsNullOrEmpty(data) && !string.IsNullOrWhiteSpace(data) && !double.TryParse(data, out num))
                        {
                            MessageBox.Show("第" + (i + 1) + "行的 " + this.GrainAnalysisTestDataGrid.Columns[j].Header + " " + data + " 不是有效数字");
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 计算取样所属分层函数
        /// </summary>
        private void CalcuSampleLayer()
        {
            List<string> zkNumbers = BoreholeDataBase.ReadZkList(Program.currentProject);

            foreach (string zkNumber in zkNumbers)
            {
                List<ZkLayer> layers = BoreholeDataBase.ReadZkLayer(Program.currentProject, zkNumber);
                for (int i = 0; i < dtGAT.Rows.Count; i++)
                {
                    string thisNumber = dtGAT.Rows[i]["zkNumber"].ToString();
                    double thisDepth = Convert.ToDouble(dtGAT.Rows[i]["sampleDepth"]);
                    if (thisNumber == zkNumber)
                    {
                        for (int j = 0; j < layers.Count; j++)
                        {
                            if (thisDepth <= layers[j].Depth)
                            {
                                dtGAT.Rows[i]["sampleLayer"] = layers[j].Number;
                                break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 保存试验数据函数
        /// </summary>
        private void Save()
        {
            // 提取参数
            List<GrainAnalysisTest> gats = new List<GrainAnalysisTest>();
            for (int i = 0; i < dtGAT.Rows.Count; i++)
            {
                double num;
                string zkNumber = dtGAT.Rows[i]["zkNumber"].ToString();
                double sampleDepth = Convert.ToDouble(dtGAT.Rows[i]["sampleDepth"]);
                string sampleLayer = dtGAT.Rows[i]["sampleLayer"].ToString();
                double group0to0075 = double.TryParse(dtGAT.Rows[i]["Group0To0_075"].ToString(), out num) ? num : -0.19880205;
                double group0075to025 = double.TryParse(dtGAT.Rows[i]["Group0_075To0_25"].ToString(), out num) ? num : -0.19880205;
                double group025to05 = double.TryParse(dtGAT.Rows[i]["Group0_25To0_5"].ToString(), out num) ? num : -0.19880205;
                double group05to2 = double.TryParse(dtGAT.Rows[i]["Group0_5To2"].ToString(), out num) ? num : -0.19880205;
                double group2to20 = double.TryParse(dtGAT.Rows[i]["Group2To20"].ToString(), out num) ? num : -0.19880205;
                double group20tomax = double.TryParse(dtGAT.Rows[i]["Group20ToMax"].ToString(), out num) ? num : -0.19880205;

                GrainAnalysisTest gat = new GrainAnalysisTest(zkNumber, sampleDepth, sampleLayer, group0to0075, group0075to025, group025to05, group05to2, group2to20, group20tomax);
                gats.Add(gat);
            }

            // 保存试验数据到数据库
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GrainAnalysisTestDataBase.Refresh(Program.currentProject, gats);
        }

        /// <summary>
        /// 点击"保存"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // 恢复筛选状态
            this.SelectByZkComboBox.SelectedIndex = 0;
            this.SelectByLayerComboBox.SelectedIndex = 0;

            // 清除空数据行
            dtGAT = DtOperation.RemoveEmptyRow(dtGAT);

            // 检查数据合法性并保存
            if (CanSave())
            {
                // 计算取样所属分层
                CalcuSampleLayer();

                // 保存
                Save();

                // 更新导航树
                if (!MainWindow.bind.IsExistSecondTreeItem(3, "颗粒分析"))
                {
                    MainWindow.bind.AddItemToSecondTree(3, "颗粒分析");

                    // 若是首次输入数据，关闭控件弹出并成功提示
                    MessageBox.Show("保存成功！");
                    this.Visibility = Visibility.Collapsed;
                }
                else
                {
                    SetButtonEnable(false);
                    MessageBox.Show("保存成功！");
                }
                MainWindow.bind.TreeItem[3].IsExpanded = true;
            }
        }


        #endregion

        #region 复制和粘贴

        /// <summary>
        /// 按下CTRL+V，复制Excel中的数据粘贴到DataGrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GrainAnalysisTestDataGrid_KeyDown(object sender, KeyEventArgs e)
        {
            // 如果不处于编辑状态则不相应键盘操作
            if (!isEditing)
            {
                return;
            }

            // 判断按下的是否为CTRL+V
            if (ModifierKeys.Control == Keyboard.Modifiers && e.Key == Key.V)
            {
                // 获取Excel复制的数据，数据为空时退出函数
                List<string[]> excelData = OfficeOperation.GetDataFromExcelClipBoard();
                if (excelData == null)
                {
                    return;
                }

                // 获取当前选中的DataGrid行号和列号
                int row, column;
                if (this.GrainAnalysisTestDataGrid.SelectedCells.Count == 0)
                {
                    row = 0;
                    column = 0;
                }
                else
                {
                    row = this.GrainAnalysisTestDataGrid.Items.IndexOf(this.GrainAnalysisTestDataGrid.SelectedCells[0].Item);
                    column = this.GrainAnalysisTestDataGrid.SelectedCells[0].Column.DisplayIndex;
                }

                // 将复制的数据添加到绑定的DataTable
                dtGAT = DtOperation.PasteFromExcel(dtGAT, row, column, excelData, 2);

            }
        }

        #endregion
    }
}
