using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.IO;
using System.Data.SqlClient;
using Sci.Production.Automation;
using System.Threading.Tasks;

namespace Sci.Production.PPIC
{
    /// <summary>
    /// P04
    /// </summary>
    public partial class P04 : Win.Tems.Input1
    {
        private string destination_path; // 放圖檔的路徑
        DataTable dtFinishingProcess;
        DataTable dtFinishingProcessAll;

        /// <summary>
        /// P04
        /// </summary>
        /// <param name="menuitem">ToolStripMenuItem</param>
        public P04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.destination_path = MyUtility.GetValue.Lookup("select StyleSketch from System WITH (NOLOCK) ", null);
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            MyUtility.Tool.SetupCombox(this.comboSizeUnit, 1, 1, "CM,INCH");

            // DB 結構為int 無法使用MyUtility.Tool.SetupCombox方法寫入
            this.comboPressing1.Add("No Pressing", 0);
            this.comboPressing1.Add("Manual Pressing", 1);
            this.comboPressing1.Add("Auto Pressing", 2);
            this.comboPressing1.Add("Manual + Auto Pressing", 3);
            this.comboPressing1.SelectedIndex = 1;
            this.comboFolding1.Add("Manual Folding", 0);
            this.comboFolding1.Add("Auto Folding", 1);
            this.comboFolding1.SelectedIndex = 0;
            this.comboFolding2.Add("0", 0);
            this.comboFolding2.SelectedIndex = 0;

