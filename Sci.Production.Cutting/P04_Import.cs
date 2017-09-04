﻿using Ict;
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
using Sci.Win.Tools;

namespace Sci.Production.Cutting
{
    public partial class P04_Import : Sci.Win.Subs.Base
    {
        private string loginID = Sci.Env.User.UserID;
        private string keyWord = Sci.Env.User.Keyword;
        DataTable gridTable,detailTable;
        public P04_Import()
        {
            InitializeComponent();
            txtCutCell.MDivisionID = keyWord;

        }
        protected override void OnFormLoaded()
        {
            DBProxy.Current.Select(null, "Select 0 as Sel, '' as poid,'' as cuttingid,'' as brandid,'' as styleid,'' as cutcellid,'' as cutref from cutplan where 1=0", out gridTable);
            base.OnFormLoaded();
            this.gridImport.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.gridImport.DataSource = gridTable;
            Helper.Controls.Grid.Generator(this.gridImport)
            .CheckBox("Sel", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
            .Text("POID", header: "PO#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("Cuttingid", header: "Cutting SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("Brandid", header: "Brand", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("Styleid", header: "Style#", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Text("Cutcellid", header: "Cut Cell", width: Widths.AnsiChars(2), iseditingreadonly: true)
            .Text("CutRef", header: "CutRef#", width: Widths.AnsiChars(40), iseditingreadonly: true);
            this.gridImport.Columns["Sel"].DefaultCellStyle.BackColor = Color.Pink;

        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(dateEstCutDate.Value))
            {
                MyUtility.Msg.WarningBox("<Est. Cut Date> can not be empty.");
                return;
            }
            gridTable.Rows.Clear();  //開始查詢前先清空資料
            string factory = txtfactory.Text;
            string estcutdate = dateEstCutDate.Text;
            string cutcellid = txtCutCell.Text;
            string sqlcmd = string.Format("Select a.*,'' as orderid_b,'' as article_b, '' as sizecode,'' as sewinglineid,1 as sel from Workorder a where (cutplanid='' or cutplanid is null) and cutcellid!='' and a.CutRef != ''  and mDivisionid ='{0}' and estcutdate = '{1}'", keyWord, estcutdate);
            if (!MyUtility.Check.Empty(cutcellid))
            {
                sqlcmd = sqlcmd + string.Format(" and cutcellid = '{0}'", cutcellid);
            }
            if (!MyUtility.Check.Empty(factory))
            {
                sqlcmd = sqlcmd + string.Format(" and a.factoryid = '{0}'", factory);
            }
            DataRow queryRow,ordersRow;
            DualResult dResult = DBProxy.Current.Select(null, sqlcmd, out detailTable);
            if (dResult)
            {
                if (detailTable.Rows.Count != 0)
                {
                    foreach (DataRow dr in detailTable.Rows)
                    {
                        if (MyUtility.Check.Seek(string.Format("Select top(1) * from WorkOrder_Distribute where workorderukey='{0}' and orderid !='' and orderid !='Excess'", dr["Ukey"]), null, out queryRow))
                        {
                            dr["orderid_b"] = queryRow["OrderId"];
                            dr["article_b"] = queryRow["article"];
                            dr["sizecode"] = queryRow["sizecode"];
                        }
                        string line = MyUtility.GetValue.Lookup(string.Format("Select SewingLineid from Sewingschedule_Detail Where Orderid = '{0}' and article ='{1}' and sizecode = '{2}'", dr["orderid_b"], dr["article_b"], dr["sizecode"]), null);
                        if (MyUtility.Check.Empty(line))
                        {
                            line = MyUtility.GetValue.Lookup(string.Format("Select SewingLineid from Sewingschedule_Detail Where Orderid = '{0}' ", dr["orderid_b"]), null);
                        }
                        dr["Sewinglineid"] = line;
                        DataRow[] griddray = gridTable.Select(string.Format("cuttingid = '{0}' and cutcellid ='{1}'", dr["id"], dr["cutcellid"]));
                        if (griddray.Length == 0)
                        {
                            DataRow newdr = gridTable.NewRow();
                            newdr["sel"] = 1;
                            MyUtility.Check.Seek(string.Format("Select * from orders where id='{0}'", dr["ID"]), out ordersRow);
                            newdr["POID"] = ordersRow["poid"];
                            newdr["Cuttingid"] = dr["ID"];
                            newdr["brandid"] = ordersRow["brandid"];
                            newdr["Styleid"] = ordersRow["styleid"];
                            newdr["Cutcellid"] = dr["cutcellid"];
                            newdr["cutref"] = dr["cutref"];
                            gridTable.Rows.Add(newdr);
                        }
                        else
                        {
                            griddray[0]["cutref"] = griddray[0]["cutref"].ToString().Trim() + "," + dr["cutref"].ToString();
                        }
                    }
                }
                else
                {
                    MyUtility.Msg.WarningBox("Data not found!!");
                }

            }
            else
            {
                ShowErr(sqlcmd,dResult);
                return;
            }
        }
        public List<String> importedIDs = new List<string>();
        private void btnImport_Click(object sender, EventArgs e)
        {
            if (gridTable.Rows.Count == 0) return;
            importedIDs.Clear();
            DataRow[] importay;
            string insertheader = "";

            #region transaction
            DualResult upResult;
            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    string remark;
                    DataTable cutplan_DetailTb;
                    foreach (DataRow dr in gridTable.Rows)
                    {
                        remark = "";
                        insertheader = "";
                        if (dr["sel"].ToString() == "1")
                        {
                            
                            string id = MyUtility.GetValue.GetID(keyWord + "CP", "Cutplan");

                            if (!string.IsNullOrWhiteSpace(id))
                            {
                                insertheader = string.Format("insert into Cutplan(id,cuttingid,mDivisionid,CutCellid,EstCutDate,Status,AddName,AddDate,POID) Values('{0}','{1}','{2}','{3}','{4}','{5}','{6}',GetDate(),'{7}');", id, dr["CuttingID"], keyWord, dr["cutcellid"], dateEstCutDate.Text, "New", loginID, dr["POId"]);
                                importay = detailTable.Select(string.Format("id = '{0}' and cutcellid = '{1}'", dr["CuttingID"], dr["cutcellid"]));
                                importedIDs.Add(id);
                                if (importay.Length > 0)
                                {
                                    foreach (DataRow ddr in importay)
                                    {
                                        DBProxy.Current.Select(null, string.Format("Select * from WorkOrder_Distribute Where workorderukey = '{0}'", ddr["Ukey"]), out cutplan_DetailTb);
                                        if (cutplan_DetailTb != null)
                                        {
                                            remark = string.Empty;  //組remark前先清空 
                                            foreach (DataRow cdr in cutplan_DetailTb.Rows)
                                            {
                                                remark = remark + cdr["Orderid"].ToString().Trim() + "\\" + cdr["Article"].ToString().Trim() + "\\" + cdr["SizeCode"].ToString().Trim() + "\\" + cdr["Qty"].ToString() + ",";
                                            }
                                        }

                                        insertheader = insertheader + string.Format("insert into Cutplan_Detail(ID,Sewinglineid,cutref,cutno,orderid,styleid,colorid,cons,WorkOrderUkey,POID,Remark) values('{0}','{1}','{2}',{3},'{4}','{5}','{6}',{7},'{8}','{9}','{10}');", id, ddr["Sewinglineid"], ddr["Cutref"], ddr["Cutno"], ddr["OrderID"], dr["styleid"], ddr["Colorid"], ddr["Cons"], ddr["Ukey"], dr["POID"], remark);
                                    }

                                    //265: CUTTING_P04_Import_Import From Work Order，將id回寫至Workorder.CutplanID
                                    string UpdateWorkorder = string.Format(@"
update Workorder set CutplanID = '{0}' 
where (cutplanid='' or cutplanid is null) and id='{1}' and cutcellid='{2}' and mDivisionid ='{3}' and estcutdate = '{4}'"
                                        , id , dr["CuttingID"] , dr["cutcellid"],  keyWord, dateEstCutDate.Text);

                                    if (!(upResult = DBProxy.Current.Execute(null, insertheader)))
                                    {
                                        importedIDs.Clear();
                                        _transactionscope.Dispose();
                                        return;
                                    }
                                    if (!(upResult = DBProxy.Current.Execute(null, UpdateWorkorder)))
                                    {
                                        importedIDs.Clear();
                                        _transactionscope.Dispose();
                                        return;
                                    }

                                }
                            }
                        }
                    }
                    _transactionscope.Complete();
                    _transactionscope.Dispose();
                    MyUtility.Msg.WarningBox("Successfully");
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    ShowErr("Commit transaction error.", ex);
                    importedIDs.Clear();
                    return;
                }
            }
            _transactionscope.Dispose();
            _transactionscope = null;
            #endregion
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

    }
}
