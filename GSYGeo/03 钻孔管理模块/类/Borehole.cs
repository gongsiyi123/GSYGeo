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
        
        // 属性，取样列表
        private List<ZkSample> samples;
        public List<ZkSample> Samples
        {
            get
            {
                return samples;
            }
            set
            {
                samples = value;
            }
        }
        
        // 属性，标贯动探列表
        private List<ZkNTest> nTests;
        public List<ZkNTest> NTests
        {
            get
            {
                return nTests;
            }
            set
            {
                nTests = value;
            }
        }

        // 方法，添加一个分层
        public void AddLayer(string _number,string _name,string _geo,string _description,double _depth)
        {
            layers.Add(new ZkLayer(_number, _name, _geo, _description,_depth));
        }

        // 方法，编辑一个分层
        public void EditLayer(string _oldNumber,string _number, string _name, string _geo, string _description,double _depth)
        {
            if (layers != null)
            {
                for (int i = 0; i < layers.Count; i++)
                {
                    if (layers[i].Number == _oldNumber)
                    {
                        layers[i] = new ZkLayer(_number, _name, _geo, _description,_depth);
                    }
                }
            }
        }

        // 方法，删除一个分层
        public void DeleteLayer(string _number)
        {
            if (layers != null)
            {
                for(int i = 0; i < layers.Count; i++)
                {
                    if (layers[i].Number == _number)
                    {
                        layers.RemoveAt(i);
                    }
                }
            }
        }

        // 方法，添加一个取样
        public void AddSample(string _name,double _depth,bool _isDisturbed)
        {
            samples.Add(new ZkSample(_name,_depth, _isDisturbed));
        }

        // 方法，编辑一个取样
        public void EditSample(string _oldName,string _name,double _depth,bool _isdisturbed)
        {
            if (samples != null)
            {
                for(int i = 0; i < samples.Count; i++)
                {
                    if (samples[i].Name == _oldName)
                    {
                        samples[i] = new ZkSample(_name,_depth, _isdisturbed);
                    }
                }
            }
        }

        // 方法，删除一个取样
        public void DeleteSample(string _name)
        {
            if (samples != null)
            {
                for (int i = 0; i < samples.Count; i++)
                {
                    if (samples[i].Name == _name)
                    {
                        samples.RemoveAt(i);
                    }
                }
            }
        }

        // 方法，添加一个标贯动探
        public void AddNTest(string _name,double _depth,double _value,ZkNTest.ntype _type)
        {
            nTests.Add(new ZkNTest(_name,_depth, _value, _type));
        }

        // 方法，编辑一个标贯动探
        public void EditNTest(string _oldName,string _name,double _depth,double _value,ZkNTest.ntype _type)
        {
            if (nTests != null)
            {
                for(int i = 0; i < nTests.Count; i++)
                {
                    if (nTests[i].Name == _oldName)
                    {
                        nTests[i] = new ZkNTest(_name,_depth, _value, _type);
                    }
                }
            }
        }

        // 方法，删除一个标贯动探
        public void DeleteNTest(string _name)
        {
            if (nTests != null)
            {
                for (int i = 0; i < nTests.Count; i++)
                {
                    if (nTests[i].Name == _name)
                    {
                        nTests.RemoveAt(i);
                    }
                }
            }
        }
    }
}
