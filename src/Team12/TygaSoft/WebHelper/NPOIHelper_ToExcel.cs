using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.HPSF;
using NPOI.SS.Formula.Functions;

namespace TygaSoft.SysHelper
{
    #region 表示一个单元格的位置
    /// <summary>
    /// 表示一个单元格的位置
    /// </summary>
    public class Cellocation
    {
        /// <summary>
        /// X轴
        /// </summary>
        public int X { get; set; }
        /// <summary>
        /// Y轴
        /// </summary>
        public int Y { get; set; }
        /// <summary>
        /// 创建一个单元格位置对象
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Cellocation(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

    }
    #endregion

    #region 合并单元格的对象
    /// <summary>
    /// 合并单元格的对象
    /// </summary>
    public class MergeCell
    {
        /// <summary>
        /// 自定义分组内容
        /// </summary>
        private string _Custom = "";
        /// <summary>
        /// 开始单元格
        /// </summary>
        public Cellocation StartCellocation { get; set; }
        /// <summary>
        /// 结束的单元格
        /// </summary>
        public Cellocation EndCellocation { get; set; }
        /// <summary>
        /// 分组内容依据
        /// </summary>
        public MergeType IMergeType { get; set; }
        /// <summary>
        /// 自定义分组内容
        /// </summary>
        public string Custom { get { return _Custom; } set { _Custom = value; } }
        /// <summary>
        /// 创建一个单元格位置对象
        /// </summary>
        /// <param name="startCellocation">开始单元格</param>
        /// <param name="endCellocation">结束单元格</param>
        /// <param name="mergeType">分组内容依据</param>
        public MergeCell(Cellocation startCellocation, Cellocation endCellocation, MergeType mergeType)
        {
            this.StartCellocation = startCellocation;
            this.EndCellocation = endCellocation;
            this.IMergeType = mergeType;
        }
        /// <summary>
        /// 创建一个单元格位置对象
        /// </summary>
        /// <param name="startCellocation">开始单元格</param>
        /// <param name="endCellocation">结束单元格</param>
        /// <param name="mergeType">分组内容依据</param>
        /// <param name="custom">自定义分组内容</param>
        public MergeCell(Cellocation startCellocation, Cellocation endCellocation, MergeType mergeType, string custom)
        {
            this.StartCellocation = startCellocation;
            this.EndCellocation = endCellocation;
            this.IMergeType = mergeType;
            this.Custom = custom;
        }

    }
    #endregion

    #region 分组内容依据
    /// <summary>
    /// 分组内容依据
    /// </summary>
    public enum MergeType
    {
        /// <summary>
        /// 取第一个单元格的内容为分组依据
        /// </summary>
        TakeFirstText,
        /// <summary>
        /// 汇总所有不同的内容作为分组依据
        /// </summary>
        GroupByText,
        /// <summary>
        /// 合并所有单元格内容作为分组依据
        /// </summary>
        MergeText,
        /// <summary>
        /// 自定义分组
        /// </summary>
        Custom
    }
    #endregion

    #region ExcelColumnWidth
    /// <summary>
    /// 自定义列表宽度
    /// </summary>
    public class ExcelColumnWidth
    {
        /// <summary>
        /// 电子表格索引位置
        /// </summary>
        public int ColumnIndex { set; get; }
        /// <summary>
        /// 自定义宽度
        /// </summary>
        public int ColumnWidth { set; get; }
        /// <summary>
        /// 自定义列表宽度
        /// </summary>
        /// <param name="_ColumnIndex"></param>
        /// <param name="_ColumnWidth"></param>
        public ExcelColumnWidth(int _ColumnIndex, int _ColumnWidth)
        {
            this.ColumnIndex = _ColumnIndex;
            this.ColumnWidth = _ColumnWidth;
        }
    }
    #endregion


