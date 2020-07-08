using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Sci.Data;
using Ict;
using System.Data.SqlClient;

namespace Sci.Production.Warehouse
{
    public partial class P52_Import : Win.Subs.Base
    {
        private object stockType;
        private DataTable dtResultImportData;
        private bool boolImport = false;

        public P52_Import(object stockType)
        {
            this.stockType = stockType;
            this.InitializeComponent();
            this.grid.IsEditingReadOnly = false;
            DualResult result;
            #region Set ComboBox
            DataTable dtComboBoxCategory;
            string strComboBoxCategory = @"
select ID = 'All'

union all
select ID
from ArtworkType
where Classify = 'p'
";
            result = DBProxy.Current.Select(null, strComboBoxCategory, out dtComboBoxCategory);
            if (result)
            {
                if (dtComboBoxCategory != null && dtComboBoxCategory.Rows.Count > 0)
                {
                    this.comboBoxCategory.DataSource = dtComboBoxCategory;
                    this.comboBoxCategory.ValueMember = "ID";
                    this.comboBoxCategory.DisplayMember = "ID";
                }
            }
            else
            {
                MyUtility.Msg.WarningBox(result.Description, "Set ComboBox");
            }
            #endregion
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            #region Set Grid Columns
            this.Helper.Controls.Grid.Generator(this.grid)
                .CheckBox("sel", header: string.Empty, trueValue: 1, falseValue: 0, iseditable: true)
                .Text("Poid", header: "SP#")
                .Text("Refno", header: "Ref#", iseditingreadonly: true)
                .Text("Color", header: "Color", iseditingreadonly: true)
                .Text("UnitID", header: "Unit", iseditingreadonly: true)
                .Text("Location", header: "Location", iseditingreadonly: true)
                .Text("QtyBefore", header: "Book Qty", iseditingreadonly: true);
            #endregion
            for (int i = 0; i < this.grid.Columns.Count; i++)
            {
                this.grid.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        }

        private void textBoxLocation_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string strLocation = string.Format(
                @"
select  [Location ID] = id
        , Description 
from    mtllocation WITH (NOLOCK) 
where   junk != '1'
        and StockType = '{0}'", this.stockType);
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(strLocation, "10,20", this.textBoxLocation.Text);
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }
            else
            {
                this.textBoxLocation.Text = item.GetSelectedString();
            }
        }