            string sql = string.Format(
                @" select distinct DM300
                    from (
	                    select 0 as DM300
	                    union all
	                    select distinct DM300 
	                    from FinishingProcess
                        where junk <> 1
                    )a ");
            DualResult selectResult = DBProxy.Current.Select(null, sql, out this.dtFinishingProcess);
            if (!selectResult)
            {
                this.ShowErr(sql, selectResult);
            }

            sql = string.Format(
                @" select distinct DM300
                    from (
	                    select 0 as DM300
	                    union all
	                    select distinct DM300 
	                    from FinishingProcess
                    )a ");
            selectResult = DBProxy.Current.Select(null, sql, out this.dtFinishingProcessAll);
            if (!selectResult)
            {
                this.ShowErr(sql, selectResult);
            }
        }

        protected override void OnEditModeChanged()
        {
            base.OnEditModeChanged();
            this.comboPressing2DataSource();
        }

        private void comboPressing2DataSource()
        {
            if (this.comboPressing2 != null && this.CurrentMaintain != null)
            {
                if (this.EditMode && this.dtFinishingProcess != null)
                {
                    MyUtility.Tool.SetupCombox(this.comboPressing2, 1, this.dtFinishingProcess);
                    this.comboPressing2.DisplayMember = "DM300";
                }

                if (!this.EditMode && this.dtFinishingProcessAll != null)
                {
                    MyUtility.Tool.SetupCombox(this.comboPressing2, 1, this.dtFinishingProcessAll);
                    this.comboPressing2.DisplayMember = "DM300";
                }

                this.comboPressing2.Text = MyUtility.Convert.GetString(this.CurrentMaintain["Pressing2"]);
            }
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.displaySpecialMark.Value = MyUtility.GetValue.Lookup(string.Format("select Name from Reason WITH (NOLOCK) where ReasonTypeID = 'Style_SpecialMark' and ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["SpecialMark"])));
            this.txttpeuserSMR.DisplayBox1Binding = MyUtility.Convert.GetString(this.CurrentMaintain["Phase"]) == "1" ? MyUtility.Convert.GetString(this.CurrentMaintain["SampleSMR"]) : MyUtility.Convert.GetString(this.CurrentMaintain["BulkSMR"]);
            this.txttpeuserHandle.DisplayBox1Binding = MyUtility.Convert.GetString(this.CurrentMaintain["Phase"]) == "1" ? MyUtility.Convert.GetString(this.CurrentMaintain["SampleMRHandle"]) : MyUtility.Convert.GetString(this.CurrentMaintain["BulkMRHandle"]);
            this.displayStyleApprove.Value = MyUtility.Convert.GetString(this.CurrentMaintain["ApvName"]) + " " + MyUtility.GetValue.Lookup(string.Format("select (Name + ' #' + ExtNo) as NameExtNo from TPEPass1 WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["ApvName"])));
            this.numCPUAdjusted.Value = MyUtility.Check.Empty(this.CurrentMaintain["CPUAdjusted"]) ? 0 : MyUtility.Convert.GetDecimal(this.CurrentMaintain["CPUAdjusted"]) * 100m;

            /*判斷路徑下圖片檔找不到,就將ImageLocation帶空值*/
            if (MyUtility.Check.Empty(this.CurrentMaintain["Picture1"]))
            {
                this.pictureBox1.ImageLocation = string.Empty;
            }
            else
            {
                if (File.Exists(this.destination_path + MyUtility.Convert.GetString(this.CurrentMaintain["Picture1"])))
                {
                    try
                    {
                        this.pictureBox1.ImageLocation = this.destination_path + MyUtility.Convert.GetString(this.CurrentMaintain["Picture1"]);
                    }
                    catch (Exception e)
                    {
                        MyUtility.Msg.WarningBox("Picture1 process error. Please check it !!" + Environment.NewLine + e.ToString());
                        this.pictureBox1.ImageLocation = string.Empty;
                    }
                }
                else
                {
                    this.pictureBox1.ImageLocation = string.Empty;
                }
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["Picture2"]))
            {
                this.pictureBox2.ImageLocation = string.Empty;
            }
            else
            {
                if (File.Exists(this.destination_path + MyUtility.Convert.GetString(this.CurrentMaintain["Picture2"])))
                {
                    try
                    {
                        this.pictureBox2.ImageLocation = this.destination_path + MyUtility.Convert.GetString(this.CurrentMaintain["Picture2"]);
                    }
                    catch (Exception e)
                    {
                        MyUtility.Msg.WarningBox("Picture2 process error. Please check it !!" + Environment.NewLine + e.ToString());
                        this.pictureBox2.ImageLocation = string.Empty;
                    }
                }
                else
                {
                    this.pictureBox2.ImageLocation = string.Empty;
                }
            }

            this.btnTMSCost.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Style_TMSCost WITH (NOLOCK) where StyleUkey = {0}", MyUtility.Convert.GetString(this.CurrentMaintain["UKey"]))) ? Color.Blue : Color.Black;
            this.btnStdGSD.ForeColor = MyUtility.Check.Seek(string.Format("select ID from IETMS WITH (NOLOCK) where ID = '{0}' and Version = '{1}'", MyUtility.Convert.GetString(this.CurrentMaintain["IETMSID"]), MyUtility.Convert.GetString(this.CurrentMaintain["IETMSVersion"]))) ? Color.Blue : Color.Black;
            this.btnFTYGSD.ForeColor = MyUtility.Check.Seek(string.Format("select StyleID from TimeStudy WITH (NOLOCK) where StyleID = '{0}' and BrandID = '{1}' and SeasonID = '{2}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), MyUtility.Convert.GetString(this.CurrentMaintain["BrandID"]), MyUtility.Convert.GetString(this.CurrentMaintain["SeasonID"]))) ? Color.Blue : Color.Black;
            this.btnProductionKits.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Style_ProductionKits WITH (NOLOCK) where StyleUkey = {0}", MyUtility.Convert.GetString(this.CurrentMaintain["UKey"]))) ? Color.Blue : Color.Black;
            this.btnArtwork.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Style_Artwork WITH (NOLOCK) where StyleUkey = {0}", MyUtility.Convert.GetString(this.CurrentMaintain["UKey"]))) ? Color.Blue : Color.Black;
            this.btnQtyCartonbyCustCD.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Style_QtyCTN WITH (NOLOCK) where StyleUkey = {0}", MyUtility.Convert.GetString(this.CurrentMaintain["UKey"]))) ? Color.Blue : Color.Black;
            this.btnWeightdata.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Style_WeightData WITH (NOLOCK) where StyleUkey = {0}", MyUtility.Convert.GetString(this.CurrentMaintain["UKey"]))) ? Color.Blue : Color.Black;
            this.btnGarmentList.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Pattern WITH (NOLOCK) where StyleUkey = {0}", MyUtility.Convert.GetString(this.CurrentMaintain["UKey"]))) ? Color.Blue : Color.Black;
            this.btnSimilarStyle.ForeColor = (MyUtility.Check.Seek(string.Format("select MasterStyleUkey from Style_SimilarStyle WITH (NOLOCK) where MasterStyleUkey = {0}", MyUtility.Convert.GetString(this.CurrentMaintain["UKey"]))) || MyUtility.Check.Seek(string.Format("select ChildrenStyleUkey  from Style_SimilarStyle where ChildrenStyleUkey = {0}", MyUtility.Convert.GetString(this.CurrentMaintain["UKey"])))) ? Color.Blue : Color.Black;
            this.btnHSCode.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Style_HSCode WITH (NOLOCK) where StyleUkey = {0}", MyUtility.Convert.GetString(this.CurrentMaintain["UKey"]))) ? Color.Blue : Color.Black;
            this.btnFtyLT.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Style_GMTLTFty WITH (NOLOCK) where StyleUkey = {0}", MyUtility.Convert.GetString(this.CurrentMaintain["UKey"]))) ? Color.Blue : Color.Black;
            this.btnComboType.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Style_Location WITH (NOLOCK) where StyleUkey = {0}", MyUtility.Convert.GetString(this.CurrentMaintain["UKey"]))) ? Color.Blue : Color.Black;
            if (!MyUtility.Check.Empty(this.CurrentMaintain["ApvDate"]))
            {
                DateTime? lastTime = (DateTime?)this.CurrentMaintain["ApvDate"];
                string ftyLastupdate = lastTime == null ? string.Empty : ((DateTime)lastTime).ToString("yyyy/MM/dd HH:mm:ss");
                this.displayStyleApprove2.Text = ftyLastupdate;
            }
            else
            {
                this.displayStyleApprove2.Text = string.Empty;
            }

            #region LocalStyle Enable [Attach][Delete] Button
            if (MyUtility.Check.Empty(this.CurrentMaintain["LocalStyle"]))
            {
                this.btnPicture1Attach.Visible = false;
                this.btnPicture2Attach.Visible = false;
                this.btnPicture1Delete.Visible = false;
                this.btnPicture2Delete.Visible = false;
            }
            else
            {
                this.btnPicture1Attach.Visible = true;
                this.btnPicture2Attach.Visible = true;
                this.btnPicture1Delete.Visible = true;
                this.btnPicture2Delete.Visible = true;
            }
            #endregion

            // 寫入TPE Edit By
            this.txtTPEEditBy.Text = MyUtility.GetValue.Lookup($@"
select Name = s.TPEEditName+' '+ isnull(p.Name,'')+' '+isnull(format(s.TPEEditDate,'yyyy/MM/dd hh:mm:ss'),'') 
from Style s
left join Pass1 p on s.TPEEditName = p.ID
where s.ukey = {this.CurrentMaintain["ukey"]}");

            this.comboPressing2DataSource();
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["LocalStyle"] = 1;
            this.CurrentMaintain["LocalMR"] = Env.User.UserID;
            this.displayStyleApprove2.Text = string.Empty;
            this.ComboPressing1_SelectedIndexChanged(null, null);
            this.ComboFolding1_SelectedIndexChanged(null, null);
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            this.txtStyleNo.ReadOnly = true;
            this.txtSeason.ReadOnly = true;
            this.txtBrand.ReadOnly = true;
            this.ComboPressing1_SelectedIndexChanged(null, null);
            this.ComboFolding1_SelectedIndexChanged(null, null);

            if (MyUtility.Convert.GetString(this.CurrentMaintain["LocalStyle"]).ToUpper() == "FALSE")
            {
                this.txtProgram.ReadOnly = true;
                this.txtDescription.ReadOnly = true;
                this.txtStyleName.ReadOnly = true;
                this.txtModel.ReadOnly = true;
                this.txtSizePage.ReadOnly = true;
                this.txtCareCode.ReadOnly = true;
                this.numGarmentLT.ReadOnly = true;
                this.numQtyperCtn.ReadOnly = true;
                this.txtcdcode.ReadOnly = true;
                this.checkRainwearTestRequest.ReadOnly = true;
                this.checkJnuk.ReadOnly = true;
                this.comboSizeUnit.ReadOnly = true;
                this.comboGender.ReadOnly = true;
            }

            if (MyUtility.Convert.GetString(this.CurrentMaintain["NoNeedPPMeeting"]).ToUpper() == "TRUE")
            {
                this.datePPMeeting.ReadOnly = true;
            }
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            #region 檢查必輸欄位
            if (MyUtility.Convert.GetString(this.CurrentMaintain["LocalStyle"]).ToUpper() == "TRUE")
            {
                if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
                {
                    MyUtility.Msg.WarningBox("Style# can't empty");
                    this.txtStyleNo.Focus();
                    return false;
                }

                if (MyUtility.Check.Empty(this.CurrentMaintain["SeasonID"]))
                {
                    MyUtility.Msg.WarningBox("Season can't empty");
                    this.txtSeason.Focus();
                    return false;
                }

                if (MyUtility.Check.Empty(this.CurrentMaintain["BrandID"]))
                {
                    MyUtility.Msg.WarningBox("Brand can't empty");
                    this.txtBrand.Focus();
                    return false;
                }

                if (MyUtility.Check.Empty(this.CurrentMaintain["Description"]))
                {
                    MyUtility.Msg.WarningBox("Description can't empty");
                    this.txtDescription.Focus();
                    return false;
                }

                if (MyUtility.Check.Empty(this.CurrentMaintain["StyleName"]))
                {
                    MyUtility.Msg.WarningBox("Style name can't empty");
                    this.txtStyleName.Focus();
                    return false;
                }

                if (MyUtility.Check.Empty(this.CurrentMaintain["CdCodeID"]))
                {
                    MyUtility.Msg.WarningBox("CD can't empty");
                    this.txtcdcode.Focus();
                    return false;
                }

                if (MyUtility.Check.Empty(this.CurrentMaintain["CPU"]))
                {
                    MyUtility.Msg.WarningBox("CPU can't empty");
                    this.numCPU.Focus();
                    return false;
                }
            }
            #endregion
            if (this.IsDetailInserting)
            {
                // 檢查Style+Brand+Season是否已存在
                if (MyUtility.Check.Seek(string.Format("select Id from Style WITH (NOLOCK) where ID = '{0}' and BrandID = '{1}' and SeasonID = '{2}'", MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), MyUtility.Convert.GetString(this.CurrentMaintain["BrandID"]), MyUtility.Convert.GetString(this.CurrentMaintain["SeasonID"]))))
                {
                    MyUtility.Msg.WarningBox("This style already exist!!");
                    return false;
                }

                DataTable styleUkey;
                string sqlCmd = "select MIN(Ukey)-1 as NewUkey from Style WITH (NOLOCK) ";
                DualResult result = DBProxy.Current.Select(null, sqlCmd, out styleUkey);
                if (!result)
                {
                    MyUtility.Msg.ErrorBox("Get Ukey fail!!\r\n" + result.ToString());
                    return false;
                }

                this.CurrentMaintain["Ukey"] = MyUtility.Convert.GetString(styleUkey.Rows[0]["NewUkey"]);
            }

            return true;
        }

        /// <inheritdoc/>
        protected override void ClickSaveAfter()
        {
            List<SqlParameter> cmds = new List<SqlParameter>();
            cmds.Add(new SqlParameter("@k", this.CurrentMaintain["Ukey"].ToString()));
            string sqlcmd = "Select StyleUkey From Style_TmsCost WITH (NOLOCK) where StyleUkey = @k";

            // 若沒有對應Ukey資料則新增
            if (!MyUtility.Check.Seek(sqlcmd, cmds))
            {
                cmds.Add(new SqlParameter("@N", Env.User.UserID));
                StringBuilder splcmdin = new StringBuilder();
                splcmdin.Append(@"INSERT INTO Style_TmsCost (Seq,ArtworkTypeID,ArtworkUnit,StyleUkey,AddDate,AddName)
                                SELECT A.seq,A.ID,A.ArtworkUnit,@k,GETDATE(),@N
                                FROM Artworktype A 
                                where ISNUMERIC(A.seq)=1");
                DualResult result = DBProxy.Current.Execute(null, splcmdin.ToString(), cmds);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "insert TmdCost fail!!\r\n" + result.ToString());
                    return;
                }
            }

            base.ClickSaveAfter();

            #region ISP20201344 資料交換 - Sunrise
            if (Sunrise_FinishingProcesses.IsSunrise_FinishingProcessesEnable)
            {
                string styleKey = $"{this.CurrentMaintain["ID"]}`{this.CurrentMaintain["SeasonID"]}`{this.CurrentMaintain["BrandID"]}";
                Task.Run(() => DBProxy.Current.Execute(null, $"exec dbo.SentStyleFPSSettingToFinishingProcesses '{styleKey}'"))
                .ContinueWith(UtilityAutomation.AutomationExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
            }
            #endregion
        }

        /// <inheritdoc/>
        protected override DualResult ClickSavePre()
        {
            if (MyUtility.Convert.GetString(this.CurrentMaintain["LocalStyle"]).ToUpper() == "TRUE" && !MyUtility.Check.Empty(this.CurrentMaintain["UKey"]))
            {
                string sqlCmd = string.Format(
                    "update Orders set CPU = {0}, CdCodeID = '{1}', StyleUnit = '{2}' where Orders.StyleUkey = {3} and not exists (select 1 from SewingOutput_Detail where OrderId = Orders.ID)",
                    MyUtility.Convert.GetString(this.CurrentMaintain["CPU"]),
                    MyUtility.Convert.GetString(this.CurrentMaintain["CdCodeID"]),
                    MyUtility.Convert.GetString(this.CurrentMaintain["StyleUnit"]),
                    MyUtility.Convert.GetString(this.CurrentMaintain["UKey"]));

                DualResult result = DBProxy.Current.Execute(null, sqlCmd);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Update order cpu fail!!\r\n" + result.ToString());
                    return failResult;
                }
            }

            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            P04_Print callNextForm = new P04_Print();
            callNextForm.ShowDialog(this);
            return base.ClickPrint();
        }

        private void TxtSeason_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Win.Tools.SelectItem item;
            string sqlCmd = "select distinct ID from Season WITH (NOLOCK) where Junk = 0 order by ID desc";
            item = new Win.Tools.SelectItem(sqlCmd, "11", this.Text);
            item.Width = 300;
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.txtSeason.Text = item.GetSelectedString();
        }

        // Brand
        private void TxtBrand_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode && MyUtility.Convert.GetString(this.CurrentMaintain["LocalStyle"]).ToUpper() == "TRUE" && this.txtBrand.OldValue != this.txtBrand.Text)
            {
                if (!MyUtility.Check.Empty(this.txtBrand.Text))
                {
                    if (this.EnterWrongChar(this.txtBrand.Text))
                    {
                        this.CurrentMaintain["BrandID"] = string.Empty;
                        e.Cancel = true;
                        return;
                    }
                    else
                    {
                        // 舊系統任意輸入都可以，感覺沒有檢核才對，先註解
                        // if (MyUtility.Check.Seek(string.Format("select ID from Brand WITH (NOLOCK) where ID = '{0}'", txtBrand.Text)))
                        // {
                        //    CurrentMaintain["BrandID"] = "";
                        //    e.Cancel = true;
                        //    MyUtility.Msg.WarningBox(string.Format("Brand:{0} is belong to SCI, Factory can't use!!", txtBrand.Text));
                        //    return;
                        // }
                    }
                }
            }
        }

        // Program
        private void TxtProgram_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlCmd = string.Format("Select id,BrandID from Program WITH (NOLOCK) where BrandID = '{0}'", MyUtility.Convert.GetString(this.CurrentMaintain["BrandID"]));
            Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlCmd, "12,8", this.Text, false, ",");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel)
            {
                return;
            }

