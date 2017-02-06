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
                if (!MyUtility.Check.Empty(dr["SciDelivery"]))
                dateBox3.Value = Convert.ToDateTime(dr["SciDelivery"]);
                else dateBox3.Text = "";
                if (!MyUtility.Check.Empty(dr["SewInLine"]))
                dateBox4.Value = Convert.ToDateTime(dr["SewInLine"]);
                else dateBox4.Text = "";
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
            @"SELECT a.*, b.description, b.MetertoCone ,c.description as colordesc,isnull(d.newCone,0) as newCone,isnull(d.usedcone,0) as usedcone
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
                if (MyUtility.Check.Seek(string.Format("Select * from ThreadColor where id = '{0}' and junk = 0", newvalue), out refdr))
                {
                    CurrentDetailData["ThreadColorid"] = refdr["ID"];
                    CurrentDetailData["Colordesc"] = refdr["Description"];
                }
                else
                {
                    CurrentDetailData["ThreadColorid"] = "";
                    CurrentDetailData["Colordesc"] = "";
                    MyUtility.Msg.WarningBox(string.Format("< Thread Color : {0} > not found!!!", e.FormattedValue.ToString()));
                    return;                   
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
                            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem("select ID,Description from ThreadColor where junk = 0 order by ID", "10,40", CurrentDetailData["ThreadColorid"].ToString().Trim());
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
            poqty2.CellValidating += (s, e) =>
            {
                /*int tq = Convert.ToInt32(CurrentDetailData["TotalQty"]);
                int aq = Convert.ToInt32(e.FormattedValue);
                int usq = Convert.ToInt32(CurrentDetailData["UseStockQty"]);
                if (!this.EditMode) return;
                CurrentDetailData["AllowanceQty"] = aq;
                CurrentDetailData["PurchaseQty"] = tq + aq - usq;
                CurrentDetailData.EndEdit();*/
            };
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
           .Text("Refno", header: "Thread Refno", width: Widths.AnsiChars(10), settings: refno).Get(out col_refno)
           .Text("description", header: "Thread Desc", width: Widths.AnsiChars(18), iseditingreadonly: true)
           .Text("ThreadColorid", header: "Thread\r\nColor", width: Widths.AnsiChars(4), settings: thcolor).Get(out col_color)
           .Text("Colordesc", header: "Thread Color Desc", width: Widths.AnsiChars(18), iseditingreadonly: true)
           .Numeric("ConsumptionQty", header: "Total\r\nCons.(M)", width: Widths.AnsiChars(2), integer_places: 6, settings: cons).Get(out col_cons)
           .Numeric("MeterToCone", header: "No. of Meters\r\nPer Cons", width: Widths.AnsiChars(6), integer_places: 7, decimal_places: 1, iseditingreadonly: true)
           .Numeric("TotalQty", header: "No. of\r\nCones", width: Widths.AnsiChars(2), integer_places: 6, iseditingreadonly: true, settings: poqty1)

           .Numeric("AllowanceQty", header: "20%\r\nallowance", width: Widths.AnsiChars(2), integer_places: 6, settings: poqty2).Get(out this.col_Allowance)
           .Numeric("NewCone", header: "New\r\nCone", width: Widths.AnsiChars(2), integer_places: 6, settings: NewCone).Get(out this.col_NewCone)
           .Numeric("UsedCone", header: "Use\r\nCone", width: Widths.AnsiChars(2), integer_places: 6, settings: UsedCone).Get(out this.col_UsedCone)

           .Numeric("UseStockQty", header: "Use\r\nStock", width: Widths.AnsiChars(2), integer_places: 6, iseditingreadonly: true, settings: poqty3)
           .Numeric("PurchaseQty", header: "PO Qty", width: Widths.AnsiChars(2), integer_places: 6, iseditingreadonly: true)
           .Text("Remark", header: "Remark", width: Widths.AnsiChars(10))
           .Text("POID", header: "PO ID", width: Widths.AnsiChars(12), iseditingreadonly: true);
            this.detailgrid.Columns["AllowanceQty"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["NewCone"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["UsedCone"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["Remark"].DefaultCellStyle.BackColor = Color.Pink;
            #endregion
            change_record();

            this.detailgrid.CellValidated += detailgrid_CellValidated;
            this.detailgrid.RowEnter+=detailgrid_RowEnter;

        }
        DataGridViewColumn col_Allowance;
        DataGridViewColumn col_NewCone;
        DataGridViewColumn col_UsedCone;
        private void detailgrid_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || EditMode == false) { return; }
            var data = ((DataRowView)this.detailgrid.Rows[e.RowIndex].DataBoundItem).Row;
            if (data == null) { return; }

            if (data["autoCreate"].ToString() == "True")
            {
                this.col_refno.IsEditingReadOnly = true;
                this.col_color.IsEditingReadOnly = true;
                this.col_cons.IsEditingReadOnly = true;
                this.refno.SupportPopup = false;
            }
            else
            {
                this.col_refno.IsEditingReadOnly = false;
                this.col_color.IsEditingReadOnly = false;
                this.col_cons.IsEditingReadOnly = false;
                this.refno.SupportPopup = true;
            }
        }
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
            CurrentDetailData["PurchaseQty"] = tq + aq - usq;
            this.detailgrid.InvalidateRow(RowIndex);
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
                    MyUtility.Msg.WarningBox("<PO Qty> can not be 0");
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
            if (textBox1.OldValue == textBox1.Text) return;
            string id = textBox1.Text;
            DataRow drOrder;
            DataTable pretb_cons, TR_DUK;
            string sqlpre;
            string sqltr_duk; 
            DataTable detailtb = (DataTable)detailgridbs.DataSource;

            foreach (Control item in masterpanel.Controls)
            {
                if (item is Sci.Win.UI.Label || item == displayBox4 || item == textBox1) { }
                else
                {
                    item.Text = "";                
                }
            }
            detailtb.Clear();

            if (textBox1.Text == "") return;
            //確認order.poid 同(po.id)有沒有這筆,沒有則return
            if (!MyUtility.Check.Seek(string.Format("Select * from PO where id='{0}'", id)))
            {
                MyUtility.Msg.WarningBox("This order is not purchase master!!!");
                e.Cancel = true;
                textBox1.Text = "";                
                return;
            }
            //確認ThreadRequisition有沒有這筆,有則return
            if (MyUtility.Check.Seek(string.Format("Select * from ThreadRequisition where OrderID='{0}'", id)))
            {
                MyUtility.Msg.WarningBox("Order number exists!!!");
                e.Cancel = true;
                textBox1.Text = "";
                return;
            }
            //輸入的POno帶出其他6個表頭
            if (!MyUtility.Check.Seek(string.Format("Select * from Orders where poid='{0}'", id), out drOrder)) return;
            dateBox3.Value = MyUtility.Convert.GetDate(drOrder["SciDelivery"]);
            dateBox4.Value = MyUtility.Convert.GetDate(drOrder["SewInLine"]);
            CurrentMaintain["Styleid"] = drOrder["Styleid"].ToString();
            CurrentMaintain["Seasonid"] = drOrder["Seasonid"].ToString();
            CurrentMaintain["Brandid"] = drOrder["Brandid"].ToString();
            CurrentMaintain["factoryid"] = drOrder["factoryid"].ToString();
            CurrentMaintain["OrderID"] = id;
            //事先整理資料
            sqlpre = string.Format(@"Select distinct   	a.ThreadColorId,  	d.Allowance,	a.Article,	a.ThreadCombId,
	                                b.OperationId,e.SeamLength,a.Seq, a.ThreadLocationId, a.Refno,d.UseRatioNumeric,
	                                a.MachineTypeId,  isnull(f.OrderQty,0) as OrderQty ,g.MeterToCone,
	                                g.Description as Threadcombdesc,h.Description as colordesc
	                                from ThreadColorComb_Detail a 
	                                cross join ThreadColorComb_Operation b
	                                left join ThreadColorcomb c on a.id=c.id 
	                                left join MachineType_ThreadRatio d on a.Machinetypeid=d.Id and a.seq=d.seq
	                                left join Operation e on b.OperationId= e.Id
	                                Left join (Select a.Article,sum(a.qty) as OrderQty 
                                               from Order_Qty a inner join orders b on a.id=b.id  where b.POID='{0}' group by Article) f 
                                              on a.Article=f.Article
	                                Left join LocalItem g on a.Refno=g.Refno
	                                Left join threadcolor h on h.id = a.threadcolorid
                                    
	                                where c.Styleukey =(select o.Styleukey from Orders o where o.id = '{0}')
	                                and a.ThreadColorId is not null and a.ThreadColorId !=''
	                                and a.Refno is not null and a.Refno !='' and Seamlength !=0
	                                and a.id=b.id
                                    and f.OrderQty !='0'",
                                    id);

            DualResult result;
            result = DBProxy.Current.Select(null, sqlpre, out pretb_cons);
            if (!result) { this.ShowErr(result); return; }
            //做資料匯整select group 後填入ThreadRequisition_Detail
            sqltr_duk = string.Format(@"select '{0}' as Orderid, #tmp.Refno,  ThreadColorId, 
                        Threadcombdesc,colordesc,
                        #tmp.MeterToCone,
                        CEILING(Sum(OrderQty * (Seamlength * UseRatioNumeric + Allowance)) / 100) as ConsumptionQty,
                        CEILING(Sum(OrderQty * (Seamlength * UseRatioNumeric + Allowance)) / #tmp.MeterToCone) as TotalQty,
                        CEILING(CEILING(Sum(OrderQty * (Seamlength * UseRatioNumeric + Allowance)) / #tmp.MeterToCone) * 0.2) as AllowanceQty,
                        CEILING(Sum(OrderQty * (Seamlength * UseRatioNumeric + Allowance)) / #tmp.MeterToCone)+
                        CEILING(CEILING(Sum(OrderQty * (Seamlength * UseRatioNumeric + Allowance)) / #tmp.MeterToCone) * 0.2) as PurchaseQty,
                        'true' as AutoCreate , 0 as UseStockQty, '' as POID, '' as Remark
                        from #tmp
                        group by Threadcombdesc,colordesc,#tmp.Refno,#tmp.MeterToCone,ThreadColorId", id);
            
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
            //col_color.EditingControlShowing += (s, e) =>
            //{
            //    if (e.RowIndex == -1) return;
            //    DataRow dr = detailgrid.GetDataRow(e.RowIndex);
            //    if (dr["autoCreate"].ToString() == "True") e.Control.Enabled = false;
            //    else e.Control.Enabled = true;

            //};
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
                    //e.CellStyle.ForeColor = Color.Red;
                }
            };
            //col_cons.EditingControlShowing += (s, e) =>
            //{
            //    if (CurrentDetailData == null) return;
            //    if (e.RowIndex == -1) return;
            //    DataRow dr = detailgrid.GetDataRow(e.RowIndex);
            //    if (dr["autoCreate"].ToString() == "True") e.Control.Enabled = false;
            //    else e.Control.Enabled = true;
            //};
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
                    //e.CellStyle.ForeColor = Color.Red;
                }
            };
            //col_refno.EditingControlShowing += (s, e) =>
            //{
            //    if (e.RowIndex == -1) return;
            //    DataRow dr = detailgrid.GetDataRow(e.RowIndex);
            //    if (dr["autoCreate"].ToString() == "True") e.Control.Enabled = false;
            //    else e.Control.Enabled = true;
            //};
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
                    //e.CellStyle.ForeColor = Color.Red;

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
