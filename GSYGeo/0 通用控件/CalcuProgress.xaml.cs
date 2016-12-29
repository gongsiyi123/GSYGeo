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
    /// CalcuProgress.xaml 的交互逻辑
    /// </summary>
    public partial class CalcuProgress : Window
    {
        /// <summary>
        /// 属性，输出类型
        /// </summary>
        private OutputProgress.OutputType BWType { get; set; }

        /// <summary>
        /// 后台进程
        /// </summary>
        BackgroundWorker bgWorker = new BackgroundWorker();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_type">输出类型</param>
        public CalcuProgress(OutputProgress.OutputType _type)
        {
            InitializeComponent();

            BWType = _type;
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

        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // 创建并后台线程和进度条线程
            Thread bw = new Thread(new ParameterizedThreadStart(DoBackGroundWork));
            bw.SetApartmentState(ApartmentState.STA);
            Thread prog = new Thread(new ParameterizedThreadStart(ReadProgress));
            prog.SetApartmentState(ApartmentState.STA);

            // 开启线程
            bw.Start();
            prog.Start();

            // 当后台线程结束时，关闭前台进度条线程
            while (true)
            {
                if (bw.ThreadState == ThreadState.Stopped)
                {
                    prog.Abort();
                    break;
                }
            }
        }

        /// <summary>
        /// 后台进程进度变化通知
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            string s = "正在计算";
            for (int i = 0; i < e.ProgressPercentage; i++)
                s += ".";
            this.CalculatingTextBlock.Text = s;
        }

        /// <summary>
        /// 后台进程结束通知
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 执行后台线程的函数
        /// </summary>
        private void DoBackGroundWork(object obj)
        {
            if (BWType == OutputProgress.OutputType.WordLoad)
                return;
            else if (BWType == OutputProgress.OutputType.NTest)
                return;
            else if (BWType == OutputProgress.OutputType.Ps)
                return;
            else if (BWType == OutputProgress.OutputType.RST)
                return;
            else if (BWType == OutputProgress.OutputType.GAT)
                return;
            else if (BWType == OutputProgress.OutputType.BearingAndModulus)
                OutputStatisticToWord.CalculatingBam();
            else if (BWType == OutputProgress.OutputType.ShearingStrength)
                OutputStatisticToWord.CalculatingSs();
            else if (BWType == OutputProgress.OutputType.AllTables)
                return;
            else if (BWType == OutputProgress.OutputType.ZkCad)
                return;
            else if (BWType == OutputProgress.OutputType.JkCad)
                return;
            else if (BWType == OutputProgress.OutputType.PreLoadAll)
                OutputStatisticToWord.PreviewLoadAll();
            else if (BWType == OutputProgress.OutputType.PreLoadNtest)
                OutputStatisticToWord.PreviewLoadNtest();
        }

        /// <summary>
        /// 滚动进度条的函数
        /// </summary>
        private void ReadProgress(object obj)
        {
            while (true)
            {
                for (int i = 0; i < 6; i++)
                {
                    bgWorker.ReportProgress(i);
                    Thread.Sleep(100);
                }
            }
        }
    }
}
