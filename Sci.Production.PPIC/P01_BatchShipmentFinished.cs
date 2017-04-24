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
using System.Transactions;
using System.Linq;
using System.Runtime.InteropServices;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.PPIC
{
    public partial class P01_BatchShipmentFinished : Sci.Win.Subs.Base
    {
        public bool haveupdate = false;
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        public P01_BatchShipmentFinished()
        {
            InitializeComponent();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            DataTable GridData;
            string sqlCmd = string.Format(@"with wantToClose
as
(
select distinct POID
from Orders o WITH (NOLOCK) 
where o.Finished = 0 
and o.MDivisionID = '{0}'
and (o.Junk = 1 or o.PulloutComplete = 1)
and (o.Category = 'B' or o.Category = 'S')
),
canNotClose
as
(
select distinct POID
from Orders o WITH (NOLOCK) 
where o.Finished = 0 
and o.MDivisionID = '{0}'
and o.PulloutComplete = 0
and o.Junk = 0
and (o.Category = 'B' or o.Category = 'S')
)
select 1 as Selected,a.POID,isnull(o.StyleID,'') as StyleID,isnull(b.BuyerID,'') as BuyerID,o.BuyerDelivery,[dbo].getPOComboList(a.POID,a.POID) as POCombo,(o.MCHandle+' - '+isnull(p.Name,'')) as MCHandle
from (select * from wantToClose
	  except
	  select * from canNotClose) a
left join Orders o WITH (NOLOCK) on a.POID = o.ID
left join Brand b WITH (NOLOCK) on o.BrandID = b.ID
left join Pass1 p WITH (NOLOCK) on p.ID = o.MCHandle", Sci.Env.User.Keyword);
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out GridData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query data fail!!" + result.ToString());
            }
            listControlBindingSource1.DataSource = GridData;

            //設定Grid1的顯示欄位
            this.grid1.IsEditingReadOnly = false;
            this.grid1.DataSource = listControlBindingSource1;
            Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)
                .Text("POID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("StyleID", header: "Style#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("BuyerID", header: "Buyer", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("BuyerDelivery", header: "Buyer Delivery", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("POCombo", header: "PO Combo", width: Widths.AnsiChars(50), iseditingreadonly: true);
        }

        private void setFilter()
        {
            StringBuilder stringFilter = new StringBuilder();
            stringFilter.Append("1=1");
            if (!MyUtility.Check.Empty(textBox1.Text))
            {
                stringFilter.Append(string.Format(" and StyleID = '{0}'", textBox1.Text));
            }
            if (!MyUtility.Check.Empty(textBox2.Text))
            {
                stringFilter.Append(string.Format(" and BuyerID = '{0}'", textBox2.Text));
            }

            if (!MyUtility.Check.Empty(dateRange1.Value1))
            {
                stringFilter.Append(string.Format(" and BuyerDelivery >= '{0}'", dateRange1.Value1));
            }

            if (!MyUtility.Check.Empty(dateRange1.Value2))
            {
                stringFilter.Append(string.Format(" and BuyerDelivery <= '{0}'", dateRange1.Value2));
            }

            ((DataTable)listControlBindingSource1.DataSource).DefaultView.RowFilter = stringFilter.ToString();
        }

        //Style#
        private void textBox1_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Tools.SelectItem item;
            string sqlCmd = "select ID,SeasonID,Description,BrandID from Style WITH (NOLOCK) where Junk = 0 order by ID";
            item = new Sci.Win.Tools.SelectItem(sqlCmd, "16,8,35,10@760,500", this.Text);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            textBox1.Text = item.GetSelectedString();
            setFilter();
        }

        //Buyer
        private void textBox2_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlCmd = "SELECT Id,NameCH,NameEN FROM Brand WITH (NOLOCK) WHERE Junk=0  ORDER BY Id";
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "10,30,30@755,500", this.Text, false, ",");

            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            textBox2.Text = item.GetSelectedString();
            setFilter();
        }

        //Style#
        private void textBox1_Validated(object sender, EventArgs e)
        {
            if (textBox1.OldValue != textBox1.Text)
            {
                if (textBox1.Text != "")
                {
                    if (textBox1.Text.IndexOf("'") != -1)
                    {
                        textBox1.Text = "";
                        MyUtility.Msg.WarningBox("Input errror!!");
                        return;
                    }
                    if (!MyUtility.Check.Seek(string.Format("select ID from Style WITH (NOLOCK) where Junk = 0 and ID = '{0}'", textBox1.Text)))
                    {
                        textBox1.Text = "";
                        MyUtility.Msg.WarningBox("Style not found!!");
                        return;
                    }
                }
                setFilter();
            }
        }

        //Buyer
        private void textBox2_Validated(object sender, EventArgs e)
        {
            if (textBox2.OldValue != textBox2.Text)
            {
                if (textBox2.Text != "")
                {
                    if (textBox2.Text.IndexOf("'") != -1)
                    {
                        textBox2.Text = "";
                        MyUtility.Msg.WarningBox("Input errror!!");
                        return;
                    }
                    if (!MyUtility.Check.Seek(string.Format("select ID from Brand WITH (NOLOCK) where Junk = 0 and ID = '{0}'", textBox2.Text)))
                    {
                        textBox2.Text = "";
                        MyUtility.Msg.WarningBox("Brand not found!!");
                        return;
                    }
                }
                setFilter();
            }
        }

        //Buyer Delivery
        private void dateRange1_Validated(object sender, EventArgs e)
        {
            if (dateRange1.Value1 != dateRange1.OldValue1)
            {
                setFilter();
            }
            if (dateRange1.Value2 != dateRange1.OldValue2)
            {
                setFilter();
            }
        }

        //update
        private void button2_Click(object sender, EventArgs e)
        {
            this.grid1.ValidateControl();
            this.grid1.EndEdit();
            listControlBindingSource1.EndEdit();
            DataTable detailData = (DataTable)listControlBindingSource1.DataSource;
            DataRow[] dr = detailData.Select("Selected = 1");
            if (dr.Length <= 0)
            {
                MyUtility.Msg.WarningBox("Please select at least one data!");
                return;
            }

            #region 更新Orders, Chgover資料
            IList<string> updateCmds = new List<string>();
            //只存畫面上看到的那幾筆資料
            foreach (DataRowView currentRowView in detailData.DefaultView)
            {
                DataRow currentRow = currentRowView.Row;
                if (currentRow["Selected"].ToString() == "1")
                {
                    updateCmds.Add(string.Format("exec [dbo].usp_closeOrder '{0}','1'", currentRowView["POID"].ToString()));
                }
            }

            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    DualResult result = DBProxy.Current.Executes(null, updateCmds);

                    if (result)
                    {
                        transactionScope.Complete();
                        transactionScope.Dispose();
                        haveupdate = true;
                        MyUtility.Msg.InfoBox("Update completed!");                      
                    }
                    else
                    {
                        transactionScope.Dispose();
                        MyUtility.Msg.WarningBox("Update failed, Pleaes re-try"+result.ToString());
                        return;
                    }
                }
                catch (Exception ex)
                {
                    transactionScope.Dispose();
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            #endregion
            DataTable GridData;
            string sqlCmd = string.Format(@"with wantToClose
as
(
select distinct POID
from Orders o WITH (NOLOCK) 
where o.Finished = 0 
and o.MDivisionID = '{0}'
and (o.Junk = 1 or o.PulloutComplete = 1)
and (o.Category = 'B' or o.Category = 'S')
),
canNotClose
as
(
select distinct POID
from Orders o WITH (NOLOCK) 
where o.Finished = 0 
and o.MDivisionID = '{0}'
and o.PulloutComplete = 0
and o.Junk = 0
and (o.Category = 'B' or o.Category = 'S')
)
select 1 as Selected,a.POID,isnull(o.StyleID,'') as StyleID,isnull(b.BuyerID,'') as BuyerID,o.BuyerDelivery,[dbo].getPOComboList(a.POID,a.POID) as POCombo,(o.MCHandle+' - '+isnull(p.Name,'')) as MCHandle
from (select * from wantToClose
	  except
	  select * from canNotClose) a
left join Orders o WITH (NOLOCK) on a.POID = o.ID
left join Brand b WITH (NOLOCK) on o.BrandID = b.ID
left join Pass1 p WITH (NOLOCK) on p.ID = o.MCHandle", Sci.Env.User.Keyword);
            DualResult Renewresult = DBProxy.Current.Select(null, sqlCmd, out GridData);
            if (!Renewresult)
            {
                MyUtility.Msg.ErrorBox("Query data fail!!" + Renewresult.ToString());
            }
            listControlBindingSource1.DataSource = GridData;
            setFilter();
        }

        //To Excel
        private void button1_Click(object sender, EventArgs e)
        {
            DataTable GridData = (DataTable)listControlBindingSource1.DataSource;
            if (GridData.DefaultView.Count <= 0)
            {
                MyUtility.Check.Empty("No data!!");
                return;
            }

            string strXltName = Sci.Env.Cfg.XltPathDir + "\\PPIC_P01_BatchShipmentFinished.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null) return;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            int intRowsStart = 2;
            int dataRowCount = GridData.DefaultView.Count;
            int rownum = 0;
            object[,] objArray = new object[1, 6];
            foreach (DataRowView dr in GridData.DefaultView)
            {
                objArray[0, 0] = dr["POID"];
                objArray[0, 1] = dr["StyleID"];
                objArray[0, 2] = dr["BuyerID"];
                objArray[0, 3] = dr["BuyerDelivery"];
                objArray[0, 4] = dr["POCombo"];
                objArray[0, 5] = dr["MCHandle"];

                worksheet.Range[String.Format("A{0}:F{0}", intRowsStart+rownum)].Value2 = objArray;
                rownum++;
            }

            excel.Visible = true;
        }
    }
}
