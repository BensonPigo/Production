﻿using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    public partial class P45 : Sci.Win.Tems.Input6
    {
        public P45(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            InsertDetailGridOnDoubleClick = false;
            this.DefaultFilter = string.Format("Type='R' and MDivisionID = '{0}'", Sci.Env.User.Keyword);
            gridicon.Append.Enabled = false;
            gridicon.Append.Visible = false;
            gridicon.Insert.Enabled = false;
            gridicon.Insert.Visible = false;
        }

        // 新增時預設資料
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            CurrentMaintain["Status"] = "New";
            CurrentMaintain["Type"] = "R";
            CurrentMaintain["IssueDate"] = DateTime.Now;
        }
        // edit前檢查
        protected override bool ClickEditBefore()
        {
            if (CurrentMaintain["Status"].EqualString("CONFIRMED"))
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't modify.", "Warning");
                return false;
            }
            return base.ClickEditBefore();
        }
        // save前檢查 & 取id
        protected override bool ClickSaveBefore()
        {
            //	檢查明細至少存在一筆資料。
            if (DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't be empty", "Warning");
                return false;
            }

            StringBuilder warningmsg = new StringBuilder();
            
            foreach (DataRow row in DetailDatas)
            {
                //	檢查所有明細資料的current qty 不可等於 original qty
                if (MyUtility.Convert.GetDecimal(row["QtyAfter"]) == MyUtility.Convert.GetDecimal(row["QtyBefore"]))
                {
                    warningmsg.Append(string.Format(@"SP#: {0} SEQ#: {1} Roll#: {2} Dyelot: {3}
Original Qty and Current Qty can’t be equal!!"
                        , row["poid"].ToString().Trim(), row["seq"].ToString().Trim(), row["Roll"].ToString().Trim(), row["Dyelot"].ToString().Trim()) 
                        + Environment.NewLine);
                }
                //	檢查所有明細資料都有填入reason
                if (MyUtility.Check.Empty(row["reasonid"]))
                {
                    warningmsg.Append(string.Format(@"SP#: {0} SEQ#: {1} Roll#: {2} Dyelot: {3}
Reason can’t be empty!!"
                       , row["poid"].ToString().Trim(), row["seq"].ToString().Trim(), row["Roll"].ToString().Trim(), row["Dyelot"].ToString().Trim()) 
                       + Environment.NewLine);
                }
            }
            //	全部檢查完再Message有問題的detail資料。	全部檢查完再Message沒填入reason的detail資料
            if (!MyUtility.Check.Empty(warningmsg.ToString()))
            {
                MyUtility.Msg.WarningBox(warningmsg.ToString());
                return false;
            }
            
            //取單號
            if (this.IsDetailInserting)
            {
                string tmpId = Sci.MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "AM", "Adjust", (DateTime)CurrentMaintain["Issuedate"], 2, "ID", null);
                if (MyUtility.Check.Empty(tmpId))
                {
                    MyUtility.Msg.WarningBox("Get document ID fail!!");
                    return false;
                }
                CurrentMaintain["id"] = tmpId;
            }
            return base.ClickSaveBefore();
        }
        // 刪除前檢查
        protected override bool ClickDeleteBefore()
        {
            if (CurrentMaintain["Status"].EqualString("CONFIRMED"))
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't delete.", "Warning");
                return false;
            }
            return base.ClickDeleteBefore();
        }
        //Confirm
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            if (null == CurrentMaintain) return;
            DataTable[] dts;
            string id = CurrentMaintain["ID"].ToString();
            DualResult res = DBProxy.Current.SelectSP("", "dbo.usp_RemoveScrapById", new List<SqlParameter> { new SqlParameter("@ID", id) }, out dts);
            if (!res)
            {
                MyUtility.Msg.ErrorBox(res.ToString(), "error");
                return ;
            }
            if (dts.Length < 1)
            {
                MyUtility.Msg.InfoBox("Confirmed Successful.");
                return;
            }
            else
            {
                StringBuilder warningmsg = new StringBuilder();
                foreach (DataRow drs in dts[0].Rows)
                {
                    if (MyUtility.Convert.GetDecimal(drs["q"]) < 0)
                    {
                        warningmsg.Append(string.Format(@"SP#: {0} SEQ#: {1} Roll#: {2} Dyelot: {3}'s balance: {4} is less than Adjust qty: {5}
Balacne Qty is not enough!!
", drs["POID"].ToString(), drs["seq"].ToString(), drs["Roll"].ToString(), drs["Dyelot"].ToString(), drs["balance"].ToString(), drs["Adjustqty"].ToString()));
                    }
                }
                if (!MyUtility.Check.Empty(warningmsg.ToString()))
                {
                    MyUtility.Msg.WarningBox(warningmsg.ToString());
                    return;
                }
            }
            //            #region 	依SP#+SEQ#+Roll#+ StockType = 'O' 檢查庫存是否足夠
            //            string sql = string.Format(@"
            //from dbo.Adjust_Detail d WITH (NOLOCK) 
            //inner join FtyInventory f WITH (NOLOCK) on d.POID = f.POID and d.Roll = f.Roll and d.Seq1 =f.Seq1 and d.Seq2 = f.Seq2
            //where d.Id = '{0}'
            //and f.StockType = 'O'", CurrentMaintain["id"]);

            //            DataTable dt;            
            //            DualResult dr;
            //            string chksql = string.Format(@"
            //Select d.POID,seq = concat(d.Seq1,'-',d.Seq2),d.Roll,d.Dyelot
            //	,balance = isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0)
            //	,Adjustqty  = isnull(d.QtyBefore,0) - isnull(d.QtyAfter,0)
            //	,q = isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) +(isnull(d.QtyAfter,0)-isnull(d.QtyBefore,0))
            //{0}", sql);
            //            if (!(dr = DBProxy.Current.Select(null, chksql, out dt)))
            //            {
            //                MyUtility.Msg.WarningBox("Update datas error!!");
            //                return;
            //            }
            //            StringBuilder warningmsg = new StringBuilder();
            //            foreach (DataRow drs in dt.Rows)
            //            {
            //                if (MyUtility.Convert.GetInt(drs["q"])<0)
            //                {
            //                    warningmsg.Append(string.Format(@"SP#: {0} SEQ#: {1} Roll#: {2} Dyelot: {3}'s balance: {4} is less than Adjust qty: {5}
            //Balacne Qty is not enough!!
            //", drs["POID"].ToString(), drs["seq"].ToString(), drs["Roll"].ToString(), drs["Dyelot"].ToString(), drs["balance"].ToString(), drs["Adjustqty"].ToString()));
            //                }
            //            }
            //            if (!MyUtility.Check.Empty(warningmsg.ToString()))
            //            {
            //                MyUtility.Msg.WarningBox(warningmsg.ToString());
            //                return;
            //            }
            //            string upcmd = string.Format(@"
            //Update f set f.AdjustQty = f.AdjustQty +(d.QtyAfter- d.QtyBefore) 
            //{0}

            //update m2 set 
            //LObQty = b.l
            //from(
            //	select l=sum(a.InQty-a.OutQty+a.AdjustQty),POID,Seq1,Seq2
            //	from
            //	(
            //		select f.InQty,f.OutQty,f.AdjustQty,d.POID,d.Seq1,d.Seq2
            //		from dbo.Adjust_Detail d WITH (NOLOCK) 
            //		inner join FtyInventory f WITH (NOLOCK) on d.POID = f.POID and d.Seq1 =f.Seq1 and d.Seq2 = f.Seq2 and d.StockType = f.StockType
            //		inner join MDivisionPoDetail m WITH (NOLOCK) on m.POID = d.POID and m.Seq1 = d.Seq1 and m.Seq2 = d.Seq2
            //		where d.Id = '{1}'
            //		and f.StockType = 'O'
            //		group by d.POID,d.Seq1,d.Seq2,f.InQty,f.OutQty,f.AdjustQty
            //	)a
            //	group by POID,Seq1,Seq2
            //)b
            //,MDivisionPoDetail m2
            //where m2.POID = b.POID and m2.Seq1 = b.Seq1 and m2.Seq2 = b.Seq2

            //update Adjust set Status ='Confirmed' where id = '{1}'
            //", sql, CurrentMaintain["id"]);
            //            if (!(dr = DBProxy.Current.Execute(null, upcmd)))
            //            {
            //                MyUtility.Msg.WarningBox("Update datas error!!");
            //                return;
            //            }
            //            #endregion 
        }
        //Confirm
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            if (null == CurrentMaintain) return;
            #region 	依SP#+SEQ#+Roll#+ StockType = 'O' 檢查庫存是否足夠
            string sql = string.Format(@"
from dbo.Adjust_Detail d WITH (NOLOCK) 
inner join FtyInventory f WITH (NOLOCK) on d.POID = f.POID and d.Roll = f.Roll and d.Seq1 =f.Seq1 and d.Seq2 = f.Seq2
where d.Id = '{0}'
and f.StockType = 'O'", CurrentMaintain["id"]);

            DataTable dt;
            DualResult dr;
            string chksql = string.Format(@"
Select d.POID,seq = concat(d.Seq1,'-',d.Seq2),d.Roll,d.Dyelot
	,balance = isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0)
	,Adjustqty  = isnull(d.QtyBefore,0) - isnull(d.QtyAfter,0)
	,q = isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) -(isnull(d.QtyAfter,0)-isnull(d.QtyBefore,0))
{0}", sql);
            if (!(dr = DBProxy.Current.Select(null, chksql, out dt)))
            {
                MyUtility.Msg.WarningBox("Update datas error!!");
                return;
            }
            StringBuilder warningmsg = new StringBuilder();
            foreach (DataRow drs in dt.Rows)
            {
                if (MyUtility.Convert.GetInt(drs["q"]) < 0)
                {
                    warningmsg.Append(string.Format(@"SP#: {0} SEQ#: {1} Roll#: {2} Dyelot: {3}'s balance: {4} is less than Adjust qty: {5}
Balacne Qty is not enough!!
", drs["POID"].ToString(), drs["seq"].ToString(), drs["Roll"].ToString(), drs["Dyelot"].ToString(), drs["balance"].ToString(), drs["Adjustqty"].ToString()));
                }
            }
            if (!MyUtility.Check.Empty(warningmsg.ToString()))
            {
                MyUtility.Msg.WarningBox(warningmsg.ToString());
                return;
            }
            string upcmd = string.Format(@"
Update f set f.AdjustQty = f.AdjustQty -(d.QtyAfter- d.QtyBefore) 
{0}

update m2 set 
LObQty = b.l
from(
	select l=sum(a.InQty-a.OutQty-a.AdjustQty),POID,Seq1,Seq2
	from
	(
		select f.InQty,f.OutQty,f.AdjustQty,d.POID,d.Seq1,d.Seq2
		from dbo.Adjust_Detail d WITH (NOLOCK) 
		inner join FtyInventory f WITH (NOLOCK) on d.POID = f.POID and d.Seq1 =f.Seq1 and d.Seq2 = f.Seq2 and d.StockType = f.StockType
		inner join MDivisionPoDetail m WITH (NOLOCK) on m.POID = d.POID and m.Seq1 = d.Seq1 and m.Seq2 = d.Seq2
		where d.Id = '{1}'
		and f.StockType = 'O'
		group by d.POID,d.Seq1,d.Seq2,f.InQty,f.OutQty,f.AdjustQty
	)a
	group by POID,Seq1,Seq2
)b
,MDivisionPoDetail m2
where m2.POID = b.POID and m2.Seq1 = b.Seq1 and m2.Seq2 = b.Seq2

update Adjust set Status ='New' where id = '{1}'
", sql, CurrentMaintain["id"]);
            if (!(dr = DBProxy.Current.Execute(null, upcmd)))
            {
                MyUtility.Msg.WarningBox("Update datas error!!");
                return;
            }
            #endregion 
        }
        //refresh
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            #region Status Label

            label25.Text = CurrentMaintain["status"].ToString();

            #endregion Status Label
        }
        //表身資料SQL Command
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            this.DetailSelectCommand = string.Format(@"
select 
	ad.POID
	,seq = concat(ad.Seq1,'-',ad.Seq2)
	,ad.Roll
	,ad.Dyelot
	,Description = f.Description
	,ad.QtyBefore
	,ad.QtyAfter
	,adjustqty= ad.QtyBefore-ad.QtyAfter
	,psd.StockUnit
	,Location = dbo.Getlocation(fi.ukey)
	,ad.ReasonId
	,reason_nm = (select Name FROM Reason WHERE id=ReasonId AND junk = 0 and ReasonTypeID='Stock_Remove')
from Adjust_detail ad WITH (NOLOCK) 
left join PO_Supp_Detail psd WITH (NOLOCK) on psd.id = ad.POID and psd.SEQ1 = ad.Seq1 and psd.SEQ2 = ad.Seq2
left join Fabric f WITH (NOLOCK) on f.SCIRefno = psd.SCIRefno
left join FtyInventory fi WITH (NOLOCK) on fi.POID = ad.POID and fi.Seq1 = ad.Seq1 and fi.Seq2 = ad.Seq2 and fi.Roll = ad.Roll and fi.StockType = ad.StockType 
where ad.Id='{0}' 
", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }
        //表身資料設定
        protected override void OnDetailGridSetup()
        {
            #region -- Current Qty Vaild 判斷 --
            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
            {
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (this.EditMode && (MyUtility.Convert.GetDecimal(CurrentDetailData["QtyBefore"]) == MyUtility.Convert.GetDecimal(e.FormattedValue)))
                {
                    MyUtility.Msg.WarningBox("Current Qty cannot equal Original Qty!");
                    e.Cancel = true;
                    return;
                }
                if (this.EditMode && !MyUtility.Check.Empty(e.FormattedValue))
                {
                    if (MyUtility.Convert.GetDecimal(dr["QtyBefore"]) - MyUtility.Convert.GetDecimal(e.FormattedValue) <= 0)
                    {
                        dr["qtyafter"] = 0;
                        return;
                    }
                    else
                    {
                        dr["qtyafter"] = e.FormattedValue;
                        dr["adjustqty"] = MyUtility.Convert.GetDecimal(dr["QtyBefore"]) - MyUtility.Convert.GetDecimal(dr["QtyAfter"]);
                    }
                }
            };

            #endregion
            
            #region -- Reason ID 右鍵開窗 --
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts = new DataGridViewGeneratorTextColumnSettings();
            ts.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    DataTable poitems;
                    string sqlcmd = "";
                    IList<DataRow> x;

                    sqlcmd = @"select id, Name from Reason WITH (NOLOCK) where ReasonTypeID='Stock_Remove' AND junk = 0";
                    DualResult result2 = DBProxy.Current.Select(null, sqlcmd, out poitems);
                    if (!result2)
                    {
                        ShowErr(sqlcmd, result2);
                        return;
                    }

                    Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(poitems
                        , "ID,Name"
                        , "5,150"
                        , CurrentDetailData["reasonid"].ToString()
                        , "ID,Name");
                    item.Width = 600;
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    x = item.GetSelecteds();

                    CurrentDetailData["reasonid"] = x[0]["id"];
                    CurrentDetailData["reason_nm"] = x[0]["name"];
                }
            };
            ts.CellValidating += (s, e) =>
            {
                DataRow dr;
                if (!this.EditMode) return;
                if (String.Compare(e.FormattedValue.ToString(), CurrentDetailData["reasonid"].ToString()) != 0)
                {
                    if (MyUtility.Check.Empty(e.FormattedValue))
                    {
                        CurrentDetailData["reasonid"] = "";
                        CurrentDetailData["reason_nm"] = "";
                    }
                    else
                    {
                        if (!MyUtility.Check.Seek(string.Format(@"select id, Name from Reason WITH (NOLOCK) where id = '{0}' 
and ReasonTypeID='Stock_Remove' AND junk = 0", e.FormattedValue), out dr, null))
                        {

                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("Data not found!", "Reason ID");
                            return;
                        }
                        else
                        {
                            CurrentDetailData["reasonid"] = e.FormattedValue;
                            CurrentDetailData["reason_nm"] = dr["name"];
                        }
                    }
                }
            };

            #endregion Seq 右鍵開窗     
            #region -- 欄位設定 --
            Helper.Controls.Grid.Generator(this.detailgrid)           
            .Text("poid", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: true)  //0
            .Text("seq", header: "Seq", width: Widths.AnsiChars(8), iseditingreadonly: true)  //1
            .Text("Roll", header: "Roll", width: Widths.AnsiChars(8), iseditingreadonly: true)  //2
            .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true)  //3     
            .Text("Description", header: "Description", width: Widths.AnsiChars(8), iseditingreadonly: true)  //4
            .Numeric("QtyBefore", header: "Original Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)    //4
            .Numeric("QtyAfter", header: "Current Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, minimum:0,settings: ns)    //5
            .Numeric("adjustqty", header: "Remove Qty", width: Widths.AnsiChars(8), decimal_places: 2, integer_places: 10, iseditingreadonly: true)    //6            
            .Text("StockUnit", header: "Unit", iseditingreadonly: true)    //7
            .Text("Location", header: "Location", iseditingreadonly: true)    //7
            .Text("reasonid", header: "Reason ID", settings: ts)    //8
            .Text("reason_nm", header: "Reason Name", iseditingreadonly: true, width: Widths.AnsiChars(20))    //9
            ;     //
            #endregion 欄位設定
            detailgrid.Columns["qtyafter"].DefaultCellStyle.BackColor = Color.Pink;
            detailgrid.Columns["reasonid"].DefaultCellStyle.BackColor = Color.Pink;
        }
        //Import
        private void btnImport_Click(object sender, EventArgs e)
        {
            var frm = new Sci.Production.Warehouse.P45_Import(CurrentMaintain, (DataTable)detailgridbs.DataSource);
            frm.ShowDialog(this);
            this.RenewData();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(detailgridbs.DataSource)) return;
            int index = detailgridbs.Find("poid", txtLocateForSP.Text.TrimEnd());
            if (index == -1)
            { MyUtility.Msg.WarningBox("Data was not found!!"); }
            else
            { detailgridbs.Position = index; }
        }
        // detail 新增時設定預設值
        protected override void OnDetailGridInsert(int index = -1)
        {
            base.OnDetailGridInsert(index);
        }
        // grid 加工填值
        protected override DualResult OnRenewDataDetailPost(RenewDataPostEventArgs e)
        {
            return base.OnRenewDataDetailPost(e);
        }
    }
}
