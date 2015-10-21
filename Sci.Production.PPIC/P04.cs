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

namespace Sci.Production.PPIC
{
    public partial class P04 : Sci.Win.Tems.Input1
    {
        private string destination_path;//放圖檔的路徑
        public P04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            destination_path = MyUtility.GetValue.Lookup("select PicPath from System", null);
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            MyUtility.Tool.SetupCombox(comboBox1, 1, 1, "CM,INCH");
            MyUtility.Tool.SetupCombox(comboBox2, 1, 1, "Male,Female");
        }

        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            displayBox4.Value = MyUtility.GetValue.Lookup(string.Format("select Name from Reason where ReasonTypeID = 'Style_SpecialMark' and ID = '{0}'", CurrentMaintain["SpecialMark"].ToString()));
            txttpeuser1.DisplayBox1Binding = CurrentMaintain["Phase"].ToString() == "1" ? CurrentMaintain["SampleSMR"].ToString() : CurrentMaintain["BulkSMR"].ToString();
            txttpeuser2.DisplayBox1Binding = CurrentMaintain["Phase"].ToString() == "1" ? CurrentMaintain["SampleMRHandle"].ToString() : CurrentMaintain["BulkMRHandle"].ToString();
            displayBox2.Value = CurrentMaintain["ApvName"].ToString() + " " + MyUtility.GetValue.Lookup(string.Format("select (Name + ' #' + ExtNo) as NameExtNo from TPEPass1 where ID = '{0}'", CurrentMaintain["ApvName"].ToString()));
            numericBox4.Value = MyUtility.Check.Empty(CurrentMaintain["CPUAdjusted"]) ? 0 : Convert.ToDecimal(CurrentMaintain["CPUAdjusted"]) * 100m;
            pictureBox1.ImageLocation = MyUtility.Check.Empty(CurrentMaintain["Picture1"]) ? null : destination_path + CurrentMaintain["Picture1"].ToString();
            pictureBox2.ImageLocation = MyUtility.Check.Empty(CurrentMaintain["Picture2"]) ? null : destination_path + CurrentMaintain["Picture2"].ToString();
            button1.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Style_TMSCost where StyleUkey = {0}", CurrentMaintain["UKey"].ToString())) ? Color.Blue : Color.Black;
            button2.ForeColor = MyUtility.Check.Seek(string.Format("select ID from IETMS where ID = '{0}' and Version = '{1}'", CurrentMaintain["IETMSID"].ToString(), CurrentMaintain["IETMSVersion"].ToString())) ? Color.Blue : Color.Black;
            button3.ForeColor = MyUtility.Check.Seek(string.Format("select StyleID from TimeStudy where StyleID = '{0}' and BrandID = '{1}' and SeasonID = '{2}'", CurrentMaintain["ID"].ToString(), CurrentMaintain["BrandID"].ToString(), CurrentMaintain["SeasonID"].ToString())) ? Color.Blue : Color.Black;
            button4.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Style_ProductionKits where StyleUkey = {0}", CurrentMaintain["UKey"].ToString())) ? Color.Blue : Color.Black;
            button5.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Style_Artwork where StyleUkey = {0}", CurrentMaintain["UKey"].ToString())) ? Color.Blue : Color.Black;
            button6.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Style_QtyCTN where StyleUkey = {0}", CurrentMaintain["UKey"].ToString())) ? Color.Blue : Color.Black;
            button7.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Style_WeightData where StyleUkey = {0}", CurrentMaintain["UKey"].ToString())) ? Color.Blue : Color.Black;
            button8.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Pattern where StyleUkey = {0}", CurrentMaintain["UKey"].ToString())) ? Color.Blue : Color.Black;
            button9.ForeColor = (MyUtility.Check.Seek(string.Format("select MasterStyleUkey  from Style_SimilarStyle where MasterStyleUkey = {0}", CurrentMaintain["UKey"].ToString())) || MyUtility.Check.Seek(string.Format("select ChildrenStyleUkey  from Style_SimilarStyle where ChildrenStyleUkey = {0}", CurrentMaintain["UKey"].ToString()))) ? Color.Blue : Color.Black;
            button10.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Style_HSCode where StyleUkey = {0}", CurrentMaintain["UKey"].ToString())) ? Color.Blue : Color.Black;
            button15.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Style_GMTLTFty where StyleUkey = {0}",CurrentMaintain["UKey"].ToString())) ? Color.Blue : Color.Black;
            button16.ForeColor = MyUtility.Check.Seek(string.Format("select StyleUkey from Style_Location where StyleUkey = {0}", CurrentMaintain["UKey"].ToString())) ? Color.Blue : Color.Black;
        }

        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            CurrentMaintain["LocalStyle"] = 1;
            CurrentMaintain["LocalMR"] = Sci.Env.User.UserID;
        }

        protected override void ClickEditAfter()
        {
            base.ClickEditAfter();
            textBox1.ReadOnly = true;
            textBox6.ReadOnly = true;
            textBox2.ReadOnly = true;
            if (CurrentMaintain["LocalStyle"].ToString().ToUpper() == "FALSE")
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
            if (CurrentMaintain["NoNeedPPMeeting"].ToString().ToUpper() == "TRUE")
            {
                dateBox1.ReadOnly = true;
            }
        }

        protected override bool ClickSaveBefore()
        {
            #region 檢查必輸欄位
            if (CurrentMaintain["LocalStyle"].ToString().ToUpper() == "TRUE")
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
                if (MyUtility.Check.Seek(string.Format("select Id from Style where ID = '{0}' and BrandID = '{1}' and SeasonID = '{2}'", CurrentMaintain["ID"].ToString(), CurrentMaintain["BrandID"].ToString(), CurrentMaintain["SeasonID"].ToString())))
                {
                    MyUtility.Msg.WarningBox("This style already exist!!");
                    return false;
                }

                DataTable StyleUkey;
                string sqlCmd = "select MIN(Ukey)-1 as NewUkey from Style";
                DualResult result = DBProxy.Current.Select(null, sqlCmd, out StyleUkey);
                if (!result)
                {
                    MyUtility.Msg.ErrorBox("Get Ukey fail!!\r\n"+result.ToString());
                    return false;
                }
                CurrentMaintain["Ukey"] = StyleUkey.Rows[0]["NewUkey"].ToString();
            }
            return true;
        }

        protected override bool ClickSavePre()
        {
            if (CurrentMaintain["LocalStyle"].ToString().ToUpper() == "TRUE" && !MyUtility.Check.Empty(CurrentMaintain["UKey"]))
            {
                string sqlCmd = string.Format("update Orders set CPU = {0}, CdCodeID = '{1}', StyleUnit = '{2}' where Orders.StyleUkey = {3} and not exists (select 1 from SewingOutput_Detail where OrderId = Orders.ID)",
                    CurrentMaintain["CPU"].ToString(), CurrentMaintain["CdCodeID"].ToString(), CurrentMaintain["StyleUnit"].ToString(), CurrentMaintain["UKey"].ToString());
                DualResult result = DBProxy.Current.Execute(null, sqlCmd);
                if (!result)
                {
                    MyUtility.Msg.ErrorBox("Update order cpu fail!!" + result.ToString());
                    return false;
                }
            }
            return base.ClickSavePre();
        }

        #region TMS & Cost不再由此新增，改到P04_TMSAndCost新增
        //        protected override bool ClickSavePre()
