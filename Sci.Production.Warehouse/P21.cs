using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.PublicPrg;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Warehouse
{
    public partial class P21 : Win.Tems.QueryForm
    {
        private DataTable dtReceiving = new DataTable();

        public P21(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.EditMode = true;
            Dictionary<string, string> comboBox1_RowSource = new Dictionary<string, string>();
            comboBox1_RowSource.Add(string.Empty, "All");
            comboBox1_RowSource.Add("F", "Fabric");
            comboBox1_RowSource.Add("A", "Accessory");
            this.cmbMaterialType.DataSource = new BindingSource(comboBox1_RowSource, null);
            this.cmbMaterialType.ValueMember = "Key";
            this.cmbMaterialType.DisplayMember = "Value";
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            DataGridViewGeneratorNumericColumnSettings cellActWeight = new DataGridViewGeneratorNumericColumnSettings();
            cellActWeight.CellValidating += (s, e) =>
            {
                DataRow curDr = this.gridReceiving.GetDataRow(e.RowIndex);
                if (curDr["ReceivingSource"].ToString() != "Receiving")
                {
                    return;
                }

                curDr["Differential"] = (decimal)e.FormattedValue - (decimal)curDr["Weight"];
                curDr["ActualWeight"] = e.FormattedValue;
                curDr.EndEdit();

                this.DifferentialColorChange(e.RowIndex);
                this.SelectModify(e.RowIndex);
                this.gridReceiving.RefreshEdit();
            };

            DataGridViewGeneratorTextColumnSettings cellLocation = new DataGridViewGeneratorTextColumnSettings();
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
                    this.SelectModify(e.RowIndex);
                    return;
                }

                string[] locationList = e.FormattedValue.ToString().Split(',');

                string notLocationExistsList = locationList.Where(a => !Prgs.CheckLocationExists(curDr["StockType"].ToString(), a)).JoinToString(",");

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

                this.SelectModify(e.RowIndex);
                this.gridReceiving.RefreshEdit();
            };

            DataGridViewGeneratorCheckBoxColumnSettings col_Select = new DataGridViewGeneratorCheckBoxColumnSettings();
            col_Select.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridReceiving.GetDataRow(e.RowIndex);
                bool isCheck = MyUtility.Convert.GetBool(e.FormattedValue);
                dr["select"] = isCheck;
                dr.EndEdit();
                DataTable dt = (DataTable)this.gridReceiving.DataSource;
                if (dt != null || dt.Rows.Count > 0)
                {
                    int cnt = MyUtility.Convert.GetInt(dt.Compute("count(select)", "select = 1")); // + (isCheck ? 1 : -1);
                    this.numSelectCnt.Value = cnt;
                }
            };

            this.Helper.Controls.Grid.Generator(this.gridReceiving)
                 .CheckBox("select", header: string.Empty, trueValue: 1, falseValue: 0, settings: col_Select)
                 .Text("ExportID", header: "WK#", width: Widths.AnsiChars(14), iseditingreadonly: true)

                 // .Text("ID", header: "Receiving ID", width: Widths.AnsiChars(14), iseditingreadonly: true)
                 .Text("poid", header: "SP#", width: Widths.AnsiChars(13), iseditingreadonly: true)
                 .Text("Seq", header: "Seq", width: Widths.AnsiChars(8), iseditingreadonly: true)
                 .Text("Roll", header: "Roll#", width: Widths.AnsiChars(8), iseditingreadonly: true)
                 .Text("Dyelot", header: "Dyelot", width: Widths.AnsiChars(8), iseditingreadonly: true)
                 .EditText("Description", header: "Description", width: Widths.AnsiChars(20), iseditingreadonly: true)
                 .Text("Color", header: "Color", width: Widths.AnsiChars(6), iseditingreadonly: true)
                 .Numeric("StockQty", header: "Qty", width: Widths.AnsiChars(10), decimal_places: 2, integer_places: 10, iseditingreadonly: true)
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
            SelectItem2 selectItem2 = Prgs.SelectLocation(curDr["StockType"].ToString());
            selectItem2.ShowDialog();
            if (selectItem2.DialogResult == DialogResult.OK)
            {
                curDr["Location"] = selectItem2.GetSelecteds().Select(s => s["ID"].ToString()).JoinToString(",");
                this.gridReceiving.Rows[rowIndex].Cells["Location"].Value = curDr["Location"];
            }

            curDr.EndEdit();
        }

        private void Query()
        {
            string sqlWhere = string.Empty;
            string sqlWhere2 = string.Empty;

            if (!this.txtSeq.CheckSeq1Empty() && this.txtSeq.CheckSeq2Empty())
            {
                sqlWhere += $" and rd.seq1 = '{this.txtSeq.Seq1}'";
                sqlWhere2 += $" and td.seq1 = '{this.txtSeq.Seq1}'";
            }
            else if (!this.txtSeq.CheckEmpty(showErrMsg: false))
            {
                sqlWhere += $" and rd.seq1 = '{this.txtSeq.Seq1}' and rd.seq2 = '{this.txtSeq.Seq2}'";
                sqlWhere2 += $" and td.seq1 = '{this.txtSeq.Seq1}' and td.seq2 = '{this.txtSeq.Seq2}'";
            }

            if (!MyUtility.Check.Empty(this.txtRef.Text))
            {
                sqlWhere += $" and psd.refno = '{this.txtRef.Text}'";
                sqlWhere2 += $" and psd.refno = '{this.txtRef.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtColor.Text))
            {
                sqlWhere += $" and (psd.SuppColor = '{this.txtColor.Text}' or psd.ColorID = '{this.txtColor.Text}')";
                sqlWhere2 += $" and (psd.SuppColor = '{this.txtColor.Text}' or psd.ColorID = '{this.txtColor.Text}')";
            }

            if (!MyUtility.Check.Empty(this.txtRoll.Text))
            {
                sqlWhere += $" and rd.roll like '%{this.txtRoll.Text}%'";
                sqlWhere2 += $" and td.roll like '%{this.txtRoll.Text}%'";
            }

            if (!MyUtility.Check.Empty(this.txtDyelot.Text))
            {
                sqlWhere += $" and rd.dyelot = '{this.txtDyelot.Text}'";
                sqlWhere2 += $" and td.dyelot = '{this.txtDyelot.Text}'";
            }

            if (!MyUtility.Check.Empty(this.cmbMaterialType.SelectedValue.ToString()))
            {
                sqlWhere += $" and psd.FabricType = '{this.cmbMaterialType.SelectedValue.ToString()}'";
                sqlWhere2 += $" and psd.FabricType = '{this.cmbMaterialType.SelectedValue.ToString()}'";
            }

            if (!MyUtility.Check.Empty(this.txtRecivingID.Text))
            {
                sqlWhere += $" and r.ID = '{this.txtRecivingID.Text}'";
                sqlWhere2 += $" and t.ID = '{this.txtRecivingID.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtWK.Text))
            {
                sqlWhere += $" and r.ExportID = '{this.txtWK.Text}'";
                sqlWhere2 += $" and 1=0 ";
            }

            if (!MyUtility.Check.Empty(this.txtSP.Text))
            {
                sqlWhere += $" and rd.POID like '%{this.txtSP.Text}%'";
                sqlWhere2 += $" and td.POID like '%{this.txtSP.Text}%'";
            }

            if (!MyUtility.Check.Empty(this.dateBoxArriveWH.Value1))
            {
                sqlWhere += $" and r.WhseArrival >= '{Convert.ToDateTime(this.dateBoxArriveWH.Value1).ToString("yyyy/MM/dd")}'";
                sqlWhere2 += $" and t.IssueDate >= '{Convert.ToDateTime(this.dateBoxArriveWH.Value1).ToString("yyyy/MM/dd")}'";
            }

            if (!MyUtility.Check.Empty(this.dateBoxArriveWH.Value2))
            {
                sqlWhere += $" and r.WhseArrival <= '{Convert.ToDateTime(this.dateBoxArriveWH.Value2).ToString("yyyy/MM/dd")}'";
                sqlWhere2 += $" and t.IssueDate <= '{Convert.ToDateTime(this.dateBoxArriveWH.Value2).ToString("yyyy/MM/dd")}'";
            }

            if (!MyUtility.Check.Empty(this.txtMtlLocation.Text))
            {
                sqlWhere += $@"
and exists(
	select 1 from FtyInventory_Detail fid 
	where fid.Ukey = fi.Ukey
	and fid.MtlLocationID = '{this.txtMtlLocation.Text}'
)";
                sqlWhere2 += $@"
and exists(
	select 1 from FtyInventory_Detail fid 
	where fid.Ukey = fi.Ukey
	and fid.MtlLocationID = '{this.txtMtlLocation.Text}'
)";
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
r.ExportID,
rd.Id,
rd.PoId,
[Seq] = rd.Seq1 + ' ' + rd.Seq2,
rd.Roll,
rd.Dyelot,
[Description] = dbo.getmtldesc(rd.POID, rd.Seq1, rd.Seq2, 2, 0),
[Color] = iif(fb.MtlTypeID = 'EMB THREAD' OR fb.MtlTypeID = 'SP THREAD' OR fb.MtlTypeID = 'THREAD' , psd.SuppColor, psd.ColorID),
rd.StockQty,
[StockTypeDesc] = st.Name,
rd.StockType,
--rd.Location,
--[OldLocation] = rd.Location,
[Location]=Location.MtlLocationID ,
[OldLocation] = Location.MtlLocationID ,
rd.Weight,
rd.ActualWeight,
[OldActualWeight] = rd.ActualWeight,
[Differential] = rd.ActualWeight - rd.Weight,
[FtyInventoryUkey] = fi.Ukey,
[FtyInventoryQty] = fi.InQty - fi.OutQty + fi.AdjustQty,
rd.Seq1,
rd.Seq2
,[Remark]=''
,[LastRemark] = LastEditDate.Remark
,[LastEditDate]=LastEditDate.EditDate
,[ReceivingSource]='Receiving'
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
	SELECT top 1 lt.EditDate, lt.Remark
	FROM LocationTrans lt
	INNER JOIN LocationTrans_detail ltd ON lt.ID=ltd.ID
	WHERE lt.Status='Confirmed' AND ltd.FtyInventoryUkey=fi.Ukey 
    order by lt.EditDate desc
)LastEditDate
OUTER APPLY(

	SELECT [MtlLocationID] = STUFF(
			(
			SELECT DISTINCT IIF(fid.MtlLocationID IS NULL OR fid.MtlLocationID = '' ,'' , ','+fid.MtlLocationID)
			FROM FtyInventory_Detail fid
			WHERE fid.Ukey = fi.Ukey
			FOR XML PATH('') )
			, 1, 1, '')
)Location

where r.MDivisionID  = '{Env.User.Keyword}' {sqlWhere}

UNION 

SELECT 
[select] = 0
,ExportID=''
,ID=t.ID
,td.PoId
,[Seq] = td.Seq1 + ' ' + td.Seq2
,td.Roll
,td.Dyelot
,[Description] = dbo.getmtldesc(td.POID, td.Seq1, td.Seq2, 2, 0)
,[Color] = iif(fb.MtlTypeID = 'EMB THREAD' OR fb.MtlTypeID = 'SP THREAD' OR fb.MtlTypeID = 'THREAD' , psd.SuppColor, psd.ColorID)
,[StockQty]=td.Qty
,[StockTypeDesc] = st.Name
,td.StockType
,[Location]=Location.MtlLocationID 
,[OldLocation] = Location.MtlLocationID 
,[Weight]=0
,ActualWeight=td.Weight
,[OldActualWeight] = td.Weight
,[Differential] = 0,
[FtyInventoryUkey] = fi.Ukey,
[FtyInventoryQty] = fi.InQty - fi.OutQty + fi.AdjustQty,
td.Seq1,
td.Seq2
,[Remark]=''
,[LastRemark] = LastEditDate.Remark
,[LastEditDate]=LastEditDate.EditDate
,[ReceivingSource]='TransferIn'
FROM TransferIn t with (nolock)
INNER JOIN TransferIn_Detail td with (nolock) ON t.ID = td.ID
INNER JOIN PO_Supp_Detail psd with (nolock) on td.PoId = psd.ID and td.Seq1 = psd.SEQ1 and td.Seq2 = psd.SEQ2
INNER JOIN Fabric fb with (nolock) on psd.SCIRefno = fb.SCIRefno
INNER JOIN Ftyinventory  fi with (nolock) on    td.POID = fi.POID and
                                                td.Seq1 = fi.Seq1 and
                                                td.Seq2 = fi.Seq2 and
                                                td.Roll = fi.Roll and
                                                td.Dyelot  = fi.Dyelot and
                                                td.StockType = fi.StockType
INNER JOIN #tmpStockType st with (nolock) on st.ID = td.StockType
OUTER APPLY(

	SELECT [MtlLocationID] = STUFF(
			(
			SELECT DISTINCT IIF(fid.MtlLocationID IS NULL OR fid.MtlLocationID = '' ,'' , ','+fid.MtlLocationID)
			FROM FtyInventory_Detail fid
			WHERE fid.Ukey = fi.Ukey
			FOR XML PATH('') )
			, 1, 1, '')
)Location
OUTER APPLY(
	SELECT top 1 lt.EditDate, lt.Remark
	FROM LocationTrans lt
	INNER JOIN LocationTrans_detail ltd ON lt.ID=ltd.ID
	WHERE lt.Status='Confirmed' AND ltd.FtyInventoryUkey=fi.Ukey 
    order by lt.EditDate desc
)LastEditDate


WHERE t.Status='Confirmed' 
AND t.MDivisionID  = '{Env.User.Keyword}'
{sqlWhere2}

DROP TABLE #tmpStockType
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
            this.numSelectCnt.Value = 0;
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            this.Query();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GridReceiving_Sorted(object sender, EventArgs e)
        {
            this.GridFormatChange();
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            var selectedReceiving = this.dtReceiving.AsEnumerable().Where(s => (int)s["select"] == 1);
            if (!selectedReceiving.Any())
            {
                MyUtility.Msg.WarningBox("Please select rows first!");
                return;
            }

            if (selectedReceiving.Any(s => MyUtility.Check.Empty(s["Location"])
                                    && !MyUtility.Check.Empty(s["OldLocation"])))
            {
                MyUtility.Msg.WarningBox("Location can not be empty");
                return;
            }

            // 排除Location沒有修改的資料
            DataRow[] drArryExistRemark = this.dtReceiving.AsEnumerable().Where(x => x.Field<int>("select") == 1
                                                                             && !MyUtility.Check.Empty(x.Field<string>("Remark"))
                                                                             && !x.Field<string>("Location").EqualString(x.Field<string>("OldLocation"))).ToArray();
            DataRow[] drArryNotExistRemark = this.dtReceiving.AsEnumerable().Where(x => x.Field<int>("select") == 1
                                                                             && MyUtility.Check.Empty(x.Field<string>("Remark"))
                                                                             && !x.Field<string>("Location").EqualString(x.Field<string>("OldLocation"))).ToArray();
            DataRow[] drArryActualWeight = this.dtReceiving.AsEnumerable().Where(x => x.Field<int>("select") == 1
                                                                             && x.Field<decimal>("ActualWeight") != x.Field<decimal>("OldActualWeight")).ToArray();

            // Remark沒資料則統一合併後寫入P26 同ID，排除Location沒有修改的資料
            var selectedReceivingSummary = drArryNotExistRemark
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
                                            FtyInventoryUkey = (long)s["FtyInventoryUkey"],
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
                                        });

            int cntID = ((selectedReceivingSummary.Count() >= 1) ? 1 : 0) + drArryExistRemark.Length; // 產生表頭數

            string sqlInsertLocationTrans = string.Empty;
            List<string> id_list = MyUtility.GetValue.GetBatchID(Env.User.Keyword + "LH", "LocationTrans", batchNumber: cntID, sequenceMode: 2); // 批次產生ID
            int idcnt = 0;

            if (id_list.Count == 0 && drArryActualWeight.Length == 0)
            {
                MyUtility.Msg.WarningBox("There is no Location, Act.(kg) changed.");
                return;
            }

            if (id_list.Count > 0)
            {
                // Remark有資料要分開寫入到P26 不同ID
                foreach (var item in drArryExistRemark)
                {
                    if (item["Remark"].ToString().Length >= (60 - 19)) // 預設要填入---Create from P21.，因此要扣掉這個文字長度
                    {
                        MyUtility.Msg.WarningBox("Remark is too long!");
                        return;
                    }

                    sqlInsertLocationTrans += $@"
Insert into LocationTrans(ID,MDivisionID,FactoryID,IssueDate,Status,Remark,AddName,AddDate,EditName,EditDate)
            values( '{id_list[idcnt]}',
                    '{Env.User.Keyword}',
                    '{Env.User.Factory}',
                    GetDate(),
                    'Confirmed',
                    '{item["Remark"]}---Create from P21.',
                    '{Env.User.UserID}',
                    GetDate(),
                    '{Env.User.UserID}',
                    GetDate()
                )
";

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
                values('{id_list[idcnt]}',
                       {item["FtyInventoryUkey"]},
                       '{item["POID"]}',
                       '{item["Seq1"]}',
                       '{item["Seq2"]}',
                       '{item["Roll"]}',
                       '{item["Dyelot"]}',
                       '{item["OldLocation"]}',
                       '{item["Location"]}',
                       {item["FtyInventoryQty"]},
                       '{item["StockType"]}')
";
                    idcnt++;
                }

                if (selectedReceivingSummary.Any())
                {
                    sqlInsertLocationTrans += $@"
Insert into LocationTrans(ID,MDivisionID,FactoryID,IssueDate,Status,Remark,AddName,AddDate,EditName,EditDate)
            values( '{id_list[idcnt]}',
                    '{Env.User.Keyword}',
                    '{Env.User.Factory}',
                    GetDate(),
                    'Confirmed',
                    '---Create from P21.',
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
                values('{id_list[idcnt]}',
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
                }

                // 重新撈取新增ID資料
                string idList = id_list.Count <= 1 ? id_list[0].ToString() : id_list.JoinToString("','");
                sqlInsertLocationTrans += $@"select * from LocationTrans_Detail where ID in ('{idList}')";
            }

            string sqlUpdateReceiving_Detail = string.Empty;
            foreach (var updateItem in drArryActualWeight)
            {
                if (updateItem["ReceivingSource"].ToString() == "Receiving")
                {
                    sqlUpdateReceiving_Detail += $@"update Receiving_Detail set ActualWeight  = {updateItem["ActualWeight"]}
                                                    where   ID = '{updateItem["ID"]}' and
                                                            POID = '{updateItem["POID"]}' and
                                                            Seq1 = '{updateItem["Seq1"]}' and
                                                            Seq2 = '{updateItem["Seq2"]}' and
                                                            Roll = '{updateItem["Roll"]}' and
                                                            Dyelot = '{updateItem["Dyelot"]}'
";
                }

                if (updateItem["ReceivingSource"].ToString() == "TransferIn")
                {
                    sqlUpdateReceiving_Detail += $@"update TransferIn_Detail set Weight  = {updateItem["ActualWeight"]}
                                                    where   ID = '{updateItem["ID"]}' and
                                                            POID = '{updateItem["POID"]}' and
                                                            Seq1 = '{updateItem["Seq1"]}' and
                                                            Seq2 = '{updateItem["Seq2"]}' and
                                                            Roll = '{updateItem["Roll"]}' and
                                                            Dyelot = '{updateItem["Dyelot"]}'
";
                }
            }

            Exception errMsg = null;
            TransactionScope transactionscope = new TransactionScope();
            using (transactionscope)
            {
                try
                {
                    DualResult result;
                    if (!MyUtility.Check.Empty(sqlInsertLocationTrans))
                    {
                        DataTable dtLocationTransDetail;
                        result = DBProxy.Current.Select(null, sqlInsertLocationTrans, out dtLocationTransDetail);
                        if (!result)
                        {
                            throw result.GetException();
                        }

                        result = Prgs.UpdateFtyInventoryMDivisionPoDetail(dtLocationTransDetail.AsEnumerable().ToList());
                        if (!result)
                        {
                            throw result.GetException();
                        }
                    }

                    if (!MyUtility.Check.Empty(sqlUpdateReceiving_Detail))
                    {
                        result = DBProxy.Current.Execute(null, sqlUpdateReceiving_Detail);
                        if (!result)
                        {
                            throw result.GetException();
                        }
                    }

                    transactionscope.Complete();
                }
                catch (Exception ex)
                {
                    errMsg = ex;
                }
            }

            if (!MyUtility.Check.Empty(errMsg))
            {
                this.ShowErr(errMsg);
                return;
            }

            // 將當前所選位置記錄起來後, 待資料重整後定位回去!
            int currentRowIndexInt = this.gridReceiving.CurrentRow.Index;
            int currentColumnIndexInt = this.gridReceiving.CurrentCell.ColumnIndex;
            this.Query();
            this.gridReceiving.CurrentCell = this.gridReceiving[currentColumnIndexInt, currentRowIndexInt];
            this.gridReceiving.FirstDisplayedScrollingRowIndex = currentRowIndexInt;
            MyUtility.Msg.InfoBox("Complete");
        }

        /// <summary>
        /// 檢查表身Location,ActualWeight是否有被修改過(跟DB資料比較)
        /// 有被修改過,就自動勾選資料
        /// </summary>
        /// <param name="rowIndex"></param>
        private void SelectModify(int rowIndex)
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

        private void GridReceiving_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                this.gridReceiving.ValidateControl();
                DataTable dt = (DataTable)this.gridReceiving.DataSource;
                if (dt != null || dt.Rows.Count > 0)
                {
                    int cnt = MyUtility.Convert.GetInt(dt.Compute("count(select)", "select = 1"));
                    this.numSelectCnt.Value = cnt;
                }
            }
        }
    }
}
