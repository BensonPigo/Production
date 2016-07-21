using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

        public P07(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = string.Format("Type='A' and MDivisionID = '{0}'", Sci.Env.User.Keyword);
            ChangeDetailColor();
            di_fabrictype.Add("F", "Fabric");
            di_fabrictype.Add("A", "Accessory");
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
            di_stocktype.Add("B", "Bulk");
            di_stocktype.Add("I", "Inventory");
        }

        // 新增時預設資料
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["MDivisionID"] = Sci.Env.User.Keyword;
            CurrentMaintain["Third"] = 1;
            CurrentMaintain["Status"] = "New";
            CurrentMaintain["Type"] = "A";
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
            DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (dr["status"].ToString().ToUpper() == "CONFIRMED")
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
            DataRow dr = grid.GetDataRow<DataRow>(grid.GetSelectedRowIndex());
            if (dr["status"].ToString().ToUpper() == "CONFIRMED")
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
                if (MyUtility.Check.Empty(row["seq1"]) || MyUtility.Check.Empty(row["seq2"]))
                {
                    warningmsg.Append(string.Format(@"SP#: {0} Seq#: {1}-{2} Roll#:{3} Dyelot:{4} Seq1 or Seq2 can't be empty"
                        , row["poid"], row["seq1"], row["seq2"], row["roll"], row["dyelot"])
                        + Environment.NewLine);
                }

                if (row["seq1"].ToString().TrimStart().StartsWith("7"))
                {
                    warningmsg.Append(string.Format(@"SP#: {0} Seq#: {1}-{2} Roll#:{3} Dyelot:{4} Seq1 can't start with '7'"
                        , row["poid"], row["seq1"], row["seq2"], row["roll"], row["dyelot"]) + Environment.NewLine);
                }

                if (MyUtility.Check.Empty(row["ActualQty"]))
                {
                    warningmsg.Append(string.Format(@"SP#: {0} Seq#: {1}-{2} Roll#:{3} Dyelot:{4} Actual Qty can't be empty"
                        , row["poid"], row["seq1"], row["seq2"], row["roll"], row["dyelot"]) + Environment.NewLine);
                }

                if (MyUtility.Check.Empty(row["stockunit"]))
                {
                    warningmsg.Append(string.Format(@"SP#: {0} Seq#: {1}-{2} Roll#:{3} Dyelot:{4} Stock Unit can't be empty"
                        , row["poid"], row["seq1"], row["seq2"], row["roll"], row["dyelot"]) + Environment.NewLine);
                }

                if (MyUtility.Check.Empty(row["stocktype"]))
                {
                    warningmsg.Append(string.Format(@"SP#: {0} Seq#: {1}-{2} Roll#:{3} Dyelot:{4} Stock Type can't be empty"
                        , row["poid"], row["seq1"], row["seq2"], row["roll"], row["dyelot"]) + Environment.NewLine);
                }

                if (row["fabrictype"].ToString().ToUpper() == "F" && (MyUtility.Check.Empty(row["roll"]) || MyUtility.Check.Empty(row["dyelot"])))
                {
                    warningmsg.Append(string.Format(@"SP#: {0} Seq#: {1}-{2} Roll#:{3} Dyelot:{4} Roll and Dyelot can't be empty"
                        , row["poid"], row["seq1"], row["seq2"], row["roll"], row["dyelot"]) + Environment.NewLine);
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

            if (!MyUtility.Check.Empty(DetailDatas) && DetailDatas.Count > 0)
            {
                MyUtility.Tool.ProcessWithDatatable(DetailDatas.CopyToDataTable(), "poid,seq1"
                , @"select #tmp.*
            from #tmp,dbo.po_supp,dbo.supp
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
                string tmpId = Sci.MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "PR", "Receiving", (DateTime)CurrentMaintain["ETA"]);
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
                if (MyUtility.Check.Seek(string.Format(@"select portarrival,docarrival from dbo.export where id='{0}'"
                    , CurrentMaintain["exportid"]), out dr, null))
                {
                    dateBox2.Value = null;
                    dateBox5.Value = null;
                    if (!MyUtility.Check.Empty(dr["portarrival"])) dateBox2.Value = DateTime.Parse(dr["portarrival"].ToString());
                    if (!MyUtility.Check.Empty(dr["docarrival"])) dateBox5.Value = DateTime.Parse(dr["docarrival"].ToString());
                }
                dateBox1.Enabled = (MyUtility.Check.Empty(CurrentMaintain["third"]) || CurrentMaintain["third"].ToString() == "True");
            }

            #region Status Label

            label25.Text = CurrentMaintain["status"].ToString();

            #endregion Status Label
        }

        // detail 新增時設定預設值
        protected override void OnDetailGridInsert(int index = -1)
        {
            base.OnDetailGridInsert(index);
            CurrentDetailData["mdivisionid"] = Sci.Env.User.Keyword;
        }

        // Detail Grid 設定
        protected override void OnDetailGridSetup()
        {
            #region SP# Vaild 判斷此sp#存在po中。

            Ict.Win.DataGridViewGeneratorTextColumnSettings ts4 = new DataGridViewGeneratorTextColumnSettings();
            DataRow dr;
            ts4.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    if (MyUtility.Check.Seek(string.Format("select 1 where exists(select * from po where id = '{0}')", e.FormattedValue), null))
                    {
                        string category = MyUtility.GetValue.Lookup(string.Format("select category from orders where id='{0}'", e.FormattedValue));
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
                        Sci.Win.Tools.SelectItem selepoitem = Prgs.SelePoItem(CurrentDetailData["poid"].ToString(), CurrentDetailData["seq"].ToString(), "left(m.seq1,1) !='7'");
                        DialogResult result = selepoitem.ShowDialog();
                        if (result == DialogResult.Cancel) { return; }
                        x = selepoitem.GetSelecteds();
                    }
                    else
                    {
                        sqlcmd = string.Format(@"select e.poid,e.seq1+e.seq2 as seq, e.Refno, dbo.getmtldesc(e.poid,e.seq1,e.seq2,2,0) as [Description]
,p.ColorID
,(SELECT eta from dbo.export where id = e.id) as eta
,p.InQty,p.pounit,p.StockUnit,p.OutQty,p.AdjustQty
,p.inqty - p.OutQty + p.AdjustQty as balance
,p.LInvQty
,p.fabrictype
,e.seq1
,e.seq2
from dbo.Export_Detail e left join dbo.PO_Supp_Detail p on e.PoID = p.ID and e.Seq1 = p.SEQ1 and e.Seq2 = p.seq2
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
                    CurrentDetailData["shipqty"] = 0m;
                    CurrentDetailData["Actualqty"] = 0m;
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
                            if (!MyUtility.Check.Seek(string.Format(@"select pounit, stockunit,fabrictype from po_supp_detail
where id = '{0}' and seq1 ='{1}'and seq2 = '{2}'", CurrentDetailData["poid"], e.FormattedValue.ToString().PadRight(5).Substring(0, 3), e.FormattedValue.ToString().PadRight(5).Substring(3, 2)), out dr, null))
                            {
                                MyUtility.Msg.WarningBox("Data not found!", "Seq");
                                e.Cancel = true;
                                return;
                            }
                            else
                            {
                                CurrentDetailData["seq"] = e.FormattedValue;
                                CurrentDetailData["seq1"] = e.FormattedValue.ToString().Substring(0, 3);
                                CurrentDetailData["seq2"] = e.FormattedValue.ToString().Substring(3, 2);
                                CurrentDetailData["pounit"] = dr["pounit"];
                                CurrentDetailData["stockunit"] = dr["stockunit"];
                                CurrentDetailData["fabrictype"] = dr["fabrictype"];
                                CurrentDetailData["shipqty"] = 0m;
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
                }
            };

            #endregion Location 右鍵開窗

            #region Ship Qty Valid

            Ict.Win.DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
            {
                if (this.EditMode && e.FormattedValue != null)
                {
                    CurrentDetailData["shipqty"] = e.FormattedValue;
                    CurrentDetailData["Actualqty"] = e.FormattedValue;
                    string rate = MyUtility.GetValue.Lookup(string.Format(@"select Rate from dbo.View_Unitrate v
                    where v.FROM_U ='{0}' and v.TO_U='{1}'", CurrentDetailData["pounit"], CurrentDetailData["stockunit"]));
                    CurrentDetailData["stockqty"] = MyUtility.Math.Round(decimal.Parse(e.FormattedValue.ToString()) * decimal.Parse(rate), 2);
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
                        string rate = MyUtility.GetValue.Lookup(string.Format(@"select Rate from dbo.View_Unitrate v
                    where v.FROM_U ='{0}' and v.TO_U='{1}'", CurrentDetailData["pounit"], CurrentDetailData["stockunit"]));
                        CurrentDetailData["stockqty"] = MyUtility.Math.Round(decimal.Parse(e.FormattedValue.ToString()) * decimal.Parse(rate), 2);
                    }
                }
            };

            #endregion In Qty Valid

            Ict.Win.UI.DataGridViewComboBoxColumn cbb_fabrictype;
            Ict.Win.UI.DataGridViewComboBoxColumn cbb_stocktype;

            #region 欄位設定

            Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("poid", header: "SP#", width: Widths.AnsiChars(13), settings: ts4)  //0
            .Text("seq", header: "Seq", width: Widths.AnsiChars(6), settings: ts)  //1
            .ComboBox("fabrictype", header: "Fabric" + Environment.NewLine + "Type", width: Widths.AnsiChars(10), iseditable: false).Get(out cbb_fabrictype)  //2
            .Numeric("shipqty", header: "Ship Qty", width: Widths.AnsiChars(13), decimal_places: 2, integer_places: 10, settings: ns)    //3
            .Numeric("weight", header: "G.W(kg)", width: Widths.AnsiChars(9), decimal_places: 2, integer_places: 7)    //4
            .Numeric("actualweight", header: "Act.(kg)", width: Widths.AnsiChars(9), decimal_places: 2, integer_places: 7)    //5
            .Text("Roll", header: "Roll#", width: Widths.AnsiChars(9))    //6
            .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(5))    //7
            .Numeric("ActualQty", header: "Actual Qty", width: Widths.AnsiChars(13), decimal_places: 2, integer_places: 10, settings: ns2)    //8
            .Text("pounit", header: "Purchase" + Environment.NewLine + "Unit", width: Widths.AnsiChars(9), iseditingreadonly: true)    //9
            .Numeric("stockqty", header: "Receiving Qty" + Environment.NewLine + "(Stock Unit)", width: Widths.AnsiChars(13), decimal_places: 2, integer_places: 10, iseditingreadonly: true)    //10
            .Text("stockunit", header: "Stock" + Environment.NewLine + "Unit", iseditingreadonly: true)    //11
            .ComboBox("Stocktype", header: "Stock" + Environment.NewLine + "Type", iseditable: false).Get(out cbb_stocktype)   //12
            .Text("Location", header: "Location", settings: ts2, iseditingreadonly: true)    //13
            .Text("remark", header: "Remark")    //14
            ;     //

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
            #region 檢查負數庫存

            sqlcmd = string.Format(@"Select d.poid,d.seq1,d.seq2,d.Roll,d.StockQty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty
from dbo.Receiving_Detail d left join FtyInventory f
on d.mdivisionid = f.mdivisionid
and d.PoId = f.PoId
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

            #region 更新庫存數量 mdivisionPoDetail & ftyinventory
            var bs1 = (from b in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                       group b by new
                       {
                           mdivisionid = b.Field<string>("mdivisionid"),
                           poid = b.Field<string>("poid"),
                           seq1 = b.Field<string>("seq1"),
                           seq2 = b.Field<string>("seq2"),
                           stocktype = b.Field<string>("stocktype")
                       } into m
                       select new
                       {
                           mdivisionid = m.First().Field<string>("mdivisionid"),
                           poid = m.First().Field<string>("poid"),
                           seq1 = m.First().Field<string>("seq1"),
                           seq2 = m.First().Field<string>("seq2"),
                           stocktype = m.First().Field<string>("stocktype"),
                           stockqty = m.Sum(w => w.Field<decimal>("stockqty")),
                           location = string.Join(",", m.Select(r => r.Field<string>("location")).Distinct()),
                       }).ToList();

            foreach (var item in bs1)
            {
                sqlupd2.Append(Prgs.UpdateMPoDetail(2, item.poid, item.seq1, item.seq2, item.stockqty, true, item.stocktype, item.mdivisionid, item.location));
                if (item.stocktype == "I") sqlupd2.Append(Prgs.UpdateMPoDetail(8, item.poid, item.seq1, item.seq2, item.stockqty, true, item.stocktype, item.mdivisionid, item.location));
            }

            sqlupd2.Append("declare @iden as bigint;");
            sqlupd2.Append("create table #tmp (ukey bigint,locationid varchar(10));");
            foreach (DataRow item in DetailDatas)
            {
                sqlupd2.Append(Prgs.UpdateFtyInventory(2, item["mdivisionid"].ToString(), item["poid"].ToString(), item["seq1"].ToString(), item["seq2"].ToString(), (decimal)item["stockqty"]
                    , item["roll"].ToString(), item["dyelot"].ToString(), item["stocktype"].ToString(), true, item["location"].ToString()));
            }
            sqlupd2.Append("drop table #tmp;" + Environment.NewLine);


            #endregion 更新庫存數量 po_supp_detail & ftyinventory
            #region Base on wkno 收料時，需回寫export
            sqlcmd4 = string.Format(@"update dbo.export set whsearrival =(select WhseArrival from dbo.receiving where id='{2}')
,packingarrival = (select PackingReceive from dbo.receiving where id='{2}'), editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, CurrentMaintain["exportid"], CurrentMaintain["id"]);
            #endregion

            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (!(result2 = DBProxy.Current.Execute(null, sqlupd2.ToString())))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlupd2.ToString(), result2);
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
                    _transactionscope = null;
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

            #region 檢查負數庫存

            sqlcmd = string.Format(@"Select d.poid,d.seq1,d.seq2,d.Roll,d.StockQty
,isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) as balanceQty
from dbo.Receiving_Detail d left join FtyInventory f
on d.mdivisionid = f.mdivisionid
and d.PoId = f.PoId
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

            sqlupd2.Append("declare @iden as bigint;");
            sqlupd2.Append("create table #tmp (ukey bigint,locationid varchar(10));");
            foreach (DataRow item in DetailDatas)
            {
                sqlupd2.Append(Prgs.UpdateFtyInventory(2, item["mdivisionid"].ToString(), item["poid"].ToString(), item["seq1"].ToString(), item["seq2"].ToString(), (decimal)item["stockqty"]
                    , item["roll"].ToString(), item["dyelot"].ToString(), item["stocktype"].ToString(), false, item["location"].ToString()));
            }
            sqlupd2.Append("drop table #tmp;" + Environment.NewLine);
            var bs1 = (from b in ((DataTable)detailgridbs.DataSource).AsEnumerable()
                       group b by new
                       {
                           mdivisionid = b.Field<string>("mdivisionid"),
                           poid = b.Field<string>("poid"),
                           seq1 = b.Field<string>("seq1"),
                           seq2 = b.Field<string>("seq2"),
                           stocktype = b.Field<string>("stocktype")
                       } into m
                       select new
                       {
                           mdivisionid = m.First().Field<string>("mdivisionid"),
                           poid = m.First().Field<string>("poid"),
                           seq1 = m.First().Field<string>("seq1"),
                           seq2 = m.First().Field<string>("seq2"),
                           stocktype = m.First().Field<string>("stocktype"),
                           stockqty = m.Sum(w => w.Field<decimal>("stockqty"))
                       }).ToList();

            foreach (var item in bs1)
            {
                sqlupd2.Append(Prgs.UpdateMPoDetail(2, item.poid, item.seq1, item.seq2, item.stockqty, false, item.stocktype, item.mdivisionid));
                if (item.stocktype == "I") sqlupd2.Append(Prgs.UpdateMPoDetail(8, item.poid, item.seq1, item.seq2, item.stockqty, false, item.stocktype, item.mdivisionid));
            }

            #endregion 更新庫存數量 po_supp_detail & ftyinventory

            sqlcmd4 = string.Format(@"update dbo.export set whsearrival =null,packingarrival =null, editname = '{0}' , editdate = GETDATE()
                                where id = '{1}'", Env.User.UserID, CurrentMaintain["exportid"], CurrentMaintain["id"]);

            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (!(result2 = DBProxy.Current.Execute(null, sqlupd2.ToString())))
                    {
                        _transactionscope.Dispose();
                        ShowErr(sqlupd2.ToString(), result2);
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

            this.DetailSelectCommand = string.Format(@"select a.id,a.MDivisionID,a.PoId,a.Seq1,a.Seq2,left(a.seq1+' ',3)+a.Seq2 as seq
,(select p1.FabricType from PO_Supp_Detail p1 where p1.ID = a.PoId and p1.seq1 = a.SEQ1 and p1.SEQ2 = a.seq2) as fabrictype
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
from dbo.Receiving_Detail a
Where a.id = '{0}' ", masterID);

            return base.OnDetailSelectCommandPrepare(e);

        }

        // invoice# valid
        private void textBox3_Validating(object sender, CancelEventArgs e)
        {
            DataRow dr;
            DataTable dt;
            if (!MyUtility.Check.Empty(textBox3.Text) && textBox3.Text != textBox3.OldValue)
            {
                foreach (DataRow dr2 in ((DataTable)detailgridbs.DataSource).Rows)
                {
                    dr2.Delete();
                }
                CurrentMaintain["invno"] = textBox3.Text;
                if (MyUtility.Check.Seek(string.Format("select packingarrival,whsearrival,eta from dbo.export where id='{0}'"
                    , textBox3.Text), out dr, null))
                {
                    CurrentMaintain["exportid"] = CurrentMaintain["invno"];
                    CurrentMaintain["PackingReceive"] = dr["packingarrival"];
                    CurrentMaintain["WhseArrival"] = dr["WhseArrival"];
                    CurrentMaintain["ETA"] = dr["ETA"];
                    CurrentMaintain["third"] = 0;
                    this.dateBox1.Enabled = false;

                    DBProxy.Current.Select(null, string.Format(@"select a.poid,a.seq1,a.seq2,a.Qty+a.Foc as shipqty,a.UnitId,a.WeightKg as Weight
, a.NetKg as ActualWeight, iif(c.category='M','I','B') as stocktype
, b.POUnit ,b.StockUnit,b.FabricType
, a.seq1+a.seq2 as seq
, a.seq1,a.seq2,a.Qty+a.Foc as Actualqty
, round((a.Qty+a.Foc)*v.rate,2) as stockqty
, '' as roll
, '' as dyelot
, '' as remark
, '' as location
, '{1}' as mdivisionid
from dbo.Export_Detail a inner join dbo.PO_Supp_Detail b on a.PoID= b.id and a.Seq1 = b.SEQ1 and a.Seq2 = b.SEQ2
inner join orders c on c.id = a.poid
inner join View_unitrate v on v.FROM_U = b.POUnit and v.TO_U = b.StockUnit
where a.id='{0}'", CurrentMaintain["exportid"], Sci.Env.User.Keyword), out dt);
                    if (MyUtility.Check.Empty(dt) || MyUtility.Check.Empty(dt.Rows.Count))
                    {
                        MyUtility.Msg.WarningBox("Export Data not found!!");
                        return;
                    }
                    foreach (var item in dt.ToList())
                    {
                        //DetailDatas.(item);
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
            foreach (DataRow dr in ((DataTable)detailgridbs.DataSource).Rows)
            {
                dr.Delete();
            }
        }

        //Accumulated Qty
        private void btAccumulated_Click(object sender, EventArgs e)
        {
            var frm = new Sci.Production.Warehouse.P07_AccumulatedQty(CurrentMaintain);
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
            int index = detailgridbs.Find("poid", textBox1.Text.TrimEnd());
            if (index == -1)
            { MyUtility.Msg.WarningBox("Data was not found!!"); }
            else
            { detailgridbs.Position = index; }
        }

        private void btImportFromExcel_Click(object sender, EventArgs e)
        {
            Sci.Production.Warehouse.P07_ExcelImport callNextForm = new Sci.Production.Warehouse.P07_ExcelImport(CurrentMaintain,(DataTable)detailgridbs.DataSource);
            callNextForm.ShowDialog(this);
        }

        private void btDownloadSample_Click(object sender, EventArgs e)
        {
            string strXltName = Sci.Env.Cfg.XltPathDir + "Warehouse_P07_ImportExcelFormat.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return;
            excel.Visible = true;
        }
        protected override bool ClickPrint()
        {
            P07_Print p = new P07_Print();
            p.CurrentDataRow = this.CurrentDataRow;
            p.ShowDialog();

            return true;
        }
    }
}