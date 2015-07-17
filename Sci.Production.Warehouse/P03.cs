using System;
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
using Sci.Production;

using Sci.Production.PublicPrg;
using System.Linq;

namespace Sci.Production.Warehouse
{
    public partial class P03 : Sci.Win.Tems.Input6
    {
        public P03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.DefaultFilter = "id = poid and IsForecast = 0 and WhseClose is null";
            ChangeDetailColor();
            MyUtility.Tool.SetGridFrozen(this.detailgrid);
        }

        private void ChangeDetailColor()
        {
            detailgrid.RowPostPaint += (s, e) =>
            {
                //DataGridViewRow dvr = detailgrid.Rows[e.RowIndex];
                //DataRow dr = ((DataRowView)dvr.DataBoundItem).Row;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (detailgrid.Rows.Count <= e.RowIndex || e.RowIndex < 0) return;
                
                int i = e.RowIndex;
                if (dr["junk"].ToString()=="True")
                {
                    detailgrid.Rows[i].DefaultCellStyle.BackColor = Color.Gray;
                }
                else
                {
                    if (dr["ThirdCountry"].ToString() == "True")
                    {
                        detailgrid.Rows[i].Cells[2].Style.BackColor = Color.DeepPink;
                    }

                    if (dr["BomTypeCalculate"].ToString() == "True")
                    {
                        detailgrid.Rows[i].Cells[6].Style.BackColor = Color.Orange;
                    }
                }
            };
        }

        public P03(ToolStripMenuItem menuitem, string history)
            : base(menuitem)
        {
            InitializeComponent();
            if (history.ToUpper() != "Y")
            {
                this.DefaultFilter = "id = poid and IsForecast = 0 and WhseClose is null";
            }
            else
            {
                this.DefaultFilter = "id = poid and IsForecast = 0 and WhseClose is not null";
                this.Text += " (History)";
            }
            ChangeDetailColor();
            MyUtility.Tool.SetGridFrozen(this.detailgrid);
            
        }

        //refresh
        protected override void OnDetailEntered()
        {
            DataRow dr;
            bool result;

            base.OnDetailEntered();

            if (!(CurrentMaintain == null))
            {
                result = MyUtility.Check.Seek(string.Format(@"select isnull(sum(qty),0) as cutqty from cuttingOutput_detail_detail where CuttingID='{0}'", CurrentMaintain["id"]), out dr, null);
                if (result) numericBox_cutqty.Value = (decimal)dr[0];
                dr = null;
                result = MyUtility.Check.Seek(string.Format(@"select isnull(sum(workday),0) as workday from sewingschedule where orderid ='{0}'", CurrentMaintain["id"]), out dr, null);
                //if (result) numericBox_NeedPerDay.Value = decimal.Parse(dr[0].ToString());
            }
            
        }

        // grid 加工填值
        protected override DualResult OnRenewDataDetailPost(RenewDataPostEventArgs e)
        {
            string tmp = "";
            if (!tabs.TabPages[0].Equals(tabs.SelectedTab))
            {
                //(e.Details).Columns.Add("description", typeof(string));
                foreach (DataRow dr in e.Details.Rows)
                {
                    dr["description"] = PublicPrg.Prgs.GetMtlDesc(dr["id"].ToString(), dr["seq1"].ToString(), dr["seq2"].ToString(), 3,tmp == dr["refno"].ToString());
                    tmp = dr["refno"].ToString();
                }
            }
            return base.OnRenewDataDetailPost(e);
        }

