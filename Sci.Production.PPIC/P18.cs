using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Win.Tems;
using Ict.Win;
using Sci.Data;
using System.Linq;
using System.Transactions;
using System.Data.SqlClient;

namespace Sci.Production.PPIC
{
    public partial class P18 : Sci.Win.Tems.Input6
    {
        private DualResult result;
        public P18(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = string.Format($"MDivisionID='{Sci.Env.User.Keyword}'");
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();
            this.DetailSelectCommand = $@"
select 
[Brand] = o.BrandID
,[SewingLine] = o.SewLine
,[OrderID] = a2.OrderID
,[Style] = o.StyleID
,[CustPoNo] = o.CustPONo
,[Destination] = c.Alias
,[Qty] = oq.Qty
,[Seq] = a2.OrderShipmodeSeq
,[ShipMode] = a2.ShipModeID
,[BuyerDelivery] = oq.BuyerDelivery
,[TotalCrtns] = Packing.qty
,[AccLacking] = a3.RefNo
,a.*,a2.*
from avo a
left join AVO_Detail a2 on a.ID=a2.ID
left join Orders o on a2.OrderID=o.ID
left join Order_QtyShip oq on oq.Id=o.ID and oq.ShipmodeID=a2.ShipModeID
	and oq.Seq=a2.OrderShipmodeSeq
left join Country c on c.ID=o.Dest
outer apply(
	select SUM(PD.CTNQty) qty
	from PackingList_Detail PD 
	LEFT JOIN PackingList P ON PD.ID=P.ID
	where PD.orderid= a2.OrderID AND P.ShipModeID= a2.ShipModeID 
	AND PD.OrderShipmodeSeq = a2.OrderShipmodeSeq
)Packing
outer apply(
	select RefNo = STUFF((
		select concat(',',RefNo)
		from(
			select distinct Refno
			from AVO_Detail_RefNo
			where AVO_DetailUkey=a2.Ukey
		) s
	for xml path ('')
	) ,1,1,'')
)a3
where a.id='{masterID}'
";
            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override void OnDetailGridSetup()
        {
            #region SP#
            Ict.Win.DataGridViewGeneratorTextColumnSettings col_SP = new DataGridViewGeneratorTextColumnSettings();
            col_SP.CellValidating += (s, e) =>
             {
                 if (CurrentDetailData == null) return;
                 string oldvalue = MyUtility.Convert.GetString(CurrentDetailData["OrderID"]);
                 string newvalue = MyUtility.Convert.GetString(e.FormattedValue);
                 if (oldvalue == newvalue) return;
                 
                 if (this.EditMode && e.FormattedValue.ToString() !="")
                 {
                     if (MyUtility.Check.Seek($@"select 1 from Orders where id='{e.FormattedValue}'
and MDivisionID='{Sci.Env.User.Keyword}'"))
                     {
                         DataTable dt;
                         string sqlcmd = $@"
SELECT Seq,ShipmodeID as 'ShipMode'
FROM Order_QtyShip
WHERE ID= '{e.FormattedValue}'
";
                         
                         if (!(result = DBProxy.Current.Select("", sqlcmd, out dt)))
                         {
                             ShowErr(result);
                             return;
                         }
                         else
                         {
                             if (dt.Rows.Count==1)
                             {
                                 CurrentDetailData["OrderShipmodeSeq"] = dt.Rows[0]["Seq"].ToString();
                                 CurrentDetailData["ShipModeID"] = dt.Rows[0]["ShipMode"].ToString();
                             }

                             CurrentDetailData["OrderID"] = e.FormattedValue;
                             CurrentDetailData.EndEdit();
                         }
                     }
                     else
                     {
                         MyUtility.Msg.WarningBox("Data not found!");
                         CurrentDetailData["OrderShipmodeSeq"] = "";
                         CurrentDetailData["ShipModeID"] ="";
                         CurrentDetailData["OrderID"] = "";
                         CurrentDetailData.EndEdit();
                         return;
                     }
                 }
             };
            #endregion

            #region SEQ
            Ict.Win.DataGridViewGeneratorTextColumnSettings col_Seq = new DataGridViewGeneratorTextColumnSettings();
            col_Seq.CellValidating += (s, e) =>
            {
                if (CurrentDetailData == null) return;
                string oldvalue = MyUtility.Convert.GetString(CurrentDetailData["OrderShipmodeSeq"]);
                string newvalue = MyUtility.Convert.GetString(e.FormattedValue);
                if (oldvalue == newvalue) return;

                if (this.EditMode && e.FormattedValue.ToString() != "")
                {
                    if (MyUtility.Check.Seek($@"
select 1 
from Order_QtyShip where id='{CurrentDetailData["OrderID"]} and seq='{e.FormattedValue}''"))
                    {
                        CurrentDetailData["OrderShipmodeSeq"] = e.FormattedValue.ToString();
                        CurrentDetailData.EndEdit();
                    }
                    else
                    {
                        MyUtility.Msg.WarningBox("Data not found!");
                        CurrentDetailData["OrderShipmodeSeq"] = "";
                        CurrentDetailData.EndEdit();
                        return;
                    }
                }
            };

            col_Seq.EditingMouseDown += (s, e) =>
            {
                if (CurrentDetailData == null) return;
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    Sci.Win.Tools.SelectItem sel = new Win.Tools.SelectItem($@"
SELECT Seq,ShipmodeID as 'ShipMode'
FROM Order_QtyShip
WHERE ID = '{CurrentDetailData["OrderID"]}'", "Seq,ShipMode", CurrentDetailData["OrderShipmodeSeq"].ToString(), null);
                    DialogResult res = sel.ShowDialog();
                    if (res == DialogResult.Cancel) return;
                    CurrentDetailData["OrderShipmodeSeq"] = sel.GetSelecteds()[0]["seq"];
                    CurrentDetailData["ShipModeID"] = sel.GetSelecteds()[0]["ShipMode"];
                    CurrentDetailData.EndEdit();
                }
            };
            #endregion

            #region Ship Mode
            Ict.Win.DataGridViewGeneratorTextColumnSettings col_ShipMode = new DataGridViewGeneratorTextColumnSettings();
            col_ShipMode.CellValidating += (s, e) =>
            {
                if (CurrentDetailData == null) return;
                string oldvalue = MyUtility.Convert.GetString(CurrentDetailData["ShipModeID"]);
                string newvalue = MyUtility.Convert.GetString(e.FormattedValue);
                if (oldvalue == newvalue) return;

                if (this.EditMode && e.FormattedValue.ToString() != "")
                {
                    if (MyUtility.Check.Seek($@"
select 1 
from Order_QtyShip where id='{CurrentDetailData["OrderID"]} and ShipmodeID='{e.FormattedValue}''"))
                    {
                        CurrentDetailData["ShipModeID"] = e.FormattedValue.ToString();
                        CurrentDetailData.EndEdit();
                    }
                    else
                    {
                        MyUtility.Msg.WarningBox("Data not found!");
                        CurrentDetailData["ShipModeID"] = "";
                        CurrentDetailData.EndEdit();
                        return;
                    }
                }
            };

            col_ShipMode.EditingMouseDown += (s, e) =>
            {
                if (CurrentDetailData == null) return;
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    Sci.Win.Tools.SelectItem sel = new Win.Tools.SelectItem($@"
SELECT Seq,ShipmodeID as 'ShipMode'
FROM Order_QtyShip
WHERE ID = '{CurrentDetailData["OrderID"]}'", "Seq,ShipMode", CurrentDetailData["OrderShipmodeSeq"].ToString(), null);
                    DialogResult res = sel.ShowDialog();
                    if (res == DialogResult.Cancel) return;
                    CurrentDetailData["OrderShipmodeSeq"] = sel.GetSelecteds()[0]["seq"];
                    CurrentDetailData["ShipModeID"] = sel.GetSelecteds()[0]["ShipMode"];
                    CurrentDetailData.EndEdit();
                }
            };
            #endregion

            #region AccLacking
            Ict.Win.DataGridViewGeneratorTextColumnSettings col_acc = new DataGridViewGeneratorTextColumnSettings();

            col_acc.CellValidating += (s, e) =>
            {
                if (CurrentDetailData == null) return;
                string oldvalue = MyUtility.Convert.GetString(CurrentDetailData["AccLacking"]);
                string newvalue = MyUtility.Convert.GetString(e.FormattedValue);
                if (oldvalue == newvalue) return;
                
                if (this.EditMode && e.FormattedValue.ToString() != "")
                {
                    CurrentDetailData["AccLacking"] = e.FormattedValue;
                    string sqlcmd = $@"
SELECT DISTINCT PSD.Refno
FROM PO_Supp_Detail PSD
LEFT JOIN Fabric F ON F.SCIRefno = PSD.SCIRefno
LEFT JOIN MtlType M ON M.ID=F.MtlTypeID
LEFT JOIN ORDERS O ON O.POID=PSD.ID
WHERE O.id ='{CurrentDetailData["OrderID"]}'
  AND PSD.FabricType='A'
  AND PSD.Junk=0
  AND M.ProductionType='Packing'
ORDER BY PSD.Refno ";
                    DataTable dt;
                    DBProxy.Current.Select(null, sqlcmd, out dt);
                    bool selectRefno = true;
                    string[] getRefno = CurrentDetailData["AccLacking"].ToString().Split(',').Distinct().ToArray();
                    List<string> errRefno = new List<string>();
                    List<string> trueRefno = new List<string>();
                    foreach (string refno in getRefno)
                    {
                        if (!dt.AsEnumerable().Any(row => row["Refno"].EqualString(refno)) && !(refno.EqualString("")))
                        {
                            selectRefno &= false;
                            errRefno.Add(refno);
                        }
                        else if (!(refno.EqualString("")))
                        {
                            trueRefno.Add(refno);
                        }
                    }

                    if (!selectRefno)
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("Acc Lacking : " + string.Join(",", (errRefno).ToArray()) + " Data not found!!", "Data not found");
                    }
                    trueRefno.Sort();
                    CurrentDetailData["AccLacking"] = string.Join(",", (trueRefno).ToArray());
                    CurrentDetailData.EndEdit();
                }
            };

            col_acc.EditingMouseDown += (s, e) =>
             {
                 if (CurrentDetailData == null) return;
                 if (this.EditMode && e.Button == MouseButtons.Right)
                 {
                     Sci.Win.Tools.SelectItem2 item = new Win.Tools.SelectItem2($@"
SELECT DISTINCT PSD.Refno
FROM PO_Supp_Detail PSD
LEFT JOIN Fabric F ON F.SCIRefno = PSD.SCIRefno
LEFT JOIN MtlType M ON M.ID=F.MtlTypeID
LEFT JOIN ORDERS O ON O.POID=PSD.ID
WHERE O.id ='{CurrentDetailData["OrderID"]}'
  AND PSD.FabricType='A'
  AND PSD.Junk=0
  AND M.ProductionType='Packing'
ORDER BY PSD.Refno ", "Refno", CurrentDetailData["AccLacking"].ToString());
                     DialogResult result = item.ShowDialog();
                     if (result == DialogResult.Cancel) { return; }
                     CurrentDetailData["AccLacking"] = item.GetSelectedString();
                     CurrentDetailData.EndEdit();
                 }
             };
            #endregion
            #region 欄位設定
            Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("Brand", header: "Brand", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("SewingLine", header: "Sewing Line", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("OrderID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: false, settings: col_SP)
                .Text("Style", header: "Style", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("CustPoNo", header: "PO#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(10), iseditingreadonly: true, decimal_places: 2, integer_places: 7)
                .Text("OrderShipmodeSeq", header: "Seq", width: Widths.AnsiChars(13), iseditingreadonly: false, settings: col_Seq)
                .Text("ShipModeID", header: "Ship Mode", width: Widths.AnsiChars(13), iseditingreadonly: false, settings: col_ShipMode)
                .Date("BuyerDelivery", header: "Buyer Delivery", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Numeric("TotalCrtns", header: "Total Crtns", width: Widths.AnsiChars(10), iseditingreadonly: true, decimal_places: 2, integer_places: 7)
                .Text("AccLacking", header: "Acc Lacking", width: Widths.AnsiChars(13), iseditingreadonly: false)
                ;
            #endregion
            base.OnDetailGridSetup();
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.labStatus.Text = MyUtility.GetValue.Lookup($@"SELECT DD.Name FROM DropDownList DD WHERE DD.Type ='Pms_AVOStatus' and DD.ID= '''{CurrentMaintain["Status"]}'''");
        }

        // 新增時預設資料
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["Status"] = "New";
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
            CurrentMaintain["Hanle"] = Sci.Env.User.UserID;
            CurrentMaintain["cDate"] = DateTime.Now.ToString("yyyy/MM/dd");
        }

        protected override void ClickSend()
        {
            CurrentMaintain["SupApvName"] = Sci.Env.User.UserID;
            CurrentMaintain["SupApvDate"] = DateTime.Now;
            base.ClickSend();
        }

        protected override void ClickRecall()
        {
            CurrentMaintain["SupApvName"] = "";
            CurrentMaintain["SupApvDate"] = DBNull.Value;
            base.ClickRecall();
        }

        protected override void ClickCheck()
        {
            CurrentMaintain["PPDApvName"] = Sci.Env.User.UserID;
            CurrentMaintain["PPDApvDate"] = DateTime.Now;
            base.ClickCheck();
        }

        protected override void ClickUncheck()
        {
            CurrentMaintain["PPDApvName"] = "";
            CurrentMaintain["PPDApvDate"] = DBNull.Value;
            base.ClickUncheck();
        }

        protected override void ClickConfirm()
        {
            CurrentMaintain["ProdApvName"] = Sci.Env.User.UserID;
            CurrentMaintain["ProdApvDate"] = DateTime.Now;
            base.ClickConfirm();
        }

        protected override void ClickUnconfirm()
        {
            CurrentMaintain["ProdApvName"] = "";
            CurrentMaintain["ProdApvDate"] = DBNull.Value;
            base.ClickUnconfirm();
        }

        // Status <> New 不能編輯
        protected override bool ClickEdit()
        {
            if (CurrentMaintain["Status"].ToString() !="New")
            {
                MyUtility.Msg.WarningBox("The record status is not new, can't modify !!");
                return false;
            }
            return base.ClickEdit();
        }

        protected override bool ClickSaveBefore()
        {
            #region 存檔檢查不可為空

            #region 表頭檢查
            if (MyUtility.Check.Empty(CurrentMaintain["cDate"]))
            {
                MyUtility.Msg.WarningBox("<Issue Date> cannot be empty!");
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["FactoryID"]))
            {
                MyUtility.Msg.WarningBox("Factory cannot be empty!");
                return false;
            }

            if (MyUtility.Check.Empty(CurrentMaintain["Handle"]))
            {
                MyUtility.Msg.WarningBox("<Handle> cannot be empty!");
                return false;
            }
            #endregion

            #region 表身檢查

            #endregion

            #endregion

            #region 取單號
            if (this.IsDetailInserting)
            {
                string tmpID = Sci.MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "AV", "AVO", DateTime.Now);
                if (MyUtility.Check.Empty(tmpID))
                {
                    MyUtility.Msg.WarningBox("Get document ID fail!");
                    return false;
                }

                CurrentMaintain["ID"] = tmpID;
            }
            #endregion
            return base.ClickSaveBefore();
        }

        protected override DualResult ClickSave()
        {
            return base.ClickSave();
            string sqlcmd = "";
            foreach (DataRow dr in DetailDatas)
            {
                string[] splitRefno = dr["AccLacking"].ToString().Split(',').Distinct().ToArray();
                foreach (string refno in splitRefno)
                {
                    sqlcmd += $@"
insert into AVO_Detail_RefNo (AVO_DetailUkey,RefNo)
values({dr["Ukey"]},'{refno}')";
                }
            }

            TransactionScope _transactionscope = new TransactionScope();
            SqlConnection sqlConn = null;
            DBProxy.Current.OpenConnection(null, out sqlConn);
            using (_transactionscope)
            using (sqlConn)
            {
                try
                {
                    if (!(result = DBProxy.Current.Execute(null, sqlcmd)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlcmd, result);
                        return result;
                    }
                    _transactionscope.Complete();
                    _transactionscope.Dispose();
                    MyUtility.Msg.InfoBox("Confirmed successful");
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    ShowErr("Commit transaction error.", ex);
                    return result;
                }
                finally
                {
                    _transactionscope.Dispose();
                }
            }
        }
    }
}
