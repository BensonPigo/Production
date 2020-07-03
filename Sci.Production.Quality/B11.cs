using Sci.Data;
using System.Data;
using System.Windows.Forms;

namespace Sci.Production.Quality
{
    public partial class B11 : Sci.Win.Tems.Input1
    {

        public B11(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtID.ReadOnly = true;         
        }

        protected override bool ClickSaveBefore()
        { 
            if (MyUtility.Check.Empty(CurrentMaintain["ID"]))
            {
                this.txtID.Focus();
                MyUtility.Msg.WarningBox("< ID > can not be empty!");
                return false;      
            }
            else if(MyUtility.Convert.GetString(CurrentMaintain["ID"]).Length > 10)
            {
                this.txtID.Focus();
                MyUtility.Msg.WarningBox("< ID > can not exceed 10 words!");
                return false;
            }

            if (this.IsDetailInserting)
            {
                DataRow dr;
                if (MyUtility.Check.Seek(string.Format("select ID from ShadebandDocLocation where ID='{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"])), out dr))
                {
                    MyUtility.Msg.WarningBox("< ID > can not duplicate!");
                }
            }

            return base.ClickSaveBefore();
        }
    }
}
