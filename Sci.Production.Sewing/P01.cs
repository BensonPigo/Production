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

namespace Sci.Production.Sewing
{
    /// <summary>
    /// P01
    /// </summary>
    public partial class P01 : Sci.Win.Tems.Input8
    {
        private DataTable dtQACheck;
        private ITableSchema sub_Schema;
        private Ict.Win.DataGridViewGeneratorTextColumnSettings qaoutput = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
        private Ict.Win.DataGridViewGeneratorTextColumnSettings orderid = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
        private Ict.Win.DataGridViewGeneratorTextColumnSettings combotype = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
        private Ict.Win.DataGridViewGeneratorTextColumnSettings article = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
        private Ict.Win.DataGridViewGeneratorNumericColumnSettings inlineqty = new Ict.Win.DataGridViewGeneratorNumericColumnSettings();
        private DateTime systemLockDate;
        private decimal? oldttlqaqty;

        /// <summary>
        /// P01
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format("FactoryID = '{0}' and Category = 'O'", Sci.Env.User.Factory);
            MyUtility.Tool.SetupCombox(this.comboTeam, 1, 1, "A,B");
            this.systemLockDate = Convert.ToDateTime(MyUtility.GetValue.Lookup("select SewLock from System WITH (NOLOCK) "));
            this.DoSubForm = new P01_QAOutput();

            // 當Grid目前在最後一筆的最後一欄時，按Enter要自動新增一筆Record
            this.detailgrid.StatusNotification += (s, e) =>
            {
                if (this.EditMode && e.Notification == Ict.Win.UI.DataGridViewStatusNotification.NoMoreRowOnEnterPress)
                {
                    this.OnDetailGridInsert();
                }
            };
        }

