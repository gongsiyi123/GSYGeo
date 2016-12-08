using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSYGeo
{
    // 常规土工试验类
    public class RoutineSoilTest
    {
        // 无参数的构造函数
        public RoutineSoilTest() { }

        // 带参数的构造函数
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

        // 属性，取样孔号
        public string zkNumber { get; set; }

        // 属性，取样深度
        public double sampleDepth { get; set; }

        // 属性，取样所属分层
        public string sampleLayer { get; set; }
         
        // 属性，含水量
        public double waterLevel { get; set; }

        // 属性，天然密度
        public double density { get; set; }

        // 属性，土粒比重
        public double specificGravity { get; set; }

        // 属性，孔隙比
        public double voidRatio { get; set; }

        // 属性，饱和度
        public double saturation { get; set; }

        // 属性，液限
        public double liquidLimit { get; set; }

        // 属性，塑限
        public double plasticLimit { get; set; }

        // 属性，塑性指数
        public double plasticIndex { get; set; }

        // 属性，液性指数
        public double liquidityIndex { get; set; }

        // 属性，压缩系数
        public double compressibility { get; set; }

        // 属性，压缩模量
        public double modulus { get; set; }

        // 属性，内摩擦角
        public double frictionAngle { get; set; }

        // 属性，粘聚力
        public double cohesion { get; set; }

        // 属性，渗透系数
        public double permeability { get; set; }

    }
}
