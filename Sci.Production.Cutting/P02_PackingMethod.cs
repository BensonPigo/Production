using Ict;
using System;
using System.Data;
using Sci.Data;
using Ict.Win;

namespace Sci.Production.Cutting
{
    public partial class P02_PackingMethod : Sci.Win.Subs.Input4
    {
        private string cuttingid;

        DataTable ODT;

        public P02_PackingMethod(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            this.InitializeComponent();
            this.cuttingid = keyvalue1;
        }

        protected override bool OnGridSetup()
        {
            this.Helper.Controls.Grid.Generator(this.grid)
                .Text("ID", header: "SP#", width: Widths.AnsiChars(16));
            return true;
        }

        protected override DualResult OnRequery(out DataTable datas)
        {
            string cmdsql = string.Format(
                @"
Select DISTINCT a.id,a.ctnqty,isnull(b.id,'0') as signalcolor,a.Packing,
case ctntype when 1 then 'Solid Color/Size' 
when 2 then 'Solid Color/Assorted Size' 
when 3 then 'Assorted Color/Solid Size'  
when 4 then 'Assorted Color/Size'  
when 5 then 'Other' 
End 'Packingmethod'   
From orders a WITH (NOLOCK) 
left join order_QtyCTN b WITH (NOLOCK) on b.id =a.id  
Where cuttingsp = '{0}'",
                this.cuttingid);
            DualResult dr = DBProxy.Current.Select(null, cmdsql, out datas);
            if (!dr)
            {
                this.ShowErr(cmdsql, dr);
                return dr;
            }

            DBProxy.Current.Select(null, string.Format("SELECT *,isnull([dbo].getPOComboList(o.ID,o.POID),'') as PoList FROM ORDERS o WITH (NOLOCK)  WHERE ID = '{0}'", this.cuttingid), out this.ODT);

            return Result.True;
        }

        private void btnBreakdown_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_QtyCTN callNextForm = new Sci.Production.PPIC.P01_QtyCTN(this.ODT.Rows[0]);
            callNextForm.ShowDialog(this);
        }

        private void gridbs_PositionChanged(object sender, EventArgs e)
        {
            DBProxy.Current.Select(null, string.Format("SELECT *,isnull([dbo].getPOComboList(o.ID,o.POID),'') as PoList FROM ORDERS o WITH (NOLOCK)  WHERE ID = '{0}'", this.CurrentData["id"]), out this.ODT);
            this.btnBreakdown.Enabled = this.ODT.Rows.Count != 0 && MyUtility.Convert.GetString(this.ODT.Rows[0]["CtnType"]) == "2";
        }
    }
}
