using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSYGeo
{
    /// <summary>
    /// 静力触探类
    /// </summary>
    public class CPT
    {
        /// <summary>
        /// 无参数的构造函数
        /// </summary>
        public CPT() { }

        /// <summary>
        /// 带2个参数的构造函数
        /// </summary>
        /// <param name="_name">触探孔编号</param>
        /// <param name="_altitude">触探孔孔口高程</param>
        public CPT(string _name,double _altitude)
        {
            name = _name;
            altitude = _altitude;
        }

        /// <summary>
        /// 带3个参数的构造函数
        /// </summary>
        /// <param name="_name">触探孔编号</param>
        /// <param name="_altitude">触探孔孔口高程</param>
        /// <param name="_psList">Ps值列表</param>
        public CPT(string _name,double _altitude,List<double> _psList)
        {
            name = _name;
            altitude = _altitude;
            psList = _psList;
        }

        /// <summary>
        /// 触探孔名称
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
        /// 孔口高程
        /// </summary>
        public double Altitude
        {
            get
            {
                return altitude;
            }
            set
            {
                altitude = value;
            }
        }
        private double altitude;

        /// <summary>
        /// 触探孔坐标X
        /// </summary>
        public double X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }
        private double x;

        /// <summary>
        /// 触探孔坐标Y
        /// </summary>
        public double Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }
        private double y;

        /// <summary>
        /// 分层列表
        /// </summary>
        public List<ZkLayer> Layers
        {
            get
            {
                return layers;
            }
            set
            {
                layers = value;
            }
        }
        private List<ZkLayer> layers;

        /// <summary>
        /// Ps值列表
        /// </summary>
        public List<double> PsList
        {
            get
            {
                return psList;
            }
            set
            {
                psList = value;
            }
        }
        private List<double> psList;

        /// <summary>
        /// 添加一个分层
        /// </summary>
        /// <param name="_number">分层编号</param>
        /// <param name="_name">岩土名称</param>
        /// <param name="_geo">地质年代成因</param>
        /// <param name="_description">分层描述</param>
        /// <param name="_depth">层底深度</param>
        public void AddLayer(string _number, string _name, string _geo, string _description, double _depth)
        {
            layers.Add(new ZkLayer(_number, _name, _geo, _description, _depth));
        }

        /// <summary>
        /// 编辑一个分层
        /// </summary>
        /// <param name="_oldNumber">老分层编号</param>
        /// <param name="_number">新分层编号</param>
        /// <param name="_name">新岩土名称</param>
        /// <param name="_geo">新地质年代成因</param>
        /// <param name="_description">新分层描述</param>
        /// <param name="_depth">新层底深度</param>
        public void EditLayer(string _oldNumber, string _number, string _name, string _geo, string _description, double _depth)
        {
            if (layers != null)
            {
                for (int i = 0; i < layers.Count; i++)
                {
                    if (layers[i].Number == _oldNumber)
                    {
                        layers[i] = new ZkLayer(_number, _name, _geo, _description, _depth);
                    }
                }
            }
        }

        /// <summary>
        /// 删除一个分层
        /// </summary>
        /// <param name="_number">分层编号</param>
        public void DeleteLayer(string _number)
        {
            if (layers != null)
            {
                for (int i = 0; i < layers.Count; i++)
                {
                    if (layers[i].Number == _number)
                    {
                        layers.RemoveAt(i);
                    }
                }
            }
        }

        /// <summary>
        /// 刷新Ps值列表
        /// </summary>
        /// <param name="_newPsList">新Ps值列表</param>
        public void RefreshPsList(List<double> _newPsList)
        {
            psList.Clear();
            for(int i = 0; i < _newPsList.Count; i++)
            {
                psList.Add(_newPsList[i]);
            }
        }

    }
}
