using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public  Sci.Win.UI.TextBox SPtxt { set; get; }

        [Browsable(true)]
        public Sci.Win.UI.TextBox Factorytxt { set; get; }

   

        protected override void OnPopUp(TextBoxPopUpEventArgs e)
        {
            base.OnPopUp(e);
            string FactoryId;

            if (Factorytxt != null)
            {

                FactoryId = Factorytxt.Text;
            }
            else {
                FactoryId = Sci.Env.User.Factory;
            }
            
            Sci.Win.Forms.Base myForm = (Sci.Win.Forms.Base)this.FindForm();
            if (myForm.EditMode == false || this.ReadOnly == true) return;
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(string.Format(@"
SELECT Distinct ss.SewingLineID as Line,SL.[Description] AS [Description], SL.FactoryID as Factory
FROM SewingSchedule SS WITH (NOLOCK)
LEFT JOIN SewingLine SL WITH (NOLOCK) ON SS.FactoryID=SL.FactoryID AND SS.SewingLineID=SL.ID 
where SS.OrderID='{0}' and SL.FactoryID='{1}'", SPtxt.Text, FactoryId), "5,20,10", this.Text);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            this.Text = item.GetSelectedString();

            

//            if (displayCellbox != null) {
//                if (MyUtility.Check.Seek(string.Format(
//@"select SewingCell from SewingLine where FactoryID='{0}' and id='{1}'", FactoryId, item.GetSelectedString()), out dr))
//                {
//                    displayCellbox.Text = dr["sewingcell"].ToString();
//                }
//                else
//                {
//                    displayCellbox.Text = "";
//                }
//            }
           
        }
    }
}
