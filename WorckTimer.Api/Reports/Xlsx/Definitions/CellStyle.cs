using NPOI.SS.UserModel;

namespace WorkTimer.Api.Reports.Xlsx.Definitions
{
    public record CellStyle
    {
        public HorizontalAlignment? Alignment { get; init; }
        public VerticalAlignment? VerticalAlignment { get; init; }
        public System.Drawing.Color? BackgroundColor { get; init; }
        public IndexedColors TextColor { get; init; }
        public double? FontHeight { get; init; }
        public double? FontHeightInPoints { get; init; }
        public bool IsWrapText { get; init; }
        public bool IsBold { get; init; }
        public bool IsItalic { get; init; }
        public bool IsStrikeout { get; init; }
        public short? DataFormat { get; init; }
        public CellBorder Border { get; init; } = new CellBorder();

    }
}
