using NPOI.SS.UserModel;

namespace WorkTimer.Api.Reports.Xlsx.Definitions
{
    public class CellBorder
    {
        public BorderStyle Left { get; set; }
        public BorderStyle Top { get; set; }
        public BorderStyle Right { get; set; }
        public BorderStyle Bottom { get; set; }

        public CellBorder(BorderStyle style = BorderStyle.None) : this(left: style, top: style, right: style, bottom: style)
        {

        }

        public CellBorder(BorderStyle left, BorderStyle top, BorderStyle right, BorderStyle bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

    }
}
