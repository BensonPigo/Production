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
    public partial class P01 : Sci.Win.Tems.Input8
    {
        DataTable dtQACheck;
        ITableSchema sub_Schema;
        Ict.Win.DataGridViewGeneratorTextColumnSettings qaoutput = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
        Ict.Win.DataGridViewGeneratorTextColumnSettings orderid = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
        Ict.Win.DataGridViewGeneratorTextColumnSettings combotype = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
        Ict.Win.DataGridViewGeneratorTextColumnSettings article = new Ict.Win.DataGridViewGeneratorTextColumnSettings();
        Ict.Win.DataGridViewGeneratorNumericColumnSettings inlineqty = new Ict.Win.DataGridViewGeneratorNumericColumnSettings();
        private DateTime systemLockDate;
        private decimal? oldttlqaqty;
        public P01(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            DefaultFilter = string.Format("FactoryID = '{0}' and Category = 'O'", Sci.Env.User.Factory);
            MyUtility.Tool.SetupCombox(comboTeam, 1, 1, "A,B");
            systemLockDate = Convert.ToDateTime(MyUtility.GetValue.Lookup("select SewLock from System WITH (NOLOCK) "));
            DoSubForm = new P01_QAOutput();

            //當Grid目前在最後一筆的最後一欄時，按Enter要自動新增一筆Record
            detailgrid.StatusNotification += (s, e) =>
            {
                if (this.EditMode && e.Notification == Ict.Win.UI.DataGridViewStatusNotification.NoMoreRowOnEnterPress)
                {
                    this.OnDetailGridInsert();
                }
            };                    
        }

        protected override void OnDetailGridDelete()
        {
            if (CurrentDetailData["AutoCreate"].EqualString("True"))
            {
                MyUtility.Msg.WarningBox("Can't delete autocreate Item.");
                return;
            }

            if (CheckRemoveRow() == false)
            {
                return;
            }

            base.OnDetailGridDelete();

            if (this.EditMode && this.detailgridbs.DataSource != null)
            {
                //重新計算表頭 QAQty, InlineQty, DefectQty
                if (((DataTable)this.detailgridbs.DataSource).AsEnumerable().Any(row => row.RowState != DataRowState.Deleted && row["AutoCreate"].EqualString("False")))
                {
                    CurrentMaintain["QAQty"] = ((DataTable)this.detailgridbs.DataSource).AsEnumerable().Where(row => row.RowState != DataRowState.Deleted
                                                                                                                     && row["AutoCreate"].EqualString("False")).CopyToDataTable().Compute("SUM(QAQty)", "");
                    CurrentMaintain["InlineQty"] = MyUtility.Convert.GetInt(CurrentMaintain["QAQty"]) + MyUtility.Convert.GetInt(CurrentMaintain["DefectQty"]);
                    CurrentMaintain["DefectQty"] = ((DataTable)this.detailgridbs.DataSource).AsEnumerable().Where(row => row.RowState != DataRowState.Deleted
                                                                                                                         && row["AutoCreate"].EqualString("False")).CopyToDataTable().Compute("SUM(DefectQty)", "");
                }
                else
                {
                    CurrentMaintain["QAQty"] = 0;
                    CurrentMaintain["InlineQty"] = 0;
                    CurrentMaintain["DefectQty"] = 0;
                }
            }
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            btnRevisedHistory.Enabled = !EditMode && MyUtility.Convert.GetDate(CurrentMaintain["OutputDate"]) <= systemLockDate;
            oldttlqaqty = numQAOutput.Value;
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : MyUtility.Convert.GetString(e.Master["ID"]);
            this.DetailSelectCommand = string.Format(@"
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
where sd.ID = '{0}'"
                , masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override DualResult OnSubDetailSelectCommandPrepare(PrepareSubDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Detail == null) ? "0" : MyUtility.Convert.GetString(e.Detail["UKey"]);

            this.SubDetailSelectCommand = string.Format(@"
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
order by a.OrderId,os.Seq"
                , masterID);
            return base.OnSubDetailSelectCommandPrepare(e);
        }

        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            qaoutput.CellMouseDoubleClick += (s, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {  
                    OpenSubDetailPage();
                }
            };
            #region SP#的Right click & Validating
            orderid.EditingMouseDown += (s, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    if (this.EditMode && CurrentDetailData["AutoCreate"].EqualString("False"))
                    {
                        if (e.RowIndex != -1)
                        {
                            if (CheckRemoveRow() == false)
                            {
                                return;
                            }

                            DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                            string sqlCmd = string.Format(@"
select distinct ss.OrderID 
from SewingSchedule ss WITH (NOLOCK) 
inner join Orders o With (NoLock) on ss.OrderID = o.ID
where   ss.FactoryID = '{0}' 
        and ss.SewingLineID = '{1}' 
        and ss.OrderFinished = 0
		and o.Category != 'G'", Sci.Env.User.Factory, MyUtility.Convert.GetString(CurrentMaintain["SewingLineID"]));
                            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "20", dr["OrderID"].ToString());
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel) { return; }
                            e.EditingControl.Text = item.GetSelectedString();
                        }
                    }
                }
            };

            orderid.CellValidating += (s, e) =>
            {
                if (this.EditMode && CurrentDetailData["OrderID"].EqualString(e.FormattedValue) == false)
                {
                    if (CurrentDetailData["OrderID"].Empty() == false && CheckRemoveRow() == false)
                    {
                        this.detailgrid.GetDataRow<DataRow>(e.RowIndex)["OrderID"] = CurrentDetailData["OrderID"];
                        return;
                    }

                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (MyUtility.Convert.GetString(e.FormattedValue) != MyUtility.Convert.GetString(dr["OrderID"]))
                    {
                        //資料有異動過就先刪除SubDetail資料
                        DeleteSubDetailData(dr);
                        if (MyUtility.Check.Empty(e.FormattedValue))
                        {
                            dr["OrderID"] = "";
                            dr["ComboType"] = "";
                            dr["Article"] = "";
                            dr["Color"] = "";
                            dr["QAOutput"] = "";
                            dr["TMS"] = 0;
                            dr["RFT"] = "0.00%";
                            dr.EndEdit();
                            return;
                        }

                        //sql參數
                        System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@factoryid", Sci.Env.User.Factory);
                        System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter("@id", MyUtility.Convert.GetString(e.FormattedValue));

                        IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                        cmds.Add(sp1);
                        cmds.Add(sp2);

                        DataTable OrdersData;
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
                        DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out OrdersData);
                        if (!result || OrdersData.Rows.Count <= 0)
                        {
                            if (!result)
                            {
                                MyUtility.Msg.WarningBox("Sql connection fail!\r\n" + result.ToString());
                            }
                            else
                            {
                                MyUtility.Msg.WarningBox("Data not found!!!");
                            }
                            dr["OrderID"] = "";
                            e.Cancel = true;
                            return;
                        }
                        else
                        {
                            if (MyUtility.Convert.GetBool(OrdersData.Rows[0]["IsForecast"]))
                            {
                                dr["OrderID"] = "";
                                e.Cancel = true;
                                MyUtility.Msg.WarningBox("SP# can't be PreOrder!!");
                                return;
                            }
                            //當該SP#+Line不屬於排程時，跳出確認訊息
                            if (!MyUtility.Check.Seek(string.Format("select ID from SewingSchedule WITH (NOLOCK) where OrderID = '{0}' and SewingLineID = '{1}' and OrderFinished=0", MyUtility.Convert.GetString(e.FormattedValue), MyUtility.Convert.GetString(CurrentMaintain["SewingLineID"]))))
                            {
                                //問是否要繼續，確定才繼續往下做
                                DialogResult buttonResult = MyUtility.Msg.WarningBox("This SP# dosen't belong to this line, please inform scheduler.\r\n\r\nDo you want to continue?", "Warning", MessageBoxButtons.YesNo);
                                if (buttonResult == System.Windows.Forms.DialogResult.No)
                                {
                                    dr["OrderID"] = "";
                                    e.Cancel = true;
                                    return;
                                }
                                else
                                {
                                    dr["Remark"] = "Data Migration (not belong to this line#)";
                                }
                            }
                            dr["OrderID"] = e.FormattedValue.ToString();
                            dr["ComboType"] = "";
                            dr["Article"] = "";
                            dr["Color"] = "";
                            dr["QAOutput"] = "";
                            dr["TMS"] = 0;
                            GetRFT(dr);

                            #region 若此SP是套裝的話，就跳出視窗讓使用者選擇部位
                            sqlCmd = string.Format("select Location,Rate from Style_Location WITH (NOLOCK) where StyleUkey = {0}", MyUtility.Convert.GetString(OrdersData.Rows[0]["StyleUkey"]));
                            DataTable StyleLocation;
                            result = DBProxy.Current.Select(null, sqlCmd, out StyleLocation);
                            if (!result || StyleLocation.Rows.Count < 0)
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
                                if (StyleLocation.Rows.Count == 1)
                                {
                                    dr["ComboType"] = StyleLocation.Rows[0]["Location"];
                                    dr["TMS"] = CalculateTMS(OrdersData.Rows[0], 100);
                                }
                                else
                                {
                                    Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(StyleLocation, "Location", "3", MyUtility.Convert.GetString(dr["ComboType"]), headercaptions: "*");
                                    DialogResult returnResult = item.ShowDialog();
                                    if (returnResult != DialogResult.Cancel)
                                    {
                                        IList<DataRow> location = item.GetSelecteds();
                                        dr["ComboType"] = item.GetSelectedString();
                                        dr["TMS"] = CalculateTMS(OrdersData.Rows[0], MyUtility.Convert.GetDecimal(location[0]["Rate"]));
                                    }
                                }
                            }
                            #endregion
                            dr.EndEdit();
                            CreateSubDetailDatas(dr);
                        }
                    }
                }
            };          
            #endregion
            #region ComboType的Right Click
            combotype.EditingMouseDown += (s, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    if (this.EditMode)
                    {
                        if (e.RowIndex != -1 && CurrentDetailData["AutoCreate"].EqualString("False"))
                        {
                            if (CheckRemoveRow() == false)
                            {
                                return;
                            }

                            DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                            string sqlCmd = string.Format("select sl.Location,sl.Rate,o.CPU,o.CPUFactor,(select StdTMS from System WITH (NOLOCK) ) as StdTMS from Orders o WITH (NOLOCK) , Style_Location sl WITH (NOLOCK) where o.ID = '{0}' and o.StyleUkey = sl.StyleUkey", MyUtility.Convert.GetString(dr["OrderID"]));
                            DataTable LocationData;
                            DualResult result = DBProxy.Current.Select(null, sqlCmd, out LocationData);
                            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(LocationData, "Location", "10", MyUtility.Convert.GetString(dr["ComboType"]), headercaptions: "*");
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel) { return; }

                            IList<DataRow> location = item.GetSelecteds();
                            bool changed = item.GetSelectedString() != MyUtility.Convert.GetString(dr["ComboType"]);
                            dr["ComboType"] = item.GetSelectedString();
                            dr["TMS"] = CalculateTMS(location[0], MyUtility.Convert.GetDecimal(location[0]["Rate"]));
                            if (changed)
                            {
                                dr["QAOutput"] = "";
                                dr.EndEdit();
                                DeleteSubDetailData(dr);
                                CreateSubDetailDatas(dr);
                                return;
                            }
                            dr.EndEdit();
                        }
                    }
                }
            };

            #endregion
            #region Article的Right Click & Validating
            article.EditingMouseDown += (s, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    if (this.EditMode && CurrentDetailData["AutoCreate"].EqualString("False"))
                    {
                        if (e.RowIndex != -1)
                        {
                            if (CheckRemoveRow() == false)
                            {
                                return;
                            }

                            DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                            string sqlCmd = string.Format("select Article,ColorID from View_OrderFAColor where Id = '{0}'", MyUtility.Convert.GetString(dr["OrderID"]));

                            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd,"8,8",MyUtility.Convert.GetString(dr["Article"]),headercaptions: "Article,Color");
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel) { return; }

                            IList<DataRow> ColorData = item.GetSelecteds();
                            bool changed = item.GetSelectedString() != MyUtility.Convert.GetString(dr["Article"]);
                            dr["Article"] = item.GetSelectedString();
                            dr["Color"] = ColorData[0]["ColorID"];
                            if (changed)
                            {
                                dr["QAOutput"] = "";
                            }
                            dr.EndEdit();
                            if (changed)
                            {
                                DeleteSubDetailData(dr);
                                CreateSubDetailDatas(dr);
                            }
                        }
                    }
                }
            };

            article.CellValidating += (s, e) =>
            {
                if (this.EditMode && CurrentDetailData["Article"].EqualString(e.FormattedValue) == false)
                {
                    if(CurrentDetailData["Article"].Empty() == false && CheckRemoveRow() == false)
                    {
                        this.detailgrid.GetDataRow<DataRow>(e.RowIndex)["Article"] = CurrentDetailData["Article"];
                        return;
                    }

                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (MyUtility.Convert.GetString(e.FormattedValue) != MyUtility.Convert.GetString(dr["Article"]))
                    {
                        //資料有異動過就先刪除SubDetail資料
                        DeleteSubDetailData(dr);
                        if (MyUtility.Check.Empty(e.FormattedValue))
                        {
                            dr["Article"] = "";
                            dr["Color"] = "";
                            dr["QAOutput"] = "";
                            dr.EndEdit();
                            return;
                        }

                        //sql參數
                        System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@id", MyUtility.Convert.GetString(dr["OrderID"]));
                        System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter("@article", MyUtility.Convert.GetString(e.FormattedValue));

                        IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
                        cmds.Add(sp1);
                        cmds.Add(sp2);

                        DataTable ColorData;
                        string sqlCmd = "select * from View_OrderFAColor where Id = @id and Article = @article";
                        DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out ColorData);
                        if (!result || ColorData.Rows.Count <= 0)
                        {
                            if (!result)
                            {
                                MyUtility.Msg.WarningBox("Sql connection fail!\r\n" + result.ToString());
                            }
                            else
                            {
                                MyUtility.Msg.WarningBox("Data not found!!!");
                            }
                            dr["Article"] = "";
                            dr["Color"] = "";
                            dr["QAOutput"] = "";
                            dr.EndEdit();
                            e.Cancel = true;
                            return;
                        }
                        else
                        {
                            dr["Article"] = MyUtility.Convert.GetString(e.FormattedValue);
                            dr["Color"] = ColorData.Rows[0]["ColorID"];
                            dr["QAOutput"] = "";
                            dr.EndEdit();
                            CreateSubDetailDatas(dr);
                        }
                    }
                }
            };
            #endregion
            #region Prod. Output的Validatng
            inlineqty.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (MyUtility.Convert.GetInt(e.FormattedValue) != MyUtility.Convert.GetInt(dr["InlineQty"]))
                    {
                        dr["InlineQty"] = MyUtility.Convert.GetInt(e.FormattedValue);
                        CalculateDefectQty(dr);
                        dr.EndEdit();
                    }
                }
            };
            #endregion

            Ict.Win.UI.DataGridViewTextBoxColumn textOrderIDSetting;
            Ict.Win.UI.DataGridViewTextBoxColumn textArticleSetting;
            Ict.Win.UI.DataGridViewNumericBoxColumn numInLineQtySetting;
            Ict.Win.UI.DataGridViewNumericBoxColumn numWorkHourSetting;
            Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("OrderID", header: "SP#", width: Widths.AnsiChars(13), settings: orderid).Get(out textOrderIDSetting)
                .Text("ComboType", header: "*", width: Widths.AnsiChars(1), iseditingreadonly: true, settings: combotype)
                .Text("Article", header: "Article", width: Widths.AnsiChars(8), settings: article).Get(out textArticleSetting)
                .Text("Color", header: "Color", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("QAOutput", header: "QA Output", width: Widths.AnsiChars(30), iseditingreadonly: true, settings: qaoutput)
                .Numeric("QAQty", header: "QA Ttl Output", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("InlineQty", header: "Prod. Output", width: Widths.AnsiChars(5), settings: inlineqty).Get(out numInLineQtySetting)
                .Numeric("DefectQty", header: "Defect Q’ty", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Numeric("WorkHour", header: "W’Hours", width: Widths.AnsiChars(5), decimal_places: 3, maximum: 999.999m, minimum: 0m).Get(out numWorkHourSetting)
                .Numeric("TMS", header: "TMS", width: Widths.AnsiChars(5), iseditingreadonly: true)
                //.Numeric("RFT", header: "RFT(%)", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("RFT", header: "RFT(%)", width: Widths.AnsiChars(7), iseditingreadonly: true)
                .Text("Remark", header: "Remarks", width: Widths.AnsiChars(40), iseditingreadonly: true)
                .CheckBox("AutoCreate", header: "Auto Create", trueValue: 1, falseValue: 0, iseditable: false);

            this.detailgrid.RowEnter += (s, e) =>
            {
                if (e.RowIndex < 0 || EditMode == false) { return; }
                var data = ((DataRowView)this.detailgrid.Rows[e.RowIndex].DataBoundItem).Row;
                if (data == null) { return; }

                bool isAutoCreate = data["AutoCreate"].EqualString("True");
                textOrderIDSetting.IsEditingReadOnly = isAutoCreate;
                textArticleSetting.IsEditingReadOnly = isAutoCreate;
                numInLineQtySetting.IsEditingReadOnly = isAutoCreate;
                numWorkHourSetting.IsEditingReadOnly = isAutoCreate;

                DoSubForm.IsSupportDelete = !isAutoCreate;
                DoSubForm.IsSupportUpdate = !isAutoCreate;
                DoSubForm.IsSupportNew = !isAutoCreate;
            };

            this.detailgrid.RowsAdded += (s, e) =>
            {
                if(EditMode == false || e.RowIndex < 0) return;
                
                int index = e.RowIndex;
                for (int i = 0; i < e.RowCount; i++)
                {
                    DataGridViewRow dr = detailgrid.Rows[index];
                    bool isAutoCreate = dr.Cells["AutoCreate"].Value.EqualString("True");
                    dr.Cells["OrderID"].Style.ForeColor = isAutoCreate ? Color.Black : Color.Red;
                    dr.Cells["Article"].Style.ForeColor = isAutoCreate ? Color.Black : Color.Red;
                    dr.Cells["InlineQty"].Style.ForeColor = isAutoCreate ? Color.Black : Color.Red;
                    dr.Cells["WorkHour"].Style.ForeColor = isAutoCreate ? Color.Black : Color.Red;
                    index++;
                }
            };
        }

        //設定表身 RFT & AutoCreate 的預設值
        protected override void OnDetailGridInsert(int index = -1)
        {
            base.OnDetailGridInsert(index);

            DataTable dt = ((DataTable)((BindingSource)detailgrid.DataSource).DataSource);
            string gridSort = dt.DefaultView.Sort;
            dt.DefaultView.Sort = "";

            if (index == -1)
            {
                CurrentDetailData["Rft"] = "0.00%";
                CurrentDetailData["AutoCreate"] = 0;
            }else
            {
                dt.Rows[index]["Rft"] = "0.00";
                dt.Rows[index]["AutoCreate"] = 0;
            }

            dt.DefaultView.Sort = gridSort;
        }
       
        //重組表身Grid的QA Qty資料
        protected override DualResult ConvertSubDetailDatasFromDoSubForm(SubDetailConvertFromEventArgs e)
        {
            if (EditMode && DoSubForm.DialogResult == DialogResult.OK)
            {
                StringBuilder QAOutput = new StringBuilder();
                int QAQty = 0;
                //新建DataTable 用來存放第三層資料
                DataTable dtQAQtyCheck = new DataTable();
                dtQAQtyCheck.Columns.Add("OrderID", typeof(string));                
                dtQAQtyCheck.Columns.Add("Article", typeof(string));
                dtQAQtyCheck.Columns.Add("SizeCode", typeof(string));
                dtQAQtyCheck.Columns.Add("QAQty", typeof(int));

                foreach (DataRow dr in e.SubDetails.Rows)
                {
                    if (dr.RowState != DataRowState.Deleted)
                    {
                        if (MyUtility.Convert.GetString(dr["SewingOutput_DetailUKey"]) == MyUtility.Convert.GetString(CurrentDetailData["UKey"]) && !MyUtility.Check.Empty(dr["QAQty"]))
                        {
                            QAOutput.Append(string.Format("{0}*{1},", MyUtility.Convert.GetString(dr["SizeCode"]), MyUtility.Convert.GetString(dr["QAQty"])));
                            QAQty = QAQty + MyUtility.Convert.GetInt(dr["QAQty"]);
                        }
                        DataRow dr1 = dtQAQtyCheck.NewRow();
                        dr1["OrderID"] = dr["Orderid"];
                        dr1["Article"] = dr["Article"];
                        dr1["SizeCode"] = dr["SizeCode"];
                        dr1["QAQty"] = MyUtility.Convert.GetInt(dr["AccumQty"])+MyUtility.Convert.GetInt( dr["QAQty"]);
                        dtQAQtyCheck.Rows.Add(dr1);
                    }                    
                }
                //將第三層資料丟進DataTable
                dtQACheck = dtQAQtyCheck.Copy();

                e.Detail["QAOutput"] = QAOutput.Length > 0 ? QAOutput.ToString() : "";
                //總計第三層 Qty 填入第二層 QAQty
                e.Detail["QAQty"] = QAQty;

                if (QAQty == 0)
                {
                    e.Detail["InlineQty"] = 0;
                }
                else
                {
                    e.Detail["InlineQty"] = e.Detail["RFT"].ToString().Substring(0, 4) == "0.00" ? QAQty : QAQty /
                        (decimal.Parse(e.Detail["RFT"].ToString().Substring(0, 4)) / 100);
                }
                
                CalculateDefectQty(e.Detail);
                //總計第二層 Qty 填入第一層 QAQty
                CurrentMaintain["QAQty"] = ((DataTable)this.detailgridbs.DataSource).Compute("SUM(QAQty)", "");
                CurrentMaintain["InlineQty"] =MyUtility.Convert.GetInt(CurrentMaintain["QAQty"]) + MyUtility.Convert.GetInt(CurrentMaintain["DefectQty"]);
            }
            return base.ConvertSubDetailDatasFromDoSubForm(e);
        }

        //計算表身Grid的TMS
        private decimal CalculateTMS(DataRow dr, decimal rate)
        {
            return MyUtility.Math.Round(MyUtility.Convert.GetDecimal(dr["CPU"]) * MyUtility.Convert.GetDecimal(dr["CPUFactor"]) * (rate / 100) * MyUtility.Convert.GetDecimal(dr["StdTMS"]), 0);
        }

        //計算表身Grid的Defect Qty
        private void CalculateDefectQty(DataRow dr)
        {
            dr["DefectQty"] = MyUtility.Convert.GetInt(dr["InlineQty"]) - MyUtility.Convert.GetInt(dr["QAQty"]);
            CurrentMaintain["DefectQty"] = ((DataTable)this.detailgridbs.DataSource).Compute("SUM(DefectQty)","");
        }

        //撈取RFT值
        private void GetRFT(DataRow dr)
        {
            //sql參數
            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@orderid", MyUtility.Convert.GetString(dr["OrderID"]));
            System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter("@cdate", MyUtility.Convert.GetDate(CurrentMaintain["OutputDate"]));
            System.Data.SqlClient.SqlParameter sp3 = new System.Data.SqlClient.SqlParameter("@sewinglineid", MyUtility.Convert.GetString(CurrentMaintain["SewingLineID"]));
            System.Data.SqlClient.SqlParameter sp4 = new System.Data.SqlClient.SqlParameter("@shift", MyUtility.Convert.GetString(CurrentMaintain["Shift"]));
            System.Data.SqlClient.SqlParameter sp5 = new System.Data.SqlClient.SqlParameter("@team", MyUtility.Convert.GetString(CurrentMaintain["Team"]));
            
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

            DataTable RFTData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out RFTData);
            if (result && RFTData.Rows.Count > 0)
            {
                dr["RFT"] = RFTData.Rows[0]["RFT"];
            }
            else
            {
                dr["RFT"] = "0.00%";
            }
        }

        //刪除SubDetail資料
        private void DeleteSubDetailData(DataRow dr)
        {
            DataTable SubDetailData;
            GetSubDetailDatas(dr, out SubDetailData);
            foreach (DataRow ddr in SubDetailData.Rows)
            {
                ddr["QAQty"] = 0;
            }
            dr["QAQty"] = 0;
            dr["InlineQty"] = 0;
            dr["DefectQty"] = 0;
        }

        //產生SubDetail資料
        private void CreateSubDetailDatas(DataRow dr)
        {
            if (MyUtility.Check.Empty(dr["ComboType"]) || MyUtility.Check.Empty(dr["OrderID"]) || MyUtility.Check.Empty(dr["Article"]))
            {
                return;
            }
            //sql參數
            System.Data.SqlClient.SqlParameter sp1 = new System.Data.SqlClient.SqlParameter("@ukey", dr["UKey"]);
            System.Data.SqlClient.SqlParameter sp2 = new System.Data.SqlClient.SqlParameter("@combotype", MyUtility.Convert.GetString(dr["ComboType"]));
            System.Data.SqlClient.SqlParameter sp3 = new System.Data.SqlClient.SqlParameter("@orderid", MyUtility.Convert.GetString(dr["OrderID"]));
            System.Data.SqlClient.SqlParameter sp4 = new System.Data.SqlClient.SqlParameter("@article", MyUtility.Convert.GetString(dr["Article"]));

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            cmds.Add(sp1);
            cmds.Add(sp2);
            cmds.Add(sp3);
            cmds.Add(sp4);

            string sqlCmd = string.Format(@"
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
order by a.OrderId,os.Seq", this.CurrentMaintain["ID"]);
            DataTable OrderQtyData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, cmds, out OrderQtyData);
            if (result && OrderQtyData.Rows.Count > 0)
            {
                DataTable SubDetailData;
                GetSubDetailDatas(dr, out SubDetailData);
                foreach(DataRow ddr in OrderQtyData.Rows)
                {
                    if (!SubDetailData.AsEnumerable().Any(row => row["ID"].EqualString(ddr["ID"])
                                                                && row["SewingOutput_DetailUkey"].EqualString(ddr["SewingOutput_DetailUkey"])
                                                                && row["OrderID"].EqualString(ddr["OrderID"])
                                                                && row["ComboType"].EqualString(ddr["ComboType"])
                                                                && row["Article"].EqualString(ddr["Article"])
                                                                && row["SizeCode"].EqualString(ddr["SizeCode"])))
                    {
                        DataRow newDr = SubDetailData.NewRow();
                        for(int i = 0; i < SubDetailData.Columns.Count; i++)
                        {
                            newDr[SubDetailData.Columns[i].ColumnName] = ddr[SubDetailData.Columns[i].ColumnName];
                        }
                        SubDetailData.Rows.Add(newDr);
                    }
                }
            }
        }
       
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["Category"] = "O";
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            CurrentMaintain["OutputDate"] = DateTime.Today.AddDays(-1);
            CurrentMaintain["Shift"] = "D";
            CurrentMaintain["Team"] = "A";
            CurrentDetailData["AutoCreate"] = 0;
            CurrentDetailData["RFT"] = "0.00%";
            CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
            
        }

        protected override bool ClickEditBefore()
        {
            if (!MyUtility.Check.Empty(CurrentMaintain["LockDate"]))
            {
                MyUtility.Msg.WarningBox("This record already locked, can't modify.");
                return false;
            }
            return base.ClickEditBefore();
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            dateDate.ReadOnly = true;
            txtsewinglineLine.ReadOnly = true;
            if (MyUtility.Convert.GetDate(CurrentMaintain["OutputDate"]) <= MyUtility.Convert.GetDate(MyUtility.GetValue.Lookup("select SewLock from System WITH (NOLOCK) ")))
            {
                txtdropdownlistShift.ReadOnly = true;
                comboTeam.ReadOnly = true;
                numManpower.ReadOnly = true;
                numWHours.ReadOnly = true;
            }

        }

        protected override bool ClickDeleteBefore()
        {
            DualResult result;
            if (!MyUtility.Check.Empty(CurrentMaintain["LockDate"]))
            {
                MyUtility.Msg.WarningBox("This record already locked, can't delete.");
                return false;
            }

            if (MyUtility.Convert.GetDate(CurrentMaintain["OutputDate"]) <= MyUtility.Convert.GetDate(MyUtility.GetValue.Lookup("select SewLock from System WITH (NOLOCK) ")))
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
            foreach (DataRow dr in DetailDatas)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    DataTable subDt;
                    GetSubDetailDatas(dr, out subDt);
                    subDt = subDt.AsEnumerable().Where(row => true).CopyToDataTable();
                    subDt.Columns.Add("AutoCreate");
                    if (dtSubDetail == null) dtSubDetail = subDt.Clone();

                    foreach (DataRow subDr in subDt.Rows)
                    {
                        subDr["AutoCreate"] = dr["AutoCreate"];
                        dtSubDetail.ImportRow(subDr);
                    }
                }
            }

            if (SaveDeleteCheckPacking(dtSubDetail, false) == false)
            {
                return false;
            }
            #endregion            

            //第3層SewingOutput_Detail_Detail刪除,以當前SewingOutput_DetailUKey為條件
            string sqlcmdD ="Delete SewingOutput_Detail_Detail where SewingOutput_DetailUKey = @K";
            List<SqlParameter> ps = new List<SqlParameter>();
            ps.Add(new SqlParameter("@K", CurrentDetailData["Ukey"]));
            if (!(result =DBProxy.Current.Execute(null, sqlcmdD, ps)))
            {
                ShowErr(result);
            }

            return base.ClickDeleteBefore();
        }

        protected override bool ClickSaveBefore()
        {
            #region 檢查不可為空值
            if (MyUtility.Check.Empty(CurrentMaintain["OutputDate"]))
            {
                dateDate.Focus();
                MyUtility.Msg.WarningBox("Date can't empty!!");
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["SewingLineID"]))
            {
                txtsewinglineLine.Focus();
                MyUtility.Msg.WarningBox("Line# can't empty!!");
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["Shift"]))
            {
                txtdropdownlistShift.Focus();
                MyUtility.Msg.WarningBox("Shift can't empty!!");
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["Team"]))
            {
                comboTeam.Focus();
                MyUtility.Msg.WarningBox("Team can't empty!!");
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["Manpower"]))
            {
                numManpower.Focus();
                MyUtility.Msg.WarningBox("Manpower can't empty!!");
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["WorkHour"]))
            {
                numWHours.Focus();
                MyUtility.Msg.WarningBox("W/Hours(Day) can't empty!!");
                return false;
            }
          
            #endregion

            CalculateManHour();

            #region 新增時檢查Date不可早於Sewing Lock Date
            if (IsDetailInserting)
            {
                if (MyUtility.Convert.GetDate(CurrentMaintain["OutputDate"]) <= MyUtility.Convert.GetDate(MyUtility.GetValue.Lookup("select SewLock from System WITH (NOLOCK) ")))
                {
                    dateDate.Focus();
                    MyUtility.Msg.WarningBox(string.Format("Date can't earlier than Sewing Lock Date: {0}.", Convert.ToDateTime(MyUtility.GetValue.Lookup("select SewLock from System WITH (NOLOCK) ")).ToString(string.Format("{0}", Sci.Env.Cfg.DateStringFormat))));
                    return false;
                }
            }
            #endregion

            #region 檢查資料是否已存在
            if (MyUtility.Check.Seek(string.Format(@"select ID from SewingOutput WITH (NOLOCK) where OutputDate = '{0}' and SewingLineID = '{1}' and Shift = '{2}' and Team = '{3}' and FactoryID = '{4}' and ID <> '{5}' and Category = 'O'", Convert.ToDateTime(CurrentMaintain["OutputDate"]).ToString("d"), MyUtility.Convert.GetString(CurrentMaintain["SewingLineID"]), MyUtility.Convert.GetString(CurrentMaintain["Shift"]), MyUtility.Convert.GetString(CurrentMaintain["Team"]), MyUtility.Convert.GetString(CurrentMaintain["FactoryID"]), MyUtility.Convert.GetString(CurrentMaintain["ID"]))))
                {
                    MyUtility.Msg.WarningBox(string.Format("Date:{0}, Line:{1}, Shift:{2}, Team:{3} already exist, can't save!!",
                        Convert.ToDateTime(CurrentMaintain["OutputDate"]).ToString(string.Format("{0}", Sci.Env.Cfg.DateStringFormat)), MyUtility.Convert.GetString(CurrentMaintain["SewingLineID"]), MyUtility.Convert.GetString(CurrentMaintain["Shift"]), MyUtility.Convert.GetString(CurrentMaintain["Team"]), MyUtility.Convert.GetString(CurrentMaintain["FactoryID"])));
                    return false;
                }
            #endregion

            #region 先撈出所有SP的SewingSchedule資料
            DataTable SewingData;
            try
            {
                MyUtility.Tool.ProcessWithDatatable(((DataTable)detailgridbs.DataSource), "OrderID,ComboType", string.Format(@"select a.OrderId,a.ComboType,s.StandardOutput 
from #tmp a, SewingSchedule s WITH (NOLOCK) 
where a.OrderId = s.OrderID
and a.ComboType = s.ComboType
and s.SewingLineID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["SewingLineID"])), out SewingData, "#tmp");
            }
            catch (Exception ex)
            {
                ShowErr("Calculate error.", ex);
                return false;
            }
            #endregion
            
            #region 先算出QAQty,InLineQty,DefectQty,W/Hours
            DataTable SumQty;
            int gridQaQty, gridInlineQty, gridDefectQty;//加總表身的QAQty,InLineQty,DefectQty
            decimal gridWHours = 0;//加總表身W/Hours
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
                MyUtility.Tool.ProcessWithDatatable(((DataTable)detailgridbs.DataSource), "WorkHour,QAQty,InlineQty,DefectQty,OrderID,ComboType,Article,AutoCreate", strSumQty, out SumQty, "#tmp");
            }
            catch (Exception ex)
            {
                ShowErr("Calculate error.", ex);
                return false;
            }

            if (SumQty == null)
            {
                gridQaQty = 0;
                gridInlineQty = 0;
                gridDefectQty = 0;
                gridWHours = 0;
            }
            else
            {
                gridQaQty = MyUtility.Convert.GetInt(SumQty.Rows[0]["sumQAQty"]);
                gridInlineQty = MyUtility.Convert.GetInt(SumQty.Rows[0]["sumInlineQty"]);
                gridDefectQty = MyUtility.Convert.GetInt(SumQty.Rows[0]["sumDefectQty"]);
                gridWHours = MyUtility.Convert.GetDecimal(SumQty.Rows[0]["sumWorkHour"]);
            }
            #endregion 

            int recCnt = 0; //紀錄表身record數
            decimal gridTms = 0; //加總表身的TMS
            #region 刪除表身資料(OrderID, ComboType, Article)
            foreach (DataRow dr in DetailDatas)
            {
                if (MyUtility.Check.Empty(dr["OrderID"])||MyUtility.Check.Empty(dr["Article"]))
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

               //回寫表頭TMS
                if (gridQaQty == 0)
                {
                    gridTms = 0;
                }
                else
                {
                    if (dr["AutoCreate"].EqualString("False"))
                        gridTms = gridTms + (MyUtility.Convert.GetDecimal(dr["TMS"]) * MyUtility.Convert.GetDecimal(dr["QAQty"]) / MyUtility.Convert.GetDecimal(gridQaQty));
                }
                recCnt += 1;
                //填入HourlyStandardOutput
                DataRow[] sewing = SewingData.Select(string.Format("OrderID = '{0}' and ComboType = '{1}'", MyUtility.Convert.GetString(dr["OrderID"]), MyUtility.Convert.GetString(dr["ComboType"])));
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
                detailgrid.Focus();
                MyUtility.Msg.WarningBox("< Detail > can't be empty!");
                return false;
            }
            #endregion

            #region 表身W/Hours加總要等於表頭的W/Hours(Day)
            if (gridWHours != MyUtility.Convert.GetDecimal(CurrentMaintain["WorkHour"]))
            {
                MyUtility.Msg.WarningBox("The working hours summary is not equal to working hours/day, please correct, or else can't be saved.");
                return false;
            }
            #endregion

            #region 若sewingoutput.outputDate <= system.sewlock 表身Qty要等於表頭的Qty            
            DataTable sys;
            DBProxy.Current.Select(null, "select sewlock from system WITH (NOLOCK) ", out sys);
            DateTime? Sod = MyUtility.Convert.GetDate(CurrentMaintain["outputDate"]);
            DateTime? sl = MyUtility.Convert.GetDate(sys.Rows[0][0]);
            if (Sod <= sl)
            {
                decimal NQ = 0;
                foreach (DataRow dr in DetailDatas)
                {
                    if (!MyUtility.Check.Empty(dr["QAQty"]))
                    {
                        NQ += MyUtility.Convert.GetDecimal(dr["QAQty"]);
                    }
                }
                if (NQ != oldttlqaqty)
                {
                    MyUtility.Msg.WarningBox("QA Output shouled be the same as before.");
                    return false;
                }
            }
            #endregion

            DataTable dtSubDetail = null;
            foreach (DataRow dr in DetailDatas)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    DataTable subDt;
                    GetSubDetailDatas(dr, out subDt);
                    subDt = subDt.AsEnumerable().Where(row => true).CopyToDataTable();
                    subDt.Columns.Add("AutoCreate");
                    if (dtSubDetail == null) dtSubDetail = subDt.Clone();

                    foreach (DataRow subDr in subDt.Rows)
                    {
                        subDr["AutoCreate"] = dr["AutoCreate"];
                        dtSubDetail.ImportRow(subDr);
                    }
                }
            }

            #region 產出數量不能小於已出貨的數量
            if (SaveDeleteCheckPacking(dtSubDetail, true) == false)
            {
                return false;
            }
            #endregion

            #region 確認子單數量加總不會超過母單數量
            DualResult resultCheckSubOutputQty;
            DataTable dtCheckSubOutputQty;
            string strCheckSubOutputQty = @"
