﻿using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;

using System.Windows.Forms;
using Sci;
using Sci.Win;
using Sci.Win.Tools;
using Ict;
using Ict.Data;

namespace Sci.Production.Thread
{
    public partial class P07_Import : Sci.Win.Subs.Base
    {
        private DataTable detTable;
        private DataTable gridTable;
        private string keyword = Sci.Env.User.Keyword;
        public P07_Import(DataTable detTable)
        {
            InitializeComponent();
            this.detTable = detTable;

        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            string sql = string.Format(@"Select 1 as sel,a.*,'' as description,'' as colordesc,'' as threadtypeid,0 as threadtex,'' as category,'' as localsuppid , '' as supp, 0 as newconevar,0 as UsedConevar from threadInventory_Detail a WITH (NOLOCK) where 1!=1", keyword);
            DBProxy.Current.Select("Production", sql, out gridTable);
            this.gridDetail.IsEditingReadOnly = false; //必設定, 否則CheckBox會顯示圖示
            this.gridDetail.DataSource = gridTable;

            Helper.Controls.Grid.Generator(this.gridDetail)
                .CheckBox("Sel", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
                .Text("Refno", header: "Thread Refno", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .CellThreadColor("ThreadColorid", header: "Color", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("Colordesc", header: "Color Description", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .CellThreadLocation("ThreadLocationid", header: "Location", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("NewConebook", header: "New Cone\nin Stock", width: Widths.AnsiChars(5), integer_places: 5, iseditingreadonly: true)
                .Numeric("UsedConebook", header: "Used cone\nin Stock", width: Widths.AnsiChars(5), integer_places: 5, iseditingreadonly:
                true)
                .Text("category", header: "Category", width: Widths.AnsiChars(20), iseditingreadonly: true);
            this.gridDetail.Columns["Sel"].DefaultCellStyle.BackColor = Color.Pink;

        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            string threadlocation1 = this.txtthreadlocationStart.Text, threadlocation2 = this.txtthreadlocationEnd.Text;
            string thradrefno1 = txtlocalitemStart.Text, thradrefno2 = txtlocalitemEnd.Text;
            string color1 = txtthreadcolorStart.Text, color2 = txtthreadcolorEnd.Text;
            if (MyUtility.Check.Empty(threadlocation1) && MyUtility.Check.Empty(threadlocation2) && MyUtility.Check.Empty(thradrefno1) && MyUtility.Check.Empty(thradrefno2))
            {
                this.txtlocalitemStart.Focus();
                MyUtility.Msg.WarningBox("At least one condition <Refno> <Location> must be entried.");
                return;
            }
            string sql = string.Format(@"Select 1 as sel,a.refno,a.threadcolorid,a.threadlocationid,
                    isnull(a.newcone,0) as newconebook,isnull(a.usedcone,0) as usedconebook,
                    b.description,c.description as colordesc,b.category
                    from Localitem b WITH (NOLOCK) 
                    left join ThreadStock a WITH (NOLOCK) on a.refno = b.refno and a.mdivisionid = '{0}'", keyword);

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
            gridTable.Clear();
            try
            {
                DBProxy.Current.Select("Production", sql, out gridTable);
                if (gridTable.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found.");
                    return;
                }
                this.gridDetail.DataSource = gridTable;

            }
            catch (Exception ex)
            {
                ShowErr("Commit transaction error.", ex);
                return;
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            gridDetail.ValidateControl();
            if (MyUtility.Check.Empty(gridTable)|| gridTable.Rows.Count == 0) return;

            DataRow[] dr2 = gridTable.Select("Sel= 1");
            if (dr2.Length > 0)
            {
                foreach (DataRow dr in dr2)
                {
                    DataRow[] findrow = this.detTable.Select(string.Format("refno = '{0}' and ThreadColorid = '{2}' and Locationfrom = '{1}' ", dr["refno"].ToString(), dr["ThreadLocationid"], dr["Threadcolorid"]));
                    if (findrow.Length == 0)
                    {
                        DataRow ndr = this.detTable.NewRow();

                        ndr["refno"] = dr["refno"];
                        ndr["Description"] = dr["Description"];
                        ndr["threadcolorid"] = dr["threadcolorid"];
                        ndr["locationfrom"] = dr["threadlocationid"];
                        ndr["colordesc"] = dr["colordesc"];
                        ndr["category"] = dr["category"];
                        ndr["NewConeBook"] = dr["NewConeBook"];
                        ndr["UsedConeBook"] = dr["UsedConeBook"];
                        ndr["NewCone"] = 0;
                        ndr["UsedCone"] = 0;
                        this.detTable.Rows.Add(ndr);
                    }
                    else
                    {
                        findrow[0]["NewConeBook"] = dr["NewConeBook"];
                        findrow[0]["UsedConeBook"] = dr["UsedConeBook"];
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
