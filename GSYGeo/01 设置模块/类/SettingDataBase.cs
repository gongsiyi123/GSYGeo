using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace GSYGeo
{
    // 操作设置信息数据库的类
    class SettingDataBase
    {
        // 创建设置信息数据库，并初始化
        public static void Create()
        {
            SQLiteConnection.CreateFile(Program.ReadProgramPath() + "\\设置信息.gsygeo");
            Initial();
        }

        // 初始化设置信息数据库，创建基本表结构
        private static void Initial()
        {
            // 创建连接到设置信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\设置信息.gsygeo";
            using (SQLiteConnection conn=new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 新建公司信息表，设置初始 公司名称 和 公司资质代码 为空字符串
                sql = "create table company(name varchar(255),code varchar(255))";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();
                sql = "insert into company(name,code) values('','')";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();

                // 新建公司人员信息表
                sql = "create table companyPeople(name varchar(255))";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();
            }
        }
        
        // 向设置信息表添加 公司人员 ，输入参数为人员姓名字符串
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

        // 编辑公司名称
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

        // 编辑公司资质代码
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

        // 编辑公司人员
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

        // 删除公司人员
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

        // 查询公司信息，返回值为 公司名称 字符串
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

        // 查询公司信息，返回值为 公司资质代码 字符串
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

        // 查询公司人员，返回值为 公司人员 字符串泛型集合
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

        // 查询是否存在同名人员，输入值为人员姓名，返回值为布尔值，true表示存在
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
    }
}
