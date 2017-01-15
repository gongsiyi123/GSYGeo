using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSYGeo
{
    /// <summary>
    /// 操作剖面图数据库的类
    /// </summary>
    class ProfileDataBase
    {
        /// <summary>
        /// 初始化剖面图数据表，若基本表结构不存在则创建基本表结构
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

                // 新建剖面图基本信息表
                // 表头 剖面图名称name 平面底图名称plane
                sql = "create table if not exists profileBasicInfo(name varchar(255),plane varchar(255))";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();

                // 新建剖面图高程点信息表
                // 表头 高程点编号number x坐标xAxis y坐标yAxis 高程altitude 当前点的钻孔zkNumber（无钻孔时存储空字符）
                sql = "create table if not exists profileAltitudePoint(profileName varchar(255),number int,xAxis double,yAxis double,altitude double,zkNumber varchar(255))";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 添加一个剖面图，输入基本信息
        /// </summary>
        /// <param name="_projectName">项目名称</param>
        /// <param name="_name">剖面图名称</param>
        /// <param name="_plane">平面底图名称</param>
        public static void AddProfileBasicInfo(string _projectName,string _name,string _plane)
        {
            // 创建连接到设置信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 添加剖面图
                sql = "insert into profileBasicInfo(name,plane) values('" + _name + "','" + _plane + "')";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 向某个剖面图添加高程点列表
        /// </summary>
        /// <param name="_projectName">项目名称</param>
        /// <param name="_altitudePointList">高程点类列表</param>
        public static void AddAltitudePointListToProfile(string _projectName, string _profileName, List<ProfileAltitudePoint> _altitudePointList)
        {
            // 创建连接到设置信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 添加高程点
                for(int i = 0; i < _altitudePointList.Count; i++)
                {
                    int number = _altitudePointList[i].Number;
                    double xAxis = _altitudePointList[i].X;
                    double yAxis = _altitudePointList[i].Y;
                    string zkNumber = _altitudePointList[i].ZkNumber;
                    sql = "insert into profileAltitudePoint(profileName,number,xAxis,yAxis,isZk) values('" + _profileName + "'," + number + "," + xAxis + "," + yAxis + "," + isZk + ")";
                    new SQLiteCommand(sql, conn).ExecuteNonQuery();
                }
            }
        }
        
        /// <summary>
        /// 清空某个剖面图的全部数据
        /// </summary>
        /// <param name="_projectName">项目名称</param>
        /// <param name="_name">剖面图名称</param>
        public static void RemoveProfile(string _projectName,string _name)
        {
            // 创建连接到设置信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 清空所有数据
                sql = "delete from profileBasicInfo where name='" + _name + "'";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();
                sql = "delete from profileAltitudePoint where profileName='" + _name + "'";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();
                sql = "delete from profileZkPoint where profileName='" + _name + "'";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 查询某个剖面图的平面底图
        /// </summary>
        /// <param name="_projectName">项目名称</param>
        /// <param name="_name">剖面图名称</param>
        /// <returns></returns>
        public static string ReadPlane(string _projectName,string _name)
        {
            // 创建连接到设置信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 查询
                sql = "select plane from profileBasicInfo where name='" + _name + "'";
                SQLiteDataReader reader = new SQLiteCommand(sql, conn).ExecuteReader();
                reader.Read();
                return reader["plane"].ToString();
            }
        }

        /// <summary>
        /// 查询某个剖面图的高程点列表
        /// </summary>
        /// <param name="_projectName">项目名称</param>
        /// <param name="_name">剖面图名称</param>
        /// <returns></returns>
        public static List<ProfileAltitudePoint> ReadAltitudePointList(string _projectName,string _name)
        {
            // 创建连接到设置信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 查询
                sql = "select * from profileAltitudePoint where profileName='" + _name + "'";
                SQLiteDataReader reader = new SQLiteCommand(sql, conn).ExecuteReader();
                List<ProfileAltitudePoint> aps = new List<ProfileAltitudePoint>();
                while (reader.Read())
                {
                    int number = Convert.ToInt32(reader["number"]);
                    double xAxis = Convert.ToInt32(reader["xAxis"]);
                    double yAxis = Convert.ToInt32(reader["yAxis"]);
                    string zkNumber = reader["zkNumber"].ToString();
                    aps.Add(new ProfileAltitudePoint(number, xAxis, yAxis, zkNumber));
                }
                return aps;
            }
        }
    }
}
