using Ict.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Win.Tems;
using Sci.Data;

namespace Sci.Production.Quality
{
    public partial class P09 : Sci.Win.Tems.Input6
    {
        private DataTable dtDB;
        private DataTable dtDetail;
        private bool insert = false;

        public P09(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            InsertDetailGridOnDoubleClick = false;
        }

        // 撈出表身資料
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? "" : e.Master["id"].ToString();
            string cmd = string.Format(@"
select distinct fd.id
    ,[Seq] = seq1+'-'+seq2
    , seq1,seq2
    ,[RefNo] = RefNo
    ,[Colorid] = fd.ColorID
    ,[Color] = fd.ColorID+'-'+c.Name
    ,[Suppid] = fd.SuppID
    ,[Supp] = fd.SuppID+'-'+s.NameEN
    ,[TestReport] = isnull(fd.TestReport,0)
    ,[InspReport] = isnull(fd.InspReport,0)
    ,[ContinuityCard] = isnull(fd.ContinuityCard,0)
    ,[BulkDyelot] = isnull(fd.BulkDyelot,0)
    ,[SeasonID] = fd.seasonid
from FabricInspDoc_Detail fd WITH (NOLOCK)
inner join orders o WITH (NOLOCK) on o.POID=fd.ID
left join color c WITH (NOLOCK) on fd.ColorID=c.ID and c.BrandId=o.BrandID
left join Supp s WITH (NOLOCK) on fd.SuppID=s.ID
where fd.id='{0}' order by seq1,seq2
", masterID);
            this.DetailSelectCommand = cmd;
            return base.OnDetailSelectCommandPrepare(e);
        }

        protected override void OnDetailGridSetup()
        {
            DataGridViewGeneratorCheckBoxColumnSettings chx1st = new DataGridViewGeneratorCheckBoxColumnSettings();
            DataGridViewGeneratorCheckBoxColumnSettings chxTestReport = new DataGridViewGeneratorCheckBoxColumnSettings();
            DataGridViewGeneratorCheckBoxColumnSettings chxInsReport = new DataGridViewGeneratorCheckBoxColumnSettings();
            DataGridViewGeneratorCheckBoxColumnSettings chxCard = new DataGridViewGeneratorCheckBoxColumnSettings();

            chxTestReport.CellValidating += (s, e) =>
             {
                 if (!EditMode)
                 {
                     return;
                 }
                 // 比對表身與DB資料是否一致
                 if (!MyUtility.Check.Empty(CurrentMaintain["ID"]) || DetailDatas.Count > 0)
                 {
                     CurrentDetailData["TestReport"] = e.FormattedValue;
                     CurrentDetailData.EndEdit();
                     chkDiff();
                 }
             };

            chxInsReport.CellValidating += (s, e) =>
            {
                if (!EditMode)
                {
                    return;
                }
                // 比對表身與DB資料是否一致
                if (!MyUtility.Check.Empty(CurrentMaintain["ID"]) || DetailDatas.Count > 0)
                {
                    CurrentDetailData["InspReport"] = e.FormattedValue;
                    CurrentDetailData.EndEdit();
                    chkDiff();
                }
            };

            chxCard.CellValidating += (s, e) =>
            {
                if (!EditMode)
                {
                    return;
                }
                // 比對表身與DB資料是否一致
                if (!MyUtility.Check.Empty(CurrentMaintain["ID"]) || DetailDatas.Count > 0)
                {
                    CurrentDetailData["ContinuityCard"] = e.FormattedValue;
                    CurrentDetailData.EndEdit();
                    chkDiff();
                }
            };

            chx1st.CellValidating += (s, e) =>
            {
                if (!EditMode)
                {
                    return;
                }
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                // 比對表身與DB資料是否一致
                if (!MyUtility.Check.Empty(CurrentMaintain["ID"]) || DetailDatas.Count > 0)
                {
                    CurrentDetailData["BulkDyelot"] = e.FormattedValue;
                    CurrentDetailData.EndEdit();                    
                    chkDiff();
                }
                foreach (DataRow drt in ((DataTable)this.detailgridbs.DataSource).Rows)
                {
                    if (drt.RowState!=DataRowState.Deleted)
                    {
                        // color,supp,refno 如果都相同,就勾選相同的1stBulkDyelot
                        if (dr["colorid"].ToString() == drt["colorid"].ToString()
                        && dr["Suppid"].ToString() == drt["Suppid"].ToString()
                        && dr["Refno"].ToString() == drt["Refno"].ToString())
                        {
                            drt["BulkDyelot"] = dr["BulkDyelot"];
                        }
                    }                   
                }
                FirstBulk();
            };

            Helper.Controls.Grid.Generator(this.detailgrid)
               .Text("Seq", header: "Seq#", width: Widths.AnsiChars(8), iseditingreadonly: true)
               .Text("RefNo", header: "Ref#", width: Widths.AnsiChars(15), iseditingreadonly: true)
               .Text("Color", header: "Color", width: Widths.AnsiChars(20), iseditingreadonly: true)
               .Text("Supp", header: "Supp", width: Widths.AnsiChars(30), iseditingreadonly: true)
               .CheckBox("TestReport", header: "Test Report", width: Widths.AnsiChars(5), trueValue: 1, falseValue: 0, settings: chxTestReport)
               .CheckBox("InspReport", header: "Inspection Report", width: Widths.AnsiChars(5), trueValue: 1, falseValue: 0, settings: chxInsReport)
               .CheckBox("ContinuityCard", header: "Continuity Card", width: Widths.AnsiChars(5), trueValue: 1, falseValue: 0, settings: chxCard)
               .CheckBox("BulkDyelot", header: "1stBulk Dyelot", width: Widths.AnsiChars(5), trueValue: 1, falseValue: 0, settings: chx1st);
        }