select Child.*
	   , MotherQaQty = isnull (Mother.MotherQaQty, 0)
from (
	select #tmp.ComboType
		   , #tmp.Article
		   , #tmp.SizeCode
		   , ChildQaQty = sum (#tmp.QAQty)
	from #tmp
	where Convert (bit, #tmp.AutoCreate) = 1
	group by #tmp.ComboType, #tmp.Article, #tmp.SizeCode
) Child
left join (
	select #tmp.OrderId, #tmp.ComboType
		   , #tmp.Article
		   , #tmp.SizeCode
		   , MotherQaQty = sum (#tmp.QAQty)
	from #tmp
	where exists (select 1 
				  from #tmp ToSPTmp
				  inner join Order_Qty_Garment OQG on ToSPTmp.OrderId = OQG.ID
													  and ToSPTmp.Article = OQG.Article
													  and ToSPTmp.SizeCode = OQG.SizeCode
				  inner join Orders ToSPOrders on OQG.ID = ToSPOrders.ID
				  inner join Style_Location SL on ToSPOrders.StyleUkey = SL.StyleUkey
												  and ToSPTmp.ComboType = SL.Location
				  where Convert (bit, ToSPTmp.AutoCreate) = 1
						and OQG.OrderIDFrom = #tmp.OrderId)
	group by #tmp.OrderId,#tmp.ComboType, #tmp.Article, #tmp.SizeCode
) Mother on Child.ComboType = Mother.ComboType
			and Child.Article = Mother.Article
			and Child.SizeCode = Mother.SizeCode
where ChildQaQty > isnull (MotherQaQty, 0)";

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
                    errMsg.Append(string.Format(@"
FromSP ComboType: <{0}> Article: <{1}> Size: <{2}> 
QAQty: <{3}>  less than AutoCreate Items QAQty: <{4}>", dtCheckSubOutputQty.Rows[i]["ComboType"].ToString()
                                                , dtCheckSubOutputQty.Rows[i]["Article"].ToString()
                                                , dtCheckSubOutputQty.Rows[i]["SizeCode"].ToString()
                                                , dtCheckSubOutputQty.Rows[i]["MotherQaQty"].ToString()
                                                , dtCheckSubOutputQty.Rows[i]["ChildQaQty"].ToString()));
                }

                if (errMsg.ToString().Empty() == false)
                {
                    MyUtility.Msg.WarningBox(errMsg.ToString());
                    return false;
                }
            }
            #endregion

            #region GetID
            if (IsDetailInserting)
            {
                string id = MyUtility.GetValue.GetID(MyUtility.GetValue.Lookup("FtyGroup", Sci.Env.User.Factory, "Factory", "ID") + "SM", "SewingOutput", DateTime.Today, 3, "Id", null);
                if (MyUtility.Check.Empty(id))
                {
                    MyUtility.Msg.WarningBox("GetID fail, please try again!");
                    return false;
                }
                CurrentMaintain["ID"] = id;
            }
            #endregion

            CurrentMaintain["QAQty"] = gridQaQty;
            CurrentMaintain["InlineQty"] = gridInlineQty;
            CurrentMaintain["DefectQty"] = gridDefectQty;
            CurrentMaintain["TMS"] = MyUtility.Math.Round(gridTms, 0);
            CurrentMaintain["Efficiency"] = MyUtility.Convert.GetDecimal(CurrentMaintain["TMS"]) * MyUtility.Convert.GetDecimal(CurrentMaintain["ManHour"]) == 0 ? 0 : MyUtility.Convert.GetDecimal(gridQaQty) / (3600 / MyUtility.Convert.GetDecimal(CurrentMaintain["TMS"]) * MyUtility.Convert.GetDecimal(CurrentMaintain["ManHour"])) * 100;
            return base.ClickSaveBefore();
        }

        protected override void ClickSaveAfter()
        {
            base.ClickSaveAfter();
            string checkNonData = string.Format(@"
delete sodd
from SewingOutput_Detail_Detail sodd
where not exists (select 1 
				  from SewingOutput_Detail sod 
				  where sodd.ID = sod.ID
						and sodd.SewingOutput_DetailUKey = sod.UKey
						and sodd.OrderId = sod.OrderId
						and sodd.ComboType = sod.ComboType
						and sodd.Article = sod.Article)
      and sodd.id = '{0}'", CurrentMaintain["ID"]);
            DualResult result = DBProxy.Current.Execute(null, checkNonData);
            if (result == false)
            {
                MyUtility.Msg.WarningBox(result.Description);
            }
        }

        protected override DualResult ClickSaveSubDetial(SubDetailSaveEventArgs e)
        {
            DualResult result = base.ClickSaveSubDetial(e);
            if (result == false)
            {
                return result;
            }

            #region 重新 修改、刪除第三層資料
            List<DataRow> Inserted = new List<DataRow>();
            List<DataRow> Updated = new List<DataRow>();
            List<DataRow> deleteList = new List<DataRow>();
            var ok = DBProxy.Current.GetTableSchema(null, this.SubGridAlias, out sub_Schema);
            if (!ok) { return ok; };

            foreach (KeyValuePair<DataRow, DataTable> it in e.SubDetails)
            {
                foreach (DataRow dr in it.Value.Rows)
                {
                    if (dr.RowState == DataRowState.Deleted)
                        continue;
                    if (MyUtility.Convert.GetInt(dr["QAQty"]) <= 0)
                        deleteList.Add(dr);
                    else
                    {
                        if (dr.RowState == DataRowState.Added && MyUtility.Convert.GetInt(dr["QAQty"]) > 0)
                        {
                            Inserted.Add(dr);
                        }
                        else if (MyUtility.Convert.GetInt(dr["QAQty"]) != MyUtility.Convert.GetInt(dr["QAQty", DataRowVersion.Original]))
                        {
                            Updated.Add(dr);
                        }
                    }
                }
            }


            List<DataRow> NewUpdated = new List<DataRow>();
            if (Updated.Count > 0 && false)
            {
                var newT = Updated[0].Table.Clone();
                for (int i = 0; i < Updated.Count; i++)
                {

                    var newOne = newT.NewRow();
                    newOne.ItemArray = Updated[i].ItemArray;
                    NewUpdated.Add(newOne);
                    newT.Rows.Add(newOne);
                    //newOne["QaQty"] = Updated[i]["qaqty"];
                }
                newT.AcceptChanges();
                for (int i = 0; i < Updated.Count; i++)
                {
                    NewUpdated[i]["QaQty"] = Updated[i]["qaqty"];
                }
            }

            List<DataRow> NewDelete = new List<DataRow>();
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
                        ShowErr("Error:", ec);
                    }
                    NewDelete.Add(newOne);
                    newT.Rows.Add(newOne);
                    //newOne["QaQty"] = Updated[i]["qaqty"];
                }
                newT.AcceptChanges();
                for (int i = 0; i < deleteList.Count; i++)
                {
                    NewDelete[i]["QaQty"] = 0;
                }
            }

            foreach (DataRow dr in Inserted)
            {
                string x = dr.RowState.ToString();
            }


            ok = DBProxy.Current.Deletes(null, sub_Schema, NewDelete);
            if (!ok) { return ok; };
             //ok = DBProxy.Current.Batch(null, sub_Schema, Updated);
            ok = DBProxy.Current.Batch(null, sub_Schema, NewUpdated);
            if (!ok) { return ok; };
           // ok = DBProxy.Current.Inserts(null, sub_Schema, Inserted);
           // if(!ok) { return ok; };
            #endregion
            return ok;
        }

        //Date
        private void dateDate_Validating(object sender, CancelEventArgs e)
        {
            if (EditMode && !MyUtility.Check.Empty(dateDate.Value) && dateDate.Value != dateDate.OldValue)
            {
                if (dateDate.Value > DateTime.Today)
                {
                    dateDate.Value = null;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("Date is later than today, pls pay attention!!");
                    return;
                }
                if (dateDate.Value <= MyUtility.Convert.GetDate(MyUtility.GetValue.Lookup("select SewLock from System WITH (NOLOCK) ")))
                {
                    dateDate.Value = null;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("Date can't earlier than Sewing Lock Date: {0}.", Convert.ToDateTime(MyUtility.GetValue.Lookup("select SewLock from System WITH (NOLOCK) ")).ToString(string.Format("{0}", Sci.Env.Cfg.DateStringFormat))));
                    return;
                }
            }
        }

        //Manpower
        private void numManpower_Validated(object sender, EventArgs e)
        {
            //值有異動過就要重算ManHour
            if (EditMode && numManpower.Value != numManpower.OldValue)
            {
                CalculateManHour();
            }
        }

        //W/Hours(Day)
        private void numWHours_Validated(object sender, EventArgs e)
        {
            //值有異動過就要重算ManHour
            if (EditMode && numWHours.Value != numWHours.OldValue)
            {
                CalculateManHour();
            }
        }

        //計算ManHour
        private void CalculateManHour()
        {
            CurrentMaintain["ManHour"] = MyUtility.Math.Round(MyUtility.Convert.GetDecimal(CurrentMaintain["Manpower"]) * MyUtility.Convert.GetDecimal(CurrentMaintain["WorkHour"]), 2);
        }

        //Share < working hours > to SP#
        private void btnShareWorkingHoursToSP_Click(object sender, EventArgs e)
        {
            DataTable SumQaQty;
            try
            {
                string strSumQaQty = @"
select sumQaqty = isnull(sum(QAQty*TMS),0)
       , RecCnt = isnull(count(QAQty),0)
from #tmp
where Convert (bit, AutoCreate) != 1";
                MyUtility.Tool.ProcessWithDatatable(((DataTable)detailgridbs.DataSource), "QAQty,TMS,OrderID,AutoCreate", strSumQaQty, out SumQaQty, "#tmp");
            }
            catch (Exception ex)
            {
                ShowErr("Calculate error.", ex);
                return;
            }

            if (SumQaQty == null)
            {
                return;
            }

            int recCnt = MyUtility.Convert.GetInt(SumQaQty.Rows[0]["RecCnt"]);
            decimal ttlQaqty = MyUtility.Convert.GetDecimal(SumQaQty.Rows[0]["sumQaqty"]);

            decimal subSum = 0;
            foreach (DataRow dr in ((DataTable)detailgridbs.DataSource).Rows)
            {
                recCnt = recCnt - 1;
                if (dr.RowState != DataRowState.Deleted)
                {
                    if (dr["AutoCreate"].EqualString("True"))
                    {
                        dr["WorkHour"] = 0;
                    }
                    else if (recCnt == 0)
                    {
                        dr["WorkHour"] = MyUtility.Convert.GetDecimal(CurrentMaintain["WorkHour"]) - subSum;
                    }
                    else
                    {
                        dr["WorkHour"] = ttlQaqty == 0 ? 0 : MyUtility.Math.Round(MyUtility.Convert.GetDecimal(dr["QAQty"]) * MyUtility.Convert.GetDecimal(dr["TMS"]) / ttlQaqty * MyUtility.Convert.GetDecimal(CurrentMaintain["WorkHour"]), 3);
                    }
                    subSum = subSum + MyUtility.Convert.GetDecimal(dr["WorkHour"]);
                }
            }
        }

        //Revised History
        private void btnRevisedHistory_Click(object sender, EventArgs e)
        {
            Sci.Win.UI.ShowHistory callNextForm = new Sci.Win.UI.ShowHistory("SewingOutput_History", MyUtility.Convert.GetString(CurrentMaintain["ID"]), "Status", reasonType: "Sewing_RVS", caption: "Revised History");
            callNextForm.ShowDialog(this);
        }

        //UnConfirm
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            Sci.Win.UI.SelectReason callReason = new Sci.Win.UI.SelectReason("Sewing_RVS", true);
            DialogResult dResult = callReason.ShowDialog(this);
            if (dResult == System.Windows.Forms.DialogResult.OK)
            {
                string insertCmd = string.Format(@"insert into SewingOutput_History (ID,HisType,OldValue,NewValue,ReasonID,Remark,AddName,AddDate)
values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}',GETDATE())", MyUtility.Convert.GetString(CurrentMaintain["ID"]), "Status", "Locked", "New", callReason.ReturnReason, callReason.ReturnRemark, Sci.Env.User.UserID);
                string updateCmd = string.Format(@"update SewingOutput set LockDate = null, Status = 'New' where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]));

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
                        ShowErr("Commit transaction error.", ex);
                        return;
                    }
                }
            }

           
        }

        /// <summary>
        /// 當 Row 資料修改 or 刪除時
        /// 必須檢查 Packing 數量
        /// </summary>
        /// <returns></returns>
        private bool CheckRemoveRow()
        {
            DataTable subDt;
            GetSubDetailDatas(CurrentDetailData, out subDt);

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
                string error = "";

                if (resultCheckQty == false)
                {
                    MyUtility.Msg.WarningBox(resultCheckQty.ToString());
                    return false;
                }
                else if (dtCheckQty != null && dtCheckQty.Rows.Count > 0)
                {
                    for (int i = 0; i < dtCheckQty.Rows.Count; i++)
                    {
                        error = error + string.Format(@"
Order: <{0}> ComboType: <{1}> Article: <{2}> Size: <{3}> QAQty: <{4}>  less than ShipQty: <{5}>", dtCheckQty.Rows[i]["Orderid"].ToString()
                                       , dtCheckQty.Rows[i]["ComboType"].ToString()
                                       , dtCheckQty.Rows[i]["Article"].ToString()
                                       , dtCheckQty.Rows[i]["SizeCode"].ToString()
                                       , dtCheckQty.Rows[i]["SewQty"].ToString()
                                       , dtCheckQty.Rows[i]["PackQty"].ToString());
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
        /// 刪除 & 儲存前 檢查 Packing 數量
        /// Save 必須加上 QaQty
        /// Delete 則不用
        /// </summary>
        /// <param name="dtSubDetail">第三層資料</param>
        /// <param name="saveCheck">true : Save; false : Delete</param>
        /// <returns></returns>
        private bool SaveDeleteCheckPacking(DataTable dtSubDetail, bool saveCheck)
        {
            DualResult resultCheckQty;
            DataTable dtCheckQty;

            if (!MyUtility.Check.Empty(dtSubDetail) && dtSubDetail.Rows.Count > 0)
            {
                string strCheckQty = string.Format(@" 
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
WHERE  sewqty < packqty ", (saveCheck) ? "+ t.qaqty " : "");

                resultCheckQty = MyUtility.Tool.ProcessWithDatatable(dtSubDetail, null, strCheckQty, out dtCheckQty);
                string error = "";

                if (resultCheckQty == false)
                {
                    MyUtility.Msg.WarningBox(resultCheckQty.ToString());
                    return false;
                }
                else if (dtCheckQty != null && dtCheckQty.Rows.Count > 0)
                {
                    for (int i = 0; i < dtCheckQty.Rows.Count; i++)
                    {
                        error = error + string.Format(@"
Order: <{0}> ComboType: <{1}> Article: <{2}> Size: <{3}> QAQty: <{4}>  less than ShipQty: <{5}>", dtCheckQty.Rows[i]["Orderid"].ToString()
                                       , dtCheckQty.Rows[i]["ComboType"].ToString()
                                       , dtCheckQty.Rows[i]["Article"].ToString()
                                       , dtCheckQty.Rows[i]["SizeCode"].ToString()
                                       , dtCheckQty.Rows[i]["SewQty"].ToString()
                                       , dtCheckQty.Rows[i]["PackQty"].ToString());
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
