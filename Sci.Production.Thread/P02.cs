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
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using Ict.Win.UI;
using Sci.Production.PublicPrg;
using System.Linq;

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
            this.InitializeComponent();
            this.DoSubForm = new P02_Detail();
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
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["OrderID"].ToString();
            this.DetailSelectCommand = string.Format(
                @"
SELECT  a.OrderID
        , a.Refno
        , a.ThreadColorID
        , a.ConsumptionQty
        , a.TotalQty
        , a.AllowanceQty
        , a.UseStockQty
        , a.PurchaseQty
        , a.PoId
        , a.Remark
        , a.Ukey
        , a.AutoCreate
        , [UseStockNewConeQty] = isnull(a.UseStockNewConeQty,0)
        , [UseStockUseConeQty]  = isnull(a.UseStockUseConeQty,0)
        , b.description
        , b.MetertoCone 
        , c.description as colordesc
        , [CurNewCone] = X.newCone
        , [CurUsedCone] = X.usedcone
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
	select isnull(sum(d.newCone),0) as newCone,isnull(sum(usedcone),0) as usedcone 
	from ThreadStock d WITH (NOLOCK) 
	INNER JOIN ThreadLocation tl ON d.ThreadLocationID=tl.ID
	where d.refno = a.refno and d.Threadcolorid = a.threadcolorid and d.Threadcolorid = a.threadcolorid AND tl.AllowAutoAllocate=1
) X
WHERE a.OrderID = '{0}'",
                masterID);

            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override DualResult OnSubDetailSelectCommandPrepare(PrepareSubDetailSelectCommandEventArgs e)
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

               string sql = string.Format("Select isnull(sum(d.newCone),0) as newCone,isnull(sum(d.usedCone),0) as usedCone from ThreadStock d WITH (NOLOCK) INNER JOIN ThreadLocation tl ON d.ThreadLocationID = Tl.ID where d.refno ='{0}' and d.threadcolorid = '{1}' AND tl.AllowAutoAllocate=1", newvalue, this.CurrentDetailData["ThreadColorid"].ToString());
               if (MyUtility.Check.Seek(sql, out refdr))
               {
                   this.CurrentDetailData["CurNewCone"] = refdr["NewCone"];
                   this.CurrentDetailData["CurUsedCone"] = refdr["UsedCone"];
               }
               else
               {
                   this.CurrentDetailData["CurNewCone"] = 0;
                   this.CurrentDetailData["CurUsedCone"] = 0;
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

                string sql = string.Format("Select isnull(sum(d.newCone),0) as newCone,isnull(sum(d.usedCone),0) as usedCone from ThreadStock d WITH (NOLOCK) INNER JOIN ThreadLocation tl ON d.ThreadLocationID = Tl.ID  where d.refno ='{0}' and d.threadcolorid = '{1}' AND tl.AllowAutoAllocate=1 ", this.CurrentDetailData["Refno"].ToString(), newvalue);
                if (MyUtility.Check.Seek(sql, out refdr))
                {
                    this.CurrentDetailData["CurNewCone"] = refdr["NewCone"];
                    this.CurrentDetailData["CurUsedCone"] = refdr["UsedCone"];
                }
                else
                {
                    this.CurrentDetailData["CurNewCone"] = 0;
                    this.CurrentDetailData["CurUsedCone"] = 0;
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

                            Win.Tools.SelectItem item = new Win.Tools.SelectItem("select ID,Description from ThreadColor WITH (NOLOCK) where junk = 0 order by ID", "10,40", this.CurrentDetailData["ThreadColorid"].ToString().Trim());
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
           .Numeric("CurNewCone", header: "Current Stock\r\nNew Cone", width: Ict.Win.Widths.AnsiChars(2), integer_places: 6, iseditingreadonly: true)
           .Numeric("CurUsedCone", header: "Current Stock\r\nUse Cone", width: Ict.Win.Widths.AnsiChars(2), integer_places: 6, iseditingreadonly: true)
           .Numeric("UseStockNewConeQty", header: "Use Stock\r\nNew Cone", width: Ict.Win.Widths.AnsiChars(2), integer_places: 6).Get(out this.col_NewCone)
           .Numeric("UseStockUseConeQty", header: "Use Stock\r\nUse Cone", width: Ict.Win.Widths.AnsiChars(2), integer_places: 6).Get(out this.col_UsedCone)
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
            int nc = Convert.ToInt32(this.CurrentDetailData["UseStockNewConeQty"]);
            int uc = Convert.ToInt32(this.CurrentDetailData["UseStockUseConeQty"]);
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

            // 如果detail資料POID有值，不能delete
            if (this.DetailDatas.Where(s => !MyUtility.Check.Empty(s["POID"])).Any())
            {
                MyUtility.Msg.WarningBox("Detail data <PO ID> has value, can't be deleted.", "Warning");
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

            foreach (DataRow drs in ((DataTable)this.detailgridbs.DataSource).Rows)
            {
                if (MyUtility.Convert.GetDecimal(drs["UseStockNewConeQty"]) > MyUtility.Convert.GetDecimal(drs["CurNewCone"]))
                {
                    MyUtility.Msg.WarningBox($"<Use Stock New Cone>{drs["UseStockNewConeQty"]} can't be more than <Current Stock New Cone>{drs["CurNewCone"]}");
                    return false;
                }

                if (MyUtility.Convert.GetDecimal(drs["useStockUseConeQty"]) > MyUtility.Convert.GetDecimal(drs["CurUsedCone"]))
                {
                    MyUtility.Msg.WarningBox($"<Use Stock Use Cone>{drs["useStockUseConeQty"]} can't be more than <Current Stock Use Cone>{drs["CurUsedCone"]}");
                    return false;
                }

                if (MyUtility.Convert.GetInt(drs["AllowanceQty"]) > 50 && MyUtility.Check.Empty(drs["Poid"]))
                {
                    MyUtility.Msg.WarningBox("<Allowance> must less than or equal to 50!");
                    return false;
                }
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override DualResult ClickSave()
        {
            var result = base.ClickSave();
            string msg = result.ToString().ToUpper();
            if (msg.Contains("PK") && msg.Contains("DUPLICAT"))
            {
                result = Result.F("<OrderID> duplicated", result.GetException());
            }

            return result;
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
                if (item is Win.UI.Label || item is Win.UI.Button || item == this.displayM || item == this.txtSP)
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
            if (!MyUtility.Check.Seek(string.Format("Select 1 from orders WITH (NOLOCK) inner join Factory on orders.FactoryID=Factory.ID where orders.id='{0}' and orders.FtyGroup = '{1}' and Factory.IsProduceFty=1", id, this.factory)))
            {
                e.Cancel = true;
                this.txtSP.Text = string.Empty;
                MyUtility.Msg.WarningBox(string.Format("<SP#: {0} >Data not found!!!!", id));
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
        , Allowance = isnull(d.Allowance,0)
        , a.Article
        , a.ThreadCombId
        , b.OperationId
        , SeamLength = isnull( e.SeamLength * b.Frequency,0)
        , a.Seq
        , a.ThreadLocationId
        , a.Refno
        , UseRatio = isnull(d.UseRatio,0)
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
            var query = from q in pretb_cons.AsEnumerable()
                        .Where(w => w.Field<decimal>("MeterToCone") == 0)
                        select q;

            foreach (var q in query)
            {
                warningmsg.Append(string.Format("Thread Refno:{0} <No. of Meters Per Cones> is 0 can't calculate <No. of Cones>", q.Field<string>("Refno")) + Environment.NewLine);
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
       , PurchaseQty = isnull(TotalQty,0) + isnull (AllowanceQty.value, 0)
        , [CurNewCone] = X.newCone
        , [CurUsedCone] = X.usedcone
from (
    select  Orderid = '{0}'
            , #tmp.Refno
            , ThreadColorId
            , Threadcombdesc
            , colordesc
            , #tmp.MeterToCone
            , ConsumptionQty = CEILING(Sum(OrderQty * (Seamlength * UseRatio + Allowance)) / 100)
            , TotalQty = IIF(#tmp.MeterToCone > 0, CEILING (Sum (OrderQty * (Seamlength * UseRatio + Allowance)) / 100 / #tmp.MeterToCone)
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
) AllowanceQty
OUTER APPLY
(
	select isnull(sum(d.newCone),0) as newCone,isnull(sum(d.usedcone),0) as usedcone 
    from ThreadStock d WITH (NOLOCK) 
    INNER JOIN ThreadLocation tl ON d.ThreadLocationID=Tl.ID
	where d.refno = tmp.refno and d.Threadcolorid = tmp.threadcolorid AND tl.AllowAutoAllocate=1
) X",
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
                newdr["CurNewCone"] = dr["CurNewCone"];
                newdr["CurUsedCone"] = dr["CurUsedCone"];
                newdr["UseStockQty"] = 0;
                newdr["Article"] = dr["Article"];

                #region 計算預設帶出使用量
                decimal purchaseQty = (decimal)newdr["PurchaseQty"];

                if ((decimal)newdr["CurUsedCone"] == 0)
                {
                    newdr["UseStockUseConeQty"] = 0;
                }
                else
                {
                    if ((decimal)newdr["CurUsedCone"] > purchaseQty)
                    {
                        newdr["UseStockUseConeQty"] = purchaseQty;
                        purchaseQty = 0;
                    }
                    else
                    {
                        newdr["UseStockUseConeQty"] = newdr["CurUsedCone"];
                        purchaseQty = purchaseQty - (decimal)newdr["CurUsedCone"];
                    }
                }

                if ((decimal)newdr["CurNewCone"] == 0)
                {
                    newdr["UseStockNewConeQty"] = 0;
                }
                else
                {
                    if ((decimal)newdr["CurNewCone"] > purchaseQty)
                    {
                        newdr["UseStockNewConeQty"] = purchaseQty;
                        purchaseQty = 0;
                    }
                    else
                    {
                        newdr["UseStockNewConeQty"] = newdr["CurNewCone"];
                        purchaseQty = purchaseQty - (decimal)newdr["CurNewCone"];
                    }
                }

                newdr["UseStockQty"] = (decimal)newdr["UseStockUseConeQty"] + (decimal)newdr["UseStockNewConeQty"];
                newdr["PurchaseQty"] = purchaseQty;

                #endregion
                detailtb.Rows.Add(newdr);
            }

            DataTable subtb;
            foreach (DataRow dr in detailtb.Rows)
            {
                var queryS = from p in pretb_cons.AsEnumerable()
                             .Where(p => p.Field<string>("Refno") == dr["Refno"].ToString() && p.Field<string>("threadColorid") == dr["threadColorid"].ToString())
                             select p;

                this.GetSubDetailDatas(dr, out subtb);
                if (subtb.Columns.Contains("Allowance") == false)
                {
                    subtb.Columns.Add("Allowance");
                }
                #region 新增第三層
                foreach (var item in queryS)
                {
                    decimal sL = Convert.ToDecimal(item["SeamLength"]);
                    decimal uRN = Convert.ToDecimal(item["UseRatio"]);
                    decimal aQ = Convert.ToDecimal(item["Allowance"]);
                    decimal oQ = Convert.ToDecimal(item["OrderQty"]);

                    DataRow newdr = subtb.NewRow();
                    newdr["Orderid"] = id;
                    newdr["Article"] = item["Article"];
                    newdr["ThreadCombID"] = item["ThreadCombId"];
                    newdr["Threadcombdesc"] = item["Threadcombdesc"];
                    newdr["operationid"] = item["operationid"];
                    newdr["SeamLength"] = item["SeamLength"];
                    newdr["SEQ"] = item["SEQ"];
                    newdr["ThreadLocationid"] = item["ThreadLocationid"];
                    newdr["UseRatio"] = item["UseRatio"];
                    newdr["UseLength"] = (sL * uRN) + aQ;
                    newdr["TotalLength"] = ((sL * uRN) + aQ) * oQ;
                    newdr["Machinetypeid"] = item["Machinetypeid"];
                    newdr["OrderQty"] = item["OrderQty"];
                    newdr["Allowance"] = item["Allowance"];
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

            var checkStock = this.DetailDatas
                            .Where(s => (decimal)s["UseStockNewConeQty"] > 0 || (decimal)s["UseStockUseConeQty"] > 0);
            DataRow checkDr;
            string checkSql = string.Empty;
            foreach (var item in checkStock)
            {
                checkSql = $@"select isnull(sum(d.newCone),0) as newCone,isnull(sum(usedcone),0) as usedcone 
    from ThreadStock d WITH(NOLOCK)
    INNER JOIN ThreadLocation tl
    ON d.ThreadLocationID=tl.ID
    where tl.AllowAutoAllocate=1 AND d.refno = '{item["Refno"]}' and d.Threadcolorid = '{item["Threadcolorid"]}' ";
                MyUtility.Check.Seek(checkSql, out checkDr);
                if ((decimal)checkDr["newCone"] < (decimal)item["UseStockNewConeQty"] ||
                   (decimal)checkDr["usedcone"] < (decimal)item["UseStockUseConeQty"])
                {
                    MyUtility.Msg.WarningBox($"<Thread Refno>{item["Refno"]},<Thread Color>{item["Threadcolorid"]} stock not enough");
                    return;
                }
            }

            #region 產生檢查用的issue detail data table
            string issueCheck = $@"
Create Table #ThreadIssue_Detail(
    [Refno] [varchar](21) NOT NULL,
	[ThreadColorID] [varchar](15) NOT NULL,
	[NewCone] [numeric](5, 0) NULL,
	[UsedCone] [numeric](5, 0) NULL,
	[ThreadLocationID] [varchar](10) NOT NULL
)

DECLARE ThreadRequisition_Detail_cur CURSOR FOR 
     select Refno,ThreadColorID,UseStockNewConeQty,UseStockUseConeQty
        from dbo.ThreadRequisition_Detail with (nolock)
        where OrderID = '{this.CurrentMaintain["Orderid"].ToString()}' and (UseStockNewConeQty > 0 or UseStockUseConeQty > 0)

declare @Refno varchar(24)
declare @ThreadColorID varchar(15)
declare @UseStockNewConeQty numeric(6,0)
declare @UseStockUseConeQty numeric(6,0)
declare @NewCone numeric(6,0)
declare @UsedCone numeric(6,0)
declare @ThreadLocationID varchar(10)

OPEN ThreadRequisition_Detail_cur --開始run cursor                   
FETCH NEXT FROM ThreadRequisition_Detail_cur INTO @Refno,@ThreadColorID,@UseStockNewConeQty,@UseStockUseConeQty
WHILE @@FETCH_STATUS = 0
BEGIN

	select ts.NewCone,ts.UsedCone,ts.ThreadLocationID
	into #Conetmp
	from dbo.ThreadStock ts with (nolock)
	INNER JOIN ThreadLocation tl ON ts.ThreadLocationID=tl.ID
	where ts.Refno = @Refno and ts.ThreadColorID = @ThreadColorID and (ts.NewCone > 0 or ts.UsedCone > 0) AND tl.AllowAutoAllocate=1
	
	--判斷有大於需求數量(@UseStockUseConeQty),若有使用最接近的
	select * into #tmpUsedCone from #Conetmp where UsedCone >= @UseStockUseConeQty and @UseStockUseConeQty > 0
	if exists(select 1 from #tmpUsedCone)
	begin
		select top 1 @UsedCone = iif(UsedCone>@UseStockUseConeQty,@UseStockUseConeQty,UsedCone), @ThreadLocationID = ThreadLocationID from #tmpUsedCone order by UsedCone
		insert into #ThreadIssue_Detail(Refno,ThreadColorID,NewCone,UsedCone,ThreadLocationID)	values( @Refno, @ThreadColorID, 0, @UsedCone, @ThreadLocationID)
		set @UseStockUseConeQty = 0
	end
	drop table #tmpUsedCone
	--判斷有大於需求數量(@UseStockNewConeQty),若有使用最接近的	
	select * into #tmpNewCone from #Conetmp where NewCone >= @UseStockNewConeQty and @UseStockNewConeQty > 0
	if exists(select 1 from #tmpNewCone)
	begin
		select top 1 @NewCone = iif(NewCone>@UseStockNewConeQty,@UseStockNewConeQty,NewCone), @ThreadLocationID = ThreadLocationID from #tmpNewCone order by UsedCone		
		insert into #ThreadIssue_Detail(Refno,ThreadColorID,NewCone,UsedCone,ThreadLocationID)	values( @Refno, @ThreadColorID, @NewCone, 0, @ThreadLocationID)
		set @UseStockNewConeQty = 0
	end
	drop table #tmpNewCone

	if @UseStockUseConeQty > 0
	begin
		DECLARE ThreadStock_cur CURSOR FOR 
		select UsedCone,ThreadLocationID from #Conetmp order by UsedCone desc--小於需求數量,從多的開始分配
		OPEN ThreadStock_cur --開始run cursor                   
		FETCH NEXT FROM ThreadStock_cur INTO @UsedCone,@ThreadLocationID
		WHILE @@FETCH_STATUS = 0
		BEGIN		
			if(@UsedCone > @UseStockUseConeQty)
			begin
				set @UsedCone = @UseStockUseConeQty
				set @UseStockUseConeQty = 0
			end
			else
			begin
				set @UseStockUseConeQty = @UseStockUseConeQty - @UsedCone
			end

			if(@UsedCone > 0)      
			begin
					insert into #ThreadIssue_Detail(Refno,ThreadColorID,NewCone,UsedCone,ThreadLocationID)
					values( @Refno, @ThreadColorID, 0, @UsedCone, @ThreadLocationID)
			end		

			if(@UseStockUseConeQty = 0)
			begin
				break
			end

		FETCH NEXT FROM ThreadStock_cur INTO @UsedCone,@ThreadLocationID
		END
		CLOSE ThreadStock_cur
		DEALLOCATE ThreadStock_cur
	end
	---------------------------------------------------------
	
	if  @UseStockNewConeQty > 0 
	begin
		DECLARE ThreadStock_cur CURSOR FOR 
		select NewCone,ThreadLocationID from #Conetmp order by NewCone desc --小於需求數量,從多的開始分配
		OPEN ThreadStock_cur --開始run cursor                   
		FETCH NEXT FROM ThreadStock_cur INTO @NewCone,@ThreadLocationID
		WHILE @@FETCH_STATUS = 0
		BEGIN
			if(@NewCone > @UseStockNewConeQty)
			begin
				set @NewCone = @UseStockNewConeQty
				set @UseStockNewConeQty = 0
			end
			else
			begin
				set @UseStockNewConeQty = @UseStockNewConeQty - @NewCone
			end

			if(@NewCone > 0)      
			begin
					insert into #ThreadIssue_Detail(Refno,ThreadColorID,NewCone,UsedCone,ThreadLocationID)
					values( @Refno, @ThreadColorID, @NewCone, 0, @ThreadLocationID)
			end		

			if(@UseStockNewConeQty = 0)
			begin
				break
			end

		FETCH NEXT FROM ThreadStock_cur INTO @NewCone,@ThreadLocationID
		END
		CLOSE ThreadStock_cur
		DEALLOCATE ThreadStock_cur
	end
	---------------------------------------------------------
	drop table #Conetmp
FETCH NEXT FROM ThreadRequisition_Detail_cur INTO @Refno,@ThreadColorID,@UseStockNewConeQty,@UseStockUseConeQty
END
CLOSE ThreadRequisition_Detail_cur
DEALLOCATE ThreadRequisition_Detail_cur


select * from #ThreadIssue_Detail
drop table #ThreadIssue_Detail
";
            DataTable issueCheckDt;
            DualResult result = DBProxy.Current.Select(null, issueCheck, out issueCheckDt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }
            #endregion

            string updSql = $@"update ThreadRequisition set Status = 'Approved' ,editname='{this.loginID}', editdate = GETDATE() where orderid='{this.CurrentMaintain["Orderid"].ToString()}'";

            if (issueCheckDt.Rows.Count > 0)
            {
                string issueID = MyUtility.GetValue.GetID(this.keyWord + "TS", "ThreadIssue", DateTime.Now);
                updSql += $@"
--建立P04 Thread issue
insert into ThreadIssue(ID,MDivisionId,CDate,Remark,Status,AddName,AddDate,RequestID)
    values('{issueID}','{Env.User.Keyword}',GETDATE(),'Auto Create By P02','Confirmed','{Env.User.UserID}',GETDATE(),'{this.CurrentMaintain["OrderID"]}')

Create Table #ThreadIssue_Detail(
	ID varchar(13) NOT NULL,
    [Refno] [varchar](21) NOT NULL,
	[ThreadColorID] [varchar](15) NOT NULL,
	[NewCone] [numeric](5, 0) NULL,
	[UsedCone] [numeric](5, 0) NULL,
	[ThreadLocationID] [varchar](10) NOT NULL
)

DECLARE ThreadRequisition_Detail_cur CURSOR FOR 
     select Refno,ThreadColorID,UseStockNewConeQty,UseStockUseConeQty
        from dbo.ThreadRequisition_Detail with (nolock)
        where OrderID = '{this.CurrentMaintain["Orderid"].ToString()}'  and (UseStockNewConeQty > 0 or UseStockUseConeQty > 0)

declare @Refno varchar(24)
declare @ThreadColorID varchar(15)
declare @UseStockNewConeQty numeric(6,0)
declare @UseStockUseConeQty numeric(6,0)
declare @NewCone numeric(6,0)
declare @UsedCone numeric(6,0)
declare @ThreadLocationID varchar(10)

OPEN ThreadRequisition_Detail_cur --開始run cursor                   
FETCH NEXT FROM ThreadRequisition_Detail_cur INTO @Refno,@ThreadColorID,@UseStockNewConeQty,@UseStockUseConeQty
WHILE @@FETCH_STATUS = 0
BEGIN
     
	select ts.NewCone,ts.UsedCone,ts.ThreadLocationID
	into #Conetmp
	from dbo.ThreadStock ts with (nolock)
	INNER JOIN ThreadLocation tl ON ts.ThreadLocationID=tl.ID
	where ts.Refno = @Refno and ts.ThreadColorID = @ThreadColorID and (ts.NewCone > 0 or ts.UsedCone > 0) AND tl.AllowAutoAllocate=1
	
	--判斷有大於需求數量(@UseStockUseConeQty),若有使用最接近的
	select * into #tmpUsedCone from #Conetmp where UsedCone >= @UseStockUseConeQty and @UseStockUseConeQty > 0
	if exists(select 1 from #tmpUsedCone)
	begin
		select top 1 @UsedCone = iif(UsedCone>@UseStockUseConeQty,@UseStockUseConeQty,UsedCone), @ThreadLocationID = ThreadLocationID from #tmpUsedCone order by UsedCone
		insert into #ThreadIssue_Detail(ID,Refno,ThreadColorID,NewCone,UsedCone,ThreadLocationID)	values('{issueID}',@Refno, @ThreadColorID, 0, @UsedCone, @ThreadLocationID)
		set @UseStockUseConeQty = 0
	end
	drop table #tmpUsedCone
	--判斷有大於需求數量(@UseStockNewConeQty),若有使用最接近的	
	select * into #tmpNewCone from #Conetmp where NewCone >= @UseStockNewConeQty and @UseStockNewConeQty > 0
	if exists(select 1 from #tmpNewCone)
	begin
		select top 1 @NewCone = iif(NewCone>@UseStockNewConeQty,@UseStockNewConeQty,NewCone), @ThreadLocationID = ThreadLocationID from #tmpNewCone order by UsedCone
		insert into #ThreadIssue_Detail(ID,Refno,ThreadColorID,NewCone,UsedCone,ThreadLocationID)	values('{issueID}', @Refno, @ThreadColorID, @NewCone, 0, @ThreadLocationID)
		set @UseStockNewConeQty = 0
	end
	drop table #tmpNewCone
	
	if @UseStockUseConeQty > 0
	begin
		DECLARE ThreadStock_cur CURSOR FOR 
		select UsedCone,ThreadLocationID from #Conetmp order by UsedCone desc--小於需求數量,從多的開始分配
		OPEN ThreadStock_cur --開始run cursor                   
		FETCH NEXT FROM ThreadStock_cur INTO @UsedCone,@ThreadLocationID
		WHILE @@FETCH_STATUS = 0
		BEGIN		
			if(@UsedCone > @UseStockUseConeQty)
			begin
				set @UsedCone = @UseStockUseConeQty
				set @UseStockUseConeQty = 0
			end
			else
			begin
				set @UseStockUseConeQty = @UseStockUseConeQty - @UsedCone
			end

			if(@UsedCone > 0)      
			begin
					insert into #ThreadIssue_Detail(ID, Refno,ThreadColorID,NewCone,UsedCone,ThreadLocationID)
					values('{issueID}', @Refno, @ThreadColorID, 0, @UsedCone, @ThreadLocationID)
			end

			if(@UseStockUseConeQty = 0)
			begin
				break
			end

		FETCH NEXT FROM ThreadStock_cur INTO @UsedCone,@ThreadLocationID
		END
		CLOSE ThreadStock_cur
		DEALLOCATE ThreadStock_cur
	end

	---------------------------------------------------------
	
	if  @UseStockNewConeQty > 0 
	begin
		DECLARE ThreadStock_cur CURSOR FOR 
		select NewCone,ThreadLocationID from #Conetmp order by NewCone desc --小於需求數量,從多的開始分配
		OPEN ThreadStock_cur --開始run cursor                   
		FETCH NEXT FROM ThreadStock_cur INTO @NewCone,@ThreadLocationID
		WHILE @@FETCH_STATUS = 0
		BEGIN
			if(@NewCone > @UseStockNewConeQty)
			begin
				set @NewCone = @UseStockNewConeQty
				set @UseStockNewConeQty = 0
			end
			else
			begin
				set @UseStockNewConeQty = @UseStockNewConeQty - @NewCone
			end

			if(@NewCone > 0)      
			begin
					insert into #ThreadIssue_Detail(ID, Refno,ThreadColorID,NewCone,UsedCone,ThreadLocationID)
					values('{issueID}', @Refno, @ThreadColorID, @NewCone, 0, @ThreadLocationID)
			end		

			if(@UseStockNewConeQty = 0)
			begin
				break
			end

		FETCH NEXT FROM ThreadStock_cur INTO @NewCone,@ThreadLocationID
		END
		CLOSE ThreadStock_cur
		DEALLOCATE ThreadStock_cur
	end
	drop table #Conetmp
FETCH NEXT FROM ThreadRequisition_Detail_cur INTO @Refno,@ThreadColorID,@UseStockNewConeQty,@UseStockUseConeQty
END
CLOSE ThreadRequisition_Detail_cur
DEALLOCATE ThreadRequisition_Detail_cur
insert into ThreadIssue_Detail(ID, Refno,ThreadColorID,NewCone,UsedCone,ThreadLocationID)
select ID,Refno,ThreadColorID,NewCone = sum(NewCone),UsedCone = sum(UsedCone),ThreadLocationID from #ThreadIssue_Detail group by  ID,Refno,ThreadColorID,ThreadLocationID
drop table #ThreadIssue_Detail
";
            }

            result = Prgs.ThreadIssueConfirm(issueCheckDt.ToList(), updSql, false);
            if (!result)
            {
                this.ShowErr(result);
            }
        }

        /// <inheritdoc/>
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();

            DataTable issueDetailDt;
            string getIssueDetail = $@"
select td.* 
from dbo.ThreadIssue t with (nolock)
inner join dbo.ThreadIssue_Detail td with (nolock) on t.id = td.id
where t.RequestID = '{this.CurrentMaintain["OrderID"]}'";
            DualResult result = DBProxy.Current.Select(null, getIssueDetail, out issueDetailDt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            string updSql = $@"
update ThreadRequisition set Status = 'New' ,editname='{this.loginID}', editdate = GETDATE() where OrderID='{this.CurrentMaintain["OrderID"]}'

delete ThreadIssue_Detail where ID = (select ID from ThreadIssue where RequestID = '{this.CurrentMaintain["OrderID"]}')

delete ThreadIssue where RequestID = '{this.CurrentMaintain["OrderID"]}'
";

            result = Prgs.ThreadIssueUnConfirm(issueDetailDt.ToList(), updSql);
            if (!result)
            {
                this.ShowErr(result);
            }
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
                string fN = this.CurrentMaintain["FactoryID"].ToString() + "-" + MyUtility.GetValue.Lookup(factoryname);
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
    }
}
