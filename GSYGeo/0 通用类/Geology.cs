using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace GSYGeo
{
    
    /// <summary>
    /// 地质相关赋值和计算操作的类
    /// </summary>
    public class Geology
    {
        /// <summary>
        /// 存储地质年代及成因字符串的字典
        /// </summary>
        public static Dictionary<string, string> Genesis = new Dictionary<string, string>
        {
            {"q4ml","Q4ml" },
            {"q4al","Q4al" },
            {"q4aldl","Q4al+dl" }
        };

        /// <summary>
        /// 计算取样深度所属的分层序号，返回值为-1则表示取样深度不属于所有分层范围内
        /// </summary>
        /// <param name="_sampleDepth">取样深度</param>
        /// <param name="_layerDepthList">分层深度列表</param>
        /// <returns>取样深度所属的分层的序号，从0开始</returns>
        public static int LayerIndex(double _sampleDepth,List<double> _layerDepthList)
        {
            for(int i = 0; i < _layerDepthList.Count; i++)
            {
                if (_sampleDepth <= _layerDepthList[i])
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
