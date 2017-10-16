using Ict;
using Ict.Win;
using Sci.Production.Class;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci.Data;
using System.Transactions;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using Ict.Win.UI;

namespace Sci.Production.Thread
{
    public partial class P02 : Win.Tems.Input8
    {
        private string factory = Sci.Env.User.Factory;
        private string loginID = Sci.Env.User.UserID;
        private string keyWord = Sci.Env.User.Keyword;
        Ict.Win.UI.DataGridViewTextBoxColumn col_refno;
        Ict.Win.UI.DataGridViewTextBoxColumn col_color;
        DataGridViewNumericBoxColumn col_cons;
        DataGridViewNumericBoxColumn col_Allowance;
        DataGridViewNumericBoxColumn col_NewCone;
        DataGridViewNumericBoxColumn col_UsedCone;
        public P02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            string defaultfilter = string.Format("MDivisionid = '{0}' ", keyWord);
            this.DefaultFilter = defaultfilter;
            InitializeComponent();
            DoSubForm = new P02_Detail();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataTable queryDT;
            string querySql = string.Format(@"
select '' FTYGroup

union 
select distinct FTYGroup 
from Factory 
where MDivisionID = '{0}'", Sci.Env.User.Keyword);
            DBProxy.Current.Select(null, querySql, out queryDT);
            MyUtility.Tool.SetupCombox(queryfors, 1, queryDT);
            queryfors.SelectedIndex = 0;
            queryfors.SelectedIndexChanged += (s, e) =>
            {
                switch (queryfors.SelectedIndex)
                {
                    case 0:
                        this.DefaultWhere = "";
                        break;
                    default:
                        this.DefaultWhere = string.Format("FactoryID = '{0}'", queryfors.SelectedValue);
                        break;
                }
                this.ReloadDatas();
            };
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            this.label7.Text = CurrentMaintain["Status"].ToString();

            DataRow dr;
            if (MyUtility.Check.Seek(string.Format("Select * from orders WITH (NOLOCK) where id='{0}'", CurrentMaintain["OrderID"].ToString()), out dr))
            {
                if (!MyUtility.Check.Empty(dr["SciDelivery"]))
                dateSCIDelivery.Value = Convert.ToDateTime(dr["SciDelivery"]);
                else dateSCIDelivery.Text = "";
                if (!MyUtility.Check.Empty(dr["SewInLine"]))
                dateSewingInLine.Value = Convert.ToDateTime(dr["SewInLine"]);
                else dateSewingInLine.Text = "";
            }
            else
            {
                dateSCIDelivery.Text = "";
                dateSewingInLine.Text = "";

            }
        }