            this.txtProgram.Text = item.GetSelectedString();
        }

        // CD
        private void Txtcdcode_Validated(object sender, EventArgs e)
        {
            if (this.EditMode && MyUtility.Convert.GetString(this.CurrentMaintain["LocalStyle"]).ToUpper() == "TRUE" && this.txtcdcode.OldValue != this.txtcdcode.Text)
            {
                if (MyUtility.Check.Empty(this.txtcdcode.Text))
                {
                    this.CurrentMaintain["CPU"] = 0;
                    this.CurrentMaintain["StyleUnit"] = string.Empty;
                }
                else
                {
                    DataRow cDCodeRow;
                    if (MyUtility.Check.Seek(string.Format("select Cpu,ComboPcs from CDCode WITH (NOLOCK) where ID = '{0}'", this.txtcdcode.Text), out cDCodeRow))
                    {
                        this.CurrentMaintain["CPU"] = cDCodeRow["Cpu"];
                    }

                    this.CurrentMaintain["StyleUnit"] = MyUtility.Convert.GetString(cDCodeRow["ComboPcs"]) == "1" ? "PCS" : "SETS";
                }
            }
        }

        // No need PP Meeting
        private void CheckNoneedPPMeeting_CheckedChanged(object sender, EventArgs e)
        {
           if (this.EditMode)
            {
                this.CurrentMaintain["NoNeedPPMeeting"] = this.checkNoneedPPMeeting.Checked;
                this.datePPMeeting.ReadOnly = this.checkNoneedPPMeeting.Value.ToUpper() == "TRUE";
            }
        }

