using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows.Controls;
using System.Collections.ObjectModel;

namespace GSYGeo
{
    // 操作静力触探数据库的类
    class CPTDataBase
    {
        // 初始化触探孔数据库，若基本表结构不存在则创建基本表结构
        public static void Initial(string _projectName)
        {
            // 创建连接到项目信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 新建触探孔基本信息表jkBasicInfo
                // 表头 触探孔编号name 孔口高程altitude X坐标xAxis Y坐标yAxis
                sql = "create table if not exists jkBasicInfo(name varchar(255),altitude double,xAxis double,yAxis double)";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();

                // 新建触探孔分层表jkLayer
                // 表头 触探孔编号jkName 分层编号number 岩土名称name 年代成因geo 地质描述description 层底深度depth
                sql = "create table if not exists jkLayer(jkName varchar(255),number varchar(255),name varchar(255),geo varchar(255),description text,depth double)";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();

                // 新建触探孔摩阻力表jkPs
                // 表头 触探孔编号jkName 摩阻力ps
                sql = "create table if not exists jkPs(jkName varchar(255),ps double)";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();
            }
        }

        // 添加一个触探孔，输入基本信息
        public static void AddJkBasicInfo(string _projectName,string _name,double _altitude,double _xAxis,double _yAxis)
        {
            // 创建连接到项目信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 添加触探孔
                sql = "insert into jkBasicInfo(name,altitude,xAxis,yAxis) values('" + _name + "'," + _altitude + "," + _xAxis + "," + _yAxis + ")";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();
            }
        }

        // 向某个触探孔添加分层列表
        public static void AddLayerListToJk(string _projectName, string _name, List<string> _numberList, List<string> _nameList, List<string> _geoList, List<string> _descriptionList, List<double> _depthList)
        {
            // 创建连接到项目信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 添加分层
                if (_numberList != null)
                {
                    for (int i = 0; i < _numberList.Count; i++)
                    {
                        sql = "insert into jkLayer(jkName,number,name,geo,description,depth) values('" + _name + "','" + _numberList[i] + "','" + _nameList[i] + "','" + _geoList[i] + "','" + _descriptionList[i] + "'," + _depthList[i] + ")";
                        new SQLiteCommand(sql, conn).ExecuteNonQuery();
                    }
                }
            }
        }

        // 向某个触探孔添加摩阻力列表
        public static void AddPsListToJk(string _projectName,string _name,List<double> _psList)
        {
            // 创建连接到项目信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 添加Ps值
                if (_psList != null)
                {
                    for(int i = 0; i < _psList.Count; i++)
                    {
                        sql = "insert into jkPs(jkName,ps) values('" + _name + "'," + _psList[i] + ")";
                        new SQLiteCommand(sql, conn).ExecuteNonQuery();
                    }
                }
            }
        }

        // 清空某个触探孔的全部数据
        public static void RemoveJk(string _projectName,string _name)
        {
            // 创建连接到项目信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 清空数据
                sql = "delete from jkBasicInfo where name='" + _name + "'";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();
                sql = "delete from jkLayer where jkName='" + _name + "'";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();
                sql = "delete from jkPs where jkName='" + _name + "'";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();
            }
        }

        // 查询触探孔编号列表
        public static List<string> ReadJkList(string _projectName)
        {
            // 创建连接到项目信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 查询
                sql = "select name from jkBasicInfo order by name";
                SQLiteDataReader reader = new SQLiteCommand(sql, conn).ExecuteReader();
                List<string> jkList = new List<string>();
                while (reader.Read())
                {
                    jkList.Add(reader["name"].ToString());
                }
                return jkList;
            }
        }

        // 查询触探孔编号列表，输出为TreeViewItem形式
        public static ObservableCollection<TreeViewItem> ReadJkListAsTreeViewItem(string _projectName)
        {
            // 创建连接到项目信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 查询
                sql = "select name from jkBasicInfo order by name";
                SQLiteDataReader reader = new SQLiteCommand(sql, conn).ExecuteReader();
                ObservableCollection<TreeViewItem> jkList = new ObservableCollection<TreeViewItem>();
                while (reader.Read())
                {
                    TreeViewItem item = new TreeViewItem();
                    item.Header = reader["name"].ToString();
                    jkList.Add(item);
                }
                return jkList;
            }
        }

        // 查询某个触探孔的孔口高程
        public static double ReadAltitude(string _projectName,string _name)
        {
            // 创建连接到项目信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 查询
                sql = "select altitude from jkBasicInfo where name='" + _name + "'";
                SQLiteDataReader reader = new SQLiteCommand(sql, conn).ExecuteReader();
                reader.Read();
                return Convert.ToDouble(reader["altitude"].ToString());
            }
        }

        // 查询某个触探孔的X坐标
        public static double ReadXAxis(string _projectName, string _name)
        {
            // 创建连接到项目信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 查询
                sql = "select xAxis from jkBasicInfo where name ='" + _name + "'";
                SQLiteDataReader reader = new SQLiteCommand(sql, conn).ExecuteReader();
                reader.Read();
                return Convert.ToDouble(reader["xAxis"].ToString());
            }
        }

        // 查询某个触探孔的Y坐标
        public static double ReadYAxis(string _projectName, string _name)
        {
            // 创建连接到项目信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 查询
                sql = "select yAxis from jkBasicInfo where name ='" + _name + "'";
                SQLiteDataReader reader = new SQLiteCommand(sql, conn).ExecuteReader();
                reader.Read();
                return Convert.ToDouble(reader["yAxis"].ToString());
            }
        }

        // 查询某个触探孔的分层列表
        public static List<ZkLayer> ReadJkLayer(string _projectName, string _name)
        {
            // 创建连接到项目信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 查询
                sql = "select number,name,geo,description,depth from jkLayer where jkName='" + _name + "'";
                SQLiteDataReader reader = new SQLiteCommand(sql, conn).ExecuteReader();
                List<ZkLayer> jkLayers = new List<ZkLayer>();
                while (reader.Read())
                {
                    string number = reader["number"].ToString();
                    string name = reader["name"].ToString();
                    string geo = reader["geo"].ToString();
                    string description = reader["description"].ToString();
                    double depth = Convert.ToDouble(reader["depth"]);
                    jkLayers.Add(new ZkLayer(number, name, geo, description, depth));
                }
                return jkLayers;
            }
        }

        // 查询某个触探孔的摩阻力列表
        public static List<double> ReadJkPs(string _projectName,string _name)
        {
            // 创建连接到项目信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 查询
                sql = "select ps from jkPs where jkName='" + _name + "'";
                SQLiteDataReader reader = new SQLiteCommand(sql, conn).ExecuteReader();
                List<double> jkPsList = new List<double>();
                while (reader.Read())
                {
                    double ps = Convert.ToDouble(reader["ps"]);
                    jkPsList.Add(ps);
                }
                return jkPsList;
            }
        }

        /// <summary>
        /// 查询某个分层的摩阻力列表
        /// </summary>
        /// <param name="_projectName">项目名称</param>
        /// <param name="_layerNumber">分层编号</param>
        /// <returns></returns>
        public static List<double> ReadLayerPs(string _projectName,string _layerNumber)
        {
            // 创建连接到项目信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 定义要返回的列表
                List<double> psList = new List<double>();

                // 读取项目分层列表
                List<string> layerList = ProjectDataBase.ReadLayerNumberList(_projectName);

                // 读取触探孔列表，在触探孔列表中循环
                List<string> jkList = ReadJkList(_projectName);
                for (int i = 0; i < jkList.Count; i++)
                {
                    // 读取该钻孔的摩阻力列表和分层列表
                    List<double> jkPsList = ReadJkPs(_projectName, jkList[i]);
                    List<ZkLayer> jkLayerList = ReadJkLayer(_projectName, jkList[i]);

                    // 计算输入的分层的深度范围
                    double oldLayerDepth, layerDepth;
                    int layerIndex = -1;
                    for(int j = 0; j < jkLayerList.Count; j++)
                    {
                        if (jkLayerList[j].Number == _layerNumber)
                        {
                            layerIndex = j;
                        }
                    }

                    if (layerIndex != -1)
                    {
                        if (layerIndex == 0)
                        {
                            oldLayerDepth = 0;
                            layerDepth = jkLayerList[layerIndex].Depth;
                        }
                        else
                        {
                            oldLayerDepth = jkLayerList[layerIndex - 1].Depth;
                            layerDepth = jkLayerList[layerIndex].Depth;
                        }
                    }
                    else
                    {
                        continue;
                    }
                    
                    // 将当前触探孔中符合条件的摩阻力赋值要返回的列表
                    for(int j = (int)(oldLayerDepth * 10); j < (int)(layerDepth * 10); j++)
                    {
                        psList.Add(jkPsList[j]);
                    }
                }

                // 返回赋值后的列表
                return psList;
            }
        }

        /// <summary>
        /// 查询某个分层的摩阻力列表，输出为StatisticPs列表形式
        /// </summary>
        /// <param name="_projectName">项目名称</param>
        /// <param name="_layerNumber">分层编号</param>
        /// <returns></returns>
        public static List<StatisticPs> ReadLayerPsAsStatePs(string _projectName,string _layerNumber)
        {
            // 创建连接到项目信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 定义要返回的列表
                List<StatisticPs> psList = new List<StatisticPs>();

                // 读取项目分层列表
                List<string> layerList = ProjectDataBase.ReadLayerNumberList(_projectName);

                // 读取触探孔列表，在触探孔列表中循环
                List<string> jkList = ReadJkList(_projectName);
                for (int i = 0; i < jkList.Count; i++)
                {
                    // 读取该钻孔的摩阻力列表和分层列表
                    List<double> jkPsList = ReadJkPs(_projectName, jkList[i]);
                    List<ZkLayer> jkLayerList = ReadJkLayer(_projectName, jkList[i]);

                    // 计算输入的分层的深度范围
                    double oldLayerDepth, layerDepth;
                    int layerIndex = -1;
                    for (int j = 0; j < jkLayerList.Count; j++)
                    {
                        if (jkLayerList[j].Number == _layerNumber)
                        {
                            layerIndex = j;
                        }
                    }
                    if (layerIndex != -1)
                    {
                        if (layerIndex == 0)
                        {
                            oldLayerDepth = 0;
                            layerDepth = jkLayerList[layerIndex].Depth;
                        }
                        else
                        {
                            oldLayerDepth = jkLayerList[layerIndex - 1].Depth;
                            layerDepth = jkLayerList[layerIndex].Depth;
                        }
                    }
                    else
                    {
                        continue;
                    }

                    // 将当前触探孔中符合条件的摩阻力赋值要返回的列表
                    for (int j = (int)(oldLayerDepth * 10) + 1; j <= (int)(layerDepth * 10); j++)
                    {
                        psList.Add(new StatisticPs(jkList[i], (double)j / 10, jkPsList[j - 1]));
                    }
                }

                // 返回赋值后的列表
                return psList;
            }
        }
    }
}
