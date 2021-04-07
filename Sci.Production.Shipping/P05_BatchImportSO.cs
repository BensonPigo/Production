using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    /// <inheritdoc/>
    public partial class P05_BatchImportSO : Win.Subs.Base
    {
        /// <inheritdoc/>
        public P05_BatchImportSO()
        {
            this.InitializeComponent();
            DBProxy.Current.Select(null, "select '' as ShipperID union all SELECT DISTINCT [ShipperID]=Shipper FROM GMTBooking WITH (NOLOCK) ", out DataTable shipperID);
            MyUtility.Tool.SetupCombox(this.comboShipper, 1, shipperID);
            this.comboShipper.Text = Env.User.Keyword;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            DataGridViewGeneratorTextColumnSettings setTerminal = new DataGridViewGeneratorTextColumnSettings();
            setTerminal.EditingMouseDown += (s, e) =>
            {
                if (this.grid.DataSource == null)
                {
                    return;
                }

                if (e.Button == MouseButtons.Right)
                {
                    DataRow cuttentDr = this.grid.GetDataRow(e.RowIndex);

                    string sqlcmd = string.Empty;

                    sqlcmd = $@"
select 
        fwd.WhseNo
        ,fwd.address 
        ,fwd.UKey
from  ForwarderWhse fw 
     ,ForwarderWhse_Detail fwd
 where 
        fw.ID = fwd.ID
        and fw.BrandID = '{cuttentDr["BrandID"]}'
        and fw.Forwarder = '{cuttentDr["Forwarder"]}'
        and fw.ShipModeID = '{cuttentDr["ShipModeID"]}'
 order by fwd.WhseNo
";
                    DBProxy.Current.Select(null, sqlcmd, out DataTable dt);

                    Win.Tools.SelectItem item = new Win.Tools.SelectItem(dt, "WhseNo,address", "20,20", string.Empty);

                    DialogResult result1 = item.ShowDialog();
                    if (result1 == DialogResult.Cancel)
                    {
                        return;
                    }

                    IList<DataRow> dr = item.GetSelecteds();
                    cuttentDr["ForwarderWhse_DetailUKey_ForShow"] = item.GetSelectedString();
                    cuttentDr["ForwarderWhse_DetailUKey"] = dr[0]["Ukey"];
                    cuttentDr.EndEdit();
                }
            };

            setTerminal.CellValidating += (s, e) =>
            {
                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    return;
                }

                DataRow dr = this.grid.GetDataRow<DataRow>(e.RowIndex);
                string sqlCmd = $@"
select fwd.UKey
from ForwarderWhse fw WITH (NOLOCK) 
inner join ForwarderWhse_Detail fwd WITH (NOLOCK) on fw.ID = fwd.ID
where fwd.whseno = '{e.FormattedValue}'
";
                if (MyUtility.Check.Seek(sqlCmd, out DataRow drr))
                {
                    dr["ForwarderWhse_DetailUKey_ForShow"] = e.FormattedValue;
                    dr["ForwarderWhse_DetailUKey"] = drr["Ukey"];
                }
                else
                {
                    dr["ForwarderWhse_DetailUKey_ForShow"] = string.Empty;
                    dr["ForwarderWhse_DetailUKey"] = 0;
                    MyUtility.Msg.WarningBox("Terminal/Whse# is not found!!");
                }

                dr.EndEdit();
            };

            DataGridViewGeneratorTextColumnSettings soNo = new DataGridViewGeneratorTextColumnSettings() { MaxLength = 16 };
            DataGridViewGeneratorTextColumnSettings documentRefNo = new DataGridViewGeneratorTextColumnSettings() { MaxLength = 15 };

            this.grid.IsEditingReadOnly = false;
            this.grid.DataSource = this.listControlBindingSource1;

            this.Helper.Controls.Grid.Generator(this.grid)
               .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
               .Text("ID", header: "Invoice No.", iseditingreadonly: true, width: Widths.AnsiChars(20))
               .Text("Shipper", header: "Shipper", iseditingreadonly: true, width: Widths.AnsiChars(8))
               .Text("BrandID", header: "Brand", iseditingreadonly: true, width: Widths.AnsiChars(15))
               .Text("InvDate", header: "Invoice Date", iseditingreadonly: true, width: Widths.AnsiChars(10))
               .Text("SoNo", header: "S/O#", iseditingreadonly: false, width: Widths.AnsiChars(15), settings: soNo)
               .Text("ForwarderWhse_DetailUKey_ForShow", header: "Terminal/Whse#", iseditingreadonly: false, width: Widths.AnsiChars(6), settings: setTerminal)
               .Text("DocumentRefNo", header: "Document Ref#", iseditingreadonly: false, width: Widths.AnsiChars(15), settings: documentRefNo)
               .Date("CutOffDate", header: "Cut-Off Date", iseditingreadonly: false)
               .Date("SOCFMDate", header: "S/O Cfm Date", iseditingreadonly: false)
              ;

            this.grid.Columns["Selected"].SortMode = DataGridViewColumnSortMode.NotSortable;
            this.QueryData();
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            this.QueryData();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            DataTable all = (DataTable)this.listControlBindingSource1.DataSource;

            // 1.檢查勾選
            #region
            DataRow[] selected = all.Select("Selected = 1");

            if (selected.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select one data first.");
                return;
            }
            #endregion

            // 2. 檢查 [S/O], [Terminal/Whse#], [Cut-Off Date], [S/O Cfm Date] 有一個為空則警示
            #region

            DataRow[] emptyRows = selected.CopyToDataTable().Select("SONo='' or DocumentRefNo = '' OR ForwarderWhse_DetailUKey=NULL OR ForwarderWhse_DetailUKey=0 OR CutOffDate IS NULL OR SOCFMDate IS NULL ");

            if (emptyRows.Length > 0)
            {
                string msg = " Below [Invoice No.] needs to fill up [S/O], [Terminal/Whse#], [Cut-Off Date], [S/O Cfm Date], [Document Ref#]!" + Environment.NewLine;

                foreach (DataRow item in emptyRows)
                {
                    msg += $"[Invoice No.]: {item["ID"]}" + Environment.NewLine;
                }

                MyUtility.Msg.WarningBox(msg);
                return;
            }
            #endregion

            // 3. 檢查沒問題，開始拼湊SQL
            #region
            StringBuilder sqlCmd = new StringBuilder();

            foreach (DataRow item in selected)
            {
                // 3-1 修改GMTBooking
                sqlCmd.Append($@"UPDATE GMTBooking SET SONo='{item["SONo"]}', DocumentRefNo = '{item["DocumentRefNo"]}', ForwarderWhse_DetailUKey ={item["ForwarderWhse_DetailUKey"]} , CutOffDate='{Convert.ToDateTime(item["CutOffDate"]).ToString("yyyy/MM/dd hh:mm:ss")}' , SOCFMDate='{Convert.ToDateTime(item["SOCFMDate"]).ToString("yyyy/MM/dd hh:mm:ss")}' WHERE ID = '{item["ID"]}' " + Environment.NewLine + Environment.NewLine);

                // 3-2 P05_SOCFMDate.cs 35-50行做對應的更新
                bool firstCFM = !MyUtility.Check.Seek($"SELECT ID FROM GMTBooking_History WITH (NOLOCK) WHERE ID = '{item["ID"]}' AND HisType = 'SOCFMDate'");

                sqlCmd.Append($@"
INSERT INTO GMTBooking_History (ID ,HisType ,OldValue ,NewValue ,AddName ,AddDate)
VALUES (
     '{item["ID"]}'
    ,'SOCFMDate'
    ,'{(firstCFM ? string.Empty : "CFM")}'
    ,'Un CFM'
    ,'{Env.User.UserID}'
    ,GETDATE()
)
" + Environment.NewLine + Environment.NewLine);
            }
            #endregion

            DualResult result;
            using (TransactionScope scope = new TransactionScope())
            {
                if (result = DBProxy.Current.Execute(null, sqlCmd.ToString()))
                {
                    scope.Complete();
                }
            }

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            MyUtility.Msg.InfoBox("Success!");
            this.QueryData();
        }

        private void QueryData()
        {
            string sqlCmd = string.Empty;
            DualResult result;

            sqlCmd = $@"

 SELECT 
    [Selected]=0
    ,g.ID
    ,Shipper
    ,g.BrandID
    ,InvDate
    ,[SONo]
    ,ForwarderWhse_DetailUKey
    ,[ForwarderWhse_DetailUKey_ForShow]=fwd.WhseNo
    ,[CutOffDate]
    ,[SOCFMDate]
    ,g.Forwarder 
    ,g.ShipModeID 
    ,g.DocumentRefNo
FROM GMTBooking g
LEFT JOIN ForwarderWhse_Detail fwd ON  g.ForwarderWhse_DetailUKey=fwd.UKey
LEFT JOIN ForwarderWhse fw ON  fw.ID = fwd.ID
WHERE 1=1 AND g.Status='New'
" + Environment.NewLine;

            if (!string.IsNullOrEmpty(this.comboShipper.Text))
            {
                sqlCmd += $"AND g.Shipper='{this.comboShipper.Text}'" + Environment.NewLine;
            }

            if (!string.IsNullOrEmpty(this.txtbrand.Text))
            {
                sqlCmd += $"AND g.BrandID='{this.txtbrand.Text}'" + Environment.NewLine;
            }

            if (!this.dateInvDate.Value1.Empty() && !this.dateInvDate.Value2.Empty())
            {
                sqlCmd += $"AND g.InvDate Between '{this.dateInvDate.Value1.Value.ToString("yyyy/MM/dd hh:mm:ss")}' AND '{this.dateInvDate.Value2.Value.AddDays(1).AddSeconds(-1).ToString("yyyy/MM/dd hh:mm:ss")}'" + Environment.NewLine;
            }

            sqlCmd += "ORDER BY g.ID" + Environment.NewLine;

            this.ShowWaitMessage("Data Loading....");
            if (result = DBProxy.Current.Select(null, sqlCmd, out DataTable dt))
            {
                if (dt.Rows.Count == 0)
                {
                    this.HideWaitMessage();
                    MyUtility.Msg.WarningBox("Data not found!!");
                }
            }
            else
            {
                this.ShowErr(result);
            }

            this.listControlBindingSource1.DataSource = dt;
            this.HideWaitMessage();
        }
    }
}