        // TMS & Cost
        private void BtnTMSCost_Click(object sender, EventArgs e)
        {
            P04_TMSAndCost callNextForm = new P04_TMSAndCost(PublicPrg.Prgs.GetAuthority(Env.User.UserID, "P04. Style Management", "CanEdit") && MyUtility.Convert.GetString(this.CurrentMaintain["LocalStyle"]).ToUpper() == "TRUE", MyUtility.Convert.GetString(this.CurrentMaintain["UKey"]), null, null, MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
            callNextForm.ShowDialog(this);

            // 按鈕變色
            this.btnTMSCost.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Style_TMSCost WITH (NOLOCK) where StyleUkey = {0}", MyUtility.Convert.GetString(this.CurrentMaintain["UKey"]))) ? Color.Blue : Color.Black;
            this.RenewData();
            this.OnDetailEntered();
        }

        // Std. GSD
        private void BtnStdGSD_Click(object sender, EventArgs e)
        {
            PublicForm.StdGSDList callNextForm = new PublicForm.StdGSDList(MyUtility.Convert.GetLong(this.CurrentMaintain["UKey"]));
            callNextForm.ShowDialog(this);
        }

        // FTY GSD
        private void BtnFTYGSD_Click(object sender, EventArgs e)
        {
            IE.P01 callNextForm = new IE.P01(MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), MyUtility.Convert.GetString(this.CurrentMaintain["BrandID"]), MyUtility.Convert.GetString(this.CurrentMaintain["SeasonID"]), null);
            callNextForm.ShowDialog(this);
        }

