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

        #endregion

        #region 构造函数

        // 无参数的构造函数
        public RoutineSoilTestControl()
        {
            InitializeComponent();

            // 初始化DataTable
            InitialTestDataListDataGrid();

            // 定义各输入框的工具提示

            // 设置绑定
            this.RoutineSoilTestDataGrid.DataContext = dtRST;
        }

        // 带参数的构造函数
        public RoutineSoilTestControl(List<RoutineSoilTest> _rsts)
        {
            InitializeComponent();

            // 初始化DataTable
            InitialTestDataListDataGrid(_rsts);

            // 定义各输入框的工具提示

            // 设置绑定
            this.RoutineSoilTestDataGrid.DataContext = dtRST;
        }

        #endregion

        #region 试验数据

        // 定义试验项目
        private string[] rstName = new string[16]
        {
            "zkNumber","sampleDepth","WaterLevel","density","specificGravity","voidRatio",
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
                dr["sampleDepth"] = _rsts[i].sampleDepth;
                dr["WaterLevel"] = _rsts[i].waterLevel;
                dr["density"] = _rsts[i].density;
                dr["specificGravity"] = _rsts[i].specificGravity;
                dr["voidRatio"] = _rsts[i].voidRatio;
                dr["saturation"] = _rsts[i].saturation;
                dr["liquidLimit"] = _rsts[i].liquidLimit;
                dr["plasticLimit"] = _rsts[i].plasticLimit;
                dr["plasticIndex"] = _rsts[i].plasticIndex;
                dr["liquidityIndex"] = _rsts[i].liquidityIndex;
                dr["compressibility"] = _rsts[i].compressibility;
                dr["modulus"] = _rsts[i].modulus;
                dr["frictionAngle"] = _rsts[i].frictionAngle;
                dr["cohesion"] = _rsts[i].cohesion;
                dr["permeability"] = _rsts[i].permeability;
                dtRST.Rows.Add(dr);
            }
        }

        #endregion

        #region 筛选和清空



        #endregion

        #region 保存

        // 检查保存合法性函数
        private bool CanSave()
        {
            for(int i = 0; i < dtRST.Rows.Count; i++)
            {
                for(int j = 0; j < 16; j++)
                {
                    if (j == 0)
                    {
                        string zkName = dtRST.Rows[i][j].ToString();
                        if (!BoreholeDataBase.ReadZkList(Program.currentProject).Contains(zkName))
                        {
                            MessageBox.Show("第" + i + "行的取样孔号 " + zkName + " 无法在钻孔数据库中找到，请核实");
                            return false;
                        }
                    }
                    else if (j == 1)
                    {
                        double num;
                        string dep = dtRST.Rows[i][j].ToString();
                        if (string.IsNullOrEmpty(dep) || string.IsNullOrWhiteSpace(dep))
                        {
                            MessageBox.Show("第" + i + "行的取样深度 " + dep + " 是空值，取样深度不能为空");
                            return false;
                        }
                        else if (!double.TryParse(dep,out num))
                        {
                            MessageBox.Show("第" + i + "行的取样深度 " + dep + " 不是有效数字");
                            return false;
                        }
                    }
                    else
                    {
                        double num;
                        string data = dtRST.Rows[i][j].ToString();
                        if(!string.IsNullOrEmpty(data) && !string.IsNullOrWhiteSpace(data) && !double.TryParse(data,out num))
                        {
                            MessageBox.Show("第" + i + "行的 " + this.RoutineSoilTestDataGrid.Columns[j].Header + " " + data + " 不是有效数字");
                            return false;
                        }
                    }
                }
            }
            MessageBox.Show("全部数据合法");
            return true;
        }

        // 点击"保存"
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (CanSave())
            {
                // 提取参数
                // 保存试验数据到数据库
                // 更新导航树
                // 成功提示
            }
        }

        #endregion
    }
}
