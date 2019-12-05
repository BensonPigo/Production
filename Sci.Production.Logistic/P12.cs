using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sci.Production.Logistic
{
    /// <inheritdoc/>
    public partial class P12 : Sci.Win.Tems.QueryForm
    {
        /// <inheritdoc/>
        public P12(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        private int s;
        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            Ict.Win.DataGridViewGeneratorCheckBoxColumnSettings col_chk = new Ict.Win.DataGridViewGeneratorCheckBoxColumnSettings();
            col_chk.CellValidating += (s, e) =>
            {
                DataRow dr = this.grid1.GetDataRow<DataRow>(e.RowIndex);
                if (!MyUtility.Check.Empty(dr["remark"]))
                {
                    MyUtility.Msg.WarningBox(dr["remark"].ToString());
                    dr["selected"] = 0;
                    dr.EndEdit();
                    return;
                }

                dr["selected"] = e.FormattedValue;
                dr.EndEdit();
                int sint = ((DataTable)this.listControlBindingSource1.DataSource).Select("selected=1").Count();
                this.numSelectedCTNQty.Value = sint;
            };

            this.grid1.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.grid1)
            .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0, settings: col_chk)
            .Text("packinglistID", header: "Pack ID", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("CTNStartNo", header: "CTN#", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("CustCTN", header: "Cust CTN#", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Text("OrderID", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("CustPONo", header: "PO No.", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("StyleID", header: "Style#", width: Widths.AnsiChars(15), iseditingreadonly: true)
            .Text("SeasonID", header: "Season", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("BrandID", header: "Brand", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("Alias", header: "Destination", width: Widths.AnsiChars(12), iseditingreadonly: true)
            .Date("BuyerDelivery", header: "Buyer Delivery", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Date("SCIDelivery", header: "SCI Delivery", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Numeric("ShipQty", header: "Ship Qty", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Numeric("ScanQty", header: "Scan Qty", width: Widths.AnsiChars(6), iseditingreadonly: true)
            .Text("ScanName", header: "Scan Name", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .DateTime("ScanEditDate", header: "Scan Date", width: Widths.AnsiChars(20), iseditingreadonly: true)
            .Text("PulloutTransportDesc", header: "Transport", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("PulloutTransportNo", header: "Transport No", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("Remark", header: "Remark", width: Widths.AnsiChars(10), iseditingreadonly: true)
            ;

            this.grid1.ColumnHeaderMouseClick += (s, e) =>
            {
                if (this.s == this.numSelectedCTNQty.Value)
                {
                    foreach (DataRow dr in ((DataTable)this.listControlBindingSource1.DataSource).Rows)
                    {
                        dr["selected"] = 0;
                        dr.EndEdit();
                    }
                }
                else
                {
                    foreach (DataRow dr in ((DataTable)this.listControlBindingSource1.DataSource).Rows)
                    {
                        if (!MyUtility.Check.Empty(dr["remark"]))
                        {
                            dr["selected"] = 0;
                            dr.EndEdit();
                        }
                    }
                }

                int sint = ((DataTable)this.listControlBindingSource1.DataSource).Select("selected=1").Count();
                this.numSelectedCTNQty.Value = sint;
            };
        }

        private void BtnFind_Click(object sender, EventArgs e)
        {
            this.Find();
        }

        private void Find()
        {
            this.numSelectedCTNQty.Value = 0;
            this.numTTLQty.Value = 0;
            this.listControlBindingSource1.DataSource = null;
            if (MyUtility.Check.Empty(this.txtSP.Text) && MyUtility.Check.Empty(this.txtPO.Text) &&
                MyUtility.Check.Empty(this.txtPackID.Text) && MyUtility.Check.Empty(this.dateBuyer.Value1))
            {
                MyUtility.Msg.WarningBox("SP#, PO#, PackID, Buyer Delivery cannot all be empty.");
                return;
            }

            string where = string.Empty;
            if (!MyUtility.Check.Empty(this.txtSP.Text))
            {
                where += $@" and pld.OrderID='{this.txtSP.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtPackID.Text))
            {
                where += $@" and pld.id='{this.txtPackID.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtPO.Text))
            {
                where += $@" and o.CustPONo = '{this.txtPO.Text}'";
            }

            if (!MyUtility.Check.Empty(this.dateBuyer.Value1))
            {
                where += $@" and o.BuyerDelivery between '{((DateTime)this.dateBuyer.Value1).ToString("d")}' and '{((DateTime)this.dateBuyer.Value2).ToString("d")}'";
            }

            string sqlcmd = $@"
select
	selected = cast(0 as bit),
	packinglistID=pld.ID,pld.CTNStartNo,pld.CustCTN,pld.OrderID,o.CustPONo,o.StyleID,o.SeasonID,o.BrandID,c.Alias,o.BuyerDelivery,
	o.SCIDelivery,pld.ShipQty,pld.ScanQty,pld.ScanName,pld.ScanEditDate,
	PulloutTransport='',
    PulloutTransportDesc='',
	PulloutTransportNo='',
	Remark='',
    pld.ukey
from PackingList_Detail pld  with(nolock)
inner join orders o WITH (NOLOCK) on o.id	= pld.orderid
left join Country c WITH (NOLOCK) on o.Dest = c.ID
where pld.ReceiveDate is not null--（Clog 從工廠端接收到紙箱的日期）     
and pld.TransferCFADate is null--（紙箱從 Clog 轉出至 CFA 的日期 - 在途）
and pld.CFAReturnClogDate is null--（紙箱從 CFA 轉回 Clog 的日期 - 在途）
and pld.DisposeFromClog = 0--（紙箱在 Clog 報廢）
and pld.ClogPulloutDate is null--（紙箱從 Clog 出貨裝在卡車 / 貨櫃
{where}
order by pld.ID,pld.CTNStartNo
";
            DataTable dt;
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out dt);
            if (!result)
            {
                MyUtility.Msg.WarningBox("Query data fail.\r\n" + result.ToString());
                return;
            }

            if (dt.Rows.Count == 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return;
            }

            this.listControlBindingSource1.DataSource = dt;

            this.numSelectedCTNQty.Value = 0;
            this.numTTLQty.Value = dt.Rows.Count;
            this.s = ((DataTable)this.listControlBindingSource1.DataSource).Select("remark is null ").Count();
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            // 設定只能選txt檔
            this.openFileDialog1.Filter = "txt files (*.txt)|*.txt";
            this.numSelectedCTNQty.Value = 0;
            this.numTTLQty.Value = 0;

            // 開窗且有選擇檔案
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.listControlBindingSource1.DataSource = null;
                DualResult result;

                DataTable readdt = new DataTable();
                readdt.Columns.Add("custCtn", typeof(string));
                readdt.Columns.Add("packinglistID", typeof(string));
                readdt.Columns.Add("CTNStartNo", typeof(string));

                // 讀檔案
                using (StreamReader reader = new StreamReader(this.openFileDialog1.FileName, System.Text.Encoding.UTF8))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        // 每一行的第一個字母必須是2
                        System.Diagnostics.Debug.WriteLine(line);
                        IList<string> sl = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        if (sl.Count == 0 || sl[0] != "3")
                        {
                            MyUtility.Msg.WarningBox("Format is not correct!");
                            return;
                        }

                        if (sl.Count < 2)
                        {
                            MyUtility.Msg.WarningBox("Format is not correct!");
                            return;
                        }

                        DataRow dr = readdt.NewRow();
                        string custCtn = sl[1];
                        string packinglistID = string.Empty;
                        string cTNStartNo = string.Empty;
                        if (sl[1].Length >= 14)
                        {
                            packinglistID = sl[1].Substring(0, 13);
                            cTNStartNo = MyUtility.Convert.GetInt(sl[1].Substring(13)).ToString();
                        }

                        dr["custCtn"] = custCtn;
                        dr["packinglistID"] = packinglistID;
                        dr["cTNStartNo"] = cTNStartNo;
                        readdt.Rows.Add(dr);
                    }
                }

                if (readdt.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("File list is empty!");
                    return;
                }

                // 去除重複
                DataTable distl = readdt.AsEnumerable().Distinct().CopyToDataTable();
                DataTable tmpdt = distl.Clone();
                tmpdt.Columns.Remove("custCtn");

                string sqlcmd = string.Empty;
                foreach (DataRow item in distl.Rows)
                {
                    sqlcmd = $@"
select PackingListID=pld.id,pld.CTNStartNo
from PackingList_Detail pld  with(nolock)
where pld.CustCTN = '{item["CustCTN"]}'
union
select PackingListID=pld.id,pld.CTNStartNo
from PackingList_Detail pld  with(nolock)
where pld.id = '{item["packinglistID"]}' and pld.CTNStartNo ='{item["CTNStartNo"]}'
";
                    DataTable packdt;
                    result = DBProxy.Current.Select(null, sqlcmd, out packdt);
                    if (!result)
                    {
                        this.ShowErr(result);
                        return;
                    }

                    // 4.將步驟 1, 2 皆沒有找到資料的箱號
                    // 新增資料列 Packing List ID 為空並且 CTN No 為找不到資料的箱號
                    if (packdt.Rows.Count == 0)
                    {
                        DataRow dr = tmpdt.NewRow();
                        dr["PackingListID"] = string.Empty;
                        dr["CTNStartNo"] = item["CTNStartNo"];
                        tmpdt.Rows.Add(dr);
                    }
                    else
                    {
                        foreach (DataRow ic in packdt.Rows)
                        {
                            DataRow dr = tmpdt.NewRow();
                            dr["PackingListID"] = ic["PackingListID"];
                            dr["CTNStartNo"] = ic["CTNStartNo"];
                            tmpdt.Rows.Add(dr);
                        }
                    }
                }

                var tmpdt2 = tmpdt.AsEnumerable().Select(s => new { PackingListID = MyUtility.Convert.GetString(s["PackingListID"]), CTNStartNo = MyUtility.Convert.GetString(s["CTNStartNo"]) }).Distinct().ToList();

                sqlcmd = $@"
select t.*,
	remark=case when a.v is null then'This carton isn''t in packing list.'
				when b.v is not null then 'This carton not yet send to CLog.'
				when e.DisposeFromClog = 1 then 'This carton had been dispose.'
				when e.ClogPulloutDate is not null then 'This carton already completed Clog Pullout.'
			end,
	e.*
from #tmp t
outer apply(select top 1 v=1 from PackingList_Detail pld with(nolock) where t.packinglistID = pld.id and t.CTNStartNo=pld.CTNStartNo)a
outer apply(
	select top 1 v='' from PackingList_Detail pld with(nolock) 
	where t.packinglistID = pld.id and t.CTNStartNo=pld.CTNStartNo
	and	(
		pld.ReceiveDate is null--（Clog 從工廠端接收到紙箱的日期）                      
		or pld.TransferCFADate is not null--（紙箱從 Clog 轉出至 CFA 的日期 - 在途）
		or pld.CFAReturnClogDate is not null--（紙箱從 CFA 轉回 Clog 的日期 - 在途）
	)
)b
outer apply(
	select
		selected = cast(0 as bit),
		pld.CustCTN,pld.OrderID,o.CustPONo,o.StyleID,o.SeasonID,o.BrandID,c.Alias,o.BuyerDelivery,
		o.SCIDelivery,pld.ShipQty,pld.ScanQty,pld.ScanName,pld.ScanEditDate,
		PulloutTransport='',
		PulloutTransportDesc='',
		PulloutTransportNo='',
        pld.ukey,
        pld.DisposeFromClog,
        pld.ClogPulloutDate
        
	from PackingList_Detail pld  with(nolock)
	inner join orders o WITH (NOLOCK) on o.id	= pld.orderid
	left join Country c WITH (NOLOCK) on o.Dest = c.ID
	where t.packinglistID = pld.id and t.CTNStartNo=pld.CTNStartNo
)e";
                DataTable lastable;
                result = MyUtility.Tool.ProcessWithObject(tmpdt2, string.Empty, sqlcmd, out lastable);
                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }

                this.listControlBindingSource1.DataSource = lastable;

                this.numSelectedCTNQty.Value = 0;
                this.numTTLQty.Value = lastable.Rows.Count;
                this.s = ((DataTable)this.listControlBindingSource1.DataSource).Select("remark is null").Count();
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            this.grid1.ValidateControl();
            string pulloutTransport = MyUtility.Convert.GetString(this.comboDropDownList1.SelectedValue);
            string pulloutTransportDesc = this.comboDropDownList1.Text;
            string pullouttransportNo = this.txtTransportNo.Text;
            int pos = this.listControlBindingSource1.Position;     // 記錄目前指標位置
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            if (dt == null || dt.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("Please select data first!");
                return;
            }

            DataRow[] selectedData = dt.Select("Selected = 1");
            if (selectedData.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select data first!");
                return;
            }

            foreach (DataRow currentRecord in selectedData)
            {
                currentRecord["PulloutTransport"] = pulloutTransport;
                currentRecord["PulloutTransportDesc"] = pulloutTransportDesc;
                currentRecord["PullouttransportNo"] = pullouttransportNo;
                currentRecord.EndEdit();
            }

            this.grid1.SuspendLayout();
            this.listControlBindingSource1.Position = pos;
            this.grid1.DataSource = null;
            this.grid1.DataSource = this.listControlBindingSource1;
            this.listControlBindingSource1.Position = pos;
            this.grid1.ResumeLayout();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // 檢查是否有勾選資料
            this.grid1.ValidateControl();
            this.listControlBindingSource1.EndEdit();
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dt))
            {
                return;
            }

            DataRow[] selectedData = dt.Select("Selected = 1");
            if (selectedData.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please choose data first!");
                return;
            }

            DataTable seleDt = selectedData.CopyToDataTable();

            foreach (DataRow dr in seleDt.Rows)
            {
                if (MyUtility.Check.Empty(dr["PulloutTransport"]) || MyUtility.Check.Empty(dr["PulloutTransportNo"]))
                {
                    MyUtility.Msg.WarningBox("Transport & Transport No cannot be empty.");
                    return;
                }
            }

            string sqlupdate = $@"
update pld set
    pld.ClogPulloutName='{Sci.Env.User.UserID}',
	pld.ClogPulloutDate=getdate(),
	pld.PulloutTransport = t.PulloutTransport,
	pld.PulloutTransportNo = t.PulloutTransportNo
from #tmp t
inner join PackingList_Detail pld on t.ukey = pld.ukey
";
            DataTable ot;
            DualResult result = MyUtility.Tool.ProcessWithDatatable(seleDt, string.Empty, sqlupdate, out ot);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            MyUtility.Msg.InfoBox("Save Completed");

            this.Find();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
