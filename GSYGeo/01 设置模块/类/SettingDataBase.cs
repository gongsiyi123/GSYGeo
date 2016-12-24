using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace GSYGeo
{
    /// <summary>
    /// 操作设置信息数据库的类
    /// </summary>
    class SettingDataBase
    {
        /// <summary>
        /// 创建设置信息数据库，并初始化
        /// </summary>
        public static void Create()
        {
            SQLiteConnection.CreateFile(Program.ReadProgramPath() + "\\设置信息.gsygeo");
            Initial();
        }

        /// <summary>
        /// 初始化设置信息数据库，创建基本表结构
        /// </summary>
        private static void Initial()
        {
            // 创建连接到设置信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\设置信息.gsygeo";
            using (SQLiteConnection conn=new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 新建公司信息表，设置初始 公司名称 和 公司资质代码 为空字符串
                sql = "create table if not exists company(name varchar(255),code varchar(255))";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();
                sql = "insert into company(name,code) values('','')";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();

                // 新建公司人员信息表
                sql = "create table if not exists companyPeople(name varchar(255))";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();

                // 新建比例尺表
                sql = "create table if not exists outputScale(scale double)";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();

                // 新建标准表，设置初始行业标准为水利水电，地方标准为湖北省
                sql = "create table if not exists standard(industrial varchar(255),local varchar(255))";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();
                sql = "insert into standard(industrial,local) values('WaterConservancy','Hubei')";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 向设置信息表添加 公司人员 ，输入参数为人员姓名字符串
        /// </summary>
        /// <param name="_peopleName">公司人员</param>
        public static void AddCompanyPeople(string _peopleName)
        {
            // 创建连接到设置信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\设置信息.gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 添加人员
                sql = "insert into companyPeople(name) values('" + _peopleName + "')";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 编辑公司名称
        /// </summary>
        /// <param name="_name">公司名称</param>
        public static void EditCompanyName(string _name)
        {
            // 创建连接到设置信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\设置信息.gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 编辑公司名称
                sql = "update company set name='" + _name + "'";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 编辑公司资质代码
        /// </summary>
        /// <param name="_code">公司资质代码</param>
        public static void EditCompanyCode(string _code)
        {
            // 创建连接到设置信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\设置信息.gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 编辑公司资质代码
                sql = "update company set code='" + _code + "'";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 编辑行业标准
        /// </summary>
        /// <param name="_standard">行业标准</param>
        public static void EditIndustrialStandard(string _standard)
        {
            // 创建连接到设置信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\设置信息.gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 编辑行业标准
                sql = "update standard set industrial='" + _standard + "'";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 编辑地方标准
        /// </summary>
        /// <param name="_standard">地方标准</param>
        public static void EditLocalStandard(string _standard)
        {
            // 创建连接到设置信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\设置信息.gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 编辑地方标准
                sql = "update standard set local='" + _standard + "'";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 查询行业标准
        /// </summary>
        /// <returns></returns>
        public static string ReadIndustrialStandard()
        {
            // 创建连接到设置信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\设置信息.gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 查询
                sql = "select industrial from standard";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();
                SQLiteDataReader reader = new SQLiteCommand(sql, conn).ExecuteReader();
                string standard = null;
                while (reader.Read())
                {
                    standard = reader["industrial"].ToString();
                }
                return standard;
            }
        }

        /// <summary>
        /// 查询地方标准
        /// </summary>
        /// <returns></returns>
        public static string ReadLocalStandard()
        {
            // 创建连接到设置信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\设置信息.gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 查询
                sql = "select local from standard";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();
                SQLiteDataReader reader = new SQLiteCommand(sql, conn).ExecuteReader();
                string standard = null;
                while (reader.Read())
                {
                    standard = reader["local"].ToString();
                }
                return standard;
            }
        }

        /// <summary>
        /// 编辑公司人员
        /// </summary>
        /// <param name="_oldName">旧名字</param>
        /// <param name="_name">新名字</param>
        public static void EditCompanyPeople(string _oldName,string _name)
        {
            // 创建连接到设置信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\设置信息.gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 编辑人员
                sql = "update companyPeople set name='" + _name + "' where name='" + _oldName + "'";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 删除公司人员
        /// </summary>
        /// <param name="_name">人员名字</param>
        public static void DeleteCompanyPeople(string _name)
        {
            // 创建连接到设置信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\设置信息.gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 删除人员
                sql = "delete from companyPeople where name='" + _name + "'";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 查询公司名称
        /// </summary>
        /// <returns></returns>
        public static string ReadCompanyName()
        {
            // 创建连接到设置信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\设置信息.gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 返回公司名称
                sql = "select name from company";
                SQLiteDataReader reader = new SQLiteCommand(sql, conn).ExecuteReader();
                string name=null;
                while (reader.Read())
                {
                    name = reader["name"].ToString();
                }
                return name;
            }
        }

        /// <summary>
        /// 查询公司资质代码
        /// </summary>
        /// <returns></returns>
        public static string ReadCompanyCode()
        {
            // 创建连接到设置信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\设置信息.gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 返回公司资质代码
                sql = "select code from company";
                SQLiteDataReader reader = new SQLiteCommand(sql, conn).ExecuteReader();
                string code = null;
                while (reader.Read())
                {
                    code = reader["code"].ToString();
                }
                return code;
            }
        }

        /// <summary>
        /// 查询公司人员，返回值为 公司人员 字符串列表
        /// </summary>
        /// <returns></returns>
        public static List<string> ReadCompanyPeople()
        {
            // 创建连接到设置信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\设置信息.gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 返回公司人员
                sql = "select name from companyPeople";
                SQLiteDataReader reader = new SQLiteCommand(sql, conn).ExecuteReader();
                List<string> people = new List<string>();
                while (reader.Read())
                {
                    people.Add(reader["name"].ToString());
                }
                return people;
            }
        }

        /// <summary>
        /// 查询是否存在同名人员，返回值为布尔值，true表示存在
        /// </summary>
        /// <param name="_name">人员姓名</param>
        /// <returns></returns>
        public static bool IsExistPeople(string _name)
        {
            // 创建连接到设置信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\设置信息.gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 返回布尔值
                sql = "select name from companyPeople";
                SQLiteDataReader reader = new SQLiteCommand(sql, conn).ExecuteReader();
                List<string> people = new List<string>();
                while (reader.Read())
                {
                    people.Add(reader["name"].ToString());
                }
                if (people.Contains(_name))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 向比例尺表添加比例尺
        /// </summary>
        /// <param name="_scale">比例尺</param>
        public static void AddScale(double _scale)
        {
            // 创建连接到设置信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\设置信息.gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 添加比例尺
                sql = "insert into outputScale values(" + _scale + ")";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 向比例尺表删除比例尺
        /// </summary>
        /// <param name="_scale">比例尺</param>
        public static void DeleteScale(double _scale)
        {
            // 创建连接到设置信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\设置信息.gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 删除比例尺
                sql = "delete from outputScale where scale='" + _scale + "'";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 查询比例尺列表
        /// </summary>
        /// <returns></returns>
        public static List<double> ReadOutputScale()
        {
            // 创建连接到设置信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\设置信息.gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 返回比例尺列表
                List<double> scaleList = new List<double>();

                sql = "select scale from outputScale";
                SQLiteDataReader reader = new SQLiteCommand(sql, conn).ExecuteReader();
                while (reader.Read())
                {
                    scaleList.Add(Convert.ToDouble(reader["scale"]));
                }

                return scaleList;
            }
        }
    }
}
