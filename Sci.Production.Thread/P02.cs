using Ict;
using Ict.Win;
using Sci.Production.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Sci.Win;
using Sci.Data;
using System.Transactions;

namespace Sci.Production.Thread
{
    public partial class P02 : Sci.Win.Tems.Input8
    {
        private string factory = Sci.Env.User.Factory;
        private string loginID = Sci.Env.User.UserID;
        private string keyWord = Sci.Env.User.Keyword;
        Ict.Win.UI.DataGridViewTextBoxColumn col_refno;
        Ict.Win.UI.DataGridViewTextBoxColumn col_color;
        Ict.Win.UI.DataGridViewNumericBoxColumn col_cons;
        public P02(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            string defaultfilter = string.Format("MDivisionid = '{0}' ", keyWord);
            this.DefaultFilter = defaultfilter;
            InitializeComponent();
            DoSubForm = new P02_Detail();
        }
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            DataRow dr;
            if (MyUtility.Check.Seek(string.Format("Select * from orders where id='{0}'", CurrentMaintain["OrderID"].ToString()), out dr))
            {
                dateBox3.Value = Convert.ToDateTime(dr["SciDelivery"]);
                dateBox4.Value = Convert.ToDateTime(dr["SewInLine"]);
            }
            else
            {
                dateBox3.Text = "";
                dateBox4.Text = "";

            }
        }
        protected override DualResult OnDetailSelectCommandPrepare(Win.Tems.InputMasterDetail.PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["OrderID"].ToString();
            this.DetailSelectCommand = string.Format(
            @"SELECT a.*, b.description, b.MetertoCone ,c.description as colordesc,d.newCone,d.usedcone
            FROM ThreadRequisition_Detail a
            Left Join Localitem b on a.refno = b.refno
            Left join ThreadColor c on c.id = a.ThreadColorid
            Left join ThreadStock d on d.refno = a.refno and d.Threadcolorid = a.threadcolorid and d.mDivisionid = '{1}'
            WHERE a.OrderID = '{0}'", masterID,keyWord);

            return base.OnDetailSelectCommandPrepare(e);
        }
        protected override DualResult OnSubDetailSelectCommandPrepare(Win.Tems.Input8.PrepareSubDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Detail == null) ? "" : e.Detail["Ukey"].ToString();

            this.SubDetailSelectCommand = string.Format(
@"Select a.*, b.description as Threadcombdesc,c.name as ThreadLocation, isnull(seamlength * UseRatioNumeric,0) as uselength,
OrderQty,isnull(seamlength * UseRatioNumeric * OrderQty,0) as TotalLength
from ThreadRequisition_Detail_Cons a
Left join ThreadComb b on b.id = a.threadcombid
Left join Reason c on c.reasontypeid = 'ThreadLocation' and c.id = a.threadlocationid
where a.ThreadRequisition_DetailUkey = '{0}'", masterID);
            return base.OnSubDetailSelectCommandPrepare(e);
        } 
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            DataGridViewGeneratorTextColumnSettings refno = celllocalitem.GetGridCell("Thread");
            DataGridViewGeneratorTextColumnSettings thcolor = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorNumericColumnSettings cons = new DataGridViewGeneratorNumericColumnSettings();
            DataGridViewGeneratorNumericColumnSettings usestock = new DataGridViewGeneratorNumericColumnSettings();
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
                if (MyUtility.Check.Seek(string.Format("Select * from Localitem where refno = '{0}' and junk = 0", newvalue), out refdr))
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
                string sql = string.Format("Select newCone,usedCone from ThreadStock where refno ='{0}' and threadcolorid = '{1}' and mDivisionid ='{2}' ", newvalue, CurrentDetailData["ThreadColorid"].ToString(),keyWord);
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
                if (MyUtility.Check.Seek(string.Format("Select * from ThreadColor where id = '{0}' and junk = 0", newvalue), out refdr))
                {
                    CurrentDetailData["ThreadColorid"] = refdr["ID"];
                    CurrentDetailData["Colordesc"] = refdr["Description"];
                }
                else
                {
                    CurrentDetailData["ThreadColorid"] = "";
                    CurrentDetailData["Colordesc"] = "";
                    
                }
                string sql = string.Format("Select newCone,usedCone from ThreadStock where refno ='{0}' and threadcolorid = '{1}' and mDivisionid ='{2}' ", CurrentDetailData["Refno"].ToString(), newvalue,keyWord);
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
            };
            #endregion
            #region useStock Cell
            usestock.CellValidating += (s, e) =>
            {
                decimal oldvalue = (decimal)CurrentDetailData["UseStockQty"];
                decimal newvalue = (decimal)e.FormattedValue;
                if (!this.EditMode || oldvalue == newvalue) return;
                CurrentDetailData["PurchaseQty"] = (decimal)CurrentDetailData["TotalQty"] + (decimal)CurrentDetailData["AllowanceQty"] - newvalue;
                CurrentDetailData["UseStockQty"] = newvalue;
                CurrentDetailData.EndEdit();
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

            

            #region set grid
            Helper.Controls.Grid.Generator(this.detailgrid)
           .Text("Refno", header: "Thread Refno", width: Widths.AnsiChars(20),settings:refno).Get(out col_refno)
           .Text("description", header: "Thread Desc", width: Widths.AnsiChars(20), iseditingreadonly: true)
           .CellThreadColor("ThreadColorid", header: "Thread Color", width: Widths.AnsiChars(15),settings: thcolor ).Get(out col_color)
           .Text("Colordesc", header: "Thread Color Desc", width: Widths.AnsiChars(10), iseditingreadonly: true)
           .Numeric("ConsumptionQty", header: "Total Consumptions", width: Widths.AnsiChars(5), integer_places: 6,settings: cons).Get(out col_cons)
           .Numeric("MeterToCone", header: "No. of Meters Per Cons", width: Widths.AnsiChars(5), integer_places: 7, decimal_places: 1, iseditingreadonly: true)
           .Numeric("TotalQty", header: "No. of Cones", width: Widths.AnsiChars(5), integer_places: 6, iseditingreadonly: true)
           .Numeric("AllowanceQty", header: "20% allowance", width: Widths.AnsiChars(5), integer_places: 6, iseditingreadonly: true)
           .Numeric("NewCone", header: "New Cone", width: Widths.AnsiChars(5), integer_places: 6, iseditingreadonly: true)
           .Numeric("UsedCone", header: "Use Cone", width: Widths.AnsiChars(5), integer_places: 6, iseditingreadonly: true)
           .Numeric("UseStockQty", header: "Use Stock", width: Widths.AnsiChars(5), integer_places: 6, settings: usestock)
           .Numeric("PurchaseQty", header: "PO Qty", width: Widths.AnsiChars(5), integer_places: 6)
           .Text("Remark", header: "Remark", width: Widths.AnsiChars(10))
           .Text("POID", header: "PO ID", width: Widths.AnsiChars(10), iseditingreadonly: true);
            this.detailgrid.Columns[10].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns[11].DefaultCellStyle.BackColor = Color.Pink;
            #endregion
            change_record();
        }
        protected override bool ClickDeleteBefore()
        {
            if (CurrentMaintain["Status"].ToString() != "New")
            {
                MyUtility.Msg.WarningBox("The record already Confirmed, you can't delete.", "Warning");
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
                MyUtility.Msg.WarningBox("<Est. Booking> can not be empty.");
                dateBox1.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["EstArriveDate"]))
            {
                MyUtility.Msg.WarningBox("<Est. Arrived> can not be empty.");
                dateBox2.Focus();
                return false;
            }
            foreach (DataRow dr in DetailDatas)
            {
                if (MyUtility.Check.Empty(dr["PurchaseQty"]))
                {
                    MyUtility.Msg.WarningBox("<PurchaseQty> can not be 0");
                    return false;
                }
            }
            return base.ClickSaveBefore();
        }
        protected override bool ClickEditBefore()
        {
            if (CurrentMaintain["Status"].ToString() != "New")
            {
                MyUtility.Msg.WarningBox("The record already Confirmed, you can't modify", "Warning");
                return false;
            }

            return base.ClickEditBefore();
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            string id = textBox1.Text;
            DataRow drOrder;
            if (!MyUtility.Check.Seek(string.Format("Select * from PO where id='{0}'", id)))
            {
                MyUtility.Msg.WarningBox("<PO No> data not found");
                e.Cancel = true;
                return;
            }
            if (!MyUtility.Check.Seek(string.Format("Select * from Orders where id='{0}'", id), out drOrder)) return;
            dateBox3.Value = MyUtility.Convert.GetDate(drOrder["SciDelivery"]);
            dateBox4.Value = MyUtility.Convert.GetDate(drOrder["SewInLine"]);
            CurrentMaintain["Styleid"] = drOrder["Styleid"].ToString();
            CurrentMaintain["Seasonid"] = drOrder["Seasonid"].ToString();
            CurrentMaintain["Brandid"] = drOrder["Brandid"].ToString();
            CurrentMaintain["factoryid"] = drOrder["factoryid"].ToString();
            CurrentMaintain["OrderID"] = id;
            //用TimeStudy 的原因是因為Operationid有可能在FTY GSD被改變
            string lengthsql = string.Format(
@"with 
a as 
(Select a.*,b.refno,b.ThreadColorid,b.Article,b.ThreadLocationid,b.seq,c.UseRatio,c.UseRatioNumeric
from ThreadColorComb a,ThreadColorComb_Detail b,MachineType_ThreadRatio c      
Where a.id = b.id and a.styleukey = '{0}' and c.id = a.Machinetypeid and c.seq = b.seq and c.threadlocation = b.threadlocationid ),
b as 
(Select a.styleukey,b.operationid,d.machinetypeid,d.seamlength from ThreadColorComb a,ThreadColorComb_operation b, timestudy c,timestudy_Detail d
Where a.id = b.id and b.operationid = d.operationid and c.id = d.id and c.styleid = '{1}' and c.seasonid = '{2}' and c.brandid = '{3}')
Select a.*,b.operationid,b.seamlength from a,b where a.machinetypeid = b.machinetypeid
", drOrder["StyleUkey"].ToString(), drOrder["Styleid"].ToString(), drOrder["Seasonid"].ToString(), drOrder["Brandid"].ToString());
            DataTable lengthdt,totalqty,refartdt;
            DBProxy.Current.Select(null, lengthsql, out lengthdt); //找出此Style 所有的用線量

            string sql = string.Format(@"Select ThreadCombId,MachineTypeId,a.Article,StyleUkey,ID,Refno,ThreadColorid,SEQ,UseRatio,UseRatioNumeric,operationid,seamlength,ThreadLocationid,Qty from #tmp a join (Select a.article,sum(a.Qty) as Qty from Order_Qty a,Orders b where a.id = b.id and b.poid = '{0}' and b.junk = 0 group by article) b On a.article = b.article", id);
            MyUtility.Tool.ProcessWithDatatable(lengthdt, "ThreadCombId,MachineTypeId,Article,StyleUkey,ID,Refno,ThreadColorid,SEQ,UseRatio,UseRatioNumeric,operationid,seamlength,ThreadLocationid",
sql, out totalqty);

            MyUtility.Tool.ProcessWithDatatable(totalqty, "Refno,ThreadColorid", 
string.Format(@"Select distinct a.Refno,a.ThreadColorid,b.description,c.description as colordesc,b.MeterToCone ,isnull(newCone,0) as newCone, isnull(usedcone,0) as usedcone
from #tmp a 
left join localitem b on a.refno = b.refno 
left join threadcolor c on c.id = a.threadcolorid 
left join (Select refno,threadcolorid,isnull(sum(NewCone),0) as newcone,isnull(sum(UsedCone),0) as usedcone from ThreadStock where  mDivisionid = '{0}' group by refno ,ThreadColorid) as d on d.refno = a.refno and d.threadcolorid = a.threadcolorid 
",keyWord), out refartdt); //表身
            DataTable detailtb = (DataTable)detailgridbs.DataSource;
            foreach (DataRow dr in refartdt.Rows) //新增表身 by refno,ThreadColorid 增加
            {
                DataRow newdr = detailtb.NewRow();
                //newdr["OrderID"] = id;
                newdr["Refno"] = dr["Refno"];
                newdr["ThreadColorid"] = dr["ThreadColorid"];
                newdr["AutoCreate"] = 1;
                newdr["usestockqty"] = 0;
                newdr["Description"] = dr["Description"];
                newdr["colordesc"] = dr["colordesc"];
                newdr["MeterToCone"] = dr["MeterToCone"];
                newdr["NewCone"] = dr["NewCone"];
                newdr["UsedCone"] = dr["UsedCone"];
                newdr["ConsumptionQty"] = 0;
                newdr["TotalQty"] = 0;
                newdr["AllowanceQty"] = 0;
                newdr["UseStockQty"] = 0;
                newdr["PurchaseQty"] = 0;
                detailtb.Rows.Add(newdr);
            }
            DataTable subtb,sqltb;
            
            foreach (DataRow dr in detailtb.Rows)
            {
                sql = string.Format(@"Select ThreadCombId,MachineTypeId,ID,Refno,Article,ThreadLocationid,SEQ,UseRatio,isnull(UseRatioNumeric,0) as UseRatioNumeric,operationid,isnull(seamlength,0) as seamlength ,isnull(Qty ,0) as Qty
from #tmp a where a.refno = '{0}' and a.threadColorid='{1}'", dr["Refno"].ToString(), dr["threadColorid"].ToString());
                GetSubDetailDatas(dr, out subtb);
                MyUtility.Tool.ProcessWithDatatable(totalqty,
@"ThreadCombId,MachineTypeId,ID,Refno,Article,ThreadLocationid,SEQ,UseRatio,UseRatioNumeric,operationid,seamlength,Qty,ThreadColorid",
sql, out sqltb);
                decimal cons = 0;
                #region 新增第三層
                foreach (DataRow ddr in sqltb.Rows)
                {
                    DataRow newdr = subtb.NewRow();
                    newdr["Orderid"] = id;
                    newdr["Article"] = ddr["Article"];
                    newdr["ThreadLocationid"] = ddr["ThreadLocationid"];
                    newdr["SEQ"] = ddr["SEQ"];
                    newdr["UseRatio"] = ddr["UseRatio"];
                    newdr["UseRatioNumeric"] = ddr["UseRatioNumeric"];
                    newdr["operationid"] = ddr["operationid"];
                    newdr["SeamLength"] = ddr["SeamLength"];
                    newdr["Machinetypeid"] = ddr["Machinetypeid"];
                    newdr["ThreadCombID"] = ddr["ThreadCombId"];
                    newdr["OrderQty"] = ddr["Qty"];
                    subtb.Rows.Add(newdr);
                    cons = cons + Convert.ToDecimal(ddr["UseRatioNumeric"]) * Convert.ToDecimal(ddr["Qty"]) * Convert.ToDecimal(ddr["SeamLength"]);
                }
                #endregion
                //填入Cons
                dr["ConsumptionQty"] = cons;
                dr["TotalQty"] =Convert.ToDecimal(dr["MeterToCone"])!=0? Math.Ceiling(cons / Convert.ToDecimal(dr["MeterToCone"])):0;
                dr["AllowanceQty"] = Convert.ToDouble(dr["TotalQty"]) * 0.2;
                dr["PurchaseQty"] =Convert.ToDecimal(dr["TotalQty"]) + Convert.ToDecimal(dr["AllowanceQty"]) - Convert.ToDecimal(dr["UseStockQty"]);
            }
        }

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

            this.RenewData();
            this.OnDetailEntered();
            EnsureToolbarExt();
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

            this.RenewData();
            this.OnDetailEntered();
            EnsureToolbarExt();
        }
        #region 是否可編輯與變色
        private void change_record()
        {
                col_color.EditingControlShowing += (s, e) =>
                {
                    if (e.RowIndex == -1) return;
                    DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                    if (dr["autoCreate"].ToString() == "True") e.Control.Enabled = false;
                    else e.Control.Enabled = true;

                };
                col_color.CellFormatting += (s, e) =>
                {
                    if (e.RowIndex == -1) return;
                    DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                    if (dr["autoCreate"].ToString() == "True")
                    {
                        e.CellStyle.BackColor = Color.White;
                        e.CellStyle.ForeColor = Color.Black;
                    }
                    else
                    {
                        e.CellStyle.BackColor = Color.Pink;
                        e.CellStyle.ForeColor = Color.Red;
                    }
                };
                col_cons.EditingControlShowing += (s, e) =>
                {
                    if (CurrentDetailData == null) return;
                    if (e.RowIndex == -1) return;
                    DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                    if (dr["autoCreate"].ToString() == "True") e.Control.Enabled = false;
                    else e.Control.Enabled = true;
                };
                col_cons.CellFormatting += (s, e) =>
                {
                    if (e.RowIndex == -1) return;
                    DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                    if (dr["autoCreate"].ToString() == "True")
                    {
                        e.CellStyle.BackColor = Color.White;
                        e.CellStyle.ForeColor = Color.Black;

                    }
                    else
                    {
                        e.CellStyle.BackColor = Color.Pink;
                        e.CellStyle.ForeColor = Color.Red;
                    }
                };
                col_refno.EditingControlShowing += (s, e) =>
                {
                    if (e.RowIndex == -1) return;
                    DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                    if (dr["autoCreate"].ToString() == "True") e.Control.Enabled = false;
                    else e.Control.Enabled = true;
                };
                col_refno.CellFormatting += (s, e) =>
                {
                    if (e.RowIndex == -1) return;
                    DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                    if (dr["autoCreate"].ToString() == "True")
                    {
                        e.CellStyle.BackColor = Color.White;
                        e.CellStyle.ForeColor = Color.Black;
                    }
                    else
                    {
                        e.CellStyle.BackColor = Color.Pink;
                        e.CellStyle.ForeColor = Color.Red;

                    }
                };
            
        }
        #endregion
        private void ReQty(DataRow dr) //重算Qty
        {
            dr["TotalQty"] = Convert.ToDecimal(dr["MeterToCone"]) != 0 ? Math.Ceiling(Convert.ToDecimal(dr["ConsumptionQty"]) / Convert.ToDecimal(dr["MeterToCone"])) : 0;
            dr["AllowanceQty"] = Convert.ToDouble(dr["TotalQty"]) * 0.2;
            dr["PurchaseQty"] = Convert.ToDecimal(dr["TotalQty"]) + Convert.ToDecimal(dr["AllowanceQty"]) - Convert.ToDecimal(dr["UseStockQty"]);
        }
    }
}
