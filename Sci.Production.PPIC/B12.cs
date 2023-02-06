using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Class;
using Sci.Production.Class.Commons;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;
using Sci.Production.Prg;
using Sci.Production.PublicForm;

namespace Sci.Production.PPIC
{
    /// <inheritdoc/>
    public partial class B12 : Win.Tems.Input1
    {
        private DataTable dtMold = new DataTable();
        private DataTable dtSpec = new DataTable();

        /// <summary>
        /// P23
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public B12(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();

            this.SetupGrid();
        }

        /// <summary>
        /// SetupGrid
        /// </summary>
        public void SetupGrid()
        {
            this.Helper.Controls.Grid.Generator(this.grid_Mold)
            .CheckBox("Junk", header: "Junk", trueValue: true, falseValue: false, iseditable: false)
            .Text("MoldID", header: "Mold#", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("Season", header: "Season", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .ComboBox("LabelFor", header: "Label For", width: Widths.AnsiChars(15), settings: this.ComboBoxColSettings("PadPrint_LabelFor"), iseditable: false)
            .ComboBox("MainSize", header: "Main Size", width: Widths.AnsiChars(15), settings: this.ComboBoxColSettings("PadPrint_MainSize"), iseditable: false)
            .ComboBox("Gender", header: "Gender", width: Widths.AnsiChars(15), settings: this.ComboBoxColSettings("PadPrint_Gender"), iseditable: false)
            .ComboBox("AgeGroup", header: "Age Group", width: Widths.AnsiChars(15), settings: this.ComboBoxColSettings("PadPrint_AgeGroup"), iseditable: false)
            .ComboBox("SizeSpec", header: "Size Spec", width: Widths.AnsiChars(15), settings: this.ComboBoxColSettings("PadPrint_SizeSpec"), iseditable: false)
            .ComboBox("Part", header: "Part", width: Widths.AnsiChars(15), settings: this.ComboBoxColSettings("PadPrint_Part"), iseditable: false)
            .Text("Region", header: "Region", width: Widths.AnsiChars(2), iseditingreadonly: true)
            ;

            this.Helper.Controls.Grid.Generator(this.grid_Spec)
            .CheckBox("Junk", header: "Junk", trueValue: true, falseValue: false)
            .Text("SizePage", header: "Size Page", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("SourceSize", header: "Source Size", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("CustomerSize", header: "Cust Order Size", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Button("Download", header: "Layout PDF", onclick: this.BtnDownload_Click)
            .Text("Version", header: "Version", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Text("Reason", header: "Junk Reason", width: Widths.AnsiChars(10), iseditingreadonly: false)
            .Text("ReversionMold", header: "Re-Ver. Mold", width: Widths.AnsiChars(2), iseditingreadonly: true)
            ;
        }

        private void BtnDownload_Click(object sender, EventArgs e)
        {
            DataRow drSelect = this.grid_Spec.GetDataRow(this.bs_Spec.Position);
            B12_Layout callForm = new B12_Layout(drSelect["MoldID"].ToString());
            callForm.ShowDialog(this);
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            this.dtMold.Clear();
            this.dtSpec.Clear();
            this.LoadData();

            // Purchase History
            string sqlHistory = $@"Select TOP 1 1
From [dbo].SciMachine_MiscOtherPO MmsPo
    ,[dbo].SciMachine_MiscOtherPO_Detail MmsPO_Detail
    ,PadPrintReq_Detail,PadPrint
Where MmsPo.ID = MmsPO_Detail.ID
and PadPrintReq_Detail.ID = MmsPO_Detail.MachineReqID
and PadPrintReq_Detail.Seq2 = MmsPO_Detail.Seq2
And PadPrint.Ukey = PadPrintReq_Detail.PadPrint_Ukey
And MmsPo.ApvName <>''
And MmsPo.Junk <> 1
And MmsPo_Detail.Junk <> 1
And PadPrint.Refno = '{this.CurrentMaintain.Field<string>("Refno")}' 
And PadPrint.BrandID = '{this.CurrentMaintain.Field<string>("BrandID")}'
And PadPrint.SuppID = '{this.CurrentMaintain.Field<string>("SuppID")}'

";
            if (MyUtility.Check.Seek(sqlHistory))
            {
                this.btn_PurchaseHis.ForeColor = VFPColor.Blue_0_0_255;
            }
            else
            {
                this.btn_PurchaseHis.ForeColor = VFPColor.Black_0_0_0;
            }
        }

        /// <inheritdoc/>
        private bool LoadData()
        {
            long ukey = MyUtility.Convert.GetLong(this.CurrentMaintain["Ukey"]);
            string sqlCmd = string.Empty;

            #region Load Mold
            sqlCmd = $@"
Select isnull(checkjunk.junk,1)as Junk,pm.* 
From PadPrint_Mold pm
outer apply (select top 1 junk From PadPrint_Mold_Spec pms where pm.PadPrint_ukey = pms.PadPrint_ukey and pm.MoldID = pms.MoldID and junk=0) checkjunk
Where pm.PadPrint_ukey = @Ukey
";
            var res = DBProxy.Current.SelectEx(sqlCmd, "UKey", ukey);
            if (res == false)
            {
                MyUtility.Msg.ErrorBox(res.ToString(), "load data error");
                return res.Result;
            }

            this.dtMold = res.ExtendedData;
            this.bs_Mold.DataSource = this.dtMold;
            #endregion

            #region Load Spec
            sqlCmd = $@"
Select pms.*  
From PadPrint_Mold pm
Left join PadPrint_Mold_Spec pms on pm.PadPrint_ukey = pms.PadPrint_ukey and pm.MoldID = pms.MoldID
Where pm.PadPrint_ukey = @Ukey
";
            res = DBProxy.Current.SelectEx(sqlCmd, "UKey", ukey);
            if (res == false)
            {
                MyUtility.Msg.ErrorBox(res.ToString(), "load data error");
                return res.Result;
            }

            this.dtSpec = res.ExtendedData;
            this.bs_Spec.DataSource = this.dtSpec;
            #endregion

            return true;
        }

        private DataGridViewGeneratorComboBoxColumnSettings ComboBoxColSettings(string type)
        {
            var ts = new DataGridViewGeneratorComboBoxColumnSettings();
            var sql = $@"Select ID, Name From DropDownList Where Type = '{type}'";
            DataTable dropDownListTable = new DataTable();
            var result = DBProxy.Current.Select(null, sql, out dropDownListTable);
            if (!result)
            {
                this.ShowErr(result.ToString());
                return ts;
            }

            ts.DataSource = dropDownListTable;
            ts.ValueMember = "ID";
            ts.DisplayMember = "Name";
            return ts;
        }

        private void Btn_PurchaseHis_Click(object sender, EventArgs e)
        {
            var dr = this.CurrentMaintain;
            if (dr == null)
            {
                return;
            }

            string refno = this.CurrentMaintain.Field<string>("Refno");
            string brandID = this.CurrentMaintain.Field<string>("BrandID");
            string suppID = this.CurrentMaintain.Field<string>("SuppID");

            string sql = @"
Select [Mold#] = pm.MoldID
	, pms.Side
	, [Source Size] = pms.SourceSize
	, [Customer Order Size] = pms.SourceSize
	, [Date] = CONVERT(varchar(100), m.CDate, 23)
	, [PO#] = m.ID
	, [Seq] = Concat(md.Seq1, '-', md.Seq2)
	, [Factory] = m.FactoryID
	, [Supplier] = Concat(md.SuppID, '-', supp.Abben)
	, [Currency] = m.CurrencyID
	, md.TPEPrice
	, md.Qty
	, md.TPEFoc
	, amt.Amount
	, [ETD] = CONVERT(varchar(10), export.Etd, 23)
	, [ETA] = CONVERT(varchar(10), export.Eta, 23)
From [dbo].SciMachine_MiscOtherPO m
Left join [dbo].SciMachine_MiscOtherPO_Detail md on md.ID = m.ID
Left join PadPrintReq_Detail prd On prd.ID = md.MachineReqID and prd.SEQ2 = md.SEQ2
Left join PadPrint_Mold pm on pm.PadPrint_Ukey = prd.PadPrint_Ukey and pm.MoldID = prd.MoldID
Left join PadPrint p on p.Ukey = prd.PadPrint_Ukey
Left join PadPrint_Mold_Spec pms on pms.PadPrint_Ukey = p.Ukey and pms.MoldID = pm.MoldID
Left join dbo.Currency as cur on cur.ID = m.CurrencyID 
Left join supp on supp.id= md.SuppID
Outer apply (
	Select Top 1 e.ID, e.ETD, e.ETA
	From Export e
	Left join Export_Detail ed on e.ID = ed.ID
	Left join [dbo].SciMachine_MiscOtherPO_Detail dmd on dmd.ID = ed.PoID And dmd.Seq1 = ed.Seq1 And dmd.Seq2 = ed.Seq2
	Left join PadPrintReq_Detail dprd On dprd.ID = dmd.MachineReqID and dprd.SEQ2 = dmd.SEQ2
	Left join PadPrint dp on dp.Ukey = dprd.PadPrint_Ukey
	Left join PadPrint_Mold dpm on dpm.PadPrint_Ukey = dp.Ukey
	Where ed.PoID = m.ID and ed.Seq1 = md.SEQ1 And dpm.MoldID = pm.MoldID
	Order by e.ID desc
) export
outer apply dbo.GetAmountByUnit(md.TPEPrice,md.Qty, md.UnitID, cur.Exact) as amt
Where m.ApvName != ''
    And m.Junk != 1
    And md.Junk != 1
    And p.Refno = @Refno
    And p.BrandID = @BrandID
    And p.SuppID = @SuppID
Order by m.Cdate, m.ID, md.Seq1, md.Seq2 
";

            var result = DBProxy.Current.SelectEx(sql, "Refno", refno, "BrandID", brandID, "SuppID", suppID);

            JustGrid form = new JustGrid("Purchase History", result.ExtendedData);
            form.Width = 1115;
            form.Height = 500;
            form.Show(this);
        }

        private void GridMold_SelectionChanged(object sender, EventArgs e)
        {
            int rowid = this.grid_Mold.GetSelectedRowIndex();

            DataRowView dr = this.grid_Mold.GetData<DataRowView>(rowid);

            if (dr != null)
            {
                this.bs_Spec.Filter = $"MoldID = '{dr["MoldID"]}'";
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            var moldid = this.txtMold.Text;
            if (!VFP.Empty(moldid))
            {
                var rows = this.grid_Mold.Rows.Cast<DataGridViewRow>()
                                .Where(r => r.Cells["MoldID"].Value.ToString().Contains(moldid));

                var curRow = (this.bs_Mold.Current as DataRowView).Row;

                if (rows.Count() != 0)
                {
                    int index = this.bs_Mold.Position;

                    if (index == rows.Last().Index)
                    {
                        index = rows.First().Index;
                    }
                    else
                    {
                        foreach (var row in rows)
                        {
                            if (index != row.Index)
                            {
                                if (curRow["MoldID"].ToString().Contains(moldid))
                                {
                                    if (row.Index > index)
                                    {
                                        index = row.Index;
                                        break;
                                    }
                                }
                                else
                                {
                                    index = row.Index;
                                    break;
                                }
                            }
                        }
                    }

                    this.bs_Mold.Position = index;
                }
                else
                {
                    MyUtility.Msg.WarningBox("No data found!");
                }
            }
        }
    }
}