    /// <summary>
    /// 根据数据源导出Excel
    /// </summary>
    public class NPOIHelper_ToExcel
    {
        #region 属性
        /// <summary>
        /// 需要合并的标记
        /// </summary>
        private List<MergeCell> _ListMergeCell;
        /// <summary>
        /// 设置数据源的标题索引
        /// </summary>
        private int _IndexSourceTitle = -1;
        /// <summary>
        /// 从第0行开始算 到第几行作为表头(不设置默认数据源的标头)
        /// </summary>
        public int IndexSourceTitle { get { return _IndexSourceTitle; } set { _IndexSourceTitle = value; } }
        /// <summary>
        /// 设置数据源
        /// </summary>
        private DataTable dtSource { get; set; }
        /// <summary>
        /// 生成的文件名称
        /// </summary>
        private string FileName { get; set; }
        /// <summary>
        /// 需要合并的标记
        /// </summary>
        private List<MergeCell> ListMergeCell { get { return _ListMergeCell; } set { _ListMergeCell = value; } }
        /// <summary>
        /// 当前列数
        /// </summary>
        public int Columnslength = 0;
        /// <summary>
        /// 当前行数
        /// </summary>
        public int Rowlength = 0;
        /// <summary>
        /// 顶部大表头
        /// </summary>
        public string HeaderText { get; set; }
        /// <summary>
        /// 工作簿名称
        /// </summary>
        public string SheetName { get; set; }
        /// <summary>
        /// 当前电子表格操作的索引
        /// </summary>
        private int SheetRowIndex = 0;
        /// <summary>
        /// 自动分组字段(中文)
        /// </summary>
        private List<string> GroupByName = new List<string>();
        /// <summary>
        /// 自动分组字段(下标)
        /// </summary>
        private List<int> GroupByIndex = new List<int>();
        /// <summary>
        /// 自定义列表宽度
        /// </summary>
        private List<ExcelColumnWidth> ListExcelColumnWidth = new List<ExcelColumnWidth>();

        /// <summary>
        /// 开始数据
        /// </summary>
        private int startIndexZone = 0;
        /// <summary>
        /// 结束数据
        /// </summary>
        private int endIndexZone = 0;


        #endregion

