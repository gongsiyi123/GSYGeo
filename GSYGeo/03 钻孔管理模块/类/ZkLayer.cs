using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSYGeo
{
    /// <summary>
    /// 钻孔分层类
    /// </summary>
    public class ZkLayer
    {
        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        /// <param name="_number">分层编号</param>
        /// <param name="_name">岩土名称</param>
        /// <param name="_geo">地质年代成因</param>
        /// <param name="_description">分层描述</param>
        /// <param name="_depth">层底深度</param>
        public ZkLayer(string _number, string _name, string _geo, string _description, double _depth)
        {
            this.number = _number;
            this.name = _name;
            this.geo = _geo;
            this.description = _description;
            this.depth = _depth;
        }

        /// <summary>
        /// 分层编号
        /// </summary>
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
        private string number;

        /// <summary>
        /// 岩土名称
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
        /// 年代成因
        /// </summary>
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
        private string geo;

        /// <summary>
        /// 地质描述
        /// </summary>
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
        private string description;

        /// <summary>
        /// 层底深度
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
    }
}
