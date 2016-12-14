using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using Microsoft.Office.Interop.Word;
using MSWord = Microsoft.Office.Interop.Word;

namespace GSYGeo
{
    /// <summary>
    /// 操作Word文档的类
    /// </summary>
    public class Word
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Word()
        {
            App = new MSWord.Application();
            Doc = App.Documents.Add(ref Nothing, ref Nothing, ref Nothing, ref Nothing);
        }

        /// <summary>
        /// 空值
        /// </summary>
        object Nothing = Missing.Value;

        /// <summary>
        /// 属性，Word应用
        /// </summary>
        public MSWord.Application App { get; set; }

        /// <summary>
        /// 属性，Word文档
        /// </summary>
        public MSWord.Document Doc { get; set; }

        /// <summary>
        /// 属性，Word文档中的表格
        /// </summary>
        public MSWord.Tables Tables
        {
            get
            {
                return Doc.Tables;
            }
        }

        /// <summary>
        /// 保存并退出Word，保存为97-2003格式
        /// </summary>
        /// <param name="_path">保存路径</param>
        public void SaveAndQuit(object _path)
        {
            object format = MSWord.WdSaveFormat.wdFormatDocument97;
            Doc.SaveAs2(ref _path, ref format, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing);
            Doc.Close(ref Nothing, ref Nothing, ref Nothing);
            App.Quit(ref Nothing, ref Nothing, ref Nothing);
        }

        /// <summary>
        /// 添加新表格
        /// </summary>
        /// <param name="_title">表格标题</param>
        /// <param name="_countRows">表格行数</param>
        /// <param name="_countColumns">表格列数</param>
        /// <returns>新添加的表格</returns>
        public MSWord.Table AddTable(string _title,int _countRows,int _countColumns)
        {
            // 填写表格标题
            Doc.Paragraphs.Last.Range.Text = _title;
            Doc.Paragraphs.Last.Range.Font.Bold = 1;
            Doc.Paragraphs.Last.Range.Font.Size = 12;
            App.Selection.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            object unite = MSWord.WdUnits.wdStory;
            App.Selection.EndKey(ref unite, ref Nothing);

            // 定义表格对象
            MSWord.Table table = Tables.Add(App.Selection.Range, _countRows, _countColumns, ref Nothing, ref Nothing);
            return table;
        }

