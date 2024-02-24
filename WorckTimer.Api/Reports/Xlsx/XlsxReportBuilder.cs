using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using WorkTimer.Api.Reports.Xlsx.Definitions;

namespace WorkTimer.Api.Reports.Xlsx
{
    public class XlsxReportBuilder
    {
        public int CurrentRow { get; set; }

        private readonly XSSFWorkbook xlsx;
        private readonly ISheet sheet;
        private int numberOfCols;
        private int maxColumnWidth = 60;

        public XlsxReportBuilder()
        {
            xlsx = new XSSFWorkbook();
            sheet = xlsx.CreateSheet();
        }

        public XlsxReportBuilder(ISheet sheet)
        {
            this.sheet = sheet;
            xlsx = (XSSFWorkbook)sheet.Workbook;
        }

        public void SetMaxColumnWidth(int width)
        {
            maxColumnWidth = width;
        }

        public void SetDefaultColumnWidth(int width)
        {
            sheet.DefaultColumnWidth = width;
        }

        public void SetColumnWidth(int columnIndex, int width)
        {
            sheet.SetColumnWidth(columnIndex, width * 256);
        }

        public void SetRowHeight(int rowIndex, float height)
        {
            var row = sheet.GetRow(rowIndex) ?? sheet.CreateRow(rowIndex);
            row.HeightInPoints = height;
        }

        public IDataFormat CreateDataFormat()
        {
            return xlsx.CreateDataFormat();
        }

        public void AddTitle(string title, int colSpan = 5, ICellStyle cellStyle = null)
        {
            var row = sheet.CreateRow(CurrentRow++);
            AddMergedRegion(CurrentRow - 1, CurrentRow - 1, 0, colSpan - 1);
            var cell = row.CreateCell(0);
            cell.SetCellValue(title);
            if (cellStyle != null) cell.CellStyle = cellStyle;
        }

        public void CreateFreezePane(int colSplit, int rowSplit)
        {
            sheet.CreateFreezePane(colSplit, rowSplit);
        }

        public void CreateFreezePane(int colSplit, int rowSplit, int leftmostColumn, int topRow)
        {
            sheet.CreateFreezePane(colSplit, rowSplit, leftmostColumn, topRow);
        }

        public int GetCurrentRow() => CurrentRow;

        public void AddMergedRegion(int firstRow, int lastRow, int firstCol, int lastCol, bool copyStyleFromFirstCell = true)
        {
            if (copyStyleFromFirstCell)
            {
                var firstCellStyle = sheet.GetRow(firstRow).GetCell(firstCol)?.CellStyle;
                SetStyleToCellRange(firstRow, lastRow, firstCol, lastCol, firstCellStyle);
            }
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(firstRow, lastRow, firstCol, lastCol));
        }

        public void SetStyleToCellRange(int firstRow, int lastRow, int firstCol, int lastCol, ICellStyle cellStyle)
        {
            for (var rowIndex = firstRow; rowIndex <= lastRow; rowIndex++)
            {
                var row = sheet.GetRow(rowIndex) ?? sheet.CreateRow(rowIndex);
                for (var colIndex = firstCol; colIndex <= lastCol; colIndex++)
                {
                    var cell = row.GetCell(colIndex) ?? row.CreateCell(colIndex);
                    cell.CellStyle = cellStyle;
                }
            }
        }

        public void AddRow(Action<IRow> rowTemplate)
        {
            var row = sheet.CreateRow(CurrentRow++);
            rowTemplate(row);
        }

        public void SetCellValue(IRow row, int columnIndex, object value, CellType? cellType = null, ICellStyle cellStyle = null, bool isDate = false, string cellComment = null)
        {
            var cell = row.CreateCell(columnIndex, cellType ?? CellType.String);
            if (isDate && value.GetType() == typeof(DateTime))
                cell.SetCellValue((DateTime)value);
            else if (cellType == CellType.Numeric || (cellType == null && double.TryParse(value.ToString(), out _)))
                cell.SetCellValue(Convert.ToDouble(value));
            else
                cell.SetCellValue(value.ToString());
            cell.CellStyle = cellStyle;
            if (!string.IsNullOrWhiteSpace(cellComment))
            {
                var patr = sheet.CreateDrawingPatriarch();
                IComment comment = patr.CreateCellComment(new XSSFClientAnchor());
                comment.String = new XSSFRichTextString(cellComment);
                cell.CellComment = comment;
            }
        }

