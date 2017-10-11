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
using System.Data.SqlClient;

namespace Sci.Production.Shipping
{
    public partial class B42 : Sci.Win.Tems.Input6
    {
        Ict.Win.DataGridViewGeneratorNumericColumnSettings qty = new DataGridViewGeneratorNumericColumnSettings();
        DataTable tmpConsumptionArticle, tmpConsumptionSizecode, VNConsumption_Detail_Detail;
        public B42(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            MyUtility.Tool.SetupCombox(comboCategory, 2, 1, "B,Bulk,S,Sample");
            //取VNConsumption_Article, VNConsumption_SizeCode結構，存檔時使用
            DBProxy.Current.Select(null, "select * from VNConsumption_Article WITH (NOLOCK) where 1 = 0", out tmpConsumptionArticle);
            DBProxy.Current.Select(null, "select * from VNConsumption_SizeCode WITH (NOLOCK) where 1 = 0", out tmpConsumptionSizecode);
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            //新增Import From Barcode按鈕
            Sci.Win.UI.Button btn = new Sci.Win.UI.Button();
            btn.Text = "Batch Create";
            btn.Click += new EventHandler(btn_Click);
            browsetop.Controls.Add(btn);
            btn.Size = new Size(120, 30);//預設是(80,30)
            btn.Enabled = PublicPrg.Prgs.GetAuthority(Sci.Env.User.UserID, "B42. Custom SP# and Consumption", "CanNew");
            this.grid.Columns[0].Visible = false;
        }

        //Batch Create按鈕的Click事件
        private void btn_Click(object sender, EventArgs e)
        {
            Sci.Production.Shipping.B42_BatchCreate callNextForm = new Sci.Production.Shipping.B42_BatchCreate();
            DialogResult result = callNextForm.ShowDialog(this);

            ReloadDatas();
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : MyUtility.Convert.GetString(e.Master["ID"]);
            string contraceNo = (e.Master == null) ? "" : MyUtility.Convert.GetString(e.Master["VNContractID"]);
            this.DetailSelectCommand = string.Format(@"
select  vd.*
        , cd.Waste
from VNConsumption_Detail vd WITH (NOLOCK) 
left join VNContract_Detail cd WITH (NOLOCK) on  cd.NLCode = vd.NLCode 
                                                 and cd.ID = '{0}'
where vd.ID = '{1}'
order by CONVERT (int, SUBSTRING (vd.NLCode, 3, 3))", contraceNo, masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            numBalanceQty.Value = MyUtility.Convert.GetDecimal(CurrentMaintain["Qty"]) - MyUtility.Convert.GetDecimal(CurrentMaintain["PulloutQty"]);
            string colorWay = MyUtility.GetValue.Lookup(string.Format("select CONCAT(Article, ',') from VNConsumption_Article WITH (NOLOCK) where ID = '{0}' order by Article for xml path('')", MyUtility.Convert.GetString(CurrentMaintain["ID"])));
            editColorway.Text = MyUtility.Check.Empty(colorWay) ? "" : colorWay.Substring(0, colorWay.Length - 1);
            string sizeGroup = MyUtility.GetValue.Lookup(string.Format("select CONCAT(SizeCode, ',') from VNConsumption_SizeCode WITH (NOLOCK) where ID = '{0}' order by SizeCode for xml path('')", MyUtility.Convert.GetString(CurrentMaintain["ID"])));
            editSizeGroup.Text = MyUtility.Check.Empty(sizeGroup) ? "" : sizeGroup.Substring(0, sizeGroup.Length - 1);
            DBProxy.Current.Select(null, string.Format("select * from VNConsumption_Detail_Detail WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"])), out VNConsumption_Detail_Detail);
        }

