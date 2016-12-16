using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSYGeo
{
    /// <summary>
    /// 湖北省地方标准
    /// </summary>
    public class HubeiLocalStandard
    {
        /// <summary>
        /// 根据试验指标查表的土质类别
        /// </summary>
        public static string[] RstSoilType =
        {
            "无法识别",
            "新近沉积黏性土",
            "一般黏性土",
            "老黏性土",
            "淤泥、淤泥质土",
            "粉土"
        };

        /// <summary>
        /// 根据Ps值查表的土质类别
        /// </summary>
        public static string[] CptSoilType =
        {
            "无法识别",
            "素填土",
            "一般黏性土",
            "老黏性土",
            "粉土",
            "中、粗砂",
            "粉、细砂"
        };

        /// <summary>
        /// 根据标贯动探查表的土质类别
        /// </summary>
        public static string[] NTestSoilType =
        {
            "无法识别",
            "一般黏性土",
            "老黏性土",
            "中、粗砂",
            "粉、细砂"
        };

        /// <summary>
        /// 按试验指标查表时的土质类型识别
        /// </summary>
        /// <param name="_soilName">岩土名称</param>
        /// <returns></returns>
        public static string SelectRstSoilType(string _soilName)
        {
            // 当遇到"XX夹XX"时，提取"夹"前面的土作为识别依据
            if (_soilName.Contains("夹"))
                _soilName.Substring(0, _soilName.IndexOf("夹"));

            // 含"淤泥"时，返回"淤泥、淤泥质土"
            if (_soilName.Contains("淤泥"))
                return "淤泥、淤泥质土";

            // 含"粘土"或"黏土"时，返回"一般黏性土"
            if (_soilName.Contains("粘土") || _soilName.Contains("黏土"))
                return "一般黏性土";

            // 含"粉土"时，返回"粉土"
            if (_soilName.Contains("粉土"))
                return "粉土";

            // 不含以上情况时，返回"无法识别"
            return "无法识别";
        }

        /// <summary>
        /// 按Ps值查表时的土质类型识别
        /// </summary>
        /// <param name="_soilName">岩土名称</param>
        /// <returns></returns>
        public static string SelectCptSoilType(string _soilName)
        {
            // 当遇到"XX夹XX"时，提取"夹"前面的土作为识别依据
            if (_soilName.Contains("夹"))
                _soilName.Substring(0, _soilName.IndexOf("夹"));

            // 含"素填土"时，返回"素填土"
            if (_soilName.Contains("素填土"))
                return "素填土";

            // 含"淤泥"或"粘土"或"黏土"时，返回"一般黏性土"
            if (_soilName.Contains("淤泥") || _soilName.Contains("粘土") || _soilName.Contains("黏土"))
                return "一般黏性土";

            // 含"粉土"时，返回"粉土"
            if (_soilName.Contains("粉土"))
                return "粉土";

            // 含"砂"且含"中"或"粗"时，返回"中、粗砂"
            if (_soilName.Contains("砂") && (_soilName.Contains("中") || _soilName.Contains("粗")))
                return "中、粗砂";

            // 含"砂"且含"粉"或"细"时，返回"粉、细砂"
            if (_soilName.Contains("砂") && (_soilName.Contains("粉") || _soilName.Contains("细")))
                return "粉、细砂";

            // 不含以上情况时，返回"无法识别"
            return "无法识别";
        }

        /// <summary>
        /// 按标贯/动探查表时的土质类型识别
        /// </summary>
        /// <param name="_soilName">岩土名称</param>
        /// <returns></returns>
        public static string SelectNTestSoilType(string _soilName)
        {
            // 当遇到"XX夹XX"时，提取"夹"前面的土作为识别依据
            if (_soilName.Contains("夹"))
                _soilName.Substring(0, _soilName.IndexOf("夹"));

            // 含"淤泥"或"粘土"或"黏土"时，返回"一般黏性土"
            if (_soilName.Contains("淤泥") || _soilName.Contains("粘土") || _soilName.Contains("黏土"))
                return "一般黏性土";

            // 含"砂"且含"中"或"粗"时，返回"中、粗砂"
            if (_soilName.Contains("砂") && (_soilName.Contains("中") || _soilName.Contains("粗")))
                return "中、粗砂";

            // 含"砂"且含"粉"或"细"时，返回"粉、细砂"
            if (_soilName.Contains("砂") && (_soilName.Contains("粉") || _soilName.Contains("细")))
                return "粉、细砂";

            // 不含以上情况时，返回"无法识别"
            return "无法识别";
        }
    }
}
