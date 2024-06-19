using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Prg;
using Sci.Production.PublicForm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Sci.Production.Class.Command.ProductionSystem;

namespace Sci.Production.Subcon
{
    /// <inheritdoc/>
    public partial class P12_Import : Win.Subs.Base
    {
        private DataRow dr_Default_AP;
        private DataTable dt_Default_AP_Detail;
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        private DataTable dtlocal;

        /// <inheritdoc/>
        public P12_Import(DataRow master, DataTable detail)
        {
            this.InitializeComponent();

            this.dr_Default_AP = master;
            this.dt_Default_AP_Detail = detail;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
            {
                DataRow ddr = this.gridImport.GetDataRow<DataRow>(e.RowIndex);
                ddr["Amount"] = MyUtility.Convert.GetDecimal(e.FormattedValue) * MyUtility.Convert.GetDecimal(ddr["Price"]);
                ddr["Qty"] = e.FormattedValue;
            };

            this.gridImport.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.gridImport.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridImport)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(5), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk) // 0
                .Text("ContractNumber", header: "Contract No.", iseditingreadonly: true)
                .Text("OrderID", header: "SP#", iseditingreadonly: true)
                .Text("ComboType", header: "Combo Type", iseditingreadonly: true)
                .Text("Article", header: "Article", iseditingreadonly: true)
                .Numeric("Qty", header: "Qty", settings: ns)
                .Numeric("Price", header: "Price", iseditingreadonly: true, integer_places: 12, decimal_places: 4)
                .Numeric("Amount", header: "Amount", iseditingreadonly: true, integer_places: 12, decimal_places: 4)
                .Numeric("AccuSewingQty", header: "Accu. Sewing Qty", iseditingreadonly: true)
                .Text("AccuPaidQty", header: "Accu. Paid Qty", iseditingreadonly: true)
                .Numeric("BalQty", header: "Bal.Qty", iseditingreadonly: true, decimal_places: 4, integer_places: 4);
            this.gridImport.Columns["qty"].DefaultCellStyle.BackColor = Color.Pink;  // Qty

            // 抓上一層值
            this.txtSupplier.TextBox1.ReadOnly = true;
            this.txtSupplier.TextBox1.Text = MyUtility.Convert.GetString(this.dr_Default_AP["LocalSuppID"]);

