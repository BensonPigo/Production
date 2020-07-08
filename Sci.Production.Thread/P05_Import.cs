using Ict.Win;
using Sci.Data;
using System;
using System.Data;
using System.Drawing;
using Ict;
using System.Linq;

namespace Sci.Production.Thread
{
    /// <summary>
    /// P05_Import
    /// </summary>
    public partial class P05_Import : Sci.Win.Subs.Base
    {
        private DataTable detTable;
        private DataTable gridTable;
        private string keyword = Sci.Env.User.Keyword;

        /// <summary>
        /// P05_Import
        /// </summary>
        /// <param name="detTable">detTable</param>
        public P05_Import(DataTable detTable)
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
            this.gridDetail.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.gridDetail.DataSource = this.gridTable;

            this.Helper.Controls.Grid.Generator(this.gridDetail)
                .CheckBox("Sel", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
                .Text("Refno", header: "Thread Refno", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .CellThreadColor("ThreadColorid", header: "Color", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("Colordesc", header: "Color Description", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .CellThreadLocation("ThreadLocationid", header: "Location", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("NewConebook", header: "New Cone\nperbooks", width: Widths.AnsiChars(5), integer_places: 5, iseditingreadonly: true)
                .Numeric("UsedConebook", header: "Used cone\nperbooks", width: Widths.AnsiChars(5), integer_places: 5, iseditingreadonly: true)
                .Text("threadtypeid", header: "Thread Type", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Numeric("ThreadTex", header: "Tex", width: Widths.AnsiChars(5), integer_places: 3, iseditingreadonly: true)
                .Text("category", header: "Category", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("supp", header: "Supplier", width: Widths.AnsiChars(20), iseditingreadonly: true);
            this.gridDetail.Columns["Sel"].DefaultCellStyle.BackColor = Color.Pink;
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            string threadlocation1 = this.txtthreadlocationStart.Text, threadlocation2 = this.txtthreadlocationEnd.Text;
            string thradrefno1 = this.txtlocalitemStart.Text, thradrefno2 = this.txtlocalitemEnd.Text;
            string color1 = this.txtthreadcolorStart.Text, color2 = this.txtthreadcolorEnd.Text;
            string allpart = this.checkAllThread.Value;
            int rand = (int)this.numCountOfRadom.Value;

            // if (MyUtility.Check.Empty(threadlocation1) && MyUtility.Check.Empty(threadlocation2) && MyUtility.Check.Empty(thradrefno1) && MyUtility.Check.Empty(thradrefno2) && allpart == "False")
            // {
            //    MyUtility.Msg.WarningBox("At least one condition <Refno> <Location> must be entried.");
            //    this.txtlocalitemStart.Focus();
            //    return;
            // }
            string sql = @"Select 1 as sel,a.refno,a.threadcolorid,a.threadlocationid,
                    isnull(a.newcone,0) as newconebook,0 as newCone,-(a.newCone) as NewconeVar,
                    isnull(a.usedcone,0) as usedconebook,0 as usedCone,-(a.usedCone) as UsedconeVar,
                    b.description,c.description as colordesc,
                    b.category,b.Localsuppid,b.threadtypeid,b.ThreadTex,
                    (b.Localsuppid+'-'+(Select name from LocalSupp d WITH (NOLOCK) where b.localsuppid = d.id)) as supp
                    from Localitem b WITH (NOLOCK) 
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
                DualResult rst;
                DataTable dt;
                if (!(rst = DBProxy.Current.Select("Production", sql, out dt)))
                {
                    this.ShowErr(sql, rst);
                    return;
                }
                else
                {
                    if (dt.Rows.Count == 0)
                    {
                        MyUtility.Msg.WarningBox("Data not found.");
                        return;
                    }

                    int rowcount = dt.Rows.Count; // 因Random是小於此數字，因此要+1
                    if (rowcount <= rand)
                    {
                        allpart = "True"; // Random多於找出來的就全部寫入
                    }

                    int[] intlist = new int[rand];
                    int danom1;
                    Random dr = new Random();
                    bool zerorand = false;
                    if (allpart != "True" && rand != 0)
                    {
                        for (int i = 0; i < rand; i++)
                        {
                            danom1 = dr.Next(rowcount);
                            if (Array.IndexOf(intlist, danom1) == -1 || (danom1 == 0 && zerorand == false))
                            {
                                intlist[i] = danom1;
                                if (danom1 == 0)
                                {
                                    zerorand = true;
                                }

                                DataRow ndtdr = dt.Rows[danom1];
                                DataRow ndr = this.gridTable.NewRow();
                                ndr["sel"] = 1;
                                ndr["refno"] = ndtdr["refno"];
                                ndr["Description"] = ndtdr["Description"];
                                ndr["threadtypeid"] = ndtdr["threadtypeid"];
                                ndr["threadcolorid"] = ndtdr["threadcolorid"];
                                ndr["threadlocationid"] = ndtdr["threadlocationid"];
                                ndr["colordesc"] = ndtdr["colordesc"];
                                ndr["category"] = ndtdr["category"];
                                ndr["supp"] = ndtdr["supp"];
                                ndr["LocalSuppid"] = ndtdr["LocalSuppid"];
                                ndr["threadTex"] = ndtdr["threadTex"];
                                ndr["NewConeBook"] = ndtdr["NewConeBook"];
                                ndr["UsedConeBook"] = ndtdr["UsedConeBook"];
                                ndr["NewConevar"] = ndtdr["NewConevar"];
                                ndr["UsedConevar"] = ndtdr["UsedConevar"];
                                ndr["NewCone"] = 0;
                                ndr["UsedCone"] = 0;

                                this.gridTable.Rows.Add(ndr);
                            }
                            else
                            {
                                i--;
                            }
                        }

                        this.gridTable.DefaultView.Sort = "refno , threadcolorid , threadlocationid";
                        this.gridTable = this.gridTable.DefaultView.ToTable();
                    }
                    else
                    {
                        this.gridTable = dt;
                    }

                    this.gridDetail.DataSource = this.gridTable;
                }
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
            this.gridDetail.ValidateControl();
            if (MyUtility.Check.Empty(this.gridTable) || this.gridTable.Rows.Count == 0)
            {
                return;
            }

            DataRow[] dr2 = this.gridTable.Select("Sel= 1");
            if (dr2.Length > 0)
            {
                int index = 1;
                var lookupData = this.detTable.AsEnumerable()
                    .Where(row => row.RowState != DataRowState.Deleted)
                    .ToLookup(row => row["refno"].ToString().ToUpper().TrimEnd() + "@" +
                    row["ThreadLocationid"].ToString().ToUpper().TrimEnd() + "@" +
                    row["Threadcolorid"].ToString().ToUpper().TrimEnd());
                foreach (DataRow dr in dr2)
                {
                    this.ShowWaitMessage(string.Format("Data Loading... ({0} / {1})", index++, dr2.Length), 500);
                    var refno = dr["refno"].ToString().ToUpper().TrimEnd();
                    var threadLocation = dr["ThreadLocationid"].ToString().ToUpper().TrimEnd();
                    var threadColor = dr["Threadcolorid"].ToString().ToUpper().TrimEnd();
                    DataRow[] findrow = lookupData[refno + "@" + threadLocation + "@" + threadColor].ToArray();
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

            this.HideWaitMessage();
            this.Close();
        }
    }
}
