using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSYGeo
{
    // 钻孔标贯/动探试验类
    public class ZkNTest
    {
        // 带参数的构造函数
        public ZkNTest(string _zkNumber,string _name,double _depth,double _value,ntype _type)
        {
            this.zkNumber = _zkNumber;
            this.name = _name;
            this.depth = _depth;
            this.testValue = _value;
            this.type = _type;
        }

        // 属性，试验所属钻孔
        private string zkNumber;
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

        // 属性，试验编号
        private string name;
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

        // 属性，试验深度
        private double depth;
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

        // 属性，试验数值
        private double testValue;
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

        // 属性，试验类型
        private ntype type;
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

        // 字段，试验类型枚举
        public enum ntype { N,N10,N635,N120}

    }
}
