﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci.Data;
using Sci;

namespace Sci.Production.Shipping
{
    public partial class P08_ShareExpense_ImportMaterial : Sci.Win.Subs.Base
    {
        DataTable detailData, gridData;
        DataRow apData;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;

        public P08_ShareExpense_ImportMaterial(DataTable DetailData, DataRow APDate)
        {
            InitializeComponent();
            detailData = DetailData;
            apData = APDate;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            //Grid設定
            this.grid1.IsEditingReadOnly = false;
            this.grid1.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)
                .Text("BLNo", header: "B/L No.", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("WKNo", header: "WK#/Fty WK#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("ShipModeID", header: "Shipping Mode", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("GW", header: "G.W.", decimal_places: 2, width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("CBM", header: "CBM", decimal_places: 2, width: Widths.AnsiChars(10), iseditingreadonly: true);
        }

        //Query
        private void button1_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(dateRange1.Value1) && MyUtility.Check.Empty(dateRange1.Value2) && MyUtility.Check.Empty(textBox1.Text) && 
                MyUtility.Check.Empty(textBox2.Text) && MyUtility.Check.Empty(textBox3.Text))
            {
                MyUtility.Msg.WarningBox("< Arrive Port Date > or < Invoice No. > or < B/L No. > or < WK No. > can not be empty!");
                dateRange1.TextBox1.Focus();
                return;
            }