            this.txtFactory.ReadOnly = true;
            this.txtFactory.Text = MyUtility.Convert.GetString(this.dr_Default_AP["FactoryID"]);
        }

        private void BtnFind_Click(object sender, EventArgs e)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            string strWhere = string.Empty;
            string sqlcmd = string.Empty;
            string sqlcmd_F = string.Empty;

            if (!MyUtility.Check.Empty(this.txtSP1.Text))
            {
                sqlParameters.Add(new SqlParameter("@SP1", this.txtSP1.Text));
                strWhere += $@" and sd.OrderID >= @SP1 ";
            }

            if (!MyUtility.Check.Empty(this.txtSP2.Text))
            {
                sqlParameters.Add(new SqlParameter("@SP2", this.txtSP2.Text));
                strWhere += $@" and sd.OrderID <= @SP2 ";
            }

            if (!MyUtility.Check.Empty(this.txtFactory.Text))
            {
                sqlParameters.Add(new SqlParameter("@FactoryID", this.txtFactory.Text));
                strWhere += $@" and s.FactoryID = @FactoryID ";
            }

            if (!MyUtility.Check.Empty(this.txtSupplier.TextBox1.Text))
            {
                sqlParameters.Add(new SqlParameter("@Supplier", this.txtSupplier.TextBox1.Text));
                strWhere += $@" and s.SubconOutFty = @Supplier ";
            }

            if (!MyUtility.Check.Empty(this.txtContractNo_1.Text))
            {
                sqlParameters.Add(new SqlParameter("@ContractNo_1", this.txtContractNo_1.Text));
                strWhere += $@" and s.ContractNumber = @ContractNo_1 ";
            }

            if (!MyUtility.Check.Empty(this.txtContractNo_2.Text))
            {
                sqlParameters.Add(new SqlParameter("@ContractNo_2", this.txtContractNo_2.Text));
                strWhere += $@" and s.ContractNumber = @ContractNo_2 ";
            }

            sqlcmd = $@"
            select 
            Selected = 0,
            s.SubConOutFty,
            [ContractNumber] = s.ContractNumber,
            [OrderID] = sd.OrderId,
            sd.ComboType,
            sd.Article,
            [Price] = sd.UnitPrice,
            [AccuSewingQty] = sum(sod.QAQty),
            [ID] = '{this.dr_Default_AP["ID"]}',
            [UKey] = 0
            into #tmp
            from SubconOutContract s
            inner join SubconOutContract_Detail sd with(nolock) on s.SubConOutFty = sd.SubConOutFty and s.ContractNumber = sd.ContractNumber
            inner join SewingOutput so with(nolock) on so.SubconOutFty = s.SubConOutFty and so.SubConOutContractNumber = s.ContractNumber
            inner join SewingOutput_Detail sod with(nolock) on sod.ID = so.ID and sod.OrderId = sd.OrderId and sod.ComboType = sd.ComboType and sod.Article = sd.Article
            where 1 = 1
            {strWhere}
            group by s.SubConOutFty,s.ContractNumber,sd.OrderId,sd.ComboType,sd.Article,sd.UnitPrice

            select 
            t.*,
            [Qty] = isnull(t.[AccuSewingQty],0)-isnull(APQty.PaidQty,0),
            Amount = isnull(t.[Price],0) * (isnull(t.[AccuSewingQty],0)-isnull(APQty.PaidQty,0)),
            [AccuPaidQty] = isnull(APQty.PaidQty,0),
            BalQty = cast(isnull(t.[AccuSewingQty],0) as int) -cast( isnull(APQty.PaidQty,0) as int)
            from #tmp t
            outer apply (	select sum(Qty) as PaidQty
				            from SubconOutContractAP_Detail sd
				            inner join SubconOutContractAP s on sd.ID = s.ID
				            where sd.ContractNumber = t.[ContractNumber]
				            and sd.OrderId = t.[OrderID]
				            and sd.ComboType = t.ComboType
				            and sd.Article = t.Article
				            and s.LocalSuppID = t.SubConOutFty
				            and s.ID <> '{MyUtility.Convert.GetString(this.dr_Default_AP["ID"])}'
				            and s.Status <> 'New' ) APQty
            where 1 = 1
            {sqlcmd_F}
            order by OrderID
            drop TABLE #tmp
            ";

            DualResult result;
            if (result = DBProxy.Current.Select(null, sqlcmd, sqlParameters, out this.dtlocal))
            {
                if (this.dtlocal.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!!");
                }

                this.listControlBindingSource1.DataSource = this.dtlocal;
                this.gridImport.AutoResizeColumns();
                this.OnlyShowBalQty();
            }
            else
            {
                this.ShowErr(sqlcmd, result);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            DataTable dtImport = (DataTable)this.listControlBindingSource1.DataSource;

            if (MyUtility.Check.Empty(dtImport) || dtImport.Rows.Count == 0)
            {
                return;
            }

            List<DataRow> list = dtImport.Select("Qty > BalQty and Selected = 1").ToList();
            if (list.Count > 0)
            {
                string strMessage = $@"Selected row can't Qty > Bal.Qty!";

                var errorTable = list.CopyToDataTable();
                errorTable.Columns.Remove("Selected");

                MessageYESNO win = new MessageYESNO(strMessage, errorTable, "Warning", true);
                win.ShowDialog(this);
                return;
            }

            List<DataRow> list_Import = dtImport.Select("Selected = 1").ToList();
            if (list_Import.Count > 0)
            {
                var importTable = list_Import.CopyToDataTable();
                foreach (DataRow row in importTable.Rows)
                {
                    var newRow = this.dt_Default_AP_Detail.NewRow();
                    this.dt_Default_AP_Detail.Rows.Add(
                            row["ContractNumber"],
                            row["OrderID"],
                            row["ComboType"],
                            row["Article"],
                            row["Qty"],
                            row["Price"],
                            row["Amount"],
                            row["AccuSewingQty"],
                            row["AccuPaidQty"],
                            row["BalQty"],
                            row["UKEY"],
                            row["ID"]);
                }
            }
            else
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }

            this.Close();
        }

        private void ChBalQty_Click(object sender, EventArgs e)
        {
            this.OnlyShowBalQty();
        }

        /// <inheritdoc/>
        public void OnlyShowBalQty()
        {
            if (this.chBalQty.Checked)
            {
                var dt = this.dtlocal.AsEnumerable()
                    .Where(x => x.Field<int>("BalQty") > 0)
                    .OrderBy(x => x.Field<string>("OrderId")).TryCopyToDataTable(this.dtlocal);
                this.listControlBindingSource1.DataSource = dt;
            }
            else
            {
                this.listControlBindingSource1.DataSource = this.dtlocal;
            }
        }
    }
}
