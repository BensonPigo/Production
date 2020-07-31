using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;
using System.Linq;
using System.Transactions;
using System.Data.SqlClient;
using Sci.Win.Tools;

namespace Sci.Production.Sewing
{
    public partial class P11 : Win.Tems.Input6
    {
        // = P01手動拆表身數量操作(多天OutputDate)
        // 要被拆的表身(From)   拆出去(To)
        // Form不能AutoCreate   To只能Category (B,S)
        public P11(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = $"FactoryID = '{Env.User.Factory}'";
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : MyUtility.Convert.GetString(e.Master["ID"]);
            this.DetailSelectCommand = $@"
select sotd.*,
	DisplayFromQty=case when sot.status = 'New' 
		then
			isnull((
				select Qty
				from Order_Qty with(nolock)
				where ID = sotd.FromOrderID
				and Article = sotd.Article
				and SizeCode = sotd.SizeCode
			),0)
		else 
			sotd.FromQty
		end
		,
	DisplayFromSewingQty=case when sot.status = 'New' 
		then
			isnull((
				select sum(QAQty)
				from SewingOutput_Detail_Detail with(nolock)
				where OrderID = sotd.FromOrderID
				and ComboType = sotd.FromComboType
				and Article = sotd.Article
				and SizeCode = sotd.SizeCode
			),0)
			-
			isnull((
				select sum(QAQty)
				from SewingOutput_Detail_Detail_Garment with(nolock)
				where OrderIDFrom = sotd.FromOrderID
				and ComboType = sotd.FromComboType
				and Article = sotd.Article
				and SizeCode = sotd.SizeCode
			),0)
		else 
			sotd.FromSewingQty
		end,
    PackingQty=
        isnull((select SUM(pd.ShipQty)
        from PackingList_Detail pd with(nolock)
        inner join PackingList p with(nolock) on p.ID = pd.ID
        where p.Status = 'Confirmed'
        and pd.OrderID = sotd.FromOrderID
        and pd.Article = sotd.Article
        and pd.SizeCode = sotd.SizeCode),0),
	DisplayToQty=isnull(a.DisplayToQty,0),
	DisplayToSewingQty=isnull(b.DisplayToSewingQty,0),
	BalanceQty=isnull(b.DisplayToSewingQty,0)-isnull(a.DisplayToQty,0),
    OrderQtyUpperlimit=dbo.GetOrderQtyUpperlimit(sotd.ToOrderID,sotd.ToArticle,sotd.ToSizeCode),--實際上限可達數量
    styleUkey = (select styleUkey from orders o with(nolock) where o.id = sotd.ToOrderID) -- confrim 需要此欄位
from SewingOutputTransfer_Detail sotd with(nolock)
inner join SewingOutputTransfer sot with(nolock) on sotd.id = sot.id
outer apply(
	select DisplayToQty =case when sot.status = 'New' 
		then(
			select Qty
			from Order_Qty with(nolock)
			where ID = sotd.ToOrderID
			and Article = sotd.ToArticle
			and SizeCode = sotd.ToSizeCode)
		else 
			sotd.ToQty
		end
)a
outer apply(
	select DisplayToSewingQty=case when sot.status = 'New' 
		then(
			select sum(QAQty)
			from SewingOutput_Detail_Detail with(nolock)
			where OrderID = sotd.ToOrderID
			and ComboType = sotd.ToComboType
			and Article = sotd.ToArticle
			and SizeCode = sotd.ToSizeCode)
		else 
			sotd.ToSewingQty
		end
)b
where sotd.ID = '{masterID}'
";
            return base.OnDetailSelectCommandPrepare(e);
        }

        private DataGridViewGeneratorTextColumnSettings FromOrderID = new DataGridViewGeneratorTextColumnSettings();
        private DataGridViewGeneratorTextColumnSettings FromComboType = new DataGridViewGeneratorTextColumnSettings();
        private DataGridViewGeneratorTextColumnSettings Article = new DataGridViewGeneratorTextColumnSettings();
        private DataGridViewGeneratorTextColumnSettings SizeCode = new DataGridViewGeneratorTextColumnSettings();
        private DataGridViewGeneratorTextColumnSettings ToOrderID = new DataGridViewGeneratorTextColumnSettings();
        private DataGridViewGeneratorTextColumnSettings ToComboType = new DataGridViewGeneratorTextColumnSettings();
        private DataGridViewGeneratorTextColumnSettings ToArticle = new DataGridViewGeneratorTextColumnSettings();
        private DataGridViewGeneratorTextColumnSettings ToSizeCode = new DataGridViewGeneratorTextColumnSettings();

