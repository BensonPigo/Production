using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace Sci.Production.Sewing
{
    public partial class P11_Import : Win.Tems.QueryForm
    {
        private DataTable DetailDatas;

        public P11_Import(DataTable dt)
        {
            this.InitializeComponent();
            this.DetailDatas = dt;
            this.EditMode = true;
        }

        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk = new Ict.Win.UI.DataGridViewCheckBoxColumn();
        private DataGridViewGeneratorTextColumnSettings ToComboType = new DataGridViewGeneratorTextColumnSettings();
        private DataGridViewGeneratorTextColumnSettings ToArticle = new DataGridViewGeneratorTextColumnSettings();
        private DataGridViewGeneratorTextColumnSettings ToSizeCode = new DataGridViewGeneratorTextColumnSettings();

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.ColumnEvent();
            this.grid1.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid1)
            .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: true, falseValue: false).Get(out this.col_chk)
            .Text("FromOrderID", header: "From SP#", width: Widths.AnsiChars(14), iseditingreadonly: true)
            .Text("FromComboType", header: "*", width: Widths.AnsiChars(1), iseditingreadonly: true)
            .Text("Article", header: "Article", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Numeric("DisplayFromSewingQty", header: "From SP# Sewing\r\nOutput Qty", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Text("ToOrderID", header: "To SP#", width: Widths.AnsiChars(14), iseditingreadonly: true)
            .Text("ToComboType", header: "*", width: Widths.AnsiChars(1), settings: this.ToComboType)
            .Text("ToArticle", header: "Article", width: Widths.AnsiChars(8), settings: this.ToArticle)
            .Text("ToSizeCode", header: "Size", width: Widths.AnsiChars(8), settings: this.ToSizeCode)
            .Numeric("TransferQty", header: "Transfer Qty", width: Widths.AnsiChars(5))
            ;
        }

        private void ColumnEvent()
        {
            this.ToComboType.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                    List<SqlParameter> lis = new List<SqlParameter>();
                    lis.Add(new SqlParameter("@ToOrderID", dr["ToOrderID"]));
                    string sqlcmd = "select Location from Order_Location with(nolock) where OrderID = @ToOrderID";
                    if (!MyUtility.Check.Seek("select 1 from Order_Location with(nolock) where OrderID = @ToOrderID", lis))
                    {
                        sqlcmd = @"select Location from Style_Location sl with(nolock) inner join Orders o with(nolock) on sl.StyleUkey = o.StyleUKey 
where o.ID = @ToOrderID";
                    }

                    SelectItem item = new SelectItem(sqlcmd, lis, "8", MyUtility.Convert.GetString(dr["ToComboType"]));
                    DialogResult dialogResult = item.ShowDialog();
                    if (dialogResult == DialogResult.Cancel)
                    {
                        return;
                    }

                    dr["ToComboType"] = item.GetSelectedString();
                    dr.EndEdit();
                }
            };
            this.ToComboType.CellValidating += (s, e) =>
            {
                DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (!MyUtility.Check.Empty(e.FormattedValue))
                {
                    if (e.FormattedValue.Equals(dr["ToComboType"]))
                    {
                        return;
                    }

                    List<SqlParameter> lis = new List<SqlParameter>();
                    lis.Add(new SqlParameter("@Location", e.FormattedValue));
                    lis.Add(new SqlParameter("@ToOrderID", dr["ToOrderID"]));
                    string sqlcmd = $@"
if exists(select 1 from Order_Location with(nolock) where OrderID = @ToOrderID and Location = @Location)
begin
	select Location from Order_Location with(nolock) where OrderID = @ToOrderID and Location = @Location 
end
else 
begin
	select Location from Style_Location sl with(nolock) inner join Orders o with(nolock) on sl.StyleUkey = o.StyleUKey 
    where ID = @ToOrderID and Location = @Location
end
";
                    try
                    {
                        if (!MyUtility.Check.Seek(sqlcmd, lis, null))
                        {
                            MyUtility.Msg.WarningBox($"{e.FormattedValue} not fround!");
                            dr["ToComboType"] = string.Empty;
                            dr.EndEdit();
                            e.Cancel = true;
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        this.ShowErr(ex);
                    }
                }

                dr["ToComboType"] = e.FormattedValue;
                dr.EndEdit();
            };

            this.ToArticle.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                    List<SqlParameter> lis = new List<SqlParameter>();
                    lis.Add(new SqlParameter("@ToOrderID", dr["ToOrderID"]));
                    string sqlcmd = $@"select distinct Article from Order_Qty with(nolock) where ID = @ToOrderID";
                    SelectItem item = new SelectItem(sqlcmd, lis, "15", MyUtility.Convert.GetString(dr["ToArticle"]));
                    DialogResult dialogResult = item.ShowDialog();
                    if (dialogResult == DialogResult.Cancel)
                    {
                        return;
                    }

                    dr["ToArticle"] = item.GetSelectedString();
                    dr.EndEdit();
                }
            };
            this.ToArticle.CellValidating += (s, e) =>
            {
                DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (!MyUtility.Check.Empty(e.FormattedValue))
                {
                    if (e.FormattedValue.Equals(dr["ToArticle"]))
                    {
                        return;
                    }

                    List<SqlParameter> lis = new List<SqlParameter>();
                    lis.Add(new SqlParameter("@ToOrderID", dr["ToOrderID"]));
                    lis.Add(new SqlParameter("@ToArticle", e.FormattedValue));
                    string sqlcmd = $@"select 1 from Order_Qty with(nolock) where ID = @ToOrderID and Article = @ToArticle";
                    try
                    {
                        if (!MyUtility.Check.Seek(sqlcmd, lis, null))
                        {
                            MyUtility.Msg.WarningBox($"Article: {e.FormattedValue} not fround!");
                            dr["ToArticle"] = string.Empty;
                            dr.EndEdit();
                            e.Cancel = true;
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        this.ShowErr(ex);
                    }
                }

                dr["ToArticle"] = e.FormattedValue;
                dr.EndEdit();
            };

            this.ToSizeCode.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                    List<SqlParameter> lis = new List<SqlParameter>();
                    lis.Add(new SqlParameter("@ToOrderID", dr["ToOrderID"]));
                    string sqlcmd = $@"select distinct SizeCode from Order_Qty with(nolock) where ID = @ToOrderID  union select distinct sizecode from sewingoutput_Detail_detail where orderid=@ToOrderID";
                    SelectItem item = new SelectItem(sqlcmd, lis, "15", MyUtility.Convert.GetString(dr["ToSizeCode"]));
                    DialogResult dialogResult = item.ShowDialog();
                    if (dialogResult == DialogResult.Cancel)
                    {
                        return;
                    }

                    dr["ToSizeCode"] = item.GetSelectedString();
                    dr.EndEdit();
                }
            };
            this.ToSizeCode.CellValidating += (s, e) =>
            {
                DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (!MyUtility.Check.Empty(e.FormattedValue))
                {
                    if (e.FormattedValue.Equals(dr["ToSizeCode"]))
                    {
                        return;
                    }

                    List<SqlParameter> lis = new List<SqlParameter>();
                    lis.Add(new SqlParameter("@ToSizeCode", e.FormattedValue));
                    lis.Add(new SqlParameter("@ToOrderID", dr["ToOrderID"]));
                    string sqlcmd = $@"select 1 from Order_Qty with(nolock) where ID = @ToOrderID and SizeCode = @ToSizeCode union select 1 from sewingoutput_Detail_detail where orderid=@ToOrderID AND SizeCode = @ToSizeCode";
                    try
                    {
                        if (!MyUtility.Check.Seek(sqlcmd, lis, null))
                        {
                            MyUtility.Msg.WarningBox($"SizeCode: {e.FormattedValue} not fround!");
                            dr["ToSizeCode"] = string.Empty;
                            dr.EndEdit();
                            e.Cancel = true;
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        this.ShowErr(ex);
                    }
                }

                dr["ToSizeCode"] = e.FormattedValue;
                dr.EndEdit();
            };
        }

        private void TxtFromSP_Validating(object sender, CancelEventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtFromSP.Text))
            {
                return;
            }

            List<SqlParameter> lis = new List<SqlParameter>();
            lis.Add(new SqlParameter("@sp", this.txtFromSP.Text));
            lis.Add(new SqlParameter("@FactoryID", Env.User.Factory));
            string sqlcmd = $@"
