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
using Sci.Win.Tools;

namespace Sci.Production.Cutting
{
    public partial class P02 : Sci.Win.Tems.Input6
    {
        private string loginID = Sci.Env.User.UserID;
        private string keyWord = Sci.Env.User.Keyword;

        private DataTable sizeratioTb, layersTb, distqtyTb, qtybreakTb, sizeGroup, spTb, artTb, PatternPanelTb;
        private DataRow drTEMP;  //紀錄目前表身選擇的資料，避免按列印時模組會重LOAD資料，導致永遠只能印到第一筆資料

        private Sci.Win.UI.BindingSource2 bindingSource2 = new Win.UI.BindingSource2();

        Ict.Win.UI.DataGridViewTextBoxColumn col_Markername;
        Ict.Win.UI.DataGridViewTextBoxColumn col_sp;
        Ict.Win.UI.DataGridViewTextBoxColumn col_seq1;
        Ict.Win.UI.DataGridViewTextBoxColumn col_seq2;
        Ict.Win.UI.DataGridViewTextBoxColumn col_cutcell;
        Ict.Win.UI.DataGridViewNumericBoxColumn col_cutno;
        Ict.Win.UI.DataGridViewNumericBoxColumn col_layer;
        Ict.Win.UI.DataGridViewDateBoxColumn col_estcutdate;
        Ict.Win.UI.DataGridViewTextBoxColumn col_cutref;
        Ict.Win.UI.DataGridViewTextBoxColumn col_sizeRatio_size;
        Ict.Win.UI.DataGridViewNumericBoxColumn col_sizeRatio_qty;
        Ict.Win.UI.DataGridViewTextBoxColumn col_dist_size;
        Ict.Win.UI.DataGridViewTextBoxColumn col_dist_article;
        Ict.Win.UI.DataGridViewTextBoxColumn col_dist_sp;
        Ict.Win.UI.DataGridViewNumericBoxColumn col_dist_qty;

