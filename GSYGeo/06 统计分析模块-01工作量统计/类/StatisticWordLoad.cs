using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSYGeo
{
    /// <summary>
    /// 统计工作量的类
    /// </summary>
    public class StatisticWordLoad
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public StatisticWordLoad()
        {

        }

        /// <summary>
        /// 属性，地形测绘
        /// </summary>
        public double Mapping { get; set; }

        /// <summary>
        /// 属性，工程地质测绘
        /// </summary>
        public double GeoMapping { get; set; }

        /// <summary>
        /// 属性，钻孔数量
        /// </summary>
        public int CountBorehole { get; set; }

        /// <summary>
        /// 属性，钻孔进尺
        /// </summary>
        public double Borehole { get; set; }

        /// <summary>
        /// 属性，钻孔封孔数量
        /// </summary>
        public int CountBoreholePacking { get; set; }

        /// <summary>
        /// 属性，钻孔封孔进尺
        /// </summary>
        public double BoreholePacking { get; set; }

        /// <summary>
        /// 属性，原状样数量
        /// </summary>
        public int UndisturbedSample { get; set; }

        /// <summary>
        /// 属性，扰动样数量
        /// </summary>
        public int DisturbedSample { get; set; }

        /// <summary>
        /// 属性，标准贯入试验数量
        /// </summary>
        public int NTestStandard { get; set; }

        /// <summary>
        /// 属性，轻型动力触探数量
        /// </summary>
        public int NTestN10 { get; set; }

        /// <summary>
        /// 属性，重型动力触探数量
        /// </summary>
        public int NTestN635 { get; set; }

        /// <summary>
        /// 属性，超重型动力触探数量
        /// </summary>
        public int NTestN120 { get; set; }

        /// <summary>
        /// 属性，静力触探数量
        /// </summary>
        public int CountCPT { get; set; }

        /// <summary>
        /// 属性，静力触探进尺
        /// </summary>
        public double CPT { get; set; }

        /// <summary>
        /// 属性，土工常规试验数量
        /// </summary>
        public int RST { get; set; }

        /// <summary>
        /// 属性，室内渗透试验数量
        /// </summary>
        public int Permeability { get; set; }

        /// <summary>
        /// 属性，颗粒分析试验数量
        /// </summary>
        public int GAT { get; set; }

        /// <summary>
        /// 属性，水质简分析数量
        /// </summary>
        public int WaterAnalysis { get; set; }

        /// <summary>
        /// 属性，天然建筑材料调查数量
        /// </summary>
        public int NBM { get; set; }
    }
}
