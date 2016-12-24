using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GSYGeo
{
    /// <summary>
    /// 操作Office相关数据的类
    /// </summary>
    public class OfficeOperation
    {
        /// <summary>
        /// 从EXCEL粘贴数据到DataGrid的函数
        /// </summary>
        /// <returns></returns>
        public static List<string[]> GetDataFromExcelClipBoard()
        {
            // 获取剪切板数据
            string pasteData = Clipboard.GetText();

            // 定义要返回的List<string[]>
            List<string[]> datalist = new List<string[]>();
            
            // 分割剪切板数据
            if (!string.IsNullOrEmpty(pasteData.Trim()) && !string.IsNullOrWhiteSpace(pasteData.Trim()))
            {
                // 分割行
                string[] rows = pasteData.Split('\n');

                // 循环分割单元格，添加到列表
                for (int i = 0; i < rows.Length; i++)
                {
                    if (string.IsNullOrEmpty(rows[i]) || string.IsNullOrWhiteSpace(rows[i]))
                    {
                        continue;
                    }
                    string[] cells = rows[i].Split('\t');

                    // 清除每行数据末尾的回车符
                    if (cells[cells.Length - 1].Contains('\r'))
                    {
                        cells[cells.Length - 1] = cells[cells.Length - 1].Replace("\r", "");
                    }

                    datalist.Add(cells);
                }
                
                return datalist;
            }
            else
            {
                return null;
            }
        }
    }
}
