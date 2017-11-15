using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win.UI;

namespace Sci.Production.Subcon
{
    /// <summary>
    /// P03_P04_Import
    /// </summary>
    public partial class P03_P04_Import : Sci.Win.Forms.Base
    {
        private DataRow dr_master;
        private DataTable dt_detail;
        private DataRow Dtl;
        private DataTable dt;
        private string formCode;
        private string Sqlcmd;
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;

        /// <summary>
        /// Dr_master
        /// </summary>
        public DataRow Dr_master
        {
            get
            {
                return this.dr_master;
            }

            set
            {
                this.dr_master = value;
            }
        }

        /// <summary>
        /// Dt_detail
        /// </summary>
        public DataTable Dt_detail
        {
            get
            {
                return this.dt_detail;
            }

            set
            {
                this.dt_detail = value;
            }
        }

        /// <summary>
        /// FormCode
        /// </summary>
        public string FormCode
        {
            get
            {
                return this.formCode;
            }

            set
            {
                this.formCode = value;
            }
        }

        /// <summary>
        /// P03_P04_Import
        /// </summary>
        /// <param name="master">DataRow master</param>
        /// <param name="detail">DataTable detail</param>
        /// <param name="formCode">string formCode</param>
        public P03_P04_Import(DataRow master, DataTable detail, string formCode, string sqlcmd, DataRow dtl)
        {
            this.InitializeComponent();
            this.Dt_detail = detail;
            this.dr_master = master;
            this.FormCode = formCode;
            this.Sqlcmd = sqlcmd;
            this.Dtl = dtl;
            this.Text = "Import" + " (" + this.FormCode + ")  ";
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            Ict.DualResult result;

            if (result = DBProxy.Current.Select(null, Sqlcmd, out dt))
            {
                if (dt.Rows.Count > 0)
                {
                    dt.Columns.Add("Selected", typeof(bool));
                }

                this.listControlBindingSource1.DataSource = dt;
            }
            else
            {
                this.ShowErr(Sqlcmd.ToString(), result);
            }

            this.gridImport.IsEditingReadOnly = false;
            this.gridImport.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridImport)
                .CheckBox("Selected", header: string.Empty, width: Ict.Win.Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                .Text("id", header: "POID", iseditingreadonly: true, width: Ict.Win.Widths.AnsiChars(6))
                .Text("artworktypeid", header: "Artwork Type", iseditingreadonly: true, width: Ict.Win.Widths.AnsiChars(8))
                .Text("artworkid", header: "Artwork", iseditingreadonly: true, width: Ict.Win.Widths.AnsiChars(8))
                .Text("patterncode", header: "Cutpart", iseditingreadonly: true, width: Ict.Win.Widths.AnsiChars(8))
                .Text("patterndesc", header: "Cutpart", iseditingreadonly: true, width: Ict.Win.Widths.AnsiChars(8))
                .Numeric("poqty", header: "PoQty", iseditingreadonly: true, width: Ict.Win.Widths.AnsiChars(6))
                .Numeric("farmout", header: "FarmOut", iseditingreadonly: true, width: Ict.Win.Widths.AnsiChars(8))
                .Numeric("farmin", header: "FarmIn", iseditingreadonly: true, width: Ict.Win.Widths.AnsiChars(8))
                .Numeric("ukey", header: "Ukey", iseditingreadonly: true, width: Ict.Win.Widths.AnsiChars(8))
                .Text("Status", header: "Status", iseditingreadonly: true, width: Ict.Win.Widths.AnsiChars(8))
               ;
            this.gridImport.AutoResizeColumns();
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.gridImport) || this.gridImport.Rows.Count == 0)
            {
                return;
            }
            this.gridImport.ValidateControl(); // 不能刪除，放在select前面，防止checkbox直接點header會卻少第一筆狀況
            DataRow[] dr2 = dt.Select("Selected = 'True'");
            int c = 0;
            foreach (DataRow tmp in dr2)
            {
                DataRow[] findrow = this.dt.Select("Status = 'New'");
                if (findrow.Length > 0)
                {
                    MyUtility.Msg.WarningBox("PO is not confirmed yet");
                }
                else
                {
                    if (FormCode == "P03")
                    {
                        if (c == 0)
                        {
                            Dtl["artworkpoID"] = tmp["id"];
                            Dtl["artworkid"] = tmp["artworkid"];
                            Dtl["patterncode"] = tmp["patterncode"];
                            Dtl["patterndesc"] = tmp["patterndesc"];
                            Dtl["artworkpoqty"] = tmp["poqty"];
                            Dtl["onhand"] = tmp["farmout"];
                            Dtl["ArtworkPo_DetailUkey"] = tmp["Ukey"];
                            Dtl["qty"] = (decimal)tmp["poqty"] - (decimal)tmp["farmout"];
                            Dtl["Variance"] = 0;
                            Dtl["BalQty"] = 0;
                            Dtl["ukey"] = 0;
                        }
                        else
                        {
                            DataRow ddr = dt_detail.NewRow();
                            ddr["Orderid"] = Dtl["Orderid"];
                            ddr["Styleid"] = Dtl["Styleid"];
                            ddr["artworkpoID"] = tmp["id"];
                            ddr["artworkid"] = tmp["artworkid"];
                            ddr["patterncode"] = tmp["patterncode"];
                            ddr["patterndesc"] = tmp["patterndesc"];
                            ddr["artworkpoqty"] = tmp["poqty"];
                            ddr["onhand"] = tmp["farmout"];
                            ddr["ArtworkPo_DetailUkey"] = tmp["Ukey"];
                            ddr["qty"] = (decimal)tmp["poqty"] - (decimal)tmp["farmout"];
                            ddr["Variance"] = 0;
                            ddr["BalQty"] = 0;
                            ddr["ukey"] = 0;
                            dt_detail.Rows.Add(ddr);
                        }
                    }
                    if (FormCode == "P04")
                    {
                        if (c == 0)
                        {
                            Dtl["artworkpoID"] = tmp["id"];
                            Dtl["artworkid"] = tmp["artworkid"];
                            Dtl["patterncode"] = tmp["patterncode"];
                            Dtl["patterndesc"] = tmp["patterndesc"];
                            Dtl["artworkpoqty"] = tmp["poqty"];
                            Dtl["onhand"] = tmp["farmin"];
                            Dtl["ArtworkPo_DetailUkey"] = tmp["Ukey"];
                            Dtl["qty"] = (decimal)tmp["farmout"] - (decimal)tmp["farmin"];
                            Dtl["Variance"] = 0;
                            Dtl["BalQty"] = 0;
                            Dtl["ukey"] = 0;
                        }
                        else
                        {
                            DataRow ddr = dt_detail.NewRow();
                            ddr["Orderid"] = Dtl["Orderid"];
                            ddr["Styleid"] = Dtl["Styleid"];
                            ddr["artworkpoID"] = tmp["id"];
                            ddr["artworkid"] = tmp["artworkid"];
                            ddr["patterncode"] = tmp["patterncode"];
                            ddr["patterndesc"] = tmp["patterndesc"];
                            ddr["artworkpoqty"] = tmp["poqty"];
                            ddr["onhand"] = tmp["farmin"];
                            ddr["ArtworkPo_DetailUkey"] = tmp["Ukey"];
                            ddr["qty"] = (decimal)tmp["farmout"] - (decimal)tmp["farmin"];
                            ddr["Variance"] = 0;
                            ddr["BalQty"] = 0;
                            ddr["ukey"] = 0;
                            dt_detail.Rows.Add(ddr);
                        }
                    }
                    c++;
                }
            }

            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
