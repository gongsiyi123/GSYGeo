using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Threading;

namespace GSYGeo
{
    /// <summary>
    /// OutputProgress.xaml 的交互逻辑
    /// </summary>
    public partial class OutputProgress : Window
    {
        /// <summary>
        /// 输出类型的枚举：标贯/动探统计，静力触探摩阻力统计，土工常规统计，颗分统计
        /// </summary>
        public enum OutputType
        {
            PreLoadAll,
            PreLoadNtest,
            PreLoadBam,
            PreLoadSs,
            WordLoad,
            NTest,
            Ps,
            RST,
            GAT,
            BearingAndModulus,
            ShearingStrength,
            AllTables,
            ZkCad,
            JkCad
        };

        /// <summary>
        /// 属性，进度条上方的文字内容
        /// </summary>
        private string ProgressInfo { get; set; }

        /// <summary>
        /// 属性，输出文件的路径
        /// </summary>
        private string Path { get; set; }

        /// <summary>
        /// 属性，输出文件的文件夹路径
        /// </summary>
        private string Folder { get; set; }

        /// <summary>
        /// 属性，输出类型
        /// </summary>
        private OutputType BWType { get; set; }

        /// <summary>
        /// 属性，输出的钻孔列表
        /// </summary>
        private List<Borehole> OutputZkList { get; set; }

        /// <summary>
        /// 属性，输出的静力触探孔列表
        /// </summary>
        private List<CPT> OutputJkList { get; set; }

        /// <summary>
        /// 属性，输出图形的的比例尺列表
        /// </summary>
        private List<double> OutputScaleList { get; set; }

        /// <summary>
        /// 后台进程
        /// </summary>
        BackgroundWorker bgWorker = new BackgroundWorker();

