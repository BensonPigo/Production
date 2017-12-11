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
using System.Runtime.InteropServices;
using Sci.Win.Tools;

namespace Sci.Production.Warehouse
{
    public partial class P38 : Sci.Win.Tems.QueryForm
    {
        const Byte UnLock = 0, Lock = 1;
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
            comboStatus.SelectedIndex = 0;
            comboStockType.SelectedIndex = 0;
            Ict.Win.UI.DataGridViewTextBoxColumn columnStatus = new Ict.Win.UI.DataGridViewTextBoxColumn();
            Ict.Win.DataGridViewGeneratorTextColumnSettings ns = new DataGridViewGeneratorTextColumnSettings();
            
            ns.CellMouseDoubleClick += (s, e) =>
                {
                    DataRow thisRow = this.gridMaterialLock.GetDataRow(e.RowIndex);
                    string dyelot = thisRow["dyelot"].ToString();
                    DataTable dt = (DataTable)listControlBindingSource1.DataSource;
                    string tempstatus = thisRow["selected"].ToString();
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["dyelot"].ToString() == dyelot)
                        {
                            if (tempstatus == "0") dr["selected"] = true;
                            else dr["selected"] = false;

                        }
                                             
                    }
                };
            
            #region -- 設定Grid1的顯示欄位 --
            this.gridMaterialLock.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.gridMaterialLock.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.gridMaterialLock)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)
                 .Text("POID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                 .Text("seq1", header: "Seq1", width: Widths.AnsiChars(3), iseditingreadonly: true)
                  .Text("seq2", header: "Seq2", width: Widths.AnsiChars(2), iseditingreadonly: true)
                  .Text("roll", header: "Roll#", width: Widths.AnsiChars(8), iseditingreadonly: true)
                  .Text("dyelot", header: "Dyelot", width: Widths.AnsiChars(4), iseditingreadonly: true,settings: ns)
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
            gridMaterialLock.Columns["dyelot"].HeaderCell.Style.BackColor = Color.Orange;
            
            #endregion
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            StringBuilder strSQLCmd = new StringBuilder();
            String sp1 = this.txtSP.Text.TrimEnd() + '%';


            if (MyUtility.Check.Empty(txtSP.Text)&& MyUtility.Check.Empty(txtwkno.Text)&& MyUtility.Check.Empty(txtReceivingid.Text))
            {
                MyUtility.Msg.WarningBox("SP# and WK NO and Receiving ID  can't be empty!!");
                return;
            }

            strSQLCmd.Append(string.Format(@"
select 0 as [selected]
        , fi.POID
        , fi.seq1
        , fi.seq2
        , fi.Roll
        , fi.Dyelot
        , iif(fi.Lock=0,'Unlocked','Locked') [status]
        , fi.InQty
        , fi.OutQty
        , fi.AdjustQty
        , fi.InQty-fi.OutQty+fi.AdjustQty as balanceqty
        , stocktype =  case fi.stocktype 
                        when 'B' then'Bulk' 
                        when 'I' then 'Inventory' 
                        else fi.StockType 
                       end 
        , fi.LockDate
        , LockName = (select id+'-'+name from dbo.pass1 WITH (NOLOCK) where id=fi.LockName) 
        , fi.ukey
        , [location] = dbo.Getlocation(fi.ukey)
        , [Description] = dbo.getMtlDesc(fi.poid,fi.seq1,fi.seq2,2,0)
        , pd.ColorID
        , o.styleid
        , o.BrandID
        , o.FactoryID
        , x.*
from dbo.FtyInventory fi WITH (NOLOCK) 
left join dbo.PO_Supp_Detail pd WITH (NOLOCK) on pd.id = fi.POID and pd.seq1 = fi.seq1 and pd.seq2  = fi.Seq2
left join dbo.orders o WITH (NOLOCK) on o.id = fi.POID
left join dbo.factory f WITH (NOLOCK) on o.FtyGroup=f.id
cross apply
(
	select  earliest_BuyerDelivery = min(o1.BuyerDelivery)  
            , earliest_SciDelivery = min(o1.SciDelivery)  
	from dbo.orders o1 WITH (NOLOCK) 
    where o1.POID = fi.POID and o1.Junk = 0
) x
where   f.MDivisionID = '{0}' 
        and fi.POID like @poid1 
", Sci.Env.User.Keyword));

            System.Data.SqlClient.SqlParameter sp1_1 = new System.Data.SqlClient.SqlParameter();
            sp1_1.ParameterName = "@poid1";

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            sp1_1.Value = sp1;
            cmds.Add(sp1_1);

            switch (comboStatus.SelectedIndex)
            {
                case 0:
                    break;
                case 1:
                    strSQLCmd.Append(@" 
        and lock=1");
                    break;
                case 2:
                    strSQLCmd.Append(@" 
        and lock=0");
                    break;
            }

            switch (comboStockType.SelectedIndex)
            {
                case 0:
                    strSQLCmd.Append(@" 
        and (stocktype='B' or stocktype ='I')");
                    break;
                case 1:
                    strSQLCmd.Append(@" 
        and stocktype='B'");
                    break;
                case 2:
                    strSQLCmd.Append(@" 
        and stocktype='I'");
                    break;
            }

            if (!txtSeq.checkSeq1Empty())
            {
                strSQLCmd.Append(string.Format(@"
        and fi.seq1 = '{0}'", txtSeq.seq1));
            }
            if (!txtSeq.checkSeq2Empty())
            {
                strSQLCmd.Append(string.Format(@" 
        and fi.seq2 = '{0}'", txtSeq.seq2));
            }
            if (!MyUtility.Check.Empty(txtwkno.Text))
            {
                strSQLCmd.Append(string.Format(@" 
and exists (select 1 from Export_Detail e where e.poid = fi.poid and e.seq1 = fi.seq1 and e.seq2 = fi.seq2 and e.id = '{0}')",
txtwkno.Text));
            }
            if (!MyUtility.Check.Empty(txtReceivingid.Text))
            {
                strSQLCmd.Append(string.Format(@" 
and exists (select 1 from Receiving_Detail r where r.poid = fi.poid and r.seq1 = fi.seq1 and r.seq2 = fi.seq2 and r.Roll = fi.Roll and r.Dyelot = fi.Dyelot and r.id = '{0}' )",
txtReceivingid.Text));
            }
            Ict.DualResult result;
            DataTable dtData;
            this.ShowWaitMessage("Data Loading...");
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
            this.HideWaitMessage();
        }

        private void btnLock_Click(object sender, EventArgs e)
        {
            if (listControlBindingSource1.DataSource == null) return;
            if (checkStatus(Lock))
            {
                LockUnlock(Lock);
            }
            else
            {
                MyUtility.Msg.InfoBox("It cannot be Lock.");
            }
        }

        private void btnUnlock_Click(object sender, EventArgs e)
        {
            if (listControlBindingSource1.DataSource == null) return;
            if (checkStatus(UnLock))
            {
                LockUnlock(UnLock);
            }
            else
            {
                MyUtility.Msg.InfoBox("It cannot be UnLock.");
            }
        }

        private bool checkStatus(Byte flag)
        {
            bool check = true;
            string strCheckStatus = "";
            #region 確認 Lock Or UnLock
            switch (flag)
            {
                case UnLock:
                    strCheckStatus = "UnLocked";
                    break;
                case Lock:
                    strCheckStatus = "Locked";
                    break;
            }
            #endregion 
            DataTable dt = (DataTable)listControlBindingSource1.DataSource;
            check = !dt.AsEnumerable().Any(row => row["status"].EqualString(strCheckStatus) && row["Selected"].EqualString("1"));
            return check;
        }

        private void LockUnlock(Byte flag)
        {
            bool x;
            if (!(x = gridMaterialLock.ValidateControl())) MyUtility.Msg.WarningBox("grid1.ValidateControl failed");
            gridMaterialLock.EndEdit();
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
            else
            {
                this.SendExcel();
                MyUtility.Msg.InfoBox(string.Format("{0} successful!!", keyword));
            }

            btnQuery_Click(null,null);
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            if (listControlBindingSource1.DataSource == null)
            {
                return;
            }

            DataTable dt = (DataTable)listControlBindingSource1.DataSource;
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Warehouse_P38");            

            if (this.SaveExcel(dt, strExcelName))
            {
                strExcelName.OpenFile();
            }
        }        

        private void SendExcel()
        {
            if (listControlBindingSource1.DataSource == null)
            {
                return;
            }

            DataTable dt = ((DataTable)listControlBindingSource1.DataSource).AsEnumerable().Where(row => row["selected"].EqualDecimal(1)).CopyToDataTable();

            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Warehouse_P38");

            if (this.SaveExcel(dt, strExcelName))
            {
                DataTable dtSendAccount;
                string strSendAccount = "select * from MailTo where Description='Material locked/Unlocked'";

                DualResult result = DBProxy.Current.Select(string.Empty, strSendAccount, out dtSendAccount);
                if (result)
                {
                    if (dtSendAccount != null && dtSendAccount.Rows.Count > 0)
                    {
                        string mailto = dtSendAccount.Rows[0]["ToAddress"].ToString();
                        string mailCC = dtSendAccount.Rows[0]["CCAddress"].ToString();
                        string subject = dtSendAccount.Rows[0]["Subject"].ToString();
                        string content = dtSendAccount.Rows[0]["Content"].ToString();
                        var email = new MailTo(Sci.Env.Cfg.MailFrom, mailto, mailCC, subject, strExcelName, content, false, true);
                        email.ShowDialog(this);
                    }
                }else
                {
                    MyUtility.Msg.WarningBox(result.ToString());
                }
            }
        }

        private bool SaveExcel(DataTable printData, string strExcelName)
        {
            string sql = @"
select t.POID
	   , t.Seq1+' '+t.Seq2
	   , t.Roll
	   , t.Dyelot
	   , t.stocktype
	   , t.status
	   , t.InQty
	   , t.OutQty
	   , t.balanceqty
	   , t.location
	   , t.Description
	   , t.StyleID
	   , t.ColorID
	   , t.earliest_BuyerDelivery
	   , t.earliest_SciDelivery
	   , t.BrandID
	   , t.FactoryID
	   , LockDate = f.LockDate
	   , LockName = f.LockName
from #tmp t
inner join ftyinventory f with (NoLock) on t.ukey = f.ukey";

            DataTable k;
            DualResult result = MyUtility.Tool.ProcessWithDatatable(printData, string.Empty, sql, out k, "#Tmp");

            if (result == false)
            {
                MyUtility.Msg.WarningBox(result.ToString());
                return false;
            }
            else if (k.Rows.Count == 0)
            {
                return false;
            }

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Warehouse_P38.xltx"); //預先開啟excel app
            MyUtility.Excel.CopyToXls(k, "", "Warehouse_P38.xltx", 2, false, null, objApp);      // 將datatable copy to excel
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表

            objApp.Cells.EntireColumn.AutoFit();    //自動欄寬
            objApp.Cells.EntireRow.AutoFit();       ////自動欄高

            #region Save Excel            
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(objSheets);
            #endregion

            return true;
        }
    }
}
