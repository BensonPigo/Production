using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using System.Data.SqlClient;

namespace Sci.Production.Packing
{
    /// <inheritdoc/>
    public partial class P25 : Win.Tems.QueryForm
    {
        private DataTable gridData;
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;

        /// <inheritdoc/>
        public P25(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            // Grid設定
            this.grid.IsEditingReadOnly = false;

            this.Helper.Controls.Grid.Generator(this.grid)
            .CheckBox("Sel", header: "Sel", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
            .Text("Id", header: "Packing List ID", width: Widths.AnsiChars(17), iseditingreadonly: true)
            .Text("OrderID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("OrderShipmodeSeq", header: "Ship mode Seq", iseditingreadonly: true)
            .Text("CustPONo", header: "P.O. No", iseditingreadonly: true)
            .Text("StyleID", header: "Style", iseditingreadonly: true)
            .Text("ShipModeID", header: "Ship Mode", iseditingreadonly: true)
            .Numeric("ShipQty", header: "Ship Qty", decimal_places: 0, iseditingreadonly: true)
            .Numeric("CTNQty", header: "Total Cartons", decimal_places: 0, iseditingreadonly: true)
            ;
        }

        /// <inheritdoc/>
        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <inheritdoc/>
        private void BtnQuery_Click(object sender, EventArgs e)
        {
            if (
                    MyUtility.Check.Empty(this.txtSP_s.Text) &&
                    MyUtility.Check.Empty(this.txtSP_e.Text) &&
                    MyUtility.Check.Empty(this.txtPOno.Text))
            {
                this.txtSP_s.Focus();
                MyUtility.Msg.WarningBox("SP#, P.O. No cannot all be empty.");
                return;
            }

            this.listControlBindingSource.DataSource = null;

            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            List<string> sqlWhere = new List<string>();
            StringBuilder sqlCmd = new StringBuilder();

            #region WHERE條件
            sqlParameters.Add(new SqlParameter("@MDivisionID", Env.User.Keyword));

            if (!MyUtility.Check.Empty(this.txtSP_s.Text))
            {
                sqlWhere.Add(" pd.OrderID >= @SP_s ");
                sqlParameters.Add(new SqlParameter("@SP_s", this.txtSP_s.Text));
            }

            if (!MyUtility.Check.Empty(this.txtSP_e.Text))
            {
                sqlWhere.Add(" pd.OrderID <= @SP_e ");
                sqlParameters.Add(new SqlParameter("@SP_e", this.txtSP_e.Text));
            }

            if (!MyUtility.Check.Empty(this.txtPOno.Text))
            {
                sqlWhere.Add(" o.CustPONo = @POno ");
                sqlParameters.Add(new SqlParameter("@POno", this.txtPOno.Text));
            }
            #endregion

            #region SQL語法
            sqlCmd.Append($@"
SELECT distinct
	[Sel]=0
	,p.Id
	,pd.OrderID
	,pd.OrderShipmodeSeq
	,o.CustPONo
	,o.StyleID
	,p.ShipModeID
	,p.ShipQty
	,p.CTNQty
FROM PackingList p WITH(NOLOCK)
INNER JOIN PackingList_Detail pd WITH(NOLOCK) on pd.ID = p.ID
INNER JOIN Orders o WITH(NOLOCK) ON pd.OrderID = o.ID
where p.Type = 'B'
and exists(
	select 1
	from (
		select v = count (1)
		from (
			select distinct b.OrderID, b.OrderShipmodeSeq
			from PackingList_Detail b
			where p.id = b.ID
		) bs
	) b
	where b.v = 1 
) 
and p.MDivisionID = @MDivisionID
").Append("AND" + sqlWhere.JoinToString(Environment.NewLine + "AND")).Append(Environment.NewLine + "Order by p.Id, pd.OrderID");
            #endregion

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), sqlParameters, out this.gridData);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (this.gridData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
            }

            this.listControlBindingSource.DataSource = this.gridData;
        }

        /// <inheritdoc/>
        private void BtnToExcel_Click(object sender, EventArgs e)
        {
            this.grid.ValidateControl();
            this.grid.EndEdit();
            this.listControlBindingSource.EndEdit();
            DataTable gridData = (DataTable)this.listControlBindingSource.DataSource;

            if (gridData == null || gridData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("No data!!");
                return;
            }

            DataRow[] selectedDatas = gridData.Select("Sel=1");

            if (selectedDatas.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please choose the data first.");
                return;
            }

            this.ShowWaitMessage("Data Loading....");
            this.btnToExcel.Enabled = false;
            foreach (DataRow item in selectedDatas)
            {
                this.Print(item);
            }

            this.btnToExcel.Enabled = true;
            this.HideWaitMessage();
        }

        private void Print(DataRow item)
        {
            int orderQty = MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup(string.Format(
                @"select isnull(oq.Qty ,0) as Qty
from (select distinct OrderID,OrderShipmodeSeq from PackingList_Detail WITH (NOLOCK) where ID = '{0}') a
left join Order_QtyShip oq WITH (NOLOCK) on oq.Id = a.OrderID and oq.Seq = a.OrderShipmodeSeq", MyUtility.Convert.GetString(item["ID"]))));
            DataRow masterData;

            if (!MyUtility.Check.Seek($"select * from packinglist where id = '{item["ID"]}'", out masterData))
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return;
            }

            DataTable printData, ctnDim, qtyCtn, articleSizeTtlShipQty, printGroupData, clipData;
            string specialInstruction;
            DualResult result = PublicPrg.Prgs.QueryPackingGuideReportData(MyUtility.Convert.GetString(item["ID"]), out printData, out ctnDim, out qtyCtn, out articleSizeTtlShipQty, out printGroupData, out clipData, out specialInstruction);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            PublicPrg.Prgs.PackingListToExcel_PackingGuideReport("\\Packing_P03_PackingGuideReport.xltx", printData, ctnDim, qtyCtn, articleSizeTtlShipQty, printGroupData, clipData, masterData, orderQty, specialInstruction, true);
        }
    }
}