        protected override bool OnGridSetup()
        {
            this.ColumnEvent();
            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("FromOrderID", header: "From SP#", width: Widths.AnsiChars(13), settings: this.FromOrderID)
            .Text("FromComboType", header: "*", width: Widths.AnsiChars(1), settings: this.FromComboType)
            .Text("Article", header: "Article", width: Widths.AnsiChars(8), settings: this.Article)
            .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8), settings: this.SizeCode)
            .Numeric("DisplayFromQty", header: "From SP#\r\nQty", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Numeric("DisplayFromSewingQty", header: "From SP# Sewing\r\nOutput Qty", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Text("ToOrderID", header: "To SP#", width: Widths.AnsiChars(13), settings: this.ToOrderID)
            .Text("ToComboType", header: "*", width: Widths.AnsiChars(1), settings: this.ToComboType)
            .Text("ToArticle", header: "Article", width: Widths.AnsiChars(8), settings: this.ToArticle)
            .Text("ToSizeCode", header: "Size", width: Widths.AnsiChars(8), settings: this.ToSizeCode)
            .Numeric("DisplayToQty", header: "To SP#\r\nQty", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Numeric("DisplayToSewingQty", header: "To SP# Sewing\r\nOutput Qty", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Numeric("BalanceQty", header: "To SP#\r\nBalance Qty", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Numeric("TransferQty", header: "TransferQty", width: Widths.AnsiChars(5))
            ;
            return base.OnGridSetup();
        }

        private void ColumnEvent()
        {
            this.FromOrderID.CellValidating += (s, e) =>
            {
                if (!MyUtility.Check.Empty(e.FormattedValue))
                {
                    if (e.FormattedValue.Equals(this.CurrentDetailData["FromOrderID"]))
                    {
                        return;
                    }

                    List<SqlParameter> lis = new List<SqlParameter>();
                    lis.Add(new SqlParameter("@sp", e.FormattedValue));
                    lis.Add(new SqlParameter("@FactoryID", this.CurrentMaintain["FactoryID"]));
                    string sqlcmd = $@"
declare @orderid varchar(13) = @sp
declare @fty nvarchar(8) = @FactoryID
select 1 
from SewingOutput_Detail sd with(nolock)
inner join SewingOutput s with(nolock) on sd.ID = s.ID
where sd.OrderId = @orderid
and s.FactoryID = @fty
and sd.AutoCreate = 0 --排除G單
";
                    try
                    {
                        if (!MyUtility.Check.Seek(sqlcmd, lis, null))
                        {
                            MyUtility.Msg.WarningBox($"Datas not found!");
                            this.CurrentDetailData["FromOrderID"] = string.Empty;
                            this.CurrentDetailData.EndEdit();
                            e.Cancel = true;
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        this.ShowErr(ex);
                    }
                }

                this.CurrentDetailData["FromOrderID"] = e.FormattedValue;
                this.CurrentDetailData.EndEdit();
                this.GetDisplayFromQty();
                this.GetDisplayFromSewingQty();
            };

            this.FromComboType.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    List<SqlParameter> lis = new List<SqlParameter>();
                    lis.Add(new SqlParameter("@FromOrderID", this.CurrentDetailData["FromOrderID"]));
                    string sqlcmd = $@"select Location from Order_Location with(nolock) where OrderID = @FromOrderID";
                    SelectItem item = new SelectItem(sqlcmd, lis, "8", MyUtility.Convert.GetString(this.CurrentDetailData["FromComboType"]));
                    DialogResult dialogResult = item.ShowDialog();
                    if (dialogResult == DialogResult.Cancel)
                    {
                        return;
                    }

                    this.CurrentDetailData["FromComboType"] = item.GetSelectedString();
                    this.CurrentDetailData.EndEdit();
                    this.GetDisplayFromSewingQty();
                }
            };
            this.FromComboType.CellValidating += (s, e) =>
            {
                if (!MyUtility.Check.Empty(e.FormattedValue))
                {
                    if (e.FormattedValue.Equals(this.CurrentDetailData["FromComboType"]))
                    {
                        return;
                    }

                    List<SqlParameter> lis = new List<SqlParameter>();
                    lis.Add(new SqlParameter("@FromOrderID", this.CurrentDetailData["FromOrderID"]));
                    lis.Add(new SqlParameter("@Location", e.FormattedValue));
                    string sqlcmd = $@"select 1 from Order_Location with(nolock) where OrderID = @FromOrderID and Location = @Location";
                    try
                    {
                        if (!MyUtility.Check.Seek(sqlcmd, lis, null))
                        {
                            MyUtility.Msg.WarningBox($"{e.FormattedValue} not fround!");
                            this.CurrentDetailData["FromComboType"] = string.Empty;
                            this.CurrentDetailData.EndEdit();
                            e.Cancel = true;
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        this.ShowErr(ex);
                    }
                }

                this.CurrentDetailData["FromComboType"] = e.FormattedValue;
                this.CurrentDetailData.EndEdit();
                this.GetDisplayFromSewingQty();
            };

            this.Article.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    List<SqlParameter> lis = new List<SqlParameter>();
                    lis.Add(new SqlParameter("@FromOrderID", this.CurrentDetailData["FromOrderID"]));
                    string sqlcmd = $@"select distinct Article from Order_Qty with(nolock) where ID = @FromOrderID";
                    SelectItem item = new SelectItem(sqlcmd, lis, "15", MyUtility.Convert.GetString(this.CurrentDetailData["Article"]));
                    DialogResult dialogResult = item.ShowDialog();
                    if (dialogResult == DialogResult.Cancel)
                    {
                        return;
                    }

                    this.CurrentDetailData["Article"] = item.GetSelectedString();
                    this.CurrentDetailData.EndEdit();
                    this.SetDefaultToArticle();
                    this.GetDisplayFromQty();
                    this.GetDisplayFromSewingQty();
                }
            };
            this.Article.CellValidating += (s, e) =>
            {
                if (!MyUtility.Check.Empty(e.FormattedValue))
                {
                    if (e.FormattedValue.Equals(this.CurrentDetailData["Article"]))
                    {
                        return;
                    }

                    List<SqlParameter> lis = new List<SqlParameter>();
                    lis.Add(new SqlParameter("@FromOrderID", this.CurrentDetailData["FromOrderID"]));
                    lis.Add(new SqlParameter("@Article", e.FormattedValue));
                    string sqlcmd = $@"select 1 from Order_Qty with(nolock) where ID = @FromOrderID and Article = @Article";
                    try
                    {
                        if (!MyUtility.Check.Seek(sqlcmd, lis, null))
                        {
                            MyUtility.Msg.WarningBox($"Article: {e.FormattedValue} not fround!");
                            this.CurrentDetailData["Article"] = string.Empty;
                            this.CurrentDetailData.EndEdit();
                            e.Cancel = true;
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        this.ShowErr(ex);
                    }
                }

                this.CurrentDetailData["Article"] = e.FormattedValue;
                this.CurrentDetailData.EndEdit();
                this.SetDefaultToArticle();
                this.GetDisplayFromQty();
                this.GetDisplayFromSewingQty();
            };

            this.SizeCode.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    List<SqlParameter> lis = new List<SqlParameter>();
                    lis.Add(new SqlParameter("@FromOrderID", this.CurrentDetailData["FromOrderID"]));
                    string sqlcmd = $@"select distinct SizeCode from Order_Qty with(nolock) where ID = @FromOrderID union select distinct sizecode from sewingoutput_Detail_detail where orderid=@FromOrderID ";
                    SelectItem item = new SelectItem(sqlcmd, lis, "15", MyUtility.Convert.GetString(this.CurrentDetailData["SizeCode"]));
                    DialogResult dialogResult = item.ShowDialog();
                    if (dialogResult == DialogResult.Cancel)
                    {
                        return;
                    }

                    this.CurrentDetailData["SizeCode"] = item.GetSelectedString();
                    this.CurrentDetailData.EndEdit();
                    this.SetDefaultToSizeCode();
                    this.GetDisplayFromQty();
                    this.GetDisplayFromSewingQty();
                }
            };
            this.SizeCode.CellValidating += (s, e) =>
            {
                if (!MyUtility.Check.Empty(e.FormattedValue))
                {
                    if (e.FormattedValue.Equals(this.CurrentDetailData["SizeCode"]))
                    {
                        return;
                    }

                    List<SqlParameter> lis = new List<SqlParameter>();
                    lis.Add(new SqlParameter("@SizeCode", e.FormattedValue));
                    lis.Add(new SqlParameter("@FromOrderID", this.CurrentDetailData["FromOrderID"]));
                    string sqlcmd = $@"select 1 from Order_Qty with(nolock) where ID = @FromOrderID and SizeCode = @SizeCode union select 1 from sewingoutput_Detail_detail where orderid=@FromOrderID AND SizeCode = @SizeCode";
                    try
                    {
                        if (!MyUtility.Check.Seek(sqlcmd, lis, null))
                        {
                            MyUtility.Msg.WarningBox($"SizeCode: {e.FormattedValue} not fround!");
                            this.CurrentDetailData["SizeCode"] = string.Empty;
                            this.CurrentDetailData.EndEdit();
                            e.Cancel = true;
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        this.ShowErr(ex);
                    }
                }

                this.CurrentDetailData["SizeCode"] = e.FormattedValue;
                this.CurrentDetailData.EndEdit();
                this.SetDefaultToSizeCode();
                this.GetDisplayFromQty();
                this.GetDisplayFromSewingQty();
            };

            this.ToOrderID.CellValidating += (s, e) =>
            {
                if (!MyUtility.Check.Empty(e.FormattedValue))
                {
                    if (e.FormattedValue.Equals(this.CurrentDetailData["ToOrderID"]))
                    {
                        return;
                    }

                    List<SqlParameter> lis = new List<SqlParameter>();
                    lis.Add(new SqlParameter("@sp", e.FormattedValue));
                    lis.Add(new SqlParameter("@FtyGroup", this.CurrentMaintain["FactoryID"]));
                    string sqlcmd = $@"
select 1
from Orders o with(nolock)
inner join Factory f with(nolock) on o.FactoryID = f.ID 
where o. ID = @sp
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
                            this.CurrentDetailData["ToOrderID"] = string.Empty;
                            this.CurrentDetailData.EndEdit();
                            e.Cancel = true;
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        this.ShowErr(ex);
                    }
                }

                this.CurrentDetailData["ToOrderID"] = e.FormattedValue;
                this.CurrentDetailData.EndEdit();
                this.GetDisplayToQty();
                this.GetDisplayToSewingQty();
            };

            this.ToComboType.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    List<SqlParameter> lis = new List<SqlParameter>();
                    lis.Add(new SqlParameter("@ToOrderID", this.CurrentDetailData["ToOrderID"]));
                    string sqlcmd = "select Location from Order_Location with(nolock) where OrderID = @ToOrderID";
                    if (!MyUtility.Check.Seek("select 1 from Order_Location with(nolock) where OrderID = @ToOrderID", lis))
                    {
                        sqlcmd = @"select Location from Style_Location sl with(nolock) inner join Orders o with(nolock) on sl.StyleUkey = o.StyleUKey 
where o.ID = @ToOrderID";
                    }

                    SelectItem item = new SelectItem(sqlcmd, lis, "8", MyUtility.Convert.GetString(this.CurrentDetailData["ToComboType"]));
                    DialogResult dialogResult = item.ShowDialog();
                    if (dialogResult == DialogResult.Cancel)
                    {
                        return;
                    }

                    this.CurrentDetailData["ToComboType"] = item.GetSelectedString();
                    this.CurrentDetailData.EndEdit();
                    this.GetDisplayToSewingQty();
                }
            };
            this.ToComboType.CellValidating += (s, e) =>
            {
                if (!MyUtility.Check.Empty(e.FormattedValue))
                {
                    if (e.FormattedValue.Equals(this.CurrentDetailData["ToComboType"]))
                    {
                        return;
                    }

                    List<SqlParameter> lis = new List<SqlParameter>();
                    lis.Add(new SqlParameter("@Location", e.FormattedValue));
                    lis.Add(new SqlParameter("@ToOrderID", this.CurrentDetailData["ToOrderID"]));
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
                            this.CurrentDetailData["ToComboType"] = string.Empty;
                            this.CurrentDetailData.EndEdit();
                            e.Cancel = true;
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        this.ShowErr(ex);
                    }
                }

