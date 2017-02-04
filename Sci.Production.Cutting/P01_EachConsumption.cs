﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Data.SqlClient;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;


namespace Sci.Production.Cutting
{
    public partial class P01_EachConsumption : Sci.Win.Subs.Input4Plus
    {
        public P01_EachConsumption()
        {
            InitializeComponent();
        }
        private DataTable sizetb;
        private String _isCuttingPiece;
        private int _cuttingPiece;

        /// <summary>
        /// inner class.
        /// </summary>
        class SizeQtyInfo
        {
            public DataRow master;
            public DataTable sizeQty;
        }
        ITableSchema _sizeQtyTableschema;
        int _isSizeQtyGridCurrentChanging;
        /// <summary>
        /// Internal used.
        /// </summary>
        IDictionary<DataRow, SizeQtyInfo> _sizeQtyInfos = new Dictionary<DataRow, SizeQtyInfo>();
        /// <summary>
        /// Internal used.
        /// </summary>
        SizeQtyInfo _sizeQtyAttached;
        private bool isSizeQtyGridCurrentChanging { get { return 0 < _isSizeQtyGridCurrentChanging; } }
        private bool IsSizeQtyAttached { get { return null != _sizeQtyAttached; } }

        public P01_EachConsumption(bool canedit, string keyvalue1, string keyvalue2, string keyvalue3, bool cuttingPiece, bool switchToWorkorder)
            : base(canedit, keyvalue1, keyvalue2, keyvalue3)
        {
            InitializeComponent();
            this.Text = "Each Consumption - ("+keyvalue1+")";

            this._isCuttingPiece = "1";
            this._cuttingPiece = 0;
            if (cuttingPiece)
            {
                this._isCuttingPiece = "2";
                this._cuttingPiece = 1;
            }

            bool EditSwitch2Order = Prgs.GetAuthority(Sci.Env.User.UserID, "P01.Cutting Master List", "CanEdit");
            if (EditSwitch2Order && switchToWorkorder)
                button1.Enabled = true;
            else
                button1.Enabled = false;
        }

        private void autocolumns() 
        {
            this.grid.AutoResizeColumns();
            this.gridSizeQty.AutoResizeColumns();
            this.detailgrid.AutoResizeColumns();
        }

        protected override Ict.DualResult OnRequery(out DataTable datas)
        {
            //return base.OnRequery();
           
            datas = null;
            string sqlCmd =  string.Format(
            @"Select a.*, 
            case
	            when b.EachConsSource='O' then 'Original'
	            when b.EachConsSource='P' then 'Prophet'
	            when b.EachConsSource='' then ''
            END
            AS EachConsSource,

            d.PatternPanel, 

            a.Article as ForArticle,

            EC_Size.TotalQty , 
            concat(RTrim(Fabric.Refno),'-',Fabric.Description) as FabricDesc,
            Fabric.Width as FabricWidth 
            ,type2 = (case when  a.TYPE = 0 then ''
	           when a.TYPE = 1 then 'Cutting Piece'
	           when a.TYPE = 2 then 'TAPE'
	           end) 
            ,createby2 = concat(a.AddName,' ' ,FORMAT(a.AddDate,'yyyy/MM/dd HH:mm:ss'))
            ,editby2 = concat(a.EditName,' ' , FORMAT(a.EditDate,'yyyy/MM/dd HH:mm:ss'))

            From dbo.Order_EachCons a 
            Left Join dbo.Orders b On a.ID = b.ID  

            outer apply(
	            select (
		            select distinct PatternPanel+',' 
		            From dbo.Order_EachCons_PatternPanel as tmp 
		            Where tmp.Order_EachConsUkey = a.Ukey
		            for XML path('')
		            )
	            as PatternPanel
            ) d

            outer apply(select sum(Qty) as TotalQty from  dbo.Order_EachCons_SizeQty  where  a.Ukey = Order_EachConsUkey  ) as EC_Size 
            left join dbo.Order_BOF bof on bof.Id = a.Id and bof.FabricCode = a.FabricCode
            left join dbo.Fabric on Fabric.SCIRefno = bof.SCIRefno
            Where a.ID = '{0}' Order by a.Seq", KeyValue1);
            DualResult result;
            if (!(result = DBProxy.Current.Select(null, sqlCmd, out datas))) return result;

            sqlCmd = string.Format("Select * from Order_EachCons_SizeQty where id = '{0}'", KeyValue1);
            if (!(result = DBProxy.Current.Select(null, sqlCmd, out sizetb))) return result;

            gridSizeQty.DataSource = sizetb;
            return Result.True;
        }

        protected override bool OnGridSetup()
        {
            Helper.Controls.Grid.Generator(this.grid)
                .Text("Seq", header: "Seq", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("Markername", header: "Marker\r\n name", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("FabricCombo", header: "Fabric\r\n Combo", width: Widths.AnsiChars(2), iseditingreadonly: true);

            Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("ColorID", header: "Color ID", width: Widths.AnsiChars(8))
                .Text("SizeList", header: "Ratio", width: Widths.AnsiChars(16))
                .Numeric("OrderQty", header: "Order Q'ty", width: Widths.Numeric(6), decimal_places: 0)
                .Numeric("Layer", header: "Layer", width: Widths.Numeric(5), decimal_places: 0)
                .Numeric("CutQty", header: "Cut Q'ty", width: Widths.Numeric(6), decimal_places: 0)
                .Numeric("Variance", header: "Variance", width: Widths.Numeric(6),decimal_places: 0)
                .Numeric("YDS", header: "Cons.(YDS)", width: Widths.Numeric(6),  decimal_places: 2);

            Helper.Controls.Grid.Generator(this.gridSizeQty)
                .Text("SizeCode", header: "Size Code", width: Widths.AnsiChars(8))
                .Numeric("Qty", header: "Q'ty", width: Widths.Numeric(4), decimal_places: 0);
            this.gridSizeQty.Font = new Font("Arial", 9);
            autocolumns();
            return true;
        }


        protected override void OnGridRowChanged()
        {
            DualResult result;
            if (MyUtility.Check.Empty(CurrentData))
            {
                return;
            }
            sizetb.DefaultView.RowFilter =string.Format("Order_EachConsUkey = '{0}'", CurrentData["ukey"]);
            base.OnGridRowChanged();
        }

        private void btnDetail_Click(object sender, EventArgs e)
        {
            var dr = this.CurrentData; if (null == dr) return;
            var frm = new Sci.Production.Cutting.P01_EachConsumption_Detail(false, dr["ID"].ToString(), dr["ukey"].ToString(), detailgrid.GetDataRow(detailgrid.GetSelectedRowIndex())["colorid"].ToString());
            frm.ShowDialog(this);
        }

        private void btnDownloadIdList_Click(object sender, EventArgs e)
        {
            var dr = this.CurrentData; if (null == dr) return;
            var frm = new Sci.Production.Cutting.P01_EachConsumption_DownloadIdList(false, dr["ID"].ToString(), null, null);
            frm.ShowDialog(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var dr = this.CurrentData; if (null == dr) return;
            var frm = new Sci.Production.Cutting.P01_EachConsumption_SwitchWorkOrder(dr["ID"].ToString());
            frm.ShowDialog(this);
        }
    }
}
