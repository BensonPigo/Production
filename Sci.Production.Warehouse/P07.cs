﻿using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    public partial class P07 : Sci.Win.Tems.Input6
    {
        private Dictionary<string, string> di_fabrictype = new Dictionary<string, string>();
        private Dictionary<string, string> di_stocktype = new Dictionary<string, string>();
        string UserID = Sci.Env.User.UserID;

        public P07(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = string.Format("Type='A'");
            ChangeDetailColor();
            di_fabrictype.Add("F", "Fabric");
            di_fabrictype.Add("A", "Accessory");
            di_fabrictype.Add("O", "Other");
            di_stocktype.Add("B", "Bulk");
            di_stocktype.Add("I", "Inventory");
            //
            detailgrid.StatusNotification += (s, e) =>
            {
                if (this.EditMode && e.Notification == Ict.Win.UI.DataGridViewStatusNotification.NoMoreRowOnEnterPress)
                {
                    DataRow tmp = detailgrid.GetDataRow(detailgrid.GetSelectedRowIndex());
                    this.OnDetailGridInsert();
                    DataRow newrow = detailgrid.GetDataRow(detailgrid.GetSelectedRowIndex());
                    newrow.ItemArray = tmp.ItemArray;
                    detailgrid.CurrentCell = detailgrid.Rows[detailgrid.RowCount - 1].Cells[0];
                }
            };
        }

        public P07(ToolStripMenuItem menuitem, string transID)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = string.Format("Type='A' and id='{0}'", transID);
            this.IsSupportNew = false;
            this.IsSupportEdit = false;
            this.IsSupportDelete = false;
            this.IsSupportConfirm = false;
            this.IsSupportUnconfirm = false;
            ChangeDetailColor();
            di_fabrictype.Add("F", "Fabric");
            di_fabrictype.Add("A", "Accessory");
            di_fabrictype.Add("O", "Other");
            di_stocktype.Add("B", "Bulk");
            di_stocktype.Add("I", "Inventory");
        }

        // 新增時預設資料
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
            CurrentMaintain["FactoryID"] = Sci.Env.User.Factory;
            CurrentMaintain["Third"] = 1;
            CurrentMaintain["Status"] = "New";
            CurrentMaintain["Type"] = "A";
            foreach (DataGridViewColumn index in detailgrid.Columns) { index.SortMode = DataGridViewColumnSortMode.NotSortable; }
        }
      

        private void ChangeDetailColor()
        {
            detailgrid.RowPostPaint += (s, e) =>
            {
                if (!this.EditMode)
                {
                    DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                    if (detailgrid.Rows.Count <= e.RowIndex || e.RowIndex < 0) return;

                    int i = e.RowIndex;
                    if (MyUtility.Check.Empty(dr["stocktype"]) || MyUtility.Check.Empty(dr["stockunit"]))
                    {
                        detailgrid.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(255, 192, 203);
                    }
                }
            };
        }

        // delete前檢查
        protected override bool ClickDeleteBefore()
        {
            if (CurrentMaintain["Status"].EqualString("CONFIRMED"))
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't delete.", "Warning");
                return false;
            }
            return base.ClickDeleteBefore();
        }

        // edit前檢查
        protected override bool ClickEditBefore()
        {
            //!EMPTY(APVName) OR !EMPTY(Closed)，只能編輯remark欄。
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
            DataTable result = null;
            StringBuilder warningmsg = new StringBuilder();

            #region 必輸檢查

            if (MyUtility.Check.Empty(CurrentMaintain["invno"]))
            {
                MyUtility.Msg.WarningBox("< Invoice# >  can't be empty!", "Warning");
                textBox3.Focus();
                return false;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["ETA"]))
            {
                MyUtility.Msg.WarningBox("< ETA >  can't be empty!", "Warning");
                dateBox1.Focus();
                return false;
            }

            #endregion 必輸檢查

            
            DateTime ArrivePortDate ;
            DateTime WhseArrival ;
            DateTime ETA;
            bool chk;
            String msg;

            if (!MyUtility.Check.Empty(dateBox2.Value) && !MyUtility.Check.Empty(CurrentMaintain["WhseArrival"]))
            {
                ArrivePortDate = DateTime.Parse(dateBox2.Text);//port
                WhseArrival = DateTime.Parse(CurrentMaintain["WhseArrival"].ToString());//warehouse
                // 到倉日不可早於到港日
                if (!(chk=Prgs.CheckArrivedWhseDateWithArrivedPortDate(ArrivePortDate, WhseArrival, out msg)))
                {
                    MyUtility.Msg.WarningBox(msg);
                    dateBox3.Focus();
                    return false;
                }
                if (DateTime.Compare(WhseArrival, ArrivePortDate.AddDays(20)) > 0)
                {
                    MyUtility.Msg.WarningBox("Arrive Warehouse date can't be later than arrive port 20 days!!");
                    dateBox3.Focus();
                    return false;
                }
            }

            if (!MyUtility.Check.Empty(CurrentMaintain["WhseArrival"]) && !MyUtility.Check.Empty(CurrentMaintain["eta"]))
            {
                ETA = DateTime.Parse(CurrentMaintain["eta"].ToString());//eta
                WhseArrival = DateTime.Parse(CurrentMaintain["WhseArrival"].ToString());//warehouse+

                // 到倉日如果早於ETA 3天，則提示窗請USER再確認是否存檔。
                // 到倉日如果晚於ETA 15天，則提示窗請USER再確認是否存檔。
                if (!(chk= Prgs.CheckArrivedWhseDateWithEta(ETA,WhseArrival,out msg)))
                {
                    DialogResult dResult = MyUtility.Msg.QuestionBox(msg);
                    if (dResult == DialogResult.No) return false;
                }
            }

            foreach (DataRow row in DetailDatas)
            {
                if (((MyUtility.Check.Empty(row["seq1"])) || MyUtility.Check.Empty(row["seq2"])) ||
                    (row["seq1"].ToString().TrimStart().StartsWith("7")) || 
                    (MyUtility.Check.Empty(row["ActualQty"])) || 
                    (MyUtility.Check.Empty(row["stocktype"])) || 
                    (row["fabrictype"].ToString().ToUpper() == "F" && (MyUtility.Check.Empty(row["roll"]) || MyUtility.Check.Empty(row["dyelot"])))
                   )
                {
                    warningmsg.Append(string.Format(@"SP#: {0} Seq#: {1}-{2} ", row["poid"], row["seq1"], row["seq2"]));  
                  
                    if ((MyUtility.Check.Empty(row["seq1"]) || MyUtility.Check.Empty(row["seq2"])))
                        warningmsg.Append(string.Format(@"Seq1 or Seq2 can't be empty ,"));

                    if (row["seq1"].ToString().TrimStart().StartsWith("7"))
                        warningmsg.Append(string.Format(@"Seq1 can't start with '7' ,"));

                    if (MyUtility.Check.Empty(row["ActualQty"]))
                        warningmsg.Append(string.Format(@"Actual Qty can't be empty ,"));

                    //565: WAREHOUSE_P07_Material Receiving，移除表身[stockunit]必填的檢核
                    //if (MyUtility.Check.Empty(row["stockunit"]))
                    //{
                    //    warningmsg.Append(string.Format(@"SP#: {0} Seq#: {1}-{2} Roll#:{3} Dyelot:{4} Stock Unit can't be empty"
                    //        , row["poid"], row["seq1"], row["seq2"], row["roll"], row["dyelot"]) + Environment.NewLine);
                    //}

                    if (MyUtility.Check.Empty(row["stocktype"]))
                        warningmsg.Append(string.Format(@"Stock Type can't be empty ,"));


                    if (row["fabrictype"].ToString().ToUpper() == "F" && (MyUtility.Check.Empty(row["roll"]) || MyUtility.Check.Empty(row["dyelot"])))
                        warningmsg.Append(string.Format(@"Roll and Dyelot can't be empty ,"));

                    warningmsg.Append(Environment.NewLine);
                }
                if (row["fabrictype"].ToString().ToUpper() != "F")
                {
                    row["roll"] = "";
                    row["dyelot"] = "";
                }
            }

            if (!MyUtility.Check.Empty(warningmsg.ToString()))
            {
                MyUtility.Msg.WarningBox(warningmsg.ToString());
                return false;
            }

            //Check Roll 是否已經存在
            if(!checkRoll())
                return false;

            if (!MyUtility.Check.Empty(DetailDatas) && DetailDatas.Count > 0)
            {
                MyUtility.Tool.ProcessWithDatatable(DetailDatas.CopyToDataTable(), "poid,seq1"
                , @"select #tmp.*
            from #tmp,dbo.po_supp WITH (NOLOCK) ,dbo.supp WITH (NOLOCK) 
            where #tmp.poid = dbo.po_supp.id
            and #tmp.seq1 = dbo.po_supp.seq1
            and dbo.po_supp.suppid = dbo.supp.id
            and dbo.supp.thirdcountry = 1 ", out result, "#tmp");

                if (!MyUtility.Check.Empty(result) && result.Rows.Count > 0)
                {
                    CurrentMaintain["third"] = 1;
                }
                else
                {
                    CurrentMaintain["third"] = 0;
                }
            }

            if (DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail can't be empty", "Warning");
                return false;
            }

            //取單號
            if (this.IsDetailInserting)
            {
                string tmpId = Sci.MyUtility.GetValue.GetID(Sci.Env.User.Factory + "PR", "Receiving", (DateTime)CurrentMaintain["ETA"]);
                if (MyUtility.Check.Empty(tmpId))
                {
                    MyUtility.Msg.WarningBox("Get document ID fail!!");
                    return false;
                }
                CurrentMaintain["id"] = tmpId;
            }

            return base.ClickSaveBefore();
        }

        // grid 加工填值
        protected override DualResult OnRenewDataDetailPost(RenewDataPostEventArgs e)
        {
            return base.OnRenewDataDetailPost(e);
        }

        //refresh
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            DataRow dr;
            if (!(CurrentMaintain == null))
            {
                dateBox2.Value = null;
                dateBox5.Value = null;
                if (MyUtility.Check.Seek(string.Format(@"select portarrival,docarrival from dbo.export WITH (NOLOCK) where id='{0}'"
                    , CurrentMaintain["exportid"]), out dr, null))
                {                                      
                    if (!MyUtility.Check.Empty(dr["portarrival"])) dateBox2.Value = DateTime.Parse(dr["portarrival"].ToString());
                    if (!MyUtility.Check.Empty(dr["docarrival"])) dateBox5.Value = DateTime.Parse(dr["docarrival"].ToString());
                }
                dateBox1.Enabled = (MyUtility.Check.Empty(CurrentMaintain["third"]) || CurrentMaintain["third"].ToString() == "True");
            }

            #region Status Label

            label25.Text = CurrentMaintain["status"].ToString();

            #endregion Status Label

            this.detailgrid.AutoResizeColumns();
        }

        DataGridViewColumn Col_ActualQty, Col_Location;
        protected override void OnDetailGridSetup()
        {
            #region SP# Vaild 判斷此sp#存在po中。

            Ict.Win.DataGridViewGeneratorTextColumnSettings ts4 = new DataGridViewGeneratorTextColumnSettings();
            DataRow dr;
            ts4.CellValidating += (s, e) =>
            {
                if (MyUtility.Check.Empty(CurrentMaintain["invno"]))
                {
                    MyUtility.Msg.WarningBox("< Invoice# >  can't be empty!", "Warning");
                    e.Cancel = true;
                    CurrentDetailData["poid"] = "";
                    textBox3.Focus();
                    return;
                }
                if (this.EditMode && e.FormattedValue.ToString()!="")
                {
                    if (MyUtility.Check.Seek(string.Format("select 1 where exists(select * from po WITH (NOLOCK) where id = '{0}')", e.FormattedValue), null))
                    {
                        string category = MyUtility.GetValue.Lookup(string.Format("select category from orders WITH (NOLOCK) where id='{0}'", e.FormattedValue));
                        if (category == "M")
                        {
                            CurrentDetailData["stocktype"] = "I";
                        }
                        else
                        {
                            CurrentDetailData["stocktype"] = "B";
                        }
                    }
                    else
                    {
                        MyUtility.Msg.WarningBox("SP# is not exist!!", "Data not found");
                        e.Cancel = true;
                        return;
                    }
                    CurrentDetailData["poid"] = e.FormattedValue;
                }
            };

            #endregion SP# Vaild 判斷此sp#的cateogry存在 order_tmscost

            #region Seq 右鍵開窗

            Ict.Win.DataGridViewGeneratorTextColumnSettings ts = new DataGridViewGeneratorTextColumnSettings();
            ts.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    DataTable poitems;
                    string sqlcmd = "";
                    IList<DataRow> x;
                    if (MyUtility.Check.Empty(CurrentMaintain["exportid"]))
                    {
                        Sci.Win.Tools.SelectItem selepoitem = Prgs.SelePoItem(CurrentDetailData["poid"].ToString(), CurrentDetailData["seq"].ToString(), "left(p.seq1,1) !='7'");
                        DialogResult result = selepoitem.ShowDialog();
                        if (result == DialogResult.Cancel) { return; }
                        x = selepoitem.GetSelecteds();
                    }
                    else
                    {
//                        --select e.poid,e.seq1+e.seq2 as seq, e.Refno, dbo.getmtldesc(e.poid,e.seq1,e.seq2,2,0) as [Description]
//--,p.ColorID
//--,(SELECT eta from dbo.export where id = e.id) as eta
//--,p.InQty,p.pounit,p.StockUnit,p.OutQty,p.AdjustQty
//--,p.inqty - p.OutQty + p.AdjustQty as balance
//--,p.LInvQty
//--,p.fabrictype
//--,e.seq1
//--,e.seq2
//--from dbo.Export_Detail e left join dbo.PO_Supp_Detail p on e.PoID = p.ID and e.Seq1 = p.SEQ1 and e.Seq2 = p.seq2
                        sqlcmd = string.Format(@"

select e.poid,concat(Ltrim(Rtrim(e.seq1)), ' ', e.Seq2) as seq, e.Refno, dbo.getmtldesc(e.poid,e.seq1,e.seq2,2,0) as [Description]
,p.ColorID
,(SELECT eta from dbo.export WITH (NOLOCK) where id = e.id) as eta
,M.InQty
,p.pounit,p.StockUnit
,M.OutQty
,M.AdjustQty
,M.inqty - M.OutQty + M.AdjustQty as balance
,M.LInvQty
,p.fabrictype
,e.seq1
,e.seq2
from dbo.Export_Detail e WITH (NOLOCK) 
left join dbo.PO_Supp_Detail p WITH (NOLOCK) on e.PoID = p.ID and e.Seq1 = p.SEQ1 and e.Seq2 = p.seq2
INNER JOIN MDivisionPoDetail M WITH (NOLOCK) ON E.PoID = M.POID and e.Seq1 = M.SEQ1 and e.Seq2 = M.seq2 
where e.PoID ='{0}' and e.id = '{1}'", CurrentDetailData["poid"], CurrentMaintain["exportid"]);

                        DBProxy.Current.Select(null, sqlcmd, out poitems);

                        Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(poitems
                            , "Seq,refno,description,colorid,eta,inqty,stockunit,outqty,adjustqty,balanceqty,linvqty"
                            , "6,15,25,8,10,6,6,6,6,6,6", CurrentDetailData["seq"].ToString(), "Seq,Ref#,Description,Color,ETA,In Qty,Stock Unit,Out Qty,Adqty,Balance,Inventory Qty");
                        item.Width = 1024;
                        DialogResult result = item.ShowDialog();
                        if (result == DialogResult.Cancel) { return; }
                        x = item.GetSelecteds();
                    }
                    CurrentDetailData["seq"] = x[0]["seq"];
                    CurrentDetailData["seq1"] = x[0]["seq1"];
                    CurrentDetailData["seq2"] = x[0]["seq2"];
                    CurrentDetailData["pounit"] = x[0]["pounit"];
                    CurrentDetailData["stockunit"] = x[0]["stockunit"];
                    CurrentDetailData["fabrictype"] = x[0]["fabrictype"];
                    //CurrentDetailData["shipqty"] = 0m;
                    CurrentDetailData["Actualqty"] = 0m;
                    CurrentDetailData.EndEdit();
                }
            };
            ts.CellValidating += (s, e) =>
                {
                    if (!this.EditMode) return;
                    if (String.Compare(e.FormattedValue.ToString(), CurrentDetailData["seq"].ToString()) != 0)
                    {
                        if (MyUtility.Check.Empty(e.FormattedValue))
                        {
                            CurrentDetailData["seq"] = "";
                            CurrentDetailData["seq1"] = "";
                            CurrentDetailData["seq2"] = "";
                            CurrentDetailData["pounit"] = "";
                            CurrentDetailData["stockunit"] = "";
                            CurrentDetailData["fabrictype"] = "";
                            CurrentDetailData["shipqty"] = 0m;
                            CurrentDetailData["Actualqty"] = 0m;
                        }
                        else
                        {
                            //check Seq Length
                            string[] seq = e.FormattedValue.ToString().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            if (seq.Length < 2)
                            {
                                MyUtility.Msg.WarningBox("Data not found!", "Seq");
                                e.Cancel = true;
                                return;
                            }

                            if (!MyUtility.Check.Seek(string.Format(Prgs.selePoItemSqlCmd +
                                    @"and p.seq1 ='{2}' and p.seq2 = '{3}' and left(p.seq1, 1) !='7'", CurrentDetailData["poid"], Sci.Env.User.Keyword, seq[0], seq[1]), out dr, null))
                            {
                                MyUtility.Msg.WarningBox("Data not found!", "Seq");
                                e.Cancel = true;
                                return;
                            }
                            else
                            {
                                DataRow dr_StockUnit;
                                bool unti_result= MyUtility.Check.Seek(string.Format(@"
select 
iif(mm.IsExtensionUnit is null or uu.ExtensionUnit = '', 
    ff.UsageUnit , 
    iif(mm.IsExtensionUnit > 0 , 
        iif(uu.ExtensionUnit is null or uu.ExtensionUnit = '', 
            ff.UsageUnit , 
            uu.ExtensionUnit), 
        ff.UsageUnit)) as StockUnit
from dbo.PO_Supp_Detail b WITH (NOLOCK) 
inner join [dbo].[Fabric] ff WITH (NOLOCK) on b.SCIRefno= ff.SCIRefno
inner join [dbo].[MtlType] mm WITH (NOLOCK) on mm.ID = ff.MtlTypeID
inner join [dbo].[Unit] uu WITH (NOLOCK) on ff.UsageUnit = uu.ID
inner join View_unitrate v on v.FROM_U = b.POUnit 
	and v.TO_U = (
	iif(mm.IsExtensionUnit is null or uu.ExtensionUnit = '', 
		ff.UsageUnit , 
		iif(mm.IsExtensionUnit > 0 , 
			iif(uu.ExtensionUnit is null or uu.ExtensionUnit = '', 
				ff.UsageUnit , 
				uu.ExtensionUnit), 
			ff.UsageUnit)))--b.StockUnit
where b.id = '{0}' and b.seq1 ='{1}'and b.seq2 = '{2}'", CurrentDetailData["poid"], seq[0], seq[1]), out dr_StockUnit, null);

                                CurrentDetailData["stockunit"] = (unti_result) ? dr_StockUnit["stockunit"] : dr["stockunit"];
                                CurrentDetailData["seq"] = e.FormattedValue;
                                CurrentDetailData["seq1"] = seq[0];
                                CurrentDetailData["seq2"] = seq[1];
                                CurrentDetailData["pounit"] = dr["pounit"];
                                CurrentDetailData["fabrictype"] = dr["fabrictype"];
                                //CurrentDetailData["shipqty"] = 0m;
                                CurrentDetailData["Actualqty"] = 0m;
                            }
                        }
                    }
                };

            #endregion Seq 右鍵開窗

            #region Location 右鍵開窗

            Ict.Win.DataGridViewGeneratorTextColumnSettings ts2 = new DataGridViewGeneratorTextColumnSettings();
            ts2.EditingMouseDown += (s, e) =>
            {
                if (this.EditMode && e.Button == MouseButtons.Right)
                {
                    Sci.Win.Tools.SelectItem2 item = Prgs.SelectLocation(CurrentDetailData["stocktype"].ToString(), CurrentDetailData["location"].ToString());
                    DialogResult result = item.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    CurrentDetailData["location"] = item.GetSelectedString();
                    CurrentDetailData.EndEdit();
                }
            };

            ts2.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    CurrentDetailData["location"] = e.FormattedValue;
                    string sqlcmd = string.Format(@"SELECT id FROM DBO.MtlLocation WITH (NOLOCK) WHERE StockType='{0}'", CurrentDetailData["stocktype"].ToString());
                    DataTable dt;
                    DBProxy.Current.Select(null, sqlcmd, out dt);
                    string[] getLocation = CurrentDetailData["location"].ToString().Split(',').Distinct().ToArray();
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
                        else if(!(location.EqualString("")))
                        {
                            trueLocation.Add(location);
                        }
                    }

                    if (!selectId)
                    {
                        MyUtility.Msg.WarningBox("Location : " + string.Join(",", (errLocation).ToArray()) + "  Data not found !!", "Data not found");
                        e.Cancel = true;
                    }
                    trueLocation.Sort();
                    CurrentDetailData["location"] = string.Join(",", (trueLocation).ToArray());
                    //去除錯誤的Location將正確的Location填回
                }
            };

            #endregion Location 右鍵開窗

            #region Ship Qty Valid

            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    if (!MyUtility.Check.Empty(CurrentDetailData["pounit"]) && !MyUtility.Check.Empty(CurrentDetailData["stockunit"]))
                    {
                        CurrentDetailData["shipqty"] = e.FormattedValue;
                        CurrentDetailData["Actualqty"] = e.FormattedValue;
                        string rate = MyUtility.GetValue.Lookup(string.Format(@"select RateValue from dbo.View_Unitrate v
                    where v.FROM_U ='{0}' and v.TO_U='{1}'", CurrentDetailData["pounit"], CurrentDetailData["stockunit"]));
                        //string aa = string.Format(@"select Rate from dbo.View_Unitrate v
                   // where v.FROM_U ='{0}' and v.TO_U='{1}'", CurrentDetailData["pounit"], CurrentDetailData["stockunit"]);
                        CurrentDetailData["stockqty"] = MyUtility.Math.Round(decimal.Parse(e.FormattedValue.ToString()) * decimal.Parse(rate), 2);
                    }
                }
            };

            #endregion Ship Qty Valid

            #region In Qty Valid

            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns2 = new DataGridViewGeneratorNumericColumnSettings();
            ns2.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    CurrentDetailData["Actualqty"] = e.FormattedValue;
                    if (!MyUtility.Check.Empty(CurrentDetailData["pounit"]) && !MyUtility.Check.Empty(CurrentDetailData["stockunit"]))
                    {
                        string rate = MyUtility.GetValue.Lookup(string.Format(@"select RateValue from dbo.View_Unitrate v
                    where v.FROM_U ='{0}' and v.TO_U='{1}'", CurrentDetailData["pounit"], CurrentDetailData["stockunit"]));
                        CurrentDetailData["stockqty"] = MyUtility.Math.Round(decimal.Parse(e.FormattedValue.ToString()) * decimal.Parse(rate), 2);
                    }
                }
            };

            #endregion In Qty Valid

            Ict.Win.UI.DataGridViewComboBoxColumn cbb_fabrictype;
            Ict.Win.UI.DataGridViewComboBoxColumn cbb_stocktype;
            Ict.Win.UI.DataGridViewTextBoxColumn cbb_Roll;
            Ict.Win.UI.DataGridViewTextBoxColumn cbb_Dyelot;

            #region 欄位設定
            Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("poid", header: "SP#", width: Widths.AnsiChars(13), settings: ts4)  //0
            .Text("seq", header: "Seq", width: Widths.AnsiChars(6), settings: ts)  //1
            .ComboBox("fabrictype", header: "Fabric" + Environment.NewLine + "Type", width: Widths.AnsiChars(10), iseditable: false).Get(out cbb_fabrictype)  //2
            .Numeric("shipqty", header: "Ship Qty", width: Widths.AnsiChars(13), decimal_places: 2, integer_places: 10, settings: ns)    //3
            .Numeric("weight", header: "G.W(kg)", width: Widths.AnsiChars(9), decimal_places: 2, integer_places: 7)    //4
            .Numeric("actualweight", header: "Act.(kg)", width: Widths.AnsiChars(9), decimal_places: 2, integer_places: 7)    //5
            .Text("Roll", header: "Roll#", width: Widths.AnsiChars(9)).Get(out cbb_Roll)    //6
            .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(5)).Get(out cbb_Dyelot)    //7
            .Numeric("ActualQty", header: "Actual Qty", width: Widths.AnsiChars(13), decimal_places: 2, integer_places: 10, settings: ns2).Get(out Col_ActualQty)    //8
            .Text("pounit", header: "Purchase" + Environment.NewLine + "Unit", iseditingreadonly: true)    //9
            .Numeric("stockqty", header: "Receiving Qty" + Environment.NewLine + "(Stock Unit)", width: Widths.AnsiChars(13), decimal_places: 2, integer_places: 10, iseditingreadonly: true)    //10
            .Text("stockunit", header: "Stock" + Environment.NewLine + "Unit", iseditingreadonly: true)    //11
            .ComboBox("Stocktype", header: "Stock" + Environment.NewLine + "Type", width: Widths.AnsiChars(8), iseditable: false).Get(out cbb_stocktype)   //12
            .Text("Location", header: "Location", settings: ts2, iseditingreadonly: false).Get(out Col_Location)    //13
            .Text("remark", header: "Remark")    //14
            ;     //
            cbb_Roll.MaxLength = 8;
            cbb_Dyelot.MaxLength = 4;
            #endregion 欄位設定

            cbb_fabrictype.DataSource = new BindingSource(di_fabrictype, null);
            cbb_fabrictype.ValueMember = "Key";
            cbb_fabrictype.DisplayMember = "Value";
            cbb_stocktype.DataSource = new BindingSource(di_stocktype, null);
            cbb_stocktype.ValueMember = "Key";
            cbb_stocktype.DisplayMember = "Value";

            #region 可編輯欄位變色

            //detailgrid.Columns[0].DefaultCellStyle.BackColor = Color.Pink;
            //detailgrid.Columns[1].DefaultCellStyle.BackColor = Color.Pink;
            //detailgrid.Columns[3].DefaultCellStyle.BackColor = Color.Pink;
            //detailgrid.Columns[4].DefaultCellStyle.BackColor = Color.Pink;
            //detailgrid.Columns[5].DefaultCellStyle.BackColor = Color.Pink;
            //detailgrid.Columns[6].DefaultCellStyle.BackColor = Color.Pink;
            //detailgrid.Columns[7].DefaultCellStyle.BackColor = Color.Pink;
            //detailgrid.Columns[8].DefaultCellStyle.BackColor = Color.Pink;
            //detailgrid.Columns[13].DefaultCellStyle.BackColor = Color.Pink;
            //detailgrid.Columns[14].DefaultCellStyle.BackColor = Color.Pink;

            #endregion 可編輯欄位變色
        }

        //Confirm
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            var dr = this.CurrentMaintain;
            if (null == dr) return;

            string upd_MD_2T = "";
            string upd_MD_8T = "";
            string upd_Fty_2T = "";
            StringBuilder sqlupd2 = new StringBuilder();
            String sqlcmd = "", sqlupd3 = "", ids = "", sqlcmd4 = "";

            DualResult result, result2, result3;
            DataTable datacheck;

            #region 檢查必輸欄位
            if (MyUtility.Check.Empty(CurrentMaintain["PackingReceive"]))
            {
                MyUtility.Msg.WarningBox("<Packing Receive Date>  can't be empty!", "Warning");
                dateBox4.Focus();
                return;
            }
            if (MyUtility.Check.Empty(CurrentMaintain["whseArrival"]))
            {
                MyUtility.Msg.WarningBox("< Arrive W/H Date >  can't be empty!", "Warning");
                dateBox3.Focus();
                return;
            }
            #endregion

            //判斷是否已經收過此種布料SP#,SEQ,Roll不能重複收
            if(!checkRoll())
                return;

            #region 檢查負數庫存

            sqlcmd = string.Format(@"Select d.poid,d.seq1,d.seq2,d.Roll,d.StockQty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty
from dbo.Receiving_Detail d WITH (NOLOCK) left join FtyInventory f WITH (NOLOCK) 
on d.PoId = f.PoId
and d.Seq1 = f.Seq1
and d.Seq2 = f.seq2
and d.StockType = f.StockType
and d.Roll = f.Roll
where (isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) + d.StockQty < 0) and d.Id = '{0}'", CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                ShowErr(sqlcmd, result2);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3}'s balance: {4} is less than stock qty: {5}" + Environment.NewLine
                            , tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["balanceqty"], tmp["stockqty"]);
                    }
                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存
            #region 更新表頭狀態資料

            sqlupd3 = string.Format(@"update Receiving set status='Confirmed', editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料

            #region 更新庫存數量 mdivisionPoDetail
            var data_MD_2T = (from b in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                       group b by new
                       {
                           poid = b.Field<string>("poid").Trim(),
                           seq1 = b.Field<string>("seq1").Trim(),
                           seq2 = b.Field<string>("seq2").Trim(),
                           stocktype = b.Field<string>("stocktype").Trim()
                       } into m
                       select new Prgs_POSuppDetailData
                       {
                           poid = m.First().Field<string>("poid"),
                           seq1 = m.First().Field<string>("seq1"),
                           seq2 = m.First().Field<string>("seq2"),
                           stocktype = m.First().Field<string>("stocktype"),
                           qty = m.Sum(w => w.Field<decimal>("stockqty")),
                           location = string.Join(",", m.Select(r => r.Field<string>("location")).Distinct()),
                       }).ToList();
            var data_MD_8T = (from b in ((DataTable)detailgridbs.DataSource).AsEnumerable().Where(w => w.Field<string>("stocktype").Trim() == "I")
                       group b by new
                       {
                           poid = b.Field<string>("poid").Trim(),
                           seq1 = b.Field<string>("seq1").Trim(),
                           seq2 = b.Field<string>("seq2").Trim(),
                           stocktype = b.Field<string>("stocktype").Trim()
                       } into m
                        select new Prgs_POSuppDetailData
                       {
                           poid = m.First().Field<string>("poid"),
                           seq1 = m.First().Field<string>("seq1"),
                           seq2 = m.First().Field<string>("seq2"),
                           stocktype = m.First().Field<string>("stocktype"),
                           qty = m.Sum(w => w.Field<decimal>("stockqty")),
                           location = string.Join(",", m.Select(r => r.Field<string>("location")).Distinct()),
                       }).ToList();

            #endregion

            #region -- 更新庫存數量  ftyinventory --
            DataTable newDt = ((DataTable)detailgridbs.DataSource).Clone();
            foreach (DataRow dtr in ((DataTable)detailgridbs.DataSource).Rows)
            {
                string[] dtrLocation = dtr["location"].ToString().Split(',');
                if (dtrLocation.Length == 0)
                {
                    DataRow newDr = newDt.NewRow();
                    newDr.ItemArray = dtr.ItemArray;
                    newDt.Rows.Add(newDr);
                }
                else
                {
                    foreach (string location in dtrLocation)
                    {
                        DataRow newDr = newDt.NewRow();
                        newDr.ItemArray = dtr.ItemArray;
                        newDr["location"] = location;
                        newDt.Rows.Add(newDr);
                    }
                }
            }

            var data_Fty_2T = (from m in newDt.AsEnumerable()
                         select new
                         {
                             poid = m.Field<string>("poid"),
                             seq1 = m.Field<string>("seq1"),
                             seq2 = m.Field<string>("seq2"),
                             stocktype = m.Field<string>("stocktype"),
                             qty = m.Field<decimal>("stockqty"),
                             location = m.Field<string>("location"),
                             roll = m.Field<string>("roll"),
                             dyelot = m.Field<string>("dyelot"),
                         }).ToList();
            upd_Fty_2T = Prgs.UpdateFtyInventory_IO(2, null, true);
            #endregion 更新庫存數量  ftyinventory

            string sql_UpdatePO_Supp_Detail = @";
--------20161109LEO新增回寫PO_Supp_Detail的StockUnit 
alter table #Tmp alter column poid varchar(20)
alter table #Tmp alter column seq1 varchar(3)
alter table #Tmp alter column seq2 varchar(3)
alter table #Tmp alter column StockUnit varchar(20)
select distinct poid,seq1,seq2,StockUnit into #tmpD from #Tmp
merge dbo.PO_Supp_Detail as target
using #tmpD as src
    on  target.ID =src.poid and target.seq1 = src.seq1 and target.seq2 =src.seq2 
when matched then
    update
    set target.StockUnit = src.StockUnit;
";

            #region Base on wkno 收料時，需回寫export
            sqlcmd4 = string.Format(@"update dbo.export set whsearrival =(select WhseArrival from dbo.receiving where id='{2}')
,packingarrival = (select PackingReceive from dbo.receiving where id='{2}'), editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, CurrentMaintain["exportid"], CurrentMaintain["id"]);
            #endregion

            #region 更新FIR,AIR資料  
            
            List<SqlParameter> Fir_Air_Proce = new List<SqlParameter>();
            Fir_Air_Proce.Add(new SqlParameter("@ID",CurrentMaintain["ID"]));
            Fir_Air_Proce.Add(new SqlParameter("@LoginID",UserID));

            DBProxy.Current.ExecuteSP(null, "insert_Air_Fir", Fir_Air_Proce);
            #endregion

            TransactionScope _transactionscope = new TransactionScope();
            SqlConnection sqlConn = null;
            DBProxy.Current.OpenConnection(null, out sqlConn);
            using (_transactionscope)
            using(sqlConn)
            {
                try
                {
                    /*
                     * 先更新 FtyInventory 後更新 MDivisionPoDetail
                     * 所有 MDivisionPoDetail 資料都在 Transaction 中更新
                     * 因為要在同一 SqlConnection 之下執行
                     */
                    DataTable resulttb;
                    #region FtyInventory
                    if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_2T, "", upd_Fty_2T, out resulttb, "#TmpSource", conn: sqlConn)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(result);
                        return;
                    }
                    #endregion

                    #region MDivisionPoDetail
                    if (data_MD_2T.Count > 0)
                    {
                        upd_MD_2T = Prgs.UpdateMPoDetail(2, data_MD_2T, true, sqlConn: sqlConn);
                        if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_2T, "", upd_MD_2T, out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            _transactionscope.Dispose();
                            ShowErr(result);
                            return;
                        }
                    }
                    if (data_MD_8T.Count > 0)
                    {
                        upd_MD_8T = Prgs.UpdateMPoDetail(8, data_MD_8T, true, sqlConn: sqlConn);
                        if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_8T, "", upd_MD_8T, out resulttb, "#TmpSource", conn: sqlConn)))
                        {
                            _transactionscope.Dispose();
                            ShowErr(result);
                            return;
                        }
                    }
                    #endregion

                    if (!(result = MyUtility.Tool.ProcessWithDatatable
                        ((DataTable)detailgridbs.DataSource, "", sql_UpdatePO_Supp_Detail, out resulttb, "#tmp", conn: sqlConn)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(result);
                        return;
                    }
                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlupd3, result);
                        return;
                    }

                    if (!MyUtility.Check.Empty(CurrentMaintain["exportid"]))
                    {
                        if (!(result3 = DBProxy.Current.Execute(null, sqlcmd4)))
                        {
                            _transactionscope.Dispose();
                            ShowErr(sqlcmd4, result);
                            return;
                        }
                    }

                    _transactionscope.Complete();
                    MyUtility.Msg.InfoBox("Confirmed successful");
                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
                finally
                {
                    _transactionscope.Dispose();
                }
            }
            this.RenewData();
            this.OnDetailEntered();
            this.EnsureToolbarExt();
        }

        //Unconfirm
        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();
            DataTable datacheck;
            DataTable dt = (DataTable)detailgridbs.DataSource;

            DialogResult dResult = MyUtility.Msg.QuestionBox("Do you want to unconfirme it?");
            if (dResult.ToString().ToUpper() == "NO") return;
            var dr = this.CurrentMaintain; if (null == dr) return;
            StringBuilder sqlupd2 = new StringBuilder();
            string sqlcmd = "", sqlupd3 = "", ids = "", sqlcmd4 = "";
            DualResult result, result2, result3;

            string upd_MD_2F = "";
            string upd_MD_8F = "";
            string upd_Fty_2F = "";

            #region 檢查負數庫存

            sqlcmd = string.Format(@"Select d.poid,d.seq1,d.seq2,d.Roll,d.StockQty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty
from dbo.Receiving_Detail d WITH (NOLOCK) left join FtyInventory f WITH (NOLOCK) 
on d.PoId = f.PoId
and d.Seq1 = f.Seq1
and d.Seq2 = f.seq2
and d.StockType = f.StockType
and d.Roll = f.Roll
where (isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) - d.StockQty < 0) and d.Id = '{0}'", CurrentMaintain["id"]);
            if (!(result2 = DBProxy.Current.Select(null, sqlcmd, out datacheck)))
            {
                ShowErr(sqlcmd, result2);
                return;
            }
            else
            {
                if (datacheck.Rows.Count > 0)
                {
                    foreach (DataRow tmp in datacheck.Rows)
                    {
                        ids += string.Format("SP#: {0} Seq#: {1}-{2} Roll#: {3}'s balance: {4} is less than stock qty: {5}" + Environment.NewLine
                            , tmp["poid"], tmp["seq1"], tmp["seq2"], tmp["roll"], tmp["balanceqty"], tmp["stockqty"]);
                    }
                    MyUtility.Msg.WarningBox("Balacne Qty is not enough!!" + Environment.NewLine + ids, "Warning");
                    return;
                }
            }

            #endregion 檢查負數庫存

            #region 更新表頭狀態資料

            sqlupd3 = string.Format(@"update Receiving set status='New', editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

            #endregion 更新表頭狀態資料

            #region 更新庫存數量 po_supp_detail & ftyinventory
            var data_MD_2F = (from b in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                       group b by new
                       {
                           poid = b.Field<string>("poid"),
                           seq1 = b.Field<string>("seq1"),
                           seq2 = b.Field<string>("seq2"),
                           stocktype = b.Field<string>("stocktype")
                       } into m
                       select new Prgs_POSuppDetailData
                       {
                           poid = m.First().Field<string>("poid"),
                           seq1 = m.First().Field<string>("seq1"),
                           seq2 = m.First().Field<string>("seq2"),
                           stocktype = m.First().Field<string>("stocktype"),
                           qty = - (m.Sum(w => w.Field<decimal>("stockqty")))
                       }).ToList();

            var data_MD_8F = (from b in ((DataTable)detailgridbs.DataSource).AsEnumerable().Where(w => w.Field<string>("stocktype").Trim() == "I")
                       group b by new
                       {
                           poid = b.Field<string>("poid"),
                           seq1 = b.Field<string>("seq1"),
                           seq2 = b.Field<string>("seq2"),
                           stocktype = b.Field<string>("stocktype")
                       } into m
                        select new Prgs_POSuppDetailData
                       {
                           poid = m.First().Field<string>("poid"),
                           seq1 = m.First().Field<string>("seq1"),
                           seq2 = m.First().Field<string>("seq2"),
                           stocktype = m.First().Field<string>("stocktype"),
                           qty = - (m.Sum(w => w.Field<decimal>("stockqty")))
                       }).ToList();

            var data_Fty_2F = (from m in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                         select new
                         {
                             poid = m.Field<string>("poid"),
                             seq1 = m.Field<string>("seq1"),
                             seq2 = m.Field<string>("seq2"),
                             stocktype = m.Field<string>("stocktype"),
                             qty = - (m.Field<decimal>("stockqty")),
                             location = m.Field<string>("location"),
                             roll = m.Field<string>("roll"),
                             dyelot = m.Field<string>("dyelot"),
                         }).ToList();

            upd_Fty_2F = Prgs.UpdateFtyInventory_IO(2, null, false);
            #endregion

            sqlcmd4 = string.Format(@"update dbo.export set whsearrival =null,packingarrival =null, editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, CurrentMaintain["exportid"], CurrentMaintain["id"]);
            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    /*
                     * 先更新 FtyInventory 後更新 MDivisionPoDetail
                     * 所有 MDivisionPoDetail 資料都在 Transaction 中更新，
                     * 因為要在同一 SqlConnection 之下執行
                     */
                    DataTable resulttb;
                    #region MdivisionPoDetail
                    if (data_MD_2F.Count > 0)
                    {
                        upd_MD_2F = Prgs.UpdateMPoDetail(2, data_MD_2F, false);
                        if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_2F, "", upd_MD_2F, out resulttb, "#TmpSource")))
                        {
                            _transactionscope.Dispose();
                            ShowErr(result);
                            return;
                        }
                    }
                    if (data_MD_8F.Count > 0)
                    {
                        upd_MD_8F = Prgs.UpdateMPoDetail(8, data_MD_8F, false);
                        if (!(result = MyUtility.Tool.ProcessWithObject(data_MD_8F, "", upd_MD_8F, out resulttb, "#TmpSource")))
                        {
                            _transactionscope.Dispose();
                            ShowErr(result);
                            return;
                        }
                    }
                    #endregion

                    #region FtyInventory
                    if (!(result = MyUtility.Tool.ProcessWithObject(data_Fty_2F, "", upd_Fty_2F, out resulttb, "#TmpSource")))
                    {
                        _transactionscope.Dispose();
                        ShowErr(result);
                        return;
                    }
                    #endregion

                    if (!(result = DBProxy.Current.Execute(null, sqlupd3)))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlupd3, result);
                        return;
                    }

                    if (!MyUtility.Check.Empty(CurrentMaintain["exportid"]))
                    {
                        if (!(result3 = DBProxy.Current.Execute(null, sqlcmd4)))
                        {
                            _transactionscope.Dispose();
                            ShowErr(sqlcmd4, result);
                            return;
                        }
                    }

                    _transactionscope.Complete();
                    MyUtility.Msg.InfoBox("UnConfirmed successful");
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
            this.EnsureToolbarExt();
        }

        //寫明細撈出的sql command
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();

            this.DetailSelectCommand = string.Format(@"select a.id,a.PoId,a.Seq1,a.Seq2,concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2) as seq
,a.Poid + concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2) as PoidSeq
,(select p1.FabricType from PO_Supp_Detail p1 WITH (NOLOCK) where p1.ID = a.PoId and p1.seq1 = a.SEQ1 and p1.SEQ2 = a.seq2) as fabrictype
,a.shipqty
,a.Weight
,a.ActualWeight
,a.Roll
,a.Dyelot
,a.ActualQty
,a.PoUnit
,a.StockQty
,a.StockUnit
,a.StockType
,a.Location
,a.remark
,a.ukey
from dbo.Receiving_Detail a WITH (NOLOCK) 
Where a.id = '{0}' ", masterID);

            return base.OnDetailSelectCommandPrepare(e);

        }

        // invoice# valid
        private void textBox3_Validating(object sender, CancelEventArgs e)
        {
            DataRow dr;
            DataTable dt;
            if (textBox3.Text != textBox3.OldValue)
            {
                ((DataTable)detailgridbs.DataSource).Rows.Clear();  //清空表身資料
                CurrentMaintain["invno"] = textBox3.Text;
                CurrentMaintain["ETA"] = DBNull.Value;
                CurrentMaintain["WhseArrival"] = DBNull.Value;
                dateBox5.Value = null;
                dateBox2.Value = null;
                if (MyUtility.Check.Seek(string.Format("select packingarrival,whsearrival,eta,PortArrival,DocArrival from dbo.export WITH (NOLOCK) where id='{0}'"
                    , textBox3.Text), out dr, null))
                {
                    if (!MyUtility.Check.Empty(dr["portarrival"])) dateBox2.Value = DateTime.Parse(dr["portarrival"].ToString());
                    CurrentMaintain["exportid"] = CurrentMaintain["invno"];
                    CurrentMaintain["PackingReceive"] = dr["packingarrival"];
                    CurrentMaintain["WhseArrival"] = dr["WhseArrival"];
                    if (!MyUtility.Check.Empty(dr["DocArrival"])) dateBox5.Value = DateTime.Parse(dr["DocArrival"].ToString());
                    CurrentMaintain["ETA"] = dr["ETA"];
                    CurrentMaintain["third"] = 0;
                    this.dateBox1.Enabled = false;
                    string selCom = string.Format(@"select a.poid,a.seq1,a.seq2,a.Qty+a.Foc as shipqty,a.UnitId,a.WeightKg as Weight
, a.NetKg as ActualWeight, iif(c.category='M','I','B') as stocktype
, b.POUnit 
--,b.StockUnit
,iif(mm.IsExtensionUnit is null or uu.ExtensionUnit = '', 
    ff.UsageUnit , 
    iif(mm.IsExtensionUnit > 0 , 
        iif(uu.ExtensionUnit is null or uu.ExtensionUnit = '', 
            ff.UsageUnit , 
            uu.ExtensionUnit), 
        ff.UsageUnit)) as StockUnit
--,IIF ( mm.IsExtensionUnit > 0, uu.ExtensionUnit, ff.UsageUnit ) AS StockUnit
,b.FabricType
, concat(Ltrim(Rtrim(a.seq1)), ' ', a.Seq2) as seq
,a.Qty+a.Foc as Actualqty
, round((a.Qty+a.Foc)*v.rate,2) as stockqty
, '' as dyelot
, '' as remark
, '' as location
--,ff.UsageUnit,mm.IsExtensionUnit,uu.ExtensionUnit
from dbo.Export_Detail a WITH (NOLOCK) inner join dbo.PO_Supp_Detail b WITH (NOLOCK) on a.PoID= b.id and a.Seq1 = b.SEQ1 and a.Seq2 = b.SEQ2
inner join orders c WITH (NOLOCK) on c.id = a.poid
inner join [dbo].[Fabric] ff WITH (NOLOCK) on b.SCIRefno= ff.SCIRefno
inner join [dbo].[MtlType] mm WITH (NOLOCK) on mm.ID = ff.MtlTypeID
inner join [dbo].[Unit] uu WITH (NOLOCK) on ff.UsageUnit = uu.ID
inner join View_unitrate v on v.FROM_U = b.POUnit
	and v.TO_U = (
	iif(mm.IsExtensionUnit is null or uu.ExtensionUnit = '', 
		ff.UsageUnit , 
		iif(mm.IsExtensionUnit > 0 , 
			iif(uu.ExtensionUnit is null or uu.ExtensionUnit = '', 
				ff.UsageUnit , 
				uu.ExtensionUnit), 
			ff.UsageUnit)))--b.StockUnit
where a.id='{0}'", CurrentMaintain["exportid"]);
                    DBProxy.Current.Select(null, selCom, out dt);
                    if (MyUtility.Check.Empty(dt) || MyUtility.Check.Empty(dt.Rows.Count))
                    {
                        MyUtility.Msg.WarningBox("Export Data not found!!");
                        return;
                    }
                    foreach (var item in dt.ToList())
                    {
                        //DetailDatas.Add(item);
                        ((DataTable)detailgridbs.DataSource).ImportRow(item);
                    }
                }
                else
                {
                    CurrentMaintain["exportid"] = "";
                    CurrentMaintain["third"] = 1;
                    this.dateBox1.Enabled = true;
                }
            }
        }

        //delete all
        private void btDeleteAllDetail_Click(object sender, EventArgs e)
        {
            ((DataTable)detailgridbs.DataSource).Rows.Clear();  //清空表身資料
        }

        //Accumulated Qty
        private void btAccumulated_Click(object sender, EventArgs e)
        {
            var frm = new Sci.Production.Warehouse.P07_AccumulatedQty(CurrentMaintain);
            frm.P07 = this;
            frm.ShowDialog(this);
        }

        //Filter
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    detailgridbs.Filter = "";
                    break;
                case 1:
                    detailgridbs.Filter = "fabrictype ='F'";
                    break;
                case 2:
                    detailgridbs.Filter = "fabrictype ='A'";
                    break;
                case 3:
                    detailgridbs.Filter = "fabrictype ='O'";
                    break;
            }
        }

        private void btModifyRollDyelot_Click(object sender, EventArgs e)
        {
            if (CurrentMaintain["Status"].ToString().ToUpper() != "CONFIRMED")
            {
                MyUtility.Msg.InfoBox("Please modify data directly!!");
                return;
            }
            var frm = new Sci.Production.Warehouse.P07_ModifyRollDyelot(detailgridbs.DataSource, CurrentMaintain["id"].ToString());
            frm.ShowDialog(this);
            this.RenewData();
        }

        private void btUpdateActWeight_Click(object sender, EventArgs e)
        {
            if (CurrentMaintain["Status"].ToString().ToUpper() != "CONFIRMED")
            {
                MyUtility.Msg.InfoBox("Please modify data directly!!");
                return;
            }
            var frm = new Sci.Production.Warehouse.P07_UpdateActualWeight(detailgridbs.DataSource, CurrentMaintain["id"].ToString());
            frm.ShowDialog(this);
            this.RenewData();
        }

        private void btFind_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(detailgridbs.DataSource)) return;
            int index = -1;

            if(txtSeq1.checkEmpty(showErrMsg: false)) {
                index = detailgridbs.Find("poid", textBox1.Text.TrimEnd());
            }else{
                index = detailgridbs.Find("PoidSeq", textBox1.Text.TrimEnd() + txtSeq1.getSeq());
            }
            
            if (index == -1)
            { MyUtility.Msg.WarningBox("Data was not found!!"); }
            else
            { detailgridbs.Position = index; }
        }

        private void btImportFromExcel_Click(object sender, EventArgs e)
        {
            if (textBox3.Text == "")
            {
                MyUtility.Msg.WarningBox("Invoice# Can't Null");
            }
            else { 
            Sci.Production.Warehouse.P07_ExcelImport callNextForm = new Sci.Production.Warehouse.P07_ExcelImport(CurrentMaintain,(DataTable)detailgridbs.DataSource);
            callNextForm.ShowDialog(this);
            }
        }

        private void btDownloadSample_Click(object sender, EventArgs e)
        {
            //呼叫執行檔絕對路徑
            DirectoryInfo dir = new DirectoryInfo(System.Windows.Forms.Application.StartupPath);
            //執行檔上一層絕對路徑
            //string xltpath = dir.Parent.FullName.ToString();
            //Microsoft.Office.Interop.Excel._Application ObjApp = MyUtility.Excel.ConnectExcel(xltpath + "\\xlt\\Warehouse_P07_ImportExcelFormat.xltx");
            //ObjApp.Visible = true;

            string strXltName = Sci.Env.Cfg.XltPathDir + "\\Warehouse_P07_ImportExcelFormat.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return;
            excel.Visible = true;
        }
        protected override bool ClickPrint()
        {
            DataTable details = (DataTable)this.detailgridbs.DataSource;
            List<String> poidList = details.AsEnumerable()
                .Select(row => row["poid"].ToString().TrimEnd())
                .Distinct()
                .ToList();

            P07_Print p = new P07_Print(poidList);
            p.CurrentDataRow = this.CurrentMaintain;
            //p.CurrentDataRow = this.CurrentDataRow;
            p.ShowDialog();
            return true;
        }

        private void btPrintSticker_Click(object sender, EventArgs e)
        {


            P07_Sticker s = new P07_Sticker(this.CurrentDataRow);
            s.ShowDialog();
        }

        protected override void ClickEditAfter()
        {
            foreach (DataGridViewColumn index in detailgrid.Columns) { index.SortMode = DataGridViewColumnSortMode.NotSortable; }
            base.ClickEditAfter();
        }

        protected override void ClickSaveAfter()
        {
            foreach (DataGridViewColumn index in detailgrid.Columns) { index.SortMode = DataGridViewColumnSortMode.Automatic; }
            base.ClickSaveAfter();
        }

        protected override void ClickUndo()
        {
            foreach (DataGridViewColumn index in detailgrid.Columns) { index.SortMode = DataGridViewColumnSortMode.Automatic; }
            base.ClickUndo();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (this.EditMode)
            {
                switch (keyData)
                {
                    case Keys.Tab:
                        var currentCell = this.detailgrid.CurrentCell;
                        if (!currentCell.Empty())
                        {
                            var columnIndex = currentCell.ColumnIndex;
                            if (columnIndex == 8)
                            {
                                this.detailgrid.CurrentCell = this.detailgrid.Rows[currentCell.RowIndex].Cells[13];

                            }
                        }                        
                        break;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private bool checkRoll()
        {
            //判斷是否已經收過此種布料SP#,SEQ,Roll不能重複收
            List<string> listMsg = new List<string>();
            foreach (DataRow row in DetailDatas)
            {
                DataRow dr;
                if (row["fabrictype"].ToString().ToUpper() == "F")
                {
                    if (MyUtility.Check.Seek(string.Format(@"
select 
	RD.RD_Count + ST.ST_Count + BB.BB_Count + TID.TID_Count as total
from(
    select COUNT(*) RD_Count 
    from dbo.Receiving_Detail RD WITH (NOLOCK) 
    inner join dbo.Receiving R WITH (NOLOCK) on RD.Id = R.Id  
    where RD.PoId = '{0}' and RD.Seq1 = '{1}' and RD.Seq2 = '{2}' and RD.Roll = '{3}' and Dyelot != '{4}' and RD.id !='{5}' and R.Status = 'Confirmed'
) RD
OUTER APPLY
(
    select COUNT(*) ST_Count 
    from dbo.SubTransfer_Detail SD WITH (NOLOCK) 
    inner join dbo.SubTransfer S WITH (NOLOCK) on SD.ID = S.Id 
    where ToPOID = '{0}' and ToSeq1 = '{1}' and ToSeq2 = '{2}' and ToRoll = '{3}' and ToDyelot != '{4}' and S.Status = 'Confirmed'
) ST
OUTER APPLY
(
    select COUNT('POID') BB_Count 
    from dbo.BorrowBack_Detail BD WITH (NOLOCK) 
    inner join dbo.BorrowBack B WITH (NOLOCK) on BD.ID = B.Id  
    where ToPOID = '{0}' and ToSeq1 = '{1}' and ToSeq2 = '{2}' and ToRoll = '{3}' and ToDyelot != '{4}' and B.Status = 'Confirmed'
) BB
Outer Apply
(
    select count(*) TID_Count 
    From dbo.TransferIn TI
    inner join dbo.TransferIn_Detail TID on TI.ID = TID.ID
    where POID = '{0}' and Seq1 = '{1}' and Seq2 = '{2}' and Roll = '{3}' and Dyelot != '{4}' and Status = 'Confirmed'
) TID", row["poid"], row["seq1"], row["seq2"], row["roll"], row["dyelot"], CurrentMaintain["id"]), out dr, null))
                    {
                        if (Convert.ToInt32(dr[0]) > 0)
                        {
                            listMsg.Add(string.Format("<SP#>:{0}, <Seq>:{1}, <Roll>:{2}, <Dyelot#>:{3}", row["poid"], row["seq1"].ToString() + " " + row["seq2"].ToString(), row["roll"], row["Dyelot"]));
                        }
                    }
                }
            }

            if (listMsg.Count > 0)
            {
                DialogResult Dr = MyUtility.Msg.QuestionBox(
                    "The Deylot of\n\r"
                    + listMsg.JoinToString("\n\r")
                    + "\n\ralready exists, system will update the Qty for original Deylot<Deylot#>."
                    , buttons: MessageBoxButtons.OKCancel);
                switch (Dr.ToString().ToUpper())
                {
                    case "OK":
                        foreach (DataRow row in DetailDatas)
                        {
                            if (row["FabricType"].EqualString("F"))
                            {
                                DataTable dt;
                                string strSql = string.Format(@"
select  Roll
        , Dyelot
from FtyInventory
where   Poid = '{0}'
        and Seq1 = '{1}'
        and Seq2 = '{2}'
        and Roll = '{3}'
", row["poid"], row["seq1"], row["seq2"], row["roll"]);
                                DualResult result = DBProxy.Current.Select(null, strSql, out dt);
                                if (!result)
                                {
                                    MyUtility.Msg.WarningBox(result.Description);
                                }

                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    /**
                                     * 如果在編輯模式下，直接改 Grid
                                     * 非編輯模式 (Confirm) 必須用 Update 才能顯示正確的資料
                                     **/
                                    row["Roll"] = dt.Rows[0]["Roll"];
                                    row["Dyelot"] = dt.Rows[0]["Dyelot"];

                                    if (this.EditMode != true)
                                    {
                                        result = DBProxy.Current.Execute(null, string.Format(@"
Update RD
set RD.Roll = '{0}'
    , RD.Dyelot = '{1}'
From Receiving_Detail RD
where RD.Ukey = '{2}'", dt.Rows[0]["Roll"], dt.Rows[0]["Dyelot"], row["Ukey"]));

                                        if (!result)
                                        {
                                            MyUtility.Msg.WarningBox(result.Description);
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    case "CANCEL":
                        return false;
                }
            }
            return true;
        }
    }
}