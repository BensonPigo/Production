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
            MyUtility.Tool.SetupCombox(comboBox1, 1, 1, "CM,INCH");
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            displayBox4.Value = MyUtility.GetValue.Lookup(string.Format("select Name from Reason WITH (NOLOCK) where ReasonTypeID = 'Style_SpecialMark' and ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["SpecialMark"])));
            txttpeuser1.DisplayBox1Binding = MyUtility.Convert.GetString(CurrentMaintain["Phase"]) == "1" ? MyUtility.Convert.GetString(CurrentMaintain["SampleSMR"]) : MyUtility.Convert.GetString(CurrentMaintain["BulkSMR"]);
            txttpeuser2.DisplayBox1Binding = MyUtility.Convert.GetString(CurrentMaintain["Phase"]) == "1" ? MyUtility.Convert.GetString(CurrentMaintain["SampleMRHandle"]) : MyUtility.Convert.GetString(CurrentMaintain["BulkMRHandle"]);
            displayBox2.Value = MyUtility.Convert.GetString(CurrentMaintain["ApvName"]) + " " + MyUtility.GetValue.Lookup(string.Format("select (Name + ' #' + ExtNo) as NameExtNo from TPEPass1 WITH (NOLOCK) where ID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["ApvName"])));
            numericBox4.Value = MyUtility.Check.Empty(CurrentMaintain["CPUAdjusted"]) ? 0 : MyUtility.Convert.GetDecimal(CurrentMaintain["CPUAdjusted"]) * 100m;
            pictureBox1.ImageLocation = MyUtility.Check.Empty(CurrentMaintain["Picture1"]) ? null : destination_path + MyUtility.Convert.GetString(CurrentMaintain["Picture1"]);
            pictureBox2.ImageLocation = MyUtility.Check.Empty(CurrentMaintain["Picture2"]) ? null : destination_path + MyUtility.Convert.GetString(CurrentMaintain["Picture2"]);
            button1.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Style_TMSCost WITH (NOLOCK) where StyleUkey = {0}", MyUtility.Convert.GetString(CurrentMaintain["UKey"]))) ? Color.Blue : Color.Black;
            button2.ForeColor = MyUtility.Check.Seek(string.Format("select ID from IETMS WITH (NOLOCK) where ID = '{0}' and Version = '{1}'", MyUtility.Convert.GetString(CurrentMaintain["IETMSID"]), MyUtility.Convert.GetString(CurrentMaintain["IETMSVersion"]))) ? Color.Blue : Color.Black;
            button3.ForeColor = MyUtility.Check.Seek(string.Format("select StyleID from TimeStudy WITH (NOLOCK) where StyleID = '{0}' and BrandID = '{1}' and SeasonID = '{2}'", MyUtility.Convert.GetString(CurrentMaintain["ID"]), MyUtility.Convert.GetString(CurrentMaintain["BrandID"]), MyUtility.Convert.GetString(CurrentMaintain["SeasonID"]))) ? Color.Blue : Color.Black;
            button4.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Style_ProductionKits WITH (NOLOCK) where StyleUkey = {0}", MyUtility.Convert.GetString(CurrentMaintain["UKey"]))) ? Color.Blue : Color.Black;
            button5.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Style_Artwork WITH (NOLOCK) where StyleUkey = {0}", MyUtility.Convert.GetString(CurrentMaintain["UKey"]))) ? Color.Blue : Color.Black;
            button6.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Style_QtyCTN WITH (NOLOCK) where StyleUkey = {0}", MyUtility.Convert.GetString(CurrentMaintain["UKey"]))) ? Color.Blue : Color.Black;
            button7.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Style_WeightData WITH (NOLOCK) where StyleUkey = {0}", MyUtility.Convert.GetString(CurrentMaintain["UKey"]))) ? Color.Blue : Color.Black;
            button8.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Pattern WITH (NOLOCK) where StyleUkey = {0}", MyUtility.Convert.GetString(CurrentMaintain["UKey"]))) ? Color.Blue : Color.Black;
            button9.ForeColor = (MyUtility.Check.Seek(string.Format("select MasterStyleUkey from Style_SimilarStyle WITH (NOLOCK) where MasterStyleUkey = {0}", MyUtility.Convert.GetString(CurrentMaintain["UKey"]))) || MyUtility.Check.Seek(string.Format("select ChildrenStyleUkey  from Style_SimilarStyle where ChildrenStyleUkey = {0}", MyUtility.Convert.GetString(CurrentMaintain["UKey"])))) ? Color.Blue : Color.Black;
            button10.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Style_HSCode WITH (NOLOCK) where StyleUkey = {0}", MyUtility.Convert.GetString(CurrentMaintain["UKey"]))) ? Color.Blue : Color.Black;
            button15.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Style_GMTLTFty WITH (NOLOCK) where StyleUkey = {0}", MyUtility.Convert.GetString(CurrentMaintain["UKey"]))) ? Color.Blue : Color.Black;
            button16.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Style_Location WITH (NOLOCK) where StyleUkey = {0}", MyUtility.Convert.GetString(CurrentMaintain["UKey"]))) ? Color.Blue : Color.Black;
            if (!MyUtility.Check.Empty(CurrentMaintain["ApvDate"]))
            {
                DateTime? lastTime = (DateTime?)this.CurrentMaintain["ApvDate"];
                string FtyLastupdate = lastTime == null ? "" : ((DateTime)lastTime).ToString("yyyy/MM/dd HH:mm:ss");
                this.displayBox9.Text = FtyLastupdate;
            }
            else
            {
                this.displayBox9.Text = "";
            }
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["LocalStyle"] = 1;
            CurrentMaintain["LocalMR"] = Sci.Env.User.UserID;
            this.displayBox9.Text = "";
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            textBox1.ReadOnly = true;
            textBox6.ReadOnly = true;
            textBox2.ReadOnly = true;
          
            if (MyUtility.Convert.GetString(CurrentMaintain["LocalStyle"]).ToUpper() == "FALSE")
            {
                textBox3.ReadOnly = true;
                textBox4.ReadOnly = true;
                textBox5.ReadOnly = true;
                textBox7.ReadOnly = true;
                textBox8.ReadOnly = true;
                textBox9.ReadOnly = true;
                numericBox1.ReadOnly = true;
                numericBox3.ReadOnly = true;
                txtcdcode1.ReadOnly = true;
                checkBox3.ReadOnly = true;
                checkBox4.ReadOnly = true;
                comboBox1.ReadOnly = true; 
                comboBox2.ReadOnly = true;
              
            }
            if (MyUtility.Convert.GetString(CurrentMaintain["NoNeedPPMeeting"]).ToUpper() == "TRUE")
            {
                dateBox1.ReadOnly = true;
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
                    textBox1.Focus();
                    return false;
                }
                if (MyUtility.Check.Empty(CurrentMaintain["SeasonID"]))
                {
                    MyUtility.Msg.WarningBox("Season can't empty");
                    textBox6.Focus();
                    return false;
                }
                if (MyUtility.Check.Empty(CurrentMaintain["BrandID"]))
                {
                    MyUtility.Msg.WarningBox("Brand can't empty");
                    textBox2.Focus();
                    return false;
                }
                if (MyUtility.Check.Empty(CurrentMaintain["Description"]))
                {
                    MyUtility.Msg.WarningBox("Description can't empty");
                    textBox4.Focus();
                    return false;
                }
                if (MyUtility.Check.Empty(CurrentMaintain["StyleName"]))
                {
                    MyUtility.Msg.WarningBox("Style name can't empty");
                    textBox5.Focus();
                    return false;
                }
                if (MyUtility.Check.Empty(CurrentMaintain["CdCodeID"]))
                {
                    MyUtility.Msg.WarningBox("CD can't empty");
                    txtcdcode1.Focus();
                    return false;
                }
                if (MyUtility.Check.Empty(CurrentMaintain["CPU"]))
                {
                    MyUtility.Msg.WarningBox("CPU can't empty");
                    numericBox2.Focus();
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

        private void textBox6_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Tools.SelectItem item;
            string sqlCmd = "select distinct ID from Season WITH (NOLOCK) where Junk = 0 order by ID desc";
            item = new Sci.Win.Tools.SelectItem(sqlCmd, "11", this.Text);
            item.Width = 300;
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            textBox6.Text = item.GetSelectedString();
        }

        //Brand
        private void textBox2_Validating(object sender, CancelEventArgs e)
        {
            if (EditMode && MyUtility.Convert.GetString(CurrentMaintain["LocalStyle"]).ToUpper() == "TRUE" && textBox2.OldValue != textBox2.Text)
            {
                if (!MyUtility.Check.Empty(textBox2.Text))
                {
                    if (EnterWrongChar(textBox2.Text))
                    {
                        CurrentMaintain["BrandID"] = "";
                        e.Cancel = true;
                        return;
                    }
                    else
                    {
                        if (MyUtility.Check.Seek(string.Format("select ID from Brand WITH (NOLOCK) where ID = '{0}'", textBox2.Text)))
                        {
                            MyUtility.Msg.WarningBox(string.Format("Brand:{0} is belong to SCI, Factory can't use!!", textBox2.Text));
                            CurrentMaintain["BrandID"] = "";
                            e.Cancel = true;
                            return;
                        }
                    }
                }
            }
        }

        //Program
        private void textBox3_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlCmd = string.Format("Select id,BrandID from Program WITH (NOLOCK) where BrandID = '{0}'", MyUtility.Convert.GetString(CurrentMaintain["BrandID"]));
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "12,8", this.Text, false, ",");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            textBox3.Text = item.GetSelectedString();
        }

        //CD
        private void txtcdcode1_Validated(object sender, EventArgs e)
        {
            if (EditMode && MyUtility.Convert.GetString(CurrentMaintain["LocalStyle"]).ToUpper() == "TRUE" && txtcdcode1.OldValue != txtcdcode1.Text)
            {
                if (MyUtility.Check.Empty(txtcdcode1.Text))
                {
                    CurrentMaintain["CPU"] = 0;
                    CurrentMaintain["StyleUnit"] = "";
                }
                else
                {
                    DataRow CDCodeRow;
                    if (MyUtility.Check.Seek(string.Format("select Cpu,ComboPcs from CDCode WITH (NOLOCK) where ID = '{0}'", txtcdcode1.Text), out CDCodeRow))
                    CurrentMaintain["CPU"] = CDCodeRow["Cpu"];
                    CurrentMaintain["StyleUnit"] = MyUtility.Convert.GetString(CDCodeRow["ComboPcs"]) == "1" ? "PCS" : "SETS";
                }
            }
        }

        //No need PP Meeting
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {  
           if (EditMode)
            {
                CurrentMaintain["NoNeedPPMeeting"] = checkBox2.Checked;
                dateBox1.ReadOnly = checkBox2.Value.ToUpper() == "TRUE";      
            }
        }

        //TMS & Cost
        private void button1_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P04_TMSAndCost callNextForm = new Sci.Production.PPIC.P04_TMSAndCost((PublicPrg.Prgs.GetAuthority(Sci.Env.User.UserID, "P04. Style Management", "CanEdit") && MyUtility.Convert.GetString(CurrentMaintain["LocalStyle"]).ToUpper() == "TRUE"), MyUtility.Convert.GetString(CurrentMaintain["UKey"]), null, null, MyUtility.Convert.GetString(CurrentMaintain["ID"]));
            callNextForm.ShowDialog(this);
            //按鈕變色
            button1.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Style_TMSCost WITH (NOLOCK) where StyleUkey = {0}", MyUtility.Convert.GetString(CurrentMaintain["UKey"]))) ? Color.Blue : Color.Black;
        }

        //Std. GSD
        private void button2_Click(object sender, EventArgs e)
        {
            Sci.Production.PublicForm.StdGSDList callNextForm = new Sci.Production.PublicForm.StdGSDList(MyUtility.Convert.GetLong(CurrentMaintain["UKey"]));
            callNextForm.ShowDialog(this);
        }

        //FTY GSD
        private void button3_Click(object sender, EventArgs e)
        {
            Sci.Production.IE.P01 callNextForm = new Sci.Production.IE.P01(MyUtility.Convert.GetString(CurrentMaintain["ID"]), MyUtility.Convert.GetString(CurrentMaintain["BrandID"]), MyUtility.Convert.GetString(CurrentMaintain["SeasonID"]), null);
            callNextForm.ShowDialog(this);
        }

        //Production Kits
        private void button4_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_ProductionKit callNextForm = new Sci.Production.PPIC.P01_ProductionKit(true, MyUtility.Convert.GetString(CurrentMaintain["UKey"]), null, null, MyUtility.Convert.GetString(CurrentMaintain["ID"]));
            callNextForm.ShowDialog(this);
        }

        //Artwork
        private void button5_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P04_Artwork callNextForm = new Sci.Production.PPIC.P04_Artwork((PublicPrg.Prgs.GetAuthority(Sci.Env.User.UserID, "P04. Style Management", "CanEdit") && MyUtility.Convert.GetString(CurrentMaintain["LocalStyle"]).ToUpper() == "TRUE"), MyUtility.Convert.GetString(CurrentMaintain["UKey"]), null, null, MyUtility.Convert.GetString(CurrentMaintain["ID"]), MyUtility.Convert.GetString(CurrentMaintain["SeasonID"]));
            callNextForm.ShowDialog(this);
            //按鈕變色
            button5.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Style_Artwork WITH (NOLOCK) where StyleUkey = {0}", MyUtility.Convert.GetString(CurrentMaintain["UKey"]))) ? Color.Blue : Color.Black;
        }

        //Q'ty/Carton by CustCD
        private void button6_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P04_QtyCartonByCustCD callNextForm = new Sci.Production.PPIC.P04_QtyCartonByCustCD(MyUtility.Convert.GetString(CurrentMaintain["UKey"]));
            callNextForm.ShowDialog(this);
        }

        //Weight data
        private void button7_Click(object sender, EventArgs e)
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
            button7.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Style_WeightData WITH (NOLOCK) where StyleUkey = {0}", MyUtility.Convert.GetString(CurrentMaintain["UKey"]))) ? Color.Blue : Color.Black;
        }

        //Garment List
        private void button8_Click(object sender, EventArgs e)
        {
            Sci.Production.PublicForm.GarmentList callNextForm = new Sci.Production.PublicForm.GarmentList(MyUtility.Convert.GetString(CurrentMaintain["Ukey"]));
            callNextForm.ShowDialog(this);
        }

        //Similar Style
        private void button9_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P04_SimilarStyle callNextForm = new Sci.Production.PPIC.P04_SimilarStyle(MyUtility.Convert.GetString(CurrentMaintain["UKey"]));
            callNextForm.ShowDialog(this);
        }

        //HS Code
        private void button10_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P04_HSCode callNextForm = new Sci.Production.PPIC.P04_HSCode(MyUtility.Convert.GetString(CurrentMaintain["UKey"]));
            callNextForm.ShowDialog(this);
        }

        //Fty L/T
        private void button15_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P04_GarmentLeadTimeByFactory callNextForm = new Sci.Production.PPIC.P04_GarmentLeadTimeByFactory(false, MyUtility.Convert.GetString(CurrentMaintain["UKey"]), null, null);
            callNextForm.ShowDialog(this);
        }

        //Picture1 Attach
        private void button11_Click(object sender, EventArgs e)
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
        private void button12_Click(object sender, EventArgs e)
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
        private void button14_Click(object sender, EventArgs e)
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
        private void button13_Click(object sender, EventArgs e)
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
        private void button16_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P04_ComboType callNextForm = new Sci.Production.PPIC.P04_ComboType((PublicPrg.Prgs.GetAuthority(Sci.Env.User.UserID, "P04. Style Management", "CanEdit") && MyUtility.Convert.GetString(CurrentMaintain["LocalStyle"]).ToUpper() == "TRUE"), MyUtility.Convert.GetString(CurrentMaintain["UKey"]), null, null, MyUtility.Convert.GetString(CurrentMaintain["StyleUnit"]));
            callNextForm.ShowDialog(this);
            //按鈕變色
            button16.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Style_Location WITH (NOLOCK) where StyleUkey = {0}", MyUtility.Convert.GetString(CurrentMaintain["UKey"]))) ? Color.Blue : Color.Black;
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
        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            if (EditMode && MyUtility.Convert.GetString(CurrentMaintain["LocalStyle"]).ToUpper() == "TRUE" && textBox1.OldValue != textBox1.Text)
            {
                if (!MyUtility.Check.Empty(textBox1.Text))
                {
                    if (EnterWrongChar(textBox1.Text))
                    {
                        CurrentMaintain["StyleID"] = "";
                        e.Cancel = true;
                        return;
                    }
                }
            }
        }

        //Season
        private void textBox6_Validating(object sender, CancelEventArgs e)
        {
            if (EditMode && MyUtility.Convert.GetString(CurrentMaintain["LocalStyle"]).ToUpper() == "TRUE" && textBox6.OldValue != textBox6.Text)
            {
                if (!MyUtility.Check.Empty(textBox6.Text))
                {
                    if (EnterWrongChar(textBox6.Text))
                    {
                        CurrentMaintain["SeasonID"] = "";
                        e.Cancel = true;
                        return;
                    }
                }
            }
        }

        //Program
        private void textBox3_Validating(object sender, CancelEventArgs e)
        {
            if (EditMode && MyUtility.Convert.GetString(CurrentMaintain["LocalStyle"]).ToUpper() == "TRUE" && textBox3.OldValue != textBox3.Text)
            {
                if (!MyUtility.Check.Empty(textBox3.Text))
                {
                    if (EnterWrongChar(textBox3.Text))
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
