using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSYGeo
{
    /// <summary>
    /// 用于统计数据的类
    /// </summary>
    public class Statistic
    {
        /// <summary>
        /// 统计统计数
        /// </summary>
        /// <param name="_datalist"></param>
        /// <returns></returns>
        public static double Count(List<double> _datalist)
        {
            return _datalist.Count;
        }

        /// <summary>
        /// 统计最大值
        /// </summary>
        /// <param name="_datalist"></param>
        /// <returns></returns>
        public static double Max(List<double> _datalist)
        {
            if (_datalist.Count >0 )
            {
                return _datalist.Max();
            }
            else
            {
                return -0.19880205;
            }
        }

        /// <summary>
        /// 统计最小值
        /// </summary>
        /// <param name="_datalist"></param>
        /// <returns></returns>
        public static double Min(List<double> _datalist)
        {
            if (_datalist.Count > 0)
            {
                return _datalist.Min();
            }
            else
            {
                return -0.19880205;
            }
        }

        /// <summary>
        /// 统计平均值
        /// </summary>
        /// <param name="_datalist"></param>
        /// <returns></returns>
        public static double Average(List<double> _datalist)
        {
            if (_datalist.Count > 0)
            {
                return _datalist.Average();
            }
            else
            {
                return -0.19880205;
            }
        }

        /// <summary>
        /// 统计标准差
        /// </summary>
        /// <param name="_datalist"></param>
        /// <param name="_startCount">起始统计样本数</param>
        /// <returns></returns>
        public static double StandardDeviation(List<double> _datalist,int _startCount)
        {
            if (_datalist.Count >= _startCount)
            {
                double x2sum = 0, xsum2 = _datalist.Sum() * _datalist.Sum(), count = _datalist.Count;
                foreach(double _data in _datalist)
                {
                    x2sum += _data * _data;
                }
                return Math.Sqrt((x2sum - xsum2 / count) / (count - 1));
            }
            else
            {
                return -0.19880205;
            }
        }

        /// <summary>
        /// 统计变异系数
        /// </summary>
        /// <param name="_datalist"></param>
        /// <param name="_startCount">起始统计样本数</param>
        /// <returns></returns>
        public static double VariableCoefficient(List<double> _datalist,int _startCount)
        {
            if (_datalist.Count >= _startCount)
            {
                return StandardDeviation(_datalist, _startCount) / Count(_datalist);
            }
            else
            {
                return -0.19880205;
            }
        }

        /// <summary>
        /// 统计标准值
        /// </summary>
        /// <param name="_datalist">要统计的列表</param>
        /// <param name="_startCount">起始统计样本数</param>
        /// <param name="_isPlus">是否为+</param>
        /// <returns></returns>
        public static double StandardValue(List<double> _datalist,int _startCount,bool _isPlus)
        {
            if (_datalist.Count >= _startCount)
            {
                if (_isPlus)
                {
                    return 1 + (1.704 / Math.Sqrt(Count(_datalist)) + 4.678 / Math.Pow(Count(_datalist), 2)) * VariableCoefficient(_datalist, _startCount);
                }
                else
                {
                    return 1 - (1.704 / Math.Sqrt(Count(_datalist)) + 4.678 / Math.Pow(Count(_datalist), 2)) * VariableCoefficient(_datalist, _startCount);
                }
            }
            else
            {
                return -0.19880205;
            }
        }
    }
}
