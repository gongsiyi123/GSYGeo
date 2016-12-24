using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSYGeo
{
    /// <summary>
    /// 常规土工试验类
    /// </summary>
    public class RoutineSoilTest
    {
        /// <summary>
        /// 无参数的构造函数
        /// </summary>
        public RoutineSoilTest() { }

        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        /// <param name="_zkNumber">钻孔编号</param>
        /// <param name="_sampleDepth">取样深度</param>
        /// <param name="_sampleLayer">取样所属分层</param>
        /// <param name="_waterLevel">含水量</param>
        /// <param name="_density">天然密度</param>
        /// <param name="_specificGravity">比重</param>
        /// <param name="_voidRatio">孔隙比</param>
        /// <param name="_saturation">饱和度</param>
        /// <param name="_liquidLimit">液限</param>
        /// <param name="_plasticLimit">塑限</param>
        /// <param name="_plasticIndex">塑性指数</param>
        /// <param name="_liquidityIndex">液性指数</param>
        /// <param name="_compressibility">压缩系数</param>
        /// <param name="_modulus">压缩模量</param>
        /// <param name="_frictionAngle">内摩擦角</param>
        /// <param name="_cohesion">粘聚力</param>
        /// <param name="_permeability">渗透系数</param>
        public RoutineSoilTest(string _zkNumber,double _sampleDepth,string _sampleLayer,double _waterLevel,double _density,double _specificGravity,double _voidRatio,double _saturation,double _liquidLimit,double _plasticLimit,double _plasticIndex,double _liquidityIndex,double _compressibility,double _modulus,double _frictionAngle,double _cohesion,double _permeability)
        {
            zkNumber = _zkNumber;
            sampleDepth = _sampleDepth;
            sampleLayer = _sampleLayer;
            waterLevel = _waterLevel;
            density = _density;
            specificGravity = _specificGravity;
            voidRatio = _voidRatio;
            saturation = _saturation;
            liquidLimit = _liquidLimit;
            plasticLimit = _plasticLimit;
            plasticIndex = _plasticIndex;
            liquidityIndex = _liquidityIndex;
            compressibility = _compressibility;
            modulus = _modulus;
            frictionAngle = _frictionAngle;
            cohesion = _cohesion;
            permeability = _permeability;
        }

        /// <summary>
        /// 取样孔号
        /// </summary>
        public string zkNumber { get; set; }

        /// <summary>
        /// 取样深度
        /// </summary>
        public double sampleDepth { get; set; }

        /// <summary>
        /// 取样所属分层
        /// </summary>
        public string sampleLayer { get; set; }

        /// <summary>
        /// 含水量
        /// </summary>
        public double waterLevel { get; set; }

        /// <summary>
        /// 天然密度
        /// </summary>
        public double density { get; set; }

        /// <summary>
        /// 土粒比重
        /// </summary>
        public double specificGravity { get; set; }

        /// <summary>
        /// 孔隙比
        /// </summary>
        public double voidRatio { get; set; }

        /// <summary>
        /// 饱和度
        /// </summary>
        public double saturation { get; set; }

        /// <summary>
        /// 液限
        /// </summary>
        public double liquidLimit { get; set; }

        /// <summary>
        /// 塑限
        /// </summary>
        public double plasticLimit { get; set; }

        /// <summary>
        /// 塑性指数
        /// </summary>
        public double plasticIndex { get; set; }

        /// <summary>
        /// 液性指数
        /// </summary>
        public double liquidityIndex { get; set; }

        /// <summary>
        /// 压缩系数
        /// </summary>
        public double compressibility { get; set; }

        /// <summary>
        /// 压缩模量
        /// </summary>
        public double modulus { get; set; }

        /// <summary>
        /// 内摩擦角
        /// </summary>
        public double frictionAngle { get; set; }

        /// <summary>
        /// 粘聚力
        /// </summary>
        public double cohesion { get; set; }

        /// <summary>
        /// 渗透系数
        /// </summary>
        public double permeability { get; set; }

    }
}
