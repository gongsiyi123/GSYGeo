using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSYGeo
{
    // 钻孔类
    public class Borehole
    {
        // 无参数的构造函数
        public Borehole() { }

        // 带参数的构造函数
        public Borehole(string _name,double _altitude)
        {
            name = _name;
            altitude = _altitude;
        }

        // 属性，钻孔名称
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

        // 属性，钻孔坐标X
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

        // 属性，钻孔坐标Y
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

        // 属性，初见水位
        private double initialWaterLevel;
        public double InitialWaterLevel
        {
            get
            {
                return initialWaterLevel;
            }
            set
            {
                initialWaterLevel = value;
            }
        }

        // 属性，稳定水位
        private double stableWaterLevel;
        public double StableWaterLevel
        {
            get
            {
                return stableWaterLevel;
            }
            set
            {
                stableWaterLevel = value;
            }
        }

        // 属性，分层列表
        public List<ZkLayer> Layers { get; set; }
        
        // 属性，取样列表
        public List<ZkSample> Samples { get; set; }
        
        // 属性，标贯动探列表
        public List<ZkNTest> NTests { get; set; }
        
        // 方法，添加一个分层
        public void AddLayer(string _number,string _name,string _geo,string _description,double _depth)
        {
            Layers.Add(new ZkLayer(_number, _name, _geo, _description,_depth));
        }

        // 方法，编辑一个分层
        public void EditLayer(string _oldNumber,string _number, string _name, string _geo, string _description,double _depth)
        {
            if (Layers != null)
            {
                for (int i = 0; i < Layers.Count; i++)
                {
                    if (Layers[i].Number == _oldNumber)
                    {
                        Layers[i] = new ZkLayer(_number, _name, _geo, _description,_depth);
                    }
                }
            }
        }

        // 方法，删除一个分层
        public void DeleteLayer(string _number)
        {
            if (Layers != null)
            {
                for(int i = 0; i < Layers.Count; i++)
                {
                    if (Layers[i].Number == _number)
                    {
                        Layers.RemoveAt(i);
                    }
                }
            }
        }

        // 方法，添加一个取样
        public void AddSample(string _name,double _depth,bool _isDisturbed)
        {
            Samples.Add(new ZkSample(_name,_depth, _isDisturbed));
        }

        // 方法，编辑一个取样
        public void EditSample(string _oldName,string _name,double _depth,bool _isdisturbed)
        {
            if (Samples != null)
            {
                for(int i = 0; i < Samples.Count; i++)
                {
                    if (Samples[i].Name == _oldName)
                    {
                        Samples[i] = new ZkSample(_name,_depth, _isdisturbed);
                    }
                }
            }
        }

        // 方法，删除一个取样
        public void DeleteSample(string _name)
        {
            if (Samples != null)
            {
                for (int i = 0; i < Samples.Count; i++)
                {
                    if (Samples[i].Name == _name)
                    {
                        Samples.RemoveAt(i);
                    }
                }
            }
        }

        // 方法，添加一个标贯动探
        public void AddNTest(string _zkNumber,string _name,double _depth,double _value,ZkNTest.ntype _type)
        {
            NTests.Add(new ZkNTest(_zkNumber, _name, _depth, _value, _type));
        }

        // 方法，编辑一个标贯动探
        public void EditNTest(string _zkNumber,string _oldName,string _name,double _depth,double _value,ZkNTest.ntype _type)
        {
            if (NTests != null)
            {
                for(int i = 0; i < NTests.Count; i++)
                {
                    if (NTests[i].Name == _oldName)
                    {
                        NTests[i] = new ZkNTest(_zkNumber, _name, _depth, _value, _type);
                    }
                }
            }
        }

        // 方法，删除一个标贯动探
        public void DeleteNTest(string _name)
        {
            if (NTests != null)
            {
                for (int i = 0; i < NTests.Count; i++)
                {
                    if (NTests[i].Name == _name)
                    {
                        NTests.RemoveAt(i);
                    }
                }
            }
        }
    }
}