        /// <summary>
        /// 添加标贯/动探统计表格
        /// </summary>
        /// <param name="_nTestStatistic">标贯/动探统计数据</param>
        /// <returns></returns>
        public MSWord.Table AddNTestStatisticTable(List<StatisticNTest> _nTestStatistic)
        {
            // 填写表格标题
            Doc.Paragraphs.Last.Range.Text = "表3 标贯/动探锤击数统计表";
            Doc.Paragraphs.Last.Range.Font.Bold = 1;
            Doc.Paragraphs.Last.Range.Font.Size = 12;
            App.Selection.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            object unite = MSWord.WdUnits.wdStory;
            App.Selection.EndKey(ref unite, ref Nothing);

            // 定义表格对象
            MSWord.Table table = Tables.Add(App.Selection.Range, _nTestStatistic.Count + 1, 11, ref Nothing, ref Nothing);
            
            // 填充行标题
            string[] rowheader = new string[] { "层号", "岩土名称", "类型", "统计数", "最大值", "最小值", "平均值", "标准差", "变异系数", "统计修正系数", "标准值" };
            for(int i = 1; i <= table.Columns.Count; i++)
            {
                table.Cell(1, i).Range.Text = rowheader[i - 1];
            }

            // 设置文档格式
            Doc.PageSetup.LeftMargin = 50F;
            Doc.PageSetup.RightMargin = 50F;

            // 设置表格格式
            table.Select();
            App.Selection.Tables[1].Rows.Alignment = WdRowAlignment.wdAlignRowCenter;
            table.Rows[1].Range.Bold = 1;
            for(int i = 2; i <= table.Rows.Count; i++)
            {
                table.Rows[i].Range.Bold = 0;
                table.Columns[7].Cells[i].Range.Bold = 1;
                table.Columns[11].Cells[i].Range.Bold = 1;
            }
            table.Range.Font.Size = 10.5F;
            table.Range.ParagraphFormat.Alignment = MSWord.WdParagraphAlignment.wdAlignParagraphCenter;
            table.Range.Cells.VerticalAlignment = MSWord.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
            table.Borders.OutsideLineStyle = MSWord.WdLineStyle.wdLineStyleDouble;
            table.Borders.InsideLineStyle = MSWord.WdLineStyle.wdLineStyleSingle;
            float[] columnWidth = new float[] { 35, 80, 35, 45, 45, 45, 45, 45, 35, 45, 45 };
            for(int i = 1; i <= table.Columns.Count; i++)
            {
                table.Columns[i].Width = columnWidth[i - 1];
            }

            // 填充标贯/动探数据
            for(int i = 2; i <= table.Rows.Count; i++)
            {
                table.Cell(i, 1).Range.Text = _nTestStatistic[i - 2].Layer;
                table.Cell(i, 2).Range.Text = _nTestStatistic[i - 2].Name;
                table.Cell(i, 3).Range.Text = _nTestStatistic[i - 2].Type.ToString();
                table.Cell(i, 4).Range.Text = _nTestStatistic[i - 2].Count.ToString("0");
                table.Cell(i, 5).Range.Text = _nTestStatistic[i - 2].Max.ToString("0");
                table.Cell(i, 6).Range.Text = _nTestStatistic[i - 2].Min.ToString("0");
                table.Cell(i, 7).Range.Text = _nTestStatistic[i - 2].Average.ToString("0.0");
                table.Cell(i, 8).Range.Text = _nTestStatistic[i - 2].StandardDeviation.ToString() == "-0.19880205" ? "/" : _nTestStatistic[i - 2].StandardDeviation.ToString("0.0");
                table.Cell(i, 9).Range.Text = _nTestStatistic[i - 2].VariableCoefficient.ToString() == "-0.19880205" ? "/" : _nTestStatistic[i - 2].VariableCoefficient.ToString("0.00");
                table.Cell(i, 10).Range.Text = _nTestStatistic[i - 2].CorrectionCoefficient.ToString() == "-0.19880205" ? "/" : _nTestStatistic[i - 2].CorrectionCoefficient.ToString("0.00");
                table.Cell(i, 11).Range.Text = _nTestStatistic[i - 2].StandardValue.ToString() == "-0.19880205" ? "/" : _nTestStatistic[i - 2].StandardValue.ToString("0.0");

            }

            //返回
            return table;
        }

