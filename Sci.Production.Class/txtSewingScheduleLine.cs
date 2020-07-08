using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Sci.Win.UI;

namespace Sci.Production.Class
{
    public partial class txtSewingScheduleLine : Sci.Win.UI.TextBox
    {
        public txtSewingScheduleLine()
        {
            this.Size = new System.Drawing.Size(118, 23);
        }

        [Browsable(true)]
        public Sci.Win.UI.TextBox SPtxt { get; set; }

        [Browsable(true)]
        public Sci.Win.UI.TextBox Factorytxt { get; set; }

        public string cell { get; set; }

        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);
            string FactoryId;

            if (this.Factorytxt != null)
            {
                FactoryId = this.Factorytxt.Text;
            }
            else
            {
                FactoryId = Sci.Env.User.Factory;
            }

            Sci.Win.Forms.Base myForm = (Sci.Win.Forms.Base)this.FindForm();
            if (myForm.EditMode == false || this.ReadOnly == true)
            {
                return;
            }

            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(
                string.Format(
                @"
SELECT ss.SewingLineID as Line,SL.[Description] AS [Description], SL.FactoryID as Factory,sl.SewingCell
FROM SewingSchedule SS WITH (NOLOCK)
LEFT JOIN SewingLine SL WITH (NOLOCK) ON SS.FactoryID=SL.FactoryID AND SS.SewingLineID=SL.ID 
where SS.OrderID='{0}' and SL.FactoryID='{1}'
union 
select SewingLineID,[Description],so.FactoryID,sl.SewingCell
from SewingOutput_Detail sod WITH (NOLOCK)
left join SewingOutput so WITH (NOLOCK) ON so.id =sod.id
LEFT JOIN SewingLine SL WITH (NOLOCK) ON so.FactoryID=SL.FactoryID AND so.SewingLineID=SL.ID 
where sod.OrderId = '{0}' and so.FactoryID = '{1}'
",
                this.SPtxt.Text,
                FactoryId),
                "5,20,10", this.Text);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.Text = item.GetSelectedString();
        }

        DataRow dr;

        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);
            string FactoryId;

            if (this.Factorytxt != null)
            {
                FactoryId = this.Factorytxt.Text;
            }
            else
            {
                FactoryId = Sci.Env.User.Factory;
            }

            Sci.Win.Forms.Base myForm = (Sci.Win.Forms.Base)this.FindForm();
            if (myForm.EditMode == false || this.ReadOnly == true)
            {
                return;
            }

            string chkline = string.Format(
                @"
SELECT ss.SewingLineID as Line,SL.[Description] AS [Description], SL.FactoryID as Factory,sl.SewingCell
FROM SewingSchedule SS WITH (NOLOCK)
LEFT JOIN SewingLine SL WITH (NOLOCK) ON SS.FactoryID=SL.FactoryID AND SS.SewingLineID=SL.ID 
where SS.OrderID='{0}' and SL.FactoryID='{1}' and ss.SewingLineID = '{2}'
union 
select SewingLineID,[Description],so.FactoryID,sl.SewingCell
from SewingOutput_Detail sod WITH (NOLOCK)
left join SewingOutput so WITH (NOLOCK) ON so.id =sod.id
LEFT JOIN SewingLine SL WITH (NOLOCK) ON so.FactoryID=SL.FactoryID AND so.SewingLineID=SL.ID 
where sod.OrderId = '{0}' and so.FactoryID = '{1}' and SewingLineID = '{2}'
",
                this.SPtxt.Text,
                FactoryId,
                this.Text);
            if (!MyUtility.Check.Seek(chkline, out this.dr))
            {
                if (this.Text != string.Empty)
                {
                    MyUtility.Msg.WarningBox(string.Format("Sewingline {0} not found", this.Text));
                }

                this.Text = string.Empty;
                this.cell = string.Empty;
            }
            else
            {
                this.cell = MyUtility.Convert.GetString(this.dr["SewingCell"]);
            }
        }
    }
}
