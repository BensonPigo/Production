using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;
using System.Linq;
using System.Transactions;
using System.Data.SqlClient;
using Sci.Win.Tools;
using System.Runtime.InteropServices;
using System.IO;

namespace Sci.Production.Sewing
{
    /// <summary>
    /// P01
    /// </summary>
    public partial class P01 : Win.Tems.Input8
    {
        private ITableSchema sub_Schema;
        private DataGridViewGeneratorTextColumnSettings qaoutput = new DataGridViewGeneratorTextColumnSettings();
        private DataGridViewGeneratorTextColumnSettings orderid = new DataGridViewGeneratorTextColumnSettings();
        private DataGridViewGeneratorTextColumnSettings combotype = new DataGridViewGeneratorTextColumnSettings();
        private DataGridViewGeneratorTextColumnSettings article = new DataGridViewGeneratorTextColumnSettings();
        private DataGridViewGeneratorNumericColumnSettings inlineqty = new DataGridViewGeneratorNumericColumnSettings();
        private DataGridViewGeneratorTextColumnSettings SewingReasonID = new DataGridViewGeneratorTextColumnSettings();
        private Ict.Win.UI.DataGridViewTextBoxColumn textOrderIDSetting;
        private decimal? oldttlqaqty;
        private decimal? oldManHour;
        private string loginFactory;
        private DateTime? dateYesterday;
        private DataTable rftDT;

        /// <summary>
        /// P01
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("FactoryID = '{0}' and Category = 'O'", Env.User.Factory);
            MyUtility.Tool.SetupCombox(this.comboTeam, 1, 1, "A,B");
            this.DoSubForm = new P01_QAOutput();

            // 當Grid目前在最後一筆的最後一欄時，按Enter要自動新增一筆Record
            this.detailgrid.StatusNotification += (s, e) =>
            {
                if (this.EditMode && e.Notification == Ict.Win.UI.DataGridViewStatusNotification.NoMoreRowOnEnterPress)
                {
                    this.OnDetailGridInsert();
                }
            };

            this.loginFactory = Env.User.Factory;
            this.dateYesterday = DateTime.Now.AddDays(-1);
        }

        /// <inheritdoc/>
        protected override void EnsureToolbarExt()
        {
            base.EnsureToolbarExt();
            if (this.Perm.Send)
            {
                this.toolbar.cmdSend.Enabled = true;
            }

            #region 顯示Recall 按鈕條件
            if (this.CurrentMaintain != null)
            {
                string strSqlcmd = $@"select * from SewingOutput_DailyUnlock
where UnLockDate is null and SewingOutputID='{this.CurrentMaintain["ID"]}'";
                if (MyUtility.Check.Seek(strSqlcmd) &&
                    this.Perm.Recall &&
                    string.Compare(this.CurrentMaintain["Status"].ToString(), "Sent") == 0)
                {
                    this.toolbar.cmdRecall.Enabled = true;
                }
                else
                {
                    this.toolbar.cmdRecall.Enabled = false;
                }
            }
            #endregion
        }