        /// <summary>
        /// 添加静力触探摩阻力统计表格
        /// </summary>
        /// <param name="_cptStatistic">静力触探摩阻力统计数据</param>
        /// <returns></returns>
        public MSWord.Table AddPsStatisticTable(List<StatisticCPT> _cptStatistic)
        {
            // 填写表格标题
            Doc.Paragraphs.Last.Range.Text = "表2 静力触探摩阻力统计表";
            Doc.Paragraphs.Last.Range.Font.Bold = 1;
            Doc.Paragraphs.Last.Range.Font.Size = 12;
            App.Selection.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            object unite = MSWord.WdUnits.wdStory;
            App.Selection.EndKey(ref unite, ref Nothing);

            // 定义表格对象
            MSWord.Table table = Tables.Add(App.Selection.Range, _cptStatistic.Count + 1, 10, ref Nothing, ref Nothing);

            // 填充行标题
            string[] rowheader = new string[] { "层号", "岩土名称", "统计数", "最大值", "最小值", "平均值", "标准差", "变异系数", "统计修正系数", "标准值" };
            for (int i = 1; i <= table.Columns.Count; i++)
            {
                table.Cell(1, i).Range.Text = rowheader[i - 1];
            }

            // 设置文档格式
            Doc.PageSetup.LeftMargin = 50F;
            Doc.PageSetup.RightMargin = 50F;

            // 设置表格格式
            table.Select();
            App.Selection.Tables[1].Rows.Alignment = WdRowAlignment.wdAlignRowCenter;
            table.Rows[1].Range.Bold = 1;
            for (int i = 2; i <= table.Rows.Count; i++)
            {
                table.Rows[i].Range.Bold = 0;
                table.Columns[6].Cells[i].Range.Bold = 1;
                table.Columns[10].Cells[i].Range.Bold = 1;
            }
            table.Range.Font.Size = 10.5F;
            table.Range.ParagraphFormat.Alignment = MSWord.WdParagraphAlignment.wdAlignParagraphCenter;
            table.Range.Cells.VerticalAlignment = MSWord.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
            table.Borders.OutsideLineStyle = MSWord.WdLineStyle.wdLineStyleDouble;
            table.Borders.InsideLineStyle = MSWord.WdLineStyle.wdLineStyleSingle;
            float[] columnWidth = new float[] { 35, 80, 45, 45, 45, 45, 45, 35, 45, 45 };
            for (int i = 1; i <= table.Columns.Count; i++)
            {
                table.Columns[i].Width = columnWidth[i - 1];
            }

            // 填充摩阻力数据
            for (int i = 2; i <= table.Rows.Count; i++)
            {
                table.Cell(i, 1).Range.Text = _cptStatistic[i - 2].Layer;
                table.Cell(i, 2).Range.Text = _cptStatistic[i - 2].Name;
                table.Cell(i, 3).Range.Text = _cptStatistic[i - 2].Count.ToString("0");
                table.Cell(i, 4).Range.Text = _cptStatistic[i - 2].Max.ToString("0.00");
                table.Cell(i, 5).Range.Text = _cptStatistic[i - 2].Min.ToString("0.00");
                table.Cell(i, 6).Range.Text = _cptStatistic[i - 2].Average.ToString("0.00");
                table.Cell(i, 7).Range.Text = _cptStatistic[i - 2].StandardDeviation.ToString() == "-0.19880205" ? "/" : _cptStatistic[i - 2].StandardDeviation.ToString("0.00");
                table.Cell(i, 8).Range.Text = _cptStatistic[i - 2].VariableCoefficient.ToString() == "-0.19880205" ? "/" : _cptStatistic[i - 2].VariableCoefficient.ToString("0.00");
                table.Cell(i, 9).Range.Text = _cptStatistic[i - 2].CorrectionCoefficient.ToString() == "-0.19880205" ? "/" : _cptStatistic[i - 2].CorrectionCoefficient.ToString("0.00");
                table.Cell(i, 10).Range.Text = _cptStatistic[i - 2].StandardValue.ToString() == "-0.19880205" ? "/" : _cptStatistic[i - 2].StandardValue.ToString("0.00");
            }

            //返回
            return table;
        }