        // Production Kits
        private void BtnProductionKits_Click(object sender, EventArgs e)
        {
            P01_ProductionKit callNextForm = new P01_ProductionKit(true, MyUtility.Convert.GetString(this.CurrentMaintain["UKey"]), null, null, MyUtility.Convert.GetString(this.CurrentMaintain["ID"]));
            callNextForm.ShowDialog(this);
        }

        // Artwork
        private void BtnArtwork_Click(object sender, EventArgs e)
        {
            P04_Artwork callNextForm = new P04_Artwork(PublicPrg.Prgs.GetAuthority(Env.User.UserID, "P04. Style Management", "CanEdit") && MyUtility.Convert.GetString(this.CurrentMaintain["LocalStyle"]).ToUpper() == "TRUE", MyUtility.Convert.GetString(this.CurrentMaintain["UKey"]), null, null, MyUtility.Convert.GetString(this.CurrentMaintain["ID"]), MyUtility.Convert.GetString(this.CurrentMaintain["SeasonID"]));
            callNextForm.ShowDialog(this);

            // 按鈕變色
            this.btnArtwork.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Style_Artwork WITH (NOLOCK) where StyleUkey = {0}", MyUtility.Convert.GetString(this.CurrentMaintain["UKey"]))) ? Color.Blue : Color.Black;
        }