//        {
//            if (!MyUtility.Check.Seek(string.Format("select * from Style_TmsCost where StyleUkey = {0}", CurrentMaintain["Ukey"].ToString())))
//            {
//                string insertCmd = string.Format(@"insert into Style_TmsCost (StyleUkey,ArtworkTypeID,Seq,ArtworkUnit,AddName,AddDate)
//select {0},ID,Seq,ArtworkUnit,'{1}',GETDATE() from ArtworkType where SystemType = 1 and Junk = 0", CurrentMaintain["Ukey"].ToString(), Sci.Env.User.UserID);
//                DualResult result = DBProxy.Current.Execute(null, insertCmd);
//                if (!result)
//                {
//                    MyUtility.Msg.ErrorBox("Insert TMS & Cost fail!!\r\n"+result.ToString());
//                    return false;
//                }
//            }
//            return true;
//        }

        //Season
        #endregion

        private void textBox6_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Sci.Win.Tools.SelectItem item;
            string sqlCmd = "select distinct ID from Season where Junk = 0";
            item = new Sci.Win.Tools.SelectItem(sqlCmd, "11", this.Text);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel) { return; }
            textBox6.Text = item.GetSelectedString();
        }

        //Brand
        private void textBox2_Validating(object sender, CancelEventArgs e)
        {
            if (EditMode && CurrentMaintain["LocalStyle"].ToString().ToUpper() == "TRUE" && textBox2.OldValue != textBox2.Text)
            {
                if (!MyUtility.Check.Empty(textBox2.Text))
                {
                    if (MyUtility.Check.Seek(string.Format("select ID from Brand where ID = '{0}'", textBox2.Text)))
                    {
                        MyUtility.Msg.WarningBox("Brand:{0} is belong to SCI, Factory can't use!!");
                        CurrentMaintain["BrandID"] = "";
                        e.Cancel = true;
                        return;
                    }
                }
            }
        }

        //Program
        private void textBox3_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            string sqlCmd = string.Format("Select id,BrandID from Program where BrandID = '{0}'", CurrentMaintain["BrandID"].ToString());
            Sci.Win.Tools.SelectItem item = new Sci.Win.Tools.SelectItem(sqlCmd, "12,8", this.Text, false, ",");
            DialogResult result = item.ShowDialog();
            if (result == DialogResult.Cancel) { return; }
            textBox3.Text = item.GetSelectedString();
        }

        //CD
        private void txtcdcode1_Validated(object sender, EventArgs e)
        {
            if (EditMode && CurrentMaintain["LocalStyle"].ToString().ToUpper() == "TRUE" && txtcdcode1.OldValue != txtcdcode1.Text)
            {
                if (MyUtility.Check.Empty(txtcdcode1.Text))
                {
                    CurrentMaintain["CPU"] = 0;
                    CurrentMaintain["StyleUnit"] = "";
                }
                else
                {
                    DataRow CDCodeRow;
                    if (MyUtility.Check.Seek(string.Format("select Cpu,ComboPcs from CDCode where ID = '{0}'", txtcdcode1.Text), out CDCodeRow))
                    CurrentMaintain["CPU"] = CDCodeRow["Cpu"].ToString();
                    CurrentMaintain["StyleUnit"] = CDCodeRow["ComboPcs"].ToString() == "1" ? "PCS" : "SETS";
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
            Sci.Production.PPIC.P04_TMSAndCost callNextForm = new Sci.Production.PPIC.P04_TMSAndCost((PublicPrg.Prgs.GetAuthority(Sci.Env.User.UserID, "P04. Style Management", "CanEdit") && CurrentMaintain["LocalStyle"].ToString().ToUpper() == "TRUE"), CurrentMaintain["UKey"].ToString(), null, null, CurrentMaintain["ID"].ToString());
            callNextForm.ShowDialog(this);
        }

        //Std. GSD
        private void button2_Click(object sender, EventArgs e)
        {
            Sci.Production.PublicForm.StdGSDList callNextForm = new Sci.Production.PublicForm.StdGSDList(Convert.ToInt64(CurrentMaintain["UKey"]));
            callNextForm.ShowDialog(this);
        }

        //FTY GSD
        private void button3_Click(object sender, EventArgs e)
        {
            Sci.Production.IE.P01 callNextForm = new Sci.Production.IE.P01(CurrentMaintain["ID"].ToString(), CurrentMaintain["BrandID"].ToString(), CurrentMaintain["SeasonID"].ToString(), null);
            callNextForm.ShowDialog(this);
        }

        //Production Kits
        private void button4_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P01_ProductionKit callNextForm = new Sci.Production.PPIC.P01_ProductionKit(true, CurrentMaintain["UKey"].ToString(), null, null, CurrentMaintain["ID"].ToString());
            callNextForm.ShowDialog(this);
        }

        //Artwork
        private void button5_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P04_Artwork callNextForm = new Sci.Production.PPIC.P04_Artwork((PublicPrg.Prgs.GetAuthority(Sci.Env.User.UserID, "P04. Style Management", "CanEdit") && CurrentMaintain["LocalStyle"].ToString().ToUpper() == "TRUE"), CurrentMaintain["UKey"].ToString(), null, null, CurrentMaintain["ID"].ToString(), CurrentMaintain["SeasonID"].ToString());
            callNextForm.ShowDialog(this);
            OnDetailEntered();
        }

        //Q'ty/Carton by CustCD
        private void button6_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P04_QtyCartonByCustCD callNextForm = new Sci.Production.PPIC.P04_QtyCartonByCustCD(CurrentMaintain["UKey"].ToString());
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
where a.Article is null", Sci.Env.User.UserID, CurrentMaintain["UKey"].ToString());
            DualResult result = DBProxy.Current.Execute(null, insertCmd);
            if (!result)
            {
                MyUtility.Msg.ErrorBox("Insert data to Style_WeightData fail!\r\n"+result.ToString());
            }

            Sci.Production.PPIC.P04_WeightData callNextForm = new Sci.Production.PPIC.P04_WeightData(PublicPrg.Prgs.GetAuthority(Sci.Env.User.UserID, "P04. Style Management", "CanEdit"), CurrentMaintain["UKey"].ToString(), null, null);
            callNextForm.ShowDialog(this);
            OnDetailEntered();
        }

        //Garment List
        private void button8_Click(object sender, EventArgs e)
        {
            string patternUkey = MyUtility.GetValue.Lookup(string.Format(@"select UKey from Pattern
where EditDate = (select max(EditDate) from Pattern where StyleUkey = {0} and Status = 'C')
and StyleUkey = {0} and Status = 'C'",CurrentMaintain["Ukey"].ToString()));

            MyUtility.Msg.InfoBox("Wait for Cutting P01_Garment"+patternUkey);
        }

        //Similar Style
        private void button9_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P04_SimilarStyle callNextForm = new Sci.Production.PPIC.P04_SimilarStyle(CurrentMaintain["UKey"].ToString());
            callNextForm.ShowDialog(this);
        }

        //HS Code
        private void button10_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P04_HSCode callNextForm = new Sci.Production.PPIC.P04_HSCode(CurrentMaintain["UKey"].ToString());
            callNextForm.ShowDialog(this);
        }

        //Fty L/T
        private void button15_Click(object sender, EventArgs e)
        {
            Sci.Production.PPIC.P04_GarmentLeadTimeByFactory callNextForm = new Sci.Production.PPIC.P04_GarmentLeadTimeByFactory(false, CurrentMaintain["UKey"].ToString(), null, null);
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
                            string destination_fileName = (this.CurrentMaintain["UKey"].ToString()).Trim() + "-1" + local_file_type;

                            System.IO.File.Copy(local_path_file, destination_path + destination_fileName, true);

                            //update picture1 path
                            DualResult result = Sci.Data.DBProxy.Current.Execute(null, "update Style set Picture1 ='" + destination_fileName.Trim() + "' where ukey=" + this.CurrentMaintain["UKey"]); ;
                            this.CurrentMaintain["Picture1"] = destination_path.Trim() + destination_fileName.Trim();
                            this.pictureBox1.ImageLocation = this.CurrentMaintain["Picture1"].ToString();
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
                if (System.IO.File.Exists(destination_path + CurrentMaintain["Picture1"].ToString()))
                {
                    try
                    {
                        System.IO.File.Delete(destination_path + CurrentMaintain["Picture1"].ToString());
                        this.CurrentMaintain["Picture1"] = string.Empty;
                        this.pictureBox1.ImageLocation = this.CurrentMaintain["Picture1"].ToString();
                        DualResult result = Sci.Data.DBProxy.Current.Execute(null, string.Format("update Style set Picture1='' where UKey={0}", this.CurrentMaintain["UKey"].ToString()));
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
                    this.pictureBox1.ImageLocation = this.CurrentMaintain["Picture1"].ToString();
                    DualResult result = Sci.Data.DBProxy.Current.Execute(null, string.Format("update Style set Picture1='' where UKey={0}", this.CurrentMaintain["UKey"].ToString()));
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
                            string destination_fileName = (this.CurrentMaintain["UKey"].ToString()).Trim() + "-2" + local_file_type;

                            System.IO.File.Copy(local_path_file, destination_path + destination_fileName, true);

                            //update picture1 path
                            DualResult result = Sci.Data.DBProxy.Current.Execute(null, "update Style set Picture2 ='" + destination_fileName.Trim() + "' where ukey=" + this.CurrentMaintain["UKey"]); ;
                            this.CurrentMaintain["Picture2"] = destination_path.Trim() + destination_fileName.Trim();
                            this.pictureBox2.ImageLocation = this.CurrentMaintain["Picture2"].ToString();
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
                if (System.IO.File.Exists(destination_path + CurrentMaintain["Picture2"].ToString()))
                {
                    try
                    {
                        System.IO.File.Delete(destination_path + CurrentMaintain["Picture2"].ToString());
                        this.CurrentMaintain["Picture2"] = string.Empty;
                        this.pictureBox2.ImageLocation = this.CurrentMaintain["Picture2"].ToString();
                        DualResult result = Sci.Data.DBProxy.Current.Execute(null, string.Format("update Style set Picture2='' where UKey={0}", this.CurrentMaintain["UKey"].ToString()));
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
                    this.pictureBox2.ImageLocation = this.CurrentMaintain["Picture2"].ToString();
                    DualResult result = Sci.Data.DBProxy.Current.Execute(null, string.Format("update Style set Picture2='' where UKey={0}", this.CurrentMaintain["UKey"].ToString()));
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
            Sci.Production.PPIC.P04_ComboType callNextForm = new Sci.Production.PPIC.P04_ComboType((PublicPrg.Prgs.GetAuthority(Sci.Env.User.UserID, "P04. Style Management", "CanEdit") && CurrentMaintain["LocalStyle"].ToString().ToUpper() == "TRUE"), CurrentMaintain["UKey"].ToString(), null, null, CurrentMaintain["StyleUnit"].ToString());
            callNextForm.ShowDialog(this);
        }
    }
}