        protected override void OnDetailGridSetup()
        {
            #region Qty DBClick
            qty.CellMouseDoubleClick += (s, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    DataRow dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (MyUtility.Convert.GetString(dr["UserCreate"]).ToUpper() == "TRUE")
                    {
                        MyUtility.Msg.InfoBox("This Customs Code is not create by the system, so no more detail can be show.");
                    }
                    else
                    {
                        DataTable detail2s;
                        try
                        {
                            MyUtility.Tool.ProcessWithDatatable(VNConsumption_Detail_Detail, "NLCode,SCIRefno,RefNo,Qty,LocalItem", string.Format(@"select a.RefNo,IIF(a.LocalItem = 1,a.Description,a.DescDetail) as Description,
IIF(a.LocalItem = 1,a.LocalSuppid,'') as SuppID,
IIF(a.LocalItem = 1,a.Category,IIF(a.Type = 'F','Fabric','Accessory')) as Type,
IIF(a.LocalItem = 1,a.LUnit,a.FUnit) as UnitID,a.Qty
from (
select t.NLCode,t.SCIRefno,t.RefNo,t.Qty,t.LocalItem,f.DescDetail,f.Type,f.CustomsUnit as FUnit,l.Description,l.Category,l.LocalSuppid,l.CustomsUnit as LUnit
from #tmp t
left join Fabric f WITH (NOLOCK) on t.SCIRefno = f.SCIRefno
left join LocalItem l WITH (NOLOCK) on t.RefNo = l.RefNo
where t.NLCode = '{0}') a
order by RefNo", MyUtility.Convert.GetString(dr["NLCode"])), out detail2s);
                        }
                        catch (Exception ex)
                        {
                            MyUtility.Msg.ErrorBox("Query detail data fail!!\r\n" + ex.ToString());
                            return;
                        }
                        Sci.Production.Shipping.B42_Detail callNextForm = new Sci.Production.Shipping.B42_Detail(detail2s);
                        DialogResult result = callNextForm.ShowDialog(this);
                        callNextForm.Dispose();
                    }
                }
            };
            #endregion

            #region NLCode
            DataGridViewGeneratorMaskedTextColumnSettings Nlcode = new DataGridViewGeneratorMaskedTextColumnSettings();
            Nlcode.CellEditable += (s, e) =>
            {
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (!MyUtility.Convert.GetBool(dr["UserCreate"])) e.IsEditable = false;
            };

            #endregion

            base.OnDetailGridSetup();
            Helper.Controls.Grid.Generator(this.detailgrid)
                .MaskedText("NLCode", header: "Customs Code", width: Widths.AnsiChars(7),mask: "LL000", settings: Nlcode)
                .Text("UnitID", header: "Unit", width: Widths.AnsiChars(7), iseditingreadonly: true)
                .Numeric("SystemQty", header: "System Qty", decimal_places: 3, width: Widths.AnsiChars(14), iseditingreadonly: true)
                .Numeric("Qty", header: "Qty", decimal_places: 6, width: Widths.AnsiChars(15), settings: qty)
                .Numeric("Waste", header: "Waste", decimal_places: 6, iseditingreadonly: true)
                .CheckBox("UserCreate", header: "Create by user", width: Widths.AnsiChars(3), iseditable: false, trueValue: 1, falseValue: 0);
        }

        protected override void EnsureToolbarExt()
        {
            base.EnsureToolbarExt();

        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["Status"] = "New";
            CurrentMaintain["Category"] = "B";
            CurrentMaintain["VNContractID"] = MyUtility.GetValue.Lookup(@"
select  ID 
from VNContract WITH (NOLOCK) 
where StartDate = (select   MAX(StartDate) 
                   from VNContract WITH (NOLOCK) 
                   where    GETDATE() between StartDate and EndDate 
                            and Status = 'Confirmed')");
            CurrentMaintain["CustomSP"] = "SP" + MyUtility.Convert.GetString(MyUtility.Convert.GetInt(MyUtility.GetValue.Lookup(string.Format(@"
select  CustomSP = isnull (MAX (CustomSP), 'SP000000') 
from VNConsumption WITH (NOLOCK) 
where VNContractID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["VNContractID"]))).Substring(2)) + 1).PadLeft(6, '0');
            CurrentMaintain["VNMultiple"] = MyUtility.GetValue.Lookup(@"
select  VNMultiple 
from System WITH (NOLOCK) ");
        }

        protected override bool ClickEditBefore()
        {
            if (MyUtility.Convert.GetString(CurrentMaintain["Status"]).ToUpper() == "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("This record already confirmed, can't edit!!");
                return false;
            }
            return base.ClickEditBefore();
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            txtCustomSPNo.ReadOnly = true;
            txtContractNo.ReadOnly = true;
            dateDate.ReadOnly = true;
        }

        protected override bool ClickDeleteBefore()
        {
            if (MyUtility.Convert.GetString(CurrentMaintain["Status"]).ToUpper() == "CONFIRMED")
            {
                MyUtility.Msg.WarningBox("This record already confirmed, can't delete!!");
                return false;
            }
            return base.ClickDeleteBefore();
        }

        protected override DualResult ClickDeletePost()
        {
            IList<string> deleteCmds = new List<string>();
            deleteCmds.Add(string.Format("delete VNConsumption_Article where ID = '{0}';", MyUtility.Convert.GetString(CurrentMaintain["ID"])));
            deleteCmds.Add(string.Format("delete VNConsumption_SizeCode where ID = '{0}';", MyUtility.Convert.GetString(CurrentMaintain["ID"])));
            deleteCmds.Add(string.Format("delete VNConsumption_Detail_Detail where ID = '{0}';", MyUtility.Convert.GetString(CurrentMaintain["ID"])));

            return DBProxy.Current.Executes(null, deleteCmds);
        }

        protected override bool ClickSaveBefore()
        {
            #region 檢查必輸欄位
            if (MyUtility.Check.Empty(CurrentMaintain["CustomSP"]))
            {
                txtCustomSPNo.Focus();
                MyUtility.Msg.WarningBox("Custom SP# can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["VNContractID"]))
            {
                txtContractNo.Focus();
                MyUtility.Msg.WarningBox("Contract no. can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["CDate"]))
            {
                dateDate.Focus();
                MyUtility.Msg.WarningBox("Date can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["StyleID"]))
            {
                txtStyle.Focus();
                MyUtility.Msg.WarningBox("Style can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["Category"]))
            {
                comboCategory.Focus();
                MyUtility.Msg.WarningBox("Category can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["SizeCode"]))
            {
                txtSize.Focus();
                MyUtility.Msg.WarningBox("Size can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["Qty"]))
            {
                numQty.Focus();
                MyUtility.Msg.WarningBox("Q'ty can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(editColorway.Text))
            {
                editColorway.Focus();
                MyUtility.Msg.WarningBox("Color way can't empty!!");
                return false;
            }

            if (MyUtility.Check.Empty(editSizeGroup.Text))
            {
                editSizeGroup.Focus();
                MyUtility.Msg.WarningBox("Size Group can't empty!!");
                return false;
            }
            #endregion

            int detailRecord = 0;   //紀錄表身資料筆數

            #region 刪除表身Qty為0的資料
            foreach (DataRow dr in DetailDatas)
            {
                if (MyUtility.Check.Empty(dr["Qty"]))
                {
                    dr.Delete();
                    continue;
                }
                detailRecord++;
            }
            #endregion

            #region 表身資料不可為空
            if (detailRecord == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't empty!!");
                return false;
            }
            #endregion

            //Get ID && Get Version
            if (IsDetailInserting)
            {
                string newID = MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "SP", "VNConsumption", Convert.ToDateTime(CurrentMaintain["CDate"]), 2, "ID", null);
                if (MyUtility.Check.Empty(newID))
                {
                    MyUtility.Msg.WarningBox("GetID fail, please try again!");
                    return false;
                }
                CurrentMaintain["ID"] = newID;

                string maxVersion = MyUtility.GetValue.Lookup(string.Format("select isnull(MAX(Version),0) as MaxVersion from VNConsumption WITH (NOLOCK) where StyleUKey = {0}", MyUtility.Convert.GetString(CurrentMaintain["StyleUKey"])));
                CurrentMaintain["Version"] = MyUtility.Convert.GetString(MyUtility.Convert.GetInt(maxVersion) + 1).PadLeft(3, '0');
            }

            //準備資料
            tmpConsumptionArticle.Clear();
            tmpConsumptionSizecode.Clear();
            string[] colorway = editColorway.Text.Split(',');
            string[] sizecode = editSizeGroup.Text.Split(',');
            foreach (string s in colorway)
            {
                DataRow dr = tmpConsumptionArticle.NewRow();
                dr["ID"] = CurrentMaintain["ID"];
                dr["Article"] = s;
                tmpConsumptionArticle.Rows.Add(dr);
            }

            foreach (string s in sizecode)
            {
                DataRow dr = tmpConsumptionSizecode.NewRow();
                dr["ID"] = CurrentMaintain["ID"];
                dr["SizeCode"] = s;
                tmpConsumptionSizecode.Rows.Add(dr);
            }

            foreach (DataRow dr in VNConsumption_Detail_Detail.Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    dr["ID"] = CurrentMaintain["ID"];
                }
            }

            #region 計算出 Qty(現有欄位)時, 將數值一併寫到 SystemQty 欄位
            foreach (DataRow dr in ((DataTable)((BindingSource)this.detailgrid.DataSource).DataSource).Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    if (Convert.ToBoolean(dr["UserCreate"]))
                    {
                        dr["SystemQty"] = dr["Qty"];
                        dr.EndEdit();
                    }
                }
            }
            #endregion

            return base.ClickSaveBefore();
        }

        protected override DualResult ClickSavePost()
        {
            //存VNConsumption_Detail_Detail, VNConsumption_Article, VNConsumption_SizeCode資料
            if (!MyUtility.Tool.CursorUpdateTable(VNConsumption_Detail_Detail, "VNConsumption_Detail_Detail", "Production"))
            {
                DualResult failResult = new DualResult(false, "Save VNConsumption_Detail_Detail fail!!");
                return failResult;
            }

            if (!MyUtility.Tool.CursorUpdateTable(tmpConsumptionArticle, "VNConsumption_Article", "Production"))
            {
                DualResult failResult = new DualResult(false, "Save VNConsumption_Article fail!!");
                return failResult;
            }

            if (!MyUtility.Tool.CursorUpdateTable(tmpConsumptionSizecode, "VNConsumption_SizeCode", "Production"))
            {
                DualResult failResult = new DualResult(false, "Save VNConsumption_SizeCode fail!!");
                return failResult;
            }

            return base.ClickSavePost();
        }

        protected override void OnDetailGridInsertClick()
        {
            base.OnDetailGridInsertClick();
            DataRow newrow = detailgrid.GetDataRow(detailgrid.GetSelectedRowIndex());
            newrow["UserCreate"] = 1;
        }

        protected override void OnDetailGridAppendClick()
        {
            base.OnDetailGridAppendClick();
            DataRow newrow = detailgrid.GetDataRow(detailgrid.GetSelectedRowIndex());
            newrow["UserCreate"] = 1;
        }

        protected override void OnDetailGridDelete()
        {
            if (CurrentDetailData != null)
            {
                string nlCode = MyUtility.Convert.GetString(CurrentDetailData["NLCode"]); //紀錄要被刪除的NLCode
                string userCreate = MyUtility.Convert.GetString(CurrentDetailData["UserCreate"]).ToUpper();
                base.OnDetailGridDelete();
                if (userCreate == "FALSE")
                {
                    foreach (DataRow dr in VNConsumption_Detail_Detail.ToList())
                    {
                        if (dr.RowState != DataRowState.Deleted && MyUtility.Convert.GetString(dr["NLCode"]) == nlCode)
                        {
                            dr.Delete();
                        }
                    }
                }
            }
        }

        protected override bool ClickPrint()
        {
            Sci.Production.Shipping.B42_Print callPurchaseForm = new Sci.Production.Shipping.B42_Print();
            callPurchaseForm.ShowDialog(this);
            return base.ClickPrint();
        }

        //Contract No.
        private void txtContractNo_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (EditMode)
            {
                Sci.Win.Tools.SelectItem item = new Win.Tools.SelectItem("select ID,StartDate,EndDate from VNContract WITH (NOLOCK) where GETDATE() between StartDate and EndDate and Status = 'Confirmed'", "15,10,10", txtContractNo.Text, headercaptions: "Contract No.,Start Date, End Date");
                DialogResult returnResult = item.ShowDialog();
                if (returnResult == DialogResult.Cancel) { return; }
                txtContractNo.Text = item.GetSelectedString();
            }
        }

        //Contract No.
        private void txtContractNo_Validating(object sender, CancelEventArgs e)
        {
            if (EditMode && txtContractNo.OldValue != txtContractNo.Text && !MyUtility.Check.Empty(txtContractNo.Text))
            {
                if (!MyUtility.Check.Seek(string.Format("select ID from VNContract WITH (NOLOCK) where ID = '{0}'", txtContractNo.Text)))
                {
                    txtContractNo.Text = "";
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("Contract no. not found!!");
                    return;
                }
                else if (!MyUtility.Check.Seek(string.Format("select ID from VNContract WITH (NOLOCK) where ID = '{0}' and GETDATE() between StartDate and EndDate'", txtContractNo.Text)))
                {
                    txtContractNo.Text = "";
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox("This Contract can't use.");
                    return;
                }
            }
        }

        //Style
        private void txtStyle_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (EditMode)
            {
                Sci.Win.Tools.SelectItem item = new Win.Tools.SelectItem("Select ID, SeasonID, BrandID,Ukey,CPU from Style WITH (NOLOCK) Order By BrandID, ID, SeasonID", "15,10,10,0", txtContractNo.Text, headercaptions: "Contract No.,Start Date, End Date,");
                DialogResult returnResult = item.ShowDialog();
                if (returnResult == DialogResult.Cancel) { return; }
                IList<DataRow> selectedData = item.GetSelecteds();
                CurrentMaintain["StyleID"] = item.GetSelectedString();
                CurrentMaintain["SeasonID"] = selectedData[0]["SeasonID"];
                CurrentMaintain["BrandID"] = selectedData[0]["BrandID"];
                CurrentMaintain["StyleUKey"] = selectedData[0]["Ukey"];
                CurrentMaintain["CPU"] = selectedData[0]["CPU"];
            }
        }

        //Size
        private void txtSize_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            if (EditMode)
            {
                Sci.Win.Tools.SelectItem item = new Win.Tools.SelectItem(string.Format("select SizeCode from Style_SizeCode WITH (NOLOCK) where {0} order by Seq", MyUtility.Check.Empty(CurrentMaintain["StyleUKey"]) ? "1=0" : "StyleUkey = " + MyUtility.Convert.GetString(CurrentMaintain["StyleUKey"])), "15,10,10,0", txtStyle.Text, headercaptions: "Size");
                DialogResult returnResult = item.ShowDialog();
                if (returnResult == DialogResult.Cancel) { return; }
                CurrentMaintain["SizeCode"] = item.GetSelectedString();
            }
        }

        //Color Way
        private void editColorway_PopUp(object sender, Win.UI.EditBoxPopUpEventArgs e)
        {
            if (EditMode)
            {
                Sci.Win.Tools.SelectItem2 item = new Win.Tools.SelectItem2(string.Format("select Article from Style_Article WITH (NOLOCK) where {0} order by Seq", MyUtility.Check.Empty(CurrentMaintain["StyleUKey"]) ? "1=0" : "StyleUkey = " + MyUtility.Convert.GetString(CurrentMaintain["StyleUKey"])), "Color Way", "8", editColorway.Text, null, null, null);
                DialogResult returnResult = item.ShowDialog();
                if (returnResult == DialogResult.Cancel) { return; }
                editColorway.Text = item.GetSelectedString();
            }
        }

        //Size Group
        private void editSizeGroup_PopUp(object sender, Win.UI.EditBoxPopUpEventArgs e)
        {
            if (EditMode)
            {
                Sci.Win.Tools.SelectItem2 item = new Win.Tools.SelectItem2(string.Format("select SizeCode from Style_SizeCode WITH (NOLOCK) where {0} order by Seq", MyUtility.Check.Empty(CurrentMaintain["StyleUKey"]) ? "1=0" : "StyleUkey = " + MyUtility.Convert.GetString(CurrentMaintain["StyleUKey"])), "Color Way", "8", editSizeGroup.Text, null, null, null);
                DialogResult returnResult = item.ShowDialog();
                if (returnResult == DialogResult.Cancel) { return; }
                editSizeGroup.Text = item.GetSelectedString();
            }
        }

        //Sketch
        private void btnSketch_Click(object sender, EventArgs e)
        {
            Sci.Production.IE.P01_Sketch callNextForm = new Sci.Production.IE.P01_Sketch(CurrentMaintain);
            DialogResult result = callNextForm.ShowDialog(this);
        }

        //Calculate
        private void btnCalculate_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(CurrentMaintain["StyleID"]) || MyUtility.Check.Empty(CurrentMaintain["Category"]) || MyUtility.Check.Empty(CurrentMaintain["SizeCode"]) || MyUtility.Check.Empty(editColorway.Text))
            {
                MyUtility.Msg.WarningBox("Style, Category, Size and Color way can't empty!!");
                return;
            }

            DataTable queryDetailData, queryDetail2Data, fixDeclareData, necessaryItem, invalidData;
            StringBuilder sqlCmd = new StringBuilder();
            StringBuilder needItem = new StringBuilder();
            StringBuilder emptyNLCode = new StringBuilder();
            StringBuilder wrongUnit = new StringBuilder();
            StringBuilder allMessage = new StringBuilder();
            string[] colorway = editColorway.Text.Split(',');
            #region 組撈Detail_Detail Data的SQL
            sqlCmd.Append(string.Format(@"Declare @styleukey bigint,
		@sizecode varchar(8),
		@article varchar(8),
		@category varchar(1),
		@vncontractid varchar(15)
set @styleukey = {0}
set @sizecode = '{1}'
set @article = '{2}'
set @category = '{3}'
set @vncontractid = '{4}';

with tmpMarkerData
as (
select sm.MarkerName,sm.StyleUkey,sm.FabricPanelCode,sma.Article,sms.SizeCode,dbo.MarkerLengthToYDS(sm.MarkerLength) as markerYDS,
sm.Width,sms.Qty,sc.FabricCode,sfqt.QTFabricCode
from Style_MarkerList sm WITH (NOLOCK) 
inner join Style_MarkerList_SizeQty sms WITH (NOLOCK) on sm.Ukey = sms.Style_MarkerListUkey and sms.SizeCode = @sizecode
inner join Style_ColorCombo sc WITH (NOLOCK) on sc.StyleUkey = sm.StyleUkey and sc.FabricPanelCode = sm.FabricPanelCode
left join Style_MarkerList_Article sma WITH (NOLOCK) on sm.Ukey = sma.Style_MarkerListUkey 
left join Style_FabricCode_QT sfqt WITH (NOLOCK) on sm.FabricPanelCode = sfqt.FabricPanelCode and sm.StyleUkey = sfqt.StyleUkey
where sm.MixedSizeMarker = 1 and sm.StyleUkey = @styleukey and (sma.Article is null or sma.Article = @article)
and sc.Article = @article
),
tmpFabricCode
as (
select t.markerYDS,t.Width,t.Qty, IIF(t.QTFabricCode is null, sb.SCIRefno, sb1.SCIRefno) as SCIRefNo,
IIF(t.QTFabricCode is null, sb.SuppIDBulk, sb1.SuppIDBulk) as SuppIDBulk
from tmpMarkerData t
left join Style_BOF sb WITH (NOLOCK) on sb.StyleUkey = t.StyleUkey and sb.FabricCode = t.FabricCode
left join Style_BOF sb1 WITH (NOLOCK) on sb1.StyleUkey = t.StyleUkey and sb1.FabricCode = t.QTFabricCode
),
tmpBOFRateData
as (
select t.markerYDS,'YDS' as UsageUnit,t.Qty,f.SCIRefno,f.Refno,f.BrandID,f.NLCode,f.HSCode,f.CustomsUnit,f.Width,f.Type,
f.PcsWidth,f.PcsLength,f.PcsKg,
isnull((select RateValue from dbo.View_Unitrate where FROM_U = 'YDS' and TO_U = f.CustomsUnit),1) as RateValue,
(select RateValue from dbo.View_Unitrate where FROM_U = 'YDS' and TO_U = 'M') as M2RateValue,
isnull((select Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = 'YDS' and UnitTo = f.CustomsUnit),'') as UnitRate,
isnull((select Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = 'YDS' and UnitTo = 'M'),'') as M2UnitRate
from tmpFabricCode t
inner join Fabric f WITH (NOLOCK) on f.SCIRefno = t.SCIRefno
where (t.SuppIDBulk <> 'FTY' or t.SuppIDBulk <> 'FTY-C')
and f.NoDeclare = 0),
tmpBOFNewQty
as (
select SCIRefno,Refno,BrandID,NLCode,HSCode,CustomsUnit,
([dbo].getVNUnitTransfer(Type,UsageUnit,CustomsUnit,markerYDS,Width,PcsWidth,PcsLength,PcsKg,IIF(CustomsUnit = 'M2',M2RateValue,RateValue),IIF(CustomsUnit = 'M2',M2UnitRate,UnitRate)))/Qty as NewQty
from tmpBOFRateData),
tmpBOFData
as (
select SCIRefno,Refno,BrandID,NLCode,HSCode,CustomsUnit,sum(isnull(NewQty,0)) as Qty, 0 as LocalItem
from tmpBOFNewQty
group by SCIRefno,Refno,BrandID,NLCode,HSCode,CustomsUnit),
tmpBOA
as (
select sb.StyleUkey,sb.Ukey,sb.Refno,sb.SCIRefno,sb.SuppIDBulk,sb.SizeItem,sb.PatternPanel,sb.BomTypeArticle,sb.BomTypeColor,sb.ConsPC,
sc.ColorID,f.UsageUnit,f.HSCode,f.NLCode,f.CustomsUnit,f.PcsWidth,f.PcsLength,f.PcsKg,f.BomTypeCalculate,f.Type,f.BrandID
from Style_BOA sb WITH (NOLOCK) 
left join Style_ColorCombo sc WITH (NOLOCK) on sc.StyleUkey = sb.StyleUkey and sc.PatternPanel = sb.PatternPanel and sc.Article = @article
left join Fabric f WITH (NOLOCK) on sb.SCIRefno = f.SCIRefno
where sb.StyleUkey = @styleukey
and sb.IsCustCD <> 2
and (sb.SuppIDBulk <> 'FTY' and sb.SuppIDBulk <> 'FTY-C')
),
tmpBOAPrepareData
as (
select t.*,IIF(BomTypeCalculate = 1,(select dbo.GetDigitalValue(SizeSpec) from Style_SizeSpec WITH (NOLOCK) where StyleUkey = t.StyleUkey and SizeItem = t.SizeItem and SizeCode = @sizecode),ConsPC) as SizeSpec,
isnull((select RateValue from dbo.View_Unitrate where FROM_U = t.UsageUnit and TO_U = t.CustomsUnit),1) as RateValue,
(select RateValue from dbo.View_Unitrate where FROM_U = t.UsageUnit and TO_U = 'M') as M2RateValue,
isnull((select Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = t.UsageUnit and UnitTo = t.CustomsUnit),'') as UnitRate,
isnull((select Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = t.UsageUnit and UnitTo = 'M'),'') as M2UnitRate
from tmpBOA t
where (t.BomTypeArticle = 0 and t.BomTypeColor = 0) or ((t.BomTypeArticle = 1 or t.BomTypeColor = 1) and t.ColorID is not null)
),
tmpBOANewQty
as (
select SCIRefno,Refno,BrandID,NLCode,HSCode,CustomsUnit,
[dbo].getVNUnitTransfer(Type,UsageUnit,CustomsUnit,SizeSpec,0,PcsWidth,PcsLength,PcsKg,IIF(CustomsUnit = 'M2',M2RateValue,RateValue),IIF(CustomsUnit = 'M2',M2UnitRate,UnitRate)) as NewQty
from tmpBOAPrepareData
),
tmpBOAData
as (
select SCIRefno,Refno,BrandID,NLCode,HSCode,CustomsUnit,sum(isnull(NewQty,0)) as Qty, 0 as LocalItem
from tmpBOANewQty
group by SCIRefno,Refno,BrandID,NLCode,HSCode,CustomsUnit),
tmpLocalPO
as (
select ld.Refno,ld.Qty,ld.UnitId,li.MeterToCone,li.NLCode,li.HSCode,li.CustomsUnit,li.PcsWidth,li.PcsLength,li.PcsKg,o.Qty as OrderQty,isnull(vd.Waste,0) as Waste
from LocalPO_Detail ld WITH (NOLOCK) 
left join LocalItem li WITH (NOLOCK) on li.RefNo = ld.Refno
left join Orders o WITH (NOLOCK) on ld.OrderId = o.ID
left join VNContract_Detail vd on vd.ID = @vncontractid and vd.NLCode = li.NLCode
where ld.OrderId = (select TOP 1 ID from Orders WITH (NOLOCK) where StyleUkey = @styleukey and Category = @category order by BuyerDelivery,ID)
and li.NoDeclare = 0
),
tmpConeToM
as (
select Refno,IIF(UnitId = 'CONE',Qty*MeterToCone,Qty) as Qty,OrderQty, IIF(UnitId = 'CONE','M',UnitId) as UnitId,Waste,
NLCode,HSCode,CustomsUnit,PcsWidth,PcsLength,PcsKg
from tmpLocalPO),
tmpPrepareRate
as (
select Refno,Qty/OrderQty as Qty,UnitId,NLCode,HSCode,CustomsUnit,PcsWidth,PcsLength,PcsKg,Waste,
isnull((select RateValue from dbo.View_Unitrate where FROM_U = UnitId and TO_U = CustomsUnit),1) as RateValue,
(select RateValue from dbo.View_Unitrate where FROM_U = UnitId and TO_U = 'M') as M2RateValue,
isnull((select Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = UnitId and UnitTo = CustomsUnit),'') as UnitRate,
isnull((select Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = UnitId and UnitTo = 'M'),'') as M2UnitRate
from tmpConeToM),
tmpLocalNewQty
as (
select Refno as SCIRefno,Refno,'' as BrandID,NLCode,HSCode,CustomsUnit,Waste,
[dbo].getVNUnitTransfer('',UnitId,CustomsUnit,Qty,0,PcsWidth,PcsLength,PcsKg,IIF(CustomsUnit = 'M2',M2RateValue,RateValue),IIF(CustomsUnit = 'M2',M2UnitRate,UnitRate)) as NewQty
from tmpPrepareRate
),
tmpLocalData
as (
select SCIRefno,Refno,BrandID,NLCode,HSCode,CustomsUnit,sum(isnull(NewQty,0)-(isnull(NewQty,0)*isnull(Waste,0))) as Qty, 1 as LocalItem
from tmpLocalNewQty
group by SCIRefno,Refno,BrandID,NLCode,HSCode,CustomsUnit)

select SCIRefno,Refno,BrandID,NLCode,HSCode,CustomsUnit,sum(Qty) as Qty,LocalItem
from (
select * from tmpBOFData
union all
select * from tmpBOAData
union all
select * from tmpLocalData) a
group by SCIRefno,Refno,BrandID,NLCode,HSCode,CustomsUnit,LocalItem", MyUtility.Convert.GetString(CurrentMaintain["StyleUKey"]), MyUtility.Convert.GetString(CurrentMaintain["SizeCode"]), colorway[0], MyUtility.Convert.GetString(CurrentMaintain["Category"]), MyUtility.Convert.GetString(CurrentMaintain["VNContractID"])));
            #endregion
            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out queryDetail2Data);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query detail2 data fail!!\r\n" + result.ToString());
                return;
            }

            #region 整理出Detail Data
            try
            {
                MyUtility.Tool.ProcessWithDatatable(queryDetail2Data, "NLCode,HSCode,CustomsUnit,Qty", "select NLCode,HSCode,CustomsUnit,SUM(Qty) as Qty from #tmp group by NLCode,HSCode,CustomsUnit", out queryDetailData);
            }
            catch (Exception ex)
            {
                MyUtility.Msg.ErrorBox("Query detail data fail!!\r\n" + ex.ToString());
                return;
            }
            #endregion

            sqlCmd.Clear();
            #region 組撈FixDeclare的資料
            sqlCmd.Append(string.Format(@"with tmpFixDeclare
as (
select vfd.* ,sa.TissuePaper as ArticleTissuePaper,isnull(s.CTNQty,0) as CTNQty
from VNFixedDeclareItem vfd WITH (NOLOCK) 
left join Style_Article sa WITH (NOLOCK) on sa.StyleUkey = {0} and sa.Article = '{1}'
left join Style s WITH (NOLOCK) on s.Ukey = {0}
)
select NLCode,HSCode,UnitID as CustomsUnit,IIF(Type = 1, Qty, IIF(CTNQty = 0,0,ROUND(Qty/CTNQty,3))) as Qty
from tmpFixDeclare
where TissuePaper = 0 or (TissuePaper = 1 and ArticleTissuePaper = 1)", MyUtility.Convert.GetString(CurrentMaintain["StyleUKey"]), colorway[0]));
            #endregion
            result = DBProxy.Current.Select(null, sqlCmd.ToString(), out fixDeclareData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query fix declare data fail!!\r\n" + result.ToString());
                return;
            }

            #region 將FixDeclare資料整合進Detail Data
            foreach (DataRow dr in fixDeclareData.Rows)
            {
                DataRow[] findrow = queryDetailData.Select(string.Format("NLCode = '{0}'", MyUtility.Convert.GetString(dr["NLCode"])));
                if (findrow.Length == 0)
                {
                    dr.AcceptChanges();
                    dr.SetAdded();
                    queryDetailData.ImportRow(dr);
                }
                else
                {
                    findrow[0]["Qty"] = MyUtility.Convert.GetDouble(dr["Qty"]);
                }
            }
            #endregion

            sqlCmd.Clear();
            #region 檢查是否有每次都該要出現的項目
            sqlCmd.Append(string.Format("select NLCode from VNContract_Detail WITH (NOLOCK) where ID = '{0}' and NecessaryItem = 1", MyUtility.Convert.GetString(CurrentMaintain["VNContractID"])));
            result = DBProxy.Current.Select(null, sqlCmd.ToString(), out necessaryItem);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query necessary item data fail!!\r\n" + result.ToString());
                return;
            }
            foreach (DataRow dr in necessaryItem.Rows)
            {
                DataRow[] findrow = queryDetailData.Select(string.Format("NLCode = '{0}'", MyUtility.Convert.GetString(dr["NLCode"])));
                if (findrow.Length == 0)
                {
                    needItem.Append(string.Format("{0},", MyUtility.Convert.GetString(dr["NLCode"])));
                }
            }
            #endregion

            sqlCmd.Clear();
            #region 撈出NL Code為空或單位無法轉換的資料
            sqlCmd.Append(string.Format(@"Declare @styleukey bigint,
		@sizecode varchar(8),
		@article varchar(8),
		@category varchar(1),
		@vncontractid varchar(15)
set @styleukey = {0}
set @sizecode = '{1}'
set @article = '{2}'
set @category = '{3}'
set @vncontractid = '{4}';

with tmpMarkerData
as (
select sm.MarkerName,sm.StyleUkey,sm.FabricPanelCode,sma.Article,sms.SizeCode,dbo.MarkerLengthToYDS(sm.MarkerLength) as markerYDS,
sm.Width,sms.Qty,sc.FabricCode,sfqt.QTFabricCode
from Style_MarkerList sm WITH (NOLOCK) 
inner join Style_MarkerList_SizeQty sms WITH (NOLOCK) on sm.Ukey = sms.Style_MarkerListUkey and sms.SizeCode = @sizecode
inner join Style_ColorCombo sc WITH (NOLOCK) on sc.StyleUkey = sm.StyleUkey and sc.FabricPanelCode = sm.FabricPanelCode
left join Style_MarkerList_Article sma WITH (NOLOCK) on sm.Ukey = sma.Style_MarkerListUkey 
left join Style_FabricCode_QT sfqtWITH (NOLOCK)  on sm.FabricPanelCode = sfqt.FabricPanelCode and sm.StyleUkey = sfqt.StyleUkey
where sm.MixedSizeMarker = 1 and sm.StyleUkey = @styleukey and (sma.Article is null or sma.Article = @article)
and sc.Article = @article
),
tmpFabricCode
as (
select t.markerYDS,t.Width,t.Qty, IIF(t.QTFabricCode is null, sb.SCIRefno, sb1.SCIRefno) as SCIRefNo,
IIF(t.QTFabricCode is null, sb.SuppIDBulk, sb1.SuppIDBulk) as SuppIDBulk
from tmpMarkerData t
left join Style_BOF sb WITH (NOLOCK) on sb.StyleUkey = t.StyleUkey and sb.FabricCode = t.FabricCode
left join Style_BOF sb1 WITH (NOLOCK) on sb1.StyleUkey = t.StyleUkey and sb1.FabricCode = t.QTFabricCode
),
tmpBOFRateData
as (
select 'YDS' as UsageUnit,f.SCIRefno,f.Refno,f.BrandID,f.NLCode,f.CustomsUnit,f.Type,
isnull((select Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = 'YDS' and UnitTo = f.CustomsUnit),'') as UnitRate,
isnull((select Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = 'YDS' and UnitTo = 'M'),'') as M2UnitRate
from tmpFabricCode t
inner join Fabric f WITH (NOLOCK) on f.SCIRefno = t.SCIRefno
where (t.SuppIDBulk <> 'FTY' or t.SuppIDBulk <> 'FTY-C')
and f.NoDeclare = 0),
tmpBOA
as (
select sb.StyleUkey,sb.Ukey,sb.Refno,sb.SCIRefno,sb.SuppIDBulk,sb.SizeItem,sb.PatternPanel,sb.BomTypeArticle,sb.BomTypeColor,sb.ConsPC,
sc.ColorID,f.UsageUnit,f.HSCode,f.NLCode,f.CustomsUnit,f.PcsWidth,f.PcsLength,f.PcsKg,f.BomTypeCalculate,f.Type,f.BrandID
from Style_BOA sb WITH (NOLOCK) 
left join Style_ColorCombo sc WITH (NOLOCK) on sc.StyleUkey = sb.StyleUkey and sc.FabricPanelCode = sb.PatternPanel and sc.Article = @article
left join Fabric f WITH (NOLOCK) on sb.SCIRefno = f.SCIRefno
where sb.StyleUkey = @styleukey
and sb.IsCustCD <> 2
and (sb.SuppIDBulk <> 'FTY' and sb.SuppIDBulk <> 'FTY-C')
),
tmpBOAPrepareData
as (
select t.UsageUnit,t.SCIRefno,t.Refno,t.BrandID,t.NLCode,t.CustomsUnit,t.Type,
isnull((select Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = t.UsageUnit and UnitTo = t.CustomsUnit),'') as UnitRate,
isnull((select Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = t.UsageUnit and UnitTo = 'M'),'') as M2UnitRate
from tmpBOA t
where (t.BomTypeArticle = 0 and t.BomTypeColor = 0) or ((t.BomTypeArticle = 1 or t.BomTypeColor = 1) and t.ColorID is not null)
),
tmpLocalPO
as (
select ld.Refno,ld.Qty,ld.UnitId,li.MeterToCone,li.NLCode,li.HSCode,li.CustomsUnit,li.PcsWidth,li.PcsLength,li.PcsKg,o.Qty as OrderQty,isnull(vd.Waste,0) as Waste
from LocalPO_Detail ld WITH (NOLOCK) 
left join LocalItem li WITH (NOLOCK) on li.RefNo = ld.Refno
left join Orders o WITH (NOLOCK) on ld.OrderId = o.ID
left join VNContract_Detail vd WITH (NOLOCK) on vd.ID = @vncontractid and vd.NLCode = li.NLCode
where ld.OrderId = (select TOP 1 ID from Orders WITH (NOLOCK) where StyleUkey = @styleukey and Category = @category order by BuyerDelivery,ID)
and li.NoDeclare = 0
),
tmpConeToM
as (
select Refno,IIF(UnitId = 'CONE',Qty*MeterToCone,Qty) as Qty,OrderQty, IIF(UnitId = 'CONE','M',UnitId) as UnitId,Waste,
NLCode,HSCode,CustomsUnit,PcsWidth,PcsLength,PcsKg
from tmpLocalPO),
tmpPrepareRate
as (
select UnitId as UsageUnit,Refno as SCIRefno,Refno,'' as BrandID,NLCode,CustomsUnit,'' as Type,
isnull((select Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = UnitId and UnitTo = CustomsUnit),'') as UnitRate,
isnull((select Rate from Unit_Rate WITH (NOLOCK) where UnitFrom = UnitId and UnitTo = 'M'),'') as M2UnitRate
from tmpConeToM),
tmpfinal
as (
select * from tmpBOFRateData
union
select * from tmpBOAPrepareData
union
select * from tmpPrepareRate),
tmpEmptyNLCode
as (
select '1' as DataType,* from tmpfinal where NLCode = ''
),
tmpUnitNotFound
as (
select distinct '2' as DataType,UsageUnit,SCIRefno,RefNo,'' as BrandID, '' as NLCode,'' as CustomsUnit,IIF(Type = 'F',Type,'') as Type,
UnitRate,M2UnitRate
from tmpfinal 
where UsageUnit <> CustomsUnit 
AND NOT (UsageUnit = 'PCS' and (CustomsUnit = 'KGS' OR CustomsUnit = 'M2' OR CustomsUnit = 'M'))
)
select * from tmpEmptyNLCode
union all
select * from tmpUnitNotFound
order by DataType,SCIRefno,UsageUnit", MyUtility.Convert.GetString(CurrentMaintain["StyleUKey"]), MyUtility.Convert.GetString(CurrentMaintain["SizeCode"]), colorway[0], MyUtility.Convert.GetString(CurrentMaintain["Category"]), MyUtility.Convert.GetString(CurrentMaintain["VNContractID"])));
            #endregion
            result = DBProxy.Current.Select(null, sqlCmd.ToString(), out invalidData);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query invalid data fail!!\r\n" + result.ToString());
                return;
            }
            #region 組出NL Code為空或單位無法轉換的訊息
            foreach (DataRow dr in invalidData.Rows)
            {
                if (MyUtility.Convert.GetString(dr["DataType"]) == "1")
                {
                    emptyNLCode.Append(string.Format("RefNo: {0}, Brand: {1}\r\n", MyUtility.Convert.GetString(dr["RefNo"]), MyUtility.Convert.GetString(dr["BrandID"])));
                }
                else
                {
                    if ((MyUtility.Convert.GetString(dr["Type"]) == "F" && MyUtility.Convert.GetString(dr["CustomsUnit"]).ToUpper() == "M2" && MyUtility.Check.Empty(dr["M2UnitRate"])) ||
                        (MyUtility.Convert.GetString(dr["Type"]) == "F" && MyUtility.Convert.GetString(dr["CustomsUnit"]).ToUpper() != "M2" && MyUtility.Check.Empty(dr["UnitRate"])) ||
                        (MyUtility.Convert.GetString(dr["Type"]) != "F" && MyUtility.Convert.GetString(dr["CustomsUnit"]).ToUpper() == "M2" && MyUtility.Check.Empty(dr["M2UnitRate"])) ||
                        (MyUtility.Convert.GetString(dr["Type"]) != "F" && MyUtility.Convert.GetString(dr["CustomsUnit"]).ToUpper() == "M" && MyUtility.Check.Empty(dr["UnitRate"])) ||
                        (MyUtility.Convert.GetString(dr["Type"]) != "F" && MyUtility.Convert.GetString(dr["CustomsUnit"]).ToUpper() != "M2" && MyUtility.Convert.GetString(dr["CustomsUnit"]).ToUpper() != "M" && MyUtility.Check.Empty(dr["UnitRate"])))
                    {
                        wrongUnit.Append(string.Format("Customs Code:{0}  RefNo:{1}   Unit:{2} transfer to Unit:{3}\r\n", MyUtility.Convert.GetString(dr["NLCode"]), MyUtility.Convert.GetString(dr["RefNo"]), MyUtility.Convert.GetString(dr["UsageUnit"]), MyUtility.Convert.GetString(dr["CustomsUnit"])));
                    }
                }
            }
            #endregion

            #region 刪除VNConsumption_Detail與VNConsumption_Detail_Detail資料
            foreach (DataRow dr in DetailDatas)
            {
                DataRow[] queryData = queryDetailData.Select(string.Format("NLCode = '{0}'", MyUtility.Convert.GetString(dr["NLCode"])));
                if (queryData.Length <= 0)
                {
                    dr.Delete();
                }
            }
            foreach (DataRow dr in VNConsumption_Detail_Detail.ToList())
            {
                 DataRow[] queryData =  queryDetail2Data.Select(string.Format("NLCode = '{0}' and SCIRefNo = '{1}'", MyUtility.Convert.GetString(dr["NLCode"]), MyUtility.Convert.GetString(dr["SCIRefNo"])));
                 if (queryData.Length <= 0)
                 {
                     dr.Delete();
                 }
            }
            #endregion

            #region 塞資料進VNConsumption_Detail與VNConsumption_Detail_Detail
            foreach (DataRow dr in queryDetailData.Rows)
            {
                DataRow[] queryData = ((DataTable)detailgridbs.DataSource).Select(string.Format("NLCode = '{0}'", MyUtility.Convert.GetString(dr["NLCode"])));
                if (queryData.Length <= 0)
                {
                    DataRow newRow = ((DataTable)detailgridbs.DataSource).NewRow();
                    newRow["NLCode"] = dr["NLCode"];
                    newRow["HSCode"] = dr["HSCode"];
                    newRow["UnitID"] = dr["CustomsUnit"];
                    newRow["Qty"] = dr["Qty"];
                    newRow["Waste"] = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup(string.Format("select isnull(Waste,0) from VNContract_Detail WITH (NOLOCK) where ID = '{0}' and NLCode = '{1}'", MyUtility.Convert.GetString(CurrentMaintain["VNContractID"]), MyUtility.Convert.GetString(dr["NLCode"]))));
                    newRow["UserCreate"] = 0;
                    ((DataTable)detailgridbs.DataSource).Rows.Add(newRow);
                }
                else
                {
                    queryData[0]["HSCode"] = dr["HSCode"];
                    queryData[0]["UnitID"] = dr["CustomsUnit"];
                    queryData[0]["Qty"] = dr["Qty"];
                    queryData[0]["UserCreate"] = 0;
                }
            }

            foreach (DataRow dr in queryDetail2Data.Rows)
            {
                DataRow[] queryData = VNConsumption_Detail_Detail.Select(string.Format("NLCode = '{0}' and SCIRefNo = '{1}'", MyUtility.Convert.GetString(dr["NLCode"]), MyUtility.Convert.GetString(dr["SCIRefNo"])));
                if (queryData.Length <= 0)
                {
                    DataRow newRow = VNConsumption_Detail_Detail.NewRow();
                    newRow["NLCode"] = dr["NLCode"];
                    newRow["SCIRefno"] = dr["SCIRefno"];
                    newRow["RefNo"] = dr["RefNo"];
                    newRow["Qty"] = MyUtility.Check.Empty(dr["Qty"]) ? 0 : MyUtility.Convert.GetDecimal(dr["Qty"]);
                    newRow["LocalItem"] = dr["LocalItem"];
                    VNConsumption_Detail_Detail.Rows.Add(newRow);
                }
                else
                {
                    queryData[0]["RefNo"] = dr["RefNo"];
                    queryData[0]["Qty"] = MyUtility.Check.Empty(dr["Qty"])?0:MyUtility.Convert.GetDecimal(dr["Qty"]);
                    queryData[0]["LocalItem"] = dr["LocalItem"];
                }
            }
            #endregion

            #region 組要顯示的訊息
            if (!MyUtility.Check.Empty(emptyNLCode.ToString()))
            {
                allMessage.Append(string.Format("Below data is no Customs Code in B40, B41:\r\n{0}\r\n", emptyNLCode.ToString()));
            }
            if (!MyUtility.Check.Empty(wrongUnit.ToString()))
            {
                allMessage.Append(string.Format("Below data is no transfer formula. Please contact with Taipei MIS.\r\n{0}\r\n", wrongUnit.ToString()));
            }
            if (!MyUtility.Check.Empty(needItem.ToString()))
            {
                allMessage.Append(string.Format("Below data is lacking item. Please pay attention!!\r\n{0}\r\n", needItem.ToString()));
            }
            #endregion

            if (MyUtility.Check.Empty(allMessage.ToString()))
            {
                MyUtility.Msg.InfoBox("Calculate complete!!");
            }
            else
            {
                MyUtility.Msg.WarningBox(allMessage.ToString());
            }
        }

        //Confirm
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            //檢查表身Grid資料是否有缺一必輸的NLCode資料，若有，就出訊息告知使用者
            string sqlCmd = string.Format(@"select NLCode from VNContract_Detail WITH (NOLOCK) where ID = '{0}' and NecessaryItem = 1
except
select NLCode from VNConsumption_Detail WITH (NOLOCK) where ID = '{1}'", MyUtility.Convert.GetString(CurrentMaintain["VNContractID"]), MyUtility.Convert.GetString(CurrentMaintain["ID"]));
            DataTable LackData;
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out LackData);
            if (LackData.Rows.Count > 0)
            {
                StringBuilder lackNLCode = new StringBuilder();
                foreach (DataRow dr in LackData.Rows)
                {
                    lackNLCode.Append(MyUtility.Convert.GetString(dr["NLCode"]) + ",");
                }
                MyUtility.Msg.WarningBox(string.Format("Lacking regular Customs Code: {0}. Please double check.", lackNLCode.ToString(0, lackNLCode.ToString().Length - 1)));
            }

            string updateCmds = string.Format("update VNConsumption set EditDate = GETDATE(), EditName = '{0}', Status = 'Confirmed' where ID = '{1}'",
                Sci.Env.User.UserID, MyUtility.Convert.GetString(CurrentMaintain["ID"]));

            result = DBProxy.Current.Execute(null, updateCmds);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Confirm fail!!\r\n" + result.ToString());
                return;
            }
           
        }

        //Unconfirm
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();

            string updateCmds = string.Format("update VNConsumption set EditDate = GETDATE(), EditName = '{0}', Status = 'New' where ID = '{1}'",
                            Sci.Env.User.UserID, MyUtility.Convert.GetString(CurrentMaintain["ID"]));

            DualResult result = DBProxy.Current.Execute(null, updateCmds);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Unconfirm fail!!\r\n" + result.ToString());
                return;
            }

        }
    }
}
