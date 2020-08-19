using Ict;
using System;
using System.Data;
using Sci.Data;
using Ict.Win;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class P02_PackingMethod : Win.Subs.Input4
    {
        private string cuttingid;

        private DataTable ODT;

        /// <summary>
        /// Initializes a new instance of the <see cref="P02_PackingMethod"/> class.
        /// </summary>
        /// <param name="canedit">Can Edit</param>
        /// <param name="keyvalue1">keyvalue1</param>
        /// <param name="keyvalue2">keyvalue2</param>
        /// <param name="keyvalue3">keyvalue3</param>
        public P02_PackingMethod(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            this.InitializeComponent();
            this.cuttingid = keyvalue1;
        }

        /// <inheritdoc/>
        protected override bool OnGridSetup()
        {
            this.Helper.Controls.Grid.Generator(this.grid)
                .Text("ID", header: "SP#", width: Widths.AnsiChars(16));
            return true;
        }

        /// <inheritdoc/>
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

            return Ict.Result.True;
        }

        private void BtnBreakdown_Click(object sender, EventArgs e)
        {
            PPIC.P01_QtyCTN callNextForm = new PPIC.P01_QtyCTN(this.ODT.Rows[0]);
            callNextForm.ShowDialog(this);
        }

        private void Gridbs_PositionChanged(object sender, EventArgs e)
        {
            DBProxy.Current.Select(null, string.Format("SELECT *,isnull([dbo].getPOComboList(o.ID,o.POID),'') as PoList FROM ORDERS o WITH (NOLOCK)  WHERE ID = '{0}'", this.CurrentData["id"]), out this.ODT);
            this.btnBreakdown.Enabled = this.ODT.Rows.Count != 0 && MyUtility.Convert.GetString(this.ODT.Rows[0]["CtnType"]) == "2";
        }
    }
}
