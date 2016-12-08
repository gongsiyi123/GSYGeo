using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace GSYGeo
{
    /// <summary>
    /// 进行DataTable操作的类
    /// </summary>
    public class DtOperation
    {
        /// <summary>
        /// 去除DataTable中的空数据行
        /// </summary>
        /// <param name="_dt"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 从Excel表格粘贴数据到DataTable的函数
        /// 返回值为粘贴后的DataTable
        /// </summary>
        /// <param name="_dt">要粘贴数据的DataTable</param>
        /// <param name="_selectRowIndex">粘贴的起始行号</param>
        /// <param name="_selectColumnIndex">粘贴的起始列号</param>
        /// <param name="_dataList">从Excel复制来的数据</param>
        /// <param name="_skipInt">要跳过的列号，-1表示没有要跳过的</param>
        /// <returns></returns>
        public static DataTable PasteFromExcel(DataTable _dt,int _selectRowIndex,int _selectColumnIndex,List<string[]> _dataList,int _skipInt)
        {
            // 行循环
            for(int i = 0; i < _dataList.Count; i++)
            {
                // 超过DataTable最大行范围时，增加一行
                if (i >= _dt.Rows.Count - _selectRowIndex)
                {
                    _dt.Rows.Add(_dt.NewRow());
                }

                //列循环
                for(int j = 0; j < _dataList[i].Length; j++)
                {
                    // 超过DataTable最大列数范围时，跳过本行的后续数据
                    if (j >= _dt.Columns.Count - _selectColumnIndex - 1)
                    {
                        break;
                    }

                    // 复制数据
                    // 当_skipInt为-1时，表示没有需要跳过的序号
                    if (_skipInt == -1)
                    {
                        _dt.Rows[i + _selectRowIndex][j + _selectColumnIndex] = _dataList[i][j];
                    }
                    else
                    {
                        if (_selectColumnIndex >= _skipInt+1)
                        {
                            _dt.Rows[i + _selectRowIndex][j + _selectColumnIndex + 1] = _dataList[i][j];
                        }
                        else
                        {
                            if (j + _selectColumnIndex < _skipInt)
                            {
                                _dt.Rows[i + _selectRowIndex][j + _selectColumnIndex] = _dataList[i][j];
                            }
                            else
                            {
                                _dt.Rows[i + _selectRowIndex][j + _selectColumnIndex + 1] = _dataList[i][j];
                            }
                        }
                    }
                }
            }
            // 返回处理后DataTable
            return _dt;
        }
    }
}
