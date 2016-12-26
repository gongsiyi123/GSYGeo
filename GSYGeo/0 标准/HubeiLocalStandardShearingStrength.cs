using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSYGeo
{
    /// <summary>
    /// 湖北省地方标准，查抗剪强度
    /// </summary>
    public class HubeiLocalStandardShearingStrength
    {
        #region 土质识别

        /// <summary>
        /// 根据标贯查抗剪强度的土质类型
        /// </summary>
        public static string[] NTestSoilType =
        {
            "无法识别",
            "砂土",
            "一般黏性土、老黏性土"
        };

        /// <summary>
        /// 根据Ps值查抗剪强度的土质类型
        /// </summary>
        public static string[] CptSoilType =
        {
            "无法识别",
            "粉土",
            "淤泥质土、一般黏性土、老黏性土"
        };

        /// <summary>
        /// 按标贯查表时的土质类型识别
        /// </summary>
        /// <param name="_soilName">岩土名称</param>
        /// <returns></returns>
        public static string SelectNTestSoilType(string _soilName)
        {
            // 当遇到"XX夹XX"时，提取"夹"前面的土作为识别依据
            if (_soilName.Contains("夹"))
                _soilName.Substring(0, _soilName.IndexOf("夹"));

            // 含"砂"且含"粉"或"细"时，返回"粉、细砂"
            if (_soilName.Contains("砂"))
                return "砂土";

            // 含"淤泥"或"粘土"或"黏土"时，返回"一般黏性土"
            if (_soilName.Contains("淤泥") || _soilName.Contains("粘土") || _soilName.Contains("黏土"))
                return "一般黏性土、老黏性土";
            
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
            if (_soilName.Contains("粉土"))
                return "粉土";

            // 含"淤泥"或"粘土"或"黏土"时，返回"一般黏性土"
            if (_soilName.Contains("淤泥") || _soilName.Contains("粘土") || _soilName.Contains("黏土"))
                return "淤泥质土、一般黏性土、老黏性土";

            // 不含以上情况时，返回"无法识别"
            return "无法识别";
        }

        #endregion

        #region 根据标贯查抗剪强度

        /// <summary>
        /// 砂土根据标贯击数查抗剪强度
        /// </summary>
        public struct ShearingStrengthNTestSand
        {
            /// <summary>
            /// 列参数数组，标贯锤击数
            /// </summary>
            public static double[] ParaCol = new double[6]
            {
                15,20,25,30,35,40
            };

            /// <summary>
            /// 内摩擦角数值参数数组
            /// </summary>
            public static double[] FrictionData = new double[6]
            {
                32,35,37,39,41,43
            };
        }

        /// <summary>
        /// 一般黏性土、老黏性土根据标贯击数查抗剪强度
        /// </summary>
        public struct ShearingStrengthNTestClay
        {
            /// <summary>
            /// 列参数数组，标贯锤击数
            /// </summary>
            public static double[] ParaCol = new double[11]
            {
                3,5,7,9,11,13,15,17,19,21,23
            };

            /// <summary>
            /// 内摩擦角数值参数数组
            /// </summary>
            public static double[] FrictionData = new double[11]
            {
                9,11,13,15,16,15,15,16,17,18,19
            };

            /// <summary>
            /// 粘聚力数值参数数组
            /// </summary>
            public static double[] CohesionData = new double[11]
            {
                16,21,26,31,37,54,57,60,63,65,68
            };
        }

        #endregion

        #region 根据Ps值查抗剪强度

        /// <summary>
        /// 粉土根据Ps值查抗剪强度
        /// </summary>
        public struct ShearingStrengthCptSilt
        {
            /// <summary>
            /// 列参数数组，Ps值
            /// </summary>
            public static double[] ParaCol = new double[5]
            {
                1.5,2,2.5,3,3.5
            };

            /// <summary>
            /// 内摩擦角数值参数数组
            /// </summary>
            public static double[] FrictionData = new double[5]
            {
                21,23,25,29,31
            };

            /// <summary>
            /// 粘聚力数值参数数组
            /// </summary>
            public static double[] CohesionData = new double[5]
            {
                15,14,13,12,11
            };
        }

        /// <summary>
        /// 淤泥质土、一般黏性土、老黏性土根据Ps值查抗剪强度
        /// </summary>
        public struct ShearingStrengthCptClay
        {
            /// <summary>
            /// 列参数数组，Ps值
            /// </summary>
            public static double[] ParaCol = new double[11]
            {
                0.4,0.8,1.2,1.6,2,2.4,2.8,3,3.5,4,4.5
            };

            /// <summary>
            /// 内摩擦角数值参数数组
            /// </summary>
            public static double[] FrictionData = new double[11]
            {
                5,9,11,13,14,15,16,16,17,18,19
            };

            /// <summary>
            /// 粘聚力数值参数数组
            /// </summary>
            public static double[] CohesionData = new double[11]
            {
                11,15,21,25,30,35,40,55,66,78,90
            };
        }

        #endregion
    }
}
