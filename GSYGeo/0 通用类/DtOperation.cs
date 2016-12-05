using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace GSYGeo
{
    // 进行DataTable操作的类
    public class DtOperation
    {
        // 去除DataTable中的空数据行
        public static DataTable RemoveEmptyRow(DataTable _dt)
        {
            List<int> removelist = new List<int>();

            for (int i = 0; i < _dt.Rows.Count; i++)
            {
                bool isEmpty = true;
                for (int j = 0; j < _dt.Columns.Count; j++)
                {
                    if (!string.IsNullOrEmpty(_dt.Rows[i][j].ToString().Trim()) || !string.IsNullOrWhiteSpace(_dt.Rows[i][j].ToString().Trim()))
                    {
                        isEmpty = false;
                        break;
                    }
                }
                if (isEmpty)
                {
                    removelist.Add(i);
                }
            }
            
            for(int i = removelist.Count - 1; i >= 0; i--)
            {
                _dt.Rows.RemoveAt(removelist[i]);
            }
            
            return _dt;
        }
    }
}
