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
    /// <summary>
    /// P02
    /// </summary>
    public partial class P02 : Win.Tems.Input8
    {
        private string factory = Sci.Env.User.Factory;
        private string loginID = Sci.Env.User.UserID;
        private string keyWord = Sci.Env.User.Keyword;
        private Ict.Win.UI.DataGridViewTextBoxColumn col_color;
        private DataGridViewNumericBoxColumn col_cons;
        private DataGridViewNumericBoxColumn col_Allowance;
        private DataGridViewNumericBoxColumn col_NewCone;
        private DataGridViewNumericBoxColumn col_UsedCone;
        /// <summary>
        /// P02
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            string defaultfilter = string.Format("MDivisionid = '{0}' ", this.keyWord);
            this.DefaultFilter = defaultfilter;
            this.InitializeComponent();
            this.DoSubForm = new P02_Detail();

            int lApprove = 0; // 有Confirm權限皆可按Pass的Approve, 有Check權限才可按Fail的Approve(TeamLeader 有Approve權限,Supervisor有Check)
            string menupk = MyUtility.GetValue.Lookup("Pkey", "Sci.Production.Thread.P02", "MenuDetail", "FormName");
            string pass0pk = MyUtility.GetValue.Lookup("FKPass0", this.loginID, "Pass1", "ID");
            string pass2_cmd = string.Format("Select * from Pass2 WITH (NOLOCK) Where FKPass0 ='{0}' and FKMenu='{1}'", pass0pk, menupk);
            DataRow pass2_dr;
            if (MyUtility.Check.Seek(pass2_cmd, out pass2_dr))
            {
                lApprove = pass2_dr["CanConfirm"].ToString() == "True" ? 1 : 0;
            }

            this.btnBatchapprove.Visible = lApprove == 1;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataTable queryDT;
            string querySql = string.Format(
                @"
select '' FTYGroup

union 
select distinct FTYGroup 
from Factory 
where MDivisionID = '{0}'",
                Sci.Env.User.Keyword);

            DBProxy.Current.Select(null, querySql, out queryDT);
            MyUtility.Tool.SetupCombox(this.queryfors, 1, queryDT);
            this.queryfors.SelectedIndex = 0;
            this.queryfors.SelectedIndexChanged += (s, e) =>
            {
                switch (this.queryfors.SelectedIndex)
                {
                    case 0:
                        this.DefaultWhere = string.Empty;
                        break;
                    default:
                        this.DefaultWhere = string.Format("FactoryID = '{0}'", this.queryfors.SelectedValue);
                        break;
                }

                this.ReloadDatas();
            };
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            this.label7.Text = this.CurrentMaintain["Status"].ToString();

            DataRow dr;
            if (MyUtility.Check.Seek(string.Format("Select GetSCI.MinSciDelivery,GetSCI.MinSewinLine from Orders WITH (NOLOCK) cross apply dbo.GetSCI(Orders.ID , Orders.Category) as GetSCI where orders.id='{0}'", this.CurrentMaintain["OrderID"].ToString()), out dr))
            {
                if (!MyUtility.Check.Empty(dr["MinSciDelivery"]))
                {
                    this.dateSCIDelivery.Value = Convert.ToDateTime(dr["MinSciDelivery"]);
                }
                else
                {
                    this.dateSCIDelivery.Text = string.Empty;
                }

                if (!MyUtility.Check.Empty(dr["MinSewinLine"]))
                {
                    this.dateSewingInLine.Value = Convert.ToDateTime(dr["MinSewinLine"]);
                }
                else
                {
                    this.dateSewingInLine.Text = string.Empty;
                }
            }
            else
            {
                this.dateSCIDelivery.Text = string.Empty;
                this.dateSewingInLine.Text = string.Empty;
            }
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(Win.Tems.InputMasterDetail.PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["OrderID"].ToString();
            this.DetailSelectCommand = string.Format(
                @"
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
WHERE a.OrderID = '{0}'",
                masterID,
                this.keyWord);

            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override DualResult OnSubDetailSelectCommandPrepare(Win.Tems.Input8.PrepareSubDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Detail == null) ? string.Empty : e.Detail["Ukey"].ToString();

            this.SubDetailSelectCommand = string.Format(
@"Select a.*, b.description as Threadcombdesc,c.name as ThreadLocation, isnull(seamlength * UseRatioNumeric + isnull(a.Allowance,0),0) as uselength,
OrderQty,isnull((seamlength * UseRatioNumeric+ isnull(a.Allowance,0)) * OrderQty,0) as TotalLength
from ThreadRequisition_Detail_Cons a WITH (NOLOCK) 
Left join ThreadComb b WITH (NOLOCK) on b.id = a.threadcombid
Left join Reason c WITH (NOLOCK) on c.reasontypeid = 'ThreadLocation' and c.id = a.threadlocationid
where a.ThreadRequisition_DetailUkey = '{0}'", masterID);
            return base.OnSubDetailSelectCommandPrepare(e);
        }

        private celllocalitem refno = (celllocalitem)celllocalitem.GetGridCell("Thread", null, ",,,description");

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();

            DataGridViewGeneratorTextColumnSettings thcolor = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorNumericColumnSettings cons = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings newCone = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings usedCone = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings poqty1 = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings poqty2 = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings poqty3 = new DataGridViewGeneratorNumericColumnSettings();
            #region Refno Cell
             this.refno.CellValidating += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                if (this.CurrentDetailData == null)
                {
                    return;
                }

                string oldvalue = this.CurrentDetailData["refno"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (!this.EditMode || oldvalue == newvalue)
                {
                    return;
                }

                if (MyUtility.Check.Empty(newvalue))
                {
                    return;
                }

                DataRow refdr;
                if (MyUtility.Check.Seek(string.Format("Select * from Localitem WITH (NOLOCK) where refno = '{0}' and junk = 0", newvalue), out refdr))
                {
                    this.CurrentDetailData["Description"] = refdr["Description"];
                    this.CurrentDetailData["Refno"] = refdr["refno"];
                    this.CurrentDetailData["MeterToCone"] = refdr["MeterToCone"];
                }
                else
                {
                    this.CurrentDetailData["Description"] = string.Empty;
                    this.CurrentDetailData["Refno"] = string.Empty;
                    this.CurrentDetailData["MeterToCone"] = 0;
                }

                string sql = string.Format("Select isnull(sum(newCone),0) as newCone,isnull(sum(usedCone),0) as usedCone from ThreadStock WITH (NOLOCK) where refno ='{0}' and threadcolorid = '{1}' and mDivisionid ='{2}' ", newvalue, this.CurrentDetailData["ThreadColorid"].ToString(), this.keyWord);
                if (MyUtility.Check.Seek(sql, out refdr))
                {
                    this.CurrentDetailData["NewCone"] = refdr["NewCone"];
                    this.CurrentDetailData["UsedCone"] = refdr["UsedCone"];
                }
                else
                {
                    this.CurrentDetailData["NewCone"] = 0;
                    this.CurrentDetailData["UsedCone"] = 0;
                }

                this.ReQty(this.CurrentDetailData);
                this.CurrentDetailData.EndEdit();
                this.Update_detailgrid_CellValidated(e.RowIndex);
                this.detailgrid.ValidateControl();
            };
            #endregion
            #region Color Cell
            thcolor.CellValidating += (s, e) =>
            {
                string oldvalue = this.CurrentDetailData["threadcolorid"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (!this.EditMode || oldvalue == newvalue)
                {
                    return;
                }

                if (this.CurrentDetailData["autoCreate"].ToString() == "True")
                {
                    return;
                }

                DataRow refdr;
                if (MyUtility.Check.Seek(string.Format("Select * from ThreadColor WITH (NOLOCK) where id = '{0}' and junk = 0", newvalue), out refdr))
                {
                    this.CurrentDetailData["ThreadColorid"] = refdr["ID"];
                    this.CurrentDetailData["Colordesc"] = refdr["Description"];
                }
                else
                {
                    this.CurrentDetailData["ThreadColorid"] = string.Empty;
                    this.CurrentDetailData["Colordesc"] = string.Empty;
                    MyUtility.Msg.WarningBox(string.Format("< Thread Color: {0}> not found.", e.FormattedValue.ToString()));
                    return;
                }

                string sql = string.Format("Select isnull(sum(newCone),0) as newCone,isnull(sum(usedCone),0) as usedCone from ThreadStock WITH (NOLOCK) where refno ='{0}' and threadcolorid = '{1}' and mDivisionid ='{2}' ", this.CurrentDetailData["Refno"].ToString(), newvalue, this.keyWord);
                if (MyUtility.Check.Seek(sql, out refdr))
                {
                    this.CurrentDetailData["NewCone"] = refdr["NewCone"];
                    this.CurrentDetailData["UsedCone"] = refdr["UsedCone"];
                }
                else
                {
                    this.CurrentDetailData["NewCone"] = 0;
                    this.CurrentDetailData["UsedCone"] = 0;
                }

               // ReQty(CurrentDetailData);
                this.CurrentDetailData.EndEdit();
                this.Update_detailgrid_CellValidated(e.RowIndex);
            };
            thcolor.EditingMouseDown = (s, e) =>
            {
                if (this.EditMode)
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        if (e.RowIndex != -1)
                        {
                            if (this.CurrentDetailData["autoCreate"].ToString() == "True")
                            {
                                return;
                            }

                            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("select ID,Description from ThreadColor WITH (NOLOCK) where junk = 0 order by ID", "10,40", this.CurrentDetailData["ThreadColorid"].ToString().Trim());
                            DialogResult returnResult = item.ShowDialog();
                            if (returnResult == DialogResult.Cancel)
                            {
                                return;
                            }

                            var sellist = item.GetSelecteds();
                            this.CurrentDetailData["Colordesc"] = sellist[0][1];
                            e.EditingControl.Text = item.GetSelectedString();
                        }
                    }
                }
            };
            #endregion
            #region cons Cell
            cons.CellValidating += (s, e) =>
            {
                decimal oldvalue = (decimal)this.CurrentDetailData["ConsumptionQty"];
                decimal newvalue = (decimal)e.FormattedValue;
                if (!this.EditMode || oldvalue == newvalue)
                {
                    return;
                }

                this.CurrentDetailData["ConsumptionQty"] = newvalue;
                this.ReQty(this.CurrentDetailData);
                this.CurrentDetailData.EndEdit();
            };
            cons.CellMouseDoubleClick += (s, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    this.OpenSubDetailPage();
                }
            };

            #endregion
            #region useStock CellValidating by (NewCone)+UsedCone

            newCone.CellValidating += (s, e) =>
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

            usedCone.CellValidating += (s, e) =>
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
            this.Helper.Controls.Grid.Generator(this.detailgrid)
           .Text("Refno", header: "Thread Refno", width: Ict.Win.Widths.AnsiChars(10), iseditingreadonly: true, settings: this.refno)
           .Text("description", header: "Thread Desc", width: Ict.Win.Widths.AnsiChars(18), iseditingreadonly: true)
           .EditText("Article", header: "Article", width: Ict.Win.Widths.AnsiChars(12), iseditingreadonly: true)
           .Text("ThreadColorid", header: "Thread\r\nColor", width: Ict.Win.Widths.AnsiChars(4), settings: thcolor).Get(out this.col_color)
           .Text("Colordesc", header: "Thread Color Desc", width: Ict.Win.Widths.AnsiChars(18), iseditingreadonly: true)
           .Numeric("ConsumptionQty", header: "Total\r\nCons.(M)", width: Ict.Win.Widths.AnsiChars(2), integer_places: 6, settings: cons).Get(out this.col_cons)
           .Numeric("MeterToCone", header: "No. of Meters\r\nPer Cones", width: Ict.Win.Widths.AnsiChars(6), integer_places: 7, decimal_places: 1, iseditingreadonly: true)
           .Numeric("TotalQty", header: "No. of\r\nCones", width: Ict.Win.Widths.AnsiChars(2), integer_places: 6, iseditingreadonly: true, settings: poqty1)
           .Numeric("AllowanceQty", header: "Allowance", width: Ict.Win.Widths.AnsiChars(2), integer_places: 6, settings: poqty2).Get(out this.col_Allowance)
           .Numeric("NewCone", header: "New\r\nCone", width: Ict.Win.Widths.AnsiChars(2), integer_places: 6, settings: newCone).Get(out this.col_NewCone)
           .Numeric("UsedCone", header: "Use\r\nCone", width: Ict.Win.Widths.AnsiChars(2), integer_places: 6, settings: usedCone).Get(out this.col_UsedCone)
           .Numeric("UseStockQty", header: "Use\r\nStock", width: Ict.Win.Widths.AnsiChars(2), integer_places: 6, iseditingreadonly: true, settings: poqty3)
           .Numeric("PurchaseQty", header: "PO Qty", width: Ict.Win.Widths.AnsiChars(2), integer_places: 6, iseditingreadonly: true)
           .Text("Remark", header: "Remark", width: Ict.Win.Widths.AnsiChars(10))
           .Text("POID", header: "PO ID", width: Ict.Win.Widths.AnsiChars(12), iseditingreadonly: true);

            #endregion

            this.detailgrid.Columns["Remark"].DefaultCellStyle.BackColor = Color.Pink;

            // 設定detailGrid Rows 是否可以編輯
            this.detailgrid.RowEnter += this.Detailgrid_RowEnter;
            this.Change_record();
            this.detailgrid.CellValidated += this.Detailgrid_CellValidated;
        }

        private void Detailgrid_RowEnter(object sender, DataGridViewCellEventArgs e)
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

            // AutoCrate為1表示資料來源為(P01計算出來的就不能再讓使用者修改線種,顏色,總長度)
            if (data["autoCreate"].ToString() == "True")
            {
                this.col_color.IsEditingReadOnly = true;
                this.col_cons.IsEditingReadOnly = true;
                this.refno.SupportPopup = false;
            }
            else
            {
                this.col_color.IsEditingReadOnly = false;
                this.col_cons.IsEditingReadOnly = false;
                this.refno.SupportPopup = true;
            }

            // POID應為全鎖，因為 POID表示在SubconP30已有建立採購單並Confrim，
            // 不能再給使用者修改請購單的所有項目。
            if (!MyUtility.Check.Empty(data["POID"]))
            {
                this.col_Allowance.IsEditingReadOnly = true;
                this.col_NewCone.IsEditingReadOnly = true;
                this.col_UsedCone.IsEditingReadOnly = true;
                this.col_color.IsEditingReadOnly = true;
                this.col_cons.IsEditingReadOnly = true;
            }
            else
            {
                this.col_Allowance.IsEditingReadOnly = false;
                this.col_NewCone.IsEditingReadOnly = false;
                this.col_UsedCone.IsEditingReadOnly = false;
            }
        }

        #region 是否可編輯與變色
        private void Change_record()
        {
            this.col_color.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (dr["autoCreate"].ToString() == "True" || !MyUtility.Check.Empty(dr["POID"]))
                {
                    e.CellStyle.BackColor = Color.White;
                    e.CellStyle.ForeColor = Color.Black;
                }
                else
                {
                    e.CellStyle.BackColor = Color.Pink;
                }
            };
            this.col_cons.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (dr["autoCreate"].ToString() == "True" || !MyUtility.Check.Empty(dr["POID"]))
                {
                    e.CellStyle.BackColor = Color.White;
                    e.CellStyle.ForeColor = Color.Black;
                }
                else
                {
                    e.CellStyle.BackColor = Color.Pink;
                }
            };
            this.col_Allowance.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
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
            this.col_NewCone.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
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
            this.col_UsedCone.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1)
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
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

        private void Detailgrid_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (!this.EditMode
                || (e.ColumnIndex != this.col_Allowance.Index
                    && e.ColumnIndex != this.col_NewCone.Index
                    && e.ColumnIndex != this.col_UsedCone.Index
                    && e.ColumnIndex != this.col_cons.Index))
            {
                return;
            }

            this.Update_detailgrid_CellValidated(e.RowIndex);
        }

        private void Update_detailgrid_CellValidated(int rowIndex)
        {
            int nc = Convert.ToInt32(this.CurrentDetailData["NewCone"]);
            int uc = Convert.ToInt32(this.CurrentDetailData["UsedCone"]);
            int usq = nc + uc;
            this.CurrentDetailData["UseStockQty"] = usq;

            int tq = Convert.ToInt32(this.CurrentDetailData["TotalQty"]);
            int aq = Convert.ToInt32(this.CurrentDetailData["AllowanceQty"]);
            int temp = tq + aq - usq;
            if (temp < 0)
            {
                temp = 0;
            }

            this.CurrentDetailData["PurchaseQty"] = temp;
            this.detailgrid.InvalidateRow(rowIndex);
        }

        /// <inheritdoc/>
        protected override bool ClickDeleteBefore()
        {
            if (this.CurrentMaintain["Status"].ToString() != "New")
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't be deleted.", "Warning");
                return false;
            }

            return base.ClickDeleteBefore();
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["Factoryid"] = this.factory;
            this.CurrentMaintain["mDivisionid"] = this.keyWord;
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["estbookDate"]))
            {
                this.dateEstBooking.Focus();
                MyUtility.Msg.WarningBox("<Est. Booking> can not be empty.");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["EstArriveDate"]))
            {
                this.dateEstArrived.Focus();
                 MyUtility.Msg.WarningBox("<Est. Arrived> can not be empty.");
                return false;
            }

            // foreach (DataRow dr in DetailDatas)
            // {
            //    if (MyUtility.Check.Empty(dr["PurchaseQty"]))
            //    {
            //        MyUtility.Msg.WarningBox("<PO Qty> can not be 0");
            //        return false;
            //    }
            // }
            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            if (this.CurrentMaintain["Status"].ToString() != "New")
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't be modify", "Warning");
                return false;
            }

            return base.ClickEditBefore();
        }

        private void TxtSP_Validating(object sender, CancelEventArgs e)
        {
            if (this.txtSP.OldValue == this.txtSP.Text)
            {
                return;
            }

            string id = this.txtSP.Text;
            DataRow drOrder;
            DataTable pretb_cons, tR_DUK;
            string sqlpre;
            string sqltr_duk;
            DataTable detailtb = (DataTable)this.detailgridbs.DataSource;

            foreach (Control item in this.masterpanel.Controls)
            {
                if (item is Sci.Win.UI.Label || item is Sci.Win.UI.Button || item == this.displayM || item == this.txtSP)
                {
                }
                else
                {
                    item.Text = string.Empty;
                }
            }

            detailtb.Clear();

            if (this.txtSP.Text == string.Empty)
            {
                return;
            }

            ////確認order.poid 同(po.id)有沒有這筆,沒有則return
            // if (!MyUtility.Check.Seek(string.Format("Select * from PO WITH (NOLOCK) where id='{0}'", id)))
            // {
            //    e.Cancel = true;
            //    txtSP.Text = "";
            //    MyUtility.Msg.WarningBox(string.Format("<SP#: {0} >does not exists in Purchase Order!!!", id));
            //    return;
            // }
            // 確認orders.id + 工廠有沒有這筆,沒有則return
            if (!MyUtility.Check.Seek(string.Format("Select * from orders WITH (NOLOCK) inner join Factory on orders.FactoryID=Factory.ID where orders.id='{0}' and orders.FtyGroup = '{1}' and Factory.IsProduceFty=1", id, this.factory)))
            {
                e.Cancel = true;
                this.txtSP.Text = string.Empty;
                MyUtility.Msg.WarningBox(string.Format("<SP#: {0} >Data not found!!!!", id));
                return;
            }

            // 確認ThreadRequisition有沒有這筆,有則return
            if (MyUtility.Check.Seek(string.Format("Select * from ThreadRequisition WITH (NOLOCK) where OrderID='{0}'", id)))
            {
                e.Cancel = true;
                this.txtSP.Text = string.Empty;
                MyUtility.Msg.WarningBox("Order number exists already.");
                return;
            }

            // 輸入的POno帶出其他6個表頭
            if (!MyUtility.Check.Seek(string.Format("Select GetSCI.MinSciDelivery,GetSCI.MinSewinLine,Styleid,Seasonid,Brandid,FtyGroup from Orders WITH (NOLOCK) cross apply dbo.GetSCI(Orders.ID , Orders.Category) as GetSCI where Orders.poid='{0}' ", id), out drOrder))
            {
                return;
            }

            this.dateSCIDelivery.Value = MyUtility.Convert.GetDate(drOrder["MinSciDelivery"]);
            this.dateSewingInLine.Value = MyUtility.Convert.GetDate(drOrder["MinSewinLine"]);
            this.CurrentMaintain["Styleid"] = drOrder["Styleid"].ToString();
            this.CurrentMaintain["Seasonid"] = drOrder["Seasonid"].ToString();
            this.CurrentMaintain["Brandid"] = drOrder["Brandid"].ToString();
            this.CurrentMaintain["factoryid"] = drOrder["FtyGroup"].ToString();
            this.CurrentMaintain["OrderID"] = id;

            // 事先整理資料
            sqlpre = string.Format(
                @"
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
    where b.POID = '{0}' and b.Category!='G' and b.junk=0
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
        and f.OrderQty != '0'",
                id);

            DualResult result;
            result = DBProxy.Current.Select(null, sqlpre, out pretb_cons);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            StringBuilder warningmsg = new StringBuilder();
            foreach (DataRow temp in pretb_cons.Rows)
            {
                if (Convert.ToDecimal(temp["MeterToCone"].ToString()) == 0)
                {
                    warningmsg.Append(string.Format("Thread Refno:{0} <No. of Meters Per Cones> is 0 can't calculate <No. of Cones>", temp["Refno"].ToString()) + Environment.NewLine);
                }
            }

            if (warningmsg.ToString() != string.Empty)
            {
                MyUtility.Msg.InfoBox(warningmsg.ToString());
            }

            // 做資料匯整select group 後填入ThreadRequisition_Detail
            sqltr_duk = string.Format(
                @"
select *
       , AllowanceQty = isnull (AllowanceQty.value, 0)
       , PurchaseQty = TotalQty + isnull (AllowanceQty.value, 0)
from (
    select  Orderid = '{0}'
            , #tmp.Refno
            , ThreadColorId
            , Threadcombdesc
            , colordesc
            , #tmp.MeterToCone
            , ConsumptionQty = CEILING(Sum(OrderQty * (Seamlength * UseRatioNumeric + Allowance)) / 100)
            , TotalQty = IIF(#tmp.MeterToCone > 0, CEILING (Sum (OrderQty * (Seamlength * UseRatioNumeric + Allowance)) / 100 / #tmp.MeterToCone)
                                                 , 0)
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
) tmp
outer apply (
    select top 1 value = CEILING (TotalQty * Allowance)
    from ThreadAllowanceScale tas
    where tas.LowerBound <= TotalQty
          and TotalQty <= tas.UpperBound
) AllowanceQty",
                id);

            if (pretb_cons.Rows.Count <= 0)
            {
                tR_DUK = pretb_cons.Clone();
            }
            else
            {
                result = MyUtility.Tool.ProcessWithDatatable(pretb_cons, string.Empty, sqltr_duk, out tR_DUK, "#tmp"); // TR_DUK為表身
                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }
            }

            // 新增表身
            foreach (DataRow dr in tR_DUK.Rows)
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
                string sqlsub = string.Format(
                    @"select * from #tmp a where a.refno = '{0}' and a.threadColorid='{1}'",
                    dr["Refno"].ToString(),
                    dr["threadColorid"].ToString());

                this.GetSubDetailDatas(dr, out subtb);
                if (subtb.Columns.Contains("Allowance") == false)
                {
                    subtb.Columns.Add("Allowance");
                }

                MyUtility.Tool.ProcessWithDatatable(pretb_cons, string.Empty, sqlsub, out sqlsubtb);

                #region 新增第三層
                foreach (DataRow ddr in sqlsubtb.Rows)
                {
                    decimal sL = Convert.ToDecimal(ddr["SeamLength"]);
                    decimal uRN = Convert.ToDecimal(ddr["UseRatioNumeric"]);
                    decimal aQ = Convert.ToDecimal(ddr["Allowance"]);
                    decimal oQ = Convert.ToDecimal(ddr["OrderQty"]);

                    DataRow newdr = subtb.NewRow();
                    newdr["Orderid"] = id;
                    newdr["Article"] = ddr["Article"];
                    newdr["ThreadCombID"] = ddr["ThreadCombId"];
                    newdr["Threadcombdesc"] = ddr["Threadcombdesc"];
                    newdr["operationid"] = ddr["operationid"];
                    newdr["SeamLength"] = ddr["SeamLength"];
                    newdr["SEQ"] = ddr["SEQ"];
                    newdr["ThreadLocationid"] = ddr["ThreadLocationid"];

                    // newdr["UseRatio"] = "";//已不需要
                    newdr["UseRatioNumeric"] = ddr["UseRatioNumeric"];
                    newdr["UseLength"] = (sL * uRN) + aQ;
                    newdr["TotalLength"] = ((sL * uRN) + aQ) * oQ;
                    newdr["Machinetypeid"] = ddr["Machinetypeid"];
                    newdr["OrderQty"] = ddr["OrderQty"];
                    newdr["Allowance"] = ddr["Allowance"];
                    subtb.Rows.Add(newdr);
                }
                #endregion
            }

            #region 確認此訂單所有 Article 是否有在 Thread P01 設定 Refno & Color
            string strCheckP01Detail = string.Format(
                @"
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
			from ThreadColorComb tcc  WITH (NOLOCK)
			inner join Orders o  WITH (NOLOCK) on tcc.StyleUkey = o.StyleUkey
			where o.id = '{0}'
		) tmp
		cross apply (
			select distinct	Article
			from Orders o WITH (NOLOCK)
			INNER JOIN  Order_Qty oq WITH (NOLOCK) on o.id = oq.id
			where o.poid = '{0}' and o.Junk = 0 and o.Category <> 'G'
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
		  and Article in (select distinct	Article
			                from Orders o WITH (NOLOCK)
			                INNER JOIN  Order_Qty oq WITH (NOLOCK) on o.id = oq.id
			                where o.poid = '{0}' and o.Junk = 0 and o.Category <> 'G')

	union all
	-- Thread P01 某一項 Refno, Color, Article 都是空值 --
	select ThreadCombID
	from #tmp_P01 t
	where t.ThreadColorID = ''
		  and t.Refno = ''
		  and t.Article = ''
) a
drop table #tmp_P01",
                id);

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

        // private void textBox1_Validating(object sender, CancelEventArgs e)
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
        // @"with
        // a as
        // (Select a.*,b.refno,b.ThreadColorid,b.Article,b.ThreadLocationid,b.seq,c.UseRatio,c.UseRatioNumeric
        // from ThreadColorComb a,ThreadColorComb_Detail b,MachineType_ThreadRatio c
        // Where a.id = b.id and a.styleukey = '{0}' and c.id = a.Machinetypeid and c.seq = b.seq and c.threadlocation = b.threadlocationid ),
        // b as
        // (Select a.styleukey,b.operationid,d.machinetypeid,d.seamlength from ThreadColorComb a,ThreadColorComb_operation b, timestudy c,timestudy_Detail d
        // Where a.id = b.id and b.operationid = d.operationid and c.id = d.id and c.styleid = '{1}' and c.seasonid = '{2}' and c.brandid = '{3}')
        // Select a.*,b.operationid,b.seamlength from a,b where a.machinetypeid = b.machinetypeid
        // ", drOrder["StyleUkey"].ToString(), drOrder["Styleid"].ToString(), drOrder["Seasonid"].ToString(), drOrder["Brandid"].ToString());
        //            DataTable lengthdt,totalqty,refartdt;
        //            DBProxy.Current.Select(null, lengthsql, out lengthdt); //找出此Style 所有的用線量
        //-------------------------------------------------------------------------------------------------------------------------------------------//
        //            string sql = string.Format(@"Select ThreadCombId,MachineTypeId,a.Article,StyleUkey,ID,Refno,ThreadColorid,SEQ,UseRatio,UseRatioNumeric,operationid,seamlength,ThreadLocationid,Qty from #tmp a join (Select a.article,sum(a.Qty) as Qty from Order_Qty a,Orders b where a.id = b.id and b.poid = '{0}' and b.junk = 0 group by article) b On a.article = b.article", id);

        // MyUtility.Tool.ProcessWithDatatable(lengthdt, "ThreadCombId,MachineTypeId,Article,StyleUkey,ID,Refno,ThreadColorid,SEQ,UseRatio,UseRatioNumeric,operationid,seamlength,ThreadLocationid",
        // sql, out totalqty);

        // MyUtility.Tool.ProcessWithDatatable(totalqty, "Refno,ThreadColorid",
        // string.Format(@"Select distinct a.Refno,a.ThreadColorid,b.description,c.description as colordesc,b.MeterToCone ,isnull(newCone,0) as newCone, isnull(usedcone,0) as usedcone
        // from #tmp a
        // left join localitem b on a.refno = b.refno
        // left join threadcolor c on c.id = a.threadcolorid
        // left join (Select refno,threadcolorid,isnull(sum(NewCone),0) as newcone,isnull(sum(UsedCone),0) as usedcone from ThreadStock where  mDivisionid = '{0}' group by refno ,ThreadColorid) as d on d.refno = a.refno and d.threadcolorid = a.threadcolorid
        // ",keyWord), out refartdt); //表身
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

        // DataTable subtb,sqltb;
        //            foreach (DataRow dr in detailtb.Rows)
        //            {
        //                sql = string.Format(@"Select ThreadCombId,MachineTypeId,ID,Refno,Article,ThreadLocationid,SEQ,UseRatio,isnull(UseRatioNumeric,0) as UseRatioNumeric,operationid,isnull(seamlength,0) as seamlength ,isnull(Qty ,0) as Qty
        // from #tmp a where a.refno = '{0}' and a.threadColorid='{1}'", dr["Refno"].ToString(), dr["threadColorid"].ToString());
        //                GetSubDetailDatas(dr, out subtb);
        //                MyUtility.Tool.ProcessWithDatatable(totalqty,
        // @"ThreadCombId,MachineTypeId,ID,Refno,Article,ThreadLocationid,SEQ,UseRatio,UseRatioNumeric,operationid,seamlength,Qty,ThreadColorid",
        // sql, out sqltb);
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

        /// <inheritdoc/>
        protected override void OnDetailGridDataInserted(DataRow data)
        {
            base.OnDetailGridDataInserted(data);
            this.CurrentDetailData["AutoCreate"] = 0;
        }

        /// <inheritdoc/>
        protected override void OnDetailGridRowChanged()
        {
            base.OnDetailGridRowChanged();
        }

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            this.ClickCheck();

            string updSql = string.Format("update ThreadRequisition set Status = 'Approved' ,editname='{0}', editdate = GETDATE() where orderid='{1}'", this.loginID, this.CurrentMaintain["Orderid"].ToString());

            DualResult upResult;
            TransactionScope transactionscope = new TransactionScope();
            using (transactionscope)
            {
                try
                {
                    if (!(upResult = DBProxy.Current.Execute(null, updSql)))
                    {
                        transactionscope.Dispose();
                        return;
                    }

                    transactionscope.Complete();
                    transactionscope.Dispose();
                    MyUtility.Msg.WarningBox("Successfully");
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }
            }

            transactionscope.Dispose();
            transactionscope = null;
        }

        /// <inheritdoc/>
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();

            string updSql = string.Format("update ThreadRequisition set Status = 'New' ,editname='{0}', editdate = GETDATE() where OrderID='{1}'", this.loginID, this.CurrentMaintain["OrderID"].ToString());

            DualResult upResult;
            TransactionScope transactionscope = new TransactionScope();
            using (transactionscope)
            {
                try
                {
                    if (!(upResult = DBProxy.Current.Execute(null, updSql)))
                    {
                        transactionscope.Dispose();
                        return;
                    }

                    transactionscope.Complete();
                    transactionscope.Dispose();
                    MyUtility.Msg.WarningBox("Successfully");
                }
                catch (Exception ex)
                {
                    transactionscope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }
            }

            transactionscope.Dispose();
            transactionscope = null;
        }

        private void ReQty(DataRow dr) // 重算Qty
        {
            dr["TotalQty"] = Convert.ToDecimal(dr["MeterToCone"]) != 0 ? Math.Ceiling(Convert.ToDecimal(dr["ConsumptionQty"]) / Convert.ToDecimal(dr["MeterToCone"])) : 0;
            string a = string.Format(
                        @"
select top 1 value = CEILING ({0} * Allowance)
from ThreadAllowanceScale tas
where tas.LowerBound <= {0}
and {0} <= tas.UpperBound",
                        this.CurrentDetailData["TotalQty"]);
            decimal allowance = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup(a));
            this.CurrentDetailData["AllowanceQty"] = allowance;
            dr["PurchaseQty"] = Convert.ToDecimal(dr["TotalQty"]) + Convert.ToDecimal(dr["AllowanceQty"]) - Convert.ToDecimal(dr["UseStockQty"]);
        }

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Refno"));
            dt.Columns.Add(new DataColumn("description"));
            dt.Columns.Add(new DataColumn("Article"));
            dt.Columns.Add(new DataColumn("ThreadColorid"));
            dt.Columns.Add(new DataColumn("Colordesc"));
            dt.Columns.Add(new DataColumn("MeterToCone"));
            dt.Columns.Add(new DataColumn("TotalQty"));
            dt.Columns.Add(new DataColumn("AllowanceQty"));
            dt.Columns.Add(new DataColumn("UseStockQty"));
            dt.Columns.Add(new DataColumn("PurchaseQty"));

            foreach (DataGridViewRow row in this.detailgrid.Rows)
            {
                DataRow dr = dt.NewRow();
                dr["Refno"] = row.Cells["Refno"].Value.ToString();
                dr["description"] = row.Cells["description"].Value.ToString();
                dr["Article"] = row.Cells["Article"].Value.ToString();
                dr["ThreadColorid"] = row.Cells["ThreadColorid"].Value.ToString();
                dr["Colordesc"] = row.Cells["Colordesc"].Value.ToString();
                dr["MeterToCone"] = row.Cells["MeterToCone"].Value.ToString();
                dr["TotalQty"] = row.Cells["TotalQty"].Value.ToString();
                dr["AllowanceQty"] = row.Cells["AllowanceQty"].Value.ToString();
                dr["UseStockQty"] = row.Cells["UseStockQty"].Value.ToString();
                dr["PurchaseQty"] = row.Cells["PurchaseQty"].Value.ToString();
                dt.Rows.Add(dr);
            }

            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + "\\Thread_P02.xltx"); // 預先開啟excel app
            bool result = MyUtility.Excel.CopyToXls(dt, string.Empty, showExcel: false, xltfile: "Thread_P02.xltx", headerRow: 7, excelApp: objApp);

            if (!result)
            {
                MyUtility.Msg.WarningBox(result.ToString(), "Warning");
            }
            else
            {
                Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
                string factoryname = string.Format(@"select  f.NameEN from Factory f where f.id = '{0}'", this.CurrentMaintain["FactoryID"]);
                string fN = this.CurrentMaintain["FactoryID"].ToString()+"-"+MyUtility.GetValue.Lookup(factoryname);
                objSheets.Cells[1, 1] = fN;

                objSheets.Cells[3, 2] = this.txtSP.Text.ToString();
                objSheets.Cells[3, 6] = this.dateSCIDelivery.Text.ToString();

                objSheets.Cells[4, 2] = this.displayBrand.Text.ToString();
                objSheets.Cells[4, 6] = this.dateSewingInLine.Text.ToString();

                objSheets.Cells[5, 2] = this.displayStyle.Text.ToString();
                objSheets.Cells[5, 6] = this.dateEstBooking.Text.ToString();

                objSheets.Cells[6, 2] = this.displaySeason.Text.ToString();
                objSheets.Cells[6, 6] = this.dateEstArrived.Text.ToString();

                objSheets.Columns.AutoFit();
                objSheets.Rows.AutoFit();

                Marshal.ReleaseComObject(objSheets);   // 釋放sheet
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

        private void ButtonQtyBreakdown_Click(object sender, EventArgs e)
        {
            new P02_QtyBreakdownByColorway(this.CurrentMaintain).ShowDialog();
        }

        private void BtnBatchapprove_Click(object sender, EventArgs e)
        {
            var frm = new Sci.Production.Thread.P02_BatchApprove();
            frm.ShowDialog(this);
            this.RenewData();
        }
    }
}
