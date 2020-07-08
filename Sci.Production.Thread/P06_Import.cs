using Ict.Win;
using Sci.Data;
using System;
using System.Data;
using System.Drawing;
using Ict;

namespace Sci.Production.Thread
{
    /// <summary>
    /// P06_Import
    /// </summary>
    public partial class P06_Import : Win.Subs.Base
    {
        private DataTable detTable;
        private DataTable gridTable;
        private string keyword = Sci.Env.User.Keyword;

        /// <summary>
        /// P06_Import
        /// </summary>
        /// <param name="detTable">detTable</param>
        public P06_Import(DataTable detTable)
        {
            this.InitializeComponent();
            this.detTable = detTable;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            string sql = string.Format(@"Select 1 as sel,a.*,'' as description,'' as colordesc,'' as threadtypeid,0 as threadtex,'' as category,'' as localsuppid , '' as supp, 0 as newconevar,0 as UsedConevar from threadInventory_Detail a WITH (NOLOCK) where 1!=1", this.keyword);
            DBProxy.Current.Select("Production", sql, out this.gridTable);
            this.grid1.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.grid1.DataSource = this.gridTable;

            this.Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Sel", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
                .Text("Refno", header: "Thread Refno", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .CellThreadColor("ThreadColorid", header: "Color", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("Colordesc", header: "Color Description", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .CellThreadLocation("ThreadLocationid", header: "Location", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("NewConebook", header: "New Cone\nper books", width: Widths.AnsiChars(5), integer_places: 5, iseditingreadonly: true)
                .Numeric("UsedConebook", header: "Used cone\nper books", width: Widths.AnsiChars(5), integer_places: 5, iseditingreadonly: true)
                .Text("threadtypeid", header: "Thread Type", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Numeric("ThreadTex", header: "Tex", width: Widths.AnsiChars(5), integer_places: 3, iseditingreadonly: true)
                .Text("category", header: "Category", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("supp", header: "Supplier", width: Widths.AnsiChars(20), iseditingreadonly: true);
            this.grid1.Columns["Sel"].DefaultCellStyle.BackColor = Color.Pink;
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            string threadlocation1 = this.txtthreadlocationStart.Text, threadlocation2 = this.txtthreadlocationEnd.Text;
            string thradrefno1 = this.txtlocalitemStart.Text, thradrefno2 = this.txtlocalitemEnd.Text;
            string color1 = this.txtthreadcolorStart.Text, color2 = this.txtthreadcolorEnd.Text;
            if (MyUtility.Check.Empty(threadlocation1) && MyUtility.Check.Empty(threadlocation2) && MyUtility.Check.Empty(thradrefno1) && MyUtility.Check.Empty(thradrefno2))
            {
                MyUtility.Msg.WarningBox("At least one condition <Refno> <Location> must be entried.");
                this.txtlocalitemStart.Focus();
                return;
            }

            string sql = @"Select 1 as sel,a.refno,a.threadcolorid,a.threadlocationid,
                    isnull(a.newcone,0) as newconebook,0 as newCone,-(a.newCone) as NewconeVar,
                    isnull(a.usedcone,0) as usedconebook,0 as usedCone,-(a.usedCone) as UsedconeVar,
                    b.description,c.description as colordesc,
                    b.category,b.Localsuppid,b.threadtypeid,b.ThreadTex,
                    (b.Localsuppid+'-'+(Select name from LocalSupp d WITH (NOLOCK) where b.localsuppid = d.id)) as supp
                    from Localitem b  WITH (NOLOCK) 
                    left join ThreadStock a WITH (NOLOCK) on a.refno = b.refno";

            if (!MyUtility.Check.Empty(threadlocation1) || !MyUtility.Check.Empty(threadlocation2))
            {
                sql = sql + string.Format(" and a.ThreadLocationid >= '{0}' and a.ThreadLocationid <= '{1}'", threadlocation1, threadlocation2);
            }

            if (!MyUtility.Check.Empty(color1) || !MyUtility.Check.Empty(color2))
            {
                sql = sql + string.Format(" and a.Threadcolorid >= '{0}' and a.Threadcolorid <= '{1}'", color1, color2);
            }

            sql = sql + " left join ThreadColor c WITH (NOLOCK) on c.id = a.threadcolorid ";
            sql = sql + " where (a.newcone !=0 or a.usedcone!=0)";
            if (!MyUtility.Check.Empty(thradrefno1) || !MyUtility.Check.Empty(thradrefno2))
            {
                sql = sql + string.Format(" and a.refno >= '{0}' and a.refno <= '{1}'", thradrefno1, thradrefno2);
            }

            this.gridTable.Clear();
            try
            {
                DBProxy.Current.Select("Production", sql, out this.gridTable);
                if (this.gridTable.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found.");
                    return;
                }

                this.grid1.DataSource = this.gridTable;
            }
            catch (Exception ex)
            {
                this.ShowErr("Commit transaction error.", ex);
                return;
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            this.grid1.ValidateControl();
            if (MyUtility.Check.Empty(this.gridTable) || this.gridTable.Rows.Count == 0)
            {
                return;
            }

            DataRow[] dr2 = this.gridTable.Select("Sel= 1");
            if (dr2.Length > 0)
            {
                foreach (DataRow dr in dr2)
                {
                    DataRow[] findrow = this.detTable.Select(string.Format("refno = '{0}' and ThreadColorid = '{2}' and threadLocationid = '{1}' ", dr["refno"].ToString(), dr["ThreadLocationid"], dr["Threadcolorid"]));
                    if (findrow.Length == 0)
                    {
                        DataRow ndr = this.detTable.NewRow();

                        ndr["refno"] = dr["refno"];
                        ndr["Description"] = dr["Description"];
                        ndr["threadtypeid"] = dr["threadtypeid"];
                        ndr["threadcolorid"] = dr["threadcolorid"];
                        ndr["threadlocationid"] = dr["threadlocationid"];
                        ndr["colordesc"] = dr["colordesc"];
                        ndr["category"] = dr["category"];
                        ndr["supp"] = dr["supp"];
                        ndr["LocalSuppid"] = dr["LocalSuppid"];
                        ndr["threadtex"] = dr["threadtex"];
                        ndr["NewConeBook"] = dr["NewConeBook"];
                        ndr["UsedConeBook"] = dr["UsedConeBook"];
                        ndr["NewConevar"] = dr["NewConevar"];
                        ndr["UsedConevar"] = dr["UsedConevar"];
                        ndr["NewCone"] = 0;
                        ndr["UsedCone"] = 0;
                        this.detTable.Rows.Add(ndr);
                    }
                    else
                    {
                        findrow[0]["NewConeBook"] = dr["NewConeBook"];
                        findrow[0]["UsedConeBook"] = dr["UsedConeBook"];
                        findrow[0]["NewConevar"] = dr["NewConevar"];
                        findrow[0]["UsedConevar"] = dr["UsedConevar"];
                        findrow[0]["NewCone"] = 0;
                        findrow[0]["UsedCone"] = 0;
                    }
                }
            }
            else
            {
                MyUtility.Msg.WarningBox("Please select data first!", "Warnning");
                return;
            }

            this.Close();
        }
    }
}
