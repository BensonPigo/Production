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
from Orders o
where o.Finished = 0 
and o.FtyGroup = '{0}'
and (o.Junk = 1 or o.PulloutComplete = 1)
and (o.Category = 'B' or o.Category = 'S')
),
canNotClose
as
(
select distinct POID
from Orders o
where o.Finished = 0 
and o.FtyGroup = '{0}'
and o.PulloutComplete = 0
and o.Junk = 0
and (o.Category = 'B' or o.Category = 'S')
)
select 1 as Selected,a.POID,isnull(o.StyleID,'') as StyleID,isnull(b.BuyerID,'') as BuyerID,o.BuyerDelivery,[dbo].getPOComboList(a.POID,a.POID) as POCombo
from (select * from wantToClose
	  except
	  select * from canNotClose) a
left join Orders o on a.POID = o.ID
left join Brand b on o.BrandID = b.ID", Sci.Env.User.Factory);
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
            string sqlCmd = "select ID,SeasonID,Description,BrandID from Style where Junk = 0 order by ID";
            item = new Sci.Win.Tools.SelectItem(sqlCmd, "16,10,50,8", this.Text);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            textBox1.Text = item.GetSelectedString();
        }

        //Buyer
        private void textBox2_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlCmd = "SELECT Id,NameCH,NameEN FROM Brand WHERE Junk=0  ORDER BY Id";
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "10,50,50", this.Text, false, ",");

            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            textBox2.Text = item.GetSelectedString();
        }

        //Style#
        private void textBox1_Validated(object sender, EventArgs e)
        {
            if (textBox1.OldValue != textBox1.Text)
            {
                setFilter();
            }
        }

        //Buyer
        private void textBox2_Validated(object sender, EventArgs e)
        {
            if (textBox2.OldValue != textBox2.Text)
            {
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
                        haveupdate = true;
                        MyUtility.Msg.InfoBox("Update completed!");
                    }
                    else
                    {
                        MyUtility.Msg.WarningBox("Update failed, Pleaes re-try"+result.ToString());
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ShowErr("Commit transaction error.", ex);
                    return;
                }
            }
            #endregion
        }
    }
}
