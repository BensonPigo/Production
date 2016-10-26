using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci;
using Sci.Data;
using System.Linq;

namespace Sci.Production.Warehouse
{
    public partial class P38 : Sci.Win.Tems.QueryForm
    {
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        public P38(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.EditMode = true;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            cbxStatus.SelectedIndex = 0;
            cbxStockType.SelectedIndex = 0;
            MyUtility.Tool.SetGridFrozen(this.grid1);
            Ict.Win.UI.DataGridViewTextBoxColumn columnStatus = new Ict.Win.UI.DataGridViewTextBoxColumn();
            #region -- 設定Grid1的顯示欄位 --
            this.grid1.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.grid1.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)
                 .Text("POID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                 .Text("seq1", header: "Seq1", width: Widths.AnsiChars(3), iseditingreadonly: true)
                  .Text("seq2", header: "Seq2", width: Widths.AnsiChars(2), iseditingreadonly: true)
                  .Text("roll", header: "Roll#", width: Widths.AnsiChars(8), iseditingreadonly: true)
                  .Text("dyelot", header: "Dyelot", width: Widths.AnsiChars(4), iseditingreadonly: true)
                  .Text("status", header: "Status", width: Widths.AnsiChars(10), iseditingreadonly: true).Get(out columnStatus)
                  .DateTime("lockdate", header: "Lock/Unlock" + Environment.NewLine + "Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                  .Text("lockname", header: "Lock/Unlock" + Environment.NewLine + "Name", width: Widths.AnsiChars(8), iseditingreadonly: true)
                  .Numeric("inqty", header: "In Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2, iseditingreadonly: true)
                  .Numeric("outqty", header: "Out Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2, iseditingreadonly: true)
                  .Numeric("adjustqty", header: "Adjust Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2, iseditingreadonly: true)
                  .Numeric("balanceqty", header: "Balance Qty", width: Widths.AnsiChars(10), integer_places: 8, decimal_places: 2, iseditingreadonly: true)
                 .Text("stocktype", header: "Stocktype", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("location", header: "location", width: Widths.AnsiChars(10), iseditingreadonly: true)
                 .Text("description", header: "description", width: Widths.AnsiChars(20), iseditingreadonly: true)
                 .Text("styleid", header: "Style", width: Widths.AnsiChars(13), iseditingreadonly: true)
                 .Text("colorid", header: "Color", width: Widths.AnsiChars(10), iseditingreadonly: true)
                  .Date("earliest_BuyerDelivery", header: "Earliest" + Environment.NewLine + "BuyerDelivery", width: Widths.AnsiChars(10), iseditingreadonly: true)
                  .Date("earliest_SciDelivery", header: "Earliest" + Environment.NewLine + "SciDelivery", width: Widths.AnsiChars(10), iseditingreadonly: true)
                  .Text("brandid", header: "Brand", width: Widths.AnsiChars(12), iseditingreadonly: true)
                  .Text("factoryid", header: "Factory", width: Widths.AnsiChars(8), iseditingreadonly: true)
                  ;
            columnStatus.DefaultCellStyle.ForeColor = Color.Blue;
            #endregion
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            StringBuilder strSQLCmd = new StringBuilder();
            String sp1 = this.textBox1.Text.TrimEnd() + '%';


            if (string.IsNullOrWhiteSpace(this.textBox1.Text.TrimEnd()))
            {
                MyUtility.Msg.WarningBox("< SP# > can't be empty!!");
                textBox1.Focus();
                return;
            }

            strSQLCmd.Append(string.Format(@"
select 0 as [selected], fi.POID,fi.seq1,fi.seq2,fi.Roll,fi.Dyelot,iif(fi.Lock=0,'Unlocked','Locked') [status]
,fi.InQty,fi.OutQty,fi.AdjustQty,fi.InQty-fi.OutQty+fi.AdjustQty as balanceqty
,case fi.stocktype when 'B' then'Bulk' when 'I' then 'Inventory' else fi.StockType end as stocktype
,fi.LockDate
,(select id+'-'+name from dbo.pass1 where id=fi.LockName) LockName
,fi.ukey
,(select mtllocationid+',' from (select MtlLocationID from dbo.FtyInventory_Detail where ukey = fi.Ukey)t for xml path('')) as [location]
,dbo.getMtlDesc(fi.poid,fi.seq1,fi.seq2,2,0) as [Description]
,pd.ColorID
,o.styleid
,o.BrandID
,o.FactoryID
,x.*
from dbo.FtyInventory fi 
left join dbo.PO_Supp_Detail pd on pd.id = fi.POID and pd.seq1 = fi.seq1 and pd.seq2  = fi.Seq2
left join dbo.orders o on o.id = fi.POID
cross apply
(
	select min(o1.BuyerDelivery) earliest_BuyerDelivery ,min(o1.SciDelivery) earliest_SciDelivery 
	from dbo.orders o1 where o1.POID = fi.POID and o1.Junk = 0
) x
where fi.MDivisionID = '{0}' and fi.POID like @poid1 
", Sci.Env.User.Keyword));

            System.Data.SqlClient.SqlParameter sp1_1 = new System.Data.SqlClient.SqlParameter();
            sp1_1.ParameterName = "@poid1";

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            sp1_1.Value = sp1;
            cmds.Add(sp1_1);

            switch (cbxStatus.SelectedIndex)
            {
                case 0:
                    break;
                case 1:
                    strSQLCmd.Append(@" and lock=1");
                    break;
                case 2:
                    strSQLCmd.Append(@" and lock=0");
                    break;
            }

            switch (cbxStockType.SelectedIndex)
            {
                case 0:
                    strSQLCmd.Append(@" and (stocktype='B' or stocktype ='I')");
                    break;
                case 1:
                    strSQLCmd.Append(@" and stocktype='B'");
                    break;
                case 2:
                    strSQLCmd.Append(@" and stocktype='I'");
                    break;
            }

            Ict.DualResult result;
            DataTable dtData;
            MyUtility.Msg.WaitWindows("Data Loading...");
            if (result = DBProxy.Current.Select(null, strSQLCmd.ToString(), cmds, out dtData))
            {
                if (dtData.Rows.Count == 0)
                { MyUtility.Msg.WarningBox("Data not found!!"); }
                listControlBindingSource1.DataSource = dtData;
                dtData.DefaultView.Sort = "poid,seq1,seq2,dyelot,roll,stocktype";
            }
            else
            {
                ShowErr(strSQLCmd.ToString(), result);
            }
            MyUtility.Msg.WaitClear();
        }

        private void btnLock_Click(object sender, EventArgs e)
        {
            LockUnlock(1);
        }

        private void btnUnlock_Click(object sender, EventArgs e)
        {
            LockUnlock(0);
        }

        private void LockUnlock(Byte flag)
        {
            bool x;
            if (!(x = grid1.ValidateControl())) MyUtility.Msg.WarningBox("grid1.ValidateControl failed");
            grid1.EndEdit();
            listControlBindingSource1.EndEdit();
            DualResult result;
            DataTable dt = (DataTable)listControlBindingSource1.DataSource;
            dt.AcceptChanges();
            
            if (dt == null || dt.Rows.Count == 0) return;
            DataRow[] find;
            DialogResult dResult;

            string keyword = "";
            if (flag == 1)
            {
                keyword = "Lock";
            }
            else
            {
                keyword = "Unlock";
            }

            find = dt.Select("Selected = 1");
            if (find.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }

            dResult = MyUtility.Msg.QuestionBox(string.Format("Are you sure to {0} it?", keyword), "Question", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);

            if (dResult == DialogResult.No) return;

            StringBuilder sqlcmd = new StringBuilder();
            foreach (DataRow item in find)
            {
                sqlcmd.Append(string.Format(@"update dbo.ftyinventory set lock={1},lockname='{2}',lockdate=GETDATE() where ukey ={0};", item["ukey"],flag,Sci.Env.User.UserID));
            }
            if (!(result = Sci.Data.DBProxy.Current.Execute(null, sqlcmd.ToString())))
            {
                MyUtility.Msg.WarningBox(string.Format("{0} failed, Pleaes re-try", keyword));
                return;
            }
            else { MyUtility.Msg.InfoBox(string.Format("{0} successful!!", keyword)); }
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)listControlBindingSource1.DataSource;
            MyUtility.Excel.CopyToXls(dt, "");
        }
    }
}
