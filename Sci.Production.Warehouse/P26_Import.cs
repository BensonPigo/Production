﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci;
using Sci.Data;
using System.Linq;

namespace Sci.Production.Warehouse
{
    public partial class P26_Import : Sci.Win.Subs.Base
    {
        DataRow dr_master;
        DataTable dt_detail;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        protected DataTable dtArtwork;
        Dictionary<string, string> selectedLocation = new Dictionary<string, string>();

        public P26_Import(DataRow master, DataTable detail)
        {
            InitializeComponent();
            dr_master = master;
            dt_detail = detail;
            this.EditMode = true;

            Dictionary<String, String> comboBox1_RowSource = new Dictionary<string, string>();
            comboBox1_RowSource.Add("", "All");
            comboBox1_RowSource.Add("F", "Fabric");
            comboBox1_RowSource.Add("A", "Accessory");
            cmbMaterialType.DataSource = new BindingSource(comboBox1_RowSource, null);
            cmbMaterialType.ValueMember = "Key";
            cmbMaterialType.DisplayMember = "Value";

            DataTable dt = (DataTable)this.comboStockType.DataSource;
            dt.Rows.InsertAt(dt.NewRow(),0);
            this.comboStockType.SelectedIndex = 0;
        }

        //Button Query
        private void btnQuery_Click(object sender, EventArgs e)
        {
            StringBuilder strSQLCmd = new StringBuilder();
            String sp = this.txtSPNo.Text.TrimEnd();
            String refno = this.txtRef.Text.TrimEnd();
            String locationid = this.txtLocation.Text.TrimEnd();
            String dyelot = this.txtDyelot.Text.TrimEnd();
            String transid = this.txtTransactionID.Text.TrimEnd();
            String MaterialType = this.cmbMaterialType.SelectedValue.ToString();
            String StockType = this.comboStockType.SelectedValue.ToString();
            switch (radioPanel1.Value)
            {
                case "1":
                    if (MyUtility.Check.Empty(sp) && MyUtility.Check.Empty(locationid) && MyUtility.Check.Empty(refno))
                    {
                        MyUtility.Msg.WarningBox("< SP# > or < Ref# > or < Location > can't be empty!!");
                        txtSPNo.Focus();
                        return;
                    }                        
                    else
                    {
                        // 建立可以符合回傳的Cursor
                        strSQLCmd.Append(string.Format(@"
select  distinct 0 as selected
        , a.Poid
        , a.seq1
        , a.seq2
        , concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2) as seq
        , a.Roll
        , a.Dyelot
        , a.InQty - a.OutQty + a.AdjustQty qty
        , a.Ukey ftyinventoryukey
        , dbo.getmtldesc(a.poid,a.seq1,a.seq2,2,0) as [description] 
        , dbo.Getlocation(a.ukey) fromlocation
        , '' tolocation
        , '' id
        , p1.refno
        , p1.colorid
        , p1.sizespec
        , rd.Ukey Receiving_Detail_ukey
        , a.StockType
from dbo.FtyInventory a WITH (NOLOCK) 
left join dbo.FtyInventory_Detail b WITH (NOLOCK) on a.Ukey = b.Ukey
left join dbo.PO_Supp_Detail p1 WITH (NOLOCK) on p1.ID = a.PoId and p1.seq1 = a.SEQ1 and p1.SEQ2 = a.seq2
inner join dbo.Factory f on f.ID=p1.factoryID 
left join dbo.Receiving_Detail rd  WITH (NOLOCK) on rd.POID = a.POID and rd.Seq1 = a.Seq1 and rd.Seq2 = a.Seq2 and rd.StockType = a.StockType and rd.Roll = a.Roll and rd.Dyelot = a.Dyelot
where    f.MDivisionID='{0}' ", Sci.Env.User.Keyword)); // 
                        
                        //if (BalanceQty.Checked)
                        //{
                        //    strSQLCmd.Append("AND a.InQty - a.OutQty + a.AdjustQty > 0 ");
                        //}

                        if (!MyUtility.Check.Empty(sp))
                        {
                            strSQLCmd.Append(string.Format(@" 
        and a.poid='{0}' ", sp));
                        }
                        if (!txtSeq.checkSeq1Empty() && txtSeq.checkSeq2Empty())
                        {
                            strSQLCmd.Append(string.Format(@" 
        and a.seq1 = '{0}'", txtSeq.seq1));
                        }else if (!txtSeq.checkEmpty(showErrMsg: false))
                        {
                            strSQLCmd.Append(string.Format(@" 
        and a.seq1 = '{0}' and a.seq2='{1}'", txtSeq.seq1, txtSeq.seq2));
                        }
                        if (!MyUtility.Check.Empty(refno))
                        {
                            strSQLCmd.Append(string.Format(@" 
        and (select refno from dbo.PO_Supp_Detail P WITH (NOLOCK) where P.id = a.poid and P.seq1 = a.seq1 and P.seq2 = a.seq2 )='{0}'", refno));
                        }
                        if (!MyUtility.Check.Empty(locationid))
                        {
                            strSQLCmd.Append(string.Format(@" 
        and b.MtlLocationID = '{0}' ", locationid));
                        }
                        if (!MyUtility.Check.Empty(dyelot))
                        {
                            strSQLCmd.Append(string.Format(@" 
        and a.dyelot='{0}' ", dyelot));
                        }
                        if (!MyUtility.Check.Empty(MaterialType))
                        {
                            strSQLCmd.Append(string.Format(@" 
        and p1.FabricType='{0}' ", MaterialType));
                        }
                        if (!MyUtility.Check.Empty(StockType))
                        {
                            strSQLCmd.Append(string.Format(@" 
        and a.StockType = {0} ", StockType));
                        }
                    }
                    break;

                case "2":
                    if (string.IsNullOrWhiteSpace(transid))
                    {
                        MyUtility.Msg.WarningBox("< Transaction ID# > can't be empty!!");
                        txtTransactionID.Focus();
                        return;
                    }
                    strSQLCmd.Append(string.Format(@"
select  0 as selected
        , a.Poid
        , a.seq1
        , a.seq2
        , concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2) as seq
        , a.Roll
        , a.Dyelot
        , a.InQty - a.OutQty + a.AdjustQty qty
        , a.Ukey
        , dbo.getmtldesc(a.poid,a.seq1,a.seq2,2,0) as [description] 
        , dbo.Getlocation(a.ukey) as fromlocation
        , '' tolocation
        , '' id
        , a.ukey as ftyinventoryukey
        , (select refno from dbo.PO_Supp_Detail P WITH (NOLOCK) where P.id = a.poid and P.seq1 = a.seq1 and P.seq2 = a.seq2 ) refno
        , (select ColorID from dbo.PO_Supp_Detail P WITH (NOLOCK) where P.id = a.poid and P.seq1 = a.seq1 and P.seq2 = a.seq2 ) ColorID
        , r2.Ukey Receiving_Detail_ukey
        , a.StockType
from dbo.Receiving r1 WITH (NOLOCK) 
inner join dbo.Receiving_Detail r2 WITH (NOLOCK) on r2.id = r1.Id
inner join dbo.FtyInventory a WITH (NOLOCK) on a.Poid = r2.PoId and a.Seq1 = r2.seq1 and a.seq2  = r2.seq2 and a.Roll = r2.Roll and a.stocktype = r2.stocktype and r2.Roll = a.Roll and r2.Dyelot = a.Dyelot
where   {3}
        r1.Status = 'Confirmed' 
        and r1.Status = 'Confirmed' 
        and r1.mdivisionid='{2}'
        and r1.id = '{0}'
        {1}
union all
select  0 as selected
        , a.Poid
        , a.seq1
        , a.seq2
        , concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2) as seq
        , a.Roll
        , a.Dyelot
        , a.InQty - a.OutQty + a.AdjustQty qty
        , a.Ukey
        , dbo.getmtldesc(a.poid,a.seq1,a.seq2,2,0) as [description] 
        , dbo.Getlocation(a.ukey) as fromlocation
        , '' tolocation
        , '' id
        , a.ukey ftyinventoryukey
        , (select refno from dbo.PO_Supp_Detail P WITH (NOLOCK) where P.id = a.poid and P.seq1 = a.seq1 and P.seq2 = a.seq2) refno
        , (select ColorID from dbo.PO_Supp_Detail P WITH (NOLOCK) where P.id = a.poid and P.seq1 = a.seq1 and P.seq2 = a.seq2 ) ColorID
        , rd.Ukey Receiving_Detail_ukey
        , a.StockType
from dbo.SubTransfer r1 WITH (NOLOCK) 
inner join dbo.SubTransfer_Detail r2 WITH (NOLOCK) on r2.id = r1.Id
inner join dbo.FtyInventory a WITH (NOLOCK) on a.ukey = r2.fromftyinventoryukey
left join dbo.Receiving_Detail rd  WITH (NOLOCK) on rd.POID = a.POID and rd.Seq1 = a.Seq1 and rd.Seq2 = a.Seq2 and rd.StockType = a.StockType and rd.Roll = a.Roll and rd.Dyelot = a.Dyelot
where   {3}
        r1.Status = 'Confirmed' 
        and r1.Status = 'Confirmed'
        and r1.mdivisionid='{2}'
        and r1.id = '{0}'
        {1}
union all
select  0 as selected
        , a.Poid
        , a.seq1
        , a.seq2
        , concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2) as seq
        , a.Roll
        , a.Dyelot
        , a.InQty - a.OutQty + a.AdjustQty qty
        , a.Ukey
        , dbo.getmtldesc(a.poid,a.seq1,a.seq2,2,0) as [description] 
        , dbo.Getlocation(a.ukey) as fromlocation
        , '' tolocation
        ,  '' id
        , a.ukey ftyinventoryukey
        , (select refno from dbo.PO_Supp_Detail P WITH (NOLOCK) where P.id = a.poid and P.seq1 = a.seq1 and P.seq2 = a.seq2 ) refno
        , (select ColorID from dbo.PO_Supp_Detail P WITH (NOLOCK) where P.id = a.poid and P.seq1 = a.seq1 and P.seq2 = a.seq2 ) ColorID
        , rd.Ukey Receiving_Detail_ukey
        , a.StockType
from dbo.Issue r1 WITH (NOLOCK) 
inner join dbo.Issue_Detail r2 WITH (NOLOCK) on r2.id = r1.Id
inner join dbo.FtyInventory a WITH (NOLOCK) on a.ukey = r2.ftyinventoryukey
left join dbo.Receiving_Detail rd  WITH (NOLOCK) on rd.POID = a.POID and rd.Seq1 = a.Seq1 and rd.Seq2 = a.Seq2 and rd.StockType = a.StockType and rd.Roll = a.Roll and rd.Dyelot = a.Dyelot
where   {3}
        r1.Status = 'Confirmed' 
        and r1.Status = 'Confirmed' 
        and r1.mdivisionid='{2}'
        and r1.id = '{0}'
        {1}
union all
select  0 as selected
        , a.Poid
        , a.seq1
        , a.seq2
        , concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2) as seq
        , a.Roll
        , a.Dyelot
        , a.InQty - a.OutQty + a.AdjustQty qty
        , a.Ukey
        , dbo.getmtldesc(a.poid,a.seq1,a.seq2,2,0) as [description] 
        , dbo.Getlocation(a.ukey) as fromlocation
        , '' tolocation
        , '' id
        , a.ukey ftyinventoryukey
        , (select refno from dbo.PO_Supp_Detail P WITH (NOLOCK) where P.id = a.poid and P.seq1 = a.seq1 and P.seq2 = a.seq2 ) refno
        , (select ColorID from dbo.PO_Supp_Detail P WITH (NOLOCK) where P.id = a.poid and P.seq1 = a.seq1 and P.seq2 = a.seq2 ) ColorID
        , rd.Ukey Receiving_Detail_ukey
        , a.StockType
from dbo.ReturnReceipt r1 WITH (NOLOCK) 
inner join dbo.ReturnReceipt_Detail r2 WITH (NOLOCK) on r2.id = r1.Id
inner join dbo.FtyInventory a WITH (NOLOCK) on a.ukey = r2.ftyinventoryukey
left join dbo.Receiving_Detail rd  WITH (NOLOCK) on rd.POID = a.POID and rd.Seq1 = a.Seq1 and rd.Seq2 = a.Seq2 and rd.StockType = a.StockType and rd.Roll = a.Roll and rd.Dyelot = a.Dyelot
where   {3}
        r1.Status = 'Confirmed' 
        and r1.Status = 'Confirmed' 
        and r1.mdivisionid='{2}'
        and r1.id = '{0}'
        {1}
union
select  0 as selected
        , a.Poid
        , a.seq1
        , a.seq2
        , concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2) as seq
        , a.Roll
        , a.Dyelot
        , a.InQty - a.OutQty + a.AdjustQty qty
        , a.Ukey
        , dbo.getmtldesc(a.poid,a.seq1,a.seq2,2,0) as [description] 
        , dbo.Getlocation(a.ukey) as fromlocation
        , '' tolocation
        , '' id
        , a.ukey ftyinventoryukey 
        , (select refno from dbo.PO_Supp_Detail P WITH (NOLOCK) where P.id = a.poid and P.seq1 = a.seq1 and P.seq2 = a.seq2 ) refno
        , (select ColorID from dbo.PO_Supp_Detail P WITH (NOLOCK) where P.id = a.poid and P.seq1 = a.seq1 and P.seq2 = a.seq2 ) ColorID
        , rd.Ukey Receiving_Detail_ukey
        , a.StockType
from dbo.TransferIn r1 WITH (NOLOCK) 
inner join dbo.TransferIn_Detail r2 WITH (NOLOCK) on r2.id = r1.Id
inner join dbo.FtyInventory a WITH (NOLOCK) on a.Poid = r2.PoId and a.Seq1 = r2.seq1 and a.seq2  = r2.seq2 and a.Roll = r2.Roll and a.stocktype = r2.stocktype
left join dbo.Receiving_Detail rd  WITH (NOLOCK) on rd.POID = a.POID and rd.Seq1 = a.Seq1 and rd.Seq2 = a.Seq2 and rd.StockType = a.StockType and rd.Roll = a.Roll and rd.Dyelot = a.Dyelot
where  
        r1.Status = 'Confirmed' 
        and r1.mdivisionid='{2}'
        and r1.id = '{0}' 
        {1} ", 
        transid,
        MyUtility.Check.Empty(StockType) ? string.Empty : $"and a.StockType = {StockType}",
        Sci.Env.User.Keyword
        )); // 
                    break;
            }

            // 增加 order by FtyInventory.POID, FtyInventory.Seq1, FtyInventory.Seq2,Receiving_Detail.Ukey,FtyInventory.StockType
            strSQLCmd.Insert(0, "select * from (");
            strSQLCmd.Append(") a order by Poid,seq1,seq2,Receiving_Detail_ukey,StockType");

            this.ShowWaitMessage("Data Loading....");
            Ict.DualResult result;
            if (!(result = DBProxy.Current.Select(null, strSQLCmd.ToString(), out dtArtwork)))
            {
                ShowErr(strSQLCmd.ToString(), result);
            }
            else
            {
                if (dtArtwork.Rows.Count == 0)
                { MyUtility.Msg.WarningBox("Data not found!!"); }

                listControlBindingSource1.DataSource = dtArtwork;
                //dtArtwork.DefaultView.Sort = "seq1,seq2,location,dyelot,balance desc";
            }
            this.HideWaitMessage();
            //全部撈完之後再勾選，觸發filter事件
            BalanceQty.Checked = true;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            txtSPNo.Focus();
            #region Location 右鍵開窗
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts2 = new DataGridViewGeneratorTextColumnSettings();
            ts2.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    DataRow currentrow = gridImport.GetDataRow(gridImport.GetSelectedRowIndex());
                    Sci.Win.Tools.SelectItem2 item = PublicPrg.Prgs.SelectLocation(currentrow["Stocktype"].ToString(), currentrow["ToLocation"].ToString());
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    currentrow["ToLocation"] = item.GetSelectedString();
                    currentrow.EndEdit();
                }
            };

            ts2.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    DataRow dr = gridImport.GetDataRow(e.RowIndex);
                    dr["ToLocation"] = e.FormattedValue;
                    string sqlcmd = string.Format(@"
SELECT  id
        , Description
        , StockType 
FROM    DBO.MtlLocation WITH (NOLOCK) 
WHERE   StockType='{0}'
        and junk != '1'", dr["Stocktype"].ToString());
                    DataTable dt;
                    DBProxy.Current.Select(null, sqlcmd, out dt);
                    string[] getLocation = dr["ToLocation"].ToString().Split(',').Distinct().ToArray();
                    bool selectId = true;
                    List<string> errLocation = new List<string>();
                    List<string> trueLocation = new List<string>();
                    foreach (string location in getLocation)
                    {
                        if (!dt.AsEnumerable().Any(row => row["id"].EqualString(location)) && !(location.EqualString("")))
                        {
                            selectId &= false;
                            errLocation.Add(location);
                        }
                        else if (!(location.EqualString("")))
                        {
                            trueLocation.Add(location);
                        }
                    }

                    if (!selectId)
                    {
                       e.Cancel = true;
                        MyUtility.Msg.WarningBox("Location : " + string.Join(",", (errLocation).ToArray()) + "  Data not found !!", "Data not found");
                        
                    }
                    trueLocation.Sort();
                    dr["ToLocation"] = string.Join(",", (trueLocation).ToArray());
                    //去除錯誤的Location將正確的Location填回

                    dr["selected"] = (!String.IsNullOrEmpty(dr["ToLocation"].ToString())) ? 1 : 0; 

                    gridImport.RefreshEdit();
                }
            };
            #endregion Location 右鍵開窗
            #region stocktype validating
            Ict.Win.DataGridViewGeneratorComboBoxColumnSettings stocktypeSet = new DataGridViewGeneratorComboBoxColumnSettings();

            stocktypeSet.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    DataRow drSelected = gridImport.GetDataRow(e.RowIndex);
                    if (e.FormattedValue.Equals(drSelected["stocktype"]))
                    {
                        return;
                    }
                    
                    string getFtyInventorySql = $@"
select 
[Qty] = InQty - OutQty + AdjustQty ,
[fromlocation] = dbo.Getlocation(ukey) 
from FtyInventory
where
Poid = '{drSelected["poid"]}' and 
Seq1 = '{drSelected["Seq1"]}' and 
seq2  = '{drSelected["seq2"]}' and 
Roll = '{drSelected["Roll"]}' and 
stocktype = '{e.FormattedValue}'
";
                    DataRow dr;
                    if (MyUtility.Check.Seek(getFtyInventorySql, out dr))
                    {
                        drSelected["qty"] = dr["Qty"];
                        drSelected["FromLocation"] = dr["fromlocation"];
                        drSelected["stocktype"] = e.FormattedValue;
                        drSelected["ToLocation"] = string.Empty;
                    }
                    else
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox($"<Stock Type> data not found");
                        return;
                    }
                }
            };

            #endregion

            Ict.Win.UI.DataGridViewComboBoxColumn cbb_stocktype;

            this.gridImport.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.gridImport.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.gridImport)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)   //0
                .Text("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)  //1
                .Text("seq", header: "Seq", width: Widths.AnsiChars(6), iseditingreadonly: true)  //2
                .Text("Roll", header: "Roll#", width: Widths.AnsiChars(9), iseditingreadonly: true)    //3
                .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(5), iseditingreadonly: true)    //4
                .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)    //5
                .Text("colorid", header: "Color", width: Widths.AnsiChars(5), iseditingreadonly: true)    //6
                .Numeric("qty", header: "Qty", width: Widths.AnsiChars(10), decimal_places: 2, integer_places: 10, iseditingreadonly: true)    //7
                .ComboBox("stocktype", header: "Stock" + Environment.NewLine + "Type", width: Widths.AnsiChars(8), iseditable: true, settings: stocktypeSet).Get(out cbb_stocktype) //8
                .Text("FromLocation", header: "FromLocation", iseditingreadonly: true)    //9
                .Text("ToLocation", header: "ToLocation", settings: ts2, iseditingreadonly: false)    //10
            ;

            DataTable stocktypeSrc;
            string stocktypeGetSql = "select ID = replace(ID,'''',''), Name = rtrim(Name) from DropDownList WITH (NOLOCK) where Type = 'Pms_StockType' order by Seq";
            DBProxy.Current.Select(null, stocktypeGetSql, out stocktypeSrc);
            cbb_stocktype.DataSource = stocktypeSrc;
            cbb_stocktype.ValueMember = "ID";
            cbb_stocktype.DisplayMember = "Name";

            this.gridImport.Columns["ToLocation"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridImport.Columns["stocktype"].DefaultCellStyle.BackColor = Color.Pink;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            listControlBindingSource1.EndEdit();
            gridImport.ValidateControl();
            DataTable dtGridBS1 = (DataTable)listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dtGridBS1) || dtGridBS1.Rows.Count == 0) return;

            DataRow[] dr2 = dtGridBS1.Select("Selected = 1");
            if (dr2.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }

            foreach (DataRow tmp in dr2)
            {

                DataRow[] findrow = dt_detail.AsEnumerable().Where(row => row.RowState != DataRowState.Deleted && row["poid"].EqualString(tmp["poid"].ToString()) && row["seq1"].EqualString(tmp["seq1"].ToString())
                                                                          && row["seq2"].EqualString(tmp["seq2"].ToString()) && row["roll"].EqualString(tmp["roll"].ToString())
                                                                          && row["dyelot"].EqualString(tmp["dyelot"].ToString())
                                                                          && row["stocktype"].EqualString(tmp["stocktype"].ToString())).ToArray();
                //DataRow[] findrow = dt_detail.Select(string.Format("poid = '{0}' and seq1 = '{1}' and seq2 = '{2}' and roll ='{3}'and dyelot='{4}'"
                //    , tmp["poid"].ToString(), tmp["seq1"].ToString(), tmp["seq2"].ToString(), tmp["roll"].ToString(), tmp["dyelot"].ToString()));

                if (findrow.Length > 0)
                {
                    findrow[0]["qty"] = tmp["qty"];
                    findrow[0]["tolocation"] = tmp["tolocation"];
                    findrow[0]["fromlocation"] = tmp["fromlocation"];
                }
                else
                {
                    tmp["id"] = dr_master["id"];
                    tmp.AcceptChanges();
                    tmp.SetAdded();
                    dt_detail.ImportRow(tmp);
                }
            }


            this.Close();
        }

        private void txtLocation2_MouseDown(object sender, MouseEventArgs e)
        {
            Sci.Win.Tools.SelectItem2 item = PublicPrg.Prgs.SelectLocation("", "");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            var select_result = item.GetSelecteds()
                .GroupBy(s => new { StockType = s["StockType"].ToString(), StockTypeCode = s["StockTypeCode"].ToString() })
                .Select(g => new { g.Key.StockType, g.Key.StockTypeCode, ToLocation = string.Join(",", g.Select(i => i["id"])) });

            if (select_result.Count() > 0)
            {
                this.selectedLocation.Clear();
                txtLocation2.Text = string.Empty;
            }
            foreach (var result_item in select_result)
            {
                this.selectedLocation.Add(result_item.StockTypeCode, result_item.ToLocation);
                txtLocation2.Text += $"({result_item.StockType}:{result_item.ToLocation})";
            }

        }

        private void radioPanel1_ValueChanged(object sender, EventArgs e)
        {
            Sci.Win.UI.RadioPanel rdoG = (Sci.Win.UI.RadioPanel)sender;
            switch (rdoG.Value)
            {
                case "1":
                    txtSPNo.ReadOnly = false;
                    txtSeq.txtSeq_ReadOnly(false);
                    txtRef.ReadOnly = false;
                    txtLocation.ReadOnly = false;
                    txtDyelot.ReadOnly = false;
                    txtTransactionID.ReadOnly = true;
                    txtTransactionID.Text = "";
                    break;
                case "2":
                    txtSPNo.ReadOnly = true;
                    txtSeq.txtSeq_ReadOnly(true);
                    txtRef.ReadOnly = true;
                    txtLocation.ReadOnly = true;
                    txtDyelot.ReadOnly = true;
                    txtSPNo.Text = "";
                    txtSeq.seq1 = "";
                    txtSeq.seq2 = "";
                    txtRef.Text = "";
                    txtLocation.Text = "";
                    txtDyelot.Text = "";
                    txtTransactionID.ReadOnly = false;
                    break;
            }
        }

        private void btnUpdateAllLocation_Click(object sender, EventArgs e)
        {
            listControlBindingSource1.EndEdit();
            DataTable dt = (DataTable)listControlBindingSource1.DataSource;
            if (dt == null || dt.Rows.Count == 0) return;
            DataRow[] drfound = dt.Select("selected = 1");

            foreach (var item in drfound)
            {
                if (this.selectedLocation.ContainsKey(item["stocktype"].ToString()))
                {
                    item["tolocation"] = this.selectedLocation[item["stocktype"].ToString()];
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            bool re = base.ProcessCmdKey(ref msg, keyData);
            if (radioTransactionID.Focused && radioTransactionID.Checked == true)
            {
                if (keyData == Keys.Tab || keyData == Keys.Enter)
                {
                    txtTransactionID.Select();
                    return true;
                }
            }

            if (txtTransactionID.Focused)
            {
                if(keyData == Keys.Tab || keyData == Keys.Enter)
                {
                    txtTransactionID.TabStop = false;
                    btnQuery.Select();
                    return true;
                }
            }
            return re;
        }

        //動態顯示列表資料
        private void BalanceQty_CheckedChanged(object sender, EventArgs e)
        {
            grid_Filter();
        }

        private void grid_Filter()
        {
            string filter = "";
            if (gridImport.RowCount > 0)
            {
                switch (BalanceQty.Checked)
                {
                    case true:
                        if (MyUtility.Check.Empty(gridImport)) break;
                        //這裡過濾的欄位，必須是剛剛SQL查出來的欄位，不是WHERE裡面的條件
                        filter = $@"qty > 0";
  
                        ((DataTable)listControlBindingSource1.DataSource).DefaultView.RowFilter = filter;
                        break;

                    case false:
                        if (MyUtility.Check.Empty(gridImport)) break;
                        filter = "";
                         ((DataTable)listControlBindingSource1.DataSource).DefaultView.RowFilter = filter;
                        break;
                }
            }
        }
    }
}
