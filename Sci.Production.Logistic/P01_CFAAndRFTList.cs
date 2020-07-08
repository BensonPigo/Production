using System.Data;
using Ict;
using Ict.Win;
using Sci.Data;

namespace Sci.Production.Logistic
{
    /// <summary>
    /// Logistic_P01_CFAAndRFTList
    /// </summary>
    public partial class P01_CFAAndRFTList : Win.Subs.Base
    {
        private DataRow masterData;

        /// <summary>
        /// P01_CFAAndRFTList
        /// </summary>
        /// <param name="masterData">masterData</param>
        public P01_CFAAndRFTList(DataRow masterData)
        {
            this.InitializeComponent();
            this.masterData = masterData;
        }

        /// <summary>
        /// OnFormLoaded()
        /// </summary>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataTable cFA, rFT;
            string sqlCmd = string.Format(
                @"select c.cDate,c.SewingLineID,isnull(s.SewingCell,'') as SewingCell,c.InspectQty,c.DefectQty,c.Remark,'{0}' as POID,
isnull((select Alias from Country WITH (NOLOCK) where ID = '{1}'),'') as Alias,
case when c.Result = 'P' then 'Pass' when c.Result = 'F' then 'Fail' else '' end as Result,
iif(c.Status = 'New','N','Y') as Status, iif(c.InspectQty = 0,0,round(cast(c.DefectQty as decimal)/c.InspectQty*100,2)) as SQR
from Cfa c WITH (NOLOCK) 
left join SewingLine s WITH (NOLOCK) on c.SewingLineID = s.ID and c.FactoryID = s.FactoryID
where c.OrderID = '{2}'
order by c.cDate",
                this.masterData["POID"].ToString(),
                this.masterData["Dest"].ToString(),
                this.masterData["ID"].ToString());
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out cFA);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query CFA fail!!" + result.ToString());
            }

            this.listControlBindingSource1.DataSource = cFA;

            sqlCmd = string.Format(
                @"select r.CDate,r.SewinglineID,isnull(s.SewingCell,'') as SewingCell,r.InspectQty,r.RejectQty,r.Remark,
isnull((select Alias from Country WITH (NOLOCK) where ID = '{0}'),'') as Alias,
iif(r.Status = 'New','N','Y') as Status, iif(r.InspectQty = 0,0,round(cast((r.InspectQty-r.RejectQty) as decimal)/r.InspectQty*100,2)) as RFT
from Rft r WITH (NOLOCK) 
left join SewingLine s WITH (NOLOCK) on r.SewingLineID = s.ID and r.FactoryID = s.FactoryID
where r.OrderID = '{1}'
order by r.cDate",
                this.masterData["Dest"].ToString(),
                this.masterData["ID"].ToString());
            result = DBProxy.Current.Select(null, sqlCmd, out rFT);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query RFT fail!!" + result.ToString());
            }

            this.listControlBindingSource2.DataSource = rFT;

            // 設定Grid1的顯示欄位
            this.gridCertifiedFactoryAudit.IsEditingReadOnly = true;
            this.gridCertifiedFactoryAudit.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridCertifiedFactoryAudit)
                .Date("cDate", header: "Audit date", width: Widths.AnsiChars(10))
                .Text("SewingLineID", header: "Line#", width: Widths.AnsiChars(2))
                .Text("SewingCell", header: "Cell#", width: Widths.AnsiChars(2))
                .Numeric("InspectQty", header: "Inspect Qty", width: Widths.AnsiChars(7))
                .Numeric("DefectQty", header: "Defects", width: Widths.AnsiChars(7))
                .Numeric("SQR", header: "SQR(%)", width: Widths.AnsiChars(8), decimal_places: 2)
                .Text("Result", header: "Result", width: Widths.AnsiChars(4))
                .Text("Status", header: "Confirmed", width: Widths.AnsiChars(2))
                .EditText("Remark", header: "Remark", width: Widths.AnsiChars(20))
                .Text("POID", header: "PO#", width: Widths.AnsiChars(13))
                .Text("Alias", header: "Destination", width: Widths.AnsiChars(13));

            // 設定Grid2的顯示欄位
            this.gridRightFirstTime.IsEditingReadOnly = true;
            this.gridRightFirstTime.DataSource = this.listControlBindingSource2;
            this.Helper.Controls.Grid.Generator(this.gridRightFirstTime)
                .Date("cDate", header: "Date", width: Widths.AnsiChars(10))
                .Text("SewingLineID", header: "Line#", width: Widths.AnsiChars(2))
                .Text("SewingCell", header: "Cell#", width: Widths.AnsiChars(2))
                .Numeric("InspectQty", header: "Qty Inspected", width: Widths.AnsiChars(7))
                .Numeric("RejectQty", header: "Qty Reject", width: Widths.AnsiChars(7))
                .Numeric("RFT", header: "RFT(%)", width: Widths.AnsiChars(8), decimal_places: 2)
                .Text("Status", header: "Confirmed", width: Widths.AnsiChars(2))
                .Text("Alias", header: "Destination", width: Widths.AnsiChars(13))
                .EditText("Remark", header: "Remark", width: Widths.AnsiChars(20));
        }
    }
}