        private void buttonFindNow_Click(object sender, EventArgs e)
        {
            List<SqlParameter> listSqlParameter = new List<SqlParameter>();
            #region SqlParameter
            listSqlParameter.Add(new SqlParameter("@StockType", this.stockType));
            listSqlParameter.Add(new SqlParameter("@Category", this.comboBoxCategory.Text));
            listSqlParameter.Add(new SqlParameter("@StartUnitPrice", this.textBoxUnitPriceStart.Text));
            listSqlParameter.Add(new SqlParameter("@EndUnitPrice", this.textBoxUnitPriceEnd.Text));
            listSqlParameter.Add(new SqlParameter("@StartSPNum", this.textBoxSPNumStart.Text));
            listSqlParameter.Add(new SqlParameter("@EndSPNum", this.textBoxSPNumEnd.Text));
            listSqlParameter.Add(new SqlParameter("@Location", this.textBoxLocation.Text));
            listSqlParameter.Add(new SqlParameter("@Count", this.textBoxCountOfRandom.Text));
            #endregion
            #region SQL Filte
            string strLocationFilte = string.Empty, strUnitPriceFilte = string.Empty, strOrderIDFilte = string.Empty;
            #region LocationFilte
            if (!this.textBoxLocation.Text.Empty())
            {
                strLocationFilte = @"
            and (select value = count(1) 
				    from SplitString (Location.value, ',')
				    where Data = @Location
				) > 0";
            }
            #endregion
            #region UnitPriceFilte
            if (!this.textBoxUnitPriceStart.Text.Empty() && !this.textBoxUnitPriceEnd.Text.Empty())
            {
                strUnitPriceFilte = "and UnitPrice.value between @StartUnitPrice and @EndUnitPrice";
            }
            else if (!this.textBoxUnitPriceStart.Text.Empty() && this.textBoxUnitPriceEnd.Text.Empty())
            {
                strUnitPriceFilte = "and @StartUnitPrice <= UnitPrice.value";
            }
            else if (this.textBoxUnitPriceStart.Text.Empty() && !this.textBoxUnitPriceEnd.Text.Empty())
            {
                strUnitPriceFilte = "and UnitPrice.value <= @EndUnitPrice";
            }
            #endregion
            #region OrderIDFilte
            if (!this.textBoxSPNumStart.Text.Empty() && !this.textBoxSPNumEnd.Text.Empty())
            {
                strOrderIDFilte = "and LInv.OrderID between @StartSPNum and @EndSPNum";
            }
            else if (!this.textBoxSPNumStart.Text.Empty() && this.textBoxSPNumEnd.Text.Empty())
            {
                strOrderIDFilte = "and LInv.OrderID like Concat (@StartSPNum, '%')";
            }
            else if (this.textBoxSPNumStart.Text.Empty() && !this.textBoxSPNumEnd.Text.Empty())
            {
                strOrderIDFilte = "and LInv.OrderID like Concat (@EndSPNum, '%')";
            }
            #endregion

            Dictionary<string, string> dicSQLFilte = new Dictionary<string, string>();
            dicSQLFilte.Add("TopCount", this.textBoxCountOfRandom.Text.Empty() ? string.Empty : "top (Convert (int, @Count))");
            dicSQLFilte.Add("Category", this.comboBoxCategory.Text.EqualString("All") ? string.Empty : "and LItem.Category = @Category");
            dicSQLFilte.Add("Location", strLocationFilte);
            dicSQLFilte.Add("UnitPrice", strUnitPriceFilte);
            dicSQLFilte.Add("OrderID", strOrderIDFilte);
            dicSQLFilte.Add("Random", this.textBoxCountOfRandom.Text.Empty() ? string.Empty : "order by NEWID() DESC");
            #endregion
            #region SQL Command
            string strSQLCommand = string.Format(
                @"
select *
from (
	-- Top Count
	select	{0} sel = 1
			, Poid = LInv.OrderID
			, Refno = LInv.Refno
			, Color = LInv.ThreadColorID
			, UnitID = LInv.UnitId
			, Location = Location.value
			, QtyBefore = BookQty.value
            , Variance = 0 - BookQty.value
	from LocalInventory LInv
    inner join Orders o on Linv.OrderID = o.ID
	left join LocalItem LItem on LInv.Refno = LItem.RefNo
	outer apply (
		select value = case @StockType
							when 'B' then LInv.ALocation
							when 'O' then LInv.CLocation
					   end
	) Location 
	outer apply (
		select value = case @StockType
							when 'B' then isnull (LInv.InQty, 0) - isnull (LInv.OutQty, 0) + isnull (LInv.AdjustQty, 0)
							when 'O' then LInv.LobQty
					   end
	) BookQty
	outer apply (
		select top 1 value = isnull (LItem.Price, 0) * rate
		from dbo.GetCurrencyRate (0, LItem.CurrencyID, 'USD', GETDATE())
	) UnitPrice
	where	BookQty.value > 0
            and o.MDivisionID = '{1}'
			-- Category
			{2}
			-- Location
			{3}
			-- UnitPrice
			{4}
			-- OrderID
			{5}
    -- Random
	{6}
) Datas
order by Datas.Poid, Datas.Refno",
                dicSQLFilte["TopCount"],
                Sci.Env.User.Keyword,
                dicSQLFilte["Category"],
                dicSQLFilte["Location"],
                dicSQLFilte["UnitPrice"],
                dicSQLFilte["OrderID"],
                dicSQLFilte["Random"]);
            #endregion
            this.ShowWaitMessage("Data Processing...");
            #region SQL Process
            DataTable dtResult;
            DualResult result = DBProxy.Current.Select(null, strSQLCommand, listSqlParameter, out dtResult);
            if (result)
            {
                if (dtResult != null && dtResult.Rows.Count == 0)
                {
                    MyUtility.Msg.InfoBox("Data not found.");
                }

                this.grid.DataSource = dtResult;
            }
            else
            {
                MyUtility.Msg.WarningBox(result.Description, "SQL Process");
            }
            #endregion
            this.HideWaitMessage();
        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            if (this.grid.Rows.Count > 0 && ((DataTable)this.grid.DataSource).AsEnumerable().Any(row => row["sel"].EqualDecimal(1)))
            {
                DataTable dtCheckData = ((DataTable)this.grid.DataSource).AsEnumerable().Where(row => row["sel"].EqualDecimal(1)).CopyToDataTable();
                this.dtResultImportData = dtCheckData;
                this.boolImport = true;
            }

            this.Close();
        }

        public bool getBoolImport()
        {
            return this.boolImport;
        }

        public DataTable getResultImportDatas()
        {
            return this.dtResultImportData;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