        #region 添加自定义合并列
        /// <summary>
        /// 添加自定义合并列(注意合并单元的Y坐标将从数据源的0坐标开始 既包括了设计标题行)
        /// </summary>
        /// <param name="Start_X">开始单元格X轴</param>
        /// <param name="Start_Y">开始单元格Y轴</param>
        /// <param name="End_X">结束单元格X轴</param>
        /// <param name="End_Y">结束单元格Y轴</param>
        /// <param name="IMergeType">分组内容依据</param>
        /// <param name="Custom">自定义分组内容(可以为空)</param>
        public void AddMergeCell(int Start_X, int Start_Y, int End_X, int End_Y, MergeType IMergeType, string Custom)
        {
            MergeCell mergeCell = new MergeCell(new Cellocation(Start_X, Start_Y), new Cellocation(End_X, End_Y), IMergeType, Custom);
            this.ListMergeCell.Add(mergeCell);
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ExceldtSource">数据源</param>
        /// <param name="ExcelFileName">文件名称</param>
        public NPOIHelper_ToExcel(DataTable ExceldtSource, string ExcelFileName)
        {
            ListMergeCell = new List<MergeCell>();
            this.dtSource = ExceldtSource;
            this.FileName = ExcelFileName;
            Columnslength = ExceldtSource.Columns.Count;
            Rowlength = ExceldtSource.Rows.Count;
        }
        #endregion

        #region 添加分组的列
        /// <summary>
        /// 添加汇总的列名
        /// </summary>
        /// <param name="Name"></param>
        public void AddGroupByName(string Name)
        {
            GroupByName.Add(Name);
        }
        /// <summary>
        /// 添加汇总的列名
        /// </summary>
        /// <param name="Name"></param>
        public void AddGroupByName(int index)
        {
            GroupByIndex.Add(index);
        }
        #endregion

        #region 添加自定义宽度的列
        /// <summary>
        /// 添加自定义宽度的列
        /// </summary>
        /// <param name="ColumnIndex">列索引</param>
        /// <param name="ColumnWidth">宽度*600</param>
        public void addExcelColumnWidth(int ColumnIndex, int ColumnWidth)
        {
            ListExcelColumnWidth.Add(new ExcelColumnWidth(ColumnIndex, ColumnWidth));
        }
        #endregion

        /// <summary>
        /// 导出CSV
        /// </summary>
        public void ExportExcelByWeb()
        {
            ExportExcelByWeb(true);
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        public void ExportExcelByWebAsExcel()
        {
            ExportExcelByWeb(false);
        }

        #region 用于Web导出到EXCEL
        /// <summary>
        /// 用于Web导出
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="headersTitle">列标题信息集合</param>
        /// <param name="strHeaderText">表头文本</param>
        /// <param name="strFileName">文件名</param>
        public void ExportExcelByWeb(bool shouldBeCSV)
        {
            HttpContext curContext = HttpContext.Current;
            byte[] content;
            if (curContext.Request.UserAgent.ToLower().IndexOf("msie") > -1)
            {
                FileName = HttpUtility.UrlPathEncode(FileName);
            }
            if (shouldBeCSV)
            {
                FileName = FileName + ".csv";
                content = GetCVSBuffer();
            }
            else
            {
                FileName = FileName + ".xls";
                content = GetExcelBuffer();
            }

            this.dtSource.Clear();
            this.dtSource = null;

            // 设置编码和附件格式
            curContext.Response.Buffer = true;
            curContext.Response.ClearContent();
            curContext.Response.ClearHeaders();
            curContext.Response.ContentType = "application/ms-excel";
            curContext.Response.ContentEncoding = Encoding.UTF8;
            curContext.Response.Charset = "utf-8";
            curContext.Response.AppendHeader("Content-Disposition", "attachment;filename=" + FileName);
            curContext.Response.BinaryWrite(content);
            curContext.Response.Flush();
            curContext.Response.Close();  
        }
        #endregion

        /// <summary>
        /// 获取CVS Buffer
        /// </summary>
        /// <returns></returns>
        private byte[] GetCVSBuffer()
        {
            return GetExportBuffer(true);
        }

        /// <summary>
        /// 获取Excel Buffer
        /// </summary>
        /// <returns></returns>
        private byte[] GetExcelBuffer()
        {
            return GetExportBuffer(false);
        }

        #region DataTable导出到Excel的MemoryStream
        /// <summary>
        /// DataTable导出到Excel的MemoryStream
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        private byte[] GetExportBuffer(bool shouldBeCVS)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = null;
            if (SheetName != null)
            {
                sheet = workbook.CreateSheet(SheetName);
            }
            else
            {
                sheet = workbook.CreateSheet();
            }
            //添加汇总列名称
            //foreach (string item in GroupByName)
            //{
            //    GroupByIndex.Add(dtSource.Columns[item].Ordinal);
            //}

            //设置文件的属性信息
            SetInformation(workbook);
            //获取表头样式
            ICellStyle HeaderCellStyle = CreateHeaderCellStyle(workbook);
            //获取内容样式
            ICellStyle ItemCellStyle = CreateCellStyle(workbook);
            //获取内容样式
            ICellStyle intCellStyle = CreateIntStyle(workbook);
            //制作大表头
            CreateFirstHeader(workbook, sheet);
            //制作标题
            CreateExcelTitle(workbook, sheet, HeaderCellStyle);
            //填充内容
            InsertCell(workbook, sheet, ItemCellStyle, intCellStyle, shouldBeCVS);
            //合并单元格
            SetMergeCell(workbook, sheet);
            //分组单元格
            SetGroupByName(workbook, sheet, ItemCellStyle);

            if (shouldBeCVS)
            {
                return GetCSVBytes(workbook);
            }
            else
            {
                return GetExcelBytes(workbook);
            }
        }

        /// <summary>
        /// 获取 Excel Bytes
        /// </summary>
        /// <param name="workbook"></param>
        /// <returns></returns>
        private byte[] GetExcelBytes(HSSFWorkbook workbook)
        {
            int len = GetStreamLength(workbook);

            using (MemoryStream ms = new MemoryStream(len))
            {
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;

                //Release the Memery by Richard
                workbook = null;

                return ms.GetBuffer();
            }
        }

        /// <summary>
        /// 获取 CSV Bytes
        /// </summary>
        /// <param name="workbook"></param>
        /// <returns></returns>
        private byte[] GetCSVBytes(HSSFWorkbook workbook)
        {
            ISheet sheet = workbook.GetSheetAt(0);

            var content = GetIEnumerable(sheet)
                .Select(row => string.Join(",", GetRowIEnumerable(row).Select(cell => cell.ToString())));


            List<byte> bytes = new List<byte>();

            foreach (string item in content)
            {
                byte[] line = System.Text.Encoding.Default.GetBytes(item + Environment.NewLine);
                bytes.AddRange(line);
            }

            //Release the Memery by Richard
            sheet = null;
            //Release the Memery by Richard
            workbook = null;

            return bytes.ToArray<byte>();
        }
      
        /// <summary>
        /// 将Sheet表转化为IEnumerable
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        public static IEnumerable<NPOI.SS.UserModel.IRow> GetIEnumerable(ISheet sheet)
        {
            //IEnumerable<NPOI.SS.UserModel.IRow> result = new Enumerable<NPOI.SS.UserModel.Row>();
            var itor = sheet.GetRowEnumerator();
            while (itor.MoveNext())
            {
                yield return (NPOI.SS.UserModel.IRow)itor.Current;
            }
        }

        /// <summary>
	    /// 将Excel的行转化为IEnumerable
	    /// </summary>
	    /// <param name="row"></param>
	    /// <returns></returns>
	    public static IEnumerable<ICell> GetRowIEnumerable(NPOI.SS.UserModel.IRow row)
	    {
	        var itor = row.Cells.GetEnumerator();//.GetCellEnumerator();
	        while (itor.MoveNext())
	        {
	            yield return (ICell)itor.Current;
	        }
	    }
        /// <summary>
        /// 获取文档的内存控件
        /// </summary>
        /// <param name="workbook"></param>
        /// <returns></returns>
        private static int GetStreamLength(HSSFWorkbook workbook)
        {
            long len = 0;
            using (MemoryStream ms = new MemoryStream())
            {

                workbook.Write(ms);
                len = ms.Length;
            }

            return (int)Math.Min(int.MaxValue, len);
        }
        #endregion

        #region 创建顶部表头
        /// <summary>
        /// 创建顶部表头
        /// </summary>
        /// <param name="workbook">当前工作区域</param>
        /// <param name="sheet">区域选项卡</param>
        private void CreateFirstHeader(HSSFWorkbook workbook, ISheet sheet)
        {
            if (HeaderText != null)
            {
                IRow headerRow = sheet.CreateRow(0);
                headerRow.HeightInPoints = 25;
                //设置值
                headerRow.CreateCell(0).SetCellValue(HeaderText);
                //设置样式
                ICellStyle headStyle = workbook.CreateCellStyle();
                //居中
                headStyle.Alignment = HorizontalAlignment.Center;
                //字体
                IFont font = workbook.CreateFont();
                font.FontHeightInPoints = 20;
                //粗度
                font.Boldweight = 700;
                //字体
                headStyle.SetFont(font);
                //样式
                headerRow.GetCell(0).CellStyle = headStyle;
                //合并
                sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, Columnslength - 1));
                ++SheetRowIndex;
            }


        }
        #endregion

