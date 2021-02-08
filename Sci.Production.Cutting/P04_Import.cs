using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Transactions;

namespace Sci.Production.Cutting
{
    /// <inheritdoc/>
    public partial class P04_Import : Win.Subs.Base
    {
        private string loginID = Env.User.UserID;
        private string keyWord = Env.User.Keyword;
        private DataTable gridTable;
        private DataTable detailTable;

        /// <summary>
        /// Initializes a new instance of the <see cref="P04_Import"/> class.
        /// </summary>
        public P04_Import()
        {
            this.InitializeComponent();
            this.txtCutCell.MDivisionID = this.keyWord;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            DBProxy.Current.Select(null, "Select 0 as Sel, '' as poid,'' as cuttingid,'' as brandid,'' as styleid,'' as cutcellid,'' as cutref ,'' as SpreadingNoID from cutplan where 1=0", out this.gridTable);
            base.OnFormLoaded();
            this.gridImport.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.gridImport.DataSource = this.gridTable;
            this.Helper.Controls.Grid.Generator(this.gridImport)
            .CheckBox("Sel", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
            .Text("POID", header: "PO#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("Cuttingid", header: "Cutting SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
            .Text("Brandid", header: "Brand", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("Styleid", header: "Style#", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Text("SpreadingNoID", header: "Spreading No", width: Widths.AnsiChars(5), iseditingreadonly: true)
            .Text("Cutcellid", header: "Cut Cell", width: Widths.AnsiChars(2), iseditingreadonly: true)
            .Text("CutRef", header: "CutRef#", width: Widths.AnsiChars(40), iseditingreadonly: true);
            this.gridImport.Columns["Sel"].DefaultCellStyle.BackColor = Color.Pink;

            // 預設MDivision = 使用者登入的 MDivision
            this.txtSpreadingNo.MDivision = Env.User.Keyword;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.dateEstCutDate.Value))
            {
                MyUtility.Msg.WarningBox("<Est. Cut Date> can not be empty.");
                return;
            }

            this.gridTable.Rows.Clear();  // 開始查詢前先清空資料
            string factory = this.txtfactory.Text;
            string estcutdate = this.dateEstCutDate.Text;
            string cutcellid = this.txtCutCell.Text;
            string spreadingNo = this.txtSpreadingNo.Text;
            string sqlcmd = $@"Select 
a.*
,'' as orderid_b
,'' as article_b
, '' as sizecode
,'' as sewinglineid
,1 as sel 
from Workorder a
where (cutplanid='' or cutplanid is null) 
and cutcellid!='' 
and a.CutRef != ''  
and mDivisionid ='{this.keyWord}' 
and estcutdate = '{estcutdate}'";

            if (!MyUtility.Check.Empty(cutcellid))
            {
                sqlcmd += string.Format(" and cutcellid = '{0}'", cutcellid);
            }

            if (!MyUtility.Check.Empty(factory))
            {
                sqlcmd += string.Format(" and a.factoryid = '{0}'", factory);
            }

            if (!MyUtility.Check.Empty(spreadingNo))
            {
                sqlcmd += string.Format(" and a.spreadingNoID = '{0}'", spreadingNo);
            }

            DualResult dResult = DBProxy.Current.Select(null, sqlcmd, out this.detailTable);
            if (dResult)
            {
                if (this.detailTable.Rows.Count != 0)
                {
                    foreach (DataRow dr in this.detailTable.Rows)
                    {
                        if (MyUtility.Check.Seek(string.Format("Select top(1) * from WorkOrder_Distribute where workorderukey='{0}' and orderid !='' and orderid !='Excess'", dr["Ukey"]), null, out DataRow queryRow))
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

                        // SpreadingNoID可能是DBNULL或空字串，對User來說都一樣，因此放進OR
                        string selwhere = $@"
cuttingid = '{dr["id"]}' 
and cutcellid ='{dr["cutcellid"]}' 
and (SpreadingNoID {(MyUtility.Check.Empty(dr["SpreadingNoID"]) ? "IS NULL OR SpreadingNoID = '' " : "='" + dr["SpreadingNoID"].ToString() + "'")})
";
                        DataRow[] griddray = this.gridTable.Select(selwhere);

                        if (griddray.Length == 0)
                        {
                            DataRow newdr = this.gridTable.NewRow();
                            newdr["sel"] = 1;
                            MyUtility.Check.Seek(string.Format("Select * from orders where id='{0}'", dr["ID"]), out DataRow ordersRow);
                            newdr["POID"] = ordersRow["poid"];
                            newdr["Cuttingid"] = dr["ID"];
                            newdr["brandid"] = ordersRow["brandid"];
                            newdr["Styleid"] = ordersRow["styleid"];
                            newdr["SpreadingNoID"] = dr["SpreadingNoID"];
                            newdr["Cutcellid"] = dr["cutcellid"];
                            newdr["cutref"] = dr["cutref"];
                            this.gridTable.Rows.Add(newdr);
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
                this.ShowErr(sqlcmd, dResult);
                return;
            }
        }

        /// <inheritdoc/>
        public List<string> ImportedIDs { get; set; } = new List<string>();

        private void BtnImport_Click(object sender, EventArgs e)
        {
            if (this.gridTable.Rows.Count == 0)
            {
                return;
            }

            this.ImportedIDs.Clear();
            DataRow[] importay;

            string id = MyUtility.GetValue.GetID(this.keyWord + "CP", "Cutplan");
            int idnum = MyUtility.Convert.GetInt(id.Substring(id.Length - 4));

            string iu = string.Empty;
            try
            {
                string remark;
                foreach (DataRow dr in this.gridTable.Rows)
                {
                    remark = string.Empty;
                    if (dr["sel"].ToString() == "1")
                    {
                        if (!string.IsNullOrWhiteSpace(id))
                        {
                            iu += string.Format(
                                @"
insert into Cutplan(id,cuttingid,mDivisionid,CutCellid,EstCutDate,Status,AddName,AddDate,POID,SpreadingNoID) Values('{0}','{1}','{2}','{3}','{4}','{5}','{6}',GetDate(),'{7}','{8}');
",
                                id,
                                dr["CuttingID"],
                                this.keyWord,
                                dr["cutcellid"],
                                this.dateEstCutDate.Text,
                                "New",
                                this.loginID,
                                dr["POId"],
                                dr["SpreadingNoID"]);

                            importay = this.detailTable.Select($@"id = '{dr["CuttingID"]}' 
and cutcellid = '{dr["cutcellid"]}' 
and (SpreadingNoID {(MyUtility.Check.Empty(dr["SpreadingNoID"]) ? "IS NULL OR SpreadingNoID = '' " : "='" + dr["SpreadingNoID"].ToString() + "'")})");

                            this.ImportedIDs.Add(id);
                            if (importay.Length > 0)
                            {
                                foreach (DataRow ddr in importay)
                                {
                                    DualResult result = DBProxy.Current.Select(null, string.Format("Select * from WorkOrder_Distribute Where workorderukey = '{0}'", ddr["Ukey"]), out DataTable cutplan_DetailTb);
                                    if (!result)
                                    {
                                        this.ShowErr(result);
                                        return;
                                    }

                                    if (cutplan_DetailTb != null)
                                    {
                                        remark = string.Empty;  // 組remark前先清空
                                        foreach (DataRow cdr in cutplan_DetailTb.Rows)
                                        {
                                            remark = remark + cdr["Orderid"].ToString().Trim() + "\\" + cdr["Article"].ToString().Trim() + "\\" + cdr["SizeCode"].ToString().Trim() + "\\" + cdr["Qty"].ToString() + ",";
                                        }
                                    }

                                    iu += string.Format(
                                        @"
insert into Cutplan_Detail(ID,Sewinglineid,cutref,cutno,orderid,styleid,colorid,cons,WorkOrderUkey,POID,Remark) values('{0}','{1}','{2}',{3},'{4}','{5}','{6}',{7},'{8}','{9}','{10}');
",
                                        id,
                                        ddr["Sewinglineid"],
                                        ddr["Cutref"],
                                        ddr["Cutno"],
                                        ddr["OrderID"],
                                        dr["styleid"],
                                        ddr["Colorid"],
                                        ddr["Cons"],
                                        ddr["Ukey"],
                                        dr["POID"],
                                        remark);

                                    // 直接以 workOrder.Ukey 寫回
                                    iu += $@" update Workorder set CutplanID = '{id}' where Ukey = {ddr["Ukey"]} ; ";
                                }
                            }

                            idnum++;
                            id = id.Substring(0, id.Length - 4) + idnum.ToString().PadLeft(4, '0');
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.ShowErr(ex);
                this.ImportedIDs.Clear();
                return;
            }

            #region transaction
            DualResult upResult;
            TransactionScope transactionscope = new TransactionScope();
            using (transactionscope)
            {
                if (upResult = DBProxy.Current.Execute(null, iu))
                {
                    transactionscope.Complete();
                }
            }

            if (!upResult)
            {
                this.ShowErr(upResult);
            }
            else
            {
                MyUtility.Msg.WarningBox("Successfully");
            }

            #endregion
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
}