        // Q'ty/Carton by CustCD
        private void BtnQtyCartonbyCustCD_Click(object sender, EventArgs e)
        {
            P04_QtyCartonByCustCD callNextForm = new P04_QtyCartonByCustCD(MyUtility.Convert.GetString(this.CurrentMaintain["UKey"]));
            callNextForm.ShowDialog(this);
        }

        // Weight data
        private void BtnWeightdata_Click(object sender, EventArgs e)
        {
            // 自動新增不存在Style_WeightData的sIZE資料
            string insertCmd = string.Format(
                @"
insert into Style_WeightData(StyleUkey,Article,SizeCode,AddName,AddDate)
select a.StyleUkey,'----',a.SizeCode,'{0}',GETDATE()
from (
select ss.StyleUkey,ss.SizeCode,sw.Article 
from Style_SizeCode ss
left join Style_WeightData sw on ss.StyleUkey = sw.StyleUkey and ss.SizeCode = sw.SizeCode
where ss.StyleUkey = {1}) a
where a.Article is null",
                Env.User.UserID,
                MyUtility.Convert.GetString(this.CurrentMaintain["UKey"]));

            DualResult result = DBProxy.Current.Execute(null, insertCmd);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Insert data to Style_WeightData fail!\r\n" + result.ToString());
            }

            P04_WeightData callNextForm = new P04_WeightData(PublicPrg.Prgs.GetAuthority(Env.User.UserID, "P04. Style Management", "CanEdit"), MyUtility.Convert.GetString(this.CurrentMaintain["UKey"]), null, null);
            callNextForm.ShowDialog(this);

