using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSYGeo
{
    /// <summary>
    /// 统计颗粒分析试验的类
    /// </summary>
    public class StatisticGAT
    {
        /// <summary>
        /// 构造函数，输入分层编号、分层岩土名称、颗分数据列表
        /// </summary>
        /// <param name="_layer">分层编号</param>
        /// <param name="_name">分层岩土名称</param>
        /// <param name="_gats">颗分数据列表</param>
        public StatisticGAT(string _layer,string _name,List<GrainAnalysisTest> _gats)
        {
            // 赋值分层编号和分层岩土名称
            Layer = _layer;
            Name = _name;

            // 赋值各累计含量百分比
            double sumLessThan100 = 0, sumLessThan20 = 0, sumLessThan2 = 0, sumLessThan0_5 = 0, sumLessThan0_25 = 0, sumLessThan0_075 = 0;
            for(int i = 0; i < _gats.Count; i++)
            {
                sumLessThan100 += _gats[i].CumulativeGroup100;
                sumLessThan20 += _gats[i].CumulativeGroup20;
                sumLessThan2 += _gats[i].CumulativeGroup2;
                sumLessThan0_5 += _gats[i].CumulativeGroup0_5;
                sumLessThan0_25 += _gats[i].CumulativeGroup0_25;
                sumLessThan0_075 += _gats[i].CumulativeGroup0_075;
            }
            GroupLessThan100 = sumLessThan100 / _gats.Count;
            GroupLessThan20 = sumLessThan20 / _gats.Count;
            GroupLessThan2 = sumLessThan2 / _gats.Count;
            GroupLessThan0_5 = sumLessThan0_5 / _gats.Count;
            GroupLessThan0_25 = sumLessThan0_25 / _gats.Count;
            GroupLessThan0_075 = sumLessThan0_075 / _gats.Count;

            // 赋值各粒组含量百分比
            for(int i = 0; i < _gats.Count; i++)
            {
                Group100To20 = _gats[i].Group20ToMax;
                Group20To2 = _gats[i].Group2To20;
                Group2To0_5 = _gats[i].Group0_5To2;
                Group0_5To0_25 = _gats[i].Group0_25To0_5;
                Group0_25To0_075 = _gats[i].Group0_075To0_25;
                Group0_075To0 = _gats[i].Group0To0_075;
            }

            // 计算定名
            if (100 - GroupLessThan20 > 50)
                SoilType = "卵石、碎石";
            else if (100 - GroupLessThan2 > 50)
                SoilType = "砾石";
            else if (100 - GroupLessThan2 > 25)
                SoilType = "砾砂";
            else if (100 - GroupLessThan0_5 > 50)
                SoilType = "粗砂";
            else if (100 - GroupLessThan0_25 > 50)
                SoilType = "中砂";
            else if (100 - GroupLessThan0_075 > 85)
                SoilType = "细砂";
            else if (100 - GroupLessThan0_075 > 50)
                SoilType = "粉砂";
            else
                SoilType = "黏性土";
        }

        /// <summary>
        /// 属性，所属分层编号
        /// </summary>
        public string Layer { get; set; }

        /// <summary>
        /// 属性，所属分层岩土名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 属性，根据颗粒分布的定名
        /// </summary>
        public string SoilType { get; set; }

        /// <summary>
        /// 属性，累积含量小于100mm的颗粒含量百分比
        /// </summary>
        public double GroupLessThan100 { get; set; }

        /// <summary>
        /// 属性，累积含量小于20mm的颗粒含量百分比
        /// </summary>
        public double GroupLessThan20 { get; set; }

        /// <summary>
        /// 属性，累积含量小于2的颗粒含量百分比
        /// </summary>
        public double GroupLessThan2 { get; set; }

        /// <summary>
        /// 属性，累积含量小于0.5mm的颗粒含量百分比
        /// </summary>
        public double GroupLessThan0_5 { get; set; }

        /// <summary>
        /// 属性，累积含量小于0.25mm的颗粒含量百分比
        /// </summary>
        public double GroupLessThan0_25 { get; set; }

        /// <summary>
        /// 属性，累积含量小于0.075mm的颗粒含量百分比
        /// </summary>
        public double GroupLessThan0_075 { get; set; }

        /// <summary>
        /// 属性，大于20mm的颗粒含量百分比
        /// </summary>
        public double Group100To20 { get; set; }

        /// <summary>
        /// 属性，20~2mm的颗粒含量百分比
        /// </summary>
        public double Group20To2 { get; set; }

        /// <summary>
        /// 属性，2~0.5mm的颗粒含量百分比
        /// </summary>
        public double Group2To0_5 { get; set; }

        /// <summary>
        /// 属性，0.5~0.25mm的颗粒含量百分比
        /// </summary>
        public double Group0_5To0_25 { get; set; }

        /// <summary>
        /// 属性，0.25~0.075mm的颗粒含量百分比
        /// </summary>
        public double Group0_25To0_075 { get; set; }

        /// <summary>
        /// 属性，小于0.075mm的颗粒含量百分比
        /// </summary>
        public double Group0_075To0 { get; set; }
    }
}