        #region 创建标题
        /// <summary>
        /// 创建标题
        /// </summary>
        /// <param name="workbook">工作区域</param>
        /// <param name="sheet">工作簿</param>
        /// <param name="HeaderCellStyle">标题样式</param>
        private void CreateExcelTitle(HSSFWorkbook workbook, ISheet sheet, ICellStyle HeaderCellStyle)
        {
            if (!string.IsNullOrEmpty(HeaderText)) { startIndexZone = startIndexZone + 1; }
            if (IndexSourceTitle != -1 && dtSource.Rows.Count > IndexSourceTitle)
            {
                for (int i = 0; i <= IndexSourceTitle; i++)
                {
                    IRow headerRow = sheet.CreateRow(SheetRowIndex);
                    for (int j = 0; j < dtSource.Columns.Count; j++)
                    {
                        //获取单元格
                        ICell icell = headerRow.CreateCell(j);
                        icell.CellStyle = HeaderCellStyle;
                        string Value = dtSource.Rows[i][j].ToString();
                        SetCellValue(icell, "System.String", Value, HeaderCellStyle, null);
                        int ColumnWidth = Encoding.GetEncoding(936).GetBytes(Value.ToString()).Length;
                        sheet.SetColumnWidth(j, ColumnWidth * 400);

                    }
                    ++SheetRowIndex;
                }
                startIndexZone = startIndexZone + IndexSourceTitle + 1;
            }
            else
            {
                IRow headerRow = sheet.CreateRow(SheetRowIndex);
                for (int i = 0; i < dtSource.Columns.Count; i++)
                {
                    //获取单元格
                    ICell icell = headerRow.CreateCell(i);
                    icell.CellStyle = HeaderCellStyle;
                    string ColumnName = dtSource.Columns[i].ColumnName;
                    SetCellValue(icell, "System.String", ColumnName, HeaderCellStyle, null);
                    int ColumnWidth = Encoding.GetEncoding(936).GetBytes(ColumnName.ToString()).Length;

                    sheet.SetColumnWidth(i, ColumnWidth * 600);
                }

                ++SheetRowIndex;
                startIndexZone = startIndexZone + 1;
            }


            foreach (ExcelColumnWidth item in ListExcelColumnWidth)
            {
                sheet.SetColumnWidth(item.ColumnIndex, item.ColumnWidth * 600);
            }
        }
        #endregion

