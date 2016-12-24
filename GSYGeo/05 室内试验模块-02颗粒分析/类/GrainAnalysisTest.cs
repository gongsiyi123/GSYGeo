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
        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        /// <param name="_zkNumber">钻孔编号</param>
        /// <param name="_sampleDepth">取样深度</param>
        /// <param name="_sampleLayer">取样所属分层</param>
        /// <param name="_Group0To0_075">小于0.075粒组含量</param>
        /// <param name="_Group0_075To0_25">0.075~0.25粒组含量</param>
        /// <param name="_Group0_25To0_5">0.25~0.5粒组含量</param>
        /// <param name="_Group0_5To2">0.5~2粒组含量</param>
        /// <param name="_Group2To20">2~20粒组含量</param>
        /// <param name="_Group20ToMax">大于20粒组含量</param>
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
        /// 小于0.075含量
        /// </summary>
        public double Group0To0_075 { get; set; }

        /// <summary>
        /// 0.075~0.25含量
        /// </summary>
        public double Group0_075To0_25 { get; set; }

        /// <summary>
        /// 0.25~0.5含量
        /// </summary>
        public double Group0_25To0_5 { get; set; }

        /// <summary>
        /// 0.5~2含量
        /// </summary>
        public double Group0_5To2 { get; set; }

        /// <summary>
        /// 2~20含量
        /// </summary>
        public double Group2To20 { get; set; }

        /// <summary>
        /// 大于20含量
        /// </summary>
        public double Group20ToMax { get; set; }

        /// <summary>
        /// 小于0.075累积含量
        /// </summary>
        public double CumulativeGroup0_075
        {
            get
            {
                return Group0To0_075;
            }
            set { }
        }

        // 小于0.25累积5含量
        public double CumulativeGroup0_25
        {
            get
            {
                return Group0To0_075 + Group0_075To0_25;
            }
            set { }
        }


        /// <summary>
        /// 小于0.5累积含量
        /// </summary>
        public double CumulativeGroup0_5
        {
            get
            {
                return Group0To0_075 + Group0_075To0_25 + Group0_25To0_5;
            }
            set { }
        }

        /// <summary>
        /// 小于2累积含量
        /// </summary>
        public double CumulativeGroup2
        {
            get
            {
                return Group0To0_075 + Group0_075To0_25 + Group0_25To0_5 + Group0_5To2;
            }
            set { }
        }

        /// <summary>
        /// 小于20累积含量
        /// </summary>
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

        /// <summary>
        /// 小于100累积含量
        /// </summary>
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