            StringBuilder sqlCmd = new StringBuilder();
            #region 組SQL
            if (MyUtility.Convert.GetString(apData["Type"]) == "EXPORT")
            {
                #region FtyExport (Type = 3)
                sqlCmd.Append(@"select 0 as Selected,ID as WKNo,Blno,ShipModeID,WeightKg as GW, Cbm, ID as InvNo, '' as ShippingAPID, 
'' as Type, '' as CurrencyID, 0 as Amount, '' as ShareBase, 1 as FtyWK
from FtyExport WITH (NOLOCK) 
where Type = 3 ");
                if (!MyUtility.Check.Empty(dateRange1.Value1))
                {
                    sqlCmd.Append(string.Format(" and PortArrival >= '{0}' ", Convert.ToDateTime(dateRange1.Value1).ToString("d")));
                }

                if (!MyUtility.Check.Empty(dateRange1.Value2))
                {
                    sqlCmd.Append(string.Format(" and PortArrival <= '{0}' ", Convert.ToDateTime(dateRange1.Value2).ToString("d")));
                }

                if (!MyUtility.Check.Empty(textBox1.Text))
                {
                    sqlCmd.Append(string.Format(" and INVNo = '{0}' ", textBox1.Text));
                }

                if (!MyUtility.Check.Empty(textBox2.Text))
                {
                    sqlCmd.Append(string.Format(" and BLNo = '{0}' ", textBox2.Text));
                }

                if (!MyUtility.Check.Empty(textBox3.Text))
                {
                    sqlCmd.Append(string.Format(" and ID = '{0}' ", textBox3.Text));
                }
                #endregion
            }
            else
            {
                #region Export, FtyExport(Type != 3)
                if (MyUtility.Convert.GetString(apData["SubType"]) == "MATERIAL" || MyUtility.Convert.GetString(apData["SubType"]).ToUpper() == "OTHER")
                {
                    sqlCmd.Append(@"with ExportData 
as 
(select 0 as Selected,ID as WKNo,Blno,ShipModeID,WeightKg as GW, Cbm, '' as InvNo, '' as ShippingAPID, 
 '' as Type, '' as CurrencyID, 0 as Amount, '' as ShareBase, 0 as FtyWK
 from Export WITH (NOLOCK) 
 where 1 = 1 ");
                    if (!MyUtility.Check.Empty(dateRange1.Value1))
                    {
                        sqlCmd.Append(string.Format(" and PortArrival >= '{0}' ", Convert.ToDateTime(dateRange1.Value1).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(dateRange1.Value2))
                    {
                        sqlCmd.Append(string.Format(" and PortArrival <= '{0}' ", Convert.ToDateTime(dateRange1.Value2).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(textBox1.Text))
                    {
                        sqlCmd.Append(string.Format(" and INVNo = '{0}' ", textBox1.Text));
                    }

                    if (!MyUtility.Check.Empty(textBox2.Text))
                    {
                        sqlCmd.Append(string.Format(" and BLNo = '{0}' ", textBox2.Text));
                    }

                    if (!MyUtility.Check.Empty(textBox3.Text))
                    {
                        sqlCmd.Append(string.Format(" and ID = '{0}' ", textBox3.Text));
                    }
                    sqlCmd.Append("), ");
                }
                else
                {
                    sqlCmd.Append(@"with ExportData 
as 
(select 0 as Selected,ID as WKNo,Blno,ShipModeID,WeightKg as GW, Cbm, '' as InvNo, '' as ShippingAPID, 
 '' as Type, '' as CurrencyID, 0 as Amount, '' as ShareBase, 0 as FtyWK
 from Export WITH (NOLOCK) where 1 = 0), ");
                }

                if (MyUtility.Convert.GetString(apData["SubType"]) == "SISTER FACTORY TRANSFER" || MyUtility.Convert.GetString(apData["SubType"]).ToUpper() == "OTHER")
                {
                    sqlCmd.Append(@"FtyExportData 
as 
(select 0 as Selected,ID as WKNo,Blno,ShipModeID,WeightKg as GW, Cbm, ID as InvNo, '' as ShippingAPID, 
 '' as Type, '' as CurrencyID, 0 as Amount, '' as ShareBase, 1 as FtyWK
 from FtyExport WITH (NOLOCK) 
 where Type <> 3 ");
                    if (!MyUtility.Check.Empty(dateRange1.Value1))
                    {
                        sqlCmd.Append(string.Format(" and PortArrival >= '{0}' ", Convert.ToDateTime(dateRange1.Value1).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(dateRange1.Value2))
                    {
                        sqlCmd.Append(string.Format(" and PortArrival <= '{0}' ", Convert.ToDateTime(dateRange1.Value2).ToString("d")));
                    }

                    if (!MyUtility.Check.Empty(textBox1.Text))
                    {
                        sqlCmd.Append(string.Format(" and INVNo = '{0}' ", textBox1.Text));
                    }

                    if (!MyUtility.Check.Empty(textBox2.Text))
                    {
                        sqlCmd.Append(string.Format(" and BLNo = '{0}' ", textBox2.Text));
                    }

                    if (!MyUtility.Check.Empty(textBox3.Text))
                    {
                        sqlCmd.Append(string.Format(" and ID = '{0}' ", textBox3.Text));
                    }
                    sqlCmd.Append(") ");
                }
                else
                {
                    sqlCmd.Append(@"FtyExportData 
as 
(select 0 as Selected,ID as WKNo,Blno,ShipModeID,WeightKg as GW, Cbm, '' as InvNo, '' as ShippingAPID, 
 '' as Type, '' as CurrencyID, 0 as Amount, '' as ShareBase, 1 as FtyWK
 from FtyExport WITH (NOLOCK) 
 where 1=0 ) ");
                }

                sqlCmd.Append(@"select * from ExportData 
union all 
select * from FtyExportData");
                #endregion
            }
            #endregion

            DualResult result = DBProxy.Current.Select(null, sqlCmd.ToString(), out gridData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query fail!\r\n" + result.ToString());
            }
            else
            {
                if (gridData.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                }
            }
            listControlBindingSource1.DataSource = gridData;
        }

        //Import
        private void button2_Click(object sender, EventArgs e)
        {
            this.grid1.ValidateControl();
            listControlBindingSource1.EndEdit();
            gridData = (DataTable)listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(gridData)|| gridData.Rows.Count > 0)
            {
                DataRow[] dr = gridData.Select("Selected = 1");
                if (dr.Length > 0)
                {
                    foreach (DataRow currentRow in dr)
                    {
                        DataRow[] findrow = detailData.Select(string.Format("BLNo = '{0}' and WKNo = '{1}' and InvNo = '{2}'", MyUtility.Convert.GetString(currentRow["BLNo"]), MyUtility.Convert.GetString(currentRow["WKNo"]), MyUtility.Convert.GetString(currentRow["InvNo"])));
                        if (findrow.Length == 0)
                        {
                            currentRow.AcceptChanges();
                            currentRow.SetAdded();
                            detailData.ImportRow(currentRow);
                        }
                        else
                        {
                            findrow[0]["GW"] = MyUtility.Convert.GetDecimal(currentRow["GW"]);
                            findrow[0]["CBM"] = MyUtility.Convert.GetDecimal(currentRow["CBM"]);
                            findrow[0]["ShipModeID"] = MyUtility.Convert.GetString(currentRow["ShipModeID"]);
                        }
                    }
                }
            }
            MyUtility.Msg.InfoBox("Import finished!");
        }
    }
}
