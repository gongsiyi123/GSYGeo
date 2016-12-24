using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSYGeo
{
    /// <summary>
    /// 钻孔取样类
    /// </summary>
    public class ZkSample
    {
        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        /// <param name="_name">取样编号</param>
        /// <param name="_depth">取样深度</param>
        /// <param name="_isDisturbed">取样类型</param>
        public ZkSample(string _name,double _depth,bool _isDisturbed)
        {
            this.name = _name;
            this.depth = _depth;
            this.isDisturbed = _isDisturbed;
        }

        /// <summary>
        /// 取样名称
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
        /// 取样深度
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
        /// 取样类型
        /// </summary>
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
        private bool isDisturbed;
    }
}
