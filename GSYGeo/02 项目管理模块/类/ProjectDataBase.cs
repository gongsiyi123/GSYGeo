﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace GSYGeo
{
    /// <summary>
    /// 操作项目数据库的类
    /// </summary>
    class ProjectDataBase
    {
        /// <summary>
        /// 查询项目文件夹中的所有项目数据库文件，返回项目名称字符串列表
        /// </summary>
        /// <returns></returns>
        public static List<string> ReadProjectList()
        {
            List<string> projectList = new List<string>();
            string path = Program.ReadProgramPath();
            DirectoryInfo folder = new DirectoryInfo(path);
            foreach(FileInfo file in folder.GetFiles("*.gsygeo"))
            {
                string s = file.Name.Remove(file.Name.Count() - 7);
                if (s != "设置信息" && !string.IsNullOrEmpty(s) && !string.IsNullOrWhiteSpace(s))
                {
                    projectList.Add(file.Name.Remove(file.Name.Count() - 7));
                }
            }
            return projectList;
        }

        /// <summary>
        /// 清理名称为空的项目数据库文件
        /// </summary>
        public static void ClearEmpthProject()
        {
            List<string> projectList = new List<string>();
            string path = Program.ReadProgramPath();
            DirectoryInfo folder = new DirectoryInfo(path);
            foreach (FileInfo file in folder.GetFiles("*.gsygeo"))
            {
                string s = file.Name.Remove(file.Name.Count() - 7);
                if (string.IsNullOrEmpty(s) || string.IsNullOrWhiteSpace(s))
                    file.Delete();
            }
        }

        /// <summary>
        /// 创建项目数据库，并初始化
        /// </summary>
        /// <param name="_projectName">项目名称</param>
        public static void Create(string _projectName)
        {
            SQLiteConnection.CreateFile(Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo");
            Initial(_projectName);
        }

        /// <summary>
        /// 初始化项目数据库，创建基本表结构
        /// </summary>
        /// <param name="_projectName">项目名称</param>
        private static void Initial(string _projectName)
        {
            // 创建连接到项目信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 新建项目基本信息表projectBasicInfo 设置 项目名称 所在省份 所在县市 为空字符串
                // 表头 项目名称name 所在省份province 所在县市city
                sql = "create table projectBasicInfo(name varchar(255),province varchar(255),city varchar(255))";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();
                sql = "insert into projectBasicInfo(name,province,city) values('','','')";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();

                // 新建项目岩土分层信息表projectLayer
                // 表头 分层编号number 分层名称name 地质年代geo 分层描述description
                sql = "create table projectLayer(number varchar(255),name varchar(255),geo varchar(255),description text)";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();

                // 新建项目公司信息表projectCompany 设置初始空字符串
                // 表头 公司名称companyName 资质代码companyCode 绘图人drawer 报告编写人writer 校核人checker 审查人inspector 核定人approver 批准人finalApprover
                sql = "create table projectCompany(companyName varchar(255),companyCode varchat(255),drawer varchat(255),writer varchat(255),checker varchar(255),inspector varchar(255),approver varchar(255),finalApprover varchar(255))";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();
                sql = "insert into projectCompany(companyName,companyCode,drawer,writer,checker,inspector,approver,finalApprover) values('','','','','','','','')";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();
            }

            // 创建必要数据表
            BoreholeDataBase.Initial(_projectName);
            CPTDataBase.Initial(_projectName);
            RoutineSoilTestDataBase.Initial(_projectName);
            GrainAnalysisTestDataBase.Initial(_projectName);
        }

        /// <summary>
        /// 向项目信息表中添加 岩土分层
        /// </summary>
        /// <param name="_projectName">项目名称</param>
        /// <param name="_number">分层编号</param>
        /// <param name="_name">岩土名称</param>
        /// <param name="_geo">地质年代成因</param>
        /// <param name="_description">分层描述</param>
        public static void AddLayer(string _projectName,string _number,string _name,string _geo,string _description)
        {
            // 创建连接到项目信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 添加分层
                sql = "insert into projectLayer(number,name,geo,description) values('" + _number + "','" + _name + "','" + _geo + "','" + _description + "')";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 编辑项目基本信息
        /// </summary>
        /// <param name="_projectName">项目名称</param>
        /// <param name="_name">项目名称</param>
        /// <param name="_province">项目所在省份</param>
        /// <param name="_city">项目所在县市</param>
        public static void EditProjectBasicInfo(string _projectName,string _name,string _province,string _city)
        {
            // 创建连接到项目信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 编辑项目基本信息
                sql = "update projectBasicInfo set name='" + _name + "',province='" + _province + "',city='" + _city + "'";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 编辑项目公司信息
        /// </summary>
        /// <param name="_projectName">项目名称</param>
        /// <param name="_companyName">公司名称</param>
        /// <param name="_companyCode">公司资质代码</param>
        /// <param name="_drawer">绘图人</param>
        /// <param name="_writer">报告编写人</param>
        /// <param name="_checker">校核人</param>
        /// <param name="_inspector">审查人</param>
        /// <param name="_approver">核定人</param>
        /// <param name="_finalApprover">批准人</param>
        public static void EditProjectCompany(string _projectName,string _companyName,string _companyCode,string _drawer,string _writer,string _checker,string _inspector,string _approver,string _finalApprover)
        {
            // 创建连接到项目信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 编辑项目公司信息
                sql = "update projectCompany set companyName='" + _companyName + "',companyCode='" + _companyCode + "',drawer='" + _drawer + "',writer='" + _writer + "',checker='" + _checker + "',inspector='" + _inspector + "',approver='" + _approver + "',finalApprover='" + _finalApprover + "'";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 编辑岩土分层
        /// </summary>
        /// <param name="_projectName">项目名称</param>
        /// <param name="_oldLayerNumber">老分层编号</param>
        /// <param name="_number">新分层编号</param>
        /// <param name="_name">新岩土名称</param>
        /// <param name="_geo">新地质年代成因</param>
        /// <param name="_description">新分层描述</param>
        public static void EditLayer(string _projectName,string _oldLayerNumber,string _number,string _name,string _geo,string _description)
        {
            // 创建连接到项目信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 编辑分层
                sql = "update projectLayer set number='" + _number + "',name='" + _name + "',geo='" + _geo + "',description='" + _description + "' where number='" + _oldLayerNumber + "'";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 删除岩土分层
        /// </summary>
        /// <param name="_projectName">项目名称</param>
        /// <param name="_number">分层编号</param>
        public static void DeleteLayer(string _projectName,string _number)
        {
            // 创建连接到项目信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 删除分层
                sql = "delete from projectLayer where number='" + _number + "'";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 删除全部岩土分层
        /// </summary>
        /// <param name="_projectName">项目名称</param>
        public static void DeleteAllLayer(string _projectName)
        {
            // 创建连接到项目信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 删除分层
                sql = "delete from projectLayer";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 查询岩土分层，返回 分层编号 列表
        /// </summary>
        /// <param name="_projectName">项目名称</param>
        /// <returns></returns>
        public static List<string> ReadLayerNumberList(string _projectName)
        {
            // 创建连接到项目信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 查询分层
                sql = "select number from projectLayer order by number * 1";
                SQLiteDataReader reader = new SQLiteCommand(sql, conn).ExecuteReader();
                List<string> layerList = new List<string>();
                while (reader.Read())
                {
                    layerList.Add(reader["number"].ToString());
                }
                return layerList;
            }
        }

        /// <summary>
        /// 查询岩土分层，返回 分层名称 列表
        /// </summary>
        /// <param name="_projectName">项目名称</param>
        /// <returns></returns>
        public static List<string> ReadLayerNameList(string _projectName)
        {
            // 创建连接到项目信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 查询分层
                sql = "select name from projectLayer order by number * 1";
                SQLiteDataReader reader = new SQLiteCommand(sql, conn).ExecuteReader();
                List<string> layerList = new List<string>();
                while (reader.Read())
                {
                    layerList.Add(reader["name"].ToString());
                }
                return layerList;
            }
        }

        /// <summary>
        /// 查询岩土分层，返回 分层年代成因 列表description
        /// </summary>
        /// <param name="_projectName">项目名称</param>
        /// <returns></returns>
        public static List<string> ReadLayerGeoList(string _projectName)
        {
            // 创建连接到项目信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 查询分层
                sql = "select geo from projectLayer order by number * 1";
                SQLiteDataReader reader = new SQLiteCommand(sql, conn).ExecuteReader();
                List<string> layerList = new List<string>();
                while (reader.Read())
                {
                    layerList.Add(reader["geo"].ToString());
                }
                return layerList;
            }
        }

        /// <summary>
        /// 查询岩土分层，返回 分层描述 列表
        /// </summary>
        /// <param name="_projectName">项目名称</param>
        /// <returns></returns>
        public static List<string> ReadLayerDescriptionList(string _projectName)
        {
            // 创建连接到项目信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 查询分层
                sql = "select description from projectLayer order by number * 1";
                SQLiteDataReader reader = new SQLiteCommand(sql, conn).ExecuteReader();
                List<string> layerList = new List<string>();
                while (reader.Read())
                {
                    layerList.Add(reader["description"].ToString());
                }
                return layerList;
            }
        }

        /// <summary>
        /// 查询项目名称
        /// </summary>
        /// <param name="_projectName">项目名称</param>
        /// <returns></returns>
        public static string ReadProjectName(string _projectName)
        {
            // 创建连接到项目信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 查询
                sql = "select name from projectBasicInfo";
                SQLiteDataReader reader = new SQLiteCommand(sql, conn).ExecuteReader();
                return reader.Read().ToString();
            }
        }

        /// <summary>
        /// 查询项目所在省份
        /// </summary>
        /// <param name="_projectName">项目名称</param>
        /// <returns></returns>
        public static string ReadProjectProvince(string _projectName)
        {
            // 创建连接到项目信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 查询
                sql = "select province from projectBasicInfo";
                SQLiteDataReader reader = new SQLiteCommand(sql, conn).ExecuteReader();
                reader.Read();
                return reader["province"].ToString();
            }
        }

        /// <summary>
        /// 查询项目所在县市
        /// </summary>
        /// <param name="_projectName">项目名称</param>
        /// <returns></returns>
        public static string ReadProjectCity(string _projectName)
        {
            // 创建连接到项目信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 查询
                sql = "select city from projectBasicInfo";
                SQLiteDataReader reader = new SQLiteCommand(sql, conn).ExecuteReader();
                reader.Read();
                return reader["city"].ToString();
            }
        }

        /// <summary>
        /// 查询项目公司信息
        /// </summary>
        /// <param name="_projectName">项目名称</param>
        /// <returns></returns>
        public static string[] ReadProjectCompany(string _projectName)
        {
            // 创建连接到项目信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 查询
                sql = "select * from projectCompany";
                SQLiteDataReader reader = new SQLiteCommand(sql, conn).ExecuteReader();
                reader.Read();
                string companyName = reader["companyName"].ToString();
                string companyCode = reader["companyCode"].ToString();
                string drawer = reader["drawer"].ToString();
                string writer = reader["writer"].ToString();
                string checker = reader["checker"].ToString();
                string inspector = reader["inspector"].ToString();
                string approver = reader["approver"].ToString();
                string finalApprover = reader["finalApprover"].ToString();
                string[] company = new string[8] { companyName, companyCode, drawer, writer, checker, inspector, approver, finalApprover };
                return company;
            }
        }
    }
}
