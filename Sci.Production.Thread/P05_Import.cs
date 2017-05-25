using Ict.Win;
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
using System.Data.SqlClient;
using System.Threading;

namespace Sci.Production.Thread
{
    public partial class P05_Import : Sci.Win.Subs.Base
    {
        private DataTable detTable;
        private DataTable gridTable;
        private string keyword = Sci.Env.User.Keyword;
        System.Threading.Thread thread = null;

        public P05_Import(DataTable detTable)
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
                .Numeric("NewConebook", header: "New Cone\nperbooks", width: Widths.AnsiChars(5), integer_places: 5, iseditingreadonly: true)
                .Numeric("UsedConebook", header: "Used cone\nperbooks", width: Widths.AnsiChars(5), integer_places: 5, iseditingreadonly:
                true)
                .Text("threadtypeid", header: "Thread Type", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Numeric("ThreadTex", header: "Tex", width: Widths.AnsiChars(5), integer_places: 3, iseditingreadonly: true)
                .Text("category", header: "Category", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("supp", header: "Supplier", width: Widths.AnsiChars(20), iseditingreadonly: true);
            this.gridDetail.Columns["Sel"].DefaultCellStyle.BackColor = Color.Pink;

        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            string threadlocation1 = this.txtthreadlocationStart.Text, threadlocation2 = this.txtthreadlocationEnd.Text;
            string thradrefno1 = txtlocalitemStart.Text, thradrefno2 = txtlocalitemEnd.Text;
            string color1 = txtthreadcolorStart.Text, color2 = txtthreadcolorEnd.Text;
            string allpart = checkAllThread.Value;
            int rand = (int)numCountOfRadom.Value;
            if (MyUtility.Check.Empty(threadlocation1) && MyUtility.Check.Empty(threadlocation2) && MyUtility.Check.Empty(thradrefno1) && MyUtility.Check.Empty(thradrefno2) && allpart == "False")
            {
                MyUtility.Msg.WarningBox("At least one condition <Refno> <Location> must be entried.");
                this.txtlocalitemStart.Focus();
                return;
            }
            string sql = string.Format(@"
Select  1 as sel
        , a.refno
        , a.threadcolorid
        , a.threadlocationid
        , isnull(a.newcone,0) as newconebook
        , 0 as newCone
        , -(a.newCone) as NewconeVar
        , isnull(a.usedcone,0) as usedconebook
        , 0 as usedCone
        , -(a.usedCone) as UsedconeVar
        , b.description
        , c.description as colordesc
        , b.category
        , b.Localsuppid
        , b.threadtypeid
        , b.ThreadTex
        , supp = (b.Localsuppid + '-' + (Select name 
                                         from LocalSupp d WITH (NOLOCK) 
                                         where b.localsuppid = d.id))
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
                DataTable dt = null;

                this.ShowWaitMessage("Data Loading...");
                using (var cn = new SqlConnection(Env.Cfg.GetConnection("").ConnectionString))
                using (var cm = cn.CreateCommand())
                {
                    cm.CommandText = sql;
                    var adp = new System.Data.SqlClient.SqlDataAdapter(cm);
                    var cnt = 0;
                    var start = 0;
                    using (var ds = new DataSet())
                    {
                        while ((cnt = adp.Fill(ds, start, 100000, "Production")) > 0)
                        {
                            start += 100000;

                            if (dt == null)
                            {
                                dt = ((DataTable)ds.Tables[0]).Clone();
                            }
                            dt.Merge(((DataTable)ds.Tables[0]));

                            ds.Tables[0].Dispose();
                            ds.Tables.Clear();
                        }
                    }
                }
                this.HideWaitMessage();

                if (dt == null || dt.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found.");
                    return;
                }
                int rowcount = dt.Rows.Count; //因Random是小於此數字，因此要+1
                if (rowcount <= rand) allpart = "True"; //Random多於找出來的就全部寫入 
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
                            if (danom1 == 0) zerorand = true;
                            DataRow ndtdr = dt.Rows[danom1];
                            DataRow ndr = gridTable.NewRow();
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

                            gridTable.Rows.Add(ndr);

                        }
                        else
                        {
                            i--;
                        }
                    }


                }
                else
                {
                    gridTable = dt;
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
            ThreadStart importThread = new ThreadStart(RunImportThread);
            thread = new System.Threading.Thread(importThread);
            thread.IsBackground = true;
            thread.Start();
        }


        private const int CtrlUI = 0, ImportADD = 1, ImportModify = 2, CloseForm = 3;

        private void RunImportThread()
        {
            gridDetail.ValidateControl();
            if (MyUtility.Check.Empty(gridTable) || gridTable.Rows.Count == 0)
            {
                return;
            }

            DataRow[] dr2 = gridTable.Select("Sel= 1");
            if (dr2.Length > 0)
            {
                UIThread(CtrlUI);
                int index = 1;
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
                        UIThread(ImportADD, index, dr2.Length, ndr);
                    }
                    else
                    {
                        UIThread(ImportModify, index, dr2.Length, dr, findrow[0]);
                    }
                    index++;
                }
                UIThread(CloseForm);
            }
            else
            {
                MyUtility.Msg.WarningBox("Please select data first!", "Warnning");
                this.thread.Abort();
                this.thread.Join();
                this.thread = null;
                return;
            }
        }

        private delegate void importUI(int doWhat, int index, int total, DataRow newDR, DataRow P05DR);

        private void UIThread(int doWhat, int index = 0, int total = 0, DataRow newDR = null, DataRow P05DR = null)
        {
            if (this.InvokeRequired)
            {
                importUI import = new importUI(UIThread);
                this.Invoke(import, doWhat, index, total, newDR, P05DR);
            }
            else
            {
                switch (doWhat)
                {
                    case CtrlUI:
                        this.EditMode = false;
                        this.btnImport.Enabled = false;
                        this.btnQuery.Enabled = false;
                        break;
                    case ImportADD:
                        this.ShowWaitMessage(string.Format("Data Loading... ({0} / {1})", index, total));
                        this.detTable.Rows.Add(newDR);
                        break;
                    case ImportModify:
                        this.ShowWaitMessage(string.Format("Data Loading... ({0} / {1})", index, total));
                        P05DR["NewConeBook"] = newDR["NewConeBook"];
                        P05DR["UsedConeBook"] = newDR["UsedConeBook"];
                        P05DR["NewConevar"] = newDR["NewConevar"];
                        P05DR["UsedConevar"] = newDR["UsedConevar"];
                        P05DR["NewCone"] = 0;
                        P05DR["UsedCone"] = 0;
                        break;
                    case CloseForm:
                        this.HideWaitMessage();
                        this.Close();
                        break;
                }
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            if (thread != null)
            {
                this.thread.Abort();
                this.thread.Join();
            }
            base.OnClosed(e);
        }
    }
}