        public P02(ToolStripMenuItem menuitem, string history)
            : base(menuitem)
        {


            InitializeComponent();

            Dictionary<String, String> comboBox1_RowSource = new Dictionary<string, string>();
            comboBox1_RowSource.Add("LectraCode", "Pattern Panel");
            comboBox1_RowSource.Add("SP", "SP");
            comboBox1_RowSource.Add("Cut#", "Cut#");
            comboBox1_RowSource.Add("Ref#", "Ref#");
            comboBox1_RowSource.Add("Cutplan#", "Cutplan#");
            comboBox1_RowSource.Add("MarkerName", "MarkerName");
            comboBox1.DataSource = new BindingSource(comboBox1_RowSource, null);
            comboBox1.ValueMember = "Key";
            comboBox1.DisplayMember = "Value";

            /*
             *設定Binding Source for Text
            */
            this.displayBox_MarkerName.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource2, "MarkerName", true));
            this.displayBox_Color.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource2, "colorid", true));
            this.numericBox_UnitCons.DataBindings.Add(new System.Windows.Forms.Binding("Value", bindingSource2, "Conspc", true));
            this.txtCell1.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource2, "CutCellid", true));
            this.numericBox_Cons.DataBindings.Add(new System.Windows.Forms.Binding("Value", bindingSource2, "Cons", true));
            this.textBox_FabricCombo.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource2, "FabricCombo", true));
            this.textBox_LectraCode.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource2, "LectraCode", true));
            this.displayBox_FabricRefno.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource2, "SCIRefno", true));
            this.displayBox_WorkOrderDownloadid.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource2, "MarkerDownLoadId", true));
            this.displayBox_Cutplanid.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource2, "Cutplanid", true));
            this.displayBox_TotalCutQty.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource2, "CutQty", true));
            this.numericBox_MarkerLengthY.DataBindings.Add(new System.Windows.Forms.Binding("Value", bindingSource2, "MarkerLengthY", true));
            this.textBox_MarkerLengthE.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource2, "MarkerLengthE", true));
            this.textBox_PatternPanel.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource2, "PatternPanel", true));

            sizeratioMenuStrip.Enabled = this.EditMode;
            distributeMenuStrip.Enabled = this.EditMode;

            if (history == "0")
            {
                this.Text = "P02.Cutting Work Order";
                this.IsSupportEdit = true;
                this.DefaultFilter = string.Format("mDivisionid = '{0}' and WorkType is not null and WorkType != '' and Finished = 0", keyWord);
            }
            else
            {
                this.Text = "P02.Cutting Work Order(History)";
                this.IsSupportEdit = false;
                this.DefaultFilter = string.Format("mDivisionid = '{0}' and WorkType is not null and WorkType != '' and Finished = 1", keyWord);
            }
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            sizeratio_grid.DataSource = sizeratiobs;
            sizeratiobs.DataSource = sizeratioTb;
            distributebs.DataSource = distqtyTb;
            distribute_grid.DataSource = distributebs;
            qtybreakds.DataSource = qtybreakTb;
            qtybreak_grid.DataSource = qtybreakds;

            sizeratioTb.DefaultView.RowFilter = "";
            qtybreakTb.DefaultView.RowFilter = "";
            OnDetailGridRowChanged();

            DataRow orderdr;
            MyUtility.Check.Seek(string.Format("Select * from Orders where id='{0}'", CurrentMaintain["ID"]), out orderdr);

            textbox_Style.Text = orderdr == null ? "" : orderdr["Styleid"].ToString();
            textbox_Line.Text = orderdr == null ? "" : orderdr["SewLine"].ToString();
            string maxcutrefCmd = string.Format("Select Max(Cutref) from workorder WITH (NOLOCK) where mDivisionid = '{0}'", keyWord);
            textbox_LastCutRef.Text = MyUtility.GetValue.Lookup(maxcutrefCmd);
            comboBox1.Enabled = !EditMode;  //Sorting於編輯模式時不可選取

            //617: CUTTING_P02_Cutting Work Order，(5) Article值不正確 (最後多了一個/)
            foreach (DataRow dr in DetailDatas) dr["Article"] = dr["Article"].ToString().TrimEnd('/');
            sorting(comboBox1.Text);
        }

        protected override Ict.DualResult OnDetailSelectCommandPrepare(Win.Tems.InputMasterDetail.PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["id"].ToString();
            string cmdsql = string.Format(
            @"
            Select a.*,
            (
                Select distinct Article+'/' 
			    From dbo.WorkOrder_Distribute b
			    Where b.workorderukey = a.Ukey and b.article!=''
                For XML path('')
            ) as article,
            (
                Select c.sizecode+'/ '+convert(varchar(8),c.qty)+', ' 
                From WorkOrder_SizeRatio c
                Where c.WorkOrderUkey =a.Ukey 
                For XML path('')
            ) as SizeCode,
            (
                Select c.sizecode+'/ '+convert(varchar(8),c.qty*a.layer)+', ' 
                From WorkOrder_SizeRatio c
                Where  c.WorkOrderUkey =a.Ukey 
                For XML path('')
            ) as CutQty,
            (
                Select LectraCode+'+ ' 
                From WorkOrder_PatternPanel c
                Where c.WorkOrderUkey =a.Ukey 
                For XML path('')
            ) as LectraCode,
            (
                Select PatternPanel+'+ ' 
                From WorkOrder_PatternPanel c
                Where c.WorkOrderUkey =a.Ukey 
                For XML path('')
            ) as PatternPanel,
			(
				Select iif(e.Complete=1,e.FinalETA,iif(e.Eta is not null,e.eta,iif(e.shipeta is not null,e.shipeta,e.finaletd)))
				From PO_Supp_Detail e 
				Where e.id = (Select distinct poid from orders where orders.cuttingsp = '{0}') and e.seq1 = a.seq1 and e.seq2 = a.seq2
			) as fabeta,
			(
				Select Min(sew.Inline)
				From SewingSchedule sew ,SewingSchedule_detail sew_b,WorkOrder_Distribute h
				Where h.WorkOrderUkey = a.ukey and sew.id=sew_b.id and h.orderid = sew_b.OrderID 
						and h.Article = sew_b.Article and h.SizeCode = h.SizeCode and h.orderid = sew.orderid
			)  as Sewinline,
			(
				Select Min(cut.cdate)
				From cuttingoutput cut ,cuttingoutput_detail cut_b
				Where cut_b.workorderukey = a.Ukey and cut.id = cut_b.id
			)  as actcutdate,
            (
                Select Name 
                From Pass1 ps
                Where ps.id = a.addName
            ) as adduser,
            (
                Select Name 
                From Pass1 ps
                Where ps.id = a.editName
            ) as edituser,
            (
               Select sum(layer)
                From Order_EachCons ea,Order_EachCons_color ea_b
                Where ea.id = a.id and ea.id = '{0}' and ea.ukey = ea_b.order_eachConsUkey 
                    and ea.Markername = a.markername and ea_b.colorid = a.colorid
            ) as totallayer,
			(
				Select iif(count(size.sizecode)>1,2,1) 
				From WorkOrder_SizeRatio size
				Where a.ukey = size.WorkOrderUkey
			) as multisize,

            --617: CUTTING_P02_Cutting Work Order
			(
				select SEQ
				from (select WO.Ukey , max(c.Seq) SEQ
						from WorkOrder WO
						left join WorkOrder_SizeRatio b on b.WorkOrderUkey=WO.Ukey
						left join Order_SizeCode c on c.Id=b.ID and c.SizeCode=b.SizeCode
						where WO.ID='{0}'
						group by WO.Ukey) tmp
				where tmp.Ukey=a.Ukey
			) as Order_SizeCode_Seq,

            0 As SORT_NUM,  --617: CUTTING_P02_Cutting Work Order

			c.MtlTypeID,c.DescDetail,0 as newkey,substring(a.MarkerLength,1,2) as MarkerLengthY, 
            substring(a.MarkerLength,4,13) as MarkerLengthE
			from Workorder a
			left join fabric c on c.SCIRefno = a.SCIRefno
            where a.id = '{0}'
            ", masterID);
            this.DetailSelectCommand = cmdsql;
            #region SizeRatio
            cmdsql = string.Format("Select *,0 as newKey from Workorder_SizeRatio where id = '{0}'", masterID);
            DualResult dr = DBProxy.Current.Select(null, cmdsql, out sizeratioTb);
            if (!dr)
            {
                ShowErr(cmdsql, dr);

            }
            // sizeratioTb.Columns.Add("newKey", typeof(int));
            #endregion
            #region layer
            cmdsql = string.Format
            (
                @"Select a.MarkerName,a.Colorid,a.Order_EachconsUkey,isnull(sum(a.layer),0) as layer,
                    
                    --(Select isnull(sum(c.layer),0) as TL
				    --from Order_EachCons b, Order_EachCons_Color c 
				    --where b.id = '{0}' and b.id = c.id and 
                    --b.ukey=c.Order_EachConsUkey and a.Order_EachconsUkey = b.Ukey ) as TotallayerUkey,
                    (Select isnull(sum(c.layer),0) as TL
	                from Order_EachCons b, Order_EachCons_Color c 
	                where b.id = '{0}' and b.id = c.id and a.MarkerName = b.Markername and a.Colorid = c.Colorid
                    and b.ukey=c.Order_EachConsUkey and a.Order_EachconsUkey = b.Ukey )  as TotallayerUkey,

                    (Select isnull(sum(c.layer),0) as TL2
				    from Order_EachCons b, Order_EachCons_Color c 
				    where b.id = '{0}' and b.id = c.id and 
                    b.ukey=c.Order_EachConsUkey and a.MarkerName = b.Markername 
                    and a.Colorid = c.Colorid )  
                    as TotallayerMarker
                From WorkOrder a 
                Where a.id = '{0}' 
                group by a.MarkerName,a.Colorid,a.Order_EachconsUkey
                Order by a.MarkerName,a.Colorid,a.Order_EachconsUkey
                ", masterID
            );
            dr = DBProxy.Current.Select(null, cmdsql, out layersTb);
            if (!dr)
            {
                ShowErr(cmdsql, dr);

            }
            //layersTb.Columns.Add("newKey", typeof(int));
            #endregion
            cmdsql = string.Format
            (
            @"Select *,0 as newKey From Workorder_distribute Where id='{0}'
            ", masterID
            );
            dr = DBProxy.Current.Select(null, cmdsql, out distqtyTb);
            if (!dr)
            {
                ShowErr(cmdsql, dr);
            }

            cmdsql = string.Format
            (
            @"Select *,0 as newKey From Workorder_PatternPanel Where id='{0}'
            ", masterID
            );
            dr = DBProxy.Current.Select(null, cmdsql, out PatternPanelTb);
            if (!dr)
            {
                ShowErr(cmdsql, dr);

            }
            getqtybreakdown(masterID);
            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();

            DataGridViewGeneratorNumericColumnSettings breakqty = new DataGridViewGeneratorNumericColumnSettings();
            breakqty.EditingMouseDoubleClick += (s, e) =>
            {
                gridValid();
                grid.ValidateControl();
                Sci.Production.Cutting.P01_Cutpartchecksummary callNextForm = new Sci.Production.Cutting.P01_Cutpartchecksummary(CurrentMaintain["ID"].ToString());
                callNextForm.ShowDialog(this);
            };

            #region set grid
            Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("Cutref", header: "CutRef#", width: Widths.AnsiChars(6)).Get(out col_cutref)
                .Numeric("Cutno", header: "Cut#", width: Widths.AnsiChars(5), integer_places: 3).Get(out col_cutno)
                .Text("MarkerName", header: "Marker Name", width: Widths.AnsiChars(5)).Get(out col_Markername)
                .Text("Fabriccombo", header: "Fabric Combo", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("LectraCode", header: "LectraCode", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("Article", header: "Article", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Colorid", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("Layer", header: "Layers", width: Widths.AnsiChars(5), integer_places: 5).Get(out col_layer)
                .Text("CutQty", header: "Total CutQty", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("orderid", header: "SP#", width: Widths.AnsiChars(13)).Get(out col_sp)
                .Text("SEQ1", header: "SEQ1", width: Widths.AnsiChars(3)).Get(out col_seq1)
                .Text("SEQ2", header: "SEQ2", width: Widths.AnsiChars(2)).Get(out col_seq2)
                .Date("Fabeta", header: "Fabric Arr Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("estcutdate", header: "Est. Cut Date", width: Widths.AnsiChars(10)).Get(out col_estcutdate)
                .Date("sewinline", header: "Sewing inline", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Cutcellid", header: "Cell", width: Widths.AnsiChars(2)).Get(out col_cutcell)
                .Text("Cutplanid", header: "Cutplan#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Date("actcutdate", header: "Act. Cut Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Edituser", header: "Edit Name", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .DateTime("EditDate", header: "Edit Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Adduser", header: "Add Name", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .DateTime("AddDate", header: "Add Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("UKey", header: "Key", width: Widths.AnsiChars(10), iseditingreadonly: true);
            #endregion

            Helper.Controls.Grid.Generator(this.sizeratio_grid)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(5)).Get(out col_sizeRatio_size)
                .Numeric("Qty", header: "Ratio", width: Widths.AnsiChars(5), integer_places: 6).Get(out col_sizeRatio_qty);

            Helper.Controls.Grid.Generator(this.distribute_grid)
                .Text("orderid", header: "SP#", width: Widths.AnsiChars(13)).Get(out col_dist_sp)
                .Text("article", header: "article", width: Widths.AnsiChars(8)).Get(out col_dist_article)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8)).Get(out col_dist_size)
                .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(5), integer_places: 6).Get(out col_dist_qty);

            Helper.Controls.Grid.Generator(this.qtybreak_grid)
                .Text("id", header: "SP#", width: Widths.AnsiChars(13))
                .Text("article", header: "article", width: Widths.AnsiChars(8))
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(4))
                .Numeric("Qty", header: "Order \nQty", width: Widths.AnsiChars(5), integer_places: 6)
                .Numeric("Balance", header: "Balance", width: Widths.AnsiChars(5), integer_places: 6, settings: breakqty);

            changeeditable();

        }

        #region Grid Cell 物件設定
        private void changeeditable()
        {
            #region maingrid
            #region cutref
            col_cutref.EditingControlShowing += (s, e) =>
            {
                ((Ict.Win.UI.TextBox)e.Control).ReadOnly = true;
            };

            col_cutref.EditingKeyDown += (s, e) =>
            {
                if ((e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back) && MyUtility.Check.Empty(CurrentDetailData["Cutplanid"]))
                {
                    e.EditingControl.Text = "";
                }

            };
            #endregion
            #region cutno
            col_cutno.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["Cutplanid"]) && this.EditMode) ((Ict.Win.UI.NumericBox)e.Control).ReadOnly = false;
                else ((Ict.Win.UI.NumericBox)e.Control).ReadOnly = true;

            };
            col_cutno.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (!MyUtility.Check.Empty(dr["Cutplanid"]) || !this.EditMode)
                {
                    e.CellStyle.BackColor = Color.White;
                    e.CellStyle.ForeColor = Color.Black;
                }
                else
                {
                    e.CellStyle.BackColor = Color.Pink;
                    e.CellStyle.ForeColor = Color.Red;
                }
            };
            #endregion
            #region markname
            col_Markername.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["Cutplanid"]) && this.EditMode) ((Ict.Win.UI.TextBox)e.Control).ReadOnly = false;
                else ((Ict.Win.UI.TextBox)e.Control).ReadOnly = true;

            };
            col_Markername.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (!MyUtility.Check.Empty(dr["Cutplanid"]) || !this.EditMode)
                {
                    e.CellStyle.BackColor = Color.White;
                    e.CellStyle.ForeColor = Color.Black;
                }
                else
                {
                    e.CellStyle.BackColor = Color.Pink;
                    e.CellStyle.ForeColor = Color.Red;
                }
            };
            #endregion
            #region Layer
            col_layer.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["Cutplanid"]) && this.EditMode) ((Ict.Win.UI.NumericBox)e.Control).ReadOnly = false;
                else ((Ict.Win.UI.NumericBox)e.Control).ReadOnly = true;

            };
            col_layer.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (!MyUtility.Check.Empty(dr["Cutplanid"]) || !this.EditMode)
                {
                    e.CellStyle.BackColor = Color.White;
                    e.CellStyle.ForeColor = Color.Black;
                }
                else
                {
                    e.CellStyle.BackColor = Color.Pink;
                    e.CellStyle.ForeColor = Color.Red;
                }
            };
            col_layer.CellValidating += (s, e) =>
            {
                if (!this.EditMode) { return; }
                // 右鍵彈出功能
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                string oldvalue = dr["layer"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (oldvalue == newvalue) return;

                dr["layer"] = newvalue;
                dr.EndEdit();
                int bal = Convert.ToInt16(newvalue) - Convert.ToInt16(oldvalue);


                int sumlayer = 0;
                if (MyUtility.Check.Empty(CurrentDetailData["Order_EachConsUkey"]))
                {
                    DataRow[] AA = ((DataTable)detailgridbs.DataSource).Select(string.Format("MarkerName = '{0}' and Colorid = '{1}'", CurrentDetailData["MarkerName"], CurrentDetailData["Colorid"]));

                    foreach (DataRow l in AA)
                    {
                        sumlayer += MyUtility.Convert.GetInt(l["layer"]);
                    }
                }
                else
                {
                    DataRow[] AA = ((DataTable)detailgridbs.DataSource).Select(string.Format("Order_EachconsUkey = '{0}' and Colorid = '{1}'", CurrentDetailData["Order_EachConsUkey"], CurrentDetailData["Colorid"]));

                    
                    foreach (DataRow l in AA)
                    {
                        sumlayer += MyUtility.Convert.GetInt(l["layer"]);
                    }
                }

                if (MyUtility.Check.Empty(CurrentDetailData["Order_EachConsUkey"]))
                {
                    DataRow[] drar = layersTb.Select(string.Format("MarkerName = '{0}' and Colorid = '{1}'", CurrentDetailData["MarkerName"], CurrentDetailData["Colorid"]));
                    if (drar.Length != 0)
                    {
                        //drar[0]["layer"] = Convert.ToInt16(drar[0]["layer"]) + bal;
                        //BalanceLayer.Value = Convert.ToInt16(drar[0]["layer"]) - Convert.ToInt16(drar[0]["TotalLayerMarker"]);

                        BalanceLayer.Value = sumlayer - Convert.ToInt16(drar[0]["TotalLayerMarker"]);
                    }

                }
                else
                {
                    DataRow[] drar = layersTb.Select(string.Format("Order_EachconsUkey = '{0}' and Colorid = '{1}'", CurrentDetailData["Order_EachConsUkey"], CurrentDetailData["Colorid"]));
                    if (drar.Length != 0)
                    {
                        //drar[0]["layer"] = Convert.ToInt16(drar[0]["layer"]) + bal;
                        //BalanceLayer.Value = Convert.ToInt16(drar[0]["layer"]) - Convert.ToInt16(drar[0]["TotalLayerUkey"]);

                        BalanceLayer.Value = sumlayer - Convert.ToInt16(drar[0]["TotalLayerUkey"]);
                    }

                }

                //1172: CUTTING_P02_Cutting Work Order
                //cal_TotalCutQty(Convert.ToInt32(CurrentDetailData["Ukey"]), Convert.ToInt32(CurrentDetailData["NewKey"]));
                cal_TotalCutQty(CurrentDetailData["Ukey"], CurrentDetailData["NewKey"]);

                totalDisQty();


            };
            #endregion
            #region SP
            col_sp.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["Cutplanid"]) && this.EditMode && CurrentMaintain["WorkType"].ToString() != "1") ((Ict.Win.UI.TextBox)e.Control).ReadOnly = false;
                else ((Ict.Win.UI.TextBox)e.Control).ReadOnly = true;

            };
            col_sp.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (!MyUtility.Check.Empty(dr["Cutplanid"]) || !this.EditMode || CurrentMaintain["WorkType"].ToString() == "1")
                {
                    e.CellStyle.BackColor = Color.White;
                    e.CellStyle.ForeColor = Color.Black;
                }
                else
                {
                    e.CellStyle.BackColor = Color.Pink;
                    e.CellStyle.ForeColor = Color.Red;
                }
            };
            col_sp.EditingMouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {

                    if (CurrentMaintain["WorkType"].ToString() == "1" || !MyUtility.Check.Empty(CurrentDetailData["Cutplanid"])) return;
                    // Parent form 若是非編輯狀態就 return 
                    if (!this.EditMode) { return; }
                    DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                    SelectItem sele;

                    sele = new SelectItem(spTb, "ID", "23", dr["OrderID"].ToString(), false, ",");
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    e.EditingControl.Text = sele.GetSelectedString();
                }
            };
            col_sp.CellValidating += (s, e) =>
            {
                if (!this.EditMode) { return; }
                // 右鍵彈出功能
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                string oldvalue = dr["orderid"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (oldvalue == newvalue) return;

                DataRow[] seledr = spTb.Select(string.Format("ID='{0}'", newvalue));
                if (seledr.Length == 0)
                {
                    MyUtility.Msg.WarningBox(string.Format("<SP> : {0} data not found!", newvalue));
                    dr["orderid"] = "";
                    dr.EndEdit();
                    e.Cancel = true;
                    return;
                }

                dr["orderid"] = newvalue;
                dr.EndEdit();
            };
            #endregion
            #region SEQ1
            col_seq1.EditingMouseDown += (s, e) =>
            {
                if (!MyUtility.Check.Empty(CurrentDetailData["Cutplanid"])) return;
                if (e.Button == MouseButtons.Right)
                {
                    // Parent form 若是非編輯狀態就 return 
                    if (!this.EditMode) { return; }
                    DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                    SelectItem sele;
                    DataTable poTb;
                    string poid = MyUtility.GetValue.Lookup(string.Format("Select poid from orders where id ='{0}'", CurrentMaintain["ID"]));
                    DBProxy.Current.Select(null, string.Format("Select SEQ1,SEQ2,Colorid From PO_Supp_Detail Where id='{0}' and SCIRefno ='{1}'", poid, dr["SCIRefno"]), out poTb);
                    sele = new SelectItem(poTb, "SEQ1,SEQ2,Colorid", "3,2,8", dr["SEQ1"].ToString(), false, ",");
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }

                    dr["SEQ2"] = sele.GetSelecteds()[0]["SEQ2"];
                    dr["Colorid"] = sele.GetSelecteds()[0]["Colorid"];
                    e.EditingControl.Text = sele.GetSelectedString();

                }
            };
            col_seq1.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["Cutplanid"]) && this.EditMode) ((Ict.Win.UI.TextBox)e.Control).ReadOnly = false;
                else ((Ict.Win.UI.TextBox)e.Control).ReadOnly = true;

            };
            col_seq1.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (!MyUtility.Check.Empty(dr["Cutplanid"]) || !this.EditMode)
                {
                    e.CellStyle.BackColor = Color.White;
                    e.CellStyle.ForeColor = Color.Black;
                }
                else
                {
                    e.CellStyle.BackColor = Color.Pink;
                    e.CellStyle.ForeColor = Color.Red;
                }
            };
            col_seq1.CellValidating += (s, e) =>
            {
                if (!this.EditMode) { return; }
                // 右鍵彈出功能
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                string oldvalue = dr["seq1"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (oldvalue == newvalue) return;
                DataRow seledr;
                string poid = MyUtility.GetValue.Lookup(string.Format("Select poid from orders where id ='{0}'", CurrentMaintain["ID"]));
                if (!MyUtility.Check.Seek(string.Format("Select * from po_Supp_Detail where id='{0}' and seq1 ='{1}'", poid, newvalue)))
                {
                    MyUtility.Msg.WarningBox(string.Format("<SEQ1> : {0} data not found!", newvalue));
                    dr["SEQ1"] = "";
                    dr.EndEdit();
                    e.Cancel = true;
                    return;
                }
                else
                {
                    if (!MyUtility.Check.Seek(string.Format("Select * from po_Supp_Detail where id='{0}' and seq1 ='{1}' and seq2 ='{2}'", poid, newvalue, CurrentDetailData["SEQ2"]), out seledr))
                    {
                        MyUtility.Msg.WarningBox(string.Format("<SEQ1>:{0},<SEQ2>:{1} data not found!", newvalue, CurrentDetailData["SEQ2"]));
                        dr["SEQ2"] = "";
                        dr["Colorid"] = "";
                    }
                    else
                    {
                        dr["Colorid"] = seledr["Colorid"];
                    }
                }

                dr["SEQ1"] = newvalue;
                dr.EndEdit();
            };
            #endregion
            #region SEQ2
            col_seq2.EditingMouseDown += (s, e) =>
            {
                if (!MyUtility.Check.Empty(CurrentDetailData["Cutplanid"])) return;
                if (e.Button == MouseButtons.Right)
                {
                    // Parent form 若是非編輯狀態就 return 
                    if (!this.EditMode) { return; }
                    DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                    SelectItem sele;
                    DataTable poTb;
                    string poid = MyUtility.GetValue.Lookup(string.Format("Select poid from orders where id ='{0}'", CurrentMaintain["ID"]));
                    DBProxy.Current.Select(null, string.Format("Select SEQ1,SEQ2,Colorid From PO_Supp_Detail Where id='{0}' and SCIRefno ='{1}'", poid, dr["SCIRefno"]), out poTb);
                    sele = new SelectItem(poTb, "SEQ1,SEQ2,Colorid", "3,2,8", dr["SEQ2"].ToString(), false, ",");
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }

                    dr["SEQ1"] = sele.GetSelecteds()[0]["SEQ1"];
                    dr["Colorid"] = sele.GetSelecteds()[0]["Colorid"];
                    e.EditingControl.Text = sele.GetSelectedString();

                }
            };
            col_seq2.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["Cutplanid"]) && this.EditMode) ((Ict.Win.UI.TextBox)e.Control).ReadOnly = false;
                else ((Ict.Win.UI.TextBox)e.Control).ReadOnly = true;

            };
            col_seq2.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (!MyUtility.Check.Empty(dr["Cutplanid"]) || !this.EditMode)
                {
                    e.CellStyle.BackColor = Color.White;
                    e.CellStyle.ForeColor = Color.Black;
                }
                else
                {
                    e.CellStyle.BackColor = Color.Pink;
                    e.CellStyle.ForeColor = Color.Red;
                }
            };
            col_seq2.CellValidating += (s, e) =>
            {
                if (!this.EditMode) { return; }
                // 右鍵彈出功能
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                string oldvalue = dr["seq2"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (oldvalue == newvalue) return;
                DataRow seledr;
                string poid = MyUtility.GetValue.Lookup(string.Format("Select poid from orders where id ='{0}'", CurrentMaintain["ID"]));
                if (!MyUtility.Check.Seek(string.Format("Select * from po_Supp_Detail where id='{0}' and seq2 ='{1}'", poid, newvalue)))
                {
                    MyUtility.Msg.WarningBox(string.Format("<SEQ2> : {0} data not found!", newvalue));
                    dr["SEQ2"] = "";
                    dr.EndEdit();
                    e.Cancel = true;
                    return;
                }
                else
                {
                    if (!MyUtility.Check.Seek(string.Format("Select * from po_Supp_Detail where id='{0}' and seq1 ='{1}' and seq2 ='{2}'", poid, CurrentDetailData["SEQ1"], newvalue), out seledr))
                    {
                        MyUtility.Msg.WarningBox(string.Format("<SEQ1>:{0},<SEQ2>:{1} data not found!", newvalue, CurrentDetailData["SEQ1"]));
                        dr["SEQ1"] = "";
                        dr["Colorid"] = "";
                    }
                    else
                    {
                        dr["Colorid"] = seledr["Colorid"];
                    }
                }

                dr["SEQ2"] = newvalue;
                dr.EndEdit();
            };
            #endregion
            #region estcutdate
            col_estcutdate.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["Cutplanid"]) && this.EditMode) ((Ict.Win.UI.DateBox)e.Control).ReadOnly = false;
                else ((Ict.Win.UI.DateBox)e.Control).ReadOnly = true;

            };
            col_estcutdate.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (!MyUtility.Check.Empty(dr["Cutplanid"]) || !this.EditMode)
                {
                    e.CellStyle.BackColor = Color.White;
                    e.CellStyle.ForeColor = Color.Black;
                }
                else
                {
                    e.CellStyle.BackColor = Color.Pink;
                    e.CellStyle.ForeColor = Color.Red;
                }
            };
            #endregion
            #region cutcell
            col_cutcell.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(dr["Cutplanid"]) && this.EditMode) ((Ict.Win.UI.TextBox)e.Control).ReadOnly = false;
                else ((Ict.Win.UI.TextBox)e.Control).ReadOnly = true;

            };
            col_cutcell.CellFormatting += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (!MyUtility.Check.Empty(dr["Cutplanid"]) || !this.EditMode)
                {
                    e.CellStyle.BackColor = Color.White;
                    e.CellStyle.ForeColor = Color.Black;
                }
                else
                {
                    e.CellStyle.BackColor = Color.Pink;
                    e.CellStyle.ForeColor = Color.Red;
                }
            };
            col_cutcell.EditingMouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    // Parent form 若是非編輯狀態就 return 
                    if (!this.EditMode) { return; }
                    DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                    SelectItem sele;
                    DataTable cellTb;
                    DBProxy.Current.Select(null, string.Format("Select id from Cutcell where mDivisionid = '{0}' and junk=0", keyWord), out cellTb);
                    sele = new SelectItem(cellTb, "ID", "10", dr["CutCellid"].ToString(), false, ",");
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    e.EditingControl.Text = sele.GetSelectedString();
                }
            };
            col_cutcell.CellValidating += (s, e) =>
            {
                if (!this.EditMode) { return; }
                // 右鍵彈出功能
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                string oldvalue = dr["cutcellid"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (oldvalue == newvalue) return;

                DataTable cellTb;
                DBProxy.Current.Select(null, string.Format("Select id from Cutcell where mDivisionid = '{0}' and junk=0", keyWord), out cellTb);

                DataRow[] seledr = cellTb.Select(string.Format("ID='{0}'", newvalue));
                if (seledr.Length == 0)
                {
                    MyUtility.Msg.WarningBox(string.Format("<Cell> : {0} data not found!", newvalue));
                    dr["cutCellid"] = "";
                    dr.EndEdit();
                    e.Cancel = true;
                    return;
                }

                dr["cutCellid"] = newvalue;
                dr.EndEdit();
            };
            #endregion
            #endregion

            #region SizeRatio
            col_sizeRatio_size.EditingMouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    // Parent form 若是非編輯狀態就 return 
                    if (!this.EditMode || CurrentDetailData["Cutplanid"].ToString() != "") { return; }
                    DataRow dr = sizeratio_grid.GetDataRow(e.RowIndex);
                    SelectItem sele;

                    sele = new SelectItem(sizeGroup, "SizeCode", "23", dr["SizeCode"].ToString(), false, ",");
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    e.EditingControl.Text = sele.GetSelectedString();
                }
            };
            col_sizeRatio_size.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = sizeratio_grid.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(CurrentDetailData["Cutplanid"]) && this.EditMode) ((Ict.Win.UI.TextBox)e.Control).ReadOnly = false;
                else ((Ict.Win.UI.TextBox)e.Control).ReadOnly = true;

            };
            col_sizeRatio_size.CellValidating += (s, e) =>
            {
                if (!this.EditMode) { return; }
                // 右鍵彈出功能
                if (e.RowIndex == -1) return;
                DataRow dr = sizeratio_grid.GetDataRow(e.RowIndex);
                string oldvalue = dr["SizeCode"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (oldvalue == newvalue) return;
                dr["SizeCode"] = newvalue;
                dr.EndEdit();

                //cal_TotalCutQty(Convert.ToInt32(CurrentDetailData["Ukey"]), Convert.ToInt32(CurrentDetailData["NewKey"]));
                cal_TotalCutQty(CurrentDetailData["Ukey"], CurrentDetailData["NewKey"]);
                totalDisQty();
                updateExcess(Convert.ToInt32(CurrentDetailData["Ukey"]), Convert.ToInt32(CurrentDetailData["NewKey"]), newvalue);
            };
            col_sizeRatio_qty.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = sizeratio_grid.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(CurrentDetailData["Cutplanid"]) && this.EditMode) ((Ict.Win.UI.NumericBox)e.Control).ReadOnly = false;
                else ((Ict.Win.UI.NumericBox)e.Control).ReadOnly = true;

            };
            col_sizeRatio_qty.CellValidating += (s, e) =>
            {
                // Parent form 若是非編輯狀態就 return 
                if (!this.EditMode) { return; }
                // 右鍵彈出功能
                if (e.RowIndex == -1) return;
                DataRow dr = sizeratio_grid.GetDataRow(e.RowIndex);
                int oldvalue = Convert.ToInt16(dr["Qty"]);
                int newvalue = Convert.ToInt16(e.FormattedValue);
                if (oldvalue == newvalue) return;
                dr["Qty"] = newvalue;
                dr.EndEdit();
                cal_Cons(true, false);
                //cal_TotalCutQty(Convert.ToInt32(CurrentDetailData["Ukey"]), Convert.ToInt32(CurrentDetailData["NewKey"]));
                cal_TotalCutQty(CurrentDetailData["Ukey"], CurrentDetailData["NewKey"]);

                updateExcess(Convert.ToInt32(CurrentDetailData["Ukey"]), Convert.ToInt32(CurrentDetailData["NewKey"]), dr["SizeCode"].ToString());
                totalDisQty();
            };
            #endregion

            #region Distribute
            col_dist_sp.EditingMouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    // Parent form 若是非編輯狀態就 return 
                    if (!this.EditMode) { return; }
                    if (CurrentDetailData == null) return;
                    DataRow dr = distribute_grid.GetDataRow(e.RowIndex);
                    SelectItem sele;
                    if (dr["OrderID"].ToString() == "EXCESS" || CurrentDetailData["Cutplanid"].ToString() != "") return;
                    sele = new SelectItem(spTb, "ID", "23", dr["OrderID"].ToString(), false, ",");
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    e.EditingControl.Text = sele.GetSelectedString();
                }
            };
            col_dist_sp.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                if (CurrentDetailData == null) return;
                DataRow dr = distribute_grid.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(CurrentDetailData["Cutplanid"]) && this.EditMode && dr["OrderID"].ToString() != "EXCESS") ((Ict.Win.UI.TextBox)e.Control).ReadOnly = false;
                else ((Ict.Win.UI.TextBox)e.Control).ReadOnly = true;

            };
            col_dist_sp.CellValidating += (s, e) =>
            {
                if (!this.EditMode) { return; }
                // 右鍵彈出功能
                if (e.RowIndex == -1) return;
                DataRow dr = distribute_grid.GetDataRow(e.RowIndex);
                string oldvalue = dr["orderid"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (oldvalue == newvalue || newvalue == "EXCESS") return;

                DataRow[] seledr = spTb.Select(string.Format("ID='{0}'", newvalue));
                if (seledr.Length == 0)
                {
                    MyUtility.Msg.WarningBox(string.Format("<SP> : {0} data not found!", newvalue));
                    dr["orderid"] = "";
                    dr.EndEdit();
                    e.Cancel = true;
                    return;
                }
                if (!MyUtility.Check.Empty(dr["SizeCode"]) && !MyUtility.Check.Empty(dr["Article"]))
                {
                    seledr = qtybreakTb.Select(string.Format("id = '{0}' and SizeCode = '{1}' and Article ='{2}'", newvalue, dr["SizeCode"], dr["Article"]));
                    if (seledr.Length == 0)
                    {
                        MyUtility.Msg.WarningBox(string.Format("<SP#>:{0},<Article>:{1},<SizeCode>:{2}", dr["OrderID"], newvalue, dr["Article"]));
                        dr["OrderID"] = "";
                        dr.EndEdit();
                        e.Cancel = true;
                        return;
                    }
                }
                dr["orderid"] = newvalue;
                dr.EndEdit();
                totalDisQty();

            };

            col_dist_size.EditingMouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    // Parent form 若是非編輯狀態就 return 
                    if (!this.EditMode) { return; }
                    if (CurrentDetailData == null) return;
                    DataRow dr = distribute_grid.GetDataRow(e.RowIndex);
                    SelectItem sele;
                    if (dr["OrderID"].ToString() == "EXCESS" || CurrentDetailData["Cutplanid"].ToString() != "") return;
                    sele = new SelectItem(sizeGroup, "SizeCode", "23", dr["SizeCode"].ToString(), false, ",");
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    e.EditingControl.Text = sele.GetSelectedString();
                }
            };
            col_dist_size.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                if (CurrentDetailData == null) return;
                DataRow dr = distribute_grid.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(CurrentDetailData["Cutplanid"]) && this.EditMode && dr["OrderID"].ToString() != "EXCESS") ((Ict.Win.UI.TextBox)e.Control).ReadOnly = false;
                else ((Ict.Win.UI.TextBox)e.Control).ReadOnly = true;

            };
            col_dist_size.CellValidating += (s, e) =>
            {
                if (!this.EditMode) { return; }
                // 右鍵彈出功能
                if (e.RowIndex == -1) return;
                DataRow dr = distribute_grid.GetDataRow(e.RowIndex);
                string oldvalue = dr["SizeCode"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (oldvalue == newvalue) return;
                DataRow[] seledr = sizeGroup.Select(string.Format("SizeCode='{0}'", newvalue));
                if (seledr.Length == 0)
                {
                    MyUtility.Msg.WarningBox(string.Format("<Size> : {0} data not found!", newvalue));
                    dr["SizeCode"] = "";
                    dr.EndEdit();
                    e.Cancel = true;
                    return;
                }
                if (!MyUtility.Check.Empty(dr["OrderID"]) && !MyUtility.Check.Empty(dr["Article"]))
                {
                    seledr = qtybreakTb.Select(string.Format("id = '{0}' and SizeCode = '{1}' and Article ='{2}'", dr["OrderID"], newvalue, dr["Article"]));
                    if (seledr.Length == 0)
                    {
                        MyUtility.Msg.WarningBox(string.Format("<SP#>:{0},<Article>:{1},<SizeCode>:{2}", dr["OrderID"], newvalue, dr["Article"]));
                        dr["SizeCode"] = "";
                        dr.EndEdit();
                        e.Cancel = true;
                        return;
                    }
                }
                dr["SizeCode"] = newvalue;
                dr.EndEdit();
                totalDisQty();
                updateExcess(Convert.ToInt32(CurrentDetailData["Ukey"]), Convert.ToInt32(CurrentDetailData["NewKey"]), dr["SizeCode"].ToString());

            };
            col_dist_article.EditingMouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    // Parent form 若是非編輯狀態就 return 
                    if (!this.EditMode) { return; }
                    if (CurrentDetailData == null) return;
                    DataRow dr = distribute_grid.GetDataRow(e.RowIndex);
                    SelectItem sele;
                    if (dr["OrderID"].ToString() == "EXCESS" || CurrentDetailData["Cutplanid"].ToString() != "") return;
                    sele = new SelectItem(artTb, "article", "23", dr["Article"].ToString(), false, ",");
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    e.EditingControl.Text = sele.GetSelectedString();
                }
            };
            col_dist_article.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                if (CurrentDetailData == null) return;
                DataRow dr = distribute_grid.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(CurrentDetailData["Cutplanid"]) && this.EditMode && dr["OrderID"].ToString() != "EXCESS") ((Ict.Win.UI.TextBox)e.Control).ReadOnly = false;
                else ((Ict.Win.UI.TextBox)e.Control).ReadOnly = true;

            };
            col_dist_article.CellValidating += (s, e) =>
            {
                if (!this.EditMode) { return; }
                // 右鍵彈出功能

                if (e.RowIndex == -1) return;
                DataRow dr = distribute_grid.GetDataRow(e.RowIndex);
                string oldvalue = dr["Article"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (oldvalue == newvalue) return;
                DataRow[] seledr = artTb.Select(string.Format("Article='{0}'", newvalue));
                if (seledr.Length == 0)
                {
                    MyUtility.Msg.WarningBox(string.Format("<Article> : {0} data not found!", newvalue));
                    dr["Article"] = "";
                    dr.EndEdit();
                    e.Cancel = true;
                    return;
                }
                if (!MyUtility.Check.Empty(dr["OrderID"]) && !MyUtility.Check.Empty(dr["SizeCode"]))
                {
                    seledr = qtybreakTb.Select(string.Format("id = '{0}' and SizeCode = '{1}' and Article ='{2}'", dr["OrderID"], dr["SizeCode"], newvalue));
                    if (seledr.Length == 0)
                    {
                        MyUtility.Msg.WarningBox(string.Format("<SP#>:{0},<Article>:{1},<SizeCode>:{2}", dr["OrderID"], newvalue, dr["SizeCode"]));
                        dr["Article"] = "";
                        dr.EndEdit();
                        e.Cancel = true;
                        return;
                    }
                }
                dr["Article"] = newvalue;
                dr.EndEdit();
            };
            col_dist_qty.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                if (CurrentDetailData == null) return;
                DataRow dr = distribute_grid.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(CurrentDetailData["Cutplanid"]) && this.EditMode && dr["OrderID"].ToString() != "EXCESS") ((Ict.Win.UI.NumericBox)e.Control).ReadOnly = false;
                else ((Ict.Win.UI.NumericBox)e.Control).ReadOnly = true;

            };
            col_dist_qty.CellValidating += (s, e) =>
            {
                if (!this.EditMode) { return; }
                // 右鍵彈出功能
                if (e.RowIndex == -1) return;
                DataRow dr = distribute_grid.GetDataRow(e.RowIndex);
                string oldvalue = dr["Qty"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (oldvalue == newvalue) return;

                dr["Qty"] = newvalue;
                dr.EndEdit();
                totalDisQty();
                updateExcess(Convert.ToInt32(CurrentDetailData["Ukey"]), Convert.ToInt32(CurrentDetailData["NewKey"]), dr["SizeCode"].ToString());

            };
            #endregion

        }
        #endregion

        protected override void OnEditModeChanged()
        {

            base.OnEditModeChanged();
            if (sizeratioMenuStrip != null) sizeratioMenuStrip.Enabled = this.EditMode;
            if (distributeMenuStrip != null) distributeMenuStrip.Enabled = this.EditMode;
        }

        private void gridValid()
        {
            sizeratio_grid.ValidateControl();
            distribute_grid.ValidateControl();
            grid.ValidateControl();
        }

        //1394: CUTTING_P02_Cutting Work Order。KEEP當前的資料。
        protected override void DoPrint()
        {
            drTEMP = this.CurrentDetailData;
            base.DoPrint();
        }

        protected override void OnDetailGridRowChanged()
        {
            gridValid();
            base.OnDetailGridRowChanged();
            //Binding 資料來源
            if (CurrentDetailData == null) return;
            bindingSource2.SetRow(this.CurrentDetailData);
            DataRow fabdr;

            if (MyUtility.Check.Seek(string.Format("Select * from Fabric Where SCIRefno ='{0}'", CurrentDetailData["SCIRefno"]), out fabdr))
            {
                displayBox_FabricType.Text = fabdr["MtlTypeid"].ToString();
                editBox_desc.Text = fabdr["Description"].ToString();
            }
            else
            {
                displayBox_FabricType.Text = "";
                editBox_desc.Text = "";
            }


            #region 根據左邊Grid Filter 右邊資訊
            if (!MyUtility.Check.Empty(CurrentDetailData["Ukey"]))
            {
                sizeratioTb.DefaultView.RowFilter = string.Format("Workorderukey = '{0}'", CurrentDetailData["Ukey"]);
                distqtyTb.DefaultView.RowFilter = string.Format("Workorderukey = '{0}'", CurrentDetailData["Ukey"]);
            }
            #endregion

            #region Total Dist
            totalDisQty();
            #endregion

            int sumlayer = 0;
            if (MyUtility.Check.Empty(CurrentDetailData["Order_EachConsUkey"]))
            {
                DataRow[] AA = ((DataTable)detailgridbs.DataSource).Select(string.Format("MarkerName = '{0}' and Colorid = '{1}'", CurrentDetailData["MarkerName"], CurrentDetailData["Colorid"]));

                foreach (DataRow l in AA)
                {
                    sumlayer += MyUtility.Convert.GetInt(l["layer"]);
                }
            }
            else
            {
                DataRow[] AA = ((DataTable)detailgridbs.DataSource).Select(string.Format("Order_EachconsUkey = '{0}' and Colorid = '{1}'", CurrentDetailData["Order_EachConsUkey"], CurrentDetailData["Colorid"]));


                foreach (DataRow l in AA)
                {
                    sumlayer += MyUtility.Convert.GetInt(l["layer"]);
                }
            }

            int order_EachConsTemp;
            if (CurrentDetailData["Order_EachConsUkey"] == DBNull.Value)
            {//old rule
                order_EachConsTemp = 0;
                string selectcondition = string.Format("MarkerName = '{0}' and Colorid = '{1}'", CurrentDetailData["MarkerName"], CurrentDetailData["Colorid"]);
                DataRow[] laydr = layersTb.Select(selectcondition);
                if (laydr.Length == 0)
                {
                    TotalLayer.Value = 0;
                    BalanceLayer.Value = 0;
                }
                else
                {
                    TotalLayer.Value = (decimal)laydr[0]["TotalLayerMarker"];
                    //BalanceLayer.Value = (decimal)laydr[0]["layer"] - (decimal)laydr[0]["TotalLayerMarker"];
                    BalanceLayer.Value = sumlayer - (decimal)laydr[0]["TotalLayerMarker"];

                }
            }
            else
            { //New rule
                order_EachConsTemp = Convert.ToInt32(CurrentDetailData["Order_EachConsUkey"]);
                //string selectcondition = string.Format("Order_EachConsUkey = {0}", order_EachConsTemp);  0000617: CUTTING_P02_Cutting Work Order，(6) Total layer & Balance layer資料不正確
                string selectcondition = string.Format("Order_EachConsUkey = {0} and  Colorid = '{1}'", order_EachConsTemp, CurrentDetailData["Colorid"]);
                DataRow[] laydr = layersTb.Select(selectcondition);
                if (laydr.Length == 0)
                {
                    TotalLayer.Value = 0;
                    BalanceLayer.Value = 0;
                }
                else
                {
                    TotalLayer.Value = (decimal)laydr[0]["TotalLayerUkey"];
                    //BalanceLayer.Value = (decimal)laydr[0]["layer"] - (decimal)laydr[0]["TotalLayerUkey"];

                    BalanceLayer.Value = sumlayer - (decimal)laydr[0]["TotalLayerUkey"];
                }
            }



            #region 判斷download id
            string downloadid = MyUtility.GetValue.Lookup("MarkerDownLoadid", CurrentDetailData["Order_EachConsUkey"].ToString(), "Order_EachCons", "Ukey");
            displayBox_Downloadid.Text = downloadid;
            if (downloadid != CurrentDetailData["MarkerDownLoadid"].ToString()) downloadid_Text.Visible = true;
            #endregion

            #region 顯示Marker Length
            //numericBox_MarkerLengthY = CurrentDetailData 
            #endregion

            #region 判斷可否開放修改
            if (MyUtility.Check.Empty(CurrentDetailData["Cutplanid"]) && this.EditMode)
            {
                numericBox_MarkerLengthY.ReadOnly = false;
                textBox_MarkerLengthE.ReadOnly = false;
                numericBox_UnitCons.ReadOnly = false;
                txtCell1.ReadOnly = false;
                numericBox_Cons.ReadOnly = false;
                textBox_FabricCombo.ReadOnly = false;
                textBox_LectraCode.ReadOnly = false;
                sizeratioMenuStrip.Enabled = true;
                distributeMenuStrip.Enabled = true;
            }
            else
            {
                numericBox_MarkerLengthY.ReadOnly = true;
                textBox_MarkerLengthE.ReadOnly = true;
                numericBox_UnitCons.ReadOnly = true;
                txtCell1.ReadOnly = true;
                numericBox_Cons.ReadOnly = true;
                textBox_FabricCombo.ReadOnly = true;
                textBox_LectraCode.ReadOnly = true;
                sizeratioMenuStrip.Enabled = false;
                distributeMenuStrip.Enabled = false;
            }
            #endregion



        }

        //程式產生的BindingSource 必須自行Dispose, 以節省資源
        protected override void OnFormDispose()
        {
            base.OnFormDispose();
            bindingSource2.Dispose();
        }

        private void getqtybreakdown(string masterID)
        {

            DataTable fabcodetb;

            #region 找出有哪些部位
            string fabcodesql = string.Format(@"Select distinct a.LectraCode
            from Order_ColorCombo a ,Order_EachCons b 
            where a.id = '{0}' and a.FabricCode is not null and a.FabricCode !='' 
            and b.id = '{0}' and a.id = b.id 
            --and b.cuttingpiece='0' and  b.FabricCombo = a.LectraCode
            order by LectraCode", masterID);
            DualResult fabresult = DBProxy.Current.Select("Production", fabcodesql, out fabcodetb);
            #endregion

            #region 建立Grid
            string settbsql = "Select a.id,article,sizecode,a.qty,0 as balance"; //寫SQL建立Table
            foreach (DataRow dr in fabcodetb.Rows) //組動態欄位
            {
                settbsql = settbsql + ", 0 as " + dr["LectraCode"];
            }
            settbsql = settbsql + string.Format(" From Order_Qty a,orders b Where b.cuttingsp ='{0}' and a.id = b.id order by id,article,sizecode", masterID);

            DualResult gridResult = DBProxy.Current.Select(null, settbsql, out qtybreakTb);
            MyUtility.Tool.ProcessWithDatatable(qtybreakTb, "sizecode", "Select distinct SizeCode from #tmp", out sizeGroup);
            MyUtility.Tool.ProcessWithDatatable(qtybreakTb, "article", "Select distinct Article from #tmp", out artTb);
            MyUtility.Tool.ProcessWithDatatable(qtybreakTb, "id", "Select distinct id from #tmp", out spTb);
            #endregion

            #region 寫入部位數量
            string getqtysql = string.Format(
            @"Select b.article,b.sizecode,b.qty,c.LectraCode,b.orderid 
            From Workorder a, workorder_Distribute b, workorder_PatternPanel c 
            Where a.id = '{0}' and a.ukey = b.workorderukey and a.ukey = c.workorderukey 
            and b.workorderukey = c.workorderukey and b.article !=''", masterID);
            DataTable getqtytb;

            gridResult = DBProxy.Current.Select(null, getqtysql, out getqtytb);
            foreach (DataRow dr in getqtytb.Rows)
            {
                DataRow[] gridselect = qtybreakTb.Select(string.Format("id = '{0}' and article = '{1}' and sizecode = '{2}'", dr["orderid"], dr["article"], dr["sizecode"], dr["LectraCode"], dr["Qty"]));
                if (gridselect.Length != 0)
                {
                    //20161007 leo 微調唯一值為LectraCode
                    //gridselect[0][dr["PatternPanel"].ToString()] = MyUtility.Convert.GetDecimal((gridselect[0][dr["PatternPanel"].ToString()])) + MyUtility.Convert.GetDecimal(dr["Qty"]);
                    gridselect[0][dr["LectraCode"].ToString()] = MyUtility.Convert.GetDecimal((gridselect[0][dr["LectraCode"].ToString()])) + MyUtility.Convert.GetDecimal(dr["Qty"]);
                }
            }
            #endregion

            #region 判斷是否Complete
            DataTable panneltb;
            fabcodesql = string.Format(@"Select distinct a.Article,a.LectraCode
            from Order_ColorCombo a ,Order_EachCons b
            where a.id = '{0}' and a.FabricCode is not null 
            and a.id = b.id and b.cuttingpiece='0' and  b.FabricCombo = a.LectraCode
            and a.FabricCode !='' order by Article,LectraCode", masterID);
            gridResult = DBProxy.Current.Select(null, fabcodesql, out panneltb);
            decimal minqty = 0;
            foreach (DataRow dr in qtybreakTb.Rows)
            {
                minqty = 0;
                DataRow[] sel = panneltb.Select(string.Format("Article = '{0}'", dr["Article"]));
                foreach (DataRow pdr in sel)
                {
                    if (minqty > MyUtility.Convert.GetDecimal(dr[pdr["LectraCode"].ToString()])) minqty = MyUtility.Convert.GetDecimal(dr[pdr["LectraCode"].ToString()]);

                }
                dr["balance"] = minqty;
            }
            #endregion

        }

        private void sorting(string sort)
        {
            grid.ValidateControl();
            if (CurrentDetailData == null) return;
            DataView dv = ((DataTable)detailgridbs.DataSource).DefaultView;
            switch (sort)
            {
                case "FabricCombo":
                    dv.Sort = "SORT_NUM,FabricCombo,multisize,Article,SizeCode,Ukey";
                    break;
                case "SP":
                    dv.Sort = "SORT_NUM,Orderid,FabricCombo,Ukey";
                    break;
                case "Cut#":
                    dv.Sort = "SORT_NUM,cutno,FabricCombo,Ukey";
                    break;
                case "Ref#":
                    dv.Sort = "SORT_NUM,cutref,Ukey";
                    break;
                case "Cutplan#":
                    dv.Sort = "SORT_NUM,Cutplanid,Ukey";
                    break;
                case "MarkerName":
                    dv.Sort = "SORT_NUM,FabricCombo,Cutno,Markername,estcutdate,Ukey";
                    break;
                default:
                    dv.Sort = "SORT_NUM ASC,FabricCombo ASC,multisize DESC,Colorid ASC,Order_SizeCode_Seq DESC,MarkerName ASC,Ukey";
                    break;
            }

        }

        private void comboBox1_Validated(object sender, EventArgs e)
        {
            gridValid();
            grid.ValidateControl();
            sorting(comboBox1.Text);
        }

        private void AutoRef_Click(object sender, EventArgs e)
        {
            gridValid();
            grid.ValidateControl();
            #region 變更先將同d,Cutref, LectraCode, CutNo, MarkerName, estcutdate 且有cutref,Cuno無cutplanid 的cutref值找出來Group by→cutref 會相同
            string cmdsql = string.Format(@"            
            SELECT isnull(Cutref,'') as cutref, isnull(FabricCombo,'') as FabricCombo, isnull(CutNo,0) as cutno, 
            isnull(MarkerName,'') as MarkerName, estcutdate 
            FROM Workorder 
            WHERE (cutplanid is null or cutplanid ='') AND (CutNo is not null ) 
                    AND (cutref is not null and cutref !='') and id = '{0}' and mDivisionid = '{1}'		
            GROUP BY Cutref, FabricCombo, CutNo, MarkerName, estcutdate", CurrentMaintain["ID"], keyWord);
            DataTable cutreftb;
            DualResult cutrefresult = DBProxy.Current.Select(null, cmdsql, out cutreftb);
            if (!cutrefresult)
            {
                ShowErr(cmdsql, cutrefresult);
                return;
            }
            #endregion
            DataTable workordertmp;
            cmdsql = string.Format(
                @"Select * 
                  From workorder 
                  Where (CutNo is not null ) and (cutref is null or cutref ='') 
                    and (estcutdate is not null and estcutdate !='' )
                    and id = '{0}' and mDivisionid = '{1}'", CurrentMaintain["ID"], keyWord);//找出空的cutref
            cutrefresult = DBProxy.Current.Select(null, cmdsql, out workordertmp);
            if (!cutrefresult)
            {
                ShowErr(cmdsql, cutrefresult);
                return;
            }
            DataTable workorderdt = ((DataTable)detailgridbs.DataSource);
            string maxref = MyUtility.GetValue.Lookup(string.Format("Select isnull(Max(cutref),'000000') from Workorder where mDivisionid = '{0}'", keyWord)); //找最大Cutref
            string updatecutref = "", newcutref = "";
            foreach (DataRow dr in workordertmp.Rows) //寫入空的Cutref
            {
                DataRow[] findrow = cutreftb.Select(string.Format(@"MarkerName = '{0}' and FabricCombo = '{1}' and Cutno = {2} and estcutdate = '{3}' ", dr["MarkerName"], dr["FabricCombo"], dr["Cutno"], dr["estcutdate"]));
                if (findrow.Length != 0) //若有找到同馬克同部位同Cutno同裁剪日就寫入同cutref
                {
                    newcutref = findrow[0]["cutref"].ToString();
                }
                else
                {
                    maxref = MyUtility.GetValue.GetNextValue(maxref, 0);
                    DataRow newdr = cutreftb.NewRow();
                    newdr["MarkerName"] = dr["MarkerName"] == null ? string.Empty : dr["MarkerName"];
                    newdr["FabricCombo"] = dr["FabricCombo"] == null ? string.Empty : dr["FabricCombo"];
                    newdr["Cutno"] = dr["Cutno"] == null ? 0 : dr["Cutno"];
                    newdr["estcutdate"] = dr["estcutdate"] == null ? string.Empty : dr["estcutdate"];
                    newdr["cutref"] = maxref;
                    cutreftb.Rows.Add(newdr);
                    newcutref = maxref;
                }
                updatecutref = updatecutref + string.Format("Update Workorder set cutref = '{0}' where ukey = '{1}';", newcutref, dr["ukey"]);
            }
            #region update Inqty,Status
            DualResult upResult;
            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    if (!MyUtility.Check.Empty(updatecutref))
                    {
                        if (!(upResult = DBProxy.Current.Execute(null, updatecutref)))
                        {
                            _transactionscope.Dispose();
                            ShowErr(updatecutref, upResult);
                            return;
                        }
                        _transactionscope.Complete();
                    }
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

            #endregion
            this.RenewData();
            sorting(comboBox1.Text);  //避免順序亂掉
            this.OnDetailEntered();

        }

        private void AutoCut_Click(object sender, EventArgs e)
        {
            gridValid();
            grid.ValidateControl();
            #region 找出各部位最大Cutno
            int maxcutno;
            foreach (DataRow dr in DetailDatas)
            {
                if (MyUtility.Check.Empty(dr["cutno"]))
                {
                    DataTable wk = (DataTable)detailgridbs.DataSource;
                    string temp = wk.Compute("Max(cutno)", string.Format("FabricCombo ='{0}'", dr["FabricCombo"])).ToString();
                    if (MyUtility.Check.Empty(temp))
                    {
                        maxcutno = 1;
                    }
                    else
                    {
                        int maxno = Convert.ToInt16(wk.Compute("Max(cutno)", string.Format("FabricCombo ='{0}'", dr["FabricCombo"])));
                        maxcutno = maxno + 1;
                    }

                    dr["cutno"] = maxcutno;
                }
            }
            #endregion
        }

        private void Packing_Click(object sender, EventArgs e)
        {
            gridValid();
            grid.ValidateControl();
            var dr = this.CurrentMaintain; if (null == dr) return;
            var frm = new Sci.Production.Cutting.P02_PackingMethod(false, CurrentMaintain["id"].ToString(), null, null);
            frm.ShowDialog(this);
            this.RenewData();
            sorting(comboBox1.Text);  //避免順序亂掉
            this.OnDetailEntered();
        }

        private void Batchassign_Click(object sender, EventArgs e)
        {
            gridValid();
            grid.ValidateControl();
            var frm = new Sci.Production.Cutting.P02_BatchAssignCellCutDate((DataTable)(detailgridbs.DataSource));
            frm.ShowDialog(this);
        }

        bool flag = false;
        protected override void OnDetailGridAppendClick()
        {
            flag = true;
            base.OnDetailGridAppendClick();
        }

        protected override void OnDetailGridInsert(int index = -1)
        {
            if (flag || ((DataTable)this.detailgridbs.DataSource).Rows.Count <= 0)
            {
                base.OnDetailGridInsert(index);
                flag = false;
                return;
            }

            DataTable table = (DataTable)this.detailgridbs.DataSource;

            DataRow newRow = table.NewRow();
            DataRow OldRow = CurrentDetailData == null ? newRow : CurrentDetailData;  //將游標停駐處的該筆資料複製起來
            //base.OnDetailGridInsert(index); //先給一個NewKey

            int maxkey;
            object comput = ((DataTable)detailgridbs.DataSource).Compute("Max(newkey)", "");
            if (comput == DBNull.Value) maxkey = 0;
            else maxkey = Convert.ToInt32(comput);
            maxkey = maxkey + 1;

            // 除Cutref, Cutno, Addname, AddDate, EditName, EditDate以外的所有欄位
            newRow["ID"] = OldRow["ID"];
            newRow["FactoryID"] = OldRow["FactoryID"];
            newRow["MDivisionId"] = OldRow["MDivisionId"];
            newRow["SEQ1"] = OldRow["SEQ1"];
            newRow["SEQ2"] = OldRow["SEQ2"];
            //CutRef
            newRow["OrderID"] = OldRow["OrderID"];
            newRow["Cutplanid"] = OldRow["Cutplanid"];
            //Cutno
            newRow["Layer"] = OldRow["Layer"];
            newRow["Colorid"] = OldRow["Colorid"];
            newRow["Markername"] = OldRow["Markername"];
            newRow["EstCutDate"] = OldRow["EstCutDate"];
            newRow["Cutcellid"] = OldRow["Cutcellid"];
            newRow["MarkerLength"] = OldRow["MarkerLength"];
            newRow["ConsPC"] = OldRow["ConsPC"];
            newRow["Cons"] = OldRow["Cons"];
            newRow["Refno"] = OldRow["Refno"];
            newRow["SCIRefno"] = OldRow["SCIRefno"];
            newRow["MarkerNo"] = OldRow["MarkerNo"];
            newRow["MarkerVersion"] = OldRow["MarkerVersion"];
            newRow["UKey"] = 0;
            newRow["Type"] = OldRow["Type"];
            //newRow["Addname"] = Sci.Env.User.UserName;
            //newRow["AddDate"] = DateTime.Now;
            //EditName
            //EditDate
            newRow["FabricCombo"] = OldRow["FabricCombo"];
            newRow["MarkerDownLoadID"] = OldRow["MarkerDownLoadID"];
            newRow["FabricCode"] = OldRow["FabricCode"];
            newRow["LectraCode"] = OldRow["LectraCode"];
            newRow["Order_EachconsUKey"] = OldRow["Order_EachconsUKey"];
            newRow["Article"] = OldRow["Article"];
            newRow["SizeCode"] = OldRow["SizeCode"];
            newRow["CutQty"] = OldRow["CutQty"];
            newRow["LectraCode1"] = OldRow["LectraCode1"];
            newRow["PatternPanel"] = OldRow["PatternPanel"];
            newRow["Fabeta"] = OldRow["Fabeta"];
            newRow["sewinline"] = OldRow["sewinline"];
            newRow["actcutdate"] = OldRow["actcutdate"];
            newRow["Adduser"] = loginID;
            newRow["edituser"] = OldRow["edituser"];
            newRow["totallayer"] = OldRow["totallayer"];
            newRow["multisize"] = OldRow["multisize"];
            newRow["Order_SizeCode_Seq"] = OldRow["Order_SizeCode_Seq"];
            newRow["SORT_NUM"] = OldRow["SORT_NUM"];
            newRow["MtlTypeID"] = OldRow["MtlTypeID"];
            newRow["DescDetail"] = OldRow["DescDetail"];
            newRow["Newkey"] = maxkey;
            newRow["MarkerLengthY"] = OldRow["MarkerLengthY"];
            newRow["MarkerLengthE"] = OldRow["MarkerLengthE"];

            DataTable detailtmp = (DataTable)detailgridbs.DataSource;
            int TEMP = ((DataTable)detailgridbs.DataSource).Rows.Count;

            if (index == -1) index = TEMP;
            OldRow.Table.Rows.InsertAt(newRow, index);

            //1172: CUTTING_P02_Cutting Work Order
            DataRow[] drTEMPS = sizeratioTb.Select(string.Format("WorkOrderUkey='{0}'", OldRow["ukey"].ToString()));
            foreach (DataRow drTEMP in drTEMPS)
            {
                DataRow drNEW = sizeratioTb.NewRow();
                drNEW["WorkOrderUkey"] = 0;  //新增WorkOrderUkey塞0
                drNEW["ID"] = drTEMP["ID"];
                drNEW["SizeCode"] = drTEMP["SizeCode"];
                drNEW["Qty"] = drTEMP["Qty"];
                drNEW["newkey"] = maxkey;
                sizeratioTb.Rows.Add(drNEW);
            }
        }

        protected override void OnDetailGridDelete()
        {
            string ukey = CurrentDetailData["Ukey"].ToString();
            int NewKey = Convert.ToInt16(CurrentDetailData["NewKey"]);
            DataRow[] drar = sizeratioTb.Select(string.Format("WorkOrderUkey = {0} and NewKey = {1}", ukey, NewKey));
            foreach (DataRow dr in drar)
            {
                dr.Delete();
            }
            drar = distqtyTb.Select(string.Format("WorkOrderUkey = {0} and NewKey = {1}", ukey, NewKey));
            foreach (DataRow dr in drar)
            {
                dr.Delete();
            }
            drar = PatternPanelTb.Select(string.Format("WorkOrderUkey = {0} and NewKey = {1}", ukey, NewKey));
            foreach (DataRow dr in drar)
            {
                dr.Delete();
            }
            base.OnDetailGridDelete();
        }

        private void insertSizeRatioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataRow ndr = sizeratioTb.NewRow();
            ndr["newkey"] = CurrentDetailData["newkey"];
            ndr["WorkorderUkey"] = CurrentDetailData["Ukey"];
            ndr["Qty"] = 0;
            sizeratioTb.Rows.Add(ndr);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataRow selectDr = ((DataRowView)sizeratio_grid.GetSelecteds(SelectedSort.Index)[0]).Row;
            selectDr.Delete();

            //cal_TotalCutQty(Convert.ToInt32(CurrentDetailData["Ukey"]), Convert.ToInt32(CurrentDetailData["NewKey"]));
            cal_TotalCutQty(CurrentDetailData["Ukey"], CurrentDetailData["NewKey"]);
        }

        private void numericBox_MarkerLengthY_Validated(object sender, EventArgs e)
        {
            cal_Cons(true, true);
        }

        private void textBox_MarkerLengthE_Validating(object sender, CancelEventArgs e)
        {
            cal_Cons(true, true);
        }

        private void numericBox_UnitCons_Validated(object sender, EventArgs e)
        {
            cal_Cons(false, true);
        }

        private void cal_Cons(bool updateConsPC, bool updateCons) //update Cons
        {
            gridValid();
            int sizeRatioQty;
            object comput;
            comput = sizeratioTb.Compute("Sum(Qty)", string.Format("WorkOrderUkey = '{0}'", CurrentDetailData["Ukey"]));
            if (comput == DBNull.Value) sizeRatioQty = 0;
            else sizeRatioQty = Convert.ToInt32(comput);

            decimal MarkerLengthNum, Conspc;
            string MarkerLengthstr, lenY, lenE;
            if (MyUtility.Check.Empty(CurrentDetailData["MarkerLengthE"])) lenY = "0";
            else lenY = CurrentDetailData["MarkerLengthY"].ToString();
            if (MyUtility.Check.Empty(CurrentDetailData["MarkerLengthE"])) lenE = "0-0/0+0\"";
            else lenE = CurrentDetailData["MarkerLengthE"].ToString();
            MarkerLengthstr = lenY + "Ｙ" + lenE;
            MarkerLengthNum = Convert.ToDecimal(MyUtility.GetValue.Lookup(string.Format("Select dbo.MarkerLengthToYDS('{0}')", MarkerLengthstr)));
            //Conspc = MarkerLength / SizeRatio Qty
            if (sizeRatioQty == 0) Conspc = 0;
            else Conspc = MarkerLengthNum / sizeRatioQty;
            if (updateConsPC == true)
                CurrentDetailData["Conspc"] = Conspc;
            if (updateCons == true)
                CurrentDetailData["Cons"] = MarkerLengthNum * Convert.ToInt32(CurrentDetailData["Layer"]);
        }

        //1172: CUTTING_P02_Cutting Work Order
        //private void cal_TotalCutQty(int workorderukey,int newkey)
        private void cal_TotalCutQty(object workorderukey, object newkey)
        {
            gridValid();
            string TotalCutQtystr;
            TotalCutQtystr = "";
            DataRow[] sizeview = sizeratioTb.Select(string.Format("WorkOrderUkey={0} and NewKey = {1} ", Convert.ToInt32(workorderukey), Convert.ToInt32(newkey)));

            foreach (DataRow dr in sizeview)
            {
                if (TotalCutQtystr == "")
                {
                    TotalCutQtystr = TotalCutQtystr + dr["SizeCode"].ToString().Trim() + "/" + (Convert.ToDecimal(dr["Qty"]) * Convert.ToDecimal(CurrentDetailData["Layer"])).ToString();
                }
                else
                {
                    TotalCutQtystr = TotalCutQtystr + "," + dr["SizeCode"].ToString().Trim() + "/" + (Convert.ToDecimal(dr["Qty"]) * Convert.ToDecimal(CurrentDetailData["Layer"])).ToString();
                }
            }
            CurrentDetailData["CutQty"] = TotalCutQtystr;
        }

        private void insertNewRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataRow ndr = distqtyTb.NewRow();
            ndr["newkey"] = CurrentDetailData["newkey"];
            ndr["WorkorderUkey"] = CurrentDetailData["Ukey"];
            ndr["Qty"] = 0;
            distqtyTb.Rows.Add(ndr);
        }

        private void deleteRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {

            DataRow selectDr = ((DataRowView)distribute_grid.GetSelecteds(SelectedSort.Index)[0]).Row;
            selectDr.Delete();

            totalDisQty();
        }

        private void totalDisQty()
        {
            gridValid();
            if (!MyUtility.Check.Empty(CurrentDetailData["Ukey"]))
            {
                object comput;
                int disqty;
                comput = distqtyTb.Compute("SUM(Qty)", string.Format("workorderUkey = '{0}'", CurrentDetailData["Ukey"]));
                if (comput == DBNull.Value) disqty = 0;
                else disqty = Convert.ToInt32(comput);
                totaldisqtybox.Value = disqty;
            }
            else
            {
                totaldisqtybox.Value = 0;
            }

        }

        private void updateExcess(int workorderukey, int newkey, string sizecode)
        {
            gridValid();
            DataRow[] sizeview = sizeratioTb.Select(string.Format("WorkOrderUkey={0} and NewKey = {1} and SizeCode = '{2}'", workorderukey, newkey, sizecode));
            foreach (DataRow dr in sizeview)
            {
                int now_distqty, org_distqty;
                object comput = distqtyTb.Compute("Sum(Qty)", string.Format("WorkOrderUkey={0} and NewKey = {1} and SizeCode = '{2}'", workorderukey, newkey, dr["SizeCode"]));
                if (comput == DBNull.Value) now_distqty = 0;
                else org_distqty = Convert.ToInt32(comput);

                now_distqty = Convert.ToInt32(dr["Qty"]) * Convert.ToInt32(CurrentDetailData["Layer"]);

                DataRow[] distdr = distqtyTb.Select(string.Format("WorkOrderUkey={0} and NewKey = {1} and SizeCode ='{2}' ", workorderukey, newkey, dr["SizeCode"]));
                if (distdr.Length == 0)
                {
                    DataRow ndr = distqtyTb.NewRow();
                    ndr["WorkOrderUKey"] = workorderukey;
                    ndr["NewKey"] = newkey;
                    ndr["OrderID"] = "EXCESS";
                    ndr["SizeCode"] = dr["SizeCode"];
                    ndr["Qty"] = now_distqty;
                    distqtyTb.Rows.Add(ndr);
                }
                else
                {
                    foreach (DataRow dr2 in distdr)
                    {
                        if (dr2["OrderID"].ToString() != "EXCESS")
                        {
                            if (now_distqty != 0)
                            {

                                if (Convert.ToInt32(dr2["Qty"]) < now_distqty)
                                {
                                    now_distqty = now_distqty - Convert.ToInt32(dr2["Qty"]);
                                }
                                else
                                {
                                    dr2["Qty"] = now_distqty;
                                    now_distqty = 0;
                                }
                            }
                            else
                            {
                                dr2.Delete();
                            }
                        }
                    }

                    if (now_distqty > 0)
                    {
                        DataRow[] exdr = distqtyTb.Select(string.Format("WorkOrderUkey={0} and NewKey = {1} and SizeCode ='{2}' and OrderID ='EXCESS' ", workorderukey, newkey, dr["SizeCode"]));
                        if (exdr.Length == 0)
                        {
                            DataRow ndr = distqtyTb.NewRow();
                            ndr["WorkOrderUKey"] = workorderukey;
                            ndr["NewKey"] = newkey;
                            ndr["OrderID"] = "EXCESS";
                            ndr["SizeCode"] = dr["SizeCode"];
                            ndr["Qty"] = now_distqty;
                            distqtyTb.Rows.Add(ndr);
                        }
                        else
                        {
                            exdr[0]["Qty"] = now_distqty;
                        }

                    }
                    else
                    {
                        DataRow[] exdr = distqtyTb.Select(string.Format("WorkOrderUkey={0} and NewKey = {1} and SizeCode ='{2}' and OrderID ='EXCESS' ", workorderukey, newkey, dr["SizeCode"]));
                        if (exdr.Length > 0)
                            exdr[0].Delete();
                    }
                }

            }
        }

        protected override bool ClickSaveBefore()
        {
            gridValid();
            #region 刪除Qty 為0
            DataTable copyTb = sizeratioTb.Copy();
            DataRow[] deledr;
            foreach (DataRow dr in copyTb.Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    if (Convert.ToInt16(dr["Qty"]) == 0 || MyUtility.Check.Empty(dr["SizeCode"]))
                    {
                        deledr = sizeratioTb.Select(string.Format("WorkOrderUkey = {0} and newKey = {1} and sizeCode = '{2}'", dr["WorkOrderUkey"], dr["NewKey"], dr["SizeCode"]));
                        if (deledr.Length > 0) deledr[0].Delete();
                    }
                }
            }
            DataTable copyTb2 = distqtyTb.Copy();
            foreach (DataRow dr2 in copyTb2.Rows)
            {
                if (dr2.RowState != DataRowState.Deleted)
                {
                    if ((Convert.ToInt16(dr2["Qty"]) == 0 || MyUtility.Check.Empty(dr2["SizeCode"]) || MyUtility.Check.Empty(dr2["Article"])) && dr2["OrderID"].ToString() != "EXCESS")
                    {
                        deledr = distqtyTb.Select(string.Format("WorkOrderUkey = {0} and newKey = {1} and sizeCode = '{2}' and Article = '{3}' and OrderID = '{4}'", dr2["WorkOrderUkey"], dr2["NewKey"], dr2["SizeCode"], dr2["Article"], dr2["OrderID"]));
                        if (deledr.Length > 0) deledr[0].Delete();
                    }
                }
            }
            #endregion
            DataTable dt;
            string msg1 = "", msg2 = "";
            MyUtility.Tool.ProcessWithDatatable(sizeratioTb, "SizeCode,WorkOrderUkey,NewKey", "Select SizeCode,WorkOrderUkey,NewKey,Count() as countN from #tmp having countN >1 Group by SizeCode,WorkOrderUkey,NewKey", out dt);
            if (dt != null)
            {

                foreach (DataRow dr in dt.Rows)
                {
                    msg1 = msg1 + dr["WorkOrderUkey"].ToString() + "\n";
                }
            }

            MyUtility.Tool.ProcessWithDatatable(distqtyTb, "OrderID,Article,SizeCode,WorkOrderUkey,NewKey", "Select OrderID,Article,SizeCode,WorkOrderUkey,NewKey,Count() as countN from #tmp having countN >1 Group by OrderID,Article,SizeCode,WorkOrderUkey,NewKey", out dt);
            if (dt != null)
            {

                foreach (DataRow dr in dt.Rows)
                {
                    msg2 = msg2 + dr["WorkOrderUkey"].ToString() + "\n";
                }
            }
            if (!MyUtility.Check.Empty(msg1))
            {
                MyUtility.Msg.WarningBox("The SizeRatio duplicate ,Please see below <Ukey> \n" + msg1);
                return false;
            }
            if (!MyUtility.Check.Empty(msg2))
            {
                MyUtility.Msg.WarningBox("The Distribute Qty data duplicate ,Please see below <Ukey> \n" + msg2);
                return false;
            }
            return base.ClickSaveBefore();
        }

        protected override DualResult ClickSavePost()
        {
            int ukey, newkey;
            DataRow[] dray;
            foreach (DataRow dr in DetailDatas)
            {
                ukey = Convert.ToInt32(dr["Ukey"]);
                newkey = Convert.ToInt32(dr["Newkey"]);
                if (dr.RowState != DataRowState.Deleted) //將Ukey 寫入其他Table
                {
                    dray = sizeratioTb.Select(string.Format("newkey={0} and workorderUkey= 0", newkey)); //0表示新增
                    foreach (DataRow dr2 in dray)
                    {
                        dr2["WorkOrderUkey"] = ukey;
                    }

                    dray = distqtyTb.Select(string.Format("newkey={0} and workorderUkey= 0", newkey)); //0表示新增
                    foreach (DataRow dr2 in dray)
                    {
                        dr2["WorkOrderUkey"] = ukey;
                    }

                    dray = PatternPanelTb.Select(string.Format("newkey={0} and workorderUkey= 0", newkey)); //0表示新增
                    foreach (DataRow dr2 in dray)
                    {
                        dr2["WorkOrderUkey"] = ukey;
                    }
                }
            }
            string delsql = "", updatesql = "", insertsql = "";
            string cId = CurrentMaintain["ID"].ToString();
            #region SizeRatio 修改
            foreach (DataRow dr in sizeratioTb.Rows)
            {
                #region 刪除
                if (dr.RowState == DataRowState.Deleted)
                {
                    delsql = delsql + string.Format("Delete From WorkOrder_SizeRatio Where WorkOrderUkey={0} and SizeCode ='{1}' and ID ='{2}';", dr["WorkOrderUkey", DataRowVersion.Original], dr["SizeCode", DataRowVersion.Original], cId);
                }
                #endregion
                #region 修改
                if (dr.RowState == DataRowState.Modified)
                {
                    updatesql = updatesql + string.Format("Update WorkOrder_SizeRatio set Qty = {0} where WorkOrderUkey ={1} and SizeCode = '{2}' and id ='{3}';", dr["Qty"], dr["WorkOrderUkey"], dr["SizeCode"], cId);
                }
                #endregion
                #region 新增
                if (dr.RowState == DataRowState.Added)
                {
                    insertsql = insertsql + string.Format("Insert into WorkOrder_SizeRatio(WorkOrderUkey,SizeCode,Qty,ID) values({0},'{1}',{2},'{3}'); ", dr["WorkOrderUkey"], dr["SizeCode"], dr["Qty"], cId);
                }
                #endregion
            }
            #endregion
            #region Distribute 修改
            DataTable DeleteTb = distqtyTb.Copy();
            foreach (DataRow dr in DeleteTb.Rows)
            {
                #region 刪除
                if (dr.RowState == DataRowState.Deleted)
                {
                    delsql = delsql + string.Format("Delete From WorkOrder_distribute Where WorkOrderUkey={0} and SizeCode ='{1}' and Article = '{2}' and OrderID = '{3}' and id='{4}';", dr["WorkOrderUkey", DataRowVersion.Original], dr["SizeCode", DataRowVersion.Original], dr["Article", DataRowVersion.Original], dr["Orderid", DataRowVersion.Original], cId);
                }
                #endregion
            }
            foreach (DataRow dr in distqtyTb.Rows)
            {
                // dr["ID", DataRowVersion.Original]
                #region 修改
                if (dr.RowState == DataRowState.Modified)
                {
                    updatesql = updatesql + string.Format("Update WorkOrder_distribute set Qty = {0} where WorkOrderUkey ={1} and SizeCode = '{2}' and Article = '{3}' and OrderID = '{4}' and ID ='{5}'; ", dr["Qty"], dr["WorkOrderUkey"], dr["SizeCode"], dr["Article"], dr["OrderID"], cId);
                }
                #endregion
                #region 新增
                if (dr.RowState == DataRowState.Added)
                {
                    insertsql = insertsql + string.Format("Insert into WorkOrder_distribute(WorkOrderUkey,SizeCode,Qty,Article,OrderID,ID) values({0},'{1}',{2},'{3}','{4}','{5}'); ", dr["WorkOrderUkey"], dr["SizeCode"], dr["Qty"], dr["Article"], dr["OrderID"], cId);
                }
                #endregion
            }
            #endregion
            #region PatternPanel 修改
            foreach (DataRow dr in PatternPanelTb.Rows)
            {
                #region 刪除
                if (dr.RowState == DataRowState.Deleted)
                {
                    delsql = delsql + string.Format("Delete From WorkOrder_distribute Where WorkOrderUkey={0} and SizeCode ='{1}' and Article = '{2}' and OrderID = '{3}' and ID = '{4}';", dr["WorkOrderUkey", DataRowVersion.Original], dr["SizeCode", DataRowVersion.Original], dr["Article", DataRowVersion.Original], dr["Orderid", DataRowVersion.Original], cId);
                }
                #endregion
                #region 修改
                if (dr.RowState == DataRowState.Modified)
                {
                    updatesql = updatesql + string.Format("Update WorkOrder_distribute set Qty = {0} where WorkOrderUkey ={1} and SizeCode = '{2}' and Article = '{3}' and OrderID = '{4}' and ID='{5}'; ", dr["Qty"], dr["WorkOrderUkey"], dr["SizeCode"], dr["Article"], dr["OrderID"], cId);
                }
                #endregion
                #region 新增
                if (dr.RowState == DataRowState.Added)
                {
                    insertsql = insertsql + string.Format("Insert into WorkOrder_distribute(WorkOrderUkey,SizeCode,Qty,Article,OrderID,ID) values({0},'{1}',{2},'{3}','{4}','{5}');", dr["WorkOrderUkey"], dr["SizeCode"], dr["Qty"], dr["Article"], dr["OrderID"], cId);
                }
                #endregion
            }
            #endregion

            DualResult upResult;
            if (!MyUtility.Check.Empty(delsql))
            {
                if (!(upResult = DBProxy.Current.Execute(null, delsql)))
                {
                    return upResult;
                }
            }
            if (!MyUtility.Check.Empty(updatesql))
            {
                if (!(upResult = DBProxy.Current.Execute(null, updatesql)))
                {
                    return upResult;
                }
            }
            if (!MyUtility.Check.Empty(insertsql))
            {
                if (!(upResult = DBProxy.Current.Execute(null, insertsql)))
                {
                    return upResult;
                }
            }
            return base.ClickSavePost();
        }

        protected override void ClickSaveAfter()
        {
            base.ClickSaveAfter();
            foreach (DataRow dr in DetailDatas) dr["SORT_NUM"] = 0;  //編輯後存檔，將[SORT_NUM]歸零
            sorting(comboBox1.Text);
        }

        private void Qtybreak_Click(object sender, EventArgs e)
        {
            gridValid();
            grid.ValidateControl();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var dr = this.CurrentDetailData; if (null == dr) return;
            var frm = new Sci.Production.Cutting.P02_PatternPanel(!this.EditMode && MyUtility.Check.Empty(CurrentDetailData["Cutplanid"]), dr["Ukey"].ToString(), null, null, layersTb);
            frm.ShowDialog(this);

            //1394: CUTTING_P02_Cutting Work Order。(1) 按了Pattern panel按鈕，執行結束回到Workorder作業畫面時不要重新load資料。
            //this.RenewData();
            //sorting(comboBox1.Text);  //避免順序亂掉
            //this.OnDetailEntered();
        }

        protected override bool ClickPrint()
        {
            Sci.Production.Cutting.P02_Print callNextForm;
            if (drTEMP != null)
            {
                callNextForm = new P02_Print(drTEMP, CurrentMaintain["ID"].ToString());
            }
            else
            {
                callNextForm = new P02_Print(CurrentDetailData, CurrentMaintain["ID"].ToString());
            }

            callNextForm.ShowDialog(this);
            return base.ClickPrint();
        }

        //編輯時，將[SORT_NUM]賦予流水號
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            int serial = 1;
            this.detailgridbs.SuspendBinding();
            foreach (DataRow dr in DetailDatas)
            {
                dr["SORT_NUM"] = serial;
                serial++;
            }
            this.detailgridbs.ResumeBinding();
        }


    }
}
