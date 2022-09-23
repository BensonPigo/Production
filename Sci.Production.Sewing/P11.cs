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
using System.Threading.Tasks;
using Sci.Production.Automation;
using System.Threading;

namespace Sci.Production.Sewing
{
    /// <inheritdoc/>
    public partial class P11 : Win.Tems.Input6
    {
        /// <summary>
        /// = P01手動拆表身數量操作(多天OutputDate)
        /// 要被拆的表身(From)   拆出去(To)
        /// Form不能AutoCreate   To只能Category (B,S)
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P11(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = $"FactoryID = '{Env.User.Factory}'";
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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
                    string sqlcmd = $@"select Location from Order_Location with(nolock) where OrderID = @FromOrderID
                                        union
                                      select distinct [Location] = ComboType from sewingoutput_Detail_detail where orderid=@FromOrderID";
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
                    string sqlcmd = $@"select 1 from Order_Location with(nolock) where OrderID = @FromOrderID and Location = @Location
                                        union
                                      select 1 from sewingoutput_Detail_detail where orderid=@FromOrderID  and ComboType = @Location";
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
                    string sqlcmd = $@"select distinct Article from Order_Qty with(nolock) where ID = @FromOrderID
                                        union
                                      select distinct Article from sewingoutput_Detail_detail where orderid=@FromOrderID";
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
                    string sqlcmd = $@"select 1 from Order_Qty with(nolock) where ID = @FromOrderID and Article = @Article
                                        union
                                      select 1 from sewingoutput_Detail_detail where orderid = @FromOrderID and Article = @Article ";
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
                    string sqlcmd = $@"select distinct SizeCode from Order_Qty with(nolock) where ID = @FromOrderID 
                                        union
                                      select distinct sizecode from sewingoutput_Detail_detail where orderid=@FromOrderID ";
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
                            where (exludeOrder.junk = 1 and exludeOrder.NeedProduction = 0 AND exludeOrder.Category='B') and
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

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["CreateDate"] = DateTime.Today;
            this.CurrentMaintain["FactoryID"] = Env.User.Factory;
            this.CurrentMaintain["Status"] = "New";
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            if (MyUtility.Convert.GetString(this.CurrentMaintain["Status"]).ToUpper() == "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("This record already confirmed, can't edit.");
                return false;
            }

            return base.ClickEditBefore();
        }

        /// <inheritdoc/>
        protected override bool ClickDeleteBefore()
        {
            if (MyUtility.Convert.GetString(this.CurrentMaintain["Status"]) == "Confirmed")
            {
                MyUtility.Msg.WarningBox("This record already confirmed, can't delete.");
                return false;
            }

            return base.ClickDeleteBefore();
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (this.DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("The detail data can't be empty!");
                return false;
            }

            P11_SewingOutputOrderChange p11_SewingOutputOrderChange = new P11_SewingOutputOrderChange(this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource, Env.User.UserID);

            int errorDetailPosition = -1;
            DualResult result = p11_SewingOutputOrderChange.BeforeSaveCheck(out errorDetailPosition);

            if (!result)
            {
                if (errorDetailPosition > -1)
                {
                    this.detailgrid.SelectRowTo(errorDetailPosition);
                    MyUtility.Msg.WarningBox(result.Description);
                    return false;
                }
                else
                {
                    this.ShowErr(result);
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

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            this.OnRefreshClick();
            base.ClickConfirm();

            P11_SewingOutputOrderChange p11_SewingOutputOrderChange = new P11_SewingOutputOrderChange(this.CurrentMaintain, (DataTable)this.detailgridbs.DataSource, Env.User.UserID);
            DBProxy.Current.DefaultTimeout = 1800;
            DualResult result = p11_SewingOutputOrderChange.Confirm();

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            #region ISP20201344 資料交換 - Sunrise
            string listUkey = this.DetailDatas.Select(s => s["Ukey"].ToString()).JoinToString(",");
            Task.Run(() => new Sunrise_FinishingProcesses().SentSewingOutputTransfer(listUkey))
                .ContinueWith(UtilityAutomation.AutomationExceptionHandler, System.Threading.CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
            #endregion
            DBProxy.Current.DefaultTimeout = 300;
            MyUtility.Msg.InfoBox("Complete!");
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

        private void BtnTransferInOutInformation_Click(object sender, EventArgs e)
        {
            P11_TransferInOutInformation frm = new P11_TransferInOutInformation();
            frm.ShowDialog(this);
        }
    }
}
