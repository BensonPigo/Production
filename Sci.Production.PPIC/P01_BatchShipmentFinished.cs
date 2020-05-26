using System;
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
    /// <summary>
    /// P01_BatchShipmentFinished
    /// </summary>
    public partial class P01_BatchShipmentFinished : Sci.Win.Subs.Base
    {
        private bool haveupdate = false;

        /// <summary>
        /// Haveupdate
        /// </summary>
        public bool Haveupdate
        {
            get
            {
                return this.haveupdate;
            }

            set
            {
                this.haveupdate = value;
            }
        }

        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;

        /// <summary>
        /// P01_BatchShipmentFinished
        /// </summary>
        public P01_BatchShipmentFinished()
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.ReLoadDatas();

            // 設定Grid1的顯示欄位
            this.gridBatchShipmentFinished.IsEditingReadOnly = false;
            this.gridBatchShipmentFinished.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridBatchShipmentFinished)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                .Text("POID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("StyleID", header: "Style#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("BuyerID", header: "Buyer", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Date("BuyerDelivery", header: "Buyer Delivery", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("POCombo", header: "PO Combo", width: Widths.AnsiChars(50), iseditingreadonly: true);
        }

        // Style#
        private void TxtStyle_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Tools.SelectItem item;
            string sqlCmd = "select ID,SeasonID,Description,BrandID from Style WITH (NOLOCK) where Junk = 0 order by ID";
            item = new Sci.Win.Tools.SelectItem(sqlCmd, "16,8,35,10@760,500", this.Text);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.txtStyle.Text = item.GetSelectedString();
            this.SetFilter();
        }

        // Buyer
        private void TxtBuyer_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlCmd = "SELECT Id,NameCH,NameEN FROM Brand WITH (NOLOCK) WHERE Junk=0  ORDER BY Id";
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "10,30,30@755,500", this.Text, false, ",");

            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.txtBuyer.Text = item.GetSelectedString();
            this.SetFilter();
        }

        // Style#
        private void TxtStyle_Validated(object sender, EventArgs e)
        {
            if (this.txtStyle.OldValue != this.txtStyle.Text)
            {
                if (this.txtStyle.Text != string.Empty)
                {
                    if (this.txtStyle.Text.IndexOf("'") != -1)
                    {
                        this.txtStyle.Text = string.Empty;
                        MyUtility.Msg.WarningBox("Input errror!!");
                        return;
                    }

                    if (!MyUtility.Check.Seek(string.Format("select ID from Style WITH (NOLOCK) where Junk = 0 and ID = '{0}'", this.txtStyle.Text)))
                    {
                        this.txtStyle.Text = string.Empty;
                        MyUtility.Msg.WarningBox("Style not found!!");
                        return;
                    }
                }

                this.SetFilter();
            }
        }

        // Buyer
        private void TxtBuyer_Validated(object sender, EventArgs e)
        {
            if (this.txtBuyer.OldValue != this.txtBuyer.Text)
            {
                if (this.txtBuyer.Text != string.Empty)
                {
                    if (this.txtBuyer.Text.IndexOf("'") != -1)
                    {
                        this.txtBuyer.Text = string.Empty;
                        MyUtility.Msg.WarningBox("Input errror!!");
                        return;
                    }

                    if (!MyUtility.Check.Seek(string.Format("select ID from Brand WITH (NOLOCK) where Junk = 0 and ID = '{0}'", this.txtBuyer.Text)))
                    {
                        this.txtBuyer.Text = string.Empty;
                        MyUtility.Msg.WarningBox("Brand not found!!");
                        return;
                    }
                }

                this.SetFilter();
            }
        }

        // Buyer Delivery
        private void DateBuyerDelivery_Validated(object sender, EventArgs e)
        {
            if (this.dateBuyerDelivery.Value1 != this.dateBuyerDelivery.OldValue1)
            {
                this.SetFilter();
            }

            if (this.dateBuyerDelivery.Value2 != this.dateBuyerDelivery.OldValue2)
            {
                this.SetFilter();
            }
        }

        // update
        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            this.gridBatchShipmentFinished.ValidateControl();
            this.gridBatchShipmentFinished.EndEdit();
            this.listControlBindingSource1.EndEdit();
            DataTable detailData = (DataTable)this.listControlBindingSource1.DataSource;
            DataRow[] dr = detailData.Select("Selected = 1");
            if (dr.Length <= 0)
            {
                MyUtility.Msg.WarningBox("Please select at least one data!");
                return;
            }

            string sqlCmds;
            foreach (DataRow item in dr)
            {
                if (MyUtility.Convert.GetString(item["Category"]) == "M")
                {
                    if (MyUtility.Check.Seek(string.Format("select ID from PO WITH (NOLOCK) where ID = '{0}' and Complete = 1", MyUtility.Convert.GetString(item["POID"]))) == false)
                    {
                        sqlCmds = string.Format(
                            @"
select A.ID
from PO_Supp_Detail A WITH (NOLOCK) 
left join MDivisionPoDetail B WITH (NOLOCK) on B.POID = A.ID 
											   and B.Seq1 = A.SEQ1 
											   and B.Seq2 = A.SEQ2
inner join dbo.Factory F WITH (NOLOCK) on F.id = A.factoryid 
										  and F.MDivisionID = '{0}'
where A.ID = '{1}' 
	  and (A.Complete = 0 or B.InQty <> B.OutQty - B.AdjustQty)",
                            Sci.Env.User.Keyword,
                            item["POID"]);

                        if (MyUtility.Check.Seek(sqlCmds))
                        {
                            this.ShowWarning(string.Format("SP#:{0}，Warehouse still have material, so can't finish shipment.", MyUtility.Convert.GetString(item["POID"])));
                            return;
                        }
                    }
                }
                else
                {
                    sqlCmds = string.Format(
                        @"
select SP = (select ID + ',' 
			 from Orders WITH (NOLOCK) 
			 where POID = '{0}' 
			 	   and (Junk=0 or (Junk=1 and NeedProduction=1))
			 	   and PulloutComplete = 0 
                   and Junk = 0
	 	     for xml path(''))",
                        MyUtility.Convert.GetString(item["POID"]));

                    string spList = MyUtility.GetValue.Lookup(sqlCmds);
                    if (!MyUtility.Check.Empty(spList))
                    {
                        this.ShowWarning(string.Format("SP#:{0}，Below combined SP# not yet ship!!\r\n", MyUtility.Convert.GetString(item["POID"])) + spList.Substring(0, spList.Length - 1));
                        return;
                    }
                }

                if (MyUtility.Convert.GetString(item["Category"]) == "A")
                {
                    DualResult resultCheckOrderCategoryTypeA = P01_Utility.CheckOrderCategoryTypeA(item["POID"].ToString());
                    if (!resultCheckOrderCategoryTypeA)
                    {
                        MyUtility.Msg.WarningBox(resultCheckOrderCategoryTypeA.Description);
                        return;
                    }
                }
            }

            #region 更新Orders, Chgover資料
            IList<string> updateCmds = new List<string>();

            // 只存畫面上看到的那幾筆資料
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
                        this.Haveupdate = true;
                        MyUtility.Msg.InfoBox("Update completed!");
                    }
                    else
                    {
                        transactionScope.Dispose();
                        MyUtility.Msg.WarningBox("Update failed, Pleaes re-try" + result.ToString());
                        return;
                    }
                }
                catch (Exception ex)
                {
                    transactionScope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            #endregion

            this.ReLoadDatas();
            this.SetFilter();
        }

        // To Excel
        private void BtnToExcel_Click(object sender, EventArgs e)
        {
            DataTable gridData = (DataTable)this.listControlBindingSource1.DataSource;
            if (gridData.DefaultView.Count <= 0)
            {
                MyUtility.Check.Empty("No data!!");
                return;
            }

            string strXltName = Sci.Env.Cfg.XltPathDir + "\\PPIC_P01_BatchShipmentFinished.xltx";
            Microsoft.Office.Interop.Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                return;
            }

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];
            int intRowsStart = 2;
            int dataRowCount = gridData.DefaultView.Count;
            int rownum = 0;
            object[,] objArray = new object[1, 6];
            foreach (DataRowView dr in gridData.DefaultView)
            {
                objArray[0, 0] = dr["POID"];
                objArray[0, 1] = dr["StyleID"];
                objArray[0, 2] = dr["BuyerID"];
                objArray[0, 3] = dr["BuyerDelivery"];
                objArray[0, 4] = dr["POCombo"];
                objArray[0, 5] = dr["MCHandle"];

                worksheet.Range[string.Format("A{0}:F{0}", intRowsStart + rownum)].Value2 = objArray;
                rownum++;
            }

            #region Save & Show Excel
            string strExcelName = Sci.Production.Class.MicrosoftFile.GetName("PPIC_P01_BatchShipmentFinished");
            Microsoft.Office.Interop.Excel.Workbook workbook = excel.ActiveWorkbook;
            workbook.SaveAs(strExcelName);
            workbook.Close();
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);
            Marshal.ReleaseComObject(workbook);

            strExcelName.OpenFile();
            #endregion
        }

        private void ReLoadDatas()
        {
            DataTable gridData;
            string sqlCmd = string.Format(
                @"
select *
into #wantToClose_step1
from (
	select distinct POID
	from Orders o WITH (NOLOCK) 
	where o.Finished = 0 
		  and o.MDivisionID = '{0}'
		  and (o.Junk = 1 or o.PulloutComplete = 1)
		  and (o.Category = 'B' or o.Category = 'S' or o.Category = 'A')
	union all
	select distinct A.ID
	from PO_Supp_Detail A WITH (NOLOCK) 
	left join MDivisionPoDetail B WITH (NOLOCK) on B.POID = A.ID 
												   and B.Seq1 = A.SEQ1 
												   and B.Seq2 = A.SEQ2
	inner join dbo.Factory F WITH (NOLOCK) on F.id = A.factoryid 
											  and F.MDivisionID='{0}'
	inner join po p on a.id = p.ID 
	inner join orders o on o.POID = p.ID
	where A.ID = o.POID 
		  and (ETA <= GETDATE() or B.InQty = (B.OutQty - B.AdjustQty))
		  and p.Complete = 0
	      and o.Finished = 0 
		  and o.Category = 'M'
) wantToClose

--將category = 'A' 底下有未finish的單子排除
select w.POID
into #wantToClose
from #wantToClose_step1 w
inner join Orders o with (nolock) on w.POID = o.ID
where not exists (select 1 from Orders where o.Category = 'A' and AllowanceComboID  = w.POID and Finished = 0) 

select *
into #canNotClose
from (
	-- Orders
	select distinct POID
	from Orders o WITH (NOLOCK) 
	where o.Finished = 0 
		  and o.MDivisionID = '{0}'
		  and o.PulloutComplete = 0
		  and o.Junk = 0
		  and (o.Category = 'B' or o.Category = 'S')

	-- Warehouse still have material
	union
	select distinct WTC.POID
	from PO_Supp_Detail A WITH (NOLOCK)
	inner join Orders o WITH (NOLOCK) on a.ID = o.ID	
	inner join #wantToClose WTC on A.ID = WTC.POID
	left join PO With (NOLOCK) on a.ID = PO.ID
								  and PO.Complete = 1
	left join MDivisionPoDetail B WITH (NOLOCK) on B.POID = A.ID 
												   and B.Seq1 = A.SEQ1 
												   and B.Seq2 = A.SEQ2
	inner join dbo.Factory F WITH (NOLOCK) on F.id = A.factoryid 
											  and F.MDivisionID = '{0}'
	where (A.Complete = 0  or B.InQty <> B.OutQty - B.AdjustQty)
		  and o.Category = 'M'
		  and PO.ID is null

	-- Below combined SP# not yet ship
	union
	select distinct WTC.POID
	from Orders WITH (NOLOCK) 	
	inner join #wantToClose WTC on Orders.POID = WTC.POID
	where (Orders.Junk=0 or (Orders.Junk=1 and Orders.NeedProduction=1))
		  and Orders.PulloutComplete = 0 
          and Orders.Junk = 0
		  and Orders.Category != 'M'
    --orders.CFMDate15天(包含)內的資料不能被關單
    union
    select distinct WTC.POID
	from Orders WITH (NOLOCK) 	
	inner join #wantToClose WTC on Orders.POID = WTC.POID
    where  Orders.CFMDate >= convert(date,getdate()-15)
) #canNotClose

select Selected = 1
	   , a.POID
	   , StyleID = isnull(o.StyleID, '')
	   , BuyerID = isnull(b.BuyerID, '')
	   , o.BuyerDelivery
	   , POCombo = [dbo].getPOComboList(a.POID, a.POID)
	   , MCHandle = (o.MCHandle + ' - ' + isnull(p.Name, ''))
	   , o.Category
from (
	select * 
	from #wantToClose
	
	except
	select * 
	from #canNotClose
) a
left join Orders o WITH (NOLOCK) on a.POID = o.ID
left join Brand b WITH (NOLOCK) on o.BrandID = b.ID
left join Pass1 p WITH (NOLOCK) on p.ID = o.MCHandle",
                Sci.Env.User.Keyword);

            DualResult result = DBProxy.Current.Select(null, sqlCmd, out gridData);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Query data fail!!" + result.ToString());
            }

            this.listControlBindingSource1.DataSource = gridData;
        }

        private void SetFilter()
        {
            StringBuilder stringFilter = new StringBuilder();
            stringFilter.Append("1=1");
            if (!MyUtility.Check.Empty(this.txtStyle.Text))
            {
                stringFilter.Append(string.Format(" and StyleID = '{0}'", this.txtStyle.Text));
            }

            if (!MyUtility.Check.Empty(this.txtBuyer.Text))
            {
                stringFilter.Append(string.Format(" and BuyerID = '{0}'", this.txtBuyer.Text));
            }

            if (!MyUtility.Check.Empty(this.dateBuyerDelivery.Value1))
            {
                stringFilter.Append(string.Format(" and BuyerDelivery >= '{0}'", this.dateBuyerDelivery.Value1));
            }

            if (!MyUtility.Check.Empty(this.dateBuyerDelivery.Value2))
            {
                stringFilter.Append(string.Format(" and BuyerDelivery <= '{0}'", this.dateBuyerDelivery.Value2));
            }

            ((DataTable)this.listControlBindingSource1.DataSource).DefaultView.RowFilter = stringFilter.ToString();
        }

    }
}