        /// <inheritdoc/>
        protected override void OnDetailGridDelete()
        {
            if (this.CurrentDetailData["AutoCreate"].EqualString("True"))
            {
                MyUtility.Msg.WarningBox("Can't delete autocreate Item.");
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
            this.btnRevisedHistory.Enabled = !this.EditMode && MyUtility.Convert.GetDate(this.CurrentMaintain["OutputDate"]) <= this.systemLockDate;
            this.oldttlqaqty = this.numQAOutput.Value;
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : MyUtility.Convert.GetString(e.Master["ID"]);
            this.DetailSelectCommand = string.Format(
                @"
select  sd.*
        , [RFT] = iif(rft.InspectQty is null or rft.InspectQty = 0,'0.00%', CONVERT(VARCHAR, convert(Decimal(5,2), round((rft.InspectQty-rft.RejectQty)/rft.InspectQty*100,2) )) + '%'  )
        , [Remark] = iif( (SELECT MAX(ID) FROM SewingSchedule ss WITH (NOLOCK) WHERE ss.OrderID = sd.OrderId and ss.FactoryID = s.FactoryID and ss.SewingLineID = s.SewingLineID)  is null,'Data Migration (not belong to this line#)','') 
        , [QAOutput] = (select t.TEMP+',' from (select sdd.SizeCode+'*'+CONVERT(varchar,sdd.QAQty) AS TEMP from SewingOutput_Detail_Detail SDD WITH (NOLOCK) where SDD.SewingOutput_DetailUKey = sd.UKey) t for xml path(''))
from SewingOutput_Detail sd WITH (NOLOCK) 
left join SewingOutput s WITH (NOLOCK) on sd.ID = s.ID
outer apply( select top 1 * from Rft WITH (NOLOCK) where rft.OrderID = sd.OrderId 
                               and rft.CDate = s.OutputDate 
                               and rft.SewinglineID = s.SewingLineID 
                               and rft.Shift = s.Shift 
                               and rft.Team = s.Team
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
        , [Variance] = a.OrderQty-a.AccumQty
        , [BalQty] = a.OrderQty-a.AccumQty-a.QAQty
        , [Seq] = isnull(os.Seq,0)
from AllQty a
left join Orders o WITH (NOLOCK) on a.OrderId = o.ID
left join Order_SizeCode os WITH (NOLOCK) on os.Id = o.POID and os.SizeCode = a.SizeCode
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
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    this.OpenSubDetailPage();
                }
            };
            #region SP#的Right click & Validating
            this.orderid.EditingMouseDown += (s, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
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
                            string sqlCmd = string.Format(
                                @"
select distinct ss.OrderID 
from SewingSchedule ss WITH (NOLOCK) 
inner join Orders o With (NoLock) on ss.OrderID = o.ID
where   ss.FactoryID = '{0}' 
        and ss.SewingLineID = '{1}' 
        and ss.OrderFinished = 0
		and o.Category != 'G'",
                                Sci.Env.User.Factory,
                                MyUtility.Convert.GetString(this.CurrentMaintain["SewingLineID"]));

                            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "20", dr["OrderID"].ToString());
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
                        System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@factoryid", Sci.Env.User.Factory);
                        System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter("@id", MyUtility.Convert.GetString(e.FormattedValue));

                        IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
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
		and o.Category != 'G'
        and f.IsProduceFty = 1";
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

                        // 當該SP#已被Junk時，跳出確認訊息
                        if (MyUtility.Check.Seek(string.Format("select ID from orders WITH (NOLOCK) where ID = '{0}' and junk=1", MyUtility.Convert.GetString(e.FormattedValue))))
                        {
                           // 問是否要繼續，確定才繼續往下做
                           DialogResult buttonResult = MyUtility.Msg.WarningBox("This SP# has been canceled already,\r\n\r\nDo you want to continue?", "Warning", MessageBoxButtons.YesNo);
                           if (buttonResult == System.Windows.Forms.DialogResult.No)
                           {
                              dr["OrderID"] = string.Empty;
                              e.Cancel = true;
                              return;
                           }
                        }

                        // 當該SP#+Line不屬於排程時，跳出確認訊息
                        if (!MyUtility.Check.Seek(string.Format("select ID from SewingSchedule WITH (NOLOCK) where OrderID = '{0}' and SewingLineID = '{1}' and OrderFinished=0", MyUtility.Convert.GetString(e.FormattedValue), MyUtility.Convert.GetString(this.CurrentMaintain["SewingLineID"]))))
                            {
                                // 問是否要繼續，確定才繼續往下做
                                DialogResult buttonResult = MyUtility.Msg.WarningBox("This SP# dosen't belong to this line, please inform scheduler.\r\n\r\nDo you want to continue?", "Warning", MessageBoxButtons.YesNo);
                                if (buttonResult == System.Windows.Forms.DialogResult.No)
                                {
                                    dr["OrderID"] = string.Empty;
                                    e.Cancel = true;
                                    return;
                                }
                                else
                                {
                                    dr["Remark"] = "Data Migration (not belong to this line#)";
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
                            sqlCmd = string.Format("select Location,Rate = isnull([dbo].[GetOrderLocation_Rate]('{1}',Location),[dbo].[GetStyleLocation_Rate]('{0}',Location)) from Style_Location WITH (NOLOCK) where StyleUkey = {0}", MyUtility.Convert.GetString(ordersData.Rows[0]["StyleUkey"]), MyUtility.Convert.GetString(dr["OrderID"]));
                            DataTable styleLocation;
                            result = DBProxy.Current.Select(null, sqlCmd, out styleLocation);
                            if (!result || styleLocation.Rows.Count < 0)
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
                                if (styleLocation.Rows.Count == 1)
                                {
                                    dr["ComboType"] = styleLocation.Rows[0]["Location"];
                                    dr["TMS"] = this.CalculateTMS(ordersData.Rows[0], 100);
                                }
                                else
                                {
                                    Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(styleLocation, "Location", "3", MyUtility.Convert.GetString(dr["ComboType"]), headercaptions: "*");
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
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    if (this.EditMode)
                    {
                        if (e.RowIndex != -1 && this.CurrentDetailData["AutoCreate"].EqualString("False"))
                        {
                            if (this.CheckRemoveRow() == false)
                            {
                                return;
                            }

                            DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                            string sqlCmd = string.Format("select sl.Location,Rate = isnull([dbo].[GetOrderLocation_Rate]('{0}' ,sl.Location),[dbo].[GetStyleLocation_Rate](o.StyleUkey ,sl.Location)),o.CPU,o.CPUFactor,(select StdTMS from System WITH (NOLOCK) ) as StdTMS from Orders o WITH (NOLOCK) , Style_Location sl WITH (NOLOCK) where o.ID = '{0}' and o.StyleUkey = sl.StyleUkey", MyUtility.Convert.GetString(dr["OrderID"]));
                            DataTable locationData;
                            DualResult result = DBProxy.Current.Select(null, sqlCmd, out locationData);
                            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(locationData, "Location", "10", MyUtility.Convert.GetString(dr["ComboType"]), headercaptions: "*");
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
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
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

                            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "8,8", MyUtility.Convert.GetString(dr["Article"]), headercaptions: "Article,Color");
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
                        System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@id", MyUtility.Convert.GetString(dr["OrderID"]));
                        System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter("@article", MyUtility.Convert.GetString(e.FormattedValue));

                        IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
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

            Ict.Win.UI.DataGridViewTextBoxColumn textOrderIDSetting;
            Ict.Win.UI.DataGridViewTextBoxColumn textArticleSetting;
            Ict.Win.UI.DataGridViewNumericBoxColumn numInLineQtySetting;
            Ict.Win.UI.DataGridViewNumericBoxColumn numWorkHourSetting;
            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("OrderID", header: "SP#", width: Widths.AnsiChars(13), settings: this.orderid).Get(out textOrderIDSetting)
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
                .Text("Remark", header: "Remarks", width: Widths.AnsiChars(40), iseditingreadonly: true)
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
                textOrderIDSetting.IsEditingReadOnly = isAutoCreate;
                textArticleSetting.IsEditingReadOnly = isAutoCreate;
                numInLineQtySetting.IsEditingReadOnly = isAutoCreate;
                numWorkHourSetting.IsEditingReadOnly = isAutoCreate;

                this.DoSubForm.IsSupportDelete = !isAutoCreate;
                this.DoSubForm.IsSupportUpdate = !isAutoCreate;
                this.DoSubForm.IsSupportNew = !isAutoCreate;
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
                    dr.Cells["OrderID"].Style.ForeColor = isAutoCreate ? Color.Black : Color.Red;
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

                // 新建DataTable 用來存放第三層資料
                DataTable dtQAQtyCheck = new DataTable();
                dtQAQtyCheck.Columns.Add("OrderID", typeof(string));
                dtQAQtyCheck.Columns.Add("Article", typeof(string));
                dtQAQtyCheck.Columns.Add("SizeCode", typeof(string));
                dtQAQtyCheck.Columns.Add("QAQty", typeof(int));

                foreach (DataRow dr in e.SubDetails.Rows)
                {
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        if (MyUtility.Convert.GetString(dr["SewingOutput_DetailUKey"]) == MyUtility.Convert.GetString(this.CurrentDetailData["UKey"]) && !MyUtility.Check.Empty(dr["QAQty"]))
                        {
                            qAOutput.Append(string.Format("{0}*{1},", MyUtility.Convert.GetString(dr["SizeCode"]), MyUtility.Convert.GetString(dr["QAQty"])));
                            qAQty = qAQty + MyUtility.Convert.GetInt(dr["QAQty"]);
                        }

                        DataRow dr1 = dtQAQtyCheck.NewRow();
                        dr1["OrderID"] = dr["Orderid"];
                        dr1["Article"] = dr["Article"];
                        dr1["SizeCode"] = dr["SizeCode"];
                        dr1["QAQty"] = MyUtility.Convert.GetInt(dr["AccumQty"]) + MyUtility.Convert.GetInt(dr["QAQty"]);
                        dtQAQtyCheck.Rows.Add(dr1);
                    }
                }

                // 將第三層資料丟進DataTable
                this.dtQACheck = dtQAQtyCheck.Copy();

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
        private void GetRFT(DataRow dr)
        {
            // sql參數
            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@orderid", MyUtility.Convert.GetString(dr["OrderID"]));
            System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter("@cdate", MyUtility.Convert.GetDate(this.CurrentMaintain["OutputDate"]));
            System.Data.SqlClient.SqlParameter sp3 = new System.Data.SqlClient.SqlParameter("@sewinglineid", MyUtility.Convert.GetString(this.CurrentMaintain["SewingLineID"]));
            System.Data.SqlClient.SqlParameter sp4 = new System.Data.SqlClient.SqlParameter("@shift", MyUtility.Convert.GetString(this.CurrentMaintain["Shift"]));
            System.Data.SqlClient.SqlParameter sp5 = new System.Data.SqlClient.SqlParameter("@team", MyUtility.Convert.GetString(this.CurrentMaintain["Team"]));

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            cmds.Add(sp1);
            cmds.Add(sp2);
            cmds.Add(sp3);
            cmds.Add(sp4);
            cmds.Add(sp5);

            string sqlCmd = @"select iif(rft.InspectQty is null or rft.InspectQty = 0,0, round((rft.InspectQty-rft.RejectQty)/rft.InspectQty*100,2)) as RFT
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
            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@ukey", dr["UKey"]);
            System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter("@combotype", MyUtility.Convert.GetString(dr["ComboType"]));
            System.Data.SqlClient.SqlParameter sp3 = new System.Data.SqlClient.SqlParameter("@orderid", MyUtility.Convert.GetString(dr["OrderID"]));
            System.Data.SqlClient.SqlParameter sp4 = new System.Data.SqlClient.SqlParameter("@article", MyUtility.Convert.GetString(dr["Article"]));

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
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
       , a.OrderQty - a.AccumQty as Variance
       , a.OrderQty - a.AccumQty - a.QAQty as BalQty
       , isnull(os.Seq,0) as Seq
       , OldDetailKey = ''
from AllQty a
left join Orders o WITH (NOLOCK) on a.OrderId = o.ID
left join Order_SizeCode os WITH (NOLOCK) on os.Id = o.POID 
                                             and os.SizeCode = a.SizeCode
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
            this.CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            this.CurrentMaintain["OutputDate"] = DateTime.Today.AddDays(-1);
            this.CurrentMaintain["Shift"] = "D";
            this.CurrentMaintain["Team"] = "A";
            this.CurrentDetailData["AutoCreate"] = 0;
            this.CurrentDetailData["RFT"] = "0.00%";
            this.CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            if (!MyUtility.Check.Empty(this.CurrentMaintain["LockDate"]))
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
            if (MyUtility.Convert.GetDate(this.CurrentMaintain["OutputDate"]) <= MyUtility.Convert.GetDate(MyUtility.GetValue.Lookup("select SewLock from System WITH (NOLOCK) ")))
            {
                this.txtdropdownlistShift.ReadOnly = true;
                this.comboTeam.ReadOnly = true;
                this.numManpower.ReadOnly = true;
                this.numWHours.ReadOnly = true;
            }
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

            if (MyUtility.Convert.GetDate(this.CurrentMaintain["OutputDate"]) <= MyUtility.Convert.GetDate(MyUtility.GetValue.Lookup("select SewLock from System WITH (NOLOCK) ")))
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

            #endregion

            this.CalculateManHour();

            #region 新增時檢查Date不可早於Sewing Lock Date
            if (this.IsDetailInserting)
            {
                if (MyUtility.Convert.GetDate(this.CurrentMaintain["OutputDate"]) <= MyUtility.Convert.GetDate(MyUtility.GetValue.Lookup("select SewLock from System WITH (NOLOCK) ")))
                {
                    this.dateDate.Focus();
                    MyUtility.Msg.WarningBox(string.Format("Date can't earlier than Sewing Lock Date: {0}.", Convert.ToDateTime(MyUtility.GetValue.Lookup("select SewLock from System WITH (NOLOCK) ")).ToString(string.Format("{0}", Sci.Env.Cfg.DateStringFormat))));
                    return false;
                }
            }
            #endregion

            #region 檢查資料是否已存在
            if (MyUtility.Check.Seek(string.Format(@"select ID from SewingOutput WITH (NOLOCK) where OutputDate = '{0}' and SewingLineID = '{1}' and Shift = '{2}' and Team = '{3}' and FactoryID = '{4}' and ID <> '{5}' and Category = 'O'", Convert.ToDateTime(this.CurrentMaintain["OutputDate"]).ToString("d"), MyUtility.Convert.GetString(this.CurrentMaintain["SewingLineID"]), MyUtility.Convert.GetString(this.CurrentMaintain["Shift"]), MyUtility.Convert.GetString(this.CurrentMaintain["Team"]), MyUtility.Convert.GetString(this.CurrentMaintain["FactoryID"]), MyUtility.Convert.GetString(this.CurrentMaintain["ID"]))))
                {
                    MyUtility.Msg.WarningBox(string.Format(
                        "Date:{0}, Line:{1}, Shift:{2}, Team:{3} already exist, can't save!!",
                        Convert.ToDateTime(this.CurrentMaintain["OutputDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateStringFormat)),
                        MyUtility.Convert.GetString(this.CurrentMaintain["SewingLineID"]),
                        MyUtility.Convert.GetString(this.CurrentMaintain["Shift"]),
                        MyUtility.Convert.GetString(this.CurrentMaintain["Team"]),
                        MyUtility.Convert.GetString(this.CurrentMaintain["FactoryID"])));
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

            #region 若sewingoutput.outputDate <= system.sewlock 表身Qty要等於表頭的Qty
            DataTable sys;
            DBProxy.Current.Select(null, "select sewlock from system WITH (NOLOCK) ", out sys);
            DateTime? sod = MyUtility.Convert.GetDate(this.CurrentMaintain["outputDate"]);
            DateTime? sl = MyUtility.Convert.GetDate(sys.Rows[0][0]);
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

            #region GetID
            if (this.IsDetailInserting)
            {
                string id = MyUtility.GetValue.GetID(MyUtility.GetValue.Lookup("FtyGroup", Sci.Env.User.Factory, "Factory", "ID") + "SM", "SewingOutput", DateTime.Today, 3, "Id", null);
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
                MyUtility.Msg.WarningBox(chkResult.ToString());
                return;
            }
            #endregion

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

drop table #Child, #updateChild";

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

            // List<DataRow> newUpdate.d = new List<DataRow>();
            // if (updated.Count > 0 && false)
            // {
            //    var newT = updated[0].Table.Clone();
            //    for (int i = 0; i < updated.Count; i++)
            //    {
            //        var newOne = newT.NewRow();
            //        newOne.ItemArray = updated[i].ItemArray;
            //        newUpdated.Add(newOne);
            //        newT.Rows.Add(newOne);
            //    }

            // newT.AcceptChanges();
            //    for (int i = 0; i < updated.Count; i++)
            //    {
            //        newUpdated[i]["QaQty"] = updated[i]["qaqty"];
            //    }
            // }
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

                if (this.dateDate.Value <= MyUtility.Convert.GetDate(MyUtility.GetValue.Lookup("select SewLock from System WITH (NOLOCK) ")))
                {
                    this.dateDate.Value = null;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("Date can't earlier than Sewing Lock Date: {0}.", Convert.ToDateTime(MyUtility.GetValue.Lookup("select SewLock from System WITH (NOLOCK) ")).ToString(string.Format("{0}", Sci.Env.Cfg.DateStringFormat))));
                    return;
                }
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
            Sci.Win.UI.ShowHistory callNextForm = new Sci.Win.UI.ShowHistory("SewingOutput_History", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), "Status", reasonType: "Sewing_RVS", caption: "Revised History");
            callNextForm.ShowDialog(this);
        }

        /// <inheritdoc/>
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            Sci.Win.UI.SelectReason callReason = new Sci.Win.UI.SelectReason("Sewing_RVS", true);
            DialogResult dResult = callReason.ShowDialog(this);
            if (dResult == System.Windows.Forms.DialogResult.OK)
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
                    Sci.Env.User.UserID);

                string updateCmd = string.Format(@"update SewingOutput set LockDate = null, Status = 'New' where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));

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
) a 
WHERE  sewqty < packqty ";

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
    where Convert (bit, t.AutoCreate) != 1
) a 
WHERE  sewqty < packqty ",
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
    }
}
