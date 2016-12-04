using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSYGeo
{
    // 当前项目设置类
    class ProjectSetting
    {
        // 无参数的构造函数
        public ProjectSetting()
        {
            name = null;
            province = null;
            city = null;
            //layerList = null;
        }

        // 带参数的构造函数
        public ProjectSetting(string _name,string _province,string _city,List<Layer> _layerList)
        {
            name = _name;
            province = _province;
            city = _city;
            layerList = _layerList;
        }

        // 属性，项目名称
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

        // 属性，所在省份
        private string province;
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

        // 属性，所在县市
        private string city;
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

        // 属性，分层列表
        private List<Layer> layerList = new List<Layer>();
        public List<Layer> LayerList
        {
            get
            {
                return layerList;
            }
        }

        // 属性，公司名称
        private string companyName;
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

        // 属性，公司资质代码
        private string companyCode;
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

        // 属性，绘图人
        private string drawer;
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

        // 属性，报告编写人
        private string writer;
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

        // 属性，校核人
        private string checker;
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

        // 属性，审查人
        private string inspector;
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

        // 属性，核定人
        private string approver;
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

        // 属性，批准人
        private string finalApprover;
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

        // 分层结构体
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

        // 方法，添加一个分层
        public void AddLayer(string _number,string _name,string _geo,string _description)
        {
            Layer l = new Layer(_number, _name, _geo, _description);
            layerList.Add(l);
            
        }

        // 方法，编辑一个分层
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

        // 方法，删除一个分层
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
