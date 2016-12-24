using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace GSYGeo
{
    /// <summary>
    /// 单位信息类，直接操作数据库，用于窗口中的数据绑定
    /// 属性包括单位名称、资质编号、人员列表，方法包括添加、编辑、删除人员
    /// </summary>
    class Company :INotifyPropertyChanged
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public Company()
        {
            // 实例化时对People成员进行初始化赋值，将人员列表赋给People集合
            List<string> s = SettingDataBase.ReadCompanyPeople();
            s.ForEach(i => People.Add(i));
        }

        /// <summary>
        /// 属性，单位名称
        /// </summary>
        public string Name
        {
            get
            {
                return SettingDataBase.ReadCompanyName();
            }
            set
            {
                SettingDataBase.EditCompanyName(value);
                Notify("Name");
            }
        }

        /// <summary>
        /// 属性，单位资质编号
        /// </summary>
        public string Code
        {
            get
            {
                return SettingDataBase.ReadCompanyCode();
            }
            set
            {
                SettingDataBase.EditCompanyCode(value);
                Notify("Code");
            }
        }

        /// <summary>
        /// 字段，人员列表集合
        /// </summary>
        public Collection<string> People = new Collection<string>();

        /// <summary>
        /// 方法，刷新人员列表，清空人员列表People，将数据库中的人员数据赋值给People
        /// </summary>
        public void RefreshPeople()
        {
            People.Clear();
            List<string> s = SettingDataBase.ReadCompanyPeople();
            for (int i = 0; i < s.Count; i++)
            {
                People.Add(s[i]);
            }
        }

        /// <summary>
        /// 方法，添加人员
        /// </summary>
        /// <param name="_name">人员名字</param>
        public void AddPeople(string _name)
        {
            SettingDataBase.AddCompanyPeople(_name);
            RefreshPeople();
        }

        /// <summary>
        /// 方法，编辑人员
        /// </summary>
        /// <param name="_oldName">旧名字</param>
        /// <param name="_name">新名字</param>
        public void EditPeople(string _oldName,string _name)
        {
            SettingDataBase.EditCompanyPeople(_oldName, _name);
            RefreshPeople();
        }

        /// <summary>
        /// 方法，删除人员
        /// </summary>
        /// <param name="_name">人员名字</param>
        public void DeletePeople(string _name)
        {
            SettingDataBase.DeleteCompanyPeople(_name);
            RefreshPeople();
        }

        /// <summary>
        /// 属性变更通知
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        protected void Notify(string propName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
