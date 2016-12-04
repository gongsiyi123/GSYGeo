using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSYGeo
{
    // 钻孔取样类
    public class ZkSample
    {
        // 带参数的构造函数
        public ZkSample(string _name,double _depth,bool _isDisturbed)
        {
            this.name = _name;
            this.depth = _depth;
            this.isDisturbed = _isDisturbed;
        }

        // 属性，取样名称
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

        // 属性，取样深度
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

        // 属性，取样类型
        private bool isDisturbed;
        public bool IsDisturbed
        {
            get
            {
                return isDisturbed;
            }
            set
            {
                isDisturbed = value;
            }
        }
    }
}
