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

namespace Sci.Production.Logistic
{
    /// <inheritdoc/>
    public partial class P16_BrokenNeedlesRecord : Sci.Win.Subs.Input4
    {
        private DataRow drDetail;
        private string factory;

        /// <inheritdoc/>
        public P16_BrokenNeedlesRecord(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3, DataRow mainDr)
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
            this.txtSPNo.Text = this.drDetail["ID"].ToString();
            this.txtPONo.Text = this.drDetail["CustPONo"].ToString();
            this.txtStyle.Text = this.drDetail["StyleID"].ToString();
            this.txtBrand.Text = this.drDetail["BrandID"].ToString();
            this.dateBuyerDel.Value = (DateTime)this.drDetail["BuyerDelivery"];
            this.dateSCIDel.Value = (DateTime)this.drDetail["SCIDelivery"];

            // sql參數
            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter();
            sp1.ParameterName = "@OrderID";
            sp1.Value = this.drDetail["ID"].ToString();
            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            cmds.Add(sp1);

            string selectCommand = @"
select * from BrokenNeedlesRecord
where SP = @OrderID
";
            DualResult returnResult = DBProxy.Current.Select(null, selectCommand, cmds, out DataTable dtDtail);
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
                .CheckBox("NeedleComplete", header: "Needle Complete", width: Widths.AnsiChars(6), trueValue: 1, falseValue: 0, iseditable: true)
                .Text("Operation", header: "Operation", width: Widths.AnsiChars(20), iseditingreadonly: false)
                ;
            return true;
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
    }
}
