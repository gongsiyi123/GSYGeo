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
        #region 土质识别

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
            "碎石土",
            "一般黏性土",
            "老黏性土",
            "素填土",
            "杂填土",
            "粉、细砂",
            "中、粗砂",
            "砾砂"
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

            // 含"填土"时，返回"新近沉积黏性土"
            if (_soilName.Contains("素填土"))
                return "新近沉积黏性土";

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

        #endregion

        #region 根据试验指标查承载力

        /// <summary>
        /// 一般黏性土根据试验指标查承载力
        /// </summary>
        public struct BearingRstClay
        {
            /// <summary>
            /// 行参数数组，孔隙比
            /// </summary>
            public static double[] ParaRow = new double[6]
            {
                0.6,0.7,0.8,0.9,1.0,1.1
            };

            /// <summary>
            /// 列参数数组，液性指数
            /// </summary>
            public static double[] ParaCol = new double[6]
            {
                0,0.25,0.5,0.75,1,1.2
            };

            /// <summary>
            /// 数值参数数组
            /// </summary>
            public static double[,] Data = new double[6, 6]
            {
                {Constants.NullNumber,270,250,230,210,Constants.NullNumber },
                {250,220,200,180,160,135 },
                {220,200,180,160,140,120 },
                {190,170,150,130,110,100 },
                {160,140,120,110,100,90 },
                {Constants.NullNumber,130,110,100,90,80 }
            };
        }

        /// <summary>
        /// 新近沉积黏性土根据试验指标查承载力
        /// </summary>
        public struct BearingRstNewClay
        {
            /// <summary>
            /// 行参数数组，孔隙比
            /// </summary>
            public static double[] ParaRow = new double[4]
            {
                0.8,0.9,1.0,1.1
            };

            /// <summary>
            /// 列参数数组，液性指数
            /// </summary>
            public static double[] ParaCol = new double[3]
            {
                0.25,0.75,1.25
            };

            /// <summary>
            /// 数值参数数组
            /// </summary>
            public static double[,] Data = new double[4, 3]
            {
                {120,100,80 },
                {110,90,80 },
                {100,80,70 },
                {90,70,Constants.NullNumber }
            };
        }

        /// <summary>
        /// 粉土根据试验指标查承载力
        /// </summary>
        public struct BearingRstSilt
        {
            /// <summary>
            /// 行参数数组，孔隙比
            /// </summary>
            public static double[] ParaRow = new double[6]
            {
                0.6,0.7,0.8,0.9,1,1.1
            };

            /// <summary>
            /// 列参数数组，含水量
            /// </summary>
            public static double[] ParaCol = new double[4]
            {
                20,25,30,35
            };

            /// <summary>
            /// 数值参数数组
            /// </summary>
            public static double[,] Data = new double[6, 4]
            {
                {260,240,Constants.NullNumber,Constants.NullNumber },
                {200,190,160,Constants.NullNumber },
                {160,150,130,Constants.NullNumber },
                {130,120,100,90 },
                {110,100,90,80 },
                {100,90,80,70 }
            };
        }

        /// <summary>
        /// 淤泥、淤泥质土根据试验指标查承载力
        /// </summary>
        public struct BearingRstSludge
        {
            /// <summary>
            /// 列参数数组，含水量
            /// </summary>
            public static double[] ParaCol = new double[6]
            {
                36,40,45,50,55,65
            };

            /// <summary>
            /// 数值参数数组
            /// </summary>
            public static double[] Data = new double[6]
            {
                70,65,60,55,50,40
            };
        }

        /// <summary>
        /// 老黏性土根据试验指标查承载力
        /// </summary>
        public struct BearingRstOldClay
        {
            /// <summary>
            /// 列参数数组，含水量
            /// </summary>
            public static double[] ParaCol = new double[6]
            {
                0.5,0.55,0.6,0.65,0.7,0.75
            };

            /// <summary>
            /// 数值参数数组
            /// </summary>
            public static double[] Data = new double[6]
            {
                630,560,480,430,380,350
            };
        }

        #endregion

        #region 根据Ps值查承载力和压缩模量

        /// <summary>
        /// 淤泥质土、一般黏性土根据Ps值查承载力、压缩模量
        /// </summary>
        public struct BearingAndModulusCptClay
        {
            /// <summary>
            /// 列参数数组，Ps值
            /// </summary>
            public static double[] ParaCol = new double[11]
            {
                0.3,0.5,0.7,0.9,1.2,1.5,1.8,2.1,2.4,2.7,2.9
            };

            /// <summary>
            /// 承载力数值参数数组
            /// </summary>
            public static double[] BearingData = new double[11]
            {
                40,60,80,100,120,150,180,210,240,270,290
            };

            /// <summary>
            /// 压缩模量数值参数数组
            /// </summary>
            public static double[] ModulusData = new double[11]
            {
                2,3,4,5,6,7,8,9,10,11,12
            };
        }

        /// <summary>
        /// 老黏性土根据Ps值查承载力、压缩模量
        /// </summary>
        public struct BearingAndModulusCptOldClay
        {
            /// <summary>
            /// 列参数数组，Ps值
            /// </summary>
            public static double[] ParaCol = new double[8]
            {
                3,3.3,3.6,3.9,4.2,4.5,4.8,5.1
            };

            /// <summary>
            /// 承载力数值参数数组
            /// </summary>
            public static double[] BearingData = new double[8]
            {
                320,360,400,450,500,560,610,660
            };

            /// <summary>
            /// 压缩模量数值参数数组
            /// </summary>
            public static double[] ModulusData = new double[8]
            {
                13,14,15,16,17,19,21,23
            };
        }

        /// <summary>
        /// 粉土根据Ps值查承载力、压缩模量
        /// </summary>
        public struct BearingAndModulusCptSilt
        {
            /// <summary>
            /// 列参数数组，Ps值
            /// </summary>
            public static double[] ParaCol = new double[5]
            {
                1,1.5,2,2.5,3
            };

            /// <summary>
            /// 承载力数值参数数组
            /// </summary>
            public static double[] BearingData = new double[5]
            {
                90,100,110,130,150
            };

            /// <summary>
            /// 压缩模量数值参数数组
            /// </summary>
            public static double[] ModulusData = new double[5]
            {
                6,7,8,9,10
            };
        }

        /// <summary>
        /// 素填土根据Ps值查承载力、压缩模量
        /// </summary>
        public struct BearingAndModulusCptFill
        {
            /// <summary>
            /// 列参数数组，Ps值
            /// </summary>
            public static double[] ParaCol = new double[4]
            {
                0.5,1,1.5,2
            };

            /// <summary>
            /// 承载力数值参数数组
            /// </summary>
            public static double[] BearingData = new double[4]
            {
                50,80,110,130
            };

            /// <summary>
            /// 压缩模量数值参数数组
            /// </summary>
            public static double[] ModulusData = new double[4]
            {
                2,3,4,5
            };
        }

        /// <summary>
        /// 粉细砂根据Ps值查承载力、压缩模量
        /// </summary>
        public struct BearingAndModulusCptFineSand
        {
            /// <summary>
            /// 列参数数组，Ps值
            /// </summary>
            public static double[] ParaCol = new double[13]
            {
                3,4,5,6,7,8,9,10,11,12,13,14,15
            };

            /// <summary>
            /// 承载力数值参数数组
            /// </summary>
            public static double[] BearingData = new double[13]
            {
                110,130,150,170,190,210,230,250,270,290,310,330,350
            };

            /// <summary>
            /// 压缩模量数值参数数组
            /// </summary>
            public static double[] ModulusData = new double[13]
            {
                10,12,13,15,17,19,21,23,25,27,29,32,35
            };
        }

        /// <summary>
        /// 中粗砂根据Ps值查承载力、压缩模量
        /// </summary>
        public struct BearingAndModulusCptCoarseSand
        {
            /// <summary>
            /// 列参数数组，Ps值
            /// </summary>
            public static double[] ParaCol = new double[13]
            {
                3,4,5,6,7,8,9,10,11,12,13,14,15
            };

            /// <summary>
            /// 承载力数值参数数组
            /// </summary>
            public static double[] BearingData = new double[13]
            {
                140,180,220,260,290,320,350,380,410,440,470,500,530
            };

            /// <summary>
            /// 压缩模量数值参数数组
            /// </summary>
            public static double[] ModulusData = new double[13]
            {
                10,12,13,15,17,19,21,23,25,27,29,32,35
            };
        }

        #endregion

        #region 根据标贯/动探查承载力和压缩模量

        /// <summary>
        /// 碎石土根据重型动探值查承载力、压缩模量
        /// </summary>
        public struct BearingAndModulusNTestGravels
        {
            /// <summary>
            /// 列参数数组，重型动探锤击数
            /// </summary>
            public static double[] ParaCol = new double[14]
            {
                3,4,5,6,7,8,9,10,11,12,13,14,16,18
            };

            /// <summary>
            /// 承载力数值参数数组
            /// </summary>
            public static double[] BearingData = new double[14]
            {
                140,170,200,240,280,320,360,400,440,480,510,540,600,600
            };
        }

        /// <summary>
        /// 一般黏性土根据标贯值查承载力、压缩模量
        /// </summary>
        public struct BearingAndModulusNTestClay
        {
            /// <summary>
            /// 列参数数组，标贯锤击数
            /// </summary>
            public static double[] ParaCol = new double[10]
            {
                3,4,5,6,7,8,9,10,11,12
            };

            /// <summary>
            /// 承载力数值参数数组
            /// </summary>
            public static double[] BearingData = new double[10]
            {
                85,100,120,140,160,180,200,230,260,290
            };

            /// <summary>
            /// 压缩模量数值参数数组
            /// </summary>
            public static double[] ModulusData = new double[10]
            {
                4,6,8,9,10,11,12,13,14,15
            };
        }

        /// <summary>
        /// 老黏性土根据标贯值查承载力、压缩模量
        /// </summary>
        public struct BearingAndModulusNTestOldClay
        {
            /// <summary>
            /// 列参数数组，标贯锤击数
            /// </summary>
            public static double[] ParaCol = new double[10]
            {
                13,14,15,16,17,18,19,20,21,22
            };

            /// <summary>
            /// 承载力数值参数数组
            /// </summary>
            public static double[] BearingData = new double[10]
            {
                330,360,390,420,450,480,510,540,570,610
            };

            /// <summary>
            /// 压缩模量数值参数数组
            /// </summary>
            public static double[] ModulusData = new double[10]
            {
                14,15,16,17,18,19,20,21,22,23
            };
        }

        /// <summary>
        /// 杂填土根据重型动探值查承载力、压缩模量
        /// </summary>
        public struct BearingAndModulusNTestMixedFill
        {
            /// <summary>
            /// 列参数数组，重型动探锤击数
            /// </summary>
            public static double[] ParaCol = new double[4]
            {
                1,2,3,4
            };

            /// <summary>
            /// 承载力数值参数数组
            /// </summary>
            public static double[] BearingData = new double[4]
            {
                40,80,120,160
            };

            /// <summary>
            /// 压缩模量数值参数数组
            /// </summary>
            public static double[] ModulusData = new double[4]
            {
                2,3.5,5,6.5
            };
        }

        /// <summary>
        /// 粉细砂根据标贯值查承载力、压缩模量
        /// </summary>
        public struct BearingAndModulusNTestFineSand
        {
            /// <summary>
            /// 列参数数组，标贯锤击数
            /// </summary>
            public static double[] ParaCol = new double[13]
            {
                3,4,5,6,7,8,9,10,11,12,13,14,15
            };

            /// <summary>
            /// 承载力数值参数数组
            /// </summary>
            public static double[] BearingData = new double[13]
            {
                110,130,150,170,190,210,230,250,270,290,310,330,350
            };

            /// <summary>
            /// 压缩模量数值参数数组
            /// </summary>
            public static double[] ModulusData = new double[13]
            {
                10,12,13,15,17,19,21,23,25,27,29,32,35
            };
        }

        /// <summary>
        /// 中粗砂根据标贯值查承载力、压缩模量
        /// </summary>
        public struct BearingAndModulusNTestCoarseSand
        {
            /// <summary>
            /// 列参数数组，标贯锤击数
            /// </summary>
            public static double[] ParaCol = new double[13]
            {
                3,4,5,6,7,8,9,10,11,12,13,14,15
            };

            /// <summary>
            /// 承载力数值参数数组
            /// </summary>
            public static double[] BearingData = new double[13]
            {
                140,180,220,260,290,320,350,380,410,440,470,500,530
            };

            /// <summary>
            /// 压缩模量数值参数数组
            /// </summary>
            public static double[] ModulusData = new double[13]
            {
                10,12,13,15,17,19,21,23,25,27,29,32,35
            };
        }

        /// <summary>
        /// 砾砂根据重型动探值查承载力、压缩模量
        /// </summary>
        public struct BearingAndModulusNTestGravellySand
        {
            /// <summary>
            /// 列参数数组，重型动探锤击数
            /// </summary>
            public static double[] ParaCol = new double[8]
            {
                3,4,5,6,7,8,9,10
            };

            /// <summary>
            /// 承载力数值参数数组
            /// </summary>
            public static double[] BearingData = new double[8]
            {
                120,150,200,240,280,320,360,400
            };
        }

        #endregion
    }
}
