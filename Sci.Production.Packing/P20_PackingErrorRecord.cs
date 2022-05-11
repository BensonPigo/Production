using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Packing
{
    /// <inheritdoc/>
    public partial class P20_PackingErrorRecord : Sci.Win.Subs.Input4
    {
        private DataRow drDetail;
        private string factory;

        /// <inheritdoc/>
        public P20_PackingErrorRecord(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3, DataRow mainDr)
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
            this.txtSPNo.Text = this.drDetail["OrderID"].ToString();
            this.txtPONo.Text = this.drDetail["CustPONo"].ToString();
            this.txtStyle.Text = this.drDetail["StyleID"].ToString();
            this.txtSeason.Text = this.drDetail["SeasonID"].ToString();
            this.txtBrand.Text = this.drDetail["BrandID"].ToString();
            this.txtErrorType.Text = this.drDetail["ErrorType"].ToString();
            this.txtDestination.Text = this.drDetail["Alias"].ToString();
            this.dateBuyerDel.Value = (DateTime)this.drDetail["BuyerDelivery"];
            this.txtRemark.Text = this.drDetail["Remark"].ToString();

            string selectCommand = $@"
select * from PackingErrorRecord
where PackID = '{this.drDetail["PackID"]}'
and CTN = '{this.drDetail["CTN"].ToString()}'
";
            DualResult returnResult = DBProxy.Current.Select(null, selectCommand, out DataTable dtDtail);
            if (!returnResult)
            {
                return;
            }

            this.SetGrid(dtDtail);
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
            this.Helper.Controls.Grid.Generator(this.grid)
                .ComboBox("Line", header: "Line", width: Widths.AnsiChars(9), iseditable: true, settings: this.Col_comboLine())
                .ComboBox("Shift", header: "Shift", width: Widths.AnsiChars(9), iseditable: true, settings: this.Col_comboShift())
                .Text("ReasonforGarmentSound", header: "Reason for Garment Sound", width: Widths.AnsiChars(20), iseditingreadonly: false)
                .Text("AreaOperation", header: "Area/Operation", width: Widths.AnsiChars(20), iseditingreadonly: false)
                .Text("ActionTaken", header: "Action Taken", width: Widths.AnsiChars(20), iseditingreadonly: false)
                ;
            return true;
        }

        protected override void OnInsert()
        {
            DataTable dt = (DataTable)this.gridbs.DataSource;
            base.OnInsert();

            DataRow selectDr = ((DataRowView)this.grid.GetSelecteds(SelectedSort.Index)[0]).Row;

            selectDr["CTN"] = this.drDetail["CTN"].ToString();
            selectDr["PackID"] = this.drDetail["PackID"].ToString();
        }

        private DataGridViewGeneratorComboBoxColumnSettings Col_comboLine()
        {
            var ts = new DataGridViewGeneratorComboBoxColumnSettings();
            var sql = $@"select ID from SewingLine where Junk = 0 and FactoryID = '{this.factory}'";
            var result = DBProxy.Current.Select(null, sql, out DataTable dtList);
            if (!result)
            {
                this.ShowErr(result.ToString());
                return ts;
            }

            ts.DataSource = dtList;
            ts.ValueMember = "ID";
            ts.DisplayMember = "ID";
            return ts;
        }

        private DataGridViewGeneratorComboBoxColumnSettings Col_comboShift()
        {
            var ts = new DataGridViewGeneratorComboBoxColumnSettings();
            var sql = $@"select ID from SewingTeam where Junk = 0";
            var result = DBProxy.Current.Select(null, sql, out DataTable dtList);
            if (!result)
            {
                this.ShowErr(result.ToString());
                return ts;
            }

            ts.DataSource = dtList;
            ts.ValueMember = "ID";
            ts.DisplayMember = "ID";
            return ts;
        }

        /// <inheritdoc/>
        protected override bool OnSaveBefore()
        {
            this.grid.ValidateControl();
            this.gridbs.EndEdit();
            foreach (DataRow dr in this.Datas)
            {
                if (MyUtility.Check.Empty(dr["Line"]) &&
                    MyUtility.Check.Empty(dr["Shift"]))
                {
                    dr.Delete();
                }
            }

            return base.OnSaveBefore();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