        protected override DualResult OnDetailSelectCommandPrepare(Win.Tems.InputMasterDetail.PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["OrderID"].ToString();
            this.DetailSelectCommand = string.Format(@"
SELECT  a.*
        , b.description
        , b.MetertoCone 
        , c.description as colordesc
        , X.newCone
        , X.usedcone
        , Article = stuff((select concat (',', Article)
                           from (
						        select distinct Article 
                                from ThreadRequisition_Detail_Cons trdc 
                                where a.Ukey = trdc.ThreadRequisition_DetailUkey
						   ) art
                           for xml path('')
                          ), 1, 1, '')
FROM ThreadRequisition_Detail a WITH (NOLOCK) 
Left Join Localitem b WITH (NOLOCK) on a.refno = b.refno
Left join ThreadColor c WITH (NOLOCK) on c.id = a.ThreadColorid
OUTER APPLY
(
	select isnull(sum(d.newCone),0) as newCone,isnull(sum(usedcone),0) as usedcone from ThreadStock d WITH (NOLOCK) 
	where d.refno = a.refno and d.Threadcolorid = a.threadcolorid and d.mDivisionid = '{1}'		
) X
WHERE a.OrderID = '{0}'"
                , masterID,keyWord);

            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override DualResult OnSubDetailSelectCommandPrepare(Win.Tems.Input8.PrepareSubDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Detail == null) ? "" : e.Detail["Ukey"].ToString();

            this.SubDetailSelectCommand = string.Format(
@"Select a.*, b.description as Threadcombdesc,c.name as ThreadLocation, isnull(seamlength * UseRatioNumeric + isnull(a.Allowance,0),0) as uselength,
OrderQty,isnull((seamlength * UseRatioNumeric+ isnull(a.Allowance,0)) * OrderQty,0) as TotalLength
from ThreadRequisition_Detail_Cons a WITH (NOLOCK) 
Left join ThreadComb b WITH (NOLOCK) on b.id = a.threadcombid
Left join Reason c WITH (NOLOCK) on c.reasontypeid = 'ThreadLocation' and c.id = a.threadlocationid
where a.ThreadRequisition_DetailUkey = '{0}'", masterID);
            return base.OnSubDetailSelectCommandPrepare(e);
        }

        celllocalitem refno = (celllocalitem)celllocalitem.GetGridCell("Thread", null, ",,,description");

        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            
            DataGridViewGeneratorTextColumnSettings thcolor = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorNumericColumnSettings cons = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings NewCone = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings UsedCone = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings poqty1 = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings poqty2 = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings poqty3 = new DataGridViewGeneratorNumericColumnSettings();          
            #region Refno Cell     
             refno.CellValidating += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                if (CurrentDetailData == null) return;
                string oldvalue = CurrentDetailData["refno"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (!this.EditMode || oldvalue == newvalue) return;
                if (CurrentDetailData["autoCreate"].ToString() == "True") return;
                DataRow refdr;
                if (MyUtility.Check.Seek(string.Format("Select * from Localitem WITH (NOLOCK) where refno = '{0}' and junk = 0", newvalue), out refdr))
                {
                    CurrentDetailData["Description"] = refdr["Description"];
                    CurrentDetailData["Refno"] = refdr["refno"];
                    CurrentDetailData["MeterToCone"] = refdr["MeterToCone"];          
                }
                else 
                {
                    CurrentDetailData["Description"] = "";
                    CurrentDetailData["Refno"] = "";
                    CurrentDetailData["MeterToCone"] = 0;  
                }
                string sql = string.Format("Select isnull(sum(newCone),0) as newCone,isnull(sum(usedCone),0) as usedCone from ThreadStock WITH (NOLOCK) where refno ='{0}' and threadcolorid = '{1}' and mDivisionid ='{2}' ", newvalue, CurrentDetailData["ThreadColorid"].ToString(), keyWord);
                if (MyUtility.Check.Seek(sql, out refdr))
                {
                    CurrentDetailData["NewCone"] = refdr["NewCone"];
                    CurrentDetailData["UsedCone"] = refdr["UsedCone"];
                }
                else
                {
                    CurrentDetailData["NewCone"] = 0;
                    CurrentDetailData["UsedCone"] = 0;
                }
                ReQty(CurrentDetailData);
                CurrentDetailData.EndEdit();
                this.update_detailgrid_CellValidated(e.RowIndex);
            };
            #endregion
            #region Color Cell
            thcolor.CellValidating += (s, e) =>
            {
                string oldvalue = CurrentDetailData["threadcolorid"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (!this.EditMode || oldvalue == newvalue) return;
                if (CurrentDetailData["autoCreate"].ToString() == "True") return;
                DataRow refdr;
                if (MyUtility.Check.Seek(string.Format("Select * from ThreadColor WITH (NOLOCK) where id = '{0}' and junk = 0", newvalue), out refdr))
                {
                    CurrentDetailData["ThreadColorid"] = refdr["ID"];
                    CurrentDetailData["Colordesc"] = refdr["Description"];
                }
                else
                {
                    CurrentDetailData["ThreadColorid"] = "";
                    CurrentDetailData["Colordesc"] = "";
                    MyUtility.Msg.WarningBox(string.Format("< Thread Color: {0}> not found.", e.FormattedValue.ToString()));
                    return;                   
                }
                string sql = string.Format("Select isnull(sum(newCone),0) as newCone,isnull(sum(usedCone),0) as usedCone from ThreadStock WITH (NOLOCK) where refno ='{0}' and threadcolorid = '{1}' and mDivisionid ='{2}' ", CurrentDetailData["Refno"].ToString(), newvalue, keyWord);
                if (MyUtility.Check.Seek(sql, out refdr))
                {
                    CurrentDetailData["NewCone"] = refdr["NewCone"];
                    CurrentDetailData["UsedCone"] = refdr["UsedCone"];
                }
                else
                {
                    CurrentDetailData["NewCone"] = 0;
                    CurrentDetailData["UsedCone"] = 0;
                }
               // ReQty(CurrentDetailData);
                CurrentDetailData.EndEdit();
                this.update_detailgrid_CellValidated(e.RowIndex);
            };
            thcolor.EditingMouseDown = (s, e) =>
            {
                if (EditMode)
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            if (CurrentDetailData["autoCreate"].ToString() == "True") return;
                            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("select ID,Description from ThreadColor WITH (NOLOCK) where junk = 0 order by ID", "10,40", CurrentDetailData["ThreadColorid"].ToString().Trim());
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel) { return; }                           
                            var sellist = item.GetSelecteds();
                            CurrentDetailData["Colordesc"] = sellist[0][1];
                            e.EditingControl.Text = item.GetSelectedString();
                        }
                    }
                }
            };
            #endregion
            #region cons Cell
            cons.CellValidating += (s, e) =>
            {
                decimal oldvalue = (decimal)CurrentDetailData["ConsumptionQty"];
                decimal newvalue = (decimal)e.FormattedValue;
                if (!this.EditMode || oldvalue == newvalue) return;
                CurrentDetailData["ConsumptionQty"] = newvalue;
                ReQty(CurrentDetailData);
                CurrentDetailData.EndEdit();
            };
            cons.CellMouseDoubleClick += (s, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    OpenSubDetailPage();
                }
            };

            #endregion
            #region useStock CellValidating by (NewCone)+UsedCone
            
            NewCone.CellValidating += (s, e) =>
            {
                /*int nc = Convert.ToInt32(e.FormattedValue);
                int uc = Convert.ToInt32(CurrentDetailData["UsedCone"]);
                if (!this.EditMode) return;
                CurrentDetailData["NewCone"] = nc;
                CurrentDetailData["UseStockQty"] = nc + uc;
                CurrentDetailData.EndEdit();*/
            };
            #endregion
            #region useStock CellValidating by NewCone+(UsedCone)
           
            UsedCone.CellValidating += (s, e) =>
            {
                /*int nc = Convert.ToInt32(CurrentDetailData["NewCone"]);
                int uc = Convert.ToInt32(e.FormattedValue);
                if (!this.EditMode) return;
                CurrentDetailData["UsedCone"] = uc;
                CurrentDetailData["UseStockQty"] = nc + uc;
                CurrentDetailData.EndEdit();*/
            };
            #endregion            
            #region poqty CellValidating by (TotalQty) + AllowanceQty - UseStockQty
            poqty1.CellValidating += (s, e) =>
            {
                /*int tq = Convert.ToInt32(CurrentDetailData["TotalQty"]);
                int aq = Convert.ToInt32(CurrentDetailData["AllowanceQty"]);
                int usq = Convert.ToInt32(CurrentDetailData["UseStockQty"]);
                if (!this.EditMode) return;
                CurrentDetailData["PurchaseQty"] = tq + aq - usq;
                CurrentDetailData.EndEdit();*/
            };
            #endregion
            #region poqty CellValidating by TotalQty + (AllowanceQty) - UseStockQty
           
            #endregion
            #region poqty CellValidating by TotalQty + AllowanceQty - (UseStockQty)
            poqty3.EditingValueChanged += (s, e) =>
            {
                /*int tq = Convert.ToInt32(CurrentDetailData["TotalQty"]);
                int aq = Convert.ToInt32(CurrentDetailData["AllowanceQty"]);
                int usq = Convert.ToInt32(CurrentDetailData["UseStockQty"]);
                if (!this.EditMode) return;
                CurrentDetailData["PurchaseQty"] = tq + aq - usq;
                CurrentDetailData.EndEdit();*/
            };
            #endregion           
            #region set grid
            Helper.Controls.Grid.Generator(this.detailgrid)
           .Text("Refno", header: "Thread Refno", width: Ict.Win.Widths.AnsiChars(10), settings: refno).Get(out col_refno)
           .Text("description", header: "Thread Desc", width: Ict.Win.Widths.AnsiChars(18), iseditingreadonly: true)
           .EditText("Article", header: "Article", width: Ict.Win.Widths.AnsiChars(12), iseditingreadonly: true)
           .Text("ThreadColorid", header: "Thread\r\nColor", width: Ict.Win.Widths.AnsiChars(4), settings: thcolor).Get(out col_color)
           .Text("Colordesc", header: "Thread Color Desc", width: Ict.Win.Widths.AnsiChars(18), iseditingreadonly: true)
           .Numeric("ConsumptionQty", header: "Total\r\nCons.(M)", width: Ict.Win.Widths.AnsiChars(2), integer_places: 6, settings: cons).Get(out col_cons)
           .Numeric("MeterToCone", header: "No. of Meters\r\nPer Cones", width: Ict.Win.Widths.AnsiChars(6), integer_places: 7, decimal_places: 1, iseditingreadonly: true)
           .Numeric("TotalQty", header: "No. of\r\nCones", width: Ict.Win.Widths.AnsiChars(2), integer_places: 6, iseditingreadonly: true, settings: poqty1)
           .Numeric("AllowanceQty", header: "20%\r\nallowance", width: Ict.Win.Widths.AnsiChars(2), integer_places: 6, settings: poqty2).Get(out col_Allowance)
           .Numeric("NewCone", header: "New\r\nCone", width: Ict.Win.Widths.AnsiChars(2), integer_places: 6, settings: NewCone).Get(out this.col_NewCone)
           .Numeric("UsedCone", header: "Use\r\nCone", width: Ict.Win.Widths.AnsiChars(2), integer_places: 6, settings: UsedCone).Get(out this.col_UsedCone)
           .Numeric("UseStockQty", header: "Use\r\nStock", width: Ict.Win.Widths.AnsiChars(2), integer_places: 6, iseditingreadonly: true, settings: poqty3)
           .Numeric("PurchaseQty", header: "PO Qty", width: Ict.Win.Widths.AnsiChars(2), integer_places: 6, iseditingreadonly: true)
           .Text("Remark", header: "Remark", width: Ict.Win.Widths.AnsiChars(10))
           .Text("POID", header: "PO ID", width: Ict.Win.Widths.AnsiChars(12), iseditingreadonly: true);

            #endregion

            this.detailgrid.Columns["Remark"].DefaultCellStyle.BackColor = Color.Pink;

            //設定detailGrid Rows 是否可以編輯
            this.detailgrid.RowEnter += detailgrid_RowEnter;           
            change_record();            
            this.detailgrid.CellValidated += detailgrid_CellValidated;
            
        }

        private void detailgrid_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || EditMode == false) { return; }
            var data = ((DataRowView)this.detailgrid.Rows[e.RowIndex].DataBoundItem).Row;
            if (data == null) { return; }
            //AutoCrate為1表示資料來源為(P01計算出來的就不能再讓使用者修改線種,顏色,總長度)
            if (data["autoCreate"].ToString() == "True")
            {
                col_color.IsEditingReadOnly = true;
                col_cons.IsEditingReadOnly = true;
                this.refno.SupportPopup = false;
            }
            else
            {
                col_color.IsEditingReadOnly = false;
                col_cons.IsEditingReadOnly = false;
                this.refno.SupportPopup = true;
            }
            //POID應為全鎖，因為 POID表示在SubconP30已有建立採購單並Confrim，
            //不能再給使用者修改請購單的所有項目。
            if (!MyUtility.Check.Empty(data["POID"]))
            {
                col_Allowance.IsEditingReadOnly = true;
                col_NewCone.IsEditingReadOnly = true;
                col_UsedCone.IsEditingReadOnly = true;
                col_refno.IsEditingReadOnly = true;
                col_color.IsEditingReadOnly = true;
                col_cons.IsEditingReadOnly = true;
            }
            else
            {
                col_refno.IsEditingReadOnly = false;
                col_Allowance.IsEditingReadOnly = false;
                col_NewCone.IsEditingReadOnly = false;
                col_UsedCone.IsEditingReadOnly = false;
            }
            
        }
        #region 是否可編輯與變色
        private void change_record()
        {
            col_color.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (dr["autoCreate"].ToString() == "True"|| !MyUtility.Check.Empty(dr["POID"]))
                {
                    e.CellStyle.BackColor = Color.White;
                    e.CellStyle.ForeColor = Color.Black;

                }
                else
                {
                    e.CellStyle.BackColor = Color.Pink;
                }
            };
            col_cons.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (dr["autoCreate"].ToString() == "True"|| !MyUtility.Check.Empty(dr["POID"]))
                {
                    e.CellStyle.BackColor = Color.White;
                    e.CellStyle.ForeColor = Color.Black;

                }
                else
                {
                    e.CellStyle.BackColor = Color.Pink;
                }
            };
            col_refno.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (!MyUtility.Check.Empty(dr["POID"]))
                {
                    e.CellStyle.BackColor = Color.White;
                    e.CellStyle.ForeColor = Color.Black;
                }
                else
                {
                    e.CellStyle.BackColor = Color.Pink;

                }
            };
            col_Allowance.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (!MyUtility.Check.Empty(dr["POID"]))
                {
                    e.CellStyle.BackColor = Color.White;
                    e.CellStyle.ForeColor = Color.Black;
                }
                else
                {
                    e.CellStyle.BackColor = Color.Pink;
                }
            };
            col_NewCone.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (!MyUtility.Check.Empty(dr["POID"]))
                {
                    e.CellStyle.BackColor = Color.White;
                    e.CellStyle.ForeColor = Color.Black;
                }
                else
                {
                    e.CellStyle.BackColor = Color.Pink;
                }
            };
            col_UsedCone.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (!MyUtility.Check.Empty(dr["POID"]))
                {
                    e.CellStyle.BackColor = Color.White;
                    e.CellStyle.ForeColor = Color.Black;
                }
                else
                {
                    e.CellStyle.BackColor = Color.Pink;
                }
            };
        }
        #endregion

        void detailgrid_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (!this.EditMode 
                || ( e.ColumnIndex != this.col_Allowance.Index
                    && e.ColumnIndex != this.col_NewCone.Index
                    && e.ColumnIndex != this.col_UsedCone.Index
                    && e.ColumnIndex != this.col_cons.Index)
                )return;
            this.update_detailgrid_CellValidated(e.RowIndex);
        }

        void update_detailgrid_CellValidated(int RowIndex) {
            int nc = Convert.ToInt32(CurrentDetailData["NewCone"]);
            int uc = Convert.ToInt32(CurrentDetailData["UsedCone"]);
            int usq = nc + uc;
            CurrentDetailData["UseStockQty"] = usq;

            int tq = Convert.ToInt32(CurrentDetailData["TotalQty"]);
            int aq = Convert.ToInt32(CurrentDetailData["AllowanceQty"]);
            int temp =tq + aq - usq;
            if (temp < 0) temp = 0;
            CurrentDetailData["PurchaseQty"] = temp;
            this.detailgrid.InvalidateRow(RowIndex);
        }

        protected override bool ClickDeleteBefore()
        {
            if (CurrentMaintain["Status"].ToString() != "New")
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't be deleted.", "Warning");
                return false;
            }
            return base.ClickDeleteBefore();
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["Status"] = "New";
            CurrentMaintain["Factoryid"] = factory;
            CurrentMaintain["mDivisionid"] = keyWord;
        }

        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(CurrentMaintain["estbookDate"]))
            {                
                dateEstBooking.Focus();
                MyUtility.Msg.WarningBox("<Est. Booking> can not be empty.");
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["EstArriveDate"]))
            {
                dateEstArrived.Focus();
                 MyUtility.Msg.WarningBox("<Est. Arrived> can not be empty.");
                return false;
            }
            //foreach (DataRow dr in DetailDatas)
            //{
            //    if (MyUtility.Check.Empty(dr["PurchaseQty"]))
            //    {
            //        MyUtility.Msg.WarningBox("<PO Qty> can not be 0");
            //        return false;
            //    }
            //}
            return base.ClickSaveBefore();
        }

        protected override bool ClickEditBefore()
        {
            if (CurrentMaintain["Status"].ToString() != "New")
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't be modify", "Warning");
                return false;
            }

            return base.ClickEditBefore();
        }

        private void txtSP_Validating(object sender, CancelEventArgs e)
        {
            if (txtSP.OldValue == txtSP.Text) return;
            string id = txtSP.Text;
            DataRow drOrder;
            DataTable pretb_cons, TR_DUK;
            string sqlpre;
            string sqltr_duk; 
            DataTable detailtb = (DataTable)detailgridbs.DataSource;

            foreach (Control item in masterpanel.Controls)
            {
                if (item is Sci.Win.UI.Label || item is Sci.Win.UI.Button || item == displayM || item == txtSP) { }
                else
                {
                    item.Text = "";                
                }
            }
            detailtb.Clear();

            if (txtSP.Text == "") return;
            ////確認order.poid 同(po.id)有沒有這筆,沒有則return
            //if (!MyUtility.Check.Seek(string.Format("Select * from PO WITH (NOLOCK) where id='{0}'", id)))
            //{              
            //    e.Cancel = true;
            //    txtSP.Text = "";
            //    MyUtility.Msg.WarningBox(string.Format("<SP#: {0} >does not exists in Purchase Order!!!", id));
            //    return;
            //}
            //確認orders.id + 工廠有沒有這筆,沒有則return
            if (!MyUtility.Check.Seek(string.Format("Select * from orders WITH (NOLOCK) where id='{0}' and FtyGroup = '{1}'", id, factory)))
            {
                e.Cancel = true;
                txtSP.Text = "";
                MyUtility.Msg.WarningBox(string.Format("<SP#: {0} >Data not found!!!!", id));
                return;
            }
            //確認ThreadRequisition有沒有這筆,有則return
            if (MyUtility.Check.Seek(string.Format("Select * from ThreadRequisition WITH (NOLOCK) where OrderID='{0}'", id)))
            {
                e.Cancel = true;
                txtSP.Text = "";
                MyUtility.Msg.WarningBox("Order number exists already.");
                return;
            }

            //輸入的POno帶出其他6個表頭
            if (!MyUtility.Check.Seek(string.Format("Select * from Orders WITH (NOLOCK) where poid='{0}'", id), out drOrder)) return;
            dateSCIDelivery.Value = MyUtility.Convert.GetDate(drOrder["SciDelivery"]);
            dateSewingInLine.Value = MyUtility.Convert.GetDate(drOrder["SewInLine"]);
            CurrentMaintain["Styleid"] = drOrder["Styleid"].ToString();
            CurrentMaintain["Seasonid"] = drOrder["Seasonid"].ToString();
            CurrentMaintain["Brandid"] = drOrder["Brandid"].ToString();
            CurrentMaintain["factoryid"] = drOrder["FtyGroup"].ToString();
            CurrentMaintain["OrderID"] = id;
            //事先整理資料
            sqlpre = string.Format(@"
Select  a.ThreadColorId
        , d.Allowance
        , a.Article
        , a.ThreadCombId
        , b.OperationId
        , SeamLength = e.SeamLength * b.Frequency
        , a.Seq
        , a.ThreadLocationId
        , a.Refno
        , d.UseRatioNumeric
        , a.MachineTypeId
        , isnull(f.OrderQty,0) as OrderQty 
        , isnull(g.MeterToCone,0.00) as MeterToCone
        , g.Description as Threadcombdesc
        , h.Description as colordesc
from ThreadColorComb_Detail a WITH (NOLOCK) 
cross join ThreadColorComb_Operation b WITH (NOLOCK) 
left join ThreadColorcomb c WITH (NOLOCK) on a.id=c.id 
left join MachineType_ThreadRatio d WITH (NOLOCK) on a.Machinetypeid=d.Id and a.seq=d.seq
left join Operation e WITH (NOLOCK) on b.OperationId= e.Id
Left join (
    Select  a.Article,sum(a.qty) as OrderQty 
    from Order_Qty a WITH (NOLOCK) 
    inner join orders b WITH (NOLOCK) on a.id = b.id  
    where b.POID = '{0}' and b.Category!='G'
    group by Article
) f on a.Article = f.Article
Left join LocalItem g WITH (NOLOCK) on a.Refno = g.Refno
Left join threadcolor h WITH (NOLOCK) on h.id = a.threadcolorid                                    
where   c.Styleukey = (select o.Styleukey 
                       from Orders o WITH (NOLOCK) 
                       where o.id = '{0}')
        and a.ThreadColorId is not null 
        and a.ThreadColorId !=''
        and a.Refno is not null 
        and a.Refno != '' 
        and Seamlength != 0
        and a.id = b.id
        and f.OrderQty != '0'", id);

            DualResult result;
            result = DBProxy.Current.Select(null, sqlpre, out pretb_cons);
            if (!result) { this.ShowErr(result); return; }

            StringBuilder warningmsg = new StringBuilder();
            foreach (DataRow temp in pretb_cons.Rows)
            {
                if (Convert.ToDecimal(temp["MeterToCone"].ToString()) == 0)
                {
                    warningmsg.Append(string.Format("Thread Refno:{0} <No. of Meters Per Cones> is 0 can't calculate <No. of Cones>", temp["Refno"].ToString()) + Environment.NewLine);
                }
            }
            if (warningmsg.ToString() != "") MyUtility.Msg.InfoBox(warningmsg.ToString());
           

            //做資料匯整select group 後填入ThreadRequisition_Detail
            sqltr_duk = string.Format(@"
select  Orderid = '{0}'
        , #tmp.Refno
        , ThreadColorId
        , Threadcombdesc
        , colordesc
        , #tmp.MeterToCone
        , ConsumptionQty = CEILING(Sum(OrderQty * (Seamlength * UseRatioNumeric + Allowance)) / 100)
        , TotalQty = IIF(#tmp.MeterToCone > 0, CEILING (Sum (OrderQty * (Seamlength * UseRatioNumeric + Allowance)) / 100 / #tmp.MeterToCone)
                                              , 0)
        , AllowanceQty =  0.00
        , PurchaseQty = IIF(#tmp.MeterToCone > 0, CEILING (Sum (OrderQty * (Seamlength * UseRatioNumeric + Allowance)) / 100 / #tmp.MeterToCone)
                                                , 0.00) 
        , AutoCreate = 'true' 
        , UseStockQty = 0
        , POID = ''
        , Remark = ''
        , Article = stuff((select concat (',', Article)
                           from (
						       select distinct Article 
                               from #tmp t
                               where  t.refno = #tmp.refno 
                                   and t.threadColorid = #tmp.threadColorid
						   ) art
                           for xml path('')
                          ), 1, 1, '')
from #tmp
group by Threadcombdesc,colordesc,#tmp.Refno,#tmp.MeterToCone,ThreadColorId
                        ", id);
            
            if (pretb_cons.Rows.Count <= 0) TR_DUK = pretb_cons.Clone();
            else
            {
                result= MyUtility.Tool.ProcessWithDatatable(pretb_cons, "", sqltr_duk, out TR_DUK, "#tmp");//TR_DUK為表身
                if (!result) { this.ShowErr(result); return; }
            }
           
            
            foreach (DataRow dr in TR_DUK.Rows) //新增表身
            {
                DataRow newdr = detailtb.NewRow();
                newdr["OrderID"] = id;
                newdr["Refno"] = dr["Refno"];
                newdr["ThreadColorid"] = dr["ThreadColorId"];
                newdr["ConsumptionQty"] = dr["ConsumptionQty"];
                newdr["MeterToCone"] = dr["MeterToCone"];
                newdr["AllowanceQty"] = dr["AllowanceQty"];
                newdr["TotalQty"] = dr["TotalQty"];
                newdr["PurchaseQty"] = dr["PurchaseQty"];
                newdr["AutoCreate"] = 1;
                newdr["UseStockQty"] = 0;
                newdr["POID"] = dr["POID"];
                newdr["Remark"] = dr["Remark"];
                newdr["Description"] = dr["Threadcombdesc"];
                newdr["colordesc"] = dr["colordesc"];
                newdr["NewCone"] = 0;
                newdr["UsedCone"] = 0;
                newdr["UseStockQty"] = 0;
                newdr["Article"] = dr["Article"];
                detailtb.Rows.Add(newdr);
            }

            DataTable subtb, sqlsubtb;
            foreach (DataRow dr in detailtb.Rows)
            {
                string sqlsub = string.Format(@"select * from #tmp a where a.refno = '{0}' and a.threadColorid='{1}'",
                                                dr["Refno"].ToString(), dr["threadColorid"].ToString());
                GetSubDetailDatas(dr, out subtb);
                if(subtb.Columns.Contains("Allowance")==false) 
                    subtb.Columns.Add("Allowance");
                MyUtility.Tool.ProcessWithDatatable(pretb_cons, "", sqlsub, out sqlsubtb);

                #region 新增第三層
                foreach (DataRow ddr in sqlsubtb.Rows)
                {
                    Decimal SL = Convert.ToDecimal(ddr["SeamLength"]);
                    Decimal URN = Convert.ToDecimal(ddr["UseRatioNumeric"]);
                    Decimal AQ = Convert.ToDecimal(ddr["Allowance"]);
                    Decimal OQ = Convert.ToDecimal(ddr["OrderQty"]);

                    DataRow newdr = subtb.NewRow();
                    newdr["Orderid"] = id;
                    newdr["Article"] = ddr["Article"];
                    newdr["ThreadCombID"] = ddr["ThreadCombId"];
                    newdr["Threadcombdesc"] = ddr["Threadcombdesc"];
                    newdr["operationid"] = ddr["operationid"];
                    newdr["SeamLength"] = ddr["SeamLength"];
                    newdr["SEQ"] = ddr["SEQ"];
                    newdr["ThreadLocationid"] = ddr["ThreadLocationid"];
                    //newdr["UseRatio"] = "";//已不需要
                    newdr["UseRatioNumeric"] = ddr["UseRatioNumeric"];
                    newdr["UseLength"]= SL*URN+AQ;
                    newdr["TotalLength"] = (SL * URN + AQ) * OQ;
                    newdr["Machinetypeid"] = ddr["Machinetypeid"];
                    newdr["OrderQty"] = ddr["OrderQty"];
                    newdr["Allowance"] = ddr["Allowance"];
                    subtb.Rows.Add(newdr);
                }
                #endregion                
            }

            #region 確認此訂單所有 Article 是否有在 Thread P01 設定 Refno & Color
            string strCheckP01Detail = string.Format(@"
select	tcc.ThreadCombID
		, mttr.ID
		, Refno = isnull(tccd.Refno, '')
		, ThreadColorID = isnull(tccd.ThreadColorID, '')
		, Article = isnull(tccd.Article, '')
into #tmp_P01
from Orders o
inner join Style s on o.StyleUkey = s.Ukey
left join ThreadColorComb tcc on s.Ukey = tcc.StyleUkey
left join MachineType_ThreadRatio mttr on tcc.Machinetypeid = mttr.ID
left join ThreadColorComb_Detail tccd on mttr.ID = tccd.Machinetypeid
										    and mttr.SEQ = tccd.SEQ			
										    and tcc.id = tccd.Id

where	o.ID = '{0}'

select distinct ThreadCombID 
from (
	-- Thread P01 資料都有但是缺少訂單中的 Article --
	select	distinct ThreadCombID
	from (	
		select *
		from (		
			select	distinct tcc.ThreadCombID
			from ThreadColorComb tcc
			inner join Orders o on tcc.StyleUkey = o.StyleUkey
			where o.id = '{0}'
		) tmp
		cross apply (
			select	Article
			from Order_Qty
			where id = '{0}'
		) orders

		Except
		select	ThreadCombID
				, Article
		from #tmp_P01
	) ExceptP01

	union all 
	-- Thread P01 資料只填一半，只需判斷訂單中的 Article --
	select ThreadCombID
	from #tmp_P01 t
	where (t.ThreadColorid = '' or t.Refno = '')
		  and Article in (select	Article
				   		  from Order_Qty
						  where id = '{0}')

	union all
	-- Thread P01 某一項 Refno, Color, Article 都是空值 --
	select ThreadCombID
	from #tmp_P01 t
	where t.ThreadColorID = ''
		  and t.Refno = ''
		  and t.Article = ''
) a
drop table #tmp_P01", id);

            DataTable dtCheckP01Detail;
            result = DBProxy.Current.Select(null, strCheckP01Detail, out dtCheckP01Detail);
            if (result)
            {
                if (dtCheckP01Detail != null && dtCheckP01Detail.Rows.Count > 0)
                {
                    StringBuilder strCheckMsg = new StringBuilder();
                    foreach (DataRow row in dtCheckP01Detail.Rows)
                    {
                        strCheckMsg.Append(string.Format("Thread Combination：{0} Refno or Color Not all set up." + Environment.NewLine, row["ThreadCombID"]));
                    }
                    MyUtility.Msg.InfoBox(strCheckMsg.ToString());
                }
            }
            else
            {
                MyUtility.Msg.WarningBox(result.Description);
            }

            #endregion 
        }
                
        #region 舊公式
//        private void textBox1_Validating(object sender, CancelEventArgs e)
//        {
//            string id = textBox1.Text;
//            DataRow drOrder;
//            if (!MyUtility.Check.Seek(string.Format("Select * from PO where id='{0}'", id)))
//            {
//                MyUtility.Msg.WarningBox("This order is not purchase master!!!");
//                e.Cancel = true;
//                textBox1.Text = "";
//                return;
//            }
//            if (MyUtility.Check.Seek(string.Format("Select * from ThreadRequisition where OrderID='{0}'", id)))
//            {
//                MyUtility.Msg.WarningBox("Order number exists!!!");
//                e.Cancel = true;
//                textBox1.Text = "";
//                return;
//            }
//            if (!MyUtility.Check.Seek(string.Format("Select * from Orders where id='{0}'", id), out drOrder)) return;
//            dateBox3.Value = MyUtility.Convert.GetDate(drOrder["SciDelivery"]);
//            dateBox4.Value = MyUtility.Convert.GetDate(drOrder["SewInLine"]);
//            CurrentMaintain["Styleid"] = drOrder["Styleid"].ToString();
//            CurrentMaintain["Seasonid"] = drOrder["Seasonid"].ToString();
//            CurrentMaintain["Brandid"] = drOrder["Brandid"].ToString();
//            CurrentMaintain["factoryid"] = drOrder["factoryid"].ToString();
//            CurrentMaintain["OrderID"] = id;
//            //用TimeStudy 的原因是因為Operationid有可能在FTY GSD被改變
//            string lengthsql = string.Format(
//@"with 
//a as 
//(Select a.*,b.refno,b.ThreadColorid,b.Article,b.ThreadLocationid,b.seq,c.UseRatio,c.UseRatioNumeric
//from ThreadColorComb a,ThreadColorComb_Detail b,MachineType_ThreadRatio c      
//Where a.id = b.id and a.styleukey = '{0}' and c.id = a.Machinetypeid and c.seq = b.seq and c.threadlocation = b.threadlocationid ),
//b as 
//(Select a.styleukey,b.operationid,d.machinetypeid,d.seamlength from ThreadColorComb a,ThreadColorComb_operation b, timestudy c,timestudy_Detail d
//Where a.id = b.id and b.operationid = d.operationid and c.id = d.id and c.styleid = '{1}' and c.seasonid = '{2}' and c.brandid = '{3}')
//Select a.*,b.operationid,b.seamlength from a,b where a.machinetypeid = b.machinetypeid
//", drOrder["StyleUkey"].ToString(), drOrder["Styleid"].ToString(), drOrder["Seasonid"].ToString(), drOrder["Brandid"].ToString());
//            DataTable lengthdt,totalqty,refartdt;
//            DBProxy.Current.Select(null, lengthsql, out lengthdt); //找出此Style 所有的用線量
//-------------------------------------------------------------------------------------------------------------------------------------------//
//            string sql = string.Format(@"Select ThreadCombId,MachineTypeId,a.Article,StyleUkey,ID,Refno,ThreadColorid,SEQ,UseRatio,UseRatioNumeric,operationid,seamlength,ThreadLocationid,Qty from #tmp a join (Select a.article,sum(a.Qty) as Qty from Order_Qty a,Orders b where a.id = b.id and b.poid = '{0}' and b.junk = 0 group by article) b On a.article = b.article", id);

//            MyUtility.Tool.ProcessWithDatatable(lengthdt, "ThreadCombId,MachineTypeId,Article,StyleUkey,ID,Refno,ThreadColorid,SEQ,UseRatio,UseRatioNumeric,operationid,seamlength,ThreadLocationid",
//sql, out totalqty);

//            MyUtility.Tool.ProcessWithDatatable(totalqty, "Refno,ThreadColorid", 
//string.Format(@"Select distinct a.Refno,a.ThreadColorid,b.description,c.description as colordesc,b.MeterToCone ,isnull(newCone,0) as newCone, isnull(usedcone,0) as usedcone
//from #tmp a 
//left join localitem b on a.refno = b.refno 
//left join threadcolor c on c.id = a.threadcolorid 
//left join (Select refno,threadcolorid,isnull(sum(NewCone),0) as newcone,isnull(sum(UsedCone),0) as usedcone from ThreadStock where  mDivisionid = '{0}' group by refno ,ThreadColorid) as d on d.refno = a.refno and d.threadcolorid = a.threadcolorid 
//",keyWord), out refartdt); //表身
//------------------------------------------------------------------------------------------------------------------------------------------//
//            DataTable detailtb = (DataTable)detailgridbs.DataSource;
//            foreach (DataRow dr in refartdt.Rows) //新增表身 by refno,ThreadColorid 增加
//            {
//                DataRow newdr = detailtb.NewRow();
//                //newdr["OrderID"] = id;
//                newdr["Refno"] = dr["Refno"];
//                newdr["ThreadColorid"] = dr["ThreadColorid"];
//                newdr["AutoCreate"] = 1;
//                newdr["usestockqty"] = 0;
//                newdr["Description"] = dr["Description"];
//                newdr["colordesc"] = dr["colordesc"];
//                newdr["MeterToCone"] = dr["MeterToCone"];
//                newdr["NewCone"] = dr["NewCone"];
//                newdr["UsedCone"] = dr["UsedCone"];
//                newdr["ConsumptionQty"] = 0;
//                newdr["TotalQty"] = 0;
//                newdr["AllowanceQty"] = 0;
//                newdr["UseStockQty"] = 0;
//                newdr["PurchaseQty"] = 0;
//                detailtb.Rows.Add(newdr);
//            }
//----------------------------------------------------------------------------------------------------------------------------------------------//

//            DataTable subtb,sqltb;            
//            foreach (DataRow dr in detailtb.Rows)
//            {
//                sql = string.Format(@"Select ThreadCombId,MachineTypeId,ID,Refno,Article,ThreadLocationid,SEQ,UseRatio,isnull(UseRatioNumeric,0) as UseRatioNumeric,operationid,isnull(seamlength,0) as seamlength ,isnull(Qty ,0) as Qty
//from #tmp a where a.refno = '{0}' and a.threadColorid='{1}'", dr["Refno"].ToString(), dr["threadColorid"].ToString());
//                GetSubDetailDatas(dr, out subtb);
//                MyUtility.Tool.ProcessWithDatatable(totalqty,
//@"ThreadCombId,MachineTypeId,ID,Refno,Article,ThreadLocationid,SEQ,UseRatio,UseRatioNumeric,operationid,seamlength,Qty,ThreadColorid",
//sql, out sqltb);
//                decimal cons = 0;
//                #region 新增第三層
//                foreach (DataRow ddr in sqltb.Rows)
//                {
//                    DataRow newdr = subtb.NewRow();
//                    newdr["Orderid"] = id;
//                    newdr["Article"] = ddr["Article"];
//                    newdr["ThreadLocationid"] = ddr["ThreadLocationid"];
//                    newdr["SEQ"] = ddr["SEQ"];
//                    newdr["UseRatio"] = ddr["UseRatio"];
//                    newdr["UseRatioNumeric"] = ddr["UseRatioNumeric"];
//                    newdr["operationid"] = ddr["operationid"];
//                    newdr["SeamLength"] = ddr["SeamLength"];
//                    newdr["Machinetypeid"] = ddr["Machinetypeid"];
//                    newdr["ThreadCombID"] = ddr["ThreadCombId"];
//                    newdr["OrderQty"] = ddr["Qty"];
//                    subtb.Rows.Add(newdr);
//                    cons = cons + Convert.ToDecimal(ddr["UseRatioNumeric"]) * Convert.ToDecimal(ddr["Qty"]) * Convert.ToDecimal(ddr["SeamLength"]);
//                }
//                #endregion
//                //填入Cons
//                dr["ConsumptionQty"] = cons;
//                dr["TotalQty"] =Convert.ToDecimal(dr["MeterToCone"])!=0? Math.Ceiling(cons / Convert.ToDecimal(dr["MeterToCone"])):0;
//                dr["AllowanceQty"] = Convert.ToDouble(dr["TotalQty"]) * 0.2;
//                dr["PurchaseQty"] =Convert.ToDecimal(dr["TotalQty"]) + Convert.ToDecimal(dr["AllowanceQty"]) - Convert.ToDecimal(dr["UseStockQty"]);
//            }
//        }
        #endregion

        protected override void OnDetailGridDataInserted(DataRow data)
        {
            base.OnDetailGridDataInserted(data);
            CurrentDetailData["AutoCreate"] = 0;
        }

        protected override void OnDetailGridRowChanged()
        {         
            base.OnDetailGridRowChanged();
        }

        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            base.ClickCheck();

            string updSql = string.Format("update ThreadRequisition set Status = 'Approved' ,editname='{0}', editdate = GETDATE() where orderid='{1}'", loginID, CurrentMaintain["Orderid"].ToString());

            DualResult upResult;
            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (!(upResult = DBProxy.Current.Execute(null, updSql)))
                    {
                        _transactionscope.Dispose();
                        return;
                    }
                    _transactionscope.Complete();
                    _transactionscope.Dispose();
                    MyUtility.Msg.WarningBox("Successfully");
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            _transactionscope.Dispose();
            _transactionscope = null;

           
        }

        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();

            string updSql = string.Format("update ThreadRequisition set Status = 'New' ,editname='{0}', editdate = GETDATE() where OrderID='{1}'", loginID, CurrentMaintain["OrderID"].ToString());

            DualResult upResult;
            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (!(upResult = DBProxy.Current.Execute(null, updSql)))
                    {
                        _transactionscope.Dispose();
                        return;
                    }
                    _transactionscope.Complete();
                    _transactionscope.Dispose();
                    MyUtility.Msg.WarningBox("Successfully");
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            _transactionscope.Dispose();
            _transactionscope = null;

            
        }
       
        private void ReQty(DataRow dr) //重算Qty
        {
            dr["TotalQty"] = Convert.ToDecimal(dr["MeterToCone"]) != 0 ? Math.Ceiling(Convert.ToDecimal(dr["ConsumptionQty"]) / Convert.ToDecimal(dr["MeterToCone"])) : 0;
            dr["AllowanceQty"] = Convert.ToDouble(dr["TotalQty"]) * 0.2;
            dr["PurchaseQty"] = Convert.ToDecimal(dr["TotalQty"]) + Convert.ToDecimal(dr["AllowanceQty"]) - Convert.ToDecimal(dr["UseStockQty"]);
        }

        protected override bool ClickPrint()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Refno"));
            dt.Columns.Add(new DataColumn("description"));
            dt.Columns.Add(new DataColumn("ThreadColorid"));
            dt.Columns.Add(new DataColumn("Colordesc"));
            dt.Columns.Add(new DataColumn("MeterToCone"));
            dt.Columns.Add(new DataColumn("TotalQty"));
            dt.Columns.Add(new DataColumn("AllowanceQty"));
            dt.Columns.Add(new DataColumn("UseStockQty"));
            dt.Columns.Add(new DataColumn("PurchaseQty"));

            foreach (DataGridViewRow row in detailgrid.Rows)
            {
                DataRow dr = dt.NewRow();
                dr["Refno"] = row.Cells["Refno"].Value.ToString();
                dr["description"] = row.Cells["description"].Value.ToString();
                dr["ThreadColorid"] = row.Cells["ThreadColorid"].Value.ToString();
                dr["Colordesc"] = row.Cells["Colordesc"].Value.ToString();
                dr["MeterToCone"] = row.Cells["MeterToCone"].Value.ToString();
                dr["TotalQty"] = row.Cells["TotalQty"].Value.ToString();
                dr["AllowanceQty"] = row.Cells["AllowanceQty"].Value.ToString();
                dr["UseStockQty"] = row.Cells["UseStockQty"].Value.ToString();
                dr["PurchaseQty"] = row.Cells["PurchaseQty"].Value.ToString();
                dt.Rows.Add(dr);
            }

            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Thread_P02.xltx"); //預先開啟excel app
            bool result = MyUtility.Excel.CopyToXls(dt, "", showExcel: false, xltfile: "Thread_P02.xltx", headerRow: 7, excelApp: objApp);


            if (!result)
            {
                MyUtility.Msg.WarningBox(result.ToString(), "Warning");
            }
            else
            {
                Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
                objSheets.Cells[3, 2] = txtSP.Text.ToString();
                objSheets.Cells[3, 6] = dateSCIDelivery.Text.ToString();

                objSheets.Cells[4, 2] = displayBrand.Text.ToString();
                objSheets.Cells[4, 6] = dateSewingInLine.Text.ToString();

                objSheets.Cells[5, 2] = displayStyle.Text.ToString();
                objSheets.Cells[5, 6] = dateEstBooking.Text.ToString();

                objSheets.Cells[6, 2] = displaySeason.Text.ToString();
                objSheets.Cells[6, 6] = dateEstArrived.Text.ToString();

                objSheets.Columns.AutoFit();
                objSheets.Rows.AutoFit();

                Marshal.ReleaseComObject(objSheets);   //釋放sheet
            }

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("Thread_P02");
            objApp.ActiveWorkbook.SaveAs(strExcelName);
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);

            strExcelName.OpenFile();
            #endregion
            return base.ClickPrint();
        }

        private void buttonQtyBreakdown_Click(object sender, EventArgs e)
        {
            new P02_QtyBreakdownByColorway(CurrentMaintain).ShowDialog();
        }
    }
}
