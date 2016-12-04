using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSYGeo
{
    // 静力触探类
    public class CPT
    {
        // 无参数的构造函数
        public CPT() { }

        // 带2个参数的构造函数
        public CPT(string _name,double _altitude)
        {
            name = _name;
            altitude = _altitude;
        }

        // 带3个参数的构造函数
        public CPT(string _name,double _altitude,List<double> _psList)
        {
            name = _name;
            altitude = _altitude;
            psList = _psList;
        }

        // 属性，触探孔名称
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

        // 属性，孔口高程
        private double altitude;
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

        // 属性，触探孔坐标X
        private double x;
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

        // 属性，触探孔坐标Y
        private double y;
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

        // 属性，分层列表
        private List<ZkLayer> layers;
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

        // 属性，Ps值列表
        private List<double> psList;
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

        // 方法，添加一个分层
        public void AddLayer(string _number, string _name, string _geo, string _description, double _depth)
        {
            layers.Add(new ZkLayer(_number, _name, _geo, _description, _depth));
        }

        // 方法，编辑一个分层
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

        // 方法，删除一个分层
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

        // 方法，刷新Ps值列表
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
