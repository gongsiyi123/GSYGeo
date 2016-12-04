using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace GSYGeo
{
    // 用于"数据存储路径"与"用户界面控件"之间进行数据绑定的专用类
    // 本类实例化后可直接操作注册表中数据存储路径的项，用作数据绑定源
    public class ProgramPath:INotifyPropertyChanged
    {
        public ProgramPath() { }
        public ProgramPath(string _pathInput)
        {
            this._path = _pathInput;
        }

        private string _path;
        public string Path
        {
            get
            {
                _path = Program.ReadProgramPath();
                return _path;
            }
            set
            {
                _path = value;
                Program.SetProgramPath(_path);
                Notify("Path");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void Notify(string _newPath)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(_newPath));
            }
        }
    }
}
