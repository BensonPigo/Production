using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Ict;
using System.Data.SqlClient;
using Sci.Data;

namespace Sci.Production.Quality
{
    public partial class P07_Wash : Win.Subs.Input2A
    {
        private readonly DataRow maindr;
        private readonly string ID;
        private readonly string PoID;
        private readonly string SEQ1;
        private readonly string SEQ2;
        private readonly bool EDIT;
        private string sql;
        private DataRow DR;

        public P07_Wash(bool canedit, string id, string poid, string seq1, string seq2, DataRow mainDr)
        {
            this.InitializeComponent();
            this.maindr = mainDr;
            this.ID = id.Trim();
            this.PoID = poid.Trim();
            this.SEQ1 = seq1.Trim();
            this.SEQ2 = seq2.Trim();

            #region 設定可否編輯
            if (!canedit)
            {
                this.SetView(this.maindr);
                this.EDIT = false;
            }
            else
            {
                this.EDIT = true;
                this.SetUpdate(this.maindr);
            }
            #endregion

        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            #region [comboResult]
            Dictionary<string, string> result_RowSource = new Dictionary<string, string>();
            result_RowSource.Add(string.Empty, string.Empty);
            result_RowSource.Add("Pass", "Pass");
            result_RowSource.Add("Fail", "Fail");
            this.comboResult.DataSource = new BindingSource(result_RowSource, null);
            this.comboResult.ValueMember = "Key";
            this.comboResult.DisplayMember = "Value";
            #endregion

            #region [btnEncode]
            this.btnEdit.Enabled = false;
            this.save.Visible = false;
            if (this.EDIT)
            {
                if (Convert.ToBoolean(this.maindr["WashEncode"]))
                {
                    this.btnEncode.Text = "Amend";
                    this.btnEncode.Enabled = true;
                }
                else
                {
                    this.btnEncode.Text = "Encode";
                    this.btnEncode.Enabled = true;
                    this.btnEdit.Enabled = true;
                }
            }
            else
            {
                this.undo.Visible = false;
                this.btnEdit.Visible = false;
                this.btnClose.Visible = true;
            }
            #endregion

            this.OnRequery();
        }

