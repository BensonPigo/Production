using Ict.Win;
using Sci.Data;
using System;
using System.Data;

namespace Sci.Production.Subcon
{
    /// <inheritdoc/>
    public partial class P11_History : Win.Forms.Base
    {
        private DataRow masterrow;

        /// <summary>
        /// Initializes a new instance of the <see cref="P11_History"/> class.
        /// </summary>
        /// <param name="mdr">Data</param>
        public P11_History(DataRow mdr)
        {
            this.InitializeComponent();
            this.EditMode = false;
            this.masterrow = mdr;
        }

        private DataTable dt;

        private void Query()
        {
            string sqlcmd = $@"
select  
     sj.OrderID,
     o.StyleID, 
     sj.ComboType,
     oq.Article,
     Qty = Sum(oq.Qty),
     sj.SubconQty, 
     sj.SubconQty,
     sj.UnitPrice,
     sj.AddName,
     sj.addDate,
     sj.Reason,
     ContractNumbers1 = (SELECT STUFF((
                            SELECT distinct ',' + sd.ContractNumber
                            FROM SubconOutContract_Detail sd with(nolock)
                            Inner Join SubconOutContract s with(nolock) on sd.SubConOutFty = s.SubConOutFty and s.ContractNumber = sd.ContractNumber
                            WHERE sd.OrderID = sj.OrderID
                              AND sd.ComboType = sj.ComboType
                              AND sd.Article = oq.Article
                              AND s.Status = 'Confirmed'
                            FOR XML PATH(''), TYPE
                         ).value('.', 'NVARCHAR(MAX)'), 1, 1, '')) ,
	 Style = o.StyleID
from SubconOutContract_Junk sj with(nolock)
inner join Orders o with(nolock) on sj.OrderID = o.ID
inner join Order_Qty oq with(nolock) on oq.ID = sj.OrderID and oq.article = sj.article
where sj.SubConOutFty = '{this.masterrow["SubConOutFty"]}'
and sj.ContractNumber = '{this.masterrow["ContractNumber"]}' 
Group by sj.OrderID, o.StyleID, sj.ComboType, oq.Article,sj.SubConOutFty,sj.UnitPrice 
        ,sj.AddName, sj.addDate, sj.SubconQty, sj.Reason";

            DBProxy.Current.Select(null, sqlcmd, out this.dt);
            this.listControlBindingSource1.DataSource = this.dt;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.Query();

            #region -- Grid 設定 --
            this.Helper.Controls.Grid.Generator(this.grid1)
            .Text("OrderID", header: "SP#", width: Widths.AnsiChars(15))
            .Text("Style", header: "Pad Refno", width: Widths.AnsiChars(15))
            .Text("ComboType", header: "ComboType", width: Widths.AnsiChars(10))
            .Text("Article", header: "Article", width: Widths.AnsiChars(10))
            .Numeric("Qty", header: "Order Qty", width: Widths.AnsiChars(10))
            .Numeric("SubconQty", header: "Subcon Out Qty ", width: Widths.AnsiChars(10))
            .Text("UnitPrice", header: "Price(Unit)", iseditingreadonly: true, iseditable: false)
            .Text("AddName", header: "Junk Name", iseditingreadonly: true, iseditable: false)
            .DateTime("AddDate", header: "Junk Date", iseditingreadonly: true, iseditable: false)
            .Text("Reason", header: "Reason", width: Widths.AnsiChars(35))
            .Text("ContractNumbers1", header: "New Contract", iseditingreadonly: true, iseditable: false)
            ;
            #endregion
        }

        /// <inheritdoc/>
        protected override void OnEditModeChanged()
        {
            base.OnEditModeChanged();

            this.btnClose.Text = "Close";
            this.grid1.IsEditingReadOnly = true;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
