using Ict;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    public partial class P61 : Sci.Win.Tems.Input6
    {
        public P61(ToolStripMenuItem menuitem)
            :base(menuitem)
        {
            InitializeComponent();
            gridicon.Append.Enabled = false;
            gridicon.Append.Visible = false;
            gridicon.Insert.Enabled = false;
            gridicon.Insert.Visible = false;
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string ID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(@"
select  LID.ID
        , LID.OrderID
        , LID.Refno
        , LID.ThreadColorID
        , [Desc] = Litem.Description
        , [unit] = Linv.UnitID
        , LID.Qty
from LocalIssue LI
join LocalIssue_Detail LID on LI.ID = LID.ID
join LocalInventory Linv on LID.OrderID = Linv.OrderID and LID.Refno = Linv.Refno and LID.ThreadColorID = Linv.ThreadColorID
left join LocalItem Litem on LID.Refno = Litem.Refno
where LI.ID = '{0}' and LI.MDivisionID = '{1}'", ID, Sci.Env.User.Keyword);
            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override void OnDetailGridSetup()
        {
            #region Set Grid
            Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("OrderID", header: "SP#", iseditingreadonly: false)
                .Text("Refno", header: "Refno", iseditingreadonly: false)
                .Text("ThreadColorID", header: "ThreadColor", iseditingreadonly: false)
                .EditText("desc", header: "Description", iseditingreadonly: false)
                .Text("unit", header: "Unit", iseditingreadonly: false)
                .Numeric("Qty", header: "Issue Qty", iseditingreadonly: false, minimum: -100);
            #endregion 
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            CurrentMaintain["Status"] = "New";
            CurrentMaintain["IssueDate"] = DateTime.Now;
        }

        protected override bool ClickEditBefore()
        {
            if (CurrentMaintain["Status"].EqualString("confirmed"))
            {
                MyUtility.Msg.InfoBox("Data is confirmed, can't modify.", "Warning");
                return false;
            }
            return base.ClickEditBefore();
        }

        protected override bool ClickSaveBefore()
        {
            #region Check 必輸條件
            /*--- IssueQty != 0 ---*/
            List<string> listErr = new List<string>();
            DataTable dataTable = this.detailgrid.GetTable();
            foreach (DataRow dr in dataTable.Rows)
            {
                if (Convert.ToDecimal(dr["Qty"]) == 0)
                {
                    listErr.Add(string.Format(@"< SP# > : {0}, < Refno > : {1}, < ThreadColor > : {2}", dr["OrderID"], dr["Refno"], dr["ThreadColorID"]));
                }
            }

            if (listErr.Count > 0)
            {
                MyUtility.Msg.InfoBox(listErr.JoinToString("\n\r"), "Issue Qty can not be Zero!!");
                return false;
            }
            #endregion
            #region 取單號
            if (this.IsDetailInserting)
            {
                string tmpId = Sci.MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "IO", "LocalIssue", (DateTime)CurrentMaintain["Issuedate"]);
                if (MyUtility.Check.Empty(tmpId))
                {
                    MyUtility.Msg.WarningBox("Get document ID fail!!");
                    return false;
                }
                CurrentMaintain["id"] = tmpId;
            }
            #endregion 
            return base.ClickSaveBefore();
        }

        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            this.RenewData();
            this.OnDetailEntered();
            this.EnsureToolbarExt();
        }

        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            this.RenewData();
            this.OnDetailEntered();
            this.EnsureToolbarExt();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(detailgridbs.DataSource)) return;
            int index = detailgridbs.Find("OrderID", txtSP.Text.TrimEnd());
            if (index == -1)
            { 
                MyUtility.Msg.InfoBox("Data was not found!!"); 
            }
            else
            { 
                detailgridbs.Position = index; 
            }
        }

        private void btnClearEmpty_Click(object sender, EventArgs e)
        {
            detailgrid.ValidateControl();
            ((DataTable)detailgridbs.DataSource).Select("qty=0.00 or qty is null").ToList().ForEach(r => r.Delete());
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            var frm = new Sci.Production.Warehouse.P61_Import(CurrentMaintain, (DataTable)detailgridbs.DataSource);
            frm.ShowDialog(this);
            this.RenewData();
        }
    }
}
