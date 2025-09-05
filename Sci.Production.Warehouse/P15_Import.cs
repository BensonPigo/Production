using System;
using System.Data;
using System.Drawing;
using System.Text;
using Ict;
using Ict.Win;
using Sci.Data;
using System.Linq;
using Sci.Production.Prg;

namespace Sci.Production.Warehouse
{
    /// <inheritdoc/>
    public partial class P15_Import : Win.Subs.Base
    {
        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public Win.Tems.Base P15;
        private DataRow dr_master;
        private DataTable dt_detail;
        private Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;

        /// <inheritdoc/>
        // bool flag;
        // string poType;
        private DataTable dtlack;
        private string Type;

        /// <summary>
        /// P15_Import
        /// </summary>
        /// <param name="master">master</param>
        /// <param name="detail">detail</param>
        /// <param name="type">type</param>
        /// <param name="title">title</param>
        public P15_Import(DataRow master, DataTable detail, string type, string title)
        {
            this.Text = title.ToString();
            this.InitializeComponent();
            this.dr_master = master;
            this.dt_detail = detail;
            this.Type = type;
        }

        private void Sum_checkedqty()
        {
            this.listControlBindingSource1.EndEdit();
            object localPrice = this.dtlack.Compute("Sum(qty)", "selected = 1");
            this.displayTotalQty.Value = localPrice.ToString();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            StringBuilder strSQLCmd = new StringBuilder();

            #region -- 抓lack的資料 --

            // grid1
            strSQLCmd.Append($@"
select  selected = 0
        , id = ''
        ,poid = rtrim(a.POID) 
	    , b.seq1
	    , b.seq2
	    , seq = concat(Ltrim(Rtrim(b.seq1)), ' ', b.Seq2)
	    , [description] = dbo.getMtlDesc(a.poid,b.seq1,b.seq2,2,0)
	    , b.RequestQty
        , Stock = iif(c.lock = 1, 0, c.inqty - c.outqty + c.adjustqty - c.ReturnQty)
        , location = dbo.Getlocation(c.ukey)
        , Qty = 0.00
        , issueqty = 0
        , ftyinventoryukey = c.ukey
        , StockType = 'B' 
        , stockunit = (select stockunit 
	   				  from po_supp_detail WITH (NOLOCK) 
	   				  where id = c.poid 
	   				  		and seq1 = c.seq1
	   				  		and seq2 = c.seq2)
        ,c.lock
        ,[LackReason] = iif(b.PPICReasonID is null, '', CONCAT(b.PPICReasonID, '-', iif(p.DeptID <> '', concat(p.DeptID, '-', p.Description), p.Description)))
        , psd.Refno
        , [Color] = dbo.GetColorMultipleID_MtlType(psd.BrandID, isnull(psdsC.SpecValue ,''), Fabric.MtlTypeID, psd.SuppColor)
        , [SizeCode] = isnull(psdsS.SpecValue,'')
from dbo.lack a WITH (NOLOCK) 
inner join dbo.Lack_Detail b WITH (NOLOCK) on a.ID = b.ID
INNER JOIN Po_Supp_Detail psd WITH (NOLOCK) on psd.ID = a.POID and psd.Seq1 = b.Seq1 and psd.Seq2 = b.Seq2
INNER JOIN Fabric with(nolock) on Fabric.SCIRefno = psd.SCIRefno
LEFT JOIN PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
LEFT JOIN PO_Supp_Detail_Spec psdsS WITH (NOLOCK) ON psdsS.ID = psd.id AND psdsS.seq1 = psd.seq1 AND psdsS.seq2 = psd.seq2 AND psdsS.SpecColumnID = 'Size'
LEFT join dbo.ftyinventory c WITH (NOLOCK) on c.poid = a.POID 
											   and c.seq1 = b.seq1 
											   and c.seq2  = b.seq2 
											   and c.stocktype = 'B'
left join PPICReason p with (nolock) on p.Type = 'AL' and p.ID = b.PPICReasonID
LEFT join Orders o ON o.ID=a.POID
where a.id = '{this.dr_master["requestid"]}' 
and o.Category != 'A'
");
            strSQLCmd.Append(Environment.NewLine); // 換行

           // string AA = strSQLCmd.ToString();
            #endregion

            this.P15.ShowWaitMessage("Data Loading....");

            DataTable data;
            DBProxy.Current.DefaultTimeout = 1200;
            try
            {
                if (!DBProxy.Current.Select(string.Empty, strSQLCmd.ToString(), out data))
                {
                    this.ShowErr(strSQLCmd.ToString());
                    return;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                DBProxy.Current.DefaultTimeout = 0;
            }

            this.P15.HideWaitMessage();

            if (data.Select("lock = 1").Any())
            {
                MyUtility.Msg.WarningBox("Some material has been locked, please check material status in WH P38.");
            }
            else
            {
                if (!data.Select("lock = 0").Any())
                {
                    MyUtility.Msg.WarningBox("Data not found!!");
                }
            }

            this.dtlack = data;

            #region -- Grid1 Setting --

            this.gridlack.CellValueChanged += (s, e) =>
            {
                if (this.gridlack.Columns[e.ColumnIndex].Name == this.col_chk.Name)
                {
                    DataRow dr = this.gridlack.GetDataRow(e.RowIndex);
                    if (Convert.ToBoolean(dr["selected"]) == true && Convert.ToDecimal(dr["qty"].ToString()) == 0)
                    {
                        dr["qty"] = dr["balance"];
                    }
                    else if (Convert.ToBoolean(dr["selected"]) == false)
                    {
                        dr["qty"] = 0;
                    }

                    dr.EndEdit();
                    this.Sum_checkedqty();
                }
            };

            DataGridViewGeneratorNumericColumnSettings ns = new DataGridViewGeneratorNumericColumnSettings();
            ns.CellValidating += (s, e) =>
            {
                if (this.EditMode && !MyUtility.Check.Empty(e.FormattedValue))
                {
                    this.gridlack.GetDataRow(e.RowIndex)["qty"] = e.FormattedValue;
                    if (this.Type != "Lacking")
                    {
                        if ((decimal)e.FormattedValue > (decimal)this.gridlack.GetDataRow(e.RowIndex)["Stock"])
                        {
                            e.Cancel = true;
                            MyUtility.Msg.WarningBox("Issue qty can't be more than Stock qty!!");
                            return;
                        }
                    }

                    this.gridlack.GetDataRow(e.RowIndex)["selected"] = true;
                    this.Sum_checkedqty();
                }
            };

            this.dtlack.Columns.Add("balance", typeof(decimal), "RequestQty - qty");

            this.gridlack.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.listControlBindingSource1.DataSource = this.dtlack;
            this.gridlack.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridlack)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                .Text("Poid", header: "SP#", iseditingreadonly: true, width: Widths.AnsiChars(13)) // 0
                .Text("seq", header: "Seq#", iseditingreadonly: true, width: Widths.AnsiChars(6)) // 1
                .EditText("Description", header: "Description", iseditingreadonly: true, width: Widths.AnsiChars(25)) // 2
                .Text("StockUnit", header: "Unit", iseditingreadonly: true) // 3
                .Numeric("Stock", header: "Stock Qty", iseditingreadonly: true, decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(8)) // 3
                .Numeric("RequestQty", header: "Request Qty", iseditable: true, decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(8)) // 4
                .Numeric("qty", header: "Issue Qty", decimal_places: 2, integer_places: 10, settings: ns, width: Widths.AnsiChars(8)) // 4
                .Numeric("balance", header: "Balance Qty", iseditable: true, decimal_places: 2, integer_places: 10, width: Widths.AnsiChars(8)) // 6
                .Text("location", header: "Bulk Location", iseditingreadonly: true) // 5
                ;
            this.gridlack.Columns["qty"].DefaultCellStyle.BackColor = Color.Pink;
            #endregion

        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Import
        private void BtnImport_Click(object sender, EventArgs e)
        {
            this.listControlBindingSource1.EndEdit();
            this.gridlack.ValidateControl();
            if (MyUtility.Check.Empty(this.dtlack) || this.dtlack.Rows.Count == 0)
            {
                return;
            }

            DataRow[] dr2 = this.dtlack.Select("Selected = 1");
            if (dr2.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }

            dr2 = this.dtlack.Select("qty = 0 and Selected = 1");
            if (dr2.Length > 0)
            {
                MyUtility.Msg.WarningBox("Qty of selected row can't be zero!", "Warning");
                return;
            }

            dr2 = this.dtlack.Select("qty <> 0 and Selected = 1");
            foreach (DataRow tmp in dr2)
            {
                DataRow[] findrow;

                // 判斷為P15(副料)呼叫還是P15(主料)呼叫
                findrow = this.dt_detail.AsEnumerable().Where(row => row.RowState != DataRowState.Deleted
                                                      && row["poid"].EqualString(tmp["poid"].ToString()) && row["seq1"].EqualString(tmp["seq1"])
                                                      && row["seq2"].EqualString(tmp["seq2"].ToString())).ToArray();

                if (findrow.Length > 0)
                {
                    findrow[0]["qty"] = tmp["qty"];
                }
                else
                {
                    tmp["id"] = this.dr_master["id"];
                    tmp.AcceptChanges();
                    tmp.SetAdded();
                    this.dt_detail.ImportRow(tmp);
                }
            }

            this.Close();
        }
    }
}
