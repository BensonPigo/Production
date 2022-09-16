using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Logistic
{
    /// <inheritdoc/>
    public partial class P20_ClogErrorRecord : Sci.Win.Subs.Input4
    {
        private DataRow drDetail;
        private string factory;

        /// <inheritdoc/>
        public P20_ClogErrorRecord(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3, DataRow mainDr)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            this.InitializeComponent();
            this.drDetail = mainDr;
            this.factory = Sci.Env.User.Factory;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.txtPackID.Text = this.drDetail["PackID"].ToString();
            this.txtCTNNo.Text = this.drDetail["CTN"].ToString();
            this.txtPackQty.Text = this.drDetail["ShipQty"].ToString();
            this.txtErrQty.Text = this.drDetail["ClogPackingErrorErrQty"].ToString();
            this.txtSPNo.Text = this.drDetail["OrderID"].ToString();
            this.txtPONo.Text = this.drDetail["CustPONo"].ToString();
            this.txtStyle.Text = this.drDetail["StyleID"].ToString();
            this.txtSeason.Text = this.drDetail["SeasonID"].ToString();
            this.txtBrand.Text = this.drDetail["BrandID"].ToString();
            this.txtErrorType.Text = this.drDetail["ErrorType"].ToString();
            this.txtDestination.Text = this.drDetail["Alias"].ToString();
            if (MyUtility.Check.Empty(this.drDetail["BuyerDelivery"]))
            {
                this.dateBuyerDel.Value = null;
            }
            else
            {
                this.dateBuyerDel.Value = (DateTime)this.drDetail["BuyerDelivery"];
            }

            this.txtRemark.Text = this.drDetail["Remark"].ToString();
        }

        /// <inheritdoc/>
        protected override void OnUIConvertToMaintain()
        {
            base.OnUIConvertToMaintain();
            this.revise.Visible = false;
            this.undo.Visible = false;
        }

        /// <inheritdoc/>
        protected override bool OnGridSetup()
        {
            DataGridViewGeneratorTextColumnSettings col_ReasonforGarmentSound = CellTextPackingReason.GetGridCell("EG");
            DataGridViewGeneratorTextColumnSettings col_AreaOperation = CellTextPackingReason.GetGridCell("EO");
            DataGridViewGeneratorTextColumnSettings col_ActionTaken = CellTextPackingReason.GetGridCell("ET");

            this.Helper.Controls.Grid.Generator(this.grid)
                .Text("ReasonforGarmentSound", header: "Reason for Garment Sound", width: Widths.AnsiChars(28), iseditingreadonly: false, settings: col_ReasonforGarmentSound)
                .Text("AreaOperation", header: "Area/Operation", width: Widths.AnsiChars(28), iseditingreadonly: false, settings: col_AreaOperation)
                .Text("ActionTaken", header: "Action Taken", width: Widths.AnsiChars(28), iseditingreadonly: false, settings: col_ActionTaken)
                ;
            return true;
        }

        /// <inheritdoc/>
        protected override void OnInsert()
        {
            DataTable dt = (DataTable)this.gridbs.DataSource;
            base.OnInsert();

            DataRow selectDr = ((DataRowView)this.grid.GetSelecteds(SelectedSort.Index)[0]).Row;

            selectDr["ClogPackingErrorID"] = this.drDetail["ClogPackingErrorID"].ToString();
            selectDr["CTN"] = this.drDetail["CTN"].ToString();
            selectDr["PackID"] = this.drDetail["PackID"].ToString();
        }

        /// <summary>
        /// OnRequery
        /// </summary>
        /// <returns>bool</returns>
        protected override DualResult OnRequery()
        {
            string selectCommand = $@"
select   b.ID
        ,b.ClogPackingErrorID
        ,PackID = a.PackingListID
        ,CTN = a.CTNStartNo
        ,[ReasonforGarmentSound] = isnull((select ID+'-'+Description from PackingReason where Type = 'EG' and ID = b.PackingReasonIDForTypeEG),'')
        ,[AreaOperation] = isnull((select ID+'-'+Description from PackingReason where Type = 'EO' and ID = b.PackingReasonIDForTypeEO),'')
        ,[ActionTaken] = isnull((select ID+'-'+Description from PackingReason where Type = 'ET' and ID = b.PackingReasonIDForTypeET),'')
        ,b.PackingReasonIDForTypeEG
        ,b.PackingReasonIDForTypeEO
        ,b.PackingReasonIDForTypeET
        ,b.AddName
        ,b.AddDate
        ,b.EditName
        ,b.EditDate
from ClogPackingError a
inner join ClogPackingError_Detail b on a.ID=b.ClogPackingErrorID
where a.PackingListID = '{this.drDetail["PackID"]}'
and a.CTNStartNo = '{this.drDetail["CTN"]}'
";
            DualResult returnResult = DBProxy.Current.Select(null, selectCommand, out DataTable dtDtail);
            if (!returnResult)
            {
                return returnResult;
            }

            this.SetGrid(dtDtail);
            return Ict.Result.True;
        }
    }
}