        /// <inheritdoc/>
        protected override void OnDetailGridDelete()
        {
            if (this.CurrentMaintain == null || this.CurrentDetailData == null)
            {
                return;
            }

            if (this.CurrentDetailData["AutoCreate"].EqualString("True"))
            {
                MyUtility.Msg.WarningBox("Can't delete autocreate Item.");
                return;
            }

            string sqlcmd = $@"
select 1
from Inspection
where orderid = '{this.CurrentDetailData["OrderID"]}'
and Article = '{this.CurrentDetailData["Article"]}'
and InspectionDate = '{((DateTime)this.CurrentMaintain["OutputDate"]).ToString("yyyy/MM/dd")}'
and FactoryID = '{this.CurrentMaintain["FactoryID"]}'
and Line = '{this.CurrentMaintain["SewingLineID"]}'
and Team = '{this.CurrentMaintain["Team"]}'
and Shift = case '{this.CurrentMaintain["Shift"]}' 
            when 'D' then 'DAY'
            when 'N' then 'Night'
            else '' end
and Location = '{this.CurrentDetailData["ComboType"]}'
and SunriseNid != 0
";

            if (this.CurrentDetailData["ImportFromDQS"].EqualString("True") &&
                !MyUtility.Check.Seek(sqlcmd, "ManufacturingExecution"))
            {
                MyUtility.Msg.WarningBox("If DQS record is inaccurate,please update QA Qty to zero manually");
                return;
            }

            if (this.CheckRemoveRow() == false)
            {
                return;
            }

            base.OnDetailGridDelete();

            if (this.EditMode && this.detailgridbs.DataSource != null)
            {
                // 重新計算表頭 QAQty, InlineQty, DefectQty
                if (((DataTable)this.detailgridbs.DataSource).AsEnumerable().Any(row => row.RowState != DataRowState.Deleted && row["AutoCreate"].EqualString("False")))
                {
                    this.CurrentMaintain["QAQty"] = ((DataTable)this.detailgridbs.DataSource).AsEnumerable().Where(row => row.RowState != DataRowState.Deleted
                                                                                                                     && row["AutoCreate"].EqualString("False")).CopyToDataTable().Compute("SUM(QAQty)", string.Empty);
                    this.CurrentMaintain["InlineQty"] = MyUtility.Convert.GetInt(this.CurrentMaintain["QAQty"]) + MyUtility.Convert.GetInt(this.CurrentMaintain["DefectQty"]);
                    this.CurrentMaintain["DefectQty"] = ((DataTable)this.detailgridbs.DataSource).AsEnumerable().Where(row => row.RowState != DataRowState.Deleted
                                                                                                                         && row["AutoCreate"].EqualString("False")).CopyToDataTable().Compute("SUM(DefectQty)", string.Empty);
                }
                else
                {
                    this.CurrentMaintain["QAQty"] = 0;
                    this.CurrentMaintain["InlineQty"] = 0;
                    this.CurrentMaintain["DefectQty"] = 0;
                }
            }
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            if (this.CurrentMaintain != null)
            {
                DateTime? sewingMonthlyLockDate = MyUtility.Convert.GetDate(MyUtility.GetValue.Lookup($"select LockDate from SewingMonthlyLock where FactoryID = '{this.CurrentMaintain["FactoryID"]}'"));
                this.btnRevisedHistory.Enabled = !this.EditMode && MyUtility.Convert.GetDate(this.CurrentMaintain["OutputDate"]) <= sewingMonthlyLockDate;

                #region "btnRequestUnlock"
                this.btnRequestUnlock.Visible = MyUtility.Convert.GetString(this.CurrentMaintain["Status"]).EqualString("Sent");
                DataTable dt;
                string sql = string.Format(
                    @"select count(*) cnt
                      from SewingOutput_DailyUnlock
                     where 1=1
                       and SewingOutputID = '{0}'
                       and UnLockDate is null", this.CurrentMaintain["ID"]);
                if (DBProxy.Current.Select(null, sql, null, out dt))
                {
                    if (!MyUtility.Check.Empty(dt) || dt.Rows.Count > 0)
                    {
                        this.btnRequestUnlock.Enabled = MyUtility.Convert.GetInt(dt.Rows[0]["cnt"]) > 0 ? false : true;
                    }
                }
                #endregion

                this.oldttlqaqty = this.numQAOutput.Value;
                this.oldManHour = MyUtility.Convert.GetDecimal(this.CurrentMaintain["ManHour"]);
                switch (MyUtility.Convert.GetString(this.CurrentMaintain["Status"]))
                {
                    case "Sent":
                        this.lbstatus.Text = "Daily Lock";
                        break;
                    case "Locked":
                        this.lbstatus.Text = "Monthly Lock";
                        break;
                    default:
                        this.lbstatus.Text = string.Empty;
                        break;
                }

                if (this.EditMode)
                {
                    if (MyUtility.Check.Seek($"select 1 from dbo.SCIFty with (nolock) where ID = '{this.CurrentMaintain["SubconOutFty"]}'"))
                    {
                        this.txtSubConOutContractNumber.ReadOnly = true;
                    }
                    else
                    {
                        this.txtSubConOutContractNumber.ReadOnly = false;
                    }
                }
            }
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : MyUtility.Convert.GetString(e.Master["ID"]);
            this.DetailSelectCommand = string.Format(
                @"
select  sd.*
        , [RFT] = CONVERT(VARCHAR, convert(Decimal(18,2), iif(sd.InlineQty = 0, 0, ROUND(sd.QAQty* 1.0 / sd.InlineQty * 1.0 * 100 ,2)))) +'%'
        , [Tips] = iif( (SELECT MAX(ID) FROM SewingSchedule ss WITH (NOLOCK) WHERE ss.OrderID = sd.OrderId and ss.FactoryID = s.FactoryID and ss.SewingLineID = s.SewingLineID)  is null,'Data Migration (not belong to this line#)','') 
        , [QAOutput] = (select t.TEMP+',' from (select sdd.SizeCode+'*'+CONVERT(varchar,sdd.QAQty) AS TEMP from SewingOutput_Detail_Detail SDD WITH (NOLOCK) where SDD.SewingOutput_DetailUKey = sd.UKey) t for xml path(''))
		, [SewingReasonID]=sr.id
		, [ReasonDescription]=sr.Description
from SewingOutput_Detail sd WITH (NOLOCK) 
left join SewingOutput s WITH (NOLOCK) on sd.ID = s.ID
LEFT JOIN SewingReason sr ON sd.SewingReasonID=sr.ID
outer apply( select top 1 * from Rft WITH (NOLOCK) where rft.OrderID = sd.OrderId 
                               and rft.CDate = s.OutputDate 
                               and rft.SewinglineID = s.SewingLineID 
                               and rft.Shift = s.Shift 
                               and rft.Team = s.Team) Rft
where sd.ID = '{0}'",
                masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override DualResult OnSubDetailSelectCommandPrepare(PrepareSubDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Detail == null) ? "0" : MyUtility.Convert.GetString(e.Detail["UKey"]);

            this.SubDetailSelectCommand = string.Format(
                @"
;with AllQty as
(
	select 
        sd.ID, 
        sd.UKey as SewingOutput_DetailUkey, sd.OrderId, sd.ComboType,oq.Article,oq.SizeCode,oq.Qty as OrderQty,
	isnull((select QAQty from SewingOutput_Detail_Detail WITH (NOLOCK) where SewingOutput_DetailUkey = sd.UKey and SizeCode = oq.SizeCode and orderid = sd.OrderId),0) as QAQty ,
	isnull(
			(
				select sum(QAQty) 
				from SewingOutput_Detail_Detail WITH (NOLOCK) 
				where OrderId = sd.OrderId and ComboType = sd.ComboType and Article = oq.Article and SizeCode = oq.SizeCode and ID != sd.ID
			),0
		    ) as AccumQty
	from SewingOutput_Detail sd WITH (NOLOCK) ,Order_Qty oq WITH (NOLOCK)  
	where sd.UKey = '{0}' and sd.OrderId = oq.ID and sd.Article=oq.Article
	union all
	select sdd.ID, sdd.SewingOutput_DetailUkey, sdd.OrderId, sdd.ComboType,sdd.Article,sdd.SizeCode,0 as OrderQty,sdd.QAQty,
	isnull(
			(
				select sum(QAQty) 
				from SewingOutput_Detail_Detail WITH (NOLOCK)  
				where OrderId = sdd.OrderId and ComboType = sdd.ComboType and Article = sdd.Article and SizeCode = sdd.SizeCode and ID != sdd.ID
			),0
		    ) as AccumQty 
	from SewingOutput_Detail_Detail sdd WITH (NOLOCK) 
	where SewingOutput_DetailUKey = '{0}'
	and not exists (select 1 from Order_Qty WITH (NOLOCK) where ID = sdd.OrderId and Article = sdd.Article and SizeCode = sdd.SizeCode)
)
select  a.*
	,OrderQty.OrderQtyUpperlimit
	, [Variance] = a.OrderQty-a.AccumQty
	, [BalQty] = a.OrderQty-a.AccumQty-a.QAQty
	, [Seq] = isnull(os.Seq,0)
from AllQty a
left join Orders o WITH (NOLOCK) on a.OrderId = o.ID
left join Order_SizeCode os WITH (NOLOCK) on os.Id = o.POID and os.SizeCode = a.SizeCode
outer apply(
	select value=1
	from Order_TmsCost ot with(nolock)
	inner join Order_Qty oq WITH (NOLOCK) on ot.id = oq.ID
	where ot.ArtworkTypeID = 'Garment Dye' and ot.Price > 0
	and oq.SizeCode=os.SizeCode and oq.Article=a.Article and ot.id=o.id
	and o.LocalOrder<>1
)b
outer apply(select OrderQtyUpperlimit=iif(b.value is not null,round(cast(a.OrderQty as decimal)* (1+ isnull(o.DyeingLoss,0)/100),0),a.OrderQty))OrderQty
order by a.OrderId,os.Seq",
                masterID);
            return base.OnSubDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            this.qaoutput.CellMouseDoubleClick += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    this.OpenSubDetailPage();
                }
            };
            #region SP#的Right click & Validating
            this.orderid.EditingMouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (this.EditMode && this.CurrentDetailData["AutoCreate"].EqualString("False"))
                    {
                        if (e.RowIndex != -1)
                        {
                            if (this.CheckRemoveRow() == false || this.CheckSPEditable() == false)
                            {
                                return;
                            }

                            DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                            string sqlCmd = string.Format(
                                @"
select distinct ss.OrderID 
from SewingSchedule ss WITH (NOLOCK) 
inner join Orders o With (NoLock) on ss.OrderID = o.ID
where   ss.FactoryID = '{0}' 
        and ss.SewingLineID = '{1}' 
        and ss.OrderFinished = 0
		and o.Category != 'G'
        and not exists (select 1 from Orders exludeOrder with (nolock) 
                            where ((exludeOrder.junk = 1 and exludeOrder.NeedProduction = 0) or 
                                  (exludeOrder.IsBuyBack = 1 and exludeOrder.BuyBackReason = 'Garment')) and
                                  exludeOrder.ID = o.ID
                        )",
                                Env.User.Factory,
                                MyUtility.Convert.GetString(this.CurrentMaintain["SewingLineID"]));

                            SelectItem item = new SelectItem(sqlCmd, "20", dr["OrderID"].ToString());
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel)
                            {
                                return;
                            }

                            e.EditingControl.Text = item.GetSelectedString();
                        }
                    }
                }
            };

            this.orderid.CellValidating += (s, e) =>
            {
                if (this.EditMode && this.CurrentDetailData["OrderID"].EqualString(e.FormattedValue) == false)
                {
                    if (this.CurrentDetailData["OrderID"].Empty() == false && this.CheckRemoveRow() == false)
                    {
                        this.detailgrid.GetDataRow<DataRow>(e.RowIndex)["OrderID"] = this.CurrentDetailData["OrderID"];
                        return;
                    }

                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (MyUtility.Convert.GetString(e.FormattedValue) != MyUtility.Convert.GetString(dr["OrderID"]))
                    {
                        // 資料有異動過就先刪除SubDetail資料
                        this.DeleteSubDetailData(dr);
                        if (MyUtility.Check.Empty(e.FormattedValue))
                        {
                            dr["OrderID"] = string.Empty;
                            dr["ComboType"] = string.Empty;
                            dr["Article"] = string.Empty;
                            dr["Color"] = string.Empty;
                            dr["QAOutput"] = string.Empty;
                            dr["TMS"] = 0;
                            dr["RFT"] = "0.00%";
                            dr.EndEdit();
                            return;
                        }

                        // sql參數
                        SqlParameter sp1 = new SqlParameter("@factoryid", Env.User.Factory);
                        SqlParameter sp2 = new SqlParameter("@id", MyUtility.Convert.GetString(e.FormattedValue));

                        IList<SqlParameter> cmds = new List<SqlParameter>();
                        cmds.Add(sp1);
                        cmds.Add(sp2);

                        DataTable ordersData;
                        string sqlCmd = @"
select  o.IsForecast
        , o.SewLine
        , o.CPU
        , o.CPUFactor
        , o.StyleUkey
        , StdTMS = (select StdTMS 
                      from System WITH (NOLOCK))
from Orders o WITH (NOLOCK) 
inner join Factory f on o.FactoryID = f.ID
where   o.FtyGroup = @factoryid 
        and o.ID = @id
		and o.Category NOT IN ('G','A')
        and f.IsProduceFty = 1
        and not exists (select 1 from Orders exludeOrder with (nolock) 
                            where ((exludeOrder.junk = 1 and exludeOrder.NeedProduction = 0) or 
                                  (exludeOrder.IsBuyBack = 1 and exludeOrder.BuyBackReason = 'Garment')) and
                                  exludeOrder.ID = o.ID
                        )";
                        DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out ordersData);
                        if (!result || ordersData.Rows.Count <= 0)
                        {
                            if (!result)
                            {
                                MyUtility.Msg.WarningBox("Sql connection fail!\r\n" + result.ToString());
                            }
                            else
                            {
                                MyUtility.Msg.WarningBox("Data not found!!!");
                            }

                            dr["OrderID"] = string.Empty;
                            e.Cancel = true;
                            return;
                        }
                        else
                        {
                            if (MyUtility.Convert.GetBool(ordersData.Rows[0]["IsForecast"]))
                            {
                                dr["OrderID"] = string.Empty;
                                e.Cancel = true;
                                MyUtility.Msg.WarningBox("SP# can't be PreOrder!!");
                                return;
                            }

                            // 當該SP#+Line不屬於排程時，跳出確認訊息
                            if (!MyUtility.Check.Seek(string.Format("select ID from SewingSchedule WITH (NOLOCK) where OrderID = '{0}' and SewingLineID = '{1}' and OrderFinished=0", MyUtility.Convert.GetString(e.FormattedValue), MyUtility.Convert.GetString(this.CurrentMaintain["SewingLineID"]))))
                            {
                                // 問是否要繼續，確定才繼續往下做
                                DialogResult buttonResult = MyUtility.Msg.WarningBox("This SP# dosen't belong to this line, please inform scheduler.\r\n\r\nDo you want to continue?", "Warning", MessageBoxButtons.YesNo);
                                if (buttonResult == DialogResult.No)
                                {
                                    dr["OrderID"] = string.Empty;
                                    e.Cancel = true;
                                    return;
                                }
                                else
                                {
                                    dr["Tips"] = "Data Migration (not belong to this line#)";
                                }
                            }

                            dr["OrderID"] = e.FormattedValue.ToString();
                            dr["ComboType"] = string.Empty;
                            dr["Article"] = string.Empty;
                            dr["Color"] = string.Empty;
                            dr["QAOutput"] = string.Empty;
                            dr["TMS"] = 0;
                            this.GetRFT(dr);

                            #region 若此SP是套裝的話，就跳出視窗讓使用者選擇部位

                            if (MyUtility.Check.Seek($@"select 1 from Order_Location where OrderId= '{dr["OrderID"]}'"))
                            {
                                sqlCmd = string.Format(
                                @"
select Location
,Rate = isnull([dbo].[GetOrderLocation_Rate]('{0}',Location)
,[dbo].[GetOrderLocation_Rate]('{0}',Location)) 
from Order_Location WITH (NOLOCK) 
where OrderId = '{0}'
", MyUtility.Convert.GetString(dr["OrderID"]));
                            }
                            else
                            {
                                sqlCmd = string.Format(
                                    @"
select Location
,Rate = isnull([dbo].[GetOrderLocation_Rate]('{1}',Location)
,[dbo].[GetStyleLocation_Rate]('{0}',Location)) 
from Style_Location WITH (NOLOCK) 
where StyleUkey = {0}",
                                    MyUtility.Convert.GetString(ordersData.Rows[0]["StyleUkey"]),
                                    MyUtility.Convert.GetString(dr["OrderID"]));
                            }

                            DataTable orderLocation;
                            result = DBProxy.Current.Select(null, sqlCmd, out orderLocation);
                            if (!result || orderLocation.Rows.Count <= 0)
                            {
                                if (!result)
                                {
                                    MyUtility.Msg.WarningBox("Sql connection fail!!\r\n" + result.ToString());
                                }
                                else
                                {
                                    MyUtility.Msg.WarningBox("No combo type data!!");
                                }
                            }

                            {
                                if (orderLocation.Rows.Count == 1)
                                {
                                    dr["ComboType"] = orderLocation.Rows[0]["Location"];
                                    dr["TMS"] = this.CalculateTMS(ordersData.Rows[0], 100);
                                }
                                else
                                {
                                    SelectItem item = new SelectItem(orderLocation, "Location", "3", MyUtility.Convert.GetString(dr["ComboType"]), headercaptions: "*");
                                    DialogResult returnResult = item.ShowDialog();
                                    if (returnResult != DialogResult.Cancel)
                                    {
                                        IList<DataRow> location = item.GetSelecteds();
                                        dr["ComboType"] = item.GetSelectedString();
                                        dr["TMS"] = this.CalculateTMS(ordersData.Rows[0], MyUtility.Convert.GetDecimal(location[0]["Rate"]));
                                    }
                                }
                            }
                            #endregion
                            dr.EndEdit();
                            this.CreateSubDetailDatas(dr);
                        }
                    }
                }
            };
            #endregion
            #region ComboType的Right Click
            this.combotype.EditingMouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (this.EditMode)
                    {
                        if (e.RowIndex != -1 && this.CurrentDetailData["AutoCreate"].EqualString("False"))
                        {
                            if (this.CheckRemoveRow() == false)
                            {
                                return;
                            }

                            string sqlCmd = string.Empty;
                            DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);

                            if (MyUtility.Check.Seek($@"select 1 from Order_Location where OrderId= '{dr["OrderID"]}'"))
                            {
                                sqlCmd = string.Format(
                                    @"
select ol.Location
,Rate = isnull([dbo].[GetOrderLocation_Rate]('{0}' ,ol.Location)
,[dbo].[GetStyleLocation_Rate](o.StyleUkey ,ol.Location)),o.CPU,o.CPUFactor
,(select StdTMS from System WITH (NOLOCK) ) as StdTMS 
from Orders o WITH (NOLOCK) , Order_Location ol WITH (NOLOCK) 
where o.ID = '{0}' and o.ID = ol.OrderId", MyUtility.Convert.GetString(dr["OrderID"]));
                            }
                            else
                            {
                                sqlCmd = string.Format(
                                    @"
select sl.Location
,Rate = isnull([dbo].[GetOrderLocation_Rate]('{0}' ,sl.Location)
,[dbo].[GetStyleLocation_Rate](o.StyleUkey ,sl.Location))
,o.CPU,o.CPUFactor,(select StdTMS from System WITH (NOLOCK) ) as StdTMS 
from Orders o WITH (NOLOCK) , Style_Location sl WITH (NOLOCK) 
where o.ID = '{0}' and o.StyleUkey = sl.StyleUkey", MyUtility.Convert.GetString(dr["OrderID"]));
                            }

                            DataTable locationData;
                            DualResult result = DBProxy.Current.Select(null, sqlCmd, out locationData);
                            SelectItem item = new SelectItem(locationData, "Location", "10", MyUtility.Convert.GetString(dr["ComboType"]), headercaptions: "*");
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel)
                            {
                                return;
                            }

                            IList<DataRow> location = item.GetSelecteds();
                            bool changed = item.GetSelectedString() != MyUtility.Convert.GetString(dr["ComboType"]);
                            dr["ComboType"] = item.GetSelectedString();
                            dr["TMS"] = this.CalculateTMS(location[0], MyUtility.Convert.GetDecimal(location[0]["Rate"]));
                            if (changed)
                            {
                                dr["QAOutput"] = string.Empty;
                                dr.EndEdit();
                                this.DeleteSubDetailData(dr);
                                this.CreateSubDetailDatas(dr);
                                return;
                            }

                            dr.EndEdit();
                        }
                    }
                }
            };

            #endregion
            #region Article的Right Click & Validating
            this.article.EditingMouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (this.EditMode && this.CurrentDetailData["AutoCreate"].EqualString("False"))
                    {
                        if (e.RowIndex != -1)
                        {
                            if (this.CheckRemoveRow() == false)
                            {
                                return;
                            }

                            DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                            string sqlCmd = string.Format("select Article,ColorID from View_OrderFAColor where Id = '{0}'", MyUtility.Convert.GetString(dr["OrderID"]));

                            SelectItem item = new SelectItem(sqlCmd, "8,8", MyUtility.Convert.GetString(dr["Article"]), headercaptions: "Article,Color");
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel)
                            {
                                return;
                            }

                            IList<DataRow> colorData = item.GetSelecteds();
                            bool changed = item.GetSelectedString() != MyUtility.Convert.GetString(dr["Article"]);
                            dr["Article"] = item.GetSelectedString();
                            dr["Color"] = colorData[0]["ColorID"];
                            if (changed)
                            {
                                dr["QAOutput"] = string.Empty;
                            }

                            dr.EndEdit();
                            if (changed)
                            {
                                this.DeleteSubDetailData(dr);
                                this.CreateSubDetailDatas(dr);
                            }
                        }
                    }
                }
            };

            this.article.CellValidating += (s, e) =>
            {
                if (this.EditMode && this.CurrentDetailData["Article"].EqualString(e.FormattedValue) == false)
                {
                    if (this.CurrentDetailData["Article"].Empty() == false && this.CheckRemoveRow() == false)
                    {
                        this.detailgrid.GetDataRow<DataRow>(e.RowIndex)["Article"] = this.CurrentDetailData["Article"];
                        return;
                    }

                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (MyUtility.Convert.GetString(e.FormattedValue) != MyUtility.Convert.GetString(dr["Article"]))
                    {
                        // 資料有異動過就先刪除SubDetail資料
                        this.DeleteSubDetailData(dr);
                        if (MyUtility.Check.Empty(e.FormattedValue))
                        {
                            dr["Article"] = string.Empty;
                            dr["Color"] = string.Empty;
                            dr["QAOutput"] = string.Empty;
                            dr.EndEdit();
                            return;
                        }

                        // sql參數
                        SqlParameter sp1 = new SqlParameter("@id", MyUtility.Convert.GetString(dr["OrderID"]));
                        SqlParameter sp2 = new SqlParameter("@article", MyUtility.Convert.GetString(e.FormattedValue));

                        IList<SqlParameter> cmds = new List<SqlParameter>();
                        cmds.Add(sp1);
                        cmds.Add(sp2);

                        DataTable colorData;
                        string sqlCmd = "select * from View_OrderFAColor where Id = @id and Article = @article";
                        DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out colorData);
                        if (!result || colorData.Rows.Count <= 0)
                        {
                            if (!result)
                            {
                                MyUtility.Msg.WarningBox("Sql connection fail!\r\n" + result.ToString());
                            }
                            else
                            {
                                MyUtility.Msg.WarningBox("Data not found!!!");
                            }

                            dr["Article"] = string.Empty;
                            dr["Color"] = string.Empty;
                            dr["QAOutput"] = string.Empty;
                            dr.EndEdit();
                            e.Cancel = true;
                            return;
                        }
                        else
                        {
                            dr["Article"] = MyUtility.Convert.GetString(e.FormattedValue);
                            dr["Color"] = colorData.Rows[0]["ColorID"];
                            dr["QAOutput"] = string.Empty;
                            dr.EndEdit();
                            this.CreateSubDetailDatas(dr);
                        }
                    }
                }
            };
            #endregion
            #region Prod. Output的Validatng
            this.inlineqty.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (MyUtility.Convert.GetInt(e.FormattedValue) != MyUtility.Convert.GetInt(dr["InlineQty"]))
                    {
                        dr["InlineQty"] = MyUtility.Convert.GetInt(e.FormattedValue);
                        this.CalculateDefectQty(dr);
                        dr.EndEdit();
                    }
                }
            };
            #endregion
            #region SewingReasonID右鍵開窗
            this.SewingReasonID.EditingMouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (this.EditMode)
                    {
                        // 取得正在異動的這筆Row
                        DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                        string whereForDQSCheck = string.Empty;
                        DualResult result;

                        if (MyUtility.Convert.GetString(this.CurrentMaintain["Shift"]) != "O")
                        {
                            if (!this.IsSameDQS(dr))
                            {
                                whereForDQSCheck = $@" and isnull(ForDQSCheck, 0) = 1";
                            }
                        }

                        // 查詢視窗資料來源
                        string sqlCmd = $"SELECT DISTINCT ID,Description FROM SewingReason WHERE Type='SO' AND isnull(Junk, 0) = 0 {whereForDQSCheck} -- SO代表SewingOutput";
                        DataTable reasonDatas;
                        result = DBProxy.Current.Select(null, sqlCmd, out reasonDatas);

                        // 寬度可以用逗號區隔開來
                        SelectItem item = new SelectItem(sqlCmd, "10,40", MyUtility.Convert.GetString(dr["SewingReasonID"]), headercaptions: "ID,Description");
                        DialogResult returnResult = item.ShowDialog();
                        if (returnResult == DialogResult.Cancel)
                        {
                            return;
                        }

                        string selectedID = item.GetSelectedString();
                        DataRow[] reasonData = reasonDatas.Select($"ID='{selectedID}'");
                        if (reasonData.Count() > 0)
                        {
                            dr["SewingReasonID"] = reasonData[0]["ID"].ToString();
                            dr["ReasonDescription"] = reasonData[0]["Description"].ToString();
                        }
                        else
                        {
                            dr["SewingReasonID"] = string.Empty;
                            dr["ReasonDescription"] = string.Empty;
                        }

                        dr.EndEdit();
                    }
                }
            };

            this.SewingReasonID.CellValidating += (s, e) =>
            {
                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    this.CurrentDetailData["SewingReasonID"] = string.Empty;
                    this.CurrentDetailData["ReasonDescription"] = string.Empty;
                    this.CurrentDetailData.EndEdit();
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                string whereForDQSCheck = string.Empty;
                DualResult result;

                if (MyUtility.Convert.GetString(this.CurrentMaintain["Shift"]) != "O")
                {
                    if (!this.IsSameDQS(dr))
                    {
                        whereForDQSCheck = $@" and isnull(ForDQSCheck, 0) = 1";
                    }
                }

                string sqlCmd = $"SELECT DISTINCT ID,Description FROM SewingReason WHERE Type='SO' AND Junk=0 AND ID ='{e.FormattedValue}' {whereForDQSCheck}  -- SO代表SewingOutput";
                DataTable reasonDatas;
                result = DBProxy.Current.Select(null, sqlCmd, out reasonDatas);
                if (reasonDatas.Rows.Count > 0)
                {
                    this.CurrentDetailData["SewingReasonID"] = reasonDatas.Rows[0]["ID"].ToString();
                    this.CurrentDetailData["ReasonDescription"] = reasonDatas.Rows[0]["Description"].ToString();
                }
                else
                {
                    MyUtility.Msg.WarningBox("Data not found!!!");
                    this.CurrentDetailData["SewingReasonID"] = string.Empty;
                    this.CurrentDetailData["ReasonDescription"] = string.Empty;
                    e.Cancel = true;
                }

                this.CurrentDetailData.EndEdit();
            };
            #endregion

            Ict.Win.UI.DataGridViewTextBoxColumn textArticleSetting;
            Ict.Win.UI.DataGridViewNumericBoxColumn numInLineQtySetting;
            Ict.Win.UI.DataGridViewNumericBoxColumn numWorkHourSetting;
            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("OrderID", header: "SP#", width: Widths.AnsiChars(13), settings: this.orderid).Get(out this.textOrderIDSetting)
                .Text("ComboType", header: "*", width: Widths.AnsiChars(1), iseditingreadonly: true, settings: this.combotype)
                .Text("Article", header: "Article", width: Widths.AnsiChars(8), settings: this.article).Get(out textArticleSetting)
                .Text("Color", header: "Color", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("QAOutput", header: "QA Output", width: Widths.AnsiChars(30), iseditingreadonly: true, settings: this.qaoutput)
                .Numeric("QAQty", header: "QA Ttl Output", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("InlineQty", header: "Prod. Output", width: Widths.AnsiChars(5), settings: this.inlineqty).Get(out numInLineQtySetting)
                .Numeric("DefectQty", header: "Defect Q’ty", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("WorkHour", header: "W’Hours", width: Widths.AnsiChars(5), decimal_places: 3, maximum: 999.999m, minimum: 0m).Get(out numWorkHourSetting)
                .Numeric("TMS", header: "TMS", width: Widths.AnsiChars(5), iseditingreadonly: true)

                // .Numeric("RFT", header: "RFT(%)", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("RFT", header: "RFT(%)", width: Widths.AnsiChars(7), iseditingreadonly: true)
                .Text("Tips", header: "Tips", width: Widths.AnsiChars(40), iseditingreadonly: true)
                .Text("Remark", header: "Remark", width: Widths.AnsiChars(40), iseditingreadonly: false)
                .Text("SewingReasonID", header: "Reason ID", width: Widths.AnsiChars(10), iseditingreadonly: false, settings: this.SewingReasonID)
                .Text("ReasonDescription", header: "Description", width: Widths.AnsiChars(40), iseditingreadonly: true)
                .CheckBox("AutoCreate", header: "Auto Create", trueValue: 1, falseValue: 0, iseditable: false);

            this.detailgrid.RowEnter += (s, e) =>
            {
                if (e.RowIndex < 0 || this.EditMode == false)
                {
                    return;
                }

                var data = ((DataRowView)this.detailgrid.Rows[e.RowIndex].DataBoundItem).Row;
                if (data == null)
                {
                    return;
                }

                bool isAutoCreate = data["AutoCreate"].EqualString("True");
                textArticleSetting.IsEditingReadOnly = isAutoCreate;
                numInLineQtySetting.IsEditingReadOnly = isAutoCreate;
                numWorkHourSetting.IsEditingReadOnly = isAutoCreate;

                this.DoSubForm.IsSupportDelete = !isAutoCreate;
                this.DoSubForm.IsSupportUpdate = !isAutoCreate;
                this.DoSubForm.IsSupportNew = !isAutoCreate;

                if (this.CheckSPEditable() == false)
                {
                    this.textOrderIDSetting.IsEditingReadOnly = true;
                }
                else
                {
                    this.textOrderIDSetting.IsEditingReadOnly = isAutoCreate;
                }
            };

            this.detailgrid.RowsAdded += (s, e) =>
            {
                if (this.EditMode == false || e.RowIndex < 0)
                {
                    return;
                }

                int index = e.RowIndex;
                for (int i = 0; i < e.RowCount; i++)
                {
                    DataGridViewRow dr = this.detailgrid.Rows[index];
                    bool isAutoCreate = dr.Cells["AutoCreate"].Value.EqualString("True");
                    if (this.CheckSPEditable() == false)
                    {
                        dr.Cells["OrderID"].Style.ForeColor = Color.Black;
                    }
                    else
                    {
                        dr.Cells["OrderID"].Style.ForeColor = isAutoCreate ? Color.Black : Color.Red;
                    }

                    dr.Cells["Article"].Style.ForeColor = isAutoCreate ? Color.Black : Color.Red;
                    dr.Cells["InlineQty"].Style.ForeColor = isAutoCreate ? Color.Black : Color.Red;
                    dr.Cells["WorkHour"].Style.ForeColor = isAutoCreate ? Color.Black : Color.Red;
                    index++;
                }
            };
        }

        /// <inheritdoc/>
        protected override void OnDetailGridInsert(int index = -1)
        {
            base.OnDetailGridInsert(index);

            DataTable dt = (DataTable)((BindingSource)this.detailgrid.DataSource).DataSource;
            string gridSort = dt.DefaultView.Sort;
            dt.DefaultView.Sort = string.Empty;

            if (index == -1)
            {
                this.CurrentDetailData["Rft"] = "0.00%";
                this.CurrentDetailData["AutoCreate"] = 0;
            }
            else
            {
                dt.Rows[index]["Rft"] = "0.00";
                dt.Rows[index]["AutoCreate"] = 0;
            }

            dt.DefaultView.Sort = gridSort;
        }

        /// <inheritdoc/>
        protected override DualResult ConvertSubDetailDatasFromDoSubForm(SubDetailConvertFromEventArgs e)
        {
            if (this.EditMode && this.DoSubForm.DialogResult == DialogResult.OK)
            {
                StringBuilder qAOutput = new StringBuilder();
                int qAQty = 0;

                foreach (DataRow dr in e.SubDetails.Rows)
                {
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        if (MyUtility.Convert.GetString(dr["SewingOutput_DetailUKey"]) == MyUtility.Convert.GetString(this.CurrentDetailData["UKey"]) && !MyUtility.Check.Empty(dr["QAQty"]))
                        {
                            qAOutput.Append(string.Format("{0}*{1},", MyUtility.Convert.GetString(dr["SizeCode"]), MyUtility.Convert.GetString(dr["QAQty"])));
                            qAQty = qAQty + MyUtility.Convert.GetInt(dr["QAQty"]);
                        }
                    }
                }

                e.Detail["QAOutput"] = qAOutput.Length > 0 ? qAOutput.ToString() : string.Empty;

                // 總計第三層 Qty 填入第二層 QAQty
                e.Detail["QAQty"] = qAQty;

                if (qAQty == 0)
                {
                    e.Detail["InlineQty"] = 0;
                }
                else
                {
                    e.Detail["InlineQty"] = e.Detail["RFT"].ToString().Substring(0, 4) == "0.00" ? qAQty : qAQty /
                        (decimal.Parse(e.Detail["RFT"].ToString().Substring(0, 4)) / 100);
                }

                this.CalculateDefectQty(e.Detail);

                // 總計第二層 Qty 填入第一層 QAQty
                this.CurrentMaintain["QAQty"] = ((DataTable)this.detailgridbs.DataSource).AsEnumerable().Where(row => row.RowState != DataRowState.Deleted
                                                                                                                     && row["AutoCreate"].EqualString("False")).CopyToDataTable().Compute("SUM(QAQty)", string.Empty);
                this.CurrentMaintain["InlineQty"] = MyUtility.Convert.GetInt(this.CurrentMaintain["QAQty"]) + MyUtility.Convert.GetInt(this.CurrentMaintain["DefectQty"]);
            }

            return base.ConvertSubDetailDatasFromDoSubForm(e);
        }

        private bool IsSameDQS(DataRow dr)
        {
            // 先看此筆是否有對應DQS OrderID,ComboType,Article
            string shift = this.CurrentMaintain["Shift"].EqualString("D") ? "Day" : this.CurrentMaintain["Shift"].EqualString("N") ? "Night" : string.Empty;
            string checkDQSexists = $@"
select 1
from inspection ins WITH (NOLOCK)
where InspectionDate= '{((DateTime)this.CurrentMaintain["OutputDate"]).ToString("d")}'
and FactoryID = '{this.CurrentMaintain["FactoryID"]}'
and Line = '{this.CurrentMaintain["SewingLineID"]}'
and Team = '{this.CurrentMaintain["Team"]}'
and Shift = '{shift}'
and SunriseNid = 0
";

            // 先判斷此表頭組合, 是否有任何一筆DQS, 若無則不用限制
            if (!MyUtility.Check.Seek(checkDQSexists, "ManufacturingExecution"))
            {
                return true;
            }

            checkDQSexists = $@"
select 1
from inspection ins WITH (NOLOCK)
where InspectionDate= '{((DateTime)this.CurrentMaintain["OutputDate"]).ToString("d")}'
and FactoryID = '{this.CurrentMaintain["FactoryID"]}'
and Line = '{this.CurrentMaintain["SewingLineID"]}'
and Team = '{this.CurrentMaintain["Team"]}'
and Shift = '{shift}'
and OrderID = '{dr["OrderID"]}'
and Location = '{dr["ComboType"]}'
and Article = '{dr["Article"]}'
and SunriseNid = 0
";
            bool hasDQS = MyUtility.Check.Seek(checkDQSexists, "ManufacturingExecution");

            // 第2層QAQty為0且有DQS
            if (hasDQS && MyUtility.Convert.GetDecimal(dr["QAQty"]) == 0)
            {
                return false;
            }

            // 第2層QAQty不為0
            if (MyUtility.Convert.GetDecimal(dr["QAQty"]) != 0)
            {
                // 第2層沒有對應DQS
                if (!hasDQS)
                {
                    return false;
                }
                else
                {
                    // 有對應DQS第2層才比較第3層是否一樣
                    DataTable subDetailData;
                    this.GetSubDetailDatas(dr, out subDetailData); // 取得此筆第3層

                    DataTable sewDt2;
                    DualResult result = this.GetDQSDataForDetail_Detail(dr, out sewDt2); // 取得DQS For 第3層
                    if (!result)
                    {
                        this.ShowErr(result);
                    }

                    // 判斷每筆DQS是否在現有第3層且Qty相等, 若無則SewingReason帶出資料限制
                    foreach (DataRow dqsRow_Size in sewDt2.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted))
                    {
                        if (!subDetailData.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted).Any(row =>
                            MyUtility.Convert.GetString(row["OrderID"]).EqualString(MyUtility.Convert.GetString(dqsRow_Size["OrderID"]))
                            && MyUtility.Convert.GetString(row["ComboType"]).EqualString(MyUtility.Convert.GetString(dqsRow_Size["ComboType"]))
                            && MyUtility.Convert.GetString(row["Article"]).EqualString(MyUtility.Convert.GetString(dqsRow_Size["Article"]))
                            && MyUtility.Convert.GetString(row["SizeCode"]).EqualString(MyUtility.Convert.GetString(dqsRow_Size["SizeCode"]))
                            && MyUtility.Convert.GetDecimal(row["QAQty"]).Equals(MyUtility.Convert.GetDecimal(dqsRow_Size["DQSQAQty"]))))
                        {
                            return false;
                        }
                    }

                    // 反過來檢查,主要是看表身有沒有多出來, DQS卻沒有的
                    foreach (DataRow row_Size in subDetailData.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted))
                    {
                        if (!sewDt2.AsEnumerable().Where(w => w.RowState != DataRowState.Deleted).Any(row =>
                            MyUtility.Convert.GetString(row["OrderID"]).EqualString(MyUtility.Convert.GetString(row_Size["OrderID"]))
                            && MyUtility.Convert.GetString(row["ComboType"]).EqualString(MyUtility.Convert.GetString(row_Size["ComboType"]))
                            && MyUtility.Convert.GetString(row["Article"]).EqualString(MyUtility.Convert.GetString(row_Size["Article"]))
                            && MyUtility.Convert.GetString(row["SizeCode"]).EqualString(MyUtility.Convert.GetString(row_Size["SizeCode"]))
                            && MyUtility.Convert.GetDecimal(row["DQSQAQty"]).Equals(MyUtility.Convert.GetDecimal(row_Size["QAQty"]))))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        // 計算表身Grid的TMS
        private decimal CalculateTMS(DataRow dr, decimal rate)
        {
            return MyUtility.Math.Round(MyUtility.Convert.GetDecimal(dr["CPU"]) * MyUtility.Convert.GetDecimal(dr["CPUFactor"]) * (rate / 100) * MyUtility.Convert.GetDecimal(dr["StdTMS"]), 0);
        }

        // 計算表身Grid的Defect Qty
        private void CalculateDefectQty(DataRow dr)
        {
            dr["DefectQty"] = MyUtility.Convert.GetInt(dr["InlineQty"]) - MyUtility.Convert.GetInt(dr["QAQty"]);
            this.CurrentMaintain["DefectQty"] = ((DataTable)this.detailgridbs.DataSource).Compute("SUM(DefectQty)", string.Empty);
        }

        // 撈取RFT值
        // Sewing P01 表身計算公式：SewingOutput_Detail.QAQty / SewingOutput_Detail.InlineQty
        private void GetRFT(DataRow dr)
        {
            if (MyUtility.Check.Empty(dr["QAQty"]) || MyUtility.Check.Empty(dr["InlineQty"]))
            {
                dr["RFT"] = "0.00%";
            }
            else
            {
                double qAqty = MyUtility.Convert.GetDouble(dr["QAQty"]);
                double inLineQty = MyUtility.Convert.GetDouble(dr["InlineQty"]);
                string rFT = MyUtility.Convert.GetString(Math.Round( qAqty / inLineQty * 100, 2)) + "%";
                dr["RFT"] = rFT;
            }

            dr.EndEdit();
            /*
            // sql參數
            SqlParameter sp1 = new SqlParameter("@orderid", MyUtility.Convert.GetString(dr["OrderID"]));
            SqlParameter sp2 = new SqlParameter("@cdate", MyUtility.Convert.GetDate(this.CurrentMaintain["OutputDate"]));
            SqlParameter sp3 = new SqlParameter("@sewinglineid", MyUtility.Convert.GetString(this.CurrentMaintain["SewingLineID"]));
            SqlParameter sp4 = new SqlParameter("@shift", MyUtility.Convert.GetString(this.CurrentMaintain["Shift"]));
            SqlParameter sp5 = new SqlParameter("@team", MyUtility.Convert.GetString(this.CurrentMaintain["Team"]));

            IList<SqlParameter> cmds = new List<SqlParameter>();
            cmds.Add(sp1);
            cmds.Add(sp2);
            cmds.Add(sp3);
            cmds.Add(sp4);
            cmds.Add(sp5);

            string sqlCmd = @"select iif(rft.InspectQty is null or rft.InspectQty = 0,'0.00%', CONVERT(VARCHAR, convert(Decimal(5,2), round((rft.InspectQty-rft.RejectQty)/rft.InspectQty*100,2) )) + '%') as RFT
from RFT WITH (NOLOCK) 
where OrderID = @orderid
and CDate = @cdate
and SewinglineID = @sewinglineid
and Shift = @shift
and Team = @team";

            DataTable rFTData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out rFTData);
            if (result && rFTData.Rows.Count > 0)
            {
                dr["RFT"] = rFTData.Rows[0]["RFT"];
            }
            else
            {
                dr["RFT"] = "0.00%";
            }

            dr.EndEdit();*/
        }

        // 刪除SubDetail資料
        private void DeleteSubDetailData(DataRow dr)
        {
            DataTable subDetailData;
            this.GetSubDetailDatas(dr, out subDetailData);
            foreach (DataRow ddr in subDetailData.Rows)
            {
                ddr["QAQty"] = 0;
            }

            dr["QAQty"] = 0;
            dr["InlineQty"] = 0;
            dr["DefectQty"] = 0;
        }

        // 產生SubDetail資料
        private void CreateSubDetailDatas(DataRow dr)
        {
            if (MyUtility.Check.Empty(dr["ComboType"]) || MyUtility.Check.Empty(dr["OrderID"]) || MyUtility.Check.Empty(dr["Article"]))
            {
                return;
            }

            // sql參數
            SqlParameter sp1 = new SqlParameter("@ukey", dr["UKey"]);
            SqlParameter sp2 = new SqlParameter("@combotype", MyUtility.Convert.GetString(dr["ComboType"]));
            SqlParameter sp3 = new SqlParameter("@orderid", MyUtility.Convert.GetString(dr["OrderID"]));
            SqlParameter sp4 = new SqlParameter("@article", MyUtility.Convert.GetString(dr["Article"]));

            IList<SqlParameter> cmds = new List<SqlParameter>();
            cmds.Add(sp1);
            cmds.Add(sp2);
            cmds.Add(sp3);
            cmds.Add(sp4);

            string sqlCmd = string.Format(
                @"
with AllQty as (
    select '{0}' as ID
           , @ukey as SewingOutput_DetailUkey
           , oq.ID as OrderId
           , @combotype  as ComboType
           , oq.Article
           , oq.SizeCode
           , oq.Qty as OrderQty
           , 0 as QAQty 
           , AccumQty = isnull((select sum(QAQty) 
                                from SewingOutput_Detail_Detail WITH (NOLOCK) 
                                where OrderId = oq.ID 
                                      and ComboType = @combotype 
                                      and Article = oq.Article 
                                      and SizeCode = oq.SizeCode
                                      and ID != '{0}'), 0) 
    from Order_Qty oq WITH (NOLOCK) 
    where oq.ID = @orderid
          and oq.Article = @article
)
select a.*
       , OrderQty.OrderQtyUpperlimit
       , a.OrderQty - a.AccumQty as Variance
       , a.OrderQty - a.AccumQty - a.QAQty as BalQty
       , isnull(os.Seq,0) as Seq
       , OldDetailKey = ''
from AllQty a
left join Orders o WITH (NOLOCK) on a.OrderId = o.ID
left join Order_SizeCode os WITH (NOLOCK) on os.Id = o.POID 
    and os.SizeCode = a.SizeCode
outer apply(
	select value=1
	from Order_TmsCost ot with(nolock)
	inner join Order_Qty oq WITH (NOLOCK) on ot.id = oq.ID
	where ot.ArtworkTypeID = 'Garment Dye' and ot.Price > 0
	and oq.SizeCode=os.SizeCode and oq.Article=a.Article and ot.id=o.id
	and o.LocalOrder<>1
)b
outer apply(select OrderQtyUpperlimit=iif(b.value is not null,round(cast(a.OrderQty as decimal)* (1+ isnull(o.DyeingLoss,0)/100),0),a.OrderQty))OrderQty
order by a.OrderId,os.Seq",
                this.CurrentMaintain["ID"]);

            DataTable orderQtyData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out orderQtyData);
            if (result && orderQtyData.Rows.Count > 0)
            {
                DataTable subDetailData;
                this.GetSubDetailDatas(dr, out subDetailData);
                foreach (DataRow ddr in orderQtyData.Rows)
                {
                    if (!subDetailData.AsEnumerable().Any(row => row["ID"].EqualString(ddr["ID"])
                                                                && row["SewingOutput_DetailUkey"].EqualString(ddr["SewingOutput_DetailUkey"])
                                                                && row["OrderID"].EqualString(ddr["OrderID"])
                                                                && row["ComboType"].EqualString(ddr["ComboType"])
                                                                && row["Article"].EqualString(ddr["Article"])
                                                                && row["SizeCode"].EqualString(ddr["SizeCode"])))
                    {
                        DataRow newDr = subDetailData.NewRow();
                        for (int i = 0; i < subDetailData.Columns.Count; i++)
                        {
                            newDr[subDetailData.Columns[i].ColumnName] = ddr[subDetailData.Columns[i].ColumnName];
                        }

                        subDetailData.Rows.Add(newDr);
                    }
                }
            }
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["Category"] = "O";
            this.CurrentMaintain["FactoryID"] = Env.User.Factory;
            this.CurrentMaintain["OutputDate"] = DateTime.Today.AddDays(-1);
            this.CurrentMaintain["Shift"] = "D";
            this.CurrentMaintain["Team"] = "A";
            this.CurrentDetailData["AutoCreate"] = 0;
            this.CurrentDetailData["RFT"] = "0.00%";
            this.CurrentMaintain["MDivisionID"] = Env.User.Keyword;
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            if (!MyUtility.Check.Empty(this.CurrentMaintain["LockDate"]) && !MyUtility.Convert.GetString(this.CurrentMaintain["Status"]).EqualString("Sent"))
            {
                MyUtility.Msg.WarningBox("This record already locked, can't modify.");
                return false;
            }

            return base.ClickEditBefore();
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.dateDate.ReadOnly = true;
            this.txtsewinglineLine.ReadOnly = true;
            DateTime? sewingMonthlyLockDate = MyUtility.Convert.GetDate(MyUtility.GetValue.Lookup($"select LockDate from SewingMonthlyLock where FactoryID = '{this.CurrentMaintain["FactoryID"]}'"));
            if (MyUtility.Convert.GetDate(this.CurrentMaintain["OutputDate"]) <= sewingMonthlyLockDate)
            {
                this.txtdropdownlistShift.ReadOnly = true;
                this.comboTeam.ReadOnly = true;
                this.numManpower.ReadOnly = true;
                this.numWHours.ReadOnly = true;
                this.txtSubconOutFty.TextBox1.ReadOnly = true;
                this.txtSubConOutContractNumber.ReadOnly = true;
            }

            if (this.CheckSPEditable() == false)
            {
                this.textOrderIDSetting.IsEditingReadOnly = true;
                this.gridicon.Enabled = false;
            }
            else
            {
                this.textOrderIDSetting.IsEditingReadOnly = false;
                this.gridicon.Enabled = true;
            }

            this.txtSubConOutContractNumber.ReadOnly = this.txtSubconOutFty.TextBox1.ReadOnly;
        }

        /// <inheritdoc/>
        protected override bool ClickDeleteBefore()
        {
            DualResult result;
            if (!MyUtility.Check.Empty(this.CurrentMaintain["LockDate"]))
            {
                MyUtility.Msg.WarningBox("This record already locked, can't delete.");
                return false;
            }

            DateTime? sewingMonthlyLockDate = MyUtility.Convert.GetDate(MyUtility.GetValue.Lookup($"select LockDate from SewingMonthlyLock where FactoryID = '{this.CurrentMaintain["FactoryID"]}'"));
            if (MyUtility.Convert.GetDate(this.CurrentMaintain["OutputDate"]) <= sewingMonthlyLockDate)
            {
                MyUtility.Msg.WarningBox("The date earlier than Sewing Lock Date, can't delete.");
                return false;
            }

            #region 若表身擁有 AutoCreate 的資料，則不可刪除
            if (((DataTable)this.detailgridbs.DataSource).AsEnumerable().Any(row => row.RowState != DataRowState.Deleted && row["AutoCreate"].EqualString("True")))
            {
                MyUtility.Msg.WarningBox("Can't delete autocreate Item.");
                return false;
            }
            #endregion

            #region 刪除的數量不能小於已出貨的數量
            DataTable dtSubDetail = null;
            foreach (DataRow dr in this.DetailDatas)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    DataTable subDt;
                    this.GetSubDetailDatas(dr, out subDt);
                    subDt = subDt.AsEnumerable().Where(row => true).CopyToDataTable();
                    subDt.Columns.Add("AutoCreate");
                    if (dtSubDetail == null)
                    {
                        dtSubDetail = subDt.Clone();
                    }

                    foreach (DataRow subDr in subDt.Rows)
                    {
                        subDr["AutoCreate"] = dr["AutoCreate"];
                        dtSubDetail.ImportRow(subDr);
                    }
                }
            }

            if (this.SaveDeleteCheckPacking(dtSubDetail, false) == false)
            {
                return false;
            }
            #endregion

            // 第3層SewingOutput_Detail_Detail刪除,以當前SewingOutput_DetailUKey為條件
            string sqlcmdD = "Delete SewingOutput_Detail_Detail where SewingOutput_DetailUKey = @K";
            List<SqlParameter> ps = new List<SqlParameter>();
            ps.Add(new SqlParameter("@K", this.CurrentDetailData["Ukey"]));
            if (!(result = DBProxy.Current.Execute(null, sqlcmdD, ps)))
            {
                this.ShowErr(result);
            }

            return base.ClickDeleteBefore();
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            #region 檢查不可為空值
            if (MyUtility.Check.Empty(this.CurrentMaintain["OutputDate"]))
            {
                this.dateDate.Focus();
                MyUtility.Msg.WarningBox("Date can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["SewingLineID"]))
            {
                this.txtsewinglineLine.Focus();
                MyUtility.Msg.WarningBox("Line# can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Shift"]))
            {
                this.txtdropdownlistShift.Focus();
                MyUtility.Msg.WarningBox("Shift can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Team"]))
            {
                this.comboTeam.Focus();
                MyUtility.Msg.WarningBox("Team can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Manpower"]))
            {
                this.numManpower.Focus();
                MyUtility.Msg.WarningBox("Manpower can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["WorkHour"]))
            {
                this.numWHours.Focus();
                MyUtility.Msg.WarningBox("W/Hours(Day) can't empty!!");
                return false;
            }

            if (this.CurrentMaintain["Shift"].Equals("O") && MyUtility.Check.Empty(this.CurrentMaintain["SubconOutFty"]))
            {
                this.txtSubconOutFty.Focus();
                MyUtility.Msg.WarningBox("Subcon-Out-Fty can't empty!!");
                return false;
            }

            if (!MyUtility.Check.Seek($"select 1 from dbo.SCIFty with (nolock) where ID = '{this.CurrentMaintain["SubconOutFty"]}'") &&
                this.CurrentMaintain["Shift"].Equals("O") &&
                !MyUtility.Check.Empty(this.CurrentMaintain["SubconOutFty"]) &&
                MyUtility.Check.Empty(this.CurrentMaintain["SubConOutContractNumber"]))
            {
                this.txtSubConOutContractNumber.Focus();
                MyUtility.Msg.WarningBox("SubCon-Out Contract Number can't empty!!");
                return false;
            }
            #endregion

            #region 檢查QA Ttl Output是否為0 和Reason ID 是否存在

            foreach (DataGridViewRow item in this.detailgrid.Rows)
            {
                if (item.Cells["QAQty"].Value.ToString() == "0" && string.IsNullOrEmpty(item.Cells["SewingReasonID"].Value.ToString()))
                {
                    MyUtility.Msg.WarningBox("Please input [Reason] if [Qa Ttl Output] is 0!");
                    return false;
                }

                string strReasonID = item.Cells["SewingReasonID"].Value.ToString();
                if (!MyUtility.Check.Empty(strReasonID) &&
                    !MyUtility.Check.Seek($@"select 1 from SewingReason where id = '{strReasonID}' and Type='SO' AND Junk=0"))
                {
                    MyUtility.Msg.WarningBox($@"<Reason ID: {strReasonID}> not found");
                    return false;
                }
            }
            #endregion

            #region 檢查表身不為0的資料, 是否與DQS相同， 若不同必須輸入SewingReason
            if (MyUtility.Convert.GetString(this.CurrentMaintain["Shift"]) != "O")
            {
                List<string> msg = new List<string>();
                foreach (DataRow dr in ((DataTable)this.detailgridbs.DataSource).AsEnumerable().Where(w => w.RowState != DataRowState.Deleted))
                {
                    string sqlChksewingReason = $@"SELECT DISTINCT ID,Description FROM SewingReason WHERE Type='SO' AND isnull(Junk, 0) = 0 and ForDQSCheck = 1 ";
                    if (!this.IsSameDQS(dr))
                    {
                        if (MyUtility.Check.Empty(dr["SewingReasonID"]) || !MyUtility.Check.Seek(sqlChksewingReason + $"and ID = '{dr["SewingReasonID"]}'"))
                        {
                            msg.Add($"Order: <{dr["OrderID"]}> ComboType: <{dr["ComboType"]}> Article: <{dr["Article"]}> ");
                            continue;
                        }
                    }
                }

                if (msg.Count > 0)
                {
                    var x = msg.Distinct().ToList();
                    MyUtility.Msg.WarningBox("Please Input Reason if Output data not equal to DQS output data !\r\n" + string.Join("\r\n", x));
                    return false;
                }
            }
            #endregion

            this.CalculateManHour();

            #region 新增時檢查Date不可早於Sewing Lock Date
            if (this.IsDetailInserting)
            {
                DateTime? sewingMonthlyLockDate = MyUtility.Convert.GetDate(MyUtility.GetValue.Lookup($"select LockDate from SewingMonthlyLock where FactoryID = '{this.CurrentMaintain["FactoryID"]}'"));
                if (MyUtility.Convert.GetDate(this.CurrentMaintain["OutputDate"]) <= sewingMonthlyLockDate)
                {
                    this.dateDate.Focus();
                    MyUtility.Msg.WarningBox(string.Format("Date can't earlier than Sewing Lock Date: {0}.", Convert.ToDateTime(sewingMonthlyLockDate).ToString(string.Format("{0}", Env.Cfg.DateStringFormat))));
                    return false;
                }
            }
            #endregion

            #region 檢查資料是否已存在
            if (MyUtility.Check.Seek(string.Format(@"select ID from SewingOutput WITH (NOLOCK) where OutputDate = '{0}' and SewingLineID = '{1}' and Shift = '{2}' and Team = '{3}' and FactoryID = '{4}' and ID <> '{5}' and SubconOutFty = '{6}' and SubConOutContractNumber ='{7}' and Category = 'O'", Convert.ToDateTime(this.CurrentMaintain["OutputDate"]).ToString("d"), MyUtility.Convert.GetString(this.CurrentMaintain["SewingLineID"]), MyUtility.Convert.GetString(this.CurrentMaintain["Shift"]), MyUtility.Convert.GetString(this.CurrentMaintain["Team"]), MyUtility.Convert.GetString(this.CurrentMaintain["FactoryID"]), MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), MyUtility.Convert.GetString(this.CurrentMaintain["SubconOutFty"]), MyUtility.Convert.GetString(this.CurrentMaintain["SubConOutContractNumber"]))))
            {
                MyUtility.Msg.WarningBox(string.Format(
                    "Date:{0}, Line:{1}, Shift:{2}, Team:{3},SubconOutFty:{4},SubConOutContractNumber:{5} already exist, can't save!!",
                    Convert.ToDateTime(this.CurrentMaintain["OutputDate"]).ToString(string.Format("{0}", Env.Cfg.DateStringFormat)),
                    MyUtility.Convert.GetString(this.CurrentMaintain["SewingLineID"]),
                    MyUtility.Convert.GetString(this.CurrentMaintain["Shift"]),
                    MyUtility.Convert.GetString(this.CurrentMaintain["Team"]),
                    MyUtility.Convert.GetString(this.CurrentMaintain["FactoryID"]),
                    MyUtility.Convert.GetString(this.CurrentMaintain["SubconOutFty"]),
                    MyUtility.Convert.GetString(this.CurrentMaintain["SubConOutContractNumber"])));

                return false;
            }
            #endregion

            #region 先撈出所有SP的SewingSchedule資料
            DataTable sewingData;
            try
            {
                MyUtility.Tool.ProcessWithDatatable(
                    (DataTable)this.detailgridbs.DataSource,
                    "OrderID,ComboType",
                    string.Format(
                        @"select a.OrderId,a.ComboType,s.StandardOutput 
from #tmp a, SewingSchedule s WITH (NOLOCK) 
where a.OrderId = s.OrderID
and a.ComboType = s.ComboType
and s.SewingLineID = '{0}'",
                        MyUtility.Convert.GetString(this.CurrentMaintain["SewingLineID"])),
                    out sewingData,
                    "#tmp");
            }
            catch (Exception ex)
            {
                this.ShowErr("Calculate error.", ex);
                return false;
            }
            #endregion

            #region 先算出QAQty,InLineQty,DefectQty,W/Hours
            DataTable sumQty;
            int gridQaQty, gridInlineQty, gridDefectQty; // 加總表身的QAQty,InLineQty,DefectQty
            decimal gridWHours = 0; // 加總表身W/Hours
            try
            {
                string strSumQty = @"
select isnull(sum(WorkHour),0) as sumWorkHour
       , isnull(sum(QAQty),0) as sumQaqty
       , isnull(sum(InlineQty),0) as sumInlineQty
       , isnull(sum(DefectQty),0) as sumDefectQty 
from #tmp 
where (OrderID <> '' or OrderID is not null) 
      and (ComboType <> '' or ComboType is not null) 
      and (Article <> '' or Article is not null)
      and Convert (bit, AutoCreate) != 1";
                MyUtility.Tool.ProcessWithDatatable((DataTable)this.detailgridbs.DataSource, "WorkHour,QAQty,InlineQty,DefectQty,OrderID,ComboType,Article,AutoCreate", strSumQty, out sumQty, "#tmp");
            }
            catch (Exception ex)
            {
                this.ShowErr("Calculate error.", ex);
                return false;
            }

            if (sumQty == null)
            {
                gridQaQty = 0;
                gridInlineQty = 0;
                gridDefectQty = 0;
                gridWHours = 0;
            }
            else
            {
                gridQaQty = MyUtility.Convert.GetInt(sumQty.Rows[0]["sumQAQty"]);
                gridInlineQty = MyUtility.Convert.GetInt(sumQty.Rows[0]["sumInlineQty"]);
                gridDefectQty = MyUtility.Convert.GetInt(sumQty.Rows[0]["sumDefectQty"]);
                gridWHours = MyUtility.Convert.GetDecimal(sumQty.Rows[0]["sumWorkHour"]);
            }
            #endregion

            int recCnt = 0; // 紀錄表身record數
            decimal gridTms = 0; // 加總表身的TMS
            #region 刪除表身資料(OrderID, ComboType, Article)
            foreach (DataRow dr in this.DetailDatas)
            {
                if (MyUtility.Check.Empty(dr["OrderID"]) || MyUtility.Check.Empty(dr["Article"]))
                {
                    dr.Delete();

                    // if (dr.RowState == DataRowState.Deleted) { continue; }
                    continue;
                }

                if (MyUtility.Check.Empty(dr["ComboType"]))
                {
                    MyUtility.Msg.WarningBox("ComboType(*) can't empty!!");
                    return false;
                }

                // 回寫表頭TMS
                if (gridQaQty == 0)
                {
                    gridTms = 0;
                }
                else
                {
                    if (dr["AutoCreate"].EqualString("False"))
                    {
                        gridTms = gridTms + (MyUtility.Convert.GetDecimal(dr["TMS"]) * MyUtility.Convert.GetDecimal(dr["QAQty"]) / MyUtility.Convert.GetDecimal(gridQaQty));
                    }
                }

                recCnt += 1;

                // 填入HourlyStandardOutput
                DataRow[] sewing = sewingData.Select(string.Format("OrderID = '{0}' and ComboType = '{1}'", MyUtility.Convert.GetString(dr["OrderID"]), MyUtility.Convert.GetString(dr["ComboType"])));
                if (sewing.Length > 0)
                {
                    dr["HourlyStandardOutput"] = sewing[0]["StandardOutput"];
                }
                else
                {
                    dr["HourlyStandardOutput"] = 0;
                }
            }
            #endregion

            #region 表身資料不可為空
            if (recCnt == 0)
            {
                this.detailgrid.Focus();
                MyUtility.Msg.WarningBox("< Detail > can't be empty!");
                return false;
            }
            #endregion

            #region 表身W/Hours加總要等於表頭的W/Hours(Day)
            if (gridWHours != MyUtility.Convert.GetDecimal(this.CurrentMaintain["WorkHour"]))
            {
                MyUtility.Msg.WarningBox("The working hours summary is not equal to working hours/day, please correct, or else can't be saved.");
                return false;
            }
            #endregion

            #region 若sewingoutput.outputDate <= SewingMonthlyLock.LockDate 表身Qty要等於表頭的Qty [月結]
            DateTime? sod = MyUtility.Convert.GetDate(this.CurrentMaintain["outputDate"]);
            DateTime? sl = MyUtility.Convert.GetDate(MyUtility.GetValue.Lookup($"select LockDate from SewingMonthlyLock where FactoryID = '{this.CurrentMaintain["FactoryID"]}'"));
            if (sod <= sl)
            {
                decimal nQ = 0;
                foreach (DataRow dr in this.DetailDatas)
                {
                    if (!MyUtility.Check.Empty(dr["QAQty"]) && dr["AutoCreate"].EqualString("False"))
                    {
                        nQ += MyUtility.Convert.GetDecimal(dr["QAQty"]);
                    }
                }

                if (nQ != this.oldttlqaqty)
                {
                    MyUtility.Msg.WarningBox("QA Output shouled be the same as before.");
                    return false;
                }

                int delDetailDatasCount = ((DataTable)this.detailgridbs.DataSource).AsEnumerable().Where(x => x.RowState == DataRowState.Deleted).ToList().Count();
                if (delDetailDatasCount > 0)
                {
                    MyUtility.Msg.WarningBox("Cannot remove SP after unconfirm. Please update QA Qty to zero manually.");
                    return false;
                }
            }
            #endregion

            #region 若status = Sent 表身Qty要等於表頭的Qty 且 Manhours不變 [日結]
            if (MyUtility.Convert.GetString(this.CurrentMaintain["Status"]).EqualString("Sent"))
            {
                decimal nQ = 0;
                foreach (DataRow dr in this.DetailDatas)
                {
                    if (!MyUtility.Check.Empty(dr["QAQty"]) && dr["AutoCreate"].EqualString("False"))
                    {
                        nQ += MyUtility.Convert.GetDecimal(dr["QAQty"]);
                    }
                }

                if (nQ != this.oldttlqaqty)
                {
                    MyUtility.Msg.WarningBox("The reocord is already lock so [QA Output]、[Manhours] can not modify!");
                    return false;
                }

                if (MyUtility.Convert.GetDecimal(this.CurrentMaintain["ManHour"]) != this.oldManHour)
                {
                    MyUtility.Msg.WarningBox("The reocord is already lock so [QA Output]、[Manhours] can not modify!");
                    return false;
                }
            }
            #endregion

            DataTable dtSubDetail = null;
            foreach (DataRow dr in this.DetailDatas)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    DataTable subDt;
                    this.GetSubDetailDatas(dr, out subDt);
                    subDt = subDt.AsEnumerable().Where(row => true).CopyToDataTable();
                    subDt.Columns.Add("AutoCreate");
                    if (dtSubDetail == null)
                    {
                        dtSubDetail = subDt.Clone();
                    }

                    foreach (DataRow subDr in subDt.Rows)
                    {
                        subDr["AutoCreate"] = dr["AutoCreate"];
                        dtSubDetail.ImportRow(subDr);
                    }
                }
            }

            #region 產出數量不能小於已出貨的數量
            if (this.SaveDeleteCheckPacking(dtSubDetail, true) == false)
            {
                return false;
            }
            #endregion

            #region 確認子單數量加總不會超過母單數量
            DualResult resultCheckSubOutputQty;
            DataTable dtCheckSubOutputQty;
            string strCheckSubOutputQty = $@"