        // Detail Grid 設定
        protected override void OnDetailGridSetup()
        {
            #region Supp 開窗
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts1 = new DataGridViewGeneratorTextColumnSettings();
            ts1.CellMouseDoubleClick += (s, e) =>
            {
                if (!this.EditMode)
                {
                    var dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (null == dr) return;
                    var frm = new Sci.Production.Warehouse.P03_Supplier(dr);
                    frm.ShowDialog(this);
                }

            };
            #endregion

            #region refno 開窗
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts2 = new DataGridViewGeneratorTextColumnSettings();
            ts2.CellMouseDoubleClick += (s, e) =>
            {
                if (!this.EditMode)
                {
                    var dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (null == dr) return;
                    var frm = new Sci.Production.Warehouse.P03_Refno(dr);
                    frm.ShowDialog(this);
                }

            };
            #endregion

            #region Ship qty 開窗
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts3 = new DataGridViewGeneratorTextColumnSettings();
            ts3.CellMouseDoubleClick += (s, e) =>
            {
                if (!this.EditMode)
                {
                    var dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (null == dr) return;
                    var frm = new Sci.Production.Warehouse.P03_Wkno(dr);
                    frm.ShowDialog(this);
                }

            };
            #endregion
            #region Taipei Stock Qty 開窗
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts4 = new DataGridViewGeneratorTextColumnSettings();
            ts4.CellMouseDoubleClick += (s, e) =>
            {
                if (!this.EditMode)
                {
                    var dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (null == dr) return;
                    var frm = new Sci.Production.Warehouse.P03_TaipeiInventory(dr);
                    frm.ShowDialog(this);
                }

            };
            #endregion
            #region Released Qty 開窗
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts5 = new DataGridViewGeneratorTextColumnSettings();
            ts5.CellMouseDoubleClick += (s, e) =>
            {
                if (!this.EditMode)
                {
                    var dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (null == dr) return;
                    var frm = new Sci.Production.Warehouse.P03_RollTransaction(dr);
                    frm.ShowDialog(this);
                }

            };
            #endregion
            #region Balance Qty 開窗
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts6 = new DataGridViewGeneratorTextColumnSettings();
            ts6.CellMouseDoubleClick += (s, e) =>
            {
                if (!this.EditMode)
                {
                    var dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (null == dr) return;
                    var frm = new Sci.Production.Warehouse.P03_Transaction(dr);
                    frm.ShowDialog(this);
                }

            };
            #endregion
            #region Inventory Qty 開窗
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts7 = new DataGridViewGeneratorTextColumnSettings();
            ts7.CellMouseDoubleClick += (s, e) =>
            {
                if (!this.EditMode)
                {
                    var dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (null == dr) return;
                    var frm = new Sci.Production.Warehouse.P03_InventoryStatus(dr);
                    frm.ShowDialog(this);
                }

            };
            #endregion
            #region Scrap Qty 開窗
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts8 = new DataGridViewGeneratorTextColumnSettings();
            ts8.CellMouseDoubleClick += (s, e) =>
            {
                if (!this.EditMode)
                {
                    var dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (null == dr) return;
                    var frm = new Sci.Production.Warehouse.P03_Scrap(dr);
                    frm.ShowDialog(this);
                }

            };
            #endregion
            #region Bulk Location 開窗
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts9 = new DataGridViewGeneratorTextColumnSettings();
            ts9.CellMouseDoubleClick += (s, e) =>
            {
                if (!this.EditMode)
                {
                    var dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (null == dr) return;
                    var frm = new Sci.Production.Warehouse.P03_BulkLocation(dr);
                    frm.ShowDialog(this);
                }

            };
            #endregion
            #region FIR 開窗
            Ict.Win.DataGridViewGeneratorTextColumnSettings ts10 = new DataGridViewGeneratorTextColumnSettings();
            ts10.CellMouseDoubleClick += (s, e) =>
            {
                if (!this.EditMode)
                {
                    var dr = this.detailgrid.GetDataRow<DataRow>(e.RowIndex);
                    if (null == dr) return;
                    var frm = new Sci.Production.Warehouse.P03_InspectionList(dr);
                    frm.ShowDialog(this);
                }

            };
            #endregion
            
//            Ict.Win.DataGridViewGeneratorTextColumnSettings ts4 = new DataGridViewGeneratorTextColumnSettings();
//            #region Supplier 右鍵開窗
//            ts4.EditingMouseDown += (s, e) =>
//            {
//                if (this.EditMode && e.Button == MouseButtons.Right && CurrentDetailData["InhouseOSP"].ToString() == "O")
//                {
//                    Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem
//                        (string.Format(@"SELECT QU.LocalSuppId,LOCALSUPP.Abb,QU.Mockup
//                                                    FROM Order_TmsCost OT
//                                                    INNER JOIN ORDERS ON OT.ID = ORDERS.ID
//                                                    INNER JOIN Style_Artwork SA ON OT.ArtworkTypeID = SA.ArtworkTypeID AND ORDERS.StyleUkey = SA.StyleUkey
//                                                    LEFT JOIN Style_Artwork_Quot QU ON QU.Ukey = SA.Ukey
//                                                    INNER JOIN LocalSupp ON LocalSupp.ID = QU.LocalSuppId
//                                                    WHERE PriceApv ='Y' AND MOCKUP IS NOT NULL AND OT.ID = '{0}' AND OT.ARTWORKTYPEID='{1}'
//                                                    GROUP BY QU.LocalSuppId,LOCALSUPP.Abb,QU.Mockup", CurrentDetailData["ID"], CurrentDetailData["Artworktypeid"])
//                                                                                                                                                                                     , "10,15,12", null, null);
//                    DialogResult result = item.ShowDialog();
//                    if (result == DialogResult.Cancel) { return; }
//                    CurrentDetailData["localsuppid"] = item.GetSelectedString();
//                }
//            };
//            #endregion

//            ts4.CellValidating += (s, e) =>
//            {
//                if (this.EditMode && !MyUtility.Check.Empty(e.FormattedValue))
//                {
//                    if (CurrentDetailData["InhouseOSP"].ToString() == "O")
//                    {
//                        bool exist = false;
//                        DualResult result = DBProxy.Current.Exists(null, string.Format(@"SELECT QU.LocalSuppId,LOCALSUPP.Abb,QU.Mockup
//                                                    FROM Order_TmsCost OT
//                                                    INNER JOIN ORDERS ON OT.ID = ORDERS.ID
//                                                    INNER JOIN Style_Artwork SA ON OT.ArtworkTypeID = SA.ArtworkTypeID AND ORDERS.StyleUkey = SA.StyleUkey
//                                                    LEFT JOIN Style_Artwork_Quot QU ON QU.Ukey = SA.Ukey
//                                                    INNER JOIN LocalSupp ON LocalSupp.ID = QU.LocalSuppId
//                                                    WHERE PriceApv ='Y' AND MOCKUP IS NOT NULL AND OT.ID = '{0}' 
//                                                        AND OT.ARTWORKTYPEID = '{1}' AND qu.Localsuppid = '{2}'
//                                                    GROUP BY QU.LocalSuppId,LOCALSUPP.Abb,QU.Mockup"
//                                                    , CurrentDetailData["ID"], CurrentDetailData["Artworktypeid"], e.FormattedValue), null, out exist);
//                        if (!exist)
//                        {
//                            e.Cancel = true;
//                            MyUtility.Msg.WarningBox("Supplier not in Style Quotation or not Mock approved or not Price approved!!", "Warning");
//                            return;
//                        }
//                    }
//                }
//            };

            
            //Dictionary<String, String> comboBox1_RowSource = new Dictionary<string, string>();
            //comboBox1_RowSource.Add("O", "OSP");
            //comboBox1_RowSource.Add("I", "InHouse");

//            Ict.Win.DataGridViewGeneratorComboBoxColumnSettings cs = new DataGridViewGeneratorComboBoxColumnSettings();
//            cs.CellValidating += (s, e) =>
//            {
//                if (this.EditMode && !MyUtility.Check.Empty(e.FormattedValue) && MyUtility.Check.Empty(CurrentDetailData["localsuppid"]))
//                {
//                    if (e.FormattedValue.ToString() == "O")
//                    {
//                        CurrentDetailData["localsuppid"] = MyUtility.GetValue.Lookup(string.Format(@"SELECT top 1 QU.LocalSuppId
//                                                    FROM Order_TmsCost OT
//                                                    INNER JOIN ORDERS ON OT.ID = ORDERS.ID
//                                                    INNER JOIN Style_Artwork SA ON OT.ArtworkTypeID = SA.ArtworkTypeID AND ORDERS.StyleUkey = SA.StyleUkey
//                                                    LEFT JOIN Style_Artwork_Quot QU ON QU.Ukey = SA.Ukey
//                                                    INNER JOIN LocalSupp ON LocalSupp.ID = QU.LocalSuppId
//                                                    WHERE PriceApv ='Y' AND MOCKUP IS NOT NULL AND OT.ID = '{0}' 
//                                                        AND OT.ARTWORKTYPEID = '{1}' 
//                                                    GROUP BY QU.LocalSuppId,LOCALSUPP.Abb,QU.Mockup"
//                                                    , CurrentDetailData["ID"], CurrentDetailData["Artworktypeid"]), null);
//                        CurrentDetailData["inhouseOSP"] = e.FormattedValue;
//                    }
//                    if (e.FormattedValue.ToString() == "I")
//                    {
//                        CurrentDetailData["localsuppid"] = Env.User.Factory;
//                        CurrentDetailData["inhouseOSP"] = e.FormattedValue;
//                    }
//                }
//            };

            #region 欄位設定
            Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("seq1", header: "Seq1", iseditingreadonly: true, width: Widths.AnsiChars(4))  //0
            .Text("seq2", header: "Seq2", iseditingreadonly: true, width: Widths.AnsiChars(4))  //1
            .Text("Suppid", header: "Supp", iseditingreadonly: true, width: Widths.AnsiChars(4),settings:ts1)  //2
            .Text("eta", header: "Sup. 1st "+Environment.NewLine+"Cfm ETA", width: Widths.AnsiChars(6), iseditingreadonly: true)    //3
            .Text("RevisedETD", header: "Sup. Delivery"+Environment.NewLine+"Rvsd ETA", width: Widths.AnsiChars(6), iseditingreadonly: true)    //4
            .Text("refno", header: "Ref#", iseditingreadonly: true,settings:ts2)  //5
            .Text("description", header: "Description", iseditingreadonly: true)  //6
            .Text("fabrictype2", header: "Fabric Type", iseditingreadonly: true)  //7
            .Text("ColorID", header: "Color", iseditingreadonly: true)  //8
            .Text("SizeSpec", header: "Size", iseditingreadonly: true)  //9
            .Text("CurrencyID", header: "Currency", iseditingreadonly: true)  //10
            .Numeric("unitqty", header: "@Qty", width: Widths.AnsiChars(6), iseditingreadonly: true)    //11
            .Numeric("Qty", header: "Order Qty", width: Widths.AnsiChars(6), iseditingreadonly: true)    //12
            .Numeric("NETQty", header: "Net Qty", width: Widths.AnsiChars(6), iseditingreadonly: true)    //13
            .Numeric("useqty", header: "Use Qty", width: Widths.AnsiChars(6), iseditingreadonly: true)    //14
            .Text("ShipQty", header: "Ship Qty", width: Widths.AnsiChars(6), iseditingreadonly: true, settings:ts3)    //15
            .Numeric("ShipFOC", header: "F.O.C", width: Widths.AnsiChars(6), iseditingreadonly: true)    //16
            .Numeric("ApQty", header: "AP Qty", width: Widths.AnsiChars(6), iseditingreadonly: true)    //17
            .Text("InputQty", header: "Taipei Stock Qty", width: Widths.AnsiChars(6), iseditingreadonly: true, settings: ts4)    //18
            .Text("POUnit", header: "PO Unit", iseditingreadonly: true)  //19
            .Text("Complete", header: "Cmplt", iseditingreadonly: true)  //20
            .Date("ATA", header: "Act. ETA", width: Widths.AnsiChars(6), iseditingreadonly: true)    //21
            .Text("OrderIdList", header: "Order List", iseditingreadonly: true)  //23
            .Numeric("InQty", header: "Arrived Qty", width: Widths.AnsiChars(6), iseditingreadonly: true)    //23
            .Text("StockUnit", header: "Stock Unit", iseditingreadonly: true)  //24
            .Text("OutQty", header: "Released Qty", width: Widths.AnsiChars(6), iseditingreadonly: true, settings: ts5)    //25
            .Numeric("AdjustQty", header: "Adjust Qty", width: Widths.AnsiChars(6), iseditingreadonly: true)    //26
            .Text("balanceqty", header: "Balance", width: Widths.AnsiChars(6), iseditingreadonly: true, settings: ts6)    //27
            .Text("LInvQty", header: "Stock Qty", width: Widths.AnsiChars(6), iseditingreadonly: true, settings: ts7)    //28
            .Text("LObQty", header: "Scrap Qty", width: Widths.AnsiChars(6), iseditingreadonly: true, settings: ts8)    //29
            .Text("ALocation", header: "Bulk Location", iseditingreadonly: true,settings:ts9)  //30
            .Text("BLocation", header: "Stock Location", iseditingreadonly: true)  //31
            .Text("Remark", header: "Remark", iseditingreadonly: true)  //32
            ;     
            #endregion

            detailgrid.Columns[7].Frozen = true;  //Fabric Type

            //#region 可編輯欄位變色
            //detailgrid.Columns[4].DefaultCellStyle.BackColor = Color.Pink;  //PCS/Stitch
            //detailgrid.Columns[5].DefaultCellStyle.BackColor = Color.Pink;  //Cutpart Name
            //detailgrid.Columns[14].DefaultCellStyle.BackColor = Color.Pink; //Unit Price
            //detailgrid.Columns[15].DefaultCellStyle.BackColor = Color.Pink; //Qty/GMT
            //#endregion
        }

        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["ID"].ToString();