        /// <summary>
        /// 添加土工常规试验统计表格
        /// </summary>
        /// <param name="_rstStatistic">土工常规试验统计数据</param>
        /// <returns></returns>
        public MSWord.Table AddRSTStatisticTable(List<StatisticRST> _rstStatistic)
        {
            // 去除统计数为0的分层的统计数据
            for(int i = 0; i < _rstStatistic.Count / 14; i++)
            {
                bool isZero = true;

                for(int j = 0; j < 14; j++)
                {
                    if (_rstStatistic[j + i * 14].Count > 0)
                    {
                        isZero = false;
                    }
                }

                if (isZero)
                {
                    for (int j = 13; j > -1; j--)
                    {
                        _rstStatistic.RemoveAt(j + i * 14);
                    }
                }
            }

            // 填写表格标题
            Doc.Paragraphs.Last.Range.Text = "表1 土工常规试验成果统计表";
            Doc.Paragraphs.Last.Range.Font.Bold = 1;
            Doc.Paragraphs.Last.Range.Font.Size = 12;
            App.Selection.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            object unite = MSWord.WdUnits.wdStory;
            App.Selection.EndKey(ref unite, ref Nothing);

            // 定义表格对象
            MSWord.Table table = Tables.Add(App.Selection.Range, _rstStatistic.Count / 14 * 8 + 1, 16, ref Nothing, ref Nothing);

            // 填充列标题
            string[] rowheader = new string[]
            {
                "层号及名称",
                "统\n计\n名\n称",
                "含\n水\n量",
                "天\n然\n密\n度",
                "比\n重",
                "孔\n隙\n比",
                "饱\n和\n度",
                "液\n限",
                "塑\n限",
                "塑\n性\n指\n数",
                "液\n性\n指\n数",
                "压\n缩\n系\n数",
                "压\n缩\n模\n量",
                "内\n摩\n擦\n角",
                "粘\n聚\n力",
                "渗\n透\n系\n数"
            };
            for (int i = 1; i <= table.Columns.Count; i++)
            {
                table.Cell(1, i).Range.Text = rowheader[i - 1];
            }

            // 设置文档格式
            Doc.PageSetup.LeftMargin = 50F;
            Doc.PageSetup.RightMargin = 50F;
            
            // 设置表格格式
            table.Select();
            App.Selection.Tables[1].Rows.Alignment = WdRowAlignment.wdAlignRowCenter;

            foreach(Row row in table.Rows)
            {
                row.Range.Bold = 0;
            }
            table.Rows[1].Range.Bold = 1;
            for (int i = 0; i < _rstStatistic.Count / 14; i++)
            {
                table.Rows[5 + i * 8].Range.Bold = 1;
                table.Rows[9 + i * 8].Range.Bold = 1;
            }

            table.Rows[1].Range.ParagraphFormat.LineSpacingRule = WdLineSpacing.wdLineSpaceExactly;
            for(int i = 1; i <= table.Rows.Count; i++)
            {
                table.Cell(i, 1).Range.ParagraphFormat.LineSpacingRule = WdLineSpacing.wdLineSpaceExactly;
            }

            table.Range.Font.Size = 9.0F;

            table.Range.ParagraphFormat.Alignment = MSWord.WdParagraphAlignment.wdAlignParagraphCenter;
            table.Range.Cells.VerticalAlignment = MSWord.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

            table.Borders.OutsideLineStyle = MSWord.WdLineStyle.wdLineStyleDouble;
            table.Borders.InsideLineStyle = MSWord.WdLineStyle.wdLineStyleSingle;

            float[] columnWidth = new float[] { 20, 50, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 40 };
            for (int i = 1; i <= table.Columns.Count; i++)
            {
                table.Columns[i].Width = columnWidth[i - 1];
            }

            // 填充试验数据
            for(int i = 0; i < _rstStatistic.Count / 14; i++)
            {
                int startRow = 2 + i * 8;

                table.Cell(startRow, 1).Range.Text = _rstStatistic[i * 14].Layer + " " + _rstStatistic[i * 14].Name;

                string[] type = new string[] { "统计数", "最大值", "最小值", "平均值", "标准差", "变异系数", "修正系数", "标准值" };
                for(int j = 0; j < 8; j++)
                {
                    table.Cell(startRow + j, 2).Range.Text = type[j];
                }

                for(int j = 0; j < 14; j++)
                {
                    if(j == 0 || j == 4 || j == 5 || j == 6 || j == 7 || j == 10 || j == 11 || j == 12)
                    {
                        table.Cell(startRow, 3 + j).Range.Text = _rstStatistic[j + i * 14].Count.ToString() == "-0.19880205" ? "/" : _rstStatistic[j + i * 14].Count.ToString("0");
                        table.Cell(startRow + 1, 3 + j).Range.Text = _rstStatistic[j + i * 14].Max.ToString() == "-0.19880205" ? "/" : _rstStatistic[j + i * 14].Max.ToString("0.0");
                        table.Cell(startRow + 2, 3 + j).Range.Text = _rstStatistic[j + i * 14].Min.ToString() == "-0.19880205" ? "/" : _rstStatistic[j + i * 14].Min.ToString("0.0");
                        table.Cell(startRow + 3, 3 + j).Range.Text = _rstStatistic[j + i * 14].Average.ToString() == "-0.19880205" ? "/" : _rstStatistic[j + i * 14].Average.ToString("0.0");
                        table.Cell(startRow + 4, 3 + j).Range.Text = _rstStatistic[j + i * 14].StandardDeviation.ToString() == "-0.19880205" ? "/" : _rstStatistic[j + i * 14].StandardDeviation.ToString("0.0");
                        table.Cell(startRow + 5, 3 + j).Range.Text = _rstStatistic[j + i * 14].VariableCoefficient.ToString() == "-0.19880205" ? "/" : _rstStatistic[j + i * 14].VariableCoefficient.ToString("0.00");
                        table.Cell(startRow + 6, 3 + j).Range.Text = _rstStatistic[j + i * 14].CorrectionCoefficient.ToString() == "-0.19880205" ? "/" : _rstStatistic[j + i * 14].CorrectionCoefficient.ToString("0.00");
                        table.Cell(startRow + 7, 3 + j).Range.Text = _rstStatistic[j + i * 14].StandardValue.ToString() == "-0.19880205" ? "/" : _rstStatistic[j + i * 14].StandardValue.ToString("0.0");
                    }
                    else if(j == 1 || j == 2 || j == 3 || j == 8 || j == 9)
                    {
                        table.Cell(startRow, 3 + j).Range.Text = _rstStatistic[j + i * 14].Count.ToString() == "-0.19880205" ? "/" : _rstStatistic[j + i * 14].Count.ToString("0");
                        table.Cell(startRow + 1, 3 + j).Range.Text = _rstStatistic[j + i * 14].Max.ToString() == "-0.19880205" ? "/" : _rstStatistic[j + i * 14].Max.ToString("0.00");
                        table.Cell(startRow + 2, 3 + j).Range.Text = _rstStatistic[j + i * 14].Min.ToString() == "-0.19880205" ? "/" : _rstStatistic[j + i * 14].Min.ToString("0.00");
                        table.Cell(startRow + 3, 3 + j).Range.Text = _rstStatistic[j + i * 14].Average.ToString() == "-0.19880205" ? "/" : _rstStatistic[j + i * 14].Average.ToString("0.00");
                        table.Cell(startRow + 4, 3 + j).Range.Text = _rstStatistic[j + i * 14].StandardDeviation.ToString() == "-0.19880205" ? "/" : _rstStatistic[j + i * 14].StandardDeviation.ToString("0.00");
                        table.Cell(startRow + 5, 3 + j).Range.Text = _rstStatistic[j + i * 14].VariableCoefficient.ToString() == "-0.19880205" ? "/" : _rstStatistic[j + i * 14].VariableCoefficient.ToString("0.00");
                        table.Cell(startRow + 6, 3 + j).Range.Text = _rstStatistic[j + i * 14].CorrectionCoefficient.ToString() == "-0.19880205" ? "/" : _rstStatistic[j + i * 14].CorrectionCoefficient.ToString("0.00");
                        table.Cell(startRow + 7, 3 + j).Range.Text = _rstStatistic[j + i * 14].StandardValue.ToString() == "-0.19880205" ? "/" : _rstStatistic[j + i * 14].StandardValue.ToString("0.00");
                    }
                    else
                    {
                        table.Cell(startRow, 3 + j).Range.Text = _rstStatistic[j + i * 14].Count.ToString() == "-0.19880205" ? "/" : _rstStatistic[j + i * 14].Count.ToString("0");
                        table.Cell(startRow + 1, 3 + j).Range.Text = _rstStatistic[j + i * 14].Max.ToString() == "-0.19880205" ? "/" : _rstStatistic[j + i * 14].Max.ToString("0.0E0");
                        table.Cell(startRow + 2, 3 + j).Range.Text = _rstStatistic[j + i * 14].Min.ToString() == "-0.19880205" ? "/" : _rstStatistic[j + i * 14].Min.ToString("0.0E0");
                        table.Cell(startRow + 3, 3 + j).Range.Text = _rstStatistic[j + i * 14].Average.ToString() == "-0.19880205" ? "/" : _rstStatistic[j + i * 14].Average.ToString("0.0E0");
                        table.Cell(startRow + 4, 3 + j).Range.Text = _rstStatistic[j + i * 14].StandardDeviation.ToString() == "-0.19880205" ? "/" : _rstStatistic[j + i * 14].StandardDeviation.ToString("0.0E0");
                        table.Cell(startRow + 5, 3 + j).Range.Text = _rstStatistic[j + i * 14].VariableCoefficient.ToString() == "-0.19880205" ? "/" : _rstStatistic[j + i * 14].VariableCoefficient.ToString("0.00");
                        table.Cell(startRow + 6, 3 + j).Range.Text = _rstStatistic[j + i * 14].CorrectionCoefficient.ToString() == "-0.19880205" ? "/" : _rstStatistic[j + i * 14].CorrectionCoefficient.ToString("0.00");
                        table.Cell(startRow + 7, 3 + j).Range.Text = _rstStatistic[j + i * 14].StandardValue.ToString() == "-0.19880205" ? "/" : _rstStatistic[j + i * 14].StandardValue.ToString("0.0E0");
                    }
                }
            }

            // 合并层号及名称单元格
            for (int i = 0; i < _rstStatistic.Count / 14; i++)
            {
                table.Cell(2 + i * 8, 1).Merge(table.Cell(9 + i * 8, 1));
            }

            // 返回
            return table;
        }

