using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSYGeo
{
    /// <summary>
    /// 剖面图高程点
    /// </summary>
    public class ProfileAltitudePoint
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_number">高程点编号</param>
        /// <param name="_xAxis">高程点X坐标</param>
        /// <param name="_yAxis">高程点Y坐标</param>
        /// <param name="_zkNumber">当前点的钻孔（无钻孔时存储空字符）</param>
        public ProfileAltitudePoint(int _number,double _xAxis,double _yAxis,string _zkNumber)
        {
            Number = _number;
            X = _xAxis;
            Y = _yAxis;
            ZkNumber = _zkNumber;
        }

        /// <summary>
        /// 高程点编号
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// 高程点X坐标
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// 高程点Y坐标
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// 当前点的钻孔（无钻孔时存储空字符）
        /// </summary>
        public string ZkNumber { get; set; }
    }
}