            this.DetailSelectCommand = string.Format(@"select a.id,a.seq1,seq2,b.SuppID,substring(convert(varchar, a.eta, 101),1,5) as eta
            ,substring(convert(varchar,a.RevisedETD, 101),1,5) as RevisedETD,a.Refno,a.SCIRefno
            ,a.FabricType , iif(a.FabricType='F','Fabric',iif(a.FabricType='A','Accessory',a.FabricType)) as fabrictype2,a.ColorID,a.SizeSpec
            ,a.UsedQty unitqty,A.Qty,A.NETQty,A.NETQty+A.lossQty useqty ,a.ShipQty,a.ShipFOC,a.ApQty,a.InputQty,a.POUnit,a.Complete
            ,a.ATA,a.OrderIdList,a.InQty,a.StockUnit
            ,a.OutQty,a.AdjustQty,a.InQty - a.OutQty + a.AdjustQty balanceqty,a.LInvQty,a.LObQty,a.ALocation,a.BLocation 
            ,s.ThirdCountry,a.junk,fabric.BomTypeCalculate,'' AS description,s.currencyid
            ,(Select cast(tmp.Remark as nvarchar)+',' 
                        from (select b1.remark 
                                    from receiving a1 
                                    inner join receiving_detail b1 on a1.id = b1.id 
                                    where a1.status = 'Confirmed' and (b1.Remark is not null or b1.Remark !='')
                                        and b1.poid = a.id
                                        and b1.seq1 = a.seq1
                                        and b1.seq2 = a.seq2 group by b1.remark) tmp 
                        for XML PATH('')) as  Remark
            from PO_Supp_Detail a
                inner join fabric on fabric.SCIRefno = a.scirefno
	            left join po_supp b on a.id = b.id and a.SEQ1 = b.SEQ1
                left join supp s on s.id = b.suppid
            where a.id='{0}' order by a.refno,a.colorid", masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    if (MyUtility.Check.Empty(detailgridbs)) break;
                    ((DataTable)detailgridbs.DataSource).DefaultView.Sort = "refno , colorid";
                    //detailgridbs.Sort(new IComparable[] { "refno", "colorid" });
                    //detailgridbs.Sort = "refno , colorid";
                    break;
                case 1:
                    if (MyUtility.Check.Empty(detailgridbs)) break;
                    ((DataTable)detailgridbs.DataSource).DefaultView.Sort = "seq1 , seq2";
                    break;
                
            }
        }
    }
}
