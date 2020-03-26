using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    public partial class P21 : Sci.Win.Tems.QueryForm
    {
        DataTable dtReceiving = new DataTable();
        public P21(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
            this.EditMode = true;
            Dictionary<String, String> comboBox1_RowSource = new Dictionary<string, string>();
            comboBox1_RowSource.Add("", "All");
            comboBox1_RowSource.Add("F", "Fabric");
            comboBox1_RowSource.Add("A", "Accessory");
            cmbMaterialType.DataSource = new BindingSource(comboBox1_RowSource, null);
            cmbMaterialType.ValueMember = "Key";
            cmbMaterialType.DisplayMember = "Value";
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            Ict.Win.DataGridViewGeneratorNumericColumnSettings cellActWeight = new DataGridViewGeneratorNumericColumnSettings();
            cellActWeight.CellValidating += (s, e) =>
            {
                DataRow curDr = this.gridReceiving.GetDataRow(e.RowIndex);
                curDr["Differential"] = (decimal)e.FormattedValue - (decimal)curDr["Weight"];
                curDr["ActualWeight"] = e.FormattedValue;
                curDr.EndEdit();

                this.DifferentialColorChange(e.RowIndex);
                this.selectModify(e.RowIndex);
                this.gridReceiving.RefreshEdit();
            };

            Ict.Win.DataGridViewGeneratorTextColumnSettings cellLocation = new DataGridViewGeneratorTextColumnSettings();
            cellLocation.CellMouseDoubleClick += (s, e) =>
            {
                this.GridLocationCellPop(e.RowIndex);
            };

            cellLocation.EditingMouseUp += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    this.GridLocationCellPop(e.RowIndex);
                }
            };

            cellLocation.CellValidating += (s, e) =>
            {
                DataRow curDr = this.gridReceiving.GetDataRow(e.RowIndex);
                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    curDr["Location"] = string.Empty;
                    this.selectModify(e.RowIndex);
                    return;
                }               
               
                string[] locationList = e.FormattedValue.ToString().Split(',');
                
                string notLocationExistsList = locationList.Where(a => !PublicPrg.Prgs.CheckLocationExists(curDr["StockType"].ToString(), a)).JoinToString(",");

                if (!MyUtility.Check.Empty(notLocationExistsList))
                {
                    e.Cancel = true;
                    MyUtility.Msg.WarningBox($"Location<{notLocationExistsList}> not Found");
                    return;
                }
                else
                {
                    curDr["Location"] = e.FormattedValue.ToString();
                    curDr.EndEdit();
                }

                this.selectModify(e.RowIndex);
                this.gridReceiving.RefreshEdit();
            };

            Helper.Controls.Grid.Generator(this.gridReceiving)
                 .CheckBox("select", header: "", trueValue: 1, falseValue: 0)
                 .Text("ID", header: "Receiving ID", width: Widths.AnsiChars(14), iseditingreadonly: true)
                 .Text("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                 .Text("Seq", header: "Seq", width: Widths.AnsiChars(8), iseditingreadonly: true)
                 .Text("Roll", header: "Roll#", width: Widths.AnsiChars(8), iseditingreadonly: true)
                 .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true)
                 .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)
                 .Text("Color", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
                 .Numeric("ActualQty", header: "Qty", width: Widths.AnsiChars(10), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
                 .Text("StockTypeDesc", header: "Stock Type", width: Widths.AnsiChars(8), iseditingreadonly: true)
                 .Text("Location", header: "Location", width: Widths.AnsiChars(12), settings: cellLocation)
                 .Numeric("Weight", header: "G.W(kg)", width: Widths.AnsiChars(8), decimal_places: 2, iseditingreadonly: true)
                 .Numeric("ActualWeight", header: "Act.(kg)", width: Widths.AnsiChars(8), decimal_places: 2, settings: cellActWeight)
                 .Numeric("Differential", header: "Differential", width: Widths.AnsiChars(8), decimal_places: 2, iseditingreadonly: true)
                 .Text("Remark", header: "Remark", width: Widths.AnsiChars(15))
                 .Text("LastRemark", header: "Last P26 Remark data", width: Widths.AnsiChars(15), iseditingreadonly: true)
                 .DateTime("LastEditDate", header: "Last Edit Date", width: Widths.AnsiChars(20), iseditingreadonly: true)
                 ;

            this.gridReceiving.Columns["Location"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridReceiving.Columns["ActualWeight"].DefaultCellStyle.BackColor = Color.Pink;
            this.gridReceiving.Columns["Remark"].DefaultCellStyle.BackColor = Color.Pink;
        }

        private void DifferentialColorChange(int rowIndex)
        {
            if ((decimal)this.gridReceiving.Rows[rowIndex].Cells["Differential"].Value < 0)
            {
                this.gridReceiving.Rows[rowIndex].Cells["Differential"].Style.ForeColor = Color.Red;
            }
            else
            {
                this.gridReceiving.Rows[rowIndex].Cells["Differential"].Style.ForeColor = Color.Black;
            }
        }

        private void GridFormatChange()
        {
            foreach (DataGridViewRow item in this.gridReceiving.Rows)
            {
                this.DifferentialColorChange(item.Index);
            }
        }

        private void GridLocationCellPop(int rowIndex)
        {
            DataRow curDr = this.gridReceiving.GetDataRow(rowIndex);
            SelectItem2 selectItem2 = PublicPrg.Prgs.SelectLocation(curDr["StockType"].ToString());
            selectItem2.ShowDialog();
            if (selectItem2.DialogResult == DialogResult.OK)
            {
                curDr["Location"] = selectItem2.GetSelecteds().Select(s => s["ID"].ToString()).JoinToString(",");
                this.gridReceiving.Rows[rowIndex].Cells["Location"].Value = curDr["Location"];
            }
        }

        private void Query()
        {
            string sqlWhere = string.Empty;

            if (!txtSeq.checkSeq1Empty() && txtSeq.checkSeq2Empty())
            {
                sqlWhere += $" and rd.seq1 = '{this.txtSeq.seq1}'";
            }
            else if (!txtSeq.checkEmpty(showErrMsg: false))
            {
                sqlWhere += $" and rd.seq1 = '{this.txtSeq.seq1}' and rd.seq2 = '{this.txtSeq.seq2}'";
            }

            if (!MyUtility.Check.Empty(this.txtRef.Text))
            {
                sqlWhere += $" and psd.refno = '{this.txtRef.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtColor.Text))
            {
                sqlWhere += $" and (psd.SuppColor = '{this.txtColor.Text}' or psd.ColorID = '{this.txtColor.Text}')";
            }

            if (!MyUtility.Check.Empty(this.txtRoll.Text))
            {
                sqlWhere += $" and rd.roll like '%{this.txtRoll.Text}%'";
            }

            if (!MyUtility.Check.Empty(this.txtDyelot.Text))
            {
                sqlWhere += $" and rd.dyelot = '{this.txtDyelot.Text}'";
            }

            if (!MyUtility.Check.Empty(this.cmbMaterialType.SelectedValue.ToString()))
            {
                sqlWhere += $" and psd.FabricType = '{this.cmbMaterialType.SelectedValue.ToString()}'";
            }

            if (!MyUtility.Check.Empty(this.txtRecivingID.Text))
            {
                sqlWhere += $" and r.ID = '{this.txtRecivingID.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtWK.Text))
            {
                sqlWhere += $" and r.ExportID = '{this.txtWK.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtSP.Text))
            {
                sqlWhere += $" and rd.POID like '%{this.txtSP.Text}%'";
            }

            if (!MyUtility.Check.Empty(this.dateBoxArriveWH.Value))
            {
                sqlWhere += $" and r.WhseArrival = '{this.dateBoxArriveWH.Text}'";
            }

            if (MyUtility.Check.Empty(sqlWhere))
            {
                MyUtility.Msg.WarningBox("The criteria can't all be empty.");
                return;
            }

            string sqlQuery = $@"
select
[ID] = REPLACE(ID,'''',''),
Name
into #tmpStockType
from DropDownList WITH (NOLOCK) where Type = 'Pms_StockType'

select
[select] = 0,
rd.Id,
rd.PoId,
[Seq] = rd.Seq1 + ' ' + rd.Seq2,
rd.Roll,
rd.Dyelot,
[Description] = dbo.getmtldesc(rd.POID, rd.Seq1, rd.Seq2, 2, 0),
[Color] = iif(fb.MtlTypeID = 'EMB THREAD' OR fb.MtlTypeID = 'SP THREAD' OR fb.MtlTypeID = 'THREAD' , psd.SuppColor, psd.ColorID),
rd.ActualQty,
[StockTypeDesc] = st.Name,
rd.StockType,
rd.Location,
[OldLocation] = rd.Location,
rd.Weight,
rd.ActualWeight,
[OldActualWeight] = rd.ActualWeight,
[Differential] = rd.ActualWeight - rd.Weight,
[FtyInventoryUkey] = fi.Ukey,
[FtyInventoryQty] = fi.InQty - fi.OutQty + fi.AdjustQty,
rd.Seq1,
rd.Seq2
,[Remark]=''
,[LastRemark] = LastRemark.Remark
,[LastEditDate]=LastEditDate.Val
from  Receiving r with (nolock)
inner join Receiving_Detail rd with (nolock) on r.ID = rd.ID
inner join PO_Supp_Detail psd with (nolock) on rd.PoId = psd.ID and rd.Seq1 = psd.SEQ1 and rd.Seq2 = psd.SEQ2
inner join Fabric fb with (nolock) on psd.SCIRefno = fb.SCIRefno
inner join Ftyinventory  fi with (nolock) on    rd.POID = fi.POID and
                                                rd.Seq1 = fi.Seq1 and
                                                rd.Seq2 = fi.Seq2 and
                                                rd.Roll = fi.Roll and
                                                rd.Dyelot  = fi.Dyelot and
                                                rd.StockType = fi.StockType
left join #tmpStockType st with (nolock) on st.ID = rd.StockType
OUTER APPLY(
	SELECT [Val]=MAX(lt.EditDate)
	FROM LocationTrans lt
	INNER JOIN LocationTrans_detail ltd ON lt.ID=ltd.ID
	WHERE lt.Status='Confirmed' AND ltd.FtyInventoryUkey=fi.Ukey 
)LastEditDate
OUTER APPLY(
	SELECT lt.Remark
	FROM LocationTrans lt
	INNER JOIN LocationTrans_detail ltd ON lt.ID=ltd.ID
	WHERE lt.Status='Confirmed' 
    AND ltd.FtyInventoryUkey=fi.Ukey 
	AND lt.EditDate = LastEditDate.Val
)LastRemark

where r.MDivisionID  = '{Env.User.Keyword}' {sqlWhere}
";

            DualResult result = DBProxy.Current.Select(null, sqlQuery, out this.dtReceiving);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (this.dtReceiving.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found");
                this.gridReceiving.DataSource = this.dtReceiving;
                return;
            }

            this.gridReceiving.DataSource = this.dtReceiving;
            this.GridFormatChange();
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            this.Query();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gridReceiving_Sorted(object sender, EventArgs e)
        {
            this.GridFormatChange();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            var selectedReceiving = this.dtReceiving.AsEnumerable().Where(s => (int)s["select"] == 1);

            if (!selectedReceiving.Any())
            {
                MyUtility.Msg.WarningBox("Please select rows first!");
                return;
            }

            if (selectedReceiving.Any(s => MyUtility.Check.Empty(s["Location"])))
            {
                MyUtility.Msg.WarningBox("Location can not be empty");
                return;
            }

            var selectedReceivingSummary = selectedReceiving
                                            .Where(s => s["Location"].ToString() != s["OldLocation"].ToString())
                                            .GroupBy(s => new
                                            {
                                                POID = s["POID"].ToString(),
                                                Seq1 = s["Seq1"].ToString(),
                                                Seq2 = s["Seq2"].ToString(),
                                                Roll = s["Roll"].ToString(),
                                                Dyelot = s["Dyelot"].ToString(),
                                                StockType = s["StockType"].ToString(),
                                                FtyInventoryQty = (decimal)s["FtyInventoryQty"],
                                                FtyInventoryUkey = (Int64)s["FtyInventoryUkey"]
                                            })
                                            .Select(s => new
                                            {
                                                s.Key.POID,
                                                s.Key.Seq1,
                                                s.Key.Seq2,
                                                s.Key.Roll,
                                                s.Key.Dyelot,
                                                s.Key.StockType,
                                                s.Key.FtyInventoryQty,
                                                s.Key.FtyInventoryUkey,
                                                Location = s.Select(d => d["Location"].ToString()).Distinct().JoinToString(","),
                                                OldLocation = s.Select(d => d["OldLocation"].ToString()).Distinct().JoinToString(","),
                                                Remark = s.Select(d => d["Remark"].ToString()).Distinct().JoinToString(",")
                                            });

            string sqlUpdateReceiving_Detail = string.Empty;
            string sqlInsertLocationTrans = string.Empty;
            string locationTransID = Sci.MyUtility.GetValue.GetID(Sci.Env.User.Keyword + "LH", "LocationTrans", DateTime.Now);

            foreach (var receivingDetailItem in selectedReceiving)
            {
                sqlUpdateReceiving_Detail += $@"update Receiving_Detail set ActualWeight  = {receivingDetailItem["ActualWeight"]},
                                                                            Location = '{receivingDetailItem["Location"]}'
                                                    where   ID = '{receivingDetailItem["ID"]}' and
                                                            POID = '{receivingDetailItem["POID"]}' and
                                                            Seq1 = '{receivingDetailItem["Seq1"]}' and
                                                            Seq2 = '{receivingDetailItem["Seq2"]}' and
                                                            Roll = '{receivingDetailItem["Roll"]}' and
                                                            Dyelot = '{receivingDetailItem["Dyelot"]}'
";
            }


            if (selectedReceivingSummary.Any())
            {
                List<string> remarks = new List<string>();

                foreach (var receivingItem in selectedReceivingSummary)
                {
                    remarks.Add(receivingItem.Remark);
                }

                string Remark = remarks.JoinToString(",");
                if (Remark.Length >= (60-19))  // 預設要填入---Create from P21.，因此要扣掉這個文字長度
                {
                    MyUtility.Msg.WarningBox("Remark is too long!");
                    return;
                }

                sqlInsertLocationTrans += $@"
Insert into LocationTrans(ID,MDivisionID,FactoryID,IssueDate,Status,Remark,AddName,AddDate,EditName,EditDate)
            values( '{locationTransID}',
                    '{Env.User.Keyword}',
                    '{Env.User.Factory}',
                    GetDate(),
                    'Confirmed',
                    '{Remark}---Create from P21.',
                    '{Env.User.UserID}',
                    GetDate(),
                    '{Env.User.UserID}',
                    GetDate()
                )
";

                foreach (var receivingItem in selectedReceivingSummary)
                {
                    sqlInsertLocationTrans += $@"
Insert into LocationTrans_Detail(   ID,
                                    FtyInventoryUkey,
                                    POID,
                                    Seq1,
                                    Seq2,
                                    Roll,
                                    Dyelot,
                                    FromLocation,
                                    ToLocation,
                                    Qty,
                                    StockType)
                values('{locationTransID}',
                       {receivingItem.FtyInventoryUkey},
                       '{receivingItem.POID}',
                       '{receivingItem.Seq1}',
                       '{receivingItem.Seq2}',
                       '{receivingItem.Roll}',
                       '{receivingItem.Dyelot}',
                       '{receivingItem.OldLocation}',
                       '{receivingItem.Location}',
                       {receivingItem.FtyInventoryQty},
                       '{receivingItem.StockType}')
           
";
                }
                sqlInsertLocationTrans += $"select * from LocationTrans_Detail where ID = '{locationTransID}'";
            }

            TransactionScope _transactionscope = new TransactionScope();
            using (_transactionscope)
            {
                try
                {
                    DualResult result;
                    if (selectedReceivingSummary.Any())
                    {
                        DataTable dtLocationTransDetail;
                        result = DBProxy.Current.Select(null, sqlInsertLocationTrans, out dtLocationTransDetail);
                        if (!result)
                        {
                            _transactionscope.Dispose();
                            this.ShowErr(result);
                            return;
                        }

                        result = Prgs.UpdateFtyInventoryMDivisionPoDetail(dtLocationTransDetail.AsEnumerable().ToList());
                        if (!result)
                        {
                            _transactionscope.Dispose();
                            this.ShowErr(result);
                            return;
                        }
                    }

                    result = DBProxy.Current.Execute(null, sqlUpdateReceiving_Detail);
                    if (!result)
                    {
                        _transactionscope.Dispose();
                        this.ShowErr(result);
                        return;
                    }

                    _transactionscope.Complete();

                }
                catch (Exception ex)
                {
                    _transactionscope.Dispose();
                    this.ShowErr(ex);
                    return;
                }
            }
            this.Query();
            MyUtility.Msg.InfoBox("Complete");

        }

        /// <summary>
        /// 檢查表身Location,ActualWeight是否有被修改過(跟DB資料比較)
        /// 有被修改過,就自動勾選資料
        /// </summary>
        /// <param name="rowIndex"></param>
        private void selectModify(int rowIndex)
        {
            DataRow dr = this.gridReceiving.GetDataRow(rowIndex);
            bool chg_ActWeight = false;
            bool chg_Location = false;

            decimal oldActualWeight = MyUtility.Convert.GetDecimal(dr["OldActualWeight"]);
            decimal newActualWeight = MyUtility.Convert.GetDecimal(dr["ActualWeight"]);
            if (!oldActualWeight.Equals(newActualWeight))
            {
                chg_ActWeight = true;
            }
            else
            {
                chg_ActWeight = false;
            }

            // 判斷Location 有變更資料就自動勾選
            string oldvalue = dr["OldLocation"].ToString();
            string newvalue = dr["Location"].ToString();            
            if (!oldvalue.Equals(newvalue))
            {
                chg_Location = true;
            }
            else
            {
                chg_Location = false;
            }

            if (chg_Location || chg_ActWeight)
            {
                dr["select"] = 1;
            }
            else
            {
                dr["select"] = 0;
            }
        }
    }
}
