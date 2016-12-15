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
    // 操作钻孔数据库的类
    class BoreholeDataBase
    {
        // 初始化钻孔数据库，若基本表结构不存在则创建基本表结构
        public static void Initial(string _projectName)
        {
            // 创建连接到项目信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 新建钻孔基本信息表zkBasicInfo
                // 表头 钻孔编号name 孔口高程altitude X坐标xAxis Y坐标yAxis 初见水位initialWaterLevel 稳定水位stableWaterLevel
                sql = "create table if not exists zkBasicInfo(name varchar(255),altitude double,xAxis double,yAxis double,initialWaterLevel double,stableWaterLevel double)";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();
                
                // 新建钻孔分层表zkLayer
                // 表头 钻孔编号zkName 分层编号number 岩土名称name 年代成因geo 地质描述description 层底深度depth
                sql = "create table if not exists zkLayer(zkName varchar(255),number varchar(255),name varchar(255),geo varchar(255),description text,depth double)";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();

                // 新建钻孔取样表zkSample
                // 表头 钻孔编号zkName  取样名称name 取样深度depth 取样类型isDisturbed
                sql = "create table if not exists zkSample(zkName varchar(255),name varchar(255),depth double,isDisturbed int)";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();

                // 新建钻孔标贯动探试验表zkNTest
                // 表头 钻孔编号zkName  试验名称name 试验深度depth 试验数值value 试验类型type
                sql = "create table if not exists zkNTest(zkName varchar(255),name varchar(255),depth double,value double,type varchar(255))";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();
            }
        }

        // 添加一个钻孔，输入基本信息
        public static void AddZkBasicInfo(string _projectName,string _name,double _altitude,double _xAxis,double _yAxis,double _iniWL,double _staWL)
        {
            // 创建连接到项目信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 添加钻孔
                sql = "insert into zkBasicInfo(name,altitude,xAxis,yAxis,initialWaterLevel,stableWaterLevel) values('" + _name + "'," + _altitude + "," + _xAxis + "," + _yAxis + "," + _iniWL + "," + _staWL + ")";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();
            }
        }

        // 向某个钻孔添加分层列表
        public static void AddLayerListToZk(string _projectName,string _name,List<string> _numberList,List<string> _nameList,List<string> _geoList,List<string> _descriptionList,List<double> _depthList)
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
                        sql = "insert into zkLayer(zkName,number,name,geo,description,depth) values('" + _name + "','" + _numberList[i] + "','" + _nameList[i] + "','" + _geoList[i] + "','" + _descriptionList[i] + "'," + _depthList[i] + ")";
                        new SQLiteCommand(sql, conn).ExecuteNonQuery();
                    }
                }
            }
        }

        // 向某个钻孔添加取样列表
        public static void AddSampleListToZk(string _projectName,string _name,List<string> _numberList,List<double> _depthList,List<int> _isDisturbedList)
        {
            // 创建连接到项目信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 添加取样
                if (_numberList != null)
                {
                    for(int i = 0; i < _numberList.Count; i++)
                    {
                        sql = "insert into zkSample(zkName,name,depth,isDisturbed) values('" + _name + "','" + _numberList[i] + "'," + _depthList[i] + "," + _isDisturbedList[i] + ")";
                        new SQLiteCommand(sql, conn).ExecuteNonQuery();
                    }
                }
            }
        }

        // 向某个钻孔添加标贯/动探列表
        public static void AddNTestListToZk(string _projectName,string _name,List<string> _numberList,List<double> _depthList,List<double> _valueList,List<string> _typeList)
        {
            // 创建连接到项目信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 添加标贯/动探
                if (_numberList != null)
                {
                    for(int i = 0; i < _numberList.Count; i++)
                    {
                        sql = "insert into zkNTest(zkName,name,depth,value,type) values('" + _name + "','" + _numberList[i] + "'," + _depthList[i] + "," + _valueList[i] + ",'" + _typeList[i] + "')";
                        new SQLiteCommand(sql, conn).ExecuteNonQuery();
                    }
                }
            }
        }

        // 清空某个钻孔的全部数据
        public static void RemoveZk(string _projectName,string _name)
        {
            // 创建连接到项目信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 清空数据
                sql = "delete from zkBasicInfo where name='" + _name + "'";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();
                sql = "delete from zkLayer where zkName='" + _name + "'";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();
                sql = "delete from zkSample where zkName='" + _name + "'";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();
                sql = "delete from zkNTest where zkName='" + _name + "'";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();
            }
        }

        // 查询钻孔编号列表
        public static List<string> ReadZkList(string _projectName)
        {
            // 创建连接到项目信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 查询
                sql = "select name from zkBasicInfo order by name";
                SQLiteDataReader reader = new SQLiteCommand(sql, conn).ExecuteReader();
                List<string> zkList = new List<string>();
                while (reader.Read())
                {
                    zkList.Add(reader["name"].ToString());
                }
                return zkList;
            }
        }

        // 查询钻孔编号列表，输出为TreeViewItem形式
        public static ObservableCollection<TreeViewItem> ReadZkListAsTreeViewItem(string _projectName)
        {
            // 创建连接到项目信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 查询
                sql = "select name from zkBasicInfo order by name";
                SQLiteDataReader reader = new SQLiteCommand(sql, conn).ExecuteReader();
                ObservableCollection<TreeViewItem> zkList = new ObservableCollection<TreeViewItem>();
                while (reader.Read())
                {
                    TreeViewItem item = new TreeViewItem();
                    item.Header = reader["name"].ToString();
                    zkList.Add(item);
                }
                return zkList;
            }
        }

        // 查询钻孔列表，输出为Borehole类形式
        public static List<Borehole> ReadZkListAsClass(string _projectName)
        {
            // 创建连接到设置信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 新建要返回的类列表
                List<Borehole> zklist = new List<Borehole>();

                // 循环读取钻孔数据
                sql = "select * from zkBasicInfo order by name";
                SQLiteDataReader reader = new SQLiteCommand(sql, conn).ExecuteReader();
                while (reader.Read())
                {
                    Borehole zk = new Borehole(reader["name"].ToString(), Convert.ToDouble(reader["altitude"]));
                    zk.X = Convert.ToDouble(reader["xAxis"]);
                    zk.Y = Convert.ToDouble(reader["yAxis"]);
                    zk.InitialWaterLevel = Convert.ToDouble(reader["initialWaterLevel"]);
                    zk.StableWaterLevel = Convert.ToDouble(reader["stableWaterLevel"]);
                    zk.Layers = BoreholeDataBase.ReadZkLayer(Program.currentProject, reader["name"].ToString());
                    zk.Samples = BoreholeDataBase.ReadZkSample(Program.currentProject, reader["name"].ToString());
                    zk.NTests = BoreholeDataBase.ReadZkNTest(Program.currentProject, reader["name"].ToString());

                    zklist.Add(zk);
                }
                
                // 返回
                return zklist;
            }
        }

        // 查询某个钻孔的孔口高程
        public static double ReadAltitude(string _projectName,string _name)
        {
            // 创建连接到项目信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 查询
                sql = "select altitude from zkBasicInfo where name='" + _name + "'";
                SQLiteDataReader reader = new SQLiteCommand(sql, conn).ExecuteReader();
                reader.Read();
                return Convert.ToDouble(reader["altitude"].ToString());
            }
        }

        // 查询某个钻孔的X坐标
        public static double ReadXAxis(string _projectName,string _name)
        {
            // 创建连接到项目信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 查询
                sql = "select xAxis from zkBasicInfo where name ='" + _name + "'";
                SQLiteDataReader reader = new SQLiteCommand(sql, conn).ExecuteReader();
                reader.Read();
                return Convert.ToDouble(reader["xAxis"].ToString());
            }
        }

        // 查询某个钻孔的Y坐标
        public static double ReadYAxis(string _projectName,string _name)
        {
            // 创建连接到项目信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 查询
                sql = "select yAxis from zkBasicInfo where name ='" + _name + "'";
                SQLiteDataReader reader = new SQLiteCommand(sql, conn).ExecuteReader();
                reader.Read();
                return Convert.ToDouble(reader["yAxis"].ToString());
            }
        }

        // 查询某个钻孔的初见水位
        public static double ReadInitialWaterLevel(string _projectName,string _name)
        {
            // 创建连接到项目信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 查询
                sql = "select initialWaterLevel from zkBasicInfo where name ='" + _name + "'";
                SQLiteDataReader reader = new SQLiteCommand(sql, conn).ExecuteReader();
                reader.Read();
                return Convert.ToDouble(reader["initialWaterLevel"].ToString());
            }
        }

        // 查询某个钻孔的稳定水位
        public static double ReadStableWaterLevel(string _projectName,string _name)
        {
            // 创建连接到项目信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 查询
                sql = "select stableWaterLevel from zkBasicInfo where name ='" + _name + "'";
                SQLiteDataReader reader = new SQLiteCommand(sql, conn).ExecuteReader();
                reader.Read();
                return Convert.ToDouble(reader["stableWaterLevel"].ToString());
            }
        }

        // 查询某个钻孔的分层列表
        public static List<ZkLayer> ReadZkLayer(string _projectName,string _name)
        {
            // 创建连接到项目信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 查询
                sql = "select number,name,geo,description,depth from zkLayer where zkName='" + _name + "'";
                SQLiteDataReader reader = new SQLiteCommand(sql, conn).ExecuteReader();
                List<ZkLayer> zkLayers = new List<ZkLayer>();
                while (reader.Read())
                {
                    string number = reader["number"].ToString();
                    string name = reader["name"].ToString();
                    string geo = reader["geo"].ToString();
                    string description = reader["description"].ToString();
                    double depth = Convert.ToDouble(reader["depth"]);
                    zkLayers.Add(new ZkLayer(number, name, geo, description, depth));
                }
                return zkLayers;
            }
        }

        // 查询某个钻孔的取样列表
        public static List<ZkSample> ReadZkSample(string _projectName,string _name)
        {
            // 创建连接到项目信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 查询
                sql = "select name,depth,isDisturbed from zkSample where zkName='" + _name + "'";
                SQLiteDataReader reader = new SQLiteCommand(sql, conn).ExecuteReader();
                List<ZkSample> zkSamples = new List<ZkSample>();
                while (reader.Read())
                {
                    string name = reader["name"].ToString();
                    double depth = Convert.ToDouble(reader["depth"]);
                    bool isDisturbed = Convert.ToBoolean(reader["isDisturbed"]);
                    zkSamples.Add(new ZkSample(name, depth, isDisturbed));
                }
                return zkSamples;
            }
        }

        // 查询某个钻孔的标贯/动探列表
        public static List<ZkNTest> ReadZkNTest(string _projectName,string _name)
        {
            // 创建连接到项目信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 查询
                sql = "select zkName,name,depth,value,type from zkNTest where zkName='" + _name + "'";
                SQLiteDataReader reader = new SQLiteCommand(sql, conn).ExecuteReader();
                List<ZkNTest> zkNTests = new List<ZkNTest>();
                while (reader.Read())
                {
                    string zkNumber = reader["zkName"].ToString();
                    string name = reader["name"].ToString();
                    double depth = Convert.ToDouble(reader["depth"]);
                    double value = Convert.ToDouble(reader["value"]);
                    ZkNTest.ntype type;
                    if (Enum.IsDefined(typeof(ZkNTest.ntype), reader["type"]))
                    {
                        type = (ZkNTest.ntype)Enum.Parse(typeof(ZkNTest.ntype), reader["type"].ToString());
                    }
                    else
                    {
                        type = ZkNTest.ntype.N;
                    }
                    zkNTests.Add(new ZkNTest(zkNumber, name, depth, value, type));
                }
                return zkNTests;
            }
        }

        /// <summary>
        /// 查询某个分层的标贯/动探列表
        /// </summary>
        /// <param name="_projectName">项目名称</param>
        /// <param name="_layerNumber">分层编号</param>
        /// <returns></returns>
        public static List<ZkNTest> ReadLayerNTest(string _projectName,string _layerNumber)
        {
            // 创建连接到设置信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 定义要返回的列表
                List<ZkNTest> ntestList = new List<ZkNTest>();

                // 读取项目分层列表
                List<string> layerList = ProjectDataBase.ReadLayerNumberList(_projectName);

                // 读取钻孔列表，在钻孔列表中循环
                List<string> zkList = ReadZkList(_projectName);
                for(int i = 0; i < zkList.Count; i++)
                {
                    // 读取该钻孔的标贯/动探列表和分层列表
                    List<ZkNTest> zkNtestList = ReadZkNTest(_projectName, zkList[i]);
                    List<ZkLayer> zkLayerList = ReadZkLayer(_projectName, zkList[i]);

                    // 在标贯/动探列表中循环
                    foreach(ZkNTest ntest in zkNtestList)
                    {
                        // 循环查找该标贯/动探所属的分层，并添加到要返回的列表中
                        int layerIndex = -1;
                        for (int j = 0; j < zkLayerList.Count; j++)
                        {
                            if (ntest.Depth <= zkLayerList[j].Depth)
                            {
                                layerIndex = j;
                                break;
                            }
                        }
                        if( layerIndex!=-1 && zkLayerList[layerIndex].Number == _layerNumber)
                        {
                            ntestList.Add(ntest);
                        }
                    }
                }

                // 返回赋值后的列表
                return ntestList;
            }
        }

        /// <summary>
        /// 查询某个分层的标贯/动探列表
        /// </summary>
        /// <param name="_projectName">项目名称</param>
        /// <param name="_layerNumber">分层编号</param>
        /// <param name="_type">试验类型</param>
        /// <returns></returns>
        public static List<ZkNTest> ReadLayerNTest(string _projectName, string _layerNumber,ZkNTest.ntype _type)
        {
            // 创建连接到设置信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 定义要返回的列表
                List<ZkNTest> ntestList = new List<ZkNTest>();

                // 读取项目分层列表
                List<string> layerList = ProjectDataBase.ReadLayerNumberList(_projectName);

                // 读取钻孔列表，在钻孔列表中循环
                List<string> zkList = ReadZkList(_projectName);
                for (int i = 0; i < zkList.Count; i++)
                {
                    // 读取该钻孔的标贯/动探列表和分层列表
                    List<ZkNTest> zkNtestList = ReadZkNTest(_projectName, zkList[i]);
                    List<ZkLayer> zkLayerList = ReadZkLayer(_projectName, zkList[i]);

                    // 在标贯/动探列表中循环
                    foreach (ZkNTest ntest in zkNtestList)
                    {
                        // 循环查找该标贯/动探所属的分层，查找试验类型，并将符合的数据添加到要返回的列表中
                        int layerIndex = -1;
                        for (int j = 0; j < zkLayerList.Count; j++)
                        {
                            if (ntest.Depth <= zkLayerList[j].Depth)
                            {
                                layerIndex = j;
                                break;
                            }
                        }
                        if (layerIndex != -1 && zkLayerList[layerIndex].Number == _layerNumber && ntest.Type == _type)
                        {
                            ntestList.Add(ntest);
                        }
                    }
                }

                // 返回赋值后的列表
                return ntestList;
            }
        }
    }
}
