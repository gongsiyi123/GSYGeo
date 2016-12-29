using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace GSYGeo
{
    /// <summary>
    /// 操作颗粒分析试验数据库类
    /// </summary>
    public class GrainAnalysisTestDataBase
    {
        /// <summary>
        /// 初始化颗粒分析数据库，若基本表结构不存在则创建基本表结构
        /// </summary>
        /// <param name="_projectName">项目名称</param>
        public static void Initial(string _projectName)
        {
            // 创建连接到设置信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 新建颗粒分析表
                // 表头 <0.075粒组 0.075~0.25粒组 0.25~0.5粒组 0.5~2粒组 2~20粒组 >20粒组
                sql = "create table if not exists GAT(zkNumber varchar(255),sampleDepth double,sampleLayer varchar(255),group0to0075 double,group0075to025 double,group025to05 double,group05to2 double,group2to20 double,group20tomax double)";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 重置并刷新土工常规数据库
        /// </summary>
        /// <param name="_projectName">项目名称</param>
        /// <param name="_gats">颗分试验数据列表</param>
        public static void Refresh(string _projectName,List<GrainAnalysisTest> _gats)
        {
            // 创建连接到设置信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 重置并刷新
                sql = "delete from GAT";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();
                for(int i = 0; i < _gats.Count; i++)
                {
                    sql = "insert into GAT(zkNumber,sampleDepth,sampleLayer,group0to0075,group0075to025,group025to05,group05to2,group2to20,group20tomax) values('" + _gats[i].zkNumber + "'," + _gats[i].sampleDepth + ",'" + _gats[i].sampleLayer + "'," + _gats[i].Group0To0_075 + "," + _gats[i].Group0_075To0_25 + "," + _gats[i].Group0_25To0_5 + "," + _gats[i].Group0_5To2 + "," + _gats[i].Group2To20 + "," + _gats[i].Group20ToMax + ")";
                    new SQLiteCommand(sql, conn).ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// 查询全部数据
        /// </summary>
        /// <param name="_projectName">项目名称</param>
        /// <returns></returns>
        public static List<GrainAnalysisTest> ReadAllData(string _projectName)
        {
            return SelectByZkAndLayer(_projectName, "", "");
        }

        /// <summary>
        /// 按钻孔和分层查询颗粒分析数据，若要筛选全部数据，则输入""
        /// </summary>
        /// <param name="_projectName">项目名称</param>
        /// <param name="_zkNumber">钻孔编号</param>
        /// <param name="_layer">分层编号</param>
        /// <returns></returns>
        public static List<GrainAnalysisTest> SelectByZkAndLayer(string _projectName, string _zkNumber, string _layer)
        {
            // 创建连接到设置信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 查询
                bool isZkNumberNull = string.IsNullOrEmpty(_zkNumber) || string.IsNullOrWhiteSpace(_zkNumber);
                bool isLayerNul = string.IsNullOrEmpty(_layer) || string.IsNullOrWhiteSpace(_layer);
                if (isZkNumberNull == false && isLayerNul == true)
                {
                    sql = "select * from GAT where zkNumber = '" + _zkNumber + "'";
                }
                else if (isZkNumberNull == true && isLayerNul == false)
                {
                    sql = "select * from GAT where sampleLayer = '" + _layer + "'";
                }
                else if (isZkNumberNull == false && isLayerNul == false)
                {
                    sql = "select * from GAT where zkNumber = '" + _zkNumber + "' and sampleLayer = '" + _layer + "'";
                }
                else
                {
                    sql = "select * from GAT";
                }

                SQLiteDataReader reader = new SQLiteCommand(sql, conn).ExecuteReader();
                List<GrainAnalysisTest> gats = new List<GrainAnalysisTest>();
                while (reader.Read())
                {
                    string zkNumber = reader["zkNumber"].ToString();
                    double sampleDepth = Convert.ToDouble(reader["sampleDepth"]);
                    string sampleLayer = reader["sampleLayer"].ToString();
                    double group0to0075 = Convert.ToDouble(reader["group0to0075"]);
                    double group0075to025 = Convert.ToDouble(reader["group0075to025"]);
                    double group025to05 = Convert.ToDouble(reader["group025to05"]);
                    double group05to2 = Convert.ToDouble(reader["group05to2"]);
                    double group2to20 = Convert.ToDouble(reader["group2to20"]);
                    double group20tomax = Convert.ToDouble(reader["group20tomax"]);
                    
                    GrainAnalysisTest gat = new GrainAnalysisTest(zkNumber, sampleDepth, sampleLayer, group0to0075, group0075to025, group025to05, group05to2, group2to20, group20tomax);
                    gats.Add(gat);
                }

                return gats;
            }
        }
    }
}