        #region 遍历数据填充单元格
        /// <summary>
        /// 遍历数据填充单元格
        /// </summary>
        /// <param name="workbook">工作区域</param>
        /// <param name="sheet">工作簿</param>
        /// <param name="ItemCellStyle">单元格样式</param>
        private void InsertCell(HSSFWorkbook workbook, ISheet sheet, ICellStyle ItemCellStyle, ICellStyle intStyle, bool shouldBeCVS)
        {

            int startdtSourceRowIndex = 0;
            //如果设置了数据源的标题行
            if (IndexSourceTitle != -1)
            {
                //数据源的有效数据将从当前标题行的下一行开始
                startdtSourceRowIndex = IndexSourceTitle + 1;
            }
            for (int i = startdtSourceRowIndex; i < dtSource.Rows.Count; i++)
            {

                IRow headerRow = sheet.CreateRow(SheetRowIndex);
                for (int j = 0; j < dtSource.Columns.Count; j++)
                {
                    //获取单元格
                    ICell icell = headerRow.CreateCell(j);
                    icell.CellStyle = ItemCellStyle;
                    string Value = dtSource.Rows[i][j].ToString();
                    if (shouldBeCVS && Value.IndexOf(",") > -1)
                    {
                        Value = string.Format("\"{0}\"", Value);
                    }
                    SetCellValue(icell, dtSource.Columns[j].DataType.ToString(), Value, ItemCellStyle, intStyle);
                    sheet.SetColumnWidth(j, 10 * 256);
                }
                ++SheetRowIndex;
                endIndexZone = SheetRowIndex;
            }

        }
        #endregion