select 1 
from SewingOutput_Detail sd with(nolock)
inner join SewingOutput s with(nolock) on sd.ID = s.ID
where sd.OrderId = @sp
and s.FactoryID = @FactoryID
and sd.AutoCreate = 0 --排除G單
";
            try
            {
                if (!MyUtility.Check.Seek(sqlcmd, lis, null))
                {
                    MyUtility.Msg.WarningBox($"Datas not found!");
                    this.txtFromSP.Text = string.Empty;
                    e.Cancel = true;
                    return;
                }

            }
            catch (Exception ex)
            {
                this.ShowErr(ex);
            }
        }

        private void TxtToSP_Validating(object sender, CancelEventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtToSP.Text))
            {
                return;
            }

            List<SqlParameter> lis = new List<SqlParameter>();
            lis.Add(new SqlParameter("@sp", this.txtToSP.Text));
            lis.Add(new SqlParameter("@FtyGroup", Env.User.Factory));
            string sqlcmd = $@"
select 1
from Orders o with(nolock) 
inner join Factory f with(nolock) on o.FactoryID = f.ID 
where o.ID = @sp
and o.FtyGroup = @FtyGroup
and f.IsProduceFty = 1
and o.Category in ('B','S')
and not exists (select 1 from Orders exludeOrder with (nolock) 
                            where (exludeOrder.junk = 1 and exludeOrder.NeedProduction = 0) and
                                  exludeOrder.ID = o.ID
                        )
