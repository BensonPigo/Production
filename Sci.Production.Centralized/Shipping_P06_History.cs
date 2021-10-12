using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Centralized
{
    /// <summary>
    /// Shipping_P06_History
    /// </summary>
    public partial class Shipping_P06_History : Win.Tems.QueryForm
    {
        /// <summary>
        /// Shipping_P06_History
        /// </summary>
        public Shipping_P06_History()
        {
            this.InitializeComponent();
            this.EditMode = true;
            this.txtShippingReason.Type = "PL";
        }

        private string LinkDB
        {
            get
            {
                string linkDB = "ProductionTPE";

#if DEBUG
                linkDB = "Production";
#endif
                return linkDB;
            }
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.Helper.Controls.Grid.Generator(this.gridShippingHistory)
               .Text("ID", header: "ID", width: Widths.AnsiChars(13), iseditingreadonly: true)
               .Date("AddDate", header: "Unlock Date", width: Widths.Auto(), iseditingreadonly: true)
               .Text("Reason", header: "Reason", width: Widths.AnsiChars(26), iseditingreadonly: true)
               .EditText("Remark", header: "Remark", width: Widths.AnsiChars(26), iseditingreadonly: true)
               .Text("Unlocker", header: "Unlocker", width: Widths.AnsiChars(13), iseditingreadonly: true);
        }

        private void Query()
        {
            DataTable dtResult;
            List<SqlParameter> listPar = new List<SqlParameter>();
            string sqlWhere = string.Empty;

            if (!MyUtility.Check.Empty(this.txtCentralizedmulitM.Text))
            {
                sqlWhere += " and sh.MDivisionID = @MDivisionID ";
                listPar.Add(new SqlParameter("@MDivisionID", this.txtCentralizedmulitM.Text));
            }

            if (!MyUtility.Check.Empty(this.txtPulloutID.Text))
            {
                sqlWhere += " and sh.ID = @PulloutID ";
                listPar.Add(new SqlParameter("@PulloutID", this.txtPulloutID.Text));
            }

            if (!MyUtility.Check.Empty(this.txtShippingReason.TextBox1.Text))
            {
                sqlWhere += " and sh.ReasonID = @ReasonID and ReasonTypeID = 'PL' ";
                listPar.Add(new SqlParameter("@ReasonID", this.txtShippingReason.TextBox1.Text));
            }

            if (!MyUtility.Check.Empty(this.txtUnlocker.Text))
            {
                sqlWhere += " and sh.AddName = @AddName ";
                listPar.Add(new SqlParameter("@AddName", this.txtUnlocker.Text));
            }

            if (this.dateRangeUnlock.HasValue1)
            {
                sqlWhere += " and sh.AddDate >= @AddDateFrom ";
                listPar.Add(new SqlParameter("@AddDateFrom", this.dateRangeUnlock.DateBox1.Value));
            }

            if (this.dateRangeUnlock.HasValue2)
            {
                sqlWhere += " and sh.AddDate <= @AddDateTo ";
                listPar.Add(new SqlParameter("@AddDateTo", this.dateRangeUnlock.DateBox2.Value));
            }

            string sqlGetData = $@"
select  sh.ID,
        sh.AddDate,
        [Reason] = sr.ID + '-' + sr.Description,
        sh.Remark,
        [Unlocker] = sh.AddName + '-' + p.Name
from    ShippingHistory sh with (nolock)
inner join ShippingReason sr with (nolock) on sh.ReasonID = sr.ID and sh.ReasonTypeID = sr.Type
inner join pass1 p with (nolock) on p.ID = sh.AddName
where 1 = 1 {sqlWhere}
";

            DualResult result = DBProxy.Current.Select(this.LinkDB, sqlGetData, listPar, out dtResult);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.gridShippingHistory.DataSource = dtResult;
        }

        private void TxtUnlocker_Validating(object sender, CancelEventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtUnlocker.Text))
            {
                return;
            }

            string sqlCheckUnlocker = @"select Name from Pass1 where ID = @ID and 
exists (select 1 from ShippingHistory with (nolock) where AddName = Pass1.ID)";
            DataRow drResult;
            bool isExistsHistory = MyUtility.Check.Seek(sqlCheckUnlocker, new List<SqlParameter>() { new SqlParameter("ID", this.txtUnlocker.Text) }, out drResult, this.LinkDB);

            if (!isExistsHistory)
            {
                this.displayUnlockerName.Text = string.Empty;
                MyUtility.Msg.WarningBox("Unlocker not found");
                return;
            }

            this.displayUnlockerName.Text = drResult["Name"].ToString();
        }

        private void TxtUnlocker_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlGetUnlockerList = @"
select distinct AddName
into    #Unlockers
from    ShippingHistory

select  p.ID, p.Name
from #Unlockers u
inner join pass1 p with (nolock) on u.AddName = p.ID  
";
            DataTable dtUnlockers;

            DualResult result = DBProxy.Current.Select(this.LinkDB, sqlGetUnlockerList, out dtUnlockers);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            SelectItem selectItem = new SelectItem(dtUnlockers, "ID,Name", null, null);

            DialogResult dialogResult = selectItem.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                this.txtUnlocker.Text = selectItem.GetSelecteds()[0]["ID"].ToString();
                this.displayUnlockerName.Text = selectItem.GetSelecteds()[0]["Name"].ToString();
            }
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            this.Query();
        }
    }
}
