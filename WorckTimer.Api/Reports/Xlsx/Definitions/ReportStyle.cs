using NPOI.SS.UserModel;
using System.Drawing;

namespace WorkTimer.Api.Reports.Xlsx.Definitions
{
    public class ReportStyle
    {
        private readonly static CellStyle HeaderBase = new()
        {
            Alignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            BackgroundColor = Color.FromArgb(89, 74, 226),
            IsBold = true,
            TextColor = IndexedColors.White,
            IsWrapText = true,
            Border = new CellBorder(BorderStyle.Thin)
        };

        public readonly CellStyle Title = new() { FontHeightInPoints = 14, IsBold = true };

        public readonly CellStyle FirstHeader = HeaderBase with { FontHeightInPoints = 12 };
        public readonly CellStyle SecondHeader = HeaderBase with { FontHeightInPoints = 10 };
        public readonly CellStyle ThirdHeader = HeaderBase with { FontHeightInPoints = 9 };

        public readonly CellStyle AlignCenter = new() { Alignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
        public readonly CellStyle AlignLeftCenter = new() { Alignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Center };

    }
}