        public MSWord.Table AddGATStatisticTable(List<StatisticGAT> _gatStatistic)
        {
            // 填写表格标题
            Doc.Paragraphs.Last.Range.Text = "表4 颗粒分析试验成果统计表";
            Doc.Paragraphs.Last.Range.Font.Bold = 1;
            Doc.Paragraphs.Last.Range.Font.Size = 12;
            App.Selection.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            object unite = MSWord.WdUnits.wdStory;
            App.Selection.EndKey(ref unite, ref Nothing);

            // 定义表格对象
            MSWord.Table table = Tables.Add(App.Selection.Range, _gatStatistic.Count+2, 7, ref Nothing, ref Nothing);

            // 填充列标题
            table.Cell(1, 1).Range.Text = "分层编号及名称";
            table.Cell(1, 2).Range.Text = "粒组分布百分含量（%）";
            string[] rowheader = new string[]
            {
                ">20mm",
                "20~2mm",
                "2~0.5mm",
                "0.5~0.25mm",
                "0.25~0.075mm",
                "<0.075mm"
            };
            for (int i = 1; i < table.Columns.Count; i++)
            {
                table.Cell(2, i + 1).Range.Text = rowheader[i - 1];
            }

            // 设置文档格式
            Doc.PageSetup.LeftMargin = 50F;
            Doc.PageSetup.RightMargin = 50F;

            // 设置表格格式
            table.Select();
            App.Selection.Tables[1].Rows.Alignment = WdRowAlignment.wdAlignRowCenter;

            foreach (Row row in table.Rows)
            {
                row.Range.Bold = 0;
            }
            table.Rows[1].Range.Bold = 1;
            table.Rows[2].Range.Bold = 1;
            
            table.Range.Font.Size = 10.0F;

            table.Range.ParagraphFormat.Alignment = MSWord.WdParagraphAlignment.wdAlignParagraphCenter;
            table.Range.Cells.VerticalAlignment = MSWord.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

            table.Borders.OutsideLineStyle = MSWord.WdLineStyle.wdLineStyleDouble;
            table.Borders.InsideLineStyle = MSWord.WdLineStyle.wdLineStyleSingle;

            float[] columnWidth = new float[] { 100, 50, 60, 60, 80, 80, 70 };
            for (int i = 1; i <= table.Columns.Count; i++)
            {
                table.Columns[i].Width = columnWidth[i - 1];
            }

            // 填充试验数据
            for (int i = 0; i < _gatStatistic.Count; i++)
            {
                table.Cell(i + 3, 1).Range.Text = _gatStatistic[i].Layer + _gatStatistic[i].Name;
                table.Cell(i + 3, 2).Range.Text = _gatStatistic[i].Group100To20.ToString() == "-0.19880205" ? "/" : _gatStatistic[i].Group100To20.ToString("0.0");
                table.Cell(i + 3, 3).Range.Text = _gatStatistic[i].Group20To2.ToString() == "-0.19880205" ? "/" : _gatStatistic[i].Group20To2.ToString("0.0");
                table.Cell(i + 3, 4).Range.Text = _gatStatistic[i].Group2To0_5.ToString() == "-0.19880205" ? "/" : _gatStatistic[i].Group2To0_5.ToString("0.0");
                table.Cell(i + 3, 5).Range.Text = _gatStatistic[i].Group0_5To0_25.ToString() == "-0.19880205" ? "/" : _gatStatistic[i].Group0_5To0_25.ToString("0.0");
                table.Cell(i + 3, 6).Range.Text = _gatStatistic[i].Group0_25To0_075.ToString() == "-0.19880205" ? "/" : _gatStatistic[i].Group0_25To0_075.ToString("0.0");
                table.Cell(i + 3, 7).Range.Text = _gatStatistic[i].Group0_075To0.ToString() == "-0.19880205" ? "/" : _gatStatistic[i].Group0_075To0.ToString("0.0");
            }

            // 合并层号及名称单元格
            table.Cell(1, 1).Merge(table.Cell(2, 1));
            table.Cell(1, 2).Merge(table.Cell(1, 7));

            // 返回
            return table;
        }
    }
}
