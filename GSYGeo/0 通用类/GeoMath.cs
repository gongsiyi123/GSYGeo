using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSYGeo
{
    /// <summary>
    /// 进行一些数学计算的类
    /// </summary>
    public class GeoMath
    {
        /// <summary>
        /// 单向线性插值函数
        /// </summary>
        /// <param name="_number">插值元素</param>
        /// <param name="_parameters">参数数组</param>
        /// <param name="_values">值数组</param>
        /// <returns></returns>
        public static double Interpolation(double _number,double[] _parameters,double[] _values)
        {
            // 当参数数组与值数组长度不相等时，返回空值
            if ((_parameters.Length != _values.Length)||
                (_number < System.Math.Min(_parameters[0], _parameters[_parameters.Length - 1]))||
                (_number > System.Math.Max(_parameters[0], _parameters[_parameters.Length - 1])))
                return Constants.NullNumber;

            // 初始化结果变量为空值
            double result = Constants.NullNumber;
            
            // 枚举计算
            for(int i = 0; i < _parameters.Length - 1; i++)
                if ((_number <= _parameters[i] && _number >= _parameters[i + 1]) || (_number >= _parameters[i] && _number <= _parameters[i + 1]))
                {
                    double minPara = System.Math.Min(_parameters[i], _parameters[i + 1]);
                    double dPara1 = _number - minPara;
                    double dPara2 = System.Math.Abs(_parameters[i] - _parameters[i + 1]);

                    if (dPara2 == 0)
                        result = _values[i];
                    else
                    {
                        double ratio = dPara1 / dPara2;
                        double minVal = System.Math.Min(_values[i], _values[i + 1]);
                        double dVal = System.Math.Abs(_values[i] - _values[i + 1]);
                        result = minVal + ratio * dVal;
                    }

                    break;
                }

            // 返回结果
            return result;
        }

        /// <summary>
        /// 双向线性插值函数
        /// </summary>
        /// <param name="_numberRow">行方向插值元素</param>
        /// <param name="_numberCol">列方向插值元素</param>
        /// <param name="_parametersRow">行方向参数数组</param>
        /// <param name="_parametersCol">列方向参数数组</param>
        /// <param name="_valus">值二维数组</param>
        /// <returns></returns>
        public static double TwoWayInterpolation(double _numberRow,double _numberCol,double[] _parametersRow,double[] _parametersCol,double[,] _valus)
        {
            // 当行参数数组长度与值二维数组列数不同，或列参数数组长度与值二维数组行数不同时，返回空值
            if (_valus.GetLength(0) != _parametersRow.Length ||
                _valus.GetLength(1) != _parametersCol.Length)
                return Constants.NullNumber;
            
            // 枚举行参数数组和列参数数组，查找两个行标和两个列标，无法找到时返回空值
            int rowIndex1 = -1, rowIndex2 = -1, colIndex1 = -1, colIndex2 = -1;
            
            for(int i = 0; i < _parametersRow.Length - 1; i++)
                if((_numberRow <= _parametersRow[i] && _numberRow >= _parametersRow[i + 1]) ||
                    (_numberRow >= _parametersRow[i] && _numberRow <= _parametersRow[i + 1]))
                {
                    rowIndex1 = i;
                    rowIndex2 = i + 1;
                    break;
                }

            for(int i = 0; i < _parametersCol.Length - 1; i++)
                if((_numberCol <= _parametersCol[i] && _numberCol >= _parametersCol[i + 1]) ||
                    (_numberCol >= _parametersCol[i] && _numberCol <= _parametersCol[i + 1]))
                {
                    colIndex1 = i;
                    colIndex2 = i + 1;
                    break;
                }

            if (rowIndex1 == -1 || rowIndex2 == -1 || colIndex1 == -1 || colIndex2 == -1)
                return Constants.NullNumber;

            // 计算行方向比率和列方向比率
            double paraRow1 = _parametersRow[rowIndex1];
            double paraRow2 = _parametersRow[rowIndex2];
            double ratioRow = (_numberRow - paraRow2) / (paraRow1 - paraRow2);

            double paraCol1 = _parametersCol[colIndex1];
            double paraCol2 = _parametersCol[colIndex2];
            double ratioCol = (_numberCol - paraCol2) / (paraCol1 - paraCol2);

            // 初始化结果变量为空值
            double result = Constants.NullNumber;

            // 计算最终结果
            double result11 = _valus[rowIndex1, colIndex1];
            double result12 = _valus[rowIndex2, colIndex1];
            double result21 = _valus[rowIndex1, colIndex2];
            double result22 = _valus[rowIndex2, colIndex2];

            double result1 = result12 + ratioRow * (result11 - result12);
            double result2 = result22 + ratioRow * (result21 - result22);

            result = result2 + ratioCol * (result1 - result2);

            // 返回结果
            return result;
        }
    }
}
