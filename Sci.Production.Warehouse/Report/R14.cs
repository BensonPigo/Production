﻿using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class R14 : Win.Tems.PrintForm
    {
        private string factory;
        private string fabrictype;
        private string ordertype;
        private int ordertypeindex;
        private DateTime? eta1;
        private DateTime? eta2;
        private DataTable printData;
        private StringBuilder condition = new StringBuilder();

        /// <inheritdoc/>
        public R14(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.txtfactory.Text = Env.User.Factory;
            MyUtility.Tool.SetupCombox(this.comboFabricType, 2, 1, ",ALL,F,Fabric,A,Accessory");
            this.comboFabricType.SelectedIndex = 0;
            this.txtdropdownlistOrderType.SelectedIndex = 0;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            if (MyUtility.Check.Empty(this.dateWKNoETA.Value1) && MyUtility.Check.Empty(this.dateWKNoETA.Value2))
            {
                MyUtility.Msg.WarningBox("< WK# ETA > can't be empty!!");
                return false;
            }

            this.eta1 = this.dateWKNoETA.Value1;
            this.eta2 = this.dateWKNoETA.Value2;
            this.ordertypeindex = this.txtdropdownlistOrderType.SelectedIndex;
            this.fabrictype = this.comboFabricType.SelectedValue.ToString();
            this.factory = this.txtfactory.Text;
            switch (this.ordertypeindex)
            {
                case 0:
                    this.ordertype = "('B')";
                    break;
                case 1:
                    this.ordertype = "('S')";
                    break;
                case 2:
                    this.ordertype = "('M')";
                    break;
                case 3:
                    this.ordertype = "('B','S')";
                    break;
                case 4:
                    this.ordertype = "('B','S')";
                    break;
                case 5:
                    this.ordertype = "('B','S','M')";
                    break;
            }

            this.condition.Clear();
            this.condition.Append(string.Format(
                @"ETA : {0} ~ {1}" + Environment.NewLine,
                Convert.ToDateTime(this.eta1).ToString("yyyy/MM/dd"),
                Convert.ToDateTime(this.eta2).ToString("yyyy/MM/dd")));
            this.condition.Append(string.Format(
                @"Material Type : {0}" + Environment.NewLine,
                this.comboFabricType.Text));
            this.condition.Append(string.Format(
                @"Factory : {0}" + Environment.NewLine,
                this.txtfactory.Text));
            this.condition.Append(string.Format(
                @"Order Type : {0}" + Environment.NewLine,
                this.txtdropdownlistOrderType.Text));
            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            #region -- sql parameters declare --

            System.Data.SqlClient.SqlParameter sp_factory = new System.Data.SqlClient.SqlParameter();
            sp_factory.ParameterName = "@factory";

            IList<System.Data.SqlClient.SqlParameter> cmds = new List<System.Data.SqlClient.SqlParameter>();
            #endregion

            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append($@"
select  d.FactoryID
        ,wkno_a = b.id 
        ,order_a = b.poid
        ,seq_a = b.seq1 + b.seq2 
        ,b.refno 
        ,ColorID = isnull(psdsC.SpecValue, '')
        ,SizeSpec= isnull(psdsS.SpecValue, '')
        ,psd.StockUnit
        ,shipqty = v.ShipQty
        ,over1 = iif (v.ShipQty > isnull(x.qty,0),'V','') 
        ,received_qty = isnull(x.qty,0) 
        ,over2 = iif (v.ShipQty < isnull(x.qty,0),'V','') 
from dbo.export A WITH (NOLOCK) 
inner join dbo.export_detail B WITH (NOLOCK) ON B.ID = A.ID
inner join dbo.PO_Supp_Detail psd WITH (NOLOCK) on psd.ID = b.PoID and psd.seq1 = b.seq1 and psd.seq2 =  b.seq2 
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
left join PO_Supp_Detail_Spec psdsS WITH (NOLOCK) on psdsS.ID = psd.id and psdsS.seq1 = psd.seq1 and psdsS.seq2 = psd.seq2 and psdsS.SpecColumnID = 'Size'
outer apply (
    select ShipQty = Round(dbo.GetUnitQty(psd.POUnit, psd.StockUnit, (b.qty + b.foc)), 2)
) v
outer apply ( 
    select sum(b1.StockQty) qty 
    from dbo.Receiving a1 WITH (NOLOCK) 
    inner join dbo.Receiving_Detail b1 WITH (NOLOCK) on b1.id = a1.Id
    where a1.ExportId = a.id and b1.PoId = b.PoID and b1.seq1 = b.seq1 and b1.seq2 = b.seq2 and a1.Status = 'Confirmed'
    ) x
inner join dbo.orders d WITH (NOLOCK) on d.id = b.poid
WHERE  D.Category in {this.ordertype}");

            if (!MyUtility.Check.Empty(this.eta1))
            {
                sqlCmd.Append(string.Format(" and '{0}' <= a.eta", Convert.ToDateTime(this.eta1).ToString("yyyy/MM/dd")));
            }

            if (!MyUtility.Check.Empty(this.eta2))
            {
                sqlCmd.Append(string.Format(" and a.eta <= '{0}'", Convert.ToDateTime(this.eta2).ToString("yyyy/MM/dd")));
            }
            #region --- 條件組合  ---
            if (!MyUtility.Check.Empty(this.factory))
            {
                sqlCmd.Append(" and d.factoryid = @factory");
                sp_factory.Value = this.factory;
                cmds.Add(sp_factory);
            }

            if (!MyUtility.Check.Empty(this.fabrictype))
            {
                sqlCmd.Append(string.Format(@" and psd.fabrictype = '{0}'", this.fabrictype));
            }

            sqlCmd.Append(" order by b.id, b.poid, b.seq1, b.seq2, b.refno, ColorID, sizeSpec, psd.stockunit");
            #endregion

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), cmds, out this.printData);
            if (!result)
            {
                DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                return failResult;
            }

            return Ict.Result.True;
        }

        // 產生Excel

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            this.SetCount(this.printData.Rows.Count);

            if (this.printData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Warehouse_R14.xltx"); // 預先開啟excel app
            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1];   // 取得工作表
            objSheets.Cells[1, 1] = this.condition.ToString();   // 條件字串寫入excel
            MyUtility.Excel.CopyToXls(this.printData, string.Empty, "Warehouse_R14.xltx", 2, true, null, objApp);      // 將datatable copy to excel

            Marshal.ReleaseComObject(objSheets);
            return true;
        }
    }
}
