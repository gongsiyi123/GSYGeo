using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSYGeo
{
    // 钻孔分层类
    public class ZkLayer
    {
        // 带参数的构造函数
        public ZkLayer(string _number, string _name, string _geo, string _description, double _depth)
        {
            this.number = _number;
            this.name = _name;
            this.geo = _geo;
            this.description = _description;
            this.depth = _depth;
        }

        // 属性，分层编号
        private string number;
        public string Number
        {
            get
            {
                return number;
            }
            set
            {
                number = value;
            }
        }

        // 属性，岩土名称
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

        // 属性，年代成因
        private string geo;
        public string Geo
        {
            get
            {
                return geo;
            }
            set
            {
                geo = value;
            }
        }

        // 属性，地质描述
        private string description;
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
            }
        }

        // 属性，层底深度
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
    }
}
