using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Prg;
using Sci.Production.PublicPrg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P02_ImportFromPO
    /// </summary>
    public partial class P02_ImportFromFtyWK : Win.Subs.Base
    {
        private DataRow masterData;

        /// <summary>
        /// P02_ImportFromFtyWK
        /// </summary>
        /// <param name="masterData">masterData</param>
        public P02_ImportFromFtyWK(DataRow masterData)
        {
            this.InitializeComponent();
            this.masterData = masterData;
            this.displayCategory.Value = "Material";
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            Dictionary<string, string> di_inhouseOsp = new Dictionary<string, string>();
            di_inhouseOsp.Add(string.Empty, "All");
            di_inhouseOsp.Add("O", "OSP");
            di_inhouseOsp.Add("I", "InHouse");


            DataGridViewGeneratorTextColumnSettings ctnno = new DataGridViewGeneratorTextColumnSettings();
            DataGridViewGeneratorTextColumnSettings receiver = new DataGridViewGeneratorTextColumnSettings();

            // CTNNo要Trim掉空白字元
            ctnno.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.gridImport.GetDataRow<DataRow>(e.RowIndex);
                    dr["CTNNo"] = MyUtility.Convert.GetString(e.FormattedValue).Trim();
                }
            };
            receiver.CharacterCasing = CharacterCasing.Normal;
            this.gridImport.IsEditingReadOnly = false;
            this.gridImport.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridImport)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out Ict.Win.UI.DataGridViewCheckBoxColumn col_chk)
                .Text("POID", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                .Text("Seq1", header: "Seq1#", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("Seq2", header: "Seq2#", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("Type", header: "Type", width: Widths.AnsiChars(3), iseditingreadonly: true)
                .Text("MtlTypeID", header: "Material Type", width: Widths.AnsiChars(2), iseditingreadonly: true)
                .Text("Supp", header: "Supplier", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("Receiver", header: "Receiver", width: Widths.AnsiChars(10), settings: receiver, iseditingreadonly: false)
                .Text("Leader", header: "Team Leader", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Text("BrandID", header: "Brand", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Text("CTNNo", header: "CTN No.", width: Widths.AnsiChars(5), settings: ctnno)
                .Numeric("Price", header: "Price", decimal_places: 4, iseditingreadonly: true)
                .Numeric("Qty", header: "Qty", integer_places: 6, decimal_places: 2, maximum: 999999.99m, minimum: 0m, iseditingreadonly: true)
                .Text("UnitID", header: "Unit", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("NW", header: "N.W. (kg)", integer_places: 5, decimal_places: 2, maximum: 99999.99m, minimum: 0m);

            this.gridImport.Columns["Selected"].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 191);
            this.gridImport.Columns["Receiver"].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 191);
            this.gridImport.Columns["CTNNo"].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 191);
            this.gridImport.Columns["NW"].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 191);

            this.gridImport.Columns["Receiver"].DefaultCellStyle.ForeColor = Color.Red;
            this.gridImport.Columns["CTNNo"].DefaultCellStyle.ForeColor = Color.Red;
            this.gridImport.Columns["NW"].DefaultCellStyle.ForeColor = Color.Red;
        }

        // Find Now
        private void BtnFindNow_Click(object sender, EventArgs e)
        {
            if (MyUtility.Check.Empty(this.txtWKNo.Text))
            {
                this.txtWKNo.Focus();
                MyUtility.Msg.WarningBox("Fty WK# cannot be empty!!");
                return;
            }

            if (MyUtility.Convert.GetString(this.txtWKNo.Text).IndexOf("'") != -1)
            {
                this.txtWKNo.Text = this.txtWKNo.Text.Replace("'", string.Empty);
            }

            var checkHC = MyUtility.GetValue.Lookup($"select ID from Express_Detail ed WHERE ed.DutyNo = '{this.txtWKNo.Text}'");
            if (!MyUtility.Check.Empty(checkHC))
            {
                if (MyUtility.Convert.GetString(this.masterData["ID"]) != checkHC)
                {
                    MyUtility.Msg.WarningBox($"The Fty WK# is already imported to another HC#({checkHC})");
                    return;
                }
            }
            string sqlCmd =
                $@"
select Selected = 0 
,fd.ID
,fd.POID
,fd.Seq1
,fd.Seq2
,[Type] = (case when fd.[FabricType] = 'F' then 'Fabric' when fd.[FabricType] = 'A' then 'Accessory' else '' end)
,fd.[MtlTypeID]
,o.SeasonID
,o.StyleID
,psd.Refno
,ColorID = psds.SpecValue
,[Supp] = ps.SuppID+'-'+s.AbbEN
,[SuppID] = ps.SuppID
,[CTNNo] = ''
,[NW] = fd.NetKg
,[Price] = fd.Price
,fd.Qty
,[UnitID]= fd.UnitID
,[Category] = 'Material' --4
,[Receiver]=''
,[BrandID] = o.BrandID
,[Leader]=o.SMR
,[Remark]=''
,[InCharge]=''
,[AddName]=''
,[AddDate]=''
from FtyExport_Detail fd with (nolock)
left join FtyExport f with (nolock) on fd.id = f.id
left join PO_Supp_Detail psd with (nolock) on psd.ID = fd.POID and psd.SEQ1 = fd.Seq1 and psd.SEQ2 = fd.Seq2
	    left join PO_Supp_Detail_Spec psds WITH (NOLOCK) on psds.ID = psd.id and psds.seq1 = psd.seq1 and psds.seq2 = psd.seq2 and psds.SpecColumnID = 'Color'
left join PO_Supp ps WITH (NOLOCK) on fd.POID = ps.ID and fd.SEQ1 = ps.SEQ1
left join Supp s WITH (NOLOCK) on ps.SuppID = s.ID
left join orders o with(nolock) on o.ID = fd.POID
where
not exists (
    select 1 
    from Express_Detail ed with(nolock)
    WHERE ed.DutyNo = fd.id
    and ed.OrderID = fd.POID
    and ed.Seq1 = fd.Seq1
    and ed.Seq2 = fd.Seq2
)
and f.OrderCompanyID = '{P02.orderCompanyID}'
and fd.ID ='{this.txtWKNo.Text}'
";
            DualResult result = DBProxy.Current.Select(null, sqlCmd, out DataTable selectData);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (selectData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!!");
            }

            this.listControlBindingSource1.DataSource = selectData;
        }

        // Update
        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            this.gridImport.ValidateControl();
            this.listControlBindingSource1.EndEdit();
            if (this.listControlBindingSource1.DataSource == null)
            {
                return;
            }

            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            var selectDT = dt.AsEnumerable().Where(x => MyUtility.Convert.GetInt(x["Selected"]) == 1).TryCopyToDataTable(dt);
            if (selectDT.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("No data, cannot import!");
                return;
            }

            if (!this.BeforeUpdate(selectDT))
            {
                return;
            }

            IList<string> insertCmds = new List<string>();
            foreach (DataRow dr in selectDT.Rows)
            {
                string sqlChk = $@"select ID,DutyNo from Express_Detail where DutyNo = '{dr["ID"]}'";
                if (MyUtility.Check.Seek(sqlChk, out DataRow drChk))
                {
                    if ((string)this.masterData["ID"] != (string)drChk["ID"])
                    {
                        MyUtility.Msg.WarningBox($@"Fty WK# <{dr["ID"]}> already exists in HC No. <{drChk["ID"]}>");
                        return;
                    }
                }

                sqlChk = $@"select ExpressID from FtyExport where ID = '{dr["ID"]}' and ExpressID !=''";
                if (MyUtility.Check.Seek(sqlChk, out drChk))
                {
                    if ((string)this.masterData["ID"] != (string)drChk["ExpressID"])
                    {
                        MyUtility.Msg.WarningBox($@"Fty WK# <{dr["ID"]}> already exists in HC No. <{drChk["ExpressID"]}>");
                        return;
                    }
                }

                insertCmds.Add($@"
insert into Express_Detail(ID,OrderID,Seq1,Seq2,SeasonID,StyleID,Qty,NW,CTNNo,Category,SuppID,Price,UnitID,Receiver,BrandID,Leader,Remark,InCharge,AddName,AddDate,DutyNo)
 values('{MyUtility.Convert.GetString(this.masterData["ID"])}'
,'{MyUtility.Convert.GetString(dr["POID"])}'
,'{MyUtility.Convert.GetString(dr["Seq1"])}'
,'{MyUtility.Convert.GetString(dr["Seq2"])}'
,'{MyUtility.Convert.GetString(dr["SeasonID"])}'
,'{MyUtility.Convert.GetString(dr["StyleID"])}'
, {MyUtility.Convert.GetString(dr["Qty"])}
, {MyUtility.Convert.GetString(dr["NW"])}
,'{MyUtility.Convert.GetString(dr["CTNNo"])}'
,'4'
,'{MyUtility.Convert.GetString(dr["SuppID"])}'
, {MyUtility.Convert.GetString(dr["Price"])}
,'{MyUtility.Convert.GetString(dr["UnitID"])}'
,'{MyUtility.Convert.GetString(dr["Receiver"])}'
,'{MyUtility.Convert.GetString(dr["BrandID"])}'
,'{MyUtility.Convert.GetString(dr["Leader"])}'
,'{this.txtRemark.Text.Replace("'", "''")}'
,'{Env.User.UserID}'
,'{Env.User.UserID}'
,GETDATE()
,'{MyUtility.Convert.GetString(dr["ID"])}'
);

update FtyExport
set ExpressID = '{MyUtility.Convert.GetString(this.masterData["ID"])}'
,EditDate = GetDate()
,EditName = '{Env.User.UserID}'
where ID = '{dr["ID"]}'
");
            }

            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    DualResult result = DBProxy.Current.Executes(null, insertCmds);
                    if (!result)
                    {
                        transactionScope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    result = DBProxy.Current.Execute(null, Prgs.ReCalculateExpress(MyUtility.Convert.GetString(this.masterData["ID"])));
                    if (!result)
                    {
                        transactionScope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    transactionScope.Complete();
                }
                catch (Exception ex)
                {
                    transactionScope.Dispose();
                    this.ShowErr("Commit transaction error.", ex);
                    return;
                }
            }

            MyUtility.Msg.InfoBox("Import complete!!");

            DataRow[] drfound = dt.Select("selected = 1");

            foreach (var item in drfound)
            {
               dt.Rows.Remove(item);
            }
        }

        private bool BeforeUpdate(DataTable sourcedt)
        {
            if (MyUtility.Check.Empty(this.txtRemark.Text))
            {
                this.txtRemark.Focus();
                MyUtility.Msg.WarningBox("Remark can't empty!!");
                return false;
            }

            // 該單Approved / Junk都不允許調整資料
            if (!Prgs.CheckP02Status(this.masterData["ID"].ToString()))
            {
                return false;
            }

            StringBuilder chkQty = new StringBuilder();
            foreach (DataRow dr in sourcedt.Rows)
            {
                if (MyUtility.Check.Empty(dr["Receiver"]))
                {
                    MyUtility.Msg.WarningBox("Receiver can't empty!!");
                    return false;
                }

                if (MyUtility.Check.Empty(dr["CTNNo"]))
                {
                    MyUtility.Msg.WarningBox("CTN No. can't empty!!");
                    return false;
                }

                if (MyUtility.Check.Empty(dr["NW"]))
                {
                    MyUtility.Msg.WarningBox("N.W. (kg) can't empty!!");
                    return false;
                }

                if (MyUtility.Check.Empty(dr["Qty"]))
                {
                    chkQty.Append($"SP#:{MyUtility.Convert.GetString(dr["ID"])}, Seq1#:{MyUtility.Convert.GetString(dr["Seq1"])}, Seq2#:{MyUtility.Convert.GetString(dr["Seq2"])}\r\n");
                }
            }

            // Qty不可為0
            if (chkQty.Length > 0)
            {
                MyUtility.Msg.WarningBox("Q'ty can't be 0!!\r\n" + chkQty.ToString());
                return false;
            }

            return true;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.listControlBindingSource1.EndEdit();
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            if (dt == null || dt.Rows.Count == 0)
            {
                return;
            }

            DataRow[] drfound = dt.Select("selected = 1");

            foreach (var item in drfound)
            {
                item["Receiver"] = this.txtReceiver.Text;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.listControlBindingSource1.EndEdit();
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            if (dt == null || dt.Rows.Count == 0)
            {
                return;
            }

            DataRow[] drfound = dt.Select("selected = 1");

            foreach (var item in drfound)
            {
                item["CTNNo"] = this.txtCtnNo.Text;
            }
        }
    }
}