            // 按鈕變色
            this.btnWeightdata.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Style_WeightData WITH (NOLOCK) where StyleUkey = {0}", MyUtility.Convert.GetString(this.CurrentMaintain["UKey"]))) ? Color.Blue : Color.Black;
        }

        // Garment List
        private void BtnGarmentList_Click(object sender, EventArgs e)
        {
            PublicForm.GarmentList callNextForm = new PublicForm.GarmentList(MyUtility.Convert.GetString(this.CurrentMaintain["Ukey"]), null, null);
            callNextForm.ShowDialog(this);
        }

        // Similar Style
        private void BtnSimilarStyle_Click(object sender, EventArgs e)
        {
            P04_SimilarStyle callNextForm = new P04_SimilarStyle(MyUtility.Convert.GetString(this.CurrentMaintain["UKey"]));
            callNextForm.ShowDialog(this);
        }

        // HS Code
        private void BtnHSCode_Click(object sender, EventArgs e)
        {
            P04_HSCode callNextForm = new P04_HSCode(MyUtility.Convert.GetString(this.CurrentMaintain["UKey"]));
            callNextForm.ShowDialog(this);
        }

        // Fty L/T
        private void BtnFtyLT_Click(object sender, EventArgs e)
        {
            P04_GarmentLeadTimeByFactory callNextForm = new P04_GarmentLeadTimeByFactory(false, MyUtility.Convert.GetString(this.CurrentMaintain["UKey"]), null, null);
            callNextForm.ShowDialog(this);
        }

        // Picture1 Attach
        private void BtnPicture1Attach_Click(object sender, EventArgs e)
        {
            // 呼叫File 選擇視窗
            OpenFileDialog file = new OpenFileDialog();
            Stream fileOpened = null;

            file.InitialDirectory = "c:\\"; // 預設路徑
            file.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*"; // 使用檔名
            file.FilterIndex = 1;
            file.RestoreDirectory = true;
            if (file.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((fileOpened = file.OpenFile()) != null)
                    {
                        {
                            string local_path_file = file.FileName;
                            string local_file_type = Path.GetExtension(local_path_file);
                            string destination_fileName = MyUtility.Convert.GetString(this.CurrentMaintain["UKey"]).Trim() + "-1" + local_file_type;

                            File.Copy(local_path_file, this.destination_path + destination_fileName, true);

                            // update picture1 path
                            DualResult result = DBProxy.Current.Execute(null, "update Style set Picture1 ='" + destination_fileName.Trim() + "' where ukey=" + this.CurrentMaintain["UKey"]);
                            this.CurrentMaintain["Picture1"] = this.destination_path.Trim() + destination_fileName.Trim();
                            this.pictureBox1.ImageLocation = MyUtility.Convert.GetString(this.CurrentMaintain["Picture1"]);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MyUtility.Msg.ErrorBox("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        // Picture1 Delete
        private void BtnPicture1Delete_Click(object sender, EventArgs e)
        {
            DialogResult deleteResult1 = MyUtility.Msg.QuestionBox("Are you sure delete the < Picture1 >?", buttons: MessageBoxButtons.YesNo);
            if (deleteResult1 == DialogResult.Yes)
            {
                if (File.Exists(this.destination_path + MyUtility.Convert.GetString(this.CurrentMaintain["Picture1"])))
                {
                    try
                    {
                        File.Delete(this.destination_path + MyUtility.Convert.GetString(this.CurrentMaintain["Picture1"]));
                        this.CurrentMaintain["Picture1"] = string.Empty;
                        this.pictureBox1.ImageLocation = MyUtility.Convert.GetString(this.CurrentMaintain["Picture1"]);
                        DualResult result = DBProxy.Current.Execute(null, string.Format("update Style set Picture1='' where UKey={0}", MyUtility.Convert.GetString(this.CurrentMaintain["UKey"])));
                        if (!result)
                        {
                            MyUtility.Msg.ErrorBox("Update data fail!!\r\n" + result.ToString());
                            return;
                        }
                    }
                    catch (IOException exception)
                    {
                        MyUtility.Msg.ErrorBox("Error: Delete file fail. Original error: " + exception.Message);
                    }
                }
                else
                {
                    this.CurrentMaintain["Picture1"] = string.Empty;
                    this.pictureBox1.ImageLocation = MyUtility.Convert.GetString(this.CurrentMaintain["Picture1"]);
                    DualResult result = DBProxy.Current.Execute(null, string.Format("update Style set Picture1='' where UKey={0}", MyUtility.Convert.GetString(this.CurrentMaintain["UKey"])));
                    if (!result)
                    {
                        MyUtility.Msg.ErrorBox("Update data fail!!\r\n" + result.ToString());
                        return;
                    }
                }
            }
        }

        // Picture2 Attach
        private void BtnPicture2Attach_Click(object sender, EventArgs e)
        {
            // 呼叫File 選擇視窗
            OpenFileDialog file = new OpenFileDialog();
            Stream fileOpened = null;

            file.InitialDirectory = "c:\\"; // 預設路徑
            file.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*"; // 使用檔名
            file.FilterIndex = 1;
            file.RestoreDirectory = true;
            if (file.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((fileOpened = file.OpenFile()) != null)
                    {
                        {
                            string local_path_file = file.FileName;
                            string local_file_type = Path.GetExtension(local_path_file);
                            string destination_fileName = MyUtility.Convert.GetString(this.CurrentMaintain["UKey"]).Trim() + "-2" + local_file_type;

                            File.Copy(local_path_file, this.destination_path + destination_fileName, true);

                            // update picture1 path
                            DualResult result = DBProxy.Current.Execute(null, "update Style set Picture2 ='" + destination_fileName.Trim() + "' where ukey=" + this.CurrentMaintain["UKey"]);
                            this.CurrentMaintain["Picture2"] = this.destination_path.Trim() + destination_fileName.Trim();
                            this.pictureBox2.ImageLocation = MyUtility.Convert.GetString(this.CurrentMaintain["Picture2"]);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MyUtility.Msg.ErrorBox("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        // Picture2 Delete
        private void BtnPicture2Delete_Click(object sender, EventArgs e)
        {
            DialogResult deleteResult1 = MyUtility.Msg.QuestionBox("Are you sure delete the < Picture2 >?", buttons: MessageBoxButtons.YesNo);
            if (deleteResult1 == DialogResult.Yes)
            {
                if (File.Exists(this.destination_path + MyUtility.Convert.GetString(this.CurrentMaintain["Picture2"])))
                {
                    try
                    {
                        File.Delete(this.destination_path + MyUtility.Convert.GetString(this.CurrentMaintain["Picture2"]));
                        this.CurrentMaintain["Picture2"] = string.Empty;
                        this.pictureBox2.ImageLocation = MyUtility.Convert.GetString(this.CurrentMaintain["Picture2"]);
                        DualResult result = DBProxy.Current.Execute(null, string.Format("update Style set Picture2='' where UKey={0}", MyUtility.Convert.GetString(this.CurrentMaintain["UKey"])));
                        if (!result)
                        {
                            MyUtility.Msg.ErrorBox("Update data fail!!\r\n" + result.ToString());
                            return;
                        }
                    }
                    catch (IOException exception)
                    {
                        MyUtility.Msg.ErrorBox("Error: Delete file fail. Original error: " + exception.Message);
                    }
                }
                else
                {
                    this.CurrentMaintain["Picture2"] = string.Empty;
                    this.pictureBox2.ImageLocation = MyUtility.Convert.GetString(this.CurrentMaintain["Picture2"]);
                    DualResult result = DBProxy.Current.Execute(null, string.Format("update Style set Picture2='' where UKey={0}", MyUtility.Convert.GetString(this.CurrentMaintain["UKey"])));
                    if (!result)
                    {
                        MyUtility.Msg.ErrorBox("Update data fail!!\r\n" + result.ToString());
                        return;
                    }
                }
            }
        }

        // Combo Type
        private void BtnComboType_Click(object sender, EventArgs e)
        {
            P04_ComboType callNextForm = new P04_ComboType(PublicPrg.Prgs.GetAuthority(Env.User.UserID, "P04. Style Management", "CanEdit") && MyUtility.Convert.GetString(this.CurrentMaintain["LocalStyle"]).ToUpper() == "TRUE", MyUtility.Convert.GetString(this.CurrentMaintain["UKey"]), null, null, MyUtility.Convert.GetString(this.CurrentMaintain["StyleUnit"]));
            callNextForm.ShowDialog(this);

            // 按鈕變色
            this.btnComboType.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Style_Location WITH (NOLOCK) where StyleUkey = {0}", MyUtility.Convert.GetString(this.CurrentMaintain["UKey"]))) ? Color.Blue : Color.Black;
        }

        // 檢查是否有輸入'字元
        private bool EnterWrongChar(string enterData)
        {
            if (enterData.IndexOf("'") != -1)
            {
                MyUtility.Msg.WarningBox("Can not enter the  '  character!!");
                return true;
            }

            return false;
        }

        // Style
        private void TxtStyleNo_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode && MyUtility.Convert.GetString(this.CurrentMaintain["LocalStyle"]).ToUpper() == "TRUE" && this.txtStyleNo.OldValue != this.txtStyleNo.Text)
            {
                if (!MyUtility.Check.Empty(this.txtStyleNo.Text))
                {
                    if (this.EnterWrongChar(this.txtStyleNo.Text))
                    {
                        this.CurrentMaintain["StyleID"] = string.Empty;
                        e.Cancel = true;
                        return;
                    }
                }
            }
        }

        // Season
        private void TxtSeason_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode && MyUtility.Convert.GetString(this.CurrentMaintain["LocalStyle"]).ToUpper() == "TRUE" && this.txtSeason.OldValue != this.txtSeason.Text)
            {
                if (!MyUtility.Check.Empty(this.txtSeason.Text))
                {
                    if (this.EnterWrongChar(this.txtSeason.Text))
                    {
                        this.CurrentMaintain["SeasonID"] = string.Empty;
                        e.Cancel = true;
                        return;
                    }
                }
            }
        }

        // Program
        private void TxtProgram_Validating(object sender, CancelEventArgs e)
        {
            if (this.EditMode && MyUtility.Convert.GetString(this.CurrentMaintain["LocalStyle"]).ToUpper() == "TRUE" && this.txtProgram.OldValue != this.txtProgram.Text)
            {
                if (!MyUtility.Check.Empty(this.txtProgram.Text))
                {
                    if (this.EnterWrongChar(this.txtProgram.Text))
                    {
                        this.CurrentMaintain["ProgramID"] = string.Empty;
                        e.Cancel = true;
                        return;
                    }
                }
            }
        }

        private void ComboPressing1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.EditMode)
            {
                return;
            }

            int selectValue = MyUtility.Convert.GetInt(this.comboPressing1.SelectedValue2);
            if (string.IsNullOrEmpty(MyUtility.Convert.GetString(this.CurrentMaintain["Pressing1"])))
            {
                this.comboPressing1.SelectedValue2 = 0;
            }
            else
            {
                if (MyUtility.Convert.GetInt(this.CurrentMaintain["Pressing1"]) == selectValue)
                {
                    return;
                }
            }

            this.comboPressing1.DataBindings["SelectedValue2"].WriteValue();
            this.comboPressing2.SelectedValue2 = 0;
            this.comboPressing2.DataBindings["SelectedValue2"].WriteValue();
            switch (selectValue)
            {
                case 2:
                case 3:
                    this.comboPressing2.Enabled = true;
                    break;
                default:
                    this.comboPressing2.Enabled = false;
                    break;
            }
        }

        private void ComboFolding1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.EditMode)
            {
                return;
            }

            int selectValue = MyUtility.Convert.GetInt(this.comboFolding1.SelectedValue2);
            if (string.IsNullOrEmpty(MyUtility.Convert.GetString(this.CurrentMaintain["Folding1"])))
            {
                this.comboFolding1.SelectedIndex = 0;
            }
            else
            {
                if (MyUtility.Convert.GetInt(this.CurrentMaintain["Folding1"]) == selectValue)
                {
                    return;
                }
            }

            this.comboFolding1.DataBindings["SelectedValue2"].WriteValue();
            this.comboFolding2.SelectedIndex = 0;
            this.comboFolding2.DataBindings["SelectedValue2"].WriteValue();
            switch (selectValue)
            {
                case 1:
                    this.comboFolding2.Enabled = true;
                    break;
                default:
                    this.comboFolding2.Enabled = false;
                    break;
            }
        }
    }
}