        public void AddRow(ICellStyle cellStyle = null, params string[] cells)
        {
            var row = sheet.CreateRow(CurrentRow++);
            if (numberOfCols < cells.Length) numberOfCols = cells.Length;
            for (int i = 0; i < cells.Length; i++)
            {
                var cell = row.CreateCell(i);
                cell.SetCellValue(cells[i]);
                if (cellStyle != null) cell.CellStyle = cellStyle;
            }
        }

        public void AddRow(params string[] cells) => AddRow(null, cells: cells);

        public void NextRow()
        {
            CurrentRow++;
        }

        public void AddTable<T>(IEnumerable<T> dataSource, Action<List<string>, T> rowTemplate)
        {
            foreach (var item in dataSource)
            {
                var row = new List<string>();
                rowTemplate(row, item);
                AddRow(row.ToArray());
            }
        }

        public void AddTable<T>(IEnumerable<T> dataSource, Action<IRow, T> rowTemplate)
        {
            foreach (var item in dataSource)
            {
                var row = sheet.CreateRow(CurrentRow++);
                rowTemplate(row, item);
                if (numberOfCols < row.Cells.Count) numberOfCols = row.Cells.Count;
            }
        }

        public IFont CreateFont(CellStyle cellStyle)
        {
            var font = xlsx.CreateFont();
            if (cellStyle.FontHeight != null) font.FontHeight = (double)cellStyle.FontHeight;
            if (cellStyle.FontHeightInPoints != null) font.FontHeightInPoints = (double)cellStyle.FontHeightInPoints;
            if (cellStyle.TextColor != null) font.Color = cellStyle.TextColor.Index;
            font.IsBold = cellStyle.IsBold;
            font.IsItalic = cellStyle.IsItalic;
            font.IsStrikeout = cellStyle.IsStrikeout;
            return font;
        }

        public ICellStyle CreateCellStyle(CellStyle cellStyle)
        {
            var style = xlsx.CreateCellStyle() as XSSFCellStyle;
            if (cellStyle.Alignment != null) style.Alignment = (HorizontalAlignment)cellStyle.Alignment;
            if (cellStyle.VerticalAlignment != null) style.VerticalAlignment = (VerticalAlignment)cellStyle.VerticalAlignment;
            if (cellStyle.DataFormat != null) style.DataFormat = (short)cellStyle.DataFormat;
            style.WrapText = cellStyle.IsWrapText;
            style.SetFont(CreateFont(cellStyle));
            if (cellStyle.BackgroundColor.HasValue)
            {
                var backgroundColor = cellStyle.BackgroundColor.Value;
                style.SetFillForegroundColor(new XSSFColor(new[]
                {
                    backgroundColor.A,
                    backgroundColor.R,
                    backgroundColor.G,
                    backgroundColor.B
                }));
                style.FillPattern = FillPattern.SolidForeground;
            }
            style.BorderLeft = cellStyle.Border.Left;
            style.BorderTop = cellStyle.Border.Top;
            style.BorderRight = cellStyle.Border.Right;
            style.BorderBottom = cellStyle.Border.Bottom;
            return style;
        }

        public void AddTable<T>(IEnumerable<T> dataSource, List<string> columns, Action<List<string>, T> rowTemplate)
        {
            var headerStyle = CreateCellStyle(new CellStyle { IsBold = true });
            AddRow(headerStyle, columns.ToArray());
            AddTable(dataSource, rowTemplate);
        }

        public void AddTable<T>(IEnumerable<T> dataSource, List<string> columns, Action<IRow, T> rowTemplate)
        {
            var headerStyle = CreateCellStyle(new CellStyle { IsBold = true });
            AddRow(headerStyle, columns.ToArray());
            AddTable(dataSource, rowTemplate);
        }

        public MemoryStream ExportToStream()
        {
            for (int i = 0; i < numberOfCols; i++)
            {
                sheet.AutoSizeColumn(i);
                if (maxColumnWidth > 0 && sheet.GetColumnWidth(i) > maxColumnWidth * 256)
                {
                    sheet.SetColumnWidth(i, maxColumnWidth * 256);
                }
            }
            MemoryStream stream = new MemoryStream();
            xlsx.Write(stream, true);
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }
    }
}
