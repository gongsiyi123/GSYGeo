using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace GSYGeo
{
    // 存储地质年代及成因字符串的常量类
    class Geology
    {
        public static Dictionary<string, string> Genesis = new Dictionary<string, string>
        {
            {"q4ml","Q4ml" },
            {"q4al","Q4al" },
            {"q4aldl","Q4al+dl" }
        };
    }
}
