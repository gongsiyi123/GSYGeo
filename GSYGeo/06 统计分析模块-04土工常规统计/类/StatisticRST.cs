using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSYGeo
{
    /// <summary>
    /// 统计土工常规试验的类
    /// </summary>
    public class StatisticRST
    {
        /// <summary>
        /// 试验类型枚举
        /// </summary>
        public enum RSTType
        {
            waterLevel,
            density,
            specificGravity,
            voidRatio,
            saturation,
            liquidLimit,
            plasticLimit,
            plasticIndex,
            liquidityIndex,
            compressibility,
            modulus,
            frictionAngle,
            cohesion,
            permeability
        }

        /// <summary>
        /// 构造函数，输入层号、岩土名称、试验类型
        /// </summary>
        /// <param name="_layer">层号</param>
        /// <param name="_name">岩土名称</param>
        /// <param name="_type">试验类型</param>
        public StatisticRST(string _layer,string _name,RSTType _type)
        {
            Layer = _layer;
            Name = _name;
            Type = _type;
        }

        /// <summary>
        /// 构造函数，直接输入统计成果
        /// </summary>
        /// <param name="_layer">层号</param>
        /// <param name="_name">岩土名称</param>
        /// <param name="_type">试验类型</param>
        /// <param name="_count">统计数</param>
        /// <param name="_max">最大值</param>
        /// <param name="_min">最小值</param>
        /// <param name="_average">平均值</param>
        /// <param name="_standardDeviation">标准差</param>
        /// <param name="_variableCoefficient">变异系数</param>
        /// <param name="_correctionCoefficient">统计修正系数</param>
        /// <param name="_standardValue">标准值</param>
        public StatisticRST(string _layer, string _name, RSTType _type, int _count, double _max, double _min, double _average, double _standardDeviation, double _variableCoefficient, double _correctionCoefficient, double _standardValue)
        {
            Layer = _layer;
            Name = _name;
            Type = _type;
            Count = _count;
            Max = _max;
            Min = _min;
            Average = _average;
            StandardDeviation = _standardDeviation;
            VariableCoefficient = _variableCoefficient;
            CorrectionCoefficient = _correctionCoefficient;
            StandardValue = _standardValue;
        }

        /// <summary>
        /// 构造函数，输入原始数据
        /// </summary>
        /// <param name="_layer">层号</param>
        /// <param name="_name">岩土名称</param>
        /// <param name="_type">统计类型</param>
        /// <param name="_datalist">试验数据列表</param>
        /// <param name="_startCount">起始统计样本数</param>
        public StatisticRST(string _layer, string _name, RSTType _type, List<double> _datalist, int _startCount)
        {
            Layer = _layer;
            Name = _name;
            Type = _type;
            Count = Convert.ToInt32(Statistic.Count(_datalist));
            Max = Statistic.Max(_datalist);
            Min = Statistic.Min(_datalist);
            Average = Statistic.Average(_datalist);
            StandardDeviation = Statistic.StandardDeviation(_datalist, _startCount);
            VariableCoefficient = Statistic.VariableCoefficient(_datalist, _startCount);
            CorrectionCoefficient = Statistic.CorrectionCoefficient(_datalist, 6, false);
            StandardValue = Statistic.StandardValue(Average, CorrectionCoefficient);
        }

        /// <summary>
        /// 试验数据列表
        /// </summary>
        public List<double> DataList { get; set; }

        /// <summary>
        /// 层号
        /// </summary>
        public string Layer { get; set; }

        /// <summary>
        /// 岩土名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 统计类型
        /// </summary>
        public RSTType Type { get; set; }

        /// <summary>
        /// 统计数
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 最大值
        /// </summary>
        public double Max { get; set; }

        /// <summary>
        /// 最小值
        /// </summary>
        public double Min { get; set; }

        /// <summary>
        /// 平均值
        /// </summary>
        public double Average { get; set; }

        /// <summary>
        /// 标准差
        /// </summary>
        public double StandardDeviation { get; set; }

        /// <summary>
        /// 变异系数
        /// </summary>
        public double VariableCoefficient { get; set; }

        /// <summary>
        /// 统计修正系数
        /// </summary>
        public double CorrectionCoefficient { get; set; }

        /// <summary>
        /// 标准值
        /// </summary>
        public double StandardValue { get; set; }
    }
}