        #region 合并单元格
        /// <summary>
        /// 合并单元格
        /// </summary>
        /// <param name="workbook">工作区域</param>
        /// <param name="sheet">工作簿</param>
        private void SetMergeCell(HSSFWorkbook workbook, ISheet sheet)
        {
            foreach (MergeCell itemCell in ListMergeCell)
            {
                Cellocation StartCellocation = itemCell.StartCellocation;
                Cellocation EndCellocation = itemCell.EndCellocation;
                if (HeaderText != null)
                {
                    StartCellocation.X += 1;
                    EndCellocation.X += 1;
                }
                if (EndCellocation.X >= StartCellocation.X && EndCellocation.Y >= StartCellocation.Y)
                {
                    string GroupName = "";
                    if (itemCell.IMergeType == MergeType.Custom)
                    {
                        GroupName = itemCell.Custom;
                    }
                    else if (itemCell.IMergeType == MergeType.GroupByText)
                    {
                        GroupName = ",";
                        for (int y1 = StartCellocation.Y; y1 <= EndCellocation.Y; y1++)
                        {
                            for (int x1 = StartCellocation.X; x1 <= EndCellocation.X; x1++)
                            {
                                string StringCellValue = sheet.GetRow(y1).GetCell(x1).StringCellValue;
                                if (!GroupName.Contains("," + StringCellValue + ","))
                                {
                                    GroupName += StringCellValue + ",";
                                }
                            }
                        }
                        if (GroupName.Length != 1)
                        {
                            GroupName = GroupName.Substring(1, GroupName.Length - 1);
                            GroupName = GroupName.Substring(0, GroupName.Length - 1);
                        }


                    }
                    else if (itemCell.IMergeType == MergeType.MergeText)
                    {
                        for (int y1 = StartCellocation.Y; y1 <= EndCellocation.Y; y1++)
                        {
                            for (int x1 = StartCellocation.X; x1 <= EndCellocation.X; x1++)
                            {
                                string StringCellValue = sheet.GetRow(y1).GetCell(x1).StringCellValue;
                                if (!GroupName.Contains("," + StringCellValue + ","))
                                {
                                    GroupName += StringCellValue + ",";
                                }
                            }
                        }
                        if (GroupName.Length != 0)
                        {
                            GroupName = GroupName.Substring(0, GroupName.Length - 1);
                        }
                    }
                    else if (itemCell.IMergeType == MergeType.TakeFirstText)
                    {
                        GroupName = sheet.GetRow(StartCellocation.X).GetCell(StartCellocation.Y).StringCellValue;
                    }
                    CellRangeAddress cellRangeAddress = new CellRangeAddress(StartCellocation.X, EndCellocation.X, StartCellocation.Y, EndCellocation.Y);
                    sheet.AddMergedRegion(cellRangeAddress);
                    ICell newCell = sheet.GetRow(StartCellocation.X).GetCell(StartCellocation.Y);
                    SetCellValue(newCell, "", GroupName, newCell.CellStyle, null);
                }

            }

        }

        #endregion

        #region 给单元格填充值
        /// <summary>
        /// 给单元格填充值
        /// </summary>
        /// <param name="newCell">当前单元格</param>
        /// <param name="ColumnsType">数据格式</param>
        /// <param name="drValue">值</param>
        /// <param name="dataStyle">单元格样式</param>
        private void SetCellValue(ICell newCell, string ColumnsType, string drValue, ICellStyle dataStyle, ICellStyle intStyle)
        {
            //newCell.CellStyle = dataStyle;
            //newCell.SetCellValue(drValue);
            //ICellStyle intStyle = workbook.CreateCellStyle();
            //IDataFormat intFormat = workbook.CreateDataFormat();
            //intStyle.DataFormat = intFormat.GetFormat("0.00_ ");
            //intStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.THIN;
            //intStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.THIN;
            //intStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.THIN;
            //intStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.THIN;
            //intStyle.VerticalAlignment = VerticalAlignment.CENTER;
            switch (ColumnsType.ToString())
            {
                case "System.String":
                    newCell.SetCellValue(drValue);

                    break;
                case "System.DateTime":
                    DateTime dateV;
                    DateTime.TryParse(drValue, out dateV);
                    newCell.SetCellValue(dateV);
                    newCell.CellStyle = dataStyle;
                    break;
                case "System.Boolean":
                    bool boolV = false;
                    bool.TryParse(drValue, out boolV);
                    newCell.SetCellValue(boolV);
                    break;
                case "System.Int16":
                case "System.Int32":
                case "System.Int64":
                    double doubVs = 0;
                    double.TryParse(drValue, out doubVs);
                    newCell.SetCellValue(doubVs);
                    newCell.CellStyle = intStyle;
                    break;
                case "System.Byte":
                    int intV = 0;
                    try
                    {
                        intV = int.Parse(drValue);
                    }
                    catch
                    {
                        intV = 0;
                    }

                    newCell.SetCellValue(intV);
                    break;
                case "System.Decimal":
                case "System.Double":
                    double doubV = 0;
                    double.TryParse(drValue, out doubV);
                    newCell.SetCellValue(doubV);
                    newCell.CellStyle = intStyle;
                    break;
                case "System.DBNull":
                    newCell.SetCellValue("");
                    break;
                default:
                    newCell.SetCellValue("");
                    break;
            }
        }
        #endregion

