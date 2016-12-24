using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSYGeo
{
    /// <summary>
    /// 查看摩阻力明细的单元类
    /// </summary>
    public class StatisticPs
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_zkNumber">钻孔编号</param>
        /// <param name="_psDepth">摩阻力的深度</param>
        /// <param name="_psValue">摩阻力的值</param>
        public StatisticPs(string _zkNumber,double _psDepth,double _psValue)
        {
            JkNumber = _zkNumber;
            PsDepth = _psDepth;
            PsValue = _psValue;
        }

        /// <summary>
        /// 钻孔编号
        /// </summary>
        public string JkNumber { get; set; }

        /// <summary>
        /// 摩阻力的深度
        /// </summary>
        public double PsDepth { get; set; }

        /// <summary>
        /// 摩阻力的值
        /// </summary>
        public double PsValue { get; set; }
    }
}