select #tmp.*
into #Mother
from #tmp
where Convert (bit, #tmp.AutoCreate) = 0

select soddG.ComboType
       , soddG.Article
       , soddG.SizeCode
       , soddG.OrderIDFrom
       , QaQty = sum (soddG.QaQty)
into #Child
from SewingOutput_Detail_Detail_Garment soddG
where soddG.ID = '{this.CurrentMaintain["ID"]}'
group by soddG.ComboType, soddG.Article, soddG.SizeCode, soddG.OrderIDFrom

select Child.*
	   , MotherQaQty = isnull (Mother.QaQty, 0)
from #Child Child
left join #Mother Mother on Child.OrderIDFrom = Mother.OrderID
                            and Child.ComboType = Mother.ComboType
			                and Child.Article = Mother.Article
			                and Child.SizeCode = Mother.SizeCode
where Child.QaQty > isnull (Mother.QaQty, 0)";

            resultCheckSubOutputQty = MyUtility.Tool.ProcessWithDatatable(dtSubDetail, null, strCheckSubOutputQty, out dtCheckSubOutputQty);

            if (resultCheckSubOutputQty == false)
            {
                MyUtility.Msg.WarningBox(resultCheckSubOutputQty.ToString());
                return false;
            }
            else if (dtCheckSubOutputQty != null && dtCheckSubOutputQty.Rows.Count > 0)
            {
                StringBuilder errMsg = new StringBuilder();
                for (int i = 0; i < dtCheckSubOutputQty.Rows.Count; i++)
                {
                    errMsg.Append(string.Format(
                        @"
FromSP ComboType: <{0}> Article: <{1}> Size: <{2}> 
QAQty: <{3}>  less than AutoCreate Items QAQty: <{4}>",
                        dtCheckSubOutputQty.Rows[i]["ComboType"].ToString(),
                        dtCheckSubOutputQty.Rows[i]["Article"].ToString(),
                        dtCheckSubOutputQty.Rows[i]["SizeCode"].ToString(),
                        dtCheckSubOutputQty.Rows[i]["MotherQaQty"].ToString(),
                        dtCheckSubOutputQty.Rows[i]["QaQty"].ToString()));
                }

                if (errMsg.ToString().Empty() == false)
                {
                    MyUtility.Msg.WarningBox(errMsg.ToString());
                    return false;
                }
            }
            #endregion

            #region 檢查報的SP,Compotype,article在此合約書是否超額
            if (!MyUtility.Check.Empty(this.CurrentMaintain["SubConOutContractNumber"]) &&
                !MyUtility.Check.Seek($"select 1 from dbo.SCIFty with (nolock) where ID = '{this.CurrentMaintain["SubconOutFty"]}'"))
            {
                foreach (DataRow dr in this.DetailDatas)
                {
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        DataRow outputDr;
                        string chkContractQty = $@"
select 
[SewingOutputQty] = isnull(sum(sod.QAQty), 0) ,
[SubconOutContractQty] = isnull((select sd.OutputQty 
from dbo.SubconOutContract_Detail sd with (nolock) 
inner join dbo.SubconOutContract s with (nolock) on s.SubConOutFty = sd.SubConOutFty and s.ContractNumber = sd.ContractNumber and s.status = 'Confirmed'
where 
sd.SubconOutFty = '{this.CurrentMaintain["SubConOutFty"]}'  and
sd.ContractNumber = '{this.CurrentMaintain["SubConOutContractNumber"]}' and
sd.OrderID = '{dr["OrderID"]}' and
sd.Article = '{dr["Article"]}' and
sd.Combotype = '{dr["ComboType"]}'
),0)
    from SewingOutput s with(nolock)
    inner join SewingOutput_Detail sod with(nolock) on s.ID = sod.ID
    where s.SubConOutContractNumber = '{this.CurrentMaintain["SubConOutContractNumber"]}' and
            s.SubconOutFty = '{this.CurrentMaintain["SubConOutFty"]}'  and
            sod.OrderID = '{dr["OrderID"]}' and
            sod.Article = '{dr["Article"]}' and
            sod.Combotype = '{dr["ComboType"]}' and
            s.ID <> '{this.CurrentDetailData["ID"]}'";

                        if (MyUtility.Check.Seek(chkContractQty, out outputDr))
                        {
                            int sewingOutputQty = (int)outputDr["SewingOutputQty"] + (int)dr["QAQty"];
                            int subconOutContractQty = (int)outputDr["SubconOutContractQty"];
                            if (sewingOutputQty > subconOutContractQty)
                            {
                                MyUtility.Msg.WarningBox($@"Sewing Output Qty({sewingOutputQty}) can't more than SubconOut Contract Qty({subconOutContractQty})!!
<SubConOutContractNumber> '{this.CurrentMaintain["SubConOutContractNumber"]}'
<SubconOutFty> '{this.CurrentMaintain["SubConOutFty"]}'
<OrderID> '{dr["OrderID"]}'
<Article> '{dr["Article"]}'
<Combotype> '{dr["ComboType"]}'
");
                                return false;
                            }
                        }
                        else
                        {
                            MyUtility.Msg.WarningBox("Check Contract Output Qty Failure!!");
                            return false;
                        }
                    }
                }
            }
            #endregion

            #region 若SP為(Orders.junk=1 and Orders.NeedProduction=1)需檢查"已報產出的數量"+"此次報的數量"+"已轉出的數量"是否超過原訂單數量
            string checkOrderNeedProduction = $@"
select
t.OrderID,
t.Article,
t.SizeCode,
t.Combotype,
t.QAQty,
[OrderQty] = oq.Qty,
[OtherSewingOutputQty] = isnull(sum(isnull(sodd.QAQty, 0)), 0)  ,
[TransOutQty] = isnull(sum(isnull(sotd.TransferQty, 0)), 0)
into #checkResult
from #tmp t
inner join Order_Qty oq with (nolock) on oq.ID = t.OrderID and oq.Article = t.Article and oq.SizeCode = t.SizeCode
left join SewingOutput_detail_detail sodd with (nolock) on   sodd.OrderID = t.OrderID and
                                                                sodd.Article = t.Article and
                                                                sodd.SizeCode = t.SizeCode and
                                                                sodd.Combotype = t.Combotype and
                                                                sodd.ID <> isnull(t.ID, '')
left join SewingOutputTransfer_detail sotd with (nolock) on  sotd.FromOrderID = t.OrderID and
                                                             sotd.Article = t.Article and
                                                             sotd.SizeCode = t.SizeCode and
                                                             sotd.FromComboType = t.Combotype
where exists( select 1 from Orders o with (nolock) 
                        where o.ID = t.OrderID and 
                              o.junk = 1 and o.NeedProduction = 1)
group by    t.OrderID,
            t.Article,
            t.SizeCode,
            t.Combotype,
            t.QAQty,
            oq.Qty

select  OrderID,
        Article,
        SizeCode,
        Combotype,
        [SewQty] = QAQty + OtherSewingOutputQty,
        OrderQty,
        TransOutQty
from #checkResult 
where   OrderQty < (QAQty + OtherSewingOutputQty + TransOutQty)

";
            DataTable dtNeedProductionOver;
            DualResult result = MyUtility.Tool.ProcessWithDatatable(dtSubDetail, string.Empty, checkOrderNeedProduction, out dtNeedProductionOver);
            if (!result)
            {
                this.ShowErr(result);
                return false;
            }

            if (dtNeedProductionOver.Rows.Count > 0)
            {
                string overDataMsg = "SewingOutput Qty + transfered qty can't more than order qty：" + Environment.NewLine +
                    dtNeedProductionOver.AsEnumerable()
                    .Select(s => $@"[SP#：{s["OrderID"].ToString()},ComboType：{s["Combotype"].ToString()}, Article：{s["Article"].ToString()},Size：{s["SizeCode"].ToString()} ]Order Qty {s["OrderQty"].ToString()} , Sew. Qty= {s["SewQty"].ToString()} ,Transfered Qty ={s["TransOutQty"].ToString()}].")
                    .JoinToString(Environment.NewLine);

                MyUtility.Msg.WarningBox(overDataMsg);
                return false;
            }
            #endregion

            #region GetID
            if (this.IsDetailInserting)
            {
                string id = MyUtility.GetValue.GetID(MyUtility.GetValue.Lookup("FtyGroup", Env.User.Factory, "Factory", "ID") + "SM", "SewingOutput", DateTime.Today, 3, "Id", null);
                if (MyUtility.Check.Empty(id))
                {
                    MyUtility.Msg.WarningBox("GetID fail, please try again!");
                    return false;
                }

                this.CurrentMaintain["ID"] = id;
            }
            #endregion

            this.CurrentMaintain["QAQty"] = gridQaQty;
            this.CurrentMaintain["InlineQty"] = gridInlineQty;
            this.CurrentMaintain["DefectQty"] = gridDefectQty;
            this.CurrentMaintain["TMS"] = MyUtility.Math.Round(gridTms, 0);
            this.CurrentMaintain["Efficiency"] = MyUtility.Convert.GetDecimal(this.CurrentMaintain["TMS"]) * MyUtility.Convert.GetDecimal(this.CurrentMaintain["ManHour"]) == 0 ? 0 : MyUtility.Convert.GetDecimal(gridQaQty) / (3600 / MyUtility.Convert.GetDecimal(this.CurrentMaintain["TMS"]) * MyUtility.Convert.GetDecimal(this.CurrentMaintain["ManHour"])) * 100;
            return base.ClickSaveBefore();
        }

        protected override DualResult ClickSavePost()
        {
            #region 檢查是否有SewingOutput_Detail_Detail沒存到
            string sqlCheckSubDetail = $@"select OrderID
from SewingOutput_Detail sod
where not exists (select 1 
				  from SewingOutput_Detail_Detail sodd
				  where sodd.ID = sod.ID
						and sodd.SewingOutput_DetailUKey = sod.UKey
						and sodd.OrderId = sod.OrderId
						and sodd.ComboType = sod.ComboType
						and sodd.Article = sod.Article)
      and sod.id = '{this.CurrentMaintain["ID"]}' and sod.QAQty > 0";
            DataTable dtLoseSubDetail;
            DualResult result = DBProxy.Current.Select(null, sqlCheckSubDetail, out dtLoseSubDetail);

            if (!result)
            {
                return result;
            }

            if (dtLoseSubDetail.Rows.Count > 0)
            {
                string loseSubDetailOrderID = dtLoseSubDetail.AsEnumerable().Select(s => s["OrderID"].ToString()).JoinToString(",");
                return new DualResult(false, $"SP# {loseSubDetailOrderID} <QA Output> can not be empty");
            }
            #endregion

            #region
            if (this.rftDT != null && this.rftDT.Rows.Count > 0)
            {
                string insertRFT = $@"
select *,MDivisionid=(select MDivisionID from Factory where id=t.FactoryID)
into #tmp1
from #tmp t

create  table #td(id varchar(13))
merge RFT t
using #tmp1 s
on  t.orderid = s.orderid and t.Cdate = s.CDate and t.SewinglineID = s.SewinglineID and 
    t.FactoryID = s.FactoryID and t.MDivisionid = s.MDivisionid and t.Shift = s.Shift and t.Team = s.Team 
when not matched by target then 
insert([OrderID],[CDate],[SewinglineID],[FactoryID],[InspectQty],[RejectQty],[DefectQty]
        ,[Shift],[Team],[Status],[Remark],[AddName],[AddDate],[MDivisionid])
values(s.[OrderID],s.[CDate],s.[SewinglineID],s.[FactoryID],s.[InspectQty],s.[RejectQty],s.[DefectQty]
        ,s.[Shift],s.[Team],s.[Status],s.[Remark],'{Env.User.UserID}',getdate(),s.[MDivisionid])
output inserted.id into #td 
;
select * from RFT with(nolock) where id in(select id from #td)
";
                DualResult dualResult = MyUtility.Tool.ProcessWithDatatable(this.rftDT, string.Empty, insertRFT, out this.rftDT);
                if (!dualResult)
                {
                    return dualResult;
                }

                string rdfdetail = $@"
select t.id
	, GarmentDefectTypeID
	, GarmentDefectCodeID
	, Qty=count(*)
from #tmp t
inner join inspection i with(nolock) on t.Cdate = i.InspectionDate and t.FactoryID = i.FactoryID 
	and t.SewinglineID = i.Line and t.Team = i.Team and t.Shift = iif(i.Shift='Day','D','N')and t.OrderId = i.OrderId and i.SunriseNid = 0
inner join Inspection_Detail id with(nolock) on i.id= id.id
where (i.Status <> 'Fixed'  or (i.Status = 'Fixed' and cast(i.AddDate as date) = i.InspectionDate))
group by t.id,GarmentDefectTypeID, GarmentDefectCodeID
";
                DataTable rftDT_Detail;
                SqlConnection sqlConn = null;
                DBProxy.Current.OpenConnection("ManufacturingExecution", out sqlConn);
                dualResult = MyUtility.Tool.ProcessWithDatatable(this.rftDT, string.Empty, rdfdetail, out rftDT_Detail, conn: sqlConn);
                if (!dualResult)
                {
                    return dualResult;
                }

                string insetRFTDetail = $@"
INSERT INTO [dbo].[Rft_Detail]([ID],[GarmentDefectCodeID],[GarmentDefectTypeid],[Qty])
select id,GarmentDefectCodeID,GarmentDefectTypeID,qty from #tmp";

                dualResult = MyUtility.Tool.ProcessWithDatatable(rftDT_Detail, string.Empty, insetRFTDetail, out rftDT_Detail);
                if (!dualResult)
                {
                    return dualResult;
                }

                this.rftDT = null;
            }
            #endregion

            #region 檢查OrderId在Order_Location是否有資料，沒資料就補
            string chk_sql = string.Format(
                @"DECLARE CUR_SewingOutput_Detail CURSOR FOR 
                      Select distinct orderid from SewingOutput_Detail where id =  '{0}' 

                 declare @orderid varchar(13) 
                 OPEN CUR_SewingOutput_Detail   
                 FETCH NEXT FROM CUR_SewingOutput_Detail INTO @orderid 
                 WHILE @@FETCH_STATUS = 0 
                 BEGIN
                   exec dbo.Ins_OrderLocation @orderid
                 FETCH NEXT FROM CUR_SewingOutput_Detail INTO @orderid
                 END
                 CLOSE CUR_SewingOutput_Detail
                 DEALLOCATE CUR_SewingOutput_Detail", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));

            DualResult chkResult = DBProxy.Current.Execute(null, chk_sql);

            if (chkResult == false)
            {
                return chkResult;
            }
            #endregion

            return base.ClickSavePost();
        }

        /// <inheritdoc/>
        protected override void ClickSaveAfter()
        {
            base.ClickSaveAfter();
            string checkNonData = string.Format(
                @"
delete sodd
from SewingOutput_Detail_Detail sodd
where not exists (select 1 
				  from SewingOutput_Detail sod 
				  where sodd.ID = sod.ID
						and sodd.SewingOutput_DetailUKey = sod.UKey
						and sodd.OrderId = sod.OrderId
						and sodd.ComboType = sod.ComboType
						and sodd.Article = sod.Article)
      and sodd.id = '{0}'",
                this.CurrentMaintain["ID"]);

            DualResult result = DBProxy.Current.Execute(null, checkNonData);
            if (result == false)
            {
                MyUtility.Msg.WarningBox(result.Description);
            }

            #region 檢查SewingOutput_Detail與SewingOutput_Detail_Detail QAQty是否相符，不相符就更新
            string chkQAQty_sql = $@"select SD.OrderId,SD.ComboType,SD.QAQty as OriginalQty,SDD_Qty as ActualQty ,ExceesQty =SD.QAQty-SDD_Qty 
from  SewingOutput_Detail SD WITH (NOLOCK)
outer apply 
( 
select isnull(SUM(SDD.QAQty),0) as SDD_Qty from SewingOutput_Detail_Detail SDD WITH (NOLOCK) where SDD.ID=SD.ID and SDD.SewingOutput_DetailUKey=SD.UKey 
) as SDD 
where SD.QAQty!=SDD.SDD_Qty and SD.ID = '{this.CurrentMaintain["ID"]}'";

            // 有QAQty不相等的資料
            if (MyUtility.Check.Seek(chkQAQty_sql))
            {
                string updQAQty_sql = $@"update SD set  SD.QAQty = SDD.SDD_Qty,SD.InlineQty = SDD.SDD_Qty,SD.DefectQty = 0 
from  SewingOutput_Detail SD WITH (NOLOCK)
outer apply 
( 
select isnull(SUM(SDD.QAQty),0) as SDD_Qty from SewingOutput_Detail_Detail SDD WITH (NOLOCK) where SDD.ID=SD.ID and SDD.SewingOutput_DetailUKey=SD.UKey 
) as SDD 
where SD.QAQty!=SDD.SDD_Qty and SD.ID = '{this.CurrentMaintain["ID"]}'";

                // 更新SewingOutput_Detail.QAQty
                DualResult updQAQty_result = DBProxy.Current.Execute(null, updQAQty_sql);
                if (!updQAQty_result)
                {
                    this.ShowErr(updQAQty_result);
                }

                // 重新計算SewingOutput QAQty,InlineQty,DefectQty,TMS,Efficiency,detail workhours
                // 觸發edit->share <working hours> to SP#->save按鈕事件依照原本流程重算重存
                this.toolbar.cmdEdit.PerformClick();
                this.btnShareWorkingHoursToSP.PerformClick();
                this.toolbar.cmdSave.PerformClick();
                return;
            }
            #endregion

            #region 更新 SewingOutputID 子單 WorkHour
            List<SqlParameter> listSqlParmeter = new List<SqlParameter>();
            listSqlParmeter.Add(new SqlParameter("@SewingID", this.CurrentMaintain["ID"]));

            string strUpdateWorkHour = @"
/*
 * 紀錄所有須更新的子單
 */
select soddG.OrderId
	   , soddG.ComboType
	   , soddG.Article
	   , soddG.OrderIDfrom
	   , sod.TMS
	   , QAQty = sum(soddG.QaQty)
	   , soddG.SewingOutput_DetailUKey
	   , AllAllot = AllAllot.value
into #Child
from SewingOutput_Detail_Detail_Garment soddG
inner join SewingOutput_Detail sod on soddG.SewingOutput_DetailUKey = sod.UKey
outer apply (
	select value = sum (mSod.QAQty)
	from SewingOutput_Detail mSod
	where sod.ID = mSod.ID
		  and soddG.OrderIDfrom = mSod.OrderId
		  and soddG.ComboType = mSod.ComboType
		  and soddG.Article = mSod.Article
) motherTTL
outer apply (
	select value = sum (cSoddG.QAQty)
	from SewingOutput_Detail_Detail_Garment cSoddG
	where sod.ID = cSoddG.ID
		  and soddG.OrderIDfrom = cSoddG.OrderIDfrom
		  and soddG.ComboType = cSoddG.ComboType
		  and soddG.Article = cSoddG.Article
) childTTL
outer apply (
	select value = iif (motherTTL.value = childTTL.value, 1, 0)
) AllAllot
where soddG.id = @SewingID
group by soddG.OrderId, soddG.ComboType, soddG.Article, soddG.OrderIDfrom, sod.TMS, soddG.SewingOutput_DetailUKey, AllAllot.value

/*
 * 重新計算子單 WorkHour
 */
select distinct #Child.OrderId
	   , #Child.ComboType
	   , #Child.Article
	   , #Child.SewingOutput_DetailUKey
	   , workHour = Convert (numeric(11, 3), 0)
into #updateChild
from #Child

/*
 * 根據 Sewing ID 取得所有母單的 OrderID, ComboType, Article
 */
declare ComputeCursor Cursor For
select OrderID
	   , ComboType
	   , Article
from SewingOutput_Detail
where ID = @SewingID
	  and AutoCreate = 0

Open ComputeCursor
declare @OrderID varchar (50);
declare @ComboType varchar (2);
declare @Article varchar (50);

Fetch Next From ComputeCursor Into @OrderID, @ComboType, @Article
while (@@FETCH_STATUS <> -1)
begin
	update upd
		set upd.WorkHour = upd.WorkHour
						   + case setWorkHour.AllAllot
                                -- 母單數量已全部分配
                                -- 子單 WorkHour 加總 = 母單 WorkHour
								when 1 then 
									case
									    when setWorkHour.rowNum = setWorkHour.rowCounts then
										    iif (setWorkHour.AccuWorkHour - setWorkHour.newWorkHour > setWorkHour.soWorkHour, 0
																														    , setWorkHour.soWorkHour - (setWorkHour.AccuWorkHour - setWorkHour.newWorkHour))
									    else
										    iif (setWorkHour.AccuWorkHour > setWorkHour.soWorkHour, iif (setWorkHour.AccuWorkHour - setWorkHour.newWorkHour >  setWorkHour.soWorkHour, 0
											 																																	        , setWorkHour.soWorkHour - (setWorkHour.AccuWorkHour - setWorkHour.newWorkHour))
																							        , setWorkHour.newWorkHour)
									end
                                -- 母單數量尚未分配完畢
								else
									case 
										when setWorkHour.AccuWorkHour > setWorkHour.soWorkHour then
											iif (setWorkHour.soWorkHour < (setWorkHour.AccuWorkHour - setWorkHour.newWorkHour), 0
																															    , setWorkHour.soWorkHour - (setWorkHour.AccuWorkHour - setWorkHour.newWorkHour))
										else
											setWorkHour.newWorkHour
									end
						     end
	from #updateChild upd
	inner join (
		select	#Child.SewingOutput_DetailUKey
				, soWorkHour = sod.WorkHour
				, newWorkHour = ComputeWorkHour.value
				, AccuWorkHour = sum(ComputeWorkHour.value) over (partition by #Child.OrderIDFrom order by #Child.OrderID)
	            , rowNum = row_number() over (partition by #Child.OrderIDFrom order by #Child.OrderID)
				, rowCounts = count(1) over (partition by #Child.OrderIDFrom)
				, #Child.AllAllot
		from SewingOutput_Detail sod
		inner join #Child on sod.OrderId = #Child.OrderIDfrom
							 and sod.ComboType = #Child.ComboType
							 and sod.Article = #Child.Article
		outer apply (
			select value = isnull(sod.QaQty * sod.TMS, 0)
		) TotalQaQty
		outer apply (
			select value = Round(1.0 * #Child.QaQty * #Child.TMS / TotalQaQty.value * sod.WorkHour, 3)
		) ComputeWorkHour
		where sod.ID = @SewingID
			  and sod.OrderId = @OrderID
			  and sod.ComboType = @ComboType
			  and sod.Article = @Article
			  and sod.AutoCreate = 0
	) setWorkHour on upd.SewingOutput_DetailUKey = setWorkHour.SewingOutput_DetailUKey

	Fetch Next From ComputeCursor Into @OrderID, @ComboType, @Article
end

close ComputeCursor
Deallocate ComputeCursor

/*
 * End & Check
 */
update sod
set sod.WorkHour = upd.workHour
from SewingOutput_Detail sod
inner join #updateChild upd on sod.UKey = upd.SewingOutput_DetailUKey

drop table #Child, #updateChild


update s set
	s.WorkHour=isnull(s.WorkHour,0)+isnull(ss.WorkHour,0)
from SewingOutput_Detail s
outer apply(
	select WorkHour=isnull(s3.WorkHour,0)-isnull(sum(sd.WorkHour),0)
	from SewingOutput s3
	inner join SewingOutput_Detail sd on s.id = sd.ID
	where s3.id=s.id and sd.AutoCreate = 0
	group by sd.ID,s3.SewingLineID,s3.OutputDate,s3.WorkHour,s3.Team,s3.Shift having sum(sd.WorkHour)<> s.WorkHour
)ss
where id = @SewingID
and ukey = (select max(ukey) from SewingOutput_Detail s2 where s.id =s2.id)
";

            TransactionScope transactionscope = new TransactionScope();
            using (transactionscope)
            {
                DualResult dualResult = DBProxy.Current.Execute(null, strUpdateWorkHour, listSqlParmeter);

                if (dualResult == false)
                {
                    transactionscope.Dispose();
                    MyUtility.Msg.WarningBox(dualResult.ToString());
                    return;
                }

                transactionscope.Complete();
                transactionscope.Dispose();
            }
            #endregion
            this.RenewData();

            // this.OnDetailEntered();
        }

        /// <inheritdoc/>
        protected override DualResult ClickSaveSubDetial(SubDetailSaveEventArgs e)
        {
            DualResult result = base.ClickSaveSubDetial(e);
            if (result == false)
            {
                return result;
            }

            #region 重新 修改、刪除第三層資料
            List<DataRow> inserted = new List<DataRow>();
            List<DataRow> updated = new List<DataRow>();
            List<DataRow> deleteList = new List<DataRow>();
            var ok = DBProxy.Current.GetTableSchema(null, this.SubGridAlias, out this.sub_Schema);
            if (!ok)
            {
                return ok;
            }

            foreach (KeyValuePair<DataRow, DataTable> it in e.SubDetails)
            {
                foreach (DataRow dr in it.Value.Rows)
                {
                    if (dr.RowState == DataRowState.Deleted)
                    {
                        continue;
                    }

                    if (MyUtility.Convert.GetInt(dr["QAQty"]) <= 0)
                    {
                        deleteList.Add(dr);
                    }
                    else
                    {
                        if (dr.RowState == DataRowState.Modified && MyUtility.Convert.GetInt(dr["QAQty"]) > 0 && MyUtility.Convert.GetInt(dr["QAQty", DataRowVersion.Original]) == 0)
                        {
                            dr.AcceptChanges();
                            dr.SetAdded();
                            inserted.Add(dr);
                        }
                    }
                }
            }

            List<DataRow> newDelete = new List<DataRow>();
            if (deleteList.Count > 0)
            {
                var newT = deleteList[0].Table.Clone();
                for (int i = 0; i < deleteList.Count; i++)
                {
                    var newOne = newT.NewRow();
                    newOne.ItemArray = deleteList[i].ItemArray;
                    try
                    {
                        if (deleteList[i].RowState != DataRowState.Added)
                        {
                            newOne["QaQty"] = deleteList[i]["qaqty", DataRowVersion.Original];
                        }
                        else
                        {
                            newOne["QaQty"] = deleteList[i]["qaqty", DataRowVersion.Current];
                        }
                    }
                    catch (Exception ec)
                    {
                        this.ShowErr("Error:", ec);
                    }

                    newDelete.Add(newOne);
                    newT.Rows.Add(newOne);

                    // newOne["QaQty"] = Updated[i]["qaqty"];
                }

                newT.AcceptChanges();
                for (int i = 0; i < deleteList.Count; i++)
                {
                    newDelete[i]["QaQty"] = 0;
                }
            }

            // foreach (DataRow dr in inserted)
            // {
            //    string x = dr.RowState.ToString();
            // }
            ok = DBProxy.Current.Deletes(null, this.sub_Schema, newDelete);
            if (!ok)
            {
                return ok;
            }

            // ok = DBProxy.Current.Batch(null, sub_Schema, Updated);
            // ok = DBProxy.Current.Batch(null, this.sub_Schema, newUpdated);
            // if (!ok)
            // {
            //    return ok;
            // }
            ok = DBProxy.Current.Inserts(null, this.sub_Schema, inserted);
            if (!ok)
            {
                return ok;
            }
            #endregion
            return ok;
        }

        // Date
        private void DateDate_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode && !MyUtility.Check.Empty(this.dateDate.Value) && this.dateDate.Value != this.dateDate.OldValue)
            {
                if (this.dateDate.Value > DateTime.Today)
                {
                    this.dateDate.Value = null;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("Date is later than today, pls pay attention!!");
                    return;
                }

                DateTime? sewingMonthlyLockDate = MyUtility.Convert.GetDate(MyUtility.GetValue.Lookup($"select LockDate from SewingMonthlyLock where FactoryID = '{this.CurrentMaintain["FactoryID"]}'"));
                if (this.dateDate.Value <= sewingMonthlyLockDate)
                {
                    this.dateDate.Value = null;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("Date can't earlier than Sewing Lock Date: {0}.", Convert.ToDateTime(sewingMonthlyLockDate).ToString(string.Format("{0}", Env.Cfg.DateStringFormat))));
                    return;
                }

                this.CurrentMaintain["OutputDate"] = this.dateDate.Value;
                this.CurrentMaintain.EndEdit();
                this.FromDQS();
            }
        }

        // Manpower
        private void NumManpower_Validated(object sender, EventArgs e)
        {
            // 值有異動過就要重算ManHour
            if (this.EditMode && this.numManpower.Value != this.numManpower.OldValue)
            {
                this.CalculateManHour();
            }
        }

        // W/Hours(Day)
        private void NumWHours_Validated(object sender, EventArgs e)
        {
            // 值有異動過就要重算ManHour
            if (this.EditMode && this.numWHours.Value != this.numWHours.OldValue)
            {
                this.CalculateManHour();
            }
        }

        // 計算ManHour
        private void CalculateManHour()
        {
            this.CurrentMaintain["ManHour"] = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(this.CurrentMaintain["Manpower"]) * MyUtility.Convert.GetDecimal(this.CurrentMaintain["WorkHour"]), 3);
        }

        // Share < working hours > to SP#
        private void BtnShareWorkingHoursToSP_Click(object sender, EventArgs e)
        {
            DataTable sumQaQty;
            try
            {
                string strSumQaQty = @"
select sumQaqty = isnull(sum(QAQty*TMS),0)
       , RecCnt = isnull(count(QAQty),0)
from #tmp
where Convert (bit, AutoCreate) != 1";
                MyUtility.Tool.ProcessWithDatatable((DataTable)this.detailgridbs.DataSource, "QAQty,TMS,OrderID,AutoCreate", strSumQaQty, out sumQaQty, "#tmp");
            }
            catch (Exception ex)
            {
                this.ShowErr("Calculate error.", ex);
                return;
            }

            if (sumQaQty == null)
            {
                return;
            }

            int recCnt = MyUtility.Convert.GetInt(sumQaQty.Rows[0]["RecCnt"]);
            decimal ttlQaqty = MyUtility.Convert.GetDecimal(sumQaQty.Rows[0]["sumQaqty"]);

            decimal subSum = 0;
            foreach (DataRow dr in ((DataTable)this.detailgridbs.DataSource).Select(" AutoCreate <>'True'"))
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    recCnt = recCnt - 1;
                    if (recCnt == 0)
                    {
                        dr["WorkHour"] = MyUtility.Convert.GetDecimal(this.CurrentMaintain["WorkHour"]) - subSum;
                    }
                    else
                    {
                        dr["WorkHour"] = ttlQaqty == 0 ? 0 : MyUtility.Math.Round(MyUtility.Convert.GetDecimal(dr["QAQty"]) * MyUtility.Convert.GetDecimal(dr["TMS"]) / ttlQaqty * MyUtility.Convert.GetDecimal(this.CurrentMaintain["WorkHour"]), 3);
                    }

                    subSum = subSum + MyUtility.Convert.GetDecimal(dr["WorkHour"]);
                }
            }
        }

        // Revised History
        private void BtnRevisedHistory_Click(object sender, EventArgs e)
        {
            Win.UI.ShowHistory callNextForm = new Win.UI.ShowHistory("SewingOutput_History", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), "Status", reasonType: "Sewing_RVS", caption: "Revised History");
            callNextForm.ShowDialog(this);
        }

        /// <inheritdoc/>
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            Win.UI.SelectReason callReason = new Win.UI.SelectReason("Sewing_RVS", true);
            DialogResult dResult = callReason.ShowDialog(this);
            if (dResult == DialogResult.OK)
            {
                string insertCmd = string.Format(
                    @"insert into SewingOutput_History (ID,HisType,OldValue,NewValue,ReasonID,Remark,AddName,AddDate)
values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}',GETDATE())",
                    MyUtility.Convert.GetString(this.CurrentMaintain["ID"]),
                    "Status",
                    "Locked",
                    "New",
                    callReason.ReturnReason,
                    callReason.ReturnRemark,
                    Env.User.UserID);

                string updateCmd = $@"
