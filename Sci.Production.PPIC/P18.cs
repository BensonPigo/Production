﻿using System;
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
using Sci.Win.Tools;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using Sci.Win;
using System.Reflection;

namespace Sci.Production.PPIC
{
    public partial class P18 : Sci.Win.Tems.Input8
    {
        private DualResult result;
        private string Excelfile;

        public P18(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.DefaultFilter = string.Format($"MDivisionID='{Sci.Env.User.Keyword}'");
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
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
,[Destination] = c.Alias
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

        protected override DualResult ConvertSubDetailDatasFromDoSubForm(SubDetailConvertFromEventArgs e)
        {
            return base.ConvertSubDetailDatasFromDoSubForm(e);
        }

        protected override void OnDetailGridSetup()
        {
            #region SP#
            Ict.Win.DataGridViewGeneratorTextColumnSettings col_SP = new DataGridViewGeneratorTextColumnSettings();
            col_SP.CellValidating += (s, e) =>
             {
                 if (this.CurrentDetailData == null) return;
                 string oldvalue = MyUtility.Convert.GetString(this.CurrentDetailData["OrderID"]);
                 string newvalue = MyUtility.Convert.GetString(e.FormattedValue);
                 if (oldvalue == newvalue) return;

                 if (this.EditMode && e.FormattedValue.ToString() != string.Empty)
                 {
                     DataRow dr;
                     if (MyUtility.Check.Seek($@"
select o.*,[Destination] = c.Alias from Orders o
inner join Country c on c.ID= o.Dest
where o.id='{e.FormattedValue}'
and o.MDivisionID='{Sci.Env.User.Keyword}'", out dr))
                     {
                         DataTable dt;
                         string sqlcmd = $@"
SELECT Seq,ShipmodeID as 'ShipMode',oq.Qty,BuyerDelivery
,[TotalCrtns] = packing.qty
FROM Order_QtyShip oq
outer apply(
	select SUM(PD.CTNQty) qty
	from PackingList_Detail PD 
	LEFT JOIN PackingList P ON PD.ID=P.ID
	where PD.orderid= oq.ID AND P.ShipModeID= oq.ShipmodeID 
	AND PD.OrderShipmodeSeq = oq.Seq
)Packing
WHERE oq.ID= '{e.FormattedValue}'
";

                         if (!(this.result = DBProxy.Current.Select(string.Empty, sqlcmd, out dt)))
                         {
                             this.ShowErr(this.result);
                             return;
                         }
                         else
                         {
                             if (dt.Rows.Count == 1)
                             {
                                 this.CurrentDetailData["OrderShipmodeSeq"] = dt.Rows[0]["Seq"].ToString();
                                 this.CurrentDetailData["ShipModeID"] = dt.Rows[0]["ShipMode"].ToString();
                                 this.CurrentDetailData["Qty"] = dt.Rows[0]["Qty"].ToString();
                                 this.CurrentDetailData["BuyerDelivery"] = dt.Rows[0]["BuyerDelivery"].ToString();
                                 this.CurrentDetailData["TotalCrtns"] = dt.Rows[0]["TotalCrtns"].ToString();
                             }
                         }

                         this.CurrentDetailData["Brand"] = dr["BrandID"];
                         this.CurrentDetailData["SewingLine"] = dr["SewLine"];
                         this.CurrentDetailData["Style"] = dr["StyleID"];
                         this.CurrentDetailData["CustPoNo"] = dr["CustPONo"];
                         this.CurrentDetailData["OrderID"] = e.FormattedValue;
                         this.CurrentDetailData["Destination"] = dr["Destination"];
                         this.CurrentDetailData.EndEdit();
                     }
                     else
                     {
                         MyUtility.Msg.WarningBox("Data not found!");
                         this.CurrentDetailData["Brand"] = string.Empty;
                         this.CurrentDetailData["SewingLine"] = string.Empty;
                         this.CurrentDetailData["OrderID"] = string.Empty;
                         this.CurrentDetailData["Style"] = string.Empty;
                         this.CurrentDetailData["CustPoNo"] = string.Empty;
                         this.CurrentDetailData["Qty"] = 0;
                         this.CurrentDetailData["OrderShipmodeSeq"] = string.Empty;
                         this.CurrentDetailData["ShipModeID"] = string.Empty;
                         this.CurrentDetailData["BuyerDelivery"] = DBNull.Value;
                         this.CurrentDetailData["TotalCrtns"] = 0;
                         this.CurrentDetailData["AccLacking"] = string.Empty;
                         this.CurrentDetailData["Destination"] = string.Empty;
                         this.CurrentDetailData.EndEdit();
                         return;
                     }
                 }
                 else if (this.EditMode && e.FormattedValue.ToString() == string.Empty)
                 {
                     this.CurrentDetailData["Brand"] = string.Empty;
                     this.CurrentDetailData["SewingLine"] = string.Empty;
                     this.CurrentDetailData["OrderID"] = string.Empty;
                     this.CurrentDetailData["Style"] = string.Empty;
                     this.CurrentDetailData["CustPoNo"] = string.Empty;
                     this.CurrentDetailData["Qty"] = 0;
                     this.CurrentDetailData["OrderShipmodeSeq"] = string.Empty;
                     this.CurrentDetailData["ShipModeID"] = string.Empty;
                     this.CurrentDetailData["BuyerDelivery"] = DBNull.Value;
                     this.CurrentDetailData["TotalCrtns"] = 0;
                     this.CurrentDetailData["AccLacking"] = string.Empty;
                     this.CurrentDetailData["Destination"] = string.Empty;
                 }
             };
            #endregion

            #region SEQ
            Ict.Win.DataGridViewGeneratorTextColumnSettings col_Seq = new DataGridViewGeneratorTextColumnSettings();
            col_Seq.CellValidating += (s, e) =>
            {
                if (this.CurrentDetailData == null) return;
                DataRow dr;
                if (this.EditMode && e.FormattedValue.ToString() != string.Empty)
                {
                    if (MyUtility.Check.Seek($@"
SELECT Seq,ShipmodeID as 'ShipMode',oq.Qty,BuyerDelivery
,[TotalCrtns] = packing.qty
FROM Order_QtyShip oq
outer apply(
	select SUM(PD.CTNQty) qty
	from PackingList_Detail PD 
	LEFT JOIN PackingList P ON PD.ID=P.ID
	where PD.orderid= oq.ID AND P.ShipModeID= oq.ShipmodeID 
	AND PD.OrderShipmodeSeq = oq.Seq
)Packing
where oq.id='{this.CurrentDetailData["OrderID"]}' and oq.seq='{e.FormattedValue}'", out dr))
                    {
                        this.CurrentDetailData["OrderShipmodeSeq"] = e.FormattedValue.ToString();
                        this.CurrentDetailData["Qty"] = dr["Qty"].ToString();
                        this.CurrentDetailData["BuyerDelivery"] = dr["BuyerDelivery"].ToString();
                        this.CurrentDetailData["TotalCrtns"] = dr["TotalCrtns"].ToString();
                        this.CurrentDetailData.EndEdit();
                    }
                    else
                    {
                        MyUtility.Msg.WarningBox("Data not found!");
                        this.CurrentDetailData["OrderShipmodeSeq"] = string.Empty;
                        this.CurrentDetailData["Qty"] = 0;
                        this.CurrentDetailData["BuyerDelivery"] = DBNull.Value;
                        this.CurrentDetailData["TotalCrtns"] = 0;
                        this.CurrentDetailData.EndEdit();
                        return;
                    }
                }
                else if (this.EditMode && e.FormattedValue.ToString() == string.Empty)
                {
                    this.CurrentDetailData["OrderShipmodeSeq"] = string.Empty;
                    this.CurrentDetailData["Qty"] = 0;
                    this.CurrentDetailData["BuyerDelivery"] = DBNull.Value;
                    this.CurrentDetailData["TotalCrtns"] = 0;
                    this.CurrentDetailData.EndEdit();
                }
            };

            col_Seq.EditingMouseDown += (s, e) =>
            {
                if (this.CurrentDetailData == null) return;
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    Sci.Win.Tools.SelectItem sel = new Win.Tools.SelectItem($@"
SELECT Seq,ShipmodeID as 'ShipMode'
FROM Order_QtyShip
WHERE ID = '{this.CurrentDetailData["OrderID"]}'", "Seq,ShipMode", this.CurrentDetailData["OrderShipmodeSeq"].ToString(), null);
                    DialogResult res = sel.ShowDialog();
                    if (res == DialogResult.Cancel) return;
                    this.CurrentDetailData["OrderShipmodeSeq"] = sel.GetSelecteds()[0]["seq"];
                    this.CurrentDetailData["ShipModeID"] = sel.GetSelecteds()[0]["ShipMode"];
                    this.CurrentDetailData.EndEdit();
                }
            };
            #endregion

            #region Ship Mode
            Ict.Win.DataGridViewGeneratorTextColumnSettings col_ShipMode = new DataGridViewGeneratorTextColumnSettings();
            col_ShipMode.CellValidating += (s, e) =>
            {
                if (this.CurrentDetailData == null) return;

                if (this.EditMode && e.FormattedValue.ToString() != string.Empty)
                {
                    if (MyUtility.Check.Seek($@"
select 1 
from Order_QtyShip where id='{this.CurrentDetailData["OrderID"]}' and ShipmodeID='{e.FormattedValue}'"))
                    {
                        this.CurrentDetailData["ShipModeID"] = e.FormattedValue.ToString();
                        this.CurrentDetailData.EndEdit();
                    }
                    else
                    {
                        MyUtility.Msg.WarningBox("Data not found!");
                        this.CurrentDetailData["ShipModeID"] = string.Empty;
                        this.CurrentDetailData.EndEdit();
                        return;
                    }
                }
            };

            col_ShipMode.EditingMouseDown += (s, e) =>
            {
                if (this.CurrentDetailData == null) return;
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    Sci.Win.Tools.SelectItem sel = new Win.Tools.SelectItem($@"
SELECT Seq,ShipmodeID as 'ShipMode'
FROM Order_QtyShip
WHERE ID = '{this.CurrentDetailData["OrderID"]}'", "Seq,ShipMode", this.CurrentDetailData["OrderShipmodeSeq"].ToString(), null);
                    DialogResult res = sel.ShowDialog();
                    if (res == DialogResult.Cancel) return;
                    this.CurrentDetailData["OrderShipmodeSeq"] = sel.GetSelecteds()[0]["seq"];
                    this.CurrentDetailData["ShipModeID"] = sel.GetSelecteds()[0]["ShipMode"];
                    this.CurrentDetailData.EndEdit();
                }
            };
            #endregion

            #region AccLacking
            Ict.Win.DataGridViewGeneratorTextColumnSettings col_acc = new DataGridViewGeneratorTextColumnSettings();

            col_acc.CellValidating += (s, e) =>
            {
                if (this.CurrentDetailData == null) return;

                if (this.EditMode && e.FormattedValue.ToString() != string.Empty)
                {
                    this.CurrentDetailData["AccLacking"] = e.FormattedValue;
                    string sqlcmd = $@"
SELECT DISTINCT PSD.Refno
FROM PO_Supp_Detail PSD
LEFT JOIN Fabric F ON F.SCIRefno = PSD.SCIRefno
LEFT JOIN MtlType M ON M.ID=F.MtlTypeID
LEFT JOIN ORDERS O ON O.POID=PSD.ID
WHERE O.id ='{this.CurrentDetailData["OrderID"]}'
  AND PSD.FabricType='A'
  AND PSD.Junk=0
ORDER BY PSD.Refno ";
                    DataTable dt;
                    DBProxy.Current.Select(null, sqlcmd, out dt);
                    bool selectRefno = true;
                    string[] getRefno = this.CurrentDetailData["AccLacking"].ToString().Split(',').Distinct().ToArray();
                    List<string> errRefno = new List<string>();
                    List<string> trueRefno = new List<string>();
                    foreach (string refno in getRefno)
                    {
                        if (!dt.AsEnumerable().Any(row => row["Refno"].EqualString(refno)) && !(refno.EqualString(string.Empty)))
                        {
                            selectRefno &= false;
                            errRefno.Add(refno);
                        }
                        else if (!(refno.EqualString(string.Empty)))
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
                    this.CurrentDetailData["AccLacking"] = string.Join(",", (trueRefno).ToArray());
                    this.CurrentDetailData.EndEdit();
                }
            };

            col_acc.EditingMouseDown += (s, e) =>
             {
                 if (this.CurrentDetailData == null) return;
                 if (this.EditMode && e.Button == MouseButtons.Right)
                 {
                     Sci.Win.Tools.SelectItem2 item = new Win.Tools.SelectItem2($@"
SELECT DISTINCT PSD.Refno
FROM PO_Supp_Detail PSD
LEFT JOIN Fabric F ON F.SCIRefno = PSD.SCIRefno
LEFT JOIN MtlType M ON M.ID=F.MtlTypeID
LEFT JOIN ORDERS O ON O.POID=PSD.ID
WHERE O.id ='{this.CurrentDetailData["OrderID"]}'
  AND PSD.FabricType='A'
  AND PSD.Junk=0
ORDER BY PSD.Refno ", "Refno", this.CurrentDetailData["AccLacking"].ToString());
                     DialogResult result = item.ShowDialog();
                     if (result == DialogResult.Cancel) { return; }
                     this.CurrentDetailData["AccLacking"] = item.GetSelectedString();
                     this.CurrentDetailData.EndEdit();
                 }
             };
            #endregion

            #region 欄位設定
            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("Brand", header: "Brand", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("SewingLine", header: "Sewing Line", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("OrderID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: false, settings: col_SP)
                .Text("Style", header: "Style", width: Widths.AnsiChars(11), iseditingreadonly: true)
                .Text("CustPoNo", header: "PO#", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(4), iseditingreadonly: true, decimal_places: 2, integer_places: 7)
                .Text("OrderShipmodeSeq", header: "Seq", width: Widths.AnsiChars(4), iseditingreadonly: false, settings: col_Seq)
                .Text("ShipModeID", header: "Ship Mode", width: Widths.AnsiChars(5), iseditingreadonly: false, settings: col_ShipMode)
                .Date("BuyerDelivery", header: "Buyer Delivery", width: Widths.AnsiChars(11), iseditingreadonly: true)
                .Numeric("TotalCrtns", header: "Total Crtns", width: Widths.AnsiChars(7), iseditingreadonly: true, decimal_places: 2, integer_places: 7)
                .Text("AccLacking", header: "Acc Lacking", width: Widths.AnsiChars(11), iseditingreadonly: false, settings: col_acc)
                .Text("Destination", header: "Destination", width: Widths.AnsiChars(10), iseditingreadonly: true)
                ;
            #endregion
            this.detailgrid.Columns["OrderID"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["OrderShipmodeSeq"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["ShipModeID"].DefaultCellStyle.BackColor = Color.Pink;
            this.detailgrid.Columns["AccLacking"].DefaultCellStyle.BackColor = Color.Pink;
            base.OnDetailGridSetup();
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.labStatus.Text = MyUtility.GetValue.Lookup($@"SELECT DD.Name FROM DropDownList DD WHERE DD.Type ='Pms_AVOStatus' and DD.ID= '''{this.CurrentMaintain["Status"]}'''");

            if (this.CurrentMaintain["Status"].ToString() != "Confirmed")
            {
                this.btnSendEMail.Enabled = false;
            }
            else
            {
                this.btnSendEMail.Enabled = true;
            }

            #region Confirmed name取值
            this.txtAddName.Text = this.CurrentMaintain["AddName"].ToString() + "-" + MyUtility.GetValue.Lookup($@"select name from pass1 where id='{this.CurrentMaintain["AddName"]}'") + " " + (MyUtility.Check.Empty(this.CurrentMaintain["AddDate"]) ? string.Empty : ((DateTime)this.CurrentMaintain["AddDate"]).ToString("yyyy/MM/dd hh:mm:ss"));
            this.txtSupApv.Text = this.CurrentMaintain["SupApvName"].ToString() + "-" + MyUtility.GetValue.Lookup($@"select name from pass1 where id='{this.CurrentMaintain["SupApvName"]}'") + " " + (MyUtility.Check.Empty(this.CurrentMaintain["SupApvDate"]) ? string.Empty : ((DateTime)this.CurrentMaintain["SupApvDate"]).ToString("yyyy/MM/dd hh:mm:ss"));
            this.txtPPDApv.Text = this.CurrentMaintain["PPDApvName"].ToString() + "-" + MyUtility.GetValue.Lookup($@"select name from pass1 where id='{this.CurrentMaintain["PPDApvName"]}'") + " " + (MyUtility.Check.Empty(this.CurrentMaintain["PPDApvDate"]) ? string.Empty : ((DateTime)this.CurrentMaintain["PPDApvDate"]).ToString("yyyy/MM/dd hh:mm:ss"));
            this.txtProdApv.Text = this.CurrentMaintain["ProdApvName"].ToString() + "-" + MyUtility.GetValue.Lookup($@"select name from pass1 where id='{this.CurrentMaintain["ProdApvName"]}'")
                + " " + (MyUtility.Check.Empty(this.CurrentMaintain["ProdApvDate"]) ? string.Empty : ((DateTime)this.CurrentMaintain["ProdApvDate"]).ToString("yyyy/MM/dd hh:mm:ss"));
            #endregion
        }

        // 新增時預設資料
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
            this.CurrentMaintain["Handle"] = Sci.Env.User.UserID;
            this.CurrentMaintain["cDate"] = DateTime.Now.ToString("yyyy/MM/dd");
        }

        protected override void ClickSend()
        {
            base.ClickSend();
            string updateCmd = $@"update AVO set Status = 'Sent', SupApvDate = GETDATE(), SupApvName = '{Sci.Env.User.UserID}', EditDate = GETDATE(),EditName='{Sci.Env.User.UserID}' where ID = '{this.CurrentMaintain["ID"]}'";
            DualResult result = DBProxy.Current.Execute(null, updateCmd);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Send data faile.\r\n" + result.ToString());
                return;
            }
            else
            {
                MyUtility.Msg.InfoBox("Successful!");
            }
        }

        protected override void ClickRecall()
        {
            base.ClickRecall();
            string updateCmd = $@"update AVO set Status = 'New', SupApvDate = null, SupApvName = '', EditDate = GETDATE(),EditName='{Sci.Env.User.UserID}' where ID = '{this.CurrentMaintain["ID"]}'";
            DualResult result = DBProxy.Current.Execute(null, updateCmd);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Send data faile.\r\n" + result.ToString());
                return;
            }
            else
            {
                MyUtility.Msg.InfoBox("Successful!");
            }
        }

        protected override void ClickCheck()
        {
            string updateCmd = $@"update AVO set Status = 'Checked', PPDApvDate = GetDate(), PPDApvName = '{Sci.Env.User.UserID}', EditDate = GETDATE(),EditName='{Sci.Env.User.UserID}' where ID = '{this.CurrentMaintain["ID"]}'";
            DualResult result = DBProxy.Current.Execute(null, updateCmd);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Send data faile.\r\n" + result.ToString());
                return;
            }
            else
            {
                MyUtility.Msg.InfoBox("Successful!");
            }

            base.ClickCheck();
        }

        protected override void ClickUncheck()
        {
            string updateCmd = $@"update AVO set Status = 'Sent', PPDApvDate = null, PPDApvName = '', EditDate = GETDATE(),EditName='{Sci.Env.User.UserID}' where ID = '{this.CurrentMaintain["ID"]}'";
            DualResult result = DBProxy.Current.Execute(null, updateCmd);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Send data faile.\r\n" + result.ToString());
                return;
            }
            else
            {
                MyUtility.Msg.InfoBox("Successful!");
            }

            base.ClickUncheck();
        }

        protected override void ClickConfirm()
        {
            string updateCmd = $@"update AVO set Status = 'Confirmed', ProdApvDate = GetDate(), ProdApvName = '{Sci.Env.User.UserID}', EditDate = GETDATE(),EditName='{Sci.Env.User.UserID}' where ID = '{this.CurrentMaintain["ID"]}'";
            DualResult result = DBProxy.Current.Execute(null, updateCmd);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Send data faile.\r\n" + result.ToString());
                return;
            }
            else
            {
                MyUtility.Msg.InfoBox("Successful!");
            }

            base.ClickConfirm();
        }

        protected override void ClickUnconfirm()
        {
            string updateCmd = $@"update AVO set Status = 'Checked', ProdApvDate = null, ProdApvName = '', EditDate = GETDATE(),EditName='{Sci.Env.User.UserID}' where ID = '{this.CurrentMaintain["ID"]}'";
            DualResult result = DBProxy.Current.Execute(null, updateCmd);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Send data faile.\r\n" + result.ToString());
                return;
            }
            else
            {
                MyUtility.Msg.InfoBox("Successful!");
            }

            base.ClickUnconfirm();
        }

        // Status <> New 不能編輯
        protected override bool ClickEdit()
        {
            if (this.CurrentMaintain["Status"].ToString() != "New")
            {
                MyUtility.Msg.WarningBox("The record status is not new, can't modify !!");
                return false;
            }
            return base.ClickEdit();
        }

        protected override bool ClickSaveBefore()
        {
            #region 存檔檢查不可為空

            // 表頭檢查
            if (MyUtility.Check.Empty(this.CurrentMaintain["cDate"]))
            {
                MyUtility.Msg.WarningBox("<Issue Date> cannot be empty!");
                return false;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Handle"]))
            {
                MyUtility.Msg.WarningBox("<Handle> cannot be empty!");
                return false;
            }

            // 表身檢查
            foreach (DataRow row in DetailDatas)
            {
                if (MyUtility.Check.Empty(row["OrderID"]))
                {
                    row.Delete();
                }
            }

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

                this.CurrentMaintain["ID"] = tmpID;
            }
            #endregion
            return base.ClickSaveBefore();
        }

        protected override DualResult ClickSave()
        {
            string sqlcmd = string.Empty;
            DataTable subDetailData;
            foreach (DataRow dr in this.DetailDatas)
            {
                string[] splitRefno = dr["AccLacking"].ToString().Split(',').Distinct().ToArray();
                if (this.GetSubDetailDatas(dr, out subDetailData))
                {
                    // 如果變更,先刪除第三層資料
                    if (dr.RowState == DataRowState.Modified)
                    {
                        foreach (DataRow sdr in subDetailData.Rows)
                        {
                            sdr.Delete();
                        }
                    }
                    // 變更或新增,就補上第三層資料
                    if (dr.RowState == DataRowState.Modified || dr.RowState == DataRowState.Added)
                    {
                        foreach (string refno in splitRefno)
                        {
                            DataRow drtmp = subDetailData.NewRow();
                            drtmp["Refno"] = refno;
                            drtmp["AVO_DetailUkey"] = dr["ukey"];
                            subDetailData.Rows.Add(drtmp);
                        }
                    }
                }
            }

            return base.ClickSave();
        }

        protected override bool ClickPrint()
        {
            IReportResource reportresource;
            ReportDefinition rd = new ReportDefinition();
            if (!(this.result = ReportResources.ByEmbeddedResource(Assembly.GetAssembly(this.GetType()), this.GetType(), "P18_Print.rdlc", out reportresource)))
            {
                MyUtility.Msg.ErrorBox(this.result.ToString());
            }
            else
            {
                string addName = MyUtility.GetValue.Lookup($"select dbo.getPass1('{this.CurrentMaintain["AddName"]}') ");
                string supApvName = MyUtility.GetValue.Lookup($"select dbo.getPass1('{this.CurrentMaintain["SupApvName"]}') ");
                string pPDApvName = MyUtility.GetValue.Lookup($"select dbo.getPass1('{this.CurrentMaintain["PPDApvName"]}') ");
                string prodApvName = MyUtility.GetValue.Lookup($"select dbo.getPass1('{this.CurrentMaintain["ProdApvName"]}') ");

                rd.ReportResource = reportresource;
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ID", this.CurrentMaintain["ID"].ToString()));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("cDate", (this.CurrentMaintain["cDate"] == DBNull.Value) ? string.Empty : ((DateTime)this.CurrentMaintain["cDate"]).ToString("yyyy/MM/dd")));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("MDivisionID", this.CurrentMaintain["MDivisionID"].ToString()));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Handle", this.CurrentMaintain["Handle"].ToString()));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("AddName", addName));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("SupApvName", supApvName));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("PPDApvName", pPDApvName));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("ProdApvName", prodApvName));
                rd.ReportParameters.Add(new Microsoft.Reporting.WinForms.ReportParameter("Remark", this.CurrentMaintain["Remark"].ToString()));

                // 傳 list 資料
                DataTable dt;
                string sqlcmd = $@"
select o.SewLine,o.SewInLine,a2.OrderID,o.CustPONo,o.StyleID,oq.Qty,SUBSTRING(a3.RefNo,2,len(a3.Refno)) as RefNo
,SUBSTRING(po3.FinalETA,2,len(po3.FinalETA)) as FinalETA,oq.BuyerDelivery,[VAS] = iif(o.VasShas=1,'Y','N'),c.Alias
from avo a
left join AVO_Detail a2 on a.ID=a2.ID
left join Orders o on a2.OrderID=o.ID
left join Order_QtyShip oq on oq.Id=o.ID and oq.ShipmodeID=a2.ShipModeID
	and oq.Seq=a2.OrderShipmodeSeq
left join Country c on c.ID=o.Dest
outer apply(
	select RefNo = STUFF((
		select concat(char(10)+',' ,RefNo)
		from(
			select distinct Refno
			from AVO_Detail_RefNo
			where AVO_DetailUkey=a2.Ukey
		) s order by RefNo
	for xml path ('')
	) ,1,1,'')
)a3
outer apply(
	select FinalETA = STUFF((
		select concat(char(10)+',',FinalETA)
		from ( 
		select * from (
			select distinct Refno,FinalETA ,row =  ROW_NUMBER() over(partition by refno order by finalETA asc)
			from PO_Supp_Detail
			where id = o.POID
			and Refno in ( 	select distinct Refno
			from AVO_Detail_RefNo
			where AVO_DetailUkey=a2.Ukey)
			and FinalETA is not null
			)s2 where row=1
		) s order by RefNo
	for xml path ('')
	) ,1,1,'')	
)po3
where a2.id ='{this.CurrentMaintain["id"]}'
";
                DualResult result = DBProxy.Current.Select(null, sqlcmd, out dt);
                if (!result)
                {
                    MyUtility.Msg.WarningBox("data faile.\r\n" + result.ToString());
                    return false;
                }

                List<P18_PrintData> data = dt.AsEnumerable()
                    .Select(row1 => new P18_PrintData()
                    {
                        SewLine = row1["SewLine"].ToString(),
                        SewInLine = (row1["SewInLine"] == DBNull.Value) ? string.Empty : ((DateTime)row1["SewInLine"]).ToString("yyyy/MM/dd"),
                        OrderID = row1["Orderid"].ToString(),
                        CustPONo = row1["CustPONo"].ToString(),
                        StyleID = row1["styleID"].ToString(),
                        Qty = MyUtility.Convert.GetInt(row1["Qty"]),
                        RefNo = row1["RefNo"].ToString(),
                        FinalETA = row1["FinalETA"].ToString(),
                        BuyerDelivery = (row1["BuyerDelivery"] == DBNull.Value) ? string.Empty : ((DateTime)row1["BuyerDelivery"]).ToString("yyyy/MM/dd"),
                        VasShas = row1["VAS"].ToString(),
                        Alias = row1["Alias"].ToString()
                    }).ToList();

                rd.ReportDataSource = data;
                rd.ReportResource = reportresource;
                var frm1 = new Sci.Win.Subs.ReportView(rd);
                frm1.MdiParent = this.MdiParent;
                frm1.TopMost = true;
                frm1.Show();
            }

            return base.ClickPrint();
        }

        private void ToExcel()
        {
            if (this.CurrentMaintain == null)
            {
                return;
            }

            DataTable dt;
            string sqlcmd = $@"
select o.SewLine,o.SewInLine,a2.OrderID,o.CustPONo,o.StyleID,oq.Qty,SUBSTRING(a3.RefNo,2,len(a3.Refno)) as RefNo
,SUBSTRING(po3.FinalETA,2,len(po3.FinalETA)) as FinalETA,oq.BuyerDelivery,[VAS] = iif(o.VasShas=1,'Y','N'),c.Alias
from avo a
left join AVO_Detail a2 on a.ID=a2.ID
left join Orders o on a2.OrderID=o.ID
left join Order_QtyShip oq on oq.Id=o.ID and oq.ShipmodeID=a2.ShipModeID
	and oq.Seq=a2.OrderShipmodeSeq
left join Country c on c.ID=o.Dest
outer apply(
	select RefNo = STUFF((
		select concat(char(10)+',' ,RefNo)
		from(
			select distinct Refno
			from AVO_Detail_RefNo
			where AVO_DetailUkey=a2.Ukey
		) s order by RefNo
	for xml path ('')
	) ,1,1,'')
)a3
outer apply(
	select FinalETA = STUFF((
		select concat(char(10)+',',FinalETA)
		from ( 
		select * from (
			select distinct Refno,FinalETA ,row =  ROW_NUMBER() over(partition by refno order by finalETA asc)
			from PO_Supp_Detail
			where id = o.POID
			and Refno in ( 	select distinct Refno
			from AVO_Detail_RefNo
			where AVO_DetailUkey=a2.Ukey)
			and FinalETA is not null
			)s2 where row=1
		) s order by RefNo
	for xml path ('')
	) ,1,1,'')	
)po3
where a2.id ='{this.CurrentMaintain["id"]}'
";
            DualResult result = DBProxy.Current.Select(null, sqlcmd,out dt);
            if (!result)
            {
                MyUtility.Msg.WarningBox("data faile.\r\n" + result.ToString());
                return;
            }

            Excel.Application objApp = new Excel.Application();
            Sci.Utility.Report.ExcelCOM com = new Sci.Utility.Report.ExcelCOM(Sci.Env.Cfg.XltPathDir + "\\PPIC_P18.xltx", objApp);

            Excel.Worksheet worksheet = objApp.Sheets[1];

            // 表頭
            worksheet.Cells[2, 2] = this.CurrentMaintain["id"];
            worksheet.Cells[2, 4] = ((DateTime)this.CurrentMaintain["cDate"]).ToString("yyyy/MM/dd");
            worksheet.Cells[2, 6] = this.CurrentMaintain["MdivisionID"];
            worksheet.Cells[2, 8] = this.CurrentMaintain["Handle"];
            worksheet.Cells[3, 2] = MyUtility.GetValue.Lookup($"select dbo.getPass1('{this.CurrentMaintain["AddName"]}') ");
            worksheet.Cells[3, 4] = MyUtility.GetValue.Lookup($"select dbo.getPass1('{this.CurrentMaintain["SupApvName"]}') ");
            worksheet.Cells[3, 6] = MyUtility.GetValue.Lookup($"select dbo.getPass1('{this.CurrentMaintain["PPDApvName"]}') ");
            worksheet.Cells[3, 10] = MyUtility.GetValue.Lookup($"select dbo.getPass1('{this.CurrentMaintain["ProdApvName"]}') ");
            worksheet.Cells[4, 3] = this.CurrentMaintain["Remark"];

            com.WriteTable(dt, 6);
            worksheet.get_Range($"A6:K{MyUtility.Convert.GetString(5 + dt.Rows.Count)}").Borders.LineStyle = Excel.XlLineStyle.xlContinuous; // 畫線
            com.ExcelApp.ActiveWorkbook.Sheets[1].Select(Type.Missing);
            objApp.Visible = false;
            objApp.Columns.AutoFit();
            objApp.Rows.AutoFit();
            this.Excelfile = Sci.Production.Class.MicrosoftFile.GetName("PPIC_P18");
            objApp.ActiveWorkbook.SaveAs(this.Excelfile);
            objApp.Quit();
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(objApp);

        }

        private void btnSendEMail_Click(object sender, EventArgs e)
        {
            DataRow dr;
            this.ToExcel();

            if (MyUtility.Check.Seek("select * from MailTo where id='017'", out dr))/////
            {
                var email = new MailTo(Sci.Env.Cfg.MailFrom, dr["ToAddress"].ToString(), dr["CCAddress"].ToString(), dr["Subject"].ToString(), this.Excelfile, dr["Content"].ToString(), false, false);
                email.ShowDialog(this);
            }
        }
    }
}