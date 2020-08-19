using Ict;
using Ict.Win;
using System;
using System.Data;
using System.Drawing;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    /// <inheritdoc/>
    public partial class P14_Old : Win.Tems.QueryForm
    {
        private DataTable dtCertOfOrigin;
        private DataTable dtExport;
        private DataSet ds = new DataSet();

        /// <summary>
        /// Initializes a new instance of the <see cref="P14_Old"/> class.
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public P14_Old(ToolStripMenuItem menuitem)
             : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            // dateRange 變色
            this.dateETA.DateBox1.TextBackColor = Color.Pink;
            this.dateETA.DateBox2.TextBackColor = Color.Pink;

            #region 欄位事件設定

            DataGridViewGeneratorDateColumnSettings col_SendDate = new DataGridViewGeneratorDateColumnSettings();
            Ict.Win.UI.DataGridViewTextBoxColumn col_MaxLength15;
            Ict.Win.UI.DataGridViewTextBoxColumn col_MaxLength20;
            Ict.Win.UI.DataGridViewTextBoxColumn col_MaxLength30;

            col_SendDate.CellValidating += (s, e) =>
            {
                if (e.RowIndex == -1 || e.FormattedValue == null)
                {
                    return;
                }

                DataRow dr = this.gridCertOfOrigin.GetDataRow<DataRow>(e.RowIndex);

                if (!MyUtility.Check.Empty(e.FormattedValue) && !MyUtility.Check.Empty(dr["ReceiveDate"]))
                {
                    DateTime date1 = Convert.ToDateTime(dr["ReceiveDate"]);
                    DateTime date2 = Convert.ToDateTime(e.FormattedValue);
                    if (DateTime.Compare(date1, date2) < 0)
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("<Form Rcvd Date> cannot be earlier than [Form Send Date]!");
                        return;
                    }
                }
            };
            #endregion

            #region 欄位設定

            // Grid CertOfOrigin
            this.gridCertOfOrigin.IsEditingReadOnly = false;
            this.gridCertOfOrigin.DataSource = this.listControlBindingSource1;

            this.Helper.Controls.Grid.Generator(this.gridCertOfOrigin)
                .Text("SuppID", header: "Supplier", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("FormXPayINV", header: "Payment Invoice#", width: Widths.AnsiChars(18), iseditingreadonly: true)
                .Text("COName", header: "Form C/O Name", width: Widths.AnsiChars(15)).Get(out col_MaxLength15)
                .Date("ReceiveDate", header: "Form Rcvd Date", width: Widths.AnsiChars(10))
                .Text("Carrier", header: "Carrier", width: Widths.AnsiChars(18)).Get(out col_MaxLength30)
                .Text("AWBNo", header: "AWB#", width: Widths.AnsiChars(20)).Get(out col_MaxLength20)
                .Date("SendDate", header: "Form Send Date", width: Widths.AnsiChars(10), settings: col_SendDate)
                ;

            this.gridCertOfOrigin.Columns["COName"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridCertOfOrigin.Columns["ReceiveDate"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridCertOfOrigin.Columns["Carrier"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridCertOfOrigin.Columns["AWBNo"].DefaultCellStyle.BackColor = Color.Pink;
            col_MaxLength15.MaxLength = 15;
            col_MaxLength20.MaxLength = 20;
            col_MaxLength30.MaxLength = 30;

            // Grid CertOfOrigin
            this.gridExport.IsEditingReadOnly = true;
            this.gridExport.DataSource = this.listControlBindingSource2;

            this.Helper.Controls.Grid.Generator(this.gridExport)
                .Text("ExportID", header: "WK No", width: Widths.AnsiChars(15))
                .Text("FactoryID", header: "Factory", width: Widths.AnsiChars(8))
                .Text("Consignee", header: "Consignee", width: Widths.AnsiChars(8))
                .Date("Eta", header: "ETA", width: Widths.AnsiChars(10))
                .Text("ShipModeID", header: "ShipModeID", width: Widths.AnsiChars(10))
                .Text("Loading", header: "Loading", width: Widths.AnsiChars(20))
                .Text("InvNo", header: "Invoice #", width: Widths.AnsiChars(20))
                .Text("Vessel", header: "Vessel Name", width: Widths.AnsiChars(20))
                .Text("Blno", header: "B/L No", width: Widths.AnsiChars(16))
                .Text("UnitID", header: "Unit", width: Widths.AnsiChars(6))
                .Numeric("ShipQty", header: "Ship Qty", integer_places: 12, decimal_places: 2)
                ;
            #endregion
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            this.QueryData();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            this.gridCertOfOrigin.ValidateControl();
            DataTable dt = this.dtCertOfOrigin.Copy();
            foreach (DataRow dr in dt.Rows)
            {
                if ((!MyUtility.Check.Empty(dr["COName"]) ||
                    !MyUtility.Check.Empty(dr["ReceiveDate"]) ||
                    !MyUtility.Check.Empty(dr["Carrier"]) ||
                    !MyUtility.Check.Empty(dr["AWBNo"]))
                    &&
                    (MyUtility.Check.Empty(dr["COName"]) ||
                    MyUtility.Check.Empty(dr["ReceiveDate"]) ||
                    MyUtility.Check.Empty(dr["Carrier"]) ||
                    MyUtility.Check.Empty(dr["AWBNo"])))
                {
                    MyUtility.Msg.WarningBox(" <Form C/O Name>、<Form Rcvd Date>、<Carrier> and <AWB#> cannot be empty.");
                    return;
                }
            }

            string sqlcmd = $@"
MERGE CertOfOrigin as t
using (
    select * from  #tmp 
    where (COName is not null or COName != '')
    and (ReceiveDate is not null or ReceiveDate != '')
    and (Carrier is not null or Carrier != '')
    and (AWBNo is not null or AWBNo != '')
)as s
on t.suppid = s.suppid and s.FormXPayINV = t.FormXPayINV
WHEN MATCHED THEN
UPDATE SET
	t.COName  = s.COName ,
	t.ReceiveDate  = convert(date,s.ReceiveDate) ,
	t.Carrier = s.Carrier,
	t.AWBNo = s.AWBNo,
	t.SendDate = convert(date, s.SendDate),
	t.EditDate = getdate(),
	t.EditName = '{Env.User.UserID}'
WHEN NOT MATCHED BY TARGET THEN
INSERT(SuppID,FormXPayINV,COName,ReceiveDate,Carrier,AWBNo,SendDate,AddDate,AddName )
Values(s.SuppID,s.FormXPayINV,s.COName,convert(date,s.ReceiveDate),s.Carrier,s.AWBNo,convert(date, s.SendDate),GetDate(),'{Env.User.UserID}' );";

            DualResult result1 = Ict.Result.True;
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    if (dt.Rows.Count > 0)
                    {
                        DataTable dtresult;
                        result1 = MyUtility.Tool.ProcessWithDatatable(dt, null, sqlcmd, out dtresult);
                        if (result1)
                        {
                            transactionScope.Complete();
                            transactionScope.Dispose();
                            MyUtility.Msg.InfoBox("Save successfully.");
                        }
                        else
                        {
                            transactionScope.Dispose();
                            this.ShowErr(result1);
                            this.HideWaitMessage();
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    transactionScope.Dispose();
                    this.ShowErr(ex);
                    this.HideWaitMessage();
                    return;
                }
            }

            this.HideWaitMessage();
            this.QueryData();
        }

        private void QueryData()
        {
            string sqlcmd;
            string sqlwhere = string.Empty;
            this.listControlBindingSource1.DataSource = null;
            this.listControlBindingSource2.DataSource = null;

            if ((!this.dateETA.HasValue1 || !this.dateETA.HasValue2) && MyUtility.Check.Empty(this.txtPayInv.Text))
            {
                MyUtility.Msg.WarningBox("Please input <ETA> first!");
                return;
            }

            this.ShowWaitMessage("Data Loading....");

            #region Where
            if (!MyUtility.Check.Empty(this.txtPayInv.Text))
            {
                sqlwhere += $@" and ed.FormXPayINV = '{this.txtPayInv.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtSupp.Text))
            {
                sqlwhere += $@" and ed.SuppID = '{this.txtSupp.Text}'";
            }

            if (this.dateETA.HasValue1 && this.dateETA.HasValue2)
            {
                sqlwhere += $@" 
and e.eta between '{this.dateETA.Value1.Value.ToShortDateString()}' 
and '{this.dateETA.Value2.Value.ToShortDateString()}'";
            }
            #endregion
            sqlcmd = $@"
select distinct
ed.SuppID
,ed.FormXPayINV
,co.COName,co.ReceiveDate,co.Carrier,co.AWBNo,co.SendDate
from Export_Detail ed
inner join Export e on e.ID=ed.ID
left join CertOfOrigin co on ed.SuppID=co.SuppID and ed.FormXPayINV = co.FormXPayINV
where ed.SuppID !='' and ed.FormXPayINV !=''
{sqlwhere}
order by ed.SuppID

select 
ed.SuppID
,ed.FormXPayINV
,[ExportID]  = e.ID
,e.FactoryID
,e.Consignee,e.Eta,e.ShipModeID
,[Loading] = e.ExportPort+'-'+e.ExportCountry
,e.InvNo
,e.Vessel
,e.Blno
,ed.UnitId
,[ShipQty] = sum(ed.Qty)
from Export e
inner join Export_Detail ed on ed.ID=e.ID
where SuppID !='' and FormXPayINV !=''
{sqlwhere}
group by e.id,e.FactoryID,e.Consignee,e.Eta,e.ShipModeID
,e.ExportPort,e.ExportCountry,e.InvNo,e.Vessel,e.Blno,ed.UnitId
,ed.FormXPayINV,ed.SuppID
";

            if (!SQL.Selects(string.Empty, sqlcmd, out this.ds))
            {
                MyUtility.Msg.WarningBox(sqlcmd, "DB error!!");
                return;
            }

            this.HideWaitMessage();

            this.dtCertOfOrigin = this.ds.Tables[0];
            this.dtExport = this.ds.Tables[1];
            this.dtCertOfOrigin.TableName = "CertOfOrigin1";
            this.dtExport.TableName = "Export";

            if (this.dtCertOfOrigin.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!!");
            }

            DataRelation rel = new DataRelation(
                "rel",
                new DataColumn[] { this.dtCertOfOrigin.Columns["SuppID"], this.dtCertOfOrigin.Columns["FormXPayINV"] },
                new DataColumn[] { this.dtExport.Columns["SuppID"], this.dtExport.Columns["FormXPayINV"] });

            this.ds.Relations.Add(rel);

            this.listControlBindingSource1.DataSource = this.ds;
            this.listControlBindingSource1.DataMember = "CertOfOrigin1";
            this.listControlBindingSource2.DataSource = this.listControlBindingSource1;
            this.listControlBindingSource2.DataMember = "rel";
        }
    }
}
