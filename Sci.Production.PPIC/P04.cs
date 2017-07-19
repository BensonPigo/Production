using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;
using System.IO;
using Sci.Production;
using System.Data.SqlClient;

namespace Sci.Production.PPIC
{
    public partial class P04 : Sci.Win.Tems.Input1
    {
        private string destination_path;//放圖檔的路徑
        public P04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            destination_path = MyUtility.GetValue.Lookup("select PicPath from System WITH (NOLOCK) ", null);
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            MyUtility.Tool.SetupCombox(comboSizeUnit, 1, 1, "CM,INCH");
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            displaySpecialMark.Value = MyUtility.GetValue.Lookup(string.Format("select Name from Reason WITH (NOLOCK) where ReasonTypeID = 'Style_SpecialMark' and ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["SpecialMark"])));
            txttpeuserSMR.DisplayBox1Binding = MyUtility.Convert.GetString(CurrentMaintain["Phase"]) == "1" ? MyUtility.Convert.GetString(CurrentMaintain["SampleSMR"]) : MyUtility.Convert.GetString(CurrentMaintain["BulkSMR"]);
            txttpeuserHandle.DisplayBox1Binding = MyUtility.Convert.GetString(CurrentMaintain["Phase"]) == "1" ? MyUtility.Convert.GetString(CurrentMaintain["SampleMRHandle"]) : MyUtility.Convert.GetString(CurrentMaintain["BulkMRHandle"]);
            displayStyleApprove.Value = MyUtility.Convert.GetString(CurrentMaintain["ApvName"]) + " " + MyUtility.GetValue.Lookup(string.Format("select (Name + ' #' + ExtNo) as NameExtNo from TPEPass1 WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ApvName"])));
            numCPUAdjusted.Value = MyUtility.Check.Empty(CurrentMaintain["CPUAdjusted"]) ? 0 : MyUtility.Convert.GetDecimal(CurrentMaintain["CPUAdjusted"]) * 100m;
            pictureBox1.ImageLocation = MyUtility.Check.Empty(CurrentMaintain["Picture1"]) ? null : destination_path + MyUtility.Convert.GetString(CurrentMaintain["Picture1"]);
            pictureBox2.ImageLocation = MyUtility.Check.Empty(CurrentMaintain["Picture2"]) ? null : destination_path + MyUtility.Convert.GetString(CurrentMaintain["Picture2"]);
            btnTMSCost.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Style_TMSCost WITH (NOLOCK) where StyleUkey = {0}", MyUtility.Convert.GetString(CurrentMaintain["UKey"]))) ? Color.Blue : Color.Black;
            btnStdGSD.ForeColor = MyUtility.Check.Seek(string.Format("select ID from IETMS WITH (NOLOCK) where ID = '{0}' and Version = '{1}'", MyUtility.Convert.GetString(CurrentMaintain["IETMSID"]), MyUtility.Convert.GetString(CurrentMaintain["IETMSVersion"]))) ? Color.Blue : Color.Black;
            btnFTYGSD.ForeColor = MyUtility.Check.Seek(string.Format("select StyleID from TimeStudy WITH (NOLOCK) where StyleID = '{0}' and BrandID = '{1}' and SeasonID = '{2}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]), MyUtility.Convert.GetString(CurrentMaintain["BrandID"]), MyUtility.Convert.GetString(CurrentMaintain["SeasonID"]))) ? Color.Blue : Color.Black;
            btnProductionKits.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Style_ProductionKits WITH (NOLOCK) where StyleUkey = {0}", MyUtility.Convert.GetString(CurrentMaintain["UKey"]))) ? Color.Blue : Color.Black;
            btnArtwork.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Style_Artwork WITH (NOLOCK) where StyleUkey = {0}", MyUtility.Convert.GetString(CurrentMaintain["UKey"]))) ? Color.Blue : Color.Black;
            btnQtyCartonbyCustCD.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Style_QtyCTN WITH (NOLOCK) where StyleUkey = {0}", MyUtility.Convert.GetString(CurrentMaintain["UKey"]))) ? Color.Blue : Color.Black;
            btnWeightdata.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Style_WeightData WITH (NOLOCK) where StyleUkey = {0}", MyUtility.Convert.GetString(CurrentMaintain["UKey"]))) ? Color.Blue : Color.Black;
            btnGarmentList.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Pattern WITH (NOLOCK) where StyleUkey = {0}", MyUtility.Convert.GetString(CurrentMaintain["UKey"]))) ? Color.Blue : Color.Black;
            btnSimilarStyle.ForeColor = (MyUtility.Check.Seek(string.Format("select MasterStyleUkey from Style_SimilarStyle WITH (NOLOCK) where MasterStyleUkey = {0}", MyUtility.Convert.GetString(CurrentMaintain["UKey"]))) || MyUtility.Check.Seek(string.Format("select ChildrenStyleUkey  from Style_SimilarStyle where ChildrenStyleUkey = {0}", MyUtility.Convert.GetString(CurrentMaintain["UKey"])))) ? Color.Blue : Color.Black;
            btnHSCode.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Style_HSCode WITH (NOLOCK) where StyleUkey = {0}", MyUtility.Convert.GetString(CurrentMaintain["UKey"]))) ? Color.Blue : Color.Black;
            btnFtyLT.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Style_GMTLTFty WITH (NOLOCK) where StyleUkey = {0}", MyUtility.Convert.GetString(CurrentMaintain["UKey"]))) ? Color.Blue : Color.Black;
            btnComboType.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Style_Location WITH (NOLOCK) where StyleUkey = {0}", MyUtility.Convert.GetString(CurrentMaintain["UKey"]))) ? Color.Blue : Color.Black;
            if (!MyUtility.Check.Empty(CurrentMaintain["ApvDate"]))
            {
                DateTime? lastTime = (DateTime?)this.CurrentMaintain["ApvDate"];
                string FtyLastupdate = lastTime == null ? "" : ((DateTime)lastTime).ToString("yyyy/MM/dd HH:mm:ss");
                this.displayStyleApprove2.Text = FtyLastupdate;
            }
            else
            {
                this.displayStyleApprove2.Text = "";
            }
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["LocalStyle"] = 1;
            CurrentMaintain["LocalMR"] = Sci.Env.User.UserID;
            this.displayStyleApprove2.Text = "";
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            txtStyleNo.ReadOnly = true;
            txtSeason.ReadOnly = true;
            txtBrand.ReadOnly = true;
          
            if (MyUtility.Convert.GetString(CurrentMaintain["LocalStyle"]).ToUpper() == "FALSE")
            {
                txtProgram.ReadOnly = true;
                txtDescription.ReadOnly = true;
                txtStyleName.ReadOnly = true;
                txtModel.ReadOnly = true;
                txtSizePage.ReadOnly = true;
                txtCareCode.ReadOnly = true;
                numGarmentLT.ReadOnly = true;
                numQtyperCtn.ReadOnly = true;
                txtcdcode.ReadOnly = true;
                checkRainwearTestRequest.ReadOnly = true;
                checkJnuk.ReadOnly = true;
                comboSizeUnit.ReadOnly = true; 
                comboGender.ReadOnly = true;
              
            }
            if (MyUtility.Convert.GetString(CurrentMaintain["NoNeedPPMeeting"]).ToUpper() == "TRUE")
            {
                datePPMeeting.ReadOnly = true;
            }
        }

        protected override bool ClickSaveBefore()
        {
            #region 檢查必輸欄位
            if (MyUtility.Convert.GetString(CurrentMaintain["LocalStyle"]).ToUpper() == "TRUE")
            {
                if (MyUtility.Check.Empty(CurrentMaintain["ID"]))
                {
                    MyUtility.Msg.WarningBox("Style# can't empty");
                    txtStyleNo.Focus();
                    return false;
                }
                if (MyUtility.Check.Empty(CurrentMaintain["SeasonID"]))
                {
                    MyUtility.Msg.WarningBox("Season can't empty");
                    txtSeason.Focus();
                    return false;
                }
                if (MyUtility.Check.Empty(CurrentMaintain["BrandID"]))
                {
                    MyUtility.Msg.WarningBox("Brand can't empty");
                    txtBrand.Focus();
                    return false;
                }
                if (MyUtility.Check.Empty(CurrentMaintain["Description"]))
                {
                    MyUtility.Msg.WarningBox("Description can't empty");
                    txtDescription.Focus();
                    return false;
                }
                if (MyUtility.Check.Empty(CurrentMaintain["StyleName"]))
                {
                    MyUtility.Msg.WarningBox("Style name can't empty");
                    txtStyleName.Focus();
                    return false;
                }
                if (MyUtility.Check.Empty(CurrentMaintain["CdCodeID"]))
                {
                    MyUtility.Msg.WarningBox("CD can't empty");
                    txtcdcode.Focus();
                    return false;
                }
                if (MyUtility.Check.Empty(CurrentMaintain["CPU"]))
                {
                    MyUtility.Msg.WarningBox("CPU can't empty");
                    numCPU.Focus();
                    return false;
                }
            }
            #endregion

            if (IsDetailInserting)
            {
                //檢查Style+Brand+Season是否已存在
                if (MyUtility.Check.Seek(string.Format("select Id from Style WITH (NOLOCK) where ID = '{0}' and BrandID = '{1}' and SeasonID = '{2}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]), MyUtility.Convert.GetString(CurrentMaintain["BrandID"]), MyUtility.Convert.GetString(CurrentMaintain["SeasonID"]))))
                {
                    MyUtility.Msg.WarningBox("This style already exist!!");
                    return false;
                }

                DataTable StyleUkey;
                string sqlCmd = "select MIN(Ukey)-1 as NewUkey from Style WITH (NOLOCK) ";
                DualResult result = DBProxy.Current.Select(null, sqlCmd, out StyleUkey);
                if (!result)
                {
                    MyUtility.Msg.ErrorBox("Get Ukey fail!!\r\n"+result.ToString());
                    return false;
                }
                CurrentMaintain["Ukey"] = MyUtility.Convert.GetString(StyleUkey.Rows[0]["NewUkey"]);
            }
            return true;
        }

        protected override void ClickSaveAfter()
        {
            List<SqlParameter> cmds = new List<SqlParameter>();
            cmds.Add(new SqlParameter("@k",CurrentMaintain["Ukey"].ToString()));
            string sqlcmd = "Select StyleUkey From Style_TmsCost WITH (NOLOCK) where StyleUkey = @k";
            //若沒有對應Ukey資料則新增
            if (!MyUtility.Check.Seek(sqlcmd, cmds))
            {
                cmds.Add(new SqlParameter("@N", Sci.Env.User.UserID));
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
        }
        

        protected override DualResult ClickSavePre()
        {
            if (MyUtility.Convert.GetString(CurrentMaintain["LocalStyle"]).ToUpper() == "TRUE" && !MyUtility.Check.Empty(CurrentMaintain["UKey"]))
            {
                string sqlCmd = string.Format("update Orders set CPU = {0}, CdCodeID = '{1}', StyleUnit = '{2}' where Orders.StyleUkey = {3} and not exists (select 1 from SewingOutput_Detail where OrderId = Orders.ID)",
                    MyUtility.Convert.GetString(CurrentMaintain["CPU"]), MyUtility.Convert.GetString(CurrentMaintain["CdCodeID"]), MyUtility.Convert.GetString(CurrentMaintain["StyleUnit"]), MyUtility.Convert.GetString(CurrentMaintain["UKey"]));
                DualResult result = DBProxy.Current.Execute(null, sqlCmd);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, "Update order cpu fail!!\r\n" + result.ToString());
                    return failResult;
                }
            }
            return Result.True;
        }

        protected override bool ClickPrint()
        {
            Sci.Production.PPIC.P04_Print callNextForm = new Sci.Production.PPIC.P04_Print();
            callNextForm.ShowDialog(this);
            return base.ClickPrint();
        }

        private void txtSeason_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Tools.SelectItem item;
            string sqlCmd = "select distinct ID from Season WITH (NOLOCK) where Junk = 0 order by ID desc";
            item = new Sci.Win.Tools.SelectItem(sqlCmd, "11", this.Text);
            item.Width = 300;
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            txtSeason.Text = item.GetSelectedString();
        }

        //Brand
        private void txtBrand_Validating(object sender, CancelEventArgs e)
        {
            if (EditMode && MyUtility.Convert.GetString(CurrentMaintain["LocalStyle"]).ToUpper() == "TRUE" && txtBrand.OldValue != txtBrand.Text)
            {
                if (!MyUtility.Check.Empty(txtBrand.Text))
                {
                    if (EnterWrongChar(txtBrand.Text))
                    {
                        CurrentMaintain["BrandID"] = "";
                        e.Cancel = true;
                        return;
                    }
                    else
                    {
                        if (MyUtility.Check.Seek(string.Format("select ID from Brand WITH (NOLOCK) where ID = '{0}'", txtBrand.Text)))
                        {
                            CurrentMaintain["BrandID"] = "";
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox(string.Format("Brand:{0} is belong to SCI, Factory can't use!!", txtBrand.Text));
                            return;
                        }
                    }
                }
            }
        }

        //Program
        private void txtProgram_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlCmd = string.Format("Select id,BrandID from Program WITH (NOLOCK) where BrandID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["BrandID"]));
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "12,8", this.Text, false, ",");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            txtProgram.Text = item.GetSelectedString();
        }

        //CD
        private void txtcdcode_Validated(object sender, EventArgs e)
        {
            if (EditMode && MyUtility.Convert.GetString(CurrentMaintain["LocalStyle"]).ToUpper() == "TRUE" && txtcdcode.OldValue != txtcdcode.Text)
            {
                if (MyUtility.Check.Empty(txtcdcode.Text))
                {
                    CurrentMaintain["CPU"] = 0;
                    CurrentMaintain["StyleUnit"] = "";
                }
                else
                {
                    DataRow CDCodeRow;
                    if (MyUtility.Check.Seek(string.Format("select Cpu,ComboPcs from CDCode WITH (NOLOCK) where ID = '{0}'", txtcdcode.Text), out CDCodeRow))
                    CurrentMaintain["CPU"] = CDCodeRow["Cpu"];
                    CurrentMaintain["StyleUnit"] = MyUtility.Convert.GetString(CDCodeRow["ComboPcs"]) == "1" ? "PCS" : "SETS";
                }
            }
        }

        //No need PP Meeting
        private void checkNoneedPPMeeting_CheckedChanged(object sender, EventArgs e)
        {  
           if (EditMode)
            {
                CurrentMaintain["NoNeedPPMeeting"] = checkNoneedPPMeeting.Checked;
                datePPMeeting.ReadOnly = checkNoneedPPMeeting.Value.ToUpper() == "TRUE";      
            }
        }

        //TMS & Cost
        private void btnTMSCost_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P04_TMSAndCost callNextForm = new Sci.Production.PPIC.P04_TMSAndCost((PublicPrg.Prgs.GetAuthority(Sci.Env.User.UserID, "P04. Style Management", "CanEdit") && MyUtility.Convert.GetString(CurrentMaintain["LocalStyle"]).ToUpper() == "TRUE"), MyUtility.Convert.GetString(CurrentMaintain["UKey"]), null, null, MyUtility.Convert.GetString(CurrentMaintain["ID"]));
            callNextForm.ShowDialog(this);
            //按鈕變色
            btnTMSCost.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Style_TMSCost WITH (NOLOCK) where StyleUkey = {0}", MyUtility.Convert.GetString(CurrentMaintain["UKey"]))) ? Color.Blue : Color.Black;
            RenewData();
            OnDetailEntered();
        }

