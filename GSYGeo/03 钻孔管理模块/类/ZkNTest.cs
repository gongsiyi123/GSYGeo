using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSYGeo
{
    /// <summary>
    /// 钻孔标贯/动探试验类
    /// </summary>
    public class ZkNTest
    {
        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        /// <param name="_zkNumber">钻孔编号</param>
        /// <param name="_name">试验编号</param>
        /// <param name="_depth">试验深度</param>
        /// <param name="_value">试验击数</param>
        /// <param name="_type">试验类型</param>
        public ZkNTest(string _zkNumber,string _name,double _depth,double _value,ntype _type)
        {
            this.zkNumber = _zkNumber;
            this.name = _name;
            this.depth = _depth;
            this.testValue = _value;
            this.type = _type;
        }

        /// <summary>
        /// 试验所属钻孔
        /// </summary>
        public string ZkNumber
        {
            get
            {
                return zkNumber;
            }
            set
            {
                zkNumber = value;
            }
        }
        private string zkNumber;

        /// <summary>
        /// 试验编号
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        private string name;

        /// <summary>
        /// 试验深度
        /// </summary>
        public double Depth
        {
            get
            {
                return depth;
            }
            set
            {
                depth = value;
            }
        }
        private double depth;

        /// <summary>
        /// 试验数值
        /// </summary>
        public double Value
        {
            get
            {
                return testValue;
            }
            set
            {
                testValue = value;
            }
        }
        private double testValue;

        /// <summary>
        /// 试验类型
        /// </summary>
        public ntype Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }
        private ntype type;

        /// <summary>
        /// 试验类型枚举
        /// </summary>
        public enum ntype { N,N10,N635,N120}
    }
}
