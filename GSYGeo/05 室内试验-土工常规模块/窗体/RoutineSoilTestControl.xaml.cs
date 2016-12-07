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
    /// RoutineSoilTestControl.xaml 的交互逻辑
    /// </summary>
    public partial class RoutineSoilTestControl : UserControl
    {
        #region 参数定义

        // 定义试验数据列表TestDataListDataGrid控件的数据源DataTable
        public DataTable dtRST = new DataTable("RST");

        // 定义判断当前是否正在编辑DataGrid的变量
        bool isEditing = false;

        // 定义备份DataTable
        private DataTable dtBackUp = new DataTable("BackUp");

        #endregion

        #region 构造函数

        // 带参数的构造函数
        public RoutineSoilTestControl(List<RoutineSoilTest> _rsts)
        {
            InitializeComponent();

            // 初始化DataTable
            InitialTestDataListDataGrid(_rsts);
            
            // 设置绑定
            this.RoutineSoilTestDataGrid.DataContext = dtRST;

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

        // 设置按钮的可用性函数
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
                RoutineSoilTestDataGrid.IsReadOnly = false;
                RoutineSoilTestDataGrid.Foreground = Brushes.Black;
            }
            else
            {
                isEditing = _isEditing;
                SelectByZkComboBox.IsEnabled = true;
                SelectByLayerComboBox.IsEnabled = true;
                EditButton.IsEnabled = true;
                SaveButton.IsEnabled = false;
                ClearButton.IsEnabled = false;
                RoutineSoilTestDataGrid.IsReadOnly = true;
                RoutineSoilTestDataGrid.Foreground = Brushes.Gray;
            }
        }

        #endregion

        #region 试验数据

        // 定义试验项目
        private string[] rstName = new string[]
        {
            "zkNumber","sampleDepth","sampleLayer","waterLevel","density","specificGravity","voidRatio",
            "saturation","liquidLimit","plasticLimit","plasticIndex","liquidityIndex",
            "compressibility","modulus","frictionAngle","cohesion","permeability"
        };

        // 初始化TestDataListDataGrid，不带参数
        private void InitialTestDataListDataGrid()
        {
            // 定义TestDataListDataGrie数据列
            foreach(string rst in rstName)
            {
                dtRST.Columns.Add(new DataColumn(rst, typeof(string)));
            }
        }

        // 初始化TestDataListDataGrid，带参数
        private void InitialTestDataListDataGrid(List<RoutineSoilTest> _rsts)
        {
            // 定义TestDataListDataGrie数据列
            foreach (string rst in rstName)
            {
                dtRST.Columns.Add(new DataColumn(rst, typeof(string)));
            }

            // 赋值
            DataRow dr;
            for(int i = 0; i < _rsts.Count; i++)
            {
                dr = dtRST.NewRow();
                dr["zkNumber"] = _rsts[i].zkNumber;
                dr["sampleDepth"] = _rsts[i].sampleDepth.ToString("0.00");
                dr["sampleLayer"] = _rsts[i].sampleLayer;
                dr["waterLevel"] = _rsts[i].waterLevel.ToString() == "-0.19880205" ? "" : _rsts[i].waterLevel.ToString("0.0");
                dr["density"] = _rsts[i].density.ToString() == "-0.19880205" ? "" : _rsts[i].density.ToString("0.00");
                dr["specificGravity"] = _rsts[i].specificGravity.ToString() == "-0.19880205" ? "" : _rsts[i].specificGravity.ToString("0.00");
                dr["voidRatio"] = _rsts[i].voidRatio.ToString() == "-0.19880205" ? "" : _rsts[i].voidRatio.ToString("0.000");
                dr["saturation"] = _rsts[i].saturation.ToString() == "-0.19880205" ? "" : _rsts[i].saturation.ToString("0");
                dr["liquidLimit"] = _rsts[i].liquidLimit.ToString() == "-0.19880205" ? "" : _rsts[i].liquidLimit.ToString("0.0");
                dr["plasticLimit"] = _rsts[i].plasticLimit.ToString() == "-0.19880205" ? "" : _rsts[i].plasticLimit.ToString("0.0");
                dr["plasticIndex"] = _rsts[i].plasticIndex.ToString() == "-0.19880205" ? "" : _rsts[i].plasticIndex.ToString("0.0");
                dr["liquidityIndex"] = _rsts[i].liquidityIndex.ToString() == "-0.19880205" ? "" : _rsts[i].liquidityIndex.ToString("0.0");
                dr["compressibility"] = _rsts[i].compressibility.ToString() == "-0.19880205" ? "" : _rsts[i].compressibility.ToString("0.00");
                dr["modulus"] = _rsts[i].modulus.ToString() == "-0.19880205" ? "" : _rsts[i].modulus.ToString("0.0");
                dr["frictionAngle"] = _rsts[i].frictionAngle.ToString() == "-0.19880205" ? "" : _rsts[i].frictionAngle.ToString("0.0");
                dr["cohesion"] = _rsts[i].cohesion.ToString() == "-0.19880205" ? "" : _rsts[i].cohesion.ToString("0.0");
                dr["permeability"] = _rsts[i].permeability.ToString() == "-0.19880205" ? "" : _rsts[i].permeability.ToString("0.0E0");
                dtRST.Rows.Add(dr);
            }
        }

        // 重置刷新TestDataListDataGrid
        private void RefreshTestDataListDataGrid(List<RoutineSoilTest> _rsts)
        {
            // 清空旧数据
            dtRST.Clear();

            // 赋值新数据
            DataRow dr;
            for (int i = 0; i < _rsts.Count; i++)
            {
                dr = dtRST.NewRow();
                dr["zkNumber"] = _rsts[i].zkNumber;
                dr["sampleDepth"] = _rsts[i].sampleDepth.ToString("0.00");
                dr["sampleLayer"] = _rsts[i].sampleLayer;
                dr["waterLevel"] = _rsts[i].waterLevel.ToString() == "-0.19880205" ? "" : _rsts[i].waterLevel.ToString("0.0");
                dr["density"] = _rsts[i].density.ToString() == "-0.19880205" ? "" : _rsts[i].density.ToString("0.00");
                dr["specificGravity"] = _rsts[i].specificGravity.ToString() == "-0.19880205" ? "" : _rsts[i].specificGravity.ToString("0.00");
                dr["voidRatio"] = _rsts[i].voidRatio.ToString() == "-0.19880205" ? "" : _rsts[i].voidRatio.ToString("0.000");
                dr["saturation"] = _rsts[i].saturation.ToString() == "-0.19880205" ? "" : _rsts[i].saturation.ToString("0");
                dr["liquidLimit"] = _rsts[i].liquidLimit.ToString() == "-0.19880205" ? "" : _rsts[i].liquidLimit.ToString("0.0");
                dr["plasticLimit"] = _rsts[i].plasticLimit.ToString() == "-0.19880205" ? "" : _rsts[i].plasticLimit.ToString("0.0");
                dr["plasticIndex"] = _rsts[i].plasticIndex.ToString() == "-0.19880205" ? "" : _rsts[i].plasticIndex.ToString("0.0");
                dr["liquidityIndex"] = _rsts[i].liquidityIndex.ToString() == "-0.19880205" ? "" : _rsts[i].liquidityIndex.ToString("0.0");
                dr["compressibility"] = _rsts[i].compressibility.ToString() == "-0.19880205" ? "" : _rsts[i].compressibility.ToString("0.00");
                dr["modulus"] = _rsts[i].modulus.ToString() == "-0.19880205" ? "" : _rsts[i].modulus.ToString("0.0");
                dr["frictionAngle"] = _rsts[i].frictionAngle.ToString() == "-0.19880205" ? "" : _rsts[i].frictionAngle.ToString("0.0");
                dr["cohesion"] = _rsts[i].cohesion.ToString() == "-0.19880205" ? "" : _rsts[i].cohesion.ToString("0.0");
                dr["permeability"] = _rsts[i].permeability.ToString() == "-0.19880205" ? "" : _rsts[i].permeability.ToString("0.0E0");
                dtRST.Rows.Add(dr);
            }
        }

        #endregion

        #region 筛选、清空和编辑

        // 填充筛选ComboBox函数
        private void InitialComboBox()
        {
            List<string> zklist = BoreholeDataBase.ReadZkList(Program.currentProject);
            zklist.Insert(0, "全部钻孔");
            List<string> layerNumberlist = ProjectDataBase.ReadLayerNumberList(Program.currentProject);
            List<string> layerNamelist = ProjectDataBase.ReadLayerNameList(Program.currentProject);
            List<string> layerlist = new List<string>();
            for(int i = 0; i < layerNumberlist.Count; i++)
            {
                layerlist.Add(layerNumberlist[i] + "   " + layerNamelist[i]);
            }
            layerlist.Insert(0, "全部分层");
            this.SelectByZkComboBox.ItemsSource = zklist;
            this.SelectByLayerComboBox.ItemsSource = layerlist;
            this.SelectByZkComboBox.SelectedIndex = 0;
            this.SelectByLayerComboBox.SelectedIndex = 0;
        }

        // 筛选数据函数
        private void SelectData(string _zk,string _layer)
        {
            if (_zk == "全部钻孔")
            {
                _zk = "";
            }
            if (_layer == "全部分层")
            {
                _layer = "";
            }
            if (_layer != "")
            {
                _layer = _layer.Substring(0, _layer.IndexOf("   "));
            }
            List<RoutineSoilTest> rsts = RoutineSoilTestDataBase.SelectByZkAndLayer(Program.currentProject, _zk, _layer);

            RefreshTestDataListDataGrid(rsts);
        }

        // 选择钻孔筛选框时
        private void SelectByZkComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string zk = this.SelectByZkComboBox.SelectedItem == null ? "" : this.SelectByZkComboBox.SelectedItem.ToString();
            string layer = this.SelectByLayerComboBox.SelectedItem == null ? "" : this.SelectByLayerComboBox.SelectedItem.ToString();
            SelectData(zk, layer);
        }

        // 选择分层筛选框时
        private void SelectByLayerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string zk = this.SelectByZkComboBox.SelectedItem == null ? "" : this.SelectByZkComboBox.SelectedItem.ToString();
            string layer = this.SelectByLayerComboBox.SelectedItem == null ? "" : this.SelectByLayerComboBox.SelectedItem.ToString();
            SelectData(zk, layer);
        }

        // 点击"编辑"按钮
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            this.SelectByZkComboBox.SelectedIndex = 0;
            this.SelectByLayerComboBox.SelectedIndex = 0;
            SetButtonEnable(true);
        }

        // 点击"清空"按钮
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            dtBackUp = dtRST.Clone();
            dtBackUp.Clear();
            foreach (DataRow dr in dtRST.Rows)
            {
                dtBackUp.ImportRow(dr);
            }
            dtRST.Clear();
            RecoverButton.IsEnabled = true;
            ClearButton.IsEnabled = false;
        }

        // 点击"恢复"按钮
        private void RecoverButton_Click(object sender, RoutedEventArgs e)
        {
            if (dtBackUp != null)
            {
                foreach (DataRow dr in dtBackUp.Rows)
                {
                    dtRST.ImportRow(dr);
                }
            }
            RecoverButton.IsEnabled = false;
            ClearButton.IsEnabled = true;
        }

        #endregion

        #region 保存

        // 检查保存数据合法性函数
        private bool CanSave()
        {
            for(int i = 0; i < dtRST.Rows.Count; i++)
            {
                for(int j = 0; j < dtRST.Columns.Count; j++)
                {
                    // 检查取样孔号是否与数据库中匹配
                    if (j == 0)
                    {
                        string zkName = dtRST.Rows[i][j].ToString();
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
                        string dep = dtRST.Rows[i][j].ToString();
                        if (string.IsNullOrEmpty(dep) || string.IsNullOrWhiteSpace(dep))
                        {
                            MessageBox.Show("第" + (i + 1) + "行的取样深度 " + dep + " 是空值，取样深度不能为空");
                            return false;
                        }
                        else if (!double.TryParse(dep,out num))
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
                        string data = dtRST.Rows[i][j].ToString();
                        if(!string.IsNullOrEmpty(data) && !string.IsNullOrWhiteSpace(data) && !double.TryParse(data,out num))
                        {
                            MessageBox.Show("第" + (i + 1) + "行的 " + this.RoutineSoilTestDataGrid.Columns[j].Header + " " + data + " 不是有效数字");
                            return false;
                        }
                    }
                }
            }
            
            return true;
        }
        
        // 计算取样所属分层函数
        private void CalcuSampleLayer()
        {
            List<string> zkNumbers = BoreholeDataBase.ReadZkList(Program.currentProject);

            foreach (string zkNumber in zkNumbers)
            {
                List<ZkLayer> layers = BoreholeDataBase.ReadZkLayer(Program.currentProject, zkNumber);
                for (int i = 0; i < dtRST.Rows.Count; i++)
                {
                    string thisNumber = dtRST.Rows[i]["zkNumber"].ToString();
                    double thisDepth = Convert.ToDouble(dtRST.Rows[i]["sampleDepth"]);
                    if (thisNumber == zkNumber)
                    {
                        for (int j = 0; j < layers.Count; j++)
                        {
                            if (thisDepth <= layers[j].Depth)
                            {
                                dtRST.Rows[i]["sampleLayer"] = layers[j].Number;
                                break;
                            }
                        }
                    }
                }
            }
        }

        // 保存试验数据函数
        private void Save()
        {
            // 提取参数
            List<RoutineSoilTest> rsts = new List<RoutineSoilTest>();
            for (int i = 0; i < dtRST.Rows.Count; i++)
            {
                double num;
                string zkNumber = dtRST.Rows[i]["zkNumber"].ToString();
                double sampleDepth = Convert.ToDouble(dtRST.Rows[i]["sampleDepth"]);
                string sampleLayer = dtRST.Rows[i]["sampleLayer"].ToString();
                double waterLevel = double.TryParse(dtRST.Rows[i]["waterLevel"].ToString(), out num) ? num : -0.19880205;
                double density = double.TryParse(dtRST.Rows[i]["density"].ToString(), out num) ? num : -0.19880205;
                double specificGravity = double.TryParse(dtRST.Rows[i]["specificGravity"].ToString(), out num) ? num : -0.19880205;
                double voidRatio = double.TryParse(dtRST.Rows[i]["voidRatio"].ToString(), out num) ? num : -0.19880205;
                double saturation = double.TryParse(dtRST.Rows[i]["saturation"].ToString(), out num) ? num : -0.19880205;
                double liquidLimit = double.TryParse(dtRST.Rows[i]["liquidLimit"].ToString(), out num) ? num : -0.19880205;
                double plasticLimit = double.TryParse(dtRST.Rows[i]["plasticLimit"].ToString(), out num) ? num : -0.19880205;
                double plasticIndex = double.TryParse(dtRST.Rows[i]["plasticIndex"].ToString(), out num) ? num : -0.19880205;
                double liquidityIndex = double.TryParse(dtRST.Rows[i]["liquidityIndex"].ToString(), out num) ? num : -0.19880205;
                double compressibility = double.TryParse(dtRST.Rows[i]["compressibility"].ToString(), out num) ? num : -0.19880205;
                double modulus = double.TryParse(dtRST.Rows[i]["modulus"].ToString(), out num) ? num : -0.19880205;
                double frictionAngle = double.TryParse(dtRST.Rows[i]["frictionAngle"].ToString(), out num) ? num : -0.19880205;
                double cohesion = double.TryParse(dtRST.Rows[i]["cohesion"].ToString(), out num) ? num : -0.19880205;
                double permeability = double.TryParse(dtRST.Rows[i]["permeability"].ToString(), out num) ? num : -0.19880205;
                RoutineSoilTest rst = new RoutineSoilTest(zkNumber, sampleDepth, sampleLayer, waterLevel, density, specificGravity, voidRatio, saturation, liquidLimit, plasticLimit, plasticIndex, liquidityIndex, compressibility, modulus, frictionAngle, cohesion, permeability);
                rsts.Add(rst);
            }

            // 保存试验数据到数据库
            GC.Collect();
            GC.WaitForPendingFinalizers();
            RoutineSoilTestDataBase.Refresh(Program.currentProject, rsts);
        }

        // 点击"保存"
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // 恢复筛选状态
            this.SelectByZkComboBox.SelectedIndex = 0;
            this.SelectByLayerComboBox.SelectedIndex = 0;

            // 清除空数据行
            dtRST = DtOperation.RemoveEmptyRow(dtRST);

            // 检查数据合法性并保存
            if (CanSave())
            {
                // 计算取样所属分层
                CalcuSampleLayer();

                // 保存
                Save();

                // 更新导航树
                if (!MainWindow.bind.IsExistSecondTreeItem(3, "土工常规"))
                {
                    MainWindow.bind.AddItemToSecondTree(3, "土工常规");

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
        private void RoutineSoilTestDataGrid_KeyDown(object sender, KeyEventArgs e)
        {
            // 如果不处于编辑状态则不相应键盘操作
            if (!isEditing)
            {
                return;
            }

            // 判断按下的是否为CTRL+V
            if(ModifierKeys.Control==Keyboard.Modifiers && e.Key == Key.V)
            {
                // 获取Excel复制的数据，数据为空时退出函数
                List<string[]> excelData = OfficeOperation.GetDataFromExcelClipBoard();
                if (excelData == null)
                {
                    return;
                }

                // 获取当前选中的DataGrid行号和列号
                int row, column;
                if (this.RoutineSoilTestDataGrid.SelectedCells.Count == 0)
                {
                    row = 0;
                    column = 0;
                }
                else
                {
                    row = this.RoutineSoilTestDataGrid.Items.IndexOf(this.RoutineSoilTestDataGrid.SelectedCells[0].Item);
                    column = this.RoutineSoilTestDataGrid.SelectedCells[0].Column.DisplayIndex;
                }

                // 将复制的数据添加到绑定的DataTable
                dtRST = DtOperation.PasteFromExcel(dtRST, row, column, excelData, 2);
                
            }
        }

        #endregion

    }
}