        #region 创建表头样式
        /// <summary>
        /// 创建表头样式
        /// </summary>
        /// <param name="workbook"></param>
        /// <returns></returns>
        private ICellStyle CreateHeaderCellStyle(HSSFWorkbook workbook)
        {
            ICellStyle headStyle = workbook.CreateCellStyle();
            headStyle.Alignment = HorizontalAlignment.Center;
            headStyle.VerticalAlignment = VerticalAlignment.Center;
            // 填充列的背景色
            //headStyle.FillForegroundColor = HSSFColor.GREY_50_PERCENT.GREY_25_PERCENT.index;
            //headStyle.FillPattern = FillPatternType.SOLID_FOREGROUND;
            //headStyle.VerticalAlignment = VerticalAlignment.CENTER;
            //headStyle.WrapText = true;
            //设置样式的字体
            IFont font = workbook.CreateFont();
            font.FontHeightInPoints = 10;
            font.Boldweight = 1000;
            headStyle.SetFont(font);
            return headStyle;
        }
        #endregion

        #region 创建单元格样式
        /// <summary>
        /// 创建单元格样式
        /// </summary>
        /// <param name="workbook"></param>
        /// <returns></returns>
        private ICellStyle CreateCellStyle(HSSFWorkbook workbook)
        {
            ICellStyle dateStyle = workbook.CreateCellStyle();
            IDataFormat format = workbook.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");
            dateStyle.Alignment = HorizontalAlignment.Center;
            dateStyle.VerticalAlignment = VerticalAlignment.Center;
            dateStyle.WrapText = true;
            return dateStyle;
        }


        private ICellStyle CreateIntStyle(HSSFWorkbook workbook)
        {
            ICellStyle dateStyle = workbook.CreateCellStyle();
            IDataFormat format = workbook.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat("0.00_ ");
            dateStyle.Alignment = HorizontalAlignment.Center;
            dateStyle.VerticalAlignment = VerticalAlignment.Center;
            dateStyle.WrapText = true;
            return dateStyle;
        }
        #endregion

        #region 设置文件的属性信息
        /// <summary>
        /// 设置文件的属性信息
        /// </summary>
        /// <param name="workbook"></param>
        private void SetInformation(HSSFWorkbook workbook)
        {
            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "";
            workbook.DocumentSummaryInformation = dsi;
            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            si.Author = "";
            si.ApplicationName = "";
            si.LastAuthor = "";
            si.Comments = "";
            si.Title = "";
            si.Subject = "";
            si.CreateDateTime = DateTime.Now;
            workbook.SummaryInformation = si;
        }
        #endregion

        #region 汇总列
        /// <summary>
        /// 汇总列
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="sheet"></param>
        private void SetGroupByName(HSSFWorkbook workbook, ISheet sheet, ICellStyle ItemCellStyle)
        {
            List<Cellocation> ListCellocation = new List<Cellocation>();
            foreach (int index in GroupByIndex)
            {

                string tempValue = "";
                bool continuation = false;
                int startCount = 0;
                int endCount = 0;
                for (int indexZone = startIndexZone; indexZone < endIndexZone; indexZone++)
                {
                    string StringCellValue = sheet.GetRow(indexZone).GetCell(index).StringCellValue;
                    if (StringCellValue == tempValue)
                    {
                        //如果是第一次进入  记录当前合并的坐标
                        if (!continuation)
                        {
                            startCount = indexZone - 1;
                        }
                        endCount = indexZone;
                        continuation = true;
                    }
                    else
                    {
                        if (startCount != endCount)
                        {
                            CellRangeAddress cellRangeAddress = new CellRangeAddress(startCount, endCount, index, index);
                            sheet.AddMergedRegion(cellRangeAddress);
                            startCount = 0;
                            endCount = 0;
                        }

                        continuation = false;
                    }
                    tempValue = StringCellValue;


                }
            }
        }
        #endregion

    }
}
