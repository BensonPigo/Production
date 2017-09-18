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
using System.Linq;

namespace Sci.Production.Cutting
{
    public partial class P02 : Sci.Win.Tems.Input6
    {
        #region
        private string loginID = Sci.Env.User.UserID;
        private string keyWord = Sci.Env.User.Keyword;

        private DataTable sizeratioTb, layersTb, distqtyTb, qtybreakTb, sizeGroup, spTb, artTb, PatternPanelTb;
        private DataTable chksize;
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
        #endregion

        public P02(ToolStripMenuItem menuitem, string history)
            : base(menuitem)
        {
            InitializeComponent();
            Dictionary<String, String> comboBox1_RowSource = new Dictionary<string, string>();
            comboBox1_RowSource.Add("FabricPanelCode", "Pattern Panel");
            comboBox1_RowSource.Add("SP", "SP");
            comboBox1_RowSource.Add("Cut#", "Cut#");
            comboBox1_RowSource.Add("Ref#", "Ref#");
            comboBox1_RowSource.Add("Cutplan#", "Cutplan#");
            comboBox1_RowSource.Add("MarkerName", "MarkerName");
            comboBox1.DataSource = new BindingSource(comboBox1_RowSource, null);
            comboBox1.ValueMember = "Key";
            comboBox1.DisplayMember = "Value";
            txtCutCell.MDivisionID = Sci.Env.User.Keyword;
            /*
             *設定Binding Source for Text
            */
            this.displayMarkerName.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource2, "MarkerName", true));
            this.displayColor.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource2, "colorid", true));
            this.numUnitCons.DataBindings.Add(new System.Windows.Forms.Binding("Value", bindingSource2, "Conspc", true));
            this.txtCutCell.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource2, "CutCellid", true));
            this.numCons.DataBindings.Add(new System.Windows.Forms.Binding("Value", bindingSource2, "Cons", true));
            this.txtFabricCombo.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource2, "FabricCombo", true));
            this.txtFabricPanelCode.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource2, "FabricPanelCode", true));
            this.editDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource2, "Description", true));
            this.displayFabricType_Refno.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource2, "MtlTypeID_SCIRefno", true));

            this.displayWorkOrderDownloadid.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource2, "MarkerDownLoadId", true));
            this.displayCutplanNo.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource2, "Cutplanid", true));
            this.displayTotalCutQty.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource2, "CutQty", true));
            this.numMarkerLengthY.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource2, "MarkerLengthY", true));
            this.txtMarkerLengthE.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource2, "MarkerLengthE", true));
            this.txtMarkerLength.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource2, "MarkerLength", true));
            this.txtPatternPanel.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource2, "PatternPanel", true));
            this.lbshc.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource2, "shc", true));
            this.displayBoxMarkerNo.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource2, "MarkerNo", true));

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
            detailgrid.Click += detailgrid_Click;
        }

        void detailgrid_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(detailgrid.CurrentCell)) return;
            detailgrid.CurrentCell = detailgrid[detailgrid.CurrentCell.ColumnIndex, detailgrid.CurrentCell.RowIndex];
            detailgrid.BeginEdit(true);
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataTable queryDT;
            string querySql = string.Format(@"
select '' FTYGroup

union 
select distinct FTYGroup 
from Factory 
where MDivisionID = '{0}'", Sci.Env.User.Keyword);
            DBProxy.Current.Select(null, querySql, out queryDT);
            MyUtility.Tool.SetupCombox(queryfors, 1, queryDT);
            queryfors.SelectedIndex = 0;
            queryfors.SelectedIndexChanged += (s, e) =>
            {
                switch (queryfors.SelectedIndex)
                {
                    case 0:
                        this.DefaultWhere = "";
                        break;
                    default:
                        this.DefaultWhere = string.Format("FactoryID = '{0}'", queryfors.SelectedValue);
                        break;
                }               
                this.ReloadDatas();
            };
        }

        protected override Ict.DualResult OnDetailSelectCommandPrepare(Win.Tems.InputMasterDetail.PrepareDetailSelectCommandEventArgs e)
        {
            #region 主Table 左邊grid
            string masterID = (e.Master == null) ? "" : e.Master["id"].ToString();
            string cmdsql = string.Format(@"
Select
	a.*
	,article.article
	,SizeCode.SizeCode
	,CutQty.CutQty
	,FabricPanelCode.FabricPanelCode
	,PatternPanel.PatternPanel
	,fabeta.fabeta
	,Sewinline.Sewinline
	,actcutdate.actcutdate
	,adduser.adduser
	,edituser.edituser
	,totallayer =
	(
		Select sum(layer)
		From Order_EachCons ea WITH (NOLOCK) 
		inner join Order_EachCons_color ea_b WITH (NOLOCK) on ea.ukey = ea_b.order_eachConsUkey 
		Where ea.id = a.ID and ea.Markername = a.markername and ea_b.colorid = a.colorid
	)
	,multisize.multisize
	,Order_SizeCode_Seq.Order_SizeCode_Seq
	,SORT_NUM =0
    ,c.MtlTypeID
    ,MtlTypeID_SCIRefno = concat(c.MtlTypeID, ' / ' , a.SCIRefno)
	,c.DescDetail
    ,c.Description
	,newkey = 0
	,MarkerLengthY = substring(a.MarkerLength,1,2)
	,MarkerLengthE = substring(a.MarkerLength,4,13) 
    ,shc = iif(isnull(shc.RefNo,'')='','','Shrinkage Issue, Spreading Backward Speed: 2, Loose Tension')
from Workorder a WITH (NOLOCK) left join fabric c WITH (NOLOCK) on c.SCIRefno = a.SCIRefno
outer apply(select RefNo from ShrinkageConcern where RefNo=a.RefNo and Junk=0) shc
outer apply
(
	select article = stuff(
	(
		Select distinct concat('/' ,Article)
		From dbo.WorkOrder_Distribute b WITH (NOLOCK) 
		Where b.workorderukey = a.Ukey and b.article!=''
		For XML path('')
	),1,1,'')
) as article
outer apply
(
	select SizeCode = stuff(
	(
		Select concat(', ' , c.sizecode, '/ ', c.qty)
		From WorkOrder_SizeRatio c WITH (NOLOCK) 
		Where c.WorkOrderUkey =a.Ukey 
		For XML path('')
	),1,1,'')
) as SizeCode
outer apply
(
	select CutQty = stuff(
	(
		Select concat(', ', c.sizecode, '/ ', c.qty * a.layer)
		From WorkOrder_SizeRatio c WITH (NOLOCK) 
		Where  c.WorkOrderUkey =a.Ukey 
		For XML path('')
	),1,1,'')
) as CutQty
outer apply
(
	select FabricPanelCode = stuff(
	(
		Select concat('+ ', FabricPanelCode)
		From WorkOrder_PatternPanel c WITH (NOLOCK) 
		Where c.WorkOrderUkey =a.Ukey 
		For XML path('')
	),1,1,'')
) as FabricPanelCode
outer apply
(
	select PatternPanel = stuff(
	(
		Select concat('+ ', PatternPanel)
		From WorkOrder_PatternPanel c WITH (NOLOCK) 
		Where c.WorkOrderUkey =a.Ukey 
		For XML path('')
	),1,1,'')
) as PatternPanel
outer apply
(
	Select fabeta = iif(e.Complete=1, e.FinalETA, iif(e.Eta is not null, e.eta, iif(e.shipeta is not null, e.shipeta,e.finaletd)))
	From PO_Supp_Detail e WITH (NOLOCK) 
	Where e.id = (Select distinct poid from orders WITH (NOLOCK) where orders.cuttingsp = a.ID) and e.seq1 = a.seq1 and e.seq2 = a.seq2
) as fabeta
outer apply
(
	Select Sewinline = Min(sew.Inline)
	From SewingSchedule sew WITH (NOLOCK)
	inner join SewingSchedule_detail sew_b WITH (NOLOCK) on sew_b.id = sew.id
	inner join WorkOrder_Distribute h WITH (NOLOCK) on h.orderid = sew_b.OrderID and h.Article = sew_b.Article and h.orderid = sew.orderid
	Where h.WorkOrderUkey = a.ukey 
) as Sewinline
outer apply
(
	Select actcutdate = Min(cut.cdate)
	From cuttingoutput cut WITH (NOLOCK) 
	inner join cuttingoutput_detail cut_b WITH (NOLOCK) on cut.id = cut_b.id
	Where cut_b.workorderukey = a.Ukey
)  as actcutdate
outer apply(Select adduser = Name From Pass1 ps WITH (NOLOCK) Where ps.id = a.addName) as adduser
outer apply(Select edituser = Name From Pass1 ps WITH (NOLOCK) Where ps.id = a.editName) as edituser
outer apply
(
	Select multisize = iif(count(size.sizecode)>1,2,1) 
	From WorkOrder_SizeRatio size WITH (NOLOCK) 
	Where a.ukey = size.WorkOrderUkey
) as multisize
outer apply
(
	select Order_SizeCode_Seq = max(c.Seq)
	from WorkOrder_SizeRatio b WITH (NOLOCK)
	left join Order_SizeCode c WITH (NOLOCK) on c.Id = b.ID and c.SizeCode = b.SizeCode
	where b.WorkOrderUkey = a.Ukey
) as Order_SizeCode_Seq
where a.id = '{0}'            
            ", masterID);
            this.DetailSelectCommand = cmdsql;
            #endregion

            #region SizeRatio
            cmdsql = string.Format("Select *,0 as newKey from Workorder_SizeRatio WITH (NOLOCK) where id = '{0}'", masterID);
            DualResult dr = DBProxy.Current.Select(null, cmdsql, out sizeratioTb);
            if (!dr) ShowErr(cmdsql, dr);
            #endregion

            #region layer
            cmdsql = string.Format(@"
Select a.MarkerName,a.Colorid,a.Order_EachconsUkey
	,layer = isnull(sum(a.layer),0)
    ,TotallayerUkey =             
    (
        Select isnull(sum(c.layer),0) as TL
	    from Order_EachCons b WITH (NOLOCK) 
		inner join Order_EachCons_Color c WITH (NOLOCK) on c.id = b.id and c.Order_EachConsUkey = b.ukey
	    where b.id = a.id and b.Markername = a.MarkerName and c.Colorid = a.Colorid and b.Ukey = a.Order_EachconsUkey 
    )
    ,TotallayerMarker =
    (
        Select isnull(sum(c.layer),0) as TL2
	    from Order_EachCons b WITH (NOLOCK) 
		inner join Order_EachCons_Color c WITH (NOLOCK) on b.id = c.id and c.Order_EachConsUkey = b.ukey
	    where b.id = a.id and b.Markername = a.MarkerName and c.Colorid = a.Colorid
    )
From WorkOrder a WITH (NOLOCK) 
Where a.id = '{0}' 
group by a.MarkerName,a.Colorid,a.Order_EachconsUkey,a.id 
Order by a.MarkerName,a.Colorid,a.Order_EachconsUkey
                ", masterID);
            dr = DBProxy.Current.Select(null, cmdsql, out layersTb);
            if (!dr) ShowErr(cmdsql, dr);
            #endregion

            #region distqtyTb / PatternPanelTb
            cmdsql = string.Format(@"Select *,0 as newKey From Workorder_distribute WITH (NOLOCK) Where id='{0}'", masterID);
            dr = DBProxy.Current.Select(null, cmdsql, out distqtyTb);
            if (!dr) ShowErr(cmdsql, dr);

            cmdsql = string.Format(@"Select *,0 as newKey From Workorder_PatternPanel WITH (NOLOCK) Where id='{0}'", masterID);
            dr = DBProxy.Current.Select(null, cmdsql, out PatternPanelTb);
            if (!dr) ShowErr(cmdsql, dr);
            #endregion
            
            #region 建立要使用右鍵開窗Grid
            string settbsql = string.Format(@"
Select a.id, a.article, a.sizecode, a.qty,  isnull(balc.minQty-a.qty,0) as balance, c.workorder_Distribute_Qty
From Order_Qty a WITH (NOLOCK)
inner join orders b WITH (NOLOCK) on a.id = b.id 
outer apply
(
    Select workorder_Distribute_Qty = Sum(Qty) 
    from workorder_Distribute WD WITH (NOLOCK) 
    where WD.ID='{0}' and WD.OrderID=a.id and WD.Article=a.Article and WD.SizeCode=a.SizeCode
) c
outer apply (
select min(qty) as minQty from (
	select FabricCombo,sum(qty) qty --min(qty) as minQty
	from Workorder wo WITH (NOLOCK) , workorder_Distribute wd WITH (NOLOCK) , Order_fabriccode ofb WITH (NOLOCK) 
	Where wo.id = '{0}' and wo.ukey = wd.workorderukey and wo.Id  = ofb.id and wo.FabricPanelCode = ofb.FabricPanelCode 
	and wd.article =a.Article and wd.SizeCode=a.SizeCode and wd.OrderID=a.ID
	group by FabricCombo
	) a
) balc
Where b.cuttingsp ='{0}'
order by id,article,sizecode"
                , masterID);
            DualResult gridResult = DBProxy.Current.Select(null, settbsql, out qtybreakTb);
            sizeGroup = qtybreakTb.DefaultView.ToTable(true, "sizecode");
            artTb = qtybreakTb.DefaultView.ToTable(true, "article");
            spTb = qtybreakTb.DefaultView.ToTable(true, "id");            

            //// 若訂單數量超過裁切分配數量，則更新balance
            //foreach (DataRow dr2 in qtybreakTb.Rows)
            //{
            //    if (MyUtility.Convert.GetDecimal(dr2["qty"]) > MyUtility.Convert.GetDecimal(dr2["workorder_Distribute_Qty"]))
            //    {
            //        dr2["balance"] = MyUtility.Convert.GetDecimal(dr2["qty"]) - MyUtility.Convert.GetDecimal(dr2["workorder_Distribute_Qty"]);
            //    }
            //}
            //用來檢查size是否存在
            string sqlsizechk = string.Format(@"
select distinct w.SizeCode
from Workorder_SizeRatio w
where w.ID = '{0}'", masterID);
            DBProxy.Current.Select(null, sqlsizechk, out chksize);
            #endregion

            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            gridSizeRatio.DataSource = sizeratiobs;
            sizeratiobs.DataSource = sizeratioTb;
            distributebs.DataSource = distqtyTb;
            gridDistributetoSPNo.DataSource = distributebs;
            qtybreakds.DataSource = qtybreakTb;
            gridQtyBreakdown.DataSource = qtybreakds;

            sizeratioTb.DefaultView.RowFilter = "";
            qtybreakTb.DefaultView.RowFilter = "";
            OnDetailGridRowChanged();

            DataRow orderdr;
            MyUtility.Check.Seek(string.Format("Select * from Orders WITH (NOLOCK) where id='{0}'", CurrentMaintain["ID"]), out orderdr);

            txtStyle.Text = orderdr == null ? "" : orderdr["Styleid"].ToString();
            txtLine.Text = orderdr == null ? "" : orderdr["SewLine"].ToString();
            string maxcutrefCmd = string.Format("Select Max(Cutref) from workorder WITH (NOLOCK) where mDivisionid = '{0}'", keyWord);
            textbox_LastCutRef.Text = MyUtility.GetValue.Lookup(maxcutrefCmd);
            comboBox1.Enabled = !EditMode;  //Sorting於編輯模式時不可選取

            foreach (DataRow dr in DetailDatas) dr["Article"] = dr["Article"].ToString().TrimEnd('/');
            sorting(comboBox1.Text);
            this.detailgrid.SelectRowTo(0);
            this.detailgrid.AutoResizeColumns();
            btnQuantityBreakdown.ForeColor = MyUtility.Check.Seek(string.Format("select ID from Order_Qty WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]))) ? Color.Blue : Color.Black;
        }

        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            DataGridViewGeneratorDateColumnSettings EstCutDate = new DataGridViewGeneratorDateColumnSettings();
            EstCutDate.CellValidating += (s, e) =>
            {
                if (!this.EditMode) { return; }
                if (!(MyUtility.Check.Empty(e.FormattedValue)))
                {
                    DataRow dr = ((Sci.Win.UI.Grid)((DataGridViewColumn)s).DataGridView).GetDataRow(e.RowIndex);
                    if (e.FormattedValue.ToString() == dr["estcutdate"].ToString()) { return; }
                    if (DateTime.Compare(DateTime.Today, Convert.ToDateTime(e.FormattedValue)) > 0)
                    {
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox("[Est. Cut Date] can not be passed !!");
                    }
                }
            };    
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
                .Text("MarkerName", header: "Marker\r\nName", width: Widths.AnsiChars(5)).Get(out col_Markername)
                .Text("Fabriccombo", header: "Fabric\r\nCombo", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("FabricPanelCode", header: "Fab_Panel\r\nCode", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("Article", header: "Article", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Colorid", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("Layer", header: "Layers", width: Widths.AnsiChars(5), integer_places: 5).Get(out col_layer)
                .Text("CutQty", header: "Total CutQty", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("orderid", header: "SP#", width: Widths.AnsiChars(13)).Get(out col_sp)
                .Text("SEQ1", header: "SEQ1", width: Widths.AnsiChars(3)).Get(out col_seq1)
                .Text("SEQ2", header: "SEQ2", width: Widths.AnsiChars(2)).Get(out col_seq2)
                .Date("Fabeta", header: "Fabric Arr Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("estcutdate", header: "Est. Cut Date", width: Widths.AnsiChars(10), settings: EstCutDate).Get(out col_estcutdate)
                .Date("sewinline", header: "Sewing inline", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Cutcellid", header: "Cell", width: Widths.AnsiChars(2)).Get(out col_cutcell)
                .Text("Cutplanid", header: "Cutplan#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Date("actcutdate", header: "Act. Cut Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Edituser", header: "Edit Name", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .DateTime("EditDate", header: "Edit Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("Adduser", header: "Add Name", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .DateTime("AddDate", header: "Add Date", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("UKey", header: "Key", width: Widths.AnsiChars(10), iseditingreadonly: true);
            
            Helper.Controls.Grid.Generator(this.gridSizeRatio)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(5)).Get(out col_sizeRatio_size)
                .Numeric("Qty", header: "Ratio", width: Widths.AnsiChars(5), integer_places: 6).Get(out col_sizeRatio_qty);

            Helper.Controls.Grid.Generator(this.gridDistributetoSPNo)
                .Text("orderid", header: "SP#", width: Widths.AnsiChars(15)).Get(out col_dist_sp)
                .Text("article", header: "article", width: Widths.AnsiChars(8)).Get(out col_dist_article)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(4)).Get(out col_dist_size)
                .Numeric("Qty", header: "Qty", width: Widths.AnsiChars(3), integer_places: 6).Get(out col_dist_qty);

            Helper.Controls.Grid.Generator(this.gridQtyBreakdown)
                .Text("id", header: "SP#", width: Widths.AnsiChars(13))
                .Text("article", header: "article", width: Widths.AnsiChars(7))
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(3))
                .Numeric("Qty", header: "Order \nQty", width: Widths.AnsiChars(3), integer_places: 6)
                .Numeric("Balance", header: "Balance", width: Widths.AnsiChars(5), integer_places: 6, settings: breakqty);

            this.detailgrid.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8F);
            #endregion

            changeeditable();
        }

        private void changeeditable()// Grid Cell 物件設定
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
            #region Layers
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
                if (!this.EditMode || e.RowIndex == -1) return; 
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                string oldvalue = dr["layer"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (oldvalue == newvalue) return;

                CurrentDetailData["layer"] = newvalue;
                
                int sumlayer = 0;
                if (MyUtility.Check.Empty(CurrentDetailData["Order_EachConsUkey"]))
                {
                    object O_sumLayer = ((DataTable)detailgridbs.DataSource).Compute("sum(layer)", string.Format("MarkerName = '{0}' and Colorid = '{1}'", CurrentDetailData["MarkerName"], CurrentDetailData["Colorid"]));
                    if (!O_sumLayer.Empty()) sumlayer = Convert.ToInt32(O_sumLayer);

                    DataRow[] drar = layersTb.Select(string.Format("MarkerName = '{0}' and Colorid = '{1}'", CurrentDetailData["MarkerName"], CurrentDetailData["Colorid"]));
                    if (drar.Length != 0) numBalanceLayer.Value = sumlayer - Convert.ToInt32(drar[0]["TotalLayerMarker"]);
                }
                else
                {
                    object O_sumLayer = ((DataTable)detailgridbs.DataSource).Compute("sum(layer)", string.Format("Order_EachconsUkey = '{0}' and Colorid = '{1}'", CurrentDetailData["Order_EachConsUkey"], CurrentDetailData["Colorid"]));
                    if (!O_sumLayer.Empty()) sumlayer = Convert.ToInt32(O_sumLayer);

                    DataRow[] drar = layersTb.Select(string.Format("Order_EachconsUkey = '{0}' and Colorid = '{1}'", CurrentDetailData["Order_EachConsUkey"], CurrentDetailData["Colorid"]));
                    if (drar.Length != 0) numBalanceLayer.Value = sumlayer - Convert.ToInt32(drar[0]["TotalLayerUkey"]);
                }
                cal_TotalCutQty(CurrentDetailData["Ukey"], CurrentDetailData["NewKey"]);
                
                int newsumQty = 0;
                if (sizeratiobs.DataSource != null)
                {
                    object Sq = ((DataTable)sizeratiobs.DataSource).DefaultView.ToTable().Compute("SUM(Qty)", "");
                    if (!Sq.Empty()) newsumQty = MyUtility.Convert.GetInt(CurrentDetailData["layer"]) * Convert.ToInt32(Sq);
                }

                //重算DistributeqQty
                int oldttlqty = (int)numTotalDistributionQty.Value;
                int diff = newsumQty - oldttlqty;
                numTotalDistributionQty.Value = newsumQty;
                
                if (diff > 0)
                {
                    DataRow[] sizeR = sizeratioTb.Select(string.Format("WorkOrderUkey = '{0}' and NewKey = '{1}'", CurrentDetailData["Ukey"].ToString(), CurrentDetailData["NewKey"].ToString()));
                    foreach (DataRow drr in sizeR)
                    {
                        updateExcess(Convert.ToInt32(CurrentDetailData["Ukey"]), Convert.ToInt32(CurrentDetailData["NewKey"]), drr["SizeCode"].ToString());
                    }                    
                }
                if (diff<0)
                {
                    string sizetmp = "";
                    DataRow[] dist = distqtyTb.Select(string.Format("WorkOrderUkey = '{0}' and NewKey = '{1}' ", CurrentDetailData["Ukey"].ToString(), CurrentDetailData["NewKey"].ToString()));
                    
                    foreach (DataRow drr in dist)
                    {
                        DataRow[] sizeR = sizeratioTb.Select(string.Format("WorkOrderUkey = '{0}' and NewKey = '{1}' and SizeCode = '{2}'", CurrentDetailData["Ukey"].ToString(), CurrentDetailData["NewKey"].ToString(), drr["SizeCode"].ToString()));
                        if (sizetmp == drr["SizeCode"].ToString())//size是否和前一筆相同，判斷是否有重複的size
                        {
                            drr["Qty"] = 0;
                        }
                        else
                        {
                            drr["Qty"] = MyUtility.Convert.GetInt(sizeR[0]["Qty"]) * MyUtility.Convert.GetInt(newvalue);
                        }
                        sizetmp = drr["SizeCode"].ToString();
                        if (drr["OrderID"].ToString() == "EXCESS")
                        {
                              drr["Qty"] = 0;
                        }
                    }
                }
                cal_Cons(true, true);
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

                    sele = new SelectItem(spTb, "ID", "15@300,400", dr["OrderID"].ToString(), columndecimals: "50");
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
                    dr["orderid"] = "";
                    dr.EndEdit();
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("<SP> : {0} data not found!", newvalue));
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
                    string poid = MyUtility.GetValue.Lookup(string.Format("Select poid from orders WITH (NOLOCK) where id ='{0}'", CurrentMaintain["ID"]));
                    DBProxy.Current.Select(null, string.Format("Select SEQ1,SEQ2,Colorid From PO_Supp_Detail WITH (NOLOCK) Where id='{0}' and SCIRefno ='{1}'", poid, dr["SCIRefno"]), out poTb);
                    sele = new SelectItem(poTb, "SEQ1,SEQ2,Colorid", "3,2,8@350,300", dr["SEQ1"].ToString(), false, ",");
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
                string poid = MyUtility.GetValue.Lookup(string.Format("Select poid from orders WITH (NOLOCK) where id ='{0}'", CurrentMaintain["ID"]));
                if (!MyUtility.Check.Seek(string.Format("Select * from po_Supp_Detail WITH (NOLOCK) where id='{0}' and seq1 ='{1}'", poid, newvalue)))
                {
                    dr["SEQ1"] = "";
                    dr.EndEdit();
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("<SEQ1> : {0} data not found!", newvalue));
                    return;
                }
                else
                {
                    if (!MyUtility.Check.Seek(string.Format("Select * from po_Supp_Detail WITH (NOLOCK) where id='{0}' and seq1 ='{1}' and seq2 ='{2}'", poid, newvalue, CurrentDetailData["SEQ2"]), out seledr))
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
                    string poid = MyUtility.GetValue.Lookup(string.Format("Select poid from orders WITH (NOLOCK) where id ='{0}'", CurrentMaintain["ID"]));
                    DBProxy.Current.Select(null, string.Format("Select SEQ1,SEQ2,Colorid From PO_Supp_Detail WITH (NOLOCK) Where id='{0}' and SCIRefno ='{1}'", poid, dr["SCIRefno"]), out poTb);
                    sele = new SelectItem(poTb, "SEQ1,SEQ2,Colorid", "3,2,8@350,300", dr["SEQ2"].ToString(), false, ",");
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
                string poid = MyUtility.GetValue.Lookup(string.Format("Select poid from orders WITH (NOLOCK) where id ='{0}'", CurrentMaintain["ID"]));
                if (!MyUtility.Check.Seek(string.Format("Select * from po_Supp_Detail WITH (NOLOCK) where id='{0}' and seq2 ='{1}'", poid, newvalue)))
                {
                    dr["SEQ2"] = "";
                    dr.EndEdit();
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("<SEQ2> : {0} data not found!", newvalue));
                    return;
                }
                else
                {
                    if (!MyUtility.Check.Seek(string.Format("Select * from po_Supp_Detail WITH (NOLOCK) where id='{0}' and seq1 ='{1}' and seq2 ='{2}'", poid, CurrentDetailData["SEQ1"], newvalue), out seledr))
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
            bool cellchk = true;
            col_cutcell.EditingMouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    // Parent form 若是非編輯狀態就 return 
                    if (!this.EditMode) { return; }
                    DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                    // 若 cutref != empty 則不可編輯
                    if (!MyUtility.Check.Empty(dr["Cutplanid"])) return;
                    SelectItem sele;
                    DataTable cellTb;
                    DBProxy.Current.Select(null, string.Format("Select id from Cutcell WITH (NOLOCK) where mDivisionid = '{0}' and junk=0", keyWord), out cellTb);
                    sele = new SelectItem(cellTb, "ID", "10@300,300", dr["CutCellid"].ToString(), false, ",");
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    dr["cutCellid"] = sele.GetSelectedString();
                    dr.EndEdit();

                    checkCuttingWidth(dr["cutCellid"].ToString(), dr["SCIRefno"].ToString());
                    cellchk = false;
                }
            };
            col_cutcell.CellValidating += (s, e) =>
            {
                if (!this.EditMode) { return; }
                // 右鍵彈出功能
                if (e.RowIndex == -1) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);

                // 空白不檢查
                if (e.FormattedValue.ToString().Empty()) return;

                string oldvalue = dr["cutcellid"].ToString();
                string newvalue = e.FormattedValue.ToString();               
                
                if (oldvalue == newvalue) return;

                DataTable cellTb;
                DBProxy.Current.Select(null, string.Format("Select id from Cutcell WITH (NOLOCK) where mDivisionid = '{0}' and junk=0", keyWord), out cellTb);

                DataRow[] seledr = cellTb.Select(string.Format("ID='{0}'", newvalue));
                if (seledr.Length == 0)
                {
                    dr["cutCellid"] = "";
                    dr.EndEdit();
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("<Cell> : {0} data not found!", newvalue));
                    return;
                }

                dr["cutCellid"] = newvalue;
                if (!cellchk)
                {
                    cellchk = true;
                }
                else
                {
                    checkCuttingWidth(dr["cutCellid"].ToString(), dr["SCIRefno"].ToString());
                }
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
                    DataRow dr = gridSizeRatio.GetDataRow(e.RowIndex);
                    SelectItem sele;

                    string oldvalue = dr["SizeCode"].ToString();

                    sele = new SelectItem(sizeGroup, "SizeCode", "15@300,300", dr["SizeCode"].ToString(), false, ",");
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    e.EditingControl.Text = sele.GetSelectedString();
                    string newvalue = sele.GetSelectedString();
                    dr["SizeCode"] = newvalue;
                    dr.EndEdit();

                    redetailsize(Convert.ToInt32(CurrentDetailData["Ukey"]), Convert.ToInt32(CurrentDetailData["NewKey"]));
                    cal_TotalCutQty(CurrentDetailData["Ukey"], CurrentDetailData["NewKey"]);
                    totalDisQty();
                    DataRow[] distdrs = distqtyTb.Select(string.Format("WorkOrderUkey={0} and NewKey = {1} and SizeCode ='{2}' ", Convert.ToInt32(CurrentDetailData["Ukey"]), Convert.ToInt32(CurrentDetailData["NewKey"]), oldvalue));
                    foreach (DataRow disdr in distdrs)
                    {
                        disdr["SizeCode"] = newvalue;
                    }
                   
                }
            };
            col_sizeRatio_size.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = gridSizeRatio.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(CurrentDetailData["Cutplanid"]) && this.EditMode) ((Ict.Win.UI.TextBox)e.Control).ReadOnly = false;
                else ((Ict.Win.UI.TextBox)e.Control).ReadOnly = true;

            };
            col_sizeRatio_size.CellValidating += (s, e) =>
            {
                if (!this.EditMode) { return; }
                // 右鍵彈出功能
                if (e.RowIndex == -1) return;
                DataRow dr = gridSizeRatio.GetDataRow(e.RowIndex);
                string oldvalue = dr["SizeCode"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (oldvalue == newvalue) return;
                DataRow[] chkrow = chksize.Select(string.Format("SizeCode = '{0}'", newvalue));
                if (chkrow.Length == 0)
                {
                    e.Cancel = true;
                    this.ShowInfo(string.Format("Size <{0}> not found", newvalue));
                    gridSizeRatio.EditingControl.Select();
                    return;
                }
                dr["SizeCode"] = newvalue;
                dr.EndEdit();

                redetailsize(Convert.ToInt32(CurrentDetailData["Ukey"]), Convert.ToInt32(CurrentDetailData["NewKey"]));
                cal_TotalCutQty(CurrentDetailData["Ukey"], CurrentDetailData["NewKey"]);
                totalDisQty();
                DataRow[] distdrs = distqtyTb.Select(string.Format("WorkOrderUkey={0} and NewKey = {1} and SizeCode ='{2}' ", Convert.ToInt32(CurrentDetailData["Ukey"]), Convert.ToInt32(CurrentDetailData["NewKey"]), oldvalue));
                foreach (DataRow disdr in distdrs)
                {
                    disdr["SizeCode"] = newvalue;
                }   
            };
            col_sizeRatio_qty.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                DataRow dr = gridSizeRatio.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(CurrentDetailData["Cutplanid"]) && this.EditMode) ((Ict.Win.UI.NumericBox)e.Control).ReadOnly = false;
                else ((Ict.Win.UI.NumericBox)e.Control).ReadOnly = true;

            };
            col_sizeRatio_qty.CellValidating += (s, e) =>
            {
                // Parent form 若是非編輯狀態就 return 
                if (!this.EditMode) { return; }
                // 右鍵彈出功能
                if (e.RowIndex == -1) return;
                DataRow dr = gridSizeRatio.GetDataRow(e.RowIndex);
                int oldvalue = Convert.ToInt32(dr["Qty"]);
                int newvalue = Convert.ToInt32(e.FormattedValue);
                if (oldvalue == newvalue) return;
                dr["Qty"] = newvalue;
                dr.EndEdit();
                cal_Cons(true, false);
                //cal_TotalCutQty(Convert.ToInt32(CurrentDetailData["Ukey"]), Convert.ToInt32(CurrentDetailData["NewKey"]));
                redetailsize(Convert.ToInt32(CurrentDetailData["Ukey"]), Convert.ToInt32(CurrentDetailData["NewKey"]));
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
                    DataRow dr = gridDistributetoSPNo.GetDataRow(e.RowIndex);
                    SelectItem sele;
                    if (dr["OrderID"].ToString().ToUpper() == "EXCESS" || CurrentDetailData["Cutplanid"].ToString() != "") return;
                    sele = new SelectItem(spTb, "ID", "15@300,400", dr["OrderID"].ToString(), false, ",");
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    e.EditingControl.Text = sele.GetSelectedString();
                }
            };
            col_dist_sp.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                if (CurrentDetailData == null) return;
                DataRow dr = gridDistributetoSPNo.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(CurrentDetailData["Cutplanid"]) && this.EditMode && dr["OrderID"].ToString().ToUpper() != "EXCESS") ((Ict.Win.UI.TextBox)e.Control).ReadOnly = false;
                else ((Ict.Win.UI.TextBox)e.Control).ReadOnly = true;

            };
            col_dist_sp.CellValidating += (s, e) =>
            {
                if (!this.EditMode) { return; }
                // 右鍵彈出功能
                if (e.RowIndex == -1) return;
                DataRow dr = gridDistributetoSPNo.GetDataRow(e.RowIndex);
                string oldvalue = dr["orderid"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (oldvalue == newvalue || newvalue.ToUpper() == "EXCESS") return;

                DataRow[] seledr = spTb.Select(string.Format("ID='{0}'", newvalue));
                if (seledr.Length == 0)
                {
                    dr["orderid"] = "";
                    dr.EndEdit();
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("<SP> : {0} data not found!", newvalue));
                    return;
                }
                if (!MyUtility.Check.Empty(dr["SizeCode"]) && !MyUtility.Check.Empty(dr["Article"]))
                {
                    seledr = qtybreakTb.Select(string.Format("id = '{0}' and SizeCode = '{1}' and Article ='{2}'", newvalue, dr["SizeCode"], dr["Article"]));
                    if (seledr.Length == 0)
                    {
                        dr["OrderID"] = "";
                        dr.EndEdit();
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox(string.Format("<SP#>:{0},<Article>:{1},<SizeCode>:{2}", dr["OrderID"], newvalue, dr["Article"]));
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
                    DataRow dr = gridDistributetoSPNo.GetDataRow(e.RowIndex);
                    SelectItem sele;
                    if (dr["OrderID"].ToString().ToUpper() == "EXCESS" || CurrentDetailData["Cutplanid"].ToString() != "") return;                   
                    DataTable srdt = ((DataTable)sizeratiobs.DataSource).DefaultView.ToTable();
                    DataTable sizeGroup2;
                    MyUtility.Tool.ProcessWithDatatable(srdt, "sizecode", "Select distinct SizeCode from #tmp", out sizeGroup2);
                    sele = new SelectItem(sizeGroup2, "SizeCode", "15@300,300", dr["SizeCode"].ToString(), false, ",");
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    e.EditingControl.Text = sele.GetSelectedString();                    
                }
            };
            col_dist_size.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                if (CurrentDetailData == null) return;
                DataRow dr = gridDistributetoSPNo.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(CurrentDetailData["Cutplanid"]) && this.EditMode && dr["OrderID"].ToString().ToUpper() != "EXCESS") 
                    ((Ict.Win.UI.TextBox)e.Control).ReadOnly = false;
                else ((Ict.Win.UI.TextBox)e.Control).ReadOnly = true;

            };
            col_dist_size.CellValidating += (s, e) =>
            {
                if (!this.EditMode) { return; }
                // 右鍵彈出功能
                if (e.RowIndex == -1) return;
                DataRow dr = gridDistributetoSPNo.GetDataRow(e.RowIndex);
                string oldvalue = dr["SizeCode"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (oldvalue == newvalue) return;
                DataRow[] seledr = sizeGroup.Select(string.Format("SizeCode='{0}'", newvalue));
                if (seledr.Length == 0)
                {
                    dr["SizeCode"] = "";
                    dr.EndEdit();
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("<Size> : {0} data not found!", newvalue));
                    return;
                }
                if (!MyUtility.Check.Empty(dr["OrderID"]) && !MyUtility.Check.Empty(dr["Article"]))
                {
                    seledr = qtybreakTb.Select(string.Format("id = '{0}' and SizeCode = '{1}' and Article ='{2}'", dr["OrderID"], newvalue, dr["Article"]));
                    if (seledr.Length == 0)
                    {
                        dr["SizeCode"] = "";
                        dr.EndEdit();
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox(string.Format("<SP#>:{0},<Article>:{1},<SizeCode>:{2}", dr["OrderID"], newvalue, dr["Article"]));
                        return;
                    }
                }
                dr["SizeCode"] = newvalue;
                dr.EndEdit();
                updateExcess(Convert.ToInt32(CurrentDetailData["Ukey"]), Convert.ToInt32(CurrentDetailData["NewKey"]), dr["SizeCode"].ToString());
                totalDisQty();

            };
            col_dist_article.EditingMouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    // Parent form 若是非編輯狀態就 return 
                    if (!this.EditMode) { return; }
                    if (CurrentDetailData == null) return;
                    DataRow dr = gridDistributetoSPNo.GetDataRow(e.RowIndex);
                    SelectItem sele;
                    if (dr["OrderID"].ToString().ToUpper() == "EXCESS" || CurrentDetailData["Cutplanid"].ToString() != "") return;
                    sele = new SelectItem(artTb, "article", "15@300,300", dr["Article"].ToString(), false, ",");
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel) { return; }
                    e.EditingControl.Text = sele.GetSelectedString();
                }
            };
            col_dist_article.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                if (CurrentDetailData == null) return;
                DataRow dr = gridDistributetoSPNo.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(CurrentDetailData["Cutplanid"]) && this.EditMode && dr["OrderID"].ToString().ToUpper() != "EXCESS") ((Ict.Win.UI.TextBox)e.Control).ReadOnly = false;
                else ((Ict.Win.UI.TextBox)e.Control).ReadOnly = true;

            };
            col_dist_article.CellValidating += (s, e) =>
            {
                if (!this.EditMode) { return; }
                // 右鍵彈出功能

                if (e.RowIndex == -1) return;
                DataRow dr = gridDistributetoSPNo.GetDataRow(e.RowIndex);
                string oldvalue = dr["Article"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (oldvalue == newvalue) return;
                DataRow[] seledr = artTb.Select(string.Format("Article='{0}'", newvalue));
                if (seledr.Length == 0)
                {
                    dr["Article"] = "";
                    dr.EndEdit();
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("<Article> : {0} data not found!", newvalue));
                    return;
                }
                if (!MyUtility.Check.Empty(dr["OrderID"]) && !MyUtility.Check.Empty(dr["SizeCode"]))
                {
                    seledr = qtybreakTb.Select(string.Format("id = '{0}' and SizeCode = '{1}' and Article ='{2}'", dr["OrderID"], dr["SizeCode"], newvalue));
                    if (seledr.Length == 0)
                    {
                        dr["Article"] = "";
                        dr.EndEdit();
                        e.Cancel = true;
                        MyUtility.Msg.WarningBox(string.Format("<SP#>:{0},<Article>:{1},<SizeCode>:{2}", dr["OrderID"], newvalue, dr["SizeCode"]));
                        return;
                    }
                }
                dr["Article"] = newvalue;
                dr.EndEdit();
            };
            //依據Cutplanid&OrderID來設定是否能修改
            col_dist_qty.EditingControlShowing += (s, e) =>
            {
                if (e.RowIndex == -1) return;
                if (CurrentDetailData == null) return;
                DataRow dr = gridDistributetoSPNo.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(CurrentDetailData["Cutplanid"]) && this.EditMode && dr["OrderID"].ToString().ToUpper() != "EXCESS") ((Ict.Win.UI.NumericBox)e.Control).ReadOnly = false;
                else ((Ict.Win.UI.NumericBox)e.Control).ReadOnly = true;
            };
            //重算qty
            col_dist_qty.CellValidating += (s, e) =>
            {
                if (!this.EditMode) { return; }
                // 右鍵彈出功能
                if (e.RowIndex == -1) return;
                DataRow dr = gridDistributetoSPNo.GetDataRow(e.RowIndex);
                string oldvalue = dr["Qty"].ToString();
                string newvalue = e.FormattedValue.ToString();
                if (oldvalue == newvalue) return;

                dr["Qty"] = newvalue;
                dr.EndEdit();

                updateExcess(Convert.ToInt32(CurrentDetailData["Ukey"]), Convert.ToInt32(CurrentDetailData["NewKey"]), dr["SizeCode"].ToString());
                //計算完EXCESS正確後再Total計算
                totalDisQty();
            };
            #endregion
        }
        
        //重組detailgrid的size
        private void redetailsize(int workorderukey, int newkey)
        {
            DataRow[] dsr = sizeratioTb.Select(string.Format("WorkOrderUkey={0} and NewKey = {1}", workorderukey, newkey));
            string sizeqty = "";
            foreach (DataRow dsrr in dsr)
            {
                sizeqty += ", "+ dsrr["SizeCode"] + "/ " + dsrr["Qty"];
            }
            sizeqty = sizeqty.Substring(1);
            DataRow[] dr = ((DataTable)detailgridbs.DataSource).Select(string.Format("Ukey={0} and NewKey = {1}", workorderukey, newkey));
            dr[0]["SizeCode"] = sizeqty;
        }

        //計算Excess
        private void updateExcess(int workorderukey, int newkey, string sizecode)
        {
            //gridValid();
            DataRow[] sizeview = sizeratioTb.Select(string.Format("WorkOrderUkey={0} and NewKey = {1} and SizeCode = '{2}'", workorderukey, newkey, sizecode));
            foreach (DataRow dr in sizeview)
            {
                int now_distqty, org_distqty;
                object comput = distqtyTb.Compute("Sum(Qty)", string.Format("WorkOrderUkey={0} and NewKey = {1} and SizeCode = '{2}'", workorderukey, newkey, dr["SizeCode"]));
                if (comput == DBNull.Value) now_distqty = 0;
                else org_distqty = Convert.ToInt32(comput);

                now_distqty = Convert.ToInt32(dr["Qty"]) * Convert.ToInt32(MyUtility.Check.Empty(CurrentDetailData["Layer"]) ? 0 : CurrentDetailData["Layer"]);
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
                        if (dr2["OrderID"].ToString().Trim().ToUpper() != "EXCESS")
                        {
                            now_distqty = now_distqty - Convert.ToInt32(dr2["Qty"]);
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
                        if (!this.EditMode) { return; }
                        DataRow[] exdr = distqtyTb.Select(string.Format("WorkOrderUkey={0} and NewKey = {1} and SizeCode ='{2}' and OrderID ='EXCESS' ", workorderukey, newkey, dr["SizeCode"]));
                        if (exdr.Length > 0)
                            exdr[0].Delete();
                    }
                }
            }
        }

        private void totalDisQty()
        {
            if (distributebs.DataSource != null)
            {
                object Sq = ((DataTable)distributebs.DataSource).DefaultView.ToTable().Compute("SUM(Qty)", "");
                if (!Sq.Empty()) numTotalDistributionQty.Value = Convert.ToInt32(Sq);
            }
        }

        private void gridValid()
        {
            gridSizeRatio.ValidateControl();
            gridDistributetoSPNo.ValidateControl();
        }

        protected override void OnEditModeChanged()
        {
            base.OnEditModeChanged();
            if (sizeratioMenuStrip != null) sizeratioMenuStrip.Enabled = this.EditMode;
            if (distributeMenuStrip != null) distributeMenuStrip.Enabled = this.EditMode;
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

            //if (MyUtility.Check.Seek(string.Format("Select * from Fabric WITH (NOLOCK) Where SCIRefno ='{0}'", CurrentDetailData["SCIRefno"]), out fabdr))
            //{
            //    displayFabricType_Refno.Text = fabdr["MtlTypeid"].ToString();
            //    editDescription.Text = fabdr["Description"].ToString();
            //}
            //else
            //{
            //    displayFabricType_Refno.Text = "";
            //    editDescription.Text = "";
            //}

            #region 根據左邊Grid Filter 右邊資訊
            if (!MyUtility.Check.Empty(CurrentDetailData["Ukey"]))
            {
           
                sizeratioTb.DefaultView.RowFilter = string.Format("Workorderukey = '{0}'", CurrentDetailData["Ukey"]);
                distqtyTb.DefaultView.RowFilter = string.Format("Workorderukey = '{0}' ", CurrentDetailData["Ukey"]);

                gridDistributetoSPNo.SelectRowTo(0);  
                for (int i = 0; i < gridDistributetoSPNo.Rows.Count; i++) {
                    if (gridDistributetoSPNo.Rows[i].Cells["OrderID"].Value.Equals(CurrentDetailData["OrderID"]))
                    {
                        gridDistributetoSPNo.SelectRowTo(i);

                        break;
                    }
                }


                                 
            }
            if (MyUtility.Convert.GetString(CurrentDetailData["Ukey"]) == "0")
            {
                string ukey = CurrentDetailData["Ukey"].ToString();
                string newkey = CurrentDetailData["newkey"].ToString();
                sizeratioTb.DefaultView.RowFilter = string.Format("Workorderukey = {0} and newkey = {1}", ukey, newkey);
                distqtyTb.DefaultView.RowFilter = string.Format("Workorderukey = {0} and newkey = {1}", ukey, newkey);
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
                    numTotalLayer.Value = 0;
                    numBalanceLayer.Value = 0;
                }
                else
                {
                    numTotalLayer.Value = (decimal)laydr[0]["TotalLayerMarker"];
                    numBalanceLayer.Value = sumlayer - (decimal)laydr[0]["TotalLayerMarker"];
                }
            }
            else
            { //New rule
                order_EachConsTemp = Convert.ToInt32(CurrentDetailData["Order_EachConsUkey"]);
                string selectcondition = string.Format("Order_EachConsUkey = {0} and  Colorid = '{1}'", order_EachConsTemp, CurrentDetailData["Colorid"]);
                DataRow[] laydr = layersTb.Select(selectcondition);
                if (laydr.Length == 0)
                {
                    numTotalLayer.Value = 0;
                    numBalanceLayer.Value = 0;
                }
                else
                {
                    numTotalLayer.Value = (decimal)laydr[0]["TotalLayerUkey"];
                    numBalanceLayer.Value = sumlayer - (decimal)laydr[0]["TotalLayerUkey"];
                }
            }
            
            #region 判斷download id
            string downloadid = MyUtility.GetValue.Lookup("MarkerDownLoadid", CurrentDetailData["Order_EachConsUkey"].ToString(), "Order_EachCons", "Ukey");
            displayEachConsDownloadID.Text = downloadid;
            if (downloadid.Trim() != CurrentDetailData["MarkerDownLoadid"].ToString().Trim())
                downloadid_Text.Visible = true;
            else
                downloadid_Text.Visible = false;
            #endregion

            #region 判斷可否開放修改
            if (MyUtility.Check.Empty(CurrentDetailData["Cutplanid"]) && this.EditMode)
            {
                numMarkerLengthY.ReadOnly = false;
                txtMarkerLengthE.ReadOnly = false;
                numUnitCons.ReadOnly = false;
                txtCutCell.ReadOnly = false;
                txtFabricCombo.ReadOnly = false;
                txtFabricPanelCode.ReadOnly = false;
                sizeratioMenuStrip.Enabled = true;
                distributeMenuStrip.Enabled = true;
            }
            else
            {
                numMarkerLengthY.ReadOnly = true;
                txtMarkerLengthE.ReadOnly = true;
                numUnitCons.ReadOnly = true;
                txtCutCell.ReadOnly = true;
                txtFabricCombo.ReadOnly = true;
                txtFabricPanelCode.ReadOnly = true;
                sizeratioMenuStrip.Enabled = false;
                distributeMenuStrip.Enabled = false;
            }
            #endregion
            totalDisQty();

            
            this.gridSizeRatio.AutoResizeColumns();            
            this.gridQtyBreakdown.AutoResizeColumns();
            //抓到當前編輯的cell
            if (MyUtility.Check.Empty(detailgrid.CurrentCell)) return;
            detailgrid.CurrentCell = detailgrid[detailgrid.CurrentCell.ColumnIndex, detailgrid.CurrentCell.RowIndex];
            detailgrid.BeginEdit(true);
        }

        //程式產生的BindingSource 必須自行Dispose, 以節省資源
        protected override void OnFormDispose()
        {
            base.OnFormDispose();
            bindingSource2.Dispose();
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
                    dv.Sort = "SORT_NUM,FabricCombo,multisize DESC,Colorid,Order_SizeCode_Seq DESC,MarkerName,Ukey";
                    break;
            }
        }

        private void AutoRef_Click(object sender, EventArgs e)
        {
            gridValid();
            grid.ValidateControl();
            #region 變更先將同d,Cutref, FabricPanelCode, CutNo, MarkerName, estcutdate 且有cutref,Cuno無cutplanid 的cutref值找出來Group by→cutref 會相同
            string cmdsql = string.Format(@"
            SELECT isnull(Cutref,'') as cutref, isnull(FabricCombo,'') as FabricCombo, isnull(CutNo,0) as cutno,
            isnull(MarkerName,'') as MarkerName, estcutdate
            FROM Workorder WITH (NOLOCK) 
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
                  From workorder WITH (NOLOCK) 
                  Where (CutNo is not null ) and (cutref is null or cutref ='') 
                    and (estcutdate is not null and estcutdate !='' )
                    and (CutCellid is not null and CutCellid !='' )
                    and id = '{0}' and mDivisionid = '{1}'
                order by FabricCombo,cutno", CurrentMaintain["ID"], keyWord);//找出空的cutref
            cutrefresult = DBProxy.Current.Select(null, cmdsql, out workordertmp);
            if (!cutrefresult)
            {
                ShowErr(cmdsql, cutrefresult);
                return;
            }
            DataTable workorderdt = ((DataTable)detailgridbs.DataSource);
            string maxref = MyUtility.GetValue.Lookup(string.Format("Select isnull(Max(cutref),'000000') from Workorder WITH (NOLOCK) where mDivisionid = '{0}'", keyWord)); //找最大Cutref
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
                        int maxno = Convert.ToInt32(wk.Compute("Max(cutno)", string.Format("FabricCombo ='{0}'", dr["FabricCombo"])));
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

        private void btnBatchAssignCellEstCutDate_Click(object sender, EventArgs e)
        {
            gridValid();
            grid.ValidateControl();
            var frm = new Sci.Production.Cutting.P02_BatchAssignCellCutDate((DataTable)(detailgridbs.DataSource));
            frm.ShowDialog(this);
        }

        //grid新增一筆的btn
        bool flag = false;
        protected override void OnDetailGridAppendClick()
        {
            flag = true;
            base.OnDetailGridAppendClick();
            this.detailgrid.SelectRowTo(0);
        }

        //grid插入的btn, override成複製功能
        protected override void OnDetailGridInsert(int index = -1)
        {
            DataTable table = (DataTable)this.detailgridbs.DataSource;
            DataRow newRow = table.NewRow();
            DataRow OldRow = CurrentDetailData == null ? newRow : CurrentDetailData;  //將游標停駐處的該筆資料複製起來
            //base.OnDetailGridInsert(index); //先給一個NewKey
            int maxkey;
            object comput = ((DataTable)detailgridbs.DataSource).Compute("Max(newkey)", "");
            if (comput == DBNull.Value) maxkey = 0;
            else maxkey = Convert.ToInt32(comput);
            maxkey = maxkey + 1;

            DataTable detailtmp = (DataTable)detailgridbs.DataSource;
            int TEMP = ((DataTable)detailgridbs.DataSource).Rows.Count;
            // 除Cutref, Cutno, Addname, AddDate, EditName, EditDate以外的所有欄位
            newRow["Newkey"] = maxkey;
            newRow["ID"] = OldRow["ID"];
            newRow["Type"] = OldRow["Type"];
            newRow["MDivisionId"] = OldRow["MDivisionId"];
            newRow["FactoryID"] = OldRow["FactoryID"];
            newRow["UKey"] = 0;
            newRow["SCIRefno"] = OldRow["SCIRefno"];
            newRow["FabricCombo"] = OldRow["FabricCombo"];
            newRow["FabricPanelCode"] = OldRow["FabricPanelCode"];
            //因按下新增也會進來這,但新增的btn不要複製全部
            if (flag || ((DataTable)this.detailgridbs.DataSource).Rows.Count <= 0)
            {
                //base.OnDetailGridInsert(index);
                if (OldRow["Type"].ToString() == "1")
                {
                    newRow["OrderID"] = OldRow["OrderID"];
                }
                else
                {
                    newRow["OrderID"] = OldRow["ID"];
                }
                if (index == -1) index = TEMP;
                OldRow.Table.Rows.InsertAt(newRow, index);
                flag = false;
                return;
            }
            newRow["OrderID"] = OldRow["OrderID"];
            newRow["SEQ1"] = OldRow["SEQ1"];
            newRow["SEQ2"] = OldRow["SEQ2"];
            //CutRef
            //newRow["Cutplanid"] = OldRow["Cutplanid"];
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
            newRow["MarkerNo"] = OldRow["MarkerNo"];
            newRow["MarkerVersion"] = OldRow["MarkerVersion"];
            //newRow["Addname"] = Sci.Env.User.UserName;
            //newRow["AddDate"] = DateTime.Now;
            //EditName
            //EditDate
            newRow["MarkerDownLoadID"] = OldRow["MarkerDownLoadID"];
            newRow["FabricCode"] = OldRow["FabricCode"];
            newRow["Order_EachconsUKey"] = OldRow["Order_EachconsUKey"];
            newRow["Article"] = OldRow["Article"];
            newRow["SizeCode"] = OldRow["SizeCode"];
            newRow["CutQty"] = OldRow["CutQty"];
            newRow["FabricPanelCode1"] = OldRow["FabricPanelCode1"];
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
            newRow["MarkerLengthY"] = OldRow["MarkerLengthY"];
            newRow["MarkerLengthE"] = OldRow["MarkerLengthE"];

            if (index == -1) index = TEMP;
            OldRow.Table.Rows.InsertAt(newRow, index);
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

            DataRow[] drTEMPdists = distqtyTb.Select(string.Format("WorkOrderUkey='{0}'", OldRow["ukey"].ToString()));
            foreach (DataRow drTEMP in drTEMPdists)
            {
                DataRow drNEW = distqtyTb.NewRow();
                drNEW["WorkOrderUkey"] = 0;  //新增WorkOrderUkey塞0
                drNEW["ID"] = drTEMP["ID"];
                drNEW["OrderID"] = drTEMP["OrderID"];
                drNEW["Article"] = drTEMP["Article"];
                drNEW["SizeCode"] = drTEMP["SizeCode"];
                drNEW["Qty"] = drTEMP["Qty"];
                drNEW["newkey"] = maxkey;
                distqtyTb.Rows.Add(drNEW);
            }

            DataRow[] drTEMPPP = PatternPanelTb.Select(string.Format("WorkOrderUkey='{0}'", OldRow["ukey"].ToString()));
            foreach (DataRow drTEMP in drTEMPPP)
            {
                DataRow drNEW = PatternPanelTb.NewRow();
                drNEW["WorkOrderUkey"] = 0;  //新增WorkOrderUkey塞0
                drNEW["PatternPanel"] = drTEMP["PatternPanel"];
                drNEW["FabricPanelCode"] = drTEMP["FabricPanelCode"];

                drNEW["newkey"] = maxkey;
                PatternPanelTb.Rows.Add(drNEW);
            }
            flag = false;
        }

        protected override void OnDetailGridDelete()
        {
            string ukey = CurrentDetailData["Ukey"].ToString() == "" ? "0" : CurrentDetailData["Ukey"].ToString();
            int NewKey = Convert.ToInt32(CurrentDetailData["NewKey"]);
            DataRow[] drar = sizeratioTb.Select(string.Format("WorkOrderUkey = '{0}' and NewKey = {1}", ukey, NewKey));
            foreach (DataRow dr in drar)
            {
                dr.Delete();
            }
            drar = distqtyTb.Select(string.Format("WorkOrderUkey = '{0}' and NewKey = {1}", ukey, NewKey));
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
            DataRow selectDr = ((DataRowView)gridSizeRatio.GetSelecteds(SelectedSort.Index)[0]).Row;
            selectDr.Delete();

            cal_TotalCutQty(CurrentDetailData["Ukey"], CurrentDetailData["NewKey"]);
        }

        #region MarkerLengt驗證/UnitCons/Cons計算
        private void numMarkerLengthY_MarkerLengthY_Validated(object sender, EventArgs e)
        {
            if (numMarkerLengthY.Text.Trim() == "") return;
            if (numMarkerLengthY.OldValue == numMarkerLengthY.Text) return;

            int y;
            y = int.Parse(numMarkerLengthY.Text);
            CurrentDetailData["MarkerLengthY"] = y.ToString("D2");
            cal_Cons(true, true);
        }

        private void txtMarkerLengthE_MarkerLengthE_Validating(object sender, CancelEventArgs e)
        {
            if (txtMarkerLengthE.OldValue == txtMarkerLengthE.Text) return;

            CurrentDetailData["MarkerLengthE"] = txtMarkerLengthE.Text;
            cal_Cons(true, true);
        }

        private void numUnitCons_UnitCons_Validated(object sender, EventArgs e)
        {
            //與marklength變更規則不一樣
            decimal cp = MyUtility.Convert.GetDecimal(CurrentDetailData["Conspc"]);
            decimal la = MyUtility.Convert.GetDecimal(CurrentDetailData["Layer"]);
            decimal ttsr = MyUtility.Convert.GetDecimal(sizeratioTb.Compute("Sum(Qty)", string.Format("WorkOrderUkey = '{0}' and newkey = '{1}'", CurrentDetailData["Ukey"], CurrentDetailData["newkey"])));
            CurrentDetailData["Cons"] = cp * la * ttsr;
        }

        private void cal_Cons(bool updateConsPC, bool updateCons) //update Cons
        {
            gridValid();
            if (numMarkerLengthY.Text.Trim() == "") return;

            int sizeRatioQty;
            object comput;
            comput = sizeratioTb.Compute("Sum(Qty)", string.Format("WorkOrderUkey = '{0}' and newkey = '{1}'", CurrentDetailData["Ukey"], CurrentDetailData["newkey"]));
            if (comput == DBNull.Value) sizeRatioQty = 0;
            else sizeRatioQty = Convert.ToInt32(comput);

            decimal MarkerLengthNum, Conspc;
            string MarkerLengthstr, lenY, lenE;
            if (MyUtility.Check.Empty(CurrentDetailData["MarkerLengthE"])) lenY = "0";
            else lenY = CurrentDetailData["MarkerLengthY"].ToString();
            if (MyUtility.Check.Empty(CurrentDetailData["MarkerLengthE"])) lenE = "0-0/0+0\"";
            else lenE = CurrentDetailData["MarkerLengthE"].ToString();
            MarkerLengthstr = lenY + "Y" + lenE;
            MarkerLengthNum = Convert.ToDecimal(MyUtility.GetValue.Lookup(string.Format("Select dbo.MarkerLengthToYDS('{0}')", MarkerLengthstr)));            
            if (sizeRatioQty == 0) Conspc = 0;
            else Conspc = MarkerLengthNum / sizeRatioQty;//Conspc = MarkerLength / SizeRatio Qty
            if (updateConsPC)
                CurrentDetailData["Conspc"] = Conspc;
            if (updateCons)
            {
                if (MyUtility.Check.Empty(CurrentDetailData["Layer"]))
                    CurrentDetailData["Cons"] = MarkerLengthNum * 0;
                else
                    CurrentDetailData["Cons"] = MarkerLengthNum * Convert.ToInt32(CurrentDetailData["Layer"]);
            }
            this.txtMarkerLength.Text = MarkerLengthstr;
            this.txtMarkerLength.ValidateControl();
        }
        #endregion

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
                    TotalCutQtystr = TotalCutQtystr + dr["SizeCode"].ToString().Trim() + "/" + (Convert.ToDecimal(dr["Qty"]) * Convert.ToDecimal(MyUtility.Check.Empty(CurrentDetailData["Layer"]) ? 0 : CurrentDetailData["Layer"])).ToString();
                }
                else
                {
                    TotalCutQtystr = TotalCutQtystr + "," + dr["SizeCode"].ToString().Trim() + "/" + (Convert.ToDecimal(dr["Qty"]) * Convert.ToDecimal(MyUtility.Check.Empty(CurrentDetailData["Layer"]) ? 0 : CurrentDetailData["Layer"])).ToString();
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
            DataRow selectDr = ((DataRowView)gridDistributetoSPNo.GetSelecteds(SelectedSort.Index)[0]).Row;
            selectDr.Delete();

            totalDisQty();
        }

        #region Save Before Post After
        protected override bool ClickSaveBefore()
        {
            gridValid();
            
            DataTable Dg = ((DataTable)detailgridbs.DataSource);
            for (int i = Dg.Rows.Count; i > 0; i--)
            {
                if (Dg.Rows[i - 1].RowState != DataRowState.Deleted)
                {
                    if (MyUtility.Check.Empty(Dg.Rows[i - 1]["MarkerName"]) || MyUtility.Check.Empty(Dg.Rows[i - 1]["Layer"]) ||
                        MyUtility.Check.Empty(Dg.Rows[i - 1]["SEQ1"]) || MyUtility.Check.Empty(Dg.Rows[i - 1]["SEQ2"]) ||
                        MyUtility.Convert.GetString(Dg.Rows[i - 1]["MarkerName"]) == "" ||
                        MyUtility.Convert.GetString(Dg.Rows[i - 1]["Layer"]) == "0" ||
                        MyUtility.Convert.GetString(Dg.Rows[i - 1]["SEQ1"]) == "" ||
                        MyUtility.Convert.GetString(Dg.Rows[i - 1]["SEQ2"]) == "")
                    {
                        Dg.Rows[i - 1].Delete();
                    }
                }
            }

            #region 刪除Qty 為0
            DataTable copyTb = sizeratioTb.Copy();
            DataRow[] deledr;
            foreach (DataRow dr in copyTb.Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    if (Convert.ToInt32(dr["Qty"]) == 0 || MyUtility.Check.Empty(dr["SizeCode"]))
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
                    if ((Convert.ToInt32(dr2["Qty"]) == 0 || MyUtility.Check.Empty(dr2["SizeCode"]) || MyUtility.Check.Empty(dr2["Article"])) && dr2["OrderID"].ToString().ToUpper() != "EXCESS")
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
            #region 檢查每一筆 Total distributionQty是否大於TotalCutQty總和
            foreach (DataRow dr_d in Dg.Rows)
            {
                if (dr_d.RowState == DataRowState.Deleted)
                {
                    continue;
                }
                decimal ttlcutqty = 0, ttldisqty = 0;
                DataRow[] sizedr = sizeratioTb.Select(string.Format("newkey = '{0}' and workorderUkey= '{1}'", dr_d["newkey"].ToString(), dr_d["Ukey"].ToString()));
                DataRow[] distdr = distqtyTb.Select(string.Format("newkey = '{0}' and workorderUkey= '{1}'", dr_d["newkey"].ToString(), dr_d["Ukey"].ToString()));
                ttlcutqty = sizedr.Sum(x => x.Field<decimal>("Qty")) * MyUtility.Convert.GetDecimal(dr_d["Layer"]);
                ttldisqty = distdr.Sum(x => x.Field<decimal>("Qty"));
                if (ttlcutqty<ttldisqty)
                {
                    ShowErr(string.Format("Key:{0} Distribution Qty can not exceed total Cut qty", dr_d["Ukey"].ToString()));
                    return false;
                }
            }
            #endregion
            CurrentMaintain["cutinline"]= ((DataTable)detailgridbs.DataSource).Compute("Min(estcutdate)", null);
            CurrentMaintain["CutOffLine"]= ((DataTable)detailgridbs.DataSource).Compute("MAX(estcutdate)", null);
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
                    updatesql = updatesql + string.Format("Update WorkOrder_SizeRatio set Qty = {0},SizeCode = '{4}' where WorkOrderUkey ={1} and SizeCode = '{2}' and id ='{3}';", dr["Qty"], dr["WorkOrderUkey"], dr["SizeCode", DataRowVersion.Original], cId, dr["SizeCode"]);
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
                    updatesql = updatesql + string.Format("Update WorkOrder_distribute set Qty = {0},SizeCode = '{6}' where WorkOrderUkey ={1} and SizeCode = '{2}' and Article = '{3}' and OrderID = '{4}' and ID ='{5}'; ", dr["Qty"], dr["WorkOrderUkey"], dr["SizeCode", DataRowVersion.Original], dr["Article"], dr["OrderID"], cId, dr["SizeCode"]);
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
                #region 新增
                if (dr.RowState == DataRowState.Added)
                {
                    insertsql = insertsql + string.Format("Insert into Workorder_PatternPanel(WorkOrderUkey,PatternPanel,FabricPanelCode,ID) values({0},'{1}','{2}','{3}');", dr["WorkOrderUkey"], dr["PatternPanel"], dr["FabricPanelCode"], cId);
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
            OnDetailEntered();
        }
        #endregion

        protected override bool ClickPrint()
        {
            Sci.Production.Cutting.P02_Print callNextForm;
            if (drTEMP != null)
            {
                callNextForm = new P02_Print(drTEMP, CurrentMaintain["ID"].ToString());
                callNextForm.ShowDialog(this);
            }
            else if (drTEMP == null && CurrentDetailData != null)
            {
                callNextForm = new P02_Print(CurrentDetailData, CurrentMaintain["ID"].ToString());
                callNextForm.ShowDialog(this);
            }
            else
            {
                MyUtility.Msg.InfoBox("No datas");
                return false;
            }

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
            this.detailgrid.SelectRowTo(0);
        }

        //Quantity Breakdown
        private void Qtybreak_Click(object sender, EventArgs e)
        {
            DataRow dr;
            MyUtility.Check.Seek(string.Format("select isnull([dbo].getPOComboList(o.ID,o.POID),'') as PoList from Orders o WITH (NOLOCK) where ID = '{0}'", CurrentMaintain["ID"]), out dr);
            Sci.Production.PPIC.P01_Qty callNextForm = new Sci.Production.PPIC.P01_Qty(MyUtility.Convert.GetString(CurrentMaintain["ID"]), MyUtility.Convert.GetString(CurrentMaintain["ID"]), dr["PoList"].ToString());
            callNextForm.ShowDialog(this);
        }

        //PatternPanel
        private void btnPatternPanel_Click(object sender, EventArgs e)
        {
            var dr = this.CurrentDetailData; if (null == dr) return;
            var frm = new Sci.Production.Cutting.P02_PatternPanel(!this.EditMode && MyUtility.Check.Empty(CurrentDetailData["Cutplanid"]), dr["Ukey"].ToString(), null, null, layersTb);
            frm.ShowDialog(this);
        }

        private void distribute_grid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void btn_Refresh_Click(object sender, EventArgs e)
        {
            this.RenewData();
            sorting(comboBox1.Text);  //避免順序亂掉
            this.OnDetailEntered(); ;
        }

        protected override void ClickUndo()
        {
            base.ClickUndo();
            RenewData();
            OnDetailEntered();
        }


        private void gridDistributetoSPNo_SelectionChanged(object sender, EventArgs e)
        {
            //更換qtybreakdown index
            DataRow SpNoRow = gridDistributetoSPNo.GetDataRow(gridDistributetoSPNo.GetSelectedRowIndex());
            if (MyUtility.Check.Empty(SpNoRow)) return;
            string Article = SpNoRow["Article"].ToString();
            string SizeCode = SpNoRow["SizeCode"].ToString();
            string SpNo = SpNoRow["Orderid"].ToString();
            int rowIndex = 0;

            if (!MyUtility.Check.Empty(distqtyTb) || distqtyTb.Rows.Count > 1)
            {
                for (int rIdx = 0; rIdx < gridQtyBreakdown.Rows.Count; rIdx++)
                {
                    DataGridViewRow dvr = gridQtyBreakdown.Rows[rIdx];
                    DataRow row = ((DataRowView)dvr.DataBoundItem).Row;
                    if (row["article"].ToString() == Article && row["SizeCode"].ToString() == SizeCode && row["id"].ToString() == SpNo)
                    {
                        rowIndex = rIdx;
                        break;
                    }
                }
                gridQtyBreakdown.SelectRowTo(rowIndex);
            }
        }

        /// <summary>
        /// 確認【布】寬是否會超過【裁桌】的寬度
        /// 若是 CutCell 取得寬度為 0 ，則不顯示訊息
        /// </summary>
        /// <param name="strCutCellID">CutCellID</param>
        /// <param name="strSCIRefno">SciRefno</param>
        private void checkCuttingWidth(string strCutCellID, string strSCIRefno)
        {
            string chkwidth = MyUtility.GetValue.Lookup(string.Format(@"
select width_cm = width*2.54 
from Fabric 
where SCIRefno = '{0}'", strSCIRefno));
            string strCuttingWidth = MyUtility.GetValue.Lookup(string.Format(@"
select cuttingWidth = isnull (cuttingWidth, 0) 
from CutCell 
where   id = '{0}'
        and MDivisionID = '{1}'", strCutCellID, Sci.Env.User.Keyword));
            if (!chkwidth.Empty() && !strCuttingWidth.Empty())
            {
                decimal width_CM = decimal.Parse(chkwidth);
                decimal decCuttingWidth = decimal.Parse(strCuttingWidth);
                if (decCuttingWidth == 0)
                {
                    return;
                }

                if (width_CM > decCuttingWidth)
                {
                    MyUtility.Msg.WarningBox(string.Format("fab width greater than cutting cell {0}, please check it.", strCutCellID));
                }
            }
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            gridValid();
            grid.ValidateControl();
            sorting(comboBox1.Text);
        }
    }
}