        protected override bool ClickEditBefore()
        {
            if (CurrentMaintain["Status"].EqualString("CONFIRMED"))
            {
                MyUtility.Msg.WarningBox("Data is confirmed, can't modify.", "Warning");
                return false;
            }
            return base.ClickEditBefore();
        }

        protected override void ClickNewAfter()
        {
            CurrentMaintain["Status"] = "New";
            this.txtID.ReadOnly = false;
            insert = true;
            btnReImport.Enabled = false;
            btnReImport.ForeColor = Color.Black;
            base.ClickNewAfter();
        }

        protected override void ClickEditAfter()
        {
            insert = false;
            chkDiff();
            base.ClickEditAfter();
        }

        protected override void ClickConfirm()
        {                        
            base.ClickConfirm();
            string sqlcmd;

            sqlcmd = string.Format("update FabricInspDoc set Status = 'Confirmed', editname = '{0}' , editdate = GETDATE() " +
                            "where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

            DualResult result;
            if (!(result = DBProxy.Current.Execute(null, sqlcmd)))
            {
                ShowErr(sqlcmd, result);
                return;
            }
            MyUtility.Msg.InfoBox("Confirmed successful.");
        }

        protected override void ClickUnconfirm()
        {
            base.ClickUnconfirm();            
            string sqlcmd;

            sqlcmd = string.Format("update FabricInspDoc set Status = 'New', editname = '{0}' , editdate = GETDATE() " +
                            "where id = '{1}'", Env.User.UserID, CurrentMaintain["id"]);

            DualResult result;
            if (!(result = DBProxy.Current.Execute(null, sqlcmd)))
            {
                ShowErr(sqlcmd, result);
                return;
            }
            MyUtility.Msg.InfoBox("UnConfirmed successful.");
        }

        private void txtID_Validating(object sender, CancelEventArgs e)
        {
            string spno = this.txtID.Text;
            bool delGrid = false;
            if (MyUtility.Check.Empty(spno))
            {
                delGrid = true;
            }
            else
            {
                // 檢查是否存在orders
                if (!MyUtility.Check.Seek($"select 1 from orders where poid='{spno}'"))
                {
                    this.txtID.Select();
                    MyUtility.Msg.WarningBox($"{spno} dose not exist!");
                    delGrid = true;
                }
            }          
          

            // 檢查該ID是否存在FabircInspDoc
            if (MyUtility.Check.Seek($"select 1 from FabricInspDoc where id='{spno}'"))
            {
                this.txtID.Select();
                MyUtility.Msg.WarningBox($"{spno} is already exists!");                
                return;
            }

            if (delGrid)
            {
                foreach (DataRow dr in ((DataTable)this.detailgridbs.DataSource).Rows)
                {
                    dr.Delete();
                }
                displayStyle.Text = "";
                displaySeason.Text = "";
                displayBrand.Text = "";
                return;
            }
           

            headerValue();
            insDetailData(true);
            //FirstBulk();
        }

        protected override DualResult ClickSave()
        {
            this.txtID.ReadOnly = true;
            btnReImport.ForeColor = Color.Black;
            return base.ClickSave();
        }

        protected override void ClickUndo()
        {
            this.txtID.ReadOnly = true;
            insert = false;
            btnReImport.ForeColor = Color.Black;
            base.ClickUndo();
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            btnReImport.Enabled = !MyUtility.Check.Empty(txtID.Text);
            headerValue();
            chkDiff();
        }

        private void headerValue()
        {
            DataRow dr;
            if (MyUtility.Check.Seek($@"select top 1 poid,StyleID,Seasonid,BrandID from orders where poid='{txtID.Text}'", out dr))
            {
                displayStyle.Text = dr["StyleID"].ToString();
                displaySeason.Text = dr["Seasonid"].ToString();
                displayBrand.Text = dr["BrandID"].ToString();
            }
            else
            {
                displayStyle.Text = "";
                displaySeason.Text = "";
                displayBrand.Text = "";
            }
        }

        // 比對表身資料跟DB資料是否一致
        private void chkDiff()
        {
            DualResult result;            
            bool diff = false;
            DataTable dtPo;

            if (!(result = DBProxy.Current.Select(null, $@"select * from FabricInspDoc_Detail where id='{txtID.Text}' order by seq1,seq2", out dtDB)))
            {
                MyUtility.Msg.WarningBox("Sql connection fail!\r\n" + result.ToString());
            }         

            string sqlcmd = $@"
SELECT DISTINCT 
[ID] =psd.id
,[Seq] = psd.SEQ1+'-'+psd.SEQ2
,psd.SEQ1,psd.SEQ2
,[Refno] = psd.Refno
,[Color] = psd.colorid+'-'+c.Name
,[Colorid]=psd.ColorID
,[Seasonid] = O.seasonID
,[Supp] = ps.suppid+'-'+s.NameEN
,[Suppid]=ps.SuppID
FROM PO_Supp_Detail psd
LEFT JOIN po_supp ps ON ps.id=psd.id AND ps.seq1=psd.seq1
LEFT JOIN ORDERS O ON O.ID=PSD.ID
left join color c WITH (NOLOCK) on psd.ColorID=c.ID and c.BrandId=o.BrandID
left join Supp s WITH (NOLOCK) on ps.SuppID=s.ID
WHERE  FabricType='F' and psd.id ='{txtID.Text}' 
and psd.SEQ1 <'70' and psd.Junk=0
";

            if (!(result = DBProxy.Current.Select(null, sqlcmd, out dtPo)))
            {
                MyUtility.Msg.WarningBox("Sql connection fail!\r\n" + result.ToString());
            }

            if ((MyUtility.Check.Empty(dtDB) || dtDB.Rows.Count < 1) || ((MyUtility.Check.Empty(dtPo) || dtPo.Rows.Count < 1)))
            {
                return;
            }

            foreach (DataRow dr in ((DataTable)this.detailgridbs.DataSource).Rows)
            {
                if (dr.RowState!=DataRowState.Deleted )
                {
                    #region 表身資料Check
                    DataRow[] selectData = dtDB.Select($@"id='{dr["id"].ToString()}' and seq1='{dr["seq1".ToString()]}' and seq2='{dr["seq2"].ToString()}'");

                    if (selectData.Length > 0)
                    {
                        if (selectData[0]["TestReport"].ToString() != dr["TestReport"].ToString() ||
                                               selectData[0]["InspReport"].ToString() != dr["InspReport"].ToString() ||
                                               selectData[0]["ContinuityCard"].ToString() != dr["ContinuityCard"].ToString() ||
                                               selectData[0]["BulkDyelot"].ToString() != dr["BulkDyelot"].ToString())
                        {
                            diff = true;
                        }
                    }
                    #endregion

                    #region PO資料Check
                    DataRow[] selectDataPO = dtPo.Select($@"id='{dr["id"].ToString()}' and seq1='{dr["seq1".ToString()]}' and seq2='{dr["seq2"].ToString()}'");
                    
                    if (selectDataPO.Length > 0)
                    {
                        if (selectDataPO[0]["ColorID"].ToString() != dr["ColorID"].ToString() ||
                            selectDataPO[0]["seasonID"].ToString() != dr["seasonID"].ToString() ||
                            selectDataPO[0]["SuppID"].ToString() != dr["SuppID"].ToString() ||
                            selectDataPO[0]["Refno"].ToString() != dr["Refno"].ToString())
                        {
                            diff = true;
                        }
                    }
                    #endregion

                }
            }
           
            if (DetailDatas.Count != dtDB.Rows.Count || DetailDatas.Count!=dtPo.Rows.Count)
            {
                diff = true;
            }

            if (diff && !insert)
            {
                btnReImport.Enabled = true;
                btnReImport.ForeColor = Color.Red;
            }
            else
            {
                btnReImport.Enabled = false;
                btnReImport.ForeColor = Color.Black;
            }

        }

        private void insDetailData(bool isInsert)
        {
            DualResult result;           
           
            string sqlcmd = $@"
SELECT DISTINCT 
[ID] =psd.id
,[Seq] = psd.SEQ1+'-'+psd.SEQ2
,psd.SEQ1,psd.SEQ2
,[Refno] = psd.Refno
,[Color] = psd.colorid+'-'+c.Name
,[Colorid]=psd.ColorID
,[Seasonid] = O.seasonID
,[Supp] = ps.suppid+'-'+s.NameEN
,[Suppid]=ps.SuppID
,[BulkDyelot] = ISNULL(OFD.BulkDyelot,0)
,[TestReport]= 0
,[InspReport]=0
,[ContinuityCard]=0
FROM PO_Supp_Detail psd
LEFT JOIN po_supp ps ON ps.id=psd.id AND ps.seq1=psd.seq1
LEFT JOIN ORDERS O ON O.ID=PSD.ID
left join color c WITH (NOLOCK) on psd.ColorID=c.ID and c.BrandId=o.BrandID
left join Supp s WITH (NOLOCK) on ps.SuppID=s.ID
outer apply
(SELECT DISTINCT FD.SeasonID, FD.Refno,FD.ColorID,FD.SuppID,FD.BulkDyelot 
FROM FabricInspDoc_Detail FD
WHERE FD.Refno=psd.Refno AND FD.seasonID
 =o.seasonID AND FD.colorid=psd.colorid AND  FD.suppid=ps.suppid AND FD.BulkDyelot=1
) 
AS OFD
WHERE  FabricType='F' and psd.id ='{txtID.Text}' 
and psd.SEQ1 <'70' and psd.Junk=0
";
            if (result = DBProxy.Current.Select(null, sqlcmd.ToString(), out dtDetail))
            {
                if (dtDetail.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                }
                else if(!insert)
                {
                    btnReImport.Enabled = true;
                    btnReImport.ForeColor = Color.Red;
                }
            }
            else
            {
                MyUtility.Msg.WarningBox("Sql connection fail!\r\n" + result.ToString());
            }

            FirstBulk();
            if (isInsert)
            {
                this.detailgridbs.DataSource = dtDetail;
            }
            else
            {
                #region 異動的資料
                string diffRows = "";
                foreach (DataRow dr in ((DataTable)this.detailgridbs.DataSource).Rows)
                {
                    if (dr.RowState!=DataRowState.Deleted)
                    {
                        DataRow[] selectDelete = dtDetail.Select($@"id='{dr["id"].ToString()}' and seq1='{dr["seq1"].ToString()}' and seq2='{dr["seq2"].ToString()}'");

                        if (selectDelete.Length < 1)
                        {
                            diffRows = diffRows + $@"Remove " + dr["seq1"].ToString() + "-" + dr["seq2"].ToString() + " " + dr["Seasonid"].ToString() + " " + dr["Refno"].ToString() + " " + dr["ColorID"].ToString() + " " + dr["suppid"].ToString() + "\r\n";
                            dr.Delete();
                        }
                    }                    
                }
                DataTable dtGrid = (DataTable)this.detailgridbs.DataSource;

                foreach (DataRow dr in dtDetail.Rows)
                {
                    DataRow[] selectIns = dtGrid.Select($@"id='{dr["id"].ToString()}' and seq1='{dr["seq1"].ToString()}' and seq2='{dr["seq2"].ToString()}'");
                    if (selectIns.Length < 1)
                    {
                        diffRows = diffRows + $@"Insert " + dr["seq1"].ToString() + "-" + dr["seq2"].ToString() + " " + dr["Seasonid"].ToString() + " " + dr["Refno"].ToString() + " " + dr["ColorID"].ToString() + " " + dr["suppid"].ToString() + "\r\n";
                        DataRow drins = dtGrid.NewRow();
                        drins["id"] = dr["id"];
                        drins["seq"] = dr["seq"];
                        drins["seq1"] = dr["seq1"];
                        drins["seq2"] = dr["seq2"];
                        drins["colorid"] = dr["colorid"];
                        drins["suppid"] = dr["suppid"];
                        drins["color"] = dr["color"];
                        drins["supp"] = dr["supp"];
                        drins["Refno"] = dr["Refno"];
                        drins["SeasonID"] = dr["SeasonID"];
                        dtGrid.Rows.Add(drins);
                    }                   
                }
                if (diffRows.Length > 1)
                {
                    MyUtility.Msg.InfoBox("The corrected data is as follows" + "\r\n" +
                    "Status Seq# Season Ref# Color Supp" + "\r\n" + diffRows);
                    btnReImport.Enabled = false;
                    btnReImport.ForeColor = Color.Black;
                }
                #endregion
            }
        }

        private void FirstBulk()
        {
            if (!insert)
            {
                return; 
            }
            string msg_bulkdyelot = "";
            foreach (DataRow drt in ((DataTable)this.detailgridbs.DataSource).Rows)
            {
                if (drt.RowState != DataRowState.Deleted)
                {
                    if ((bool)drt["BulkDyelot"])
                    {
                        foreach (DataRow drt1 in ((DataTable)this.detailgridbs.DataSource).Rows)
                        {
                            // color,supp,refno 如果都相同,就勾選相同的1stBulkDyelot
                            if (drt1["colorid"].ToString() == drt["colorid"].ToString()
                   && drt1["Suppid"].ToString() == drt["Suppid"].ToString()
                   && drt1["Refno"].ToString() == drt["Refno"].ToString())
                            {
                                drt["BulkDyelot"] = drt1["BulkDyelot"];
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < dtDetail.Rows.Count; i++)
            {
                if (MyUtility.Convert.GetBool(dtDetail.Rows[i]["BulkDyelot"]))
                {
                    msg_bulkdyelot = msg_bulkdyelot + $"{dtDetail.Rows[i]["Seq"]}, {dtDetail.Rows[i]["SeasonID"]},{dtDetail.Rows[i]["Refno"]},{dtDetail.Rows[i]["Color"]},{dtDetail.Rows[i]["supp"]}" + "\r\n";
                }
            }

            if (msg_bulkdyelot.Length > 1)
            {
                MyUtility.Msg.WarningBox("Already received 1st bulk dyelot. Below data will be selected automatically.\r\n" + "seq# Season Ref# Color Supp \r\n" + msg_bulkdyelot.ToString());
            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {            
            if (!EditMode)
            {
                MyUtility.Msg.InfoBox("Please go to 'modify mode' to click this button!");
                return;
            }
            else
            {
                DialogResult dResult = MyUtility.Msg.QuestionBox("Purchase item has been modified. Reload data?", "Question", MessageBoxButtons.YesNo);
                if (dResult.ToString().ToUpper() == "NO")
                {
                    return;
                }
                else
                {
                    insDetailData(insert);
                    foreach (DataRow drt in ((DataTable)this.detailgridbs.DataSource).Rows)
                    {
                        if (drt.RowState != DataRowState.Deleted)
                        {
                            if ((bool)drt["BulkDyelot"])
                            {
                                foreach (DataRow drt1 in ((DataTable)this.detailgridbs.DataSource).Rows)
                                {
                                    // color,supp,refno 如果都相同,就勾選相同的1stBulkDyelot
                                    if (drt1["colorid"].ToString() == drt["colorid"].ToString()
                           && drt1["Suppid"].ToString() == drt["Suppid"].ToString()
                           && drt1["Refno"].ToString() == drt["Refno"].ToString())
                                    {
                                        drt["BulkDyelot"] = drt1["BulkDyelot"];
                                    }
                                }
                            }  
                        }
                    }
                    FirstBulk();
                }
            }
        }
    }
}