        /// <summary>
        /// 构造函数，用于输出数据统计Word文档
        /// </summary>
        /// <param name="_type">输出类型</param>
        /// <param name="_path">输出路径</param>
        /// <param name="_title">窗体的标题</param>
        /// <param name="_progressInfo">进度条上方的文字</param>
        public OutputProgress(OutputType _type,string _path, string _title, string _progressInfo)
        {
            InitializeComponent();

            this.Title = _title;
            BWType = _type;
            Path = _path;
            Folder = _path.Substring(0, _path.LastIndexOf(@"\"));
            ProgressInfo = _progressInfo;
        }

        /// <summary>
        /// 构造函数，用于输出钻孔柱状图
        /// </summary>
        /// <param name="_type">输出类型枚举</param>
        /// <param name="_path">输出文件路径</param>
        /// <param name="_title">输出窗口的标题</param>
        /// <param name="_progressInfo">输出提示信息</param>
        /// <param name="_checkedZkList">输出的触探孔列表</param>
        /// <param name="_checkedScaleList">输出的比例尺列表</param>
        public OutputProgress(OutputType _type, string _path, string _title, string _progressInfo, List<Borehole> _checkedZkList, List<double> _checkedScaleList)
        {
            InitializeComponent();

            this.Title = _title;
            BWType = _type;
            Path = _path;
            Folder = _path.Substring(0, _path.LastIndexOf(@"\"));
            ProgressInfo = _progressInfo;
            OutputZkList = _checkedZkList;
            OutputScaleList = _checkedScaleList;
        }

        /// <summary>
        /// 构造函数，用于输出静力触探曲线图
        /// </summary>
        /// <param name="_type">输出类型枚举</param>
        /// <param name="_path">输出文件路径</param>
        /// <param name="_title">输出窗口的标题</param>
        /// <param name="_progressInfo">输出提示信息</param>
        /// <param name="_checkedJkList">输出的触探孔列表</param>
        /// <param name="_checkedScaleList">输出的比例尺列表</param>
        public OutputProgress(OutputType _type, string _path, string _title, string _progressInfo, List<CPT> _checkedJkList, List<double> _checkedScaleList)
        {
            InitializeComponent();

            this.Title = _title;
            BWType = _type;
            Path = _path;
            Folder = _path.Substring(0, _path.LastIndexOf(@"\"));
            ProgressInfo = _progressInfo;
            OutputJkList = _checkedJkList;
            OutputScaleList = _checkedScaleList;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_ContentRendered(object sender, EventArgs e)
        {
            bgWorker.WorkerReportsProgress = true;
            bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork);
            bgWorker.ProgressChanged += new ProgressChangedEventHandler(bgWorker_ProgressChanged);
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted);
            bgWorker.RunWorkerAsync();
        }
        
        /// <summary>
        /// 启动后台进程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bgWorker_DoWork(object sender,DoWorkEventArgs e)
        {
            // 创建并后台线程和进度条线程
            Thread bw = new Thread(new ParameterizedThreadStart(DoBackGroundWork));
            bw.SetApartmentState(ApartmentState.STA);
            Thread prog = new Thread(new ParameterizedThreadStart(ReadProgress));
            prog.SetApartmentState(ApartmentState.STA);

            // 开启线程
            bw.Start(Path);
            prog.Start();
            
            // 当后台线程结束时，关闭前台进度条线程
            while (true)
            {
                if (bw.ThreadState == ThreadState.Stopped)
                {
                    prog.Abort();
                    bgWorker.ReportProgress(100);
                    break;
                }
            }
        }

        /// <summary>
        /// 后台进程进度变化通知
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bgWorker_ProgressChanged(object sender,ProgressChangedEventArgs e)
        {
            this.OutputProgressBar.Value = e.ProgressPercentage;
            this.ProgressInfoTextBlock.Text = ProgressInfo;
        }

        /// <summary>
        /// 后台进程结束通知
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bgWorker_RunWorkerCompleted(object sender,RunWorkerCompletedEventArgs e)
        {
            this.ProgressInfoTextBlock.Text = "输出完成！";
            this.OpenFileButton.Visibility = Visibility.Visible;
            this.OpenFolderButton.Visibility = Visibility.Visible;
            this.CloseButton.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 执行后台线程的函数
        /// </summary>
        /// <param name="_path">输出路径</param>
        private void DoBackGroundWork(object _path)
        {
            if (BWType == OutputType.WordLoad)
                WorkLoadStatistic.OutputToWord(Path);
            else if (BWType == OutputType.NTest)
                NTestStatistic.OutputToWord(Path);
            else if (BWType == OutputType.Ps)
                CPTStatistic.OutputToWord(Path);
            else if (BWType == OutputType.RST)
                RSTStatistic.OutputToWord(Path);
            else if (BWType == OutputType.GAT)
                GATStatistic.OutputToWord(Path);
            else if (BWType == OutputType.BearingAndModulus)
                BearingAndModulusCalculation.OutputToWord(Path);
            else if (BWType == OutputType.ShearingStrength)
                ShearingStrengthCalculation.OutputToWord(Path);
            else if (BWType == OutputType.AllTables)
                OutputStatisticToWord.OutputToWord(Path);
            else if (BWType == OutputType.ZkCad)
                OutputZkToCad.OutputToCad(Path, OutputZkList, OutputScaleList);
            else if (BWType == OutputType.JkCad)
                OutputJkToCad.OutputToCad(Path, OutputJkList, OutputScaleList);
        }

        /// <summary>
        /// 滚动进度条的函数
        /// </summary>
        /// <param name="_obj"></param>
        private void ReadProgress(object _obj)
        {
            for (int i = 0; i <= 99; i++)
            {
                bgWorker.ReportProgress(i);
                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// 点击"打开文件"按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            System.Diagnostics.Process.Start("explorer.exe", Path);
        }

        /// <summary>
        /// 点击"打开文件夹"按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenFolderButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            System.Diagnostics.Process.Start("explorer.exe", Folder);
        }

        /// <summary>
        /// 点击"关闭"按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