update SewingOutput 
set LockDate = null, Status = 'New' 
, EditDate = GetDate(), EditName = '{Env.User.UserID}'
where ID = '{MyUtility.Convert.GetString(this.CurrentMaintain["ID"])}'";

                using (TransactionScope transactionScope = new TransactionScope())
                {
                    try
                    {
                        DualResult result = DBProxy.Current.Execute(null, insertCmd);
                        DualResult result2 = DBProxy.Current.Execute(null, updateCmd);

                        if (result && result2)
                        {
                            transactionScope.Complete();
                        }
                        else
                        {
                            transactionScope.Dispose();
                            MyUtility.Msg.WarningBox("UnConfirm failed, Pleaes re-try");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        transactionScope.Dispose();
                        this.ShowErr("Commit transaction error.", ex);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 當 Row 資料修改 or 刪除時
        /// 必須檢查 Packing 數量
        /// </summary>
        /// <returns>bool</returns>
        private bool CheckRemoveRow()
        {
            DataTable subDt;
            this.GetSubDetailDatas(this.CurrentDetailData, out subDt);

            #region 產出數量不能小於已出貨的數量
            DualResult resultCheckQty;
            DataTable dtCheckQty;

            if (subDt.Rows.Count > 0)
            {
                string strCheckQty = @" 
SELECT * 
FROM   (
    SELECT SewQty = t.AccumQty
           , PackQty = pack.PackQty
           , t.orderid
           , t.ComboType
           , t.article
           , t.sizecode 
           , adjQty = InvAdjustDiffQty.value
    FROM   #tmp t 
	outer apply
	(
		select Sum(b.shipqty) AS PackQty
		from PackingList a
		inner join PackingList_Detail b on a.ID=b.ID
		where b.OrderID=t.OrderId
		      and b.Article = t.Article 
              and b.SizeCode = t.SizeCode
		      and a.Status= 'Confirmed'
	) pack
	outer apply (
		select value = isnull(sum (iaq.DiffQty),0)
		from InvAdjust ia WITH (NOLOCK)
		inner join InvAdjust_Qty iaq WITH (NOLOCK) on iaq.ID = ia.ID
		where ia.OrderID = t.OrderId 			  
			  and iaq.Article = t.Article 
			  and iaq.SizeCode = t.SizeCode
	) InvAdjustDiffQty
) a 
WHERE sewqty < (packqty + adjQty) ";

                resultCheckQty = MyUtility.Tool.ProcessWithDatatable(subDt, null, strCheckQty, out dtCheckQty);
                string error = string.Empty;

                if (resultCheckQty == false)
                {
                    MyUtility.Msg.WarningBox(resultCheckQty.ToString());
                    return false;
                }
                else if (dtCheckQty != null && dtCheckQty.Rows.Count > 0)
                {
                    for (int i = 0; i < dtCheckQty.Rows.Count; i++)
                    {
                        error = error + string.Format(
                            @"Order: <{0}> ComboType: <{1}> Article: <{2}> Size: <{3}> QAQty: <{4}>  less than ShipQty: <{5}>",
                            dtCheckQty.Rows[i]["Orderid"].ToString(),
                            dtCheckQty.Rows[i]["ComboType"].ToString(),
                            dtCheckQty.Rows[i]["Article"].ToString(),
                            dtCheckQty.Rows[i]["SizeCode"].ToString(),
                            dtCheckQty.Rows[i]["SewQty"].ToString(),
                            dtCheckQty.Rows[i]["PackQty"].ToString());
                    }

                    if (!MyUtility.Check.Empty(error.ToString()))
                    {
                        MyUtility.Msg.WarningBox(error.ToString());
                        return false;
                    }
                }
            }
            #endregion
            return true;
        }

        /// <summary>
        /// 有過daily lock紀錄後不得手動修改SP by #ISP20200604
        /// </summary>
        /// <returns>bool</returns>
        private bool CheckSPEditable()
        {
            string canReviseDailyLockData = MyUtility.GetValue.Lookup("select CanReviseDailyLockData from System");
            if (canReviseDailyLockData.ToUpper() != "TRUE" && !MyUtility.Check.Empty(this.CurrentMaintain["Status"]))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 刪除, 儲存前 檢查 Packing 數量
        /// Save 必須加上 QaQty
        /// Delete 則不用
        /// </summary>
        /// <param name="dtSubDetail">第三層資料</param>
        /// <param name="saveCheck">true : Save; false : Delete</param>
        /// <returns>bool</returns>
        private bool SaveDeleteCheckPacking(DataTable dtSubDetail, bool saveCheck)
        {
            DualResult resultCheckQty;
            DataTable dtCheckQty;

            if (!MyUtility.Check.Empty(dtSubDetail) && dtSubDetail.Rows.Count > 0)
            {
                string strCheckQty = string.Format(
                    @" 
SELECT * 
FROM   (
    SELECT SewQty = t.AccumQty {0}
           , PackQty = pack.PackQty
           , t.orderid
           , t.ComboType
           , t.article
           , t.sizecode 
           , adjQty = InvAdjustDiffQty.value
    FROM   #tmp t 
	outer apply
	(
		select Sum(b.shipqty) AS PackQty
		from PackingList a
		inner join PackingList_Detail b on a.ID=b.ID
		where b.OrderID=t.OrderId
		      and b.Article = t.Article 
              and b.SizeCode = t.SizeCode
		      and a.Status= 'Confirmed'
	) pack
	outer apply (
		select value = isnull(sum (iaq.DiffQty),0)
		from InvAdjust ia WITH (NOLOCK)
		inner join InvAdjust_Qty iaq WITH (NOLOCK) on iaq.ID = ia.ID
		where ia.OrderID = t.OrderId 			  
			  and iaq.Article = t.Article 
			  and iaq.SizeCode = t.SizeCode
	) InvAdjustDiffQty
    where Convert (bit, t.AutoCreate) != 1
) a 
WHERE sewqty < (packqty + adjQty)",
                    saveCheck ? "+ t.qaqty " : string.Empty);

                resultCheckQty = MyUtility.Tool.ProcessWithDatatable(dtSubDetail, null, strCheckQty, out dtCheckQty);
                string error = string.Empty;

                if (resultCheckQty == false)
                {
                    MyUtility.Msg.WarningBox(resultCheckQty.ToString());
                    return false;
                }
                else if (dtCheckQty != null && dtCheckQty.Rows.Count > 0)
                {
                    for (int i = 0; i < dtCheckQty.Rows.Count; i++)
                    {
                        error = error + string.Format(
                            @"Order: <{0}> ComboType: <{1}> Article: <{2}> Size: <{3}> QAQty: <{4}>  less than ShipQty: <{5}>",
                            dtCheckQty.Rows[i]["Orderid"].ToString(),
                            dtCheckQty.Rows[i]["ComboType"].ToString(),
                            dtCheckQty.Rows[i]["Article"].ToString(),
                            dtCheckQty.Rows[i]["SizeCode"].ToString(),
                            dtCheckQty.Rows[i]["SewQty"].ToString(),
                            dtCheckQty.Rows[i]["PackQty"].ToString());
                    }

                    if (!MyUtility.Check.Empty(error.ToString()))
                    {
                        MyUtility.Msg.WarningBox(error.ToString());
                        return false;
                    }
                }
            }

            return true;
        }

        private void TxtdropdownlistShift_SelectedValueChanged(object sender, EventArgs e)
        {
            if (this.EditMode)
            {
                if (this.txtdropdownlistShift.SelectedValue == null)
                {
                    return;
                }

                if (this.txtdropdownlistShift.SelectedValue.Equals("O"))
                {
                    this.txtSubconOutFty.TextBox1.ReadOnly = false;
                    this.txtSubConOutContractNumber.ReadOnly = false;
                    this.CurrentMaintain["Shift"] = this.txtdropdownlistShift.SelectedValue;
                    this.CurrentMaintain.EndEdit();
                }
                else
                {
                    this.txtSubconOutFty.TextBox1.ReadOnly = true;
                    this.txtSubConOutContractNumber.ReadOnly = true;
                    this.CurrentMaintain["Shift"] = this.txtdropdownlistShift.SelectedValue;
                    this.CurrentMaintain["SubconOutFty"] = string.Empty;
                    this.CurrentMaintain["SubConOutContractNumber"] = string.Empty;
                    this.CurrentMaintain.EndEdit();
                }

                if (MyUtility.Convert.GetString(this.txtdropdownlistShift.SelectedValue) != MyUtility.Convert.GetString(this.txtdropdownlistShift.OldValue))
                {
                    this.FromDQS();
                }
            }
        }

        private void TxtSubConOutContractNumber_Validating(object sender, CancelEventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtSubConOutContractNumber.Text))
            {
                return;
            }

            string checkCmd = $@"select 1 from dbo.SubconOutContract with (nolock) where SubConOutFty = '{this.CurrentMaintain["SubconOutFty"]}'   and ContractNumber = '{this.txtSubConOutContractNumber.Text}'";
            if (!MyUtility.Check.Seek(checkCmd))
            {
                MyUtility.Msg.WarningBox("Subcon-Out-Fty and SubCon-Out Contract Number not exists!");
                e.Cancel = true;
            }
        }

        private void TxtSubconOutFty_Validating(object sender, CancelEventArgs e)
        {
            this.CurrentMaintain["SubconOutFty"] = this.txtSubconOutFty.TextBox1.Text;
            this.CurrentMaintain["SubConOutContractNumber"] = string.Empty;
            if (MyUtility.Check.Seek($"select 1 from dbo.SCIFty with (nolock) where ID = '{this.txtSubconOutFty.TextBox1.Text}'"))
            {
                this.txtSubConOutContractNumber.ReadOnly = true;
            }
            else
            {
                this.txtSubConOutContractNumber.ReadOnly = false;
            }
        }

        private void BtnRequestUnlock_Click(object sender, EventArgs e)
        {
            Win.UI.SelectReason callReason = new Win.UI.SelectReason("Sewing_RVS");
            DialogResult dResult = callReason.ShowDialog(this);
            if (dResult == DialogResult.OK)
            {
                string toAddress = MyUtility.GetValue.Lookup($@"
SELECT CONCAT(p1.EMail,';')  
FROM Factory f  
INNER JOIN Pass1 p1 ON p1.id = f.Manager  
WHERE  f.ID = '{this.CurrentMaintain["FactoryID"]}' AND p1.EMail <>''

UNION ALL

SELECT TOP 2 CONCAT(p1.EMail,';')  
FROM  Pass1 p1 
INNER JOIN Pass0 p0 ON p0.PKey=p1.FKPass0  
INNER JOIN Pass2 p2 ON p2.PKey=p0.PKey 
WHERE p1.Factory LIKE ('%{this.CurrentMaintain["FactoryID"]}%') AND p1.ID <> 'SCIMIS' AND p1.EMail <>''
AND p0.PKey IN ( 
				SELECT FKPass0 
				FROM Pass2 WHERE MenuName = 'Sewing' AND BarPrompt='P01. Sewing daily output' 
				AND CanRecall = 1)  
FOR XML PATH('')

");
                string ccAddress = string.Empty;
                string subject = "Request Unlock Sewing";

                string od = string.Empty;
                if (!MyUtility.Check.Empty(this.CurrentMaintain["OutputDate"]))
                {
                    od = ((DateTime)this.CurrentMaintain["OutputDate"]).ToString("yyyy/MM/dd");
                }

                string description = $@"Date : {od}
Factory : {this.CurrentMaintain["FactoryID"]}
Line# : {this.CurrentMaintain["SewingLineID"]}
Team : {this.CurrentMaintain["Team"]}
Shift : {this.CurrentMaintain["Shift"]}
QA Output: {this.CurrentMaintain["QAQty"]}
Manhours : {this.CurrentMaintain["Manhour"]}
Reason : {MyUtility.GetValue.Lookup($@"select name from Reason where ReasonTypeID='Sewing_RVS' and id= '{callReason.ReturnReason}'")}
Remark : {callReason.ReturnRemark}
";
                var email = new MailTo(Env.Cfg.MailFrom, toAddress, ccAddress, subject, null, description, false, true);

                // email畫面關閉後額外塞入CC人員
                email.SendingBefore += this.Email_SendingBefore;
                email.ShowDialog(this);

                if (email.DialogResult == DialogResult.OK)
                {
                    this.btnRequestUnlock.Enabled = false;
                    string sqlcmd = $@"insert into SewingOutput_DailyUnlock(SewingOutputID,ReasonID,Remark,RequestDate,RequestName)
values('{this.CurrentMaintain["ID"]}','{callReason.ReturnReason}','{callReason.ReturnRemark}',getdate(),'{Env.User.UserID}')";
                    DualResult rs = DBProxy.Current.Execute("Production", sqlcmd);
                    if (!rs)
                    {
                        this.ShowErr(rs);
                    }
                }
            }
        }

        /// <summary>
        /// email畫面關閉後額外塞入CC人員
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void Email_SendingBefore(object sender, MailTo.SendMailBeforeArg e)
        {
            e.Mail.CC.Add("planning@sportscity.com.tw");
            e.Mail.CC.Add("team3@sportscity.com.tw");
        }

        /// <inheritdoc/>
        protected override void ClickRecall()
        {
            base.ClickRecall();
            if (!this.Perm.Recall)
            {
                MyUtility.Msg.WarningBox("You have no permission.");
                return;
            }

            string sqlcmd = $@"
declare @reasonID nvarchar(5)
declare @remark nvarchar(max)
declare @ukey bigint
select top 1 @ukey=ukey,@reasonID=reasonID,@remark=remark from SewingOutput_DailyUnlock where SewingOutputID = '{this.CurrentMaintain["ID"]}' order by Ukey desc

insert into SewingOutput_History (ID,HisType,OldValue,NewValue,ReasonID,Remark,AddName,AddDate)
values ('{this.CurrentMaintain["ID"]}','Status','Sent','New',isnull(@reasonID,''),isnull(@remark,''),'{Env.User.UserID}',GETDATE())

Update SewingOutput_DailyUnlock set 
	UnLockDate = getdate()
	,UnLockName= '{Env.User.UserID}'
where ukey=@ukey

update SewingOutput set Status='New', LockDate = null
, editname='{Env.User.UserID}' 
, editdate=getdate()
where ID = '{this.CurrentMaintain["ID"]}' 
";

            using (TransactionScope scope = new TransactionScope())
            {
                DualResult upResult;
                if (!MyUtility.Check.Empty(sqlcmd))
                {
                    if (!(upResult = DBProxy.Current.Execute(null, sqlcmd)))
                    {
                        this.ShowErr(upResult);
                    }
                }

                scope.Complete();
            }
        }

        private void BtnBatchRecall_Click(object sender, EventArgs e)
        {
            if (!this.Perm.Recall)
            {
                MyUtility.Msg.WarningBox("You have no permission.");
                return;
            }

            P01_BatchRecall callNextForm = new P01_BatchRecall();
            callNextForm.ShowDialog(this);
        }

        protected override void ClickSend()
        {
            base.ClickSend();
            string sqlcmdChk = $@"
select 1
FROM SewingOutput s
INNER JOIN SewingOutput_Detail sd ON sd.ID = s.ID
INNER JOIN Orders o ON o.ID = sd.OrderId
where 1=1
    and s.OutputDate < = CAST (GETDATE() AS DATE) 
    and s.LockDate is null 
    and s.FactoryID  = '{Env.User.Factory}'
";
            if (!MyUtility.Check.Seek(sqlcmdChk))
            {
                MyUtility.Msg.WarningBox("Already lock now!");
                return;
            }

            if (MyUtility.Msg.QuestionBox("Lock sewing data?") == DialogResult.No)
            {
                return;
            }

            string sqlcmd = $@"
UPDATE  s 
SET s.LockDate = CONVERT(date, GETDATE()) , s.Status='Sent'
, s.editname='{Env.User.UserID}', s.editdate=getdate()
FROM SewingOutput s
INNER JOIN SewingOutput_Detail sd ON sd.ID = s.ID
INNER JOIN Orders o ON o.ID = sd.OrderId
where 1=1
    and s.OutputDate < = CAST (GETDATE() AS DATE) 
    and s.LockDate is null 
    and s.FactoryID  = '{Env.User.Factory}'
";

            string sqlFixWrongSewingOutput = $@"
Declare @ID varchar(13)
Declare @WorkHour numeric(6,2)
Declare @QAQty int
Declare @DefectQty int
Declare @InlineQty int
Declare @TMS int
Declare @Efficiency numeric(6,1)
Declare @ManHour numeric(9,3)
Declare @AllQAQty int

DECLARE Sewingoutput_cursor CURSOR FOR 
	select S.ID,S.WorkHour,S.ManHour
	from SewingOutput S with (nolock)
	where S.Category='O' and s.FactoryID = '{Env.User.Factory}' and s.OutputDate <= GETDATE() and s.LockDate is null  and
		 exists(select 1  from  SewingOutput_Detail SD WITH (NOLOCK)
					outer apply 
					( 
					select isnull(SUM(SDD.QAQty),0) as SDD_Qty from SewingOutput_Detail_Detail SDD WITH (NOLOCK) where SDD.ID=SD.ID and SDD.SewingOutput_DetailUKey=SD.UKey 
					) as SDD 
					where SD.ID = S.ID and SD.QAQty != SDD.SDD_Qty) 


OPEN Sewingoutput_cursor --開始run cursor                   
FETCH NEXT FROM Sewingoutput_cursor INTO @ID,@WorkHour,@ManHour
WHILE @@FETCH_STATUS = 0 --檢查是否有讀取到資料; WHILE用來處理迴圈，當為true時則進入迴圈執行
BEGIN
	--更新SewingOutput_Detail數量
	update SD set  SD.QAQty = SDD.SDD_Qty,SD.InlineQty = SDD.SDD_Qty,SD.DefectQty = 0 
			from  SewingOutput_Detail SD WITH (NOLOCK)
			outer apply 
			( 
			select isnull(SUM(SDD.QAQty),0) as SDD_Qty from SewingOutput_Detail_Detail SDD WITH (NOLOCK) where SDD.ID=SD.ID and SDD.SewingOutput_DetailUKey=SD.UKey 
			) as SDD 
			where SD.QAQty!=SDD.SDD_Qty and SD.ID = @ID

	--重新計算 WorkHour
	
	select	UKey,
		    [WorkHour] = case	When AllCost = 0 or @WorkHour = 0 then 0
								when IsLast = 0 then Round(Cost / AllCost * @WorkHour,3)
								--最後一筆要用前面的值加總後剪掉最後一筆的值，這樣才不會有小數差
								else @WorkHour + Round(Cost / AllCost * @WorkHour,3) - sum(iif(AllCost = 0,0, Round(Cost / AllCost * @WorkHour,3))) OVER ()   end
	into #tmpUpdWorkHour
	from (select UKey,
		[Cost] = cast( (isnull(sd.TMS,0) * isnull(sd.QAQty,0)) as numeric),
		[AllCost] = Cast(sum(isnull(sd.TMS,0) * isnull(sd.QAQty,0))	OVER () as numeric),
		[IsLast] = iif(LEAD(sd.QAQty,1,0) OVER (order by ukey) = 0 ,1,0)
	from SewingOutput_Detail  SD WITH (NOLOCK)
	where ID = @ID and AutoCreate = 0) a
	
	update SD set SD.WorkHour = t.WorkHour
	from SewingOutput_Detail  SD
	inner join #tmpUpdWorkHour t on sd.ukey = t.ukey

	drop table #tmpUpdWorkHour

	--更新表頭資料
	select @AllQAQty = isnull(sum(isnull(QAQty,0)),0) from SewingOutput_Detail where id = @ID and AutoCreate = 0

	select
	@QAQty = @AllQAQty,
	@InlineQty = isnull(sum(isnull(InlineQty,0)),0),
	@DefectQty = isnull(sum(isnull(DefectQty,0)),0),
	@TMS = iif(@AllQAQty = 0,0,Round(sum(isnull(SD.TMS,0) * isnull(SD.QAQty,0) * 1.0 ) / @AllQAQty,0))
	from SewingOutput_Detail SD WITH (NOLOCK)
	where ID = @ID and AutoCreate = 0

	if(@TMS = 0 or @ManHour = 0)
	begin
		set @Efficiency = 0
	end
	else
	begin
		set @Efficiency = @QAQty * 1.0 / (3600.0 / @TMS * @ManHour * 1.0) * 100
	end

	update SewingOutput set	QAQty = @QAQty,
							InlineQty = @InlineQty,
							DefectQty = @DefectQty,
							TMS = @TMS,
							Efficiency = @Efficiency
	where ID = @ID

FETCH NEXT FROM Sewingoutput_cursor INTO @ID,@WorkHour,@ManHour
END
--關閉cursor與參數的關聯
CLOSE Sewingoutput_cursor
DEALLOCATE Sewingoutput_cursor --將cursor物件從記憶體移除
";

            using (TransactionScope scope = new TransactionScope())
            {
                DualResult upResult;

                upResult = DBProxy.Current.Execute(null, sqlFixWrongSewingOutput);
                if (!upResult)
                {
                    scope.Dispose();
                    this.ShowErr(upResult);
                    return;
                }

                if (!MyUtility.Check.Empty(sqlcmd))
                {
                    if (!(upResult = DBProxy.Current.Execute(null, sqlcmd)))
                    {
                        scope.Dispose();
                        this.ShowErr(upResult);
                        return;
                    }
                }

                scope.Complete();
            }

            if (MyUtility.Check.Seek($@"select 1 from Factory where type !='S' and id = '{Env.User.Factory}'"))
            {
                SendMail();
            }
        }

        /// <summary>
        /// ClickSend 自動寄信給Planning Team , Team3
        /// </summary>
        public static void SendMail()
        {
            DataTable dtR01;
            DataTable ttlData = null;
            DataTable subprocessData = null;
            List<APIData> dataMode = new List<APIData>();
            DateTime? dateMaxOutputDate = MyUtility.Convert.GetDate(MyUtility.GetValue.Lookup(@"
select max(OutputDate) 
from SewingOutput 
where OutputDate != convert(date,GETDATE())"));

            #region 判斷Sewing.P01+P02 是否Locked

            DualResult result;
            DataRow drData;
            string sql =
            $@"select OutputDate
	                ,FactoryID	                
                    ,Category
                from SewingOutput
                where 1=1
                    and OutputDate = '{Convert.ToDateTime(dateMaxOutputDate).ToString("d")}'
                    and Status in('','NEW')
                    and FactoryID = '{Env.User.Factory}'
            ";
            if (MyUtility.Check.Seek(sql, out drData))
            {
                if (string.Compare(drData["Category"].ToString(), "M") == 0)
                {
                    MyUtility.Msg.WarningBox($@"<OutputDate>: {Convert.ToDateTime(dateMaxOutputDate).ToString("d")} does not daily lock in Sewing P02");
                }
                else if (string.Compare(drData["Category"].ToString(), "O") == 0)
                {
                    MyUtility.Msg.WarningBox($@"<OutputDate>: {Convert.ToDateTime(dateMaxOutputDate).ToString("d")} does not daily lock in Sewing P01");
                }

                return;
            }

            #endregion

            #region 產生R01 報表

            #region 撈R01 SQL
            StringBuilder sqlCmd = new StringBuilder();
            #region 組撈Data SQL
            sqlCmd.Append(string.Format(
                @"
select  s.OutputDate
		, s.Category
		, s.Shift
		, s.SewingLineID
		, [ActManPower] = IIF(sd.QAQty = 0, s.Manpower, s.Manpower * sd.QAQty)
		, s.Team
		, sd.OrderId
		, sd.ComboType
		, sd.WorkHour
		, sd.QAQty
		, sd.InlineQty
		, [OrderCategory] = isnull(o.Category,'')
		, o.LocalOrder
		, [OrderCdCodeID] = isnull(o.CdCodeID,'')
		, [MockupCDCodeID] = isnull(mo.MockupID,'')
		, s.FactoryID
		, [OrderCPU] = isnull(o.CPU,0)
		, [OrderCPUFactor] = isnull(o.CPUFactor,0)
		, [MockupCPU] = isnull(mo.Cpu,0)
		, [MockupCPUFactor] = isnull(mo.CPUFactor,0)
		, [OrderStyle] = isnull(o.StyleID,'')
		, [MockupStyle] = isnull(mo.StyleID,'')
		, [OrderSeason] = isnull(o.SeasonID,'')
		, [MockupSeason] = isnull(mo.SeasonID,'')
	    , [Rate] = isnull([dbo].[GetOrderLocation_Rate](o.id, sd.ComboType),100)/100
		, System.StdTMS
		, [ori_QAQty] = sd.QAQty
		, [ori_InlineQty] = sd.InlineQty
        , [SubconInSisterFty] = isnull(o.SubconInSisterFty,0)
into #tmpSewingDetail
from System,SewingOutput s WITH (NOLOCK) 
inner join SewingOutput_Detail sd WITH (NOLOCK) on sd.ID = s.ID
left join Orders o WITH (NOLOCK) on o.ID = sd.OrderId 
left join MockupOrder mo WITH (NOLOCK) on mo.ID = sd.OrderId

where s.OutputDate = '{0}'
	  and s.FactoryID = '{1}'
      and (o.CateGory NOT IN ('G','A') or s.Category='M')  ",
                Convert.ToDateTime(dateMaxOutputDate).ToString("d"),
                Env.User.Factory));

            sqlCmd.Append(@"
select OutputDate
	   , Category
	   , Shift
	   , SewingLineID
	   , ActManPower = Round(Sum(ActManPower),2)
	   , Team
	   , OrderId
	   , ComboType
	   , WorkHour = Round(sum(WorkHour),3)
	   , QAQty = sum(QAQty) 
	   , InlineQty = sum(InlineQty) 
	   , OrderCategory
	   , LocalOrder
	   , OrderCdCodeID
	   , MockupCDCodeID
	   , FactoryID
	   , OrderCPU
	   , OrderCPUFactor
	   , MockupCPU
	   , MockupCPUFactor
	   , OrderStyle
	   , MockupStyle
	   , OrderSeason
	   , MockupSeason
	   , Rate
	   , StdTMS
	   , ori_QAQty
	   , ori_InlineQty
       , SubconInSisterFty
into #tmpSewingGroup
from #tmpSewingDetail
group by OutputDate, Category, Shift, SewingLineID, Team, OrderId
		 , ComboType, OrderCategory, LocalOrder, OrderCdCodeID
		 , MockupCDCodeID, FactoryID, OrderCPU, OrderCPUFactor
		 , MockupCPU, MockupCPUFactor, OrderStyle, MockupStyle
		 , OrderSeason, MockupSeason, Rate, StdTMS, ori_QAQty
		 , ori_InlineQty, SubconInSisterFty
----↓計算累計天數 function table太慢直接寫在這
select distinct scOutputDate = s.OutputDate 
	   , style = IIF(t.Category <> 'M', OrderStyle, MockupStyle)
	   , t.SewingLineID
	   , t.FactoryID
	   , t.Shift
	   , t.Team
	   , t.OrderId
	   , t.ComboType
into #stmp
from #tmpSewingGroup t
inner join SewingOutput s WITH (NOLOCK) on s.SewingLineID = t.SewingLineID 
										   and s.OutputDate between dateadd(day,-90,t.OutputDate) and  t.OutputDate 
										   and s.FactoryID = t.FactoryID
inner join SewingOutput_Detail sd WITH (NOLOCK) on s.ID = sd.ID
left join Orders o WITH (NOLOCK) on o.ID =  sd.OrderId
left join MockupOrder mo WITH (NOLOCK) on mo.ID = sd.OrderId
where (o.StyleID = OrderStyle or mo.StyleID = MockupStyle) and (o.CateGory NOT IN ('G','A') or t.Category='M')   
order by style, s.OutputDate

select w.Hours
	   , w.Date
	   , style = IIF(t.Category <> 'M', OrderStyle, MockupStyle)
	   , t.SewingLineID
	   , t.FactoryID
	   , t.Shift
	   , t.Team
	   , t.OrderId
	   , t.ComboType
into #wtmp
from #tmpSewingGroup t
inner join  WorkHour w WITH (NOLOCK) on w.FactoryID = t.FactoryID 
										and w.SewingLineID = t.SewingLineID 
										and w.Date between dateadd(day,-90,t.OutputDate) and  t.OutputDate and isnull(w.Hours,0) != 0

select cumulate = IIF(Count(1)=0, 1, Count(1))
	   , s.style
	   , s.SewingLineID
	   , s.FactoryID
	   , s.Shift
	   , s.Team
	   , s.OrderId
	   , s.ComboType
into #cl
from #stmp s
where s.scOutputDate > isnull((select date = max(Date)
						from #wtmp w 
						left join #stmp s2 on s2.scOutputDate = w.Date 
											  and w.style = s2.style 
											  and w.SewingLineID = s2.SewingLineID 
											  and w.FactoryID = s2.FactoryID 
											  and w.Shift = s2.Shift 
											  and w.Team = s2.Team
											  and w.OrderId = s2.OrderId 
											  and w.ComboType = s2.ComboType
						where s2.scOutputDate is null
							  and w.style = s.style 
							  and w.SewingLineID = s.SewingLineID 
							  and w.FactoryID = s.FactoryID 
							  and w.Shift = s.Shift 
							  and w.Team = s.Team 
							  and w.OrderId = s.OrderId 
							  and w.ComboType = s.ComboType),'1900/01/01')
group by s.style, s.SewingLineID, s.FactoryID, s.Shift, s.Team
		 , s.OrderId, s.ComboType
-----↑計算累計天數
select t.*
	   , LastShift = CASE WHEN t.Shift <> 'O' and t.Category <> 'M' and t.LocalOrder = 1 and t.SubconInSisterFty = 1 then 'I'
                          WHEN t.Shift <> 'O' and t.Category <> 'M' and t.LocalOrder = 1 and t.SubconInSisterFty = 0 then 'IN'
                     ELSE t.Shift END
	   , FtyType = f.Type
	   , FtyCountry = f.CountryID
	   , CumulateDate = isnull(c.cumulate,1)
into #tmp1stFilter
from #tmpSewingGroup t
left join #cl c on c.style = IIF(t.Category <> 'M', OrderStyle, MockupStyle) 
				   and c.SewingLineID = t.SewingLineID 
				   and c.FactoryID = t.FactoryID 
				   and c.Shift = t.Shift 
				   and c.Team = t.Team 
				   and c.OrderId = t.OrderId 
				   and c.ComboType = t.ComboType
left join Factory f WITH (NOLOCK) on t.FactoryID = f.ID
---↓最後組成
select Shift =    CASE    WHEN LastShift='D' then 'Day'
                          WHEN LastShift='N' then 'Night'
                          WHEN LastShift='O' then 'Subcon-Out'
                          WHEN LastShift='I' then 'Subcon-In(Sister)'
                          else 'Subcon-In(Non Sister)' end				
	   , Team
	   , SewingLineID
	   , OrderId
	   , Style = IIF(Category='M',MockupStyle,OrderStyle) 
	   , CDNo = IIF(Category = 'M', MockupCDCodeID, OrderCdCodeID) + '-' + ComboType
	   , ActManPower = IIF(SHIFT = 'O'
                            ,MAX(IIF(QAQty > 0, ActManPower / QAQty, ActManPower)) OVER (PARTITION BY SHIFT,Team,SewingLineID)
                            ,IIF(QAQty > 0, ActManPower / QAQty, ActManPower))
	   , WorkHour
	   , ManHour = IIF(QAQty > 0, ActManPower / QAQty, ActManPower) * WorkHour
	   , TargetCPU = ROUND(ROUND(IIF(QAQty > 0, ActManPower / QAQty, ActManPower) * WorkHour, 3) * 3600 / StdTMS, 3) 
	   , TMS = IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate) * StdTMS
	   , CPUPrice = IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate)
	   , TargetQty = IIF(IIF(Category = 'M', MockupCPU * MockupCPUFactor
	   									   , OrderCPU * OrderCPUFactor * Rate) > 0
	   					    , ROUND(ROUND(IIF(QAQty > 0, ActManPower / QAQty
	   					    						   , ActManPower) * WorkHour, 2) * 3600 / StdTMS, 2) / IIF(Category = 'M', MockupCPU * MockupCPUFactor
	   					    																							     , OrderCPU * OrderCPUFactor * Rate)
						    , 0) 
	   , QAQty
	   , TotalCPU = IIF(Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate) * QAQty
	   , CPUSewer = IIF(ROUND(IIF(QAQty > 0, ActManPower / QAQty
	   									   , ActManPower) * WorkHour, 2) > 0
   							     , ROUND((IIF(Category = 'M', MockupCPU * MockupCPUFactor
   							     						    , OrderCPU * OrderCPUFactor * Rate) * QAQty), 3) / ROUND(IIF(QAQty > 0, ActManPower / QAQty
   							     																								  , ActManPower) * WorkHour, 3)
     						     , 0) 
	   , EFF = ROUND(IIF(ROUND(IIF(QAQty > 0, ActManPower / QAQty
	   										, ActManPower) * WorkHour, 2) > 0
	   						      , (ROUND(IIF(Category = 'M', MockupCPU * MockupCPUFactor
	   						      							 , OrderCPU * OrderCPUFactor * Rate) * QAQty, 2) / (ROUND(IIF(QAQty > 0, ActManPower / QAQty
	   						      							 																	   , ActManPower) * WorkHour, 2) * 3600 / StdTMS)) * 100, 0)
	   							  , 1) 
	   , RFT = IIF(ori_InlineQty = 0, 0, ROUND(ori_QAQty* 1.0 / ori_InlineQty * 1.0 * 100 ,2))
	   , CumulateDate
	   , InlineQty
	   , Diff = QAQty - InlineQty
	   , LastShift
	   , ComboType
from #tmp1stFilter
where 1 =1");

            sqlCmd.Append(@" order by LastShift,Team,SewingLineID,OrderId");
            #endregion
            result = DBProxy.Current.Select(null, sqlCmd.ToString(), out dtR01);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return;
            }

            #region 整理Total資料
            if (dtR01.Rows.Count > 0)
            {
                try
                {
                    DualResult resultTotal = MyUtility.Tool.ProcessWithDatatable(
                        dtR01,
                        "Shift,Team,SewingLineID,ActManPower,TMS,QAQty,RFT,LastShift",
                        string.Format(@"
;with SubMaxActManpower as (
	select Shift
		   , Team
		   , SewingLineID
		   , ActManPower = max(ActManPower)
	from #tmp
	group by Shift, Team, SewingLineID
),
SubSummaryData as (
	select Shift
		   , Team
		   , TMS = sum(TMS * QAQty)
		   , QAQty = sum(QAQty)
		   , RFT = AVG(RFT)
	from #tmp
	group by Shift, Team
),
SubTotal as (
	select s.Shift
		   , s.Team
		   , TMS = case 
						when s.QAQty = 0 then 0
						else (s.TMS/s.QAQty)
				   end
		   , s.RFT
		   , ActManPower = Round(sum(m.ActManPower),2)
	from SubSummaryData s 
	left join SubMaxActManpower m on s.Shift = m.Shift 
									 and s.Team = m.Team
	group by s.Shift, s.Team, s.RFT, s.TMS, s.QAQty
),
GrandIncludeInOutMaxActManpower as (
	select Shift
		   , Team
		   , SewingLineID
		   , ActManPower = max(ActManPower) 
	from #tmp
	group by Shift, Team, SewingLineID
),
GrandIncludeInOutSummaryData as (
	select TMS = sum(TMS*QAQty)
		   , QAQty = sum(QAQty)
		   , RFT = AVG(RFT)
	from #tmp
),
GenTotal1 as (
	select TMS = Case 
                    when s.QaQty = 0 then 0
                    else (s.TMS/s.QAQty)
                 end
		   , s.RFT
		   , ActManPower = sum(m.ActManPower) - sum(iif(shift = 'Subcon-In', 0, isnull(d.ActManPower,0))) 
	from GrandIncludeInOutSummaryData s
	left join GrandIncludeInOutMaxActManpower m on 1 = 1
	outer apply(
		select ActManPower
		from GrandIncludeInOutMaxActManpower m2
		where m2.Shift = 'Subcon-In' 
			  and m2.Team = m.Team 
			  and m2.SewingLineID = m.SewingLineID	
	) d
	group by s.TMS, s.QAQty, s.RFT
),
GrandExcludeOutMaxActManpower as (
	select Shift
		   , Team
		   , SewingLineID
		   , ActManPower = max(ActManPower)
	from #tmp
	where LastShift <> 'O'
	group by Shift, Team, SewingLineID
),
GrandExcludeOutSummaryData as (
	select TMS = sum(TMS * QAQty)
		   , QAQty = sum(QAQty)
		   , RFT = AVG(RFT)
	from #tmp
	where LastShift <> 'O'
),
GenTotal2 as (
	select TMS = case
                    when s.QaQty = 0 then 0
                    else (s.TMS / s.QAQty)
                 end
		   , s.RFT
		   , ActManPower = sum(m.ActManPower) - sum(iif(shift = 'Subcon-In', 0, isnull(d.ActManPower,0))) 
	from GrandExcludeOutSummaryData s
	left join GrandExcludeOutMaxActManpower m on 1 = 1
	outer apply(
		select ActManPower
		from GrandExcludeOutMaxActManpower m2
		where m2.Shift = 'Subcon-In' and m2.Team = m.Team 
									     and m2.SewingLineID = m.SewingLineID	
	) d
	group by s.TMS, s.QAQty, s.RFT
),
GrandExcludeInOutMaxActManpower as (
	select Shift
		   , Team
		   , SewingLineID
		   , ActManPower = max(ActManPower)
	from #tmp
	where LastShift <> 'O' 
	and LastShift <> 'IN' 
	group by Shift, Team, SewingLineID
),
GrandExcludeInOutSummaryData as (
	select TMS = sum(TMS*QAQty)
		   , QAQty = sum(QAQty)
		   , RFT = AVG(RFT)
	from #tmp
	where LastShift <> 'O'
	and LastShift <> 'IN' 
),
GenTotal3 as (
	select TMS = case 
                    when s.QaQty = 0 then 0
                    else (s.TMS/s.QAQty)
                 end
		   , s.RFT
		   , ActManPower = sum(m.ActManPower)
	from GrandExcludeInOutSummaryData s
	left join GrandExcludeInOutMaxActManpower m on 1 = 1
	group by s.TMS, s.QAQty, s.RFT
)
select Type = 'Sub'
	   , Sort = '1'
	   , * 
from SubTotal

union all
select Type = 'Grand'  
	   , Sort = '2' 
	   , Shift = '' 
	   , Team = ''
	   , TMS
	   , RFT
	   , ActManPower 
from GenTotal1

union all
select Type = 'Grand'
	   , Sort = '3'
	   , Shift = '' 
	   , Team = ''
	   , TMS
	   , RFT
	   , ActManPower 
from GenTotal2

union all
select Type = 'Grand'
	   , Sort = '4'
	   , Shift = ''
	   , Team = '' 
	   , TMS
	   , RFT
	   , ActManPower 
from GenTotal3"),
                        out ttlData);

                    if (resultTotal == false)
                    {
                        return;
                    }
                }
                catch (Exception ex)
                {
                    DualResult failResult = new DualResult(false, "Query total data fail\r\n" + ex.ToString());
                    return;
                }
            }
            #endregion

            #region 整理Subprocess資料
            if (dtR01.Rows.Count > 0)
            {
                try
                {
                    DualResult resultSubprocess = MyUtility.Tool.ProcessWithDatatable(
                        dtR01,
                        "OrderId,ComboType,QAQty,LastShift",
                        string.Format(@"
	--準備台北資料(須排除這些)
	select ps.ID
	into #TPEtmp
	from PO_Supp ps
	inner join PO_Supp_Detail psd on ps.ID=psd.id and ps.SEQ1=psd.Seq1
	inner join Fabric fb on psd.SCIRefno = fb.SCIRefno 
	inner join MtlType ml on ml.id = fb.MtlTypeID
	where 1=1 and ml.Junk =0 and psd.Junk=0 and fb.Junk =0
	and ml.isThread=1 
	and ps.SuppID <> 'FTY' and ps.Seq1 not Like '5%'

;with tmpArtwork as (
	Select  ID,
            [DecimalNumber] =case   when ProductionUnit = 'QTY' then 4
							        when ProductionUnit = 'TMS' then 3
							        else 0 end
	from ArtworkType WITH (NOLOCK) 
	where Classify in ('I','A','P') 
	      and IsTtlTMS = 0
          and IsPrintToCMP=1
),
tmpAllSubprocess as (
	select ot.ArtworkTypeID
		   , a.OrderId
		   , a.ComboType
           , Price = Round(sum(a.QAQty) * ot.Price * (isnull([dbo].[GetOrderLocation_Rate](o.id ,a.ComboType), 100) / 100), ta.DecimalNumber) 
	from #tmp a
	inner join Order_TmsCost ot WITH (NOLOCK) on ot.ID = a.OrderId
	inner join Orders o WITH (NOLOCK) on o.ID = a.OrderId and o.Category != 'G'
	inner join tmpArtwork ta on ta.ID = ot.ArtworkTypeID
--	left join Style_Location sl WITH (NOLOCK) on sl.StyleUkey = o.StyleUkey 
--												 and sl.Location = a.ComboType
	where ((a.LastShift = 'O' and o.LocalOrder <> 1) or (a.LastShift <> 'O')) 
          and o.LocalOrder <> 1
		  and ot.Price > 0         
		  and ((ot.ArtworkTypeID = 'SP_THREAD' and not exists(select 1 from #TPEtmp t where t.ID = o.POID))
			  or ot.ArtworkTypeID <> 'SP_THREAD')
    group by ot.ArtworkTypeID, a.OrderId, a.ComboType, ot.Price,[dbo].[GetOrderLocation_Rate](o.id ,a.ComboType),ta.DecimalNumber
)
select ArtworkTypeID
	   , Price = sum(Price)
	   , rs = iif(att.ProductionUnit = 'TMS','CPU',iif(att.ProductionUnit = 'QTY','AMT',''))
from tmpAllSubprocess t
left join ArtworkType att WITH (NOLOCK) on att.id = t.ArtworkTypeID
group by ArtworkTypeID,att.ProductionUnit
order by ArtworkTypeID"),
                        out subprocessData);

                    if (resultSubprocess == false)
                    {
                        return;
                    }
                }
                catch (Exception ex)
                {
                    DualResult failResult = new DualResult(false, "Query sub process data fail\r\n" + ex.ToString());
                    return;
                }
            }
            #endregion

            string factoryName = MyUtility.GetValue.Lookup(string.Format("select NameEN from Factory WITH (NOLOCK) where ID = '{0}'", Env.User.Factory));
            #endregion

            #region ToExcel

            string strXltName = Env.Cfg.XltPathDir + "\\Sewing_R01_DailyCMPReport.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return;
            }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            worksheet.Cells[1, 1] = factoryName;
            worksheet.Cells[2, 1] = string.Format("{0} Daily CMP Report, DD.{1} {2}", Env.User.Factory, Convert.ToDateTime(dateMaxOutputDate).ToString("MM/dd"), "(Included Subcon-IN)");

            // 沒資料就顯示空的Excel
            if (dtR01.Rows.Count > 0 && !MyUtility.Check.Empty(dtR01))
            {
                object[,] objArray = new object[1, 19];
                string[] subTtlRowInOut = new string[8];
                string[] subTtlRowExOut = new string[8];
                string[] subTtlRowExInOut = new string[8];

                string shift = MyUtility.Convert.GetString(dtR01.Rows[0]["Shift"]);
                string team = MyUtility.Convert.GetString(dtR01.Rows[0]["Team"]);

                int insertRow = 5, startRow = 5, ttlShift = 1, subRows = 0;
                worksheet.Cells[3, 1] = string.Format("{0} SHIFT: {1} Team", shift, team);
                DataRow[] selectRow;
                foreach (DataRow dr in dtR01.Rows)
                {
                    if (shift != MyUtility.Convert.GetString(dr["Shift"]) || team != MyUtility.Convert.GetString(dr["Team"]))
                    {
                        // 將多出來的Record刪除
                        for (int i = 1; i <= 2; i++)
                        {
                            Microsoft.Office.Interop.Excel.Range rng = (Microsoft.Office.Interop.Excel.Range)excel.Rows[insertRow, Type.Missing];
                            rng.Select();
                            rng.Delete(Microsoft.Office.Interop.Excel.XlDirection.xlUp);
                            Marshal.ReleaseComObject(rng);
                        }

                        // 填入Sub Total資料
                        if (ttlData != null)
                        {
                            selectRow = ttlData.Select(string.Format("Type = 'Sub' and Shift = '{0}' and  Team = '{1}'", shift, team));
                            if (selectRow.Length > 0)
                            {
                                worksheet.Cells[insertRow, 5] = MyUtility.Convert.GetDecimal(selectRow[0]["ActManPower"]);
                                worksheet.Cells[insertRow, 9] = MyUtility.Convert.GetDecimal(selectRow[0]["TMS"]);
                                worksheet.Cells[insertRow, 16] = MyUtility.Convert.GetDecimal(selectRow[0]["RFT"]);
                            }
                        }

                        worksheet.Cells[insertRow, 7] = string.Format("=SUM(G{0}:G{1})", MyUtility.Convert.GetString(startRow), MyUtility.Convert.GetString(insertRow - 1));
                        worksheet.Cells[insertRow, 8] = string.Format("=SUM(H{0}:H{1})", MyUtility.Convert.GetString(startRow), MyUtility.Convert.GetString(insertRow - 1));
                        worksheet.Cells[insertRow, 11] = string.Format("=SUM(K{0}:K{1})", MyUtility.Convert.GetString(startRow), MyUtility.Convert.GetString(insertRow - 1));
                        worksheet.Cells[insertRow, 12] = string.Format("=SUM(L{0}:L{1})", MyUtility.Convert.GetString(startRow), MyUtility.Convert.GetString(insertRow - 1));
                        worksheet.Cells[insertRow, 13] = string.Format("=SUM(M{0}:M{1})", MyUtility.Convert.GetString(startRow), MyUtility.Convert.GetString(insertRow - 1));
                        worksheet.Cells[insertRow, 14] = string.Format("=M{0}/G{0}", MyUtility.Convert.GetString(insertRow));
                        worksheet.Cells[insertRow, 15] = string.Format("=ROUND((M{0}/(G{0}*3600/1400))*100,1)", MyUtility.Convert.GetString(insertRow));
                        worksheet.Cells[insertRow, 18] = string.Format("=SUM(R{0}:R{1})", MyUtility.Convert.GetString(startRow), MyUtility.Convert.GetString(insertRow - 1));
                        worksheet.Cells[insertRow, 19] = string.Format("=SUM(S{0}:S{1})", MyUtility.Convert.GetString(startRow), MyUtility.Convert.GetString(insertRow - 1));

                        subTtlRowInOut[subRows] = MyUtility.Convert.GetString(insertRow);
                        if (shift != "Subcon-Out")
                        {
                            subTtlRowExOut[subRows] = MyUtility.Convert.GetString(insertRow);
                        }

                        if (shift != "Subcon-Out" && shift != "Subcon-In(Non Sister)")
                        {
                            subTtlRowExInOut[subRows] = MyUtility.Convert.GetString(insertRow);
                        }

                        // 重置參數資料
                        shift = MyUtility.Convert.GetString(dr["Shift"]);
                        team = MyUtility.Convert.GetString(dr["Team"]);
                        worksheet.Cells[insertRow + 2, 1] = string.Format("{0} SHIFT: {1} Team", shift, team);
                        insertRow = insertRow + 4;
                        startRow = insertRow;
                        ttlShift++;
                        subRows++;
                    }

                    objArray[0, 0] = dr["SewingLineID"];
                    objArray[0, 1] = dr["OrderId"];
                    objArray[0, 2] = dr["Style"];
                    objArray[0, 3] = dr["CDNo"];
                    objArray[0, 4] = dr["ActManPower"];
                    objArray[0, 5] = dr["WorkHour"];
                    objArray[0, 6] = dr["ManHour"];
                    objArray[0, 7] = dr["TargetCPU"];
                    objArray[0, 8] = dr["TMS"];
                    objArray[0, 9] = dr["CPUPrice"];
                    objArray[0, 10] = dr["TargetQty"];
                    objArray[0, 11] = dr["QAQty"];
                    objArray[0, 12] = dr["TotalCPU"];
                    objArray[0, 13] = dr["CPUSewer"];
                    objArray[0, 14] = string.Format("=ROUND((M{0}/(G{0}*3600/1400))*100,1)", insertRow);
                    objArray[0, 15] = dr["RFT"];
                    objArray[0, 16] = dr["CumulateDate"];
                    objArray[0, 17] = dr["InlineQty"];
                    objArray[0, 18] = dr["Diff"];
                    worksheet.Range[string.Format("A{0}:S{0}", insertRow)].Value2 = objArray;
                    insertRow++;

                    // 插入一筆Record
                    Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(insertRow)), Type.Missing).EntireRow;
                    rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                    Marshal.ReleaseComObject(rngToInsert);
                }

                // 最後一個Shift資料
                // 將多出來的Record刪除
                for (int i = 1; i <= 2; i++)
                {
                    Microsoft.Office.Interop.Excel.Range rng = (Microsoft.Office.Interop.Excel.Range)excel.Rows[insertRow, Type.Missing];
                    rng.Select();
                    rng.Delete(Microsoft.Office.Interop.Excel.XlDirection.xlUp);
                    Marshal.ReleaseComObject(rng);
                }

                // 填入Sub Total資料
                if (ttlData != null)
                {
                    selectRow = ttlData.Select(string.Format("Type = 'Sub' and Shift = '{0}' and  Team = '{1}'", shift, team));
                    if (selectRow.Length > 0)
                    {
                        worksheet.Cells[insertRow, 5] = MyUtility.Convert.GetDecimal(selectRow[0]["ActManPower"]);
                        worksheet.Cells[insertRow, 9] = MyUtility.Convert.GetDecimal(selectRow[0]["TMS"]);
                        worksheet.Cells[insertRow, 16] = MyUtility.Convert.GetDecimal(selectRow[0]["RFT"]);
                    }
                }

                worksheet.Cells[insertRow, 7] = string.Format("=SUM(G{0}:G{1})", MyUtility.Convert.GetString(startRow), MyUtility.Convert.GetString(insertRow - 1));
                worksheet.Cells[insertRow, 8] = string.Format("=SUM(H{0}:H{1})", MyUtility.Convert.GetString(startRow), MyUtility.Convert.GetString(insertRow - 1));
                worksheet.Cells[insertRow, 11] = string.Format("=SUM(K{0}:K{1})", MyUtility.Convert.GetString(startRow), MyUtility.Convert.GetString(insertRow - 1));
                worksheet.Cells[insertRow, 12] = string.Format("=SUM(L{0}:L{1})", MyUtility.Convert.GetString(startRow), MyUtility.Convert.GetString(insertRow - 1));
                worksheet.Cells[insertRow, 13] = string.Format("=SUM(M{0}:M{1})", MyUtility.Convert.GetString(startRow), MyUtility.Convert.GetString(insertRow - 1));
                worksheet.Cells[insertRow, 14] = string.Format("=M{0}/G{0}", MyUtility.Convert.GetString(insertRow));
                worksheet.Cells[insertRow, 15] = string.Format("=ROUND((M{0}/(G{0}*3600/1400))*100,1)", MyUtility.Convert.GetString(insertRow));
                worksheet.Cells[insertRow, 18] = string.Format("=SUM(R{0}:R{1})", MyUtility.Convert.GetString(startRow), MyUtility.Convert.GetString(insertRow - 1));
                worksheet.Cells[insertRow, 19] = string.Format("=SUM(S{0}:S{1})", MyUtility.Convert.GetString(startRow), MyUtility.Convert.GetString(insertRow - 1));
                subTtlRowInOut[subRows] = MyUtility.Convert.GetString(insertRow);
                if (shift != "Subcon-Out")
                {
                    subTtlRowExOut[subRows] = MyUtility.Convert.GetString(insertRow);
                }

                if (shift != "Subcon-Out" && shift != "Subcon-In(Non Sister)")
                {
                    subTtlRowExInOut[subRows] = MyUtility.Convert.GetString(insertRow);
                }

                // 刪除多出來的Shift Record
                for (int i = 1; i <= (8 - ttlShift) * 6; i++)
                {
                    Microsoft.Office.Interop.Excel.Range rng = (Microsoft.Office.Interop.Excel.Range)excel.Rows[insertRow + 1, Type.Missing];
                    rng.Select();
                    rng.Delete(Microsoft.Office.Interop.Excel.XlDirection.xlUp);
                    Marshal.ReleaseComObject(rng);
                }

                insertRow += 2;

                // 填Grand Total資料
                string ttlManhour, targetCPU, targetQty, qaQty, ttlCPU, prodOutput, diff;
                if (ttlData != null)
                {
                    selectRow = ttlData.Select("Type = 'Grand'");
                    if (selectRow.Length > 0)
                    {
                        for (int i = 0; i < selectRow.Length; i++)
                        {
                            worksheet.Cells[insertRow, 5] = MyUtility.Convert.GetDecimal(selectRow[i]["ActManPower"]);
                            worksheet.Cells[insertRow, 9] = MyUtility.Convert.GetDecimal(selectRow[i]["TMS"]);
                            worksheet.Cells[insertRow, 16] = MyUtility.Convert.GetDecimal(selectRow[i]["RFT"]);
                            ttlManhour = "=";
                            targetCPU = "=";
                            targetQty = "=";
                            qaQty = "=";
                            ttlCPU = "=";
                            prodOutput = "=";
                            diff = "=";
                            #region 組公式
                            if (MyUtility.Convert.GetString(selectRow[i]["Sort"]) == "2")
                            {
                                for (int j = 0; j < 8; j++)
                                {
                                    if (!MyUtility.Check.Empty(subTtlRowInOut[j]))
                                    {
                                        ttlManhour = ttlManhour + string.Format("G{0}+", subTtlRowInOut[j]);
                                        targetCPU = targetCPU + string.Format("H{0}+", subTtlRowInOut[j]);
                                        targetQty = targetQty + string.Format("K{0}+", subTtlRowInOut[j]);
                                        qaQty = qaQty + string.Format("L{0}+", subTtlRowInOut[j]);
                                        ttlCPU = ttlCPU + string.Format("M{0}+", subTtlRowInOut[j]);
                                        prodOutput = prodOutput + string.Format("R{0}+", subTtlRowInOut[j]);
                                        diff = diff + string.Format("S{0}+", subTtlRowInOut[j]);
                                    }
                                }
                            }
                            else if (MyUtility.Convert.GetString(selectRow[i]["Sort"]) == "3")
                            {
                                for (int j = 0; j < 8; j++)
                                {
                                    if (!MyUtility.Check.Empty(subTtlRowExOut[j]))
                                    {
                                        ttlManhour = ttlManhour + string.Format("G{0}+", subTtlRowExOut[j]);
                                        targetCPU = targetCPU + string.Format("H{0}+", subTtlRowExOut[j]);
                                        targetQty = targetQty + string.Format("K{0}+", subTtlRowExOut[j]);
                                        qaQty = qaQty + string.Format("L{0}+", subTtlRowExOut[j]);
                                        ttlCPU = ttlCPU + string.Format("M{0}+", subTtlRowExOut[j]);
                                        prodOutput = prodOutput + string.Format("R{0}+", subTtlRowExOut[j]);
                                        diff = diff + string.Format("S{0}+", subTtlRowExOut[j]);
                                    }
                                }
                            }
                            else
                            {
                                for (int j = 0; j < 8; j++)
                                {
                                    if (!MyUtility.Check.Empty(subTtlRowExInOut[j]))
                                    {
                                        ttlManhour = ttlManhour + string.Format("G{0}+", subTtlRowExInOut[j]);
                                        targetCPU = targetCPU + string.Format("H{0}+", subTtlRowExInOut[j]);
                                        targetQty = targetQty + string.Format("K{0}+", subTtlRowExInOut[j]);
                                        qaQty = qaQty + string.Format("L{0}+", subTtlRowExInOut[j]);
                                        ttlCPU = ttlCPU + string.Format("M{0}+", subTtlRowExInOut[j]);
                                        prodOutput = prodOutput + string.Format("R{0}+", subTtlRowExInOut[j]);
                                        diff = diff + string.Format("S{0}+", subTtlRowExInOut[j]);
                                    }
                                }
                            }
                            #endregion

                            worksheet.Cells[insertRow, 7] = ttlManhour.Substring(0, ttlManhour.Length - 1);
                            worksheet.Cells[insertRow, 8] = targetCPU.Substring(0, targetCPU.Length - 1);
                            worksheet.Cells[insertRow, 11] = targetQty.Substring(0, targetQty.Length - 1);
                            worksheet.Cells[insertRow, 12] = qaQty.Substring(0, qaQty.Length - 1);
                            worksheet.Cells[insertRow, 13] = ttlCPU.Substring(0, ttlCPU.Length - 1);
                            worksheet.Cells[insertRow, 14] = string.Format("=M{0}/G{0}", MyUtility.Convert.GetString(insertRow));
                            worksheet.Cells[insertRow, 15] = string.Format("=ROUND((M{0}/(G{0}*3600/1400))*100,1)", MyUtility.Convert.GetString(insertRow));
                            worksheet.Cells[insertRow, 18] = prodOutput.Substring(0, prodOutput.Length - 1);
                            worksheet.Cells[insertRow, 19] = diff.Substring(0, diff.Length - 1);
                            insertRow++;
                        }
                    }
                }
                #region Direct Manpower(From PAMS)
                if (Env.User.Keyword.EqualString("CM1") ||
                    Env.User.Keyword.EqualString("CM2") ||
                    Env.User.Keyword.EqualString("CM3"))
                {
                    worksheet.Cells[insertRow, 5] = 0;
                    worksheet.Cells[insertRow, 7] = 0;
                }
                else
                {
                    dataMode = new List<APIData>();
                    GetApiData.GetAPIData(string.Empty, Env.User.Factory, (DateTime)DateTime.Now.AddDays(-1), (DateTime)DateTime.Now.AddDays(-1), out dataMode);
                    if (dataMode != null)
                    {
                        worksheet.Cells[insertRow, 5] = dataMode[0].SewTtlManpower;
                        worksheet.Cells[insertRow, 7] = dataMode[0].SewTtlManhours;
                    }

                    insertRow++;
                }
                #endregion

                insertRow = insertRow + 2;
                foreach (DataRow dr in subprocessData.Rows)
                {
                    worksheet.Cells[insertRow, 3] = string.Format("{0}{1}", MyUtility.Convert.GetString(dr["ArtworkTypeID"]).PadRight(20, ' '), MyUtility.Convert.GetString(dr["rs"]));
                    worksheet.Cells[insertRow, 6] = MyUtility.Convert.GetString(dr["Price"]);
                    insertRow++;

                    // 插入一筆Record
                    Microsoft.Office.Interop.Excel.Range rngToInsert = worksheet.get_Range(string.Format("A{0}:A{0}", MyUtility.Convert.GetString(insertRow)), Type.Missing).EntireRow;
                    rngToInsert.Insert(Microsoft.Office.Interop.Excel.XlInsertShiftDirection.xlShiftDown);
                    Marshal.ReleaseComObject(rngToInsert);
                }
            }

            excel.Visible = false;
            #region Save & Show Excel
            string excelFileR01 = Path.Combine(
                Env.Cfg.ReportTempDir,
                "Daily CMP Report" + ((DateTime)dateMaxOutputDate).ToString("_yyyyMMdd") + DateTime.Now.ToString("_HHmmssfff") + "(" + Env.User.Factory + ").xlsx");
            excel.ActiveWorkbook.SaveAs(excelFileR01);
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);
            #endregion

            #endregion

            #endregion

            #region 產生R04 報表
            DataTable dtR04;
            string sqlcmd = $"exec [dbo].[Send_SewingDailyOutput] '{Env.User.Factory}', '{Convert.ToDateTime(dateMaxOutputDate).ToString("d")}'";
            result = DBProxy.Current.Select(string.Empty, sqlcmd, out dtR04);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return;
            }

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Sewing_R04_SewingDailyOutputList.xltx"); // 預先開啟excel app
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得
            objSheets.get_Range("AM:AN").EntireColumn.Delete();
            for (int i = 40; i < dtR04.Columns.Count; i++)
            {
                objSheets.Cells[1, i + 1] = dtR04.Columns[i].ColumnName;
            }

            string r = MyUtility.Excel.ConvertNumericToExcelColumn(dtR04.Columns.Count);
            objSheets.get_Range("A1", r + "1").Cells.Interior.Color = Color.LightGreen;
            objSheets.get_Range("A1", r + "1").AutoFilter(1);
            objApp.Visible = false;

            if (dtR04.Rows.Count != 0)
            {
                MyUtility.Excel.CopyToXls(dtR04, string.Empty, "Sewing_R04_SewingDailyOutputList.xltx", 1, false, null, objApp);
            }

            string excelFileR04 = Path.Combine(
                Env.Cfg.ReportTempDir,
                "Sewing daily output list -" + ((DateTime)dateMaxOutputDate).ToString("_yyyyMMdd") + DateTime.Now.ToString("_HHmmssfff") + "(" + Env.User.Factory + ").xlsx");
            objApp.ActiveWorkbook.SaveAs(excelFileR04);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            Marshal.ReleaseComObject(objSheets);
            #endregion

            #region SendMail
            if (!MyUtility.Check.Empty(excelFileR01) && !MyUtility.Check.Empty(excelFileR04))
            {
                DataRow dr, drMail;
                if (MyUtility.Check.Seek("select * from system", out dr))
                {
                    string desc = $@"
Hi all,
     Output date: {Convert.ToDateTime(dateMaxOutputDate).ToString("d")} Factory: {Env.User.Factory} sewing output data is already daily lock, these attachments are system generated from Sewing R01(Daily CMP Report) and Sewing R04(Sewing Daily Output List). This mail is automatically sent, please do not reply directly.
      
";
                    if (MyUtility.Check.Seek("select * from mailto where id='020'", out drMail))
                    {
                        List<string> attachFiles = new List<string>();
                        attachFiles.Add(excelFileR01);
                        attachFiles.Add(excelFileR04);

                        string subject = drMail["Subject"].ToString() + $@"{Convert.ToDateTime(dateMaxOutputDate).ToString("d")} ({Env.User.Factory})";
                        MailTo mail = new MailTo(dr["SendFrom"].ToString(), drMail["ToAddress"].ToString(), drMail["ccAddress"].ToString(), subject, desc, attachFiles, true, true);
                        mail.ShowDialog();
                    }
                }
            }

            #endregion

            MyUtility.Msg.InfoBox("Lock data successfully!");
        }

        private DualResult GetDQSDataForDetail(out DataTable sewDt1)
        {
            string shift = this.CurrentMaintain["Shift"].EqualString("D") ? "Day" : this.CurrentMaintain["Shift"].EqualString("N") ? "Night" : string.Empty;
            string frommes = $@"
select
	OrderId
	, Article
	, ComboType=Location 
	, ColorID=''
	, TMS=0
	, HourlyStandardOutput = 0
	, [QAQty] = sum(iif(ins.Status in ('Pass','Fixed'),1,0))
	, [DefectQty] = sum(iif(ins.Status in ('Reject','Dispose'),1,0))
	, [InlineQty] = count(1)
	, [ImportFromDQS] = 1
	, [AutoCreate] = 0
from inspection ins WITH (NOLOCK)
where InspectionDate= '{((DateTime)this.CurrentMaintain["OutputDate"]).ToString("d")}'
and FactoryID = '{this.CurrentMaintain["FactoryID"]}'
and Line = '{this.CurrentMaintain["SewingLineID"]}'
and Team = '{this.CurrentMaintain["Team"]}'
and Shift = '{shift}'
group by InspectionDate, FactoryID, Line, Shift, Team, OrderId, Article, Location
";
            DualResult result = DBProxy.Current.Select("ManufacturingExecution", frommes, out sewDt1);
            if (!result)
            {
                return result;
            }

            if (sewDt1.Rows.Count == 0)
            {
                return Ict.Result.F("DQS Data not found!");
            }

            string sqlcmd = $@"
select t.OrderId
,t.Article
,t.ComboType
,Color = (
    select ColorID 
    from View_OrderFAColor vof with(nolock) 
    where vof.id=t.OrderId and vof.Article=t.Article
)
,TMS = iif(O_Location.Value = 0, S_Location.value,O_Location.value)
,HourlyStandardOutput = 
    (   select top 1 ss.StandardOutput 
        from SewingSchedule ss WITH (NOLOCK) 
        where ss.OrderID = t.OrderID 
        and ss.ComboType = t.ComboType 
        and ss.SewingLineID = '{this.CurrentMaintain["SewingLineID"]}')
,t.QAQty,t.DefectQty,t.InlineQty,t.ImportFromDQS,t.AutoCreate
,ukey = 0
,RFT = CONVERT(VARCHAR, convert(Decimal(5, 2), round((t.InlineQty - t.DefectQty) /  cast(t.InlineQty as decimal) * 100.0, 2))) + '%'
,ID = '{this.CurrentMaintain["ID"]}'
from #tmp t
left join orders o  with(nolock) on o.id = t.OrderId
outer apply(
    select value = ROUND(
    isnull(o.cpu,0) * isnull(o.CPUFactor,0) * 
	isnull(
            (select Rate = [dbo].[GetStyleLocation_Rate](sl.StyleUkey,sl.Location)
			from Style_Location sl WITH (NOLOCK) 
            where sl.StyleUkey = o.StyleUkey 
            and sl.Location = t.ComboType)
            ,0)
	    /100 * (select StdTMS from System WITH (NOLOCK))
    ,0)
)S_Location
outer apply(
    select value = ROUND(
    isnull(o.cpu,0) * isnull(o.CPUFactor,0) * 
	isnull(
            (select Rate = [dbo].[GetOrderLocation_Rate](t.OrderId,ol.Location)
			from Order_Location ol WITH (NOLOCK) 
            where ol.OrderID = o.id 
            and ol.Location = t.ComboType)
            ,0)
	    /100 * (select StdTMS from System WITH (NOLOCK))
    ,0)
)O_Location
";
            result = MyUtility.Tool.ProcessWithDatatable(sewDt1, string.Empty, sqlcmd, out sewDt1);
            if (!result)
            {
                return result;
            }

            return Ict.Result.True;
        }

        private DualResult GetDQSDataForDetail_Detail(DataRow item, out DataTable sewDt2)
        {
            string shift = this.CurrentMaintain["Shift"].EqualString("D") ? "Day" : this.CurrentMaintain["Shift"].EqualString("N") ? "Night" : string.Empty;
            string frommes = $@"
select
    ID = '{this.CurrentMaintain["ID"]}'
    , SewingOutput_DetailUKey='{item["ukey"]}'
	, OrderId
	, ComboType=Location 
	, Article
	, SizeCode=Size
	, [QAQty] = sum(iif(ins.Status in ('Pass','Fixed'),1,0))
    , [ExistsSunriseNid] = sum(iif(ins.SunriseNid = 0,0,1))
from inspection ins WITH (NOLOCK)
where InspectionDate= '{((DateTime)this.CurrentMaintain["OutputDate"]).ToString("d")}'
and FactoryID = '{this.CurrentMaintain["FactoryID"]}'
and Line = '{this.CurrentMaintain["SewingLineID"]}'
and Team = '{this.CurrentMaintain["Team"]}'
and Shift = '{shift}'
and Article = '{item["Article"]}'
and Location = '{item["ComboType"]}'
and OrderId = '{item["OrderId"]}'
group by InspectionDate, FactoryID, Line, Shift, Team, OrderId, Article, Location,Size
";
            DualResult result = DBProxy.Current.Select("ManufacturingExecution", frommes, out sewDt2);
            if (!result)
            {
                return result;
            }

            string sqlcmd = $@"
with AllQty as (
    select 
            ID = '{this.CurrentMaintain["ID"]}'
           , SewingOutput_DetailUkey = '{item["UKey"]}'
           , oq.ID as OrderId
           , ComboType = '{item["ComboType"]}'
           , oq.Article
           , oq.SizeCode
           , oq.Qty as OrderQty
           , QAQty  = isnull(t.QAQty,0)
           , AccumQty = isnull((select sum(QAQty) 
                                from SewingOutput_Detail_Detail WITH (NOLOCK) 
                                where OrderId = oq.ID 
                                      and ComboType = '{item["ComboType"]}'
                                      and Article = oq.Article 
                                      and SizeCode = oq.SizeCode
                                      and ID != '{this.CurrentMaintain["ID"]}'), 0) 
           , [ExistsSunriseNid] = isnull(t.ExistsSunriseNid,0)
    from Order_Qty oq WITH (NOLOCK) 
    left join #tmp t on oq.id = t.Orderid and oq.Article = t.Article and oq.SizeCode = t.SizeCode
    where oq.ID = '{item["Orderid"]}'
          and oq.Article = '{item["Article"]}'
)
select a.ID,a.SewingOutput_DetailUkey,a.OrderId,a.ComboType,a.Article,a.SizeCode,a.OrderQty
       , Last.QAQty
       , a.AccumQty
       , OrderQty.OrderQtyUpperlimit
       , a.OrderQty - a.AccumQty as Variance
       , a.OrderQty - a.AccumQty - Last.QAQty as BalQty
       , isnull(os.Seq,0) as Seq
       , OldDetailKey = ''
       , DQSQAQty = a.QAQty
       , a.ExistsSunriseNid
from AllQty a
left join Orders o WITH (NOLOCK) on a.OrderId = o.ID
left join Order_SizeCode os WITH (NOLOCK) on os.Id = o.POID 
    and os.SizeCode = a.SizeCode
outer apply(
	select value=1
	from Order_TmsCost ot with(nolock)
	inner join Order_Qty oq WITH (NOLOCK) on ot.id = oq.ID
	where ot.ArtworkTypeID = 'Garment Dye' and ot.Price > 0
	and oq.SizeCode=os.SizeCode and oq.Article=a.Article and ot.id=o.id
	and o.LocalOrder<>1
)b
outer apply(select OrderQtyUpperlimit=iif(b.value is not null,round(cast(a.OrderQty as decimal)* (1+ isnull(o.DyeingLoss,0)/100),0),a.OrderQty))OrderQty
outer apply(select QAQty=iif(OrderQty.OrderQtyUpperlimit - a.AccumQty < a.QAQty, OrderQty.OrderQtyUpperlimit - a.AccumQty, a.QAQty))Last
order by a.OrderId,os.Seq
";
            result = MyUtility.Tool.ProcessWithDatatable(sewDt2, string.Empty, sqlcmd, out sewDt2);
            if (!result)
            {
                return result;
            }

            return Ict.Result.True;
        }

        private void FromDQS()
        {
            if (this.CurrentMaintain == null || !this.EditMode || !this.IsDetailInserting)
            {
                return;
            }

            this.CurrentMaintain.EndEdit();

            if (MyUtility.Check.Empty(this.CurrentMaintain["OutputDate"]) ||
                MyUtility.Check.Empty(this.CurrentMaintain["FactoryID"]) ||
                MyUtility.Check.Empty(this.CurrentMaintain["SewingLineID"]) ||
                MyUtility.Check.Empty(this.CurrentMaintain["Team"]) ||
                MyUtility.Check.Empty(this.CurrentMaintain["Shift"]))
            {
                return;
            }

            DataTable sewDt1;
            DualResult result = this.GetDQSDataForDetail(out sewDt1);
            if (!result)
            {
                if (result.Messages.Count == 0)
                {
                    MyUtility.Msg.InfoBox("DQS Data not found!");
                }
                else
                {
                    this.ShowErr(result);
                }

                return;
            }

            string frommes = string.Empty;
            string sqlcmd = string.Empty;
            string shift = this.CurrentMaintain["Shift"].EqualString("D") ? "Day" : this.CurrentMaintain["Shift"].EqualString("N") ? "Night" : string.Empty;

            // 先用刪除原本表身下的第3層, 不用.Clear()
            DataTable subDetailData;
            foreach (DataRow dr in this.DetailDatas)
            {
                this.GetSubDetailDatas(dr, out subDetailData);
                for (int i = subDetailData.Rows.Count - 1; i >= 0; i--)
                {
                    subDetailData.Rows[i].Delete();
                }
            }

            // 刪除表身, 不用.Clear()
            for (int i = this.DetailDatas.Count - 1; i >= 0; i--)
            {
                this.DetailDatas[i].Delete();
            }

            // 加入表身
            foreach (DataRow row in sewDt1.Rows)
            {
                ((DataTable)this.detailgridbs.DataSource).ImportRow(row);
            }

            List<string> remarkList = new List<string>();
            foreach (DataRow item in this.DetailDatas)
            {
                DataTable sewDt2;
                result = this.GetDQSDataForDetail_Detail(item, out sewDt2);
                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }

                // 組remark
                List<string> remarkList2 = new List<string>(); // 填入第2層用
                foreach (DataRow dataRow in sewDt2.Rows)
                {
                    int dQSQAQty = (int)MyUtility.Convert.GetDecimal(dataRow["DQSQAQty"]);
                    int qAQtyn = (int)MyUtility.Convert.GetDecimal(dataRow["QAQty"]);
                    int existsSunriseNid = (int)MyUtility.Convert.GetDecimal(dataRow["ExistsSunriseNid"]);

                    if (dQSQAQty > qAQtyn)
                    {
                        remarkList.Add($@"SP#{dataRow["OrderId"]}, Size:{dataRow["SizeCode"]} / DQS Q'ty : {dQSQAQty} / Bal QA Q'ty : {qAQtyn}");
                    }

                    // 若有含非Endline資料就不做自動填入reason
                    if (dQSQAQty > qAQtyn && existsSunriseNid == 0)
                    {
                        remarkList2.Add($@"SP#{dataRow["OrderId"]}, Size:{dataRow["SizeCode"]} / DQS Q'ty : {dQSQAQty} / Bal QA Q'ty : {qAQtyn}");
                    }
                }

                sewDt2.Columns.Remove("DQSQAQty");
                this.GetSubDetailDatas(item, out subDetailData);
                foreach (DataRow ddr in sewDt2.Rows)
                {
                    if (!subDetailData.AsEnumerable().AsEnumerable().Where(w => w.RowState != DataRowState.Deleted)
                                                               .Any(row => row["ID"].EqualString(ddr["ID"])
                                                                && row["SewingOutput_DetailUkey"].EqualString(ddr["SewingOutput_DetailUkey"])
                                                                && row["OrderID"].EqualString(ddr["OrderID"])
                                                                && row["ComboType"].EqualString(ddr["ComboType"])
                                                                && row["Article"].EqualString(ddr["Article"])
                                                                && row["SizeCode"].EqualString(ddr["SizeCode"])))
                    {
                        DataRow newDr = subDetailData.NewRow();
                        for (int i = 0; i < subDetailData.Columns.Count; i++)
                        {
                            newDr[subDetailData.Columns[i].ColumnName] = ddr[subDetailData.Columns[i].ColumnName];
                        }

                        subDetailData.Rows.Add(newDr);
                    }
                }

                StringBuilder qAOutput = new StringBuilder();
                int qAQty = 0;

                foreach (DataRow dr in subDetailData.Rows)
                {
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        if (MyUtility.Convert.GetString(dr["SewingOutput_DetailUKey"]) == MyUtility.Convert.GetString(item["UKey"]) && !MyUtility.Check.Empty(dr["QAQty"]))
                        {
                            qAOutput.Append(string.Format("{0}*{1},", MyUtility.Convert.GetString(dr["SizeCode"]), MyUtility.Convert.GetString(dr["QAQty"])));
                            qAQty = qAQty + MyUtility.Convert.GetInt(dr["QAQty"]);
                        }
                    }
                }

                item["QAOutput"] = qAOutput.Length > 0 ? qAOutput.ToString() : string.Empty;

                // 總計第三層 Qty 填入第二層 QAQty
                item["QAQty"] = qAQty;

                if (qAQty == 0)
                {
                    item["InlineQty"] = 0;
                }
                else
                {
                    item["InlineQty"] = item["RFT"].ToString().Substring(0, 4) == "0.00" ? qAQty : qAQty /
                        (decimal.Parse(item["RFT"].ToString().Substring(0, 4)) / 100);
                }

                this.CalculateDefectQty(item);

                string remark = string.Empty;
                if (remarkList2.Count > 0)
                {
                    remark = string.Join("\r\n", remarkList2) + "\r\n" + "DQS Pass Q'ty is more than balance, please inform related team";
                    string sewingReasonID = "00012";
                    string sqlCmd = $"SELECT DISTINCT ID,Description FROM SewingReason WHERE Type='SO' AND Junk=0 AND ID ='{sewingReasonID}'  -- SO代表SewingOutput";
                    DataTable reasonDatas;
                    result = DBProxy.Current.Select(null, sqlCmd, out reasonDatas);
                    if (!result)
                    {
                        this.ShowErr(result);
                    }

                    if (reasonDatas.Rows.Count > 0)
                    {
                        item["SewingReasonID"] = sewingReasonID;
                        item["ReasonDescription"] = reasonDatas.Rows[0]["Description"];
                    }

                    if (remark.Length > 1000)
                    {
                        item["remark"] = remark.Substring(0, 1000);
                    }
                    else
                    {
                        item["remark"] = remark;
                    }
                }

                // 將第2層重新設定為新增狀態
                item.AcceptChanges();
                item.SetAdded();
            }

            string msg = string.Empty;
            if (remarkList.Count > 0)
            {
                msg = string.Join("\r\n", remarkList) + "\r\n" + "DQS Pass Q'ty is more than balance, please inform related team";
                MyUtility.Msg.WarningBox(msg);
            }

            // 總計第二層 Qty 填入第一層 QAQty
            this.CurrentMaintain["QAQty"] = ((DataTable)this.detailgridbs.DataSource).AsEnumerable().Where(row => row.RowState != DataRowState.Deleted
                                                                                                                 && row["AutoCreate"].EqualString("False")).CopyToDataTable().Compute("SUM(QAQty)", string.Empty);
            this.CurrentMaintain["InlineQty"] = MyUtility.Convert.GetInt(this.CurrentMaintain["QAQty"]) + MyUtility.Convert.GetInt(this.CurrentMaintain["DefectQty"]);

            string rftfrommes = $@"
select t.OrderId
	, CDate='{((DateTime)this.CurrentMaintain["OutputDate"]).ToString("d")}'
	, SewinglineID='{this.CurrentMaintain["SewingLineID"]}'
	, FactoryID = '{this.CurrentMaintain["FactoryID"]}'
	, InspectQty= t.InlineQty - DiffInspectQty.Qty
	, RejectQty= RejectData.Qty--t.DefectQty
	, [DefectQty] = DefectData.Qty
	, Shift='{this.CurrentMaintain["Shift"]}'
	, Team = '{this.CurrentMaintain["Team"]}'
	, Status='New'
	, Remark=''
    ,t.Article,t.ComboType
INTO #tmp2
from #tmp t
outer apply(
	select Qty=count(*)
	from Inspection ins with (nolock)
    inner join Inspection_Detail id with(nolock) on ins.id = id.id
	where InspectionDate= '{((DateTime)this.CurrentMaintain["OutputDate"]).ToString("d")}'
           and ins.FactoryID = '{this.CurrentMaintain["FactoryID"]}'
           and ins.Line = '{this.CurrentMaintain["SewingLineID"]}'
           and ins.Team = '{this.CurrentMaintain["Team"]}'
           and ins.Shift = '{shift}' 
           and ins.Article = t.Article
           and ins.Location = t.ComboType
           and ins.OrderId = t.OrderId            
           and (ins.Status <> 'Fixed'  or (ins.Status = 'Fixed' and cast(ins.AddDate as date) = ins.InspectionDate))
           and ins.SunriseNid = 0
) DefectData
outer apply(
    -- 最後計算RFT 排除Fixed，但若同一天被Reject又被修好這時候也要抓進來並算reject。
    select Qty=count(*)
	from Inspection ins with (nolock)
	where InspectionDate= '{((DateTime)this.CurrentMaintain["OutputDate"]).ToString("d")}'
           and ins.FactoryID = '{this.CurrentMaintain["FactoryID"]}'
           and ins.Line = '{this.CurrentMaintain["SewingLineID"]}'
           and ins.Team = '{this.CurrentMaintain["Team"]}'
           and ins.Shift = '{shift}' 
           and ins.Article = t.Article
           and ins.Location = t.ComboType
           and ins.OrderId = t.OrderId
           and not (ins.Status <> 'Fixed'  or (ins.Status = 'Fixed' and cast(ins.AddDate as date) = ins.InspectionDate))
           and ins.SunriseNid = 0
) DiffInspectQty
outer apply(
    -- 最後計算RFT 排除Fixed，但若同一天被Reject又被修好這時候也要抓進來並算reject。
    select Qty=count(*)
	from Inspection ins with (nolock)
	where InspectionDate= '{((DateTime)this.CurrentMaintain["OutputDate"]).ToString("d")}'
           and ins.FactoryID = '{this.CurrentMaintain["FactoryID"]}'
           and ins.Line = '{this.CurrentMaintain["SewingLineID"]}'
           and ins.Team = '{this.CurrentMaintain["Team"]}'
           and ins.Shift = '{shift}' 
           and ins.Article = t.Article
           and ins.Location = t.ComboType
           and ins.OrderId = t.OrderId
           --and not (ins.Status <> 'Fixed'  or (ins.Status = 'Fixed' and cast(ins.AddDate as date) = ins.InspectionDate))
           and ins.Status IN ('Fixed','Reject','Dispode')
           and cast(ins.AddDate as date) = ins.InspectionDate
           and ins.SunriseNid = 0
) RejectData



SELECT  OrderId
	, CDate
	, SewinglineID
	, FactoryID 
	, [InspectQty]=SUM(InspectQty)
	, [RejectQty]= SUM(RejectQty)
	, [DefectQty] =SUM(DefectQty)
	, Shift
	, Team
	, Status
	, Remark
FROM #tmp2
GROUP BY  OrderId, CDate, SewinglineID, FactoryID , Shift, Team, Status, Remark


drop table #tmp,#tmp2
";

            using (SqlConnection mesConn = new SqlConnection(Env.Cfg.GetConnection("ManufacturingExecution", DBProxy.Current.DefaultModuleName).ConnectionString))
            {
                mesConn.Open();
                result = MyUtility.Tool.ProcessWithDatatable(
                    (DataTable)this.detailgridbs.DataSource,
                    string.Empty,
                    rftfrommes,
                    out this.rftDT,
                    conn: mesConn);
                mesConn.Close();
                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }
            }
        }

        private void TxtsewinglineLine_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode && this.txtsewinglineLine.Text != this.txtsewinglineLine.OldValue)
            {
                this.CurrentMaintain["SewingLineID"] = this.txtsewinglineLine.Text;
                this.CurrentMaintain.EndEdit();
                this.FromDQS();
            }
        }

        private void ComboTeam_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.EditMode && this.comboTeam.Text != MyUtility.Convert.GetString(this.comboTeam.OldValue))
            {
                this.CurrentMaintain["Team"] = this.comboTeam.Text;
                this.CurrentMaintain.EndEdit();
                this.FromDQS();
            }
        }
    }
}
