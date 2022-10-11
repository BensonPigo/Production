using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Data;

namespace Sci.Production.Packing
{
    public partial class P18_Calibration_History : Sci.Win.Tems.QueryForm
    {
        private string CalibrationTimeStart;
        private string CalibrationTimeEnd;

        public P18_Calibration_History(string LastTime, string SecondTime)
        {
            this.InitializeComponent();
            this.CalibrationTimeStart = SecondTime;
            this.CalibrationTimeEnd = LastTime;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.gridInfo.IsEditingReadOnly = true;
            this.gridInfo.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridInfo)
                .Text("ID", header: "Pack ID", width: Widths.Auto(), iseditingreadonly: true)
                .Text("CTNStartNo", header: "CTN#", width: Widths.Auto(), iseditingreadonly: true)
                .Numeric("ShipQty", header: "Pack Qty", width: Widths.Auto(), iseditingreadonly: true)
                .Text("OrderID", header: "SP#", width: Widths.Auto(), iseditingreadonly: true)
                .Text("CustPONo", header: "PO#", width: Widths.Auto(), iseditingreadonly: true)
                .Text("StyleID", header: "Style", width: Widths.Auto(), iseditingreadonly: true)
                ;
            this.GetData();
        }

        private void GetData()
        {
            string sqlcmd = $@"
use Production
select pd.id,pd.CTNStartNo,
    ShipQty = sum(pd.ShipQty),
	[OrderID] = Stuff((select distinct concat( '/',OrderID)   
        from PackingList_Detail where ID = pd.ID 
        and CTNStartNo = pd.CTNStartNo and DisposeFromClog= 0  FOR XML PATH('')),1,1,''),
    o.CustPONo,
	o.StyleID
from PackingList_Detail pd with(nolock)
left join Orders o with (nolock) on o.ID = pd.OrderID
where ScanEditDate between '{this.CalibrationTimeStart}' and '{this.CalibrationTimeEnd}'
group by pd.id,pd.CTNStartNo,o.CustPONo,o.StyleID

-- CTN# 若是數值先依照數值排序
order by pd.id,iif(ISNUMERIC(CTNStartno)=0,'ZZZZZZZZ',RIGHT(REPLICATE('0', 8) + CTNStartno, 8)), RIGHT(REPLICATE('0', 8) + CTNStartno, 8)
";
            DualResult result = DBProxy.Current.Select(string.Empty, sqlcmd, out DataTable dtS);
            if (!result)
            {
                this.ShowErr(result);
            }

            this.listControlBindingSource1.DataSource = dtS;
            this.gridInfo.AutoResizeColumns();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
