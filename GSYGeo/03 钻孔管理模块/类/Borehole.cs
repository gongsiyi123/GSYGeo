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
        /// <summary>
        /// 无参数的构造函数
        /// </summary>
        public Borehole() { }

        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        /// <param name="_name">钻孔编号</param>
        /// <param name="_altitude">孔口高程</param>
        public Borehole(string _name,double _altitude)
        {
            name = _name;
            altitude = _altitude;
        }

        /// <summary>
        /// 钻孔名称
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
        /// 钻孔坐标X
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
        /// 钻孔坐标Y
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
        /// 初见水位
        /// </summary>
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
        private double initialWaterLevel;

        /// <summary>
        /// 稳定水位
        /// </summary>
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
        private double stableWaterLevel;

        /// <summary>
        /// 分层列表
        /// </summary>
        public List<ZkLayer> Layers { get; set; }

        /// <summary>
        /// 取样列表
        /// </summary>
        public List<ZkSample> Samples { get; set; }

        /// <summary>
        /// 标贯动探列表
        /// </summary>
        public List<ZkNTest> NTests { get; set; }

        /// <summary>
        /// 添加一个分层
        /// </summary>
        /// <param name="_number">分层编号</param>
        /// <param name="_name">岩土名称</param>
        /// <param name="_geo">地质年代成因</param>
        /// <param name="_description">分层描述</param>
        /// <param name="_depth">层底深度</param>
        public void AddLayer(string _number,string _name,string _geo,string _description,double _depth)
        {
            Layers.Add(new ZkLayer(_number, _name, _geo, _description,_depth));
        }

        /// <summary>
        /// 编辑一个分层
        /// </summary>
        /// <param name="_oldNumber">老分层编号</param>
        /// <param name="_number">新分层编号</param>
        /// <param name="_name">新岩土名称</param>
        /// <param name="_geo">新年代成因</param>
        /// <param name="_description">新分层描述</param>
        /// <param name="_depth">新层底深度</param>
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

        /// <summary>
        /// 删除一个分层
        /// </summary>
        /// <param name="_number">分层编号</param>
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

        /// <summary>
        /// 添加一个取样
        /// </summary>
        /// <param name="_name">取样编号</param>
        /// <param name="_depth">取样深度</param>
        /// <param name="_isDisturbed">取样类型，true为扰动样</param>
        public void AddSample(string _name,double _depth,bool _isDisturbed)
        {
            Samples.Add(new ZkSample(_name,_depth, _isDisturbed));
        }

        /// <summary>
        /// 编辑一个取样
        /// </summary>
        /// <param name="_oldName">老取样编号</param>
        /// <param name="_name">新取样编号</param>
        /// <param name="_depth">取样深度</param>
        /// <param name="_isDisturbed">取样类型，true为扰动样</param>
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

        /// <summary>
        /// 删除一个取样
        /// </summary>
        /// <param name="_name">取样编号</param>
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

        /// <summary>
        /// 添加一个标贯动探
        /// </summary>
        /// <param name="_zkNumber">钻孔编号</param>
        /// <param name="_name">试验编号</param>
        /// <param name="_depth">试验深度</param>
        /// <param name="_value">试验击数</param>
        /// <param name="_type">试验类型</param>
        public void AddNTest(string _zkNumber,string _name,double _depth,double _value,ZkNTest.ntype _type)
        {
            NTests.Add(new ZkNTest(_zkNumber, _name, _depth, _value, _type));
        }

        /// <summary>
        /// 编辑一个标贯动探
        /// </summary>
        /// <param name="_zkNumber">钻孔编号</param>
        /// <param name="_oldName">老试验编号</param>
        /// <param name="_name">新试验编号</param>
        /// <param name="_depth">新试验深度</param>
        /// <param name="_value">新试验击数</param>
        /// <param name="_type">新试验类型</param>
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

        /// <summary>
        /// 删除一个标贯动探
        /// </summary>
        /// <param name="_name">试验编号</param>
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