";
            try
            {
                if (!MyUtility.Check.Seek(sqlcmd, lis, null))
                {
                    MyUtility.Msg.WarningBox($"Datas not found!");
                    this.txtToSP.Text = string.Empty;
                    e.Cancel = true;
                    return;
                }
            }
            catch (Exception ex)
            {
                this.ShowErr(ex);
            }
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtFromSP.Text))
            {
                MyUtility.Msg.WarningBox("From SP# can not empty");
                this.txtFromSP.Focus();
                return;
            }

            if (MyUtility.Check.Empty(this.txtToSP.Text))
            {
                MyUtility.Msg.WarningBox("To SP# can not empty");
                this.txtToSP.Focus();
                return;
            }

            List<SqlParameter> lis = new List<SqlParameter>();
            lis.Add(new SqlParameter("@sp", this.txtFromSP.Text));
            string sqlcmd = $@"
Select Distinct Selected = 0,
    FromOrderID=sdd.OrderID,
    FromComboType=sdd.ComboType,
    sdd.Article,
    sdd.SizeCode,
    [DisplayFromSewingQty] = Sum(sdd.QAQty),
    ToOrderID = '{this.txtToSP.Text}', ToComboType = '', TransferQty = 0,
    ToArticle=sdd.Article,
    ToSizeCode=sdd.SizeCode
From SewingOutput_Detail_Detail sdd with(nolock)
Where OrderID = @sp
And QAQty > 0
group by sdd.OrderID, sdd.ComboType, sdd.Article, sdd.SizeCode
";
            DataTable dt;
            DualResult result = DBProxy.Current.Select(null, sqlcmd, lis, out dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            this.listControlBindingSource1.DataSource = dt;

            if (dt.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Datas not found!");
            }
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            if (this.listControlBindingSource1.DataSource == null || ((DataTable)this.listControlBindingSource1.DataSource).Rows.Count == 0)
            {
                return;
            }

            if (((DataTable)this.listControlBindingSource1.DataSource).Select("selected = 1").Count() == 0)
            {
                MyUtility.Msg.WarningBox("Please select the data first!");
                return;
            }

            this.grid1.ValidateControl();
            DataTable dt = ((DataTable)this.listControlBindingSource1.DataSource).Select("selected = 1").CopyToDataTable();
            foreach (DataRow item in dt.Rows)
            {
                if (MyUtility.Check.Empty(item["ToComboType"]) ||
                    MyUtility.Check.Empty(item["ToArticle"]) ||
                    MyUtility.Check.Empty(item["ToSizeCode"]) ||
                    MyUtility.Check.Empty(item["TransferQty"]))
                {
                    MyUtility.Msg.WarningBox("* , Article, Sizecode, Transfer Qty can not empty!");
                    return;
                }
            }

            foreach (DataRow item in dt.Rows)
            {
                DataRow[] dataRows = this.DetailDatas.Select($"FromOrderID = '{item["FromOrderID"]}' and FromComboType = '{item["FromComboType"]}' and Article = '{item["Article"]}' and SizeCode ='{item["SizeCode"]}' and ToComboType = '{item["ToComboType"]}' and ToOrderID = '{item["ToOrderID"]}' and ToArticle = '{item["ToArticle"]}' and ToSizeCode = '{item["ToSizeCode"]}'  ");
                foreach (DataRow row in dataRows)
                {
                    row["TransferQty"] = item["TransferQty"];
                }

                if (dataRows.Count() == 0)
                {
                    item.SetAdded();
                    this.DetailDatas.ImportRow(item);
                }
            }

            MyUtility.Msg.InfoBox("Complete");
            this.listControlBindingSource1.DataSource = null;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
