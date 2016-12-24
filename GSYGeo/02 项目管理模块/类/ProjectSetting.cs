using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSYGeo
{
    /// <summary>
    /// 当前项目设置类
    /// </summary>
    class ProjectSetting
    {
        /// <summary>
        /// 无参数的构造函数
        /// </summary>
        public ProjectSetting()
        {
            name = null;
            province = null;
            city = null;
            //layerList = null;
        }

        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        /// <param name="_name">项目名称</param>
        /// <param name="_province">项目所在省份</param>
        /// <param name="_city">项目所在县市</param>
        /// <param name="_layerList">分层信息列表</param>
        public ProjectSetting(string _name,string _province,string _city,List<Layer> _layerList)
        {
            name = _name;
            province = _province;
            city = _city;
            layerList = _layerList;
        }

        /// <summary>
        /// 属性，项目名称
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
        /// 属性，所在省份
        /// </summary>
        public string Province
        {
            get
            {
                return province;
            }
            set
            {
                province = value;
            }
        }
        private string province;

        /// <summary>
        /// 属性，所在县市
        /// </summary>
        public string City
        {
            get
            {
                return city;
            }
            set
            {
                city = value;
            }
        }
        private string city;

        /// <summary>
        /// 属性，分层列表
        /// </summary>
        public List<Layer> LayerList
        {
            get
            {
                return layerList;
            }
        }
        private List<Layer> layerList = new List<Layer>();

        /// <summary>
        /// 属性，公司名称
        /// </summary>
        public string CompanyName
        {
            get
            {
                return companyName;
            }
            set
            {
                companyName = value;
            }
        }
        private string companyName;

        /// <summary>
        /// 属性，公司资质代码
        /// </summary>
        public string CompanyCode
        {
            get
            {
                return companyCode;
            }
            set
            {
                companyCode = value;
            }
        }
        private string companyCode;

        /// <summary>
        /// 属性，绘图人
        /// </summary>
        public string Drawer
        {
            get
            {
                return drawer;
            }
            set
            {
                drawer = value;
            }
        }
        private string drawer;

        /// <summary>
        /// 属性，报告编写人
        /// </summary>
        public string Writer
        {
            get
            {
                return writer;
            }
            set
            {
                writer = value;
            }
        }
        private string writer;

        /// <summary>
        /// 属性，校核人
        /// </summary>
        public string Checker
        {
            get
            {
                return checker;
            }
            set
            {
                checker = value;
            }
        }
        private string checker;

        /// <summary>
        /// 属性，审查人
        /// </summary>
        public string Inspector
        {
            get
            {
                return inspector;
            }
            set
            {
                inspector = value;
            }
        }
        private string inspector;

        /// <summary>
        /// 属性，核定人
        /// </summary>
        public string Approver
        {
            get
            {
                return approver;
            }
            set
            {
                approver = value;
            }
        }
        private string approver;

        /// <summary>
        /// 属性，批准人
        /// </summary>
        public string FinalApprover
        {
            get
            {
                return finalApprover;
            }
            set
            {
                finalApprover = value;
            }
        }
        private string finalApprover;

        /// <summary>
        /// 分层结构体
        /// </summary>
        public struct Layer
        {
            public string Number;
            public string Name;
            public string Geo;
            public string Description;
            public Layer(string _number,string _name,string _geo,string _description)
            {
                Number = _number;
                Name = _name;
                Geo = _geo;
                Description = _description;
            }
        }

        /// <summary>
        /// 添加一个分层
        /// </summary>
        /// <param name="_number">分层编号</param>
        /// <param name="_name">岩土名称</param>
        /// <param name="_geo">地质年代成因</param>
        /// <param name="_description">分层描述</param>
        public void AddLayer(string _number,string _name,string _geo,string _description)
        {
            Layer l = new Layer(_number, _name, _geo, _description);
            layerList.Add(l);
            
        }

        /// <summary>
        /// 编辑一个分层
        /// </summary>
        /// <param name="_oldNumber">老分层编号</param>
        /// <param name="_number">新分层编号</param>
        /// <param name="_name">岩土名称</param>
        /// <param name="_geo">地质年代成因</param>
        /// <param name="_description">分层描述</param>
        public void EditLayer(string _oldNumber, string _number, string _name, string _geo, string _description)
        {
            for(int i = 0; i < layerList.Count; i++)
            {
                if (layerList[i].Number == _oldNumber)
                {
                    Layer newLayer = new Layer(_number, _name, _geo, _description);
                    layerList[i] = newLayer;
                }
            }
        }

        /// <summary>
        /// 删除一个分层
        /// </summary>
        /// <param name="_number">分层编号</param>
        public void DeleteLayer(string _number)
        {
            for (int i = 0; i < layerList.Count; i++)
            {
                if (layerList[i].Number == _number)
                {
                    layerList.RemoveAt(i);
                }
            }
        }
    }
}