        private void OnRequery()
        {
            this.sql = string.Format(
                @"select  C.ExportId , B.ArriveQty , E.StockUnit , E.SizeSpec , B.SCIRefno
	                                    , B.Refno , B.Suppid + '-' + D.AbbEN as supplier , E.ColorID
                                    from AIR_Laboratory A WITH (NOLOCK) 
                                    left join AIR B WITH (NOLOCK) on A.id=B.id
                                    left join Receiving C WITH (NOLOCK) on C.id=B.receivingID
                                    left join Supp D WITH (NOLOCK) on D.ID=B.Suppid
                                    left join PO_Supp_Detail E WITH (NOLOCK) on E.ID=A.POID and E.SEQ1=A.SEQ1 and E.SEQ2=A.SEQ2
                                    where A.id={0} and A.POID='{1}' and A.SEQ1='{2}' and A.SEQ2='{3}'", this.ID, this.PoID, this.SEQ1, this.SEQ2);
            if (MyUtility.Check.Seek(this.sql, out this.DR))
            {
                this.displaySP.Text = this.maindr["SEQ1"] + "-" + this.maindr["SEQ2"];
                this.displayWKNO.Text = this.DR["ExportId"].ToString().Trim();
                this.numActiveQty.Value = Convert.ToDecimal(this.DR["ArriveQty"]);
                this.displayUnit.Text = this.DR["StockUnit"].ToString().Trim();
                this.displaySize.Text = this.DR["SizeSpec"].ToString().Trim();
                this.displaySCIRefno.Text = this.DR["SCIRefno"].ToString().Trim();
                this.displayRefno.Text = this.DR["Refno"].ToString().Trim();
                this.displaySupplier.Text = this.DR["supplier"].ToString().Trim();
                this.displayColor.Text = this.DR["ColorID"].ToString().Trim();
            }

            this.comboResult.SelectedValue2 = this.maindr["Wash"].ToString().Trim();
        }

        private void TxtScale_PopUp(object sender, Win.UI.TextBoxPopUpEventArgs e)
        {
            Win.Forms.Base myForm = (Win.Forms.Base)this.FindForm();
            if (!this.EDIT)
            {
                return;
            }

            Win.Tools.SelectItem item = new Win.Tools.SelectItem("Select id from Scale WITH (NOLOCK) where junk=0", "10", this.txtScale.Text);
            DialogResult returnResult = item.ShowDialog();
            if (returnResult == DialogResult.Cancel)
            {
                return;
            }

            this.txtScale.Text = item.GetSelectedString();
        }

        private void TxtScale_Validating(object sender, CancelEventArgs e)
        {
            string textValue = this.txtScale.Text;
            if (!string.IsNullOrWhiteSpace(textValue) && textValue != this.txtScale.OldValue)
            {
                if (!MyUtility.Check.Seek(string.Format(@"Select id from Scale WITH (NOLOCK) where junk=0 and id = '{0}'", textValue)))
                {
                    this.txtScale.Text = string.Empty;
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox(string.Format("< Scale: {0} > not found!!!", textValue));
                    return;
                }
            }
        }

        // [Encode][Amend]
        private void BtnEncode_Click(object sender, EventArgs e)
        {
            if (this.btnEncode.Text == "Encode")
            {
                #region Valid
                if (MyUtility.Check.Empty(this.comboResult.SelectedValue2))
                {
                    MyUtility.Msg.WarningBox("[Result] can not be empty !!");
                    return;
                }

                if (MyUtility.Check.Empty(this.dateInspectDate.Value))
                {
                    MyUtility.Msg.WarningBox("[Inspect Date] can not be empty !!");
                    return;
                }

                if (MyUtility.Check.Empty(this.txtuserLabTech.TextBox1.Text))
                {
                    MyUtility.Msg.WarningBox("[Lab Tech] can not be empty !!");
                    return;
                }
                #endregion

                #region ii.填入Air_Laboratory.WashEncode = T。iv.填入Air_Laboratory.EditName=Userid, Air_Laboratory.EditDate=Datetime()
                this.sql = string.Format(
                    @"update Air_Laboratory set WashEncode=1 , editName='{0}', editDate='{1}'
                                      where ID='{2}' and POID='{3}' and SEQ1='{4}' and SEQ2='{5}'",
                    Env.User.UserID, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), this.ID, this.PoID, this.SEQ1, this.SEQ2);
                DBProxy.Current.Execute(null, this.sql);
                #endregion

                #region iii.Air_Laboratory.OvenEncode 為.T.時或Air_Laboratory.NonOven=.T.時，判斷當Air_Laboratory.Oven 與Air_Laboratory.Wash 只要有一個為’Fail’則Air_Laboratory.Result=‘Fail’，否則填入Air_Laboratory.Result=‘Pass’
                if (Convert.ToBoolean(this.maindr["OvenEncode"]) || Convert.ToBoolean(this.maindr["NonOven"]))
                {
                    if (this.maindr["Oven"].ToString().Trim() == "Fail" || this.maindr["Wash"].ToString().Trim() == "Fail")
                    {
                        this.sql = string.Format(
                            @"update Air_Laboratory set Result='Fail' 
                                      where ID='{0}' and POID='{1}' and SEQ1='{2}' and SEQ2='{3}'",
                            this.ID, this.PoID, this.SEQ1, this.SEQ2);
                        DBProxy.Current.Execute(null, this.sql);
                    }
                    else
                    {
                        this.sql = string.Format(
                            @"update Air_Laboratory set Result='Pass' 
                                      where ID='{0}' and POID='{1}' and SEQ1='{2}' and SEQ2='{3}'",
                            this.ID, this.PoID, this.SEQ1, this.SEQ2);
                        DBProxy.Current.Execute(null, this.sql);
                    }
                }
                #endregion

                // ISP20200575 Encode全部執行後
                string sqlcmd = $@"select distinct orderid=o.ID from Orders o with(nolock) where o.poid = '{this.PoID}'";
                DataTable dtid;
                DualResult result1 = DBProxy.Current.Select(string.Empty, sqlcmd, out dtid);
                if (!result1)
                {
                    this.ShowErr(result1);
                }
                else
                {
                    string sqlup = $@"
update a 
set Status = 'Preparing'
from #tmp t
inner join AccessoryOrderList a with(nolock) on a.OrderID = t.orderid and a.Status = 'Waiting'
where dbo.GetAirQaRecord(t.orderid) ='PASS'
";
                    SqlConnection sqlConn = null;
                    DBProxy.Current.OpenConnection("ManufacturingExecution", out sqlConn);
                    result1 = MyUtility.Tool.ProcessWithDatatable(dtid, string.Empty, sqlup, out dtid, "#tmp", sqlConn);
                    if (!result1)
                    {
                        this.ShowErr(result1);
                    }
                }

                this.btnEncode.Text = "Amend";
                this.btnEdit.Enabled = false;
            }
            else if (this.btnEncode.Text == "Amend")
            {
                #region i.將Air_Laboratory.WashEncode 改為.F。ii.清空Air_Laboratory.Result。iii.填入Air_Laboratory.EditName=Userid, Air_Laboratory.EditDate=Datetime()
                this.sql = string.Format(
                    @"update Air_Laboratory set WashEncode=0 , Result='' ,  editName='{0}', editDate='{1}'
                                      where ID='{2}' and POID='{3}' and SEQ1='{4}' and SEQ2='{5}'",
                    Env.User.UserID, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), this.ID, this.PoID, this.SEQ1, this.SEQ2);
                DBProxy.Current.Execute(null, this.sql);
                #endregion

                this.btnEncode.Text = "Encode";

                // btnEncode.Enabled = false;
                this.btnEdit.Enabled = true;
            }

