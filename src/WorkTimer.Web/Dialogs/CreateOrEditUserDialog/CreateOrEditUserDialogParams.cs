using WorkTimer.Common.Models;

namespace WorkTimer.Web.Dialogs.CreateOrEditUserDialog
{
    public class CreateOrEditUserDialogParams
    {
        public bool EditMode { get; set; }
        public User User { get; set; }
    }
}
