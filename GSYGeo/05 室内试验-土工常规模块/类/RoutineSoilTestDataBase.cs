using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace GSYGeo
{
    /// <summary>
    /// 操作常规土工试验数据库类
    /// </summary>
    public class RoutineSoilTestDataBase
    {
        // 初始化土工常规数据库，若基本表结构不存在则创建基本表结构
        public static void Initial(string _projectName)
        {
            // 创建连接到设置信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 新建土工常规表
                // 表头 取样孔号zkNumber 取样深度sampleDepth 含水量waterLevel 天然密度density 比重specificGravity 孔隙比voidRatio 饱和度saturation 液限liquidLimit 塑限plasticLimit 塑性指数plasticIndex 液性指数liquidityIndex 压缩系数compressibility 压缩模量modulus 内摩擦角frictionAngle 粘聚力cohesion 渗透系数permeability
                sql = "create table if not exists RST(zkNumber varchar(255),sampleDepth double,sampleLayer varchar(255),waterLevel double,density double,specificGravity double,voidRatio double,saturation double,liquidLimit double,plasticLimit double,plasticIndex double,liquidityIndex double,compressibility double,modulus double,frictionAngle double,cohesion double,permeability double)";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();
            }
        }

        // 重置并刷新土工常规数据库
        public static void Refresh(string _projectName,List<RoutineSoilTest> _rsts)
        {
            // 创建连接到设置信息数据库
            string sql = "Data Source=" + Program.ReadProgramPath() + "\\" + _projectName + ".gsygeo";
            using (SQLiteConnection conn = new SQLiteConnection(sql))
            {
                // 打开连接
                conn.Open();

                // 重置并刷新
                sql = "delete from RST";
                new SQLiteCommand(sql, conn).ExecuteNonQuery();
                for(int i = 0; i < _rsts.Count; i++)
                {
                    sql = "insert into RST(zkNumber,sampleDepth,sampleLayer,waterLevel,density,specificGravity,voidRatio,saturation,liquidLimit,plasticLimit,plasticIndex,liquidityIndex,compressibility,modulus,frictionAngle,cohesion,permeability) values('" + _rsts[i].zkNumber + "'," + _rsts[i].sampleDepth + ",'" + _rsts[i].sampleLayer + "'," + _rsts[i].waterLevel + "," + _rsts[i].density + "," + _rsts[i].specificGravity + "," + _rsts[i].voidRatio + "," + _rsts[i].saturation + "," + _rsts[i].liquidLimit + "," + _rsts[i].plasticLimit + "," + _rsts[i].plasticIndex + "," + _rsts[i].liquidityIndex + "," + _rsts[i].compressibility + "," + _rsts[i].modulus + "," + _rsts[i].frictionAngle + "," + _rsts[i].cohesion + "," + _rsts[i].permeability + ")";
                    new SQLiteCommand(sql, conn).ExecuteNonQuery();
                }
            }
        }
        
        // 查询全部数据
        public static List<RoutineSoilTest> ReadAllData(string _projectName)
        {
            return SelectByZkAndLayer(_projectName, "", "");
        }

        // 按钻孔和分层查询土工常规数据，若要筛选全部数据，则输入""
        public static List<RoutineSoilTest> SelectByZkAndLayer(string _projectName,string _zkNumber,string _layer)
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
                    sql = "select * from RST where zkNumber = '" + _zkNumber + "'";
                }
                else if (isZkNumberNull == true && isLayerNul == false)
                {
                    sql = "select * from RST where sampleLayer = '" + _layer + "'";
                }
                else if (isZkNumberNull == false && isLayerNul == false)
                {
                    sql = "select * from RST where zkNumber = '" + _zkNumber + "' and sampleLayer = '" + _layer + "'";
                }
                else
                {
                    sql = "select * from RST";
                }

                SQLiteDataReader reader = new SQLiteCommand(sql, conn).ExecuteReader();
                List<RoutineSoilTest> rsts = new List<RoutineSoilTest>();
                while (reader.Read())
                {
                    string zkNumber = reader["zkNumber"].ToString();
                    double sampleDepth = Convert.ToDouble(reader["sampleDepth"]);
                    string sampleLayer = reader["sampleLayer"].ToString();
                    double waterLevel = Convert.ToDouble(reader["waterLevel"]);
                    double density = Convert.ToDouble(reader["density"]);
                    double specificGravity = Convert.ToDouble(reader["specificGravity"]);
                    double voidRatio = Convert.ToDouble(reader["voidRatio"]);
                    double saturation = Convert.ToDouble(reader["saturation"]);
                    double liquidLimit = Convert.ToDouble(reader["liquidLimit"]);
                    double plasticLimit = Convert.ToDouble(reader["plasticLimit"]);
                    double plasticIndex = Convert.ToDouble(reader["plasticIndex"]);
                    double liquidityIndex = Convert.ToDouble(reader["liquidityIndex"]);
                    double compressibility = Convert.ToDouble(reader["compressibility"]);
                    double modulus = Convert.ToDouble(reader["modulus"]);
                    double frictionAngle = Convert.ToDouble(reader["frictionAngle"]);
                    double cohesion = Convert.ToDouble(reader["cohesion"]);
                    double permeability = Convert.ToDouble(reader["permeability"]);
                    RoutineSoilTest rst = new RoutineSoilTest(zkNumber, sampleDepth, sampleLayer, waterLevel, density, specificGravity, voidRatio, saturation, liquidLimit, plasticLimit, plasticIndex, liquidityIndex, compressibility, modulus, frictionAngle, cohesion, permeability);
                    rsts.Add(rst);
                }

                return rsts;
            }
        }
    }
}