        //Std. GSD
        private void btnStdGSD_Click(object sender, EventArgs e)
        {
            Sci.Production.PublicForm.StdGSDList callNextForm = new Sci.Production.PublicForm.StdGSDList(MyUtility.Convert.GetLong(CurrentMaintain["UKey"]));
            callNextForm.ShowDialog(this);
        }

        //FTY GSD
        private void btnFTYGSD_Click(object sender, EventArgs e)
        {
            Sci.Production.IE.P01 callNextForm = new Sci.Production.IE.P01(MyUtility.Convert.GetString(CurrentMaintain["ID"]), MyUtility.Convert.GetString(CurrentMaintain["BrandID"]), MyUtility.Convert.GetString(CurrentMaintain["SeasonID"]), null);
            callNextForm.ShowDialog(this);
        }

        //Production Kits
        private void btnProductionKits_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_ProductionKit callNextForm = new Sci.Production.PPIC.P01_ProductionKit(true, MyUtility.Convert.GetString(CurrentMaintain["UKey"]), null, null, MyUtility.Convert.GetString(CurrentMaintain["ID"]));
            callNextForm.ShowDialog(this);
        }

        //Artwork
        private void btnArtwork_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P04_Artwork callNextForm = new Sci.Production.PPIC.P04_Artwork((PublicPrg.Prgs.GetAuthority(Sci.Env.User.UserID, "P04. Style Management", "CanEdit") && MyUtility.Convert.GetString(CurrentMaintain["LocalStyle"]).ToUpper() == "TRUE"), MyUtility.Convert.GetString(CurrentMaintain["UKey"]), null, null, MyUtility.Convert.GetString(CurrentMaintain["ID"]), MyUtility.Convert.GetString(CurrentMaintain["SeasonID"]));
            callNextForm.ShowDialog(this);
            //按鈕變色
            btnArtwork.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Style_Artwork WITH (NOLOCK) where StyleUkey = {0}", MyUtility.Convert.GetString(CurrentMaintain["UKey"]))) ? Color.Blue : Color.Black;
        }

        //Q'ty/Carton by CustCD
        private void btnQtyCartonbyCustCD_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P04_QtyCartonByCustCD callNextForm = new Sci.Production.PPIC.P04_QtyCartonByCustCD(MyUtility.Convert.GetString(CurrentMaintain["UKey"]));
            callNextForm.ShowDialog(this);
        }

        //Weight data
        private void btnWeightdata_Click(object sender, EventArgs e)
        {
            //自動新增不存在Style_WeightData的sIZE資料
            string insertCmd = string.Format(@"insert into Style_WeightData(StyleUkey,Article,SizeCode,AddName,AddDate)
select a.StyleUkey,'----',a.SizeCode,'{0}',GETDATE()
from (
select ss.StyleUkey,ss.SizeCode,sw.Article 
from Style_SizeCode ss
left join Style_WeightData sw on ss.StyleUkey = sw.StyleUkey and ss.SizeCode = sw.SizeCode
where ss.StyleUkey = {1}) a
where a.Article is null", Sci.Env.User.UserID, MyUtility.Convert.GetString(CurrentMaintain["UKey"]));
            DualResult result = DBProxy.Current.Execute(null, insertCmd);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Insert data to Style_WeightData fail!\r\n"+result.ToString());
            }

            Sci.Production.PPIC.P04_WeightData callNextForm = new Sci.Production.PPIC.P04_WeightData(PublicPrg.Prgs.GetAuthority(Sci.Env.User.UserID, "P04. Style Management", "CanEdit"), MyUtility.Convert.GetString(CurrentMaintain["UKey"]), null, null);
            callNextForm.ShowDialog(this);
            //按鈕變色
            btnWeightdata.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Style_WeightData WITH (NOLOCK) where StyleUkey = {0}", MyUtility.Convert.GetString(CurrentMaintain["UKey"]))) ? Color.Blue : Color.Black;
        }

        //Garment List
        private void btnGarmentList_Click(object sender, EventArgs e)
        {
            Sci.Production.PublicForm.GarmentList callNextForm = new Sci.Production.PublicForm.GarmentList(MyUtility.Convert.GetString(CurrentMaintain["Ukey"]),null,null);
            callNextForm.ShowDialog(this);
        }

        //Similar Style
        private void btnSimilarStyle_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P04_SimilarStyle callNextForm = new Sci.Production.PPIC.P04_SimilarStyle(MyUtility.Convert.GetString(CurrentMaintain["UKey"]));
            callNextForm.ShowDialog(this);
        }

        //HS Code
        private void btnHSCode_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P04_HSCode callNextForm = new Sci.Production.PPIC.P04_HSCode(MyUtility.Convert.GetString(CurrentMaintain["UKey"]));
            callNextForm.ShowDialog(this);
        }

        //Fty L/T
        private void btnFtyLT_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P04_GarmentLeadTimeByFactory callNextForm = new Sci.Production.PPIC.P04_GarmentLeadTimeByFactory(false, MyUtility.Convert.GetString(CurrentMaintain["UKey"]), null, null);
            callNextForm.ShowDialog(this);
        }

        //Picture1 Attach
        private void btnPicture1Attach_Click(object sender, EventArgs e)
        {
            //呼叫File 選擇視窗
            OpenFileDialog file = new OpenFileDialog();
            Stream fileOpened = null;

            file.InitialDirectory = "c:\\"; //預設路徑
            file.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*"; //使用檔名
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
                            string destination_fileName = (MyUtility.Convert.GetString(CurrentMaintain["UKey"])).Trim() + "-1" + local_file_type;

                            System.IO.File.Copy(local_path_file, destination_path + destination_fileName, true);

                            //update picture1 path
                            DualResult result = Sci.Data.DBProxy.Current.Execute(null, "update Style set Picture1 ='" + destination_fileName.Trim() + "' where ukey=" + this.CurrentMaintain["UKey"]); ;
                            this.CurrentMaintain["Picture1"] = destination_path.Trim() + destination_fileName.Trim();
                            this.pictureBox1.ImageLocation = MyUtility.Convert.GetString(CurrentMaintain["Picture1"]);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MyUtility.Msg.ErrorBox("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        //Picture1 Delete
        private void btnPicture1Delete_Click(object sender, EventArgs e)
        {
            DialogResult deleteResult1 = MyUtility.Msg.QuestionBox("Are you sure delete the < Picture1 >?", buttons: MessageBoxButtons.YesNo);
            if (deleteResult1 == System.Windows.Forms.DialogResult.Yes)
            {
                if (System.IO.File.Exists(destination_path + MyUtility.Convert.GetString(CurrentMaintain["Picture1"])))
                {
                    try
                    {
                        System.IO.File.Delete(destination_path + MyUtility.Convert.GetString(CurrentMaintain["Picture1"]));
                        this.CurrentMaintain["Picture1"] = string.Empty;
                        this.pictureBox1.ImageLocation = MyUtility.Convert.GetString(CurrentMaintain["Picture1"]);
                        DualResult result = Sci.Data.DBProxy.Current.Execute(null, string.Format("update Style set Picture1='' where UKey={0}", MyUtility.Convert.GetString(CurrentMaintain["UKey"])));
                        if (!result)
                        {
                            MyUtility.Msg.ErrorBox("Update data fail!!\r\n" + result.ToString());
                            return;
                        }
                    }
                    catch (System.IO.IOException exception)
                    {
                        MyUtility.Msg.ErrorBox("Error: Delete file fail. Original error: " + exception.Message);
                    }
                }
                else
                {
                    this.CurrentMaintain["Picture1"] = string.Empty;
                    this.pictureBox1.ImageLocation = MyUtility.Convert.GetString(CurrentMaintain["Picture1"]);
                    DualResult result = Sci.Data.DBProxy.Current.Execute(null, string.Format("update Style set Picture1='' where UKey={0}", MyUtility.Convert.GetString(CurrentMaintain["UKey"])));
                    if (!result)
                    {
                        MyUtility.Msg.ErrorBox("Update data fail!!\r\n" + result.ToString());
                        return;
                    }
                }
            }
        }

        //Picture2 Attach
        private void btnPicture2Attach_Click(object sender, EventArgs e)
        {
            //呼叫File 選擇視窗
            OpenFileDialog file = new OpenFileDialog();
            Stream fileOpened = null;

            file.InitialDirectory = "c:\\"; //預設路徑
            file.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*"; //使用檔名
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
                            string destination_fileName = (MyUtility.Convert.GetString(CurrentMaintain["UKey"])).Trim() + "-2" + local_file_type;

                            System.IO.File.Copy(local_path_file, destination_path + destination_fileName, true);

                            //update picture1 path
                            DualResult result = Sci.Data.DBProxy.Current.Execute(null, "update Style set Picture2 ='" + destination_fileName.Trim() + "' where ukey=" + this.CurrentMaintain["UKey"]); ;
                            this.CurrentMaintain["Picture2"] = destination_path.Trim() + destination_fileName.Trim();
                            this.pictureBox2.ImageLocation = MyUtility.Convert.GetString(CurrentMaintain["Picture2"]);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MyUtility.Msg.ErrorBox("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        //Picture2 Delete
        private void btnPicture2Delete_Click(object sender, EventArgs e)
        {
            DialogResult deleteResult1 = MyUtility.Msg.QuestionBox("Are you sure delete the < Picture2 >?", buttons: MessageBoxButtons.YesNo);
            if (deleteResult1 == System.Windows.Forms.DialogResult.Yes)
            {
                if (System.IO.File.Exists(destination_path + MyUtility.Convert.GetString(CurrentMaintain["Picture2"])))
                {
                    try
                    {
                        System.IO.File.Delete(destination_path + MyUtility.Convert.GetString(CurrentMaintain["Picture2"]));
                        this.CurrentMaintain["Picture2"] = string.Empty;
                        this.pictureBox2.ImageLocation = MyUtility.Convert.GetString(CurrentMaintain["Picture2"]);
                        DualResult result = Sci.Data.DBProxy.Current.Execute(null, string.Format("update Style set Picture2='' where UKey={0}", MyUtility.Convert.GetString(CurrentMaintain["UKey"])));
                        if (!result)
                        {
                            MyUtility.Msg.ErrorBox("Update data fail!!\r\n" + result.ToString());
                            return;
                        }
                    }
                    catch (System.IO.IOException exception)
                    {
                        MyUtility.Msg.ErrorBox("Error: Delete file fail. Original error: " + exception.Message);
                    }
                }
                else
                {
                    this.CurrentMaintain["Picture2"] = string.Empty;
                    this.pictureBox2.ImageLocation = MyUtility.Convert.GetString(CurrentMaintain["Picture2"]);
                    DualResult result = Sci.Data.DBProxy.Current.Execute(null, string.Format("update Style set Picture2='' where UKey={0}", MyUtility.Convert.GetString(CurrentMaintain["UKey"])));
                    if (!result)
                    {
                        MyUtility.Msg.ErrorBox("Update data fail!!\r\n" + result.ToString());
                        return;
                    }
                }
            }
        }

        //Combo Type
        private void btnComboType_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P04_ComboType callNextForm = new Sci.Production.PPIC.P04_ComboType((PublicPrg.Prgs.GetAuthority(Sci.Env.User.UserID, "P04. Style Management", "CanEdit") && MyUtility.Convert.GetString(CurrentMaintain["LocalStyle"]).ToUpper() == "TRUE"), MyUtility.Convert.GetString(CurrentMaintain["UKey"]), null, null, MyUtility.Convert.GetString(CurrentMaintain["StyleUnit"]));
            callNextForm.ShowDialog(this);
            //按鈕變色
            btnComboType.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Style_Location WITH (NOLOCK) where StyleUkey = {0}", MyUtility.Convert.GetString(CurrentMaintain["UKey"]))) ? Color.Blue : Color.Black;
        }

        //檢查是否有輸入'字元
        private bool EnterWrongChar(string EnterData)
        {
            if (EnterData.IndexOf("'") != -1)
            {
                MyUtility.Msg.WarningBox("Can not enter the  '  character!!");
                return true;
            }
            return false;
        }

        //Style
        private void txtStyleNo_Validating(object sender, CancelEventArgs e)
        {
            if (EditMode && MyUtility.Convert.GetString(CurrentMaintain["LocalStyle"]).ToUpper() == "TRUE" && txtStyleNo.OldValue != txtStyleNo.Text)
            {
                if (!MyUtility.Check.Empty(txtStyleNo.Text))
                {
                    if (EnterWrongChar(txtStyleNo.Text))
                    {
                        CurrentMaintain["StyleID"] = "";
                        e.Cancel = true;
                        return;
                    }
                }
            }
        }

        //Season
        private void txtSeason_Validating(object sender, CancelEventArgs e)
        {
            if (EditMode && MyUtility.Convert.GetString(CurrentMaintain["LocalStyle"]).ToUpper() == "TRUE" && txtSeason.OldValue != txtSeason.Text)
            {
                if (!MyUtility.Check.Empty(txtSeason.Text))
                {
                    if (EnterWrongChar(txtSeason.Text))
                    {
                        CurrentMaintain["SeasonID"] = "";
                        e.Cancel = true;
                        return;
                    }
                }
            }
        }

        //Program
        private void txtProgram_Validating(object sender, CancelEventArgs e)
        {
            if (EditMode && MyUtility.Convert.GetString(CurrentMaintain["LocalStyle"]).ToUpper() == "TRUE" && txtProgram.OldValue != txtProgram.Text)
            {
                if (!MyUtility.Check.Empty(txtProgram.Text))
                {
                    if (EnterWrongChar(txtProgram.Text))
                    {
                        CurrentMaintain["ProgramID"] = "";
                        e.Cancel = true;
                        return;
                    }
                }
            }
        }
    }
}
