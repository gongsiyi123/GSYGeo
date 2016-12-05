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
using System.Net.Mail;

namespace GSYGeo
{
    /// <summary>
    /// FeedBack.xaml 的交互逻辑
    /// </summary>
    public partial class FeedBack : Window
    {
        // 初始文字
        string t1 = "我将对您宝贵意见的回复发送至此邮箱。";
        string t2 = "亲爱的用户您好，欢迎使用本软件。如果您使用中遇到问题，或对软件功能有宝贵建议，请通过本页面反馈给我。我将及时回复并根据您反馈的意见对软件进行更新。让我们共同进步！";
        
        // 构造函数
        public FeedBack()
        {
            InitializeComponent();

            // 设置初始提示文字
            InitialText();
        }

        // 设置初始文字
        private void InitialText()
        {
            NameTextBox.Text = t1;
            NameTextBox.Foreground = Brushes.DarkGray;
            FeedBackTextBox.Text = t2;
            FeedBackTextBox.Foreground = Brushes.DarkGray;
        }

        // 点击"发送"
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.163.com";
            client.UseDefaultCredentials = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new System.Net.NetworkCredential("gsygeofeedback", "gsygeo12345");
            MailMessage message = new MailMessage();
            message.From = new MailAddress("gsygeofeedback@163.com");
            message.To.Add("gongsiyi198825@gmail.com");
            message.Subject = "GSYGeo用户反馈";
            string feedBacker = this.NameTextBox.Text == t1 ? "无邮箱" : this.NameTextBox.Text;
            string feedBackBody = this.FeedBackTextBox.Text == t2 ? "无反馈" : this.FeedBackTextBox.Text;
            StringBuilder feedback = new StringBuilder();
            feedback.AppendLine("--------------------\n");
            feedback.AppendLine("反馈人邮箱：" + feedBacker + "\n");
            feedback.AppendLine("--------------------\n");
            feedback.AppendLine("反馈内容：\n");
            feedback.AppendLine(feedBackBody + "\n");
            feedback.AppendLine("--------------------");
            message.Body = feedback.ToString();
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.Priority = MailPriority.High;
            message.IsBodyHtml = false;
            try
            {
                client.Send(message);
                MessageBox.Show("发送成功！感谢您的宝贵建议！");
                this.Close();
            }
            catch
            {
                MessageBox.Show("发送失败！");
            }
        }

        // 点击邮箱输入框时，清除初始文字
        private void NameTextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.NameTextBox.Text == t1)
            {
                this.NameTextBox.Clear();
                this.NameTextBox.Foreground = Brushes.Black;
            }
        }

        // 点击反馈输入框时，清除初始文字
        private void FeedBackTextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.FeedBackTextBox.Text == t2)
            {
                this.FeedBackTextBox.Clear();
                this.FeedBackTextBox.Foreground = Brushes.Black;
            }
        }
    }
}
