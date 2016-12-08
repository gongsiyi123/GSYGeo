using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSYGeo
{
    /// <summary>
    /// 颗粒分析试验类
    /// </summary>
    public class GrainAnalysisTest
    {
        // 带参数的构造函数
        public GrainAnalysisTest(string _zkNumber, double _sampleDepth, string _sampleLayer, double _Group0To0_075,double _Group0_075To0_25,double _Group0_25To0_5,double _Group0_5To2,double _Group2To20,double _Group20ToMax)
        {
            zkNumber = _zkNumber;
            sampleDepth = _sampleDepth;
            sampleLayer = _sampleLayer;
            Group0To0_075 = _Group0To0_075;
            Group0_075To0_25 = _Group0_075To0_25;
            Group0_25To0_5 = _Group0_25To0_5;
            Group0_5To2 = _Group0_5To2;
            Group2To20 = _Group2To20;
            Group20ToMax = _Group20ToMax;
        }

        // 属性，取样孔号
        public string zkNumber { get; set; }

        // 属性，取样深度
        public double sampleDepth { get; set; }

        // 属性，取样所属分层
        public string sampleLayer { get; set; }

        // 属性，<0.075含量
        public double Group0To0_075 { get; set; }

        // 属性，0.075~0.25含量
        public double Group0_075To0_25 { get; set; }

        // 属性，0.25~0.5含量
        public double Group0_25To0_5 { get; set; }

        // 属性，0.5~2含量
        public double Group0_5To2 { get; set; }

        // 属性，2~20含量
        public double Group2To20 { get; set; }

        // 属性，>20含量
        public double Group20ToMax { get; set; }

        // 属性，<0.075累积含量
        public double CumulativeGroup0_075
        {
            get
            {
                return Group0To0_075;
            }
            set { }
        }

        // 属性，<0.25累积5含量
        public double CumulativeGroup0_25
        {
            get
            {
                return Group0To0_075 + Group0_075To0_25;
            }
            set { }
        }


        // 属性，<0.5累积含量
        public double CumulativeGroup0_5
        {
            get
            {
                return Group0To0_075 + Group0_075To0_25 + Group0_25To0_5;
            }
            set { }
        }

        // 属性，<2累积含量
        public double CumulativeGroup2
        {
            get
            {
                return Group0To0_075 + Group0_075To0_25 + Group0_25To0_5 + Group0_5To2;
            }
            set { }
        }

        // 属性，<20累积含量
        public double CumulativeGroup20
        {
            get
            {
                return Group0To0_075 + Group0_075To0_25 + Group0_25To0_5 + Group0_5To2 + Group2To20;
            }
            set
            {

            }
        }

        // 属性，<100累积含量
        public double CumulativeGroup100
        {
            get
            {
                return Group0To0_075 + Group0_075To0_25 + Group0_25To0_5 + Group0_5To2 + Group2To20 + Group20ToMax;
            }
            set { }
        }
    }
}