                this.CurrentDetailData["ToComboType"] = e.FormattedValue;
                this.CurrentDetailData.EndEdit();
                this.GetDisplayToSewingQty();
            };

            this.ToArticle.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    List<SqlParameter> lis = new List<SqlParameter>();
                    lis.Add(new SqlParameter("@ToOrderID", this.CurrentDetailData["ToOrderID"]));
                    string sqlcmd = $@"select distinct Article from Order_Qty with(nolock) where ID = @ToOrderID";
                    SelectItem item = new SelectItem(sqlcmd, lis, "15", MyUtility.Convert.GetString(this.CurrentDetailData["ToArticle"]));
                    DialogResult dialogResult = item.ShowDialog();
                    if (dialogResult == DialogResult.Cancel)
                    {
                        return;
                    }

                    this.CurrentDetailData["ToArticle"] = item.GetSelectedString();
                    this.CurrentDetailData.EndEdit();
                    this.GetDisplayToQty();
                    this.GetDisplayToSewingQty();
                }
            };
            this.ToArticle.CellValidating += (s, e) =>
            {
                if (!MyUtility.Check.Empty(e.FormattedValue))
                {
                    if (e.FormattedValue.Equals(this.CurrentDetailData["ToArticle"]))
                    {
                        return;
                    }

                    List<SqlParameter> lis = new List<SqlParameter>();
                    lis.Add(new SqlParameter("@ToOrderID", this.CurrentDetailData["ToOrderID"]));
                    lis.Add(new SqlParameter("@ToArticle", e.FormattedValue));
                    string sqlcmd = $@"select 1 from Order_Qty with(nolock) where ID = @ToOrderID and Article = @ToArticle";
                    try
                    {
                        if (!MyUtility.Check.Seek(sqlcmd, lis, null))
                        {
                            MyUtility.Msg.WarningBox($"Article: {e.FormattedValue} not fround!");
                            this.CurrentDetailData["ToArticle"] = string.Empty;
                            this.CurrentDetailData.EndEdit();
                            e.Cancel = true;
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        this.ShowErr(ex);
                    }
                }

                this.CurrentDetailData["ToArticle"] = e.FormattedValue;
                this.CurrentDetailData.EndEdit();
                this.GetDisplayToQty();
                this.GetDisplayToSewingQty();
            };

            this.ToSizeCode.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    List<SqlParameter> lis = new List<SqlParameter>();
                    lis.Add(new SqlParameter("@ToOrderID", this.CurrentDetailData["ToOrderID"]));
                    string sqlcmd = $@"select distinct SizeCode from Order_Qty with(nolock) where ID = @ToOrderID union select DISTINCT SizeCode from sewingoutput_Detail_detail where orderid=@ToOrderID ";
                    SelectItem item = new SelectItem(sqlcmd, lis, "15", MyUtility.Convert.GetString(this.CurrentDetailData["ToSizeCode"]));
                    DialogResult dialogResult = item.ShowDialog();
                    if (dialogResult == DialogResult.Cancel)
                    {
                        return;
                    }

                    this.CurrentDetailData["ToSizeCode"] = item.GetSelectedString();
                    this.CurrentDetailData.EndEdit();
                    this.GetDisplayToQty();
                    this.GetDisplayToSewingQty();
                }
            };
            this.ToSizeCode.CellValidating += (s, e) =>
            {
                if (!MyUtility.Check.Empty(e.FormattedValue))
                {
                    if (e.FormattedValue.Equals(this.CurrentDetailData["ToSizeCode"]))
                    {
                        return;
                    }

                    List<SqlParameter> lis = new List<SqlParameter>();
                    lis.Add(new SqlParameter("@ToSizeCode", e.FormattedValue));
                    lis.Add(new SqlParameter("@ToOrderID", this.CurrentDetailData["ToOrderID"]));
                    string sqlcmd = $@"select 1 from Order_Qty with(nolock) where ID = @ToOrderID and SizeCode = @ToSizeCode union select 1 from sewingoutput_Detail_detail where orderid=@ToOrderID AND SizeCode = @ToSizeCode";
                    try
                    {
                        if (!MyUtility.Check.Seek(sqlcmd, lis, null))
                        {
                            MyUtility.Msg.WarningBox($"SizeCode: {e.FormattedValue} not fround!");
                            this.CurrentDetailData["ToSizeCode"] = string.Empty;
                            this.CurrentDetailData.EndEdit();
                            e.Cancel = true;
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        this.ShowErr(ex);
                    }
                }

                this.CurrentDetailData["ToSizeCode"] = e.FormattedValue;
                this.CurrentDetailData.EndEdit();
                this.GetDisplayToQty();
                this.GetDisplayToSewingQty();
            };
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["CreateDate"] = DateTime.Today;
            this.CurrentMaintain["FactoryID"] = Env.User.Factory;
            this.CurrentMaintain["Status"] = "New";
        }

        protected override bool ClickEditBefore()
        {
            if (MyUtility.Convert.GetString(this.CurrentMaintain["Status"]).ToUpper() == "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("This record already confirmed, can't edit.");
                return false;
            }

            return base.ClickEditBefore();
        }

        protected override bool ClickDeleteBefore()
        {
            if (MyUtility.Convert.GetString(this.CurrentMaintain["Status"]) == "Confirmed")
            {
                MyUtility.Msg.WarningBox("This record already confirmed, can't delete.");
                return false;
            }

            return base.ClickDeleteBefore();
        }

        protected override bool ClickSaveBefore()
        {
            if (this.DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("The detail data can't be empty!");
                return false;
            }

            for (int i = 0; i < this.DetailDatas.Count; i++)
            {
                if (MyUtility.Check.Empty(this.DetailDatas[i]["FromOrderID"]))
                {
                    MyUtility.Msg.WarningBox("From SP# can't empty!");
                    this.detailgrid.CurrentCell = this.detailgrid.Rows[i].Cells["FromOrderID"]; // 移動到指定cell
                    this.detailgrid.BeginEdit(true);
                    return false;
                }

                if (MyUtility.Check.Empty(this.DetailDatas[i]["FromComboType"]))
                {
                    MyUtility.Msg.WarningBox("* can't empty!");
                    this.detailgrid.CurrentCell = this.detailgrid.Rows[i].Cells["FromComboType"]; // 移動到指定cell
                    this.detailgrid.BeginEdit(true);
                    return false;
                }

                if (MyUtility.Check.Empty(this.DetailDatas[i]["Article"]))
                {
                    MyUtility.Msg.WarningBox("Article can't empty!");
                    this.detailgrid.CurrentCell = this.detailgrid.Rows[i].Cells["Article"]; // 移動到指定cell
                    this.detailgrid.BeginEdit(true);
                    return false;
                }

                if (MyUtility.Check.Empty(this.DetailDatas[i]["SizeCode"]))
                {
                    MyUtility.Msg.WarningBox("Size can't empty!");
                    this.detailgrid.CurrentCell = this.detailgrid.Rows[i].Cells["SizeCode"]; // 移動到指定cell
                    this.detailgrid.BeginEdit(true);
                    return false;
                }

                if (MyUtility.Check.Empty(this.DetailDatas[i]["ToOrderID"]))
                {
                    MyUtility.Msg.WarningBox("To SP# can't empty!");
                    this.detailgrid.CurrentCell = this.detailgrid.Rows[i].Cells["ToOrderID"]; // 移動到指定cell
                    this.detailgrid.BeginEdit(true);
                    return false;
                }

                if (MyUtility.Check.Empty(this.DetailDatas[i]["ToComboType"]))
                {
                    MyUtility.Msg.WarningBox("* can't empty!");
                    this.detailgrid.CurrentCell = this.detailgrid.Rows[i].Cells["ToComboType"]; // 移動到指定cell
                    this.detailgrid.BeginEdit(true);
                    return false;
                }

                if (MyUtility.Check.Empty(this.DetailDatas[i]["ToArticle"]))
                {
                    MyUtility.Msg.WarningBox("Article can't empty!");
                    this.detailgrid.CurrentCell = this.detailgrid.Rows[i].Cells["ToArticle"]; // 移動到指定cell
                    this.detailgrid.BeginEdit(true);
                    return false;
                }

                if (MyUtility.Check.Empty(this.DetailDatas[i]["ToSizeCode"]))
                {
                    MyUtility.Msg.WarningBox("Size can't empty!");
                    this.detailgrid.CurrentCell = this.detailgrid.Rows[i].Cells["ToSizeCode"]; // 移動到指定cell
                    this.detailgrid.BeginEdit(true);
                    return false;
                }
            }

            for (int i = 0; i < this.DetailDatas.Count; i++)
            {
                if (MyUtility.Check.Empty(this.DetailDatas[i]["TransferQty"]) || MyUtility.Convert.GetInt(this.DetailDatas[i]["TransferQty"]) == 0)
                {
                    MyUtility.Msg.WarningBox("The transfer qty can't be 0!");
                    this.detailgrid.CurrentCell = this.detailgrid.Rows[i].Cells["TransferQty"]; // 移動到指定cell
                    this.detailgrid.BeginEdit(true);
                    return false;
                }
            }

            if (!this.CheckTransferQty() || !this.CheckOrderQtyUpperlimit())
            {
                return false;
            }

            for (int i = 0; i < this.DetailDatas.Count; i++)
            {
                if (MyUtility.Convert.GetString(this.DetailDatas[i]["FromOrderID"]) == MyUtility.Convert.GetString(this.DetailDatas[i]["ToOrderID"]))
                {
                    MyUtility.Msg.WarningBox("From SP# cannot be the same as To SP#");
                    this.detailgrid.CurrentCell = this.detailgrid.Rows[i].Cells["FromOrderID"]; // 移動到指定cell
                    this.detailgrid.BeginEdit(true);
                    return false;
                }
            }

            #region GetID
            if (this.IsDetailInserting)
            {
                string id = MyUtility.GetValue.GetID(MyUtility.GetValue.Lookup("FtyGroup", Env.User.Factory, "Factory", "ID") + "OT", "SewingOutputTransfer", DateTime.Today, 3, "Id", null);
                if (MyUtility.Check.Empty(id))
                {
                    MyUtility.Msg.WarningBox("GetID fail, please try again!");
                    return false;
                }

                this.CurrentMaintain["ID"] = id;
            }
            #endregion

            return base.ClickSaveBefore();
        }

        protected override void ClickConfirm()
        {
            this.OnRefreshClick();
            base.ClickConfirm();
            if (!this.CheckTransferQty() || !this.CheckOrderQtyUpperlimit())
            {
                return;
            }

            try
            {
                TransactionOptions tOpt = default(TransactionOptions);
                tOpt.Timeout = new TimeSpan(0, 5, 0);
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew, tOpt))
                {
                    if (!this.UpdateSewingOutputTransfer(scope))
                    {
                        return;
                    }

                    if (!this.TransferQty(scope))
                    {
                        return;
                    }

                    if (!this.UpdateMESInspection(scope))
                    {
                        return;
                    }

                    scope.Complete();
                }
            }
            catch (Exception e)
            {
                this.ShowErr(e);
                return;
            }

            MyUtility.Msg.InfoBox("Complete!");
        }

        private bool UpdateSewingOutputTransfer(TransactionScope scope)
        {
            string sqlcmd = $@"
update SewingOutputTransfer set
    Status ='Confirmed',
    EditDate =GetDate(),
    EditName = '{Env.User.UserID}'
where id = '{this.CurrentMaintain["ID"]}'
    
update sotd set
    sotd.FromQty = t.DisplayFromQty,
    sotd.FromSewingQty  = t.DisplayFromSewingQty,
    sotd.ToQty = t.DisplayToQty,
    sotd.ToSewingQty = t.DisplayToSewingQty
from SewingOutputTransfer_Detail sotd
inner join #tmp t on t.ukey = sotd.ukey and sotd.id = t.id
";
            DataTable dt;
            DualResult result = MyUtility.Tool.ProcessWithDatatable((DataTable)this.detailgridbs.DataSource, string.Empty, sqlcmd, out dt);
            if (!result)
            {
                scope.Dispose();
                this.ShowErr(result);
                return false;
            }

            return true;
        }

        private bool TransferQty(TransactionScope scope)
        {
            #region 準備分配資料與計算 轉走Qty, 轉至Qty
            string sqlcmd = $@"
-- 準備要尋找轉出來源<From> SP,ComboType,Artile,SizeCode組合，以及將要轉移總數
select t.FromOrderID,t.FromComboType,t.Article,t.SizeCode
into #tmpFromG
from #tmp t
group by t.FromOrderID,t.FromComboType,t.Article,t.SizeCode

--準備可轉移數資料, 依據<From> ID, SP,ComboType, Artile, SizeCode
SELECT so.OutputDate,sodd.*,
	CanTransQty = isnull(sodd.QAQty,0) - isnull(G.QAQty,0), -- 此筆Sdd剩下可轉移數
	TransferredQty = 0 -- 用來記錄轉走Qty
into #tmpByIDCanTransQty
FROM #tmpFromG t
INNER JOIN SewingOutput_Detail_Detail sodd with(nolock) on -- 找要轉出的資料(必定有)
	sodd.OrderId = t.FromOrderID and sodd.ComboType = t.FromComboType
	and sodd.Article = t.Article and sodd.SizeCode = t.SizeCode
inner join SewingOutput so with(nolock) on so.id = sodd.ID
outer apply(
	select QAQty=sum(soddg.QAQty) -- 找G單(虛數量) 有可能多筆
	from SewingOutput_Detail_Detail_Garment soddg with(nolock) 
	where soddg.ID = sodd.ID -- 同SewingOutput單下
	and soddg.OrderIDfrom = sodd.OrderId and soddg.ComboType = sodd.ComboType
	and soddg.Article = sodd.Article and soddg.SizeCode = sodd.SizeCode
)G
where  isnull(sodd.QAQty,0) - isnull(G.QAQty,0) > 0

--By From & To 所有欄組合 加總需轉移數 (表身可輸入同樣組合多筆)
select t.FromOrderID,t.FromComboType,t.Article,t.SizeCode,t.ToOrderID,t.ToComboType,t.ToArticle,t.ToSizeCode,TransferQty=sum(t.TransferQty)
into #tmpFromToG
from #tmp t
group by t.FromOrderID,t.FromComboType,t.Article,t.SizeCode,t.ToOrderID,t.ToComboType,t.ToArticle,t.ToSizeCode

-- 可轉移數資料, 再依據<To 4欄位> 資料展開-- WillTransferQty最後要用來更新sdd, 或是新增sdd,sd用
select t.OutputDate,t.ID,t.OrderId,t.ComboType,t.Article,t.SizeCode,t.SewingOutput_DetailUKey,
	t2.ToOrderID,t2.ToComboType,t2.ToArticle,t2.ToSizeCode,
	WillTransferQty = 0 -- 用來紀錄轉進Qty
into #tmpUp
from #tmpByIDCanTransQty t
inner join #tmpFromToG t2 on t2.FromOrderID = t.OrderId and t2.FromComboType = t.ComboType and t2.Article = t.Article and t2.SizeCode = t.SizeCode

select*from #tmpByIDCanTransQty t order by t.OutputDate,t.ID,t.OrderId,t.ComboType,t.Article,t.SizeCode
select*from #tmpFromToG t  order by t.FromOrderID,t.FromComboType,t.Article,t.SizeCode,t.ToOrderID,t.ToComboType,t.ToArticle,t.ToSizeCode
select*from #tmpUp t order by t.OutputDate,t.ID,t.OrderId,t.ComboType,t.Article,t.SizeCode,t.ToOrderID,t.ToComboType,t.ToArticle,t.ToSizeCode

--檢查OrderId在Order_Location是否有資料，沒資料就補 此處只檢查轉To SP，From SP是P01已有資料，P01存檔時檢查 ※Sewing_P01
DECLARE CUR_SewingOutput_Detail CURSOR FOR 
    Select distinct orderid = ToOrderID from #tmpUp t
declare @orderid varchar(13) 
OPEN CUR_SewingOutput_Detail   
FETCH NEXT FROM CUR_SewingOutput_Detail INTO @orderid 
WHILE @@FETCH_STATUS = 0 
BEGIN
    exec dbo.Ins_OrderLocation @orderid
FETCH NEXT FROM CUR_SewingOutput_Detail INTO @orderid
END
CLOSE CUR_SewingOutput_Detail
DEALLOCATE CUR_SewingOutput_Detail

drop table #tmp,#tmpFromG,#tmpByIDCanTransQty,#tmpFromToG,#tmpUp
";
            DataTable[] dt;
            DualResult result = MyUtility.Tool.ProcessWithDatatable((DataTable)this.detailgridbs.DataSource, string.Empty, sqlcmd, out dt);
            if (!result)
            {
                scope.Dispose();
                this.ShowErr(result);
                return false;
            }

            try
            {
                // 開始分配計算
                // dt[0]的TransferredQty是紀錄轉出數，要回到轉出來源將此數量扣除
                // dt[2]的WillTransferQty是紀錄轉入目標數量，若目標已有這筆組合，則加上這數量
                foreach (DataRow r2 in dt[2].Rows)
                {
                    DataRow[] dr0s = dt[0].Select($@"ID = '{r2["ID"]}' and OutputDate='{r2["OutputDate"]}' and OrderId='{r2["OrderId"]}' and ComboType='{r2["ComboType"]}' and Article = '{r2["Article"]}' and SizeCode='{r2["SizeCode"]}'"); // 必有, dt2是dt0為主展開
                    DataRow[] dr1s = dt[1].Select($@"FromOrderID='{r2["OrderId"]}' and FromComboType='{r2["ComboType"]}' and Article = '{r2["Article"]}' and SizeCode='{r2["SizeCode"]}' and ToOrderID='{r2["ToOrderID"]}' and ToComboType='{r2["ToComboType"]}' and ToArticle='{r2["ToArticle"]}' and ToSizeCode='{r2["ToSizeCode"]}'");

                    // 準備資料已Group過, 只會找到對應一筆, 或沒有
                    if (dr1s.Count() > 0)
                    {
                        if (MyUtility.Convert.GetInt(dr0s[0]["CanTransQty"]) >= MyUtility.Convert.GetInt(dr1s[0]["TransferQty"]))
                        {
                            r2["WillTransferQty"] = dr1s[0]["TransferQty"]; // 紀錄此筆將轉Qty. 增到sdd, 或<加>到已有sdd 的數量
                            dr0s[0]["CanTransQty"] = MyUtility.Convert.GetInt(dr0s[0]["CanTransQty"]) - MyUtility.Convert.GetInt(r2["WillTransferQty"]); // 還剩餘數
                            dr1s[0]["TransferQty"] = 0; // 紀錄還須轉移數
                        }
                        else
                        {
                            r2["WillTransferQty"] = dr0s[0]["CanTransQty"]; // 紀錄此筆將轉Qty. 增到sdd, 或<加>到已有sdd 的數量
                            dr1s[0]["TransferQty"] = MyUtility.Convert.GetInt(dr1s[0]["TransferQty"]) - MyUtility.Convert.GetInt(r2["WillTransferQty"]); // 還須轉移數
                            dr0s[0]["CanTransQty"] = 0; // 還剩餘數
                        }

                        dr0s[0]["TransferredQty"] = MyUtility.Convert.GetInt(dr0s[0]["TransferredQty"]) + MyUtility.Convert.GetInt(r2["WillTransferQty"]); // 紀錄轉走數
                        dr0s[0].EndEdit();
                        dr1s[0].EndEdit();
                        r2.EndEdit();
                    }
                }
            }
            catch (Exception e)
            {
                scope.Dispose();
                this.ShowErr(e);
                return false;
            }

            DataTable fromDt = dt[0];
            DataTable toDt = dt[2];
            DataTable dto;
            #endregion

            #region 更新/寫入SewingOutput第3層，第2層 在分配完數量後

            // 回找到Form SP 對應 sdd 更新 QAQty = QAQty - 轉走數。 PS:不是剩餘數(QAQty-G單數)
            sqlcmd = $@"
select t.ID, t.OrderId, t.ComboType, t.Article, t.SizeCode, TransferredQty = sum(t.TransferredQty)
into #tmpupdate
from #tmp t
group by t.ID, t.OrderId, t.ComboType, t.Article, t.SizeCode

update sodd set
	sodd.QAQty = sodd.QAQty - t.TransferredQty --減去轉走數
from #tmpupdate t
inner join SewingOutput_Detail_Detail sodd on sodd.ID = t.ID
	and sodd.OrderId = t.OrderId and sodd.ComboType = t.ComboType and sodd.Article = t.Article and sodd.SizeCode = t.SizeCode
where t.TransferredQty > 0

update sod set
    sod.AutoSplit=1
from #tmpupdate t
inner join SewingOutput_Detail_Detail sodd on sodd.ID = t.ID
	and sodd.OrderId = t.OrderId and sodd.ComboType = t.ComboType and sodd.Article = t.Article and sodd.SizeCode = t.SizeCode
inner join SewingOutput_Detail sod on sodd.SewingOutput_DetailUKey = sod.Ukey
where t.TransferredQty > 0 -- 找到有更新第3層對應第2層更新
";
            result = MyUtility.Tool.ProcessWithDatatable(fromDt, string.Empty, sqlcmd, out dto);
            if (!result)
            {
                scope.Dispose();
                this.ShowErr(result);
                return false;
            }

            // 找to SP 對應 sdd, 更新/新增
            sqlcmd = $@"
--更新已有第3層sdd
select t.ID, t.ToOrderID, t.ToComboType, t.ToArticle, t.ToSizeCode, WillTransferQty = sum(t.WillTransferQty)
into #tmpupdate
from #tmp t
group by t.ID, t.ToOrderID, t.ToComboType, t.ToArticle, t.ToSizeCode

update sodd set
	sodd.QAQty = sodd.QAQty + t.WillTransferQty -- 已有的數量加上轉移數
from #tmpupdate t
inner join SewingOutput_Detail_Detail sodd on sodd.ID = t.ID 
	and sodd.OrderId = t.ToOrderID and sodd.ComboType = t.ToComboType and sodd.Article = t.ToArticle and sodd.SizeCode = t.ToSizeCode
where t.WillTransferQty > 0

--寫入第2層sd有,但第3層sdd未有
insert into SewingOutput_Detail_Detail(ID, SewingOutput_DetailUKey, OrderId, ComboType, Article, SizeCode, QAQty, OldDetailKey)
select t.ID,
	SewingOutput_DetailUKey=sod.UKey,
	t.ToOrderID,
    t.ToComboType,
    t.ToArticle,
    t.ToSizeCode,
    WillTransferQty=sum(t.WillTransferQty),
	OldDetailKey=''
from #tmp t
inner join SewingOutput_Detail sod with(nolock) on sod.id = t.ID and sod.OrderId = t.ToOrderID and sod.ComboType = t.ToComboType and sod.Article = t.ToArticle
left join SewingOutput_Detail_Detail sodd on sodd.ID = t.ID 
	and sodd.OrderId = t.ToOrderID and sodd.ComboType = t.ToComboType and sodd.Article = t.ToArticle and sodd.SizeCode = t.ToSizeCode
where sodd.id is null
and t.WillTransferQty > 0
group by t.ID, sod.UKey, t.ToOrderID, t.ToComboType, t.ToArticle, t.ToSizeCode

--第2層也沒有的資料
--先寫第2層
Declare @SewingOutput_Detail table(
	[ID] [varchar](13) NOT NULL,
	[OrderId] [varchar](13) NOT NULL,
	[ComboType] [varchar](1) NULL,
	[Article] [varchar](8) NULL,
	[Color] [varchar](6) NULL,
	[TMS] [int] NULL,
	[HourlyStandardOutput] [int] NULL,
	[WorkHour] [numeric](6, 3) NOT NULL,
	[UKey] [bigint] NOT NULL,
	[QAQty] [int] NULL,
	[DefectQty] [int] NULL,
	[InlineQty] [int] NULL,
	[OldDetailKey] [varchar](13) NULL,
	[AutoCreate] [bit] NULL,
	[SewingReasonID] [varchar](5) NOT NULL,
	[Remark] [nvarchar](1000) NULL,
	[ImportFromDQS] [bit] NOT NULL,
    [AutoSplit] [bit] NULL
)
INSERT INTO [dbo].[SewingOutput_Detail]
([ID],[OrderId],[ComboType],[Article],[Color],[TMS],[HourlyStandardOutput],[WorkHour],[QAQty],[DefectQty],[InlineQty],[OldDetailKey],[AutoCreate],[SewingReasonID],[Remark],[ImportFromDQS],[AutoSplit])
OUTPUT INSERTED.* into @SewingOutput_Detail  -- 取得寫入的資料,ukey欄位
select t.ID,t.ToOrderID,t.ToComboType,t.ToArticle,
	Color = (select top 1 ColorID from View_OrderFAColor where Id = t.ToOrderID and Article = t.ToArticle),--※Sewing_P01
	TMS = isnull(Round(o.CPU * o.CPUFactor * (r.rate / 100) * (select StdTMS from System WITH (NOLOCK)), 0),0),--※Sewing_P01
	HourlyStandardOutput = isnull(h.StandardOutput,0),
	WorkHour = 0,-- 先給0下方再重算
	[QAQty] = sum(t.WillTransferQty),--第3層加總
	[DefectQty] = 0, -- P11不處理DefectQty
	[InlineQty] = 0, -- P11不處理InlineQty
	OldDetailKey = '',
	[AutoCreate] = 0,
	[SewingReasonID] = '',
	[Remark] = '',
	[ImportFromDQS] = 0,
    AutoSplit = 1
from #tmp t
inner join orders o WITH (NOLOCK) on o.id = t.ToOrderID
inner join SewingOutput so WITH (NOLOCK) on so.ID = t.ID
left join SewingOutput_Detail_Detail sodd on sodd.ID = t.ID 
	and sodd.OrderId = t.ToOrderID and sodd.ComboType = t.ToComboType and sodd.Article = t.ToArticle and sodd.SizeCode = t.ToSizeCode
outer apply(select Rate = isnull([dbo].GetOrderLocation_Rate(t.ToOrderID ,t.ComboType), ([dbo].GetStyleLocation_Rate(o.StyleUkey, t.ComboType))))r--※Sewing_P01
outer apply(
    select top 1 StandardOutput
    from SewingSchedule s WITH (NOLOCK)
    where s.OrderID = t.ToOrderID and s.ComboType = t.ToComboType and s.SewingLineID = so.SewingLineID
)h--※Sewing_P01
where sodd.id is null
and not exists(select 1 from SewingOutput_Detail sod with(nolock) where sod.id = t.ID and sod.OrderId = t.ToOrderID and sod.ComboType = t.ToComboType and sod.Article = t.ToArticle)
and t.WillTransferQty > 0
group by t.ID,t.ToOrderID,t.ToComboType,t.ToArticle,o.CPU,o.CPUFactor,r.Rate,h.StandardOutput

--再寫第3層
INSERT INTO [dbo].[SewingOutput_Detail_Detail]
([ID],[SewingOutput_DetailUKey],[OrderId],[ComboType],[Article],[SizeCode],[QAQty],[OldDetailKey])
select s.ID,s.UKey,s.OrderId,s.ComboType,s.Article,t.ToSizeCode,QAQty=sum(t.WillTransferQty),''
from #tmp t
inner join @SewingOutput_Detail s on s.ID = t.ID and s.OrderId = t.ToOrderID and s.ComboType = t.ToComboType and s.Article = t.ToArticle
where t.WillTransferQty > 0
group by s.ID,s.UKey,s.OrderId,s.ComboType,s.Article,t.ToSizeCode

--以上第3層都寫入後
update sod set
    sod.AutoSplit=1
from #tmpupdate t
inner join SewingOutput_Detail_Detail sodd with(nolock) on sodd.ID = t.ID 
	and sodd.OrderId = t.ToOrderID and sodd.ComboType = t.ToComboType and sodd.Article = t.ToArticle and sodd.SizeCode = t.ToSizeCode
inner join SewingOutput_Detail sod with(nolock) on sodd.SewingOutput_DetailUKey = sod.Ukey
where t.WillTransferQty > 0 -- 找到有更新/新增第3層, 對應第2層

--更新表頭
update so set
    so.EditName ='{Env.User.UserID}',
    so.EditDate = GetDate(),
    so.ReDailyTransferDate = GetDate()    
from SewingOutput so
where so.id in(select distinct id from #tmp t where t.WillTransferQty > 0 )

--刪除為0的第3層，等上面更新完再執行
delete SewingOutput_Detail_Detail where QAQty=0 and id in(select distinct id from #tmp t where t.WillTransferQty > 0) -- 只找此次有更新Qty的資料

--重算第2層 QAQty ※Sewing_P01
update SD set SD.QAQty = SDD.SDD_Qty
from  SewingOutput_Detail SD WITH (NOLOCK)
outer apply 
( 
    select isnull(SUM(SDD.QAQty),0) as SDD_Qty from SewingOutput_Detail_Detail SDD WITH (NOLOCK) where SDD.ID=SD.ID and SDD.SewingOutput_DetailUKey=SD.UKey 
) as SDD 
where SD.QAQty <> SDD.SDD_Qty and SD.ID in (select distinct t.id from #tmp t where t.WillTransferQty > 0) -- 只找此次有更新Qty的資料

--重算表頭TMS, Efficiency (在重算完 第2層 QAQty 之後) ※Sewing_P01
select id, tms = cast(TMS as numeric(24,10)) * QAQty / sum(QAQty) over(partition by id), QAQty
into #tmpdTms
from SewingOutput_Detail sod with(nolock)
where sod.AutoCreate <> 1
and sod.id in (select distinct id from #tmp t where t.WillTransferQty > 0) -- 只找此次有更新Qty的資料

select id,TMS = round(Sum(tms), 0), QAQty = sum(QAQty) into #tmp_upTms from #tmpdTms group by id
--不更新表頭QAQty，因為正確狀況下總數不變動
update so set
    TMS = t.TMS,
    Efficiency = iif(t.TMS * so.ManHour = 0, 0, cast(t.QAQty as numeric(24,10))/ (3600 / t.TMS * so.ManHour) * 100)
from #tmp_upTms t
inner join SewingOutput so with(nolock) on so.id = t.id

--重算第2層 WorkHour 撈需要重算資料 (在重算完 第2層 QAQty 之後) ※Sewing_P01
select ID,
    sumQaqty = sum(isnull(QAQty,0) * isnull(TMS,0)),
    RecCnt = count(1)
from SewingOutput_Detail sod with(nolock)
where sod.AutoCreate <> 1
and sod.id in (select distinct id from #tmp t where t.WillTransferQty > 0) -- 只找此次有更新Qty的資料
group by ID
";
            DataTable sumQaQty;
            result = MyUtility.Tool.ProcessWithDatatable(toDt, string.Empty, sqlcmd, out sumQaQty);
            if (!result)
            {
                scope.Dispose();
                this.ShowErr(result);
                return false;
            }
            #endregion

            #region 重算第2層 WorkHour (在重算完 第2層 QAQty 之後) ※Sewing_P01
            foreach (DataRow item in sumQaQty.Rows)
            {
                int recCnt = MyUtility.Convert.GetInt(item["RecCnt"]);
                decimal ttlQaqty = MyUtility.Convert.GetDecimal(item["sumQaqty"]);
                decimal subSum = 0;
                sqlcmd = $@"select * from SewingOutput sod with(nolock) where id = '{item["id"]}'";
                DataTable dtid;
                result = DBProxy.Current.Select(null, sqlcmd, out dtid);
                if (!result)
                {
                    scope.Dispose();
                    this.ShowErr(result);
                    return false;
                }

                decimal workHour = MyUtility.Convert.GetDecimal(dtid.Rows[0]["WorkHour"]); // 取得表頭 WorkHour 總數
                sqlcmd = $@"select * from SewingOutput_Detail sod with(nolock) where AutoCreate <> 1 and id = '{item["id"]}'";
                result = DBProxy.Current.Select(null, sqlcmd, out dtid);
                if (!result)
                {
                    scope.Dispose();
                    this.ShowErr(result);
                    return false;
                }

                foreach (DataRow dr in dtid.Rows)
                {
                    recCnt = recCnt - 1;
                    if (recCnt == 0)
                    {
                        dr["WorkHour"] = workHour - subSum;
                    }
                    else
                    {
                        dr["WorkHour"] = ttlQaqty == 0 ? 0 : MyUtility.Math.Round(MyUtility.Convert.GetDecimal(dr["QAQty"]) * MyUtility.Convert.GetDecimal(dr["TMS"]) / ttlQaqty * workHour, 3);
                    }

                    subSum = subSum + MyUtility.Convert.GetDecimal(dr["WorkHour"]);
                }

                // 重算好的WorkHour寫回 SewingOutput_Detail
                sqlcmd = $@"
update sod set
    WorkHour = t.WorkHour
from SewingOutput_Detail sod with(nolock)
inner join #tmp t on t.[UKey] = sod.[UKey]
";
                result = MyUtility.Tool.ProcessWithDatatable(dtid, string.Empty, sqlcmd, out dto);
                if (!result)
                {
                    scope.Dispose();
                    this.ShowErr(result);
                    return false;
                }
            }
            #endregion

            return true;
        }

        private bool CheckTransferQty()
        {
            // FromOrderID,FromComboType,Article,SizeCode組合判斷TransferQty(移轉數)不可大於FromSewingQty(來源數)
            var listCheckDataBase = this.DetailDatas
                                .Where(s => s.RowState != DataRowState.Deleted)
                                .GroupBy(s => new
                                {
                                    FromOrderID = s["FromOrderID"].ToString(),
                                    FromComboType = s["FromComboType"].ToString(),
                                    Article = s["Article"].ToString(),
                                    SizeCode = s["SizeCode"].ToString(),
                                    PackingQty = MyUtility.Convert.GetInt(s["PackingQty"]),
                                    DisplayFromSewingQty = MyUtility.Convert.GetInt(s["DisplayFromSewingQty"]),
                                })
                                .Select(g => new
                                {
                                    g.Key.FromOrderID,
                                    g.Key.FromComboType,
                                    g.Key.Article,
                                    g.Key.SizeCode,
                                    g.Key.PackingQty,
                                    g.Key.DisplayFromSewingQty,
                                    TransferQty = g.Sum(s => MyUtility.Convert.GetInt(s["TransferQty"])),
                                });

            var listSewingQtyOver = listCheckDataBase.Where(w => w.TransferQty > w.DisplayFromSewingQty);
            if (listSewingQtyOver.Any())
            {
                string errorMsg = "<Transfer Qty> can not more than <From SP# Sewing Output Qty>!" + Environment.NewLine +
                                  listSewingQtyOver.Select(s => $"From SP#: {s.FromOrderID}, {s.FromComboType}, Article:{s.Article}, Size:{s.SizeCode}").JoinToString(Environment.NewLine);
                MyUtility.Msg.WarningBox(errorMsg);
                return false;
            }

            var listSewingPackQtyOver = listCheckDataBase.Where(w => w.TransferQty > (w.DisplayFromSewingQty - w.PackingQty));
            if (listSewingPackQtyOver.Any())
            {
                string errorMsg = "<Transfer Qty> can not more than <From SP# Packing Qty>!" + Environment.NewLine +
                                  listSewingPackQtyOver.Select(s => $"From SP#: {s.FromOrderID}, {s.FromComboType}, Article:{s.Article}, Size:{s.SizeCode}, Packing Qty:{s.PackingQty.ToString()}").JoinToString(Environment.NewLine);
                MyUtility.Msg.WarningBox(errorMsg);
                return false;
            }

            return true;
        }

        private bool CheckOrderQtyUpperlimit()
        {
            // ToOrderID,ToComboType,ToArticle,ToSizeCode 組合判斷 TransferQty(移轉數)加DisplayToSewingQty(已報產出數) 不可大於 OrderQtyUpperlimit(實際上限)
            var list = this.DetailDatas.AsEnumerable().Where(s => s.RowState != DataRowState.Deleted);
            var list2 = list.GroupBy(s => new
            {
                ToOrderID = s["ToOrderID"].ToString(),
                ToComboType = s["ToComboType"].ToString(),
                ToArticle = s["ToArticle"].ToString(),
                ToSizeCode = s["ToSizeCode"].ToString(),
                DisplayToSewingQty = MyUtility.Convert.GetInt(s["DisplayToSewingQty"]),
                OrderQtyUpperlimit = MyUtility.Convert.GetInt(s["OrderQtyUpperlimit"]),
            })
            .Select(g => new
            {
                g.Key.ToOrderID,
                g.Key.ToComboType,
                g.Key.ToArticle,
                g.Key.ToSizeCode,
                g.Key.DisplayToSewingQty,
                g.Key.OrderQtyUpperlimit,
                TransferQty = g.Sum(s => MyUtility.Convert.GetInt(s["TransferQty"])),
            }).Where(w => w.TransferQty + w.DisplayToSewingQty > w.OrderQtyUpperlimit).ToList();

            if (list2.Count > 0)
            {
                string msg = "< Transfer Qty > + < To SP# Sewing Output Qty > can not more than < To SP# Qty >!";
                foreach (var item in list2)
                {
                    msg += $"\r\nTo SP#: {item.ToOrderID}, {item.ToComboType}, Article:{item.ToArticle}, Size:{item.ToSizeCode}";
                }

                MyUtility.Msg.WarningBox(msg);
                return false;
            }

            return true;
        }

        private bool UpdateMESInspection(TransactionScope scope)
        {
            string sqlcmd = string.Empty;

            // 跑 P11 表身迴圈，在MES inspection一筆資料是一件，更新是用 Top (TransferQty) 數量。一筆轉移就只會有一段update
            foreach (DataRow item in this.DetailDatas)
            {
                sqlcmd += $@"
update Inspection set 
	OrderId = '{item["ToOrderID"]}',
	Location = '{item["ToComboType"]}', 
	Article = '{item["ToArticle"]}', 
	Size = '{item["ToSizeCode"]}',  
	StyleUkey = {item["styleUkey"]}, 
	SewingOutputTransfer_DetailUkey = {item["Ukey"]}
from Inspection with(nolock)
where Status in ('Pass','Fixed') and OrderId = '{item["FromOrderID"]}' and Location = '{item["FromComboType"]}'
and Article='{item["Article"]}' and Size='{item["SizeCode"]}'
and AddDate in (
	select top {item["TransferQty"]} AddDate
	from Inspection with(nolock)
	where Status in ('Pass','Fixed') and OrderId = '{item["FromOrderID"]}' and Location = '{item["FromComboType"]}'
    and Article='{item["Article"]}' and Size='{item["SizeCode"]}'
	order by AddDate 
)
;
";
            }

            DualResult result = DBProxy.Current.Execute("ManufacturingExecution", sqlcmd);
            if (!result)
            {
                scope.Dispose();
                this.ShowErr(result);
                return false;
            }

            return true;
        }

        private void SetDefaultToArticle()
        {
            if (MyUtility.Check.Empty(this.CurrentDetailData["ToArticle"]))
            {
                this.CurrentDetailData["ToArticle"] = this.CurrentDetailData["Article"];
                this.CurrentDetailData.EndEdit();
            }
        }

        private void SetDefaultToSizeCode()
        {
            if (MyUtility.Check.Empty(this.CurrentDetailData["ToSizeCode"]))
            {
                this.CurrentDetailData["ToSizeCode"] = this.CurrentDetailData["SizeCode"];
                this.CurrentDetailData.EndEdit();
            }
        }

        private void GetDisplayFromQty()
        {
            List<SqlParameter> lis = new List<SqlParameter>();
            lis.Add(new SqlParameter("@FromOrderID", MyUtility.Convert.GetString(this.CurrentDetailData["FromOrderID"])));
            lis.Add(new SqlParameter("@Article", MyUtility.Convert.GetString(this.CurrentDetailData["Article"])));
            lis.Add(new SqlParameter("@SizeCode", MyUtility.Convert.GetString(this.CurrentDetailData["SizeCode"])));
            string sqlcmd = $@"
declare @Orderid varchar(13) = @FromOrderID
declare @A varchar(8) = @Article
declare @S varchar(8) = @SizeCode

select Qty
from Order_Qty with(nolock)
where ID = @Orderid
and Article = @A
and SizeCode = @S
";
            try
            {
                DataRow dr;
                if (MyUtility.Check.Seek(sqlcmd, lis, out dr))
                {
                    this.CurrentDetailData["DisplayFromQty"] = MyUtility.Convert.GetInt(dr["Qty"]);
                    this.CurrentDetailData.EndEdit();
                }
            }
            catch (Exception ex)
            {
                this.ShowErr(ex);
            }
        }

        private void GetDisplayFromSewingQty()
        {
            List<SqlParameter> lis = new List<SqlParameter>();
            lis.Add(new SqlParameter("@FromOrderID", MyUtility.Convert.GetString(this.CurrentDetailData["FromOrderID"])));
            lis.Add(new SqlParameter("@FromComboType", MyUtility.Convert.GetString(this.CurrentDetailData["FromComboType"])));
            lis.Add(new SqlParameter("@Article", MyUtility.Convert.GetString(this.CurrentDetailData["Article"])));
            lis.Add(new SqlParameter("@SizeCode", MyUtility.Convert.GetString(this.CurrentDetailData["SizeCode"])));
            string sqlcmd = $@"
declare @Orderid varchar(13) = @FromOrderID
declare @ComboType varchar(1) = @FromComboType
declare @A varchar(8) = @Article
declare @S varchar(8) = @SizeCode

select QAQty =
isnull((select sum(QAQty)
from SewingOutput_Detail_Detail with(nolock)
where OrderID = @Orderid
and ComboType = @ComboType
and Article = @A
and SizeCode = @S),0)
-
isnull((select sum(QAQty)
from SewingOutput_Detail_Detail_Garment with(nolock)
where OrderIDFrom = @Orderid
and ComboType = @ComboType
and Article = @A
and SizeCode = @S),0)

,PackingQty=
isnull((select SUM(pd.ShipQty)
from PackingList_Detail pd with(nolock)
inner join PackingList p with(nolock) on p.ID = pd.ID
where p.Status = 'Confirmed'
and pd.OrderID = @Orderid
and pd.Article = @A
and pd.SizeCode = @S),0)
";
            try
            {
                DataRow dr;
                if (MyUtility.Check.Seek(sqlcmd, lis, out dr))
                {
                    this.CurrentDetailData["DisplayFromSewingQty"] = MyUtility.Convert.GetInt(dr["QAQty"]);
                    this.CurrentDetailData["PackingQty"] = MyUtility.Convert.GetInt(dr["PackingQty"]);
                    this.CurrentDetailData.EndEdit();
                }
            }
            catch (Exception ex)
            {
                this.ShowErr(ex);
            }
        }

        private void GetDisplayToQty()
        {
            List<SqlParameter> lis = new List<SqlParameter>();
            lis.Add(new SqlParameter("@ToOrderID ", MyUtility.Convert.GetString(this.CurrentDetailData["ToOrderID"])));
            lis.Add(new SqlParameter("@ToArticle", MyUtility.Convert.GetString(this.CurrentDetailData["ToArticle"])));
            lis.Add(new SqlParameter("@ToSizeCode", MyUtility.Convert.GetString(this.CurrentDetailData["ToSizeCode"])));
            string sqlcmd = $@"
declare @Orderid varchar(13) = @ToOrderID
declare @A varchar(8) = @ToArticle
declare @S varchar(8) = @ToSizeCode

select Qty, OrderQtyUpperlimit=dbo.GetOrderQtyUpperlimit(ID,Article,SizeCode)
from Order_Qty with(nolock)
where ID = @Orderid
and Article = @A
and SizeCode = @S
";
            try
            {
                DataRow dr;
                if (MyUtility.Check.Seek(sqlcmd, lis, out dr))
                {
                    this.CurrentDetailData["DisplayToQty"] = MyUtility.Convert.GetInt(dr["Qty"]);
                    this.CurrentDetailData["OrderQtyUpperlimit"] = MyUtility.Convert.GetInt(dr["OrderQtyUpperlimit"]);
                    this.CurrentDetailData.EndEdit();
                }
            }
            catch (Exception ex)
            {
                this.ShowErr(ex);
            }

            this.GetBalanceQty();
        }

        private void GetDisplayToSewingQty()
        {
            List<SqlParameter> lis = new List<SqlParameter>();
            lis.Add(new SqlParameter("@ToOrderID", MyUtility.Convert.GetString(this.CurrentDetailData["ToOrderID"])));
            lis.Add(new SqlParameter("@ToComboType", MyUtility.Convert.GetString(this.CurrentDetailData["ToComboType"])));
            lis.Add(new SqlParameter("@ToArticle", MyUtility.Convert.GetString(this.CurrentDetailData["ToArticle"])));
            lis.Add(new SqlParameter("@ToSizeCode", MyUtility.Convert.GetString(this.CurrentDetailData["ToSizeCode"])));
            string sqlcmd = $@"
declare @Orderid varchar(13) = @ToOrderID
declare @ComboType varchar(1) = @ToComboType
declare @A varchar(8) = @ToArticle
declare @S varchar(8) = @ToSizeCode

select QAQty=sum(QAQty)
from SewingOutput_Detail_Detail with(nolock)
where OrderID = @Orderid
and ComboType = @ComboType
and Article = @A
and SizeCode = @S
";
            try
            {
                DataRow dr;
                if (MyUtility.Check.Seek(sqlcmd, lis, out dr))
                {
                    this.CurrentDetailData["DisplayToSewingQty"] = MyUtility.Convert.GetInt(dr["QAQty"]);
                    this.CurrentDetailData.EndEdit();
                }
            }
            catch (Exception ex)
            {
                this.ShowErr(ex);
            }

            this.GetBalanceQty();
        }

        private void GetBalanceQty()
        {
            this.CurrentDetailData["BalanceQty"] = MyUtility.Convert.GetInt(this.CurrentDetailData["DisplayToSewingQty"]) - MyUtility.Convert.GetInt(this.CurrentDetailData["DisplayToQty"]);
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            DataTable detailDt = (DataTable)this.detailgridbs.DataSource;
            P11_Import form = new P11_Import(detailDt);
            form.ShowDialog(this);

            this.detailgridbs.Position = 0;
            for (int i = 0; i < detailDt.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted).Count(); i++)
            {
                this.detailgridbs.Position = i;
                this.GetDisplayFromQty();
                this.GetDisplayFromSewingQty();
                this.GetDisplayToQty();
                this.GetDisplayToSewingQty();
            }

            for (int i = detailDt.Rows.Count - 1; i >= 0; i--)
            {
                if (MyUtility.Check.Empty(detailDt.Rows[i]["FromOrderID"]) && MyUtility.Check.Empty(detailDt.Rows[i]["ToOrderID"]))
                {
                    detailDt.Rows[i].Delete();
                }
            }
        }
    }
}