            // 更新PO.AIRLabInspPercent
            DualResult upResult;
            if (!(upResult = DBProxy.Current.Execute(null, $"exec UpdateInspPercent 'AIRLab','{this.maindr["POID"]}'")))
            {
                this.ShowErr(upResult);
                return;
            }

            this.OnRequery();
        }

        // 20161105 改變寫法
        // protected override bool OnSaveBefore()
        // {
        //    if (save.Text == "Edit")
        //    {
        //        txtScale.Enabled = true;
        //        comboResult.Enabled = true;
        //        txtRemark.Enabled = true;
        //        txtuser1.Enabled = true;
        //        WashDate.Enabled = true;
        //        save.Text = "save";
        //        this.btnEncode.Enabled = false;
        //        return false;
        //    }
        //    else
        //    {
        //        if (MyUtility.Check.Empty(txtuser1.TextBox1.Text))
        //        {
        //            CurrentData["WashInspector"] = Sci.Env.User.UserID;
        //        }
        //        if (MyUtility.Check.Empty(WashDate.Value))
        //        {
        //            CurrentData["WashDate"] = DateTime.Now;
        //        }
        //        txtScale.Enabled = false;
        //        comboResult.Enabled = false;
        //        txtRemark.Enabled = false;
        //        txtuser1.Enabled = false;
        //        WashDate.Enabled = false;
        //        save.Text = "Edit";
        //        this.btnEncode.Enabled = true;
        //        return true;
        //    }

        // }
        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.SetView(this.maindr);
            this.Close();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (this.btnEdit.Text == "Edit")
            {
                this.txtScale.Enabled = true;
                this.comboResult.Enabled = true;
                this.txtRemark.Enabled = true;
                this.txtuserLabTech.Enabled = true;
                this.dateInspectDate.Enabled = true;
                this.btnEdit.Text = "Save";
                this.btnEncode.Enabled = false;
                this.btnClose.Visible = false;
                this.undo.Visible = true;
                return;
            }
            else
            {
                if (MyUtility.Check.Empty(this.txtuserLabTech.TextBox1.Text))
                {
                    this.CurrentData["WashInspector"] = Env.User.UserID;
                }

                if (MyUtility.Check.Empty(this.dateInspectDate.Value))
                {
                    this.CurrentData["WashDate"] = DateTime.Now;
                }

                this.txtScale.Enabled = false;
                this.comboResult.Enabled = false;
                this.txtRemark.Enabled = false;
                this.txtuserLabTech.Enabled = false;
                this.dateInspectDate.Enabled = false;
                this.btnClose.Visible = true;
                this.undo.Visible = false;
                string sqlcmd = string.Format(
                    @"update AIR_Laboratory
set WashScale = '{3}',
Wash='{4}',
WashRemark='{5}',
WashInspector='{6}',
WashDate='{7}'
where POID='{0}' and seq1='{1}' and SEQ2='{2}'",
                    this.PoID, this.SEQ1, this.SEQ2, this.txtScale.Text, this.comboResult.Text, this.txtRemark.Text, this.txtuserLabTech.TextBox1.Text, ((DateTime)this.dateInspectDate.Value).ToShortDateString());
                DBProxy.Current.Execute(null, sqlcmd);
                this.btnEdit.Text = "Edit";
                this.btnEncode.Enabled = true;
            }
        }
    }
}
